using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIWidgets
{
	// Token: 0x02000954 RID: 2388
	public class OnDragKeepSelected : MonoBehaviour, IEndDragHandler, IEventSystemHandler
	{
		// Token: 0x0600485D RID: 18525 RVA: 0x001842FD File Offset: 0x001826FD
		public void OnEndDrag(PointerEventData eventData)
		{
			EventSystem.current.SetSelectedGameObject(eventData.selectedObject);
		}
	}
}
