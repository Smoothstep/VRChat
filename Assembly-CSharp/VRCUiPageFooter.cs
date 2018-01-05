using System;
using UnityEngine;
using UnityEngine.Events;
using VRC;

// Token: 0x02000C76 RID: 3190
public class VRCUiPageFooter : VRCUiPageTabGroup
{
	// Token: 0x06006310 RID: 25360 RVA: 0x00233F3C File Offset: 0x0023233C
	public override void SetupTabs()
	{
		VRCUiPageTabManager instance = VRCUiPageTabManager.Instance;
		base.SetCenterOffset(400f);
		instance.CreateTab("CONSOLE", "UserInterface/MenuContent/Screens/Console", VRCUiPageTabManager.TabContext.InRoom, this, null);
		instance.CreateTab("LOGOUT", null, VRCUiPageTabManager.TabContext.InMainMenu, this, new UnityAction(this.Logout));
		instance.CreateTab("EXIT VRCHAT", null, VRCUiPageTabManager.TabContext.Everywhere, this, new UnityAction(this.Exit));
	}

	// Token: 0x06006311 RID: 25361 RVA: 0x00233FA4 File Offset: 0x002323A4
	public override float GetYPosition()
	{
		return -460f;
	}

	// Token: 0x06006312 RID: 25362 RVA: 0x00233FAB File Offset: 0x002323AB
	public override float GetWidth()
	{
		return 1470f;
	}

	// Token: 0x06006313 RID: 25363 RVA: 0x00233FB2 File Offset: 0x002323B2
	public void Logout()
	{
		User.Logout();
	}

	// Token: 0x06006314 RID: 25364 RVA: 0x00233FB9 File Offset: 0x002323B9
	public void Exit()
	{
		Application.Quit();
	}
}
