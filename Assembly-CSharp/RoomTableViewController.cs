using System;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.Core;

// Token: 0x02000C35 RID: 3125
public class RoomTableViewController : MonoBehaviour, ITableViewDataSource
{
	// Token: 0x06006118 RID: 24856 RVA: 0x002240F0 File Offset: 0x002224F0
	private void Awake()
	{
		this.mTableView = base.GetComponentInChildren<TableView>();
		this.mTableView.dataSource = this;
		this.myRoomsToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnMyRoomsToggled));
		this.mTableViewExtensions = this.mTableView.gameObject.GetOrAddComponent<TableViewExtensions>();
	}

	// Token: 0x06006119 RID: 24857 RVA: 0x00224147 File Offset: 0x00222547
	private void Start()
	{
		this.Reset(true);
	}

	// Token: 0x0600611A RID: 24858 RVA: 0x00224150 File Offset: 0x00222550
	private void Reset(bool clear = true)
	{
		if (clear)
		{
			this.worlds.Clear();
			this.mTableView.ReloadData();
		}
		this.timeSinceUpdate = 0f;
		this.currentSearchString = this.searchField.text;
		ApiWorld.SortOwnership owner = ApiWorld.SortOwnership.Any;
		if (this.myRoomsToggle.isOn)
		{
			owner = ApiWorld.SortOwnership.Mine;
		}
		ApiWorld.FetchList(delegate(List<ApiWorld> list)
		{
			this.worlds = list;
			this.mTableView.ReloadData();
		}, delegate(string msg)
		{
			Debug.LogWarning(msg);
		}, ApiWorld.SortHeading.Updated, owner, ApiWorld.SortOrder.Descending, 0, 100, this.currentSearchString, null, null, string.Empty, ApiWorld.ReleaseStatus.Public, true);
	}

	// Token: 0x0600611B RID: 24859 RVA: 0x002241EC File Offset: 0x002225EC
	public int GetNumberOfRowsForTableView(TableView tableView)
	{
		return this.worlds.Count;
	}

	// Token: 0x0600611C RID: 24860 RVA: 0x002241FC File Offset: 0x002225FC
	public float GetHeightForRowInTableView(TableView tableView, int row)
	{
		return this.roomTableViewCellPrefab.GetComponent<RectTransform>().rect.height;
	}

	// Token: 0x0600611D RID: 24861 RVA: 0x00224224 File Offset: 0x00222624
	public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
	{
		RoomTableViewCell roomTableViewCell = tableView.GetReusableCell(this.roomTableViewCellPrefab.reuseIdentifier) as RoomTableViewCell;
		if (roomTableViewCell == null)
		{
			roomTableViewCell = (RoomTableViewCell)AssetManagement.Instantiate(this.roomTableViewCellPrefab);
			UnityEngine.Object gameObject = roomTableViewCell.gameObject;
			string str = "RoomTableViewCellInstance_";
			int num = ++this.mNumInstancesCreated;
			gameObject.name = str + num.ToString();
		}
		roomTableViewCell.RefreshCell(this.worlds[row]);
		return roomTableViewCell;
	}

	// Token: 0x0600611E RID: 24862 RVA: 0x002242AC File Offset: 0x002226AC
	public void SortBy(string sortTypeString)
	{
		this.Reset(true);
	}

	// Token: 0x0600611F RID: 24863 RVA: 0x002242B8 File Offset: 0x002226B8
	private void Update()
	{
		this.timeSinceUpdate += Time.deltaTime;
		if (this.timeSinceUpdate > 30f || this.currentSearchString != this.searchField.text)
		{
			this.Reset(true);
		}
	}

	// Token: 0x06006120 RID: 24864 RVA: 0x00224309 File Offset: 0x00222709
	private void OnMyRoomsToggled(bool value)
	{
		this.Reset(true);
	}

	// Token: 0x040046BF RID: 18111
	private TableView mTableView;

	// Token: 0x040046C0 RID: 18112
	private TableViewExtensions mTableViewExtensions;

	// Token: 0x040046C1 RID: 18113
	public RoomTableViewCell roomTableViewCellPrefab;

	// Token: 0x040046C2 RID: 18114
	public bool useMyRoomData;

	// Token: 0x040046C3 RID: 18115
	public UiInputField searchField;

	// Token: 0x040046C4 RID: 18116
	public Toggle myRoomsToggle;

	// Token: 0x040046C5 RID: 18117
	private List<ApiWorld> worlds = new List<ApiWorld>();

	// Token: 0x040046C6 RID: 18118
	private int mNumInstancesCreated;

	// Token: 0x040046C7 RID: 18119
	private string currentSearchString = string.Empty;

	// Token: 0x040046C8 RID: 18120
	private float timeSinceUpdate;
}
