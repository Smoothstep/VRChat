using System;
using UnityEngine;

// Token: 0x020009DC RID: 2524
public class UnityNetworkExample : MonoBehaviour
{
	// Token: 0x06004CC6 RID: 19654 RVA: 0x0019BBD8 File Offset: 0x00199FD8
	private void OnGUI()
	{
		if (Network.peerType == NetworkPeerType.Disconnected)
		{
			if (GUI.Button(new Rect(10f, 10f, 100f, 30f), "Connect"))
			{
				Network.Connect(this.remoteIP, this.remotePort);
			}
			if (GUI.Button(new Rect(10f, 50f, 100f, 30f), "Start Server"))
			{
				Network.InitializeServer(32, this.listenPort, true);
			}
			this.remoteIP = GUI.TextField(new Rect(120f, 10f, 100f, 20f), this.remoteIP);
			this.remotePort = int.Parse(GUI.TextField(new Rect(230f, 10f, 40f, 20f), this.remotePort.ToString()));
		}
		else
		{
			if (GUI.Button(new Rect(10f, 10f, 100f, 50f), "Disconnect"))
			{
				Network.Disconnect(200);
			}
			GUILayout.BeginArea(new Rect(10f, 60f, 200f, 500f));
			int num = 0;
			foreach (string text in Microphone.devices)
			{
				if (GUILayout.Button(text, new GUILayoutOption[]
				{
					GUILayout.Width(200f)
				}))
				{
					USpeaker.SetInputDevice(num);
				}
				num++;
			}
			GUILayout.EndArea();
		}
	}

	// Token: 0x06004CC7 RID: 19655 RVA: 0x0019BD66 File Offset: 0x0019A166
	private void OnConnectedToServer()
	{
		Network.Instantiate(this.PlayerObject, UnityEngine.Random.insideUnitSphere * 5f, Quaternion.identity, 0);
	}

	// Token: 0x06004CC8 RID: 19656 RVA: 0x0019BD89 File Offset: 0x0019A189
	private void OnServerInitialized()
	{
		Network.Instantiate(this.PlayerObject, UnityEngine.Random.insideUnitSphere * 5f, Quaternion.identity, 0);
	}

	// Token: 0x040034E1 RID: 13537
	public GameObject PlayerObject;

	// Token: 0x040034E2 RID: 13538
	private string remoteIP = "127.0.0.1";

	// Token: 0x040034E3 RID: 13539
	private int remotePort = 25000;

	// Token: 0x040034E4 RID: 13540
	private int listenPort = 25000;
}
