using System;
using UnityEngine;

// Token: 0x02000C90 RID: 3216
public class VRCWorldPlayerUiProfile : MonoBehaviour
{
	// Token: 0x060063D2 RID: 25554 RVA: 0x0023725C File Offset: 0x0023565C
	public void ShowPlayerMenu()
	{
		this.playerInteractiveUI.SetActive(!this.playerInteractiveUI.activeSelf);
	}

	// Token: 0x0400491D RID: 18717
	public GameObject playerInteractiveUI;
}
