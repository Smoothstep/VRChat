using System;
using System.Globalization;
using Sabresaurus.PlayerPrefsExtensions;
using UnityEngine;

// Token: 0x020009FA RID: 2554
public static class PlayerPrefsUtility
{
	// Token: 0x06004DAC RID: 19884 RVA: 0x001A0EF4 File Offset: 0x0019F2F4
	public static bool IsEncryptedKey(string key)
	{
		return key.StartsWith("ENC-");
	}

	// Token: 0x06004DAD RID: 19885 RVA: 0x001A0F0C File Offset: 0x0019F30C
	public static string DecryptKey(string encryptedKey)
	{
		if (encryptedKey.StartsWith("ENC-"))
		{
			string sourceString = encryptedKey.Substring("ENC-".Length);
			return SimpleEncryption.DecryptString(sourceString);
		}
		throw new InvalidOperationException("Could not decrypt item, no match found in known encrypted key prefixes");
	}

	// Token: 0x06004DAE RID: 19886 RVA: 0x001A0F4C File Offset: 0x0019F34C
	public static void SetEncryptedFloat(string key, float value)
	{
		string str = SimpleEncryption.EncryptString(key);
		string str2 = SimpleEncryption.EncryptFloat(value);
		PlayerPrefs.SetString("ENC-" + str, "0" + str2);
	}

	// Token: 0x06004DAF RID: 19887 RVA: 0x001A0F84 File Offset: 0x0019F384
	public static void SetEncryptedInt(string key, int value)
	{
		string str = SimpleEncryption.EncryptString(key);
		string str2 = SimpleEncryption.EncryptInt(value);
		PlayerPrefs.SetString("ENC-" + str, "1" + str2);
	}

	// Token: 0x06004DB0 RID: 19888 RVA: 0x001A0FBC File Offset: 0x0019F3BC
	public static void SetEncryptedString(string key, string value)
	{
		string str = SimpleEncryption.EncryptString(key);
		string str2 = SimpleEncryption.EncryptString(value);
		PlayerPrefs.SetString("ENC-" + str, "2" + str2);
	}

	// Token: 0x06004DB1 RID: 19889 RVA: 0x001A0FF4 File Offset: 0x0019F3F4
	public static object GetEncryptedValue(string encryptedKey, string encryptedValue)
	{
		if (encryptedValue.StartsWith("0"))
		{
			return PlayerPrefsUtility.GetEncryptedFloat(SimpleEncryption.DecryptString(encryptedKey.Substring("ENC-".Length)), 0f);
		}
		if (encryptedValue.StartsWith("1"))
		{
			return PlayerPrefsUtility.GetEncryptedInt(SimpleEncryption.DecryptString(encryptedKey.Substring("ENC-".Length)), 0);
		}
		if (encryptedValue.StartsWith("2"))
		{
			return PlayerPrefsUtility.GetEncryptedString(SimpleEncryption.DecryptString(encryptedKey.Substring("ENC-".Length)), string.Empty);
		}
		throw new InvalidOperationException("Could not decrypt item, no match found in known encrypted key prefixes");
	}

	// Token: 0x06004DB2 RID: 19890 RVA: 0x001A10A4 File Offset: 0x0019F4A4
	public static float GetEncryptedFloat(string key, float defaultValue = 0f)
	{
		string key2 = "ENC-" + SimpleEncryption.EncryptString(key);
		string text = PlayerPrefs.GetString(key2);
		if (!string.IsNullOrEmpty(text))
		{
			text = text.Remove(0, 1);
			return SimpleEncryption.DecryptFloat(text);
		}
		return defaultValue;
	}

	// Token: 0x06004DB3 RID: 19891 RVA: 0x001A10E8 File Offset: 0x0019F4E8
	public static int GetEncryptedInt(string key, int defaultValue = 0)
	{
		string key2 = "ENC-" + SimpleEncryption.EncryptString(key);
		string text = PlayerPrefs.GetString(key2);
		if (!string.IsNullOrEmpty(text))
		{
			text = text.Remove(0, 1);
			return SimpleEncryption.DecryptInt(text);
		}
		return defaultValue;
	}

