using System;
using UnityEngine;

// Token: 0x0200061C RID: 1564
[AddComponentMenu("NGUI/Tween/Tween Color")]
public class TweenColor : UITweener
{
	// Token: 0x0600344C RID: 13388 RVA: 0x00109330 File Offset: 0x00107730
	private void Cache()
	{
		this.mCached = true;
		this.mWidget = base.GetComponent<UIWidget>();
		if (this.mWidget != null)
		{
			return;
		}
		this.mSr = base.GetComponent<SpriteRenderer>();
		if (this.mSr != null)
		{
			return;
		}
		Renderer component = base.GetComponent<Renderer>();
		if (component != null)
		{
			this.mMat = component.material;
			return;
		}
		this.mLight = base.GetComponent<Light>();
		if (this.mLight == null)
		{
			this.mWidget = base.GetComponentInChildren<UIWidget>();
		}
	}

	// Token: 0x170007E5 RID: 2021
	// (get) Token: 0x0600344D RID: 13389 RVA: 0x001093C9 File Offset: 0x001077C9
	// (set) Token: 0x0600344E RID: 13390 RVA: 0x001093D1 File Offset: 0x001077D1
	[Obsolete("Use 'value' instead")]
	public Color color
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

	// Token: 0x170007E6 RID: 2022
	// (get) Token: 0x0600344F RID: 13391 RVA: 0x001093DC File Offset: 0x001077DC
	// (set) Token: 0x06003450 RID: 13392 RVA: 0x00109474 File Offset: 0x00107874
	public Color value
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mWidget != null)
			{
				return this.mWidget.color;
			}
			if (this.mMat != null)
			{
				return this.mMat.color;
			}
			if (this.mSr != null)
			{
				return this.mSr.color;
			}
			if (this.mLight != null)
			{
				return this.mLight.color;
			}
			return Color.black;
		}
		set
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mWidget != null)
			{
				this.mWidget.color = value;
			}
			else if (this.mMat != null)
			{
				this.mMat.color = value;
			}
			else if (this.mSr != null)
			{
				this.mSr.color = value;
			}
			else if (this.mLight != null)
			{
				this.mLight.color = value;
				this.mLight.enabled = (value.r + value.g + value.b > 0.01f);
			}
		}
	}

	// Token: 0x06003451 RID: 13393 RVA: 0x0010953E File Offset: 0x0010793E
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Color.Lerp(this.from, this.to, factor);
	}

	// Token: 0x06003452 RID: 13394 RVA: 0x00109558 File Offset: 0x00107958
	public static TweenColor Begin(GameObject go, float duration, Color color)
	{
		TweenColor tweenColor = UITweener.Begin<TweenColor>(go, duration);
		tweenColor.from = tweenColor.value;
		tweenColor.to = color;
		if (duration <= 0f)
		{
			tweenColor.Sample(1f, true);
			tweenColor.enabled = false;
		}
		return tweenColor;
	}

	// Token: 0x06003453 RID: 13395 RVA: 0x0010959F File Offset: 0x0010799F
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06003454 RID: 13396 RVA: 0x001095AD File Offset: 0x001079AD
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06003455 RID: 13397 RVA: 0x001095BB File Offset: 0x001079BB
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06003456 RID: 13398 RVA: 0x001095C9 File Offset: 0x001079C9
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x04001DD2 RID: 7634
	public Color from = Color.white;

	// Token: 0x04001DD3 RID: 7635
	public Color to = Color.white;

	// Token: 0x04001DD4 RID: 7636
	private bool mCached;

	// Token: 0x04001DD5 RID: 7637
	private UIWidget mWidget;

	// Token: 0x04001DD6 RID: 7638
	private Material mMat;

	// Token: 0x04001DD7 RID: 7639
	private Light mLight;

	// Token: 0x04001DD8 RID: 7640
	private SpriteRenderer mSr;
}
