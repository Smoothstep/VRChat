using System;
using UnityEngine;

// Token: 0x02000423 RID: 1059
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Blend")]
public class CC_Blend : CC_Base
{
	// Token: 0x06002676 RID: 9846 RVA: 0x000BD824 File Offset: 0x000BBC24
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.texture == null || this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetTexture("_OverlayTex", this.texture);
		base.material.SetFloat("_Amount", this.amount);
		Graphics.Blit(source, destination, base.material, this.mode);
	}

	// Token: 0x04001350 RID: 4944
	public Texture texture;

	// Token: 0x04001351 RID: 4945
	[Range(0f, 1f)]
	public float amount = 1f;

	// Token: 0x04001352 RID: 4946
	public int mode;
}
