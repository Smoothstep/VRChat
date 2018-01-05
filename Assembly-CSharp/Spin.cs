using System;
using UnityEngine;

// Token: 0x02000592 RID: 1426
[AddComponentMenu("NGUI/Examples/Spin")]
public class Spin : MonoBehaviour
{
	// Token: 0x06002FE2 RID: 12258 RVA: 0x000EA67D File Offset: 0x000E8A7D
	private void Start()
	{
		this.mTrans = base.transform;
		this.mRb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06002FE3 RID: 12259 RVA: 0x000EA697 File Offset: 0x000E8A97
	private void Update()
	{
		if (this.mRb == null)
		{
			this.ApplyDelta((!this.ignoreTimeScale) ? Time.deltaTime : RealTime.deltaTime);
		}
	}

	// Token: 0x06002FE4 RID: 12260 RVA: 0x000EA6CA File Offset: 0x000E8ACA
	private void FixedUpdate()
	{
		if (this.mRb != null)
		{
			this.ApplyDelta(Time.deltaTime);
		}
	}

	// Token: 0x06002FE5 RID: 12261 RVA: 0x000EA6E8 File Offset: 0x000E8AE8
	public void ApplyDelta(float delta)
	{
		delta *= 360f;
		Quaternion rhs = Quaternion.Euler(this.rotationsPerSecond * delta);
		if (this.mRb == null)
		{
			this.mTrans.rotation = this.mTrans.rotation * rhs;
		}
		else
		{
			this.mRb.MoveRotation(this.mRb.rotation * rhs);
		}
	}

	// Token: 0x04001A4D RID: 6733
	public Vector3 rotationsPerSecond = new Vector3(0f, 0.1f, 0f);

	// Token: 0x04001A4E RID: 6734
	public bool ignoreTimeScale;

	// Token: 0x04001A4F RID: 6735
	private Rigidbody mRb;

	// Token: 0x04001A50 RID: 6736
	private Transform mTrans;
}
