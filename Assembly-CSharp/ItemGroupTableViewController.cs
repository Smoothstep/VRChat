using System;
using System.Collections.Generic;
using System.Linq;
using Tacticsoft;
using UnityEngine;
using VRC.Core;

// Token: 0x02000C22 RID: 3106
public class ItemGroupTableViewController : MonoBehaviour, ITableViewDataSource
{
	// Token: 0x17000D92 RID: 3474
	// (get) Token: 0x0600601F RID: 24607 RVA: 0x0021D357 File Offset: 0x0021B757
	// (set) Token: 0x06006020 RID: 24608 RVA: 0x0021D35F File Offset: 0x0021B75F
	public List<IUIGroupItemDatasource> items
	{
		get
		{
			return this.mItems;
		}
		set
		{
			this.mItems = value;
			this.searchedItemsSubset = new List<IUIGroupItemDatasource>(value);
		}
	}

	// Token: 0x06006021 RID: 24609 RVA: 0x0021D374 File Offset: 0x0021B774
	private void OnEnable()
	{
		this.RefreshController();
	}

	// Token: 0x06006022 RID: 24610 RVA: 0x0021D37C File Offset: 0x0021B77C
	private void SetupController()
	{
		this.mTableView = base.GetComponentInChildren<TableView>();
		this.mTableView.dataSource = this;
		this.items = new List<IUIGroupItemDatasource>();
		this.isInitialized = true;
	}

	// Token: 0x06006023 RID: 24611 RVA: 0x0021D3A8 File Offset: 0x0021B7A8
	public void RefreshController()
	{
		if (!this.isInitialized)
		{
			this.SetupController();
		}
		this.currentSearchString = string.Empty;
		this.searchedItemsSubset = new List<IUIGroupItemDatasource>(this.items);
		this.lcdItemsPerRow = Mathf.Min(this.itemsPerRow, this.searchedItemsSubset.Count);
		this.mTableView.ReloadData();
	}

	// Token: 0x06006024 RID: 24612 RVA: 0x0021D40C File Offset: 0x0021B80C
	public int GetNumberOfRowsForTableView(TableView tableView)
	{
		this.lcdItemsPerRow = Mathf.Min(this.itemsPerRow, this.searchedItemsSubset.Count);
		int result = 0;
		if (this.lcdItemsPerRow > 0)
		{
			result = (this.searchedItemsSubset.Count + this.lcdItemsPerRow - 1) / this.lcdItemsPerRow;
		}
		return result;
	}

	// Token: 0x06006025 RID: 24613 RVA: 0x0021D460 File Offset: 0x0021B860
	public float GetHeightForRowInTableView(TableView tableView, int row)
	{
		return this.uiGroupItemPrefab.GetComponent<RectTransform>().rect.height;
	}

	// Token: 0x06006026 RID: 24614 RVA: 0x0021D488 File Offset: 0x0021B888
	public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
	{
		ItemGroupTableViewCell itemGroupTableViewCell = tableView.GetReusableCell(this.cellPrefab.reuseIdentifier) as ItemGroupTableViewCell;
		if (itemGroupTableViewCell == null)
		{
			itemGroupTableViewCell = (ItemGroupTableViewCell)AssetManagement.Instantiate(this.cellPrefab);
			UnityEngine.Object gameObject = itemGroupTableViewCell.gameObject;
			string str = "ItemGroupTableViewCell_";
			int num = ++this.mNumInstancesCreated;
			gameObject.name = str + num.ToString();
			itemGroupTableViewCell.transform.localRotation = Quaternion.identity;
		}
		List<IUIGroupItemDatasource> list = new List<IUIGroupItemDatasource>();
		for (int i = 0; i < this.lcdItemsPerRow; i++)
		{
			int num2 = row * this.lcdItemsPerRow + i;
			if (num2 >= this.searchedItemsSubset.Count)
			{
				break;
			}
			list.Add(this.searchedItemsSubset[num2]);
		}
		itemGroupTableViewCell.Setup(list, this.itemsPerRow, this.uiGroupItemPrefab, this.onItemSelected);
		return itemGroupTableViewCell;
	}

	// Token: 0x06006027 RID: 24615 RVA: 0x0021D580 File Offset: 0x0021B980
	public void TrimListBySearch(string searchString = "")
	{
		this.currentSearchString = searchString;
		this.searchedItemsSubset = new List<IUIGroupItemDatasource>(this.items);
		if (!string.IsNullOrEmpty(this.currentSearchString))
		{
			searchString = searchString.ToLower();
			this.searchedItemsSubset = (from r in this.items
			where r.name.ToLower().Contains(searchString)
			select r).ToList<IUIGroupItemDatasource>();
		}
		if (this.mTableView != null)
		{
			this.mTableView.ReloadData();
		}
	}

	// Token: 0x040045D5 RID: 17877
	public ItemGroupTableViewCell cellPrefab;

	// Token: 0x040045D6 RID: 17878
	public UiGroupItem uiGroupItemPrefab;

	// Token: 0x040045D7 RID: 17879
	private TableView mTableView;

	// Token: 0x040045D8 RID: 17880
	private int mNumInstancesCreated;

	// Token: 0x040045D9 RID: 17881
	private List<IUIGroupItemDatasource> mItems;

	// Token: 0x040045DA RID: 17882
	private string currentSearchString = string.Empty;

	// Token: 0x040045DB RID: 17883
	private List<IUIGroupItemDatasource> searchedItemsSubset;

	// Token: 0x040045DC RID: 17884
	public int itemsPerRow = 2;

	// Token: 0x040045DD RID: 17885
	private int lcdItemsPerRow = 2;

	// Token: 0x040045DE RID: 17886
	public Action<IUIGroupItemDatasource> onItemSelected;

	// Token: 0x040045DF RID: 17887
	private bool isInitialized;
}
