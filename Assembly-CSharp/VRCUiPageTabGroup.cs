using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C7E RID: 3198
public abstract class VRCUiPageTabGroup : VRCUiPage
{
	// Token: 0x0600633C RID: 25404 RVA: 0x00233D95 File Offset: 0x00232195
	public override void Start()
	{
		base.Start();
		this.mTabs = new List<VRCUiPageTab>();
		this.SetupTabs();
		VRCUiPageTabManager.Instance.UpdateTabsForCurrentContext();
	}

	// Token: 0x0600633D RID: 25405 RVA: 0x00233DB8 File Offset: 0x002321B8
	public void AddTab(VRCUiPageTab tab)
	{
		tab.transform.SetParent(base.transform);
		this.mTabs.Add(tab);
	}

	// Token: 0x0600633E RID: 25406 RVA: 0x00233DD7 File Offset: 0x002321D7
	public void SetCenterOffset(float offsetWidth)
	{
		this.mCenterOffsetWidth = offsetWidth;
		this.RefreshLayout();
	}

	// Token: 0x0600633F RID: 25407 RVA: 0x00233DE8 File Offset: 0x002321E8
	public void RefreshLayout()
	{
		if (this.mTabs != null)
		{
			List<VRCUiPageTab> list = new List<VRCUiPageTab>();
			foreach (VRCUiPageTab vrcuiPageTab in this.mTabs)
			{
				if (vrcuiPageTab.gameObject.activeSelf)
				{
					list.Add(vrcuiPageTab);
				}
			}
			float num = 350f;
			float yposition = this.GetYPosition();
			for (int i = 0; i < list.Count; i++)
			{
				Vector3 position = list[i].transform.position;
				position.x = ((float)(-(float)(list.Count - 1)) * 0.5f + (float)i) * num;
				if (position.x > 0f)
				{
					position.x += this.mCenterOffsetWidth / 2f;
				}
				else
				{
					position.x -= this.mCenterOffsetWidth / 2f;
				}
				position.y = yposition;
				position.z = -1.5f;
				list[i].transform.localPosition = position;
			}
		}
	}

	// Token: 0x06006340 RID: 25408
	public abstract void SetupTabs();

	// Token: 0x06006341 RID: 25409
	public abstract float GetYPosition();

	// Token: 0x06006342 RID: 25410
	public abstract float GetWidth();

	// Token: 0x040048AC RID: 18604
	private List<VRCUiPageTab> mTabs;

	// Token: 0x040048AD RID: 18605
	private float mCenterOffsetWidth;
}
