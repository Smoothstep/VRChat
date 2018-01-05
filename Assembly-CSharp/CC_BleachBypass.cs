using System;
using UnityEngine;

// Token: 0x02000422 RID: 1058
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Bleach Bypass")]
public class CC_BleachBypass : CC_Base
{
	// Token: 0x06002674 RID: 9844 RVA: 0x000BD7D1 File Offset: 0x000BBBD1
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

	// Token: 0x0400134F RID: 4943
	[Range(0f, 1f)]
	public float amount = 1f;
}
