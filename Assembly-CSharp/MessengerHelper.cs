using System;
using UnityEngine;

// Token: 0x020006F2 RID: 1778
public sealed class MessengerHelper : MonoBehaviour
{
	// Token: 0x06003A7A RID: 14970 RVA: 0x001274E3 File Offset: 0x001258E3
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06003A7B RID: 14971 RVA: 0x001274F0 File Offset: 0x001258F0
	public void OnDisable()
	{
		OVRMessenger.Cleanup();
	}
}
