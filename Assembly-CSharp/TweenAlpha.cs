using System;
using UnityEngine;

// Token: 0x0200061B RID: 1563
[AddComponentMenu("NGUI/Tween/Tween Alpha")]
public class TweenAlpha : UITweener
{
	// Token: 0x170007E3 RID: 2019
	// (get) Token: 0x06003442 RID: 13378 RVA: 0x001090BB File Offset: 0x001074BB
	// (set) Token: 0x06003443 RID: 13379 RVA: 0x001090C3 File Offset: 0x001074C3
	[Obsolete("Use 'value' instead")]
	public float alpha
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

	// Token: 0x06003444 RID: 13380 RVA: 0x001090CC File Offset: 0x001074CC
	private void Cache()
	{
		this.mCached = true;
		this.mRect = base.GetComponent<UIRect>();
		this.mSr = base.GetComponent<SpriteRenderer>();
		if (this.mRect == null && this.mSr == null)
		{
			Renderer component = base.GetComponent<Renderer>();
			if (component != null)
			{
				this.mMat = component.material;
			}
			if (this.mMat == null)
			{
				this.mRect = base.GetComponentInChildren<UIRect>();
			}
		}
	}

	// Token: 0x170007E4 RID: 2020
	// (get) Token: 0x06003445 RID: 13381 RVA: 0x00109158 File Offset: 0x00107558
	// (set) Token: 0x06003446 RID: 13382 RVA: 0x001091E8 File Offset: 0x001075E8
	public float value
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mRect != null)
			{
				return this.mRect.alpha;
			}
			if (this.mSr != null)
			{
				return this.mSr.color.a;
			}
			return (!(this.mMat != null)) ? 1f : this.mMat.color.a;
		}
		set
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mRect != null)
			{
				this.mRect.alpha = value;
			}
			else if (this.mSr != null)
			{
				Color color = this.mSr.color;
				color.a = value;
				this.mSr.color = color;
			}
			else if (this.mMat != null)
			{
				Color color2 = this.mMat.color;
				color2.a = value;
				this.mMat.color = color2;
			}
		}
	}

	// Token: 0x06003447 RID: 13383 RVA: 0x0010928F File Offset: 0x0010768F
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Mathf.Lerp(this.from, this.to, factor);
	}

	// Token: 0x06003448 RID: 13384 RVA: 0x001092AC File Offset: 0x001076AC
	public static TweenAlpha Begin(GameObject go, float duration, float alpha)
	{
		TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(go, duration);
		tweenAlpha.from = tweenAlpha.value;
		tweenAlpha.to = alpha;
		if (duration <= 0f)
		{
			tweenAlpha.Sample(1f, true);
			tweenAlpha.enabled = false;
		}
		return tweenAlpha;
	}

	// Token: 0x06003449 RID: 13385 RVA: 0x001092F3 File Offset: 0x001076F3
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x0600344A RID: 13386 RVA: 0x00109301 File Offset: 0x00107701
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x04001DCC RID: 7628
	[Range(0f, 1f)]
	public float from = 1f;

	// Token: 0x04001DCD RID: 7629
	[Range(0f, 1f)]
	public float to = 1f;

	// Token: 0x04001DCE RID: 7630
	private bool mCached;

	// Token: 0x04001DCF RID: 7631
	private UIRect mRect;

	// Token: 0x04001DD0 RID: 7632
	private Material mMat;

	// Token: 0x04001DD1 RID: 7633
	private SpriteRenderer mSr;
}
