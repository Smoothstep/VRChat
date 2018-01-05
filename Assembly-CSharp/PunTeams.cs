using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200079B RID: 1947
public class PunTeams : MonoBehaviour
{
	// Token: 0x06003F09 RID: 16137 RVA: 0x0013DDA4 File Offset: 0x0013C1A4
	public void Start()
	{
		PunTeams.PlayersPerTeam = new Dictionary<PunTeams.Team, List<PhotonPlayer>>();
		Array values = Enum.GetValues(typeof(PunTeams.Team));
		IEnumerator enumerator = values.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				PunTeams.PlayersPerTeam[(PunTeams.Team)obj] = new List<PhotonPlayer>();
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06003F0A RID: 16138 RVA: 0x0013DE28 File Offset: 0x0013C228
	public void OnDisable()
	{
		PunTeams.PlayersPerTeam = new Dictionary<PunTeams.Team, List<PhotonPlayer>>();
	}

	// Token: 0x06003F0B RID: 16139 RVA: 0x0013DE34 File Offset: 0x0013C234
	public void OnJoinedRoom()
	{
		this.UpdateTeams();
	}

	// Token: 0x06003F0C RID: 16140 RVA: 0x0013DE3C File Offset: 0x0013C23C
	public void OnLeftRoom()
	{
		this.Start();
	}

	// Token: 0x06003F0D RID: 16141 RVA: 0x0013DE44 File Offset: 0x0013C244
	public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
	{
		this.UpdateTeams();
	}

	// Token: 0x06003F0E RID: 16142 RVA: 0x0013DE4C File Offset: 0x0013C24C
	public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		this.UpdateTeams();
	}

	// Token: 0x06003F0F RID: 16143 RVA: 0x0013DE54 File Offset: 0x0013C254
	public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		this.UpdateTeams();
	}

	// Token: 0x06003F10 RID: 16144 RVA: 0x0013DE5C File Offset: 0x0013C25C
	public void UpdateTeams()
	{
		Array values = Enum.GetValues(typeof(PunTeams.Team));
		IEnumerator enumerator = values.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				PunTeams.PlayersPerTeam[(PunTeams.Team)obj].Clear();
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
		{
			PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
			PunTeams.Team team = photonPlayer.GetTeam();
			PunTeams.PlayersPerTeam[team].Add(photonPlayer);
		}
	}

	// Token: 0x0400278A RID: 10122
	public static Dictionary<PunTeams.Team, List<PhotonPlayer>> PlayersPerTeam;

	// Token: 0x0400278B RID: 10123
	public const string TeamPlayerProp = "team";

	// Token: 0x0200079C RID: 1948
	public enum Team : byte
	{
		// Token: 0x0400278D RID: 10125
		none,
		// Token: 0x0400278E RID: 10126
		red,
		// Token: 0x0400278F RID: 10127
		blue
	}
}
