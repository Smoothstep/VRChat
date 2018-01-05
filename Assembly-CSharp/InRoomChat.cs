using System;
using System.Collections.Generic;
using Photon;
using UnityEngine;

// Token: 0x02000789 RID: 1929
[RequireComponent(typeof(PhotonView))]
public class InRoomChat : Photon.MonoBehaviour
{
	// Token: 0x06003EB0 RID: 16048 RVA: 0x0013BF76 File Offset: 0x0013A376
	public void Start()
	{
		if (this.AlignBottom)
		{
			this.GuiRect.y = (float)Screen.height - this.GuiRect.height;
		}
	}

	// Token: 0x06003EB1 RID: 16049 RVA: 0x0013BFA0 File Offset: 0x0013A3A0
	public void OnGUI()
	{
		if (!this.IsVisible || !PhotonNetwork.inRoom)
		{
			return;
		}
		if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
		{
			if (!string.IsNullOrEmpty(this.inputLine))
			{
				base.photonView.RPC("Chat", PhotonTargets.All, new object[]
				{
					this.inputLine
				});
				this.inputLine = string.Empty;
				GUI.FocusControl(string.Empty);
				return;
			}
			GUI.FocusControl("ChatInput");
		}
		GUI.SetNextControlName(string.Empty);
		GUILayout.BeginArea(this.GuiRect);
		this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		for (int i = this.messages.Count - 1; i >= 0; i--)
		{
			GUILayout.Label(this.messages[i], new GUILayoutOption[0]);
		}
		GUILayout.EndScrollView();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUI.SetNextControlName("ChatInput");
		this.inputLine = GUILayout.TextField(this.inputLine, new GUILayoutOption[0]);
		if (GUILayout.Button("Send", new GUILayoutOption[]
		{
			GUILayout.ExpandWidth(false)
		}))
		{
			base.photonView.RPC("Chat", PhotonTargets.All, new object[]
			{
				this.inputLine
			});
			this.inputLine = string.Empty;
			GUI.FocusControl(string.Empty);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	// Token: 0x06003EB2 RID: 16050 RVA: 0x0013C13C File Offset: 0x0013A53C
	[PunRPC]
	public void Chat(string newLine, PhotonMessageInfo mi)
	{
		string str = "anonymous";
		if (mi.sender != null)
		{
			if (!string.IsNullOrEmpty(mi.sender.NickName))
			{
				str = mi.sender.NickName;
			}
			else
			{
				str = "player " + mi.sender.ID;
			}
		}
		this.messages.Add(str + ": " + newLine);
	}

	// Token: 0x06003EB3 RID: 16051 RVA: 0x0013C1B6 File Offset: 0x0013A5B6
	public void AddLine(string newLine)
	{
		this.messages.Add(newLine);
	}

	// Token: 0x0400274F RID: 10063
	public Rect GuiRect = new Rect(0f, 0f, 250f, 300f);

	// Token: 0x04002750 RID: 10064
	public bool IsVisible = true;

	// Token: 0x04002751 RID: 10065
	public bool AlignBottom;

	// Token: 0x04002752 RID: 10066
	public List<string> messages = new List<string>();

	// Token: 0x04002753 RID: 10067
	private string inputLine = string.Empty;

	// Token: 0x04002754 RID: 10068
	private Vector2 scrollPos = Vector2.zero;

	// Token: 0x04002755 RID: 10069
	public static readonly string ChatRPC = "Chat";
}
