using System;

// Token: 0x0200073B RID: 1851
public class ErrorCode
{
	// Token: 0x040024CF RID: 9423
	public const int Ok = 0;

	// Token: 0x040024D0 RID: 9424
	public const int OperationNotAllowedInCurrentState = -3;

	// Token: 0x040024D1 RID: 9425
	[Obsolete("Use InvalidOperation.")]
	public const int InvalidOperationCode = -2;

	// Token: 0x040024D2 RID: 9426
	public const int InvalidOperation = -2;

	// Token: 0x040024D3 RID: 9427
	public const int InternalServerError = -1;

	// Token: 0x040024D4 RID: 9428
	public const int InvalidAuthentication = 32767;

	// Token: 0x040024D5 RID: 9429
	public const int GameIdAlreadyExists = 32766;

	// Token: 0x040024D6 RID: 9430
	public const int GameFull = 32765;

	// Token: 0x040024D7 RID: 9431
	public const int GameClosed = 32764;

	// Token: 0x040024D8 RID: 9432
	[Obsolete("No longer used, cause random matchmaking is no longer a process.")]
	public const int AlreadyMatched = 32763;

	// Token: 0x040024D9 RID: 9433
	public const int ServerFull = 32762;

	// Token: 0x040024DA RID: 9434
	public const int UserBlocked = 32761;

	// Token: 0x040024DB RID: 9435
	public const int NoRandomMatchFound = 32760;

	// Token: 0x040024DC RID: 9436
	public const int GameDoesNotExist = 32758;

	// Token: 0x040024DD RID: 9437
	public const int MaxCcuReached = 32757;

	// Token: 0x040024DE RID: 9438
	public const int InvalidRegion = 32756;

	// Token: 0x040024DF RID: 9439
	public const int CustomAuthenticationFailed = 32755;

	// Token: 0x040024E0 RID: 9440
	public const int AuthenticationTicketExpired = 32753;

	// Token: 0x040024E1 RID: 9441
	public const int PluginReportedError = 32752;

	// Token: 0x040024E2 RID: 9442
	public const int PluginMismatch = 32751;

	// Token: 0x040024E3 RID: 9443
	public const int JoinFailedPeerAlreadyJoined = 32750;

	// Token: 0x040024E4 RID: 9444
	public const int JoinFailedFoundInactiveJoiner = 32749;

	// Token: 0x040024E5 RID: 9445
	public const int JoinFailedWithRejoinerNotFound = 32748;

	// Token: 0x040024E6 RID: 9446
	public const int JoinFailedFoundExcludedUserId = 32747;

	// Token: 0x040024E7 RID: 9447
	public const int JoinFailedFoundActiveJoiner = 32746;

	// Token: 0x040024E8 RID: 9448
	public const int HttpLimitReached = 32745;

	// Token: 0x040024E9 RID: 9449
	public const int ExternalHttpCallFailed = 32744;

	// Token: 0x040024EA RID: 9450
	public const int SlotError = 32742;

	// Token: 0x040024EB RID: 9451
	public const int InvalidEncryptionParameters = 32741;
}
