using System;
using UnityEngine;

namespace UIWidgets
{
	// Token: 0x0200095C RID: 2396
	[AddComponentMenu("UI/RangeSliderFloat", 310)]
	public class RangeSliderFloat : RangeSliderBase<float>
	{
		// Token: 0x060048B6 RID: 18614 RVA: 0x00185438 File Offset: 0x00183838
		protected override float ValueToPosition(float value)
		{
			float num = base.FillSize() / (this.limitMax - this.limitMin);
			return num * (this.InBounds(value) - this.limitMin) + base.GetStartPoint();
		}

		// Token: 0x060048B7 RID: 18615 RVA: 0x00185474 File Offset: 0x00183874
		protected override float PositionToValue(float position)
		{
			float num = base.FillSize() / (this.limitMax - this.limitMin);
			float num2 = position / num + base.LimitMin;
			if (this.WholeNumberOfSteps)
			{
				return this.InBounds(Mathf.Round(num2 / this.step) * this.step);
			}
			return this.InBounds(num2);
		}

		// Token: 0x060048B8 RID: 18616 RVA: 0x001854CE File Offset: 0x001838CE
		protected override Vector2 MinPositionLimits()
		{
			return new Vector2(this.ValueToPosition(base.LimitMin), this.ValueToPosition(this.valueMax - this.step));
		}

		// Token: 0x060048B9 RID: 18617 RVA: 0x001854F4 File Offset: 0x001838F4
		protected override Vector2 MaxPositionLimits()
		{
			return new Vector2(this.ValueToPosition(this.valueMin + this.step), this.ValueToPosition(this.limitMax));
		}

		// Token: 0x060048BA RID: 18618 RVA: 0x0018551A File Offset: 0x0018391A
		protected override float InBounds(float value)
		{
			if (value < this.limitMin)
			{
				return this.limitMin;
			}
			if (value > this.limitMax)
			{
				return this.limitMax;
			}
			return value;
		}

		// Token: 0x060048BB RID: 18619 RVA: 0x00185543 File Offset: 0x00183943
		protected override float InBoundsMin(float value)
		{
			if (value < this.limitMin)
			{
				return this.limitMin;
			}
			if (value > this.valueMax)
			{
				return this.valueMax;
			}
			return value;
		}

		// Token: 0x060048BC RID: 18620 RVA: 0x0018556C File Offset: 0x0018396C
		protected override float InBoundsMax(float value)
		{
			if (value < this.valueMin)
			{
				return this.valueMin;
			}
			if (value > this.limitMax)
			{
				return this.limitMax;
			}
			return value;
		}

		// Token: 0x060048BD RID: 18621 RVA: 0x00185595 File Offset: 0x00183995
		protected override void IncreaseMin()
		{
			base.ValueMin += this.step;
		}

		// Token: 0x060048BE RID: 18622 RVA: 0x001855AA File Offset: 0x001839AA
		protected override void DecreaseMin()
		{
			base.ValueMin -= this.step;
		}

		// Token: 0x060048BF RID: 18623 RVA: 0x001855BF File Offset: 0x001839BF
		protected override void IncreaseMax()
		{
			base.ValueMax += this.step;
		}

		// Token: 0x060048C0 RID: 18624 RVA: 0x001855D4 File Offset: 0x001839D4
		protected override void DecreaseMax()
		{
			base.ValueMax -= this.step;
		}
	}
}
