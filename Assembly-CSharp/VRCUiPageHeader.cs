using System;
using UnityEngine.Events;

// Token: 0x02000C77 RID: 3191
public class VRCUiPageHeader : VRCUiPage
{
	// Token: 0x06006316 RID: 25366 RVA: 0x00233FC8 File Offset: 0x002323C8
	public override void Start()
	{
		VRCUiManager.Instance.onPageShown += this.EnableSearchIfNeeded;
		VRCUiManager.Instance.onUiEnabled += this.ResetSearch;
		VRCUiPage activeScreen = VRCUiManager.Instance.GetActiveScreen("SCREEN");
		this.EnableSearchIfNeeded(activeScreen);
	}

	// Token: 0x06006317 RID: 25367 RVA: 0x00234018 File Offset: 0x00232418
	private void ResetSearch()
	{
		VRCUiPage activeScreen = VRCUiManager.Instance.GetActiveScreen("SCREEN");
		this.searchBar.text = string.Empty;
		if (activeScreen != null && activeScreen is VRCSearchableUiPage)
		{
			((VRCSearchableUiPage)activeScreen).HideSearch();
		}
	}

	// Token: 0x06006318 RID: 25368 RVA: 0x00234067 File Offset: 0x00232467
	public void EnableSearchIfNeeded(VRCUiPage page)
	{
		if (page.screenType != "SCREEN")
		{
			return;
		}
		this.ResetSearch();
		if (page is VRCSearchableUiPage)
		{
			this.EnableSearch(page as VRCSearchableUiPage);
		}
		else
		{
			this.DisableSearch();
		}
	}

	// Token: 0x06006319 RID: 25369 RVA: 0x002340A7 File Offset: 0x002324A7
	private void EnableSearch(VRCSearchableUiPage searchable)
	{
		this.searchBar.editButton.interactable = true;
		this.searchBar.onDoneInputting = new UnityAction<string>(searchable.OnSearch);
	}

	// Token: 0x0600631A RID: 25370 RVA: 0x002340D1 File Offset: 0x002324D1
	private void DisableSearch()
	{
		this.searchBar.editButton.interactable = false;
		this.searchBar.onDoneInputting = null;
	}

	// Token: 0x0400489B RID: 18587
	public UiInputField searchBar;
}
