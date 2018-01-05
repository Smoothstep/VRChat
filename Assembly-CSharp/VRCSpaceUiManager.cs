using System;
using UnityEngine;

// Token: 0x02000C5F RID: 3167
public class VRCSpaceUiManager : VRCUiManager
{
	// Token: 0x06006259 RID: 25177 RVA: 0x00231257 File Offset: 0x0022F657
	public void ButtonDownloadUpdate()
	{
		VRCApplicationSetup.DownloadUpdate();
		Application.Quit();
	}

	// Token: 0x0600625A RID: 25178 RVA: 0x00231263 File Offset: 0x0022F663
	public void SetScreenName(string s)
	{
		Debug.LogError("Set Screen Name is depricated.");
	}

	// Token: 0x0600625B RID: 25179 RVA: 0x0023126F File Offset: 0x0022F66F
	public void CompleteFirstTimeSettings()
	{
		base.HideScreen("SCREEN");
	}

	// Token: 0x0600625C RID: 25180 RVA: 0x0023127C File Offset: 0x0022F67C
	public void QueueTransitionToTutorial()
	{
		VRCFlowManager.Instance.GoTutorial();
	}
}
