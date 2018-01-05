using System;
using UnityEngine;

// Token: 0x0200043B RID: 1083
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Negative")]
public class CC_Negative : CC_Base
{
	// Token: 0x060026B5 RID: 9909 RVA: 0x000BEF2B File Offset: 0x000BD32B
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetFloat("_Amount", this.amount);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040013E9 RID: 5097
	[Range(0f, 1f)]
	public float amount = 1f;
}
