using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000ACF RID: 2767
public class PlayerModManager : MonoBehaviour
{
	// Token: 0x0600540A RID: 21514 RVA: 0x001D0565 File Offset: 0x001CE965
	private void Awake()
	{
		this.mCurrentMods = new List<VRCPlayerMod>();
	}

	// Token: 0x0600540B RID: 21515 RVA: 0x001D0574 File Offset: 0x001CE974
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => PlayerMods.ActivePlayerMods.Count > 0);
		this.mRoomMods = this.FindRoomMods();
		this.AddModsFromRoom();
		yield break;
	}

	// Token: 0x0600540C RID: 21516 RVA: 0x001D0590 File Offset: 0x001CE990
	private List<VRCPlayerMod> FindRoomMods()
	{
		List<VRCPlayerMod> result = null;
		IEnumerable<PlayerMods> activePlayerMods = PlayerMods.ActivePlayerMods;
		foreach (PlayerMods playerMods in activePlayerMods)
		{
			if (playerMods.isRoomPlayerMods)
			{
				result = playerMods.playerMods;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600540D RID: 21517 RVA: 0x001D0600 File Offset: 0x001CEA00
	public void AddModsFromRoom()
	{
		this.AddMods(this.mRoomMods);
	}

	// Token: 0x0600540E RID: 21518 RVA: 0x001D0610 File Offset: 0x001CEA10
	public void AddMods(IEnumerable<VRCPlayerMod> mods)
	{
		if (mods != null)
		{
			foreach (VRCPlayerMod mod in mods)
			{
				this.AddMod(mod);
			}
		}
	}

	// Token: 0x0600540F RID: 21519 RVA: 0x001D066C File Offset: 0x001CEA6C
	public void RemoveMods()
	{
		List<VRCPlayerMod> list = new List<VRCPlayerMod>(this.mCurrentMods);
		foreach (VRCPlayerMod mod in list)
		{
			this.RemoveMod(mod);
		}
	}

	// Token: 0x06005410 RID: 21520 RVA: 0x001D06D0 File Offset: 0x001CEAD0
	private void AddMod(VRCPlayerMod mod)
	{
		this.mCurrentMods.RemoveAll((VRCPlayerMod m) => m.name == mod.name);
		this.mCurrentMods.Add(mod);
		mod.AddOrUpdateModComponentOn(base.gameObject);
	}

	// Token: 0x06005411 RID: 21521 RVA: 0x001D0728 File Offset: 0x001CEB28
	private void RemoveMod(VRCPlayerMod mod)
	{
		if (this.mCurrentMods.Exists((VRCPlayerMod m) => m.name == mod.name))
		{
			VRCPlayerMod vrcplayerMod = null;
			if (this.mRoomMods != null)
			{
				vrcplayerMod = this.mRoomMods.Find((VRCPlayerMod x) => x.name == mod.name);
			}
			if (vrcplayerMod != null)
			{
				vrcplayerMod.AddOrUpdateModComponentOn(base.gameObject);
			}
			else
			{
				Component component = base.gameObject.GetComponent(mod.modComponentName);
				UnityEngine.Object.Destroy(component);
			}
			this.mCurrentMods.Remove(mod);
		}
	}

	// Token: 0x06005412 RID: 21522 RVA: 0x001D07CC File Offset: 0x001CEBCC
	public void Reset()
	{
		foreach (VRCPlayerMod vrcplayerMod in this.mCurrentMods)
		{
			vrcplayerMod.AddOrUpdateModComponentOn(base.gameObject);
		}
	}

	// Token: 0x04003B56 RID: 15190
	public List<VRCPlayerMod> mCurrentMods = new List<VRCPlayerMod>();

	// Token: 0x04003B57 RID: 15191
	public List<VRCPlayerMod> mRoomMods = new List<VRCPlayerMod>();
}
