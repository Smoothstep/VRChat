using System;
using UnityEngine;

// Token: 0x02000C7D RID: 3197
[RequireComponent(typeof(stdButton))]
public class VRCUiPageTab : MonoBehaviour
{
	// Token: 0x06006339 RID: 25401 RVA: 0x002347BB File Offset: 0x00232BBB
	public void Fill(string title, string screenPath, VRCUiPageTabManager.TabContext context)
	{
	}

	// Token: 0x0600633A RID: 25402 RVA: 0x002347BD File Offset: 0x00232BBD
	public void ShowPage()
	{
		if (!string.IsNullOrEmpty(this.screen))
		{
			VRCUiManager.Instance.ShowScreen(this.screen);
		}
	}

	// Token: 0x040048A9 RID: 18601
	[HideInInspector]
	public string buttonTitle;

	// Token: 0x040048AA RID: 18602
	public string screen;

	// Token: 0x040048AB RID: 18603
	[HideInInspector]
	public VRCUiPageTabManager.TabContext enabledContext;
}
