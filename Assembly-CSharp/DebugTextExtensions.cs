using System;
using UnityEngine;

// Token: 0x02000A75 RID: 2677
public static class DebugTextExtensions
{
	// Token: 0x060050C9 RID: 20681 RVA: 0x001B9E3E File Offset: 0x001B823E
	public static void DebugPrint(this UnityEngine.Object obj, string format, params object[] values)
	{
		DebugTextGUI.DebugPrint(format, values);
		if (Application.isEditor)
		{
			Debug.Log(string.Format(format, values), obj);
		}
	}
}
