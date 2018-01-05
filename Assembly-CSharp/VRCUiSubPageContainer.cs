using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000C8D RID: 3213
public class VRCUiSubPageContainer : VRCUiPage
{
	// Token: 0x17000DBC RID: 3516
	// (get) Token: 0x060063C3 RID: 25539 RVA: 0x00236F8D File Offset: 0x0023538D
	public bool isAnimatingToTab
	{
		get
		{
			return this.tbController.isAnimatingToTab;
		}
	}

	// Token: 0x17000DBD RID: 3517
	// (get) Token: 0x060063C4 RID: 25540 RVA: 0x00236F9A File Offset: 0x0023539A
	public string currentTabName
	{
		get
		{
			return this.tbController.currentTabName;
		}
	}

	// Token: 0x060063C5 RID: 25541 RVA: 0x00236FA7 File Offset: 0x002353A7
	public void ScrollToTabImmediate(string displayName)
	{
		this.tbController.SetCurrentSubScreenTab(displayName);
	}

	// Token: 0x060063C6 RID: 25542 RVA: 0x00236FB8 File Offset: 0x002353B8
	public override void Awake()
	{
		base.Awake();
		this.onPageActivated = (Action)Delegate.Combine(this.onPageActivated, new Action(this.ShowSubScreens));
		this.onPageDeactivated = (Action)Delegate.Combine(this.onPageDeactivated, new Action(this.HideSubScreens));
		VRCUiPage[] componentsInChildren = base.GetComponentsInChildren<VRCUiSubPage>(true);
		this.subPages = new List<VRCUiPage>();
		foreach (VRCUiSubPage vrcuiSubPage in componentsInChildren)
		{
			if (vrcuiSubPage.shouldHaveSubPageTab)
			{
				this.subPages.Add(vrcuiSubPage);
			}
		}
		this.tbController = base.GetComponentInChildren<SubScreenTableViewController>();
		this.tbController.SetSubPages(this.subPages);
		this.tbController.SetCurrentSubScreenTab(this.defaultSubScreenTab);
		this.upButton.onClick.AddListener(new UnityAction(this.ScrollUpOne));
		this.downButton.onClick.AddListener(new UnityAction(this.ScrollDownOne));
	}

	// Token: 0x060063C7 RID: 25543 RVA: 0x002370BE File Offset: 0x002354BE
	private void ShowSubScreens()
	{
		this.tbController.ShowCurrentSubScreenTabSubScreen();
	}

	// Token: 0x060063C8 RID: 25544 RVA: 0x002370CB File Offset: 0x002354CB
	private void HideSubScreens()
	{
		if (this.subPages.Count > 0)
		{
			VRCUiManager.Instance.HideScreen(this.subPages[0].screenType);
		}
	}

	// Token: 0x060063C9 RID: 25545 RVA: 0x002370F9 File Offset: 0x002354F9
	private void ScrollUpOne()
	{
		this.tbController.ScrollUpOne();
	}

	// Token: 0x060063CA RID: 25546 RVA: 0x00237106 File Offset: 0x00235506
	private void ScrollDownOne()
	{
		this.tbController.ScrollDownOne();
	}

	// Token: 0x04004914 RID: 18708
	protected List<VRCUiPage> subPages;

	// Token: 0x04004915 RID: 18709
	public Button upButton;

	// Token: 0x04004916 RID: 18710
	public Button downButton;

	// Token: 0x04004917 RID: 18711
	public string defaultSubScreenTab;

	// Token: 0x04004918 RID: 18712
	private SubScreenTableViewController tbController;
}
