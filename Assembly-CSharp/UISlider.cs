using System;
using UnityEngine;

// Token: 0x020005D1 RID: 1489
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/NGUI Slider")]
public class UISlider : UIProgressBar
{
	// Token: 0x17000771 RID: 1905
	// (get) Token: 0x06003182 RID: 12674 RVA: 0x000F2A84 File Offset: 0x000F0E84
	// (set) Token: 0x06003183 RID: 12675 RVA: 0x000F2A8C File Offset: 0x000F0E8C
	[Obsolete("Use 'value' instead")]
	public float sliderValue
	{
		get
		{
			return base.value;
		}
		set
		{
			base.value = value;
		}
	}

	// Token: 0x17000772 RID: 1906
	// (get) Token: 0x06003184 RID: 12676 RVA: 0x000F2A95 File Offset: 0x000F0E95
	// (set) Token: 0x06003185 RID: 12677 RVA: 0x000F2A9D File Offset: 0x000F0E9D
	[Obsolete("Use 'fillDirection' instead")]
	public bool inverted
	{
		get
		{
			return base.isInverted;
		}
		set
		{
		}
	}

	// Token: 0x06003186 RID: 12678 RVA: 0x000F2AA0 File Offset: 0x000F0EA0
	protected override void Upgrade()
	{
		if (this.direction != UISlider.Direction.Upgraded)
		{
			this.mValue = this.rawValue;
			if (this.foreground != null)
			{
				this.mFG = this.foreground.GetComponent<UIWidget>();
			}
			if (this.direction == UISlider.Direction.Horizontal)
			{
				this.mFill = ((!this.mInverted) ? UIProgressBar.FillDirection.LeftToRight : UIProgressBar.FillDirection.RightToLeft);
			}
			else
			{
				this.mFill = ((!this.mInverted) ? UIProgressBar.FillDirection.BottomToTop : UIProgressBar.FillDirection.TopToBottom);
			}
			this.direction = UISlider.Direction.Upgraded;
		}
	}

	// Token: 0x06003187 RID: 12679 RVA: 0x000F2B30 File Offset: 0x000F0F30
	protected override void OnStart()
	{
		GameObject go = (!(this.mBG != null) || (!(this.mBG.GetComponent<Collider>() != null) && !(this.mBG.GetComponent<Collider2D>() != null))) ? base.gameObject : this.mBG.gameObject;
		UIEventListener uieventListener = UIEventListener.Get(go);
		UIEventListener uieventListener2 = uieventListener;
		uieventListener2.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener2.onPress, new UIEventListener.BoolDelegate(this.OnPressBackground));
		UIEventListener uieventListener3 = uieventListener;
		uieventListener3.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener3.onDrag, new UIEventListener.VectorDelegate(this.OnDragBackground));
		if (this.thumb != null && (this.thumb.GetComponent<Collider>() != null || this.thumb.GetComponent<Collider2D>() != null) && (this.mFG == null || this.thumb != this.mFG.cachedTransform))
		{
			UIEventListener uieventListener4 = UIEventListener.Get(this.thumb.gameObject);
			UIEventListener uieventListener5 = uieventListener4;
			uieventListener5.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener5.onPress, new UIEventListener.BoolDelegate(this.OnPressForeground));
			UIEventListener uieventListener6 = uieventListener4;
			uieventListener6.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener6.onDrag, new UIEventListener.VectorDelegate(this.OnDragForeground));
		}
	}

	// Token: 0x06003188 RID: 12680 RVA: 0x000F2C9C File Offset: 0x000F109C
	protected void OnPressBackground(GameObject go, bool isPressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = base.ScreenToValue(UICamera.lastTouchPosition);
		if (!isPressed && this.onDragFinished != null)
		{
			this.onDragFinished();
		}
	}

	// Token: 0x06003189 RID: 12681 RVA: 0x000F2CED File Offset: 0x000F10ED
	protected void OnDragBackground(GameObject go, Vector2 delta)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = base.ScreenToValue(UICamera.lastTouchPosition);
	}

	// Token: 0x0600318A RID: 12682 RVA: 0x000F2D18 File Offset: 0x000F1118
	protected void OnPressForeground(GameObject go, bool isPressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		if (isPressed)
		{
			this.mOffset = ((!(this.mFG == null)) ? (base.value - base.ScreenToValue(UICamera.lastTouchPosition)) : 0f);
		}
		else if (this.onDragFinished != null)
		{
			this.onDragFinished();
		}
	}

	// Token: 0x0600318B RID: 12683 RVA: 0x000F2D90 File Offset: 0x000F1190
	protected void OnDragForeground(GameObject go, Vector2 delta)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = this.mOffset + base.ScreenToValue(UICamera.lastTouchPosition);
	}

	// Token: 0x0600318C RID: 12684 RVA: 0x000F2DC4 File Offset: 0x000F11C4
	protected void OnKey(KeyCode key)
	{
		if (base.enabled)
		{
			float num = ((float)this.numberOfSteps <= 1f) ? 0.125f : (1f / (float)(this.numberOfSteps - 1));
			switch (this.mFill)
			{
			case UIProgressBar.FillDirection.LeftToRight:
				if (key == KeyCode.LeftArrow)
				{
					base.value = this.mValue - num;
				}
				else if (key == KeyCode.RightArrow)
				{
					base.value = this.mValue + num;
				}
				break;
			case UIProgressBar.FillDirection.RightToLeft:
				if (key == KeyCode.LeftArrow)
				{
					base.value = this.mValue + num;
				}
				else if (key == KeyCode.RightArrow)
				{
					base.value = this.mValue - num;
				}
				break;
			case UIProgressBar.FillDirection.BottomToTop:
				if (key == KeyCode.DownArrow)
				{
					base.value = this.mValue - num;
				}
				else if (key == KeyCode.UpArrow)
				{
					base.value = this.mValue + num;
				}
				break;
			case UIProgressBar.FillDirection.TopToBottom:
				if (key == KeyCode.DownArrow)
				{
					base.value = this.mValue + num;
				}
				else if (key == KeyCode.UpArrow)
				{
					base.value = this.mValue - num;
				}
				break;
			}
		}
	}

	// Token: 0x04001C0A RID: 7178
	[HideInInspector]
	[SerializeField]
	private Transform foreground;

	// Token: 0x04001C0B RID: 7179
	[HideInInspector]
	[SerializeField]
	private float rawValue = 1f;

	// Token: 0x04001C0C RID: 7180
	[HideInInspector]
	[SerializeField]
	private UISlider.Direction direction = UISlider.Direction.Upgraded;

	// Token: 0x04001C0D RID: 7181
	[HideInInspector]
	[SerializeField]
	protected bool mInverted;

	// Token: 0x020005D2 RID: 1490
	private enum Direction
	{
		// Token: 0x04001C0F RID: 7183
		Horizontal,
		// Token: 0x04001C10 RID: 7184
		Vertical,
		// Token: 0x04001C11 RID: 7185
		Upgraded
	}
}
