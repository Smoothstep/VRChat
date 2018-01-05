using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIWidgets
{
	// Token: 0x0200094F RID: 2383
	[RequireComponent(typeof(Image))]
	public class ListViewItem : UIBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler, ICancelHandler, ISelectHandler, IDeselectHandler, IMoveHandler, IEventSystemHandler
	{
		// Token: 0x06004828 RID: 18472 RVA: 0x0017C33E File Offset: 0x0017A73E
		protected override void Awake()
		{
			this.Background = base.GetComponent<ImageAdvanced>();
		}

		// Token: 0x06004829 RID: 18473 RVA: 0x0017C34C File Offset: 0x0017A74C
		public void OnMove(AxisEventData eventData)
		{
			this.onMove.Invoke(eventData, this);
		}

		// Token: 0x0600482A RID: 18474 RVA: 0x0017C35B File Offset: 0x0017A75B
		public void OnSubmit(BaseEventData eventData)
		{
			this.onSubmit.Invoke(this);
		}

		// Token: 0x0600482B RID: 18475 RVA: 0x0017C369 File Offset: 0x0017A769
		public void OnCancel(BaseEventData eventData)
		{
			this.onCancel.Invoke(this);
		}

		// Token: 0x0600482C RID: 18476 RVA: 0x0017C377 File Offset: 0x0017A777
		public void OnSelect(BaseEventData eventData)
		{
			this.Select();
			this.onSelect.Invoke(this);
		}

		// Token: 0x0600482D RID: 18477 RVA: 0x0017C38B File Offset: 0x0017A78B
		public void OnDeselect(BaseEventData eventData)
		{
			this.onDeselect.Invoke(this);
		}

		// Token: 0x0600482E RID: 18478 RVA: 0x0017C399 File Offset: 0x0017A799
		public void OnPointerClick(PointerEventData eventData)
		{
			this.onPointerClick.Invoke(eventData);
			this.onClick.Invoke();
			this.Select();
		}

		// Token: 0x0600482F RID: 18479 RVA: 0x0017C3B8 File Offset: 0x0017A7B8
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.onPointerEnter.Invoke(eventData);
		}

		// Token: 0x06004830 RID: 18480 RVA: 0x0017C3C6 File Offset: 0x0017A7C6
		public void OnPointerExit(PointerEventData eventData)
		{
			this.onPointerExit.Invoke(eventData);
		}

		// Token: 0x06004831 RID: 18481 RVA: 0x0017C3D4 File Offset: 0x0017A7D4
		public virtual void Select()
		{
			if (EventSystem.current.alreadySelecting)
			{
				return;
			}
			ListViewItemEventData listViewItemEventData = new ListViewItemEventData(EventSystem.current)
			{
				NewSelectedObject = base.gameObject
			};
			EventSystem.current.SetSelectedGameObject(listViewItemEventData.NewSelectedObject, listViewItemEventData);
		}

		// Token: 0x04003111 RID: 12561
		[HideInInspector]
		public int Index;

		// Token: 0x04003112 RID: 12562
		public UnityEvent onClick = new UnityEvent();

		// Token: 0x04003113 RID: 12563
		public ListViewItemSelect onSubmit = new ListViewItemSelect();

		// Token: 0x04003114 RID: 12564
		public ListViewItemSelect onCancel = new ListViewItemSelect();

		// Token: 0x04003115 RID: 12565
		public ListViewItemSelect onSelect = new ListViewItemSelect();

		// Token: 0x04003116 RID: 12566
		public ListViewItemSelect onDeselect = new ListViewItemSelect();

		// Token: 0x04003117 RID: 12567
		public ListViewItemMove onMove = new ListViewItemMove();

		// Token: 0x04003118 RID: 12568
		public PointerUnityEvent onPointerClick = new PointerUnityEvent();

		// Token: 0x04003119 RID: 12569
		public PointerUnityEvent onPointerEnter = new PointerUnityEvent();

		// Token: 0x0400311A RID: 12570
		public PointerUnityEvent onPointerExit = new PointerUnityEvent();

		// Token: 0x0400311B RID: 12571
		[NonSerialized]
		public ImageAdvanced Background;
	}
}
