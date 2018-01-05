using System;
using UnityEngine;

// Token: 0x02000AED RID: 2797
public class SessionManager : MonoBehaviour
{
	// Token: 0x060054BC RID: 21692 RVA: 0x001D3CCC File Offset: 0x001D20CC
	private void Awake()
	{
		SessionManager._StartTime = Time.time;
		if (PlayerPrefs.HasKey("UserId"))
		{
			SessionManager._UserId = PlayerPrefs.GetString("UserId");
		}
		else
		{
			SessionManager._UserId = SessionManager.GetRandomDigits(8);
			PlayerPrefs.SetString("UserId", SessionManager._UserId);
		}
		SessionManager._SessionId = SessionManager.GetRandomDigits(8);
	}

	// Token: 0x060054BD RID: 21693 RVA: 0x001D3D2C File Offset: 0x001D212C
	public static string GetRandomDigits(int digits)
	{
		string text = string.Empty;
		for (int i = 0; i < digits; i++)
		{
			text += UnityEngine.Random.Range(0, 9).ToString();
		}
		return text;
	}

	// Token: 0x060054BE RID: 21694 RVA: 0x001D3D6F File Offset: 0x001D216F
	public static string GetSessionId()
	{
		return SessionManager._SessionId;
	}

	// Token: 0x060054BF RID: 21695 RVA: 0x001D3D76 File Offset: 0x001D2176
	public static string GetUserId()
	{
		return SessionManager._UserId;
	}

	// Token: 0x060054C0 RID: 21696 RVA: 0x001D3D7D File Offset: 0x001D217D
	public static float GetSessionTime()
	{
		return SessionManager._StartTime;
	}

	// Token: 0x04003BCE RID: 15310
	private static float _StartTime;

	// Token: 0x04003BCF RID: 15311
	private static string _UserId;

	// Token: 0x04003BD0 RID: 15312
	private static string _SessionId;

	// Token: 0x04003BD1 RID: 15313
	public const string K_USER_ID = "UserId";
}
