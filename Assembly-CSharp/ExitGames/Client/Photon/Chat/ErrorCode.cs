using System;

namespace ExitGames.Client.Photon.Chat
{
	// Token: 0x020007B4 RID: 1972
	public class ErrorCode
	{
		// Token: 0x0400280D RID: 10253
		public const int Ok = 0;

		// Token: 0x0400280E RID: 10254
		public const int OperationNotAllowedInCurrentState = -3;

		// Token: 0x0400280F RID: 10255
		public const int InvalidOperationCode = -2;

		// Token: 0x04002810 RID: 10256
		public const int InternalServerError = -1;

		// Token: 0x04002811 RID: 10257
		public const int InvalidAuthentication = 32767;

		// Token: 0x04002812 RID: 10258
		public const int GameIdAlreadyExists = 32766;

		// Token: 0x04002813 RID: 10259
		public const int GameFull = 32765;

		// Token: 0x04002814 RID: 10260
		public const int GameClosed = 32764;

		// Token: 0x04002815 RID: 10261
		public const int ServerFull = 32762;

		// Token: 0x04002816 RID: 10262
		public const int UserBlocked = 32761;

		// Token: 0x04002817 RID: 10263
		public const int NoRandomMatchFound = 32760;

		// Token: 0x04002818 RID: 10264
		public const int GameDoesNotExist = 32758;

		// Token: 0x04002819 RID: 10265
		public const int MaxCcuReached = 32757;

		// Token: 0x0400281A RID: 10266
		public const int InvalidRegion = 32756;

		// Token: 0x0400281B RID: 10267
		public const int CustomAuthenticationFailed = 32755;
	}
}
