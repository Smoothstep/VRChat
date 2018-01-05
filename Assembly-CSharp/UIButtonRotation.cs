using System;
using UnityEngine;

// Token: 0x020005A3 RID: 1443
[AddComponentMenu("NGUI/Interaction/Button Rotation")]
public class UIButtonRotation : MonoBehaviour
{
	// Token: 0x06003037 RID: 12343 RVA: 0x000EC7E4 File Offset: 0x000EABE4
	private void Start()
	{
		if (!this.mStarted)
		{
			this.mStarted = true;
			if (this.tweenTarget == null)
			{
				this.tweenTarget = base.transform;
			}
			this.mRot = this.tweenTarget.localRotation;
		}
	}

	// Token: 0x06003038 RID: 12344 RVA: 0x000EC831 File Offset: 0x000EAC31
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x06003039 RID: 12345 RVA: 0x000EC850 File Offset: 0x000EAC50
	private void OnDisable()
	{
		if (this.mStarted && this.tweenTarget != null)
		{
			TweenRotation component = this.tweenTarget.GetComponent<TweenRotation>();
			if (component != null)
			{
				component.value = this.mRot;
				component.enabled = false;
			}
		}
	}

	// Token: 0x0600303A RID: 12346 RVA: 0x000EC8A4 File Offset: 0x000EACA4
	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenRotation.Begin(this.tweenTarget.gameObject, this.duration, (!isPressed) ? ((!UICamera.IsHighlighted(base.gameObject)) ? this.mRot : (this.mRot * Quaternion.Euler(this.hover))) : (this.mRot * Quaternion.Euler(this.pressed))).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x0600303B RID: 12347 RVA: 0x000EC93C File Offset: 0x000EAD3C
	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenRotation.Begin(this.tweenTarget.gameObject, this.duration, (!isOver) ? this.mRot : (this.mRot * Quaternion.Euler(this.hover))).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x0600303C RID: 12348 RVA: 0x000EC9A8 File Offset: 0x000EADA8
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x04001AAA RID: 6826
	public Transform tweenTarget;

	// Token: 0x04001AAB RID: 6827
	public Vector3 hover = Vector3.zero;

	// Token: 0x04001AAC RID: 6828
	public Vector3 pressed = Vector3.zero;

	// Token: 0x04001AAD RID: 6829
	public float duration = 0.2f;

	// Token: 0x04001AAE RID: 6830
	private Quaternion mRot;

	// Token: 0x04001AAF RID: 6831
	private bool mStarted;
}
