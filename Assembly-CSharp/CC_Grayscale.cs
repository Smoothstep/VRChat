using System;
using UnityEngine;

// Token: 0x02000433 RID: 1075
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Grayscale")]
public class CC_Grayscale : CC_Base
{
	// Token: 0x0600269A RID: 9882 RVA: 0x000BE59C File Offset: 0x000BC99C
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetVector("_Data", new Vector4(this.redLuminance, this.greenLuminance, this.blueLuminance, this.amount));
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040013A4 RID: 5028
	[Range(0f, 1f)]
	public float redLuminance = 0.299f;

	// Token: 0x040013A5 RID: 5029
	[Range(0f, 1f)]
	public float greenLuminance = 0.587f;

	// Token: 0x040013A6 RID: 5030
	[Range(0f, 1f)]
	public float blueLuminance = 0.114f;

	// Token: 0x040013A7 RID: 5031
	[Range(0f, 1f)]
	public float amount = 1f;
}
