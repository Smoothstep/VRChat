using System;
using UnityEngine;

// Token: 0x0200043E RID: 1086
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Posterize")]
public class CC_Posterize : CC_Base
{
	// Token: 0x060026BC RID: 9916 RVA: 0x000BF0F0 File Offset: 0x000BD4F0
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Levels", (float)this.levels);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040013F1 RID: 5105
	[Range(2f, 255f)]
	public int levels = 4;
}
