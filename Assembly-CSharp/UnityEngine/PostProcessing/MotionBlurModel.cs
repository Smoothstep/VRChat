using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200081D RID: 2077
	[Serializable]
	public class MotionBlurModel : PostProcessingModel
	{
		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x0600410F RID: 16655 RVA: 0x00148A11 File Offset: 0x00146E11
		// (set) Token: 0x06004110 RID: 16656 RVA: 0x00148A19 File Offset: 0x00146E19
		public MotionBlurModel.Settings settings
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

		// Token: 0x06004111 RID: 16657 RVA: 0x00148A22 File Offset: 0x00146E22
		public override void Reset()
		{
			this.m_Settings = MotionBlurModel.Settings.defaultSettings;
		}

		// Token: 0x04002A03 RID: 10755
		[SerializeField]
		private MotionBlurModel.Settings m_Settings = MotionBlurModel.Settings.defaultSettings;

		// Token: 0x0200081E RID: 2078
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000A66 RID: 2662
			// (get) Token: 0x06004112 RID: 16658 RVA: 0x00148A30 File Offset: 0x00146E30
			public static MotionBlurModel.Settings defaultSettings
			{
				get
				{
					return new MotionBlurModel.Settings
					{
						shutterAngle = 270f,
						sampleCount = 10,
						frameBlending = 0f
					};
				}
			}

			// Token: 0x04002A04 RID: 10756
			[Range(0f, 360f)]
			[Tooltip("The angle of rotary shutter. Larger values give longer exposure.")]
			public float shutterAngle;

			// Token: 0x04002A05 RID: 10757
			[Range(4f, 32f)]
			[Tooltip("The amount of sample points, which affects quality and performances.")]
			public int sampleCount;

			// Token: 0x04002A06 RID: 10758
			[Range(0f, 1f)]
			[Tooltip("The strength of multiple frame blending. The opacity of preceding frames are determined from this coefficient and time differences.")]
			public float frameBlending;
		}
	}
}
