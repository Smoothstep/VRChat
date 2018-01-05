using System;
using UnityEngine;

// Token: 0x02000620 RID: 1568
[AddComponentMenu("NGUI/Tween/Tween Position")]
public class TweenPosition : UITweener
{
	// Token: 0x170007F0 RID: 2032
	// (get) Token: 0x0600347A RID: 13434 RVA: 0x00109953 File Offset: 0x00107D53
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

	// Token: 0x170007F1 RID: 2033
	// (get) Token: 0x0600347B RID: 13435 RVA: 0x00109978 File Offset: 0x00107D78
	// (set) Token: 0x0600347C RID: 13436 RVA: 0x00109980 File Offset: 0x00107D80
	[Obsolete("Use 'value' instead")]
	public Vector3 position
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

	// Token: 0x170007F2 RID: 2034
	// (get) Token: 0x0600347D RID: 13437 RVA: 0x00109989 File Offset: 0x00107D89
	// (set) Token: 0x0600347E RID: 13438 RVA: 0x001099B4 File Offset: 0x00107DB4
	public Vector3 value
	{
		get
		{
			return (!this.worldSpace) ? this.cachedTransform.localPosition : this.cachedTransform.position;
		}
		set
		{
			if (this.mRect == null || !this.mRect.isAnchored || this.worldSpace)
			{
				if (this.worldSpace)
				{
					this.cachedTransform.position = value;
				}
				else
				{
					this.cachedTransform.localPosition = value;
				}
			}
			else
			{
				value -= this.cachedTransform.localPosition;
				NGUIMath.MoveRect(this.mRect, value.x, value.y);
			}
		}
	}

	// Token: 0x0600347F RID: 13439 RVA: 0x00109A46 File Offset: 0x00107E46
	private void Awake()
	{
		this.mRect = base.GetComponent<UIRect>();
	}

	// Token: 0x06003480 RID: 13440 RVA: 0x00109A54 File Offset: 0x00107E54
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
	}

	// Token: 0x06003481 RID: 13441 RVA: 0x00109A80 File Offset: 0x00107E80
	public static TweenPosition Begin(GameObject go, float duration, Vector3 pos)
	{
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(go, duration);
		tweenPosition.from = tweenPosition.value;
		tweenPosition.to = pos;
		if (duration <= 0f)
		{
			tweenPosition.Sample(1f, true);
			tweenPosition.enabled = false;
		}
		return tweenPosition;
	}

	// Token: 0x06003482 RID: 13442 RVA: 0x00109AC8 File Offset: 0x00107EC8
	public static TweenPosition Begin(GameObject go, float duration, Vector3 pos, bool worldSpace)
	{
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(go, duration);
		tweenPosition.worldSpace = worldSpace;
		tweenPosition.from = tweenPosition.value;
		tweenPosition.to = pos;
		if (duration <= 0f)
		{
			tweenPosition.Sample(1f, true);
			tweenPosition.enabled = false;
		}
		return tweenPosition;
	}

	// Token: 0x06003483 RID: 13443 RVA: 0x00109B16 File Offset: 0x00107F16
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06003484 RID: 13444 RVA: 0x00109B24 File Offset: 0x00107F24
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06003485 RID: 13445 RVA: 0x00109B32 File Offset: 0x00107F32
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06003486 RID: 13446 RVA: 0x00109B40 File Offset: 0x00107F40
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x04001DE4 RID: 7652
	public Vector3 from;

	// Token: 0x04001DE5 RID: 7653
	public Vector3 to;

	// Token: 0x04001DE6 RID: 7654
	[HideInInspector]
	public bool worldSpace;

	// Token: 0x04001DE7 RID: 7655
	private Transform mTrans;

	// Token: 0x04001DE8 RID: 7656
	private UIRect mRect;
}
