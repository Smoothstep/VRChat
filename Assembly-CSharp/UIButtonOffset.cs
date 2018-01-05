using System;
using UnityEngine;

// Token: 0x020005A2 RID: 1442
[AddComponentMenu("NGUI/Interaction/Button Offset")]
public class UIButtonOffset : MonoBehaviour
{
	// Token: 0x06003030 RID: 12336 RVA: 0x000EC5DC File Offset: 0x000EA9DC
	private void Start()
	{
		if (!this.mStarted)
		{
			this.mStarted = true;
			if (this.tweenTarget == null)
			{
				this.tweenTarget = base.transform;
			}
			this.mPos = this.tweenTarget.localPosition;
		}
	}

	// Token: 0x06003031 RID: 12337 RVA: 0x000EC629 File Offset: 0x000EAA29
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x06003032 RID: 12338 RVA: 0x000EC648 File Offset: 0x000EAA48
	private void OnDisable()
	{
		if (this.mStarted && this.tweenTarget != null)
		{
			TweenPosition component = this.tweenTarget.GetComponent<TweenPosition>();
			if (component != null)
			{
				component.value = this.mPos;
				component.enabled = false;
			}
		}
	}

	// Token: 0x06003033 RID: 12339 RVA: 0x000EC69C File Offset: 0x000EAA9C
	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, (!isPressed) ? ((!UICamera.IsHighlighted(base.gameObject)) ? this.mPos : (this.mPos + this.hover)) : (this.mPos + this.pressed)).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06003034 RID: 12340 RVA: 0x000EC72C File Offset: 0x000EAB2C
	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, (!isOver) ? this.mPos : (this.mPos + this.hover)).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06003035 RID: 12341 RVA: 0x000EC793 File Offset: 0x000EAB93
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x04001AA4 RID: 6820
	public Transform tweenTarget;

	// Token: 0x04001AA5 RID: 6821
	public Vector3 hover = Vector3.zero;

	// Token: 0x04001AA6 RID: 6822
	public Vector3 pressed = new Vector3(2f, -2f);

	// Token: 0x04001AA7 RID: 6823
	public float duration = 0.2f;

	// Token: 0x04001AA8 RID: 6824
	private Vector3 mPos;

	// Token: 0x04001AA9 RID: 6825
	private bool mStarted;
}
