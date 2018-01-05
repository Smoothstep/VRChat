using System;
using System.Collections.Generic;
using System.Diagnostics;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200075F RID: 1887
public static class PhotonNetwork
{
	// Token: 0x06003D0A RID: 15626 RVA: 0x00134988 File Offset: 0x00132D88
	static PhotonNetwork()
	{
		if (PhotonNetwork.PhotonServerSettings != null)
		{
			Application.runInBackground = PhotonNetwork.PhotonServerSettings.RunInBackground;
		}
		GameObject gameObject = new GameObject();
		PhotonNetwork.photonMono = gameObject.AddComponent<PhotonHandler>();
		gameObject.name = "PhotonMono";
		gameObject.hideFlags = HideFlags.HideInHierarchy;
		ConnectionProtocol protocol = PhotonNetwork.PhotonServerSettings.Protocol;
		PhotonNetwork.networkingPeer = new NetworkingPeer(string.Empty, protocol);
		PhotonNetwork.networkingPeer.QuickResendAttempts = 3;
		PhotonNetwork.networkingPeer.SentCountAllowance = 7;
		if (PhotonNetwork.UsePreciseTimer)
		{
			UnityEngine.Debug.Log("Using Stopwatch as precision timer for PUN.");
			PhotonNetwork.startupStopwatch = new Stopwatch();
			PhotonNetwork.startupStopwatch.Start();
			PhotonNetwork.networkingPeer.LocalMsTimestampDelegate = (() => (int)PhotonNetwork.startupStopwatch.ElapsedMilliseconds);
		}
		CustomTypes.Register();
	}

	// Token: 0x17000991 RID: 2449
	// (get) Token: 0x06003D0B RID: 15627 RVA: 0x00134B22 File Offset: 0x00132F22
	// (set) Token: 0x06003D0C RID: 15628 RVA: 0x00134B29 File Offset: 0x00132F29
	public static string gameVersion { get; set; }

	// Token: 0x17000992 RID: 2450
	// (get) Token: 0x06003D0D RID: 15629 RVA: 0x00134B31 File Offset: 0x00132F31
	public static string ServerAddress
	{
		get
		{
			return (PhotonNetwork.networkingPeer == null) ? "<not connected>" : PhotonNetwork.networkingPeer.ServerAddress;
		}
	}

	// Token: 0x17000993 RID: 2451
	// (get) Token: 0x06003D0E RID: 15630 RVA: 0x00134B51 File Offset: 0x00132F51
	public static CloudRegionCode CloudRegion
	{
		get
		{
			return (PhotonNetwork.networkingPeer == null || !PhotonNetwork.connected || PhotonNetwork.Server == ServerConnection.NameServer) ? CloudRegionCode.none : PhotonNetwork.networkingPeer.CloudRegion;
		}
	}

	// Token: 0x17000994 RID: 2452
	// (get) Token: 0x06003D0F RID: 15631 RVA: 0x00134B84 File Offset: 0x00132F84
	public static bool connected
	{
		get
		{
			return PhotonNetwork.offlineMode || (PhotonNetwork.networkingPeer != null && (!PhotonNetwork.networkingPeer.IsInitialConnect && PhotonNetwork.networkingPeer.State != ClientState.PeerCreated && PhotonNetwork.networkingPeer.State != ClientState.Disconnected && PhotonNetwork.networkingPeer.State != ClientState.Disconnecting) && PhotonNetwork.networkingPeer.State != ClientState.ConnectingToNameServer);
		}
	}

	// Token: 0x17000995 RID: 2453
	// (get) Token: 0x06003D10 RID: 15632 RVA: 0x00134BFE File Offset: 0x00132FFE
	public static bool connecting
	{
		get
		{
			return PhotonNetwork.networkingPeer.IsInitialConnect && !PhotonNetwork.offlineMode;
		}
	}

	// Token: 0x17000996 RID: 2454
	// (get) Token: 0x06003D11 RID: 15633 RVA: 0x00134C1C File Offset: 0x0013301C
	public static bool connectedAndReady
	{
		get
		{
			if (!PhotonNetwork.connected)
			{
				return false;
			}
			if (PhotonNetwork.offlineMode)
			{
				return true;
			}
			ClientState connectionStateDetailed = PhotonNetwork.connectionStateDetailed;
			switch (connectionStateDetailed)
			{
			case ClientState.ConnectingToMasterserver:
			case ClientState.Disconnecting:
			case ClientState.Disconnected:
			case ClientState.ConnectingToNameServer:
			case ClientState.Authenticating:
				break;
			default:
				switch (connectionStateDetailed)
				{
				case ClientState.ConnectingToGameserver:
				case ClientState.Joining:
					break;
				default:
					if (connectionStateDetailed != ClientState.PeerCreated)
					{
						return true;
					}
					break;
				}
				break;
			}
			return false;
		}
	}

	// Token: 0x17000997 RID: 2455
	// (get) Token: 0x06003D12 RID: 15634 RVA: 0x00134C98 File Offset: 0x00133098
	public static ConnectionState connectionState
	{
		get
		{
			if (PhotonNetwork.offlineMode)
			{
				return ConnectionState.Connected;
			}
			if (PhotonNetwork.networkingPeer == null)
			{
				return ConnectionState.Disconnected;
			}
			PeerStateValue peerState = PhotonNetwork.networkingPeer.PeerState;
			switch (peerState)
			{
			case PeerStateValue.Disconnected:
				return ConnectionState.Disconnected;
			case PeerStateValue.Connecting:
				return ConnectionState.Connecting;
			default:
				if (peerState != PeerStateValue.InitializingApplication)
				{
					return ConnectionState.Disconnected;
				}
				return ConnectionState.InitializingApplication;
			case PeerStateValue.Connected:
				return ConnectionState.Connected;
			case PeerStateValue.Disconnecting:
				return ConnectionState.Disconnecting;
			}
		}
	}

	// Token: 0x17000998 RID: 2456
	// (get) Token: 0x06003D13 RID: 15635 RVA: 0x00134CFA File Offset: 0x001330FA
	public static ClientState connectionStateDetailed
	{
		get
		{
			if (PhotonNetwork.offlineMode)
			{
				return (PhotonNetwork.offlineModeRoom == null) ? ClientState.ConnectedToMaster : ClientState.Joined;
			}
			if (PhotonNetwork.networkingPeer == null)
			{
				return ClientState.Disconnected;
			}
			return PhotonNetwork.networkingPeer.State;
		}
	}

	// Token: 0x17000999 RID: 2457
	// (get) Token: 0x06003D14 RID: 15636 RVA: 0x00134D31 File Offset: 0x00133131
	public static ServerConnection Server
	{
		get
		{
			return (PhotonNetwork.networkingPeer == null) ? ServerConnection.NameServer : PhotonNetwork.networkingPeer.Server;
		}
	}

	// Token: 0x1700099A RID: 2458
	// (get) Token: 0x06003D15 RID: 15637 RVA: 0x00134D4D File Offset: 0x0013314D
	// (set) Token: 0x06003D16 RID: 15638 RVA: 0x00134D69 File Offset: 0x00133169
	public static AuthenticationValues AuthValues
	{
		get
		{
			return (PhotonNetwork.networkingPeer == null) ? null : PhotonNetwork.networkingPeer.AuthValues;
		}
		set
		{
			if (PhotonNetwork.networkingPeer != null)
			{
				PhotonNetwork.networkingPeer.AuthValues = value;
			}
		}
	}

	// Token: 0x1700099B RID: 2459
	// (get) Token: 0x06003D17 RID: 15639 RVA: 0x00134D80 File Offset: 0x00133180
	public static Room room
	{
		get
		{
			if (PhotonNetwork.isOfflineMode)
			{
				return PhotonNetwork.offlineModeRoom;
			}
			return PhotonNetwork.networkingPeer.CurrentRoom;
		}
	}

	// Token: 0x1700099C RID: 2460
	// (get) Token: 0x06003D18 RID: 15640 RVA: 0x00134D9C File Offset: 0x0013319C
	public static PhotonPlayer player
	{
		get
		{
			if (PhotonNetwork.networkingPeer == null)
			{
				return null;
			}
			return PhotonNetwork.networkingPeer.LocalPlayer;
		}
	}

	// Token: 0x1700099D RID: 2461
	// (get) Token: 0x06003D19 RID: 15641 RVA: 0x00134DB4 File Offset: 0x001331B4
	public static PhotonPlayer masterClient
	{
		get
		{
			if (PhotonNetwork.offlineMode)
			{
				return PhotonNetwork.player;
			}
			if (PhotonNetwork.networkingPeer == null)
			{
				return null;
			}
			return PhotonNetwork.networkingPeer.GetPlayerWithId(PhotonNetwork.networkingPeer.mMasterClientId);
		}
	}

	// Token: 0x1700099E RID: 2462
	// (get) Token: 0x06003D1A RID: 15642 RVA: 0x00134DE6 File Offset: 0x001331E6
	// (set) Token: 0x06003D1B RID: 15643 RVA: 0x00134DF2 File Offset: 0x001331F2
	public static string playerName
	{
		get
		{
			return PhotonNetwork.networkingPeer.PlayerName;
		}
		set
		{
			PhotonNetwork.networkingPeer.PlayerName = value;
		}
	}

	// Token: 0x1700099F RID: 2463
	// (get) Token: 0x06003D1C RID: 15644 RVA: 0x00134DFF File Offset: 0x001331FF
	public static PhotonPlayer[] playerList
	{
		get
		{
			if (PhotonNetwork.networkingPeer == null)
			{
				return new PhotonPlayer[0];
			}
			return PhotonNetwork.networkingPeer.mPlayerListCopy;
		}
	}

