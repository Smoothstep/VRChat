using System;
using UnityEngine;

namespace Tacticsoft.Examples
{
	// Token: 0x020008DA RID: 2266
	public class SimpleTableViewController : MonoBehaviour, ITableViewDataSource
	{
		// Token: 0x060044F1 RID: 17649 RVA: 0x00170FF4 File Offset: 0x0016F3F4
		private void Start()
		{
			this.m_tableView.dataSource = this;
		}

		// Token: 0x060044F2 RID: 17650 RVA: 0x00171002 File Offset: 0x0016F402
		public void SendBeer()
		{
			Application.OpenURL("https://www.paypal.com/cgi-bin/webscr?business=contact@tacticsoft.net&cmd=_xclick&item_name=Beer%20for%20TSTableView&currency_code=USD&amount=5.00");
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x0017100E File Offset: 0x0016F40E
		public int GetNumberOfRowsForTableView(TableView tableView)
		{
			return this.m_numRows;
		}

		// Token: 0x060044F4 RID: 17652 RVA: 0x00171018 File Offset: 0x0016F418
		public float GetHeightForRowInTableView(TableView tableView, int row)
		{
			return (this.m_cellPrefab.transform as RectTransform).rect.height;
		}

		// Token: 0x060044F5 RID: 17653 RVA: 0x00171044 File Offset: 0x0016F444
		public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
		{
			VisibleCounterCell visibleCounterCell = tableView.GetReusableCell(this.m_cellPrefab.reuseIdentifier) as VisibleCounterCell;
			if (visibleCounterCell == null)
			{
				visibleCounterCell = UnityEngine.Object.Instantiate<VisibleCounterCell>(this.m_cellPrefab);
				UnityEngine.Object @object = visibleCounterCell;
				string str = "VisibleCounterCellInstance_";
				int num = ++this.m_numInstancesCreated;
				@object.name = str + num.ToString();
			}
			visibleCounterCell.SetRowNumber(row);
			return visibleCounterCell;
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x001710B8 File Offset: 0x0016F4B8
		public void TableViewCellVisibilityChanged(int row, bool isVisible)
		{
			if (isVisible)
			{
				VisibleCounterCell visibleCounterCell = (VisibleCounterCell)this.m_tableView.GetCellAtRow(row);
				visibleCounterCell.NotifyBecameVisible();
			}
		}

		// Token: 0x04002EEE RID: 12014
		public VisibleCounterCell m_cellPrefab;

		// Token: 0x04002EEF RID: 12015
		public TableView m_tableView;

		// Token: 0x04002EF0 RID: 12016
		public int m_numRows;

		// Token: 0x04002EF1 RID: 12017
		private int m_numInstancesCreated;
	}
}
