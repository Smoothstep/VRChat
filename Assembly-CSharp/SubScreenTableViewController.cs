using System;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using VRC.Core;

// Token: 0x02000C3B RID: 3131
public class SubScreenTableViewController : MonoBehaviour, ITableViewDataSource
{
	// Token: 0x17000DA0 RID: 3488
	// (get) Token: 0x06006139 RID: 24889 RVA: 0x0022493A File Offset: 0x00222D3A
	// (set) Token: 0x0600613A RID: 24890 RVA: 0x00224944 File Offset: 0x00222D44
	public int currentSubScreenTabRow
	{
		get
		{
			return this.mCurrentSubScreenTabRow;
		}
		set
		{
			SubScreenTabTableViewCell oldCell = (SubScreenTabTableViewCell)this.mTableView.GetCellAtRow(this.mCurrentSubScreenTabRow);
			this.mCurrentSubScreenTabRow = value;
			SubScreenTabTableViewCell subScreenTabTableViewCell = (SubScreenTabTableViewCell)this.mTableView.GetCellAtRow(this.mCurrentSubScreenTabRow);
			if (subScreenTabTableViewCell != null)
			{
				this.currentTabName = subScreenTabTableViewCell.name.text;
			}
			this.RefreshCurrentSubScreenTabCell(oldCell, subScreenTabTableViewCell);
		}
	}

	// Token: 0x0600613B RID: 24891 RVA: 0x002249AC File Offset: 0x00222DAC
	public void Initialize()
	{
		this.mSubPages = new List<VRCUiPage>();
		this.mTableView = base.GetComponentInChildren<TableView>();
		this.mTableView.dataSource = this;
		this.mTableViewExtensions = this.mTableView.gameObject.GetOrAddComponent<TableViewExtensions>();
		this.mIsInitialized = true;
	}

	// Token: 0x0600613C RID: 24892 RVA: 0x002249F9 File Offset: 0x00222DF9
	public void SetSubPages(List<VRCUiPage> subPages)
	{
		if (!this.mIsInitialized)
		{
			this.Initialize();
		}
		this.mSubPages = subPages;
		this.mTableView.ReloadData();
	}

	// Token: 0x0600613D RID: 24893 RVA: 0x00224A1E File Offset: 0x00222E1E
	public int GetNumberOfRowsForTableView(TableView tableView)
	{
		return this.mSubPages.Count * this.mInfiniteScrollNumber;
	}

	// Token: 0x0600613E RID: 24894 RVA: 0x00224A34 File Offset: 0x00222E34
	public float GetHeightForRowInTableView(TableView tableView, int row)
	{
		return this.subScreenTabPrefab.GetComponent<RectTransform>().rect.height;
	}

	// Token: 0x0600613F RID: 24895 RVA: 0x00224A5C File Offset: 0x00222E5C
	public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
	{
		SubScreenTabTableViewCell subScreenTabTableViewCell = tableView.GetReusableCell(this.subScreenTabPrefab.reuseIdentifier) as SubScreenTabTableViewCell;
		if (subScreenTabTableViewCell == null)
		{
			subScreenTabTableViewCell = (SubScreenTabTableViewCell)AssetManagement.Instantiate(this.subScreenTabPrefab);
			UnityEngine.Object gameObject = subScreenTabTableViewCell.gameObject;
			string str = "SubScreenTableViewCellInstance_";
			int num = ++this.mNumInstancesCreated;
			gameObject.name = str + num.ToString();
		}
		VRCUiPage subPage = this.mSubPages[row % this.mSubPages.Count];
		subScreenTabTableViewCell.SetupCell(subPage, this.defaultTabFontSize, this.defaultTabColor, this.selectedTabFontSize, this.selectedTabColor);
		return subScreenTabTableViewCell;
	}

	// Token: 0x06006140 RID: 24896 RVA: 0x00224B0A File Offset: 0x00222F0A
	public void ShowCurrentSubScreenTabSubScreen()
	{
		VRCUiManager.Instance.ShowScreen(this.mSubPages[this.currentSubScreenTabRow % this.mSubPages.Count].transform.GetHierarchyPath());
	}

