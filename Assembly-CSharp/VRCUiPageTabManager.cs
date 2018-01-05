using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC;
using VRC.Core;

// Token: 0x02000C7F RID: 3199
public class VRCUiPageTabManager : MonoBehaviour
{
	// Token: 0x17000DB6 RID: 3510
	// (get) Token: 0x06006344 RID: 25412 RVA: 0x002347EE File Offset: 0x00232BEE
	public static VRCUiPageTabManager Instance
	{
		get
		{
			return VRCUiPageTabManager.mInstance;
		}
	}

	// Token: 0x17000DB7 RID: 3511
	// (get) Token: 0x06006345 RID: 25413 RVA: 0x002347F5 File Offset: 0x00232BF5
	public VRCUiPageTabManager.TabContext currentContext
	{
		get
		{
			return this.mCurrentContext;
		}
	}

	// Token: 0x06006346 RID: 25414 RVA: 0x00234800 File Offset: 0x00232C00
	private void Awake()
	{
		if (VRCUiPageTabManager.mInstance != null)
		{
			Debug.LogError("More than one VRCUiPageTabManager detected!!!");
		}
		else
		{
			VRCUiPageTabManager.mInstance = this;
		}
		this.allPageTabs = new List<VRCUiPageTab>();
		this.currentPageTabs = new List<VRCUiPageTab>();
		this.tabGroups = Tools.FindSceneObjectsOfTypeAll<VRCUiPageTabGroup>();
	}

	// Token: 0x06006347 RID: 25415 RVA: 0x00234854 File Offset: 0x00232C54
	public VRCUiPageTab CreateTab(string title, string screenPath, VRCUiPageTabManager.TabContext context, VRCUiPageTabGroup tabGroup = null, UnityAction onClick = null)
	{
		VRCUiPageTab vrcuiPageTab = (VRCUiPageTab)AssetManagement.Instantiate(this.pageTabPrefab);
		vrcuiPageTab.Fill(title, screenPath, context);
		this.allPageTabs.Add(vrcuiPageTab);
		this.currentPageTabs.Add(vrcuiPageTab);
		if (tabGroup != null)
		{
			tabGroup.AddTab(vrcuiPageTab);
			vrcuiPageTab.transform.SetParent(tabGroup.transform);
			vrcuiPageTab.transform.Reset();
		}
		if (onClick != null)
		{
			Button component = vrcuiPageTab.GetComponent<Button>();
			component.onClick.AddListener(onClick);
		}
		return vrcuiPageTab;
	}

	// Token: 0x06006348 RID: 25416 RVA: 0x002348E1 File Offset: 0x00232CE1
	public void SetContext(VRCUiPageTabManager.TabContext context)
	{
		this.mCurrentContext = context;
		this.UpdateTabsForCurrentContext();
	}

	// Token: 0x06006349 RID: 25417 RVA: 0x002348F0 File Offset: 0x00232CF0
	public void UpdateTabsForCurrentContext()
	{
		this.DisableCurrentTabs();
		this.EnableTabsForCurrentContext();
		foreach (VRCUiPageTabGroup vrcuiPageTabGroup in this.tabGroups)
		{
			vrcuiPageTabGroup.RefreshLayout();
		}
	}

	// Token: 0x0600634A RID: 25418 RVA: 0x00234930 File Offset: 0x00232D30
	private void EnableTabsForCurrentContext()
	{
		this.currentPageTabs.Clear();
		foreach (VRCUiPageTab vrcuiPageTab in this.allPageTabs)
		{
			if (vrcuiPageTab.enabledContext == this.mCurrentContext || vrcuiPageTab.enabledContext == VRCUiPageTabManager.TabContext.Everywhere)
			{
				vrcuiPageTab.gameObject.SetActive(true);
				this.currentPageTabs.Add(vrcuiPageTab);
			}
		}
	}

	// Token: 0x0600634B RID: 25419 RVA: 0x002349C4 File Offset: 0x00232DC4
	private void DisableCurrentTabs()
	{
		foreach (VRCUiPageTab vrcuiPageTab in this.currentPageTabs)
		{
			vrcuiPageTab.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600634C RID: 25420 RVA: 0x00234A28 File Offset: 0x00232E28
	public void ShowTab(VRCUiPageTab tab)
	{
		for (int i = 0; i < this.currentPageTabs.Count; i++)
		{
			if (this.currentPageTabs[i] == tab)
			{
				this.selected = i;
			}
		}
		VRCUiManager.Instance.ShowScreen(tab.screen);
	}

	// Token: 0x0600634D RID: 25421 RVA: 0x00234A80 File Offset: 0x00232E80
	public void Select(int delta)
	{
		if (this.currentPageTabs.Count == 0)
		{
			return;
		}
		int num = this.selected;
		do
		{
			num += delta;
			if (num < 0)
			{
				num = this.currentPageTabs.Count - 1;
			}
			if (num >= this.currentPageTabs.Count)
			{
				num = 0;
			}
			if (num == this.selected)
			{
				break;
			}
		}
		while (this.currentPageTabs[num].screen == null);
		this.selected = num;
		VRCUiManager.Instance.ShowScreen(this.currentPageTabs[this.selected].screen);
	}

	// Token: 0x040048AE RID: 18606
	private static VRCUiPageTabManager mInstance;

	// Token: 0x040048AF RID: 18607
	public VRCUiPageTab pageTabPrefab;

	// Token: 0x040048B0 RID: 18608
	private int selected;

	// Token: 0x040048B1 RID: 18609
	private VRCUiPageTabGroup[] tabGroups;

	// Token: 0x040048B2 RID: 18610
	private VRCUiPageTabManager.TabContext mCurrentContext = VRCUiPageTabManager.TabContext.InMainMenu;

	// Token: 0x040048B3 RID: 18611
	private List<VRCUiPageTab> allPageTabs;

	// Token: 0x040048B4 RID: 18612
	private List<VRCUiPageTab> currentPageTabs;

	// Token: 0x02000C80 RID: 3200
	public enum TabContext
	{
		// Token: 0x040048B6 RID: 18614
		Everywhere,
		// Token: 0x040048B7 RID: 18615
		InMainMenu,
		// Token: 0x040048B8 RID: 18616
		InRoom
	}
}
