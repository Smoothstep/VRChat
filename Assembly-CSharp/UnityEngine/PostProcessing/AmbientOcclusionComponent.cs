using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020007C8 RID: 1992
	public sealed class AmbientOcclusionComponent : PostProcessingComponentCommandBuffer<AmbientOcclusionModel>
	{
		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06004020 RID: 16416 RVA: 0x00142400 File Offset: 0x00140800
		private AmbientOcclusionComponent.OcclusionSource occlusionSource
		{
			get
			{
				if (this.context.isGBufferAvailable && !base.model.settings.forceForwardCompatibility)
				{
					return AmbientOcclusionComponent.OcclusionSource.GBuffer;
				}
				if (base.model.settings.highPrecision && (!this.context.isGBufferAvailable || base.model.settings.forceForwardCompatibility))
				{
					return AmbientOcclusionComponent.OcclusionSource.DepthTexture;
				}
				return AmbientOcclusionComponent.OcclusionSource.DepthNormalsTexture;
			}
		}

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x06004021 RID: 16417 RVA: 0x0014247C File Offset: 0x0014087C
		private bool ambientOnlySupported
		{
			get
			{
				return this.context.isHdr && base.model.settings.ambientOnly && this.context.isGBufferAvailable && !base.model.settings.forceForwardCompatibility;
			}
		}

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x06004022 RID: 16418 RVA: 0x001424DC File Offset: 0x001408DC
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.intensity > 0f && !this.context.interrupted;
			}
		}

		// Token: 0x06004023 RID: 16419 RVA: 0x00142528 File Offset: 0x00140928
		public override DepthTextureMode GetCameraFlags()
		{
			DepthTextureMode depthTextureMode = DepthTextureMode.None;
			if (this.occlusionSource == AmbientOcclusionComponent.OcclusionSource.DepthTexture)
			{
				depthTextureMode |= DepthTextureMode.Depth;
			}
			if (this.occlusionSource != AmbientOcclusionComponent.OcclusionSource.GBuffer)
			{
				depthTextureMode |= DepthTextureMode.DepthNormals;
			}
			return depthTextureMode;
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x00142557 File Offset: 0x00140957
		public override string GetName()
		{
			return "Ambient Occlusion";
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x0014255E File Offset: 0x0014095E
		public override CameraEvent GetCameraEvent()
		{
			return (!this.ambientOnlySupported || this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.AmbientOcclusion)) ? CameraEvent.BeforeImageEffectsOpaque : CameraEvent.BeforeReflections;
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x00142590 File Offset: 0x00140990
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			AmbientOcclusionModel.Settings settings = base.model.settings;
			Material mat = this.context.materialFactory.Get("Hidden/Post FX/Blit");
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Ambient Occlusion");
			material.shaderKeywords = null;
			material.SetFloat(AmbientOcclusionComponent.Uniforms._Intensity, settings.intensity);
			material.SetFloat(AmbientOcclusionComponent.Uniforms._Radius, settings.radius);
			material.SetFloat(AmbientOcclusionComponent.Uniforms._Downsample, (!settings.downsampling) ? 1f : 0.5f);
			material.SetInt(AmbientOcclusionComponent.Uniforms._SampleCount, (int)settings.sampleCount);
			if (!this.context.isGBufferAvailable && RenderSettings.fog)
			{
				material.SetVector(AmbientOcclusionComponent.Uniforms._FogParams, new Vector3(RenderSettings.fogDensity, RenderSettings.fogStartDistance, RenderSettings.fogEndDistance));
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
			}
			else
			{
				material.EnableKeyword("FOG_OFF");
			}
			int width = this.context.width;
			int height = this.context.height;
			int num = (!settings.downsampling) ? 1 : 2;
			int nameID = AmbientOcclusionComponent.Uniforms._OcclusionTexture1;
			cb.GetTemporaryRT(nameID, width / num, height / num, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			cb.Blit(null, nameID, material, (int)this.occlusionSource);
			int occlusionTexture = AmbientOcclusionComponent.Uniforms._OcclusionTexture2;
			cb.GetTemporaryRT(occlusionTexture, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, nameID);
			cb.Blit(nameID, occlusionTexture, material, (this.occlusionSource != AmbientOcclusionComponent.OcclusionSource.GBuffer) ? 3 : 4);
			cb.ReleaseTemporaryRT(nameID);
			nameID = AmbientOcclusionComponent.Uniforms._OcclusionTexture;
			cb.GetTemporaryRT(nameID, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, occlusionTexture);
			cb.Blit(occlusionTexture, nameID, material, 5);
			cb.ReleaseTemporaryRT(occlusionTexture);
			if (this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.AmbientOcclusion))
			{
				cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, nameID);
				cb.Blit(nameID, BuiltinRenderTextureType.CameraTarget, material, 8);
				this.context.Interrupt();
			}
			else if (this.ambientOnlySupported)
			{
				cb.SetRenderTarget(this.m_MRT, BuiltinRenderTextureType.CameraTarget);
				cb.DrawMesh(GraphicsUtils.quad, Matrix4x4.identity, material, 0, 7);
			}
			else
			{
				RenderTextureFormat format = (!this.context.isHdr) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR;
				int tempRT = AmbientOcclusionComponent.Uniforms._TempRT;
				cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, FilterMode.Bilinear, format);
				cb.Blit(BuiltinRenderTextureType.CameraTarget, tempRT, mat, 0);
				cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, tempRT);
				cb.Blit(tempRT, BuiltinRenderTextureType.CameraTarget, material, 6);
				cb.ReleaseTemporaryRT(tempRT);
			}
			cb.ReleaseTemporaryRT(nameID);
		}

		// Token: 0x0400286D RID: 10349
		private const string k_BlitShaderString = "Hidden/Post FX/Blit";

		// Token: 0x0400286E RID: 10350
		private const string k_ShaderString = "Hidden/Post FX/Ambient Occlusion";

		// Token: 0x0400286F RID: 10351
		private readonly RenderTargetIdentifier[] m_MRT = new RenderTargetIdentifier[]
		{
			BuiltinRenderTextureType.GBuffer0,
			BuiltinRenderTextureType.CameraTarget
		};

		// Token: 0x020007C9 RID: 1993
		private static class Uniforms
		{
			// Token: 0x04002870 RID: 10352
			internal static readonly int _Intensity = Shader.PropertyToID("_Intensity");

			// Token: 0x04002871 RID: 10353
			internal static readonly int _Radius = Shader.PropertyToID("_Radius");

			// Token: 0x04002872 RID: 10354
			internal static readonly int _FogParams = Shader.PropertyToID("_FogParams");

			// Token: 0x04002873 RID: 10355
			internal static readonly int _Downsample = Shader.PropertyToID("_Downsample");

			// Token: 0x04002874 RID: 10356
			internal static readonly int _SampleCount = Shader.PropertyToID("_SampleCount");

			// Token: 0x04002875 RID: 10357
			internal static readonly int _OcclusionTexture1 = Shader.PropertyToID("_OcclusionTexture1");

			// Token: 0x04002876 RID: 10358
			internal static readonly int _OcclusionTexture2 = Shader.PropertyToID("_OcclusionTexture2");

			// Token: 0x04002877 RID: 10359
			internal static readonly int _OcclusionTexture = Shader.PropertyToID("_OcclusionTexture");

			// Token: 0x04002878 RID: 10360
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x04002879 RID: 10361
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");
		}

		// Token: 0x020007CA RID: 1994
		private enum OcclusionSource
		{
			// Token: 0x0400287B RID: 10363
			DepthTexture,
			// Token: 0x0400287C RID: 10364
			DepthNormalsTexture,
			// Token: 0x0400287D RID: 10365
			GBuffer
		}
	}
}
