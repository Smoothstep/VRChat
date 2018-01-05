using System;
using UnityEngine;

// Token: 0x02000453 RID: 1107
public class ConsoleNamedKeyBugFix : MonoBehaviour
{
	// Token: 0x060026EE RID: 9966 RVA: 0x000BF858 File Offset: 0x000BDC58
	private void OnGUI()
	{
		string nextControlName = base.gameObject.GetHashCode().ToString();
		GUI.SetNextControlName(nextControlName);
		Rect position = new Rect(0f, 0f, 0f, 0f);
		GUI.TextField(position, string.Empty, 0);
	}
}
