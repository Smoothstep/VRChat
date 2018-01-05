using System;
using UnityEngine;

// Token: 0x02000493 RID: 1171
public class F3DTurnTable : MonoBehaviour
{
	// Token: 0x06002834 RID: 10292 RVA: 0x000D11E9 File Offset: 0x000CF5E9
	private void Start()
	{
	}

	// Token: 0x06002835 RID: 10293 RVA: 0x000D11EB File Offset: 0x000CF5EB
	private void Update()
	{
		base.transform.rotation = base.transform.rotation * Quaternion.Euler(0f, this.speed * Time.deltaTime, 0f);
	}

	// Token: 0x0400167B RID: 5755
	public float speed;
}
