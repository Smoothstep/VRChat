using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020007E7 RID: 2023
	public sealed class ScreenSpaceReflectionComponent : PostProcessingComponentCommandBuffer<ScreenSpaceReflectionModel>
	{
		// Token: 0x060040A1 RID: 16545 RVA: 0x0014666B File Offset: 0x00144A6B
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth;
		}

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x060040A2 RID: 16546 RVA: 0x0014666E File Offset: 0x00144A6E
		public override bool active
		{
			get
			{
				return base.model.enabled && this.context.isGBufferAvailable && !this.context.interrupted;
			}
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x001466A4 File Offset: 0x00144AA4
		public override void OnEnable()
		{
			this.m_ReflectionTextures[0] = Shader.PropertyToID("_ReflectionTexture0");
			this.m_ReflectionTextures[1] = Shader.PropertyToID("_ReflectionTexture1");
			this.m_ReflectionTextures[2] = Shader.PropertyToID("_ReflectionTexture2");
			this.m_ReflectionTextures[3] = Shader.PropertyToID("_ReflectionTexture3");
			this.m_ReflectionTextures[4] = Shader.PropertyToID("_ReflectionTexture4");
		}

		// Token: 0x060040A4 RID: 16548 RVA: 0x0014670B File Offset: 0x00144B0B
		public override string GetName()
		{
			return "Screen Space Reflection";
		}

		// Token: 0x060040A5 RID: 16549 RVA: 0x00146712 File Offset: 0x00144B12
		public override CameraEvent GetCameraEvent()
		{
			return CameraEvent.AfterFinalPass;
		}

		// Token: 0x060040A6 RID: 16550 RVA: 0x00146718 File Offset: 0x00144B18
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			ScreenSpaceReflectionModel.Settings settings = base.model.settings;
			Camera camera = this.context.camera;
			int num = (settings.reflection.reflectionQuality != ScreenSpaceReflectionModel.SSRResolution.High) ? 2 : 1;
			int num2 = this.context.width / num;
			int num3 = this.context.height / num;
			float num4 = (float)this.context.width;
			float num5 = (float)this.context.height;
			float num6 = num4 / 2f;
			float num7 = num5 / 2f;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Screen Space Reflection");
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._RayStepSize, settings.reflection.stepSize);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._AdditiveReflection, (settings.reflection.blendType != ScreenSpaceReflectionModel.SSRReflectionBlendType.Additive) ? 0 : 1);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._BilateralUpsampling, (!this.k_BilateralUpsample) ? 0 : 1);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._TreatBackfaceHitAsMiss, (!this.k_TreatBackfaceHitAsMiss) ? 0 : 1);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._AllowBackwardsRays, (!settings.reflection.reflectBackfaces) ? 0 : 1);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._TraceBehindObjects, (!this.k_TraceBehindObjects) ? 0 : 1);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._MaxSteps, settings.reflection.iterationCount);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._FullResolutionFiltering, 0);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._HalfResolution, (settings.reflection.reflectionQuality == ScreenSpaceReflectionModel.SSRResolution.High) ? 0 : 1);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._HighlightSuppression, (!this.k_HighlightSuppression) ? 0 : 1);
			float value = num4 / (-2f * Mathf.Tan(camera.fieldOfView / 180f * 3.14159274f * 0.5f));
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._PixelsPerMeterAtOneMeter, value);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._ScreenEdgeFading, settings.screenEdgeMask.intensity);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._ReflectionBlur, settings.reflection.reflectionBlur);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._MaxRayTraceDistance, settings.reflection.maxDistance);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._FadeDistance, settings.intensity.fadeDistance);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._LayerThickness, settings.reflection.widthModifier);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._SSRMultiplier, settings.intensity.reflectionMultiplier);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._FresnelFade, settings.intensity.fresnelFade);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._FresnelFadePower, settings.intensity.fresnelFadePower);
			Matrix4x4 projectionMatrix = camera.projectionMatrix;
			Vector4 value2 = new Vector4(-2f / (num4 * projectionMatrix[0]), -2f / (num5 * projectionMatrix[5]), (1f - projectionMatrix[2]) / projectionMatrix[0], (1f + projectionMatrix[6]) / projectionMatrix[5]);
			Vector3 v = (!float.IsPositiveInfinity(camera.farClipPlane)) ? new Vector3(camera.nearClipPlane * camera.farClipPlane, camera.nearClipPlane - camera.farClipPlane, camera.farClipPlane) : new Vector3(camera.nearClipPlane, -1f, 1f);
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._ReflectionBufferSize, new Vector2((float)num2, (float)num3));
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._ScreenSize, new Vector2(num4, num5));
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._InvScreenSize, new Vector2(1f / num4, 1f / num5));
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._ProjInfo, value2);
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._CameraClipInfo, v);
			Matrix4x4 lhs = default(Matrix4x4);
			lhs.SetRow(0, new Vector4(num6, 0f, 0f, num6));
			lhs.SetRow(1, new Vector4(0f, num7, 0f, num7));
			lhs.SetRow(2, new Vector4(0f, 0f, 1f, 0f));
			lhs.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
			Matrix4x4 value3 = lhs * projectionMatrix;
			material.SetMatrix(ScreenSpaceReflectionComponent.Uniforms._ProjectToPixelMatrix, value3);
			material.SetMatrix(ScreenSpaceReflectionComponent.Uniforms._WorldToCameraMatrix, camera.worldToCameraMatrix);
			material.SetMatrix(ScreenSpaceReflectionComponent.Uniforms._CameraToWorldMatrix, camera.worldToCameraMatrix.inverse);
			RenderTextureFormat format = (!this.context.isHdr) ? RenderTextureFormat.ARGB32 : RenderTextureFormat.ARGBHalf;
			int normalAndRoughnessTexture = ScreenSpaceReflectionComponent.Uniforms._NormalAndRoughnessTexture;
			int hitPointTexture = ScreenSpaceReflectionComponent.Uniforms._HitPointTexture;
			int blurTexture = ScreenSpaceReflectionComponent.Uniforms._BlurTexture;
			int filteredReflections = ScreenSpaceReflectionComponent.Uniforms._FilteredReflections;
			int finalReflectionTexture = ScreenSpaceReflectionComponent.Uniforms._FinalReflectionTexture;
			int tempTexture = ScreenSpaceReflectionComponent.Uniforms._TempTexture;
			cb.GetTemporaryRT(normalAndRoughnessTexture, -1, -1, 0, FilterMode.Point, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			cb.GetTemporaryRT(hitPointTexture, num2, num3, 0, FilterMode.Bilinear, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
			for (int i = 0; i < 5; i++)
			{
				cb.GetTemporaryRT(this.m_ReflectionTextures[i], num2 >> i, num3 >> i, 0, FilterMode.Bilinear, format);
			}
			cb.GetTemporaryRT(filteredReflections, num2, num3, 0, (!this.k_BilateralUpsample) ? FilterMode.Bilinear : FilterMode.Point, format);
			cb.GetTemporaryRT(finalReflectionTexture, num2, num3, 0, FilterMode.Point, format);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, normalAndRoughnessTexture, material, 6);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, hitPointTexture, material, 0);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, filteredReflections, material, 5);
			cb.Blit(filteredReflections, this.m_ReflectionTextures[0], material, 8);
			for (int j = 1; j < 5; j++)
			{
				int nameID = this.m_ReflectionTextures[j - 1];
				int num8 = j;
				cb.GetTemporaryRT(blurTexture, num2 >> num8, num3 >> num8, 0, FilterMode.Bilinear, format);
				cb.SetGlobalVector(ScreenSpaceReflectionComponent.Uniforms._Axis, new Vector4(1f, 0f, 0f, 0f));
				cb.SetGlobalFloat(ScreenSpaceReflectionComponent.Uniforms._CurrentMipLevel, (float)j - 1f);
				cb.Blit(nameID, blurTexture, material, 2);
				cb.SetGlobalVector(ScreenSpaceReflectionComponent.Uniforms._Axis, new Vector4(0f, 1f, 0f, 0f));
				nameID = this.m_ReflectionTextures[j];
				cb.Blit(blurTexture, nameID, material, 2);
				cb.ReleaseTemporaryRT(blurTexture);
			}
			cb.Blit(this.m_ReflectionTextures[0], finalReflectionTexture, material, 3);
			cb.GetTemporaryRT(tempTexture, camera.pixelWidth, camera.pixelHeight, 0, FilterMode.Bilinear, format);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, tempTexture, material, 1);
			cb.Blit(tempTexture, BuiltinRenderTextureType.CameraTarget);
			cb.ReleaseTemporaryRT(tempTexture);
		}

		// Token: 0x0400291B RID: 10523
		private bool k_HighlightSuppression;

		// Token: 0x0400291C RID: 10524
		private bool k_TraceBehindObjects = true;

		// Token: 0x0400291D RID: 10525
		private bool k_TreatBackfaceHitAsMiss;

		// Token: 0x0400291E RID: 10526
		private bool k_BilateralUpsample = true;

		// Token: 0x0400291F RID: 10527
		private readonly int[] m_ReflectionTextures = new int[5];

		// Token: 0x020007E8 RID: 2024
		private static class Uniforms
		{
			// Token: 0x04002920 RID: 10528
			internal static readonly int _RayStepSize = Shader.PropertyToID("_RayStepSize");

			// Token: 0x04002921 RID: 10529
			internal static readonly int _AdditiveReflection = Shader.PropertyToID("_AdditiveReflection");

			// Token: 0x04002922 RID: 10530
			internal static readonly int _BilateralUpsampling = Shader.PropertyToID("_BilateralUpsampling");

			// Token: 0x04002923 RID: 10531
			internal static readonly int _TreatBackfaceHitAsMiss = Shader.PropertyToID("_TreatBackfaceHitAsMiss");

			// Token: 0x04002924 RID: 10532
			internal static readonly int _AllowBackwardsRays = Shader.PropertyToID("_AllowBackwardsRays");

			// Token: 0x04002925 RID: 10533
			internal static readonly int _TraceBehindObjects = Shader.PropertyToID("_TraceBehindObjects");

			// Token: 0x04002926 RID: 10534
			internal static readonly int _MaxSteps = Shader.PropertyToID("_MaxSteps");

			// Token: 0x04002927 RID: 10535
			internal static readonly int _FullResolutionFiltering = Shader.PropertyToID("_FullResolutionFiltering");

			// Token: 0x04002928 RID: 10536
			internal static readonly int _HalfResolution = Shader.PropertyToID("_HalfResolution");

			// Token: 0x04002929 RID: 10537
			internal static readonly int _HighlightSuppression = Shader.PropertyToID("_HighlightSuppression");

			// Token: 0x0400292A RID: 10538
			internal static readonly int _PixelsPerMeterAtOneMeter = Shader.PropertyToID("_PixelsPerMeterAtOneMeter");

			// Token: 0x0400292B RID: 10539
			internal static readonly int _ScreenEdgeFading = Shader.PropertyToID("_ScreenEdgeFading");

			// Token: 0x0400292C RID: 10540
			internal static readonly int _ReflectionBlur = Shader.PropertyToID("_ReflectionBlur");

			// Token: 0x0400292D RID: 10541
			internal static readonly int _MaxRayTraceDistance = Shader.PropertyToID("_MaxRayTraceDistance");

			// Token: 0x0400292E RID: 10542
			internal static readonly int _FadeDistance = Shader.PropertyToID("_FadeDistance");

			// Token: 0x0400292F RID: 10543
			internal static readonly int _LayerThickness = Shader.PropertyToID("_LayerThickness");

			// Token: 0x04002930 RID: 10544
			internal static readonly int _SSRMultiplier = Shader.PropertyToID("_SSRMultiplier");

			// Token: 0x04002931 RID: 10545
			internal static readonly int _FresnelFade = Shader.PropertyToID("_FresnelFade");

			// Token: 0x04002932 RID: 10546
			internal static readonly int _FresnelFadePower = Shader.PropertyToID("_FresnelFadePower");

			// Token: 0x04002933 RID: 10547
			internal static readonly int _ReflectionBufferSize = Shader.PropertyToID("_ReflectionBufferSize");

			// Token: 0x04002934 RID: 10548
			internal static readonly int _ScreenSize = Shader.PropertyToID("_ScreenSize");

			// Token: 0x04002935 RID: 10549
			internal static readonly int _InvScreenSize = Shader.PropertyToID("_InvScreenSize");

			// Token: 0x04002936 RID: 10550
			internal static readonly int _ProjInfo = Shader.PropertyToID("_ProjInfo");

			// Token: 0x04002937 RID: 10551
			internal static readonly int _CameraClipInfo = Shader.PropertyToID("_CameraClipInfo");

			// Token: 0x04002938 RID: 10552
			internal static readonly int _ProjectToPixelMatrix = Shader.PropertyToID("_ProjectToPixelMatrix");

			// Token: 0x04002939 RID: 10553
			internal static readonly int _WorldToCameraMatrix = Shader.PropertyToID("_WorldToCameraMatrix");

			// Token: 0x0400293A RID: 10554
			internal static readonly int _CameraToWorldMatrix = Shader.PropertyToID("_CameraToWorldMatrix");

			// Token: 0x0400293B RID: 10555
			internal static readonly int _Axis = Shader.PropertyToID("_Axis");

			// Token: 0x0400293C RID: 10556
			internal static readonly int _CurrentMipLevel = Shader.PropertyToID("_CurrentMipLevel");

			// Token: 0x0400293D RID: 10557
			internal static readonly int _NormalAndRoughnessTexture = Shader.PropertyToID("_NormalAndRoughnessTexture");

			// Token: 0x0400293E RID: 10558
			internal static readonly int _HitPointTexture = Shader.PropertyToID("_HitPointTexture");

			// Token: 0x0400293F RID: 10559
			internal static readonly int _BlurTexture = Shader.PropertyToID("_BlurTexture");

			// Token: 0x04002940 RID: 10560
			internal static readonly int _FilteredReflections = Shader.PropertyToID("_FilteredReflections");

			// Token: 0x04002941 RID: 10561
			internal static readonly int _FinalReflectionTexture = Shader.PropertyToID("_FinalReflectionTexture");

			// Token: 0x04002942 RID: 10562
			internal static readonly int _TempTexture = Shader.PropertyToID("_TempTexture");
		}

		// Token: 0x020007E9 RID: 2025
		private enum PassIndex
		{
			// Token: 0x04002944 RID: 10564
			RayTraceStep,
			// Token: 0x04002945 RID: 10565
			CompositeFinal,
			// Token: 0x04002946 RID: 10566
			Blur,
			// Token: 0x04002947 RID: 10567
			CompositeSSR,
			// Token: 0x04002948 RID: 10568
			MinMipGeneration,
			// Token: 0x04002949 RID: 10569
			HitPointToReflections,
			// Token: 0x0400294A RID: 10570
			BilateralKeyPack,
			// Token: 0x0400294B RID: 10571
			BlitDepthAsCSZ,
			// Token: 0x0400294C RID: 10572
			PoissonBlur
		}
	}
}
