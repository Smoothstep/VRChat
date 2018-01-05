using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UIWidgets
{
	// Token: 0x02000960 RID: 2400
	public class ResizeListener : UIBehaviour
	{
		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x060048D4 RID: 18644 RVA: 0x00185ABF File Offset: 0x00183EBF
		public RectTransform RectTransform
		{
			get
			{
				if (this.rectTransform == null)
				{
					this.rectTransform = base.GetComponent<RectTransform>();
				}
				return this.rectTransform;
			}
		}

		// Token: 0x060048D5 RID: 18645 RVA: 0x00185AE4 File Offset: 0x00183EE4
		protected override void OnRectTransformDimensionsChange()
		{
			if (this.old_rect.Equals(this.RectTransform.rect))
			{
				return;
			}
			this.old_rect = this.RectTransform.rect;
			this.OnResize.Invoke();
		}

		// Token: 0x0400316B RID: 12651
		private RectTransform rectTransform;

		// Token: 0x0400316C RID: 12652
		public UnityEvent OnResize = new UnityEvent();

		// Token: 0x0400316D RID: 12653
		private Rect old_rect;
	}
}
