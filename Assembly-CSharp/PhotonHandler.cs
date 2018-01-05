using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200075D RID: 1885
internal class PhotonHandler : MonoBehaviour
{
	// Token: 0x06003CF2 RID: 15602 RVA: 0x00134038 File Offset: 0x00132438
	protected void Awake()
	{
		if (PhotonHandler.SP != null && PhotonHandler.SP != this && PhotonHandler.SP.gameObject != null)
		{
			UnityEngine.Object.DestroyImmediate(PhotonHandler.SP.gameObject);
		}
		PhotonHandler.SP = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.updateInterval = 1000 / PhotonNetwork.sendRate;
		this.updateIntervalOnSerialize = 1000 / PhotonNetwork.sendRateOnSerialize;
		PhotonHandler.StartFallbackSendAckThread();
	}

	// Token: 0x06003CF3 RID: 15603 RVA: 0x001340C1 File Offset: 0x001324C1
	protected void Start()
	{
		SceneManager.sceneLoaded += delegate(Scene scene, LoadSceneMode loadingMode)
		{
			PhotonNetwork.networkingPeer.NewSceneLoaded();
			PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(SceneManagerHelper.ActiveSceneName);
		};
	}

	// Token: 0x06003CF4 RID: 15604 RVA: 0x001340E5 File Offset: 0x001324E5
	protected void OnApplicationQuit()
	{
		PhotonHandler.AppQuits = true;
		PhotonHandler.StopFallbackSendAckThread();
		PhotonNetwork.Disconnect();
	}

	// Token: 0x06003CF5 RID: 15605 RVA: 0x001340F8 File Offset: 0x001324F8
	protected void OnApplicationPause(bool pause)
	{
		if (PhotonNetwork.BackgroundTimeout > 0.1f)
		{
			if (PhotonHandler.timerToStopConnectionInBackground == null)
			{
				PhotonHandler.timerToStopConnectionInBackground = new Stopwatch();
			}
			PhotonHandler.timerToStopConnectionInBackground.Reset();
			if (pause)
			{
				PhotonHandler.timerToStopConnectionInBackground.Start();
			}
			else
			{
				PhotonHandler.timerToStopConnectionInBackground.Stop();
			}
		}
	}

	// Token: 0x06003CF6 RID: 15606 RVA: 0x00134151 File Offset: 0x00132551
	protected void OnDestroy()
	{
		PhotonHandler.StopFallbackSendAckThread();
	}

