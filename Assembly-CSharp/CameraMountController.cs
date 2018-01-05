using System;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000A31 RID: 2609
public class CameraMountController : MonoBehaviour
{
	// Token: 0x06004E8C RID: 20108 RVA: 0x001A5644 File Offset: 0x001A3A44
	public void Initialize(VRC_AvatarDescriptor descriptor, Animator animator, bool Local)
	{
		if (!Local)
		{
			return;
		}
		this.CameraMount = base.transform.Find("../CameraMount");
		Vector3 viewPosition = descriptor.ViewPosition;
		viewPosition.z = 0f;
		this.CameraMount.localPosition = viewPosition;
		this.BaseHeadPos = this.CameraMount.localPosition;
		this.Initialized = true;
	}

	// Token: 0x06004E8D RID: 20109 RVA: 0x001A56A8 File Offset: 0x001A3AA8
	public void Apply()
	{
		if (!this.Initialized)
		{
			return;
		}
		this.CameraMount.localPosition = this.BaseHeadPos;
		if (!VRCVrCamera.GetInstance().IsTrackingPosition())
		{
			this.CameraMount.localPosition += this.manualHeadTracking;
		}
	}

	// Token: 0x040036AB RID: 13995
	private Vector3 BaseHeadPos;

	// Token: 0x040036AC RID: 13996
	private Transform CameraMount;

	// Token: 0x040036AD RID: 13997
	private bool Initialized;

	// Token: 0x040036AE RID: 13998
	public Vector3 manualHeadTracking = Vector3.zero;
}
