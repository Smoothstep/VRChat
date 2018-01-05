using System;
using UnityEngine;

// Token: 0x02000596 RID: 1430
[AddComponentMenu("NGUI/Examples/Window Auto-Yaw")]
public class WindowAutoYaw : MonoBehaviour
{
	// Token: 0x06002FEF RID: 12271 RVA: 0x000EAA3E File Offset: 0x000E8E3E
	private void OnDisable()
	{
		this.mTrans.localRotation = Quaternion.identity;
	}

	// Token: 0x06002FF0 RID: 12272 RVA: 0x000EAA50 File Offset: 0x000E8E50
	private void OnEnable()
	{
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.mTrans = base.transform;
	}

	// Token: 0x06002FF1 RID: 12273 RVA: 0x000EAA88 File Offset: 0x000E8E88
	private void Update()
	{
		if (this.uiCamera != null)
		{
			Vector3 vector = this.uiCamera.WorldToViewportPoint(this.mTrans.position);
			this.mTrans.localRotation = Quaternion.Euler(0f, (vector.x * 2f - 1f) * this.yawAmount, 0f);
		}
	}

	// Token: 0x04001A58 RID: 6744
	public int updateOrder;

	// Token: 0x04001A59 RID: 6745
	public Camera uiCamera;

	// Token: 0x04001A5A RID: 6746
	public float yawAmount = 20f;

	// Token: 0x04001A5B RID: 6747
	private Transform mTrans;
}
