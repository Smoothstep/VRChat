using System;
using UnityEngine;

// Token: 0x02000424 RID: 1060
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Brightness, Contrast, Gamma")]
public class CC_BrightnessContrastGamma : CC_Base
{
	// Token: 0x06002678 RID: 9848 RVA: 0x000BD8D0 File Offset: 0x000BBCD0
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.brightness == 0f && this.contrast == 0f && this.gamma == 1f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetVector("_BCG", new Vector4((this.brightness + 100f) * 0.01f, (this.contrast + 100f) * 0.01f, 1f / this.gamma));
		base.material.SetVector("_Coeffs", new Vector4(this.redCoeff, this.greenCoeff, this.blueCoeff, 1f));
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04001353 RID: 4947
	[Range(-100f, 100f)]
	public float brightness;

	// Token: 0x04001354 RID: 4948
	[Range(-100f, 100f)]
	public float contrast;

	// Token: 0x04001355 RID: 4949
	[Range(0f, 1f)]
	public float redCoeff = 0.5f;

	// Token: 0x04001356 RID: 4950
	[Range(0f, 1f)]
	public float greenCoeff = 0.5f;

	// Token: 0x04001357 RID: 4951
	[Range(0f, 1f)]
	public float blueCoeff = 0.5f;

	// Token: 0x04001358 RID: 4952
	[Range(0.1f, 9.9f)]
	public float gamma = 1f;
}
