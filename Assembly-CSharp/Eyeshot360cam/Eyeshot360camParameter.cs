using System;

namespace Eyeshot360cam
{
	// Token: 0x0200046F RID: 1135
	[Flags]
	public enum Eyeshot360camParameter
	{
		// Token: 0x04001528 RID: 5416
		PanoramaName = 1,
		// Token: 0x04001529 RID: 5417
		QualitySetting = 2,
		// Token: 0x0400152A RID: 5418
		DefaultCaptureKey = 4,
		// Token: 0x0400152B RID: 5419
		SelfieCaptureKey = 8,
		// Token: 0x0400152C RID: 5420
		ImageFormat = 16,
		// Token: 0x0400152D RID: 5421
		PanoramaFormat = 32,
		// Token: 0x0400152E RID: 5422
		CaptureStereoscopic = 64,
		// Token: 0x0400152F RID: 5423
		InterpupillaryDistance = 128,
		// Token: 0x04001530 RID: 5424
		NumCirclePoints = 256,
		// Token: 0x04001531 RID: 5425
		PanoramaWidth = 512,
		// Token: 0x04001532 RID: 5426
		AntiAliasing = 1024,
		// Token: 0x04001533 RID: 5427
		SsaaFactor = 2048,
		// Token: 0x04001534 RID: 5428
		DepthMap = 4096,
		// Token: 0x04001535 RID: 5429
		DepthLevel = 8192,
		// Token: 0x04001536 RID: 5430
		DepthFar = 16384,
		// Token: 0x04001537 RID: 5431
		SaveImagePath = 32768,
		// Token: 0x04001538 RID: 5432
		SaveCubemap = 65536,
		// Token: 0x04001539 RID: 5433
		UploadImages = 131072,
		// Token: 0x0400153A RID: 5434
		SaveShortUrl = 262144,
		// Token: 0x0400153B RID: 5435
		UseDefaultOrientation = 524288,
		// Token: 0x0400153C RID: 5436
		UseGPUTransform = 1048576,
		// Token: 0x0400153D RID: 5437
		UserToken = 2097152,
		// Token: 0x0400153E RID: 5438
		Album = 4194304
	}
}
