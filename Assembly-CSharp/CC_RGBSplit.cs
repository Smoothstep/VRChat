using System;
using UnityEngine;

// Token: 0x0200043F RID: 1087
[ExecuteInEditMode]
[AddComponentMenu("Colorful/RGB Split")]
public class CC_RGBSplit : CC_Base
{
	// Token: 0x060026BE RID: 9918 RVA: 0x000BF120 File Offset: 0x000BD520
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetFloat("_RGBShiftAmount", this.amount * 0.001f);
		base.material.SetFloat("_RGBShiftAngleCos", Mathf.Cos(this.angle));
		base.material.SetFloat("_RGBShiftAngleSin", Mathf.Sin(this.angle));
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040013F2 RID: 5106
	public float amount;

	// Token: 0x040013F3 RID: 5107
	public float angle;
}
