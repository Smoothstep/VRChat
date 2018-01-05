using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020007F0 RID: 2032
	[Serializable]
	public class AmbientOcclusionModel : PostProcessingModel
	{
		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x060040C0 RID: 16576 RVA: 0x00147BEA File Offset: 0x00145FEA
		// (set) Token: 0x060040C1 RID: 16577 RVA: 0x00147BF2 File Offset: 0x00145FF2
		public AmbientOcclusionModel.Settings settings
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

		// Token: 0x060040C2 RID: 16578 RVA: 0x00147BFB File Offset: 0x00145FFB
		public override void Reset()
		{
			this.m_Settings = AmbientOcclusionModel.Settings.defaultSettings;
		}

		// Token: 0x04002960 RID: 10592
		[SerializeField]
		private AmbientOcclusionModel.Settings m_Settings = AmbientOcclusionModel.Settings.defaultSettings;

		// Token: 0x020007F1 RID: 2033
		public enum SampleCount
		{
			// Token: 0x04002962 RID: 10594
			Lowest = 3,
			// Token: 0x04002963 RID: 10595
			Low = 6,
			// Token: 0x04002964 RID: 10596
			Medium = 10,
			// Token: 0x04002965 RID: 10597
			High = 16
		}

		// Token: 0x020007F2 RID: 2034
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000A3F RID: 2623
			// (get) Token: 0x060040C3 RID: 16579 RVA: 0x00147C08 File Offset: 0x00146008
			public static AmbientOcclusionModel.Settings defaultSettings
			{
				get
				{
					return new AmbientOcclusionModel.Settings
					{
						intensity = 1f,
						radius = 0.3f,
						sampleCount = AmbientOcclusionModel.SampleCount.Medium,
						downsampling = true,
						forceForwardCompatibility = false,
						ambientOnly = false,
						highPrecision = false
					};
				}
			}

			// Token: 0x04002966 RID: 10598
			[Range(0f, 4f)]
			[Tooltip("Degree of darkness produced by the effect.")]
			public float intensity;

			// Token: 0x04002967 RID: 10599
			[Min(0.0001f)]
			[Tooltip("Radius of sample points, which affects extent of darkened areas.")]
			public float radius;

			// Token: 0x04002968 RID: 10600
			[Tooltip("Number of sample points, which affects quality and performance.")]
			public AmbientOcclusionModel.SampleCount sampleCount;

			// Token: 0x04002969 RID: 10601
			[Tooltip("Halves the resolution of the effect to increase performance at the cost of visual quality.")]
			public bool downsampling;

			// Token: 0x0400296A RID: 10602
			[Tooltip("Forces compatibility with Forward rendered objects when working with the Deferred rendering path.")]
			public bool forceForwardCompatibility;

			// Token: 0x0400296B RID: 10603
			[Tooltip("Enables the ambient-only mode in that the effect only affects ambient lighting. This mode is only available with the Deferred rendering path and HDR rendering.")]
			public bool ambientOnly;

			// Token: 0x0400296C RID: 10604
			[Tooltip("Toggles the use of a higher precision depth texture with the forward rendering path (may impact performances). Has no effect with the deferred rendering path.")]
			public bool highPrecision;
		}
	}
}
