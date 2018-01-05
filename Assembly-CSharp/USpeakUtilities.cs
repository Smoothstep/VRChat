using System;
using System.Collections.Generic;
using UnityEngine;
using VRC.Core;

// Token: 0x020009E3 RID: 2531
public class USpeakUtilities
{
	// Token: 0x06004CE8 RID: 19688 RVA: 0x0019C064 File Offset: 0x0019A464
	public static void PlayerJoined(string PlayerID)
	{
		GameObject gameObject = (GameObject)AssetManagement.Instantiate(Resources.Load("USpeakerPrefab"));
		USpeakOwnerInfo uspeakOwnerInfo = gameObject.AddComponent<USpeakOwnerInfo>();
		uspeakOwnerInfo.Init(new USpeakPlayer
		{
			PlayerID = PlayerID
		});
	}

	// Token: 0x06004CE9 RID: 19689 RVA: 0x0019C0A1 File Offset: 0x0019A4A1
	public static void PlayerLeft(string PlayerID)
	{
		USpeakOwnerInfo.FindPlayerByID(PlayerID).DeInit();
	}

	// Token: 0x06004CEA RID: 19690 RVA: 0x0019C0B0 File Offset: 0x0019A4B0
	public static void ListPlayers(IEnumerable<string> PlayerIDs)
	{
		foreach (string playerID in PlayerIDs)
		{
			USpeakUtilities.PlayerJoined(playerID);
		}
	}

	// Token: 0x06004CEB RID: 19691 RVA: 0x0019C104 File Offset: 0x0019A504
	public static void Clear()
	{
		foreach (string key in USpeakOwnerInfo.USpeakPlayerMap.Keys)
		{
			USpeakOwnerInfo.USpeakPlayerMap[key].DeInit();
		}
	}

	// Token: 0x040034EE RID: 13550
	public static string USpeakerPrefabPath = "USpeakerPrefab";
}
