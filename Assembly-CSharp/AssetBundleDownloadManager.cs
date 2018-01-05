using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using BestHTTP;
using BestHTTP.Caching;
using BestHTTP.JSON;
using UnityEngine;
using VRC;
using VRC.Compression;
using VRC.Core;

// Token: 0x02000A85 RID: 2693
public class AssetBundleDownloadManager : MonoBehaviour
{
	// Token: 0x17000BFC RID: 3068
	// (get) Token: 0x06005117 RID: 20759 RVA: 0x001BAC48 File Offset: 0x001B9048
	public static AssetBundleDownloadManager Instance
	{
		get
		{
			return AssetBundleDownloadManager.mInstance;
		}
	}

	// Token: 0x06005118 RID: 20760 RVA: 0x001BAC50 File Offset: 0x001B9050
	public void DownloadAssetBundle(ApiWorld world, OnDownloadProgressDelegate onProgress, AssetBundleDownloadManager.OnDownloadCompleted onSuccess, AssetBundleDownloadManager.OnDownloadError onError, AssetBundleDownloadManager.UnpackType unpackType)
	{
		base.StartCoroutine(this.DownloadAndUnpackAssetBundleCoroutine(world.assetUrl, world.id, world.version, world.pluginUrl, onProgress, onSuccess, onError, unpackType, 0, 0, false));
	}

	// Token: 0x06005119 RID: 20761 RVA: 0x001BAC8C File Offset: 0x001B908C
	public void DownloadAssetBundle(ApiAvatar avatar, OnDownloadProgressDelegate onProgress, AssetBundleDownloadManager.OnDownloadCompleted onSuccess, AssetBundleDownloadManager.OnDownloadError onError, AssetBundleDownloadManager.UnpackType unpackType)
	{
		base.StartCoroutine(this.DownloadAndUnpackAssetBundleCoroutine(avatar.assetUrl, avatar.id, avatar.version, null, onProgress, onSuccess, onError, unpackType, 0, 0, false));
	}

	// Token: 0x0600511A RID: 20762 RVA: 0x001BACC4 File Offset: 0x001B90C4
	public void CancelAssetBundleDownload(string url)
	{
		if (this.mAssetBundleDownloads.ContainsKey(url))
		{
			AssetBundleDownload assetBundleDownload = this.mAssetBundleDownloads[url];
			assetBundleDownload.isCancelled = true;
			Debug.Log("Successfully set download cancelled flag");
		}
		else
		{
			Debug.LogWarning("Cancel download failed, couldn't find bundle for url " + url);
		}
	}

	// Token: 0x0600511B RID: 20763 RVA: 0x001BAD18 File Offset: 0x001B9118
	public void DestroyOldAssets()
	{
		this.mOldAssets.Clear();
		this.mOldAssets = this.mLoadedAssets;
		this.mLoadedAssets = new Dictionary<string, UnityEngine.Object>();
		foreach (KeyValuePair<string, AssetBundleDownload> keyValuePair in this.mAssetBundleDownloads)
		{
			if (keyValuePair.Value.assetBundle != null)
			{
				keyValuePair.Value.assetBundle.Unload(true);
				keyValuePair.Value.assetBundle = null;
			}
		}
		this.mAssetBundleDownloads.Clear();
		foreach (AssetBundle assetBundle in this.manualAssetBundles)
		{
			try
			{
				if (assetBundle != null)
				{
					assetBundle.Unload(true);
				}
			}
			catch (NullReferenceException)
			{
			}
		}
		this.manualAssetBundles.Clear();
	}

