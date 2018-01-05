using System;

// Token: 0x0200074E RID: 1870
public enum ClientState
{
	// Token: 0x040025AB RID: 9643
	Uninitialized,
	// Token: 0x040025AC RID: 9644
	PeerCreated,
	// Token: 0x040025AD RID: 9645
	Queued,
	// Token: 0x040025AE RID: 9646
	Authenticated,
	// Token: 0x040025AF RID: 9647
	JoinedLobby,
	// Token: 0x040025B0 RID: 9648
	DisconnectingFromMasterserver,
	// Token: 0x040025B1 RID: 9649
	ConnectingToGameserver,
	// Token: 0x040025B2 RID: 9650
	ConnectedToGameserver,
	// Token: 0x040025B3 RID: 9651
	Joining,
	// Token: 0x040025B4 RID: 9652
	Joined,
	// Token: 0x040025B5 RID: 9653
	Leaving,
	// Token: 0x040025B6 RID: 9654
	DisconnectingFromGameserver,
	// Token: 0x040025B7 RID: 9655
	ConnectingToMasterserver,
	// Token: 0x040025B8 RID: 9656
	QueuedComingFromGameserver,
	// Token: 0x040025B9 RID: 9657
	Disconnecting,
	// Token: 0x040025BA RID: 9658
	Disconnected,
	// Token: 0x040025BB RID: 9659
	ConnectedToMaster,
	// Token: 0x040025BC RID: 9660
	ConnectingToNameServer,
	// Token: 0x040025BD RID: 9661
	ConnectedToNameServer,
	// Token: 0x040025BE RID: 9662
	DisconnectingFromNameServer,
	// Token: 0x040025BF RID: 9663
	Authenticating
}
