using System;
using System.Collections;
using UnityEngine;
using VRC;

// Token: 0x02000C59 RID: 3161
public class VRCFlowNetworkManager : MonoBehaviour
{
	// Token: 0x17000DAA RID: 3498
	// (get) Token: 0x0600622E RID: 25134 RVA: 0x0022E23C File Offset: 0x0022C63C
	public static VRCFlowNetworkManager Instance
	{
		get
		{
			return VRCFlowNetworkManager.mInstance;
		}
	}

	// Token: 0x17000DAB RID: 3499
	// (get) Token: 0x0600622F RID: 25135 RVA: 0x0022E243 File Offset: 0x0022C643
	public bool isConnected
	{
		get
		{
			return PhotonNetwork.connected && PhotonNetwork.connectedAndReady;
		}
	}

	// Token: 0x17000DAC RID: 3500
	// (get) Token: 0x06006230 RID: 25136 RVA: 0x0022E257 File Offset: 0x0022C657
	public bool isConnecting
	{
		get
		{
			return PhotonNetwork.connecting && (PhotonNetwork.connectionStateDetailed == ClientState.ConnectingToGameserver || PhotonNetwork.connectionStateDetailed == ClientState.ConnectingToMasterserver || PhotonNetwork.connectionStateDetailed == ClientState.ConnectingToNameServer || PhotonNetwork.connectionStateDetailed == ClientState.Authenticating);
		}
	}

	// Token: 0x06006231 RID: 25137 RVA: 0x0022E295 File Offset: 0x0022C695
	private void Awake()
	{
		if (VRCFlowNetworkManager.mInstance != null)
		{
			Debug.LogError("More than one VRCFlowNetworkManager detected!!!");
			UnityEngine.Object.Destroy(this);
			return;
		}
		VRCFlowNetworkManager.mInstance = this;
	}

