using System;
using UnityEngine;

// Token: 0x0200042D RID: 1069
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Frost")]
public class CC_Frost : CC_Base
{
	// Token: 0x0600268C RID: 9868 RVA: 0x000BE144 File Offset: 0x000BC544
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.scale == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetFloat("_Scale", this.scale);
		base.material.SetFloat("_Sharpness", this.sharpness * 0.01f);
		base.material.SetFloat("_Darkness", this.darkness * 0.02f);
		Graphics.Blit(source, destination, base.material, (!this.enableVignette) ? 0 : 1);
	}

	// Token: 0x04001385 RID: 4997
	[Range(0f, 16f)]
	public float scale = 1.2f;

	// Token: 0x04001386 RID: 4998
	[Range(-100f, 100f)]
	public float sharpness = 40f;

	// Token: 0x04001387 RID: 4999
	[Range(0f, 100f)]
	public float darkness = 35f;

	// Token: 0x04001388 RID: 5000
	public bool enableVignette = true;
}
