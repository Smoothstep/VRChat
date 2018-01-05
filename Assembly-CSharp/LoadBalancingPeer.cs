using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000737 RID: 1847
internal class LoadBalancingPeer : PhotonPeer
{
	// Token: 0x06003BB3 RID: 15283 RVA: 0x0012C3BA File Offset: 0x0012A7BA
	public LoadBalancingPeer(ConnectionProtocol protocolType) : base(protocolType)
	{
	}

	// Token: 0x06003BB4 RID: 15284 RVA: 0x0012C3CE File Offset: 0x0012A7CE
	public LoadBalancingPeer(IPhotonPeerListener listener, ConnectionProtocol protocolType) : this(protocolType)
	{
		base.Listener = listener;
	}

	// Token: 0x17000957 RID: 2391
	// (get) Token: 0x06003BB5 RID: 15285 RVA: 0x0012C3DE File Offset: 0x0012A7DE
	internal bool IsProtocolSecure
	{
		get
		{
			return base.UsedProtocol == ConnectionProtocol.WebSocketSecure;
		}
	}

	// Token: 0x06003BB6 RID: 15286 RVA: 0x0012C3EC File Offset: 0x0012A7EC
	public virtual bool OpGetRegions(string appId)
	{
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary[224] = appId;
		return this.OpCustom(220, dictionary, true, 0, true);
	}

