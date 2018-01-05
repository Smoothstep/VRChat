using System;
using UnityEngine;

// Token: 0x02000B4B RID: 2891
public class VRC_DisableAfterTime : MonoBehaviour
{
	// Token: 0x0600589D RID: 22685 RVA: 0x001EB41A File Offset: 0x001E981A
	private void Start()
	{
		this.timer = 0f;
	}

	// Token: 0x0600589E RID: 22686 RVA: 0x001EB427 File Offset: 0x001E9827
	private void Update()
	{
		this.timer += Time.deltaTime;
		if (this.timer > this.timeOut)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x04003F80 RID: 16256
	public float timeOut = 1f;

	// Token: 0x04003F81 RID: 16257
	private float timer;
}
