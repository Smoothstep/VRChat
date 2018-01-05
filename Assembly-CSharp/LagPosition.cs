using System;
using UnityEngine;

// Token: 0x0200058A RID: 1418
[AddComponentMenu("NGUI/Examples/Lag Position")]
public class LagPosition : MonoBehaviour
{
	// Token: 0x06002FC9 RID: 12233 RVA: 0x000E9E08 File Offset: 0x000E8208
	public void OnRepositionEnd()
	{
		this.Interpolate(1000f);
	}

	// Token: 0x06002FCA RID: 12234 RVA: 0x000E9E18 File Offset: 0x000E8218
	private void Interpolate(float delta)
	{
		Transform parent = this.mTrans.parent;
		if (parent != null)
		{
			Vector3 vector = parent.position + parent.rotation * this.mRelative;
			this.mAbsolute.x = Mathf.Lerp(this.mAbsolute.x, vector.x, Mathf.Clamp01(delta * this.speed.x));
			this.mAbsolute.y = Mathf.Lerp(this.mAbsolute.y, vector.y, Mathf.Clamp01(delta * this.speed.y));
			this.mAbsolute.z = Mathf.Lerp(this.mAbsolute.z, vector.z, Mathf.Clamp01(delta * this.speed.z));
			this.mTrans.position = this.mAbsolute;
		}
	}

	// Token: 0x06002FCB RID: 12235 RVA: 0x000E9F07 File Offset: 0x000E8307
	private void OnEnable()
	{
		this.mTrans = base.transform;
		this.mAbsolute = this.mTrans.position;
		this.mRelative = this.mTrans.localPosition;
	}

	// Token: 0x06002FCC RID: 12236 RVA: 0x000E9F37 File Offset: 0x000E8337
	private void Update()
	{
		this.Interpolate((!this.ignoreTimeScale) ? Time.deltaTime : RealTime.deltaTime);
	}

	// Token: 0x04001A32 RID: 6706
	public Vector3 speed = new Vector3(10f, 10f, 10f);

	// Token: 0x04001A33 RID: 6707
	public bool ignoreTimeScale;

	// Token: 0x04001A34 RID: 6708
	private Transform mTrans;

	// Token: 0x04001A35 RID: 6709
	private Vector3 mRelative;

	// Token: 0x04001A36 RID: 6710
	private Vector3 mAbsolute;
}
