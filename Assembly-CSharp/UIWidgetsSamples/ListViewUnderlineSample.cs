using System;
using System.Collections.Generic;
using System.Linq;
using UIWidgets;

namespace UIWidgetsSamples
{
	// Token: 0x02000913 RID: 2323
	public class ListViewUnderlineSample : ListViewCustom<ListViewUnderlineSampleComponent, ListViewUnderlineSampleItemDescription>
	{
		// Token: 0x060045E0 RID: 17888 RVA: 0x0017C4F6 File Offset: 0x0017A8F6
		protected override void Awake()
		{
			this.Start();
		}

		// Token: 0x060045E1 RID: 17889 RVA: 0x0017C4FE File Offset: 0x0017A8FE
		public override void Start()
		{
			if (this.isStartedListViewCustomSample)
			{
				return;
			}
			this.isStartedListViewCustomSample = true;
			base.SortFunc = ((List<ListViewUnderlineSampleItemDescription> x) => (from y in x
			orderby y.Name
			select y).ToList<ListViewUnderlineSampleItemDescription>());
			base.Start();
		}

		// Token: 0x060045E2 RID: 17890 RVA: 0x0017C53C File Offset: 0x0017A93C
		protected override void SetData(ListViewUnderlineSampleComponent component, ListViewUnderlineSampleItemDescription item)
		{
			component.SetData(item);
		}

		// Token: 0x060045E3 RID: 17891 RVA: 0x0017C545 File Offset: 0x0017A945
		protected override void HighlightColoring(ListViewUnderlineSampleComponent component)
		{
			component.Underline.color = this.HighlightedColor;
			component.Text.color = this.HighlightedColor;
		}

		// Token: 0x060045E4 RID: 17892 RVA: 0x0017C569 File Offset: 0x0017A969
		protected override void SelectColoring(ListViewUnderlineSampleComponent component)
		{
			component.Underline.color = base.SelectedColor;
			component.Text.color = base.SelectedColor;
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x0017C58D File Offset: 0x0017A98D
		protected override void DefaultColoring(ListViewUnderlineSampleComponent component)
		{
			component.Underline.color = base.DefaultColor;
			component.Text.color = base.DefaultColor;
		}

		// Token: 0x04002FFB RID: 12283
		private bool isStartedListViewCustomSample;
	}
}
