using System;
using UnityEngine;

// Token: 0x02000448 RID: 1096
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Wiggle")]
public class CC_Wiggle : CC_Base
{
	// Token: 0x060026CF RID: 9935 RVA: 0x000BF653 File Offset: 0x000BDA53
	protected virtual void Update()
	{
		if (this.autoTimer)
		{
			this.timer += this.speed * Time.deltaTime;
		}
	}

	// Token: 0x060026D0 RID: 9936 RVA: 0x000BF679 File Offset: 0x000BDA79
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Timer", this.timer);
		base.material.SetFloat("_Scale", this.scale);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04001426 RID: 5158
	public float timer;

	// Token: 0x04001427 RID: 5159
	public float speed = 1f;

	// Token: 0x04001428 RID: 5160
	public float scale = 12f;

	// Token: 0x04001429 RID: 5161
	public bool autoTimer = true;
}
