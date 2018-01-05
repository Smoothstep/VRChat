using System;

// Token: 0x02000750 RID: 1872
public enum DisconnectCause
{
	// Token: 0x040025C6 RID: 9670
	DisconnectByServerUserLimit = 1042,
	// Token: 0x040025C7 RID: 9671
	ExceptionOnConnect = 1023,
	// Token: 0x040025C8 RID: 9672
	DisconnectByServerTimeout = 1041,
	// Token: 0x040025C9 RID: 9673
	DisconnectByServerLogic = 1043,
	// Token: 0x040025CA RID: 9674
	Exception = 1026,
	// Token: 0x040025CB RID: 9675
	InvalidAuthentication = 32767,
	// Token: 0x040025CC RID: 9676
	MaxCcuReached = 32757,
	// Token: 0x040025CD RID: 9677
	InvalidRegion = 32756,
	// Token: 0x040025CE RID: 9678
	SecurityExceptionOnConnect = 1022,
	// Token: 0x040025CF RID: 9679
	DisconnectByClientTimeout = 1040,
	// Token: 0x040025D0 RID: 9680
	InternalReceiveException = 1039,
	// Token: 0x040025D1 RID: 9681
	AuthenticationTicketExpired = 32753
}
