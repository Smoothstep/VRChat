using System;
using UIWidgets;

namespace UIWidgetsSamples
{
	// Token: 0x02000916 RID: 2326
	public class ListViewVariableHeight : ListViewCustomHeight<ListViewVariableHeightComponent, ListViewVariableHeightItemDescription>
	{
		// Token: 0x060045EE RID: 17902 RVA: 0x0017CC54 File Offset: 0x0017B054
		protected override void SetData(ListViewVariableHeightComponent component, ListViewVariableHeightItemDescription item)
		{
			component.SetData(item);
		}

		// Token: 0x060045EF RID: 17903 RVA: 0x0017CC5D File Offset: 0x0017B05D
		protected override void HighlightColoring(ListViewVariableHeightComponent component)
		{
			base.HighlightColoring(component);
			component.Name.color = this.HighlightedColor;
			component.Text.color = this.HighlightedColor;
		}

		// Token: 0x060045F0 RID: 17904 RVA: 0x0017CC88 File Offset: 0x0017B088
		protected override void SelectColoring(ListViewVariableHeightComponent component)
		{
			base.SelectColoring(component);
			component.Name.color = base.SelectedColor;
			component.Text.color = base.SelectedColor;
		}

		// Token: 0x060045F1 RID: 17905 RVA: 0x0017CCB3 File Offset: 0x0017B0B3
		protected override void DefaultColoring(ListViewVariableHeightComponent component)
		{
			base.DefaultColoring(component);
			component.Name.color = base.DefaultColor;
			component.Text.color = base.DefaultColor;
		}
	}
}