	// Token: 0x06003BB7 RID: 15287 RVA: 0x0012C41C File Offset: 0x0012A81C
	public virtual bool OpJoinLobby(TypedLobby lobby = null)
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpJoinLobby()");
		}
		Dictionary<byte, object> dictionary = null;
		if (lobby != null && !lobby.IsDefault)
		{
			dictionary = new Dictionary<byte, object>();
			dictionary[213] = lobby.Name;
			dictionary[212] = (byte)lobby.Type;
		}
		return this.OpCustom(229, dictionary, true);
	}

	// Token: 0x06003BB8 RID: 15288 RVA: 0x0012C493 File Offset: 0x0012A893
	public virtual bool OpLeaveLobby()
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpLeaveLobby()");
		}
		return this.OpCustom(228, null, true);
	}

	// Token: 0x06003BB9 RID: 15289 RVA: 0x0012C4C0 File Offset: 0x0012A8C0
	private void RoomOptionsToOpParameters(Dictionary<byte, object> op, RoomOptions roomOptions)
	{
		if (roomOptions == null)
		{
			roomOptions = new RoomOptions();
		}
		Hashtable hashtable = new Hashtable();
		hashtable[253] = roomOptions.IsOpen;
		hashtable[254] = roomOptions.IsVisible;
		hashtable[250] = ((roomOptions.CustomRoomPropertiesForLobby != null) ? roomOptions.CustomRoomPropertiesForLobby : new string[0]);
		hashtable.MergeStringKeys(roomOptions.CustomRoomProperties);
		if (roomOptions.MaxPlayers > 0)
		{
			hashtable[byte.MaxValue] = roomOptions.MaxPlayers;
		}
		op[248] = hashtable;
		int num = 0;
		op[241] = roomOptions.CleanupCacheOnLeave;
		if (roomOptions.CleanupCacheOnLeave)
		{
			num |= 2;
			hashtable[249] = true;
		}
		if (roomOptions.PlayerTtl > 0 || roomOptions.PlayerTtl == -1)
		{
			num |= 1;
			op[232] = true;
			op[235] = roomOptions.PlayerTtl;
		}
		if (roomOptions.EmptyRoomTtl > 0)
		{
			op[236] = roomOptions.EmptyRoomTtl;
		}
		if (roomOptions.SuppressRoomEvents)
		{
			num |= 4;
			op[237] = true;
		}
		if (roomOptions.Plugins != null)
		{
			op[204] = roomOptions.Plugins;
		}
		if (roomOptions.PublishUserId)
		{
			num |= 8;
			op[239] = true;
		}
		if (roomOptions.DeleteNullProperties)
		{
			num |= 16;
		}
		op[191] = num;
	}

	// Token: 0x06003BBA RID: 15290 RVA: 0x0012C6A0 File Offset: 0x0012AAA0
	public virtual bool OpCreateRoom(EnterRoomParams opParams)
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpCreateRoom()");
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (!string.IsNullOrEmpty(opParams.RoomName))
		{
			dictionary[byte.MaxValue] = opParams.RoomName;
		}
		if (opParams.Lobby != null && !string.IsNullOrEmpty(opParams.Lobby.Name))
		{
			dictionary[213] = opParams.Lobby.Name;
			dictionary[212] = (byte)opParams.Lobby.Type;
		}
		if (opParams.ExpectedUsers != null && opParams.ExpectedUsers.Length > 0)
		{
			dictionary[238] = opParams.ExpectedUsers;
		}
		if (opParams.OnGameServer)
		{
			if (opParams.PlayerProperties != null && opParams.PlayerProperties.Count > 0)
			{
				dictionary[249] = opParams.PlayerProperties;
				dictionary[250] = true;
			}
			this.RoomOptionsToOpParameters(dictionary, opParams.RoomOptions);
		}
		return this.OpCustom(227, dictionary, true);
	}

	// Token: 0x06003BBB RID: 15291 RVA: 0x0012C7D0 File Offset: 0x0012ABD0
	public virtual bool OpJoinRoom(EnterRoomParams opParams)
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpJoinRoom()");
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (!string.IsNullOrEmpty(opParams.RoomName))
		{
			dictionary[byte.MaxValue] = opParams.RoomName;
		}
		if (opParams.CreateIfNotExists)
		{
			dictionary[215] = 1;
			if (opParams.Lobby != null)
			{
				dictionary[213] = opParams.Lobby.Name;
				dictionary[212] = (byte)opParams.Lobby.Type;
			}
		}
		if (opParams.RejoinOnly)
		{
			dictionary[215] = 3;
		}
		if (opParams.ExpectedUsers != null && opParams.ExpectedUsers.Length > 0)
		{
			dictionary[238] = opParams.ExpectedUsers;
		}
		if (opParams.OnGameServer)
		{
			if (opParams.PlayerProperties != null && opParams.PlayerProperties.Count > 0)
			{
				dictionary[249] = opParams.PlayerProperties;
				dictionary[250] = true;
			}
			if (opParams.CreateIfNotExists)
			{
				this.RoomOptionsToOpParameters(dictionary, opParams.RoomOptions);
			}
		}
		return this.OpCustom(226, dictionary, true);
	}

	// Token: 0x06003BBC RID: 15292 RVA: 0x0012C930 File Offset: 0x0012AD30
	public virtual bool OpJoinRandomRoom(OpJoinRandomRoomParams opJoinRandomRoomParams)
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpJoinRandomRoom()");
		}
		Hashtable hashtable = new Hashtable();
		hashtable.MergeStringKeys(opJoinRandomRoomParams.ExpectedCustomRoomProperties);
		if (opJoinRandomRoomParams.ExpectedMaxPlayers > 0)
		{
			hashtable[byte.MaxValue] = opJoinRandomRoomParams.ExpectedMaxPlayers;
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (hashtable.Count > 0)
		{
			dictionary[248] = hashtable;
		}
		if (opJoinRandomRoomParams.MatchingType != MatchmakingMode.FillRoom)
		{
			dictionary[223] = (byte)opJoinRandomRoomParams.MatchingType;
		}
		if (opJoinRandomRoomParams.TypedLobby != null && !string.IsNullOrEmpty(opJoinRandomRoomParams.TypedLobby.Name))
		{
			dictionary[213] = opJoinRandomRoomParams.TypedLobby.Name;
			dictionary[212] = (byte)opJoinRandomRoomParams.TypedLobby.Type;
		}
		if (!string.IsNullOrEmpty(opJoinRandomRoomParams.SqlLobbyFilter))
		{
			dictionary[245] = opJoinRandomRoomParams.SqlLobbyFilter;
		}
		if (opJoinRandomRoomParams.ExpectedUsers != null && opJoinRandomRoomParams.ExpectedUsers.Length > 0)
		{
			dictionary[238] = opJoinRandomRoomParams.ExpectedUsers;
		}
		return this.OpCustom(225, dictionary, true);
	}

	// Token: 0x06003BBD RID: 15293 RVA: 0x0012CA7C File Offset: 0x0012AE7C
	public virtual bool OpLeaveRoom(bool becomeInactive)
	{
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (becomeInactive)
		{
			dictionary[233] = becomeInactive;
		}
		return this.OpCustom(254, dictionary, true);
	}

	// Token: 0x06003BBE RID: 15294 RVA: 0x0012CAB4 File Offset: 0x0012AEB4
	public virtual bool OpGetGameList(TypedLobby lobby, string queryData)
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpGetGameList()");
		}
		if (lobby == null)
		{
			if (this.DebugOut >= DebugLevel.INFO)
			{
				base.Listener.DebugReturn(DebugLevel.INFO, "OpGetGameList not sent. Lobby cannot be null.");
			}
			return false;
		}
		if (lobby.Type != LobbyType.SqlLobby)
		{
			if (this.DebugOut >= DebugLevel.INFO)
			{
				base.Listener.DebugReturn(DebugLevel.INFO, "OpGetGameList not sent. LobbyType must be SqlLobby.");
			}
			return false;
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary[213] = lobby.Name;
		dictionary[212] = (byte)lobby.Type;
		dictionary[245] = queryData;
		return this.OpCustom(217, dictionary, true);
	}

	// Token: 0x06003BBF RID: 15295 RVA: 0x0012CB74 File Offset: 0x0012AF74
	public virtual bool OpFindFriends(string[] friendsToFind)
	{
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (friendsToFind != null && friendsToFind.Length > 0)
		{
			dictionary[1] = friendsToFind;
		}
		return this.OpCustom(222, dictionary, true);
	}

	// Token: 0x06003BC0 RID: 15296 RVA: 0x0012CBAB File Offset: 0x0012AFAB
	public bool OpSetCustomPropertiesOfActor(int actorNr, Hashtable actorProperties)
	{
		return this.OpSetPropertiesOfActor(actorNr, actorProperties.StripToStringKeys(), null, false);
	}

	// Token: 0x06003BC1 RID: 15297 RVA: 0x0012CBBC File Offset: 0x0012AFBC
	protected internal bool OpSetPropertiesOfActor(int actorNr, Hashtable actorProperties, Hashtable expectedProperties = null, bool webForward = false)
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpSetPropertiesOfActor()");
		}
		if (actorNr <= 0 || actorProperties == null)
		{
			if (this.DebugOut >= DebugLevel.INFO)
			{
				base.Listener.DebugReturn(DebugLevel.INFO, "OpSetPropertiesOfActor not sent. ActorNr must be > 0 and actorProperties != null.");
			}
			return false;
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary.Add(251, actorProperties);
		dictionary.Add(254, actorNr);
		dictionary.Add(250, true);
		if (expectedProperties != null && expectedProperties.Count != 0)
		{
			dictionary.Add(231, expectedProperties);
		}
		if (webForward)
		{
			dictionary[234] = true;
		}
		return this.OpCustom(252, dictionary, true, 0, false);
	}

	// Token: 0x06003BC2 RID: 15298 RVA: 0x0012CC8C File Offset: 0x0012B08C
	protected void OpSetPropertyOfRoom(byte propCode, object value)
	{
		Hashtable hashtable = new Hashtable();
		hashtable[propCode] = value;
		this.OpSetPropertiesOfRoom(hashtable, null, false);
	}

	// Token: 0x06003BC3 RID: 15299 RVA: 0x0012CCB6 File Offset: 0x0012B0B6
	public bool OpSetCustomPropertiesOfRoom(Hashtable gameProperties, bool broadcast, byte channelId)
	{
		return this.OpSetPropertiesOfRoom(gameProperties.StripToStringKeys(), null, false);
	}

	// Token: 0x06003BC4 RID: 15300 RVA: 0x0012CCC8 File Offset: 0x0012B0C8
	protected internal bool OpSetPropertiesOfRoom(Hashtable gameProperties, Hashtable expectedProperties = null, bool webForward = false)
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpSetPropertiesOfRoom()");
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary.Add(251, gameProperties);
		dictionary.Add(250, true);
		if (expectedProperties != null && expectedProperties.Count != 0)
		{
			dictionary.Add(231, expectedProperties);
		}
		if (webForward)
		{
			dictionary[234] = true;
		}
		return this.OpCustom(252, dictionary, true, 0, false);
	}

	// Token: 0x06003BC5 RID: 15301 RVA: 0x0012CD58 File Offset: 0x0012B158
	public virtual bool OpAuthenticate(string appId, string appVersion, AuthenticationValues authValues, string regionCode, bool getLobbyStatistics)
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpAuthenticate()");
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (getLobbyStatistics)
		{
			dictionary[211] = true;
		}
		if (authValues != null && authValues.Token != null)
		{
			dictionary[221] = authValues.Token;
			return this.OpCustom(230, dictionary, true, 0, false);
		}
		dictionary[220] = appVersion;
		dictionary[224] = appId;
		if (!string.IsNullOrEmpty(regionCode))
		{
			dictionary[210] = regionCode;
		}
		if (authValues != null)
		{
			if (!string.IsNullOrEmpty(authValues.UserId))
			{
				dictionary[225] = authValues.UserId;
			}
			if (authValues.AuthType != CustomAuthenticationType.None)
			{
				if (!this.IsProtocolSecure && !base.IsEncryptionAvailable)
				{
					base.Listener.DebugReturn(DebugLevel.ERROR, "OpAuthenticate() failed. When you want Custom Authentication encryption is mandatory.");
					return false;
				}
				dictionary[217] = (byte)authValues.AuthType;
				if (!string.IsNullOrEmpty(authValues.Token))
				{
					dictionary[221] = authValues.Token;
				}
				else
				{
					if (!string.IsNullOrEmpty(authValues.AuthGetParameters))
					{
						dictionary[216] = authValues.AuthGetParameters;
					}
					if (authValues.AuthPostData != null)
					{
						dictionary[214] = authValues.AuthPostData;
					}
				}
			}
		}
		bool flag = this.OpCustom(230, dictionary, true, 0, base.IsEncryptionAvailable);
		if (!flag)
		{
			base.Listener.DebugReturn(DebugLevel.ERROR, "Error calling OpAuthenticate! Did not work. Check log output, AuthValues and if you're connected.");
		}
		return flag;
	}

	// Token: 0x06003BC6 RID: 15302 RVA: 0x0012CF0C File Offset: 0x0012B30C
	public virtual bool OpAuthenticateOnce(string appId, string appVersion, AuthenticationValues authValues, string regionCode, EncryptionMode encryptionMode, ConnectionProtocol expectedProtocol)
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpAuthenticate()");
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (authValues != null && authValues.Token != null)
		{
			dictionary[221] = authValues.Token;
			return this.OpCustom(231, dictionary, true, 0, false);
		}
		if (encryptionMode == EncryptionMode.DatagramEncryption && expectedProtocol != ConnectionProtocol.Udp)
		{
			Debug.LogWarning("Expected protocol set to UDP, due to encryption mode DatagramEncryption. Changing protocol in PhotonServerSettings from: " + PhotonNetwork.PhotonServerSettings.Protocol);
			PhotonNetwork.PhotonServerSettings.Protocol = ConnectionProtocol.Udp;
			expectedProtocol = ConnectionProtocol.Udp;
		}
		dictionary[195] = (byte)expectedProtocol;
		dictionary[193] = (byte)encryptionMode;
		dictionary[220] = appVersion;
		dictionary[224] = appId;
		if (!string.IsNullOrEmpty(regionCode))
		{
			dictionary[210] = regionCode;
		}
		if (authValues != null)
		{
			if (!string.IsNullOrEmpty(authValues.UserId))
			{
				dictionary[225] = authValues.UserId;
			}
			if (authValues.AuthType != CustomAuthenticationType.None)
			{
				dictionary[217] = (byte)authValues.AuthType;
				if (!string.IsNullOrEmpty(authValues.Token))
				{
					dictionary[221] = authValues.Token;
				}
				else
				{
					if (!string.IsNullOrEmpty(authValues.AuthGetParameters))
					{
						dictionary[216] = authValues.AuthGetParameters;
					}
					if (authValues.AuthPostData != null)
					{
						dictionary[214] = authValues.AuthPostData;
					}
				}
			}
		}
		return this.OpCustom(231, dictionary, true, 0, base.IsEncryptionAvailable);
	}

	// Token: 0x06003BC7 RID: 15303 RVA: 0x0012D0C4 File Offset: 0x0012B4C4
	public virtual bool OpChangeGroups(byte[] groupsToRemove, byte[] groupsToAdd)
	{
		if (this.DebugOut >= DebugLevel.ALL)
		{
			base.Listener.DebugReturn(DebugLevel.ALL, "OpChangeGroups()");
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (groupsToRemove != null)
		{
			dictionary[239] = groupsToRemove;
		}
		if (groupsToAdd != null)
		{
			dictionary[238] = groupsToAdd;
		}
		return this.OpCustom(248, dictionary, true, 0);
	}

	// Token: 0x06003BC8 RID: 15304 RVA: 0x0012D128 File Offset: 0x0012B528
	public virtual bool OpRaiseEvent(byte eventCode, object customEventContent, bool sendReliable, RaiseEventOptions raiseEventOptions)
	{
		this.opParameters.Clear();
		this.opParameters[244] = eventCode;
		if (customEventContent != null)
		{
			this.opParameters[245] = customEventContent;
		}
		if (raiseEventOptions == null)
		{
			raiseEventOptions = RaiseEventOptions.Default;
		}
		else
		{
			if (raiseEventOptions.CachingOption != EventCaching.DoNotCache)
			{
				this.opParameters[247] = (byte)raiseEventOptions.CachingOption;
			}
			if (raiseEventOptions.Receivers != ReceiverGroup.Others)
			{
				this.opParameters[246] = (byte)raiseEventOptions.Receivers;
			}
			if (raiseEventOptions.InterestGroup != 0)
			{
				this.opParameters[240] = raiseEventOptions.InterestGroup;
			}
			if (raiseEventOptions.TargetActors != null)
			{
				this.opParameters[252] = raiseEventOptions.TargetActors;
			}
			if (raiseEventOptions.ForwardToWebhook)
			{
				this.opParameters[234] = true;
			}
		}
		PhotonBandwidthGui.RecordEvent(new EventData
		{
			Code = eventCode,
			Parameters = this.opParameters
		}, PhotonNetwork.player);
		return this.OpCustom(253, this.opParameters, sendReliable, raiseEventOptions.SequenceChannel, raiseEventOptions.Encrypt);
	}

	// Token: 0x06003BC9 RID: 15305 RVA: 0x0012D284 File Offset: 0x0012B684
	public virtual bool OpSettings(bool receiveLobbyStats)
	{
		if (this.DebugOut >= DebugLevel.ALL)
		{
			base.Listener.DebugReturn(DebugLevel.ALL, "OpSettings()");
		}
		this.opParameters.Clear();
		if (receiveLobbyStats)
		{
			this.opParameters[0] = receiveLobbyStats;
		}
		return this.opParameters.Count == 0 || this.OpCustom(218, this.opParameters, true);
	}

	// Token: 0x040024B9 RID: 9401
	private readonly Dictionary<byte, object> opParameters = new Dictionary<byte, object>();

	// Token: 0x02000738 RID: 1848
	private enum RoomOptionBit
	{
		// Token: 0x040024BB RID: 9403
		CheckUserOnJoin = 1,
		// Token: 0x040024BC RID: 9404
		DeleteCacheOnLeave,
		// Token: 0x040024BD RID: 9405
		SuppressRoomEvents = 4,
		// Token: 0x040024BE RID: 9406
		PublishUserId = 8,
		// Token: 0x040024BF RID: 9407
		DeleteNullProps = 16,
		// Token: 0x040024C0 RID: 9408
		BroadcastPropsChangeToAll = 32
	}
}
