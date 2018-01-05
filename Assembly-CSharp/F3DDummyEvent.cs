using System;
using UnityEngine;

// Token: 0x0200048F RID: 1167
public class F3DDummyEvent : MonoBehaviour
{
	// Token: 0x06002827 RID: 10279 RVA: 0x000D0F49 File Offset: 0x000CF349
	private void Start()
	{
		base.BroadcastMessage("OnSpawned", SendMessageOptions.DontRequireReceiver);
	}
}
