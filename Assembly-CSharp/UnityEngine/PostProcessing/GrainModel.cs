using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200081B RID: 2075
	[Serializable]
	public class GrainModel : PostProcessingModel
	{
		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x0600410A RID: 16650 RVA: 0x0014899D File Offset: 0x00146D9D
		// (set) Token: 0x0600410B RID: 16651 RVA: 0x001489A5 File Offset: 0x00146DA5
		public GrainModel.Settings settings
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

		// Token: 0x0600410C RID: 16652 RVA: 0x001489AE File Offset: 0x00146DAE
		public override void Reset()
		{
			this.m_Settings = GrainModel.Settings.defaultSettings;
		}

		// Token: 0x040029FE RID: 10750
		[SerializeField]
		private GrainModel.Settings m_Settings = GrainModel.Settings.defaultSettings;

		// Token: 0x0200081C RID: 2076
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000A64 RID: 2660
			// (get) Token: 0x0600410D RID: 16653 RVA: 0x001489BC File Offset: 0x00146DBC
			public static GrainModel.Settings defaultSettings
			{
				get
				{
					return new GrainModel.Settings
					{
						colored = true,
						intensity = 0.5f,
						size = 1f,
						luminanceContribution = 0.8f
					};
				}
			}

			// Token: 0x040029FF RID: 10751
			[Tooltip("Enable the use of colored grain.")]
			public bool colored;

			// Token: 0x04002A00 RID: 10752
			[Range(0f, 1f)]
			[Tooltip("Grain strength. Higher means more visible grain.")]
			public float intensity;

			// Token: 0x04002A01 RID: 10753
			[Range(0.3f, 3f)]
			[Tooltip("Grain particle size.")]
			public float size;

			// Token: 0x04002A02 RID: 10754
			[Range(0f, 1f)]
			[Tooltip("Controls the noisiness response curve based on scene luminance. Lower values mean less noise in dark areas.")]
			public float luminanceContribution;
		}
	}
}
