using System;
using UnityEngine;

// Token: 0x02000441 RID: 1089
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Sharpen")]
public class CC_Sharpen : CC_Base
{
	// Token: 0x060026C2 RID: 9922 RVA: 0x000BF27C File Offset: 0x000BD67C
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.strength == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetFloat("_PX", 1f / (float)Screen.width);
		base.material.SetFloat("_PY", 1f / (float)Screen.height);
		base.material.SetFloat("_Strength", this.strength);
		base.material.SetFloat("_Clamp", this.clamp);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040013F8 RID: 5112
	[Range(0f, 5f)]
	public float strength = 0.6f;

	// Token: 0x040013F9 RID: 5113
	[Range(0f, 1f)]
	public float clamp = 0.05f;
}
