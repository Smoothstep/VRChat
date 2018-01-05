using System;
using UnityEngine;

// Token: 0x02000486 RID: 1158
public class F3DRift : MonoBehaviour
{
	// Token: 0x06002801 RID: 10241 RVA: 0x000D0420 File Offset: 0x000CE820
	private void Start()
	{
		this.dScale = base.transform.localScale;
	}

	// Token: 0x06002802 RID: 10242 RVA: 0x000D0434 File Offset: 0x000CE834
	private void Update()
	{
		base.transform.rotation = base.transform.rotation * Quaternion.Euler(0f, 0f, this.RotationSpeed * Time.deltaTime);
		base.transform.localScale = new Vector3(this.dScale.x, this.dScale.y, this.dScale.z + Mathf.Sin(Time.time * this.MorphSpeed) * this.MorphFactor);
	}

	// Token: 0x04001643 RID: 5699
	public float RotationSpeed;

	// Token: 0x04001644 RID: 5700
	public float MorphSpeed;

	// Token: 0x04001645 RID: 5701
	public float MorphFactor;

	// Token: 0x04001646 RID: 5702
	private Vector3 dScale;
}
