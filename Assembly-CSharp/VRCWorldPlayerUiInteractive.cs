using System;
using UnityEngine;

// Token: 0x02000C8F RID: 3215
public class VRCWorldPlayerUiInteractive : MonoBehaviour
{
	// Token: 0x060063CF RID: 25551 RVA: 0x00237237 File Offset: 0x00235637
	public void CloseClicked()
	{
		base.gameObject.SetActive(!base.gameObject.activeSelf);
	}

	// Token: 0x060063D0 RID: 25552 RVA: 0x00237252 File Offset: 0x00235652
	public void Mute()
	{
	}
}
