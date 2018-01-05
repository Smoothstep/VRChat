using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000752 RID: 1874
internal class NetworkingPeer : LoadBalancingPeer, IPhotonPeerListener
{
	// Token: 0x06003C08 RID: 15368 RVA: 0x0012D66C File Offset: 0x0012BA6C
	public NetworkingPeer(string playername, ConnectionProtocol connectionProtocol) : base(connectionProtocol)
	{
		base.Listener = this;
		base.LimitOfUnreliableCommands = 40;
		this.lobby = TypedLobby.Default;
		this.PlayerName = playername;
		this.LocalPlayer = new PhotonPlayer(true, -1, this.playername);
		this.AddNewPlayer(this.LocalPlayer.ID, this.LocalPlayer);
		this.rpcShortcuts = new Dictionary<string, int>(PhotonNetwork.PhotonServerSettings.RpcList.Count);
		for (int i = 0; i < PhotonNetwork.PhotonServerSettings.RpcList.Count; i++)
		{
			string key = PhotonNetwork.PhotonServerSettings.RpcList[i];
			this.rpcShortcuts[key] = i;
		}
		this.State = ClientState.PeerCreated;
	}

	// Token: 0x1700096D RID: 2413
	// (get) Token: 0x06003C09 RID: 15369 RVA: 0x0012D7E1 File Offset: 0x0012BBE1
	protected internal string AppVersion
	{
		get
		{
			return string.Format("{0}_{1}", PhotonNetwork.gameVersion, "1.83");
		}
	}

	// Token: 0x1700096E RID: 2414
	// (get) Token: 0x06003C0A RID: 15370 RVA: 0x0012D7F7 File Offset: 0x0012BBF7
	// (set) Token: 0x06003C0B RID: 15371 RVA: 0x0012D7FF File Offset: 0x0012BBFF
	public AuthenticationValues AuthValues { get; set; }

	// Token: 0x1700096F RID: 2415
	// (get) Token: 0x06003C0C RID: 15372 RVA: 0x0012D808 File Offset: 0x0012BC08
	private string TokenForInit
	{
		get
		{
			if (this.AuthMode == AuthModeOption.Auth)
			{
				return null;
			}
			return (this.AuthValues == null) ? null : this.AuthValues.Token;
		}
	}

	// Token: 0x17000970 RID: 2416
	// (get) Token: 0x06003C0D RID: 15373 RVA: 0x0012D833 File Offset: 0x0012BC33
	// (set) Token: 0x06003C0E RID: 15374 RVA: 0x0012D83B File Offset: 0x0012BC3B
	public bool IsUsingNameServer { get; protected internal set; }

	// Token: 0x17000971 RID: 2417
	// (get) Token: 0x06003C0F RID: 15375 RVA: 0x0012D844 File Offset: 0x0012BC44
	public string NameServerAddress
	{
		get
		{
			return this.GetNameServerAddress();
		}
	}

	// Token: 0x17000972 RID: 2418
	// (get) Token: 0x06003C10 RID: 15376 RVA: 0x0012D84C File Offset: 0x0012BC4C
	// (set) Token: 0x06003C11 RID: 15377 RVA: 0x0012D854 File Offset: 0x0012BC54
	public string MasterServerAddress { get; protected internal set; }

	// Token: 0x17000973 RID: 2419
	// (get) Token: 0x06003C12 RID: 15378 RVA: 0x0012D85D File Offset: 0x0012BC5D
	// (set) Token: 0x06003C13 RID: 15379 RVA: 0x0012D865 File Offset: 0x0012BC65
	public string GameServerAddress { get; protected internal set; }

	// Token: 0x17000974 RID: 2420
	// (get) Token: 0x06003C14 RID: 15380 RVA: 0x0012D86E File Offset: 0x0012BC6E
	// (set) Token: 0x06003C15 RID: 15381 RVA: 0x0012D876 File Offset: 0x0012BC76
	protected internal ServerConnection Server { get; private set; }

	// Token: 0x17000975 RID: 2421
	// (get) Token: 0x06003C16 RID: 15382 RVA: 0x0012D87F File Offset: 0x0012BC7F
	// (set) Token: 0x06003C17 RID: 15383 RVA: 0x0012D887 File Offset: 0x0012BC87
	public ClientState State { get; internal set; }

	// Token: 0x17000976 RID: 2422
	// (get) Token: 0x06003C18 RID: 15384 RVA: 0x0012D890 File Offset: 0x0012BC90
	// (set) Token: 0x06003C19 RID: 15385 RVA: 0x0012D898 File Offset: 0x0012BC98
	public TypedLobby lobby { get; set; }

	// Token: 0x17000977 RID: 2423
	// (get) Token: 0x06003C1A RID: 15386 RVA: 0x0012D8A1 File Offset: 0x0012BCA1
	private bool requestLobbyStatistics
	{
		get
		{
			return PhotonNetwork.EnableLobbyStatistics && this.Server == ServerConnection.MasterServer;
		}
	}

	// Token: 0x17000978 RID: 2424
	// (get) Token: 0x06003C1B RID: 15387 RVA: 0x0012D8B9 File Offset: 0x0012BCB9
	// (set) Token: 0x06003C1C RID: 15388 RVA: 0x0012D8C4 File Offset: 0x0012BCC4
	public string PlayerName
	{
		get
		{
			return this.playername;
		}
		set
		{
			if (string.IsNullOrEmpty(value) || value.Equals(this.playername))
			{
				return;
			}
			if (this.LocalPlayer != null)
			{
				this.LocalPlayer.NickName = value;
			}
			this.playername = value;
			if (this.CurrentRoom != null)
			{
				this.SendPlayerName();
			}
		}
	}

	// Token: 0x17000979 RID: 2425
	// (get) Token: 0x06003C1D RID: 15389 RVA: 0x0012D91D File Offset: 0x0012BD1D
	// (set) Token: 0x06003C1E RID: 15390 RVA: 0x0012D942 File Offset: 0x0012BD42
	public Room CurrentRoom
	{
		get
		{
			if (this.currentRoom != null && this.currentRoom.IsLocalClientInside)
			{
				return this.currentRoom;
			}
			return null;
		}
		private set
		{
			this.currentRoom = value;
		}
	}

	// Token: 0x1700097A RID: 2426
	// (get) Token: 0x06003C1F RID: 15391 RVA: 0x0012D94B File Offset: 0x0012BD4B
	// (set) Token: 0x06003C20 RID: 15392 RVA: 0x0012D953 File Offset: 0x0012BD53
	public PhotonPlayer LocalPlayer { get; internal set; }

	// Token: 0x1700097B RID: 2427
	// (get) Token: 0x06003C21 RID: 15393 RVA: 0x0012D95C File Offset: 0x0012BD5C
	// (set) Token: 0x06003C22 RID: 15394 RVA: 0x0012D964 File Offset: 0x0012BD64
	public int PlayersOnMasterCount { get; internal set; }

	// Token: 0x1700097C RID: 2428
	// (get) Token: 0x06003C23 RID: 15395 RVA: 0x0012D96D File Offset: 0x0012BD6D
	// (set) Token: 0x06003C24 RID: 15396 RVA: 0x0012D975 File Offset: 0x0012BD75
	public int PlayersInRoomsCount { get; internal set; }

	// Token: 0x1700097D RID: 2429
	// (get) Token: 0x06003C25 RID: 15397 RVA: 0x0012D97E File Offset: 0x0012BD7E
	// (set) Token: 0x06003C26 RID: 15398 RVA: 0x0012D986 File Offset: 0x0012BD86
	public int RoomsCount { get; internal set; }

	// Token: 0x1700097E RID: 2430
	// (get) Token: 0x06003C27 RID: 15399 RVA: 0x0012D98F File Offset: 0x0012BD8F
	protected internal int FriendListAge
	{
		get
		{
			return (!this.isFetchingFriendList && this.friendListTimestamp != 0) ? (Environment.TickCount - this.friendListTimestamp) : 0;
		}
	}

	// Token: 0x1700097F RID: 2431
	// (get) Token: 0x06003C28 RID: 15400 RVA: 0x0012D9B9 File Offset: 0x0012BDB9
	public bool IsAuthorizeSecretAvailable
	{
		get
		{
			return this.AuthValues != null && !string.IsNullOrEmpty(this.AuthValues.Token);
		}
	}

	// Token: 0x17000980 RID: 2432
	// (get) Token: 0x06003C29 RID: 15401 RVA: 0x0012D9DC File Offset: 0x0012BDDC
	// (set) Token: 0x06003C2A RID: 15402 RVA: 0x0012D9E4 File Offset: 0x0012BDE4
	public List<Region> AvailableRegions { get; protected internal set; }

	// Token: 0x17000981 RID: 2433
	// (get) Token: 0x06003C2B RID: 15403 RVA: 0x0012D9ED File Offset: 0x0012BDED
	// (set) Token: 0x06003C2C RID: 15404 RVA: 0x0012D9F5 File Offset: 0x0012BDF5
	public CloudRegionCode CloudRegion { get; protected internal set; }

	// Token: 0x17000982 RID: 2434
	// (get) Token: 0x06003C2D RID: 15405 RVA: 0x0012D9FE File Offset: 0x0012BDFE
	// (set) Token: 0x06003C2E RID: 15406 RVA: 0x0012DA32 File Offset: 0x0012BE32
	public int mMasterClientId
	{
		get
		{
			if (PhotonNetwork.offlineMode)
			{
				return this.LocalPlayer.ID;
			}
			return (this.CurrentRoom != null) ? this.CurrentRoom.MasterClientId : 0;
		}
		private set
		{
			if (this.CurrentRoom != null)
			{
				this.CurrentRoom.MasterClientId = value;
			}
		}
	}

	// Token: 0x06003C2F RID: 15407 RVA: 0x0012DA4C File Offset: 0x0012BE4C
	private string GetNameServerAddress()
	{
		ConnectionProtocol transportProtocol = base.TransportProtocol;
		int num = 0;
		NetworkingPeer.ProtocolToNameServerPort.TryGetValue(transportProtocol, out num);
		string arg = string.Empty;
		if (transportProtocol == ConnectionProtocol.WebSocket)
		{
			arg = "ws://";
		}
		else if (transportProtocol == ConnectionProtocol.WebSocketSecure)
		{
			arg = "wss://";
		}
		return string.Format("{0}{1}:{2}", arg, "ns.exitgames.com", num);
	}

	// Token: 0x06003C30 RID: 15408 RVA: 0x0012DAAD File Offset: 0x0012BEAD
	public override bool Connect(string serverAddress, string applicationName)
	{
		Debug.LogError("Avoid using this directly. Thanks.");
		return false;
	}

	// Token: 0x06003C31 RID: 15409 RVA: 0x0012DABA File Offset: 0x0012BEBA
	public bool ReconnectToMaster()
	{
		if (this.AuthValues == null)
		{
			Debug.LogWarning("ReconnectToMaster() with AuthValues == null is not correct!");
			this.AuthValues = new AuthenticationValues();
		}
		this.AuthValues.Token = this.tokenCache;
		return this.Connect(this.MasterServerAddress, ServerConnection.MasterServer);
	}

	// Token: 0x06003C32 RID: 15410 RVA: 0x0012DAFC File Offset: 0x0012BEFC
	public bool ReconnectAndRejoin()
	{
		if (this.AuthValues == null)
		{
			Debug.LogWarning("ReconnectAndRejoin() with AuthValues == null is not correct!");
			this.AuthValues = new AuthenticationValues();
		}
		this.AuthValues.Token = this.tokenCache;
		if (!string.IsNullOrEmpty(this.GameServerAddress) && this.enterRoomParamsCache != null)
		{
			this.lastJoinType = JoinType.JoinRoom;
			this.enterRoomParamsCache.RejoinOnly = true;
			return this.Connect(this.GameServerAddress, ServerConnection.GameServer);
		}
		return false;
	}

	// Token: 0x06003C33 RID: 15411 RVA: 0x0012DB78 File Offset: 0x0012BF78
	public bool Connect(string serverAddress, ServerConnection type)
	{
		if (PhotonHandler.AppQuits)
		{
			Debug.LogWarning("Ignoring Connect() because app gets closed. If this is an error, check PhotonHandler.AppQuits.");
			return false;
		}
		if (this.State == ClientState.Disconnecting)
		{
			Debug.LogError("Connect() failed. Can't connect while disconnecting (still). Current state: " + PhotonNetwork.connectionStateDetailed);
			return false;
		}
		this.SetupProtocol(type);
		bool flag = base.Connect(serverAddress, string.Empty, this.TokenForInit);
		if (flag)
		{
			if (type != ServerConnection.NameServer)
			{
				if (type != ServerConnection.MasterServer)
				{
					if (type == ServerConnection.GameServer)
					{
						this.State = ClientState.ConnectingToGameserver;
					}
				}
				else
				{
					this.State = ClientState.ConnectingToMasterserver;
				}
			}
			else
			{
				this.State = ClientState.ConnectingToNameServer;
			}
		}
		return flag;
	}

	// Token: 0x06003C34 RID: 15412 RVA: 0x0012DC24 File Offset: 0x0012C024
	public bool ConnectToNameServer()
	{
		if (PhotonHandler.AppQuits)
		{
			Debug.LogWarning("Ignoring Connect() because app gets closed. If this is an error, check PhotonHandler.AppQuits.");
			return false;
		}
		this.IsUsingNameServer = true;
		this.CloudRegion = CloudRegionCode.none;
		if (this.State == ClientState.ConnectedToNameServer)
		{
			return true;
		}
		this.SetupProtocol(ServerConnection.NameServer);
		if (!base.Connect(this.NameServerAddress, "ns", this.TokenForInit))
		{
			return false;
		}
		this.State = ClientState.ConnectingToNameServer;
		return true;
	}

	// Token: 0x06003C35 RID: 15413 RVA: 0x0012DC94 File Offset: 0x0012C094
	public bool ConnectToRegionMaster(CloudRegionCode region)
	{
		if (PhotonHandler.AppQuits)
		{
			Debug.LogWarning("Ignoring Connect() because app gets closed. If this is an error, check PhotonHandler.AppQuits.");
			return false;
		}
		this.IsUsingNameServer = true;
		this.CloudRegion = region;
		if (this.State == ClientState.ConnectedToNameServer)
		{
			return this.CallAuthenticate();
		}
		this.SetupProtocol(ServerConnection.NameServer);
		if (!base.Connect(this.NameServerAddress, "ns", this.TokenForInit))
		{
			return false;
		}
		this.State = ClientState.ConnectingToNameServer;
		return true;
	}