	// Token: 0x170009A0 RID: 2464
	// (get) Token: 0x06003D1D RID: 15645 RVA: 0x00134E1C File Offset: 0x0013321C
	public static PhotonPlayer[] otherPlayers
	{
		get
		{
			if (PhotonNetwork.networkingPeer == null)
			{
				return new PhotonPlayer[0];
			}
			return PhotonNetwork.networkingPeer.mOtherPlayerListCopy;
		}
	}

	// Token: 0x170009A1 RID: 2465
	// (get) Token: 0x06003D1E RID: 15646 RVA: 0x00134E39 File Offset: 0x00133239
	// (set) Token: 0x06003D1F RID: 15647 RVA: 0x00134E40 File Offset: 0x00133240
	public static List<FriendInfo> Friends { get; internal set; }

	// Token: 0x170009A2 RID: 2466
	// (get) Token: 0x06003D20 RID: 15648 RVA: 0x00134E48 File Offset: 0x00133248
	public static int FriendsListAge
	{
		get
		{
			return (PhotonNetwork.networkingPeer == null) ? 0 : PhotonNetwork.networkingPeer.FriendListAge;
		}
	}

	// Token: 0x170009A3 RID: 2467
	// (get) Token: 0x06003D21 RID: 15649 RVA: 0x00134E64 File Offset: 0x00133264
	// (set) Token: 0x06003D22 RID: 15650 RVA: 0x00134E70 File Offset: 0x00133270
	public static IPunPrefabPool PrefabPool
	{
		get
		{
			return PhotonNetwork.networkingPeer.ObjectPool;
		}
		set
		{
			PhotonNetwork.networkingPeer.ObjectPool = value;
		}
	}

