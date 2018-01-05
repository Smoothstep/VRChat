using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;

// Token: 0x02000754 RID: 1876
public interface IPunCallbacks
{
	// Token: 0x06003C8C RID: 15500
	void OnConnectedToPhoton();

	// Token: 0x06003C8D RID: 15501
	void OnLeftRoom();

	// Token: 0x06003C8E RID: 15502
	void OnMasterClientSwitched(PhotonPlayer newMasterClient);

	// Token: 0x06003C8F RID: 15503
	void OnPhotonCreateRoomFailed(object[] codeAndMsg);

	// Token: 0x06003C90 RID: 15504
	void OnPhotonJoinRoomFailed(object[] codeAndMsg);

	// Token: 0x06003C91 RID: 15505
	void OnCreatedRoom();

	// Token: 0x06003C92 RID: 15506
	void OnJoinedLobby();

	// Token: 0x06003C93 RID: 15507
	void OnLeftLobby();

	// Token: 0x06003C94 RID: 15508
	void OnFailedToConnectToPhoton(DisconnectCause cause);

	// Token: 0x06003C95 RID: 15509
	void OnConnectionFail(DisconnectCause cause);

	// Token: 0x06003C96 RID: 15510
	void OnDisconnectedFromPhoton();

	// Token: 0x06003C97 RID: 15511
	void OnPhotonInstantiate(PhotonMessageInfo info);

	// Token: 0x06003C98 RID: 15512
	void OnReceivedRoomListUpdate();

	// Token: 0x06003C99 RID: 15513
	void OnJoinedRoom();

	// Token: 0x06003C9A RID: 15514
	void OnPhotonPlayerConnected(PhotonPlayer newPlayer);

	// Token: 0x06003C9B RID: 15515
	void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer);

	// Token: 0x06003C9C RID: 15516
	void OnPhotonRandomJoinFailed(object[] codeAndMsg);

	// Token: 0x06003C9D RID: 15517
	void OnConnectedToMaster();

	// Token: 0x06003C9E RID: 15518
	void OnPhotonMaxCccuReached();

	// Token: 0x06003C9F RID: 15519
	void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged);

	// Token: 0x06003CA0 RID: 15520
	void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps);

	// Token: 0x06003CA1 RID: 15521
	void OnUpdatedFriendList();

	// Token: 0x06003CA2 RID: 15522
	void OnCustomAuthenticationFailed(string debugMessage);

	// Token: 0x06003CA3 RID: 15523
	void OnCustomAuthenticationResponse(Dictionary<string, object> data);

	// Token: 0x06003CA4 RID: 15524
	void OnWebRpcResponse(OperationResponse response);

	// Token: 0x06003CA5 RID: 15525
	void OnOwnershipRequest(object[] viewAndPlayer);

	// Token: 0x06003CA6 RID: 15526
	void OnLobbyStatisticsUpdate();

	// Token: 0x06003CA7 RID: 15527
	void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer);

	// Token: 0x06003CA8 RID: 15528
	void OnOwnershipTransfered(object[] viewAndPlayers);
}
