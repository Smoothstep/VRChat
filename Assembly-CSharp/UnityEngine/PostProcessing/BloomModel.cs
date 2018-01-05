using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020007FB RID: 2043
	[Serializable]
	public class BloomModel : PostProcessingModel
	{
		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x060040CE RID: 16590 RVA: 0x00147FED File Offset: 0x001463ED
		// (set) Token: 0x060040CF RID: 16591 RVA: 0x00147FF5 File Offset: 0x001463F5
		public BloomModel.Settings settings
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

		// Token: 0x060040D0 RID: 16592 RVA: 0x00147FFE File Offset: 0x001463FE
		public override void Reset()
		{
			this.m_Settings = BloomModel.Settings.defaultSettings;
		}

		// Token: 0x04002988 RID: 10632
		[SerializeField]
		private BloomModel.Settings m_Settings = BloomModel.Settings.defaultSettings;

		// Token: 0x020007FC RID: 2044
		[Serializable]
		public struct BloomSettings
		{
			// Token: 0x17000A45 RID: 2629
			// (get) Token: 0x060040D2 RID: 16594 RVA: 0x00148019 File Offset: 0x00146419
			// (set) Token: 0x060040D1 RID: 16593 RVA: 0x0014800B File Offset: 0x0014640B
			public float thresholdLinear
			{
				get
				{
					return Mathf.GammaToLinearSpace(this.threshold);
				}
				set
				{
					this.threshold = Mathf.LinearToGammaSpace(value);
				}
			}

			// Token: 0x17000A46 RID: 2630
			// (get) Token: 0x060040D3 RID: 16595 RVA: 0x00148028 File Offset: 0x00146428
			public static BloomModel.BloomSettings defaultSettings
			{
				get
				{
					return new BloomModel.BloomSettings
					{
						intensity = 0.5f,
						threshold = 1.1f,
						softKnee = 0.5f,
						radius = 4f,
						antiFlicker = false
					};
				}
			}

			// Token: 0x04002989 RID: 10633
			[Min(0f)]
			[Tooltip("Strength of the bloom filter.")]
			public float intensity;

			// Token: 0x0400298A RID: 10634
			[Min(0f)]
			[Tooltip("Filters out pixels under this level of brightness.")]
			public float threshold;

			// Token: 0x0400298B RID: 10635
			[Range(0f, 1f)]
			[Tooltip("Makes transition between under/over-threshold gradual (0 = hard threshold, 1 = soft threshold).")]
			public float softKnee;

			// Token: 0x0400298C RID: 10636
			[Range(1f, 7f)]
			[Tooltip("Changes extent of veiling effects in a screen resolution-independent fashion.")]
			public float radius;

			// Token: 0x0400298D RID: 10637
			[Tooltip("Reduces flashing noise with an additional filter.")]
			public bool antiFlicker;
		}

		// Token: 0x020007FD RID: 2045
		[Serializable]
		public struct LensDirtSettings
		{
			// Token: 0x17000A47 RID: 2631
			// (get) Token: 0x060040D4 RID: 16596 RVA: 0x00148078 File Offset: 0x00146478
			public static BloomModel.LensDirtSettings defaultSettings
			{
				get
				{
					return new BloomModel.LensDirtSettings
					{
						texture = null,
						intensity = 3f
					};
				}
			}

			// Token: 0x0400298E RID: 10638
			[Tooltip("Dirtiness texture to add smudges or dust to the lens.")]
			public Texture texture;

			// Token: 0x0400298F RID: 10639
			[Min(0f)]
			[Tooltip("Amount of lens dirtiness.")]
			public float intensity;
		}

		// Token: 0x020007FE RID: 2046
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000A48 RID: 2632
			// (get) Token: 0x060040D5 RID: 16597 RVA: 0x001480A4 File Offset: 0x001464A4
			public static BloomModel.Settings defaultSettings
			{
				get
				{
					return new BloomModel.Settings
					{
						bloom = BloomModel.BloomSettings.defaultSettings,
						lensDirt = BloomModel.LensDirtSettings.defaultSettings
					};
				}
			}

			// Token: 0x04002990 RID: 10640
			public BloomModel.BloomSettings bloom;

			// Token: 0x04002991 RID: 10641
			public BloomModel.LensDirtSettings lensDirt;
		}
	}
}
