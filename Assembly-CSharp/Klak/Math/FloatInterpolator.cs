using System;
using UnityEngine;

namespace Klak.Math
{
	// Token: 0x0200051D RID: 1309
	public struct FloatInterpolator
	{
		// Token: 0x06002E00 RID: 11776 RVA: 0x000E18B4 File Offset: 0x000DFCB4
		public FloatInterpolator(float initialValue, FloatInterpolator.Config config)
		{
			this.config = config;
			this.targetValue = initialValue;
			this.currentValue = initialValue;
			this._velocity = 0f;
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06002E01 RID: 11777 RVA: 0x000E18E3 File Offset: 0x000DFCE3
		// (set) Token: 0x06002E02 RID: 11778 RVA: 0x000E18EB File Offset: 0x000DFCEB
		public FloatInterpolator.Config config { get; set; }

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06002E03 RID: 11779 RVA: 0x000E18F4 File Offset: 0x000DFCF4
		// (set) Token: 0x06002E04 RID: 11780 RVA: 0x000E18FC File Offset: 0x000DFCFC
		public float currentValue { get; set; }

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06002E05 RID: 11781 RVA: 0x000E1905 File Offset: 0x000DFD05
		// (set) Token: 0x06002E06 RID: 11782 RVA: 0x000E190D File Offset: 0x000DFD0D
		public float targetValue { get; set; }

		// Token: 0x06002E07 RID: 11783 RVA: 0x000E1916 File Offset: 0x000DFD16
		public float Step(float targetValue)
		{
			this.targetValue = targetValue;
			return this.Step();
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x000E1928 File Offset: 0x000DFD28
		public float Step()
		{
			if (this.config.interpolationType == FloatInterpolator.Config.InterpolationType.Exponential)
			{
				this.currentValue = ETween.Step(this.currentValue, this.targetValue, this.config.interpolationSpeed);
			}
			else if (this.config.interpolationType == FloatInterpolator.Config.InterpolationType.DampedSpring)
			{
				this.currentValue = DTween.Step(this.currentValue, this.targetValue, ref this._velocity, this.config.interpolationSpeed);
			}
			else
			{
				this.currentValue = this.targetValue;
			}
			return this.currentValue;
		}

		// Token: 0x04001855 RID: 6229
		private float _velocity;

		// Token: 0x0200051E RID: 1310
		[Serializable]
		public class Config
		{
			// Token: 0x06002E09 RID: 11785 RVA: 0x000E19BD File Offset: 0x000DFDBD
			public Config()
			{
			}

			// Token: 0x06002E0A RID: 11786 RVA: 0x000E19D7 File Offset: 0x000DFDD7
			public Config(FloatInterpolator.Config.InterpolationType type, float speed)
			{
				this._interpolationType = type;
				this._interpolationSpeed = speed;
			}

			// Token: 0x170006E9 RID: 1769
			// (get) Token: 0x06002E0B RID: 11787 RVA: 0x000E19FF File Offset: 0x000DFDFF
			// (set) Token: 0x06002E0C RID: 11788 RVA: 0x000E1A07 File Offset: 0x000DFE07
			public FloatInterpolator.Config.InterpolationType interpolationType
			{
				get
				{
					return this._interpolationType;
				}
				set
				{
					this._interpolationType = value;
				}
			}

			// Token: 0x170006EA RID: 1770
			// (get) Token: 0x06002E0D RID: 11789 RVA: 0x000E1A10 File Offset: 0x000DFE10
			public bool enabled
			{
				get
				{
					return this.interpolationType != FloatInterpolator.Config.InterpolationType.Direct;
				}
			}

			// Token: 0x170006EB RID: 1771
			// (get) Token: 0x06002E0E RID: 11790 RVA: 0x000E1A1E File Offset: 0x000DFE1E
			// (set) Token: 0x06002E0F RID: 11791 RVA: 0x000E1A26 File Offset: 0x000DFE26
			public float interpolationSpeed
			{
				get
				{
					return this._interpolationSpeed;
				}
				set
				{
					this._interpolationSpeed = value;
				}
			}

			// Token: 0x170006EC RID: 1772
			// (get) Token: 0x06002E10 RID: 11792 RVA: 0x000E1A2F File Offset: 0x000DFE2F
			public static FloatInterpolator.Config Direct
			{
				get
				{
					return new FloatInterpolator.Config(FloatInterpolator.Config.InterpolationType.Direct, 10f);
				}
			}

			// Token: 0x04001859 RID: 6233
			[SerializeField]
			private FloatInterpolator.Config.InterpolationType _interpolationType = FloatInterpolator.Config.InterpolationType.DampedSpring;

			// Token: 0x0400185A RID: 6234
			[SerializeField]
			[Range(0.1f, 50f)]
			private float _interpolationSpeed = 10f;

			// Token: 0x0200051F RID: 1311
			public enum InterpolationType
			{
				// Token: 0x0400185C RID: 6236
				Direct,
				// Token: 0x0400185D RID: 6237
				Exponential,
				// Token: 0x0400185E RID: 6238
				DampedSpring
			}
		}
	}
}
