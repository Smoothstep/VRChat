using System;
using UnityEngine;

// Token: 0x02000B0C RID: 2828
public class VRCVrCameraCardboard : VRCVrCamera
{
	// Token: 0x060055AC RID: 21932 RVA: 0x001D890F File Offset: 0x001D6D0F
	public override Ray GetWorldLookRay()
	{
		return new Ray(base.transform.position, base.transform.forward);
	}

	// Token: 0x060055AD RID: 21933 RVA: 0x001D892C File Offset: 0x001D6D2C
	public override Vector3 GetLocalCameraPos()
	{
		return Vector3.zero;
	}

	// Token: 0x060055AE RID: 21934 RVA: 0x001D8933 File Offset: 0x001D6D33
	public override Quaternion GetLocalCameraRot()
	{
		return Quaternion.identity;
	}

	// Token: 0x060055AF RID: 21935 RVA: 0x001D893A File Offset: 0x001D6D3A
	public override void SetMode(VRCVrCamera.CameraMode mode)
	{
	}
}
