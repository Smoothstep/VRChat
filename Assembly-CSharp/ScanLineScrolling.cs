using System;
using UnityEngine;

// Token: 0x02000C36 RID: 3126
public class ScanLineScrolling : MonoBehaviour
{
	// Token: 0x06006124 RID: 24868 RVA: 0x00224341 File Offset: 0x00222741
	private void Start()
	{
		this.analogTv = base.GetComponent<CC_AnalogTV>();
	}

	// Token: 0x06006125 RID: 24869 RVA: 0x0022434F File Offset: 0x0022274F
	private void Update()
	{
		this.analogTv.scanlinesOffset += this.ScrollSpeed * Time.deltaTime;
	}

	// Token: 0x040046CA RID: 18122
	public float ScrollSpeed = 1f;

	// Token: 0x040046CB RID: 18123
	private CC_AnalogTV analogTv;
}
