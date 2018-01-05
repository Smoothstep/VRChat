using System;
using UnityEngine;

// Token: 0x02000427 RID: 1063
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Channel Swapper")]
public class CC_ChannelSwapper : CC_Base
{
	// Token: 0x0600267E RID: 9854 RVA: 0x000BDB90 File Offset: 0x000BBF90
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_Red", CC_ChannelSwapper.m_Channels[this.red]);
		base.material.SetVector("_Green", CC_ChannelSwapper.m_Channels[this.green]);
		base.material.SetVector("_Blue", CC_ChannelSwapper.m_Channels[this.blue]);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04001369 RID: 4969
	public int red;

	// Token: 0x0400136A RID: 4970
	public int green = 1;

	// Token: 0x0400136B RID: 4971
	public int blue = 2;

	// Token: 0x0400136C RID: 4972
	private static Vector4[] m_Channels = new Vector4[]
	{
		new Vector4(1f, 0f, 0f, 0f),
		new Vector4(0f, 1f, 0f, 0f),
		new Vector4(0f, 0f, 1f, 0f)
	};
}
