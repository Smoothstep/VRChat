using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005EC RID: 1516
public static class Localization
{
	// Token: 0x17000790 RID: 1936
	// (get) Token: 0x06003234 RID: 12852 RVA: 0x000F8D28 File Offset: 0x000F7128
	// (set) Token: 0x06003235 RID: 12853 RVA: 0x000F8D4D File Offset: 0x000F714D
	public static Dictionary<string, string[]> dictionary
	{
		get
		{
			if (!Localization.localizationHasBeenSet)
			{
				Localization.language = PlayerPrefs.GetString("Language", "English");
			}
			return Localization.mDictionary;
		}
		set
		{
			Localization.localizationHasBeenSet = (value != null);
			Localization.mDictionary = value;
		}
	}

	// Token: 0x17000791 RID: 1937
	// (get) Token: 0x06003236 RID: 12854 RVA: 0x000F8D61 File Offset: 0x000F7161
	public static string[] knownLanguages
	{
		get
		{
			if (!Localization.localizationHasBeenSet)
			{
				Localization.LoadDictionary(PlayerPrefs.GetString("Language", "English"));
			}
			return Localization.mLanguages;
		}
	}

	// Token: 0x17000792 RID: 1938
	// (get) Token: 0x06003237 RID: 12855 RVA: 0x000F8D88 File Offset: 0x000F7188
	// (set) Token: 0x06003238 RID: 12856 RVA: 0x000F8DDC File Offset: 0x000F71DC
	public static string language
	{
		get
		{
			if (string.IsNullOrEmpty(Localization.mLanguage))
			{
				string[] knownLanguages = Localization.knownLanguages;
				Localization.mLanguage = PlayerPrefs.GetString("Language", (knownLanguages == null) ? "English" : knownLanguages[0]);
				Localization.LoadAndSelect(Localization.mLanguage);
			}
			return Localization.mLanguage;
		}
		set
		{
			if (Localization.mLanguage != value)
			{
				Localization.mLanguage = value;
				Localization.LoadAndSelect(value);
			}
		}
	}

