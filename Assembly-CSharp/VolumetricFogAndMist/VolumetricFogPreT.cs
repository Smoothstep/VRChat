using System;
using UnityEngine;

namespace VolumetricFogAndMist
{
	// Token: 0x020009C1 RID: 2497
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera), typeof(VolumetricFog))]
	public class VolumetricFogPreT : MonoBehaviour, IVolumetricFogRenderComponent
	{
		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x06004C0E RID: 19470 RVA: 0x00196877 File Offset: 0x00194C77
		// (set) Token: 0x06004C0F RID: 19471 RVA: 0x0019687F File Offset: 0x00194C7F
		public VolumetricFog fog { get; set; }

		// Token: 0x06004C10 RID: 19472 RVA: 0x00196888 File Offset: 0x00194C88
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.fog == null || !this.fog.enabled)
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.fog.renderBeforeTransparent)
			{
				this.fog.DoOnRenderImage(source, destination);
			}
			else
			{
				this.opaqueFrame = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
				Graphics.Blit(source, this.opaqueFrame);
				this.fog.fogMat.SetTexture("_OriginalTex", this.opaqueFrame);
				Graphics.Blit(source, destination);
			}
		}

		// Token: 0x06004C11 RID: 19473 RVA: 0x0019692C File Offset: 0x00194D2C
		private void OnPostRender()
		{
			if (this.opaqueFrame != null)
			{
				RenderTexture.ReleaseTemporary(this.opaqueFrame);
				this.opaqueFrame = null;
			}
		}

		// Token: 0x06004C12 RID: 19474 RVA: 0x00196951 File Offset: 0x00194D51
		public void DestroySelf()
		{
			UnityEngine.Object.DestroyImmediate(this);
		}

		// Token: 0x040033DE RID: 13278
		private RenderTexture opaqueFrame;
	}
}
