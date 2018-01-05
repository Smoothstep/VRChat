using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000732 RID: 1842
public static class Extensions
{
	// Token: 0x06003B98 RID: 15256 RVA: 0x0012BF10 File Offset: 0x0012A310
	public static ParameterInfo[] GetCachedParemeters(this MethodInfo mo)
	{
		ParameterInfo[] parameters;
		if (!Extensions.ParametersOfMethods.TryGetValue(mo, out parameters))
		{
			parameters = mo.GetParameters();
			Extensions.ParametersOfMethods[mo] = parameters;
		}
		return parameters;
	}

	// Token: 0x06003B99 RID: 15257 RVA: 0x0012BF45 File Offset: 0x0012A345
	public static PhotonView[] GetPhotonViewsInChildren(this GameObject go)
	{
		return go.GetComponentsInChildren<PhotonView>(true);
	}

	// Token: 0x06003B9A RID: 15258 RVA: 0x0012BF4E File Offset: 0x0012A34E
	public static PhotonView GetPhotonView(this GameObject go)
	{
		return go.GetComponent<PhotonView>();
	}

	// Token: 0x06003B9B RID: 15259 RVA: 0x0012BF58 File Offset: 0x0012A358
	public static bool AlmostEquals(this Vector3 target, Vector3 second, float sqrMagnitudePrecision)
	{
		return (target - second).sqrMagnitude < sqrMagnitudePrecision;
	}

	// Token: 0x06003B9C RID: 15260 RVA: 0x0012BF78 File Offset: 0x0012A378
	public static bool AlmostEquals(this Vector2 target, Vector2 second, float sqrMagnitudePrecision)
	{
		return (target - second).sqrMagnitude < sqrMagnitudePrecision;
	}

	// Token: 0x06003B9D RID: 15261 RVA: 0x0012BF97 File Offset: 0x0012A397
	public static bool AlmostEquals(this Quaternion target, Quaternion second, float maxAngle)
	{
		return Quaternion.Angle(target, second) < maxAngle;
	}

	// Token: 0x06003B9E RID: 15262 RVA: 0x0012BFA3 File Offset: 0x0012A3A3
	public static bool AlmostEquals(this float target, float second, float floatDiff)
	{
		return Mathf.Abs(target - second) < floatDiff;
	}

	// Token: 0x06003B9F RID: 15263 RVA: 0x0012BFB0 File Offset: 0x0012A3B0
	public static void Merge(this IDictionary target, IDictionary addHash)
	{
		if (addHash == null || target.Equals(addHash))
		{
			return;
		}
		IEnumerator enumerator = addHash.Keys.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object key = enumerator.Current;
				target[key] = addHash[key];
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

	// Token: 0x06003BA0 RID: 15264 RVA: 0x0012C02C File Offset: 0x0012A42C
	public static void MergeStringKeys(this IDictionary target, IDictionary addHash)
	{
		if (addHash == null || target.Equals(addHash))
		{
			return;
		}
		IEnumerator enumerator = addHash.Keys.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (obj is string)
				{
					target[obj] = addHash[obj];
				}
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

	// Token: 0x06003BA1 RID: 15265 RVA: 0x0012C0B4 File Offset: 0x0012A4B4
	public static string ToStringFull(this IDictionary origin)
	{
		return SupportClass.DictionaryToString(origin, false);
	}

	// Token: 0x06003BA2 RID: 15266 RVA: 0x0012C0C0 File Offset: 0x0012A4C0
	public static string ToStringFull(this object[] data)
	{
		if (data == null)
		{
			return "null";
		}
		string[] array = new string[data.Length];
		for (int i = 0; i < data.Length; i++)
		{
			object obj = data[i];
			array[i] = ((obj == null) ? "null" : obj.ToString());
		}
		return string.Join(", ", array);
	}

	// Token: 0x06003BA3 RID: 15267 RVA: 0x0012C120 File Offset: 0x0012A520
	public static ExitGames.Client.Photon.Hashtable StripToStringKeys(this IDictionary original)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		if (original != null)
		{
			IEnumerator enumerator = original.Keys.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					if (obj is string)
					{
						hashtable[obj] = original[obj];
					}
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
		return hashtable;
	}

	// Token: 0x06003BA4 RID: 15268 RVA: 0x0012C1A0 File Offset: 0x0012A5A0
	public static void StripKeysWithNullValues(this IDictionary original)
	{
		object[] array = new object[original.Count];
		int num = 0;
		IEnumerator enumerator = original.Keys.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				array[num++] = obj;
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
		foreach (object key in array)
		{
			if (original[key] == null)
			{
				original.Remove(key);
			}
		}
	}

	// Token: 0x06003BA5 RID: 15269 RVA: 0x0012C244 File Offset: 0x0012A644
	public static bool Contains(this int[] target, int nr)
	{
		if (target == null)
		{
			return false;
		}
		for (int i = 0; i < target.Length; i++)
		{
			if (target[i] == nr)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040024B0 RID: 9392
	public static Dictionary<MethodInfo, ParameterInfo[]> ParametersOfMethods = new Dictionary<MethodInfo, ParameterInfo[]>();
}
