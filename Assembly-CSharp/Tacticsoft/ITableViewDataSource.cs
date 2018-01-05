using System;

namespace Tacticsoft
{
	// Token: 0x020008DD RID: 2269
	public interface ITableViewDataSource
	{
		// Token: 0x06004503 RID: 17667
		int GetNumberOfRowsForTableView(TableView tableView);

		// Token: 0x06004504 RID: 17668
		float GetHeightForRowInTableView(TableView tableView, int row);

		// Token: 0x06004505 RID: 17669
		TableViewCell GetCellForRowInTableView(TableView tableView, int row);
	}
}
