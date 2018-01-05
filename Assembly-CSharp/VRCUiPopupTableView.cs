using System;
using System.Collections.Generic;

// Token: 0x02000C89 RID: 3209
public class VRCUiPopupTableView : VRCUiPopup
{
	// Token: 0x060063B4 RID: 25524 RVA: 0x002367E7 File Offset: 0x00234BE7
	public override void Awake()
	{
		base.Awake();
		this.tableViewController = base.GetComponentInChildren<ItemGroupTableViewController>();
	}

	// Token: 0x060063B5 RID: 25525 RVA: 0x002367FB File Offset: 0x00234BFB
	public override void Initialize(string title, string body)
	{
		base.Initialize(title, body);
	}

	// Token: 0x060063B6 RID: 25526 RVA: 0x00236805 File Offset: 0x00234C05
	public void SetupTableView(IEnumerable<IUIGroupItemDatasource> items, int itemsPerRow, Action<IUIGroupItemDatasource> onItemSelected)
	{
		this.tableViewController.items = new List<IUIGroupItemDatasource>(items);
		this.tableViewController.itemsPerRow = itemsPerRow;
		this.tableViewController.onItemSelected = onItemSelected;
		this.tableViewController.RefreshController();
	}

	// Token: 0x04004907 RID: 18695
	private ItemGroupTableViewController tableViewController;
}
