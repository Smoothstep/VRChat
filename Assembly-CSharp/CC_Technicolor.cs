using System;
using UnityEngine;

// Token: 0x02000442 RID: 1090
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Technicolor")]
public class CC_Technicolor : CC_Base
{
	// Token: 0x060026C4 RID: 9924 RVA: 0x000BF34C File Offset: 0x000BD74C
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Exposure", 8f - this.exposure);
		base.material.SetVector("_Balance", Vector3.one - this.balance);
		base.material.SetFloat("_Amount", this.amount);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040013FA RID: 5114
	[Range(0f, 8f)]
	public float exposure = 4f;

	// Token: 0x040013FB RID: 5115
	public Vector3 balance = new Vector3(0.25f, 0.25f, 0.25f);

	// Token: 0x040013FC RID: 5116
	[Range(0f, 1f)]
	public float amount = 0.5f;
}
