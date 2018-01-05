using System;
using UnityEngine;

// Token: 0x020006D9 RID: 1753
public class OVRTracker
{
	// Token: 0x1700091D RID: 2333
	// (get) Token: 0x060039E2 RID: 14818 RVA: 0x00123C93 File Offset: 0x00122093
	public bool isPresent
	{
		get
		{
			return OVRManager.isHmdPresent && OVRPlugin.positionSupported;
		}
	}

	// Token: 0x1700091E RID: 2334
	// (get) Token: 0x060039E3 RID: 14819 RVA: 0x00123CA6 File Offset: 0x001220A6
	public bool isPositionTracked
	{
		get
		{
			return OVRPlugin.positionTracked;
		}
	}

	// Token: 0x1700091F RID: 2335
	// (get) Token: 0x060039E4 RID: 14820 RVA: 0x00123CAD File Offset: 0x001220AD
	// (set) Token: 0x060039E5 RID: 14821 RVA: 0x00123CC0 File Offset: 0x001220C0
	public bool isEnabled
	{
		get
		{
			return OVRManager.isHmdPresent && OVRPlugin.position;
		}
		set
		{
			if (!OVRManager.isHmdPresent)
			{
				return;
			}
			OVRPlugin.position = value;
		}
	}

	// Token: 0x17000920 RID: 2336
	// (get) Token: 0x060039E6 RID: 14822 RVA: 0x00123CD4 File Offset: 0x001220D4
	public int count
	{
		get
		{
			int num = 0;
			for (int i = 0; i < 2; i++)
			{
				if (this.GetPresent(i))
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x060039E7 RID: 14823 RVA: 0x00123D08 File Offset: 0x00122108
	public OVRTracker.Frustum GetFrustum(int tracker = 0)
	{
		if (!OVRManager.isHmdPresent)
		{
			return default(OVRTracker.Frustum);
		}
		return OVRPlugin.GetTrackerFrustum((OVRPlugin.Tracker)tracker).ToFrustum();
	}

	// Token: 0x060039E8 RID: 14824 RVA: 0x00123D34 File Offset: 0x00122134
	public OVRPose GetPose(int tracker = 0)
	{
		if (!OVRManager.isHmdPresent)
		{
			return OVRPose.identity;
		}
		OVRPose ovrpose;
		switch (tracker)
		{
		case 0:
			ovrpose = OVRPlugin.GetNodePose(OVRPlugin.Node.TrackerZero, false).ToOVRPose();
			break;
		case 1:
			ovrpose = OVRPlugin.GetNodePose(OVRPlugin.Node.TrackerOne, false).ToOVRPose();
			break;
		case 2:
			ovrpose = OVRPlugin.GetNodePose(OVRPlugin.Node.TrackerTwo, false).ToOVRPose();
			break;
		case 3:
			ovrpose = OVRPlugin.GetNodePose(OVRPlugin.Node.TrackerThree, false).ToOVRPose();
			break;
		default:
			return OVRPose.identity;
		}
		return new OVRPose
		{
			position = ovrpose.position,
			orientation = ovrpose.orientation * Quaternion.Euler(0f, 180f, 0f)
		};
	}

	// Token: 0x060039E9 RID: 14825 RVA: 0x00123DF8 File Offset: 0x001221F8
	public bool GetPoseValid(int tracker = 0)
	{
		if (!OVRManager.isHmdPresent)
		{
			return false;
		}
		switch (tracker)
		{
		case 0:
			return OVRPlugin.GetNodePositionTracked(OVRPlugin.Node.TrackerZero);
		case 1:
			return OVRPlugin.GetNodePositionTracked(OVRPlugin.Node.TrackerOne);
		case 2:
			return OVRPlugin.GetNodePositionTracked(OVRPlugin.Node.TrackerTwo);
		case 3:
			return OVRPlugin.GetNodePositionTracked(OVRPlugin.Node.TrackerThree);
		default:
			return false;
		}
	}

	// Token: 0x060039EA RID: 14826 RVA: 0x00123E4C File Offset: 0x0012224C
	public bool GetPresent(int tracker = 0)
	{
		if (!OVRManager.isHmdPresent)
		{
			return false;
		}
		switch (tracker)
		{
		case 0:
			return OVRPlugin.GetNodePresent(OVRPlugin.Node.TrackerZero);
		case 1:
			return OVRPlugin.GetNodePresent(OVRPlugin.Node.TrackerOne);
		case 2:
			return OVRPlugin.GetNodePresent(OVRPlugin.Node.TrackerTwo);
		case 3:
			return OVRPlugin.GetNodePresent(OVRPlugin.Node.TrackerThree);
		default:
			return false;
		}
	}

	// Token: 0x020006DA RID: 1754
	public struct Frustum
	{
		// Token: 0x040022BB RID: 8891
		public float nearZ;

		// Token: 0x040022BC RID: 8892
		public float farZ;

		// Token: 0x040022BD RID: 8893
		public Vector2 fov;
	}
}
