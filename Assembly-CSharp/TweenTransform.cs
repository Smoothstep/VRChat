using System;
using UnityEngine;

// Token: 0x02000623 RID: 1571
[AddComponentMenu("NGUI/Tween/Tween Transform")]
public class TweenTransform : UITweener
{
	// Token: 0x060034A0 RID: 13472 RVA: 0x00109E60 File Offset: 0x00108260
	protected override void OnUpdate(float factor, bool isFinished)
	{
		if (this.to != null)
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
				this.mPos = this.mTrans.position;
				this.mRot = this.mTrans.rotation;
				this.mScale = this.mTrans.localScale;
			}
			if (this.from != null)
			{
				this.mTrans.position = this.from.position * (1f - factor) + this.to.position * factor;
				this.mTrans.localScale = this.from.localScale * (1f - factor) + this.to.localScale * factor;
				this.mTrans.rotation = Quaternion.Slerp(this.from.rotation, this.to.rotation, factor);
			}
			else
			{
				this.mTrans.position = this.mPos * (1f - factor) + this.to.position * factor;
				this.mTrans.localScale = this.mScale * (1f - factor) + this.to.localScale * factor;
				this.mTrans.rotation = Quaternion.Slerp(this.mRot, this.to.rotation, factor);
			}
			if (this.parentWhenFinished && isFinished)
			{
				this.mTrans.parent = this.to;
			}
		}
	}

	// Token: 0x060034A1 RID: 13473 RVA: 0x0010A025 File Offset: 0x00108425
	public static TweenTransform Begin(GameObject go, float duration, Transform to)
	{
		return TweenTransform.Begin(go, duration, null, to);
	}

	// Token: 0x060034A2 RID: 13474 RVA: 0x0010A030 File Offset: 0x00108430
	public static TweenTransform Begin(GameObject go, float duration, Transform from, Transform to)
	{
		TweenTransform tweenTransform = UITweener.Begin<TweenTransform>(go, duration);
		tweenTransform.from = from;
		tweenTransform.to = to;
		if (duration <= 0f)
		{
			tweenTransform.Sample(1f, true);
			tweenTransform.enabled = false;
		}
		return tweenTransform;
	}

	// Token: 0x04001DF1 RID: 7665
	public Transform from;

	// Token: 0x04001DF2 RID: 7666
	public Transform to;

	// Token: 0x04001DF3 RID: 7667
	public bool parentWhenFinished;

	// Token: 0x04001DF4 RID: 7668
	private Transform mTrans;

	// Token: 0x04001DF5 RID: 7669
	private Vector3 mPos;

	// Token: 0x04001DF6 RID: 7670
	private Quaternion mRot;

	// Token: 0x04001DF7 RID: 7671
	private Vector3 mScale;
}
