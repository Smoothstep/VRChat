using System;
using UnityEngine;

// Token: 0x02000428 RID: 1064
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Contrast Vignette")]
public class CC_ContrastVignette : CC_Base
{
	// Token: 0x06002681 RID: 9857 RVA: 0x000BDD10 File Offset: 0x000BC110
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_Data", new Vector4(this.sharpness * 0.01f, this.darkness * 0.02f, this.contrast * 0.01f, this.edge * 0.01f));
		base.material.SetVector("_Coeffs", new Vector4(this.redCoeff, this.greenCoeff, this.blueCoeff, 1f));
		base.material.SetVector("_Center", this.center);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x0400136D RID: 4973
	public Vector2 center = new Vector2(0.5f, 0.5f);

	// Token: 0x0400136E RID: 4974
	[Range(-100f, 100f)]
	public float sharpness = 32f;

	// Token: 0x0400136F RID: 4975
	[Range(0f, 100f)]
	public float darkness = 28f;

	// Token: 0x04001370 RID: 4976
	[Range(0f, 200f)]
	public float contrast = 20f;

	// Token: 0x04001371 RID: 4977
	[Range(0f, 1f)]
	public float redCoeff = 0.5f;

	// Token: 0x04001372 RID: 4978
	[Range(0f, 1f)]
	public float greenCoeff = 0.5f;

	// Token: 0x04001373 RID: 4979
	[Range(0f, 1f)]
	public float blueCoeff = 0.5f;

	// Token: 0x04001374 RID: 4980
	[Range(0f, 200f)]
	public float edge;
}