	// Token: 0x170009A4 RID: 2468
	// (get) Token: 0x06003D23 RID: 15651 RVA: 0x00134E7D File Offset: 0x0013327D
	// (set) Token: 0x06003D24 RID: 15652 RVA: 0x00134E84 File Offset: 0x00133284
	public static bool offlineMode
	{
		get
		{
			return PhotonNetwork.isOfflineMode;
		}
		set
		{
			if (value == PhotonNetwork.isOfflineMode)
			{
				return;
			}
			if (value && PhotonNetwork.connected)
			{
				UnityEngine.Debug.LogError("Can't start OFFLINE mode while connected!");
				return;
			}
			if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
			{
				PhotonNetwork.networkingPeer.Disconnect();
			}
			PhotonNetwork.isOfflineMode = value;
			if (PhotonNetwork.isOfflineMode)
			{
				PhotonNetwork.networkingPeer.ChangeLocalID(-1);
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster, new object[0]);
			}
			else
			{
				PhotonNetwork.offlineModeRoom = null;
				PhotonNetwork.networkingPeer.ChangeLocalID(-1);
			}
		}
	}

	// Token: 0x170009A5 RID: 2469
	// (get) Token: 0x06003D25 RID: 15653 RVA: 0x00134F0F File Offset: 0x0013330F
	// (set) Token: 0x06003D26 RID: 15654 RVA: 0x00134F16 File Offset: 0x00133316
	public static bool automaticallySyncScene
	{
		get
		{
			return PhotonNetwork._mAutomaticallySyncScene;
		}
		set
		{
			PhotonNetwork._mAutomaticallySyncScene = value;
			if (PhotonNetwork._mAutomaticallySyncScene && PhotonNetwork.room != null)
			{
				PhotonNetwork.networkingPeer.LoadLevelIfSynced();
			}
		}
	}

	// Token: 0x170009A6 RID: 2470
	// (get) Token: 0x06003D27 RID: 15655 RVA: 0x00134F3C File Offset: 0x0013333C
	// (set) Token: 0x06003D28 RID: 15656 RVA: 0x00134F43 File Offset: 0x00133343
	public static bool autoCleanUpPlayerObjects
	{
		get
		{
			return PhotonNetwork.m_autoCleanUpPlayerObjects;
		}
		set
		{
			if (PhotonNetwork.room != null)
			{
				UnityEngine.Debug.LogError("Setting autoCleanUpPlayerObjects while in a room is not supported.");
			}
			else
			{
				PhotonNetwork.m_autoCleanUpPlayerObjects = value;
			}
		}
	}

	// Token: 0x170009A7 RID: 2471
	// (get) Token: 0x06003D29 RID: 15657 RVA: 0x00134F64 File Offset: 0x00133364
	// (set) Token: 0x06003D2A RID: 15658 RVA: 0x00134F70 File Offset: 0x00133370
	public static bool autoJoinLobby
	{
		get
		{
			return PhotonNetwork.PhotonServerSettings.JoinLobby;
		}
		set
		{
			PhotonNetwork.PhotonServerSettings.JoinLobby = value;
		}
	}

	// Token: 0x170009A8 RID: 2472
	// (get) Token: 0x06003D2B RID: 15659 RVA: 0x00134F7D File Offset: 0x0013337D
	// (set) Token: 0x06003D2C RID: 15660 RVA: 0x00134F89 File Offset: 0x00133389
	public static bool EnableLobbyStatistics
	{
		get
		{
			return PhotonNetwork.PhotonServerSettings.EnableLobbyStatistics;
		}
		set
		{
			PhotonNetwork.PhotonServerSettings.EnableLobbyStatistics = value;
		}
	}

	// Token: 0x170009A9 RID: 2473
	// (get) Token: 0x06003D2D RID: 15661 RVA: 0x00134F96 File Offset: 0x00133396
	// (set) Token: 0x06003D2E RID: 15662 RVA: 0x00134FA2 File Offset: 0x001333A2
	public static List<TypedLobbyInfo> LobbyStatistics
	{
		get
		{
			return PhotonNetwork.networkingPeer.LobbyStatistics;
		}
		private set
		{
			PhotonNetwork.networkingPeer.LobbyStatistics = value;
		}
	}

	// Token: 0x170009AA RID: 2474
	// (get) Token: 0x06003D2F RID: 15663 RVA: 0x00134FAF File Offset: 0x001333AF
	public static bool insideLobby
	{
		get
		{
			return PhotonNetwork.networkingPeer.insideLobby;
		}
	}

	// Token: 0x170009AB RID: 2475
	// (get) Token: 0x06003D30 RID: 15664 RVA: 0x00134FBB File Offset: 0x001333BB
	// (set) Token: 0x06003D31 RID: 15665 RVA: 0x00134FC7 File Offset: 0x001333C7
	public static TypedLobby lobby
	{
		get
		{
			return PhotonNetwork.networkingPeer.lobby;
		}
		set
		{
			PhotonNetwork.networkingPeer.lobby = value;
		}
	}

	// Token: 0x170009AC RID: 2476
	// (get) Token: 0x06003D32 RID: 15666 RVA: 0x00134FD4 File Offset: 0x001333D4
	// (set) Token: 0x06003D33 RID: 15667 RVA: 0x00134FE1 File Offset: 0x001333E1
	public static int sendRate
	{
		get
		{
			return 1000 / PhotonNetwork.sendInterval;
		}
		set
		{
			PhotonNetwork.sendInterval = 1000 / value;
			if (PhotonNetwork.photonMono != null)
			{
				PhotonNetwork.photonMono.updateInterval = PhotonNetwork.sendInterval;
			}
			if (value < PhotonNetwork.sendRateOnSerialize)
			{
				PhotonNetwork.sendRateOnSerialize = value;
			}
		}
	}

	// Token: 0x170009AD RID: 2477
	// (get) Token: 0x06003D34 RID: 15668 RVA: 0x0013501F File Offset: 0x0013341F
	// (set) Token: 0x06003D35 RID: 15669 RVA: 0x0013502C File Offset: 0x0013342C
	public static int sendRateOnSerialize
	{
		get
		{
			return 1000 / PhotonNetwork.sendIntervalOnSerialize;
		}
		set
		{
			if (value > PhotonNetwork.sendRate)
			{
				UnityEngine.Debug.LogError("Error: Can not set the OnSerialize rate higher than the overall SendRate.");
				value = PhotonNetwork.sendRate;
			}
			PhotonNetwork.sendIntervalOnSerialize = 1000 / value;
			if (PhotonNetwork.photonMono != null)
			{
				PhotonNetwork.photonMono.updateIntervalOnSerialize = PhotonNetwork.sendIntervalOnSerialize;
			}
		}
	}

	// Token: 0x170009AE RID: 2478
	// (get) Token: 0x06003D36 RID: 15670 RVA: 0x00135080 File Offset: 0x00133480
	// (set) Token: 0x06003D37 RID: 15671 RVA: 0x00135087 File Offset: 0x00133487
	public static bool isMessageQueueRunning
	{
		get
		{
			return PhotonNetwork.m_isMessageQueueRunning;
		}
		set
		{
			if (value)
			{
				PhotonHandler.StartFallbackSendAckThread();
			}
			PhotonNetwork.networkingPeer.IsSendingOnlyAcks = !value;
			PhotonNetwork.m_isMessageQueueRunning = value;
		}
	}

	// Token: 0x170009AF RID: 2479
	// (get) Token: 0x06003D38 RID: 15672 RVA: 0x001350A8 File Offset: 0x001334A8
	// (set) Token: 0x06003D39 RID: 15673 RVA: 0x001350B4 File Offset: 0x001334B4
	public static int unreliableCommandsLimit
	{
		get
		{
			return PhotonNetwork.networkingPeer.LimitOfUnreliableCommands;
		}
		set
		{
			PhotonNetwork.networkingPeer.LimitOfUnreliableCommands = value;
		}
	}

	// Token: 0x170009B0 RID: 2480
	// (get) Token: 0x06003D3A RID: 15674 RVA: 0x001350C4 File Offset: 0x001334C4
	public static double time
	{
		get
		{
			uint serverTimestamp = (uint)PhotonNetwork.ServerTimestamp;
			double num = serverTimestamp;
			return num / 1000.0;
		}
	}

	// Token: 0x170009B1 RID: 2481
	// (get) Token: 0x06003D3B RID: 15675 RVA: 0x001350E8 File Offset: 0x001334E8
	public static int ServerTimestamp
	{
		get
		{
			if (!PhotonNetwork.offlineMode)
			{
				return PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds;
			}
			if (PhotonNetwork.UsePreciseTimer && PhotonNetwork.startupStopwatch != null && PhotonNetwork.startupStopwatch.IsRunning)
			{
				return (int)PhotonNetwork.startupStopwatch.ElapsedMilliseconds;
			}
			return Environment.TickCount;
		}
	}

	// Token: 0x170009B2 RID: 2482
	// (get) Token: 0x06003D3C RID: 15676 RVA: 0x0013513E File Offset: 0x0013353E
	public static bool isMasterClient
	{
		get
		{
			return PhotonNetwork.offlineMode || PhotonNetwork.networkingPeer.mMasterClientId == PhotonNetwork.player.ID;
		}
	}

	// Token: 0x170009B3 RID: 2483
	// (get) Token: 0x06003D3D RID: 15677 RVA: 0x00135162 File Offset: 0x00133562
	public static bool inRoom
	{
		get
		{
			return PhotonNetwork.connectionStateDetailed == ClientState.Joined;
		}
	}

	// Token: 0x170009B4 RID: 2484
	// (get) Token: 0x06003D3E RID: 15678 RVA: 0x0013516D File Offset: 0x0013356D
	public static bool isNonMasterClientInRoom
	{
		get
		{
			return !PhotonNetwork.isMasterClient && PhotonNetwork.room != null;
		}
	}

	// Token: 0x170009B5 RID: 2485
	// (get) Token: 0x06003D3F RID: 15679 RVA: 0x00135187 File Offset: 0x00133587
	public static int countOfPlayersOnMaster
	{
		get
		{
			return PhotonNetwork.networkingPeer.PlayersOnMasterCount;
		}
	}

	// Token: 0x170009B6 RID: 2486
	// (get) Token: 0x06003D40 RID: 15680 RVA: 0x00135193 File Offset: 0x00133593
	public static int countOfPlayersInRooms
	{
		get
		{
			return PhotonNetwork.networkingPeer.PlayersInRoomsCount;
		}
	}

	// Token: 0x170009B7 RID: 2487
	// (get) Token: 0x06003D41 RID: 15681 RVA: 0x0013519F File Offset: 0x0013359F
	public static int countOfPlayers
	{
		get
		{
			return PhotonNetwork.networkingPeer.PlayersInRoomsCount + PhotonNetwork.networkingPeer.PlayersOnMasterCount;
		}
	}

	// Token: 0x170009B8 RID: 2488
	// (get) Token: 0x06003D42 RID: 15682 RVA: 0x001351B6 File Offset: 0x001335B6
	public static int countOfRooms
	{
		get
		{
			return PhotonNetwork.networkingPeer.RoomsCount;
		}
	}

	// Token: 0x170009B9 RID: 2489
	// (get) Token: 0x06003D43 RID: 15683 RVA: 0x001351C2 File Offset: 0x001335C2
	// (set) Token: 0x06003D44 RID: 15684 RVA: 0x001351CE File Offset: 0x001335CE
	public static bool NetworkStatisticsEnabled
	{
		get
		{
			return PhotonNetwork.networkingPeer.TrafficStatsEnabled;
		}
		set
		{
			PhotonNetwork.networkingPeer.TrafficStatsEnabled = value;
		}
	}

	// Token: 0x170009BA RID: 2490
	// (get) Token: 0x06003D45 RID: 15685 RVA: 0x001351DB File Offset: 0x001335DB
	public static int ResentReliableCommands
	{
		get
		{
			return PhotonNetwork.networkingPeer.ResentReliableCommands;
		}
	}

	// Token: 0x170009BB RID: 2491
	// (get) Token: 0x06003D46 RID: 15686 RVA: 0x001351E7 File Offset: 0x001335E7
	// (set) Token: 0x06003D47 RID: 15687 RVA: 0x001351F4 File Offset: 0x001335F4
	public static bool CrcCheckEnabled
	{
		get
		{
			return PhotonNetwork.networkingPeer.CrcEnabled;
		}
		set
		{
			if (!PhotonNetwork.connected && !PhotonNetwork.connecting)
			{
				PhotonNetwork.networkingPeer.CrcEnabled = value;
			}
			else
			{
				UnityEngine.Debug.Log("Can't change CrcCheckEnabled while being connected. CrcCheckEnabled stays " + PhotonNetwork.networkingPeer.CrcEnabled);
			}
		}
	}

	// Token: 0x170009BC RID: 2492
	// (get) Token: 0x06003D48 RID: 15688 RVA: 0x00135243 File Offset: 0x00133643
	public static int PacketLossByCrcCheck
	{
		get
		{
			return PhotonNetwork.networkingPeer.PacketLossByCrc;
		}
	}

	// Token: 0x170009BD RID: 2493
	// (get) Token: 0x06003D49 RID: 15689 RVA: 0x0013524F File Offset: 0x0013364F
	// (set) Token: 0x06003D4A RID: 15690 RVA: 0x0013525B File Offset: 0x0013365B
	public static int MaxResendsBeforeDisconnect
	{
		get
		{
			return PhotonNetwork.networkingPeer.SentCountAllowance;
		}
		set
		{
			if (value < 3)
			{
				value = 3;
			}
			if (value > 10)
			{
				value = 10;
			}
			PhotonNetwork.networkingPeer.SentCountAllowance = value;
		}
	}

	// Token: 0x170009BE RID: 2494
	// (get) Token: 0x06003D4B RID: 15691 RVA: 0x0013527E File Offset: 0x0013367E
	// (set) Token: 0x06003D4C RID: 15692 RVA: 0x0013528A File Offset: 0x0013368A
	public static int QuickResends
	{
		get
		{
			return (int)PhotonNetwork.networkingPeer.QuickResendAttempts;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			if (value > 3)
			{
				value = 3;
			}
			PhotonNetwork.networkingPeer.QuickResendAttempts = (byte)value;
		}
	}

	// Token: 0x06003D4D RID: 15693 RVA: 0x001352AC File Offset: 0x001336AC
	public static void SwitchToProtocol(ConnectionProtocol cp)
	{
		PhotonNetwork.networkingPeer.TransportProtocol = cp;
	}

	// Token: 0x06003D4E RID: 15694 RVA: 0x001352BC File Offset: 0x001336BC
	public static bool ConnectUsingSettings(string gameVersion, bool retry = false)
	{
		if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			UnityEngine.Debug.LogWarning("ConnectUsingSettings() failed. Can only connect while in state 'Disconnected'. Current state: " + PhotonNetwork.networkingPeer.PeerState);
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings == null)
		{
			UnityEngine.Debug.LogError("Can't connect: Loading settings failed. ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.NotSet)
		{
			UnityEngine.Debug.LogError("You did not select a Hosting Type in your PhotonServerSettings. Please set it up or don't use ConnectUsingSettings().");
			return false;
		}
		if (PhotonNetwork.logLevel == PhotonLogLevel.ErrorsOnly)
		{
			PhotonNetwork.logLevel = PhotonNetwork.PhotonServerSettings.PunLogging;
		}
		if (PhotonNetwork.networkingPeer.DebugOut == DebugLevel.ERROR)
		{
			PhotonNetwork.networkingPeer.DebugOut = PhotonNetwork.PhotonServerSettings.NetworkLogging;
		}
		PhotonNetwork.SwitchToProtocol(PhotonNetwork.PhotonServerSettings.Protocol);
		PhotonNetwork.networkingPeer.SetApp(PhotonNetwork.PhotonServerSettings.AppID, gameVersion);
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
		{
			PhotonNetwork.offlineMode = true;
			return true;
		}
		if (PhotonNetwork.offlineMode)
		{
			UnityEngine.Debug.LogWarning("ConnectUsingSettings() disabled the offline mode. No longer offline.");
		}
		PhotonNetwork.offlineMode = false;
		PhotonNetwork.isMessageQueueRunning = true;
		PhotonNetwork.networkingPeer.IsInitialConnect = true;
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.SelfHosted)
		{
			PhotonNetwork.networkingPeer.IsUsingNameServer = false;
			PhotonNetwork.networkingPeer.MasterServerAddress = ((PhotonNetwork.PhotonServerSettings.ServerPort != 0) ? (PhotonNetwork.PhotonServerSettings.ServerAddress + ":" + PhotonNetwork.PhotonServerSettings.ServerPort) : PhotonNetwork.PhotonServerSettings.ServerAddress);
			return PhotonNetwork.networkingPeer.Connect(PhotonNetwork.networkingPeer.MasterServerAddress, ServerConnection.MasterServer);
		}
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.BestRegion)
		{
			return PhotonNetwork.ConnectToBestCloudServer(gameVersion);
		}
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.RoundRobin)
		{
			if (PhotonNetwork.nextRegionToAttempt == null)
			{
				PhotonNetwork.nextRegionToAttempt = new CloudRegionCode?(PhotonNetwork.PhotonServerSettings.PreferredRegion);
			}
			else if (retry)
			{
				int num = (int)PhotonNetwork.nextRegionToAttempt.Value;
				Array values = Enum.GetValues(typeof(CloudRegionCode));
				num = (num + 1) % values.Length;
				PhotonNetwork.nextRegionToAttempt = new CloudRegionCode?((CloudRegionCode)values.GetValue(num));
			}
			UnityEngine.Debug.Log("<color=red>Attempting region " + PhotonNetwork.nextRegionToAttempt.Value.ToString() + "</color>");
			return PhotonNetwork.networkingPeer.ConnectToRegionMaster(PhotonNetwork.nextRegionToAttempt.Value);
		}
		return PhotonNetwork.networkingPeer.ConnectToRegionMaster(PhotonNetwork.PhotonServerSettings.PreferredRegion);
	}

	// Token: 0x06003D4F RID: 15695 RVA: 0x00135538 File Offset: 0x00133938
	public static bool ConnectToMaster(string masterServerAddress, int port, string appID, string gameVersion)
	{
		if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			UnityEngine.Debug.LogWarning("ConnectToMaster() failed. Can only connect while in state 'Disconnected'. Current state: " + PhotonNetwork.networkingPeer.PeerState);
			return false;
		}
		if (PhotonNetwork.offlineMode)
		{
			PhotonNetwork.offlineMode = false;
			UnityEngine.Debug.LogWarning("ConnectToMaster() disabled the offline mode. No longer offline.");
		}
		if (!PhotonNetwork.isMessageQueueRunning)
		{
			PhotonNetwork.isMessageQueueRunning = true;
			UnityEngine.Debug.LogWarning("ConnectToMaster() enabled isMessageQueueRunning. Needs to be able to dispatch incoming messages.");
		}
		PhotonNetwork.networkingPeer.SetApp(appID, gameVersion);
		PhotonNetwork.networkingPeer.IsUsingNameServer = false;
		PhotonNetwork.networkingPeer.IsInitialConnect = true;
		PhotonNetwork.networkingPeer.MasterServerAddress = ((port != 0) ? (masterServerAddress + ":" + port) : masterServerAddress);
		return PhotonNetwork.networkingPeer.Connect(PhotonNetwork.networkingPeer.MasterServerAddress, ServerConnection.MasterServer);
	}

	// Token: 0x06003D50 RID: 15696 RVA: 0x00135608 File Offset: 0x00133A08
	public static bool Reconnect()
	{
		if (string.IsNullOrEmpty(PhotonNetwork.networkingPeer.MasterServerAddress))
		{
			UnityEngine.Debug.LogWarning("Reconnect() failed. It seems the client wasn't connected before?! Current state: " + PhotonNetwork.networkingPeer.PeerState);
			return false;
		}
		if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			UnityEngine.Debug.LogWarning("Reconnect() failed. Can only connect while in state 'Disconnected'. Current state: " + PhotonNetwork.networkingPeer.PeerState);
			return false;
		}
		if (PhotonNetwork.offlineMode)
		{
			PhotonNetwork.offlineMode = false;
			UnityEngine.Debug.LogWarning("Reconnect() disabled the offline mode. No longer offline.");
		}
		if (!PhotonNetwork.isMessageQueueRunning)
		{
			PhotonNetwork.isMessageQueueRunning = true;
			UnityEngine.Debug.LogWarning("Reconnect() enabled isMessageQueueRunning. Needs to be able to dispatch incoming messages.");
		}
		PhotonNetwork.networkingPeer.IsUsingNameServer = false;
		PhotonNetwork.networkingPeer.IsInitialConnect = false;
		return PhotonNetwork.networkingPeer.ReconnectToMaster();
	}

	// Token: 0x06003D51 RID: 15697 RVA: 0x001356CC File Offset: 0x00133ACC
	public static bool ReconnectAndRejoin()
	{
		if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			UnityEngine.Debug.LogWarning("ReconnectAndRejoin() failed. Can only connect while in state 'Disconnected'. Current state: " + PhotonNetwork.networkingPeer.PeerState);
			return false;
		}
		if (PhotonNetwork.offlineMode)
		{
			PhotonNetwork.offlineMode = false;
			UnityEngine.Debug.LogWarning("ReconnectAndRejoin() disabled the offline mode. No longer offline.");
		}
		if (string.IsNullOrEmpty(PhotonNetwork.networkingPeer.GameServerAddress))
		{
			UnityEngine.Debug.LogWarning("ReconnectAndRejoin() failed. It seems the client wasn't connected to a game server before (no address).");
			return false;
		}
		if (PhotonNetwork.networkingPeer.enterRoomParamsCache == null)
		{
			UnityEngine.Debug.LogWarning("ReconnectAndRejoin() failed. It seems the client doesn't have any previous room to re-join.");
			return false;
		}
		if (!PhotonNetwork.isMessageQueueRunning)
		{
			PhotonNetwork.isMessageQueueRunning = true;
			UnityEngine.Debug.LogWarning("ReconnectAndRejoin() enabled isMessageQueueRunning. Needs to be able to dispatch incoming messages.");
		}
		PhotonNetwork.networkingPeer.IsUsingNameServer = false;
		PhotonNetwork.networkingPeer.IsInitialConnect = false;
		return PhotonNetwork.networkingPeer.ReconnectAndRejoin();
	}

	// Token: 0x06003D52 RID: 15698 RVA: 0x00135798 File Offset: 0x00133B98
	public static bool ConnectToBestCloudServer(string gameVersion)
	{
		if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			UnityEngine.Debug.LogWarning("ConnectToBestCloudServer() failed. Can only connect while in state 'Disconnected'. Current state: " + PhotonNetwork.networkingPeer.PeerState);
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings == null)
		{
			UnityEngine.Debug.LogError("Can't connect: Loading settings failed. ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
		{
			return PhotonNetwork.ConnectUsingSettings(gameVersion, false);
		}
		PhotonNetwork.networkingPeer.IsInitialConnect = true;
		PhotonNetwork.networkingPeer.SetApp(PhotonNetwork.PhotonServerSettings.AppID, gameVersion);
		CloudRegionCode bestRegionCodeInPreferences = PhotonHandler.BestRegionCodeInPreferences;
		if (bestRegionCodeInPreferences != CloudRegionCode.none)
		{
			UnityEngine.Debug.Log("Best region found in PlayerPrefs. Connecting to: " + bestRegionCodeInPreferences);
			return PhotonNetwork.networkingPeer.ConnectToRegionMaster(bestRegionCodeInPreferences);
		}
		return PhotonNetwork.networkingPeer.ConnectToNameServer();
	}

	// Token: 0x06003D53 RID: 15699 RVA: 0x00135864 File Offset: 0x00133C64
	public static bool ConnectToRegion(CloudRegionCode region, string gameVersion)
	{
		if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			UnityEngine.Debug.LogWarning("ConnectToRegion() failed. Can only connect while in state 'Disconnected'. Current state: " + PhotonNetwork.networkingPeer.PeerState);
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings == null)
		{
			UnityEngine.Debug.LogError("Can't connect: ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
		{
			return PhotonNetwork.ConnectUsingSettings(gameVersion, false);
		}
		PhotonNetwork.networkingPeer.IsInitialConnect = true;
		PhotonNetwork.networkingPeer.SetApp(PhotonNetwork.PhotonServerSettings.AppID, gameVersion);
		if (region != CloudRegionCode.none)
		{
			UnityEngine.Debug.Log("ConnectToRegion: " + region);
			return PhotonNetwork.networkingPeer.ConnectToRegionMaster(region);
		}
		return false;
	}

	// Token: 0x06003D54 RID: 15700 RVA: 0x0013591D File Offset: 0x00133D1D
	public static void OverrideBestCloudServer(CloudRegionCode region)
	{
		PhotonHandler.BestRegionCodeInPreferences = region;
	}

	// Token: 0x06003D55 RID: 15701 RVA: 0x00135925 File Offset: 0x00133D25
	public static void RefreshCloudServerRating()
	{
		throw new NotImplementedException("not available at the moment");
	}

	// Token: 0x06003D56 RID: 15702 RVA: 0x00135931 File Offset: 0x00133D31
	public static void NetworkStatisticsReset()
	{
		PhotonNetwork.networkingPeer.TrafficStatsReset();
	}

	// Token: 0x06003D57 RID: 15703 RVA: 0x0013593D File Offset: 0x00133D3D
	public static string NetworkStatisticsToString()
	{
		if (PhotonNetwork.networkingPeer == null || PhotonNetwork.offlineMode)
		{
			return "Offline or in OfflineMode. No VitalStats available.";
		}
		return PhotonNetwork.networkingPeer.VitalStatsToString(false);
	}

	// Token: 0x06003D58 RID: 15704 RVA: 0x00135964 File Offset: 0x00133D64
	[Obsolete("Used for compatibility with Unity networking only. Encryption is automatically initialized while connecting.")]
	public static void InitializeSecurity()
	{
	}

	// Token: 0x06003D59 RID: 15705 RVA: 0x00135966 File Offset: 0x00133D66
	private static bool VerifyCanUseNetwork()
	{
		if (PhotonNetwork.connected)
		{
			return true;
		}
		UnityEngine.Debug.LogError("Cannot send messages when not connected. Either connect to Photon OR use offline mode!");
		return false;
	}

	// Token: 0x06003D5A RID: 15706 RVA: 0x00135980 File Offset: 0x00133D80
	public static void Disconnect()
	{
		if (PhotonNetwork.offlineMode)
		{
			PhotonNetwork.offlineMode = false;
			PhotonNetwork.offlineModeRoom = null;
			PhotonNetwork.networkingPeer.State = ClientState.Disconnecting;
			PhotonNetwork.networkingPeer.OnStatusChanged(StatusCode.Disconnect);
			return;
		}
		if (PhotonNetwork.networkingPeer == null)
		{
			return;
		}
		PhotonNetwork.networkingPeer.Disconnect();
	}

	// Token: 0x06003D5B RID: 15707 RVA: 0x001359D4 File Offset: 0x00133DD4
	public static bool FindFriends(string[] friendsToFind)
	{
		return PhotonNetwork.networkingPeer != null && !PhotonNetwork.isOfflineMode && PhotonNetwork.networkingPeer.OpFindFriends(friendsToFind);
	}

	// Token: 0x06003D5C RID: 15708 RVA: 0x001359F7 File Offset: 0x00133DF7
	public static bool CreateRoom(string roomName)
	{
		return PhotonNetwork.CreateRoom(roomName, null, null, null);
	}

	// Token: 0x06003D5D RID: 15709 RVA: 0x00135A02 File Offset: 0x00133E02
	public static bool CreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby)
	{
		return PhotonNetwork.CreateRoom(roomName, roomOptions, typedLobby, null);
	}

	// Token: 0x06003D5E RID: 15710 RVA: 0x00135A10 File Offset: 0x00133E10
	public static bool CreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby, string[] expectedUsers)
	{
		if (PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.offlineModeRoom != null)
			{
				UnityEngine.Debug.LogError("CreateRoom failed. In offline mode you still have to leave a room to enter another.");
				return false;
			}
			PhotonNetwork.EnterOfflineRoom(roomName, roomOptions, true);
			return true;
		}
		else
		{
			if (PhotonNetwork.networkingPeer.Server != ServerConnection.MasterServer || !PhotonNetwork.connectedAndReady)
			{
				UnityEngine.Debug.LogError("CreateRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
				return false;
			}
			typedLobby = (typedLobby ?? ((!PhotonNetwork.networkingPeer.insideLobby) ? null : PhotonNetwork.networkingPeer.lobby));
			EnterRoomParams enterRoomParams = new EnterRoomParams();
			enterRoomParams.RoomName = roomName;
			enterRoomParams.RoomOptions = roomOptions;
			enterRoomParams.Lobby = typedLobby;
			enterRoomParams.ExpectedUsers = expectedUsers;
			return PhotonNetwork.networkingPeer.OpCreateGame(enterRoomParams);
		}
	}

	// Token: 0x06003D5F RID: 15711 RVA: 0x00135AC2 File Offset: 0x00133EC2
	public static bool JoinRoom(string roomName)
	{
		return PhotonNetwork.JoinRoom(roomName, null);
	}

	// Token: 0x06003D60 RID: 15712 RVA: 0x00135ACC File Offset: 0x00133ECC
	public static bool JoinRoom(string roomName, string[] expectedUsers)
	{
		if (PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.offlineModeRoom != null)
			{
				UnityEngine.Debug.LogError("JoinRoom failed. In offline mode you still have to leave a room to enter another.");
				return false;
			}
			PhotonNetwork.EnterOfflineRoom(roomName, null, true);
			return true;
		}
		else
		{
			if (PhotonNetwork.networkingPeer.Server != ServerConnection.MasterServer || !PhotonNetwork.connectedAndReady)
			{
				UnityEngine.Debug.LogError("JoinRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
				return false;
			}
			if (string.IsNullOrEmpty(roomName))
			{
				UnityEngine.Debug.LogError("JoinRoom failed. A roomname is required. If you don't know one, how will you join?");
				return false;
			}
			EnterRoomParams enterRoomParams = new EnterRoomParams();
			enterRoomParams.RoomName = roomName;
			enterRoomParams.ExpectedUsers = expectedUsers;
			return PhotonNetwork.networkingPeer.OpJoinRoom(enterRoomParams);
		}
	}

	// Token: 0x06003D61 RID: 15713 RVA: 0x00135B5E File Offset: 0x00133F5E
	public static bool JoinOrCreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby)
	{
		return PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby, null);
	}

	// Token: 0x06003D62 RID: 15714 RVA: 0x00135B6C File Offset: 0x00133F6C
	public static bool JoinOrCreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby, string[] expectedUsers)
	{
		if (PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.offlineModeRoom != null)
			{
				UnityEngine.Debug.LogError("JoinOrCreateRoom failed. In offline mode you still have to leave a room to enter another.");
				return false;
			}
			PhotonNetwork.EnterOfflineRoom(roomName, roomOptions, true);
			return true;
		}
		else
		{
			if (PhotonNetwork.networkingPeer.Server != ServerConnection.MasterServer || !PhotonNetwork.connectedAndReady)
			{
				UnityEngine.Debug.LogError("JoinOrCreateRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
				return false;
			}
			if (string.IsNullOrEmpty(roomName))
			{
				UnityEngine.Debug.LogError("JoinOrCreateRoom failed. A roomname is required. If you don't know one, how will you join?");
				return false;
			}
			typedLobby = (typedLobby ?? ((!PhotonNetwork.networkingPeer.insideLobby) ? null : PhotonNetwork.networkingPeer.lobby));
			EnterRoomParams enterRoomParams = new EnterRoomParams();
			enterRoomParams.RoomName = roomName;
			enterRoomParams.RoomOptions = roomOptions;
			enterRoomParams.Lobby = typedLobby;
			enterRoomParams.CreateIfNotExists = true;
			enterRoomParams.PlayerProperties = PhotonNetwork.player.CustomProperties;
			enterRoomParams.ExpectedUsers = expectedUsers;
			return PhotonNetwork.networkingPeer.OpJoinRoom(enterRoomParams);
		}
	}

	// Token: 0x06003D63 RID: 15715 RVA: 0x00135C4C File Offset: 0x0013404C
	public static bool JoinRandomRoom()
	{
		return PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, null, null, null);
	}

	// Token: 0x06003D64 RID: 15716 RVA: 0x00135C59 File Offset: 0x00134059
	public static bool JoinRandomRoom(Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers)
	{
		return PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, expectedMaxPlayers, MatchmakingMode.FillRoom, null, null, null);
	}

	// Token: 0x06003D65 RID: 15717 RVA: 0x00135C68 File Offset: 0x00134068
	public static bool JoinRandomRoom(Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers, MatchmakingMode matchingType, TypedLobby typedLobby, string sqlLobbyFilter, string[] expectedUsers = null)
	{
		if (PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.offlineModeRoom != null)
			{
				UnityEngine.Debug.LogError("JoinRandomRoom failed. In offline mode you still have to leave a room to enter another.");
				return false;
			}
			PhotonNetwork.EnterOfflineRoom("offline room", null, true);
			return true;
		}
		else
		{
			if (PhotonNetwork.networkingPeer.Server != ServerConnection.MasterServer || !PhotonNetwork.connectedAndReady)
			{
				UnityEngine.Debug.LogError("JoinRandomRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
				return false;
			}
			typedLobby = (typedLobby ?? ((!PhotonNetwork.networkingPeer.insideLobby) ? null : PhotonNetwork.networkingPeer.lobby));
			OpJoinRandomRoomParams opJoinRandomRoomParams = new OpJoinRandomRoomParams();
			opJoinRandomRoomParams.ExpectedCustomRoomProperties = expectedCustomRoomProperties;
			opJoinRandomRoomParams.ExpectedMaxPlayers = expectedMaxPlayers;
			opJoinRandomRoomParams.MatchingType = matchingType;
			opJoinRandomRoomParams.TypedLobby = typedLobby;
			opJoinRandomRoomParams.SqlLobbyFilter = sqlLobbyFilter;
			opJoinRandomRoomParams.ExpectedUsers = expectedUsers;
			return PhotonNetwork.networkingPeer.OpJoinRandomRoom(opJoinRandomRoomParams);
		}
	}

	// Token: 0x06003D66 RID: 15718 RVA: 0x00135D30 File Offset: 0x00134130
	public static bool ReJoinRoom(string roomName)
	{
		if (PhotonNetwork.offlineMode)
		{
			UnityEngine.Debug.LogError("ReJoinRoom failed due to offline mode.");
			return false;
		}
		if (PhotonNetwork.networkingPeer.Server != ServerConnection.MasterServer || !PhotonNetwork.connectedAndReady)
		{
			UnityEngine.Debug.LogError("ReJoinRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
			return false;
		}
		if (string.IsNullOrEmpty(roomName))
		{
			UnityEngine.Debug.LogError("ReJoinRoom failed. A roomname is required. If you don't know one, how will you join?");
			return false;
		}
		EnterRoomParams enterRoomParams = new EnterRoomParams();
		enterRoomParams.RoomName = roomName;
		enterRoomParams.RejoinOnly = true;
		enterRoomParams.PlayerProperties = PhotonNetwork.player.CustomProperties;
		return PhotonNetwork.networkingPeer.OpJoinRoom(enterRoomParams);
	}

	// Token: 0x06003D67 RID: 15719 RVA: 0x00135DC0 File Offset: 0x001341C0
	private static void EnterOfflineRoom(string roomName, RoomOptions roomOptions, bool createdRoom)
	{
		PhotonNetwork.offlineModeRoom = new Room(roomName, roomOptions);
		PhotonNetwork.networkingPeer.ChangeLocalID(1);
		PhotonNetwork.offlineModeRoom.MasterClientId = 1;
		if (createdRoom)
		{
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom, new object[0]);
		}
		NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom, new object[0]);
	}

	// Token: 0x06003D68 RID: 15720 RVA: 0x00135E0E File Offset: 0x0013420E
	public static bool JoinLobby()
	{
		return PhotonNetwork.JoinLobby(null);
	}

	// Token: 0x06003D69 RID: 15721 RVA: 0x00135E18 File Offset: 0x00134218
	public static bool JoinLobby(TypedLobby typedLobby)
	{
		if (PhotonNetwork.connected && PhotonNetwork.Server == ServerConnection.MasterServer)
		{
			if (typedLobby == null)
			{
				typedLobby = TypedLobby.Default;
			}
			bool flag = PhotonNetwork.networkingPeer.OpJoinLobby(typedLobby);
			if (flag)
			{
				PhotonNetwork.networkingPeer.lobby = typedLobby;
			}
			return flag;
		}
		return false;
	}

	// Token: 0x06003D6A RID: 15722 RVA: 0x00135E66 File Offset: 0x00134266
	public static bool LeaveLobby()
	{
		return PhotonNetwork.connected && PhotonNetwork.Server == ServerConnection.MasterServer && PhotonNetwork.networkingPeer.OpLeaveLobby();
	}

	// Token: 0x06003D6B RID: 15723 RVA: 0x00135E88 File Offset: 0x00134288
	public static bool LeaveRoom()
	{
		if (PhotonNetwork.offlineMode)
		{
			PhotonNetwork.offlineModeRoom = null;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLeftRoom, new object[0]);
			return true;
		}
		if (PhotonNetwork.room == null)
		{
			UnityEngine.Debug.LogWarning("PhotonNetwork.room is null. You don't have to call LeaveRoom() when you're not in one. State: " + PhotonNetwork.connectionStateDetailed);
		}
		return PhotonNetwork.networkingPeer.OpLeave();
	}

	// Token: 0x06003D6C RID: 15724 RVA: 0x00135EE5 File Offset: 0x001342E5
	public static bool GetCustomRoomList(TypedLobby typedLobby, string sqlLobbyFilter)
	{
		return PhotonNetwork.networkingPeer.OpGetGameList(typedLobby, sqlLobbyFilter);
	}

	// Token: 0x06003D6D RID: 15725 RVA: 0x00135EF3 File Offset: 0x001342F3
	public static RoomInfo[] GetRoomList()
	{
		if (PhotonNetwork.offlineMode || PhotonNetwork.networkingPeer == null)
		{
			return new RoomInfo[0];
		}
		return PhotonNetwork.networkingPeer.mGameListCopy;
	}

	// Token: 0x06003D6E RID: 15726 RVA: 0x00135F1C File Offset: 0x0013431C
	public static void SetPlayerCustomProperties(Hashtable customProperties)
	{
		if (customProperties == null)
		{
			customProperties = new Hashtable();
			foreach (object obj in PhotonNetwork.player.CustomProperties.Keys)
			{
				customProperties[(string)obj] = null;
			}
		}
		if (PhotonNetwork.room != null && PhotonNetwork.room.IsLocalClientInside)
		{
			PhotonNetwork.player.SetCustomProperties(customProperties, null, false);
		}
		else
		{
			PhotonNetwork.player.InternalCacheProperties(customProperties);
		}
	}

	// Token: 0x06003D6F RID: 15727 RVA: 0x00135FCC File Offset: 0x001343CC
	public static void RemovePlayerCustomProperties(string[] customPropertiesToDelete)
	{
		if (customPropertiesToDelete == null || customPropertiesToDelete.Length == 0 || PhotonNetwork.player.CustomProperties == null)
		{
			PhotonNetwork.player.CustomProperties = new Hashtable();
			return;
		}
		foreach (string key in customPropertiesToDelete)
		{
			if (PhotonNetwork.player.CustomProperties.ContainsKey(key))
			{
				PhotonNetwork.player.CustomProperties.Remove(key);
			}
		}
	}

	// Token: 0x06003D70 RID: 15728 RVA: 0x00136044 File Offset: 0x00134444
	public static bool RaiseEvent(byte eventCode, object eventContent, bool sendReliable, RaiseEventOptions options)
	{
		if (!PhotonNetwork.inRoom || eventCode >= 200)
		{
			UnityEngine.Debug.LogWarning("RaiseEvent() failed. Your event is not being sent! Check if your are in a Room and the eventCode must be less than 200 (0..199).");
			return false;
		}
		return PhotonNetwork.networkingPeer.OpRaiseEvent(eventCode, eventContent, sendReliable, options);
	}

	// Token: 0x06003D71 RID: 15729 RVA: 0x00136078 File Offset: 0x00134478
	public static int AllocateViewID()
	{
		int num = PhotonNetwork.AllocateViewID(PhotonNetwork.player.ID);
		PhotonNetwork.manuallyAllocatedViewIds.Add(num);
		return num;
	}

	// Token: 0x06003D72 RID: 15730 RVA: 0x001360A4 File Offset: 0x001344A4
	public static int AllocateSceneViewID()
	{
		if (!PhotonNetwork.isMasterClient)
		{
			UnityEngine.Debug.LogError("Only the Master Client can AllocateSceneViewID(). Check PhotonNetwork.isMasterClient!");
			return -1;
		}
		int num = PhotonNetwork.AllocateViewID(0);
		PhotonNetwork.manuallyAllocatedViewIds.Add(num);
		return num;
	}

	// Token: 0x06003D73 RID: 15731 RVA: 0x001360DC File Offset: 0x001344DC
	private static int AllocateViewID(int ownerId)
	{
		if (ownerId == 0)
		{
			int num = PhotonNetwork.lastUsedViewSubIdStatic;
			int num2 = ownerId * PhotonNetwork.MAX_VIEW_IDS;
			for (int i = 1; i < PhotonNetwork.MAX_VIEW_IDS; i++)
			{
				num = (num + 1) % PhotonNetwork.MAX_VIEW_IDS;
				if (num != 0)
				{
					int num3 = num + num2;
					if (!PhotonNetwork.networkingPeer.photonViewList.ContainsKey(num3))
					{
						PhotonNetwork.lastUsedViewSubIdStatic = num;
						return num3;
					}
				}
			}
			throw new Exception(string.Format("AllocateViewID() failed. Room (user {0}) is out of 'scene' viewIDs. It seems all available are in use.", ownerId));
		}
		int num4 = PhotonNetwork.lastUsedViewSubId;
		int num5 = ownerId * PhotonNetwork.MAX_VIEW_IDS;
		for (int j = 1; j < PhotonNetwork.MAX_VIEW_IDS; j++)
		{
			num4 = (num4 + 1) % PhotonNetwork.MAX_VIEW_IDS;
			if (num4 != 0)
			{
				int num6 = num4 + num5;
				if (!PhotonNetwork.networkingPeer.photonViewList.ContainsKey(num6) && !PhotonNetwork.manuallyAllocatedViewIds.Contains(num6))
				{
					PhotonNetwork.lastUsedViewSubId = num4;
					return num6;
				}
			}
		}
		throw new Exception(string.Format("AllocateViewID() failed. User {0} is out of subIds, as all viewIDs are used.", ownerId));
	}

	// Token: 0x06003D74 RID: 15732 RVA: 0x001361F0 File Offset: 0x001345F0
	private static int[] AllocateSceneViewIDs(int countOfNewViews)
	{
		int[] array = new int[countOfNewViews];
		for (int i = 0; i < countOfNewViews; i++)
		{
			array[i] = PhotonNetwork.AllocateViewID(0);
		}
		return array;
	}

	// Token: 0x06003D75 RID: 15733 RVA: 0x00136220 File Offset: 0x00134620
	public static void UnAllocateViewID(int viewID)
	{
		PhotonNetwork.manuallyAllocatedViewIds.Remove(viewID);
		if (PhotonNetwork.networkingPeer.photonViewList.ContainsKey(viewID))
		{
			UnityEngine.Debug.LogWarning(string.Format("UnAllocateViewID() should be called after the PhotonView was destroyed (GameObject.Destroy()). ViewID: {0} still found in: {1}", viewID, PhotonNetwork.networkingPeer.photonViewList[viewID]));
		}
	}

	// Token: 0x06003D76 RID: 15734 RVA: 0x00136273 File Offset: 0x00134673
	public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, int group)
	{
		return PhotonNetwork.Instantiate(prefabName, position, rotation, group, null);
	}

	// Token: 0x06003D77 RID: 15735 RVA: 0x00136280 File Offset: 0x00134680
	public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, int group, object[] data)
	{
		if (!PhotonNetwork.connected || (PhotonNetwork.InstantiateInRoomOnly && !PhotonNetwork.inRoom))
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Failed to Instantiate prefab: ",
				prefabName,
				". Client should be in a room. Current connectionStateDetailed: ",
				PhotonNetwork.connectionStateDetailed
			}));
			return null;
		}
		GameObject gameObject;
		if (!PhotonNetwork.UsePrefabCache || !PhotonNetwork.PrefabCache.TryGetValue(prefabName, out gameObject))
		{
			gameObject = (GameObject)Resources.Load(prefabName, typeof(GameObject));
			if (PhotonNetwork.UsePrefabCache)
			{
				PhotonNetwork.PrefabCache.Add(prefabName, gameObject);
			}
		}
		if (gameObject == null)
		{
			UnityEngine.Debug.LogError("Failed to Instantiate prefab: " + prefabName + ". Verify the Prefab is in a Resources folder (and not in a subfolder)");
			return null;
		}
		if (gameObject.GetComponent<PhotonView>() == null)
		{
			UnityEngine.Debug.LogError("Failed to Instantiate prefab:" + prefabName + ". Prefab must have a PhotonView component.");
			return null;
		}
		Component[] photonViewsInChildren = gameObject.GetPhotonViewsInChildren();
		int[] array = new int[photonViewsInChildren.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = PhotonNetwork.AllocateViewID(PhotonNetwork.player.ID);
		}
		Hashtable evData = PhotonNetwork.networkingPeer.SendInstantiate(prefabName, position, rotation, group, array, data, false);
		return PhotonNetwork.networkingPeer.DoInstantiate(evData, PhotonNetwork.networkingPeer.LocalPlayer, gameObject);
	}

	// Token: 0x06003D78 RID: 15736 RVA: 0x001363D4 File Offset: 0x001347D4
	public static GameObject InstantiateSceneObject(string prefabName, Vector3 position, Quaternion rotation, int group, object[] data)
	{
		if (!PhotonNetwork.connected || (PhotonNetwork.InstantiateInRoomOnly && !PhotonNetwork.inRoom))
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Failed to InstantiateSceneObject prefab: ",
				prefabName,
				". Client should be in a room. Current connectionStateDetailed: ",
				PhotonNetwork.connectionStateDetailed
			}));
			return null;
		}
		if (!PhotonNetwork.isMasterClient)
		{
			UnityEngine.Debug.LogError("Failed to InstantiateSceneObject prefab: " + prefabName + ". Client is not the MasterClient in this room.");
			return null;
		}
		GameObject gameObject;
		if (!PhotonNetwork.UsePrefabCache || !PhotonNetwork.PrefabCache.TryGetValue(prefabName, out gameObject))
		{
			gameObject = (GameObject)Resources.Load(prefabName, typeof(GameObject));
			if (PhotonNetwork.UsePrefabCache)
			{
				PhotonNetwork.PrefabCache.Add(prefabName, gameObject);
			}
		}
		if (gameObject == null)
		{
			UnityEngine.Debug.LogError("Failed to InstantiateSceneObject prefab: " + prefabName + ". Verify the Prefab is in a Resources folder (and not in a subfolder)");
			return null;
		}
		if (gameObject.GetComponent<PhotonView>() == null)
		{
			UnityEngine.Debug.LogError("Failed to InstantiateSceneObject prefab:" + prefabName + ". Prefab must have a PhotonView component.");
			return null;
		}
		Component[] photonViewsInChildren = gameObject.GetPhotonViewsInChildren();
		int[] array = PhotonNetwork.AllocateSceneViewIDs(photonViewsInChildren.Length);
		if (array == null)
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Failed to InstantiateSceneObject prefab: ",
				prefabName,
				". No ViewIDs are free to use. Max is: ",
				PhotonNetwork.MAX_VIEW_IDS
			}));
			return null;
		}
		Hashtable evData = PhotonNetwork.networkingPeer.SendInstantiate(prefabName, position, rotation, group, array, data, true);
		return PhotonNetwork.networkingPeer.DoInstantiate(evData, PhotonNetwork.networkingPeer.LocalPlayer, gameObject);
	}

	// Token: 0x06003D79 RID: 15737 RVA: 0x00136558 File Offset: 0x00134958
	public static int GetPing()
	{
		return PhotonNetwork.networkingPeer.RoundTripTime;
	}

	// Token: 0x06003D7A RID: 15738 RVA: 0x00136564 File Offset: 0x00134964
	public static void FetchServerTimestamp()
	{
		if (PhotonNetwork.networkingPeer != null)
		{
			PhotonNetwork.networkingPeer.FetchServerTimestamp();
		}
	}

	// Token: 0x06003D7B RID: 15739 RVA: 0x0013657A File Offset: 0x0013497A
	public static void SendOutgoingCommands()
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		while (PhotonNetwork.networkingPeer.SendOutgoingCommands())
		{
		}
	}

	// Token: 0x06003D7C RID: 15740 RVA: 0x0013659C File Offset: 0x0013499C
	public static bool CloseConnection(PhotonPlayer kickPlayer)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return false;
		}
		if (!PhotonNetwork.player.IsMasterClient)
		{
			UnityEngine.Debug.LogError("CloseConnection: Only the masterclient can kick another player.");
			return false;
		}
		if (kickPlayer == null)
		{
			UnityEngine.Debug.LogError("CloseConnection: No such player connected!");
			return false;
		}
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			TargetActors = new int[]
			{
				kickPlayer.ID
			}
		};
		return PhotonNetwork.networkingPeer.OpRaiseEvent(203, null, true, raiseEventOptions);
	}

	// Token: 0x06003D7D RID: 15741 RVA: 0x00136614 File Offset: 0x00134A14
	public static bool SetMasterClient(PhotonPlayer masterClientPlayer)
	{
		if (!PhotonNetwork.inRoom || !PhotonNetwork.VerifyCanUseNetwork() || PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.logLevel == PhotonLogLevel.Informational)
			{
				UnityEngine.Debug.Log("Can not SetMasterClient(). Not in room or in offlineMode.");
			}
			return false;
		}
		if (PhotonNetwork.room.serverSideMasterClient)
		{
			Hashtable gameProperties = new Hashtable
			{
				{
					248,
					masterClientPlayer.ID
				}
			};
			Hashtable expectedProperties = new Hashtable
			{
				{
					248,
					PhotonNetwork.networkingPeer.mMasterClientId
				}
			};
			return PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(gameProperties, expectedProperties, false);
		}
		return PhotonNetwork.isMasterClient && PhotonNetwork.networkingPeer.SetMasterClient(masterClientPlayer.ID, true);
	}

	// Token: 0x06003D7E RID: 15742 RVA: 0x001366DA File Offset: 0x00134ADA
	public static void Destroy(PhotonView targetView)
	{
		if (targetView != null)
		{
			PhotonNetwork.networkingPeer.RemoveInstantiatedGO(targetView.gameObject, !PhotonNetwork.inRoom);
		}
		else
		{
			UnityEngine.Debug.LogError("Destroy(targetPhotonView) failed, cause targetPhotonView is null.");
		}
	}

	// Token: 0x06003D7F RID: 15743 RVA: 0x0013670F File Offset: 0x00134B0F
	public static void Destroy(GameObject targetGo)
	{
		PhotonNetwork.networkingPeer.RemoveInstantiatedGO(targetGo, !PhotonNetwork.inRoom);
	}

	// Token: 0x06003D80 RID: 15744 RVA: 0x00136724 File Offset: 0x00134B24
	public static void DestroyPlayerObjects(PhotonPlayer targetPlayer)
	{
		if (PhotonNetwork.player == null)
		{
			UnityEngine.Debug.LogError("DestroyPlayerObjects() failed, cause parameter 'targetPlayer' was null.");
		}
		PhotonNetwork.DestroyPlayerObjects(targetPlayer.ID);
	}

	// Token: 0x06003D81 RID: 15745 RVA: 0x00136748 File Offset: 0x00134B48
	public static void DestroyPlayerObjects(int targetPlayerId)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		if (PhotonNetwork.player.IsMasterClient || targetPlayerId == PhotonNetwork.player.ID)
		{
			PhotonNetwork.networkingPeer.DestroyPlayerObjects(targetPlayerId, false);
		}
		else
		{
			UnityEngine.Debug.LogError("DestroyPlayerObjects() failed, cause players can only destroy their own GameObjects. A Master Client can destroy anyone's. This is master: " + PhotonNetwork.isMasterClient);
		}
	}

	// Token: 0x06003D82 RID: 15746 RVA: 0x001367A9 File Offset: 0x00134BA9
	public static void DestroyAll()
	{
		if (PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.networkingPeer.DestroyAll(false);
		}
		else
		{
			UnityEngine.Debug.LogError("Couldn't call DestroyAll() as only the master client is allowed to call this.");
		}
	}

	// Token: 0x06003D83 RID: 15747 RVA: 0x001367CF File Offset: 0x00134BCF
	public static void RemoveRPCs(PhotonPlayer targetPlayer)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		if (!targetPlayer.IsLocal && !PhotonNetwork.isMasterClient)
		{
			UnityEngine.Debug.LogError("Error; Only the MasterClient can call RemoveRPCs for other players.");
			return;
		}
		PhotonNetwork.networkingPeer.OpCleanRpcBuffer(targetPlayer.ID);
	}

	// Token: 0x06003D84 RID: 15748 RVA: 0x0013680C File Offset: 0x00134C0C
	public static void RemoveRPCs(PhotonView targetPhotonView)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.CleanRpcBufferIfMine(targetPhotonView);
	}

	// Token: 0x06003D85 RID: 15749 RVA: 0x00136824 File Offset: 0x00134C24
	public static void RemoveRPCsInGroup(int targetGroup)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.RemoveRPCsInGroup(targetGroup);
	}

	// Token: 0x06003D86 RID: 15750 RVA: 0x0013683C File Offset: 0x00134C3C
	internal static void RPC(PhotonView view, string methodName, PhotonTargets target, bool encrypt, params object[] parameters)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		if (PhotonNetwork.room == null)
		{
			UnityEngine.Debug.LogWarning("RPCs can only be sent in rooms. Call of \"" + methodName + "\" gets executed locally only, if at all.");
			return;
		}
		if (PhotonNetwork.networkingPeer != null)
		{
			if (PhotonNetwork.room.serverSideMasterClient)
			{
				PhotonNetwork.networkingPeer.RPC(view, methodName, target, null, encrypt, parameters);
			}
			else if (PhotonNetwork.networkingPeer.hasSwitchedMC && target == PhotonTargets.MasterClient)
			{
				PhotonNetwork.networkingPeer.RPC(view, methodName, PhotonTargets.Others, PhotonNetwork.masterClient, encrypt, parameters);
			}
			else
			{
				PhotonNetwork.networkingPeer.RPC(view, methodName, target, null, encrypt, parameters);
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("Could not execute RPC " + methodName + ". Possible scene loading in progress?");
		}
	}

	// Token: 0x06003D87 RID: 15751 RVA: 0x00136900 File Offset: 0x00134D00
	internal static void RPC(PhotonView view, string methodName, PhotonPlayer targetPlayer, bool encrpyt, params object[] parameters)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		if (PhotonNetwork.room == null)
		{
			UnityEngine.Debug.LogWarning("RPCs can only be sent in rooms. Call of \"" + methodName + "\" gets executed locally only, if at all.");
			return;
		}
		if (PhotonNetwork.player == null)
		{
			UnityEngine.Debug.LogError("RPC can't be sent to target PhotonPlayer being null! Did not send \"" + methodName + "\" call.");
		}
		if (PhotonNetwork.networkingPeer != null)
		{
			PhotonNetwork.networkingPeer.RPC(view, methodName, PhotonTargets.Others, targetPlayer, encrpyt, parameters);
		}
		else
		{
			UnityEngine.Debug.LogWarning("Could not execute RPC " + methodName + ". Possible scene loading in progress?");
		}
	}

	// Token: 0x06003D88 RID: 15752 RVA: 0x0013698C File Offset: 0x00134D8C
	public static void CacheSendMonoMessageTargets(Type type)
	{
		if (type == null)
		{
			type = PhotonNetwork.SendMonoMessageTargetType;
		}
		PhotonNetwork.SendMonoMessageTargets = PhotonNetwork.FindGameObjectsWithComponent(type);
	}

	// Token: 0x06003D89 RID: 15753 RVA: 0x001369A8 File Offset: 0x00134DA8
	public static HashSet<GameObject> FindGameObjectsWithComponent(Type type)
	{
		HashSet<GameObject> hashSet = new HashSet<GameObject>();
		Component[] array = (Component[])UnityEngine.Object.FindObjectsOfType(type);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				hashSet.Add(array[i].gameObject);
			}
		}
		return hashSet;
	}

	// Token: 0x06003D8A RID: 15754 RVA: 0x001369F9 File Offset: 0x00134DF9
	[Obsolete("Use SetInterestGroups(byte group, bool enabled) instead.")]
	public static void SetReceivingEnabled(int group, bool enabled)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.SetInterestGroups((byte)group, enabled);
	}

	// Token: 0x06003D8B RID: 15755 RVA: 0x00136A10 File Offset: 0x00134E10
	public static void SetInterestGroups(byte group, bool enabled)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		if (enabled)
		{
			byte[] enableGroups = new byte[]
			{
				group
			};
			PhotonNetwork.networkingPeer.SetInterestGroups(null, enableGroups);
		}
		else
		{
			byte[] disableGroups = new byte[]
			{
				group
			};
			PhotonNetwork.networkingPeer.SetInterestGroups(disableGroups, null);
		}
	}

	// Token: 0x06003D8C RID: 15756 RVA: 0x00136A64 File Offset: 0x00134E64
	[Obsolete("Use SetInterestGroups(byte[] disableGroups, byte[] enableGroups) instead. Mind the parameter order!")]
	public static void SetReceivingEnabled(int[] enableGroups, int[] disableGroups)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		byte[] array = null;
		byte[] array2 = null;
		if (enableGroups != null)
		{
			array2 = new byte[enableGroups.Length];
			Array.Copy(enableGroups, array2, enableGroups.Length);
		}
		if (disableGroups != null)
		{
			array = new byte[disableGroups.Length];
			Array.Copy(disableGroups, array, disableGroups.Length);
		}
		PhotonNetwork.networkingPeer.SetInterestGroups(array, array2);
	}

	// Token: 0x06003D8D RID: 15757 RVA: 0x00136ABE File Offset: 0x00134EBE
	public static void SetInterestGroups(byte[] disableGroups, byte[] enableGroups)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.SetInterestGroups(disableGroups, enableGroups);
	}

	// Token: 0x06003D8E RID: 15758 RVA: 0x00136AD7 File Offset: 0x00134ED7
	[Obsolete("Use SetSendingEnabled(byte group, bool enabled). Interest Groups have a byte-typed ID. Mind the parameter order.")]
	public static void SetSendingEnabled(int group, bool enabled)
	{
		PhotonNetwork.SetSendingEnabled((byte)group, enabled);
	}

	// Token: 0x06003D8F RID: 15759 RVA: 0x00136AE1 File Offset: 0x00134EE1
	public static void SetSendingEnabled(byte group, bool enabled)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.SetSendingEnabled(group, enabled);
	}

	// Token: 0x06003D90 RID: 15760 RVA: 0x00136AFC File Offset: 0x00134EFC
	[Obsolete("Use SetSendingEnabled(byte group, bool enabled). Interest Groups have a byte-typed ID. Mind the parameter order.")]
	public static void SetSendingEnabled(int[] enableGroups, int[] disableGroups)
	{
		byte[] array = null;
		byte[] array2 = null;
		if (enableGroups != null)
		{
			array2 = new byte[enableGroups.Length];
			Array.Copy(enableGroups, array2, enableGroups.Length);
		}
		if (disableGroups != null)
		{
			array = new byte[disableGroups.Length];
			Array.Copy(disableGroups, array, disableGroups.Length);
		}
		PhotonNetwork.SetSendingEnabled(array, array2);
	}

	// Token: 0x06003D91 RID: 15761 RVA: 0x00136B46 File Offset: 0x00134F46
	public static void SetSendingEnabled(byte[] disableGroups, byte[] enableGroups)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.SetSendingEnabled(disableGroups, enableGroups);
	}

	// Token: 0x06003D92 RID: 15762 RVA: 0x00136B5F File Offset: 0x00134F5F
	public static void SetLevelPrefix(short prefix)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.SetLevelPrefix(prefix);
	}

	// Token: 0x06003D93 RID: 15763 RVA: 0x00136B77 File Offset: 0x00134F77
	public static void LoadLevel(int levelNumber)
	{
		PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(levelNumber);
		PhotonNetwork.isMessageQueueRunning = false;
		PhotonNetwork.networkingPeer.loadingLevelAndPausedNetwork = true;
		SceneManager.LoadScene(levelNumber);
	}

	// Token: 0x06003D94 RID: 15764 RVA: 0x00136BA0 File Offset: 0x00134FA0
	public static void LoadLevel(string levelName)
	{
		PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(levelName);
		PhotonNetwork.isMessageQueueRunning = false;
		PhotonNetwork.networkingPeer.loadingLevelAndPausedNetwork = true;
		SceneManager.LoadScene(levelName);
	}

	// Token: 0x06003D95 RID: 15765 RVA: 0x00136BC4 File Offset: 0x00134FC4
	public static bool WebRpc(string name, object parameters)
	{
		return PhotonNetwork.networkingPeer.WebRpc(name, parameters);
	}

	// Token: 0x0400263A RID: 9786
	public const string versionPUN = "1.83";

	// Token: 0x0400263C RID: 9788
	internal static readonly PhotonHandler photonMono;

	// Token: 0x0400263D RID: 9789
	internal static NetworkingPeer networkingPeer;

	// Token: 0x0400263E RID: 9790
	public static readonly int MAX_VIEW_IDS = 1000;

	// Token: 0x0400263F RID: 9791
	internal const string serverSettingsAssetFile = "PhotonServerSettings";

	// Token: 0x04002640 RID: 9792
	internal const string serverSettingsAssetPath = "Assets/Photon Unity Networking/Resources/PhotonServerSettings.asset";

	// Token: 0x04002641 RID: 9793
	public static ServerSettings PhotonServerSettings = (ServerSettings)Resources.Load("PhotonServerSettings", typeof(ServerSettings));

	// Token: 0x04002642 RID: 9794
	public static bool InstantiateInRoomOnly = true;

	// Token: 0x04002643 RID: 9795
	public static PhotonLogLevel logLevel = PhotonLogLevel.ErrorsOnly;

	// Token: 0x04002645 RID: 9797
	public static float precisionForVectorSynchronization = 9.9E-05f;

	// Token: 0x04002646 RID: 9798
	public static float precisionForQuaternionSynchronization = 1f;

	// Token: 0x04002647 RID: 9799
	public static float precisionForFloatSynchronization = 0.01f;

	// Token: 0x04002648 RID: 9800
	public static bool UseRpcMonoBehaviourCache;

	// Token: 0x04002649 RID: 9801
	public static bool UsePrefabCache = true;

	// Token: 0x0400264A RID: 9802
	public static Dictionary<string, GameObject> PrefabCache = new Dictionary<string, GameObject>();

	// Token: 0x0400264B RID: 9803
	public static HashSet<GameObject> SendMonoMessageTargets;

	// Token: 0x0400264C RID: 9804
	public static Type SendMonoMessageTargetType = typeof(MonoBehaviour);

	// Token: 0x0400264D RID: 9805
	public static bool StartRpcsAsCoroutine = true;

	// Token: 0x0400264E RID: 9806
	private static bool isOfflineMode = false;

	// Token: 0x0400264F RID: 9807
	private static Room offlineModeRoom = null;

	// Token: 0x04002650 RID: 9808
	[Obsolete("Used for compatibility with Unity networking only.")]
	public static int maxConnections;

	// Token: 0x04002651 RID: 9809
	private static bool _mAutomaticallySyncScene = false;

	// Token: 0x04002652 RID: 9810
	private static bool m_autoCleanUpPlayerObjects = true;

	// Token: 0x04002653 RID: 9811
	private static int sendInterval = 50;

	// Token: 0x04002654 RID: 9812
	private static int sendIntervalOnSerialize = 100;

	// Token: 0x04002655 RID: 9813
	private static bool m_isMessageQueueRunning = true;

	// Token: 0x04002656 RID: 9814
	private static bool UsePreciseTimer = false;

	// Token: 0x04002657 RID: 9815
	private static Stopwatch startupStopwatch;

	// Token: 0x04002658 RID: 9816
	public static float BackgroundTimeout = 60f;

	// Token: 0x04002659 RID: 9817
	public static PhotonNetwork.EventCallback OnEventCall;

	// Token: 0x0400265A RID: 9818
	internal static int lastUsedViewSubId = 0;

	// Token: 0x0400265B RID: 9819
	internal static int lastUsedViewSubIdStatic = 0;

	// Token: 0x0400265C RID: 9820
	internal static List<int> manuallyAllocatedViewIds = new List<int>();

	// Token: 0x0400265D RID: 9821
	private static CloudRegionCode? nextRegionToAttempt = null;

	// Token: 0x02000760 RID: 1888
	// (Invoke) Token: 0x06003D98 RID: 15768
	public delegate void EventCallback(byte eventCode, object content, int senderId);
}
