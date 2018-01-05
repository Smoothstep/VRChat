using System;
using UIWidgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UIWidgetsSamples
{
	// Token: 0x0200091B RID: 2331
	public class SampleProgressBar1 : MonoBehaviour
	{
		// Token: 0x06004600 RID: 17920 RVA: 0x0017CF30 File Offset: 0x0017B330
		private void Start()
		{
			Button component = base.GetComponent<Button>();
			component.onClick.AddListener(new UnityAction(this.Click));
		}

		// Token: 0x06004601 RID: 17921 RVA: 0x0017CF5C File Offset: 0x0017B35C
		private void Click()
		{
			if (this.bar.IsAnimationRun)
			{
				this.bar.Stop();
			}
			else
			{
				this.bar.Animate(null);
			}
		}

		// Token: 0x04003009 RID: 12297
		public Progressbar bar;
	}
}
