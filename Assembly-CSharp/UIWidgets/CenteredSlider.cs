using System;
using UnityEngine;

namespace UIWidgets
{
	// Token: 0x02000929 RID: 2345
	[AddComponentMenu("UI/CenteredSlider", 300)]
	public class CenteredSlider : CenteredSliderBase<int>
	{
		// Token: 0x06004647 RID: 17991 RVA: 0x0017E7D8 File Offset: 0x0017CBD8
		protected override float ValueToPosition(int value)
		{
			value = this.InBounds(value);
			float num = (base.RangeSize() + base.HandleSize() / 2f) / 2f;
			if (value == 0)
			{
				return num + base.GetStartPoint();
			}
			if (value > 0)
			{
				float num2 = num / (float)this.limitMax;
				return num2 * (float)value + base.GetStartPoint() + num;
			}
			float num3 = num / (float)this.limitMin;
			return num3 * (float)(this.limitMin - value) + base.GetStartPoint();
		}

		// Token: 0x06004648 RID: 17992 RVA: 0x0017E854 File Offset: 0x0017CC54
		protected override int PositionToValue(float position)
		{
			float num = (base.RangeSize() + base.HandleSize() / 2f) / 2f;
			if (position == num)
			{
				return 0;
			}
			float num3;
			if (position > num)
			{
				float num2 = num / (float)this.limitMax;
				num3 = (position - num) / num2;
			}
			else
			{
				float num4 = num / (float)this.limitMin;
				num3 = -position / num4 + (float)base.LimitMin;
			}
			if (this.WholeNumberOfSteps)
			{
				return this.InBounds((int)Math.Round((double)(num3 / (float)this.step)) * this.step);
			}
			return this.InBounds((int)Math.Round((double)num3));
		}

		// Token: 0x06004649 RID: 17993 RVA: 0x0017E8F4 File Offset: 0x0017CCF4
		protected override Vector2 PositionLimits()
		{
			return new Vector2(this.ValueToPosition(base.LimitMin), this.ValueToPosition(base.LimitMax));
		}

		// Token: 0x0600464A RID: 17994 RVA: 0x0017E913 File Offset: 0x0017CD13
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

		// Token: 0x0600464B RID: 17995 RVA: 0x0017E93C File Offset: 0x0017CD3C
		protected override void Increase()
		{
			base.Value += this.step;
		}

		// Token: 0x0600464C RID: 17996 RVA: 0x0017E951 File Offset: 0x0017CD51
		protected override void Decrease()
		{
			base.Value -= this.step;
		}

		// Token: 0x0600464D RID: 17997 RVA: 0x0017E966 File Offset: 0x0017CD66
		protected override bool IsPositiveValue()
		{
			return base.Value > 0;
		}
	}
}