	// Token: 0x06003239 RID: 12857 RVA: 0x000F8DFC File Offset: 0x000F71FC
	private static bool LoadDictionary(string value)
	{
		byte[] array = null;
		if (!Localization.localizationHasBeenSet)
		{
			if (Localization.loadFunction == null)
			{
				TextAsset textAsset = Resources.Load<TextAsset>("Localization");
				if (textAsset != null)
				{
					array = textAsset.bytes;
				}
			}
			else
			{
				array = Localization.loadFunction("Localization");
			}
			Localization.localizationHasBeenSet = true;
		}
		if (Localization.LoadCSV(array))
		{
			return true;
		}
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}
		if (Localization.loadFunction == null)
		{
			TextAsset textAsset2 = Resources.Load<TextAsset>(value);
			if (textAsset2 != null)
			{
				array = textAsset2.bytes;
			}
		}
		else
		{
			array = Localization.loadFunction(value);
		}
		if (array != null)
		{
			Localization.Set(value, array);
			return true;
		}
		return false;
	}

	// Token: 0x0600323A RID: 12858 RVA: 0x000F8EB8 File Offset: 0x000F72B8
	private static bool LoadAndSelect(string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			if (Localization.mDictionary.Count == 0 && !Localization.LoadDictionary(value))
			{
				return false;
			}
			if (Localization.SelectLanguage(value))
			{
				return true;
			}
		}
		if (Localization.mOldDictionary.Count > 0)
		{
			return true;
		}
		Localization.mOldDictionary.Clear();
		Localization.mDictionary.Clear();
		if (string.IsNullOrEmpty(value))
		{
			PlayerPrefs.DeleteKey("Language");
		}
		return false;
	}

	// Token: 0x0600323B RID: 12859 RVA: 0x000F8F38 File Offset: 0x000F7338
	public static void Load(TextAsset asset)
	{
		ByteReader byteReader = new ByteReader(asset);
		Localization.Set(asset.name, byteReader.ReadDictionary());
	}

	// Token: 0x0600323C RID: 12860 RVA: 0x000F8F60 File Offset: 0x000F7360
	public static void Set(string languageName, byte[] bytes)
	{
		ByteReader byteReader = new ByteReader(bytes);
		Localization.Set(languageName, byteReader.ReadDictionary());
	}

	// Token: 0x0600323D RID: 12861 RVA: 0x000F8F80 File Offset: 0x000F7380
	public static bool LoadCSV(TextAsset asset)
	{
		return Localization.LoadCSV(asset.bytes, asset);
	}

	// Token: 0x0600323E RID: 12862 RVA: 0x000F8F8E File Offset: 0x000F738E
	public static bool LoadCSV(byte[] bytes)
	{
		return Localization.LoadCSV(bytes, null);
	}

	// Token: 0x0600323F RID: 12863 RVA: 0x000F8F98 File Offset: 0x000F7398
	private static bool LoadCSV(byte[] bytes, TextAsset asset)
	{
		if (bytes == null)
		{
			return false;
		}
		ByteReader byteReader = new ByteReader(bytes);
		BetterList<string> betterList = byteReader.ReadCSV();
		if (betterList.size < 2)
		{
			return false;
		}
		betterList[0] = "KEY";
		if (!string.Equals(betterList[0], "KEY"))
		{
			Debug.LogError("Invalid localization CSV file. The first value is expected to be 'KEY', followed by language columns.\nInstead found '" + betterList[0] + "'", asset);
			return false;
		}
		Localization.mLanguages = new string[betterList.size - 1];
		for (int i = 0; i < Localization.mLanguages.Length; i++)
		{
			Localization.mLanguages[i] = betterList[i + 1];
		}
		Localization.mDictionary.Clear();
		while (betterList != null)
		{
			Localization.AddCSV(betterList);
			betterList = byteReader.ReadCSV();
		}
		return true;
	}

	// Token: 0x06003240 RID: 12864 RVA: 0x000F9068 File Offset: 0x000F7468
	private static bool SelectLanguage(string language)
	{
		Localization.mLanguageIndex = -1;
		if (Localization.mDictionary.Count == 0)
		{
			return false;
		}
		string[] array;
		if (Localization.mDictionary.TryGetValue("KEY", out array))
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == language)
				{
					Localization.mOldDictionary.Clear();
					Localization.mLanguageIndex = i;
					Localization.mLanguage = language;
					PlayerPrefs.SetString("Language", Localization.mLanguage);
					UIRoot.Broadcast("OnLocalize");
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003241 RID: 12865 RVA: 0x000F90F8 File Offset: 0x000F74F8
	private static void AddCSV(BetterList<string> values)
	{
		if (values.size < 2)
		{
			return;
		}
		string[] array = new string[values.size - 1];
		for (int i = 1; i < values.size; i++)
		{
			array[i - 1] = values[i];
		}
		try
		{
			Localization.mDictionary.Add(values[0], array);
		}
		catch (Exception ex)
		{
			Debug.LogError("Unable to add '" + values[0] + "' to the Localization dictionary.\n" + ex.Message);
		}
	}

	// Token: 0x06003242 RID: 12866 RVA: 0x000F9194 File Offset: 0x000F7594
	public static void Set(string languageName, Dictionary<string, string> dictionary)
	{
		Localization.mLanguage = languageName;
		PlayerPrefs.SetString("Language", Localization.mLanguage);
		Localization.mOldDictionary = dictionary;
		Localization.localizationHasBeenSet = false;
		Localization.mLanguageIndex = -1;
		Localization.mLanguages = new string[]
		{
			languageName
		};
		UIRoot.Broadcast("OnLocalize");
	}

	// Token: 0x06003243 RID: 12867 RVA: 0x000F91E4 File Offset: 0x000F75E4
	public static string Get(string key)
	{
		if (!Localization.localizationHasBeenSet)
		{
			Localization.language = PlayerPrefs.GetString("Language", "English");
		}
		string[] array;
		string result;
		if (Localization.mLanguageIndex != -1 && Localization.mDictionary.TryGetValue(key, out array))
		{
			if (Localization.mLanguageIndex < array.Length)
			{
				return array[Localization.mLanguageIndex];
			}
		}
		else if (Localization.mOldDictionary.TryGetValue(key, out result))
		{
			return result;
		}
		return key;
	}

	// Token: 0x06003244 RID: 12868 RVA: 0x000F925B File Offset: 0x000F765B
	public static string Format(string key, params object[] parameters)
	{
		return string.Format(Localization.Get(key), parameters);
	}

	// Token: 0x17000793 RID: 1939
	// (get) Token: 0x06003245 RID: 12869 RVA: 0x000F9269 File Offset: 0x000F7669
	[Obsolete("Localization is now always active. You no longer need to check this property.")]
	public static bool isActive
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003246 RID: 12870 RVA: 0x000F926C File Offset: 0x000F766C
	[Obsolete("Use Localization.Get instead")]
	public static string Localize(string key)
	{
		return Localization.Get(key);
	}

	// Token: 0x06003247 RID: 12871 RVA: 0x000F9274 File Offset: 0x000F7674
	public static bool Exists(string key)
	{
		if (!Localization.localizationHasBeenSet)
		{
			Localization.language = PlayerPrefs.GetString("Language", "English");
		}
		return Localization.mDictionary.ContainsKey(key) || Localization.mOldDictionary.ContainsKey(key);
	}

	// Token: 0x04001CA9 RID: 7337
	public static Localization.LoadFunction loadFunction;

	// Token: 0x04001CAA RID: 7338
	public static bool localizationHasBeenSet = false;

	// Token: 0x04001CAB RID: 7339
	private static string[] mLanguages = null;

	// Token: 0x04001CAC RID: 7340
	private static Dictionary<string, string> mOldDictionary = new Dictionary<string, string>();

	// Token: 0x04001CAD RID: 7341
	private static Dictionary<string, string[]> mDictionary = new Dictionary<string, string[]>();

	// Token: 0x04001CAE RID: 7342
	private static int mLanguageIndex = -1;

	// Token: 0x04001CAF RID: 7343
	private static string mLanguage;

	// Token: 0x020005ED RID: 1517
	// (Invoke) Token: 0x0600324A RID: 12874
	public delegate byte[] LoadFunction(string path);
}
