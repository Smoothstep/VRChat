using System;
using UnityEngine;

// Token: 0x02000ADC RID: 2780
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class BlurOptimized : PostEffectsBase
{
	// Token: 0x06005461 RID: 21601 RVA: 0x001D2655 File Offset: 0x001D0A55
	public override bool CheckResources()
	{
		base.CheckSupport(false);
		this.blurMaterial = base.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
		if (!this.isSupported)
		{
			base.ReportAutoDisable();
		}
		return this.isSupported;
	}

	// Token: 0x06005462 RID: 21602 RVA: 0x001D268E File Offset: 0x001D0A8E
	public void OnDisable()
	{
		if (this.blurMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.blurMaterial);
		}
	}

	// Token: 0x06005463 RID: 21603 RVA: 0x001D26AC File Offset: 0x001D0AAC
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.CheckResources())
		{
			Graphics.Blit(source, destination);
			return;
		}
		float num = 1f / (1f * (float)(1 << this.downsample));
		this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num, -this.blurSize * num, 0f, 0f));
		source.filterMode = FilterMode.Bilinear;
		int width = source.width >> this.downsample;
		int height = source.height >> this.downsample;
		RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
		renderTexture.filterMode = FilterMode.Bilinear;
		Graphics.Blit(source, renderTexture, this.blurMaterial, 0);
		int num2 = (this.blurType != BlurOptimized.BlurType.StandardGauss) ? 2 : 0;
		for (int i = 0; i < this.blurIterations; i++)
		{
			float num3 = (float)i * 1f;
			this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num + num3, -this.blurSize * num - num3, 0f, 0f));
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
			temporary.filterMode = FilterMode.Bilinear;
			Graphics.Blit(renderTexture, temporary, this.blurMaterial, 1 + num2);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary;
			temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
			temporary.filterMode = FilterMode.Bilinear;
			Graphics.Blit(renderTexture, temporary, this.blurMaterial, 2 + num2);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary;
		}
		Graphics.Blit(renderTexture, destination);
		RenderTexture.ReleaseTemporary(renderTexture);
	}

	// Token: 0x04003B7B RID: 15227
	[Range(0f, 2f)]
	public int downsample = 1;

	// Token: 0x04003B7C RID: 15228
	[Range(0f, 10f)]
	public float blurSize = 3f;

	// Token: 0x04003B7D RID: 15229
	[Range(1f, 4f)]
	public int blurIterations = 2;

	// Token: 0x04003B7E RID: 15230
	public BlurOptimized.BlurType blurType;

	// Token: 0x04003B7F RID: 15231
	public Shader blurShader;

	// Token: 0x04003B80 RID: 15232
	private Material blurMaterial;

	// Token: 0x02000ADD RID: 2781
	public enum BlurType
	{
		// Token: 0x04003B82 RID: 15234
		StandardGauss,
		// Token: 0x04003B83 RID: 15235
		SgxGauss
	}
}
