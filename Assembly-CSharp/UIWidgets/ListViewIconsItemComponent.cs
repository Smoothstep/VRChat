using System;
using UnityEngine;
using UnityEngine.UI;

namespace UIWidgets
{
	// Token: 0x0200094C RID: 2380
	public class ListViewIconsItemComponent : ListViewItem
	{
		// Token: 0x06004824 RID: 18468 RVA: 0x0018377C File Offset: 0x00181B7C
		public void SetData(ListViewIconsItemDescription item)
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

		// Token: 0x0400310F RID: 12559
		[SerializeField]
		public Image Icon;

		// Token: 0x04003110 RID: 12560
		[SerializeField]
		public Text Text;
	}
}
