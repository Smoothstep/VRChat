using System;
using UnityEngine;

// Token: 0x020009E6 RID: 2534
public class USpeakTNetManager : MonoBehaviour
{
	// Token: 0x06004D17 RID: 19735 RVA: 0x0019D916 File Offset: 0x0019BD16
	public void OnNetworkJoinChannel(bool success, string message)
	{
		if (success)
		{
			Debug.LogError("FIX PREVIOUS LINE");
		}
	}

	// Token: 0x0400350C RID: 13580
	public static int Channel;

	// Token: 0x0400350D RID: 13581
	public GameObject USpeakerPrefab;
}
