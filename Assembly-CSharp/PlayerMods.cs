using System;
using System.Collections.Generic;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000AD0 RID: 2768
public class PlayerMods : MonoBehaviour
{
	// Token: 0x06005414 RID: 21524 RVA: 0x001D0964 File Offset: 0x001CED64
	private void Start()
	{
		PlayerMods.ActivePlayerMods.Add(this);
		Collider component = base.gameObject.GetComponent<Collider>();
		if (component == null && !this.isRoomPlayerMods)
		{
			BoxCollider boxCollider = base.gameObject.AddComponent<BoxCollider>();
			boxCollider.isTrigger = true;
		}
	}

	// Token: 0x06005415 RID: 21525 RVA: 0x001D09B3 File Offset: 0x001CEDB3
	private void OnDestroy()
	{
		PlayerMods.ActivePlayerMods.Remove(this);
	}

	// Token: 0x06005416 RID: 21526 RVA: 0x001D09C4 File Offset: 0x001CEDC4
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{

	})]
	private void AddPlayerMods(int playerInstigatorId)
	{
		PhotonView photonView = PhotonView.Find(playerInstigatorId);
		if (photonView != null)
		{
			GameObject gameObject = photonView.gameObject;
			if (gameObject != null)
			{
				PlayerModManager component = gameObject.GetComponent<PlayerModManager>();
				component.AddMods(this.playerMods);
			}
		}
	}

	// Token: 0x06005417 RID: 21527 RVA: 0x001D0A0C File Offset: 0x001CEE0C
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{

	})]
	private void RemovePlayerMods(int playerInstigatorId)
	{
		PhotonView photonView = PhotonView.Find(playerInstigatorId);
		if (photonView != null)
		{
			GameObject gameObject = photonView.gameObject;
			if (gameObject != null)
			{
				PlayerModManager component = gameObject.GetComponent<PlayerModManager>();
				component.RemoveMods();
				component.AddModsFromRoom();
			}
		}
	}

	// Token: 0x04003B58 RID: 15192
	public bool isRoomPlayerMods;

	// Token: 0x04003B59 RID: 15193
	public List<VRCPlayerMod> playerMods;

	// Token: 0x04003B5A RID: 15194
	public static HashSet<PlayerMods> ActivePlayerMods = new HashSet<PlayerMods>();
}
