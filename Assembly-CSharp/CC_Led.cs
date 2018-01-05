using System;
using UnityEngine;

// Token: 0x02000438 RID: 1080
[ExecuteInEditMode]
[AddComponentMenu("Colorful/LED")]
public class CC_Led : CC_Base
{
	// Token: 0x060026AC RID: 9900 RVA: 0x000BEA7C File Offset: 0x000BCE7C
	protected override void Start()
	{
		base.Start();
		this.m_Camera = base.GetComponent<Camera>();
	}

	// Token: 0x060026AD RID: 9901 RVA: 0x000BEA90 File Offset: 0x000BCE90
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
		base.material.SetFloat("_Brightness", this.brightness);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040013CA RID: 5066
	[Range(1f, 255f)]
	public float scale = 80f;

	// Token: 0x040013CB RID: 5067
	[Range(0f, 10f)]
	public float brightness = 1f;

	// Token: 0x040013CC RID: 5068
	public bool automaticRatio;

	// Token: 0x040013CD RID: 5069
	public float ratio = 1f;

	// Token: 0x040013CE RID: 5070
	public int mode;

	// Token: 0x040013CF RID: 5071
	protected Camera m_Camera;
}
