using System;
using UnityEngine;

// Token: 0x02000444 RID: 1092
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Vibrance")]
public class CC_Vibrance : CC_Base
{
	// Token: 0x060026C8 RID: 9928 RVA: 0x000BF46C File Offset: 0x000BD86C
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		if (this.advanced)
		{
			base.material.SetFloat("_Amount", this.amount * 0.01f);
			base.material.SetVector("_Channels", new Vector3(this.redChannel, this.greenChannel, this.blueChannel));
			Graphics.Blit(source, destination, base.material, 1);
		}
		else
		{
			base.material.SetFloat("_Amount", this.amount * 0.02f);
			Graphics.Blit(source, destination, base.material, 0);
		}
	}

	// Token: 0x04001400 RID: 5120
	[Range(-100f, 100f)]
	public float amount;

	// Token: 0x04001401 RID: 5121
	[Range(-5f, 5f)]
	public float redChannel = 1f;

	// Token: 0x04001402 RID: 5122
	[Range(-5f, 5f)]
	public float greenChannel = 1f;

	// Token: 0x04001403 RID: 5123
	[Range(-5f, 5f)]
	public float blueChannel = 1f;

	// Token: 0x04001404 RID: 5124
	public bool advanced;
}
