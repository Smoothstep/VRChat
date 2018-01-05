using System;
using UnityEngine;

// Token: 0x02000B61 RID: 2913
public class SyncPhysicsLateUpdate : MonoBehaviour
{
	// Token: 0x0600595D RID: 22877 RVA: 0x001F0BBA File Offset: 0x001EEFBA
	private void Awake()
	{
		this.sync = base.GetComponent<SyncPhysics>();
	}

	// Token: 0x0600595E RID: 22878 RVA: 0x001F0BC8 File Offset: 0x001EEFC8
	private void LateUpdate()
	{
		if (this.sync.enabled)
		{
			this.sync.DoPositionSync((double)Time.time, (double)Time.deltaTime);
		}
	}

	// Token: 0x04003FED RID: 16365
	private SyncPhysics sync;
}
