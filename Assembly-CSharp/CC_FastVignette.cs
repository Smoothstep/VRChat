using System;
using UnityEngine;

// Token: 0x0200042C RID: 1068
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Fast Vignette")]
public class CC_FastVignette : CC_Base
{
	// Token: 0x0600268A RID: 9866 RVA: 0x000BE0A4 File Offset: 0x000BC4A4
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_Data", new Vector4(this.center.x, this.center.y, this.sharpness * 0.01f, this.darkness * 0.02f));
		Graphics.Blit(source, destination, base.material, (!this.desaturate) ? 0 : 1);
	}

	// Token: 0x04001381 RID: 4993
	public Vector2 center = new Vector2(0.5f, 0.5f);

	// Token: 0x04001382 RID: 4994
	[Range(-100f, 100f)]
	public float sharpness = 10f;

	// Token: 0x04001383 RID: 4995
	[Range(0f, 100f)]
	public float darkness = 30f;

	// Token: 0x04001384 RID: 4996
	public bool desaturate;
}
