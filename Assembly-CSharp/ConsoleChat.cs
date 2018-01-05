using System;
using Photon;
using UnityEngine;

// Token: 0x0200044A RID: 1098
public class ConsoleChat : Photon.MonoBehaviour
{
	// Token: 0x060026D4 RID: 9940 RVA: 0x000BF6F1 File Offset: 0x000BDAF1
	public void SendTextChat(string playerName, string s, int channel)
	{
		if (RoomManager.inRoom)
		{
			Debug.LogError("FIX PREVIOUS LINE");
		}
	}

	// Token: 0x060026D5 RID: 9941 RVA: 0x000BF707 File Offset: 0x000BDB07
	private void LogChatMessage(string playerName, string s)
	{
		ConsoleLog.Instance.Log(playerName + ": " + s);
	}
}
