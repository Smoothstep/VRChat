using System;
using UnityEngine;

namespace UIWidgets
{
	// Token: 0x0200092C RID: 2348
	[AddComponentMenu("UI/CenteredSliderVertical", 300)]
	public class CenteredSliderVertical : CenteredSlider
	{
		// Token: 0x06004670 RID: 18032 RVA: 0x0017E979 File Offset: 0x0017CD79
		protected override bool IsHorizontal()
		{
			return false;
		}
	}
}
