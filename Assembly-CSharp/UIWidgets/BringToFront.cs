using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIWidgets
{
	// Token: 0x02000926 RID: 2342
	public class BringToFront : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x0600463D RID: 17981 RVA: 0x0017E14F File Offset: 0x0017C54F
		public void OnPointerDown(PointerEventData eventData)
		{
			this.ToFront();
		}

		// Token: 0x0600463E RID: 17982 RVA: 0x0017E157 File Offset: 0x0017C557
		public void ToFront()
		{
			this.ToFront(base.transform);
		}

		// Token: 0x0600463F RID: 17983 RVA: 0x0017E165 File Offset: 0x0017C565
		private void ToFront(Transform obj)
		{
			obj.SetAsLastSibling();
			if (this.WithParents && obj.parent != null)
			{
				this.ToFront(obj.parent);
			}
		}

		// Token: 0x04003027 RID: 12327
		[SerializeField]
		public bool WithParents;
	}
}
