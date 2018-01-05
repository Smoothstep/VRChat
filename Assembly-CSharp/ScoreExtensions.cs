using System;
using ExitGames.Client.Photon;

// Token: 0x0200079A RID: 1946
public static class ScoreExtensions
{
	// Token: 0x06003F05 RID: 16133 RVA: 0x0013DD08 File Offset: 0x0013C108
	public static void SetScore(this PhotonPlayer player, int newScore)
	{
		Hashtable hashtable = new Hashtable();
		hashtable["score"] = newScore;
		player.SetCustomProperties(hashtable, null, false);
	}

	// Token: 0x06003F06 RID: 16134 RVA: 0x0013DD38 File Offset: 0x0013C138
	public static void AddScore(this PhotonPlayer player, int scoreToAddToCurrent)
	{
		int num = player.GetScore();
		num += scoreToAddToCurrent;
		Hashtable hashtable = new Hashtable();
		hashtable["score"] = num;
		player.SetCustomProperties(hashtable, null, false);
	}

	// Token: 0x06003F07 RID: 16135 RVA: 0x0013DD70 File Offset: 0x0013C170
	public static int GetScore(this PhotonPlayer player)
	{
		object obj;
		if (player.CustomProperties.TryGetValue("score", out obj))
		{
			return (int)obj;
		}
		return 0;
	}
}
