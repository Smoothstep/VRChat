using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using VRC;
using VRC.Core;

// Token: 0x02000AAE RID: 2734
public class NetworkManager : MonoBehaviour, IPunCallbacks
{
	// Token: 0x14000053 RID: 83
	// (add) Token: 0x06005272 RID: 21106 RVA: 0x001C50A8 File Offset: 0x001C34A8
	// (remove) Token: 0x06005273 RID: 21107 RVA: 0x001C50E0 File Offset: 0x001C34E0
	public event Action<GameObject, Player, Player> OnOwnershipTransferedEvent;

	// Token: 0x14000054 RID: 84
	// (add) Token: 0x06005274 RID: 21108 RVA: 0x001C5118 File Offset: 0x001C3518
	// (remove) Token: 0x06005275 RID: 21109 RVA: 0x001C5150 File Offset: 0x001C3550
	public event Action<Player> OnPhotonPlayerActivityChangedEvent;

	// Token: 0x14000055 RID: 85
	// (add) Token: 0x06005276 RID: 21110 RVA: 0x001C5188 File Offset: 0x001C3588
	// (remove) Token: 0x06005277 RID: 21111 RVA: 0x001C51C0 File Offset: 0x001C35C0
	public event Action OnLobbyStatisticsUpdateEvent;

	// Token: 0x14000056 RID: 86
	// (add) Token: 0x06005278 RID: 21112 RVA: 0x001C51F8 File Offset: 0x001C35F8
	// (remove) Token: 0x06005279 RID: 21113 RVA: 0x001C5230 File Offset: 0x001C3630
	public event Action<Dictionary<string, object>> OnCustomAuthenticationResponseEvent;

	// Token: 0x14000057 RID: 87
	// (add) Token: 0x0600527A RID: 21114 RVA: 0x001C5268 File Offset: 0x001C3668
	// (remove) Token: 0x0600527B RID: 21115 RVA: 0x001C52A0 File Offset: 0x001C36A0
	public event Action<object[]> OnOwnershipRequestEvent;

	// Token: 0x14000058 RID: 88
	// (add) Token: 0x0600527C RID: 21116 RVA: 0x001C52D8 File Offset: 0x001C36D8
	// (remove) Token: 0x0600527D RID: 21117 RVA: 0x001C5310 File Offset: 0x001C3710
	public event Action OnConnectedToPhotonEvent;

	// Token: 0x14000059 RID: 89
	// (add) Token: 0x0600527E RID: 21118 RVA: 0x001C5348 File Offset: 0x001C3748
	// (remove) Token: 0x0600527F RID: 21119 RVA: 0x001C5380 File Offset: 0x001C3780
	public event Action OnLeftRoomEvent;

	// Token: 0x1400005A RID: 90
	// (add) Token: 0x06005280 RID: 21120 RVA: 0x001C53B8 File Offset: 0x001C37B8
	// (remove) Token: 0x06005281 RID: 21121 RVA: 0x001C53F0 File Offset: 0x001C37F0
	public event Action<Player> OnMasterClientSwitchedEvent;

	// Token: 0x1400005B RID: 91
	// (add) Token: 0x06005282 RID: 21122 RVA: 0x001C5428 File Offset: 0x001C3828
	// (remove) Token: 0x06005283 RID: 21123 RVA: 0x001C5460 File Offset: 0x001C3860
	public event Action<object[]> OnPhotonCreateRoomFailedEvent;

	// Token: 0x1400005C RID: 92
	// (add) Token: 0x06005284 RID: 21124 RVA: 0x001C5498 File Offset: 0x001C3898
	// (remove) Token: 0x06005285 RID: 21125 RVA: 0x001C54D0 File Offset: 0x001C38D0
	public event Action<object[]> OnPhotonJoinRoomFailedEvent;

	// Token: 0x1400005D RID: 93
	// (add) Token: 0x06005286 RID: 21126 RVA: 0x001C5508 File Offset: 0x001C3908
	// (remove) Token: 0x06005287 RID: 21127 RVA: 0x001C5540 File Offset: 0x001C3940
	public event Action OnCreatedRoomEvent;

