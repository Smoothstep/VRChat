using System;
using UnityEngine;

// Token: 0x02000447 RID: 1095
[ExecuteInEditMode]
[AddComponentMenu("Colorful/White Balance")]
public class CC_WhiteBalance : CC_Base
{
	// Token: 0x060026CC RID: 9932 RVA: 0x000BF5C4 File Offset: 0x000BD9C4
	protected virtual void Reset()
	{
		this.white = ((!CC_Base.IsLinear()) ? new Color(0.5f, 0.5f, 0.5f) : new Color(0.72974f, 0.72974f, 0.72974f));
	}

	// Token: 0x060026CD RID: 9933 RVA: 0x000BF603 File Offset: 0x000BDA03
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetColor("_White", this.white);
		Graphics.Blit(source, destination, base.material, this.mode);
	}

	// Token: 0x04001424 RID: 5156
	public Color white = new Color(0.5f, 0.5f, 0.5f);

	// Token: 0x04001425 RID: 5157
	public int mode = 1;
}