	// Token: 0x06006141 RID: 24897 RVA: 0x00224B40 File Offset: 0x00222F40
	public void SetCurrentSubScreenTab(string displayName)
	{
		int num = this.mInfiniteScrollNumber / 2;
		for (int i = 0; i < this.numberOfVisibleSubScreenTabs; i++)
		{
			int num2 = num + i;
			if (this.mSubPages[num2 % this.mSubPages.Count].displayName == displayName)
			{
				int row = this.CalculateTopRowFromRow(num2);
				this.mTableViewExtensions.ScrollToRowImmediate(row);
				this.mTableView.ReloadData();
				this.currentSubScreenTabRow = num2;
				return;
			}
		}
		Debug.LogError("Could not find subpage - " + displayName);
	}

	// Token: 0x06006142 RID: 24898 RVA: 0x00224BD0 File Offset: 0x00222FD0
	public void ScrollUpOne()
	{
		int nextRow = this.currentSubScreenTabRow + 1;
		int row = this.CalculateTopRowFromRow(nextRow);
		this.isAnimatingToTab = true;
		this.mTableViewExtensions.ScrollToRowAnimated(row, 0.5f, delegate
		{
			this.currentSubScreenTabRow = nextRow;
			this.isAnimatingToTab = false;
		});
		VRCUiPage vrcuiPage = this.mSubPages[nextRow % this.mSubPages.Count];
		VRCUiManager.Instance.ShowScreen(vrcuiPage.transform.GetHierarchyPath());
	}

	// Token: 0x06006143 RID: 24899 RVA: 0x00224C5C File Offset: 0x0022305C
	public void ScrollDownOne()
	{
		int nextRow = this.currentSubScreenTabRow - 1;
		int row = this.CalculateTopRowFromRow(nextRow);
		this.isAnimatingToTab = true;
		this.mTableViewExtensions.ScrollToRowAnimated(row, 0.5f, delegate
		{
			this.currentSubScreenTabRow = nextRow;
			this.isAnimatingToTab = false;
		});
		VRCUiPage vrcuiPage = this.mSubPages[nextRow % this.mSubPages.Count];
		VRCUiManager.Instance.ShowScreen(vrcuiPage.transform.GetHierarchyPath());
	}

	// Token: 0x06006144 RID: 24900 RVA: 0x00224CE8 File Offset: 0x002230E8
	private void RefreshCurrentSubScreenTabCell(SubScreenTabTableViewCell oldCell, SubScreenTabTableViewCell newCell)
	{
		if (oldCell != null)
		{
			oldCell.RefreshAsNotSelected();
		}
		if (newCell != null)
		{
			newCell.RefreshAsSelected();
		}
	}

	// Token: 0x06006145 RID: 24901 RVA: 0x00224D10 File Offset: 0x00223110
	private int CalculateTopRowFromRow(int row)
	{
		int num = row - (this.numberOfVisibleSubScreenTabs - 1) / 2;
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	// Token: 0x040046DA RID: 18138
	public SubScreenTabTableViewCell subScreenTabPrefab;

	// Token: 0x040046DB RID: 18139
	private TableView mTableView;

	// Token: 0x040046DC RID: 18140
	private TableViewExtensions mTableViewExtensions;

	// Token: 0x040046DD RID: 18141
	private int mNumInstancesCreated;

	// Token: 0x040046DE RID: 18142
	private List<VRCUiPage> mSubPages;

	// Token: 0x040046DF RID: 18143
	public int numberOfVisibleSubScreenTabs = 3;

	// Token: 0x040046E0 RID: 18144
	[HideInInspector]
	public string currentTabName;

	// Token: 0x040046E1 RID: 18145
	private int mCurrentSubScreenTabRow;

	// Token: 0x040046E2 RID: 18146
	private int mInfiniteScrollNumber = 100;

	// Token: 0x040046E3 RID: 18147
	public Color selectedTabColor = Color.white;

	// Token: 0x040046E4 RID: 18148
	public int selectedTabFontSize = 60;

	// Token: 0x040046E5 RID: 18149
	public Color defaultTabColor = Color.white;

	// Token: 0x040046E6 RID: 18150
	public int defaultTabFontSize = 39;

	// Token: 0x040046E7 RID: 18151
	[HideInInspector]
	public bool isAnimatingToTab;

	// Token: 0x040046E8 RID: 18152
	private bool mIsInitialized;
}
