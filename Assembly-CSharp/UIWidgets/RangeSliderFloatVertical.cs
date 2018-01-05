using System;
using UnityEngine;

namespace UIWidgets
{
	// Token: 0x0200095D RID: 2397
	[AddComponentMenu("UI/RangeSliderFloatVertical", 300)]
	public class RangeSliderFloatVertical : RangeSliderFloat
	{
		// Token: 0x060048C2 RID: 18626 RVA: 0x001855F1 File Offset: 0x001839F1
		protected override bool IsHorizontal()
		{
			return false;
		}
	}
}
