using System;
using UnityEngine;

// Token: 0x02000A38 RID: 2616
public class HandCollision : MonoBehaviour
{
	// Token: 0x06004EBC RID: 20156 RVA: 0x001A7190 File Offset: 0x001A5590
	private void Start()
	{
		this.collisionRB = base.GetComponent<Rigidbody>();
		this.collisionRB.maxAngularVelocity = 100f;
	}

	// Token: 0x06004EBD RID: 20157 RVA: 0x001A71B0 File Offset: 0x001A55B0
	private void FixedUpdate()
	{
		Vector3 vector = (this.FollowTransform.position - base.transform.position) / Time.fixedDeltaTime;
		if (!vector.IsBad())
		{
			this.collisionRB.velocity = vector;
		}
		float num = 0f;
		Vector3 zero = Vector3.zero;
		(this.FollowTransform.rotation * Quaternion.Inverse(base.transform.rotation)).ToAngleAxis(out num, out zero);
		if (num > 180f)
		{
			num -= 360f;
		}
		Vector3 vector2 = num * 0.0174532924f / Time.fixedDeltaTime * zero;
		if (!vector2.IsBad())
		{
			this.collisionRB.angularVelocity = vector2;
		}
	}

	// Token: 0x04003709 RID: 14089
	public Transform FollowTransform;

	// Token: 0x0400370A RID: 14090
	private Rigidbody collisionRB;
}
