using System;
using UIWidgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UIWidgetsSamples
{
	// Token: 0x0200090D RID: 2317
	[RequireComponent(typeof(CenteredSlider))]
	public class CenteredSliderLabel : MonoBehaviour
	{
		// Token: 0x060045CB RID: 17867 RVA: 0x00179FB4 File Offset: 0x001783B4
		private void Start()
		{
			CenteredSlider component = base.GetComponent<CenteredSlider>();
			component.OnValuesChange.AddListener(new UnityAction<int>(this.ValueChanged));
			this.ValueChanged(component.Value);
		}

		// Token: 0x060045CC RID: 17868 RVA: 0x00179FEB File Offset: 0x001783EB
		private void ValueChanged(int value)
		{
			this.label.text = value.ToString();
		}

		// Token: 0x04002FED RID: 12269
		[SerializeField]
		private Text label;
	}
}
