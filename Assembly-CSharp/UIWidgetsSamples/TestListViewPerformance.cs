using System;
using System.Collections.Generic;
using System.Linq;
using UIWidgets;
using UnityEngine;

namespace UIWidgetsSamples
{
	// Token: 0x0200091F RID: 2335
	public class TestListViewPerformance : MonoBehaviour
	{
		// Token: 0x0600460C RID: 17932 RVA: 0x0017D150 File Offset: 0x0017B550
		private void TestN(int n)
		{
			this.lv.Strings = (from x in Enumerable.Range(1, n)
			select x.ToString("00000")).ToList<string>();
		}

		// Token: 0x0600460D RID: 17933 RVA: 0x0017D18B File Offset: 0x0017B58B
		public void Test2()
		{
			this.TestN(2);
		}

		// Token: 0x0600460E RID: 17934 RVA: 0x0017D194 File Offset: 0x0017B594
		public void Test5()
		{
			this.TestN(5);
		}

		// Token: 0x0600460F RID: 17935 RVA: 0x0017D19D File Offset: 0x0017B59D
		public void Test10()
		{
			this.TestN(10);
		}

		// Token: 0x06004610 RID: 17936 RVA: 0x0017D1A7 File Offset: 0x0017B5A7
		public void Test100()
		{
			this.TestN(100);
		}

		// Token: 0x06004611 RID: 17937 RVA: 0x0017D1B1 File Offset: 0x0017B5B1
		public void Test1000()
		{
			this.TestN(1000);
		}

		// Token: 0x06004612 RID: 17938 RVA: 0x0017D1BE File Offset: 0x0017B5BE
		public void Test10000()
		{
			this.TestN(10000);
		}

		// Token: 0x06004613 RID: 17939 RVA: 0x0017D1CC File Offset: 0x0017B5CC
		public void TestiN(int n)
		{
			List<ListViewIconsItemDescription> items = (from x in Enumerable.Range(1, n)
			select x.ToString("00000") into x
			select new ListViewIconsItemDescription
			{
				Name = x
			}).ToList<ListViewIconsItemDescription>();
			this.lvi.Items = items;
		}

		// Token: 0x06004614 RID: 17940 RVA: 0x0017D236 File Offset: 0x0017B636
		public void Testi2()
		{
			this.TestiN(2);
		}

		// Token: 0x06004615 RID: 17941 RVA: 0x0017D23F File Offset: 0x0017B63F
		public void Testi5()
		{
			this.TestiN(5);
		}

		// Token: 0x06004616 RID: 17942 RVA: 0x0017D248 File Offset: 0x0017B648
		public void Testi1000()
		{
			this.lvi.SortFunc = null;
			this.TestiN(1000);
		}

		// Token: 0x06004617 RID: 17943 RVA: 0x0017D261 File Offset: 0x0017B661
		public void Testi10000()
		{
			this.lvi.SortFunc = null;
			this.TestiN(10000);
		}

		// Token: 0x0400300D RID: 12301
		[SerializeField]
		private ListView lv;

		// Token: 0x0400300E RID: 12302
		[SerializeField]
		private ListViewIcons lvi;
	}
}
