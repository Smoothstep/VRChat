using System;
using System.Collections;
using UnityEngine;

namespace Tacticsoft.Examples
{
	// Token: 0x020008D9 RID: 2265
	public class ScrollingEventsHandler : MonoBehaviour
	{
		// Token: 0x060044EC RID: 17644 RVA: 0x00170E37 File Offset: 0x0016F237
		public void ScrollToTopAnimated()
		{
			base.StartCoroutine(this.AnimateToScrollY(0f, 2f));
		}

		// Token: 0x060044ED RID: 17645 RVA: 0x00170E50 File Offset: 0x0016F250
		public void ScrollToBottomImmediate()
		{
			this.m_tableView.scrollY = this.m_tableView.scrollableHeight;
		}

		// Token: 0x060044EE RID: 17646 RVA: 0x00170E68 File Offset: 0x0016F268
		public void ScrollToRow10Animated()
		{
			float scrollYForRow = this.m_tableView.GetScrollYForRow(10, true);
			base.StartCoroutine(this.AnimateToScrollY(scrollYForRow, 2f));
		}

		// Token: 0x060044EF RID: 17647 RVA: 0x00170E98 File Offset: 0x0016F298
		private IEnumerator AnimateToScrollY(float scrollY, float time)
		{
			float startTime = Time.time;
			float startScrollY = this.m_tableView.scrollY;
			float endTime = startTime + time;
			while (Time.time < endTime)
			{
				float relativeProgress = Mathf.InverseLerp(startTime, endTime, Time.time);
				this.m_tableView.scrollY = Mathf.Lerp(startScrollY, scrollY, relativeProgress);
				yield return new WaitForEndOfFrame();
			}
			this.m_tableView.scrollY = scrollY;
			yield break;
		}

		// Token: 0x04002EED RID: 12013
		public TableView m_tableView;
	}
}
