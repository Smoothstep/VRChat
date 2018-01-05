using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIWidgets
{
	// Token: 0x0200096D RID: 2413
	[AddComponentMenu("UI/Tooltip", 300)]
	[RequireComponent(typeof(RectTransform))]
	public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IEventSystemHandler
	{
		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x0600493A RID: 18746 RVA: 0x001874DF File Offset: 0x001858DF
		// (set) Token: 0x0600493B RID: 18747 RVA: 0x001874E7 File Offset: 0x001858E7
		public GameObject TooltipObject
		{
			get
			{
				return this.tooltipObject;
			}
			set
			{
				this.tooltipObject = value;
				if (this.tooltipObject != null)
				{
					this.tooltipObjectParent = this.tooltipObject.transform.parent;
				}
			}
		}

		// Token: 0x0600493C RID: 18748 RVA: 0x00187517 File Offset: 0x00185917
		private void Start()
		{
			this.TooltipObject = this.tooltipObject;
			if (this.TooltipObject != null)
			{
				this.canvasTransform = Utilites.FindCanvas(this.tooltipObjectParent);
			}
			this.Hide();
		}

		// Token: 0x0600493D RID: 18749 RVA: 0x00187550 File Offset: 0x00185950
		private IEnumerator ShowCorutine()
		{
			yield return new WaitForSeconds(this.ShowDelay);
			if (this.canvasTransform != null)
			{
				this.tooltipObjectParent = this.tooltipObject.transform.parent;
				this.TooltipObject.transform.SetParent(this.canvasTransform);
			}
			this.TooltipObject.SetActive(true);
			yield break;
		}

		// Token: 0x0600493E RID: 18750 RVA: 0x0018756B File Offset: 0x0018596B
		public void Show()
		{
			if (this.TooltipObject == null)
			{
				return;
			}
			this.currentCorutine = this.ShowCorutine();
			base.StartCoroutine(this.currentCorutine);
		}

		// Token: 0x0600493F RID: 18751 RVA: 0x00187598 File Offset: 0x00185998
		private IEnumerator HideCoroutine()
		{
			if (this.currentCorutine != null)
			{
				base.StopCoroutine(this.currentCorutine);
			}
			if (this.TooltipObject != null)
			{
				this.TooltipObject.SetActive(false);
				yield return null;
				if (this.canvasTransform != null)
				{
					this.TooltipObject.transform.SetParent(this.tooltipObjectParent);
				}
			}
			yield break;
		}

		// Token: 0x06004940 RID: 18752 RVA: 0x001875B3 File Offset: 0x001859B3
		public void Hide()
		{
			base.StartCoroutine(this.HideCoroutine());
		}

		// Token: 0x06004941 RID: 18753 RVA: 0x001875C2 File Offset: 0x001859C2
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.Show();
		}

		// Token: 0x06004942 RID: 18754 RVA: 0x001875CA File Offset: 0x001859CA
		public void OnPointerExit(PointerEventData eventData)
		{
			this.Hide();
		}

		// Token: 0x06004943 RID: 18755 RVA: 0x001875D2 File Offset: 0x001859D2
		public void OnSelect(BaseEventData eventData)
		{
			this.Show();
		}

		// Token: 0x06004944 RID: 18756 RVA: 0x001875DA File Offset: 0x001859DA
		public void OnDeselect(BaseEventData eventData)
		{
			this.Hide();
		}

		// Token: 0x0400319F RID: 12703
		[SerializeField]
		private GameObject tooltipObject;

		// Token: 0x040031A0 RID: 12704
		[SerializeField]
		public float ShowDelay = 0.3f;

		// Token: 0x040031A1 RID: 12705
		private Transform canvasTransform;

		// Token: 0x040031A2 RID: 12706
		private Transform tooltipObjectParent;

		// Token: 0x040031A3 RID: 12707
		private IEnumerator currentCorutine;
	}
}
