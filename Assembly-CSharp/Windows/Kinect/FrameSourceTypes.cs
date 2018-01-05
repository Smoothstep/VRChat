using System;

namespace Windows.Kinect
{
	// Token: 0x020004F4 RID: 1268
	[Flags]
	public enum FrameSourceTypes : uint
	{
		// Token: 0x040017C5 RID: 6085
		None = 0u,
		// Token: 0x040017C6 RID: 6086
		Color = 1u,
		// Token: 0x040017C7 RID: 6087
		Infrared = 2u,
		// Token: 0x040017C8 RID: 6088
		LongExposureInfrared = 4u,
		// Token: 0x040017C9 RID: 6089
		Depth = 8u,
		// Token: 0x040017CA RID: 6090
		BodyIndex = 16u,
		// Token: 0x040017CB RID: 6091
		Body = 32u,
		// Token: 0x040017CC RID: 6092
		Audio = 64u
	}
}