	// Token: 0x1400005E RID: 94
	// (add) Token: 0x06005288 RID: 21128 RVA: 0x001C5578 File Offset: 0x001C3978
	// (remove) Token: 0x06005289 RID: 21129 RVA: 0x001C55B0 File Offset: 0x001C39B0
	public event Action OnJoinedLobbyEvent;

	// Token: 0x1400005F RID: 95
	// (add) Token: 0x0600528A RID: 21130 RVA: 0x001C55E8 File Offset: 0x001C39E8
	// (remove) Token: 0x0600528B RID: 21131 RVA: 0x001C5620 File Offset: 0x001C3A20
	public event Action OnLeftLobbyEvent;

	// Token: 0x14000060 RID: 96
	// (add) Token: 0x0600528C RID: 21132 RVA: 0x001C5658 File Offset: 0x001C3A58
	// (remove) Token: 0x0600528D RID: 21133 RVA: 0x001C5690 File Offset: 0x001C3A90
	public event Action<DisconnectCause> OnFailedToConnectToPhotonEvent;

	// Token: 0x14000061 RID: 97
	// (add) Token: 0x0600528E RID: 21134 RVA: 0x001C56C8 File Offset: 0x001C3AC8
	// (remove) Token: 0x0600528F RID: 21135 RVA: 0x001C5700 File Offset: 0x001C3B00
	public event Action<DisconnectCause> OnConnectionFailEvent;

	// Token: 0x14000062 RID: 98
	// (add) Token: 0x06005290 RID: 21136 RVA: 0x001C5738 File Offset: 0x001C3B38
	// (remove) Token: 0x06005291 RID: 21137 RVA: 0x001C5770 File Offset: 0x001C3B70
	public event Action OnDisconnectedFromPhotonEvent;

	// Token: 0x14000063 RID: 99
	// (add) Token: 0x06005292 RID: 21138 RVA: 0x001C57A8 File Offset: 0x001C3BA8
	// (remove) Token: 0x06005293 RID: 21139 RVA: 0x001C57E0 File Offset: 0x001C3BE0
	public event Action<PhotonMessageInfo> OnPhotonInstantiateEvent;

	// Token: 0x14000064 RID: 100
	// (add) Token: 0x06005294 RID: 21140 RVA: 0x001C5818 File Offset: 0x001C3C18
	// (remove) Token: 0x06005295 RID: 21141 RVA: 0x001C5850 File Offset: 0x001C3C50
	public event Action OnReceivedRoomListUpdateEvent;

	// Token: 0x14000065 RID: 101
	// (add) Token: 0x06005296 RID: 21142 RVA: 0x001C5888 File Offset: 0x001C3C88
	// (remove) Token: 0x06005297 RID: 21143 RVA: 0x001C58C0 File Offset: 0x001C3CC0
	public event Action OnJoinedRoomEvent;

	// Token: 0x14000066 RID: 102
	// (add) Token: 0x06005298 RID: 21144 RVA: 0x001C58F8 File Offset: 0x001C3CF8
	// (remove) Token: 0x06005299 RID: 21145 RVA: 0x001C5930 File Offset: 0x001C3D30
	public event Action<PhotonPlayer> OnPhotonPlayerConnectedEvent;

	// Token: 0x14000067 RID: 103
	// (add) Token: 0x0600529A RID: 21146 RVA: 0x001C5968 File Offset: 0x001C3D68
	// (remove) Token: 0x0600529B RID: 21147 RVA: 0x001C59A0 File Offset: 0x001C3DA0
	public event Action<PhotonPlayer> OnPhotonPlayerDisconnectedEvent;

	// Token: 0x14000068 RID: 104
	// (add) Token: 0x0600529C RID: 21148 RVA: 0x001C59D8 File Offset: 0x001C3DD8
	// (remove) Token: 0x0600529D RID: 21149 RVA: 0x001C5A10 File Offset: 0x001C3E10
	public event Action<object[]> OnPhotonRandomJoinFailedEvent;

	// Token: 0x14000069 RID: 105
	// (add) Token: 0x0600529E RID: 21150 RVA: 0x001C5A48 File Offset: 0x001C3E48
	// (remove) Token: 0x0600529F RID: 21151 RVA: 0x001C5A80 File Offset: 0x001C3E80
	public event Action OnConnectedToMasterEvent;

