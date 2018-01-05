using System;
using UnityEngine;

// Token: 0x02000B35 RID: 2869
public class VRCBotLauncher : MonoBehaviour
{
	// Token: 0x060057A1 RID: 22433 RVA: 0x001E3992 File Offset: 0x001E1D92
	private void Start()
	{
		this.ConnectToServer();
	}

	// Token: 0x060057A2 RID: 22434 RVA: 0x001E399A File Offset: 0x001E1D9A
	private void ConnectToServer()
	{
		Debug.LogError("FIX PREVIOUS LINE");
	}

	// Token: 0x060057A3 RID: 22435 RVA: 0x001E39A6 File Offset: 0x001E1DA6
	private void OnNetworkConnect(bool result, string message)
	{
		Debug.LogError("FIX PREVIOUS LINE");
	}

	// Token: 0x04003E9C RID: 16028
	public uint id;

	// Token: 0x04003E9D RID: 16029
	public string server = "us1.vrchat.net";

	// Token: 0x04003E9E RID: 16030
	public int serverPort = 5127;

	// Token: 0x04003E9F RID: 16031
	public string channelName = "Bot Test";

	// Token: 0x04003EA0 RID: 16032
	public int channelID = 117;

	// Token: 0x04003EA1 RID: 16033
	public string levelName = "CoffeeShop";

	// Token: 0x04003EA2 RID: 16034
	public int playerLimit = 64;

	// Token: 0x04003EA3 RID: 16035
	public string password = "bottest";

	// Token: 0x04003EA4 RID: 16036
	public string successFunctionName;

	// Token: 0x04003EA5 RID: 16037
	public string failureFunctionName;
}