	// Token: 0x06004DB4 RID: 19892 RVA: 0x001A112C File Offset: 0x0019F52C
	public static string GetEncryptedString(string key, string defaultValue = "")
	{
		string key2 = "ENC-" + SimpleEncryption.EncryptString(key);
		string text = PlayerPrefs.GetString(key2);
		if (!string.IsNullOrEmpty(text))
		{
			text = text.Remove(0, 1);
			return SimpleEncryption.DecryptString(text);
		}
		return defaultValue;
	}

	// Token: 0x06004DB5 RID: 19893 RVA: 0x001A116D File Offset: 0x0019F56D
	public static void SetBool(string key, bool value)
	{
		if (value)
		{
			PlayerPrefs.SetInt(key, 1);
		}
		else
		{
			PlayerPrefs.SetInt(key, 0);
		}
	}

	// Token: 0x06004DB6 RID: 19894 RVA: 0x001A1188 File Offset: 0x0019F588
	public static bool GetBool(string key, bool defaultValue = false)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefs.GetInt(key) != 0;
		}
		return defaultValue;
	}

	// Token: 0x06004DB7 RID: 19895 RVA: 0x001A11B2 File Offset: 0x0019F5B2
	public static void SetEnum(string key, Enum value)
	{
		PlayerPrefs.SetString(key, value.ToString());
	}

	// Token: 0x06004DB8 RID: 19896 RVA: 0x001A11C0 File Offset: 0x0019F5C0
	public static T GetEnum<T>(string key, T defaultValue = default(T)) where T : struct
	{
		string @string = PlayerPrefs.GetString(key);
		if (!string.IsNullOrEmpty(@string))
		{
			return (T)((object)Enum.Parse(typeof(T), @string));
		}
		return defaultValue;
	}

	// Token: 0x06004DB9 RID: 19897 RVA: 0x001A11F8 File Offset: 0x0019F5F8
	public static object GetEnum(string key, Type enumType, object defaultValue)
	{
		string @string = PlayerPrefs.GetString(key);
		if (!string.IsNullOrEmpty(@string))
		{
			return Enum.Parse(enumType, @string);
		}
		return defaultValue;
	}

	// Token: 0x06004DBA RID: 19898 RVA: 0x001A1220 File Offset: 0x0019F620
	public static void SetDateTime(string key, DateTime value)
	{
		PlayerPrefs.SetString(key, value.ToString("o", CultureInfo.InvariantCulture));
	}

	// Token: 0x06004DBB RID: 19899 RVA: 0x001A123C File Offset: 0x0019F63C
	public static DateTime GetDateTime(string key, DateTime defaultValue = default(DateTime))
	{
		string @string = PlayerPrefs.GetString(key);
		if (!string.IsNullOrEmpty(@string))
		{
			return DateTime.Parse(@string, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
		}
		return defaultValue;
	}

	// Token: 0x06004DBC RID: 19900 RVA: 0x001A126D File Offset: 0x0019F66D
	public static void SetTimeSpan(string key, TimeSpan value)
	{
		PlayerPrefs.SetString(key, value.ToString());
	}

	// Token: 0x06004DBD RID: 19901 RVA: 0x001A1284 File Offset: 0x0019F684
	public static TimeSpan GetTimeSpan(string key, TimeSpan defaultValue = default(TimeSpan))
	{
		string @string = PlayerPrefs.GetString(key);
		if (!string.IsNullOrEmpty(@string))
		{
			return TimeSpan.Parse(@string);
		}
		return defaultValue;
	}

	// Token: 0x040035B7 RID: 13751
	public const string KEY_PREFIX = "ENC-";

	// Token: 0x040035B8 RID: 13752
	public const string VALUE_FLOAT_PREFIX = "0";

	// Token: 0x040035B9 RID: 13753
	public const string VALUE_INT_PREFIX = "1";

	// Token: 0x040035BA RID: 13754
	public const string VALUE_STRING_PREFIX = "2";
}
