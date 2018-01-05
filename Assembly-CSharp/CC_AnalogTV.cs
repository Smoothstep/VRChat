using System;
using UnityEngine;

// Token: 0x02000420 RID: 1056
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Analog TV")]
public class CC_AnalogTV : CC_Base
{
	// Token: 0x0600266C RID: 9836 RVA: 0x000BD6BB File Offset: 0x000BBABB
	protected virtual void Update()
	{
		if (this.autoPhase)
		{
			this.phase += Time.deltaTime * 0.25f;
		}
	}

	// Token: 0x0600266D RID: 9837 RVA: 0x000BD6E0 File Offset: 0x000BBAE0
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Phase", this.phase);
		base.material.SetFloat("_NoiseIntensity", this.noiseIntensity);
		base.material.SetFloat("_ScanlinesIntensity", this.scanlinesIntensity);
		base.material.SetFloat("_ScanlinesCount", (float)((int)this.scanlinesCount));
		base.material.SetFloat("_ScanlinesOffset", this.scanlinesOffset);
		base.material.SetFloat("_Distortion", this.distortion);
		base.material.SetFloat("_CubicDistortion", this.cubicDistortion);
		base.material.SetFloat("_Scale", this.scale);
		Graphics.Blit(source, destination, base.material, (!this.grayscale) ? 0 : 1);
	}

	// Token: 0x04001343 RID: 4931
	public bool autoPhase = true;

	// Token: 0x04001344 RID: 4932
	public float phase = 0.5f;

	// Token: 0x04001345 RID: 4933
	public bool grayscale;

	// Token: 0x04001346 RID: 4934
	[Range(0f, 1f)]
	public float noiseIntensity = 0.5f;

	// Token: 0x04001347 RID: 4935
	[Range(0f, 10f)]
	public float scanlinesIntensity = 2f;

	// Token: 0x04001348 RID: 4936
	[Range(0f, 4096f)]
	public float scanlinesCount = 768f;

	// Token: 0x04001349 RID: 4937
	public float scanlinesOffset;

	// Token: 0x0400134A RID: 4938
	[Range(-2f, 2f)]
	public float distortion = 0.2f;

	// Token: 0x0400134B RID: 4939
	[Range(-2f, 2f)]
	public float cubicDistortion = 0.6f;

	// Token: 0x0400134C RID: 4940
	[Range(0.01f, 2f)]
	public float scale = 0.8f;
}
