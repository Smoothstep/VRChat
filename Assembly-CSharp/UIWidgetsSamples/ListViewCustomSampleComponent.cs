using System;
using UIWidgets;
using UnityEngine;
using UnityEngine.UI;

namespace UIWidgetsSamples
{
	// Token: 0x02000911 RID: 2321
	public class ListViewCustomSampleComponent : ListViewItem
	{
		// Token: 0x060045DD RID: 17885 RVA: 0x0017C424 File Offset: 0x0017A824
		public void SetData(ListViewCustomSampleItemDescription item)
		{
			if (item == null)
			{
				this.Icon.sprite = null;
				this.Text.text = string.Empty;
				this.Progressbar.Value = 0;
			}
			else
			{
				this.Icon.sprite = item.Icon;
				this.Text.text = item.Name;
				this.Progressbar.Value = item.Progress;
			}
			this.Icon.SetNativeSize();
			this.Icon.color = ((!(this.Icon.sprite == null)) ? Color.white : new Color(0f, 0f, 0f, 0f));
		}

		// Token: 0x04002FF6 RID: 12278
		[SerializeField]
		public Image Icon;

		// Token: 0x04002FF7 RID: 12279
		[SerializeField]
		public Text Text;

		// Token: 0x04002FF8 RID: 12280
		[SerializeField]
		public Progressbar Progressbar;
	}
}
