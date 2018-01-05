using System;
using UnityEngine;

// Token: 0x02000BFF RID: 3071
[ExecuteInEditMode]
public class SteamVR_GameView : MonoBehaviour
{
	// Token: 0x06005F1F RID: 24351 RVA: 0x00214BDD File Offset: 0x00212FDD
	private void Awake()
	{
		Debug.Log("SteamVR_GameView is deprecated in Unity 5.4 - REMOVING");
		UnityEngine.Object.DestroyImmediate(this);
	}
}
