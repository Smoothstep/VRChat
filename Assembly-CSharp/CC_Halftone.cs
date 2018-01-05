using System;
using UnityEngine;

// Token: 0x02000434 RID: 1076
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Halftone")]
public class CC_Halftone : CC_Base
{
	// Token: 0x0600269C RID: 9884 RVA: 0x000BE61C File Offset: 0x000BCA1C
	protected override void Start()
	{
		base.Start();
		this.m_Camera = base.GetComponent<Camera>();
	}

	// Token: 0x0600269D RID: 9885 RVA: 0x000BE630 File Offset: 0x000BCA30
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Density", this.density);
		base.material.SetFloat("_AspectRatio", this.m_Camera.aspect);
		int pass = 0;
		if (this.mode == 0)
		{
			if (this.antialiasing && this.showOriginal)
			{
				pass = 3;
			}
			else if (this.antialiasing)
			{
				pass = 1;
			}
			else if (this.showOriginal)
			{
				pass = 2;
			}
		}
		else if (this.mode == 1)
		{
			pass = ((!this.antialiasing) ? 4 : 5);
		}
		Graphics.Blit(source, destination, base.material, pass);
	}

	// Token: 0x040013A8 RID: 5032
	[Range(0f, 512f)]
	public float density = 64f;

	// Token: 0x040013A9 RID: 5033
	public int mode = 1;

	// Token: 0x040013AA RID: 5034
	public bool antialiasing = true;

	// Token: 0x040013AB RID: 5035
	public bool showOriginal;

	// Token: 0x040013AC RID: 5036
	protected Camera m_Camera;
}
