using System;
using UnityEngine;

// Token: 0x02000622 RID: 1570
[AddComponentMenu("NGUI/Tween/Tween Scale")]
public class TweenScale : UITweener
{
	// Token: 0x170007F6 RID: 2038
	// (get) Token: 0x06003494 RID: 13460 RVA: 0x00109CFD File Offset: 0x001080FD
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

	// Token: 0x170007F7 RID: 2039
	// (get) Token: 0x06003495 RID: 13461 RVA: 0x00109D22 File Offset: 0x00108122
	// (set) Token: 0x06003496 RID: 13462 RVA: 0x00109D2F File Offset: 0x0010812F
	public Vector3 value
	{
		get
		{
			return this.cachedTransform.localScale;
		}
		set
		{
			this.cachedTransform.localScale = value;
		}
	}

	// Token: 0x170007F8 RID: 2040
	// (get) Token: 0x06003497 RID: 13463 RVA: 0x00109D3D File Offset: 0x0010813D
	// (set) Token: 0x06003498 RID: 13464 RVA: 0x00109D45 File Offset: 0x00108145
	[Obsolete("Use 'value' instead")]
	public Vector3 scale
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

	// Token: 0x06003499 RID: 13465 RVA: 0x00109D50 File Offset: 0x00108150
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
		if (this.updateTable)
		{
			if (this.mTable == null)
			{
				this.mTable = NGUITools.FindInParents<UITable>(base.gameObject);
				if (this.mTable == null)
				{
					this.updateTable = false;
					return;
				}
			}
			this.mTable.repositionNow = true;
		}
	}

	// Token: 0x0600349A RID: 13466 RVA: 0x00109DD8 File Offset: 0x001081D8
	public static TweenScale Begin(GameObject go, float duration, Vector3 scale)
	{
		TweenScale tweenScale = UITweener.Begin<TweenScale>(go, duration);
		tweenScale.from = tweenScale.value;
		tweenScale.to = scale;
		if (duration <= 0f)
		{
			tweenScale.Sample(1f, true);
			tweenScale.enabled = false;
		}
		return tweenScale;
	}

	// Token: 0x0600349B RID: 13467 RVA: 0x00109E1F File Offset: 0x0010821F
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x0600349C RID: 13468 RVA: 0x00109E2D File Offset: 0x0010822D
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x0600349D RID: 13469 RVA: 0x00109E3B File Offset: 0x0010823B
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x0600349E RID: 13470 RVA: 0x00109E49 File Offset: 0x00108249
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x04001DEC RID: 7660
	public Vector3 from = Vector3.one;

	// Token: 0x04001DED RID: 7661
	public Vector3 to = Vector3.one;

	// Token: 0x04001DEE RID: 7662
	public bool updateTable;

	// Token: 0x04001DEF RID: 7663
	private Transform mTrans;

	// Token: 0x04001DF0 RID: 7664
	private UITable mTable;
}
