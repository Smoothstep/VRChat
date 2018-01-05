using System;
using System.Collections;
using UnityEngine;

namespace Tacticsoft
{
	// Token: 0x020008DB RID: 2267
	[RequireComponent(typeof(TableView))]
	public class TableViewExtensions : MonoBehaviour
	{
		// Token: 0x060044F8 RID: 17656 RVA: 0x001710EB File Offset: 0x0016F4EB
		private void Awake()
		{
			this.mTableView = base.GetComponent<TableView>();
		}

		// Token: 0x060044F9 RID: 17657 RVA: 0x001710FC File Offset: 0x0016F4FC
		public void ScrollToRowAnimated(int row, float time = 2f, Action onScrolledToRow = null)
		{
			float scrollYForRow = this.mTableView.GetScrollYForRow(row, true);
			base.StartCoroutine(this.AnimateToScrollY(scrollYForRow, time, onScrolledToRow));
		}

		// Token: 0x060044FA RID: 17658 RVA: 0x00171127 File Offset: 0x0016F527
		public void ScrollToRowImmediate(int row)
		{
			this.mTableView.scrollY = this.mTableView.GetScrollYForRow(row, true);
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x00171141 File Offset: 0x0016F541
		public void ScrollToTopAnimated(Action onScrolledToRow = null)
		{
			base.StartCoroutine(this.AnimateToScrollY(0f, 2f, onScrolledToRow));
		}

		// Token: 0x060044FC RID: 17660 RVA: 0x0017115B File Offset: 0x0016F55B
		public void ScrollToBottomAnimated(Action onScrolledToRow = null)
		{
			base.StartCoroutine(this.AnimateToScrollY(this.mTableView.scrollableHeight, 2f, onScrolledToRow));
		}

		// Token: 0x060044FD RID: 17661 RVA: 0x0017117B File Offset: 0x0016F57B
		public void ScrollToTopImmediate()
		{
			this.mTableView.scrollY = 0f;
		}

		// Token: 0x060044FE RID: 17662 RVA: 0x0017118D File Offset: 0x0016F58D
		public void ScrollToBottomImmediate()
		{
			this.mTableView.scrollY = this.mTableView.scrollableHeight;
		}

		// Token: 0x060044FF RID: 17663 RVA: 0x001711A8 File Offset: 0x0016F5A8
		private IEnumerator AnimateToScrollY(float scrollY, float time, Action onScrolledToRow = null)
		{
			float startTime = Time.time;
			float startScrollY = this.mTableView.scrollY;
			float endTime = startTime + time;
			while (Time.time < endTime)
			{
				float relativeProgress = Mathf.InverseLerp(startTime, endTime, Time.time);
				this.mTableView.scrollY = Mathf.Lerp(startScrollY, scrollY, relativeProgress);
				yield return new WaitForEndOfFrame();
			}
			this.mTableView.scrollY = scrollY;
			if (onScrolledToRow != null)
			{
				onScrolledToRow();
			}
			yield break;
		}

		// Token: 0x04002EF2 RID: 12018
		private TableView mTableView;
	}
}
