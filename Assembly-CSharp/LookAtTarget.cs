using System;
using UnityEngine;

// Token: 0x0200058D RID: 1421
[AddComponentMenu("NGUI/Examples/Look At Target")]
public class LookAtTarget : MonoBehaviour
{
	// Token: 0x06002FD5 RID: 12245 RVA: 0x000EA066 File Offset: 0x000E8466
	private void Start()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x06002FD6 RID: 12246 RVA: 0x000EA074 File Offset: 0x000E8474
	private void LateUpdate()
	{
		if (this.target != null)
		{
			Vector3 forward = this.target.position - this.mTrans.position;
			float magnitude = forward.magnitude;
			if (magnitude > 0.001f)
			{
				Quaternion b = Quaternion.LookRotation(forward);
				this.mTrans.rotation = Quaternion.Slerp(this.mTrans.rotation, b, Mathf.Clamp01(this.speed * Time.deltaTime));
			}
		}
	}

	// Token: 0x04001A3D RID: 6717
	public int level;

	// Token: 0x04001A3E RID: 6718
	public Transform target;

	// Token: 0x04001A3F RID: 6719
	public float speed = 8f;

	// Token: 0x04001A40 RID: 6720
	private Transform mTrans;
}
