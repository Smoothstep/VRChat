using System;
using UnityEngine;

// Token: 0x0200059D RID: 1437
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Button Color")]
public class UIButtonColor : UIWidgetContainer
{
	// Token: 0x17000748 RID: 1864
	// (get) Token: 0x06003010 RID: 12304 RVA: 0x000EB2FF File Offset: 0x000E96FF
	// (set) Token: 0x06003011 RID: 12305 RVA: 0x000EB307 File Offset: 0x000E9707
	public UIButtonColor.State state
	{
		get
		{
			return this.mState;
		}
		set
		{
			this.SetState(value, false);
		}
	}

	// Token: 0x17000749 RID: 1865
	// (get) Token: 0x06003012 RID: 12306 RVA: 0x000EB311 File Offset: 0x000E9711
	// (set) Token: 0x06003013 RID: 12307 RVA: 0x000EB32C File Offset: 0x000E972C
	public Color defaultColor
	{
		get
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			return this.mDefaultColor;
		}
		set
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			this.mDefaultColor = value;
			UIButtonColor.State state = this.mState;
			this.mState = UIButtonColor.State.Disabled;
			this.SetState(state, false);
		}
	}

	// Token: 0x1700074A RID: 1866
	// (get) Token: 0x06003014 RID: 12308 RVA: 0x000EB367 File Offset: 0x000E9767
	// (set) Token: 0x06003015 RID: 12309 RVA: 0x000EB36F File Offset: 0x000E976F
	public virtual bool isEnabled
	{
		get
		{
			return base.enabled;
		}
		set
		{
			base.enabled = value;
		}
	}

	// Token: 0x06003016 RID: 12310 RVA: 0x000EB378 File Offset: 0x000E9778
	public void ResetDefaultColor()
	{
		this.defaultColor = this.mStartingColor;
	}

	// Token: 0x06003017 RID: 12311 RVA: 0x000EB386 File Offset: 0x000E9786
	private void Awake()
	{
		if (!this.mInitDone)
		{
			this.OnInit();
		}
	}

	// Token: 0x06003018 RID: 12312 RVA: 0x000EB399 File Offset: 0x000E9799
	private void Start()
	{
		if (!this.isEnabled)
		{
			this.SetState(UIButtonColor.State.Disabled, true);
		}
	}

	// Token: 0x06003019 RID: 12313 RVA: 0x000EB3B0 File Offset: 0x000E97B0
	protected virtual void OnInit()
	{
		this.mInitDone = true;
		if (this.tweenTarget == null)
		{
			this.tweenTarget = base.gameObject;
		}
		this.mWidget = this.tweenTarget.GetComponent<UIWidget>();
		if (this.mWidget != null)
		{
			this.mDefaultColor = this.mWidget.color;
			this.mStartingColor = this.mDefaultColor;
		}
		else
		{
			Renderer component = this.tweenTarget.GetComponent<Renderer>();
			if (component != null)
			{
				this.mDefaultColor = ((!Application.isPlaying) ? component.sharedMaterial.color : component.material.color);
				this.mStartingColor = this.mDefaultColor;
			}
			else
			{
				Light component2 = this.tweenTarget.GetComponent<Light>();
				if (component2 != null)
				{
					this.mDefaultColor = component2.color;
					this.mStartingColor = this.mDefaultColor;
				}
				else
				{
					this.tweenTarget = null;
					this.mInitDone = false;
				}
			}
		}
	}

	// Token: 0x0600301A RID: 12314 RVA: 0x000EB4BC File Offset: 0x000E98BC
	protected virtual void OnEnable()
	{
		if (this.mInitDone)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
		if (UICamera.currentTouch != null)
		{
			if (UICamera.currentTouch.pressed == base.gameObject)
			{
				this.OnPress(true);
			}
			else if (UICamera.currentTouch.current == base.gameObject)
			{
				this.OnHover(true);
			}
		}
	}

	// Token: 0x0600301B RID: 12315 RVA: 0x000EB538 File Offset: 0x000E9938
	protected virtual void OnDisable()
	{
		if (this.mInitDone && this.tweenTarget != null)
		{
			this.SetState(UIButtonColor.State.Normal, true);
			TweenColor component = this.tweenTarget.GetComponent<TweenColor>();
			if (component != null)
			{
				component.value = this.mDefaultColor;
				component.enabled = false;
			}
		}
	}

	// Token: 0x0600301C RID: 12316 RVA: 0x000EB594 File Offset: 0x000E9994
	protected virtual void OnHover(bool isOver)
	{
		if (this.isEnabled)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				this.SetState((!isOver) ? UIButtonColor.State.Normal : UIButtonColor.State.Hover, false);
			}
		}
	}

	// Token: 0x0600301D RID: 12317 RVA: 0x000EB5E4 File Offset: 0x000E99E4
	protected virtual void OnPress(bool isPressed)
	{
		if (this.isEnabled && UICamera.currentTouch != null)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				if (isPressed)
				{
					this.SetState(UIButtonColor.State.Pressed, false);
				}
				else if (UICamera.currentTouch.current == base.gameObject)
				{
					if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
					{
						this.SetState(UIButtonColor.State.Hover, false);
					}
					else if (UICamera.currentScheme == UICamera.ControlScheme.Mouse && UICamera.hoveredObject == base.gameObject)
					{
						this.SetState(UIButtonColor.State.Hover, false);
					}
					else
					{
						this.SetState(UIButtonColor.State.Normal, false);
					}
				}
				else
				{
					this.SetState(UIButtonColor.State.Normal, false);
				}
			}
		}
	}

	// Token: 0x0600301E RID: 12318 RVA: 0x000EB6AE File Offset: 0x000E9AAE
	protected virtual void OnDragOver()
	{
		if (this.isEnabled)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				this.SetState(UIButtonColor.State.Pressed, false);
			}
		}
	}

	// Token: 0x0600301F RID: 12319 RVA: 0x000EB6E5 File Offset: 0x000E9AE5
	protected virtual void OnDragOut()
	{
		if (this.isEnabled)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				this.SetState(UIButtonColor.State.Normal, false);
			}
		}
	}

	// Token: 0x06003020 RID: 12320 RVA: 0x000EB71C File Offset: 0x000E9B1C
	protected virtual void OnSelect(bool isSelected)
	{
		if (this.isEnabled && this.tweenTarget != null)
		{
			if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
			{
				this.OnHover(isSelected);
			}
			else if (!isSelected && UICamera.touchCount < 2)
			{
				this.OnHover(isSelected);
			}
		}
	}

	// Token: 0x06003021 RID: 12321 RVA: 0x000EB774 File Offset: 0x000E9B74
	public virtual void SetState(UIButtonColor.State state, bool instant)
	{
		if (!this.mInitDone)
		{
			this.mInitDone = true;
			this.OnInit();
		}
		if (this.mState != state)
		{
			this.mState = state;
			this.UpdateColor(instant);
		}
	}

	// Token: 0x06003022 RID: 12322 RVA: 0x000EB7A8 File Offset: 0x000E9BA8
	public void UpdateColor(bool instant)
	{
		TweenColor tweenColor;
		switch (this.mState)
		{
		case UIButtonColor.State.Hover:
			tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.hover);
			break;
		case UIButtonColor.State.Pressed:
			tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.pressed);
			break;
		case UIButtonColor.State.Disabled:
			tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.disabledColor);
			break;
		default:
			tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.mDefaultColor);
			break;
		}
		if (instant && tweenColor != null)
		{
			tweenColor.value = tweenColor.to;
			tweenColor.enabled = false;
		}
	}

	// Token: 0x04001A84 RID: 6788
	public GameObject tweenTarget;

	// Token: 0x04001A85 RID: 6789
	public Color hover = new Color(0.882352948f, 0.784313738f, 0.5882353f, 1f);

	// Token: 0x04001A86 RID: 6790
	public Color pressed = new Color(0.7176471f, 0.6392157f, 0.482352942f, 1f);

	// Token: 0x04001A87 RID: 6791
	public Color disabledColor = Color.grey;

	// Token: 0x04001A88 RID: 6792
	public float duration = 0.2f;

	// Token: 0x04001A89 RID: 6793
	[NonSerialized]
	protected Color mStartingColor;

	// Token: 0x04001A8A RID: 6794
	[NonSerialized]
	protected Color mDefaultColor;

	// Token: 0x04001A8B RID: 6795
	[NonSerialized]
	protected bool mInitDone;

	// Token: 0x04001A8C RID: 6796
	[NonSerialized]
	protected UIWidget mWidget;

	// Token: 0x04001A8D RID: 6797
	[NonSerialized]
	protected UIButtonColor.State mState;

	// Token: 0x0200059E RID: 1438
	public enum State
	{
		// Token: 0x04001A8F RID: 6799
		Normal,
		// Token: 0x04001A90 RID: 6800
		Hover,
		// Token: 0x04001A91 RID: 6801
		Pressed,
		// Token: 0x04001A92 RID: 6802
		Disabled
	}
}