	// Token: 0x06003C36 RID: 15414 RVA: 0x0012DD08 File Offset: 0x0012C108
	protected internal void SetupProtocol(ServerConnection serverType)
	{
		ConnectionProtocol connectionProtocol = base.TransportProtocol;
		if (this.AuthMode == AuthModeOption.AuthOnceWss)
		{
			if (serverType != ServerConnection.NameServer)
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.ErrorsOnly)
				{
					Debug.LogWarning("Using PhotonServerSettings.Protocol when leaving the NameServer (AuthMode is AuthOnceWss): " + PhotonNetwork.PhotonServerSettings.Protocol);
				}
				connectionProtocol = PhotonNetwork.PhotonServerSettings.Protocol;
			}
			else
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.ErrorsOnly)
				{
					Debug.LogWarning("Using WebSocket to connect NameServer (AuthMode is AuthOnceWss).");
				}
				connectionProtocol = ConnectionProtocol.WebSocketSecure;
			}
		}
		Type type = Type.GetType("ExitGames.Client.Photon.SocketWebTcp, Assembly-CSharp", false);
		if (type == null)
		{
			type = Type.GetType("ExitGames.Client.Photon.SocketWebTcp, Assembly-CSharp-firstpass", false);
		}
		if (type != null)
		{
			this.SocketImplementationConfig[ConnectionProtocol.WebSocket] = type;
			this.SocketImplementationConfig[ConnectionProtocol.WebSocketSecure] = type;
		}
		if (PhotonHandler.PingImplementation == null)
		{
			PhotonHandler.PingImplementation = typeof(PingMono);
		}
		if (base.TransportProtocol == connectionProtocol)
		{
			return;
		}
		if (PhotonNetwork.logLevel >= PhotonLogLevel.ErrorsOnly)
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				"Protocol switch from: ",
				base.TransportProtocol,
				" to: ",
				connectionProtocol,
				"."
			}));
		}
		base.TransportProtocol = connectionProtocol;
	}

	// Token: 0x06003C37 RID: 15415 RVA: 0x0012DE34 File Offset: 0x0012C234
	public override void Disconnect()
	{
		if (base.PeerState == PeerStateValue.Disconnected)
		{
			if (!PhotonHandler.AppQuits)
			{
				Debug.LogWarning(string.Format("Can't execute Disconnect() while not connected. Nothing changed. State: {0}", this.State));
			}
			return;
		}
		this.State = ClientState.Disconnecting;
		base.Disconnect();
	}

	// Token: 0x06003C38 RID: 15416 RVA: 0x0012DE74 File Offset: 0x0012C274
	private bool CallAuthenticate()
	{
		AuthenticationValues authenticationValues;
		if ((authenticationValues = this.AuthValues) == null)
		{
			authenticationValues = new AuthenticationValues
			{
				UserId = this.PlayerName
			};
		}
		AuthenticationValues authValues = authenticationValues;
		if (this.AuthMode == AuthModeOption.Auth)
		{
			return this.OpAuthenticate(this.AppId, this.AppVersion, authValues, this.CloudRegion.ToString(), this.requestLobbyStatistics);
		}
		return this.OpAuthenticateOnce(this.AppId, this.AppVersion, authValues, this.CloudRegion.ToString(), this.EncryptionMode, PhotonNetwork.PhotonServerSettings.Protocol);
	}

	// Token: 0x06003C39 RID: 15417 RVA: 0x0012DF14 File Offset: 0x0012C314
	private void DisconnectToReconnect()
	{
		ServerConnection server = this.Server;
		if (server != ServerConnection.NameServer)
		{
			if (server != ServerConnection.MasterServer)
			{
				if (server == ServerConnection.GameServer)
				{
					this.State = ClientState.DisconnectingFromGameserver;
					base.Disconnect();
				}
			}
			else
			{
				this.State = ClientState.DisconnectingFromMasterserver;
				base.Disconnect();
			}
		}
		else
		{
			this.State = ClientState.DisconnectingFromNameServer;
			base.Disconnect();
		}
	}

	// Token: 0x06003C3A RID: 15418 RVA: 0x0012DF7C File Offset: 0x0012C37C
	public bool GetRegions()
	{
		if (this.Server != ServerConnection.NameServer)
		{
			return false;
		}
		bool flag = this.OpGetRegions(this.AppId);
		if (flag)
		{
			this.AvailableRegions = null;
		}
		return flag;
	}

	// Token: 0x06003C3B RID: 15419 RVA: 0x0012DFB2 File Offset: 0x0012C3B2
	public override bool OpFindFriends(string[] friendsToFind)
	{
		if (this.isFetchingFriendList)
		{
			return false;
		}
		this.friendListRequested = friendsToFind;
		this.isFetchingFriendList = true;
		return base.OpFindFriends(friendsToFind);
	}

	// Token: 0x06003C3C RID: 15420 RVA: 0x0012DFD8 File Offset: 0x0012C3D8
	public bool OpCreateGame(EnterRoomParams enterRoomParams)
	{
		bool flag = this.Server == ServerConnection.GameServer;
		enterRoomParams.OnGameServer = flag;
		enterRoomParams.PlayerProperties = this.GetLocalActorProperties();
		if (!flag)
		{
			this.enterRoomParamsCache = enterRoomParams;
		}
		this.lastJoinType = JoinType.CreateRoom;
		return base.OpCreateRoom(enterRoomParams);
	}

	// Token: 0x06003C3D RID: 15421 RVA: 0x0012E020 File Offset: 0x0012C420
	public override bool OpJoinRoom(EnterRoomParams opParams)
	{
		bool flag = this.Server == ServerConnection.GameServer;
		opParams.OnGameServer = flag;
		if (!flag)
		{
			this.enterRoomParamsCache = opParams;
		}
		this.lastJoinType = ((!opParams.CreateIfNotExists) ? JoinType.JoinRoom : JoinType.JoinOrCreateRoom);
		return base.OpJoinRoom(opParams);
	}

	// Token: 0x06003C3E RID: 15422 RVA: 0x0012E06A File Offset: 0x0012C46A
	public override bool OpJoinRandomRoom(OpJoinRandomRoomParams opJoinRandomRoomParams)
	{
		this.enterRoomParamsCache = new EnterRoomParams();
		this.enterRoomParamsCache.Lobby = opJoinRandomRoomParams.TypedLobby;
		this.enterRoomParamsCache.ExpectedUsers = opJoinRandomRoomParams.ExpectedUsers;
		this.lastJoinType = JoinType.JoinRandomRoom;
		return base.OpJoinRandomRoom(opJoinRandomRoomParams);
	}

	// Token: 0x06003C3F RID: 15423 RVA: 0x0012E0A7 File Offset: 0x0012C4A7
	public virtual bool OpLeave()
	{
		if (this.State != ClientState.Joined)
		{
			Debug.LogWarning("Not sending leave operation. State is not 'Joined': " + this.State);
			return false;
		}
		return this.OpCustom(254, null, true, 0);
	}

	// Token: 0x06003C40 RID: 15424 RVA: 0x0012E0E0 File Offset: 0x0012C4E0
	public override bool OpRaiseEvent(byte eventCode, object customEventContent, bool sendReliable, RaiseEventOptions raiseEventOptions)
	{
		return !PhotonNetwork.offlineMode && base.OpRaiseEvent(eventCode, customEventContent, sendReliable, raiseEventOptions);
	}

	// Token: 0x06003C41 RID: 15425 RVA: 0x0012E0FC File Offset: 0x0012C4FC
	private void ReadoutProperties(ExitGames.Client.Photon.Hashtable gameProperties, ExitGames.Client.Photon.Hashtable pActorProperties, int targetActorNr)
	{
		if (pActorProperties != null && pActorProperties.Count > 0)
		{
			if (targetActorNr > 0)
			{
				PhotonPlayer playerWithId = this.GetPlayerWithId(targetActorNr);
				if (playerWithId != null)
				{
					ExitGames.Client.Photon.Hashtable hashtable = this.ReadoutPropertiesForActorNr(pActorProperties, targetActorNr);
					playerWithId.InternalCacheProperties(hashtable);
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, new object[]
					{
						playerWithId,
						hashtable
					});
				}
			}
			else
			{
				foreach (object obj in pActorProperties.Keys)
				{
					int num = (int)obj;
					ExitGames.Client.Photon.Hashtable hashtable2 = (ExitGames.Client.Photon.Hashtable)pActorProperties[obj];
					string name = (string)hashtable2[byte.MaxValue];
					PhotonPlayer photonPlayer = this.GetPlayerWithId(num);
					if (photonPlayer == null)
					{
						photonPlayer = new PhotonPlayer(false, num, name);
						this.AddNewPlayer(num, photonPlayer);
					}
					photonPlayer.InternalCacheProperties(hashtable2);
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, new object[]
					{
						photonPlayer,
						hashtable2
					});
				}
			}
		}
		if (this.CurrentRoom != null && gameProperties != null)
		{
			this.CurrentRoom.InternalCacheProperties(gameProperties);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCustomRoomPropertiesChanged, new object[]
			{
				gameProperties
			});
			if (PhotonNetwork.automaticallySyncScene)
			{
				this.LoadLevelIfSynced();
			}
		}
	}

	// Token: 0x06003C42 RID: 15426 RVA: 0x0012E254 File Offset: 0x0012C654
	private ExitGames.Client.Photon.Hashtable ReadoutPropertiesForActorNr(ExitGames.Client.Photon.Hashtable actorProperties, int actorNr)
	{
		if (actorProperties.ContainsKey(actorNr))
		{
			return (ExitGames.Client.Photon.Hashtable)actorProperties[actorNr];
		}
		return actorProperties;
	}

	// Token: 0x06003C43 RID: 15427 RVA: 0x0012E27C File Offset: 0x0012C67C
	public void ChangeLocalID(int newID)
	{
		if (this.LocalPlayer == null)
		{
			Debug.LogWarning(string.Format("LocalPlayer is null or not in mActors! LocalPlayer: {0} mActors==null: {1} newID: {2}", this.LocalPlayer, this.mActors == null, newID));
		}
		if (this.mActors.ContainsKey(this.LocalPlayer.ID))
		{
			this.mActors.Remove(this.LocalPlayer.ID);
		}
		this.LocalPlayer.InternalChangeLocalID(newID);
		this.mActors[this.LocalPlayer.ID] = this.LocalPlayer;
		this.RebuildPlayerListCopies();
	}

	// Token: 0x06003C44 RID: 15428 RVA: 0x0012E31D File Offset: 0x0012C71D
	private void LeftLobbyCleanup()
	{
		this.mGameList = new Dictionary<string, RoomInfo>();
		this.mGameListCopy = new RoomInfo[0];
		if (this.insideLobby)
		{
			this.insideLobby = false;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLeftLobby, new object[0]);
		}
	}

	// Token: 0x06003C45 RID: 15429 RVA: 0x0012E354 File Offset: 0x0012C754
	private void LeftRoomCleanup()
	{
		bool flag = this.CurrentRoom != null;
		bool flag2 = (this.CurrentRoom == null) ? PhotonNetwork.autoCleanUpPlayerObjects : this.CurrentRoom.AutoCleanUp;
		this.hasSwitchedMC = false;
		this.CurrentRoom = null;
		this.mActors = new Dictionary<int, PhotonPlayer>();
		this.mPlayerListCopy = new PhotonPlayer[0];
		this.mOtherPlayerListCopy = new PhotonPlayer[0];
		this.allowedReceivingGroups = new HashSet<byte>();
		this.blockSendingGroups = new HashSet<byte>();
		this.mGameList = new Dictionary<string, RoomInfo>();
		this.mGameListCopy = new RoomInfo[0];
		this.isFetchingFriendList = false;
		this.ChangeLocalID(-1);
		if (flag2)
		{
			this.LocalCleanupAnythingInstantiated(true);
			PhotonNetwork.manuallyAllocatedViewIds = new List<int>();
		}
		if (flag)
		{
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLeftRoom, new object[0]);
		}
	}

	// Token: 0x06003C46 RID: 15430 RVA: 0x0012E424 File Offset: 0x0012C824
	protected internal void LocalCleanupAnythingInstantiated(bool destroyInstantiatedGameObjects)
	{
		if (this.tempInstantiationData.Count > 0)
		{
			Debug.LogWarning("It seems some instantiation is not completed, as instantiation data is used. You should make sure instantiations are paused when calling this method. Cleaning now, despite this.");
		}
		if (destroyInstantiatedGameObjects)
		{
			HashSet<GameObject> hashSet = new HashSet<GameObject>();
			foreach (PhotonView photonView in this.photonViewList.Values)
			{
				if (photonView.isRuntimeInstantiated)
				{
					hashSet.Add(photonView.gameObject);
				}
			}
			foreach (GameObject go in hashSet)
			{
				this.RemoveInstantiatedGO(go, true);
			}
		}
		this.tempInstantiationData.Clear();
		PhotonNetwork.lastUsedViewSubId = 0;
		PhotonNetwork.lastUsedViewSubIdStatic = 0;
	}

	// Token: 0x06003C47 RID: 15431 RVA: 0x0012E51C File Offset: 0x0012C91C
	private void GameEnteredOnGameServer(OperationResponse operationResponse)
	{
		if (operationResponse.ReturnCode != 0)
		{
			byte operationCode = operationResponse.OperationCode;
			if (operationCode != 227)
			{
				if (operationCode != 226)
				{
					if (operationCode == 225)
					{
						if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
						{
							Debug.Log("Join failed on GameServer. Changing back to MasterServer. Msg: " + operationResponse.DebugMessage);
							if (operationResponse.ReturnCode == 32758)
							{
								Debug.Log("Most likely the game became empty during the switch to GameServer.");
							}
						}
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonRandomJoinFailed, new object[]
						{
							operationResponse.ReturnCode,
							operationResponse.DebugMessage
						});
					}
				}
				else
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
					{
						Debug.Log("Join failed on GameServer. Changing back to MasterServer. Msg: " + operationResponse.DebugMessage);
						if (operationResponse.ReturnCode == 32758)
						{
							Debug.Log("Most likely the game became empty during the switch to GameServer.");
						}
					}
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonJoinRoomFailed, new object[]
					{
						operationResponse.ReturnCode,
						operationResponse.DebugMessage
					});
				}
			}
			else
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
				{
					Debug.Log("Create failed on GameServer. Changing back to MasterServer. Msg: " + operationResponse.DebugMessage);
				}
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCreateRoomFailed, new object[]
				{
					operationResponse.ReturnCode,
					operationResponse.DebugMessage
				});
			}
			this.DisconnectToReconnect();
			return;
		}
		this.CurrentRoom = new Room(this.enterRoomParamsCache.RoomName, null)
		{
			IsLocalClientInside = true
		};
		this.State = ClientState.Joined;
		if (operationResponse.Parameters.ContainsKey(252))
		{
			int[] actorsInRoom = (int[])operationResponse.Parameters[252];
			this.UpdatedActorList(actorsInRoom);
		}
		int newID = (int)operationResponse[254];
		this.ChangeLocalID(newID);
		ExitGames.Client.Photon.Hashtable pActorProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[249];
		ExitGames.Client.Photon.Hashtable gameProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[248];
		this.ReadoutProperties(gameProperties, pActorProperties, 0);
		if (!this.CurrentRoom.serverSideMasterClient)
		{
			this.CheckMasterClient(-1);
		}
		if (this.mPlayernameHasToBeUpdated)
		{
			this.SendPlayerName();
		}
		byte operationCode2 = operationResponse.OperationCode;
		if (operationCode2 != 227)
		{
			if (operationCode2 != 226 && operationCode2 != 225)
			{
			}
		}
		else
		{
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom, new object[0]);
		}
	}

	// Token: 0x06003C48 RID: 15432 RVA: 0x0012E785 File Offset: 0x0012CB85
	private void AddNewPlayer(int ID, PhotonPlayer player)
	{
		if (!this.mActors.ContainsKey(ID))
		{
			this.mActors[ID] = player;
			this.RebuildPlayerListCopies();
		}
		else
		{
			Debug.LogError("Adding player twice: " + ID);
		}
	}

	// Token: 0x06003C49 RID: 15433 RVA: 0x0012E7C5 File Offset: 0x0012CBC5
	private void RemovePlayer(int ID, PhotonPlayer player)
	{
		this.mActors.Remove(ID);
		if (!player.IsLocal)
		{
			this.RebuildPlayerListCopies();
		}
	}

	// Token: 0x06003C4A RID: 15434 RVA: 0x0012E7E8 File Offset: 0x0012CBE8
	private void RebuildPlayerListCopies()
	{
		this.mPlayerListCopy = new PhotonPlayer[this.mActors.Count];
		this.mActors.Values.CopyTo(this.mPlayerListCopy, 0);
		List<PhotonPlayer> list = new List<PhotonPlayer>();
		for (int i = 0; i < this.mPlayerListCopy.Length; i++)
		{
			PhotonPlayer photonPlayer = this.mPlayerListCopy[i];
			if (!photonPlayer.IsLocal)
			{
				list.Add(photonPlayer);
			}
		}
		this.mOtherPlayerListCopy = list.ToArray();
	}

	// Token: 0x06003C4B RID: 15435 RVA: 0x0012E868 File Offset: 0x0012CC68
	private void ResetPhotonViewsOnSerialize()
	{
		foreach (PhotonView photonView in this.photonViewList.Values)
		{
			photonView.lastOnSerializeDataSent = null;
		}
	}

	// Token: 0x06003C4C RID: 15436 RVA: 0x0012E8CC File Offset: 0x0012CCCC
	private void HandleEventLeave(int actorID, EventData evLeave)
	{
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log(string.Concat(new object[]
			{
				"HandleEventLeave for player ID: ",
				actorID,
				" evLeave: ",
				evLeave.ToStringFull()
			}));
		}
		PhotonPlayer playerWithId = this.GetPlayerWithId(actorID);
		if (playerWithId == null)
		{
			Debug.LogError(string.Format("Received event Leave for unknown player ID: {0}", actorID));
			return;
		}
		bool isInactive = playerWithId.IsInactive;
		if (evLeave.Parameters.ContainsKey(233))
		{
			playerWithId.IsInactive = (bool)evLeave.Parameters[233];
			if (playerWithId.IsInactive != isInactive)
			{
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerActivityChanged, new object[]
				{
					playerWithId
				});
			}
			if (playerWithId.IsInactive && isInactive)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					"HandleEventLeave for player ID: ",
					actorID,
					" isInactive: ",
					playerWithId.IsInactive,
					". Stopping handling if inactive."
				}));
				return;
			}
		}
		if (evLeave.Parameters.ContainsKey(203))
		{
			int num = (int)evLeave[203];
			if (num != 0)
			{
				this.mMasterClientId = (int)evLeave[203];
				this.UpdateMasterClient();
			}
		}
		else if (!this.CurrentRoom.serverSideMasterClient)
		{
			this.CheckMasterClient(actorID);
		}
		if (playerWithId.IsInactive && !isInactive)
		{
			return;
		}
		if (this.CurrentRoom != null && this.CurrentRoom.AutoCleanUp)
		{
			this.DestroyPlayerObjects(actorID, true);
		}
		this.RemovePlayer(actorID, playerWithId);
		NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerDisconnected, new object[]
		{
			playerWithId
		});
	}

	// Token: 0x06003C4D RID: 15437 RVA: 0x0012EA94 File Offset: 0x0012CE94
	private void CheckMasterClient(int leavingPlayerId)
	{
		bool flag = this.mMasterClientId == leavingPlayerId;
		bool flag2 = leavingPlayerId > 0;
		if (flag2 && !flag)
		{
			return;
		}
		int num;
		if (this.mActors.Count <= 1)
		{
			num = this.LocalPlayer.ID;
		}
		else
		{
			num = int.MaxValue;
			foreach (int num2 in this.mActors.Keys)
			{
				if (num2 < num && num2 != leavingPlayerId)
				{
					num = num2;
				}
			}
		}
		this.mMasterClientId = num;
		if (flag2)
		{
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, new object[]
			{
				this.GetPlayerWithId(num)
			});
		}
	}

	// Token: 0x06003C4E RID: 15438 RVA: 0x0012EB68 File Offset: 0x0012CF68
	protected internal void UpdateMasterClient()
	{
		NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, new object[]
		{
			PhotonNetwork.masterClient
		});
	}

	// Token: 0x06003C4F RID: 15439 RVA: 0x0012EB80 File Offset: 0x0012CF80
	private static int ReturnLowestPlayerId(PhotonPlayer[] players, int playerIdToIgnore)
	{
		if (players == null || players.Length == 0)
		{
			return -1;
		}
		int num = int.MaxValue;
		foreach (PhotonPlayer photonPlayer in players)
		{
			if (photonPlayer.ID != playerIdToIgnore)
			{
				if (photonPlayer.ID < num)
				{
					num = photonPlayer.ID;
				}
			}
		}
		return num;
	}

	// Token: 0x06003C50 RID: 15440 RVA: 0x0012EBE0 File Offset: 0x0012CFE0
	protected internal bool SetMasterClient(int playerId, bool sync)
	{
		bool flag = this.mMasterClientId != playerId;
		if (!flag || !this.mActors.ContainsKey(playerId))
		{
			return false;
		}
		if (sync && !this.OpRaiseEvent(208, new ExitGames.Client.Photon.Hashtable
		{
			{
				1,
				playerId
			}
		}, true, null))
		{
			return false;
		}
		this.hasSwitchedMC = true;
		this.CurrentRoom.MasterClientId = playerId;
		NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, new object[]
		{
			this.GetPlayerWithId(playerId)
		});
		return true;
	}

	// Token: 0x06003C51 RID: 15441 RVA: 0x0012EC74 File Offset: 0x0012D074
	public bool SetMasterClient(int nextMasterId)
	{
		ExitGames.Client.Photon.Hashtable gameProperties = new ExitGames.Client.Photon.Hashtable
		{
			{
				248,
				nextMasterId
			}
		};
		ExitGames.Client.Photon.Hashtable expectedProperties = new ExitGames.Client.Photon.Hashtable
		{
			{
				248,
				this.mMasterClientId
			}
		};
		return base.OpSetPropertiesOfRoom(gameProperties, expectedProperties, false);
	}

	// Token: 0x06003C52 RID: 15442 RVA: 0x0012ECCC File Offset: 0x0012D0CC
	protected internal PhotonPlayer GetPlayerWithId(int number)
	{
		if (this.mActors == null)
		{
			return null;
		}
		PhotonPlayer result = null;
		this.mActors.TryGetValue(number, out result);
		return result;
	}

	// Token: 0x06003C53 RID: 15443 RVA: 0x0012ECF8 File Offset: 0x0012D0F8
	private void SendPlayerName()
	{
		if (this.State == ClientState.Joining)
		{
			this.mPlayernameHasToBeUpdated = true;
			return;
		}
		if (this.LocalPlayer != null)
		{
			this.LocalPlayer.NickName = this.PlayerName;
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[byte.MaxValue] = this.PlayerName;
			if (this.LocalPlayer.ID > 0)
			{
				base.OpSetPropertiesOfActor(this.LocalPlayer.ID, hashtable, null, false);
				this.mPlayernameHasToBeUpdated = false;
			}
		}
	}

	// Token: 0x06003C54 RID: 15444 RVA: 0x0012ED80 File Offset: 0x0012D180
	private ExitGames.Client.Photon.Hashtable GetLocalActorProperties()
	{
		if (PhotonNetwork.player != null)
		{
			return PhotonNetwork.player.AllProperties;
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[byte.MaxValue] = this.PlayerName;
		return hashtable;
	}

	// Token: 0x06003C55 RID: 15445 RVA: 0x0012EDC0 File Offset: 0x0012D1C0
	public void DebugReturn(DebugLevel level, string message)
	{
		if (level == DebugLevel.ERROR)
		{
			Debug.LogError(message);
		}
		else if (level == DebugLevel.WARNING)
		{
			Debug.LogWarning(message);
		}
		else if (level == DebugLevel.INFO && PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log(message);
		}
		else if (level == DebugLevel.ALL && PhotonNetwork.logLevel == PhotonLogLevel.Full)
		{
			Debug.Log(message);
		}
	}

	// Token: 0x06003C56 RID: 15446 RVA: 0x0012EE28 File Offset: 0x0012D228
	public void OnOperationResponse(OperationResponse operationResponse)
	{
		if (PhotonNetwork.networkingPeer.State == ClientState.Disconnecting)
		{
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log("OperationResponse ignored while disconnecting. Code: " + operationResponse.OperationCode);
			}
			return;
		}
		if (operationResponse.ReturnCode == 0)
		{
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log(operationResponse.ToString());
			}
		}
		else if (operationResponse.ReturnCode == -3)
		{
			Debug.LogError("Operation " + operationResponse.OperationCode + " could not be executed (yet). Wait for state JoinedLobby or ConnectedToMaster and their callbacks before calling operations. WebRPCs need a server-side configuration. Enum OperationCode helps identify the operation.");
		}
		else if (operationResponse.ReturnCode == 32752)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Operation ",
				operationResponse.OperationCode,
				" failed in a server-side plugin. Check the configuration in the Dashboard. Message from server-plugin: ",
				operationResponse.DebugMessage
			}));
		}
		else if (operationResponse.ReturnCode == 32760)
		{
			Debug.LogWarning("Operation failed: " + operationResponse.ToStringFull());
		}
		else
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Operation failed: ",
				operationResponse.ToStringFull(),
				" Server: ",
				this.Server
			}));
		}
		if (operationResponse.Parameters.ContainsKey(221))
		{
			if (this.AuthValues == null)
			{
				this.AuthValues = new AuthenticationValues();
			}
			this.AuthValues.Token = (operationResponse[221] as string);
			this.tokenCache = this.AuthValues.Token;
		}
		byte operationCode = operationResponse.OperationCode;
		switch (operationCode)
		{
		case 217:
			if (operationResponse.ReturnCode != 0)
			{
				this.DebugReturn(DebugLevel.ERROR, "GetGameList failed: " + operationResponse.ToStringFull());
			}
			else
			{
				this.mGameList = new Dictionary<string, RoomInfo>();
				ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)operationResponse[222];
				foreach (object obj in hashtable.Keys)
				{
					string text = (string)obj;
					this.mGameList[text] = new RoomInfo(text, (ExitGames.Client.Photon.Hashtable)hashtable[obj]);
				}
				this.mGameListCopy = new RoomInfo[this.mGameList.Count];
				this.mGameList.Values.CopyTo(this.mGameListCopy, 0);
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnReceivedRoomListUpdate, new object[0]);
			}
			break;
		default:
			switch (operationCode)
			{
			case 251:
			{
				ExitGames.Client.Photon.Hashtable pActorProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[249];
				ExitGames.Client.Photon.Hashtable gameProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[248];
				this.ReadoutProperties(gameProperties, pActorProperties, 0);
				break;
			}
			case 252:
				break;
			case 253:
				break;
			case 254:
				this.DisconnectToReconnect();
				break;
			default:
				Debug.LogWarning(string.Format("OperationResponse unhandled: {0}", operationResponse.ToString()));
				break;
			}
			break;
		case 219:
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnWebRpcResponse, new object[]
			{
				operationResponse
			});
			break;
		case 220:
			if (operationResponse.ReturnCode == 32767)
			{
				Debug.LogError(string.Format("The appId this client sent is unknown on the server (Cloud). Check settings. If using the Cloud, check account.", new object[0]));
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, new object[]
				{
					DisconnectCause.InvalidAuthentication
				});
				this.State = ClientState.Disconnecting;
				this.Disconnect();
			}
			else if (operationResponse.ReturnCode != 0)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"GetRegions failed. Can't provide regions list. Error: ",
					operationResponse.ReturnCode,
					": ",
					operationResponse.DebugMessage
				}));
			}
			else
			{
				string[] array = operationResponse[210] as string[];
				string[] array2 = operationResponse[230] as string[];
				if (array == null || array2 == null || array.Length != array2.Length)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"The region arrays from Name Server are not ok. Must be non-null and same length. ",
						array == null,
						" ",
						array2 == null,
						"\n",
						operationResponse.ToStringFull()
					}));
				}
				else
				{
					this.AvailableRegions = new List<Region>(array.Length);
					for (int i = 0; i < array.Length; i++)
					{
						string text2 = array[i];
						if (!string.IsNullOrEmpty(text2))
						{
							text2 = text2.ToLower();
							CloudRegionCode cloudRegionCode = Region.Parse(text2);
							bool flag = true;
							if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.BestRegion && PhotonNetwork.PhotonServerSettings.EnabledRegions != (CloudRegionFlag)0)
							{
								CloudRegionFlag cloudRegionFlag = Region.ParseFlag(cloudRegionCode);
								flag = ((PhotonNetwork.PhotonServerSettings.EnabledRegions & cloudRegionFlag) != (CloudRegionFlag)0);
								if (!flag && PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
								{
									Debug.Log("Skipping region because it's not in PhotonServerSettings.EnabledRegions: " + cloudRegionCode);
								}
							}
							if (flag)
							{
								this.AvailableRegions.Add(new Region(cloudRegionCode, text2, array2[i]));
							}
						}
					}
					if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.BestRegion)
					{
						PhotonHandler.PingAvailableRegionsAndConnectToBest();
					}
				}
			}
			break;
		case 222:
		{
			bool[] array3 = operationResponse[1] as bool[];
			string[] array4 = operationResponse[2] as string[];
			if (array3 != null && array4 != null && this.friendListRequested != null && array3.Length == this.friendListRequested.Length)
			{
				List<FriendInfo> list = new List<FriendInfo>(this.friendListRequested.Length);
				for (int j = 0; j < this.friendListRequested.Length; j++)
				{
					list.Insert(j, new FriendInfo
					{
						Name = this.friendListRequested[j],
						Room = array4[j],
						IsOnline = array3[j]
					});
				}
				PhotonNetwork.Friends = list;
			}
			else
			{
				Debug.LogError("FindFriends failed to apply the result, as a required value wasn't provided or the friend list length differed from result.");
			}
			this.friendListRequested = null;
			this.isFetchingFriendList = false;
			this.friendListTimestamp = Environment.TickCount;
			if (this.friendListTimestamp == 0)
			{
				this.friendListTimestamp = 1;
			}
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnUpdatedFriendList, new object[0]);
			break;
		}
		case 225:
			if (operationResponse.ReturnCode != 0)
			{
				if (operationResponse.ReturnCode == 32760)
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
					{
						Debug.Log("JoinRandom failed: No open game. Calling: OnPhotonRandomJoinFailed() and staying on master server.");
					}
				}
				else if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
				{
					Debug.LogWarning(string.Format("JoinRandom failed: {0}.", operationResponse.ToStringFull()));
				}
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonRandomJoinFailed, new object[]
				{
					operationResponse.ReturnCode,
					operationResponse.DebugMessage
				});
			}
			else
			{
				string roomName = (string)operationResponse[byte.MaxValue];
				this.enterRoomParamsCache.RoomName = roomName;
				this.GameServerAddress = (string)operationResponse[230];
				this.DisconnectToReconnect();
			}
			break;
		case 226:
			if (this.Server != ServerConnection.GameServer)
			{
				if (operationResponse.ReturnCode != 0)
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
					{
						Debug.Log(string.Format("JoinRoom failed (room maybe closed by now). Client stays on masterserver: {0}. State: {1}", operationResponse.ToStringFull(), this.State));
					}
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonJoinRoomFailed, new object[]
					{
						operationResponse.ReturnCode,
						operationResponse.DebugMessage
					});
				}
				else
				{
					this.GameServerAddress = (string)operationResponse[230];
					this.DisconnectToReconnect();
				}
			}
			else
			{
				this.GameEnteredOnGameServer(operationResponse);
			}
			break;
		case 227:
			if (this.Server == ServerConnection.GameServer)
			{
				this.GameEnteredOnGameServer(operationResponse);
			}
			else if (operationResponse.ReturnCode != 0)
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
				{
					Debug.LogWarning(string.Format("CreateRoom failed, client stays on masterserver: {0}.", operationResponse.ToStringFull()));
				}
				this.State = ((!this.insideLobby) ? ClientState.ConnectedToMaster : ClientState.JoinedLobby);
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCreateRoomFailed, new object[]
				{
					operationResponse.ReturnCode,
					operationResponse.DebugMessage
				});
			}
			else
			{
				string text3 = (string)operationResponse[byte.MaxValue];
				if (!string.IsNullOrEmpty(text3))
				{
					this.enterRoomParamsCache.RoomName = text3;
				}
				this.GameServerAddress = (string)operationResponse[230];
				this.DisconnectToReconnect();
			}
			break;
		case 228:
			this.State = ClientState.Authenticated;
			this.LeftLobbyCleanup();
			break;
		case 229:
			this.State = ClientState.JoinedLobby;
			this.insideLobby = true;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedLobby, new object[0]);
			break;
		case 230:
		case 231:
			if (operationResponse.ReturnCode != 0)
			{
				if (operationResponse.ReturnCode == -2)
				{
					Debug.LogError(string.Format("If you host Photon yourself, make sure to start the 'Instance LoadBalancing' " + base.ServerAddress, new object[0]));
				}
				else if (operationResponse.ReturnCode == 32767)
				{
					Debug.LogError(string.Format("The appId this client sent is unknown on the server (Cloud). Check settings. If using the Cloud, check account.", new object[0]));
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, new object[]
					{
						DisconnectCause.InvalidAuthentication
					});
				}
				else if (operationResponse.ReturnCode == 32755)
				{
					Debug.LogError(string.Format("Custom Authentication failed (either due to user-input or configuration or AuthParameter string format). Calling: OnCustomAuthenticationFailed()", new object[0]));
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCustomAuthenticationFailed, new object[]
					{
						operationResponse.DebugMessage
					});
				}
				else
				{
					Debug.LogError(string.Format("Authentication failed: '{0}' Code: {1}", operationResponse.DebugMessage, operationResponse.ReturnCode));
				}
				this.State = ClientState.Disconnecting;
				this.Disconnect();
				if (operationResponse.ReturnCode == 32757)
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
					{
						Debug.LogWarning(string.Format("Currently, the limit of users is reached for this title. Try again later. Disconnecting", new object[0]));
					}
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonMaxCccuReached, new object[0]);
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, new object[]
					{
						DisconnectCause.MaxCcuReached
					});
				}
				else if (operationResponse.ReturnCode == 32756)
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
					{
						Debug.LogError(string.Format("The used master server address is not available with the subscription currently used. Got to Photon Cloud Dashboard or change URL. Disconnecting.", new object[0]));
					}
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, new object[]
					{
						DisconnectCause.InvalidRegion
					});
				}
				else if (operationResponse.ReturnCode == 32753)
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
					{
						Debug.LogError(string.Format("The authentication ticket expired. You need to connect (and authenticate) again. Disconnecting.", new object[0]));
					}
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, new object[]
					{
						DisconnectCause.AuthenticationTicketExpired
					});
				}
			}
			else
			{
				if (this.Server == ServerConnection.NameServer || this.Server == ServerConnection.MasterServer)
				{
					if (operationResponse.Parameters.ContainsKey(225))
					{
						string text4 = (string)operationResponse.Parameters[225];
						if (!string.IsNullOrEmpty(text4))
						{
							if (this.AuthValues == null)
							{
								this.AuthValues = new AuthenticationValues();
							}
							this.AuthValues.UserId = text4;
							PhotonNetwork.player.UserId = text4;
							if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
							{
								this.DebugReturn(DebugLevel.INFO, string.Format("Received your UserID from server. Updating local value to: {0}", text4));
							}
						}
					}
					if (operationResponse.Parameters.ContainsKey(202))
					{
						this.playername = (string)operationResponse.Parameters[202];
						if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
						{
							this.DebugReturn(DebugLevel.INFO, string.Format("Received your NickName from server. Updating local value to: {0}", this.playername));
						}
					}
					if (operationResponse.Parameters.ContainsKey(192))
					{
						this.SetupEncryption((Dictionary<byte, object>)operationResponse.Parameters[192]);
					}
				}
				if (this.Server == ServerConnection.NameServer)
				{
					this.MasterServerAddress = (operationResponse[230] as string);
					this.DisconnectToReconnect();
				}
				else if (this.Server == ServerConnection.MasterServer)
				{
					if (this.AuthMode != AuthModeOption.Auth)
					{
						this.OpSettings(this.requestLobbyStatistics);
					}
					if (PhotonNetwork.autoJoinLobby)
					{
						this.State = ClientState.Authenticated;
						this.OpJoinLobby(this.lobby);
					}
					else
					{
						this.State = ClientState.ConnectedToMaster;
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster, new object[0]);
					}
				}
				else if (this.Server == ServerConnection.GameServer)
				{
					this.State = ClientState.Joining;
					this.enterRoomParamsCache.PlayerProperties = this.GetLocalActorProperties();
					this.enterRoomParamsCache.OnGameServer = true;
					if (this.lastJoinType == JoinType.JoinRoom || this.lastJoinType == JoinType.JoinRandomRoom || this.lastJoinType == JoinType.JoinOrCreateRoom)
					{
						this.OpJoinRoom(this.enterRoomParamsCache);
					}
					else if (this.lastJoinType == JoinType.CreateRoom)
					{
						this.OpCreateGame(this.enterRoomParamsCache);
					}
				}
				if (operationResponse.Parameters.ContainsKey(245))
				{
					Dictionary<string, object> dictionary = (Dictionary<string, object>)operationResponse.Parameters[245];
					if (dictionary != null)
					{
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCustomAuthenticationResponse, new object[]
						{
							dictionary
						});
					}
				}
			}
			break;
		}
	}

	// Token: 0x06003C57 RID: 15447 RVA: 0x0012FB68 File Offset: 0x0012DF68
	public void OnStatusChanged(StatusCode statusCode)
	{
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log(string.Format("OnStatusChanged: {0} current State: {1}", statusCode.ToString(), this.State));
		}
		switch (statusCode)
		{
		case StatusCode.SecurityExceptionOnConnect:
		case StatusCode.ExceptionOnConnect:
		{
			this.State = ClientState.PeerCreated;
			if (this.AuthValues != null)
			{
				this.AuthValues.Token = null;
			}
			DisconnectCause disconnectCause = (DisconnectCause)statusCode;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, new object[]
			{
				disconnectCause
			});
			return;
		}
		case StatusCode.Connect:
			if (this.State == ClientState.ConnectingToNameServer)
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
				{
					Debug.Log("Connected to NameServer.");
				}
				this.Server = ServerConnection.NameServer;
				if (this.AuthValues != null)
				{
					this.AuthValues.Token = null;
				}
			}
			if (this.State == ClientState.ConnectingToGameserver)
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
				{
					Debug.Log("Connected to gameserver.");
				}
				this.Server = ServerConnection.GameServer;
				this.State = ClientState.ConnectedToGameserver;
			}
			if (this.State == ClientState.ConnectingToMasterserver)
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
				{
					Debug.Log("Connected to masterserver.");
				}
				this.Server = ServerConnection.MasterServer;
				this.State = ClientState.Authenticating;
				if (this.IsInitialConnect)
				{
					this.IsInitialConnect = false;
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToPhoton, new object[0]);
				}
			}
			if (base.TransportProtocol != ConnectionProtocol.WebSocketSecure)
			{
				if (this.Server == ServerConnection.NameServer || this.AuthMode == AuthModeOption.Auth)
				{
					base.EstablishEncryption();
				}
				return;
			}
			if (this.DebugOut == DebugLevel.INFO)
			{
				Debug.Log("Skipping EstablishEncryption. Protocol is secure.");
			}
			break;
		case StatusCode.Disconnect:
			this.didAuthenticate = false;
			this.isFetchingFriendList = false;
			if (this.Server == ServerConnection.GameServer)
			{
				this.LeftRoomCleanup();
			}
			if (this.Server == ServerConnection.MasterServer)
			{
				this.LeftLobbyCleanup();
			}
			if (this.State == ClientState.DisconnectingFromMasterserver)
			{
				if (this.Connect(this.GameServerAddress, ServerConnection.GameServer))
				{
					this.State = ClientState.ConnectingToGameserver;
				}
			}
			else if (this.State == ClientState.DisconnectingFromGameserver || this.State == ClientState.DisconnectingFromNameServer)
			{
				this.SetupProtocol(ServerConnection.MasterServer);
				if (this.Connect(this.MasterServerAddress, ServerConnection.MasterServer))
				{
					this.State = ClientState.ConnectingToMasterserver;
				}
			}
			else
			{
				if (this.AuthValues != null)
				{
					this.AuthValues.Token = null;
				}
				this.State = ClientState.PeerCreated;
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnDisconnectedFromPhoton, new object[0]);
			}
			return;
		case StatusCode.Exception:
			if (this.IsInitialConnect)
			{
				Debug.LogError("Exception while connecting to: " + base.ServerAddress + ". Check if the server is available.");
				if (base.ServerAddress == null || base.ServerAddress.StartsWith("127.0.0.1"))
				{
					Debug.LogWarning("The server address is 127.0.0.1 (localhost): Make sure the server is running on this machine. Android and iOS emulators have their own localhost.");
					if (base.ServerAddress == this.GameServerAddress)
					{
						Debug.LogWarning("This might be a misconfiguration in the game server config. You need to edit it to a (public) address.");
					}
				}
				this.State = ClientState.PeerCreated;
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, new object[]
				{
					disconnectCause
				});
			}
			else
			{
				this.State = ClientState.PeerCreated;
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, new object[]
				{
					disconnectCause
				});
			}
			this.Disconnect();
			return;
		case StatusCode.QueueOutgoingReliableWarning:
		case StatusCode.QueueOutgoingUnreliableWarning:
		case StatusCode.QueueOutgoingAcksWarning:
		case StatusCode.QueueSentWarning:
			return;
		case (StatusCode)1028:
		case (StatusCode)1032:
		case (StatusCode)1034:
		case (StatusCode)1036:
		case (StatusCode)1038:
		case StatusCode.TcpRouterResponseOk:
		case StatusCode.TcpRouterResponseNodeIdUnknown:
		case StatusCode.TcpRouterResponseEndpointUnknown:
		case StatusCode.TcpRouterResponseNodeNotReady:
			goto IL_5FC;
		case StatusCode.SendError:
			return;
		case StatusCode.QueueIncomingReliableWarning:
		case StatusCode.QueueIncomingUnreliableWarning:
			Debug.Log(statusCode + ". This client buffers many incoming messages. This is OK temporarily. With lots of these warnings, check if you send too much or execute messages too slow. " + ((!PhotonNetwork.isMessageQueueRunning) ? "Your isMessageQueueRunning is false. This can cause the issue temporarily." : string.Empty));
			return;
		case StatusCode.ExceptionOnReceive:
		case StatusCode.DisconnectByServer:
		case StatusCode.DisconnectByServerUserLimit:
		case StatusCode.DisconnectByServerLogic:
			if (this.IsInitialConnect)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					statusCode,
					" while connecting to: ",
					base.ServerAddress,
					". Check if the server is available."
				}));
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, new object[]
				{
					disconnectCause
				});
			}
			else
			{
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, new object[]
				{
					disconnectCause
				});
			}
			if (this.AuthValues != null)
			{
				this.AuthValues.Token = null;
			}
			this.Disconnect();
			return;
		case StatusCode.TimeoutDisconnect:
			if (this.IsInitialConnect)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					statusCode,
					" while connecting to: ",
					base.ServerAddress,
					". Check if the server is available."
				}));
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, new object[]
				{
					disconnectCause
				});
			}
			else
			{
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, new object[]
				{
					disconnectCause
				});
			}
			if (this.AuthValues != null)
			{
				this.AuthValues.Token = null;
			}
			this.Disconnect();
			return;
		case StatusCode.EncryptionEstablished:
			break;
		case StatusCode.EncryptionFailedToEstablish:
		{
			Debug.LogError("Encryption wasn't established: " + statusCode + ". Going to authenticate anyways.");
			AuthenticationValues authenticationValues;
			if ((authenticationValues = this.AuthValues) == null)
			{
				authenticationValues = new AuthenticationValues
				{
					UserId = this.PlayerName
				};
			}
			AuthenticationValues authValues = authenticationValues;
			this.OpAuthenticate(this.AppId, this.AppVersion, authValues, this.CloudRegion.ToString(), this.requestLobbyStatistics);
			return;
		}
		default:
			goto IL_5FC;
		}
		if (this.Server == ServerConnection.NameServer)
		{
			this.State = ClientState.ConnectedToNameServer;
			if (!this.didAuthenticate && this.CloudRegion == CloudRegionCode.none)
			{
				this.OpGetRegions(this.AppId);
			}
		}
		if (this.Server != ServerConnection.NameServer && (this.AuthMode == AuthModeOption.AuthOnce || this.AuthMode == AuthModeOption.AuthOnceWss))
		{
			return;
		}
		if (!this.didAuthenticate && (!this.IsUsingNameServer || this.CloudRegion != CloudRegionCode.none))
		{
			this.didAuthenticate = this.CallAuthenticate();
			if (this.didAuthenticate)
			{
				this.State = ClientState.Authenticating;
			}
		}
		return;
		IL_5FC:
		Debug.LogError("Received unknown status code: " + statusCode);
	}

	// Token: 0x06003C58 RID: 15448 RVA: 0x0013018C File Offset: 0x0012E58C
	public void OnEvent(EventData photonEvent)
	{
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log(string.Format("OnEvent: {0}", photonEvent.ToString()));
		}
		int num = -1;
		PhotonPlayer photonPlayer = null;
		if (photonEvent.Parameters.ContainsKey(254))
		{
			num = (int)photonEvent[254];
			photonPlayer = this.GetPlayerWithId(num);
		}
		byte code = photonEvent.Code;
		switch (code)
		{
		case 200:
			this.ExecuteRpc(photonEvent[245] as ExitGames.Client.Photon.Hashtable, photonPlayer);
			break;
		case 201:
		case 206:
		{
			ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
			int networkTime = (int)hashtable[0];
			short correctPrefix = -1;
			byte b = 10;
			int num2 = 1;
			if (hashtable.ContainsKey(1))
			{
				correctPrefix = (short)hashtable[1];
				num2 = 2;
			}
			byte b2 = b;
			while ((int)(b2 - b) < hashtable.Count - num2)
			{
				this.OnSerializeRead(hashtable[b2] as object[], photonPlayer, networkTime, correctPrefix);
				b2 += 1;
			}
			break;
		}
		case 202:
			this.DoInstantiate((ExitGames.Client.Photon.Hashtable)photonEvent[245], photonPlayer, null);
			break;
		case 203:
			if (photonPlayer == null || !photonPlayer.IsMasterClient)
			{
				Debug.LogError("Error: Someone else(" + photonPlayer + ") then the masterserver requests a disconnect!");
			}
			else
			{
				PhotonNetwork.LeaveRoom();
			}
			break;
		case 204:
		{
			ExitGames.Client.Photon.Hashtable hashtable2 = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
			int num3 = (int)hashtable2[0];
			PhotonView photonView = null;
			if (this.photonViewList.TryGetValue(num3, out photonView))
			{
				this.RemoveInstantiatedGO(photonView.gameObject, true);
			}
			else if (this.DebugOut >= DebugLevel.ERROR)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Ev Destroy Failed. Could not find PhotonView with instantiationId ",
					num3,
					". Sent by actorNr: ",
					num
				}));
			}
			break;
		}
		default:
			switch (code)
			{
			case 223:
				if (this.AuthValues == null)
				{
					this.AuthValues = new AuthenticationValues();
				}
				this.AuthValues.Token = (photonEvent[221] as string);
				this.tokenCache = this.AuthValues.Token;
				break;
			case 224:
			{
				string[] array = photonEvent[213] as string[];
				byte[] array2 = photonEvent[212] as byte[];
				int[] array3 = photonEvent[229] as int[];
				int[] array4 = photonEvent[228] as int[];
				this.LobbyStatistics.Clear();
				for (int i = 0; i < array.Length; i++)
				{
					TypedLobbyInfo typedLobbyInfo = new TypedLobbyInfo();
					typedLobbyInfo.Name = array[i];
					typedLobbyInfo.Type = (LobbyType)array2[i];
					typedLobbyInfo.PlayerCount = array3[i];
					typedLobbyInfo.RoomCount = array4[i];
					this.LobbyStatistics.Add(typedLobbyInfo);
				}
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLobbyStatisticsUpdate, new object[0]);
				break;
			}
			default:
				switch (code)
				{
				case 251:
					if (PhotonNetwork.OnEventCall != null)
					{
						object content = photonEvent[245];
						PhotonNetwork.OnEventCall(photonEvent.Code, content, num);
					}
					else
					{
						Debug.LogWarning("Warning: Unhandled Event ErrorInfo (251). Set PhotonNetwork.OnEventCall to the method PUN should call for this event.");
					}
					goto IL_B34;
				case 253:
				{
					int num4 = (int)photonEvent[253];
					ExitGames.Client.Photon.Hashtable gameProperties = null;
					ExitGames.Client.Photon.Hashtable pActorProperties = null;
					if (num4 == 0)
					{
						gameProperties = (ExitGames.Client.Photon.Hashtable)photonEvent[251];
					}
					else
					{
						pActorProperties = (ExitGames.Client.Photon.Hashtable)photonEvent[251];
					}
					this.ReadoutProperties(gameProperties, pActorProperties, num4);
					goto IL_B34;
				}
				case 254:
					this.HandleEventLeave(num, photonEvent);
					goto IL_B34;
				case 255:
				{
					bool flag = false;
					ExitGames.Client.Photon.Hashtable properties = (ExitGames.Client.Photon.Hashtable)photonEvent[249];
					if (photonPlayer == null)
					{
						bool isLocal = this.LocalPlayer.ID == num;
						this.AddNewPlayer(num, new PhotonPlayer(isLocal, num, properties));
						this.ResetPhotonViewsOnSerialize();
					}
					else
					{
						flag = photonPlayer.IsInactive;
						photonPlayer.InternalCacheProperties(properties);
						photonPlayer.IsInactive = false;
					}
					if (num == this.LocalPlayer.ID)
					{
						int[] actorsInRoom = (int[])photonEvent[252];
						this.UpdatedActorList(actorsInRoom);
						if (this.lastJoinType == JoinType.JoinOrCreateRoom && this.LocalPlayer.ID == 1)
						{
							NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom, new object[0]);
						}
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom, new object[0]);
					}
					else
					{
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerConnected, new object[]
						{
							this.mActors[num]
						});
						if (flag)
						{
							NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerActivityChanged, new object[]
							{
								this.mActors[num]
							});
						}
					}
					goto IL_B34;
				}
				}
				if (photonEvent.Code < 200)
				{
					if (PhotonNetwork.OnEventCall != null)
					{
						object content2 = photonEvent[245];
						PhotonNetwork.OnEventCall(photonEvent.Code, content2, num);
					}
					else
					{
						Debug.LogWarning("Warning: Unhandled event " + photonEvent + ". Set PhotonNetwork.OnEventCall.");
					}
				}
				break;
			case 226:
				this.PlayersInRoomsCount = (int)photonEvent[229];
				this.PlayersOnMasterCount = (int)photonEvent[227];
				this.RoomsCount = (int)photonEvent[228];
				break;
			case 229:
			{
				ExitGames.Client.Photon.Hashtable hashtable3 = (ExitGames.Client.Photon.Hashtable)photonEvent[222];
				foreach (object obj in hashtable3.Keys)
				{
					string text = (string)obj;
					RoomInfo roomInfo = new RoomInfo(text, (ExitGames.Client.Photon.Hashtable)hashtable3[obj]);
					if (roomInfo.removedFromList)
					{
						this.mGameList.Remove(text);
					}
					else
					{
						this.mGameList[text] = roomInfo;
					}
				}
				this.mGameListCopy = new RoomInfo[this.mGameList.Count];
				this.mGameList.Values.CopyTo(this.mGameListCopy, 0);
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnReceivedRoomListUpdate, new object[0]);
				break;
			}
			case 230:
			{
				this.mGameList = new Dictionary<string, RoomInfo>();
				ExitGames.Client.Photon.Hashtable hashtable4 = (ExitGames.Client.Photon.Hashtable)photonEvent[222];
				foreach (object obj2 in hashtable4.Keys)
				{
					string text2 = (string)obj2;
					this.mGameList[text2] = new RoomInfo(text2, (ExitGames.Client.Photon.Hashtable)hashtable4[obj2]);
				}
				this.mGameListCopy = new RoomInfo[this.mGameList.Count];
				this.mGameList.Values.CopyTo(this.mGameListCopy, 0);
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnReceivedRoomListUpdate, new object[0]);
				break;
			}
			}
			break;
		case 207:
		{
			ExitGames.Client.Photon.Hashtable hashtable2 = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
			int num5 = (int)hashtable2[0];
			if (num5 >= 0)
			{
				this.DestroyPlayerObjects(num5, true);
			}
			else
			{
				if (this.DebugOut >= DebugLevel.INFO)
				{
					Debug.Log("Ev DestroyAll! By PlayerId: " + num);
				}
				this.DestroyAll(true);
			}
			break;
		}
		case 208:
		{
			ExitGames.Client.Photon.Hashtable hashtable2 = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
			int playerId = (int)hashtable2[1];
			this.SetMasterClient(playerId, false);
			break;
		}
		case 209:
		{
			int[] array5 = (int[])photonEvent.Parameters[245];
			int num6 = array5[0];
			int num7 = array5[1];
			PhotonView photonView2 = PhotonView.Find(num6);
			if (photonView2 == null)
			{
				Debug.LogWarning("Can't find PhotonView of incoming OwnershipRequest. ViewId not found: " + num6);
			}
			else
			{
				if (PhotonNetwork.logLevel == PhotonLogLevel.Informational)
				{
					Debug.Log(string.Concat(new object[]
					{
						"Ev OwnershipRequest ",
						photonView2.ownershipTransfer,
						". ActorNr: ",
						num,
						" takes from: ",
						num7,
						". local RequestedView.ownerId: ",
						photonView2.ownerId,
						" isOwnerActive: ",
						photonView2.isOwnerActive,
						". MasterClient: ",
						this.mMasterClientId,
						". This client's player: ",
						PhotonNetwork.player.ToStringFull()
					}));
				}
				switch (photonView2.ownershipTransfer)
				{
				case OwnershipOption.Fixed:
					Debug.LogWarning("Ownership mode == fixed. Ignoring request.");
					break;
				case OwnershipOption.Takeover:
					if (num7 == photonView2.ownerId || (num7 == 0 && photonView2.ownerId == this.mMasterClientId) || photonView2.ownerId == 0)
					{
						photonView2.OwnerShipWasTransfered = true;
						int ownerId = photonView2.ownerId;
						PhotonPlayer playerWithId = this.GetPlayerWithId(ownerId);
						photonView2.ownerId = num;
						if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
						{
							Debug.LogWarning(photonView2 + " ownership transfered to: " + num);
						}
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnOwnershipTransfered, new object[]
						{
							photonView2,
							photonPlayer,
							playerWithId
						});
					}
					break;
				case OwnershipOption.Request:
					if ((num7 == PhotonNetwork.player.ID || PhotonNetwork.player.IsMasterClient) && (photonView2.ownerId == PhotonNetwork.player.ID || (PhotonNetwork.player.IsMasterClient && !photonView2.isOwnerActive)))
					{
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnOwnershipRequest, new object[]
						{
							photonView2,
							photonPlayer
						});
					}
					break;
				}
			}
			break;
		}
		case 210:
		{
			int[] array6 = (int[])photonEvent.Parameters[245];
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log(string.Concat(new object[]
				{
					"Ev OwnershipTransfer. ViewID ",
					array6[0],
					" to: ",
					array6[1],
					" Time: ",
					Environment.TickCount % 1000
				}));
			}
			int viewID = array6[0];
			int num8 = array6[1];
			PhotonView photonView3 = PhotonView.Find(viewID);
			if (photonView3 != null)
			{
				int ownerId2 = photonView3.ownerId;
				photonView3.OwnerShipWasTransfered = true;
				photonView3.ownerId = num8;
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnOwnershipTransfered, new object[]
				{
					photonView3,
					PhotonPlayer.Find(num8),
					PhotonPlayer.Find(ownerId2)
				});
			}
			break;
		}
		}
		IL_B34:
		PhotonBandwidthGui.RecordEvent(photonEvent, photonPlayer);
	}

	// Token: 0x06003C59 RID: 15449 RVA: 0x00130CF0 File Offset: 0x0012F0F0
	public void OnMessage(object messages)
	{
	}

	// Token: 0x06003C5A RID: 15450 RVA: 0x00130CF4 File Offset: 0x0012F0F4
	private void SetupEncryption(Dictionary<byte, object> encryptionData)
	{
		if (this.AuthMode == AuthModeOption.Auth && this.DebugOut == DebugLevel.ERROR)
		{
			Debug.LogWarning("SetupEncryption() called but ignored. Not XB1 compiled. EncryptionData: " + encryptionData.ToStringFull());
			return;
		}
		if (this.DebugOut == DebugLevel.INFO)
		{
			Debug.Log("SetupEncryption() got called. " + encryptionData.ToStringFull());
		}
		EncryptionMode encryptionMode = (EncryptionMode)((byte)encryptionData[0]);
		if (encryptionMode != EncryptionMode.PayloadEncryption)
		{
			if (encryptionMode != EncryptionMode.DatagramEncryption)
			{
				throw new ArgumentOutOfRangeException();
			}
			byte[] encryptionSecret = (byte[])encryptionData[1];
			byte[] hmacSecret = (byte[])encryptionData[2];
			base.InitDatagramEncryption(encryptionSecret, hmacSecret);
		}
		else
		{
			byte[] secret = (byte[])encryptionData[1];
			base.InitPayloadEncryption(secret);
		}
	}

	// Token: 0x06003C5B RID: 15451 RVA: 0x00130DB8 File Offset: 0x0012F1B8
	protected internal void UpdatedActorList(int[] actorsInRoom)
	{
		foreach (int num in actorsInRoom)
		{
			if (this.LocalPlayer.ID != num && !this.mActors.ContainsKey(num))
			{
				this.AddNewPlayer(num, new PhotonPlayer(false, num, string.Empty));
			}
		}
	}

	// Token: 0x06003C5C RID: 15452 RVA: 0x00130E14 File Offset: 0x0012F214
	private void SendVacantViewIds()
	{
		Debug.Log("SendVacantViewIds()");
		List<int> list = new List<int>();
		foreach (PhotonView photonView in this.photonViewList.Values)
		{
			if (!photonView.isOwnerActive)
			{
				list.Add(photonView.viewID);
			}
		}
		Debug.Log("Sending vacant view IDs. Length: " + list.Count);
		this.OpRaiseEvent(211, list.ToArray(), true, null);
	}

	// Token: 0x06003C5D RID: 15453 RVA: 0x00130EC4 File Offset: 0x0012F2C4
	public static void SendMonoMessage(PhotonNetworkingMessage methodString, params object[] parameters)
	{
		HashSet<GameObject> hashSet;
		if (PhotonNetwork.SendMonoMessageTargets != null)
		{
			hashSet = PhotonNetwork.SendMonoMessageTargets;
		}
		else
		{
			Debug.LogError("PhotonNetwork.SendMonoMessageTargets was not set");
			hashSet = PhotonNetwork.FindGameObjectsWithComponent(PhotonNetwork.SendMonoMessageTargetType);
		}
		string text = methodString.ToString();
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log("SendMonoMessage(" + text + ")");
		}
		object value = (parameters == null || parameters.Length != 1) ? parameters : parameters[0];
		foreach (GameObject gameObject in hashSet)
		{
			if (gameObject != null)
			{
				gameObject.SendMessage(text, value, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	// Token: 0x06003C5E RID: 15454 RVA: 0x00130F9C File Offset: 0x0012F39C
	protected internal void ExecuteRpc(ExitGames.Client.Photon.Hashtable rpcData, PhotonPlayer sender)
	{
		if (rpcData == null || !rpcData.ContainsKey(0))
		{
			Debug.LogError("Malformed RPC; this should never occur. Content: " + SupportClass.DictionaryToString(rpcData));
			return;
		}
		int num = (int)rpcData[0];
		int num2 = 0;
		if (rpcData.ContainsKey(1))
		{
			num2 = (int)((short)rpcData[1]);
		}
		string text;
		if (rpcData.ContainsKey(5))
		{
			int num3 = (int)((byte)rpcData[5]);
			if (num3 > PhotonNetwork.PhotonServerSettings.RpcList.Count - 1)
			{
				Debug.LogError("Could not find RPC with index: " + num3 + ". Going to ignore! Check PhotonServerSettings.RpcList");
				return;
			}
			text = PhotonNetwork.PhotonServerSettings.RpcList[num3];
		}
		else
		{
			text = (string)rpcData[3];
		}
		object[] array = null;
		if (rpcData.ContainsKey(4))
		{
			array = new List<object>((object[])rpcData[4])
			{
				sender
			}.ToArray();
		}
		if (array == null)
		{
			array = new object[]
			{
				sender
			};
		}
		PhotonView photonView = this.GetPhotonView(num);
		if (photonView == null)
		{
			int num4 = num / PhotonNetwork.MAX_VIEW_IDS;
			bool flag = num4 == this.LocalPlayer.ID;
			bool flag2 = num4 == sender.ID;
			if (flag)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					"Received RPC \"",
					text,
					"\" for viewID ",
					num,
					" but this PhotonView does not exist! View was/is ours.",
					(!flag2) ? " Remote called." : " Owner called.",
					" By: ",
					sender.ID
				}));
			}
			else
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					"Received RPC \"",
					text,
					"\" for viewID ",
					num,
					" but this PhotonView does not exist! Was remote PV.",
					(!flag2) ? " Remote called." : " Owner called.",
					" By: ",
					sender.ID,
					" Maybe GO was destroyed but RPC not cleaned up."
				}));
			}
			return;
		}
		if (photonView.prefix != num2)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Received RPC \"",
				text,
				"\" on viewID ",
				num,
				" with a prefix of ",
				num2,
				", our prefix is ",
				photonView.prefix,
				". The RPC has been ignored."
			}));
			return;
		}
		if (string.IsNullOrEmpty(text))
		{
			Debug.LogError("Malformed RPC; this should never occur. Content: " + SupportClass.DictionaryToString(rpcData));
			return;
		}
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
		{
			Debug.Log("Received RPC: " + text);
		}
		if (photonView.group != 0 && !this.allowedReceivingGroups.Contains(photonView.group))
		{
			return;
		}
		Type[] array2 = new Type[0];
		if (array.Length > 0)
		{
			array2 = new Type[array.Length];
			int num5 = 0;
			foreach (object obj in array)
			{
				if (obj == null)
				{
					array2[num5] = null;
				}
				else
				{
					array2[num5] = obj.GetType();
				}
				num5++;
			}
		}
		int num6 = 0;
		int num7 = 0;
		if (!PhotonNetwork.UseRpcMonoBehaviourCache || photonView.RpcMonoBehaviours == null || photonView.RpcMonoBehaviours.Length == 0)
		{
			photonView.RefreshRpcMonoBehaviourCache();
		}
		for (int j = 0; j < photonView.RpcMonoBehaviours.Length; j++)
		{
			MonoBehaviour monoBehaviour = photonView.RpcMonoBehaviours[j];
			if (monoBehaviour == null)
			{
				Debug.LogError("ERROR You have missing MonoBehaviours on your gameobjects!");
			}
			else
			{
				Type type = monoBehaviour.GetType();
				List<MethodInfo> list = null;
				if (!this.monoRPCMethodsCache.TryGetValue(type, out list))
				{
					List<MethodInfo> methods = SupportClass.GetMethods(type, typeof(PunRPC));
					this.monoRPCMethodsCache[type] = methods;
					list = methods;
				}
				if (list != null)
				{
					for (int k = 0; k < list.Count; k++)
					{
						MethodInfo methodInfo = list[k];
						if (methodInfo.Name.Equals(text))
						{
							num7++;
							ParameterInfo[] cachedParemeters = methodInfo.GetCachedParemeters();
							if (cachedParemeters.Length == array2.Length)
							{
								if (this.CheckTypeMatch(cachedParemeters, array2))
								{
									num6++;
									object obj2 = methodInfo.Invoke(monoBehaviour, array);
									if (PhotonNetwork.StartRpcsAsCoroutine && methodInfo.ReturnType == typeof(IEnumerator))
									{
										monoBehaviour.StartCoroutine((IEnumerator)obj2);
									}
								}
							}
							else if (cachedParemeters.Length - 1 == array2.Length)
							{
								if (this.CheckTypeMatch(cachedParemeters, array2) && cachedParemeters[cachedParemeters.Length - 1].ParameterType == typeof(PhotonMessageInfo))
								{
									num6++;
									int timestamp = (int)rpcData[2];
									object[] array3 = new object[array.Length + 1];
									array.CopyTo(array3, 0);
									array3[array3.Length - 1] = new PhotonMessageInfo(sender, timestamp, photonView);
									object obj3 = methodInfo.Invoke(monoBehaviour, array3);
									if (PhotonNetwork.StartRpcsAsCoroutine && methodInfo.ReturnType == typeof(IEnumerator))
									{
										monoBehaviour.StartCoroutine((IEnumerator)obj3);
									}
								}
							}
							else if (cachedParemeters.Length == 1 && cachedParemeters[0].ParameterType.IsArray)
							{
								num6++;
								object obj4 = methodInfo.Invoke(monoBehaviour, new object[]
								{
									array
								});
								if (PhotonNetwork.StartRpcsAsCoroutine && methodInfo.ReturnType == typeof(IEnumerator))
								{
									monoBehaviour.StartCoroutine((IEnumerator)obj4);
								}
							}
						}
					}
				}
			}
		}
		if (num6 != 1)
		{
			string text2 = string.Empty;
			foreach (Type type2 in array2)
			{
				if (text2 != string.Empty)
				{
					text2 += ", ";
				}
				if (type2 == null)
				{
					text2 += "null";
				}
				else
				{
					text2 += type2.Name;
				}
			}
			if (num6 == 0)
			{
				if (num7 == 0)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"PhotonView with ID ",
						num,
						" has no method \"",
						text,
						"\" marked with the [PunRPC](C#) or @PunRPC(JS) property! Args: ",
						text2
					}));
				}
				else
				{
					Debug.LogError(string.Concat(new object[]
					{
						"PhotonView with ID ",
						num,
						" has no method \"",
						text,
						"\" that takes ",
						array2.Length,
						" argument(s): ",
						text2
					}));
				}
			}
			else
			{
				Debug.LogError(string.Concat(new object[]
				{
					"PhotonView with ID ",
					num,
					" has ",
					num6,
					" methods \"",
					text,
					"\" that takes ",
					array2.Length,
					" argument(s): ",
					text2,
					". Should be just one?"
				}));
			}
		}
	}

	// Token: 0x06003C5F RID: 15455 RVA: 0x0013174C File Offset: 0x0012FB4C
	private bool CheckTypeMatch(ParameterInfo[] methodParameters, Type[] callParameterTypes)
	{
		if (methodParameters.Length < callParameterTypes.Length)
		{
			return false;
		}
		for (int i = 0; i < callParameterTypes.Length; i++)
		{
			Type parameterType = methodParameters[i].ParameterType;
			if (callParameterTypes[i] != null && !parameterType.IsAssignableFrom(callParameterTypes[i]) && (!parameterType.IsEnum || !Enum.GetUnderlyingType(parameterType).IsAssignableFrom(callParameterTypes[i])))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003C60 RID: 15456 RVA: 0x001317BC File Offset: 0x0012FBBC
	internal ExitGames.Client.Photon.Hashtable SendInstantiate(string prefabName, Vector3 position, Quaternion rotation, int group, int[] viewIDs, object[] data, bool isGlobalObject)
	{
		int num = viewIDs[0];
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[0] = prefabName;
		if (position != Vector3.zero)
		{
			hashtable[1] = position;
		}
		if (rotation != Quaternion.identity)
		{
			hashtable[2] = rotation;
		}
		if (group != 0)
		{
			hashtable[3] = group;
		}
		if (viewIDs.Length > 1)
		{
			hashtable[4] = viewIDs;
		}
		if (data != null)
		{
			hashtable[5] = data;
		}
		if (this.currentLevelPrefix > 0)
		{
			hashtable[8] = this.currentLevelPrefix;
		}
		hashtable[6] = PhotonNetwork.ServerTimestamp;
		hashtable[7] = num;
		this.OpRaiseEvent(202, hashtable, true, new RaiseEventOptions
		{
			CachingOption = ((!isGlobalObject) ? EventCaching.AddToRoomCache : EventCaching.AddToRoomCacheGlobal)
		});
		return hashtable;
	}

	// Token: 0x06003C61 RID: 15457 RVA: 0x001318E4 File Offset: 0x0012FCE4
	internal GameObject DoInstantiate(ExitGames.Client.Photon.Hashtable evData, PhotonPlayer photonPlayer, GameObject resourceGameObject)
	{
		string text = (string)evData[0];
		int timestamp = (int)evData[6];
		int num = (int)evData[7];
		Vector3 position;
		if (evData.ContainsKey(1))
		{
			position = (Vector3)evData[1];
		}
		else
		{
			position = Vector3.zero;
		}
		Quaternion rotation = Quaternion.identity;
		if (evData.ContainsKey(2))
		{
			rotation = (Quaternion)evData[2];
		}
		byte b = 0;
		if (evData.ContainsKey(3))
		{
			b = (byte)evData[3];
		}
		short prefix = 0;
		if (evData.ContainsKey(8))
		{
			prefix = (short)evData[8];
		}
		int[] array;
		if (evData.ContainsKey(4))
		{
			array = (int[])evData[4];
		}
		else
		{
			array = new int[]
			{
				num
			};
		}
		object[] array2;
		if (evData.ContainsKey(5))
		{
			array2 = (object[])evData[5];
		}
		else
		{
			array2 = null;
		}
		if (b != 0 && !this.allowedReceivingGroups.Contains(b))
		{
			return null;
		}
		if (this.ObjectPool != null)
		{
			GameObject gameObject = this.ObjectPool.Instantiate(text, position, rotation);
			PhotonView[] photonViewsInChildren = gameObject.GetPhotonViewsInChildren();
			if (photonViewsInChildren.Length != array.Length)
			{
				throw new Exception("Error in Instantiation! The resource's PhotonView count is not the same as in incoming data.");
			}
			for (int i = 0; i < photonViewsInChildren.Length; i++)
			{
				photonViewsInChildren[i].didAwake = false;
				photonViewsInChildren[i].viewID = 0;
				photonViewsInChildren[i].prefix = (int)prefix;
				photonViewsInChildren[i].instantiationId = num;
				photonViewsInChildren[i].isRuntimeInstantiated = true;
				photonViewsInChildren[i].instantiationDataField = array2;
				photonViewsInChildren[i].didAwake = true;
				photonViewsInChildren[i].viewID = array[i];
			}
			gameObject.SendMessage(NetworkingPeer.OnPhotonInstantiateString, new PhotonMessageInfo(photonPlayer, timestamp, null), SendMessageOptions.DontRequireReceiver);
			return gameObject;
		}
		else
		{
			if (resourceGameObject == null)
			{
				if (!NetworkingPeer.UsePrefabCache || !NetworkingPeer.PrefabCache.TryGetValue(text, out resourceGameObject))
				{
					resourceGameObject = (GameObject)Resources.Load(text, typeof(GameObject));
					if (NetworkingPeer.UsePrefabCache)
					{
						NetworkingPeer.PrefabCache.Add(text, resourceGameObject);
					}
				}
				if (resourceGameObject == null)
				{
					Debug.LogError("PhotonNetwork error: Could not Instantiate the prefab [" + text + "]. Please verify you have this gameobject in a Resources folder.");
					return null;
				}
			}
			PhotonView[] photonViewsInChildren2 = resourceGameObject.GetPhotonViewsInChildren();
			if (photonViewsInChildren2.Length != array.Length)
			{
				throw new Exception("Error in Instantiation! The resource's PhotonView count is not the same as in incoming data.");
			}
			for (int j = 0; j < array.Length; j++)
			{
				photonViewsInChildren2[j].viewID = array[j];
				photonViewsInChildren2[j].prefix = (int)prefix;
				photonViewsInChildren2[j].instantiationId = num;
				photonViewsInChildren2[j].isRuntimeInstantiated = true;
			}
			this.StoreInstantiationData(num, array2);
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(resourceGameObject, position, rotation);
			for (int k = 0; k < array.Length; k++)
			{
				photonViewsInChildren2[k].viewID = 0;
				photonViewsInChildren2[k].prefix = -1;
				photonViewsInChildren2[k].prefixBackup = -1;
				photonViewsInChildren2[k].instantiationId = -1;
				photonViewsInChildren2[k].isRuntimeInstantiated = false;
			}
			this.RemoveInstantiationData(num);
			gameObject2.SendMessage(NetworkingPeer.OnPhotonInstantiateString, new PhotonMessageInfo(photonPlayer, timestamp, null), SendMessageOptions.DontRequireReceiver);
			return gameObject2;
		}
	}

	// Token: 0x06003C62 RID: 15458 RVA: 0x00131C84 File Offset: 0x00130084
	private void StoreInstantiationData(int instantiationId, object[] instantiationData)
	{
		this.tempInstantiationData[instantiationId] = instantiationData;
	}

	// Token: 0x06003C63 RID: 15459 RVA: 0x00131C94 File Offset: 0x00130094
	public object[] FetchInstantiationData(int instantiationId)
	{
		object[] result = null;
		if (instantiationId == 0)
		{
			return null;
		}
		this.tempInstantiationData.TryGetValue(instantiationId, out result);
		return result;
	}

	// Token: 0x06003C64 RID: 15460 RVA: 0x00131CBB File Offset: 0x001300BB
	private void RemoveInstantiationData(int instantiationId)
	{
		this.tempInstantiationData.Remove(instantiationId);
	}

	// Token: 0x06003C65 RID: 15461 RVA: 0x00131CCC File Offset: 0x001300CC
	public void DestroyPlayerObjects(int playerId, bool localOnly)
	{
		if (playerId <= 0)
		{
			Debug.LogError("Failed to Destroy objects of playerId: " + playerId);
			return;
		}
		if (!localOnly)
		{
			this.OpRemoveFromServerInstantiationsOfPlayer(playerId);
			this.OpCleanRpcBuffer(playerId);
			this.SendDestroyOfPlayer(playerId);
		}
		HashSet<GameObject> hashSet = new HashSet<GameObject>();
		foreach (PhotonView photonView in this.photonViewList.Values)
		{
			if (photonView != null && !photonView.isProtectedFromCleanup && photonView.CreatorActorNr == playerId)
			{
				hashSet.Add(photonView.gameObject);
			}
		}
		foreach (GameObject go in hashSet)
		{
			this.RemoveInstantiatedGO(go, true);
		}
		foreach (PhotonView photonView2 in this.photonViewList.Values)
		{
			if (photonView2.ownerId == playerId)
			{
				photonView2.ownerId = photonView2.CreatorActorNr;
			}
		}
	}

	// Token: 0x06003C66 RID: 15462 RVA: 0x00131E44 File Offset: 0x00130244
	public void DestroyAll(bool localOnly)
	{
		if (!localOnly)
		{
			this.OpRemoveCompleteCache();
			this.SendDestroyOfAll();
		}
		this.LocalCleanupAnythingInstantiated(true);
	}

	// Token: 0x06003C67 RID: 15463 RVA: 0x00131E60 File Offset: 0x00130260
	protected internal void RemoveInstantiatedGO(GameObject go, bool localOnly)
	{
		if (go == null)
		{
			Debug.LogError("Failed to 'network-remove' GameObject because it's null.");
			return;
		}
		PhotonView[] componentsInChildren = go.GetComponentsInChildren<PhotonView>(true);
		if (componentsInChildren == null || componentsInChildren.Length <= 0)
		{
			Debug.LogError("Failed to 'network-remove' GameObject because has no PhotonView components: " + go);
			return;
		}
		PhotonView photonView = componentsInChildren[0];
		int creatorActorNr = photonView.CreatorActorNr;
		int instantiationId = photonView.instantiationId;
		if (!localOnly)
		{
			if (!photonView.isMine)
			{
				Debug.LogError("Failed to 'network-remove' GameObject. Client is neither owner nor masterClient taking over for owner who left: " + photonView);
				return;
			}
			if (instantiationId < 1)
			{
				Debug.LogError("Failed to 'network-remove' GameObject because it is missing a valid InstantiationId on view: " + photonView + ". Not Destroying GameObject or PhotonViews!");
				return;
			}
		}
		if (!localOnly)
		{
			this.ServerCleanInstantiateAndDestroy(instantiationId, creatorActorNr, photonView.isRuntimeInstantiated);
		}
		for (int i = componentsInChildren.Length - 1; i >= 0; i--)
		{
			PhotonView photonView2 = componentsInChildren[i];
			if (!(photonView2 == null))
			{
				if (photonView2.instantiationId >= 1)
				{
					this.LocalCleanPhotonView(photonView2);
				}
				if (!localOnly)
				{
					this.OpCleanRpcBuffer(photonView2);
				}
			}
		}
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
		{
			Debug.Log("Network destroy Instantiated GO: " + go.name);
		}
		if (this.ObjectPool != null)
		{
			PhotonView[] photonViewsInChildren = go.GetPhotonViewsInChildren();
			for (int j = 0; j < photonViewsInChildren.Length; j++)
			{
				photonViewsInChildren[j].viewID = 0;
			}
			this.ObjectPool.Destroy(go);
		}
		else
		{
			UnityEngine.Object.Destroy(go);
		}
	}

	// Token: 0x06003C68 RID: 15464 RVA: 0x00131FD8 File Offset: 0x001303D8
	private void ServerCleanInstantiateAndDestroy(int instantiateId, int creatorId, bool isRuntimeInstantiated)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[7] = instantiateId;
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache,
			TargetActors = new int[]
			{
				creatorId
			}
		};
		this.OpRaiseEvent(202, hashtable, true, raiseEventOptions);
		ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
		hashtable2[0] = instantiateId;
		raiseEventOptions = null;
		if (!isRuntimeInstantiated)
		{
			raiseEventOptions = new RaiseEventOptions();
			raiseEventOptions.CachingOption = EventCaching.AddToRoomCacheGlobal;
			Debug.Log("Destroying GO as global. ID: " + instantiateId);
		}
		this.OpRaiseEvent(204, hashtable2, true, raiseEventOptions);
	}

	// Token: 0x06003C69 RID: 15465 RVA: 0x0013207C File Offset: 0x0013047C
	private void SendDestroyOfPlayer(int actorNr)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[0] = actorNr;
		this.OpRaiseEvent(207, hashtable, true, null);
	}

	// Token: 0x06003C6A RID: 15466 RVA: 0x001320B0 File Offset: 0x001304B0
	private void SendDestroyOfAll()
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[0] = -1;
		this.OpRaiseEvent(207, hashtable, true, null);
	}

	// Token: 0x06003C6B RID: 15467 RVA: 0x001320E4 File Offset: 0x001304E4
	private void OpRemoveFromServerInstantiationsOfPlayer(int actorNr)
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache,
			TargetActors = new int[]
			{
				actorNr
			}
		};
		this.OpRaiseEvent(202, null, true, raiseEventOptions);
	}

	// Token: 0x06003C6C RID: 15468 RVA: 0x00132120 File Offset: 0x00130520
	protected internal void RequestOwnership(int viewID, int fromOwner)
	{
		Debug.Log(string.Concat(new object[]
		{
			"RequestOwnership(): ",
			viewID,
			" from: ",
			fromOwner,
			" Time: ",
			Environment.TickCount % 1000
		}));
		this.OpRaiseEvent(209, new int[]
		{
			viewID,
			fromOwner
		}, true, new RaiseEventOptions
		{
			Receivers = ReceiverGroup.All
		});
	}

	// Token: 0x06003C6D RID: 15469 RVA: 0x001321A4 File Offset: 0x001305A4
	protected internal void TransferOwnership(int viewID, int playerID)
	{
		Debug.Log(string.Concat(new object[]
		{
			"TransferOwnership() view ",
			viewID,
			" to: ",
			playerID,
			" Time: ",
			Environment.TickCount % 1000
		}));
		this.OpRaiseEvent(210, new int[]
		{
			viewID,
			playerID
		}, true, new RaiseEventOptions
		{
			Receivers = ReceiverGroup.All
		});
	}

	// Token: 0x06003C6E RID: 15470 RVA: 0x00132227 File Offset: 0x00130627
	public bool LocalCleanPhotonView(PhotonView view)
	{
		view.removedFromLocalViewList = true;
		return this.photonViewList.Remove(view.viewID);
	}

	// Token: 0x06003C6F RID: 15471 RVA: 0x00132244 File Offset: 0x00130644
	public PhotonView GetPhotonView(int viewID)
	{
		PhotonView result = null;
		this.photonViewList.TryGetValue(viewID, out result);
		return result;
	}

	// Token: 0x06003C70 RID: 15472 RVA: 0x00132264 File Offset: 0x00130664
	public void RegisterPhotonView(PhotonView netView)
	{
		if (!Application.isPlaying)
		{
			this.photonViewList = new Dictionary<int, PhotonView>();
			return;
		}
		if (netView.viewID == 0)
		{
			Debug.Log("PhotonView register is ignored, because viewID is 0. No id assigned yet to: " + netView);
			return;
		}
		PhotonView photonView = null;
		bool flag = this.photonViewList.TryGetValue(netView.viewID, out photonView);
		if (flag)
		{
			if (!(netView != photonView))
			{
				return;
			}
			Debug.LogError(string.Format("PhotonView ID duplicate found: {0}. New: {1} old: {2}. Maybe one wasn't destroyed on scene load?! Check for 'DontDestroyOnLoad'. Destroying old entry, adding new.", netView.viewID, netView, photonView));
			this.RemoveInstantiatedGO(photonView.gameObject, true);
		}
		this.photonViewList.Add(netView.viewID, netView);
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
		{
			Debug.Log("Registered PhotonView: " + netView.viewID);
		}
	}

	// Token: 0x06003C71 RID: 15473 RVA: 0x00132334 File Offset: 0x00130734
	public void OpCleanRpcBuffer(int actorNumber)
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache,
			TargetActors = new int[]
			{
				actorNumber
			}
		};
		this.OpRaiseEvent(200, null, true, raiseEventOptions);
	}

	// Token: 0x06003C72 RID: 15474 RVA: 0x00132370 File Offset: 0x00130770
	public void OpRemoveCompleteCacheOfPlayer(int actorNumber)
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache,
			TargetActors = new int[]
			{
				actorNumber
			}
		};
		this.OpRaiseEvent(0, null, true, raiseEventOptions);
	}

	// Token: 0x06003C73 RID: 15475 RVA: 0x001323A8 File Offset: 0x001307A8
	public void OpRemoveCompleteCache()
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache,
			Receivers = ReceiverGroup.MasterClient
		};
		this.OpRaiseEvent(0, null, true, raiseEventOptions);
	}

	// Token: 0x06003C74 RID: 15476 RVA: 0x001323D8 File Offset: 0x001307D8
	private void RemoveCacheOfLeftPlayers()
	{
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary[244] = 0;
		dictionary[247] = 7;
		this.OpCustom(253, dictionary, true, 0);
	}

	// Token: 0x06003C75 RID: 15477 RVA: 0x0013241C File Offset: 0x0013081C
	public void CleanRpcBufferIfMine(PhotonView view)
	{
		if (view.ownerId != this.LocalPlayer.ID && !this.LocalPlayer.IsMasterClient)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Cannot remove cached RPCs on a PhotonView thats not ours! ",
				view.owner,
				" scene: ",
				view.isSceneView
			}));
			return;
		}
		this.OpCleanRpcBuffer(view);
	}

	// Token: 0x06003C76 RID: 15478 RVA: 0x00132490 File Offset: 0x00130890
	public void OpCleanRpcBuffer(PhotonView view)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[0] = view.viewID;
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache
		};
		this.OpRaiseEvent(200, hashtable, true, raiseEventOptions);
	}

	// Token: 0x06003C77 RID: 15479 RVA: 0x001324D8 File Offset: 0x001308D8
	public void RemoveRPCsInGroup(int group)
	{
		foreach (PhotonView photonView in this.photonViewList.Values)
		{
			if ((int)photonView.group == group)
			{
				this.CleanRpcBufferIfMine(photonView);
			}
		}
	}

	// Token: 0x06003C78 RID: 15480 RVA: 0x00132548 File Offset: 0x00130948
	public void SetLevelPrefix(short prefix)
	{
		this.currentLevelPrefix = prefix;
	}

	// Token: 0x06003C79 RID: 15481 RVA: 0x00132554 File Offset: 0x00130954
	internal void RPC(PhotonView view, string methodName, PhotonTargets target, PhotonPlayer player, bool encrypt, params object[] parameters)
	{
		if (this.blockSendingGroups.Contains(view.group))
		{
			return;
		}
		if (view.viewID < 1)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Illegal view ID:",
				view.viewID,
				" method: ",
				methodName,
				" GO:",
				view.gameObject.name
			}));
		}
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Sending RPC \"",
				methodName,
				"\" to target: ",
				target,
				" or player:",
				player,
				"."
			}));
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[0] = view.viewID;
		if (view.prefix > 0)
		{
			hashtable[1] = (short)view.prefix;
		}
		hashtable[2] = PhotonNetwork.ServerTimestamp;
		int num = 0;
		if (this.rpcShortcuts.TryGetValue(methodName, out num))
		{
			hashtable[5] = (byte)num;
		}
		else
		{
			hashtable[3] = methodName;
		}
		if (parameters != null && parameters.Length > 0)
		{
			hashtable[4] = parameters;
		}
		if (player != null)
		{
			if (this.LocalPlayer.ID == player.ID)
			{
				this.ExecuteRpc(hashtable, player);
			}
			else
			{
				RaiseEventOptions raiseEventOptions = new RaiseEventOptions
				{
					TargetActors = new int[]
					{
						player.ID
					},
					Encrypt = encrypt
				};
				this.OpRaiseEvent(200, hashtable, true, raiseEventOptions);
			}
			return;
		}
		if (target == PhotonTargets.All)
		{
			RaiseEventOptions raiseEventOptions2 = new RaiseEventOptions
			{
				InterestGroup = view.group,
				Encrypt = encrypt
			};
			this.OpRaiseEvent(200, hashtable, true, raiseEventOptions2);
			this.ExecuteRpc(hashtable, this.LocalPlayer);
		}
		else if (target == PhotonTargets.Others)
		{
			RaiseEventOptions raiseEventOptions3 = new RaiseEventOptions
			{
				InterestGroup = view.group,
				Encrypt = encrypt
			};
			this.OpRaiseEvent(200, hashtable, true, raiseEventOptions3);
		}
		else if (target == PhotonTargets.AllBuffered)
		{
			RaiseEventOptions raiseEventOptions4 = new RaiseEventOptions
			{
				CachingOption = EventCaching.AddToRoomCache,
				Encrypt = encrypt
			};
			this.OpRaiseEvent(200, hashtable, true, raiseEventOptions4);
			this.ExecuteRpc(hashtable, this.LocalPlayer);
		}
		else if (target == PhotonTargets.OthersBuffered)
		{
			RaiseEventOptions raiseEventOptions5 = new RaiseEventOptions
			{
				CachingOption = EventCaching.AddToRoomCache,
				Encrypt = encrypt
			};
			this.OpRaiseEvent(200, hashtable, true, raiseEventOptions5);
		}
		else if (target == PhotonTargets.MasterClient)
		{
			if (this.mMasterClientId == this.LocalPlayer.ID)
			{
				this.ExecuteRpc(hashtable, this.LocalPlayer);
			}
			else
			{
				RaiseEventOptions raiseEventOptions6 = new RaiseEventOptions
				{
					Receivers = ReceiverGroup.MasterClient,
					Encrypt = encrypt
				};
				this.OpRaiseEvent(200, hashtable, true, raiseEventOptions6);
			}
		}
		else if (target == PhotonTargets.AllViaServer)
		{
			RaiseEventOptions raiseEventOptions7 = new RaiseEventOptions
			{
				InterestGroup = view.group,
				Receivers = ReceiverGroup.All,
				Encrypt = encrypt
			};
			this.OpRaiseEvent(200, hashtable, true, raiseEventOptions7);
			if (PhotonNetwork.offlineMode)
			{
				this.ExecuteRpc(hashtable, this.LocalPlayer);
			}
		}
		else if (target == PhotonTargets.AllBufferedViaServer)
		{
			RaiseEventOptions raiseEventOptions8 = new RaiseEventOptions
			{
				InterestGroup = view.group,
				Receivers = ReceiverGroup.All,
				CachingOption = EventCaching.AddToRoomCache,
				Encrypt = encrypt
			};
			this.OpRaiseEvent(200, hashtable, true, raiseEventOptions8);
			if (PhotonNetwork.offlineMode)
			{
				this.ExecuteRpc(hashtable, this.LocalPlayer);
			}
		}
		else
		{
			Debug.LogError("Unsupported target enum: " + target);
		}
	}

	// Token: 0x06003C7A RID: 15482 RVA: 0x00132944 File Offset: 0x00130D44
	public void SetInterestGroups(byte[] disableGroups, byte[] enableGroups)
	{
		if (disableGroups != null)
		{
			if (disableGroups.Length == 0)
			{
				this.allowedReceivingGroups.Clear();
			}
			else
			{
				foreach (byte b in disableGroups)
				{
					if (b <= 0)
					{
						Debug.LogError("Error: PhotonNetwork.SetInterestGroups was called with an illegal group number: " + b + ". The group number should be at least 1.");
					}
					else if (this.allowedReceivingGroups.Contains(b))
					{
						this.allowedReceivingGroups.Remove(b);
					}
				}
			}
		}
		if (enableGroups != null)
		{
			if (enableGroups.Length == 0)
			{
				for (byte b2 = 0; b2 <= 255; b2 += 1)
				{
					this.allowedReceivingGroups.Add(b2);
				}
			}
			else
			{
				foreach (byte b3 in enableGroups)
				{
					if (b3 <= 0)
					{
						Debug.LogError("Error: PhotonNetwork.SetInterestGroups was called with an illegal group number: " + b3 + ". The group number should be at least 1.");
					}
					else
					{
						this.allowedReceivingGroups.Add(b3);
					}
				}
			}
		}
		this.OpChangeGroups(disableGroups, enableGroups);
	}

	// Token: 0x06003C7B RID: 15483 RVA: 0x00132A5A File Offset: 0x00130E5A
	public void SetSendingEnabled(byte group, bool enabled)
	{
		if (!enabled)
		{
			this.blockSendingGroups.Add(group);
		}
		else
		{
			this.blockSendingGroups.Remove(group);
		}
	}

	// Token: 0x06003C7C RID: 15484 RVA: 0x00132A84 File Offset: 0x00130E84
	public void SetSendingEnabled(byte[] disableGroups, byte[] enableGroups)
	{
		if (disableGroups != null)
		{
			foreach (byte item in disableGroups)
			{
				this.blockSendingGroups.Add(item);
			}
		}
		if (enableGroups != null)
		{
			foreach (byte item2 in enableGroups)
			{
				this.blockSendingGroups.Remove(item2);
			}
		}
	}

	// Token: 0x06003C7D RID: 15485 RVA: 0x00132AE8 File Offset: 0x00130EE8
	public void NewSceneLoaded()
	{
		if (this.loadingLevelAndPausedNetwork)
		{
			this.loadingLevelAndPausedNetwork = false;
			PhotonNetwork.isMessageQueueRunning = true;
		}
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, PhotonView> keyValuePair in this.photonViewList)
		{
			PhotonView value = keyValuePair.Value;
			if (value == null)
			{
				list.Add(keyValuePair.Key);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			int key = list[i];
			this.photonViewList.Remove(key);
		}
		if (list.Count > 0 && PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log("New level loaded. Removed " + list.Count + " scene view IDs from last level.");
		}
	}

	// Token: 0x06003C7E RID: 15486 RVA: 0x00132BE8 File Offset: 0x00130FE8
	public void RunViewUpdate()
	{
		if (!PhotonNetwork.connected || PhotonNetwork.offlineMode || this.mActors == null)
		{
			return;
		}
		if (this.mActors.Count <= 1)
		{
			return;
		}
		int num = 0;
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
		foreach (KeyValuePair<int, PhotonView> keyValuePair in this.photonViewList)
		{
			PhotonView value = keyValuePair.Value;
			if (value.synchronization != ViewSynchronization.Off && value.isMine && value.gameObject.activeInHierarchy)
			{
				if (!this.blockSendingGroups.Contains(value.group))
				{
					object[] array = this.OnSerializeWrite(value);
					if (array != null)
					{
						if (value.synchronization == ViewSynchronization.ReliableDeltaCompressed || value.mixedModeIsReliable)
						{
							ExitGames.Client.Photon.Hashtable hashtable = null;
							if (!this.dataPerGroupReliable.TryGetValue((int)value.group, out hashtable))
							{
								hashtable = new ExitGames.Client.Photon.Hashtable(NetworkingPeer.ObjectsInOneUpdate);
								this.dataPerGroupReliable[(int)value.group] = hashtable;
							}
							hashtable.Add((byte)(hashtable.Count + 10), array);
							num++;
							if (hashtable.Count >= NetworkingPeer.ObjectsInOneUpdate)
							{
								num -= hashtable.Count;
								raiseEventOptions.InterestGroup = value.group;
								hashtable[0] = PhotonNetwork.ServerTimestamp;
								if (this.currentLevelPrefix >= 0)
								{
									hashtable[1] = this.currentLevelPrefix;
								}
								this.OpRaiseEvent(206, hashtable, true, raiseEventOptions);
								hashtable.Clear();
							}
						}
						else
						{
							ExitGames.Client.Photon.Hashtable hashtable2 = null;
							if (!this.dataPerGroupUnreliable.TryGetValue((int)value.group, out hashtable2))
							{
								hashtable2 = new ExitGames.Client.Photon.Hashtable(NetworkingPeer.ObjectsInOneUpdate);
								this.dataPerGroupUnreliable[(int)value.group] = hashtable2;
							}
							hashtable2.Add((byte)(hashtable2.Count + 10), array);
							num++;
							if (hashtable2.Count >= NetworkingPeer.ObjectsInOneUpdate)
							{
								num -= hashtable2.Count;
								raiseEventOptions.InterestGroup = value.group;
								hashtable2[0] = PhotonNetwork.ServerTimestamp;
								if (this.currentLevelPrefix >= 0)
								{
									hashtable2[1] = this.currentLevelPrefix;
								}
								this.OpRaiseEvent(201, hashtable2, false, raiseEventOptions);
								hashtable2.Clear();
							}
						}
					}
				}
			}
		}
		if (num == 0)
		{
			return;
		}
		foreach (int num2 in this.dataPerGroupReliable.Keys)
		{
			raiseEventOptions.InterestGroup = (byte)num2;
			ExitGames.Client.Photon.Hashtable hashtable3 = this.dataPerGroupReliable[num2];
			if (hashtable3.Count != 0)
			{
				hashtable3[0] = PhotonNetwork.ServerTimestamp;
				if (this.currentLevelPrefix >= 0)
				{
					hashtable3[1] = this.currentLevelPrefix;
				}
				this.OpRaiseEvent(206, hashtable3, true, raiseEventOptions);
				hashtable3.Clear();
			}
		}
		foreach (int num3 in this.dataPerGroupUnreliable.Keys)
		{
			raiseEventOptions.InterestGroup = (byte)num3;
			ExitGames.Client.Photon.Hashtable hashtable4 = this.dataPerGroupUnreliable[num3];
			if (hashtable4.Count != 0)
			{
				hashtable4[0] = PhotonNetwork.ServerTimestamp;
				if (this.currentLevelPrefix >= 0)
				{
					hashtable4[1] = this.currentLevelPrefix;
				}
				this.OpRaiseEvent(201, hashtable4, false, raiseEventOptions);
				hashtable4.Clear();
			}
		}
	}

	// Token: 0x06003C7F RID: 15487 RVA: 0x00133014 File Offset: 0x00131414
	private object[] OnSerializeWrite(PhotonView view)
	{
		if (view.synchronization == ViewSynchronization.Off)
		{
			return null;
		}
		PhotonMessageInfo info = new PhotonMessageInfo(this.LocalPlayer, PhotonNetwork.ServerTimestamp, view);
		this.pStream.ResetWriteStream();
		this.pStream.SendNext(null);
		this.pStream.SendNext(null);
		this.pStream.SendNext(null);
		view.SerializeView(this.pStream, info);
		if (this.pStream.Count <= 3)
		{
			return null;
		}
		object[] array = this.pStream.ToArray();
		array[0] = view.viewID;
		array[1] = false;
		array[2] = null;
		if (view.synchronization == ViewSynchronization.Unreliable)
		{
			return array;
		}
		if (view.synchronization == ViewSynchronization.UnreliableOnChange)
		{
			if (this.AlmostEquals(array, view.lastOnSerializeDataSent))
			{
				if (view.mixedModeIsReliable)
				{
					return null;
				}
				view.mixedModeIsReliable = true;
				view.lastOnSerializeDataSent = array;
			}
			else
			{
				view.mixedModeIsReliable = false;
				view.lastOnSerializeDataSent = array;
			}
			return array;
		}
		if (view.synchronization == ViewSynchronization.ReliableDeltaCompressed)
		{
			object[] result = this.DeltaCompressionWrite(view.lastOnSerializeDataSent, array);
			view.lastOnSerializeDataSent = array;
			return result;
		}
		return null;
	}

	// Token: 0x06003C80 RID: 15488 RVA: 0x00133138 File Offset: 0x00131538
	private void OnSerializeRead(object[] data, PhotonPlayer sender, int networkTime, short correctPrefix)
	{
		int num = (int)data[0];
		PhotonView photonView = this.GetPhotonView(num);
		if (photonView == null)
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				"Received OnSerialization for view ID ",
				num,
				". We have no such PhotonView! Ignored this if you're entering or leaving a room. State: ",
				this.State
			}));
			return;
		}
		if (photonView.prefix > 0 && (int)correctPrefix != photonView.prefix)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Received OnSerialization for view ID ",
				num,
				" with prefix ",
				correctPrefix,
				". Our prefix is ",
				photonView.prefix
			}));
			return;
		}
		if (photonView.group != 0 && !this.allowedReceivingGroups.Contains(photonView.group))
		{
			return;
		}
		if (photonView.synchronization == ViewSynchronization.ReliableDeltaCompressed)
		{
			object[] array = this.DeltaCompressionRead(photonView.lastOnSerializeDataReceived, data);
			if (array == null)
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
				{
					Debug.Log(string.Concat(new object[]
					{
						"Skipping packet for ",
						photonView.name,
						" [",
						photonView.viewID,
						"] as we haven't received a full packet for delta compression yet. This is OK if it happens for the first few frames after joining a game."
					}));
				}
				return;
			}
			photonView.lastOnSerializeDataReceived = array;
			data = array;
		}
		if (sender.ID != photonView.ownerId && (!photonView.OwnerShipWasTransfered || photonView.ownerId == 0) && photonView.currentMasterID == -1)
		{
			photonView.ownerId = sender.ID;
		}
		this.readStream.SetReadStream(data, 3);
		PhotonMessageInfo info = new PhotonMessageInfo(sender, networkTime, photonView);
		photonView.DeserializeView(this.readStream, info);
	}

	// Token: 0x06003C81 RID: 15489 RVA: 0x001332F8 File Offset: 0x001316F8
	private object[] DeltaCompressionWrite(object[] previousContent, object[] currentContent)
	{
		if (currentContent == null || previousContent == null || previousContent.Length != currentContent.Length)
		{
			return currentContent;
		}
		if (currentContent.Length <= 3)
		{
			return null;
		}
		previousContent[1] = false;
		int num = 0;
		Queue<int> queue = null;
		for (int i = 3; i < currentContent.Length; i++)
		{
			object obj = currentContent[i];
			object two = previousContent[i];
			if (this.AlmostEquals(obj, two))
			{
				num++;
				previousContent[i] = null;
			}
			else
			{
				previousContent[i] = obj;
				if (obj == null)
				{
					if (queue == null)
					{
						queue = new Queue<int>(currentContent.Length);
					}
					queue.Enqueue(i);
				}
			}
		}
		if (num > 0)
		{
			if (num == currentContent.Length - 3)
			{
				return null;
			}
			previousContent[1] = true;
			if (queue != null)
			{
				previousContent[2] = queue.ToArray();
			}
		}
		previousContent[0] = currentContent[0];
		return previousContent;
	}

	// Token: 0x06003C82 RID: 15490 RVA: 0x001333C8 File Offset: 0x001317C8
	private object[] DeltaCompressionRead(object[] lastOnSerializeDataReceived, object[] incomingData)
	{
		if (!(bool)incomingData[1])
		{
			return incomingData;
		}
		if (lastOnSerializeDataReceived == null)
		{
			return null;
		}
		int[] array = incomingData[2] as int[];
		for (int i = 3; i < incomingData.Length; i++)
		{
			if (array == null || !array.Contains(i))
			{
				if (incomingData[i] == null)
				{
					object obj = lastOnSerializeDataReceived[i];
					incomingData[i] = obj;
				}
			}
		}
		return incomingData;
	}

	// Token: 0x06003C83 RID: 15491 RVA: 0x00133434 File Offset: 0x00131834
	private bool AlmostEquals(object[] lastData, object[] currentContent)
	{
		if (lastData == null && currentContent == null)
		{
			return true;
		}
		if (lastData == null || currentContent == null || lastData.Length != currentContent.Length)
		{
			return false;
		}
		for (int i = 0; i < currentContent.Length; i++)
		{
			object one = currentContent[i];
			object two = lastData[i];
			if (!this.AlmostEquals(one, two))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003C84 RID: 15492 RVA: 0x00133494 File Offset: 0x00131894
	private bool AlmostEquals(object one, object two)
	{
		if (one == null || two == null)
		{
			return one == null && two == null;
		}
		if (!one.Equals(two))
		{
			if (one is Vector3)
			{
				Vector3 target = (Vector3)one;
				Vector3 second = (Vector3)two;
				if (target.AlmostEquals(second, PhotonNetwork.precisionForVectorSynchronization))
				{
					return true;
				}
			}
			else if (one is Vector2)
			{
				Vector2 target2 = (Vector2)one;
				Vector2 second2 = (Vector2)two;
				if (target2.AlmostEquals(second2, PhotonNetwork.precisionForVectorSynchronization))
				{
					return true;
				}
			}
			else if (one is Quaternion)
			{
				Quaternion target3 = (Quaternion)one;
				Quaternion second3 = (Quaternion)two;
				if (target3.AlmostEquals(second3, PhotonNetwork.precisionForQuaternionSynchronization))
				{
					return true;
				}
			}
			else if (one is float)
			{
				float target4 = (float)one;
				float second4 = (float)two;
				if (target4.AlmostEquals(second4, PhotonNetwork.precisionForFloatSynchronization))
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}

	// Token: 0x06003C85 RID: 15493 RVA: 0x00133594 File Offset: 0x00131994
	protected internal static bool GetMethod(MonoBehaviour monob, string methodType, out MethodInfo mi)
	{
		mi = null;
		if (monob == null || string.IsNullOrEmpty(methodType))
		{
			return false;
		}
		List<MethodInfo> methods = SupportClass.GetMethods(monob.GetType(), null);
		for (int i = 0; i < methods.Count; i++)
		{
			MethodInfo methodInfo = methods[i];
			if (methodInfo.Name.Equals(methodType))
			{
				mi = methodInfo;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003C86 RID: 15494 RVA: 0x00133600 File Offset: 0x00131A00
	protected internal void LoadLevelIfSynced()
	{
		if (!PhotonNetwork.automaticallySyncScene || PhotonNetwork.isMasterClient || PhotonNetwork.room == null)
		{
			return;
		}
		if (!PhotonNetwork.room.CustomProperties.ContainsKey("curScn"))
		{
			return;
		}
		object obj = PhotonNetwork.room.CustomProperties["curScn"];
		if (obj is int)
		{
			if (SceneManagerHelper.ActiveSceneBuildIndex != (int)obj)
			{
				PhotonNetwork.LoadLevel((int)obj);
			}
		}
		else if (obj is string && SceneManagerHelper.ActiveSceneName != (string)obj)
		{
			PhotonNetwork.LoadLevel((string)obj);
		}
	}

	// Token: 0x06003C87 RID: 15495 RVA: 0x001336B4 File Offset: 0x00131AB4
	protected internal void SetLevelInPropsIfSynced(object levelId)
	{
		if (!PhotonNetwork.automaticallySyncScene || !PhotonNetwork.isMasterClient || PhotonNetwork.room == null)
		{
			return;
		}
		if (levelId == null)
		{
			Debug.LogError("Parameter levelId can't be null!");
			return;
		}
		if (PhotonNetwork.room.CustomProperties.ContainsKey("curScn"))
		{
			object obj = PhotonNetwork.room.CustomProperties["curScn"];
			if (obj is int && SceneManagerHelper.ActiveSceneBuildIndex == (int)obj)
			{
				return;
			}
			if (obj is string && SceneManagerHelper.ActiveSceneName != null && SceneManagerHelper.ActiveSceneName.Equals((string)obj))
			{
				return;
			}
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		if (levelId is int)
		{
			hashtable["curScn"] = (int)levelId;
		}
		else if (levelId is string)
		{
			hashtable["curScn"] = (string)levelId;
		}
		else
		{
			Debug.LogError("Parameter levelId must be int or string!");
		}
		PhotonNetwork.room.SetCustomProperties(hashtable, null, false);
		this.SendOutgoingCommands();
	}

	// Token: 0x06003C88 RID: 15496 RVA: 0x001337D1 File Offset: 0x00131BD1
	public void SetApp(string appId, string gameVersion)
	{
		this.AppId = appId.Trim();
		if (!string.IsNullOrEmpty(gameVersion))
		{
			PhotonNetwork.gameVersion = gameVersion.Trim();
		}
	}

	// Token: 0x06003C89 RID: 15497 RVA: 0x001337F8 File Offset: 0x00131BF8
	public bool WebRpc(string uriPath, object parameters)
	{
		return this.OpCustom(219, new Dictionary<byte, object>
		{
			{
				209,
				uriPath
			},
			{
				208,
				parameters
			}
		}, true);
	}

	// Token: 0x040025D6 RID: 9686
	protected internal string AppId;

	// Token: 0x040025D8 RID: 9688
	private string tokenCache;

	// Token: 0x040025D9 RID: 9689
	public AuthModeOption AuthMode;

	// Token: 0x040025DA RID: 9690
	public EncryptionMode EncryptionMode;

	// Token: 0x040025DC RID: 9692
	public const string NameServerHost = "ns.exitgames.com";

	// Token: 0x040025DD RID: 9693
	public const string NameServerHttp = "http://ns.exitgamescloud.com:80/photon/n";

	// Token: 0x040025DE RID: 9694
	private static readonly Dictionary<ConnectionProtocol, int> ProtocolToNameServerPort = new Dictionary<ConnectionProtocol, int>
	{
		{
			ConnectionProtocol.Udp,
			5058
		},
		{
			ConnectionProtocol.Tcp,
			4533
		},
		{
			ConnectionProtocol.WebSocket,
			9093
		},
		{
			ConnectionProtocol.WebSocketSecure,
			19093
		}
	};

	// Token: 0x040025E3 RID: 9699
	public bool IsInitialConnect;

	// Token: 0x040025E4 RID: 9700
	public bool insideLobby;

	// Token: 0x040025E6 RID: 9702
	protected internal List<TypedLobbyInfo> LobbyStatistics = new List<TypedLobbyInfo>();

	// Token: 0x040025E7 RID: 9703
	public Dictionary<string, RoomInfo> mGameList = new Dictionary<string, RoomInfo>();

	// Token: 0x040025E8 RID: 9704
	public RoomInfo[] mGameListCopy = new RoomInfo[0];

	// Token: 0x040025E9 RID: 9705
	private string playername = string.Empty;

	// Token: 0x040025EA RID: 9706
	private bool mPlayernameHasToBeUpdated;

	// Token: 0x040025EB RID: 9707
	private Room currentRoom;

	// Token: 0x040025F0 RID: 9712
	private JoinType lastJoinType;

	// Token: 0x040025F1 RID: 9713
	protected internal EnterRoomParams enterRoomParamsCache;

	// Token: 0x040025F2 RID: 9714
	private bool didAuthenticate;

	// Token: 0x040025F3 RID: 9715
	private string[] friendListRequested;

	// Token: 0x040025F4 RID: 9716
	private int friendListTimestamp;

	// Token: 0x040025F5 RID: 9717
	private bool isFetchingFriendList;

	// Token: 0x040025F8 RID: 9720
	public Dictionary<int, PhotonPlayer> mActors = new Dictionary<int, PhotonPlayer>();

	// Token: 0x040025F9 RID: 9721
	public PhotonPlayer[] mOtherPlayerListCopy = new PhotonPlayer[0];

	// Token: 0x040025FA RID: 9722
	public PhotonPlayer[] mPlayerListCopy = new PhotonPlayer[0];

	// Token: 0x040025FB RID: 9723
	public bool hasSwitchedMC;

	// Token: 0x040025FC RID: 9724
	private HashSet<byte> allowedReceivingGroups = new HashSet<byte>();

	// Token: 0x040025FD RID: 9725
	private HashSet<byte> blockSendingGroups = new HashSet<byte>();

	// Token: 0x040025FE RID: 9726
	protected internal Dictionary<int, PhotonView> photonViewList = new Dictionary<int, PhotonView>();

	// Token: 0x040025FF RID: 9727
	private readonly PhotonStream readStream = new PhotonStream(false, null);

	// Token: 0x04002600 RID: 9728
	private readonly PhotonStream pStream = new PhotonStream(true, null);

	// Token: 0x04002601 RID: 9729
	private readonly Dictionary<int, ExitGames.Client.Photon.Hashtable> dataPerGroupReliable = new Dictionary<int, ExitGames.Client.Photon.Hashtable>();

	// Token: 0x04002602 RID: 9730
	private readonly Dictionary<int, ExitGames.Client.Photon.Hashtable> dataPerGroupUnreliable = new Dictionary<int, ExitGames.Client.Photon.Hashtable>();

	// Token: 0x04002603 RID: 9731
	protected internal short currentLevelPrefix;

	// Token: 0x04002604 RID: 9732
	protected internal bool loadingLevelAndPausedNetwork;

	// Token: 0x04002605 RID: 9733
	protected internal const string CurrentSceneProperty = "curScn";

	// Token: 0x04002606 RID: 9734
	public static bool UsePrefabCache = true;

	// Token: 0x04002607 RID: 9735
	internal IPunPrefabPool ObjectPool;

	// Token: 0x04002608 RID: 9736
	public static Dictionary<string, GameObject> PrefabCache = new Dictionary<string, GameObject>();

	// Token: 0x04002609 RID: 9737
	private Dictionary<Type, List<MethodInfo>> monoRPCMethodsCache = new Dictionary<Type, List<MethodInfo>>();

	// Token: 0x0400260A RID: 9738
	private readonly Dictionary<string, int> rpcShortcuts;

	// Token: 0x0400260B RID: 9739
	private static readonly string OnPhotonInstantiateString = PhotonNetworkingMessage.OnPhotonInstantiate.ToString();

	// Token: 0x0400260C RID: 9740
	private Dictionary<int, object[]> tempInstantiationData = new Dictionary<int, object[]>();

	// Token: 0x0400260D RID: 9741
	public static int ObjectsInOneUpdate = 10;

	// Token: 0x0400260E RID: 9742
	public const int SyncViewId = 0;

	// Token: 0x0400260F RID: 9743
	public const int SyncCompressed = 1;

	// Token: 0x04002610 RID: 9744
	public const int SyncNullValues = 2;

	// Token: 0x04002611 RID: 9745
	public const int SyncFirstValue = 3;
}
