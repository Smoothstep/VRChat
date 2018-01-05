using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BestHTTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000B83 RID: 2947
public class CustomScenes
{
	// Token: 0x06005BCC RID: 23500 RVA: 0x00200804 File Offset: 0x001FEC04
	public bool LoadCustomEnvironment(ApiWorld world)
	{
		if (world == null)
		{
			Debug.LogError("LoadCustomEnvironment failed - world is NULL!");
			return false;
		}
		if (string.IsNullOrEmpty(world.assetUrl))
		{
			Debug.LogError("LoadCustomEnvironment failed - world.assetUrl is empty!");
			return false;
		}
		this.Progress = 0f;
		this.lastDownloadProgressTime = Time.unscaledTime;
		Downloader.DownloadAssetBundle(world, new OnDownloadProgressDelegate(this.OnDownloadProgress), new AssetBundleDownloadManager.OnDownloadCompleted(this.ReportDownloadComplete), new AssetBundleDownloadManager.OnDownloadError(this.OnDownloadErrorCallback), AssetBundleDownloadManager.UnpackType.AsyncCreateNoLoad);
		return true;
	}

	// Token: 0x06005BCD RID: 23501 RVA: 0x00200881 File Offset: 0x001FEC81
	public void CancelLoad(ApiWorld world)
	{
		if (world == null || string.IsNullOrEmpty(world.assetUrl))
		{
			return;
		}
		Downloader.CancelAssetBundleDownload(world.assetUrl);
	}

	// Token: 0x06005BCE RID: 23502 RVA: 0x002008A5 File Offset: 0x001FECA5
	private void ReportDownloadComplete(string url, AssetBundleDownload download)
	{
		if (this.OnSceneCreated != null)
		{
			this.OnSceneCreated(download);
		}
	}

	// Token: 0x06005BCF RID: 23503 RVA: 0x002008C0 File Offset: 0x001FECC0
	private void AddSpawnsToSpawnManager(Transform[] spawnTransforms)
	{
		foreach (Transform transform in spawnTransforms)
		{
			Spawn spawn = transform.gameObject.AddComponent<Spawn>();
			spawn.spawnId = transform.name;
			spawn.prefab = SpawnManager.Instance.playerPrefab;
			spawn.transform.position = transform.transform.position;
			spawn.transform.rotation = transform.transform.rotation;
			SpawnManager.Instance.AddSpawn(spawn);
		}
	}

	// Token: 0x06005BD0 RID: 23504 RVA: 0x00200946 File Offset: 0x001FED46
	private void RunStaticBatching(GameObject go)
	{
	}

	// Token: 0x06005BD1 RID: 23505 RVA: 0x00200948 File Offset: 0x001FED48
	private void OnDownloadProgress(HTTPRequest request, int downloaded, int length)
	{
		if (request == null)
		{
			return;
		}
		if (this.mLastDownloadedAmount == downloaded)
		{
			Debug.LogError("Server stopped responding. Could not download world.");
			UserMessage.SetMessage("Server stopped responding. Could not download world.");
			request.Abort();
		}
		this.lastDownloadProgressTime = Time.unscaledTime;
		this.mLastDownloadedAmount = downloaded;
		this.Progress = (float)downloaded / (float)length;
		if (request != null && request.Response != null && request.Response.GetFirstHeaderValue("Content-Length") == null)
		{
			this.Progress = (float)downloaded;
		}
	}

	// Token: 0x06005BD2 RID: 23506 RVA: 0x002009CD File Offset: 0x001FEDCD
	public float GetDownloadStalledTime()
	{
		return Time.unscaledTime - this.lastDownloadProgressTime;
	}

	// Token: 0x06005BD3 RID: 23507 RVA: 0x002009DC File Offset: 0x001FEDDC
	public IEnumerator InstantiateDownloadedScene(AssetBundleDownload download, float timeLimit, Action onSuccess, Action<string> onError)
	{
		if (download == null)
		{
			Debug.LogError("Download was null");
			if (onError != null)
			{
				onError("Download was null");
			}
			yield break;
		}
		AssetBundle ab = download.assetBundle;
		if (ab == null)
		{
			Debug.LogError("Asset bundle did not load");
			if (onError != null)
			{
				onError("Asset bundle did not load");
			}
			yield break;
		}
		VRCAudioManager.EnableAllAudio(false);
		string[] sceneFiles = ab.GetAllScenePaths();
		if (sceneFiles.Length != 1)
		{
			Debug.LogWarning("VRCW file has bad scene count - " + download.assetUrl);
		}
		string sceneName = Path.GetFileNameWithoutExtension(sceneFiles[0]);
		Debug.Log("Loading scene: " + sceneName);
		AssetBundleDownloadManager.RegisterManuallyLoadedAssetBundle(download.assetUrl, ab, ab);
		bool loadLevelSuccess = false;
		yield return AssetManagement.LoadLevelAsync(sceneName, LoadSceneMode.Single, timeLimit, delegate
		{
			loadLevelSuccess = true;
		});
		if (!loadLevelSuccess)
		{
			Debug.LogError("Failed to load scene " + sceneName + ", LoadLevelAsync failed");
			if (onError != null)
			{
				onError("Failed to load scene " + sceneName + ", LoadLevelAsync failed");
			}
			yield break;
		}
		Scene scene = SceneManager.GetSceneByName(sceneName);
		if (!scene.isLoaded)
		{
			Debug.LogError("Failed to load scene " + sceneName);
			if (onError != null)
			{
				onError("Failed to load scene " + sceneName);
			}
			yield break;
		}
		if (!this.ProcessSceneObjectsImmediate())
		{
			Debug.LogError("Error processing scene objects post load");
			if (onError != null)
			{
				onError("Error processing scene objects post load, scene " + sceneName);
			}
			yield break;
		}
		yield return null;
		yield return null;
		SceneManager.SetActiveScene(scene);
		yield return null;
		onSuccess();
		yield break;
	}

