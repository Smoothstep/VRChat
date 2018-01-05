using System;
using UnityEngine;

// Token: 0x02000B60 RID: 2912
public class SyncPhysicsFixedUpdate : MonoBehaviour
{
	// Token: 0x0600595A RID: 22874 RVA: 0x001F0B7B File Offset: 0x001EEF7B
	private void Awake()
	{
		this.sync = base.GetComponent<SyncPhysics>();
	}

	// Token: 0x0600595B RID: 22875 RVA: 0x001F0B89 File Offset: 0x001EEF89
	private void FixedUpdate()
	{
		if (this.sync.enabled)
		{
			this.sync.DoPositionSync((double)Time.fixedTime, (double)Time.fixedDeltaTime);
		}
	}

	// Token: 0x04003FEC RID: 16364
	private SyncPhysics sync;
}
