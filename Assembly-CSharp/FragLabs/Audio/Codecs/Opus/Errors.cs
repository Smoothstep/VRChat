using System;

namespace FragLabs.Audio.Codecs.Opus
{
	// Token: 0x0200070C RID: 1804
	public enum Errors
	{
		// Token: 0x040023B6 RID: 9142
		OK,
		// Token: 0x040023B7 RID: 9143
		BadArg = -1,
		// Token: 0x040023B8 RID: 9144
		BufferToSmall = -2,
		// Token: 0x040023B9 RID: 9145
		InternalError = -3,
		// Token: 0x040023BA RID: 9146
		InvalidPacket = -4,
		// Token: 0x040023BB RID: 9147
		Unimplemented = -5,
		// Token: 0x040023BC RID: 9148
		InvalidState = -6,
		// Token: 0x040023BD RID: 9149
		AllocFail = -7
	}
}
