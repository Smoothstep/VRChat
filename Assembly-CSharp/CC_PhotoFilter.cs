using System;
using UnityEngine;

// Token: 0x0200043C RID: 1084
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Photo Filter")]
public class CC_PhotoFilter : CC_Base
{
	// Token: 0x060026B7 RID: 9911 RVA: 0x000BEF9C File Offset: 0x000BD39C
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.density == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetColor("_RGB", this.color);
		base.material.SetFloat("_Density", this.density);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040013EA RID: 5098
	public Color color = new Color(1f, 0.5f, 0.2f, 1f);

	// Token: 0x040013EB RID: 5099
	[Range(0f, 1f)]
	public float density = 0.35f;
}
