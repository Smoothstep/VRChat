using System;
using UnityEngine;

// Token: 0x02000C4C RID: 3148
public class UiSpinner : MonoBehaviour
{
	// Token: 0x06006196 RID: 24982 RVA: 0x00227094 File Offset: 0x00225494
	private void Update()
	{
		base.transform.Rotate(this.RotationSpeed * Time.deltaTime);
	}

	// Token: 0x04004730 RID: 18224
	public Vector3 RotationSpeed;
}
