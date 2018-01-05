using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tacticsoft.Examples
{
	// Token: 0x020008D8 RID: 2264
	public class DynamicHeightTableViewController : MonoBehaviour, ITableViewDataSource
	{
		// Token: 0x060044E4 RID: 17636 RVA: 0x00170D0D File Offset: 0x0016F10D
		private void Start()
		{
			this.m_customRowHeights = new Dictionary<int, float>();
			this.m_tableView.dataSource = this;
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x00170D26 File Offset: 0x0016F126
		public void SendBeer()
		{
			Application.OpenURL("https://www.paypal.com/cgi-bin/webscr?business=contact@tacticsoft.net&cmd=_xclick&item_name=Beer%20for%20TSTableView&currency_code=USD&amount=5.00");
		}

		// Token: 0x060044E6 RID: 17638 RVA: 0x00170D32 File Offset: 0x0016F132
		public int GetNumberOfRowsForTableView(TableView tableView)
		{
			return this.m_numRows;
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x00170D3A File Offset: 0x0016F13A
		public float GetHeightForRowInTableView(TableView tableView, int row)
		{
			return this.GetHeightOfRow(row);
		}

		// Token: 0x060044E8 RID: 17640 RVA: 0x00170D44 File Offset: 0x0016F144
		public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
		{
			DynamicHeightCell dynamicHeightCell = tableView.GetReusableCell(this.m_cellPrefab.reuseIdentifier) as DynamicHeightCell;
			if (dynamicHeightCell == null)
			{
				dynamicHeightCell = UnityEngine.Object.Instantiate<DynamicHeightCell>(this.m_cellPrefab);
				UnityEngine.Object @object = dynamicHeightCell;
				string str = "DynamicHeightCellInstance_";
				int num = ++this.m_numInstancesCreated;
				@object.name = str + num.ToString();
				dynamicHeightCell.onCellHeightChanged.AddListener(new UnityAction<int, float>(this.OnCellHeightChanged));
			}
			dynamicHeightCell.rowNumber = row;
			dynamicHeightCell.height = this.GetHeightOfRow(row);
			return dynamicHeightCell;
		}

		// Token: 0x060044E9 RID: 17641 RVA: 0x00170DDB File Offset: 0x0016F1DB
		private float GetHeightOfRow(int row)
		{
			if (this.m_customRowHeights.ContainsKey(row))
			{
				return this.m_customRowHeights[row];
			}
			return this.m_cellPrefab.height;
		}

		// Token: 0x060044EA RID: 17642 RVA: 0x00170E06 File Offset: 0x0016F206
		private void OnCellHeightChanged(int row, float newHeight)
		{
			if (this.GetHeightOfRow(row) == newHeight)
			{
				return;
			}
			this.m_customRowHeights[row] = newHeight;
			this.m_tableView.NotifyCellDimensionsChanged(row);
		}

		// Token: 0x04002EE8 RID: 12008
		public DynamicHeightCell m_cellPrefab;

		// Token: 0x04002EE9 RID: 12009
		public TableView m_tableView;

		// Token: 0x04002EEA RID: 12010
		public int m_numRows;

		// Token: 0x04002EEB RID: 12011
		private int m_numInstancesCreated;

		// Token: 0x04002EEC RID: 12012
		private Dictionary<int, float> m_customRowHeights;
	}
}
