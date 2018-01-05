using System;
using UnityEngine;
using VRC;

// Token: 0x02000C5C RID: 3164
public class VRCSimpleUiPageFooter : MonoBehaviour
{
	// Token: 0x06006246 RID: 25158 RVA: 0x0022E993 File Offset: 0x0022CD93
	public void Logout()
	{
		User.Logout();
	}

	// Token: 0x06006247 RID: 25159 RVA: 0x0022E99A File Offset: 0x0022CD9A
	public void Exit()
	{
		Application.Quit();
	}
}
