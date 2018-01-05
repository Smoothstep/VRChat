using System;
using UIWidgets;
using UnityEngine;
using UnityEngine.UI;

namespace UIWidgetsSamples
{
	// Token: 0x02000914 RID: 2324
	public class ListViewUnderlineSampleComponent : ListViewItem
	{
		// Token: 0x060045E9 RID: 17897 RVA: 0x0017C5EC File Offset: 0x0017A9EC
		public void SetData(ListViewUnderlineSampleItemDescription item)
		{
			if (item == null)
			{
				this.Icon.sprite = null;
				this.Text.text = string.Empty;
			}
			else
			{
				this.Icon.sprite = item.Icon;
				this.Text.text = item.Name;
			}
			this.Icon.SetNativeSize();
			this.Icon.color = ((!(this.Icon.sprite == null)) ? Color.white : new Color(0f, 0f, 0f, 0f));
		}

		// Token: 0x04002FFE RID: 12286
		[SerializeField]
		public Image Icon;

		// Token: 0x04002FFF RID: 12287
		[SerializeField]
		public Text Text;

		// Token: 0x04003000 RID: 12288
		[SerializeField]
		public Image Underline;
	}
}
