using System;
using ExitGames.Client.Photon;

// Token: 0x0200073A RID: 1850
internal class EnterRoomParams
{
	// Token: 0x040024C7 RID: 9415
	public string RoomName;

	// Token: 0x040024C8 RID: 9416
	public RoomOptions RoomOptions;

	// Token: 0x040024C9 RID: 9417
	public TypedLobby Lobby;

	// Token: 0x040024CA RID: 9418
	public Hashtable PlayerProperties;

	// Token: 0x040024CB RID: 9419
	public bool OnGameServer = true;

	// Token: 0x040024CC RID: 9420
	public bool CreateIfNotExists;

	// Token: 0x040024CD RID: 9421
	public bool RejoinOnly;

	// Token: 0x040024CE RID: 9422
	public string[] ExpectedUsers;
}
