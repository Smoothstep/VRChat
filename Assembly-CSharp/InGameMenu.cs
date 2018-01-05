using System;
using UnityEngine;
using UnityEngine.VR;

// Token: 0x02000A99 RID: 2713
public class InGameMenu : MonoBehaviour
{
	// Token: 0x06005196 RID: 20886 RVA: 0x001BEF80 File Offset: 0x001BD380
	private void Awake()
	{
		this.inGameMenu = base.transform.Find("InGameMenu").gameObject;
		this.inGameMenu.SetActive(false);
	}

	// Token: 0x06005197 RID: 20887 RVA: 0x001BEFA9 File Offset: 0x001BD3A9
	private void Start()
	{
		this.inMenu = VRCInputManager.FindInput("Select");
	}

	// Token: 0x06005198 RID: 20888 RVA: 0x001BEFBB File Offset: 0x001BD3BB
	private void Update()
	{
		if (this.inMenu.down)
		{
			if (this.inGameMenu.activeSelf)
			{
				this.ReturnToMenu();
			}
			else
			{
				this.OpenMenu();
			}
		}
	}

	// Token: 0x06005199 RID: 20889 RVA: 0x001BEFEE File Offset: 0x001BD3EE
	private void OpenMenu()
	{
		this.inGameMenu.SetActive(true);
	}

	// Token: 0x0600519A RID: 20890 RVA: 0x001BEFFC File Offset: 0x001BD3FC
	private void CloseMenu()
	{
		this.inGameMenu.SetActive(false);
	}

	// Token: 0x0600519B RID: 20891 RVA: 0x001BF00A File Offset: 0x001BD40A
	public void ReturnToMenu()
	{
		Debug.LogError("Shouldn't go here");
		RoomManager.LeaveRoom();
	}

	// Token: 0x0600519C RID: 20892 RVA: 0x001BF01B File Offset: 0x001BD41B
	public void ReorientCamera()
	{
		InputTracking.Recenter();
	}

	// Token: 0x0600519D RID: 20893 RVA: 0x001BF022 File Offset: 0x001BD422
	public void Respawn()
	{
		SpawnManager.Instance.RespawnPlayerUsingOrder(VRCPlayer.Instance);
	}

	// Token: 0x040039E7 RID: 14823
	private GameObject inGameMenu;

	// Token: 0x040039E8 RID: 14824
	private VRCInput inMenu;
}
