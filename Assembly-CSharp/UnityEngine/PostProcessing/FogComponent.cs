using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020007DB RID: 2011
	public sealed class FogComponent : PostProcessingComponentCommandBuffer<FogModel>
	{
		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06004076 RID: 16502 RVA: 0x0014542B File Offset: 0x0014382B
		public override bool active
		{
			get
			{
				return base.model.enabled && this.context.isGBufferAvailable && RenderSettings.fog && !this.context.interrupted;
			}
		}

		// Token: 0x06004077 RID: 16503 RVA: 0x00145468 File Offset: 0x00143868
		public override string GetName()
		{
			return "Fog";
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x0014546F File Offset: 0x0014386F
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth;
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x00145472 File Offset: 0x00143872
		public override CameraEvent GetCameraEvent()
		{
			return CameraEvent.AfterImageEffectsOpaque;
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x00145478 File Offset: 0x00143878
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			FogModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Fog");
			material.shaderKeywords = null;
			Color value = (!GraphicsUtils.isLinearColorSpace) ? RenderSettings.fogColor : RenderSettings.fogColor.linear;
			material.SetColor(FogComponent.Uniforms._FogColor, value);
			material.SetFloat(FogComponent.Uniforms._Density, RenderSettings.fogDensity);
			material.SetFloat(FogComponent.Uniforms._Start, RenderSettings.fogStartDistance);
			material.SetFloat(FogComponent.Uniforms._End, RenderSettings.fogEndDistance);
			FogMode fogMode = RenderSettings.fogMode;
			if (fogMode != FogMode.Linear)
			{
				if (fogMode != FogMode.Exponential)
				{
					if (fogMode == FogMode.ExponentialSquared)
					{
						material.EnableKeyword("FOG_EXP2");
					}
				}
				else
				{
					material.EnableKeyword("FOG_EXP");
				}
			}
			else
			{
				material.EnableKeyword("FOG_LINEAR");
			}
			RenderTextureFormat format = (!this.context.isHdr) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR;
			cb.GetTemporaryRT(FogComponent.Uniforms._TempRT, this.context.width, this.context.height, 24, FilterMode.Bilinear, format);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, FogComponent.Uniforms._TempRT);
			cb.Blit(FogComponent.Uniforms._TempRT, BuiltinRenderTextureType.CameraTarget, material, (!settings.excludeSkybox) ? 0 : 1);
			cb.ReleaseTemporaryRT(FogComponent.Uniforms._TempRT);
		}

		// Token: 0x040028DD RID: 10461
		private const string k_ShaderString = "Hidden/Post FX/Fog";

		// Token: 0x020007DC RID: 2012
		private static class Uniforms
		{
			// Token: 0x040028DE RID: 10462
			internal static readonly int _FogColor = Shader.PropertyToID("_FogColor");

			// Token: 0x040028DF RID: 10463
			internal static readonly int _Density = Shader.PropertyToID("_Density");

			// Token: 0x040028E0 RID: 10464
			internal static readonly int _Start = Shader.PropertyToID("_Start");

			// Token: 0x040028E1 RID: 10465
			internal static readonly int _End = Shader.PropertyToID("_End");

			// Token: 0x040028E2 RID: 10466
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");
		}
	}
}