	// Token: 0x1400006A RID: 106
	// (add) Token: 0x060052A0 RID: 21152 RVA: 0x001C5AB8 File Offset: 0x001C3EB8
	// (remove) Token: 0x060052A1 RID: 21153 RVA: 0x001C5AF0 File Offset: 0x001C3EF0
	public event Action OnPhotonMaxCccuReachedEvent;

	// Token: 0x1400006B RID: 107
	// (add) Token: 0x060052A2 RID: 21154 RVA: 0x001C5B28 File Offset: 0x001C3F28
	// (remove) Token: 0x060052A3 RID: 21155 RVA: 0x001C5B60 File Offset: 0x001C3F60
	public event Action<Hashtable> OnPhotonCustomRoomPropertiesChangedEvent;

	// Token: 0x1400006C RID: 108
	// (add) Token: 0x060052A4 RID: 21156 RVA: 0x001C5B98 File Offset: 0x001C3F98
	// (remove) Token: 0x060052A5 RID: 21157 RVA: 0x001C5BD0 File Offset: 0x001C3FD0
	public event Action<object[]> OnPhotonPlayerPropertiesChangedEvent;

	// Token: 0x1400006D RID: 109
	// (add) Token: 0x060052A6 RID: 21158 RVA: 0x001C5C08 File Offset: 0x001C4008
	// (remove) Token: 0x060052A7 RID: 21159 RVA: 0x001C5C40 File Offset: 0x001C4040
	public event Action OnUpdatedFriendListEvent;

	// Token: 0x1400006E RID: 110
	// (add) Token: 0x060052A8 RID: 21160 RVA: 0x001C5C78 File Offset: 0x001C4078
	// (remove) Token: 0x060052A9 RID: 21161 RVA: 0x001C5CB0 File Offset: 0x001C40B0
	public event Action<string> OnCustomAuthenticationFailedEvent;

	// Token: 0x1400006F RID: 111
	// (add) Token: 0x060052AA RID: 21162 RVA: 0x001C5CE8 File Offset: 0x001C40E8
	// (remove) Token: 0x060052AB RID: 21163 RVA: 0x001C5D20 File Offset: 0x001C4120
	public event Action<OperationResponse> OnWebRpcResponseEvent;

	// Token: 0x060052AC RID: 21164 RVA: 0x001C5D58 File Offset: 0x001C4158
	private void Awake()
	{
		if (NetworkManager.Instance == null)
		{
			NetworkManager.Instance = this;
		}
        PhotonNetwork.SendMonoMessageTargets = new HashSet<GameObject>();
        PhotonNetwork.SendMonoMessageTargets.Add(NetworkManager.Instance.gameObject);
	}

	// Token: 0x060052AD RID: 21165 RVA: 0x001C5D98 File Offset: 0x001C4198
	private void Start()
	{
		this.worldUpdate = new ApiWorldUpdate();
	}

	// Token: 0x060052AE RID: 21166 RVA: 0x001C5DA8 File Offset: 0x001C41A8
	private void Update()
	{
		if (!PhotonNetwork.inRoom)
		{
			if (this.worldUpdate.inWorld)
			{
				this.worldUpdate.Leave();
				this.timeSinceUpdate = 0f;
			}
		}
		else
		{
			this.timeSinceUpdate += Time.deltaTime;
			if (!this.worldUpdate.inWorld)
			{
				this.worldUpdate.Join(APIUser.CurrentUser.id, PhotonNetwork.room.Name);
				this.timeSinceUpdate = 0f;
			}
			else if (this.timeSinceUpdate > this.updatePeriod)
			{
				this.worldUpdate.Update();
				this.timeSinceUpdate = 0f;
			}
		}
	}

