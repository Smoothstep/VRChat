using System;
using UIWidgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UIWidgetsSamples
{
	// Token: 0x02000918 RID: 2328
	[RequireComponent(typeof(RangeSliderFloat))]
	public class RangeSliderFloatSample : MonoBehaviour
	{
		// Token: 0x060045F7 RID: 17911 RVA: 0x0017CD6C File Offset: 0x0017B16C
		private void Start()
		{
			RangeSliderFloat component = base.GetComponent<RangeSliderFloat>();
			component.OnValuesChange.AddListener(new UnityAction<float, float>(this.SliderChanged));
			this.SliderChanged(component.ValueMin, component.ValueMax);
		}

		// Token: 0x060045F8 RID: 17912 RVA: 0x0017CDA9 File Offset: 0x0017B1A9
		private void SliderChanged(float min, float max)
		{
			if (this.Text != null)
			{
				this.Text.text = string.Format("Range: {0:000.00} - {1:000.00}", min, max);
			}
		}

		// Token: 0x04003006 RID: 12294
		[SerializeField]
		private Text Text;
	}
}
