using System;
using UnityEngine;

namespace VolumetricFogAndMist
{
	// Token: 0x020009C0 RID: 2496
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera), typeof(VolumetricFog))]
	public class VolumetricFogPosT : MonoBehaviour, IVolumetricFogRenderComponent
	{
		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x06004C09 RID: 19465 RVA: 0x0019681E File Offset: 0x00194C1E
		// (set) Token: 0x06004C0A RID: 19466 RVA: 0x00196826 File Offset: 0x00194C26
		public VolumetricFog fog { get; set; }

		// Token: 0x06004C0B RID: 19467 RVA: 0x0019682F File Offset: 0x00194C2F
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.fog == null || !this.fog.enabled)
			{
				Graphics.Blit(source, destination);
				return;
			}
			this.fog.DoOnRenderImage(source, destination);
		}

		// Token: 0x06004C0C RID: 19468 RVA: 0x00196867 File Offset: 0x00194C67
		public void DestroySelf()
		{
			UnityEngine.Object.DestroyImmediate(this);
		}
	}
}
