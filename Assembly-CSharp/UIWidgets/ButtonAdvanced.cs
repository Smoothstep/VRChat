using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIWidgets
{
	// Token: 0x02000928 RID: 2344
	[AddComponentMenu("UI/Progressbar", 210)]
	public class ButtonAdvanced : Button, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
	{
		// Token: 0x06004642 RID: 17986 RVA: 0x0017E1D1 File Offset: 0x0017C5D1
		public override void OnPointerDown(PointerEventData eventData)
		{
			this.onPointerDown.Invoke(eventData);
			base.OnPointerDown(eventData);
		}

		// Token: 0x06004643 RID: 17987 RVA: 0x0017E1E6 File Offset: 0x0017C5E6
		public override void OnPointerUp(PointerEventData eventData)
		{
			this.onPointerUp.Invoke(eventData);
			base.OnPointerUp(eventData);
		}

		// Token: 0x06004644 RID: 17988 RVA: 0x0017E1FB File Offset: 0x0017C5FB
		public override void OnPointerEnter(PointerEventData eventData)
		{
			this.onPointerEnter.Invoke(eventData);
			base.OnPointerEnter(eventData);
		}

		// Token: 0x06004645 RID: 17989 RVA: 0x0017E210 File Offset: 0x0017C610
		public override void OnPointerExit(PointerEventData eventData)
		{
			this.onPointerExit.Invoke(eventData);
			base.OnPointerExit(eventData);
		}

		// Token: 0x04003028 RID: 12328
		public PointerUnityEvent onPointerDown = new PointerUnityEvent();

		// Token: 0x04003029 RID: 12329
		public PointerUnityEvent onPointerUp = new PointerUnityEvent();

		// Token: 0x0400302A RID: 12330
		public PointerUnityEvent onPointerEnter = new PointerUnityEvent();

		// Token: 0x0400302B RID: 12331
		public PointerUnityEvent onPointerExit = new PointerUnityEvent();
	}
}
