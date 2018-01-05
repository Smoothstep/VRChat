using System;
using UnityEngine;

// Token: 0x020008A2 RID: 2210
public class FixSlowDistortionOnMobile : MonoBehaviour
{
	// Token: 0x060043CE RID: 17358 RVA: 0x00166441 File Offset: 0x00164841
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest);
	}
}
