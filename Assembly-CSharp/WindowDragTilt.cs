using System;
using UnityEngine;

// Token: 0x02000597 RID: 1431
[AddComponentMenu("NGUI/Examples/Window Drag Tilt")]
public class WindowDragTilt : MonoBehaviour
{
	// Token: 0x06002FF3 RID: 12275 RVA: 0x000EAB04 File Offset: 0x000E8F04
	private void OnEnable()
	{
		this.mTrans = base.transform;
		this.mLastPos = this.mTrans.position;
	}

	// Token: 0x06002FF4 RID: 12276 RVA: 0x000EAB24 File Offset: 0x000E8F24
	private void Update()
	{
		Vector3 vector = this.mTrans.position - this.mLastPos;
		this.mLastPos = this.mTrans.position;
		this.mAngle += vector.x * this.degrees;
		this.mAngle = NGUIMath.SpringLerp(this.mAngle, 0f, 20f, Time.deltaTime);
		this.mTrans.localRotation = Quaternion.Euler(0f, 0f, -this.mAngle);
	}

	// Token: 0x04001A5C RID: 6748
	public int updateOrder;

	// Token: 0x04001A5D RID: 6749
	public float degrees = 30f;

	// Token: 0x04001A5E RID: 6750
	private Vector3 mLastPos;

	// Token: 0x04001A5F RID: 6751
	private Transform mTrans;

	// Token: 0x04001A60 RID: 6752
	private float mAngle;
}
