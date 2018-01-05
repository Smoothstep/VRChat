using System;

namespace ExitGames.Client.Photon.Chat
{
	// Token: 0x020007B5 RID: 1973
	public enum ChatState
	{
		// Token: 0x0400281D RID: 10269
		Uninitialized,
		// Token: 0x0400281E RID: 10270
		ConnectingToNameServer,
		// Token: 0x0400281F RID: 10271
		ConnectedToNameServer,
		// Token: 0x04002820 RID: 10272
		Authenticating,
		// Token: 0x04002821 RID: 10273
		Authenticated,
		// Token: 0x04002822 RID: 10274
		DisconnectingFromNameServer,
		// Token: 0x04002823 RID: 10275
		ConnectingToFrontEnd,
		// Token: 0x04002824 RID: 10276
		ConnectedToFrontEnd,
		// Token: 0x04002825 RID: 10277
		DisconnectingFromFrontEnd,
		// Token: 0x04002826 RID: 10278
		QueuedComingFromFrontEnd,
		// Token: 0x04002827 RID: 10279
		Disconnecting,
		// Token: 0x04002828 RID: 10280
		Disconnected
	}
}
