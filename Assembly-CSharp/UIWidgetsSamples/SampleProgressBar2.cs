using System;
using UIWidgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UIWidgetsSamples
{
	// Token: 0x0200091C RID: 2332
	public class SampleProgressBar2 : MonoBehaviour
	{
		// Token: 0x06004603 RID: 17923 RVA: 0x0017CFA8 File Offset: 0x0017B3A8
		private void Start()
		{
			Button component = base.GetComponent<Button>();
			component.onClick.AddListener(new UnityAction(this.Click));
		}

		// Token: 0x06004604 RID: 17924 RVA: 0x0017CFD4 File Offset: 0x0017B3D4
		private void Click()
		{
			if (this.bar.IsAnimationRun)
			{
				this.bar.Stop();
			}
			else if (this.bar.Value == 0)
			{
				this.bar.Animate(new int?(this.bar.Max));
			}
			else
			{
				this.bar.Animate(new int?(0));
			}
		}

		// Token: 0x0400300A RID: 12298
		public Progressbar bar;
	}
}
