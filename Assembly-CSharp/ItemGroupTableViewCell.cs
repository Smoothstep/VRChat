using System;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using VRC.Core;

// Token: 0x02000C21 RID: 3105
public class ItemGroupTableViewCell : TableViewCell
{
	// Token: 0x0600601A RID: 24602 RVA: 0x0021D13C File Offset: 0x0021B53C
	public void Setup(List<IUIGroupItemDatasource> items, int itemsPerRow, UiGroupItem groupItemPrefab, Action<IUIGroupItemDatasource> onItemSelected)
	{
		if (this.uiGroupItems == null)
		{
			this.uiGroupItems = new List<UiGroupItem>();
		}
		this.uiGroupItemPrefab = groupItemPrefab;
		this.uiGroupItemWidth = this.uiGroupItemPrefab.GetComponent<RectTransform>().rect.width;
		if (this.uiGroupItems.Count > 0 && this.uiGroupItems.Count == items.Count)
		{
			this.RecycleGroup(items, onItemSelected);
		}
		else
		{
			this.DestroyUiGroupItems();
			this.CreateGroup(items, itemsPerRow, onItemSelected);
		}
	}

	// Token: 0x0600601B RID: 24603 RVA: 0x0021D1CC File Offset: 0x0021B5CC
	private void CreateGroup(List<IUIGroupItemDatasource> items, int itemsPerRow, Action<IUIGroupItemDatasource> onItemSelected)
	{
		for (int i = 0; i < items.Count; i++)
		{
			GameObject gameObject = (GameObject)AssetManagement.Instantiate(this.uiGroupItemPrefab.gameObject);
			gameObject.transform.SetParent(base.transform);
			Vector3 zero = Vector3.zero;
			zero.x = ((float)(-(float)(itemsPerRow - 1)) * 0.5f + (float)i) * this.uiGroupItemWidth;
			gameObject.transform.localPosition = zero;
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.transform.localRotation = Quaternion.identity;
			UiGroupItem component = gameObject.GetComponent<UiGroupItem>();
			this.uiGroupItems.Add(component);
			component.Setup(items[i], onItemSelected);
		}
	}

	// Token: 0x0600601C RID: 24604 RVA: 0x0021D298 File Offset: 0x0021B698
	private void RecycleGroup(List<IUIGroupItemDatasource> items, Action<IUIGroupItemDatasource> onItemSelected)
	{
		for (int i = 0; i < items.Count; i++)
		{
			UiGroupItem uiGroupItem = this.uiGroupItems[i];
			uiGroupItem.Setup(items[i], onItemSelected);
		}
	}

	// Token: 0x0600601D RID: 24605 RVA: 0x0021D2D8 File Offset: 0x0021B6D8
	private void DestroyUiGroupItems()
	{
		int count = this.uiGroupItems.Count;
		for (int i = 0; i < count; i++)
		{
			this.uiGroupItems[i].gameObject.SetActive(false);
			UnityEngine.Object.DestroyImmediate(this.uiGroupItems[i]);
		}
		this.uiGroupItems.Clear();
	}

	// Token: 0x040045D2 RID: 17874
	private UiGroupItem uiGroupItemPrefab;

	// Token: 0x040045D3 RID: 17875
	private float uiGroupItemWidth;

	// Token: 0x040045D4 RID: 17876
	private List<UiGroupItem> uiGroupItems;
}