	// Token: 0x06006232 RID: 25138 RVA: 0x0022E2C0 File Offset: 0x0022C6C0
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => NetworkManager.Instance != null);
		NetworkManager.Instance.OnPhotonCreateRoomFailedEvent += this.OnPhotonCreateRoomFailed;
		NetworkManager.Instance.OnPhotonJoinRoomFailedEvent += this.OnPhotonJoinRoomFailed;
		NetworkManager.Instance.OnFailedToConnectToPhotonEvent += this.OnFailedToConnectToPhoton;
		NetworkManager.Instance.OnConnectionFailEvent += this.OnConnectionFail;
		NetworkManager.Instance.OnDisconnectedFromPhotonEvent += this.OnDisconnectedFromPhoton;
		NetworkManager.Instance.OnConnectedToMasterEvent += this.OnConnectedToMaster;
		yield break;
	}

	// Token: 0x06006233 RID: 25139 RVA: 0x0022E2DC File Offset: 0x0022C6DC
	private void OnDestroy()
	{
		if (NetworkManager.Instance != null)
		{
			NetworkManager.Instance.OnPhotonCreateRoomFailedEvent -= this.OnPhotonCreateRoomFailed;
			NetworkManager.Instance.OnPhotonJoinRoomFailedEvent -= this.OnPhotonJoinRoomFailed;
			NetworkManager.Instance.OnFailedToConnectToPhotonEvent -= this.OnFailedToConnectToPhoton;
			NetworkManager.Instance.OnConnectionFailEvent -= this.OnConnectionFail;
			NetworkManager.Instance.OnDisconnectedFromPhotonEvent -= this.OnDisconnectedFromPhoton;
			NetworkManager.Instance.OnConnectedToMasterEvent -= this.OnConnectedToMaster;
		}
	}

	// Token: 0x06006234 RID: 25140 RVA: 0x0022E37D File Offset: 0x0022C77D
	public void BeginConnection(bool isRetry)
	{
		this.Connect(isRetry);
	}

	// Token: 0x06006235 RID: 25141 RVA: 0x0022E388 File Offset: 0x0022C788
	private void Connect(bool isRetry)
	{
		if (!this.isConnected && !this.isConnecting)
		{
			Debug.Log("<color=red>Connecting to Photon</color>");
			UserMessage.SetMessage("Connecting to Server...");
			if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated)
			{
				string gameServerVersion = VRCApplicationSetup.Instance.GetGameServerVersion();
				Debug.LogWarning("Using server url: " + gameServerVersion);
				if (!PhotonNetwork.ConnectUsingSettings(gameServerVersion, isRetry))
				{
					Debug.LogError("Could not connect to Photon");
				}
			}
			VRC.Network.SyncNetworkDateTime();
		}
	}

	// Token: 0x06006236 RID: 25142 RVA: 0x0022E400 File Offset: 0x0022C800
	public void Disconnect()
	{
		if ((this.isConnected || this.isConnecting) && PhotonNetwork.networkingPeer.State != ClientState.Disconnected)
		{
			PhotonNetwork.Disconnect();
		}
	}

	// Token: 0x06006237 RID: 25143 RVA: 0x0022E430 File Offset: 0x0022C830
	private void ConnectWithWifi(bool isRetry)
	{
		NetworkReachability internetReachability = Application.internetReachability;
		if (internetReachability != NetworkReachability.NotReachable)
		{
			if (internetReachability != NetworkReachability.ReachableViaCarrierDataNetwork)
			{
				if (internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
				{
					this.Connect(isRetry);
				}
			}
			else
			{
				UserMessage.SetMessage("VRChat requires a Wifi connection. Please make sure you are connected to a Wifi network.");
			}
		}
		else
		{
			UserMessage.SetMessage("VRChat requires a Wifi connection. Please make sure you are connected to the internet.");
		}
	}

	// Token: 0x06006238 RID: 25144 RVA: 0x0022E486 File Offset: 0x0022C886
	private void OnConnectedToMaster()
	{
		Debug.Log("Connect to master");
	}

	// Token: 0x06006239 RID: 25145 RVA: 0x0022E494 File Offset: 0x0022C894
	private void OnPhotonCreateRoomFailed(object[] codeAndMsg)
	{
		object[] array = new object[]
		{
			(codeAndMsg.Length < 1) ? "unknown" : codeAndMsg[0],
			(codeAndMsg.Length < 2) ? "unknown" : codeAndMsg[1]
		};
		Debug.LogError(string.Concat(new object[]
		{
			"OnPhotonCreateRoomFailed. error code = ",
			array[0],
			" msg = ",
			array[1]
		}));
		UserMessage.SetMessage(string.Concat(new object[]
		{
			"OnPhotonCreateRoomFailed. error code = ",
			array[0],
			" msg = ",
			array[1]
		}));
		VRCFlowManager.Instance.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
		{
			VRCFlowManager.ResetGameFlags.SkipLogin,
			VRCFlowManager.ResetGameFlags.ClearDestinationWorld
		});
	}

	// Token: 0x0600623A RID: 25146 RVA: 0x0022E54C File Offset: 0x0022C94C
	private void OnPhotonJoinRoomFailed(object[] codeAndMsg)
	{
		object[] array = new object[]
		{
			(codeAndMsg.Length < 1) ? "unknown" : codeAndMsg[0],
			(codeAndMsg.Length < 2) ? "unknown" : codeAndMsg[1]
		};
		Debug.LogError(string.Concat(new object[]
		{
			"OnPhotonJoinRoomFailed. error code = ",
			array[0],
			" msg = ",
			array[1]
		}));
		UserMessage.SetMessage(string.Concat(new object[]
		{
			"OnPhotonJoinRoomFailed. error code = ",
			array[0],
			" msg = ",
			array[1]
		}));
		VRCFlowManager.Instance.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
		{
			VRCFlowManager.ResetGameFlags.SkipLogin,
			VRCFlowManager.ResetGameFlags.TryAlternateInstance
		});
	}

	// Token: 0x0600623B RID: 25147 RVA: 0x0022E604 File Offset: 0x0022CA04
	private void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.LogError("OnFailedToConnectToPhoton - " + cause);
		UserMessage.SetMessage("[" + cause + "] Could not connect to server. Make sure you're connected to the internet. Left click or tap trackpad to try reconnecting.");
		VRCFlowManager.Instance.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
		{
			VRCFlowManager.ResetGameFlags.SkipLogin,
			VRCFlowManager.ResetGameFlags.IsConnectionRetry
		});
	}

	// Token: 0x0600623C RID: 25148 RVA: 0x0022E658 File Offset: 0x0022CA58
	private void OnConnectionFail(DisconnectCause cause)
	{
		Debug.LogError("OnConnectionFail: " + cause);
		if (PhotonNetwork.networkingPeer.ResentReliableCommands > 0)
		{
			Debug.LogError("ResentReliableCommands: " + PhotonNetwork.networkingPeer.ResentReliableCommands);
		}
		if (PhotonNetwork.PacketLossByCrcCheck > 0)
		{
			Debug.LogError("PacketLossByCrcCheck: " + PhotonNetwork.PacketLossByCrcCheck);
		}
		UserMessage.SetMessage("[" + cause + "] Disconnected from server. Make sure you're still connected to the internet. Left click or tap trackpad to try reconnecting.");
		VRCFlowManager.Instance.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
		{
			VRCFlowManager.ResetGameFlags.SkipLogin,
			VRCFlowManager.ResetGameFlags.IsConnectionRetry
		});
	}

	// Token: 0x0600623D RID: 25149 RVA: 0x0022E6FE File Offset: 0x0022CAFE
	private void OnDisconnectedFromPhoton()
	{
		Debug.Log("Disconnected from Photon");
	}

	// Token: 0x0600623E RID: 25150 RVA: 0x0022E70A File Offset: 0x0022CB0A
	public static void TestConnectionFail(DisconnectCause cause)
	{
		VRCFlowNetworkManager.Instance.OnConnectionFail(cause);
	}

	// Token: 0x040047AB RID: 18347
	private static VRCFlowNetworkManager mInstance;
}
