using System;
using UnityEngine;

// Token: 0x0200088D RID: 2189
public class LazyLoad : MonoBehaviour
{
	// Token: 0x0600435D RID: 17245 RVA: 0x00162DD1 File Offset: 0x001611D1
	private void Awake()
	{
		this.GO.SetActive(false);
	}

	// Token: 0x0600435E RID: 17246 RVA: 0x00162DDF File Offset: 0x001611DF
	private void LazyEnable()
	{
		this.GO.SetActive(true);
	}

	// Token: 0x0600435F RID: 17247 RVA: 0x00162DED File Offset: 0x001611ED
	private void OnEnable()
	{
		base.Invoke("LazyEnable", this.TimeDelay);
	}

	// Token: 0x06004360 RID: 17248 RVA: 0x00162E00 File Offset: 0x00161200
	private void OnDisable()
	{
		base.CancelInvoke("LazyEnable");
		this.GO.SetActive(false);
	}

	// Token: 0x04002BC4 RID: 11204
	public GameObject GO;

	// Token: 0x04002BC5 RID: 11205
	public float TimeDelay = 0.3f;
}
