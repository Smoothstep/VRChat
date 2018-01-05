using System;
using UnityEngine;

// Token: 0x02000492 RID: 1170
public class F3DTrailExample : MonoBehaviour
{
	// Token: 0x06002831 RID: 10289 RVA: 0x000D1171 File Offset: 0x000CF571
	private void Start()
	{
		this.defaultPos = base.transform.position;
	}

	// Token: 0x06002832 RID: 10290 RVA: 0x000D1184 File Offset: 0x000CF584
	private void Update()
	{
		base.transform.position = this.defaultPos + new Vector3(Mathf.Sin(Time.time * this.TimeMult) * this.Mult, 0f, Mathf.Cos(Time.time * this.TimeMult) * this.Mult);
	}

	// Token: 0x04001678 RID: 5752
	public float Mult;

	// Token: 0x04001679 RID: 5753
	public float TimeMult;

	// Token: 0x0400167A RID: 5754
	private Vector3 defaultPos;
}
