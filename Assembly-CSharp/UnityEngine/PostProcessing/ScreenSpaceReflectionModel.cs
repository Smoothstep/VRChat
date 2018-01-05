using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200081F RID: 2079
	[Serializable]
	public class ScreenSpaceReflectionModel : PostProcessingModel
	{
		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06004114 RID: 16660 RVA: 0x00148A7A File Offset: 0x00146E7A
		// (set) Token: 0x06004115 RID: 16661 RVA: 0x00148A82 File Offset: 0x00146E82
		public ScreenSpaceReflectionModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x00148A8B File Offset: 0x00146E8B
		public override void Reset()
		{
			this.m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings;
		}

		// Token: 0x04002A07 RID: 10759
		[SerializeField]
		private ScreenSpaceReflectionModel.Settings m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings;

		// Token: 0x02000820 RID: 2080
		public enum SSRResolution
		{
			// Token: 0x04002A09 RID: 10761
			High,
			// Token: 0x04002A0A RID: 10762
			Low = 2
		}

		// Token: 0x02000821 RID: 2081
		public enum SSRReflectionBlendType
		{
			// Token: 0x04002A0C RID: 10764
			PhysicallyBased,
			// Token: 0x04002A0D RID: 10765
			Additive
		}

		// Token: 0x02000822 RID: 2082
		[Serializable]
		public struct IntensitySettings
		{
			// Token: 0x04002A0E RID: 10766
			[Tooltip("Nonphysical multiplier for the SSR reflections. 1.0 is physically based.")]
			[Range(0f, 2f)]
			public float reflectionMultiplier;

			// Token: 0x04002A0F RID: 10767
			[Tooltip("How far away from the maxDistance to begin fading SSR.")]
			[Range(0f, 1000f)]
			public float fadeDistance;

			// Token: 0x04002A10 RID: 10768
			[Tooltip("Amplify Fresnel fade out. Increase if floor reflections look good close to the surface and bad farther 'under' the floor.")]
			[Range(0f, 1f)]
			public float fresnelFade;

			// Token: 0x04002A11 RID: 10769
			[Tooltip("Higher values correspond to a faster Fresnel fade as the reflection changes from the grazing angle.")]
			[Range(0.1f, 10f)]
			public float fresnelFadePower;
		}

		// Token: 0x02000823 RID: 2083
		[Serializable]
		public struct ReflectionSettings
		{
			// Token: 0x04002A12 RID: 10770
			[Tooltip("How the reflections are blended into the render.")]
			public ScreenSpaceReflectionModel.SSRReflectionBlendType blendType;

			// Token: 0x04002A13 RID: 10771
			[Tooltip("Half resolution SSRR is much faster, but less accurate.")]
			public ScreenSpaceReflectionModel.SSRResolution reflectionQuality;

			// Token: 0x04002A14 RID: 10772
			[Tooltip("Maximum reflection distance in world units.")]
			[Range(0.1f, 300f)]
			public float maxDistance;

			// Token: 0x04002A15 RID: 10773
			[Tooltip("Max raytracing length.")]
			[Range(16f, 1024f)]
			public int iterationCount;

			// Token: 0x04002A16 RID: 10774
			[Tooltip("Log base 2 of ray tracing coarse step size. Higher traces farther, lower gives better quality silhouettes.")]
			[Range(1f, 16f)]
			public int stepSize;

			// Token: 0x04002A17 RID: 10775
			[Tooltip("Typical thickness of columns, walls, furniture, and other objects that reflection rays might pass behind.")]
			[Range(0.01f, 10f)]
			public float widthModifier;

			// Token: 0x04002A18 RID: 10776
			[Tooltip("Blurriness of reflections.")]
			[Range(0.1f, 8f)]
			public float reflectionBlur;

			// Token: 0x04002A19 RID: 10777
			[Tooltip("Disable for a performance gain in scenes where most glossy objects are horizontal, like floors, water, and tables. Leave on for scenes with glossy vertical objects.")]
			public bool reflectBackfaces;
		}

		// Token: 0x02000824 RID: 2084
		[Serializable]
		public struct ScreenEdgeMask
		{
			// Token: 0x04002A1A RID: 10778
			[Tooltip("Higher = fade out SSRR near the edge of the screen so that reflections don't pop under camera motion.")]
			[Range(0f, 1f)]
			public float intensity;
		}

		// Token: 0x02000825 RID: 2085
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000A68 RID: 2664
			// (get) Token: 0x06004117 RID: 16663 RVA: 0x00148A98 File Offset: 0x00146E98
			public static ScreenSpaceReflectionModel.Settings defaultSettings
			{
				get
				{
					return new ScreenSpaceReflectionModel.Settings
					{
						reflection = new ScreenSpaceReflectionModel.ReflectionSettings
						{
							blendType = ScreenSpaceReflectionModel.SSRReflectionBlendType.PhysicallyBased,
							reflectionQuality = ScreenSpaceReflectionModel.SSRResolution.Low,
							maxDistance = 100f,
							iterationCount = 256,
							stepSize = 3,
							widthModifier = 0.5f,
							reflectionBlur = 1f,
							reflectBackfaces = false
						},
						intensity = new ScreenSpaceReflectionModel.IntensitySettings
						{
							reflectionMultiplier = 1f,
							fadeDistance = 100f,
							fresnelFade = 1f,
							fresnelFadePower = 1f
						},
						screenEdgeMask = new ScreenSpaceReflectionModel.ScreenEdgeMask
						{
							intensity = 0.03f
						}
					};
				}
			}

			// Token: 0x04002A1B RID: 10779
			public ScreenSpaceReflectionModel.ReflectionSettings reflection;

			// Token: 0x04002A1C RID: 10780
			public ScreenSpaceReflectionModel.IntensitySettings intensity;

			// Token: 0x04002A1D RID: 10781
			public ScreenSpaceReflectionModel.ScreenEdgeMask screenEdgeMask;
		}
	}
}
