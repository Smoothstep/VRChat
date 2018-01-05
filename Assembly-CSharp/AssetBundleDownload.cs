using System;
using System.IO;
using System.Text.RegularExpressions;
using BestHTTP;
using UnityEngine;

// Token: 0x02000A83 RID: 2691
public class AssetBundleDownload
{
	// Token: 0x0600510D RID: 20749 RVA: 0x001BAA58 File Offset: 0x001B8E58
	public AssetBundleDownload(string url, AssetBundleDownloadManager.UnpackType unpack = AssetBundleDownloadManager.UnpackType.Async)
	{
		this.persistentDataPath = Application.persistentDataPath;
		this.currentState = AssetBundleDownload.State.WaitingForRequestToBeSent;
		this.downloadId = UnityEngine.Random.Range(100, 999);
		this.assetId = null;
		this.assetUrl = url;
		this.unpackType = unpack;
	}

	// Token: 0x17000BFB RID: 3067
	// (get) Token: 0x0600510E RID: 20750 RVA: 0x001BAAB2 File Offset: 0x001B8EB2
	public string assetBundleFilePath
	{
		get
		{
			return this.persistentDataPath + "/ab/" + this.assetId;
		}
	}

	// Token: 0x0600510F RID: 20751 RVA: 0x001BAACA File Offset: 0x001B8ECA
	public void CopyAssetData(AssetBundleDownload download)
	{
		this.isLzma = download.isLzma;
		this.assetBundleBytes = download.assetBundleBytes;
		this.assetBundle = download.assetBundle;
		this.asset = download.asset;
	}

	// Token: 0x06005110 RID: 20752 RVA: 0x001BAAFC File Offset: 0x001B8EFC
	public void Unload()
	{
		if (this.assetBundle != null)
		{
			this.assetBundle.Unload(false);
			this.assetBundle = null;
		}
		this.asset = null;
	}

	// Token: 0x06005111 RID: 20753 RVA: 0x001BAB29 File Offset: 0x001B8F29
	public void OnDownloadProgress(HTTPRequest request, int downloaded, int length)
	{
		if (this.onProgress != null)
		{
			this.onProgress(request, downloaded, length);
		}
	}

	// Token: 0x06005112 RID: 20754 RVA: 0x001BAB44 File Offset: 0x001B8F44
	public void OnDownloadCompleted()
	{
		this.currentState = AssetBundleDownload.State.Done;
		if (this.onDownloadCompleted != null)
		{
			this.onDownloadCompleted(this.assetUrl, this);
		}
	}

	// Token: 0x06005113 RID: 20755 RVA: 0x001BAB6B File Offset: 0x001B8F6B
	public void OnDownloadError(string message, LoadErrorReason reason)
	{
		this.currentState = AssetBundleDownload.State.Error;
		if (this.onDownloadError != null)
		{
			this.onDownloadError(this.assetUrl, message, reason);
		}
	}

	// Token: 0x06005114 RID: 20756 RVA: 0x001BAB92 File Offset: 0x001B8F92
	public void OnPluginLoadError(string message)
	{
		this.currentState = AssetBundleDownload.State.Error;
		if (this.onPluginLoadError != null)
		{
			this.onPluginLoadError(message);
		}
	}

	// Token: 0x06005115 RID: 20757 RVA: 0x001BABB4 File Offset: 0x001B8FB4
	private static string MakeValidFileName(string name)
	{
		string arg = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
		string pattern = string.Format("([{0}]*\\.+$)|([{0}]+)", arg);
		return Regex.Replace(name, pattern, "_");
	}

	// Token: 0x0400396C RID: 14700
	public AssetBundleDownload.State currentState;

	// Token: 0x0400396D RID: 14701
	public bool isCancelled;

	// Token: 0x0400396E RID: 14702
	public AssetBundleDownloadManager.OnPluginLoadError onPluginLoadError;

	// Token: 0x0400396F RID: 14703
	public AssetBundleDownloadManager.OnDownloadCompleted onDownloadCompleted;

	// Token: 0x04003970 RID: 14704
	public AssetBundleDownloadManager.OnDownloadError onDownloadError;

	// Token: 0x04003971 RID: 14705
	public OnDownloadProgressDelegate onProgress;

	// Token: 0x04003972 RID: 14706
	public AssetBundleDownloadManager.UnpackType unpackType = AssetBundleDownloadManager.UnpackType.Immediate;

	// Token: 0x04003973 RID: 14707
	public int downloadId = -1;

	// Token: 0x04003974 RID: 14708
	public int retryCount;

	// Token: 0x04003975 RID: 14709
	public string assetUrl;

	// Token: 0x04003976 RID: 14710
	public string pluginUrl;

	// Token: 0x04003977 RID: 14711
	public string assetId;

	// Token: 0x04003978 RID: 14712
	public int assetVersion;

	// Token: 0x04003979 RID: 14713
	public bool isLzma;

	// Token: 0x0400397A RID: 14714
	public bool isGZip;

	// Token: 0x0400397B RID: 14715
	public byte[] assetBundleBytes;

	// Token: 0x0400397C RID: 14716
	private string persistentDataPath;

	// Token: 0x0400397D RID: 14717
	public bool isSavedOnDisk;

	// Token: 0x0400397E RID: 14718
	public AssetBundle assetBundle;

	// Token: 0x0400397F RID: 14719
	public UnityEngine.Object asset;

	// Token: 0x04003980 RID: 14720
	public bool forceRefreshCache;

	// Token: 0x02000A84 RID: 2692
	public enum State
	{
		// Token: 0x04003982 RID: 14722
		Error,
		// Token: 0x04003983 RID: 14723
		WaitingForRequestToBeSent,
		// Token: 0x04003984 RID: 14724
		HttpRequestSent,
		// Token: 0x04003985 RID: 14725
		WaitingForDuplicateToBeDone,
		// Token: 0x04003986 RID: 14726
		HttpResponseReceived,
		// Token: 0x04003987 RID: 14727
		QueuedForUnpack,
		// Token: 0x04003988 RID: 14728
		Decompressing,
		// Token: 0x04003989 RID: 14729
		CreatingFromMemory,
		// Token: 0x0400398A RID: 14730
		LoadingAssetFromAssetBundle,
		// Token: 0x0400398B RID: 14731
		Done
	}
}
