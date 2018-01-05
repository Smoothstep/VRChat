using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020007F3 RID: 2035
	[Serializable]
	public class AntialiasingModel : PostProcessingModel
	{
		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x060040C5 RID: 16581 RVA: 0x00147C72 File Offset: 0x00146072
		// (set) Token: 0x060040C6 RID: 16582 RVA: 0x00147C7A File Offset: 0x0014607A
		public AntialiasingModel.Settings settings
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

		// Token: 0x060040C7 RID: 16583 RVA: 0x00147C83 File Offset: 0x00146083
		public override void Reset()
		{
			this.m_Settings = AntialiasingModel.Settings.defaultSettings;
		}

		// Token: 0x0400296D RID: 10605
		[SerializeField]
		private AntialiasingModel.Settings m_Settings = AntialiasingModel.Settings.defaultSettings;

		// Token: 0x020007F4 RID: 2036
		public enum Method
		{
			// Token: 0x0400296F RID: 10607
			Fxaa,
			// Token: 0x04002970 RID: 10608
			Taa
		}

		// Token: 0x020007F5 RID: 2037
		public enum FxaaPreset
		{
			// Token: 0x04002972 RID: 10610
			ExtremePerformance,
			// Token: 0x04002973 RID: 10611
			Performance,
			// Token: 0x04002974 RID: 10612
			Default,
			// Token: 0x04002975 RID: 10613
			Quality,
			// Token: 0x04002976 RID: 10614
			ExtremeQuality
		}

		// Token: 0x020007F6 RID: 2038
		[Serializable]
		public struct FxaaQualitySettings
		{
			// Token: 0x04002977 RID: 10615
			[Tooltip("The amount of desired sub-pixel aliasing removal. Effects the sharpeness of the output.")]
			[Range(0f, 1f)]
			public float subpixelAliasingRemovalAmount;

			// Token: 0x04002978 RID: 10616
			[Tooltip("The minimum amount of local contrast required to qualify a region as containing an edge.")]
			[Range(0.063f, 0.333f)]
			public float edgeDetectionThreshold;

			// Token: 0x04002979 RID: 10617
			[Tooltip("Local contrast adaptation value to disallow the algorithm from executing on the darker regions.")]
			[Range(0f, 0.0833f)]
			public float minimumRequiredLuminance;

			// Token: 0x0400297A RID: 10618
			public static AntialiasingModel.FxaaQualitySettings[] presets = new AntialiasingModel.FxaaQualitySettings[]
			{
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0f,
					edgeDetectionThreshold = 0.333f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0.25f,
					edgeDetectionThreshold = 0.25f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0.75f,
					edgeDetectionThreshold = 0.166f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 1f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.0625f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 1f,
					edgeDetectionThreshold = 0.063f,
					minimumRequiredLuminance = 0.0312f
				}
			};
		}

		// Token: 0x020007F7 RID: 2039
		[Serializable]
		public struct FxaaConsoleSettings
		{
			// Token: 0x0400297B RID: 10619
			[Tooltip("The amount of spread applied to the sampling coordinates while sampling for subpixel information.")]
			[Range(0.33f, 0.5f)]
			public float subpixelSpreadAmount;

			// Token: 0x0400297C RID: 10620
			[Tooltip("This value dictates how sharp the edges in the image are kept; a higher value implies sharper edges.")]
			[Range(2f, 8f)]
			public float edgeSharpnessAmount;

			// Token: 0x0400297D RID: 10621
			[Tooltip("The minimum amount of local contrast required to qualify a region as containing an edge.")]
			[Range(0.125f, 0.25f)]
			public float edgeDetectionThreshold;

			// Token: 0x0400297E RID: 10622
			[Tooltip("Local contrast adaptation value to disallow the algorithm from executing on the darker regions.")]
			[Range(0.04f, 0.06f)]
			public float minimumRequiredLuminance;

			// Token: 0x0400297F RID: 10623
			public static AntialiasingModel.FxaaConsoleSettings[] presets = new AntialiasingModel.FxaaConsoleSettings[]
			{
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.33f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.25f,
					minimumRequiredLuminance = 0.06f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.33f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.06f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.05f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 4f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.04f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 2f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.04f
				}
			};
		}

		// Token: 0x020007F8 RID: 2040
		[Serializable]
		public struct FxaaSettings
		{
			// Token: 0x17000A41 RID: 2625
			// (get) Token: 0x060040CA RID: 16586 RVA: 0x00147F3C File Offset: 0x0014633C
			public static AntialiasingModel.FxaaSettings defaultSettings
			{
				get
				{
					return new AntialiasingModel.FxaaSettings
					{
						preset = AntialiasingModel.FxaaPreset.Default
					};
				}
			}

			// Token: 0x04002980 RID: 10624
			public AntialiasingModel.FxaaPreset preset;
		}

		// Token: 0x020007F9 RID: 2041
		[Serializable]
		public struct TaaSettings
		{
			// Token: 0x17000A42 RID: 2626
			// (get) Token: 0x060040CB RID: 16587 RVA: 0x00147F5C File Offset: 0x0014635C
			public static AntialiasingModel.TaaSettings defaultSettings
			{
				get
				{
					return new AntialiasingModel.TaaSettings
					{
						jitterSpread = 0.75f,
						sharpen = 0.3f,
						stationaryBlending = 0.95f,
						motionBlending = 0.85f
					};
				}
			}

			// Token: 0x04002981 RID: 10625
			[Tooltip("The diameter (in texels) inside which jitter samples are spread. Smaller values result in crisper but more aliased output, while larger values result in more stable but blurrier output.")]
			[Range(0.1f, 1f)]
			public float jitterSpread;

			// Token: 0x04002982 RID: 10626
			[Tooltip("Controls the amount of sharpening applied to the color buffer.")]
			[Range(0f, 3f)]
			public float sharpen;

			// Token: 0x04002983 RID: 10627
			[Tooltip("The blend coefficient for a stationary fragment. Controls the percentage of history sample blended into the final color.")]
			[Range(0f, 0.99f)]
			public float stationaryBlending;

			// Token: 0x04002984 RID: 10628
			[Tooltip("The blend coefficient for a fragment with significant motion. Controls the percentage of history sample blended into the final color.")]
			[Range(0f, 0.99f)]
			public float motionBlending;
		}

		// Token: 0x020007FA RID: 2042
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000A43 RID: 2627
			// (get) Token: 0x060040CC RID: 16588 RVA: 0x00147FA4 File Offset: 0x001463A4
			public static AntialiasingModel.Settings defaultSettings
			{
				get
				{
					return new AntialiasingModel.Settings
					{
						method = AntialiasingModel.Method.Fxaa,
						fxaaSettings = AntialiasingModel.FxaaSettings.defaultSettings,
						taaSettings = AntialiasingModel.TaaSettings.defaultSettings
					};
				}
			}

			// Token: 0x04002985 RID: 10629
			public AntialiasingModel.Method method;

			// Token: 0x04002986 RID: 10630
			public AntialiasingModel.FxaaSettings fxaaSettings;

			// Token: 0x04002987 RID: 10631
			public AntialiasingModel.TaaSettings taaSettings;
		}
	}
}
