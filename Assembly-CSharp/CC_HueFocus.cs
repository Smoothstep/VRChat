using System;
using UnityEngine;

// Token: 0x02000435 RID: 1077
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Hue Focus")]
public class CC_HueFocus : CC_Base
{
	// Token: 0x0600269F RID: 9887 RVA: 0x000BE714 File Offset: 0x000BCB14
	[ImageEffectTransformsToLDR]
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		float num = this.hue / 360f;
		float num2 = this.range / 180f;
		base.material.SetVector("_Range", new Vector2(num - num2, num + num2));
		base.material.SetVector("_Params", new Vector3(num, this.boost + 1f, this.amount));
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040013AD RID: 5037
	[Range(0f, 360f)]
	public float hue;

	// Token: 0x040013AE RID: 5038
	[Range(1f, 180f)]
	public float range = 30f;

	// Token: 0x040013AF RID: 5039
	[Range(0f, 1f)]
	public float boost = 0.5f;

	// Token: 0x040013B0 RID: 5040
	[Range(0f, 1f)]
	public float amount = 1f;
}
