using System;

namespace FragLabs.Audio.Codecs.Opus
{
	// Token: 0x0200070A RID: 1802
	public enum Ctl
	{
		// Token: 0x040023AA RID: 9130
		SetBitrateRequest = 4002,
		// Token: 0x040023AB RID: 9131
		GetBitrateRequest,
		// Token: 0x040023AC RID: 9132
		SetInbandFECRequest = 4012,
		// Token: 0x040023AD RID: 9133
		GetInbandFECRequest,
		// Token: 0x040023AE RID: 9134
		SetPacketLossPercRequest,
		// Token: 0x040023AF RID: 9135
		GetPacketLossPercRequest,
		// Token: 0x040023B0 RID: 9136
		OpusResetState = 4028
	}
}
