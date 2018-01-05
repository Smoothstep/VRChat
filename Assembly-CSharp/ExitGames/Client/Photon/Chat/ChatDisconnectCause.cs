using System;

namespace ExitGames.Client.Photon.Chat
{
	// Token: 0x020007AC RID: 1964
	public enum ChatDisconnectCause
	{
		// Token: 0x040027C8 RID: 10184
		None,
		// Token: 0x040027C9 RID: 10185
		DisconnectByServerUserLimit,
		// Token: 0x040027CA RID: 10186
		ExceptionOnConnect,
		// Token: 0x040027CB RID: 10187
		DisconnectByServer,
		// Token: 0x040027CC RID: 10188
		TimeoutDisconnect,
		// Token: 0x040027CD RID: 10189
		Exception,
		// Token: 0x040027CE RID: 10190
		InvalidAuthentication,
		// Token: 0x040027CF RID: 10191
		MaxCcuReached,
		// Token: 0x040027D0 RID: 10192
		InvalidRegion,
		// Token: 0x040027D1 RID: 10193
		OperationNotAllowedInCurrentState,
		// Token: 0x040027D2 RID: 10194
		CustomAuthenticationFailed
	}
}
