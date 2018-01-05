using System;
using UnityEngine;

// Token: 0x02000440 RID: 1088
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Radial Blur")]
public class CC_RadialBlur : CC_Base
{
	// Token: 0x060026C0 RID: 9920 RVA: 0x000BF1DC File Offset: 0x000BD5DC
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetFloat("_Amount", this.amount);
		base.material.SetVector("_Center", this.center);
		base.material.SetFloat("_Samples", (float)this.samples);
		Graphics.Blit(source, destination, base.material, this.quality);
	}

	// Token: 0x040013F4 RID: 5108
	[Range(0f, 1f)]
	public float amount = 0.1f;

	// Token: 0x040013F5 RID: 5109
	[Range(2f, 24f)]
	public int samples = 10;

	// Token: 0x040013F6 RID: 5110
	public Vector2 center = new Vector2(0.5f, 0.5f);

	// Token: 0x040013F7 RID: 5111
	public int quality = 1;
}
