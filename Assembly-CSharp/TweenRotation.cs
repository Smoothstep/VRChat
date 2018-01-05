using System;
using UnityEngine;

// Token: 0x02000621 RID: 1569
[AddComponentMenu("NGUI/Tween/Tween Rotation")]
public class TweenRotation : UITweener
{
	// Token: 0x170007F3 RID: 2035
	// (get) Token: 0x06003488 RID: 13448 RVA: 0x00109B56 File Offset: 0x00107F56
	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	// Token: 0x170007F4 RID: 2036
	// (get) Token: 0x06003489 RID: 13449 RVA: 0x00109B7B File Offset: 0x00107F7B
	// (set) Token: 0x0600348A RID: 13450 RVA: 0x00109B83 File Offset: 0x00107F83
	[Obsolete("Use 'value' instead")]
	public Quaternion rotation
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x170007F5 RID: 2037
	// (get) Token: 0x0600348B RID: 13451 RVA: 0x00109B8C File Offset: 0x00107F8C
	// (set) Token: 0x0600348C RID: 13452 RVA: 0x00109B99 File Offset: 0x00107F99
	public Quaternion value
	{
		get
		{
			return this.cachedTransform.localRotation;
		}
		set
		{
			this.cachedTransform.localRotation = value;
		}
	}

	// Token: 0x0600348D RID: 13453 RVA: 0x00109BA8 File Offset: 0x00107FA8
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Quaternion.Euler(new Vector3(Mathf.Lerp(this.from.x, this.to.x, factor), Mathf.Lerp(this.from.y, this.to.y, factor), Mathf.Lerp(this.from.z, this.to.z, factor)));
	}

	// Token: 0x0600348E RID: 13454 RVA: 0x00109C1C File Offset: 0x0010801C
	public static TweenRotation Begin(GameObject go, float duration, Quaternion rot)
	{
		TweenRotation tweenRotation = UITweener.Begin<TweenRotation>(go, duration);
		tweenRotation.from = tweenRotation.value.eulerAngles;
		tweenRotation.to = rot.eulerAngles;
		if (duration <= 0f)
		{
			tweenRotation.Sample(1f, true);
			tweenRotation.enabled = false;
		}
		return tweenRotation;
	}

	// Token: 0x0600348F RID: 13455 RVA: 0x00109C74 File Offset: 0x00108074
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value.eulerAngles;
	}

	// Token: 0x06003490 RID: 13456 RVA: 0x00109C98 File Offset: 0x00108098
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value.eulerAngles;
	}

	// Token: 0x06003491 RID: 13457 RVA: 0x00109CB9 File Offset: 0x001080B9
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = Quaternion.Euler(this.from);
	}

	// Token: 0x06003492 RID: 13458 RVA: 0x00109CCC File Offset: 0x001080CC
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = Quaternion.Euler(this.to);
	}

	// Token: 0x04001DE9 RID: 7657
	public Vector3 from;

	// Token: 0x04001DEA RID: 7658
	public Vector3 to;

	// Token: 0x04001DEB RID: 7659
	private Transform mTrans;
}
