using System;
using UnityEngine;

// Token: 0x02000439 RID: 1081
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Levels")]
public class CC_Levels : CC_Base
{
	// Token: 0x170005ED RID: 1517
	// (get) Token: 0x060026AF RID: 9903 RVA: 0x000BEBF3 File Offset: 0x000BCFF3
	// (set) Token: 0x060026B0 RID: 9904 RVA: 0x000BEC07 File Offset: 0x000BD007
	public int mode
	{
		get
		{
			return (!this.isRGB) ? 0 : 1;
		}
		set
		{
			this.isRGB = (value > 0);
		}
	}

	// Token: 0x060026B1 RID: 9905 RVA: 0x000BEC20 File Offset: 0x000BD020
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.isRGB)
		{
			base.material.SetVector("_InputMin", new Vector4(this.inputMinL / 255f, this.inputMinL / 255f, this.inputMinL / 255f, 1f));
			base.material.SetVector("_InputMax", new Vector4(this.inputMaxL / 255f, this.inputMaxL / 255f, this.inputMaxL / 255f, 1f));
			base.material.SetVector("_InputGamma", new Vector4(this.inputGammaL, this.inputGammaL, this.inputGammaL, 1f));
			base.material.SetVector("_OutputMin", new Vector4(this.outputMinL / 255f, this.outputMinL / 255f, this.outputMinL / 255f, 1f));
			base.material.SetVector("_OutputMax", new Vector4(this.outputMaxL / 255f, this.outputMaxL / 255f, this.outputMaxL / 255f, 1f));
		}
		else
		{
			base.material.SetVector("_InputMin", new Vector4(this.inputMinR / 255f, this.inputMinG / 255f, this.inputMinB / 255f, 1f));
			base.material.SetVector("_InputMax", new Vector4(this.inputMaxR / 255f, this.inputMaxG / 255f, this.inputMaxB / 255f, 1f));
			base.material.SetVector("_InputGamma", new Vector4(this.inputGammaR, this.inputGammaG, this.inputGammaB, 1f));
			base.material.SetVector("_OutputMin", new Vector4(this.outputMinR / 255f, this.outputMinG / 255f, this.outputMinB / 255f, 1f));
			base.material.SetVector("_OutputMax", new Vector4(this.outputMaxR / 255f, this.outputMaxG / 255f, this.outputMaxB / 255f, 1f));
		}
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040013D0 RID: 5072
	public bool isRGB;

	// Token: 0x040013D1 RID: 5073
	public float inputMinL;

	// Token: 0x040013D2 RID: 5074
	public float inputMaxL = 255f;

	// Token: 0x040013D3 RID: 5075
	public float inputGammaL = 1f;

	// Token: 0x040013D4 RID: 5076
	public float inputMinR;

	// Token: 0x040013D5 RID: 5077
	public float inputMaxR = 255f;

	// Token: 0x040013D6 RID: 5078
	public float inputGammaR = 1f;

	// Token: 0x040013D7 RID: 5079
	public float inputMinG;

	// Token: 0x040013D8 RID: 5080
	public float inputMaxG = 255f;

	// Token: 0x040013D9 RID: 5081
	public float inputGammaG = 1f;

	// Token: 0x040013DA RID: 5082
	public float inputMinB;

	// Token: 0x040013DB RID: 5083
	public float inputMaxB = 255f;

	// Token: 0x040013DC RID: 5084
	public float inputGammaB = 1f;

	// Token: 0x040013DD RID: 5085
	public float outputMinL;

	// Token: 0x040013DE RID: 5086
	public float outputMaxL = 255f;

	// Token: 0x040013DF RID: 5087
	public float outputMinR;

	// Token: 0x040013E0 RID: 5088
	public float outputMaxR = 255f;

	// Token: 0x040013E1 RID: 5089
	public float outputMinG;

	// Token: 0x040013E2 RID: 5090
	public float outputMaxG = 255f;

	// Token: 0x040013E3 RID: 5091
	public float outputMinB;

	// Token: 0x040013E4 RID: 5092
	public float outputMaxB = 255f;

	// Token: 0x040013E5 RID: 5093
	public int currentChannel;

	// Token: 0x040013E6 RID: 5094
	public bool logarithmic;
}
