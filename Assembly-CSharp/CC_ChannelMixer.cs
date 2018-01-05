using System;
using UnityEngine;

// Token: 0x02000426 RID: 1062
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Channel Mixer")]
public class CC_ChannelMixer : CC_Base
{
	// Token: 0x0600267C RID: 9852 RVA: 0x000BDA7C File Offset: 0x000BBE7C
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_Red", new Vector4(this.redR * 0.01f, this.greenR * 0.01f, this.blueR * 0.01f));
		base.material.SetVector("_Green", new Vector4(this.redG * 0.01f, this.greenG * 0.01f, this.blueG * 0.01f));
		base.material.SetVector("_Blue", new Vector4(this.redB * 0.01f, this.greenB * 0.01f, this.blueB * 0.01f));
		base.material.SetVector("_Constant", new Vector4(this.constantR * 0.01f, this.constantG * 0.01f, this.constantB * 0.01f));
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x0400135C RID: 4956
	[Range(-200f, 200f)]
	public float redR = 100f;

	// Token: 0x0400135D RID: 4957
	[Range(-200f, 200f)]
	public float redG;

	// Token: 0x0400135E RID: 4958
	[Range(-200f, 200f)]
	public float redB;

	// Token: 0x0400135F RID: 4959
	[Range(-200f, 200f)]
	public float greenR;

	// Token: 0x04001360 RID: 4960
	[Range(-200f, 200f)]
	public float greenG = 100f;

	// Token: 0x04001361 RID: 4961
	[Range(-200f, 200f)]
	public float greenB;

	// Token: 0x04001362 RID: 4962
	[Range(-200f, 200f)]
	public float blueR;

	// Token: 0x04001363 RID: 4963
	[Range(-200f, 200f)]
	public float blueG;

	// Token: 0x04001364 RID: 4964
	[Range(-200f, 200f)]
	public float blueB = 100f;

	// Token: 0x04001365 RID: 4965
	[Range(-200f, 200f)]
	public float constantR;

	// Token: 0x04001366 RID: 4966
	[Range(-200f, 200f)]
	public float constantG;

	// Token: 0x04001367 RID: 4967
	[Range(-200f, 200f)]
	public float constantB;

	// Token: 0x04001368 RID: 4968
	public int currentChannel;
}
