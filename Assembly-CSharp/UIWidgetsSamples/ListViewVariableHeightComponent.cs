using System;
using UIWidgets;
using UnityEngine;
using UnityEngine.UI;

namespace UIWidgetsSamples
{
	// Token: 0x02000917 RID: 2327
	public class ListViewVariableHeightComponent : ListViewItem, IListViewItemHeight
	{
		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x060045F3 RID: 17907 RVA: 0x0017CCE6 File Offset: 0x0017B0E6
		public float Height
		{
			get
			{
				return this.CalculateHeight();
			}
		}

		// Token: 0x060045F4 RID: 17908 RVA: 0x0017CCEE File Offset: 0x0017B0EE
		public void SetData(ListViewVariableHeightItemDescription item)
		{
			this.Name.text = item.Name;
			this.Text.text = item.Text.Replace("\\n", "\n");
		}

		// Token: 0x060045F5 RID: 17909 RVA: 0x0017CD24 File Offset: 0x0017B124
		private float CalculateHeight()
		{
			float num = 63f;
			float num2 = 21f;
			float num3 = 17f;
			float num4 = num - num2 - num3;
			return num4 + this.Name.preferredHeight + this.Text.preferredHeight;
		}

		// Token: 0x04003004 RID: 12292
		[SerializeField]
		public Text Name;

		// Token: 0x04003005 RID: 12293
		[SerializeField]
		public Text Text;
	}
}
