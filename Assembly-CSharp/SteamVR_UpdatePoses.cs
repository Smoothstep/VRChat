using System;
using UnityEngine;

// Token: 0x02000C15 RID: 3093
[ExecuteInEditMode]
public class SteamVR_UpdatePoses : MonoBehaviour
{
	// Token: 0x06005FBF RID: 24511 RVA: 0x0021A93B File Offset: 0x00218D3B
	private void Awake()
	{
		Debug.Log("SteamVR_UpdatePoses has been deprecated - REMOVING");
		UnityEngine.Object.DestroyImmediate(this);
	}
}
