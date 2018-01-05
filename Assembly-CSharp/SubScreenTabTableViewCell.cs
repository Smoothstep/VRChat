using System;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C3A RID: 3130
public class SubScreenTabTableViewCell : TableViewCell
{
	// Token: 0x06006134 RID: 24884 RVA: 0x0022486F File Offset: 0x00222C6F
	public void SetupCell(VRCUiPage subPage, int defaultFontSize = 39, Color defaultColor = default(Color), int selectedFontSize = 60, Color selectedColor = default(Color))
	{
		this.mSelectedColor = selectedColor;
		this.mSelectedFontSize = selectedFontSize;
		this.name.text = subPage.displayName;
		this.page = subPage;
	}

	// Token: 0x06006135 RID: 24885 RVA: 0x00224899 File Offset: 0x00222C99
	public void RefreshAsSelected()
	{
		this.name.color = this.mSelectedColor;
		this.name.fontSize = this.mSelectedFontSize;
	}

	// Token: 0x06006136 RID: 24886 RVA: 0x002248BD File Offset: 0x00222CBD
	public void RefreshAsNotSelected()
	{
		this.name.color = this.mDefaultColor;
		this.name.fontSize = this.mDefaultFontSize;
	}

	// Token: 0x06006137 RID: 24887 RVA: 0x002248E1 File Offset: 0x00222CE1
	public void ShowSubPage()
	{
		VRCUiManager.Instance.ShowScreen(this.page.transform.GetHierarchyPath());
	}

	// Token: 0x040046D4 RID: 18132
	public new Text name;

	// Token: 0x040046D5 RID: 18133
	private VRCUiPage page;

	// Token: 0x040046D6 RID: 18134
	private Color mSelectedColor = Color.white;

	// Token: 0x040046D7 RID: 18135
	private int mSelectedFontSize = 60;

	// Token: 0x040046D8 RID: 18136
	private Color mDefaultColor = Color.white;

	// Token: 0x040046D9 RID: 18137
	private int mDefaultFontSize = 39;
}
