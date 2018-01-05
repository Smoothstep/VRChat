using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000804 RID: 2052
	[Serializable]
	public class ChromaticAberrationModel : PostProcessingModel
	{
		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x060040E0 RID: 16608 RVA: 0x0014821D File Offset: 0x0014661D
		// (set) Token: 0x060040E1 RID: 16609 RVA: 0x00148225 File Offset: 0x00146625
		public ChromaticAberrationModel.Settings settings
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

		// Token: 0x060040E2 RID: 16610 RVA: 0x0014822E File Offset: 0x0014662E
		public override void Reset()
		{
			this.m_Settings = ChromaticAberrationModel.Settings.defaultSettings;
		}

		// Token: 0x040029A8 RID: 10664
		[SerializeField]
		private ChromaticAberrationModel.Settings m_Settings = ChromaticAberrationModel.Settings.defaultSettings;

		// Token: 0x02000805 RID: 2053
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000A4F RID: 2639
			// (get) Token: 0x060040E3 RID: 16611 RVA: 0x0014823C File Offset: 0x0014663C
			public static ChromaticAberrationModel.Settings defaultSettings
			{
				get
				{
					return new ChromaticAberrationModel.Settings
					{
						spectralTexture = null,
						intensity = 0.1f
					};
				}
			}

			// Token: 0x040029A9 RID: 10665
			[Tooltip("Shift the hue of chromatic aberrations.")]
			public Texture2D spectralTexture;

			// Token: 0x040029AA RID: 10666
			[Range(0f, 1f)]
			[Tooltip("Amount of tangential distortion.")]
			public float intensity;
		}
	}
}
