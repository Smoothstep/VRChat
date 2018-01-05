using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000811 RID: 2065
	[Serializable]
	public class DepthOfFieldModel : PostProcessingModel
	{
		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x060040F6 RID: 16630 RVA: 0x001487CD File Offset: 0x00146BCD
		// (set) Token: 0x060040F7 RID: 16631 RVA: 0x001487D5 File Offset: 0x00146BD5
		public DepthOfFieldModel.Settings settings
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

		// Token: 0x060040F8 RID: 16632 RVA: 0x001487DE File Offset: 0x00146BDE
		public override void Reset()
		{
			this.m_Settings = DepthOfFieldModel.Settings.defaultSettings;
		}

		// Token: 0x040029E1 RID: 10721
		[SerializeField]
		private DepthOfFieldModel.Settings m_Settings = DepthOfFieldModel.Settings.defaultSettings;

		// Token: 0x02000812 RID: 2066
		public enum KernelSize
		{
			// Token: 0x040029E3 RID: 10723
			Small,
			// Token: 0x040029E4 RID: 10724
			Medium,
			// Token: 0x040029E5 RID: 10725
			Large,
			// Token: 0x040029E6 RID: 10726
			VeryLarge
		}

		// Token: 0x02000813 RID: 2067
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000A5C RID: 2652
			// (get) Token: 0x060040F9 RID: 16633 RVA: 0x001487EC File Offset: 0x00146BEC
			public static DepthOfFieldModel.Settings defaultSettings
			{
				get
				{
					return new DepthOfFieldModel.Settings
					{
						focusDistance = 10f,
						aperture = 5.6f,
						focalLength = 50f,
						useCameraFov = false,
						kernelSize = DepthOfFieldModel.KernelSize.Medium
					};
				}
			}

			// Token: 0x040029E7 RID: 10727
			[Min(0.1f)]
			[Tooltip("Distance to the point of focus.")]
			public float focusDistance;

			// Token: 0x040029E8 RID: 10728
			[Range(0.05f, 32f)]
			[Tooltip("Ratio of aperture (known as f-stop or f-number). The smaller the value is, the shallower the depth of field is.")]
			public float aperture;

			// Token: 0x040029E9 RID: 10729
			[Range(1f, 300f)]
			[Tooltip("Distance between the lens and the film. The larger the value is, the shallower the depth of field is.")]
			public float focalLength;

			// Token: 0x040029EA RID: 10730
			[Tooltip("Calculate the focal length automatically from the field-of-view value set on the camera. Using this setting isn't recommended.")]
			public bool useCameraFov;

			// Token: 0x040029EB RID: 10731
			[Tooltip("Convolution kernel size of the bokeh filter, which determines the maximum radius of bokeh. It also affects the performance (the larger the kernel is, the longer the GPU time is required).")]
			public DepthOfFieldModel.KernelSize kernelSize;
		}
	}
}