	// Token: 0x06005BD4 RID: 23508 RVA: 0x00200A14 File Offset: 0x001FEE14
	private bool ProcessSceneObjectsImmediate()
	{
		VRC_SceneDescriptor[] array = Tools.FindSceneObjectsOfTypeAll<VRC_SceneDescriptor>();
		if (array.Length == 0)
		{
			Debug.LogError("No Scene Descriptor found");
			return false;
		}
		this.sceneDescriptor = array[0];
		if (this.sceneDescriptor == null)
		{
			Debug.LogError("No Scene Descriptor found");
			return false;
		}
		if (this.sceneDescriptor.ReferenceCamera != null && this.sceneDescriptor.ReferenceCamera.activeInHierarchy)
		{
			this.sceneDescriptor.ReferenceCamera.SetActive(false);
		}
		Camera[] source = Tools.FindSceneObjectsOfTypeAll<Camera>();
		foreach (Camera camera in from c in source
		where c != null && !c.transform.root.CompareTag("VRCGlobalRoot")
		select c)
		{
			if (camera.targetTexture == null)
			{
				camera.enabled = false;
			}
			if (camera.GetComponent<AudioListener>() != null)
			{
				camera.GetComponent<AudioListener>().enabled = false;
			}
		}
		return true;
	}

	// Token: 0x06005BD5 RID: 23509 RVA: 0x00200B3C File Offset: 0x001FEF3C
	public IEnumerator ProcessSceneObjects(Action onDone)
	{
		List<Transform> rootTransforms = new List<Transform>();
		Transform[] allTransforms = Tools.FindSceneObjectsOfTypeAll<Transform>();
		foreach (Transform item in from t in allTransforms
		where t != null && t.parent == null && t.gameObject != null && !t.gameObject.CompareTag("VRCGlobalRoot")
		select t)
		{
			rootTransforms.Add(item);
		}
		yield return null;
		if (!this.sceneDescriptor.useAssignedLayers)
		{
			foreach (Transform transform in rootTransforms)
			{
				try
				{
					Tools.SetLayerRecursively(transform.gameObject, LayerMask.NameToLayer("Environment"), LayerMask.NameToLayer("Interactive"));
				}
				catch (Exception exception)
				{
					Debug.LogException(exception, transform.gameObject);
				}
			}
		}
		else
		{
			Debug.Log("This level has elected to use it's own Unity Layers. If things like physics and collision are messed up, this is a likely reason.");
		}
		yield return null;
		Physics.gravity = this.sceneDescriptor.gravity;
		Debug.Log("<color=yellow>Loading scene's render settings.</color>");
		if (this.sceneDescriptor.layerCollisionArr != null)
		{
			bool[,] array = Tools.OneDArrayToTwoDArray<bool>(this.sceneDescriptor.layerCollisionArr, 32, 32);
			int numReservedLayers = Tools.GetNumReservedLayers();
			for (int i = 0; i < 32; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					if (i >= numReservedLayers || j >= numReservedLayers)
					{
						bool ignore = !array[i, j];
						if (i == LayerMask.NameToLayer("UI") || i == LayerMask.NameToLayer("UiMenu") || i == LayerMask.NameToLayer("StereoLeft") || i == LayerMask.NameToLayer("StereoRight") || j == LayerMask.NameToLayer("UI") || j == LayerMask.NameToLayer("UiMenu") || j == LayerMask.NameToLayer("StereoLeft") || j == LayerMask.NameToLayer("StereoRight"))
						{
							ignore = true;
						}
						Physics.IgnoreLayerCollision(i, j, ignore);
					}
				}
			}
		}
		yield return null;
		VRC_SpecialLayer[] layers = Tools.FindSceneObjectsOfTypeAll<VRC_SpecialLayer>();
		foreach (VRC_SpecialLayer vrc_SpecialLayer in from l in layers
		where l != null && !l.transform.root.gameObject.CompareTag("VRCGlobalRoot")
		select l)
		{
			try
			{
				vrc_SpecialLayer.Apply();
			}
			catch (Exception exception2)
			{
				Debug.LogException(exception2, vrc_SpecialLayer.gameObject);
			}
		}
		yield return null;
		VRCAudioManager.DisableAllExtraAudioListeners();
		yield return null;
		AudioSource[] audioSources = Tools.FindSceneObjectsOfTypeAll<AudioSource>();
		foreach (AudioSource audioSource in audioSources)
		{
			VRCAudioManager.ApplyGameAudioMixerSettings(audioSource);
		}
		yield return null;
		if (this.sceneDescriptor.autoSpatializeAudioSources)
		{
			Debug.Log("Auto spatializing AudioSources...");
			try
			{
				AddONSPAudioSourceComponent.ApplyDefaultSpatializationToAudioSources();
			}
			catch (Exception exception3)
			{
				Debug.LogException(exception3, this.sceneDescriptor.gameObject);
			}
		}
		yield return null;
		onDone();
		yield break;
	}

	// Token: 0x06005BD6 RID: 23510 RVA: 0x00200B5E File Offset: 0x001FEF5E
	private void OnDownloadErrorCallback(string url, string message, LoadErrorReason reason)
	{
		VRC.Core.Logger.LogError("Error Downloading Custom Scene - " + url + " - " + message, DebugLevel.Always);
		UserMessage.SetMessage(message);
		if (this.OnDownloadError != null)
		{
			this.OnDownloadError(message, reason);
		}
	}

	// Token: 0x06005BD7 RID: 23511 RVA: 0x00200B98 File Offset: 0x001FEF98
	public void FinalizeScene()
	{
		VRCVrCamera.IsStereoRequired = false;
		VRC_StereoObject[] array = Tools.FindSceneObjectsOfTypeAll<VRC_StereoObject>();
		if (array.Length > 0)
		{
			VRCVrCamera.IsStereoRequired = true;
			foreach (VRC_StereoObject vrc_StereoObject in array)
			{
				if (vrc_StereoObject.eye == VRC_StereoObject.Eye.Left)
				{
					vrc_StereoObject.gameObject.layer = LayerMask.NameToLayer("StereoLeft");
				}
				else
				{
					vrc_StereoObject.gameObject.layer = LayerMask.NameToLayer("StereoRight");
				}
			}
		}
		VRC_SceneDescriptor instance = VRC_SceneDescriptor.Instance;
		if (instance == null)
		{
			return;
		}
		GameObject gameObject = new GameObject();
		Transform[] array3 = instance.spawns;
		if (array3 == null || array3.Length < 1)
		{
			if (instance.SpawnLocation == null)
			{
				gameObject.transform.position = instance.SpawnPosition;
			}
			else
			{
				Spawn spawn = instance.SpawnLocation.gameObject.AddComponent<Spawn>();
				spawn.prefab = SpawnManager.Instance.playerPrefab;
				gameObject.transform.position = instance.SpawnLocation.position;
				gameObject.transform.rotation = instance.SpawnLocation.rotation;
			}
			array3 = new Transform[]
			{
				gameObject.transform
			};
		}
		else
		{
			SpawnManager.Instance.spawnOrder = instance.spawnOrder;
			SpawnManager.Instance.spawnOrientation = instance.spawnOrientation;
		}
		this.AddSpawnsToSpawnManager(array3);
		RoomManager.SetUserPortalForbid(this.sceneDescriptor.ForbidUserPortals);
		if (QuickMenu.Instance != null && QuickMenu.Instance)
		{
			QuickMenu.Instance.CloseMenu();
		}
	}

	// Token: 0x04004172 RID: 16754
	public float Progress;

	// Token: 0x04004173 RID: 16755
	public CustomScenes.SceneCreationCallback OnSceneCreated;

	// Token: 0x04004174 RID: 16756
	public CustomScenes.DownloadErrorCallback OnDownloadError;

	// Token: 0x04004175 RID: 16757
	private float lastDownloadProgressTime;

	// Token: 0x04004176 RID: 16758
	private int mLastDownloadedAmount = -1;

	// Token: 0x04004177 RID: 16759
	private VRC_SceneDescriptor sceneDescriptor;

	// Token: 0x02000B84 RID: 2948
	// (Invoke) Token: 0x06005BDA RID: 23514
	public delegate void SceneCreationCallback(AssetBundleDownload download);

	// Token: 0x02000B85 RID: 2949
	// (Invoke) Token: 0x06005BDE RID: 23518
	public delegate void DownloadErrorCallback(string error, LoadErrorReason reason);
}
