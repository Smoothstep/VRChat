using System;
using System.Collections;
using UIWidgets;
using UnityEngine;

namespace UIWidgetsSamples
{
	// Token: 0x0200091A RID: 2330
	[RequireComponent(typeof(Combobox))]
	public class SampleFilter : MonoBehaviour
	{
		// Token: 0x060045FD RID: 17917 RVA: 0x0017CE64 File Offset: 0x0017B264
		private void Start()
		{
			Combobox component = base.GetComponent<Combobox>();
			component.OnSelect.AddListener(delegate(int index, string item)
			{
				IEnumerator enumerator = this.Container.transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Transform transform = (Transform)obj;
						bool active = item == "All" || transform.gameObject.name.StartsWith(item, StringComparison.InvariantCulture);
						transform.gameObject.SetActive(active);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			});
		}

		// Token: 0x04003008 RID: 12296
		public GameObject Container;
	}
}
