using System;
using UnityEngine;

// Token: 0x0200043A RID: 1082
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Lookup Filter (Color Grading)")]
public class CC_LookupFilter : CC_Base
{
	// Token: 0x060026B3 RID: 9907 RVA: 0x000BEEA8 File Offset: 0x000BD2A8
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.lookupTexture == null)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetTexture("_LookupTex", this.lookupTexture);
		base.material.SetFloat("_Amount", this.amount);
		Graphics.Blit(source, destination, base.material, (!CC_Base.IsLinear()) ? 0 : 1);
	}

	// Token: 0x040013E7 RID: 5095
	public Texture lookupTexture;

	// Token: 0x040013E8 RID: 5096
	[Range(0f, 1f)]
	public float amount = 1f;
}
