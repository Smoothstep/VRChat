using System;
using UnityEngine;

// Token: 0x020007A2 RID: 1954
public class ShowStatusWhenConnecting : MonoBehaviour
{
	// Token: 0x06003F30 RID: 16176 RVA: 0x0013E470 File Offset: 0x0013C870
	private void OnGUI()
	{
		if (this.Skin != null)
		{
			GUI.skin = this.Skin;
		}
		float num = 400f;
		float num2 = 100f;
		Rect screenRect = new Rect(((float)Screen.width - num) / 2f, ((float)Screen.height - num2) / 2f, num, num2);
		GUILayout.BeginArea(screenRect, GUI.skin.box);
		GUILayout.Label("Connecting" + this.GetConnectingDots(), GUI.skin.customStyles[0], new GUILayoutOption[0]);
		GUILayout.Label("Status: " + PhotonNetwork.connectionStateDetailed, new GUILayoutOption[0]);
		GUILayout.EndArea();
		if (PhotonNetwork.inRoom)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06003F31 RID: 16177 RVA: 0x0013E538 File Offset: 0x0013C938
	private string GetConnectingDots()
	{
		string text = string.Empty;
		int num = Mathf.FloorToInt(Time.timeSinceLevelLoad * 3f % 4f);
		for (int i = 0; i < num; i++)
		{
			text += " .";
		}
		return text;
	}

	// Token: 0x0400279A RID: 10138
	public GUISkin Skin;
}
