using System;
using UnityEngine;

// Token: 0x02000443 RID: 1091
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Threshold")]
public class CC_Threshold : CC_Base
{
	// Token: 0x060026C6 RID: 9926 RVA: 0x000BF3DC File Offset: 0x000BD7DC
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Threshold", this.threshold / 255f);
		base.material.SetFloat("_Range", this.noiseRange / 255f);
		Graphics.Blit(source, destination, base.material, (!this.useNoise) ? 0 : 1);
	}

	// Token: 0x040013FD RID: 5117
	[Range(1f, 255f)]
	public float threshold = 128f;

	// Token: 0x040013FE RID: 5118
	[Range(0f, 128f)]
	public float noiseRange = 48f;

	// Token: 0x040013FF RID: 5119
	public bool useNoise;
}
