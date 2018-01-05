using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIWidgets
{
	// Token: 0x0200093E RID: 2366
	[AddComponentMenu("UI/ImageAdvanced", 240)]
	public class ImageAdvanced : Image, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
	{
		// Token: 0x06004725 RID: 18213 RVA: 0x00181721 File Offset: 0x0017FB21
		public virtual void OnPointerDown(PointerEventData eventData)
		{
			this.onPointerDown.Invoke(eventData);
		}

		// Token: 0x06004726 RID: 18214 RVA: 0x0018172F File Offset: 0x0017FB2F
		public virtual void OnPointerUp(PointerEventData eventData)
		{
			this.onPointerUp.Invoke(eventData);
		}

		// Token: 0x06004727 RID: 18215 RVA: 0x0018173D File Offset: 0x0017FB3D
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			this.onPointerEnter.Invoke(eventData);
		}

		// Token: 0x06004728 RID: 18216 RVA: 0x0018174B File Offset: 0x0017FB4B
		public virtual void OnPointerExit(PointerEventData eventData)
		{
			this.onPointerExit.Invoke(eventData);
		}

		// Token: 0x0400309E RID: 12446
		public PointerUnityEvent onPointerDown = new PointerUnityEvent();

		// Token: 0x0400309F RID: 12447
		public PointerUnityEvent onPointerUp = new PointerUnityEvent();

		// Token: 0x040030A0 RID: 12448
		public PointerUnityEvent onPointerEnter = new PointerUnityEvent();

		// Token: 0x040030A1 RID: 12449
		public PointerUnityEvent onPointerExit = new PointerUnityEvent();
	}
}
