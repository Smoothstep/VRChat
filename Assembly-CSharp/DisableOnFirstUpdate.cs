using System;
using UnityEngine;

// Token: 0x02000A78 RID: 2680
public class DisableOnFirstUpdate : MonoBehaviour
{
	// Token: 0x060050D6 RID: 20694 RVA: 0x001BA279 File Offset: 0x001B8679
	private void Update()
	{
		base.gameObject.SetActive(false);
		UnityEngine.Object.Destroy(this);
	}
}
