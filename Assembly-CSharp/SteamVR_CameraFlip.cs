using System;
using UnityEngine;

// Token: 0x02000BE9 RID: 3049
[ExecuteInEditMode]
public class SteamVR_CameraFlip : MonoBehaviour
{
	// Token: 0x06005E9E RID: 24222 RVA: 0x002123F9 File Offset: 0x002107F9
	private void Awake()
	{
		Debug.Log("SteamVR_CameraFlip is deprecated in Unity 5.4 - REMOVING");
		UnityEngine.Object.DestroyImmediate(this);
	}
}
