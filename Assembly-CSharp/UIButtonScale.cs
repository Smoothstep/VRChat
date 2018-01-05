using System;
using UnityEngine;

// Token: 0x020005A4 RID: 1444
[AddComponentMenu("NGUI/Interaction/Button Scale")]
public class UIButtonScale : MonoBehaviour
{
	// Token: 0x0600303E RID: 12350 RVA: 0x000ECA24 File Offset: 0x000EAE24
	private void Start()
	{
		if (!this.mStarted)
		{
			this.mStarted = true;
			if (this.tweenTarget == null)
			{
				this.tweenTarget = base.transform;
			}
			this.mScale = this.tweenTarget.localScale;
		}
	}

	// Token: 0x0600303F RID: 12351 RVA: 0x000ECA71 File Offset: 0x000EAE71
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x06003040 RID: 12352 RVA: 0x000ECA90 File Offset: 0x000EAE90
	private void OnDisable()
	{
		if (this.mStarted && this.tweenTarget != null)
		{
			TweenScale component = this.tweenTarget.GetComponent<TweenScale>();
			if (component != null)
			{
				component.value = this.mScale;
				component.enabled = false;
			}
		}
	}

	// Token: 0x06003041 RID: 12353 RVA: 0x000ECAE4 File Offset: 0x000EAEE4
	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenScale.Begin(this.tweenTarget.gameObject, this.duration, (!isPressed) ? ((!UICamera.IsHighlighted(base.gameObject)) ? this.mScale : Vector3.Scale(this.mScale, this.hover)) : Vector3.Scale(this.mScale, this.pressed)).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06003042 RID: 12354 RVA: 0x000ECB74 File Offset: 0x000EAF74
	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenScale.Begin(this.tweenTarget.gameObject, this.duration, (!isOver) ? this.mScale : Vector3.Scale(this.mScale, this.hover)).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06003043 RID: 12355 RVA: 0x000ECBDB File Offset: 0x000EAFDB
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x04001AB0 RID: 6832
	public Transform tweenTarget;

	// Token: 0x04001AB1 RID: 6833
	public Vector3 hover = new Vector3(1.1f, 1.1f, 1.1f);

	// Token: 0x04001AB2 RID: 6834
	public Vector3 pressed = new Vector3(1.05f, 1.05f, 1.05f);

	// Token: 0x04001AB3 RID: 6835
	public float duration = 0.2f;

	// Token: 0x04001AB4 RID: 6836
	private Vector3 mScale;

	// Token: 0x04001AB5 RID: 6837
	private bool mStarted;
}
