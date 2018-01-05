using System;
using UnityEngine;

// Token: 0x0200058B RID: 1419
[AddComponentMenu("NGUI/Examples/Lag Rotation")]
public class LagRotation : MonoBehaviour
{
	// Token: 0x06002FCE RID: 12238 RVA: 0x000E9F6C File Offset: 0x000E836C
	public void OnRepositionEnd()
	{
		this.Interpolate(1000f);
	}

	// Token: 0x06002FCF RID: 12239 RVA: 0x000E9F7C File Offset: 0x000E837C
	private void Interpolate(float delta)
	{
		Transform parent = this.mTrans.parent;
		if (parent != null)
		{
			this.mAbsolute = Quaternion.Slerp(this.mAbsolute, parent.rotation * this.mRelative, delta * this.speed);
			this.mTrans.rotation = this.mAbsolute;
		}
	}

	// Token: 0x06002FD0 RID: 12240 RVA: 0x000E9FDC File Offset: 0x000E83DC
	private void OnEnable()
	{
		this.mTrans = base.transform;
		this.mRelative = this.mTrans.localRotation;
		this.mAbsolute = this.mTrans.rotation;
	}

	// Token: 0x06002FD1 RID: 12241 RVA: 0x000EA00C File Offset: 0x000E840C
	private void Update()
	{
		this.Interpolate((!this.ignoreTimeScale) ? Time.deltaTime : RealTime.deltaTime);
	}

	// Token: 0x04001A37 RID: 6711
	public float speed = 10f;

	// Token: 0x04001A38 RID: 6712
	public bool ignoreTimeScale;

	// Token: 0x04001A39 RID: 6713
	private Transform mTrans;

	// Token: 0x04001A3A RID: 6714
	private Quaternion mRelative;

	// Token: 0x04001A3B RID: 6715
	private Quaternion mAbsolute;
}
