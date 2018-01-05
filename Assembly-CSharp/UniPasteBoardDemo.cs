using System;
using UnityEngine;

// Token: 0x02000970 RID: 2416
public class UniPasteBoardDemo : MonoBehaviour
{
	// Token: 0x0600494C RID: 18764 RVA: 0x00187A3B File Offset: 0x00185E3B
	private void Start()
	{
		this.screenWidth = Screen.width;
	}

	// Token: 0x0600494D RID: 18765 RVA: 0x00187A48 File Offset: 0x00185E48
	private void OnGUI()
	{
		this.s = GUILayout.TextArea(this.s, new GUILayoutOption[]
		{
			GUILayout.Width((float)this.screenWidth),
			GUILayout.MinHeight(200f)
		});
		if (GUILayout.Button("Copy to System paste board", new GUILayoutOption[]
		{
			GUILayout.Height(80f)
		}))
		{
			UniPasteBoard.SetClipBoardString(this.s);
		}
		if (GUILayout.Button("Paste From System paste board", new GUILayoutOption[]
		{
			GUILayout.Height(80f)
		}))
		{
			this.s = UniPasteBoard.GetClipBoardString();
		}
	}

	// Token: 0x040031A4 RID: 12708
	private string s = "Input here...";

	// Token: 0x040031A5 RID: 12709
	private int screenWidth;
}
