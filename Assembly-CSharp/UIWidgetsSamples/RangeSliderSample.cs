using System;
using UIWidgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UIWidgetsSamples
{
	// Token: 0x02000919 RID: 2329
	[RequireComponent(typeof(RangeSlider))]
	public class RangeSliderSample : MonoBehaviour
	{
		// Token: 0x060045FA RID: 17914 RVA: 0x0017CDE8 File Offset: 0x0017B1E8
		private void Start()
		{
			RangeSlider component = base.GetComponent<RangeSlider>();
			component.OnValuesChange.AddListener(new UnityAction<int, int>(this.SliderChanged));
			this.SliderChanged(component.ValueMin, component.ValueMax);
		}

		// Token: 0x060045FB RID: 17915 RVA: 0x0017CE25 File Offset: 0x0017B225
		private void SliderChanged(int min, int max)
		{
			if (this.Text != null)
			{
				this.Text.text = string.Format("Range: {0:000} - {1:000}", min, max);
			}
		}

		// Token: 0x04003007 RID: 12295
		[SerializeField]
		private Text Text;
	}
}
