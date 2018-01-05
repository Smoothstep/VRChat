using System;
using UnityEngine;

// Token: 0x0200042A RID: 1066
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Cross Stitch")]
public class CC_CrossStitch : CC_Base
{
	// Token: 0x06002685 RID: 9861 RVA: 0x000BDEF3 File Offset: 0x000BC2F3
	protected override void Start()
	{
		base.Start();
		this.m_Camera = base.GetComponent<Camera>();
	}

	// Token: 0x06002686 RID: 9862 RVA: 0x000BDF08 File Offset: 0x000BC308
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_StitchSize", (float)this.size);
		base.material.SetFloat("_Brightness", this.brightness);
		int num = (!this.invert) ? 0 : 1;
		if (this.pixelize)
		{
			num += 2;
			base.material.SetFloat("_Scale", (float)(this.m_Camera.pixelWidth / this.size));
			base.material.SetFloat("_Ratio", (float)(this.m_Camera.pixelWidth / this.m_Camera.pixelHeight));
		}
		Graphics.Blit(source, destination, base.material, num);
	}

	// Token: 0x0400137A RID: 4986
	[Range(1f, 128f)]
	public int size = 8;

	// Token: 0x0400137B RID: 4987
	public float brightness = 1.5f;

	// Token: 0x0400137C RID: 4988
	public bool invert;

	// Token: 0x0400137D RID: 4989
	public bool pixelize = true;

	// Token: 0x0400137E RID: 4990
	protected Camera m_Camera;
}
