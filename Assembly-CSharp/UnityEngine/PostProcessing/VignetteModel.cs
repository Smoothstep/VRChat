using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000828 RID: 2088
	[Serializable]
	public class VignetteModel : PostProcessingModel
	{
		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x0600411E RID: 16670 RVA: 0x00148BD9 File Offset: 0x00146FD9
		// (set) Token: 0x0600411F RID: 16671 RVA: 0x00148BE1 File Offset: 0x00146FE1
		public VignetteModel.Settings settings
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

		// Token: 0x06004120 RID: 16672 RVA: 0x00148BEA File Offset: 0x00146FEA
		public override void Reset()
		{
			this.m_Settings = VignetteModel.Settings.defaultSettings;
		}

		// Token: 0x04002A21 RID: 10785
		[SerializeField]
		private VignetteModel.Settings m_Settings = VignetteModel.Settings.defaultSettings;

		// Token: 0x02000829 RID: 2089
		public enum Mode
		{
			// Token: 0x04002A23 RID: 10787
			Classic,
			// Token: 0x04002A24 RID: 10788
			Masked
		}

		// Token: 0x0200082A RID: 2090
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000A6C RID: 2668
			// (get) Token: 0x06004121 RID: 16673 RVA: 0x00148BF8 File Offset: 0x00146FF8
			public static VignetteModel.Settings defaultSettings
			{
				get
				{
					return new VignetteModel.Settings
					{
						mode = VignetteModel.Mode.Classic,
						color = new Color(0f, 0f, 0f, 1f),
						center = new Vector2(0.5f, 0.5f),
						intensity = 0.45f,
						smoothness = 0.2f,
						roundness = 1f,
						mask = null,
						opacity = 1f,
						rounded = false
					};
				}
			}

			// Token: 0x04002A25 RID: 10789
			[Tooltip("Use the \"Classic\" mode for parametric controls. Use the \"Masked\" mode to use your own texture mask.")]
			public VignetteModel.Mode mode;

			// Token: 0x04002A26 RID: 10790
			[ColorUsage(false)]
			[Tooltip("Vignette color. Use the alpha channel for transparency.")]
			public Color color;

			// Token: 0x04002A27 RID: 10791
			[Tooltip("Sets the vignette center point (screen center is [0.5,0.5]).")]
			public Vector2 center;

			// Token: 0x04002A28 RID: 10792
			[Range(0f, 1f)]
			[Tooltip("Amount of vignetting on screen.")]
			public float intensity;

			// Token: 0x04002A29 RID: 10793
			[Range(0.01f, 1f)]
			[Tooltip("Smoothness of the vignette borders.")]
			public float smoothness;

			// Token: 0x04002A2A RID: 10794
			[Range(0f, 1f)]
			[Tooltip("Lower values will make a square-ish vignette.")]
			public float roundness;

			// Token: 0x04002A2B RID: 10795
			[Tooltip("A black and white mask to use as a vignette.")]
			public Texture mask;

			// Token: 0x04002A2C RID: 10796
			[Range(0f, 1f)]
			[Tooltip("Mask opacity.")]
			public float opacity;

			// Token: 0x04002A2D RID: 10797
			[Tooltip("Should the vignette be perfectly round or be dependent on the current aspect ratio?")]
			public bool rounded;
		}
	}
}
