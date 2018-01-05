using System;
using UnityEngine;

// Token: 0x0200056E RID: 1390
public class DestroyOnFadeCompletion : MonoBehaviour
{
	// Token: 0x06002F68 RID: 12136 RVA: 0x000E6339 File Offset: 0x000E4739
	private void FadeCompleted()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
