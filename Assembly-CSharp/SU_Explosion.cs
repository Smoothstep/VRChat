using System;
using UnityEngine;

// Token: 0x020008D0 RID: 2256
public class SU_Explosion : MonoBehaviour
{
	// Token: 0x060044C5 RID: 17605 RVA: 0x00170220 File Offset: 0x0016E620
	private void Awake()
	{
		UnityEngine.Object.Destroy(base.gameObject, this.destroyAfterSeconds);
	}

	// Token: 0x04002EC1 RID: 11969
	public float destroyAfterSeconds = 8f;
}
