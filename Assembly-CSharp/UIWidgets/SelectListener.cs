using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIWidgets
{
	// Token: 0x02000962 RID: 2402
	public class SelectListener : MonoBehaviour, ISelectHandler, IDeselectHandler, IEventSystemHandler
	{
		// Token: 0x060048D8 RID: 18648 RVA: 0x00185B5A File Offset: 0x00183F5A
		public void OnSelect(BaseEventData eventData)
		{
			this.onSelect.Invoke(eventData);
		}

		// Token: 0x060048D9 RID: 18649 RVA: 0x00185B68 File Offset: 0x00183F68
		public void OnDeselect(BaseEventData eventData)
		{
			this.onDeselect.Invoke(eventData);
		}

		// Token: 0x0400316E RID: 12654
		public SelectEvent onSelect = new SelectEvent();

		// Token: 0x0400316F RID: 12655
		public SelectEvent onDeselect = new SelectEvent();
	}
}
