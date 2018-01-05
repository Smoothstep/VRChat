using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A79 RID: 2681
public class DisableOtherUIInteraction : MonoBehaviour
{
	// Token: 0x060050D8 RID: 20696 RVA: 0x001BA29C File Offset: 0x001B869C
	private void Awake()
	{
		this.selectables = UnityEngine.Object.FindObjectsOfType<Selectable>();
	}

	// Token: 0x060050D9 RID: 20697 RVA: 0x001BA2A9 File Offset: 0x001B86A9
	private void OnEnable()
	{
		if (this.disableOthersOnEnable)
		{
			this.DisableOthers();
		}
	}

	// Token: 0x060050DA RID: 20698 RVA: 0x001BA2BC File Offset: 0x001B86BC
	private void OnDisable()
	{
		if (this.disableOthersOnEnable)
		{
			this.EnableOthers();
		}
	}

	// Token: 0x060050DB RID: 20699 RVA: 0x001BA2D0 File Offset: 0x001B86D0
	public void EnableOthers()
	{
		foreach (Selectable selectable in this.selectables)
		{
			if (selectable.transform.root.name != base.transform.root.name)
			{
				selectable.interactable = true;
			}
		}
	}

	// Token: 0x060050DC RID: 20700 RVA: 0x001BA330 File Offset: 0x001B8730
	public void DisableOthers()
	{
		foreach (Selectable selectable in this.selectables)
		{
			if (selectable.transform.root.name != base.transform.root.name)
			{
				selectable.interactable = false;
			}
		}
	}

	// Token: 0x0400395E RID: 14686
	public bool disableOthersOnEnable = true;

	// Token: 0x0400395F RID: 14687
	private Selectable[] selectables;
}
