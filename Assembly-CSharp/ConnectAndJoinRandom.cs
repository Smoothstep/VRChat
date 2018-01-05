using System;
using Photon;
using UnityEngine;

// Token: 0x02000784 RID: 1924
public class ConnectAndJoinRandom : Photon.MonoBehaviour
{
	// Token: 0x06003E8F RID: 16015 RVA: 0x0013B44E File Offset: 0x0013984E
	public virtual void Start()
	{
		PhotonNetwork.autoJoinLobby = false;
	}

	// Token: 0x06003E90 RID: 16016 RVA: 0x0013B458 File Offset: 0x00139858
	public virtual void Update()
	{
		if (this.ConnectInUpdate && this.AutoConnect && !PhotonNetwork.connected)
		{
			Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
			this.ConnectInUpdate = false;
			PhotonNetwork.ConnectUsingSettings(this.Version + "." + SceneManagerHelper.ActiveSceneBuildIndex, false);
		}
	}

	// Token: 0x06003E91 RID: 16017 RVA: 0x0013B4BC File Offset: 0x001398BC
	public virtual void OnConnectedToMaster()
	{
		Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
		PhotonNetwork.JoinRandomRoom();
	}

	// Token: 0x06003E92 RID: 16018 RVA: 0x0013B4CE File Offset: 0x001398CE
	public virtual void OnJoinedLobby()
	{
		Debug.Log("OnJoinedLobby(). This client is connected and does get a room-list, which gets stored as PhotonNetwork.GetRoomList(). This script now calls: PhotonNetwork.JoinRandomRoom();");
		PhotonNetwork.JoinRandomRoom();
	}

	// Token: 0x06003E93 RID: 16019 RVA: 0x0013B4E0 File Offset: 0x001398E0
	public virtual void OnPhotonRandomJoinFailed()
	{
		Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
		PhotonNetwork.CreateRoom(null, new RoomOptions
		{
			MaxPlayers = 4
		}, null);
	}

	// Token: 0x06003E94 RID: 16020 RVA: 0x0013B50D File Offset: 0x0013990D
	public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.LogError("Cause: " + cause);
	}

	// Token: 0x06003E95 RID: 16021 RVA: 0x0013B524 File Offset: 0x00139924
	public void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
	}

	// Token: 0x0400272E RID: 10030
	public bool AutoConnect = true;

	// Token: 0x0400272F RID: 10031
	public byte Version = 1;

	// Token: 0x04002730 RID: 10032
	private bool ConnectInUpdate = true;
}
