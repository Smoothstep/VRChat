using System;
using UnityEngine;

namespace ExitGames.UtilityScripts
{
	// Token: 0x02000794 RID: 1940
	public static class PlayerRoomIndexingExtensions
	{
		// Token: 0x06003EE8 RID: 16104 RVA: 0x0013D38A File Offset: 0x0013B78A
		public static int GetRoomIndex(this PhotonPlayer player)
		{
			if (PlayerRoomIndexing.instance == null)
			{
				Debug.LogError("Missing PlayerRoomIndexing Component in Scene");
				return -1;
			}
			return PlayerRoomIndexing.instance.GetRoomIndex(player);
		}
	}
}
