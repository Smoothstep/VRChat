using System;
using UnityEngine;

// Token: 0x02000425 RID: 1061
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Channel Clamper")]
public class CC_ChannelClamper : CC_Base
{
	// Token: 0x0600267A RID: 9850 RVA: 0x000BD9E8 File Offset: 0x000BBDE8
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_RedClamp", this.red);
		base.material.SetVector("_GreenClamp", this.green);
		base.material.SetVector("_BlueClamp", this.blue);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04001359 RID: 4953
	public Vector2 red = new Vector2(0f, 1f);

	// Token: 0x0400135A RID: 4954
	public Vector2 green = new Vector2(0f, 1f);

	// Token: 0x0400135B RID: 4955
	public Vector2 blue = new Vector2(0f, 1f);
}
