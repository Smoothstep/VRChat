using System;
using UnityEngine;

// Token: 0x02000AFD RID: 2813
public class Storage : MonoBehaviour
{
	// Token: 0x06005516 RID: 21782 RVA: 0x001D5494 File Offset: 0x001D3894
	public static void Write(string key, object value)
	{
		Type type = value.GetType();
		if (type == typeof(float))
		{
			PlayerPrefs.SetFloat(key, (float)value);
		}
		else if (type == typeof(int))
		{
			PlayerPrefs.SetInt(key, (int)value);
		}
		else if (type == typeof(string))
		{
			PlayerPrefs.SetString(key, (string)value);
		}
		else if (type == typeof(bool))
		{
			PlayerPrefs.SetInt(key, (!(bool)value) ? 0 : 1);
		}
		else
		{
			Debug.LogError("PlayerPref don't support type " + type);
		}
	}

	// Token: 0x06005517 RID: 21783 RVA: 0x001D5548 File Offset: 0x001D3948
	public static object Read(string key, Type type, object defaultValue = null)
	{
		object result = defaultValue;
		if (PlayerPrefs.HasKey(key))
		{
			if (type == typeof(float))
			{
				result = PlayerPrefs.GetFloat(key);
			}
			else if (type == typeof(int))
			{
				result = PlayerPrefs.GetInt(key);
			}
			else if (type == typeof(string))
			{
				result = PlayerPrefs.GetString(key);
			}
			else if (type == typeof(bool))
			{
				result = (PlayerPrefs.GetInt(key) == 1);
			}
			else
			{
				Debug.LogError("PlayerPref don't support type " + type);
			}
		}
		else if (defaultValue != null)
		{
			if (type == typeof(float))
			{
				PlayerPrefs.SetFloat(key, (float)defaultValue);
			}
			else if (type == typeof(int))
			{
				PlayerPrefs.SetInt(key, (int)defaultValue);
			}
			else if (type == typeof(string))
			{
				PlayerPrefs.SetString(key, (string)defaultValue);
			}
			else if (type == typeof(bool))
			{
				PlayerPrefs.SetInt(key, (!(bool)defaultValue) ? 0 : 1);
			}
			else
			{
				Debug.LogError("PlayerPref don't support type " + type);
			}
		}
		return result;
	}

	// Token: 0x06005518 RID: 21784 RVA: 0x001D56AA File Offset: 0x001D3AAA
	public static void SaveNow()
	{
		PlayerPrefs.Save();
	}
}
