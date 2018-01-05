using System;
using UIWidgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UIWidgetsSamples
{
	// Token: 0x0200091E RID: 2334
	[RequireComponent(typeof(Button))]
	public class TestListView : MonoBehaviour
	{
		// Token: 0x06004609 RID: 17929 RVA: 0x0017D0B0 File Offset: 0x0017B4B0
		private void Start()
		{
			Button component = base.GetComponent<Button>();
			component.onClick.AddListener(new UnityAction(this.Click));
		}

		// Token: 0x0600460A RID: 17930 RVA: 0x0017D0DC File Offset: 0x0017B4DC
		private void Click()
		{
			Debug.Log(this.listView.Add("Added from script"));
			Debug.Log(this.listView.Add("Added from script"));
			Debug.Log(this.listView.Add("Added from script"));
			this.listView.Remove("Caster");
		}

		// Token: 0x0400300C RID: 12300
		public ListView listView;
	}
}