	// Token: 0x0600511C RID: 20764 RVA: 0x001BAE4C File Offset: 0x001B924C
	private void Awake()
	{
		this.mManifestPath = Application.persistentDataPath + "/ab/manifest.json";
		if (AssetBundleDownloadManager.mInstance != null)
		{
			VRC.Core.Logger.LogError("More than one AssetBundleDownloadManager detected!!!", DebugLevel.AssetBundleDownloadManager);
		}
		else
		{
			AssetBundleDownloadManager.mInstance = this;
		}
		this.LoadManifest();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x0600511D RID: 20765 RVA: 0x001BAEA5 File Offset: 0x001B92A5
	private void Start()
	{
		this.MonitorAndProcessQueue();
	}

	// Token: 0x0600511E RID: 20766 RVA: 0x001BAEAD File Offset: 0x001B92AD
	private void QueueAssetBundleDownload(AssetBundleDownload download)
	{
		download.currentState = AssetBundleDownload.State.QueuedForUnpack;
		this.mAssetUnpackQueue.Enqueue(download.assetUrl);
	}

	// Token: 0x0600511F RID: 20767 RVA: 0x001BAEC7 File Offset: 0x001B92C7
	private void MonitorAndProcessQueue()
	{
		base.StartCoroutine(this.MonitorAndProcessQueueCoroutine());
	}

	// Token: 0x06005120 RID: 20768 RVA: 0x001BAED8 File Offset: 0x001B92D8
	private void Unpack(AssetBundleDownload download)
	{
		try
		{
			base.StartCoroutine(this.UnpackAssetBundleCoroutine(download));
		}
		catch (Exception ex)
		{
			VRC.Core.Logger.LogWarning(string.Concat(new object[]
			{
				"[",
				download.downloadId,
				"] Error unpacking ab - ",
				ex.Message
			}), DebugLevel.AssetBundleDownloadManager);
			download.OnDownloadError(ex.Message, LoadErrorReason.Unknown);
		}
	}

	// Token: 0x06005121 RID: 20769 RVA: 0x001BAF54 File Offset: 0x001B9354
	private Coroutine DownloadAndLoadLocalPlugin(AssetBundleDownload download)
	{
		return base.StartCoroutine(this.DownloadAndLoadLocalPluginCoroutine(download));
	}

	// Token: 0x06005122 RID: 20770 RVA: 0x001BAF63 File Offset: 0x001B9363
	private Coroutine DownloadAndLoadRemotePlugin(AssetBundleDownload download)
	{
		return base.StartCoroutine(this.DownloadAndLoadRemotePluginCoroutine(download));
	}

	// Token: 0x06005123 RID: 20771 RVA: 0x001BAF74 File Offset: 0x001B9374
	private Coroutine RequestLocalAssetBundle(AssetBundleDownload download, bool hasExistingDownload)
	{
		Coroutine result;
		if (hasExistingDownload)
		{
			VRC.Core.Logger.Log(string.Concat(new object[]
			{
				"[",
				download.downloadId,
				"] Previous request ",
				download.assetUrl,
				" detected. Waiting for it to complete."
			}), DebugLevel.AssetBundleDownloadManager);
			result = base.StartCoroutine(this.WaitForRemoteAssetRequest(download));
		}
		else
		{
			result = base.StartCoroutine(this.RequestLocalAssetBundleCoroutine(download));
		}
		return result;
	}

	// Token: 0x06005124 RID: 20772 RVA: 0x001BAFEC File Offset: 0x001B93EC
	private Coroutine RequestRemoteAssetBundle(AssetBundleDownload download, bool hasExistingDownload)
	{
		Coroutine result;
		if (hasExistingDownload)
		{
			result = base.StartCoroutine(this.WaitForRemoteAssetRequest(download));
		}
		else
		{
			result = base.StartCoroutine(this.RequestRemoteAssetBundleCoroutine(download));
		}
		return result;
	}

	// Token: 0x06005125 RID: 20773 RVA: 0x001BB024 File Offset: 0x001B9424
	private void RetryRequestRemoteAssetBundle(AssetBundleDownload download)
	{
		download.currentState = AssetBundleDownload.State.Error;
		if (false && download.retryCount < 3)
		{
			VRC.Core.Logger.Log("[" + download.downloadId + "] RETRYING DOWNLOAD", DebugLevel.AssetBundleDownloadManager);
			this.mAssetBundleDownloads.Remove(download.assetUrl);
			download.retryCount++;
			base.StartCoroutine(this.DownloadAndUnpackAssetBundleCoroutine(download));
		}
		else
		{
			VRC.Core.Logger.Log("[" + download.downloadId + "] REACHED MAX RETRY COUNT!!", DebugLevel.AssetBundleDownloadManager);
			download.OnDownloadError("[" + download.downloadId + "] Invalid assetbundle url", LoadErrorReason.InvalidURL);
		}
	}

	// Token: 0x06005126 RID: 20774 RVA: 0x001BB0D8 File Offset: 0x001B94D8
	private void OnLocalAssetBundleRequestCompleted(WWW www)
	{
		string url = www.url;
		if (!this.mAssetBundleDownloads.ContainsKey(url))
		{
			return;
		}
		AssetBundleDownload assetBundleDownload = this.mAssetBundleDownloads[url];
		if (this.CheckForCancellation(assetBundleDownload))
		{
			return;
		}
		assetBundleDownload.assetBundleBytes = www.bytes;
		AssetBundleDownloadManager.Instance.QueueAssetBundleDownload(assetBundleDownload);
	}

	// Token: 0x06005127 RID: 20775 RVA: 0x001BB130 File Offset: 0x001B9530
	private void OnHttpAssetBundleRequestCompleted(HTTPRequest request, HTTPResponse response)
	{
		string originalString = request.Uri.OriginalString;
		if (!this.mAssetBundleDownloads.ContainsKey(originalString))
		{
			return;
		}
		AssetBundleDownload assetBundleDownload = this.mAssetBundleDownloads[originalString];
		if (this.CheckForCancellation(assetBundleDownload))
		{
			return;
		}
		assetBundleDownload.currentState = AssetBundleDownload.State.Error;
		switch (request.State)
		{
		case HTTPRequestStates.Queued:
			VRC.Core.Logger.Log("[" + assetBundleDownload.downloadId + "] Queued request.", DebugLevel.AssetBundleDownloadManager);
			request.Callback = new OnRequestFinishedDelegate(this.OnHttpAssetBundleRequestCompleted);
			return;
		case HTTPRequestStates.Finished:
			if (request.Response.IsSuccess)
			{
				if (Player.Instance != null && Player.Instance.isInternal)
				{
					Debug.Log(string.Format(string.Concat(new object[]
					{
						"[",
						assetBundleDownload.downloadId,
						"] AssetBundle [",
						assetBundleDownload.assetUrl,
						"] downloaded! Loaded from local cache: {0}, with size: {1}"
					}), request.Response.IsFromCache.ToString(), response.Data.Length));
				}
				assetBundleDownload.assetBundleBytes = response.Data;
				AssetBundleDownloadManager.Instance.QueueAssetBundleDownload(assetBundleDownload);
				assetBundleDownload.currentState = AssetBundleDownload.State.HttpResponseReceived;
			}
			else
			{
				string message = string.Format(string.Concat(new object[]
				{
					"[",
					assetBundleDownload.downloadId,
					"] Request [",
					originalString,
					"] finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}"
				}), request.Response.StatusCode, request.Response.Message, request.Response.DataAsText);
				VRC.Core.Logger.LogError(message, DebugLevel.AssetBundleDownloadManager);
				assetBundleDownload.OnDownloadError(message, LoadErrorReason.ServerReturnedError);
			}
			return;
		case HTTPRequestStates.Error:
			VRC.Core.Logger.Log(string.Concat(new object[]
			{
				"[",
				assetBundleDownload.downloadId,
				"] Request completed with state: ",
				request.State,
				" and message: ",
				request.Exception.Message
			}), DebugLevel.AssetBundleDownloadManager);
			this.RetryRequestRemoteAssetBundle(assetBundleDownload);
			return;
		case HTTPRequestStates.Aborted:
			assetBundleDownload.OnDownloadError("[" + assetBundleDownload.downloadId + "] Download aborted. Server closed the connection.", LoadErrorReason.ConnectionError);
			return;
		}
		assetBundleDownload.OnDownloadError(string.Concat(new object[]
		{
			"[",
			assetBundleDownload.assetUrl,
			"] Error Downloading AssetBundle: ",
			request.State
		}), LoadErrorReason.ConnectionError);
	}

	// Token: 0x06005128 RID: 20776 RVA: 0x001BB3D4 File Offset: 0x001B97D4
	private IEnumerator MonitorAndProcessQueueCoroutine()
	{
		for (;;)
		{
			if (this.mAssetUnpackQueue.Count > 0 && this.mNumberOfConcurrentUnpacks < this.maxConcurrentUnpacks)
			{
				string key = this.mAssetUnpackQueue.Dequeue();
				AssetBundleDownload download;
				if (this.mAssetBundleDownloads.TryGetValue(key, out download) && !this.CheckForCancellation(download))
				{
					this.Unpack(download);
				}
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06005129 RID: 20777 RVA: 0x001BB3F0 File Offset: 0x001B97F0
	private IEnumerator DownloadAndUnpackAssetBundleCoroutine(string url, string assetId, int assetVersion, string pluginUrl, OnDownloadProgressDelegate onProgress, AssetBundleDownloadManager.OnDownloadCompleted onSuccess, AssetBundleDownloadManager.OnDownloadError onError, AssetBundleDownloadManager.UnpackType unpackType, int retryCount = 0, int downloadId = 0, bool forceRefreshCache = false)
	{
		AssetBundleDownload download = new AssetBundleDownload(url, unpackType);
		download.onDownloadCompleted = onSuccess;
		download.onDownloadError = onError;
		download.onProgress = onProgress;
		download.retryCount = retryCount;
		download.forceRefreshCache = forceRefreshCache;
		download.assetId = assetId;
		download.assetVersion = assetVersion;
		download.pluginUrl = pluginUrl;
		download.onPluginLoadError = delegate(string message)
		{
			Debug.LogError("Plugin failed to load: " + message);
			if (!download.forceRefreshCache)
			{
				download.forceRefreshCache = true;
				this.StartCoroutine(this.DownloadAndUnpackAssetBundleCoroutine(download));
			}
			else
			{
				download.OnDownloadError("Plugin failed to load: " + message, LoadErrorReason.PluginLoadFailed);
			}
		};
		if (Player.Instance != null && Player.Instance.isInternal)
		{
			Debug.Log(string.Concat(new object[]
			{
				"[",
				download.downloadId,
				"] Downloading asset: '",
				download.assetUrl,
				"'"
			}));
		}
		if (string.IsNullOrEmpty(download.assetUrl))
		{
			download.OnDownloadError("URL is empty", LoadErrorReason.InvalidURL);
			yield break;
		}
		if (downloadId > 0)
		{
			download.downloadId = downloadId;
		}
		bool hasExistingDownload = this.mAssetBundleDownloads.ContainsKey(download.assetUrl);
		if (!hasExistingDownload)
		{
			this.mAssetBundleDownloads[download.assetUrl] = download;
		}
		if (download.assetUrl.StartsWith("file"))
		{
			if (!string.IsNullOrEmpty(download.pluginUrl))
			{
				yield return this.DownloadAndLoadLocalPlugin(download);
			}
			if (download.currentState == AssetBundleDownload.State.Error)
			{
				yield break;
			}
			if (this.CheckForCancellation(download))
			{
				yield break;
			}
			if (!string.IsNullOrEmpty(download.assetUrl))
			{
				yield return this.RequestLocalAssetBundle(download, hasExistingDownload);
			}
			yield return this.WaitForAssetToLoad(download);
		}
		else
		{
			UnityEngine.Object asset = AssetBundleDownloadManager.Instance.GetLoadedAsset(download.assetUrl);
			if (asset == null)
			{
				download.currentState = AssetBundleDownload.State.WaitingForRequestToBeSent;
				if (!string.IsNullOrEmpty(download.pluginUrl) && PluginManager.IsAllowedUrl(download.pluginUrl))
				{
					yield return this.DownloadAndLoadRemotePlugin(download);
				}
				else
				{
					download.pluginUrl = null;
				}
				if (download.currentState == AssetBundleDownload.State.Error)
				{
					yield break;
				}
				if (this.CheckForCancellation(download))
				{
					yield break;
				}
				if (!string.IsNullOrEmpty(download.assetUrl))
				{
					yield return this.RequestRemoteAssetBundle(download, hasExistingDownload);
				}
				yield return this.WaitForAssetToLoad(download);
			}
			else
			{
				VRC.Core.Logger.Log(string.Concat(new object[]
				{
					"[",
					download.downloadId,
					"] Asset: ",
					download.assetUrl,
					" already loaded."
				}), DebugLevel.AssetBundleDownloadManager);
				download.asset = asset;
				download.OnDownloadCompleted();
			}
		}
		yield break;
	}

	// Token: 0x0600512A RID: 20778 RVA: 0x001BB460 File Offset: 0x001B9860
	private IEnumerator DownloadAndUnpackAssetBundleCoroutine(AssetBundleDownload download)
	{
		if (this.CheckForCancellation(download))
		{
			yield break;
		}
		yield return StartCoroutine(this.DownloadAndUnpackAssetBundleCoroutine(download.assetUrl, download.assetId, download.assetVersion, download.pluginUrl, download.onProgress, download.onDownloadCompleted, download.onDownloadError, download.unpackType, download.retryCount, download.downloadId, download.forceRefreshCache));
		yield break;
	}

	// Token: 0x0600512B RID: 20779 RVA: 0x001BB484 File Offset: 0x001B9884
	private IEnumerator UnpackAssetBundleCoroutine(AssetBundleDownload download)
	{
		this.mNumberOfConcurrentUnpacks++;
		AssetBundleDownloadManager.UnpackType unpackType = download.unpackType;
		if (unpackType != AssetBundleDownloadManager.UnpackType.Async)
		{
			if (unpackType == AssetBundleDownloadManager.UnpackType.AsyncCreateNoLoad)
			{
				yield return base.StartCoroutine(this.CreateAsyncCoroutine(download));
			}
		}
		else
		{
			yield return base.StartCoroutine(this.CreateAsyncCoroutine(download));
			if (download.currentState != AssetBundleDownload.State.Error)
			{
				yield return base.StartCoroutine(this.LoadAsyncCoroutine(download));
			}
		}
		if (download.currentState != AssetBundleDownload.State.Error && download.currentState != AssetBundleDownload.State.WaitingForRequestToBeSent)
		{
			download.currentState = AssetBundleDownload.State.Done;
		}
		this.mNumberOfConcurrentUnpacks--;
		if (this.CheckForCancellation(download))
		{
			yield break;
		}
		yield break;
	}

	// Token: 0x0600512C RID: 20780 RVA: 0x001BB4A8 File Offset: 0x001B98A8
	public static void RegisterManuallyLoadedAssetBundle(string url, AssetBundle bundle, UnityEngine.Object obj)
	{
		if (bundle != null)
		{
			AssetBundleDownloadManager.Instance.manualAssetBundles.Add(bundle);
		}
		if (!AssetBundleDownloadManager.Instance.mLoadedAssets.ContainsKey(url))
		{
			AssetBundleDownloadManager.Instance.mLoadedAssets.Add(url, obj);
		}
	}

	// Token: 0x0600512D RID: 20781 RVA: 0x001BB4F8 File Offset: 0x001B98F8
	private IEnumerator DownloadAndLoadLocalPluginCoroutine(AssetBundleDownload download)
	{
		if (!string.IsNullOrEmpty(download.pluginUrl))
		{
			WWW www = new WWW(download.pluginUrl);
			www.threadPriority = UnityEngine.ThreadPriority.Normal;
			while (!www.isDone)
			{
				yield return null;
			}
			if (string.IsNullOrEmpty(www.error))
			{
				if (this.CheckForCancellation(download))
				{
					yield break;
				}
				try
				{
					PluginManager.Instance.Load(download.pluginUrl, www.bytes);
				}
				catch (Exception ex)
				{
					download.OnPluginLoadError(ex.Message);
				}
			}
			else
			{
				Debug.LogError("Error loading plugin from " + download.pluginUrl + ": " + www.error);
				download.OnDownloadError("Error loading plugin from " + download.pluginUrl + ": " + www.error, LoadErrorReason.PluginLoadFailed);
			}
		}
		yield break;
	}

	// Token: 0x0600512E RID: 20782 RVA: 0x001BB51C File Offset: 0x001B991C
	private IEnumerator DownloadAndLoadRemotePluginCoroutine(AssetBundleDownload download)
	{
		if (Player.Instance != null && Player.Instance.isInternal)
		{
			Debug.Log(string.Concat(new object[]
			{
				"[",
				download.downloadId,
				"] Requires plugin. About to download ",
				download.pluginUrl
			}));
		}
		if (!PluginManager.IsAllowedUrl(download.pluginUrl))
		{
			yield break;
		}
		HTTPRequest pluginReq = null;
		try
		{
			pluginReq = new HTTPRequest(new Uri(download.pluginUrl), null);
		}
		catch (Exception ex)
		{
			download.OnDownloadError("Bad plugin url: " + ex.Message, LoadErrorReason.InvalidURL);
			yield break;
		}
		if (download.forceRefreshCache)
		{
			HTTPCacheService.DeleteEntity(pluginReq.CurrentUri, true);
		}
		pluginReq.OnProgress = new OnDownloadProgressDelegate(download.OnDownloadProgress);
        pluginReq.Send();
        yield return base.StartCoroutine(pluginReq);
        VRC.Network.ValidateCompletedHTTPRequest(pluginReq, delegate(HTTPResponse response)
		{
			if (!this.CheckForCancellation(download))
			{
				VRC.Core.Logger.Log("[" + download.downloadId + "] Downloaded and loading plugin...", DebugLevel.AssetBundleDownloadManager);
				try
				{
					PluginManager.Instance.Load(download.pluginUrl, pluginReq.Response.Data);
				}
				catch (Exception ex2)
				{
					download.OnPluginLoadError(ex2.Message);
				}
			}
		}, delegate(string errorStr)
		{
			string text = "Failed to download plugin from " + download.pluginUrl + " - " + errorStr;
			VRC.Core.Logger.LogError(string.Concat(new object[]
			{
				"[",
				download.downloadId,
				"] ",
				text
			}), DebugLevel.AssetBundleDownloadManager);
			download.OnDownloadError(text, LoadErrorReason.ConnectionError);
		});
		yield break;
	}

	// Token: 0x0600512F RID: 20783 RVA: 0x001BB540 File Offset: 0x001B9940
	private IEnumerator RequestLocalAssetBundleCoroutine(AssetBundleDownload download)
	{
		WWW www = new WWW(download.assetUrl);
		www.threadPriority = UnityEngine.ThreadPriority.Normal;
		while (!www.isDone)
		{
			download.OnDownloadProgress(null, www.bytesDownloaded, www.size);
			yield return null;
		}
		download.currentState = AssetBundleDownload.State.HttpResponseReceived;
		if (!string.IsNullOrEmpty(www.error) || !this.mAssetBundleDownloads.ContainsKey(www.url))
		{
			download.OnDownloadError(www.error, LoadErrorReason.ConnectionError);
		}
		else
		{
			this.OnLocalAssetBundleRequestCompleted(www);
		}
		yield break;
	}

	// Token: 0x06005130 RID: 20784 RVA: 0x001BB564 File Offset: 0x001B9964
	private IEnumerator RequestRemoteAssetBundleCoroutine(AssetBundleDownload download)
	{
		bool error = false;
		HTTPRequest request = null;
		try
		{
			request = new HTTPRequest(new Uri(download.assetUrl), null);
			request.OnProgress = new OnDownloadProgressDelegate(download.OnDownloadProgress);
			request.Timeout = TimeSpan.FromDays(10.0);
			if (download.forceRefreshCache)
			{
				HTTPCacheService.DeleteEntity(request.CurrentUri, true);
            }
            request.Send();
            if (Player.Instance != null && Player.Instance.isInternal)
			{
				Debug.Log(string.Concat(new object[]
				{
					"[",
					download.downloadId,
					"] Sending request : ",
					Json.Encode(request)
				}));
			}
			download.currentState = AssetBundleDownload.State.HttpRequestSent;
		}
		catch (Exception)
		{
			error = true;
		}
		if (error)
		{
			string message = (request == null) ? "Bad request url." : request.Exception.Message;
			download.OnDownloadError(message, LoadErrorReason.InvalidURL);
		}
		else
		{
            yield return base.StartCoroutine(request);
            this.OnHttpAssetBundleRequestCompleted(request, request.Response);
		}
		yield break;
	}

	// Token: 0x06005131 RID: 20785 RVA: 0x001BB588 File Offset: 0x001B9988
	private IEnumerator WaitForRemoteAssetRequest(AssetBundleDownload download)
	{
		if (download == null)
		{
			yield break;
		}
		if (AssetBundleDownloadManager.Instance == null)
		{
			download.currentState = AssetBundleDownload.State.Error;
			yield break;
		}
		if (AssetBundleDownloadManager.Instance.mAssetBundleDownloads.ContainsKey(download.assetUrl))
		{
			download.currentState = AssetBundleDownload.State.WaitingForDuplicateToBeDone;
			yield return new WaitWhile(() => AssetBundleDownloadManager.Instance != null && download != null && AssetBundleDownloadManager.Instance.mAssetBundleDownloads.ContainsKey(download.assetUrl) && AssetBundleDownloadManager.Instance.mAssetBundleDownloads[download.assetUrl] != null && AssetBundleDownloadManager.Instance.mAssetBundleDownloads[download.assetUrl].currentState != AssetBundleDownload.State.Done && AssetBundleDownloadManager.Instance.mAssetBundleDownloads[download.assetUrl].currentState != AssetBundleDownload.State.Error);
			if (AssetBundleDownloadManager.Instance == null || download == null || !AssetBundleDownloadManager.Instance.mAssetBundleDownloads.ContainsKey(download.assetUrl) || AssetBundleDownloadManager.Instance.mAssetBundleDownloads[download.assetUrl] == null)
			{
				if (download != null)
				{
					download.currentState = AssetBundleDownload.State.Error;
				}
				yield break;
			}
			if (AssetBundleDownloadManager.Instance.mAssetBundleDownloads[download.assetUrl].currentState == AssetBundleDownload.State.Done)
			{
				download.asset = this.GetLoadedAsset(download.assetUrl);
				download.currentState = AssetBundleDownload.State.Done;
				VRC.Core.Logger.Log("[" + download.downloadId + "] Duplicate finished!", DebugLevel.AssetBundleDownloadManager);
			}
			else
			{
				VRC.Core.Logger.Log("[" + download.downloadId + "] Duplicate failed to load because original failed!", DebugLevel.AssetBundleDownloadManager);
				download.OnDownloadError("Duplicate failed to load because original failed!", LoadErrorReason.DuplicateLoadFailed);
			}
		}
		yield break;
	}

	// Token: 0x06005132 RID: 20786 RVA: 0x001BB5AC File Offset: 0x001B99AC
	private IEnumerator AttemptLzmaDecompressionCoroutine(AssetBundleDownload download)
	{
		download.currentState = AssetBundleDownload.State.Decompressing;
		bool isLoomFinished = false;
		byte[] uncompressedData = new byte[0];
		Loom.StartSingleThread(delegate
		{
			if (download == null || download.assetBundleBytes == null)
			{
				VRC.Core.Logger.LogError("[" + download.downloadId + "] AssetBundle was NULL?!", DebugLevel.Always);
				download.isLzma = false;
				download.assetBundleBytes = new byte[0];
				isLoomFinished = true;
				download.forceRefreshCache = true;
				HTTPCacheService.DeleteEntity(new Uri(download.assetUrl), true);
				return;
			}
			VRC.Core.Logger.Log("Download Asset bytes size: " + download.assetBundleBytes.Length, DebugLevel.AssetBundleDownloadManager);
			try
			{
				if (lzma.decompressBuffer(download.assetBundleBytes, ref uncompressedData, true, 0) == 0)
				{
					VRC.Core.Logger.Log("[" + download.downloadId + "] LZMA Compressed AssetBundle Detected!", DebugLevel.AssetBundleDownloadManager);
					download.assetBundleBytes = uncompressedData;
					download.isLzma = true;
				}
			}
			catch (Exception ex)
			{
				VRC.Core.Logger.LogError(string.Concat(new object[]
				{
					"[",
					download.downloadId,
					"] LZMA exception: ",
					ex.Message
				}), DebugLevel.Always);
				download.isLzma = false;
				download.assetBundleBytes = new byte[0];
				isLoomFinished = true;
				download.forceRefreshCache = true;
				HTTPCacheService.DeleteEntity(new Uri(download.assetUrl), true);
			}
			isLoomFinished = true;
		}, System.Threading.ThreadPriority.Normal, true);
		while (!isLoomFinished)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06005133 RID: 20787 RVA: 0x001BB5C8 File Offset: 0x001B99C8
	private IEnumerator AttemptGZipDecompressionCoroutine(AssetBundleDownload download)
	{
		download.currentState = AssetBundleDownload.State.Decompressing;
		bool isGZipUncompressionComplete = false;
		Loom.StartSingleThread(delegate
		{
			if (GZip.IsValid(download.assetBundleBytes))
			{
				VRC.Core.Logger.Log("[" + download.downloadId + "] GZip Compressed AssetBundle Detected!", DebugLevel.AssetBundleDownloadManager);
				download.assetBundleBytes = GZip.Decompress(download.assetBundleBytes);
				download.isGZip = true;
			}
			isGZipUncompressionComplete = true;
		}, System.Threading.ThreadPriority.Normal, true);
		while (!isGZipUncompressionComplete)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06005134 RID: 20788 RVA: 0x001BB5E4 File Offset: 0x001B99E4
	private IEnumerator CreateAsyncCoroutineWithDiskSaving(AssetBundleDownload download)
	{
		if (!download.isSavedOnDisk)
		{
			yield return base.StartCoroutine(this.AttemptLzmaDecompressionCoroutine(download));
			yield return base.StartCoroutine(this.AttemptGZipDecompressionCoroutine(download));
			if (!download.isLzma && !download.isGZip)
			{
				VRC.Core.Logger.Log("[" + download.downloadId + "] Unity Compressed AssetBundle Detected!", DebugLevel.AssetBundleDownloadManager);
			}
			bool doneSavingFile = false;
			Loom.StartSingleThread(delegate
			{
				this.SaveToDisk(download);
				if (download.isLzma || download.isGZip)
				{
					Loom.DispatchToMainThread(delegate
					{
						download.assetBundle = this.LoadFromDisk(download);
						doneSavingFile = true;
					}, false, true);
				}
				else
				{
					doneSavingFile = true;
				}
			}, System.Threading.ThreadPriority.Normal, true);
			while (!doneSavingFile)
			{
				yield return null;
			}
			if (!download.isLzma && !download.isGZip)
			{
				AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(download.assetBundleBytes);
				yield return assetBundleCreateRequest;
				download.assetBundle = assetBundleCreateRequest.assetBundle;
			}
		}
		else
		{
			VRC.Core.Logger.Log(string.Concat(new object[]
			{
				"[",
				download.downloadId,
				"] Attempting to create uncompressed ab from disk - ",
				download.assetUrl
			}), DebugLevel.AssetBundleDownloadManager);
			download.assetBundle = this.LoadFromDisk(download);
			if (download.assetBundle == null)
			{
				VRC.Core.Logger.Log(string.Concat(new object[]
				{
					"[",
					download.downloadId,
					"] Creating unity compressed AB from disk - ",
					download.assetUrl
				}), DebugLevel.AssetBundleDownloadManager);
				Loom.StartSingleThread(delegate
				{
					download.assetBundleBytes = File.ReadAllBytes(download.assetBundleFilePath);
				}, System.Threading.ThreadPriority.Normal, true);
				while (download.assetBundleBytes == null)
				{
					yield return null;
				}
				AssetBundleCreateRequest assetBundleCreateRequest2 = AssetBundle.LoadFromMemoryAsync(download.assetBundleBytes);
				yield return assetBundleCreateRequest2;
				download.assetBundle = assetBundleCreateRequest2.assetBundle;
			}
		}
		yield break;
	}

	// Token: 0x06005135 RID: 20789 RVA: 0x001BB608 File Offset: 0x001B9A08
	private IEnumerator CreateAsyncCoroutine(AssetBundleDownload download)
	{
		yield return base.StartCoroutine(this.AttemptLzmaDecompressionCoroutine(download));
		AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(download.assetBundleBytes);
		yield return assetBundleCreateRequest;
		download.assetBundle = assetBundleCreateRequest.assetBundle;
		if (this.CheckForCancellation(download))
		{
			yield break;
		}
		if (download.assetBundle == null)
		{
			if (!download.forceRefreshCache && !download.assetUrl.StartsWith("file"))
			{
				Debug.LogError(string.Concat(new object[]
				{
					"[",
					download.downloadId,
					"] cached asset bundle was corrupt, redownloading.. - ",
					download.assetUrl
				}));
				download.forceRefreshCache = true;
				download.currentState = AssetBundleDownload.State.WaitingForRequestToBeSent;
				this.mAssetBundleDownloads.Remove(download.assetUrl);
				base.StartCoroutine(this.DownloadAndUnpackAssetBundleCoroutine(download));
			}
			else
			{
				download.OnDownloadError("AssetBundle.LoadFromMemoryAsync failed!", LoadErrorReason.AssetBundleCorrupt);
			}
		}
		yield break;
	}

	// Token: 0x06005136 RID: 20790 RVA: 0x001BB62C File Offset: 0x001B9A2C
	private IEnumerator LoadAsyncCoroutine(AssetBundleDownload download)
	{
		if (download.currentState == AssetBundleDownload.State.Error)
		{
			yield break;
		}
		download.currentState = AssetBundleDownload.State.LoadingAssetFromAssetBundle;
		if (download.assetBundle != null)
		{
			string mainAssetName = AssetBundleDownloadManager.GetMainAssetName(download.assetBundle);
			AssetBundleRequest asyncRequest = null;
			UnityEngine.Object asset = null;
			if (string.IsNullOrEmpty(mainAssetName))
			{
				VRC.Core.Logger.Log(string.Concat(new object[]
				{
					"[",
					download.downloadId,
					"] Loading asset ",
					download.assetUrl,
					" synchronously."
				}), DebugLevel.AssetBundleDownloadManager);
				asset = download.assetBundle.mainAsset;
			}
			else
			{
				VRC.Core.Logger.Log(string.Concat(new object[]
				{
					"[",
					download.downloadId,
					"] Loading asset ",
					download.assetUrl,
					" aynchronously."
				}), DebugLevel.AssetBundleDownloadManager);
				asyncRequest = download.assetBundle.LoadAssetWithSubAssetsAsync(mainAssetName);
				yield return asyncRequest;
				asset = asyncRequest.asset;
			}
			if (asset != null)
			{
				AssetBundleDownloadManager.Instance.mLoadedAssets[download.assetUrl] = asset;
				download.asset = asset;
				download.currentState = AssetBundleDownload.State.Done;
			}
			else
			{
				VRC.Core.Logger.Log("[" + download.downloadId + "] Error unpacking asset from asset bundle.", DebugLevel.AssetBundleDownloadManager);
				download.OnDownloadError("Error unpacking asset from asset bundle.", LoadErrorReason.AssetBundleInvalidOrNull);
			}
			VRC.Core.Logger.Log(string.Concat(new object[]
			{
				"[",
				download.downloadId,
				"] UNLOADING ASSET BUNDLE ",
				download.assetUrl
			}), DebugLevel.AssetBundleDownloadManager);
			download.assetBundle.Unload(false);
		}
		else
		{
			VRC.Core.Logger.Log(string.Concat(new object[]
			{
				"[",
				download.downloadId,
				"] Asset bundle for url ",
				download.assetUrl,
				" is invalid (and null)"
			}), DebugLevel.AssetBundleDownloadManager);
			download.OnDownloadError("LoadAsyncCoroutine: Asset bundle is null.", LoadErrorReason.AssetBundleInvalidOrNull);
		}
		yield break;
	}

	// Token: 0x06005137 RID: 20791 RVA: 0x001BB647 File Offset: 0x001B9A47
	private Coroutine WaitForAssetToLoad(AssetBundleDownload download)
	{
		return base.StartCoroutine(this.WaitForAssetToLoadCoroutine(download));
	}

	// Token: 0x06005138 RID: 20792 RVA: 0x001BB658 File Offset: 0x001B9A58
	private IEnumerator WaitForAssetToLoadCoroutine(AssetBundleDownload download)
	{
		while (download.currentState != AssetBundleDownload.State.Error && download.currentState != AssetBundleDownload.State.Done)
		{
			yield return null;
		}
		if (download.currentState != AssetBundleDownload.State.Error)
		{
			download.OnDownloadCompleted();
		}
		yield break;
	}

	// Token: 0x06005139 RID: 20793 RVA: 0x001BB674 File Offset: 0x001B9A74
	private bool CheckForCancellation(AssetBundleDownload download)
	{
		if (download == null)
		{
			return false;
		}
		if (download.isCancelled)
		{
			download.Unload();
			this.mAssetBundleDownloads.Remove(download.assetUrl);
			if (download.currentState != AssetBundleDownload.State.Error && download.currentState != AssetBundleDownload.State.Done)
			{
				download.OnDownloadError("Download cancelled!", LoadErrorReason.Cancelled);
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600513A RID: 20794 RVA: 0x001BB6D4 File Offset: 0x001B9AD4
	private UnityEngine.Object GetLoadedAsset(string url)
	{
		UnityEngine.Object @object = null;
		if (!string.IsNullOrEmpty(url))
		{
			if (this.mLoadedAssets.ContainsKey(url))
			{
				@object = this.mLoadedAssets[url];
			}
			else if (this.mOldAssets.ContainsKey(url))
			{
				@object = this.mOldAssets[url];
				this.mOldAssets.Remove(url);
				this.mLoadedAssets.Add(url, @object);
			}
		}
		return @object;
	}

	// Token: 0x0600513B RID: 20795 RVA: 0x001BB74C File Offset: 0x001B9B4C
	public static string GetMainAssetName(AssetBundle assetBundle)
	{
		if (assetBundle == null)
		{
			return null;
		}
		string text = null;
		string[] allAssetNames = assetBundle.GetAllAssetNames();
		foreach (string text2 in allAssetNames)
		{
			if (text2 == "assets/_customavatar.prefab")
			{
				text = text2;
			}
			else if (text2 == "assets/_customscene.prefab")
			{
				text = text2;
			}
			if (!string.IsNullOrEmpty(text))
			{
				break;
			}
		}
		return text;
	}

	// Token: 0x0600513C RID: 20796 RVA: 0x001BB7CC File Offset: 0x001B9BCC
	private string GetURLForPlatform(string assetUrl)
	{
		int num = assetUrl.ToLower().IndexOf(".vrc");
		if (num == -1)
		{
			return assetUrl;
		}
		string empty = string.Empty;
		assetUrl = assetUrl.Insert(num, empty);
		return assetUrl;
	}

	// Token: 0x0600513D RID: 20797 RVA: 0x001BB804 File Offset: 0x001B9C04
	private bool IsSavedOnDisk(AssetBundleDownload download)
	{
		this.PrintManifest();
		VRC.Core.Logger.Log(string.Concat(new object[]
		{
			"[",
			download.downloadId,
			"] Is assetId: ",
			download.assetId,
			" saved to disk?"
		}), DebugLevel.AssetBundleDownloadManager);
		return this.mManifest.ContainsKey(download.assetId);
	}

	// Token: 0x0600513E RID: 20798 RVA: 0x001BB868 File Offset: 0x001B9C68
	private AssetBundle LoadFromDisk(AssetBundleDownload download)
	{
		return AssetBundle.LoadFromFile(download.assetBundleFilePath);
	}

	// Token: 0x0600513F RID: 20799 RVA: 0x001BB878 File Offset: 0x001B9C78
	private bool IsUpToDate(AssetBundleDownload download)
	{
		VRC.Core.Logger.Log("manifest v: " + (string)this.mManifest[download.assetId]["version"], DebugLevel.AssetBundleDownloadManager);
		VRC.Core.Logger.Log("bp v: " + download.assetVersion, DebugLevel.AssetBundleDownloadManager);
		return Convert.ToInt32(this.mManifest[download.assetId]["version"]) == download.assetVersion;
	}

	// Token: 0x06005140 RID: 20800 RVA: 0x001BB8F8 File Offset: 0x001B9CF8
	private void SaveToDisk(AssetBundleDownload download)
	{
		FileInfo fileInfo = new FileInfo(download.assetBundleFilePath);
		fileInfo.Directory.Create();
		File.WriteAllBytes(fileInfo.FullName, download.assetBundleBytes);
		this.UpdateManifest(download);
		this.SaveManifest();
	}

	// Token: 0x06005141 RID: 20801 RVA: 0x001BB93C File Offset: 0x001B9D3C
	private void LoadManifest()
	{
		if (File.Exists(this.mManifestPath))
		{
			string json = File.ReadAllText(this.mManifestPath);
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Decode(json);
			foreach (KeyValuePair<string, object> keyValuePair in dictionary)
			{
				this.mManifest[keyValuePair.Key] = (Dictionary<string, object>)keyValuePair.Value;
			}
		}
		else
		{
			this.mManifest = new Dictionary<string, Dictionary<string, object>>();
		}
	}

	// Token: 0x06005142 RID: 20802 RVA: 0x001BB9E4 File Offset: 0x001B9DE4
	private void UpdateManifest(AssetBundleDownload download)
	{
		if (!this.mManifest.ContainsKey(download.assetId))
		{
			this.mManifest[download.assetId] = new Dictionary<string, object>();
		}
		this.mManifest[download.assetId]["version"] = download.assetVersion;
	}

	// Token: 0x06005143 RID: 20803 RVA: 0x001BBA44 File Offset: 0x001B9E44
	private void SaveManifest()
	{
		FileInfo fileInfo = new FileInfo(this.mManifestPath);
		fileInfo.Directory.Create();
		string contents = Json.Encode(this.mManifest);
		File.WriteAllText(this.mManifestPath, contents);
	}

	// Token: 0x06005144 RID: 20804 RVA: 0x001BBA80 File Offset: 0x001B9E80
	private void PrintManifest()
	{
		VRC.Core.Logger.Log("ab manifest: " + Json.Encode(this.mManifest), DebugLevel.AssetBundleDownloadManager);
	}

	// Token: 0x06005145 RID: 20805 RVA: 0x001BBAA0 File Offset: 0x001B9EA0
	public static void ClearSavedAssetBundles()
	{
		string path = Application.persistentDataPath + "/ab";
		Directory.Delete(path, true);
		if (AssetBundleDownloadManager.mInstance != null)
		{
			AssetBundleDownloadManager.mInstance.LoadManifest();
		}
	}

	// Token: 0x0400398C RID: 14732
	private static AssetBundleDownloadManager mInstance;

	// Token: 0x0400398D RID: 14733
	public int maxConcurrentUnpacks = 1;

	// Token: 0x0400398E RID: 14734
	public const int MAX_RETRY_COUNT = 3;

	// Token: 0x0400398F RID: 14735
	private Dictionary<string, AssetBundleDownload> mAssetBundleDownloads = new Dictionary<string, AssetBundleDownload>();

	// Token: 0x04003990 RID: 14736
	private Queue<string> mAssetUnpackQueue = new Queue<string>();

	// Token: 0x04003991 RID: 14737
	private int mNumberOfConcurrentUnpacks;

	// Token: 0x04003992 RID: 14738
	private Dictionary<string, UnityEngine.Object> mOldAssets = new Dictionary<string, UnityEngine.Object>();

	// Token: 0x04003993 RID: 14739
	private Dictionary<string, UnityEngine.Object> mLoadedAssets = new Dictionary<string, UnityEngine.Object>();

	// Token: 0x04003994 RID: 14740
	private Dictionary<string, Dictionary<string, object>> mManifest = new Dictionary<string, Dictionary<string, object>>();

	// Token: 0x04003995 RID: 14741
	private string mManifestPath;

	// Token: 0x04003996 RID: 14742
	private List<AssetBundle> manualAssetBundles = new List<AssetBundle>();

	// Token: 0x02000A86 RID: 2694
	public enum UnpackType
	{
		// Token: 0x04003998 RID: 14744
		Async,
		// Token: 0x04003999 RID: 14745
		Immediate,
		// Token: 0x0400399A RID: 14746
		AsyncCreateImmediateLoad,
		// Token: 0x0400399B RID: 14747
		ImmediateCreateAsyncLoad,
		// Token: 0x0400399C RID: 14748
		AsyncCreateNoLoad,
		// Token: 0x0400399D RID: 14749
		ImmediateCreateNoLoad
	}

	// Token: 0x02000A87 RID: 2695
	// (Invoke) Token: 0x06005147 RID: 20807
	public delegate void OnPluginLoadError(string message);

	// Token: 0x02000A88 RID: 2696
	// (Invoke) Token: 0x0600514B RID: 20811
	public delegate void OnDownloadCompleted(string url, AssetBundleDownload download);

	// Token: 0x02000A89 RID: 2697
	// (Invoke) Token: 0x0600514F RID: 20815
	public delegate void OnDownloadError(string url, string message, LoadErrorReason reason);
}
