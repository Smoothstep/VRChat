using System;
using UnityEngine;

// Token: 0x02000432 RID: 1074
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Gradient Ramp")]
public class CC_GradientRamp : CC_Base
{
	// Token: 0x06002698 RID: 9880 RVA: 0x000BE4F8 File Offset: 0x000BC8F8
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.rampTexture == null || this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetTexture("_RampTex", this.rampTexture);
		base.material.SetFloat("_Amount", this.amount);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040013A2 RID: 5026
	public Texture rampTexture;

	// Token: 0x040013A3 RID: 5027
	[Range(0f, 1f)]
	public float amount = 1f;
}
