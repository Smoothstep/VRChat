using System;

namespace Windows.Kinect
{
	// Token: 0x020004F3 RID: 1267
	[Flags]
	public enum FrameEdges : uint
	{
		// Token: 0x040017BF RID: 6079
		None = 0u,
		// Token: 0x040017C0 RID: 6080
		Right = 1u,
		// Token: 0x040017C1 RID: 6081
		Left = 2u,
		// Token: 0x040017C2 RID: 6082
		Top = 4u,
		// Token: 0x040017C3 RID: 6083
		Bottom = 8u
	}
}
