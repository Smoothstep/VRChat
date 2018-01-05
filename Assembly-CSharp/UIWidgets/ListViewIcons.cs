using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UIWidgets
{
	// Token: 0x0200094B RID: 2379
	[AddComponentMenu("UI/ListViewIcons", 252)]
	public class ListViewIcons : ListViewCustom<ListViewIconsItemComponent, ListViewIconsItemDescription>
	{
		// Token: 0x0600481B RID: 18459 RVA: 0x001836A5 File Offset: 0x00181AA5
		protected override void Awake()
		{
			this.Start();
		}

		// Token: 0x0600481C RID: 18460 RVA: 0x001836AD File Offset: 0x00181AAD
		public override void Start()
		{
			if (this.isStartedListViewIcons)
			{
				return;
			}
			this.isStartedListViewIcons = true;
			base.SortFunc = ((List<ListViewIconsItemDescription> x) => (from y in x
			orderby y.Name
			select y).ToList<ListViewIconsItemDescription>());
			base.Start();
		}

		// Token: 0x0600481D RID: 18461 RVA: 0x001836EB File Offset: 0x00181AEB
		protected override void SetData(ListViewIconsItemComponent component, ListViewIconsItemDescription item)
		{
			component.SetData(item);
		}

		// Token: 0x0600481E RID: 18462 RVA: 0x001836F4 File Offset: 0x00181AF4
		protected override void HighlightColoring(ListViewIconsItemComponent component)
		{
			base.HighlightColoring(component);
			component.Text.color = this.HighlightedColor;
		}

		// Token: 0x0600481F RID: 18463 RVA: 0x0018370E File Offset: 0x00181B0E
		protected override void SelectColoring(ListViewIconsItemComponent component)
		{
			base.SelectColoring(component);
			component.Text.color = base.SelectedColor;
		}

		// Token: 0x06004820 RID: 18464 RVA: 0x00183728 File Offset: 0x00181B28
		protected override void DefaultColoring(ListViewIconsItemComponent component)
		{
			base.DefaultColoring(component);
			component.Text.color = base.DefaultColor;
		}

		// Token: 0x0400310C RID: 12556
		[NonSerialized]
		private bool isStartedListViewIcons;
	}
}
