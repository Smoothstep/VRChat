using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExitGames.UtilityScripts
{
	// Token: 0x020007A7 RID: 1959
	public class ButtonInsideScrollList : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x06003F4E RID: 16206 RVA: 0x0013EA3F File Offset: 0x0013CE3F
		private void Start()
		{
			this.scrollRect = base.GetComponentInParent<ScrollRect>();
		}

		// Token: 0x06003F4F RID: 16207 RVA: 0x0013EA4D File Offset: 0x0013CE4D
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (this.scrollRect != null)
			{
				this.scrollRect.StopMovement();
				this.scrollRect.enabled = false;
			}
		}

		// Token: 0x06003F50 RID: 16208 RVA: 0x0013EA77 File Offset: 0x0013CE77
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (this.scrollRect != null && !this.scrollRect.enabled)
			{
				this.scrollRect.enabled = true;
			}
		}

		// Token: 0x040027A4 RID: 10148
		private ScrollRect scrollRect;
	}
}
