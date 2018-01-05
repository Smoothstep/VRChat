using System;
using UnityEngine;

// Token: 0x02000B22 RID: 2850
public class VRCTrackingHandProxy : VRCTracking
{
	// Token: 0x17000C98 RID: 3224
	// (get) Token: 0x06005693 RID: 22163 RVA: 0x001DCAE5 File Offset: 0x001DAEE5
	public override bool calibrated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06005694 RID: 22164 RVA: 0x001DCAE8 File Offset: 0x001DAEE8
	public override Transform GetTrackedTransform(VRCTracking.ID id)
	{
		switch (id)
		{
		case VRCTracking.ID.HandTracker_LeftWrist:
			return this.leftWrist;
		case VRCTracking.ID.HandTracker_RightWrist:
			return this.rightWrist;
		case VRCTracking.ID.HandTracker_LeftPointer:
			return this.leftPointer;
		case VRCTracking.ID.HandTracker_RightPointer:
			return this.rightPointer;
		case VRCTracking.ID.HandTracker_LeftGun:
			return this.leftGun;
		case VRCTracking.ID.HandTracker_RightGun:
			return this.rightGun;
		case VRCTracking.ID.HandTracker_LeftGrip:
			return this.leftGrip;
		case VRCTracking.ID.HandTracker_RightGrip:
			return this.rightGrip;
		case VRCTracking.ID.HandTracker_LeftPalm:
			return this.leftPalm;
		case VRCTracking.ID.HandTracker_RightPalm:
			return this.rightPalm;
		default:
			return null;
		}
	}

	// Token: 0x06005695 RID: 22165 RVA: 0x001DCB71 File Offset: 0x001DAF71
	private void FixedUpdate()
	{
		this.UpdateHandPositions();
	}

	// Token: 0x06005696 RID: 22166 RVA: 0x001DCB7C File Offset: 0x001DAF7C
	private void UpdateHandPositions()
	{
		Transform trackedTransform = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.Hmd);
		if (trackedTransform == null)
		{
			return;
		}
		this.left.position = trackedTransform.position + trackedTransform.right * this.handOffset.x + trackedTransform.forward * this.handOffset.y + trackedTransform.up * this.handOffset.z;
		this.left.rotation = trackedTransform.rotation * Quaternion.FromToRotation(Vector3.right, Vector3.up);
		this.right.position = trackedTransform.position + trackedTransform.right * -this.handOffset.x + trackedTransform.forward * this.handOffset.y + trackedTransform.up * this.handOffset.z;
		this.right.rotation = trackedTransform.rotation * Quaternion.FromToRotation(Vector3.left, Vector3.up);
	}

	// Token: 0x06005697 RID: 22167 RVA: 0x001DCCAC File Offset: 0x001DB0AC
	public override bool IsTracked(VRCTracking.ID id)
	{
		return false;
	}

	// Token: 0x04003D82 RID: 15746
	public Transform left;

	// Token: 0x04003D83 RID: 15747
	public Transform right;

	// Token: 0x04003D84 RID: 15748
	public Transform leftWrist;

	// Token: 0x04003D85 RID: 15749
	public Transform rightWrist;

	// Token: 0x04003D86 RID: 15750
	public Transform leftPointer;

	// Token: 0x04003D87 RID: 15751
	public Transform rightPointer;

	// Token: 0x04003D88 RID: 15752
	public Transform leftGun;

	// Token: 0x04003D89 RID: 15753
	public Transform rightGun;

	// Token: 0x04003D8A RID: 15754
	public Transform leftGrip;

	// Token: 0x04003D8B RID: 15755
	public Transform rightGrip;

	// Token: 0x04003D8C RID: 15756
	public Transform leftPalm;

	// Token: 0x04003D8D RID: 15757
	public Transform rightPalm;

	// Token: 0x04003D8E RID: 15758
	public Vector3 handOffset = new Vector3(-0.15f, 0.35f, -0.15f);
}