	// Token: 0x06003CF7 RID: 15607 RVA: 0x00134158 File Offset: 0x00132558
	protected void Update()
	{
		if (PhotonNetwork.networkingPeer == null)
		{
			UnityEngine.Debug.LogError("NetworkPeer broke!");
			return;
		}
		if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated || PhotonNetwork.connectionStateDetailed == ClientState.Disconnected || PhotonNetwork.offlineMode)
		{
			return;
		}
		if (!PhotonNetwork.isMessageQueueRunning)
		{
			return;
		}
		bool flag = true;
		while (PhotonNetwork.isMessageQueueRunning && flag)
		{
			flag = PhotonNetwork.networkingPeer.DispatchIncomingCommands();
		}
		int num = (int)(Time.realtimeSinceStartup * 1000f);
		if (PhotonNetwork.isMessageQueueRunning && num > this.nextSendTickCountOnSerialize)
		{
			PhotonNetwork.networkingPeer.RunViewUpdate();
			this.nextSendTickCountOnSerialize = num + this.updateIntervalOnSerialize;
			this.nextSendTickCount = 0;
		}
		num = (int)(Time.realtimeSinceStartup * 1000f);
		if (num > this.nextSendTickCount)
		{
			bool flag2 = true;
			while (PhotonNetwork.isMessageQueueRunning && flag2)
			{
				flag2 = PhotonNetwork.networkingPeer.SendOutgoingCommands();
			}
			this.nextSendTickCount = num + this.updateInterval;
		}
	}

	// Token: 0x06003CF8 RID: 15608 RVA: 0x00134254 File Offset: 0x00132654
	protected void OnJoinedRoom()
	{
		PhotonNetwork.networkingPeer.LoadLevelIfSynced();
	}

	// Token: 0x06003CF9 RID: 15609 RVA: 0x00134260 File Offset: 0x00132660
	protected void OnCreatedRoom()
	{
		PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(SceneManagerHelper.ActiveSceneName);
	}

	// Token: 0x06003CFA RID: 15610 RVA: 0x00134271 File Offset: 0x00132671
	public static void StartFallbackSendAckThread()
	{
		if (PhotonHandler.sendThreadShouldRun)
		{
			return;
		}
		PhotonHandler.sendThreadShouldRun = true;
		if (PhotonHandler.f__mg0 == null)
		{
			PhotonHandler.f__mg0 = new Func<bool>(PhotonHandler.FallbackSendAckThread);
		}
		SupportClass.CallInBackground(PhotonHandler.f__mg0);
	}

	// Token: 0x06003CFB RID: 15611 RVA: 0x001342A6 File Offset: 0x001326A6
	public static void StopFallbackSendAckThread()
	{
		PhotonHandler.sendThreadShouldRun = false;
	}

	// Token: 0x06003CFC RID: 15612 RVA: 0x001342B0 File Offset: 0x001326B0
	public static bool FallbackSendAckThread()
	{
		if (PhotonHandler.sendThreadShouldRun && !PhotonNetwork.offlineMode && PhotonNetwork.networkingPeer != null)
		{
			if (PhotonHandler.timerToStopConnectionInBackground != null && PhotonNetwork.BackgroundTimeout > 0.1f && (float)PhotonHandler.timerToStopConnectionInBackground.ElapsedMilliseconds > PhotonNetwork.BackgroundTimeout * 1000f)
			{
				if (PhotonNetwork.connected)
				{
					PhotonNetwork.Disconnect();
				}
				PhotonHandler.timerToStopConnectionInBackground.Stop();
				PhotonHandler.timerToStopConnectionInBackground.Reset();
				return PhotonHandler.sendThreadShouldRun;
			}
			if (PhotonNetwork.networkingPeer.ConnectionTime - PhotonNetwork.networkingPeer.LastSendOutgoingTime > 200)
			{
				PhotonNetwork.networkingPeer.SendAcksOnly();
			}
		}
		return PhotonHandler.sendThreadShouldRun;
	}

	// Token: 0x1700098F RID: 2447
	// (get) Token: 0x06003CFD RID: 15613 RVA: 0x00134368 File Offset: 0x00132768
	// (set) Token: 0x06003CFE RID: 15614 RVA: 0x0013439A File Offset: 0x0013279A
	internal static CloudRegionCode BestRegionCodeInPreferences
	{
		get
		{
			string @string = PlayerPrefs.GetString("PUNCloudBestRegion", string.Empty);
			if (!string.IsNullOrEmpty(@string))
			{
				return Region.Parse(@string);
			}
			return CloudRegionCode.none;
		}
		set
		{
			if (value == CloudRegionCode.none)
			{
				PlayerPrefs.DeleteKey("PUNCloudBestRegion");
			}
			else
			{
				PlayerPrefs.SetString("PUNCloudBestRegion", value.ToString());
			}
		}
	}

	// Token: 0x06003CFF RID: 15615 RVA: 0x001343C9 File Offset: 0x001327C9
	protected internal static void PingAvailableRegionsAndConnectToBest()
	{
		PhotonHandler.SP.StartCoroutine(PhotonHandler.SP.PingAvailableRegionsCoroutine(true));
	}

	// Token: 0x06003D00 RID: 15616 RVA: 0x001343E4 File Offset: 0x001327E4
	internal IEnumerator PingAvailableRegionsCoroutine(bool connectToBest)
	{
		while (PhotonNetwork.networkingPeer.AvailableRegions == null)
		{
			if (PhotonNetwork.connectionStateDetailed != ClientState.ConnectingToNameServer && PhotonNetwork.connectionStateDetailed != ClientState.ConnectedToNameServer)
			{
				UnityEngine.Debug.LogError("Call ConnectToNameServer to ping available regions.");
				yield break;
			}
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Waiting for AvailableRegions. State: ",
				PhotonNetwork.connectionStateDetailed,
				" Server: ",
				PhotonNetwork.Server,
				" PhotonNetwork.networkingPeer.AvailableRegions ",
				PhotonNetwork.networkingPeer.AvailableRegions != null
			}));
			yield return new WaitForSeconds(0.25f);
		}
		if (PhotonNetwork.networkingPeer.AvailableRegions == null || PhotonNetwork.networkingPeer.AvailableRegions.Count == 0)
		{
			UnityEngine.Debug.LogError("No regions available. Are you sure your appid is valid and setup?");
			yield break;
		}
		PhotonPingManager pingManager = new PhotonPingManager();
		foreach (Region region in PhotonNetwork.networkingPeer.AvailableRegions)
		{
			PhotonHandler.SP.StartCoroutine(pingManager.PingSocket(region));
		}
		while (!pingManager.Done)
		{
			yield return new WaitForSeconds(0.1f);
		}
		Region best = pingManager.BestRegion;
		PhotonHandler.BestRegionCodeInPreferences = best.Code;
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Found best region: '",
			best.Code,
			"' ping: ",
			best.Ping,
			". Calling ConnectToRegionMaster() is: ",
			connectToBest
		}));
		if (connectToBest)
		{
			PhotonNetwork.networkingPeer.ConnectToRegionMaster(best.Code);
		}
		yield break;
	}

	// Token: 0x0400262A RID: 9770
	public static PhotonHandler SP;

	// Token: 0x0400262B RID: 9771
	public int updateInterval;

	// Token: 0x0400262C RID: 9772
	public int updateIntervalOnSerialize;

	// Token: 0x0400262D RID: 9773
	private int nextSendTickCount;

	// Token: 0x0400262E RID: 9774
	private int nextSendTickCountOnSerialize;

	// Token: 0x0400262F RID: 9775
	private static bool sendThreadShouldRun;

	// Token: 0x04002630 RID: 9776
	private static Stopwatch timerToStopConnectionInBackground;

	// Token: 0x04002631 RID: 9777
	protected internal static bool AppQuits;

	// Token: 0x04002632 RID: 9778
	protected internal static Type PingImplementation;

	// Token: 0x04002633 RID: 9779
	private const string PlayerPrefsKey = "PUNCloudBestRegion";

	// Token: 0x04002635 RID: 9781
	[CompilerGenerated]
	private static Func<bool> f__mg0;
}
