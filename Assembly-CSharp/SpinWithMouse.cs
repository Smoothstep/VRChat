using System;
using UnityEngine;

// Token: 0x02000593 RID: 1427
[AddComponentMenu("NGUI/Examples/Spin With Mouse")]
public class SpinWithMouse : MonoBehaviour
{
	// Token: 0x06002FE7 RID: 12263 RVA: 0x000EA771 File Offset: 0x000E8B71
	private void Start()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x06002FE8 RID: 12264 RVA: 0x000EA780 File Offset: 0x000E8B80
	private void OnDrag(Vector2 delta)
	{
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
		if (this.target != null)
		{
			this.target.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * this.speed, 0f) * this.target.localRotation;
		}
		else
		{
			this.mTrans.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * this.speed, 0f) * this.mTrans.localRotation;
		}
	}

	// Token: 0x04001A51 RID: 6737
	public Transform target;

	// Token: 0x04001A52 RID: 6738
	public float speed = 1f;

	// Token: 0x04001A53 RID: 6739
	private Transform mTrans;
}