	// Token: 0x060052AF RID: 21167 RVA: 0x001C5E64 File Offset: 0x001C4264
	public void OnOwnershipRequest(object[] viewAndPlayer)
	{
		PhotonView photonView = viewAndPlayer[0] as PhotonView;
		PhotonPlayer photonPlayer = viewAndPlayer[1] as PhotonPlayer;
		if (photonView == null || photonPlayer == null)
		{
			return;
		}
		VRC.Network.SetOwner(VRC.Network.GetPlayer(photonPlayer), photonView.gameObject, VRC.Network.OwnershipModificationType.Request, true);
		if (this.OnOwnershipRequestEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnOwnershipRequestEvent(viewAndPlayer);
			});
		}
	}

	// Token: 0x060052B0 RID: 21168 RVA: 0x001C5EE4 File Offset: 0x001C42E4
	public void OnVRCPlayerJoined(Player player)
	{
		Debug.LogFormat("<color=grey>OnPlayerJoined {0}</color>", new object[]
		{
			player.name
		});
		if (this.OnPlayerJoinedEvent != null && player != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnPlayerJoinedEvent.Invoke(player);
			});
		}
	}

	// Token: 0x060052B1 RID: 21169 RVA: 0x001C5F50 File Offset: 0x001C4350
	public void OnVRCPlayerLeft(Player player)
	{
		Debug.LogFormat("<color=grey>OnPlayerLeft {0}</color>", new object[]
		{
			player.name
		});
		if (this.OnPlayerLeftEvent != null && player != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnPlayerLeftEvent.Invoke(player);
			});
		}
	}

	// Token: 0x060052B2 RID: 21170 RVA: 0x001C5FBC File Offset: 0x001C43BC
	public void OnConnectedToPhoton()
	{
		Debug.Log("<color=grey>OnConnectedToPhoton</color>");
		if (this.OnConnectedToPhotonEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnConnectedToPhotonEvent();
			});
		}
	}

	// Token: 0x060052B3 RID: 21171 RVA: 0x001C5FE4 File Offset: 0x001C43E4
	public void OnLeftRoom()
	{
		Debug.Log("<color=grey>OnLeftRoom</color>");
		if (this.OnLeftRoomEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnLeftRoomEvent();
			});
		}
	}

	// Token: 0x060052B4 RID: 21172 RVA: 0x001C600C File Offset: 0x001C440C
	public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		Debug.Log("<color=grey>OnMasterClientSwitched</color>");
		if (this.OnMasterClientSwitchedEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnMasterClientSwitchedEvent(VRC.Network.GetPlayer(newMasterClient));
			});
		}
	}

	// Token: 0x060052B5 RID: 21173 RVA: 0x001C6054 File Offset: 0x001C4454
	public void OnPhotonCreateRoomFailed(object[] codeAndMsg)
	{
		Debug.Log("<color=grey>OnPhotonCreateRoomFailed</color>");
		if (this.OnPhotonCreateRoomFailedEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnPhotonCreateRoomFailedEvent(codeAndMsg);
			});
		}
	}

	// Token: 0x060052B6 RID: 21174 RVA: 0x001C609C File Offset: 0x001C449C
	public void OnPhotonJoinRoomFailed(object[] codeAndMsg)
	{
		Debug.Log("<color=grey>OnPhotonJoinRoomFailed</color>");
		if (this.OnPhotonJoinRoomFailedEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnPhotonJoinRoomFailedEvent(codeAndMsg);
			});
		}
	}

	// Token: 0x060052B7 RID: 21175 RVA: 0x001C60E3 File Offset: 0x001C44E3
	public void OnCreatedRoom()
	{
		Debug.Log("<color=grey>OnCreatedRoom</color>");
		if (this.OnCreatedRoomEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnCreatedRoomEvent();
			});
		}
	}

	// Token: 0x060052B8 RID: 21176 RVA: 0x001C610B File Offset: 0x001C450B
	public void OnJoinedLobby()
	{
		Debug.Log("<color=grey>OnJoinedLobby</color>");
		if (this.OnJoinedLobbyEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnJoinedLobbyEvent();
			});
		}
	}

	// Token: 0x060052B9 RID: 21177 RVA: 0x001C6133 File Offset: 0x001C4533
	public void OnLeftLobby()
	{
		Debug.Log("<color=grey>OnLeftLobby</color>");
		if (this.OnLeftLobbyEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnLeftLobbyEvent();
			});
		}
	}

	// Token: 0x060052BA RID: 21178 RVA: 0x001C615C File Offset: 0x001C455C
	public void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.Log("<color=grey>OnFailedToConnectToPhoton</color>");
		if (this.OnFailedToConnectToPhotonEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnFailedToConnectToPhotonEvent(cause);
			});
		}
	}

	// Token: 0x060052BB RID: 21179 RVA: 0x001C61A4 File Offset: 0x001C45A4
	public void OnConnectionFail(DisconnectCause cause)
	{
		Debug.Log("<color=grey>OnConnectionFail</color>");
		if (this.OnConnectionFailEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnConnectionFailEvent(cause);
			});
		}
	}

	// Token: 0x060052BC RID: 21180 RVA: 0x001C61EB File Offset: 0x001C45EB
	public void OnDisconnectedFromPhoton()
	{
		Debug.Log("<color=grey>OnDisconnectedFromPhoton</color>");
		if (this.OnDisconnectedFromPhotonEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnDisconnectedFromPhotonEvent();
			});
		}
	}

	// Token: 0x060052BD RID: 21181 RVA: 0x001C6214 File Offset: 0x001C4614
	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		Debug.Log("<color=grey>OnPhotonInstantiate</color>");
		if (this.OnPhotonInstantiateEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnPhotonInstantiateEvent(info);
			});
		}
	}

	// Token: 0x060052BE RID: 21182 RVA: 0x001C625B File Offset: 0x001C465B
	public void OnReceivedRoomListUpdate()
	{
		Debug.Log("<color=grey>OnReceivedRoomListUpdate</color>");
		if (this.OnReceivedRoomListUpdateEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnReceivedRoomListUpdateEvent();
			});
		}
	}

	// Token: 0x060052BF RID: 21183 RVA: 0x001C6283 File Offset: 0x001C4683
	public void OnJoinedRoom()
	{
		Debug.Log("<color=grey>OnJoinedRoom</color>");
		if (this.OnJoinedRoomEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnJoinedRoomEvent();
			});
		}
	}

	// Token: 0x060052C0 RID: 21184 RVA: 0x001C62AC File Offset: 0x001C46AC
	public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		Debug.Log("<color=grey>OnPhotonPlayerConnected</color>");
		if (this.OnPhotonPlayerConnectedEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnPhotonPlayerConnectedEvent(newPlayer);
			});
		}
	}

	// Token: 0x060052C1 RID: 21185 RVA: 0x001C62F4 File Offset: 0x001C46F4
	public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		Debug.Log("<color=grey>OnPhotonPlayerDisconnected</color>");
		if (this.OnPhotonPlayerDisconnectedEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnPhotonPlayerDisconnectedEvent(otherPlayer);
			});
		}
	}

	// Token: 0x060052C2 RID: 21186 RVA: 0x001C633C File Offset: 0x001C473C
	public void OnPhotonRandomJoinFailed(object[] codeAndMsg)
	{
		Debug.Log("<color=grey>OnPhotonRandomJoinFailed</color>");
		if (this.OnPhotonRandomJoinFailedEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnPhotonRandomJoinFailedEvent(codeAndMsg);
			});
		}
	}

	// Token: 0x060052C3 RID: 21187 RVA: 0x001C6383 File Offset: 0x001C4783
	public void OnConnectedToMaster()
	{
		Debug.Log("<color=grey>OnConnectedToMaster</color>");
		if (this.OnConnectedToMasterEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnConnectedToMasterEvent();
			});
		}
	}

	// Token: 0x060052C4 RID: 21188 RVA: 0x001C63AB File Offset: 0x001C47AB
	public void OnPhotonMaxCccuReached()
	{
		Debug.Log("<color=grey>OnPhotonMaxCccuReached</color>");
		if (this.OnPhotonMaxCccuReachedEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnPhotonMaxCccuReachedEvent();
			});
		}
	}

	// Token: 0x060052C5 RID: 21189 RVA: 0x001C63D4 File Offset: 0x001C47D4
	public void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
	{
		Debug.Log("<color=grey>OnPhotonCustomRoomPropertiesChanged</color>");
		if (this.OnPhotonCustomRoomPropertiesChangedEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnPhotonCustomRoomPropertiesChangedEvent(propertiesThatChanged);
			});
		}
	}

	// Token: 0x060052C6 RID: 21190 RVA: 0x001C641C File Offset: 0x001C481C
	public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
	{
		Debug.Log("<color=grey>OnPhotonPlayerPropertiesChanged</color>");
		if (this.OnPhotonPlayerPropertiesChangedEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnPhotonPlayerPropertiesChangedEvent(playerAndUpdatedProps);
			});
		}
	}

	// Token: 0x060052C7 RID: 21191 RVA: 0x001C6463 File Offset: 0x001C4863
	public void OnUpdatedFriendList()
	{
		Debug.Log("<color=grey>OnUpdatedFriendList</color>");
		if (this.OnUpdatedFriendListEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnUpdatedFriendListEvent();
			});
		}
	}

	// Token: 0x060052C8 RID: 21192 RVA: 0x001C648C File Offset: 0x001C488C
	public void OnCustomAuthenticationFailed(string debugMessage)
	{
		Debug.Log("<color=grey>OnCustomAuthenticationFailed</color>");
		if (this.OnCustomAuthenticationFailedEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnCustomAuthenticationFailedEvent(debugMessage);
			});
		}
	}

	// Token: 0x060052C9 RID: 21193 RVA: 0x001C64D4 File Offset: 0x001C48D4
	public void OnWebRpcResponse(OperationResponse response)
	{
		Debug.Log("<color=grey>OnWebRpcResponse</color>");
		if (this.OnWebRpcResponseEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnWebRpcResponseEvent(response);
			});
		}
	}

	// Token: 0x060052CA RID: 21194 RVA: 0x001C651C File Offset: 0x001C491C
	public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
	{
		Debug.Log("<color=grey>OnCustomAuthenticationResponse</color>");
		if (this.OnCustomAuthenticationResponseEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnCustomAuthenticationResponseEvent(data);
			});
		}
	}

	// Token: 0x060052CB RID: 21195 RVA: 0x001C6563 File Offset: 0x001C4963
	public void OnLobbyStatisticsUpdate()
	{
		Debug.Log("<color=grey>OnLobbyStatisticsUpdate</color>");
		if (this.OnLobbyStatisticsUpdateEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnLobbyStatisticsUpdateEvent();
			});
		}
	}

	// Token: 0x060052CC RID: 21196 RVA: 0x001C658C File Offset: 0x001C498C
	public void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer)
	{
		Debug.Log("<color=grey>OnPhotonPlayerActivityChanged</color>");
		if (this.OnPhotonPlayerActivityChangedEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnPhotonPlayerActivityChangedEvent(VRC.Network.GetPlayer(otherPlayer));
			});
		}
	}

	// Token: 0x060052CD RID: 21197 RVA: 0x001C65D4 File Offset: 0x001C49D4
	public void OnOwnershipTransfered(object[] viewAndPlayers)
	{
		PhotonView view = viewAndPlayers[0] as PhotonView;
		if (view != null)
		{
			view.ownershipTransferTime = (double)Time.time;
		}
		PhotonPlayer player1 = viewAndPlayers[1] as PhotonPlayer;
		PhotonPlayer player2 = viewAndPlayers[2] as PhotonPlayer;
		Debug.LogFormat("<color=grey>OnOwnershipTransfered {0} from {1} to {2}</color>", new object[]
		{
			(!(view != null)) ? "(null)" : view.name,
			(player2 == null) ? "(null)" : player2.ID.ToString(),
			(player1 == null) ? "(null)" : player1.ID.ToString()
		});
		if (this.OnOwnershipTransferedEvent != null)
		{
			VRCWorker.DispatchToMain(delegate
			{
				this.OnOwnershipTransferedEvent(view.gameObject, VRC.Network.GetPlayer(player1), VRC.Network.GetPlayer(player2));
			});
		}
	}

	// Token: 0x04003A77 RID: 14967
	public VRCEventDelegate<Player> OnPlayerJoinedEvent = new VRCEventDelegate<Player>();

	// Token: 0x04003A78 RID: 14968
	public VRCEventDelegate<Player> OnPlayerLeftEvent = new VRCEventDelegate<Player>();

	// Token: 0x04003A79 RID: 14969
	public static NetworkManager Instance;

	// Token: 0x04003A7A RID: 14970
	public float updatePeriod = 30f;

	// Token: 0x04003A7B RID: 14971
	private float timeSinceUpdate;

	// Token: 0x04003A7C RID: 14972
	private ApiWorldUpdate worldUpdate;
}
