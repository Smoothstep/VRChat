using System;
using UnityEngine;

// Token: 0x02000BEA RID: 3050
[ExecuteInEditMode]
public class SteamVR_CameraMask : MonoBehaviour
{
	// Token: 0x06005EA0 RID: 24224 RVA: 0x00212413 File Offset: 0x00210813
	private void Awake()
	{
		Debug.Log("SteamVR_CameraMask is deprecated in Unity 5.4 - REMOVING");
		UnityEngine.Object.DestroyImmediate(this);
	}
}
