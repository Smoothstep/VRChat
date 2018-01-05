using System;
using System.Collections.Generic;
using System.Linq;
using UIWidgets;

namespace UIWidgetsSamples
{
	// Token: 0x02000910 RID: 2320
	public class ListViewCustomSample : ListViewCustom<ListViewCustomSampleComponent, ListViewCustomSampleItemDescription>
	{
		// Token: 0x060045D4 RID: 17876 RVA: 0x0017C1F8 File Offset: 0x0017A5F8
		protected override void Awake()
		{
			this.Start();
		}

		// Token: 0x060045D5 RID: 17877 RVA: 0x0017C200 File Offset: 0x0017A600
		public override void Start()
		{
			if (this.isStartedListViewCustomSample)
			{
				return;
			}
			this.isStartedListViewCustomSample = true;
			base.SortFunc = ((List<ListViewCustomSampleItemDescription> x) => (from y in x
			orderby y.Name
			select y).ToList<ListViewCustomSampleItemDescription>());
			base.Start();
		}

		// Token: 0x060045D6 RID: 17878 RVA: 0x0017C23E File Offset: 0x0017A63E
		protected override void SetData(ListViewCustomSampleComponent component, ListViewCustomSampleItemDescription item)
		{
			component.SetData(item);
		}

		// Token: 0x060045D7 RID: 17879 RVA: 0x0017C247 File Offset: 0x0017A647
		protected override void HighlightColoring(ListViewCustomSampleComponent component)
		{
			base.HighlightColoring(component);
			component.Text.color = this.HighlightedColor;
		}

		// Token: 0x060045D8 RID: 17880 RVA: 0x0017C261 File Offset: 0x0017A661
		protected override void SelectColoring(ListViewCustomSampleComponent component)
		{
			base.SelectColoring(component);
			component.Text.color = base.SelectedColor;
		}

		// Token: 0x060045D9 RID: 17881 RVA: 0x0017C27B File Offset: 0x0017A67B
		protected override void DefaultColoring(ListViewCustomSampleComponent component)
		{
			base.DefaultColoring(component);
			component.Text.color = base.DefaultColor;
		}

		// Token: 0x04002FF3 RID: 12275
		private bool isStartedListViewCustomSample;
	}
}
