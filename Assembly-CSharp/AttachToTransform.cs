using System;
using UnityEngine;

// Token: 0x02000A59 RID: 2649
public class AttachToTransform : MonoBehaviour
{
	// Token: 0x06005035 RID: 20533 RVA: 0x001B754B File Offset: 0x001B594B
	private void Start()
	{
		this.ResetLocalTransform();
	}

	// Token: 0x06005036 RID: 20534 RVA: 0x001B7554 File Offset: 0x001B5954
	private void Update()
	{
		if (this.Parent == null)
		{
			return;
		}
		if (this.FollowRotation)
		{
			base.transform.rotation = this.Parent.rotation * this.LocalRotation;
		}
		if (this.FollowPosition)
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(this.Parent.position, (!this.FollowRotation) ? Quaternion.identity : this.Parent.rotation, Vector3.one);
			base.transform.position = matrix4x.MultiplyPoint3x4(this.LocalPosition);
		}
	}

	// Token: 0x06005037 RID: 20535 RVA: 0x001B75F8 File Offset: 0x001B59F8
	public void ResetLocalTransform()
	{
		if (this.Parent == null)
		{
			return;
		}
		this.LocalPosition = Matrix4x4.TRS(this.Parent.position, (!this.FollowRotation) ? Quaternion.identity : this.Parent.rotation, Vector3.one).inverse.MultiplyPoint3x4(base.transform.position);
		this.LocalRotation = Quaternion.Inverse(this.Parent.rotation) * base.transform.rotation;
	}

	// Token: 0x040038FB RID: 14587
	public Transform Parent;

	// Token: 0x040038FC RID: 14588
	public bool FollowPosition = true;

	// Token: 0x040038FD RID: 14589
	public bool FollowRotation = true;

	// Token: 0x040038FE RID: 14590
	private Vector3 LocalPosition;

	// Token: 0x040038FF RID: 14591
	private Quaternion LocalRotation;
}
