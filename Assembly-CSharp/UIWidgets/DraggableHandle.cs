using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIWidgets
{
	// Token: 0x02000934 RID: 2356
	public class DraggableHandle : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler, IEventSystemHandler
	{
		// Token: 0x060046DD RID: 18141 RVA: 0x00180388 File Offset: 0x0017E788
		public void Drag(RectTransform newDrag)
		{
			this.drag = newDrag;
		}

		// Token: 0x060046DE RID: 18142 RVA: 0x00180391 File Offset: 0x0017E791
		public void OnInitializePotentialDrag(PointerEventData eventData)
		{
			this.canvasRect = (Utilites.FindCanvas(base.transform) as RectTransform);
			this.canvas = this.canvasRect.GetComponent<Canvas>();
		}

		// Token: 0x060046DF RID: 18143 RVA: 0x001803BC File Offset: 0x0017E7BC
		public void OnDrag(PointerEventData eventData)
		{
			if (this.canvas == null)
			{
				throw new MissingComponentException(base.gameObject.name + " not in Canvas hierarchy.");
			}
			Vector3 b = Utilites.CalculateDragPosition(eventData.position, this.canvas, this.canvasRect) - Utilites.CalculateDragPosition(eventData.position - eventData.delta, this.canvas, this.canvasRect);
			this.drag.localPosition += b;
		}

		// Token: 0x0400305E RID: 12382
		private RectTransform drag;

		// Token: 0x0400305F RID: 12383
		private Canvas canvas;

		// Token: 0x04003060 RID: 12384
		private RectTransform canvasRect;
	}
}
