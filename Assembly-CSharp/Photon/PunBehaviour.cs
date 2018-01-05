using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;

namespace Photon
{
	// Token: 0x02000757 RID: 1879
	public class PunBehaviour : MonoBehaviour, IPunCallbacks
	{
		// Token: 0x06003CAE RID: 15534 RVA: 0x001338AD File Offset: 0x00131CAD
		public virtual void OnConnectedToPhoton()
		{
		}

		// Token: 0x06003CAF RID: 15535 RVA: 0x001338AF File Offset: 0x00131CAF
		public virtual void OnLeftRoom()
		{
		}

		// Token: 0x06003CB0 RID: 15536 RVA: 0x001338B1 File Offset: 0x00131CB1
		public virtual void OnMasterClientSwitched(PhotonPlayer newMasterClient)
		{
		}

		// Token: 0x06003CB1 RID: 15537 RVA: 0x001338B3 File Offset: 0x00131CB3
		public virtual void OnPhotonCreateRoomFailed(object[] codeAndMsg)
		{
		}

		// Token: 0x06003CB2 RID: 15538 RVA: 0x001338B5 File Offset: 0x00131CB5
		public virtual void OnPhotonJoinRoomFailed(object[] codeAndMsg)
		{
		}

		// Token: 0x06003CB3 RID: 15539 RVA: 0x001338B7 File Offset: 0x00131CB7
		public virtual void OnCreatedRoom()
		{
		}

		// Token: 0x06003CB4 RID: 15540 RVA: 0x001338B9 File Offset: 0x00131CB9
		public virtual void OnJoinedLobby()
		{
		}

		// Token: 0x06003CB5 RID: 15541 RVA: 0x001338BB File Offset: 0x00131CBB
		public virtual void OnLeftLobby()
		{
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x001338BD File Offset: 0x00131CBD
		public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
		{
		}

		// Token: 0x06003CB7 RID: 15543 RVA: 0x001338BF File Offset: 0x00131CBF
		public virtual void OnDisconnectedFromPhoton()
		{
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x001338C1 File Offset: 0x00131CC1
		public virtual void OnConnectionFail(DisconnectCause cause)
		{
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x001338C3 File Offset: 0x00131CC3
		public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
		{
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x001338C5 File Offset: 0x00131CC5
		public virtual void OnReceivedRoomListUpdate()
		{
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x001338C7 File Offset: 0x00131CC7
		public virtual void OnJoinedRoom()
		{
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x001338C9 File Offset: 0x00131CC9
		public virtual void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
		{
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x001338CB File Offset: 0x00131CCB
		public virtual void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x001338CD File Offset: 0x00131CCD
		public virtual void OnPhotonRandomJoinFailed(object[] codeAndMsg)
		{
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x001338CF File Offset: 0x00131CCF
		public virtual void OnConnectedToMaster()
		{
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x001338D1 File Offset: 0x00131CD1
		public virtual void OnPhotonMaxCccuReached()
		{
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x001338D3 File Offset: 0x00131CD3
		public virtual void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
		{
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x001338D5 File Offset: 0x00131CD5
		public virtual void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
		{
		}

		// Token: 0x06003CC3 RID: 15555 RVA: 0x001338D7 File Offset: 0x00131CD7
		public virtual void OnUpdatedFriendList()
		{
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x001338D9 File Offset: 0x00131CD9
		public virtual void OnCustomAuthenticationFailed(string debugMessage)
		{
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x001338DB File Offset: 0x00131CDB
		public virtual void OnCustomAuthenticationResponse(Dictionary<string, object> data)
		{
		}

		// Token: 0x06003CC6 RID: 15558 RVA: 0x001338DD File Offset: 0x00131CDD
		public virtual void OnWebRpcResponse(OperationResponse response)
		{
		}

		// Token: 0x06003CC7 RID: 15559 RVA: 0x001338DF File Offset: 0x00131CDF
		public virtual void OnOwnershipRequest(object[] viewAndPlayer)
		{
		}

		// Token: 0x06003CC8 RID: 15560 RVA: 0x001338E1 File Offset: 0x00131CE1
		public virtual void OnLobbyStatisticsUpdate()
		{
		}

		// Token: 0x06003CC9 RID: 15561 RVA: 0x001338E3 File Offset: 0x00131CE3
		public virtual void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer)
		{
		}

		// Token: 0x06003CCA RID: 15562 RVA: 0x001338E5 File Offset: 0x00131CE5
		public virtual void OnOwnershipTransfered(object[] viewAndPlayers)
		{
		}
	}
}
