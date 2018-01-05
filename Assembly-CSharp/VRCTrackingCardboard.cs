using System;
using UnityEngine;

// Token: 0x02000B21 RID: 2849
public class VRCTrackingCardboard : VRCTracking
{
	// Token: 0x06005690 RID: 22160 RVA: 0x001DCAB3 File Offset: 0x001DAEB3
	public override Transform GetTrackedTransform(VRCTracking.ID id)
	{
		Debug.LogError("Find transform of camera in start and return it here");
		return null;
	}

	// Token: 0x06005691 RID: 22161 RVA: 0x001DCAC0 File Offset: 0x001DAEC0
	public override bool IsTracked(VRCTracking.ID id)
	{
		return false;
	}
}
