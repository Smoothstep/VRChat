using System;
using UnityEngine;

// Token: 0x02000B46 RID: 2886
public class VRCPlayerPrefs
{
	// Token: 0x06005887 RID: 22663 RVA: 0x001EAC7C File Offset: 0x001E907C
	public static void DeleteAllExceptPersistant()
	{
		bool flag = PlayerPrefs.HasKey("UserId");
		string value = string.Empty;
		if (flag)
		{
			value = PlayerPrefs.GetString("UserId");
		}
		PlayerPrefs.DeleteAll();
		if (flag)
		{
			PlayerPrefs.SetString("UserId", value);
		}
	}
}
