using System;
using UnityEngine;

namespace UIWidgets
{
	// Token: 0x0200095F RID: 2399
	[AddComponentMenu("UI/RangeSliderVertical", 300)]
	public class RangeSliderVertical : RangeSlider
	{
		// Token: 0x060048D2 RID: 18642 RVA: 0x00185AA9 File Offset: 0x00183EA9
		protected override bool IsHorizontal()
		{
			return false;
		}
	}
}
