using System;
using UniPasteBoardPlugin;

// Token: 0x02000971 RID: 2417
public class UniPasteBoard
{
	// Token: 0x0600494F RID: 18767 RVA: 0x00187AEA File Offset: 0x00185EEA
	public static string GetClipBoardString()
	{
		return UniPasteBoardPlugin.UniPasteBoard.GetClipBoardString();
	}

	// Token: 0x06004950 RID: 18768 RVA: 0x00187AF1 File Offset: 0x00185EF1
	public static void SetClipBoardString(string text)
	{
		UniPasteBoardPlugin.UniPasteBoard.SetClipBoardString(text);
	}
}
