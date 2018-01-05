using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200056D RID: 1389
public class DestroyAfterTime : MonoBehaviour
{
	// Token: 0x06002F66 RID: 12134 RVA: 0x000E6274 File Offset: 0x000E4674
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(this.waitTime);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04001998 RID: 6552
	public float waitTime;
}
