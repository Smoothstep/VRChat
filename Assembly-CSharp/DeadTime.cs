using System;
using UnityEngine;

// Token: 0x02000895 RID: 2197
public class DeadTime : MonoBehaviour
{
	// Token: 0x0600437F RID: 17279 RVA: 0x001645A4 File Offset: 0x001629A4
	private void Awake()
	{
		UnityEngine.Object.Destroy(this.destroyRoot ? base.transform.root.gameObject : base.gameObject, this.deadTime);
	}

	// Token: 0x04002C21 RID: 11297
	public float deadTime = 1.5f;

	// Token: 0x04002C22 RID: 11298
	public bool destroyRoot;
}
