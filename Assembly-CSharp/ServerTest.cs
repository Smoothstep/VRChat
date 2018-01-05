using System;
using UnityEngine;

// Token: 0x02000AEB RID: 2795
public class ServerTest : MonoBehaviour
{
	// Token: 0x060054B8 RID: 21688 RVA: 0x001D3BF4 File Offset: 0x001D1FF4
	private void OnGUI()
	{
		Debug.LogError("FIX PREVIOUS LINE");
		GUI.Label(new Rect(10f, 100f, 300f, 50f), "Status: " + this.status.ToString());
		GUI.Label(new Rect(10f, 130f, 500f, 50f), "Message: " + this._networkMessage);
	}

	// Token: 0x060054B9 RID: 21689 RVA: 0x001D3C72 File Offset: 0x001D2072
	private void OnNetworkConnect(bool success, string message)
	{
		if (success)
		{
			Debug.Log("Successfully connected to server.");
			this.status = ServerTest.ConnectionStatus.Connected;
		}
		else
		{
			Debug.Log("Could not connect to server - " + message);
			this._networkMessage = message;
			this.status = ServerTest.ConnectionStatus.Disconnected;
		}
	}

	// Token: 0x060054BA RID: 21690 RVA: 0x001D3CAE File Offset: 0x001D20AE
	private void OnNetworkDisconnect()
	{
		this._networkMessage = string.Empty;
		this.status = ServerTest.ConnectionStatus.Disconnected;
	}

	// Token: 0x04003BC7 RID: 15303
	public string dedicatedServerIp = "129.59.30.56";

	// Token: 0x04003BC8 RID: 15304
	public int serverPort = 5129;

	// Token: 0x04003BC9 RID: 15305
	private string _networkMessage = string.Empty;

	// Token: 0x04003BCA RID: 15306
	[SerializeField]
	private ServerTest.ConnectionStatus status;

	// Token: 0x02000AEC RID: 2796
	private enum ConnectionStatus
	{
		// Token: 0x04003BCC RID: 15308
		Disconnected,
		// Token: 0x04003BCD RID: 15309
		Connected
	}
}
