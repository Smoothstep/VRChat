using System;
using UnityEngine;

// Token: 0x02000791 RID: 1937
public class OnStartDelete : MonoBehaviour
{
	// Token: 0x06003ED5 RID: 16085 RVA: 0x0013CF84 File Offset: 0x0013B384
	private void Start()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
