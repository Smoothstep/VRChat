using System;
using UnityEngine;

// Token: 0x02000B27 RID: 2855
public class VRCTrackingTest : VRCTracking
{
	// Token: 0x060056F9 RID: 22265 RVA: 0x001DF574 File Offset: 0x001DD974
	public override Transform GetTrackedTransform(VRCTracking.ID id)
	{
		switch (id)
		{
		case VRCTracking.ID.Hmd:
			return this.head;
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
		default:
			return null;
		}
	}

	// Token: 0x060056FA RID: 22266 RVA: 0x001DF5F0 File Offset: 0x001DD9F0
	public override bool IsTracked(VRCTracking.ID id)
	{
		return this.GetTrackedTransform(id) != null;
	}

	// Token: 0x04003DE2 RID: 15842
	public Transform head;

	// Token: 0x04003DE3 RID: 15843
	public Transform left;

	// Token: 0x04003DE4 RID: 15844
	public Transform right;

	// Token: 0x04003DE5 RID: 15845
	public Transform leftWrist;

	// Token: 0x04003DE6 RID: 15846
	public Transform rightWrist;

	// Token: 0x04003DE7 RID: 15847
	public Transform leftPointer;

	// Token: 0x04003DE8 RID: 15848
	public Transform rightPointer;

	// Token: 0x04003DE9 RID: 15849
	public Transform leftGun;

	// Token: 0x04003DEA RID: 15850
	public Transform rightGun;

	// Token: 0x04003DEB RID: 15851
	public Transform leftGrip;

	// Token: 0x04003DEC RID: 15852
	public Transform rightGrip;
}
