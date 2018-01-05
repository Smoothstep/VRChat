using System;
using UnityEngine;

namespace UIWidgets
{
	// Token: 0x02000959 RID: 2393
	[AddComponentMenu("UI/RangeSlider", 300)]
	public class RangeSlider : RangeSliderBase<int>
	{
		// Token: 0x06004879 RID: 18553 RVA: 0x00185260 File Offset: 0x00183660
		protected override float ValueToPosition(int value)
		{
			float num = base.FillSize() / (float)(this.limitMax - this.limitMin);
			return num * (float)(this.InBounds(value) - this.limitMin) + base.GetStartPoint();
		}

		// Token: 0x0600487A RID: 18554 RVA: 0x0018529C File Offset: 0x0018369C
		protected override int PositionToValue(float position)
		{
			float num = base.FillSize() / (float)(this.limitMax - this.limitMin);
			float num2 = position / num + (float)base.LimitMin;
			if (this.WholeNumberOfSteps)
			{
				return this.InBounds((int)Math.Round((double)(num2 / (float)this.step)) * this.step);
			}
			return this.InBounds((int)Math.Round((double)num2));
		}

		// Token: 0x0600487B RID: 18555 RVA: 0x00185302 File Offset: 0x00183702
		protected override Vector2 MinPositionLimits()
		{
			return new Vector2(this.ValueToPosition(base.LimitMin), this.ValueToPosition(this.valueMax - this.step));
		}

		// Token: 0x0600487C RID: 18556 RVA: 0x00185328 File Offset: 0x00183728
		protected override Vector2 MaxPositionLimits()
		{
			return new Vector2(this.ValueToPosition(this.valueMin + this.step), this.ValueToPosition(this.limitMax));
		}

		// Token: 0x0600487D RID: 18557 RVA: 0x0018534E File Offset: 0x0018374E
		protected override int InBounds(int value)
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

		// Token: 0x0600487E RID: 18558 RVA: 0x00185377 File Offset: 0x00183777
		protected override int InBoundsMin(int value)
		{
			if (value < this.limitMin)
			{
				return this.limitMin;
			}
			if (value > this.valueMax - this.step)
			{
				return this.valueMax - this.step;
			}
			return value;
		}

		// Token: 0x0600487F RID: 18559 RVA: 0x001853AE File Offset: 0x001837AE
		protected override int InBoundsMax(int value)
		{
			if (value < value + this.step)
			{
				return value + this.step;
			}
			if (value > this.limitMax)
			{
				return this.limitMax;
			}
			return value;
		}

		// Token: 0x06004880 RID: 18560 RVA: 0x001853DB File Offset: 0x001837DB
		protected override void IncreaseMin()
		{
			base.ValueMin += this.step;
		}

		// Token: 0x06004881 RID: 18561 RVA: 0x001853F0 File Offset: 0x001837F0
		protected override void DecreaseMin()
		{
			base.ValueMin -= this.step;
		}

		// Token: 0x06004882 RID: 18562 RVA: 0x00185405 File Offset: 0x00183805
		protected override void IncreaseMax()
		{
			base.ValueMax += this.step;
		}

		// Token: 0x06004883 RID: 18563 RVA: 0x0018541A File Offset: 0x0018381A
		protected override void DecreaseMax()
		{
			base.ValueMax -= this.step;
		}
	}
}
