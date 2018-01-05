using System;
using UnityEngine;

// Token: 0x02000437 RID: 1079
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Kuwahara")]
public class CC_Kuwahara : CC_Base
{
	// Token: 0x060026A9 RID: 9897 RVA: 0x000BE9CA File Offset: 0x000BCDCA
	protected override void Start()
	{
		base.Start();
		this.m_Camera = base.GetComponent<Camera>();
	}

	// Token: 0x060026AA RID: 9898 RVA: 0x000BE9E0 File Offset: 0x000BCDE0
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.radius = Mathf.Clamp(this.radius, 1, 4);
		base.material.SetVector("_TexelSize", new Vector2(1f / (float)this.m_Camera.pixelWidth, 1f / (float)this.m_Camera.pixelHeight));
		Graphics.Blit(source, destination, base.material, this.radius - 1);
	}

	// Token: 0x040013C8 RID: 5064
	[Range(1f, 4f)]
	public int radius = 3;

	// Token: 0x040013C9 RID: 5065
	protected Camera m_Camera;
}
