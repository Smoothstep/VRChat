using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020007FF RID: 2047
	[Serializable]
	public class BuiltinDebugViewsModel : PostProcessingModel
	{
		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x060040D7 RID: 16599 RVA: 0x001480E5 File Offset: 0x001464E5
		// (set) Token: 0x060040D8 RID: 16600 RVA: 0x001480ED File Offset: 0x001464ED
		public BuiltinDebugViewsModel.Settings settings
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

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x060040D9 RID: 16601 RVA: 0x001480F6 File Offset: 0x001464F6
		public bool willInterrupt
		{
			get
			{
				return !this.IsModeActive(BuiltinDebugViewsModel.Mode.None) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.PreGradingLog) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.LogLut) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.UserLut);
			}
		}

		// Token: 0x060040DA RID: 16602 RVA: 0x00148136 File Offset: 0x00146536
		public override void Reset()
		{
			this.settings = BuiltinDebugViewsModel.Settings.defaultSettings;
		}

		// Token: 0x060040DB RID: 16603 RVA: 0x00148143 File Offset: 0x00146543
		public bool IsModeActive(BuiltinDebugViewsModel.Mode mode)
		{
			return this.m_Settings.mode == mode;
		}

		// Token: 0x04002992 RID: 10642
		[SerializeField]
		private BuiltinDebugViewsModel.Settings m_Settings = BuiltinDebugViewsModel.Settings.defaultSettings;

		// Token: 0x02000800 RID: 2048
		[Serializable]
		public struct DepthSettings
		{
			// Token: 0x17000A4B RID: 2635
			// (get) Token: 0x060040DC RID: 16604 RVA: 0x00148154 File Offset: 0x00146554
			public static BuiltinDebugViewsModel.DepthSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.DepthSettings
					{
						scale = 1f
					};
				}
			}

			// Token: 0x04002993 RID: 10643
			[Range(0f, 1f)]
			[Tooltip("Scales the camera far plane before displaying the depth map.")]
			public float scale;
		}

		// Token: 0x02000801 RID: 2049
		[Serializable]
		public struct MotionVectorsSettings
		{
			// Token: 0x17000A4C RID: 2636
			// (get) Token: 0x060040DD RID: 16605 RVA: 0x00148178 File Offset: 0x00146578
			public static BuiltinDebugViewsModel.MotionVectorsSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.MotionVectorsSettings
					{
						sourceOpacity = 1f,
						motionImageOpacity = 0f,
						motionImageAmplitude = 16f,
						motionVectorsOpacity = 1f,
						motionVectorsResolution = 24,
						motionVectorsAmplitude = 64f
					};
				}
			}

			// Token: 0x04002994 RID: 10644
			[Range(0f, 1f)]
			[Tooltip("Opacity of the source render.")]
			public float sourceOpacity;

			// Token: 0x04002995 RID: 10645
			[Range(0f, 1f)]
			[Tooltip("Opacity of the per-pixel motion vector colors.")]
			public float motionImageOpacity;

			// Token: 0x04002996 RID: 10646
			[Min(0f)]
			[Tooltip("Because motion vectors are mainly very small vectors, you can use this setting to make them more visible.")]
			public float motionImageAmplitude;

			// Token: 0x04002997 RID: 10647
			[Range(0f, 1f)]
			[Tooltip("Opacity for the motion vector arrows.")]
			public float motionVectorsOpacity;

			// Token: 0x04002998 RID: 10648
			[Range(8f, 64f)]
			[Tooltip("The arrow density on screen.")]
			public int motionVectorsResolution;

			// Token: 0x04002999 RID: 10649
			[Min(0f)]
			[Tooltip("Tweaks the arrows length.")]
			public float motionVectorsAmplitude;
		}

		// Token: 0x02000802 RID: 2050
		public enum Mode
		{
			// Token: 0x0400299B RID: 10651
			None,
			// Token: 0x0400299C RID: 10652
			Depth,
			// Token: 0x0400299D RID: 10653
			Normals,
			// Token: 0x0400299E RID: 10654
			MotionVectors,
			// Token: 0x0400299F RID: 10655
			AmbientOcclusion,
			// Token: 0x040029A0 RID: 10656
			EyeAdaptation,
			// Token: 0x040029A1 RID: 10657
			FocusPlane,
			// Token: 0x040029A2 RID: 10658
			PreGradingLog,
			// Token: 0x040029A3 RID: 10659
			LogLut,
			// Token: 0x040029A4 RID: 10660
			UserLut
		}

		// Token: 0x02000803 RID: 2051
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000A4D RID: 2637
			// (get) Token: 0x060040DE RID: 16606 RVA: 0x001481D4 File Offset: 0x001465D4
			public static BuiltinDebugViewsModel.Settings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.Settings
					{
						mode = BuiltinDebugViewsModel.Mode.None,
						depth = BuiltinDebugViewsModel.DepthSettings.defaultSettings,
						motionVectors = BuiltinDebugViewsModel.MotionVectorsSettings.defaultSettings
					};
				}
			}

			// Token: 0x040029A5 RID: 10661
			public BuiltinDebugViewsModel.Mode mode;

			// Token: 0x040029A6 RID: 10662
			public BuiltinDebugViewsModel.DepthSettings depth;

			// Token: 0x040029A7 RID: 10663
			public BuiltinDebugViewsModel.MotionVectorsSettings motionVectors;
		}
	}
}
