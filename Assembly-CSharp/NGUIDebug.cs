using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005EE RID: 1518
[AddComponentMenu("NGUI/Internal/Debug")]
public class NGUIDebug : MonoBehaviour
{
	// Token: 0x17000794 RID: 1940
	// (get) Token: 0x0600324E RID: 12878 RVA: 0x000F92E2 File Offset: 0x000F76E2
	// (set) Token: 0x0600324F RID: 12879 RVA: 0x000F92E9 File Offset: 0x000F76E9
	public static bool debugRaycast
	{
		get
		{
			return NGUIDebug.mRayDebug;
		}
		set
		{
			if (Application.isPlaying)
			{
				NGUIDebug.mRayDebug = value;
				if (value)
				{
					NGUIDebug.CreateInstance();
				}
			}
		}
	}

	// Token: 0x06003250 RID: 12880 RVA: 0x000F9308 File Offset: 0x000F7708
	public static void CreateInstance()
	{
		if (NGUIDebug.mInstance == null)
		{
			GameObject gameObject = new GameObject("_NGUI Debug");
			NGUIDebug.mInstance = gameObject.AddComponent<NGUIDebug>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
	}

	// Token: 0x06003251 RID: 12881 RVA: 0x000F9344 File Offset: 0x000F7744
	private static void LogString(string text)
	{
		if (Application.isPlaying)
		{
			if (NGUIDebug.mLines.Count > 20)
			{
				NGUIDebug.mLines.RemoveAt(0);
			}
			NGUIDebug.mLines.Add(text);
			NGUIDebug.CreateInstance();
		}
		else
		{
			Debug.Log(text);
		}
	}

	// Token: 0x06003252 RID: 12882 RVA: 0x000F9394 File Offset: 0x000F7794
	public static void Log(params object[] objs)
	{
		string text = string.Empty;
		for (int i = 0; i < objs.Length; i++)
		{
			if (i == 0)
			{
				text += objs[i].ToString();
			}
			else
			{
				text = text + ", " + objs[i].ToString();
			}
		}
		NGUIDebug.LogString(text);
	}

	// Token: 0x06003253 RID: 12883 RVA: 0x000F93EF File Offset: 0x000F77EF
	public static void Clear()
	{
		NGUIDebug.mLines.Clear();
	}

	// Token: 0x06003254 RID: 12884 RVA: 0x000F93FC File Offset: 0x000F77FC
	public static void DrawBounds(Bounds b)
	{
		Vector3 center = b.center;
		Vector3 vector = b.center - b.extents;
		Vector3 vector2 = b.center + b.extents;
		Debug.DrawLine(new Vector3(vector.x, vector.y, center.z), new Vector3(vector2.x, vector.y, center.z), Color.red);
		Debug.DrawLine(new Vector3(vector.x, vector.y, center.z), new Vector3(vector.x, vector2.y, center.z), Color.red);
		Debug.DrawLine(new Vector3(vector2.x, vector.y, center.z), new Vector3(vector2.x, vector2.y, center.z), Color.red);
		Debug.DrawLine(new Vector3(vector.x, vector2.y, center.z), new Vector3(vector2.x, vector2.y, center.z), Color.red);
	}

	// Token: 0x06003255 RID: 12885 RVA: 0x000F9534 File Offset: 0x000F7934
	private void OnGUI()
	{
		if (NGUIDebug.mLines.Count == 0)
		{
			if (NGUIDebug.mRayDebug && UICamera.hoveredObject != null && Application.isPlaying)
			{
				GUILayout.Label("Last Hit: " + NGUITools.GetHierarchy(UICamera.hoveredObject).Replace("\"", string.Empty), new GUILayoutOption[0]);
			}
		}
		else
		{
			int i = 0;
			int count = NGUIDebug.mLines.Count;
			while (i < count)
			{
				GUILayout.Label(NGUIDebug.mLines[i], new GUILayoutOption[0]);
				i++;
			}
		}
	}

	// Token: 0x04001CB0 RID: 7344
	private static bool mRayDebug = false;

	// Token: 0x04001CB1 RID: 7345
	private static List<string> mLines = new List<string>();

	// Token: 0x04001CB2 RID: 7346
	private static NGUIDebug mInstance = null;
}
