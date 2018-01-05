using System;
using ExitGames.Client.Photon;

// Token: 0x02000739 RID: 1849
internal class OpJoinRandomRoomParams
{
	// Token: 0x040024C1 RID: 9409
	public Hashtable ExpectedCustomRoomProperties;

	// Token: 0x040024C2 RID: 9410
	public byte ExpectedMaxPlayers;

	// Token: 0x040024C3 RID: 9411
	public MatchmakingMode MatchingType;

	// Token: 0x040024C4 RID: 9412
	public TypedLobby TypedLobby;

	// Token: 0x040024C5 RID: 9413
	public string SqlLobbyFilter;

	// Token: 0x040024C6 RID: 9414
	public string[] ExpectedUsers;
}
