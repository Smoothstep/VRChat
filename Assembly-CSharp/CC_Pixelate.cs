using System;
using UnityEngine;

// Token: 0x0200043D RID: 1085
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Pixelate")]
public class CC_Pixelate : CC_Base
{
	// Token: 0x060026B9 RID: 9913 RVA: 0x000BF018 File Offset: 0x000BD418
	protected override void Start()
	{
		base.Start();
		this.m_Camera = base.GetComponent<Camera>();
	}

	// Token: 0x060026BA RID: 9914 RVA: 0x000BF02C File Offset: 0x000BD42C
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		int num = this.mode;
		if (num != 0)
		{
			if (num != 1)
			{
			}
			base.material.SetFloat("_Scale", (float)this.m_Camera.pixelWidth / this.scale);
		}
		else
		{
			base.material.SetFloat("_Scale", this.scale);
		}
		base.material.SetFloat("_Ratio", (!this.automaticRatio) ? this.ratio : ((float)this.m_Camera.pixelWidth / (float)this.m_Camera.pixelHeight));
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040013EC RID: 5100
	[Range(1f, 1024f)]
	public float scale = 80f;

	// Token: 0x040013ED RID: 5101
	public bool automaticRatio;

	// Token: 0x040013EE RID: 5102
	public float ratio = 1f;

	// Token: 0x040013EF RID: 5103
	public int mode;

	// Token: 0x040013F0 RID: 5104
	protected Camera m_Camera;
}
