using System;
using UnityEngine;

// Token: 0x020006E6 RID: 1766
public class OVRWaitCursor : MonoBehaviour
{
	// Token: 0x06003A39 RID: 14905 RVA: 0x001268D8 File Offset: 0x00124CD8
	private void Update()
	{
		base.transform.Rotate(this.rotateSpeeds * Time.smoothDeltaTime);
	}

	// Token: 0x0400231E RID: 8990
	public Vector3 rotateSpeeds = new Vector3(0f, 0f, -60f);
}
