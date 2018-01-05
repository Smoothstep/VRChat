using System;
using UnityEngine;

namespace PhysSound
{
	// Token: 0x020007C3 RID: 1987
	[Serializable]
	public struct Range
	{
		// Token: 0x06004014 RID: 16404 RVA: 0x00142283 File Offset: 0x00140683
		public Range(float min, float max)
		{
			this.Min = min;
			this.Max = max;
		}

		// Token: 0x06004015 RID: 16405 RVA: 0x00142293 File Offset: 0x00140693
		public bool isWithinRange(float f)
		{
			return f >= this.Min && f <= this.Max;
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x001422B0 File Offset: 0x001406B0
		public float Clamp(float f)
		{
			return Mathf.Clamp(f, this.Min, this.Max);
		}

		// Token: 0x06004017 RID: 16407 RVA: 0x001422C4 File Offset: 0x001406C4
		public float RandomInRange()
		{
			return UnityEngine.Random.Range(this.Min, this.Max);
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x001422D7 File Offset: 0x001406D7
		public int RandomInRangeInteger()
		{
			return (int)UnityEngine.Random.Range(this.Min, this.Max + 1f);
		}

		// Token: 0x06004019 RID: 16409 RVA: 0x001422F1 File Offset: 0x001406F1
		public float Lerp(float t)
		{
			return this.Min + (this.Max - this.Min) * t;
		}

		// Token: 0x0600401A RID: 16410 RVA: 0x00142309 File Offset: 0x00140709
		public float Normalize(float val)
		{
			if (val <= this.Min)
			{
				return 0f;
			}
			if (val >= this.Max)
			{
				return 1f;
			}
			return (val - this.Min) / (this.Max - this.Min);
		}

		// Token: 0x04002867 RID: 10343
		public float Min;

		// Token: 0x04002868 RID: 10344
		public float Max;
	}
}
