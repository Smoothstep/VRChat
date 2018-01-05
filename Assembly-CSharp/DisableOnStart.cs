using System;
using UnityEngine;

// Token: 0x020004AF RID: 1199
public class DisableOnStart : MonoBehaviour
{
	// Token: 0x06002A07 RID: 10759 RVA: 0x000D648C File Offset: 0x000D488C
	private void Start()
	{
		base.gameObject.SetActive(false);
	}
}
