using System;

namespace VolumetricFogAndMist
{
	// Token: 0x020009B3 RID: 2483
	internal interface IVolumetricFogRenderComponent
	{
		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x06004ADE RID: 19166
		// (set) Token: 0x06004ADF RID: 19167
		VolumetricFog fog { get; set; }

		// Token: 0x06004AE0 RID: 19168
		void DestroySelf();
	}
}
