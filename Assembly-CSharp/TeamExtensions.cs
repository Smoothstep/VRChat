using System;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x0200079D RID: 1949
public static class TeamExtensions
{
	// Token: 0x06003F11 RID: 16145 RVA: 0x0013DF18 File Offset: 0x0013C318
	public static PunTeams.Team GetTeam(this PhotonPlayer player)
	{
		object obj;
		if (player.CustomProperties.TryGetValue("team", out obj))
		{
			return (PunTeams.Team)obj;
		}
		return PunTeams.Team.none;
	}

	// Token: 0x06003F12 RID: 16146 RVA: 0x0013DF44 File Offset: 0x0013C344
	public static void SetTeam(this PhotonPlayer player, PunTeams.Team team)
	{
		if (!PhotonNetwork.connectedAndReady)
		{
			Debug.LogWarning("JoinTeam was called in state: " + PhotonNetwork.connectionStateDetailed + ". Not connectedAndReady.");
			return;
		}
		PunTeams.Team team2 = player.GetTeam();
		if (team2 != team)
		{
			player.SetCustomProperties(new Hashtable
			{
				{
					"team",
					(byte)team
				}
			}, null, false);
		}
	}
}
