using System;
using UnityEngine;

// Token: 0x02000429 RID: 1065
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Convolution Matrix 3x3")]
public class CC_Convolution3x3 : CC_Base
{
	// Token: 0x06002683 RID: 9859 RVA: 0x000BDDF8 File Offset: 0x000BC1F8
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_PX", 1f / (float)Screen.width);
		base.material.SetFloat("_PY", 1f / (float)Screen.height);
		base.material.SetFloat("_Amount", this.amount);
		base.material.SetVector("_KernelT", this.kernelTop / this.divisor);
		base.material.SetVector("_KernelM", this.kernelMiddle / this.divisor);
		base.material.SetVector("_KernelB", this.kernelBottom / this.divisor);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04001375 RID: 4981
	public Vector3 kernelTop = Vector3.zero;

	// Token: 0x04001376 RID: 4982
	public Vector3 kernelMiddle = Vector3.up;

	// Token: 0x04001377 RID: 4983
	public Vector3 kernelBottom = Vector3.zero;

	// Token: 0x04001378 RID: 4984
	public float divisor = 1f;

	// Token: 0x04001379 RID: 4985
	[Range(0f, 1f)]
	public float amount = 1f;
}
