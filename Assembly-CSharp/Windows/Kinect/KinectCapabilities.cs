using System;

namespace Windows.Kinect
{
	// Token: 0x02000503 RID: 1283
	[Flags]
	public enum KinectCapabilities : uint
	{
		// Token: 0x04001813 RID: 6163
		None = 0u,
		// Token: 0x04001814 RID: 6164
		Vision = 1u,
		// Token: 0x04001815 RID: 6165
		Audio = 2u,
		// Token: 0x04001816 RID: 6166
		Face = 4u,
		// Token: 0x04001817 RID: 6167
		Expressions = 8u,
		// Token: 0x04001818 RID: 6168
		Gamechat = 16u
	}
}
