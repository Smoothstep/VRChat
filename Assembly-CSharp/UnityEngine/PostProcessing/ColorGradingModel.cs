using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000806 RID: 2054
	[Serializable]
	public class ColorGradingModel : PostProcessingModel
	{
		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x060040E5 RID: 16613 RVA: 0x00148279 File Offset: 0x00146679
		// (set) Token: 0x060040E6 RID: 16614 RVA: 0x00148281 File Offset: 0x00146681
		public ColorGradingModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
				this.OnValidate();
			}
		}

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x060040E7 RID: 16615 RVA: 0x00148290 File Offset: 0x00146690
		// (set) Token: 0x060040E8 RID: 16616 RVA: 0x00148298 File Offset: 0x00146698
		public bool isDirty { get; internal set; }

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x060040E9 RID: 16617 RVA: 0x001482A1 File Offset: 0x001466A1
		// (set) Token: 0x060040EA RID: 16618 RVA: 0x001482A9 File Offset: 0x001466A9
		public RenderTexture bakedLut { get; internal set; }

		// Token: 0x060040EB RID: 16619 RVA: 0x001482B2 File Offset: 0x001466B2
		public override void Reset()
		{
			this.m_Settings = ColorGradingModel.Settings.defaultSettings;
			this.OnValidate();
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x001482C5 File Offset: 0x001466C5
		public override void OnValidate()
		{
			this.isDirty = true;
		}

		// Token: 0x040029AB RID: 10667
		[SerializeField]
		private ColorGradingModel.Settings m_Settings = ColorGradingModel.Settings.defaultSettings;

		// Token: 0x02000807 RID: 2055
		public enum Tonemapper
		{
			// Token: 0x040029AF RID: 10671
			None,
			// Token: 0x040029B0 RID: 10672
			ACES,
			// Token: 0x040029B1 RID: 10673
			Neutral
		}

		// Token: 0x02000808 RID: 2056
		[Serializable]
		public struct TonemappingSettings
		{
			// Token: 0x17000A53 RID: 2643
			// (get) Token: 0x060040ED RID: 16621 RVA: 0x001482D0 File Offset: 0x001466D0
			public static ColorGradingModel.TonemappingSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.TonemappingSettings
					{
						tonemapper = ColorGradingModel.Tonemapper.Neutral,
						neutralBlackIn = 0.02f,
						neutralWhiteIn = 10f,
						neutralBlackOut = 0f,
						neutralWhiteOut = 10f,
						neutralWhiteLevel = 5.3f,
						neutralWhiteClip = 10f
					};
				}
			}

			// Token: 0x040029B2 RID: 10674
			[Tooltip("Tonemapping algorithm to use at the end of the color grading process. Use \"Neutral\" if you need a customizable tonemapper or \"Filmic\" to give a standard filmic look to your scenes.")]
			public ColorGradingModel.Tonemapper tonemapper;

			// Token: 0x040029B3 RID: 10675
			[Range(-0.1f, 0.1f)]
			public float neutralBlackIn;

			// Token: 0x040029B4 RID: 10676
			[Range(1f, 20f)]
			public float neutralWhiteIn;

			// Token: 0x040029B5 RID: 10677
			[Range(-0.09f, 0.1f)]
			public float neutralBlackOut;

			// Token: 0x040029B6 RID: 10678
			[Range(1f, 19f)]
			public float neutralWhiteOut;

			// Token: 0x040029B7 RID: 10679
			[Range(0.1f, 20f)]
			public float neutralWhiteLevel;

			// Token: 0x040029B8 RID: 10680
			[Range(1f, 10f)]
			public float neutralWhiteClip;
		}

		// Token: 0x02000809 RID: 2057
		[Serializable]
		public struct BasicSettings
		{
			// Token: 0x17000A54 RID: 2644
			// (get) Token: 0x060040EE RID: 16622 RVA: 0x00148338 File Offset: 0x00146738
			public static ColorGradingModel.BasicSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.BasicSettings
					{
						postExposure = 0f,
						temperature = 0f,
						tint = 0f,
						hueShift = 0f,
						saturation = 1f,
						contrast = 1f
					};
				}
			}

			// Token: 0x040029B9 RID: 10681
			[Tooltip("Adjusts the overall exposure of the scene in EV units. This is applied after HDR effect and right before tonemapping so it won't affect previous effects in the chain.")]
			public float postExposure;

			// Token: 0x040029BA RID: 10682
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to a custom color temperature.")]
			public float temperature;

			// Token: 0x040029BB RID: 10683
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to compensate for a green or magenta tint.")]
			public float tint;

			// Token: 0x040029BC RID: 10684
			[Range(-180f, 180f)]
			[Tooltip("Shift the hue of all colors.")]
			public float hueShift;

			// Token: 0x040029BD RID: 10685
			[Range(0f, 2f)]
			[Tooltip("Pushes the intensity of all colors.")]
			public float saturation;

			// Token: 0x040029BE RID: 10686
			[Range(0f, 2f)]
			[Tooltip("Expands or shrinks the overall range of tonal values.")]
			public float contrast;
		}

		// Token: 0x0200080A RID: 2058
		[Serializable]
		public struct ChannelMixerSettings
		{
			// Token: 0x17000A55 RID: 2645
			// (get) Token: 0x060040EF RID: 16623 RVA: 0x00148398 File Offset: 0x00146798
			public static ColorGradingModel.ChannelMixerSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ChannelMixerSettings
					{
						red = new Vector3(1f, 0f, 0f),
						green = new Vector3(0f, 1f, 0f),
						blue = new Vector3(0f, 0f, 1f),
						currentEditingChannel = 0
					};
				}
			}

			// Token: 0x040029BF RID: 10687
			public Vector3 red;

			// Token: 0x040029C0 RID: 10688
			public Vector3 green;

			// Token: 0x040029C1 RID: 10689
			public Vector3 blue;

			// Token: 0x040029C2 RID: 10690
			[HideInInspector]
			public int currentEditingChannel;
		}

		// Token: 0x0200080B RID: 2059
		[Serializable]
		public struct LogWheelsSettings
		{
			// Token: 0x17000A56 RID: 2646
			// (get) Token: 0x060040F0 RID: 16624 RVA: 0x00148408 File Offset: 0x00146808
			public static ColorGradingModel.LogWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LogWheelsSettings
					{
						slope = Color.clear,
						power = Color.clear,
						offset = Color.clear
					};
				}
			}

			// Token: 0x040029C3 RID: 10691
			[Trackball("GetSlopeValue")]
			public Color slope;

			// Token: 0x040029C4 RID: 10692
			[Trackball("GetPowerValue")]
			public Color power;

			// Token: 0x040029C5 RID: 10693
			[Trackball("GetOffsetValue")]
			public Color offset;
		}

		// Token: 0x0200080C RID: 2060
		[Serializable]
		public struct LinearWheelsSettings
		{
			// Token: 0x17000A57 RID: 2647
			// (get) Token: 0x060040F1 RID: 16625 RVA: 0x00148444 File Offset: 0x00146844
			public static ColorGradingModel.LinearWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LinearWheelsSettings
					{
						lift = Color.clear,
						gamma = Color.clear,
						gain = Color.clear
					};
				}
			}

			// Token: 0x040029C6 RID: 10694
			[Trackball("GetLiftValue")]
			public Color lift;

			// Token: 0x040029C7 RID: 10695
			[Trackball("GetGammaValue")]
			public Color gamma;

			// Token: 0x040029C8 RID: 10696
			[Trackball("GetGainValue")]
			public Color gain;
		}

		// Token: 0x0200080D RID: 2061
		public enum ColorWheelMode
		{
			// Token: 0x040029CA RID: 10698
			Linear,
			// Token: 0x040029CB RID: 10699
			Log
		}

		// Token: 0x0200080E RID: 2062
		[Serializable]
		public struct ColorWheelsSettings
		{
			// Token: 0x17000A58 RID: 2648
			// (get) Token: 0x060040F2 RID: 16626 RVA: 0x00148480 File Offset: 0x00146880
			public static ColorGradingModel.ColorWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ColorWheelsSettings
					{
						mode = ColorGradingModel.ColorWheelMode.Log,
						log = ColorGradingModel.LogWheelsSettings.defaultSettings,
						linear = ColorGradingModel.LinearWheelsSettings.defaultSettings
					};
				}
			}

			// Token: 0x040029CC RID: 10700
			public ColorGradingModel.ColorWheelMode mode;

			// Token: 0x040029CD RID: 10701
			[TrackballGroup]
			public ColorGradingModel.LogWheelsSettings log;

			// Token: 0x040029CE RID: 10702
			[TrackballGroup]
			public ColorGradingModel.LinearWheelsSettings linear;
		}

		// Token: 0x0200080F RID: 2063
		[Serializable]
		public struct CurvesSettings
		{
			// Token: 0x17000A59 RID: 2649
			// (get) Token: 0x060040F3 RID: 16627 RVA: 0x001484B8 File Offset: 0x001468B8
			public static ColorGradingModel.CurvesSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.CurvesSettings
					{
						master = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						red = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						green = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						blue = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						hueVShue = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						hueVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						satVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						lumVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						e_CurrentEditingCurve = 0,
						e_CurveY = true,
						e_CurveR = false,
						e_CurveG = false,
						e_CurveB = false
					};
				}
			}

			// Token: 0x040029CF RID: 10703
			public ColorGradingCurve master;

			// Token: 0x040029D0 RID: 10704
			public ColorGradingCurve red;

			// Token: 0x040029D1 RID: 10705
			public ColorGradingCurve green;

			// Token: 0x040029D2 RID: 10706
			public ColorGradingCurve blue;

			// Token: 0x040029D3 RID: 10707
			public ColorGradingCurve hueVShue;

			// Token: 0x040029D4 RID: 10708
			public ColorGradingCurve hueVSsat;

			// Token: 0x040029D5 RID: 10709
			public ColorGradingCurve satVSsat;

			// Token: 0x040029D6 RID: 10710
			public ColorGradingCurve lumVSsat;

			// Token: 0x040029D7 RID: 10711
			[HideInInspector]
			public int e_CurrentEditingCurve;

			// Token: 0x040029D8 RID: 10712
			[HideInInspector]
			public bool e_CurveY;

			// Token: 0x040029D9 RID: 10713
			[HideInInspector]
			public bool e_CurveR;

			// Token: 0x040029DA RID: 10714
			[HideInInspector]
			public bool e_CurveG;

			// Token: 0x040029DB RID: 10715
			[HideInInspector]
			public bool e_CurveB;
		}

		// Token: 0x02000810 RID: 2064
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000A5A RID: 2650
			// (get) Token: 0x060040F4 RID: 16628 RVA: 0x00148768 File Offset: 0x00146B68
			public static ColorGradingModel.Settings defaultSettings
			{
				get
				{
					return new ColorGradingModel.Settings
					{
						tonemapping = ColorGradingModel.TonemappingSettings.defaultSettings,
						basic = ColorGradingModel.BasicSettings.defaultSettings,
						channelMixer = ColorGradingModel.ChannelMixerSettings.defaultSettings,
						colorWheels = ColorGradingModel.ColorWheelsSettings.defaultSettings,
						curves = ColorGradingModel.CurvesSettings.defaultSettings
					};
				}
			}

			// Token: 0x040029DC RID: 10716
			public ColorGradingModel.TonemappingSettings tonemapping;

			// Token: 0x040029DD RID: 10717
			public ColorGradingModel.BasicSettings basic;

			// Token: 0x040029DE RID: 10718
			public ColorGradingModel.ChannelMixerSettings channelMixer;

			// Token: 0x040029DF RID: 10719
			public ColorGradingModel.ColorWheelsSettings colorWheels;

			// Token: 0x040029E0 RID: 10720
			public ColorGradingModel.CurvesSettings curves;
		}
	}
}
