using System;
using UnityEngine;

namespace UIWidgets
{
	// Token: 0x02000930 RID: 2352
	[AddComponentMenu("UI/ComboboxIcons", 230)]
	public class ComboboxIcons : ComboboxCustom<ListViewIcons, ListViewIconsItemComponent, ListViewIconsItemDescription>
	{
		// Token: 0x060046A6 RID: 18086 RVA: 0x0017F9F1 File Offset: 0x0017DDF1
		private void Awake()
		{
			this.Start();
		}

		// Token: 0x060046A7 RID: 18087 RVA: 0x0017F9F9 File Offset: 0x0017DDF9
		public override void Start()
		{
			if (this.is_started)
			{
				return;
			}
			this.is_started = true;
			base.Start();
		}

		// Token: 0x060046A8 RID: 18088 RVA: 0x0017FA14 File Offset: 0x0017DE14
		protected override void UpdateCurrent()
		{
			base.Current.SetData(base.ListView.SelectedItem);
			base.HideList();
		}

		// Token: 0x0400304B RID: 12363
		[NonSerialized]
		private bool is_started;
	}
}
