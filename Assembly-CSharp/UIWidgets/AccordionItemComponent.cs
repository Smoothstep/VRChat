using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UIWidgets
{
	// Token: 0x02000924 RID: 2340
	public class AccordionItemComponent : MonoBehaviour, IPointerClickHandler, ISubmitHandler, IEventSystemHandler
	{
		// Token: 0x06004637 RID: 17975 RVA: 0x0017DCD6 File Offset: 0x0017C0D6
		public void OnPointerClick(PointerEventData eventData)
		{
			this.OnClick.Invoke();
		}

		// Token: 0x06004638 RID: 17976 RVA: 0x0017DCE3 File Offset: 0x0017C0E3
		public void OnSubmit(BaseEventData eventData)
		{
			this.OnClick.Invoke();
		}

		// Token: 0x04003026 RID: 12326
		public UnityEvent OnClick = new UnityEvent();
	}
}
