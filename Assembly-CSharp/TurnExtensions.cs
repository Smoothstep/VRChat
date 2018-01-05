using System;
using ExitGames.Client.Photon;

// Token: 0x020007A0 RID: 1952
public static class TurnExtensions
{
	// Token: 0x06003F27 RID: 16167 RVA: 0x0013E298 File Offset: 0x0013C698
	public static void SetTurn(this Room room, int turn, bool setStartTime = false)
	{
		if (room == null || room.CustomProperties == null)
		{
			return;
		}
		Hashtable hashtable = new Hashtable();
		hashtable[TurnExtensions.TurnPropKey] = turn;
		if (setStartTime)
		{
			hashtable[TurnExtensions.TurnStartPropKey] = PhotonNetwork.ServerTimestamp;
		}
		room.SetCustomProperties(hashtable, null, false);
	}

	// Token: 0x06003F28 RID: 16168 RVA: 0x0013E2F2 File Offset: 0x0013C6F2
	public static int GetTurn(this RoomInfo room)
	{
		if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(TurnExtensions.TurnPropKey))
		{
			return 0;
		}
		return (int)room.CustomProperties[TurnExtensions.TurnPropKey];
	}

	// Token: 0x06003F29 RID: 16169 RVA: 0x0013E331 File Offset: 0x0013C731
	public static int GetTurnStart(this RoomInfo room)
	{
		if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(TurnExtensions.TurnStartPropKey))
		{
			return 0;
		}
		return (int)room.CustomProperties[TurnExtensions.TurnStartPropKey];
	}

	// Token: 0x06003F2A RID: 16170 RVA: 0x0013E370 File Offset: 0x0013C770
	public static int GetFinishedTurn(this PhotonPlayer player)
	{
		Room room = PhotonNetwork.room;
		if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(TurnExtensions.TurnPropKey))
		{
			return 0;
		}
		string key = TurnExtensions.FinishedTurnPropKey + player.ID;
		return (int)room.CustomProperties[key];
	}

	// Token: 0x06003F2B RID: 16171 RVA: 0x0013E3D4 File Offset: 0x0013C7D4
	public static void SetFinishedTurn(this PhotonPlayer player, int turn)
	{
		Room room = PhotonNetwork.room;
		if (room == null || room.CustomProperties == null)
		{
			return;
		}
		string key = TurnExtensions.FinishedTurnPropKey + player.ID;
		Hashtable hashtable = new Hashtable();
		hashtable[key] = turn;
		room.SetCustomProperties(hashtable, null, false);
	}

	// Token: 0x04002797 RID: 10135
	public static readonly string TurnPropKey = "Turn";

	// Token: 0x04002798 RID: 10136
	public static readonly string TurnStartPropKey = "TStart";

	// Token: 0x04002799 RID: 10137
	public static readonly string FinishedTurnPropKey = "FToA";
}
