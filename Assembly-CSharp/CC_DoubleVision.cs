using System;
using UnityEngine;

// Token: 0x0200042B RID: 1067
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Double Vision")]
public class CC_DoubleVision : CC_Base
{
	// Token: 0x06002688 RID: 9864 RVA: 0x000BDFE8 File Offset: 0x000BC3E8
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetVector("_Displace", new Vector2(this.displace.x / (float)Screen.width, this.displace.y / (float)Screen.height));
		base.material.SetFloat("_Amount", this.amount);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x0400137F RID: 4991
	public Vector2 displace = new Vector2(0.7f, 0f);

	// Token: 0x04001380 RID: 4992
	[Range(0f, 1f)]
	public float amount = 1f;
}
