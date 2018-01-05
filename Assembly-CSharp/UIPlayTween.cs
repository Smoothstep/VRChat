using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x020005C1 RID: 1473
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Play Tween")]
public class UIPlayTween : MonoBehaviour
{
	// Token: 0x060030F2 RID: 12530 RVA: 0x000EFEF1 File Offset: 0x000EE2F1
	private void Awake()
	{
		if (this.eventReceiver != null && EventDelegate.IsValid(this.onFinished))
		{
			this.eventReceiver = null;
			this.callWhenFinished = null;
		}
	}

	// Token: 0x060030F3 RID: 12531 RVA: 0x000EFF22 File Offset: 0x000EE322
	private void Start()
	{
		this.mStarted = true;
		if (this.tweenTarget == null)
		{
			this.tweenTarget = base.gameObject;
		}
	}

	// Token: 0x060030F4 RID: 12532 RVA: 0x000EFF48 File Offset: 0x000EE348
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
		if (UICamera.currentTouch != null)
		{
			if (this.trigger == Trigger.OnPress || this.trigger == Trigger.OnPressTrue)
			{
				this.mActivated = (UICamera.currentTouch.pressed == base.gameObject);
			}
			if (this.trigger == Trigger.OnHover || this.trigger == Trigger.OnHoverTrue)
			{
				this.mActivated = (UICamera.currentTouch.current == base.gameObject);
			}
		}
		UIToggle component = base.GetComponent<UIToggle>();
		if (component != null)
		{
			EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnToggle));
		}
	}

	// Token: 0x060030F5 RID: 12533 RVA: 0x000F000C File Offset: 0x000EE40C
	private void OnDisable()
	{
		UIToggle component = base.GetComponent<UIToggle>();
		if (component != null)
		{
			EventDelegate.Remove(component.onChange, new EventDelegate.Callback(this.OnToggle));
		}
	}

	// Token: 0x060030F6 RID: 12534 RVA: 0x000F0044 File Offset: 0x000EE444
	private void OnDragOver()
	{
		if (this.trigger == Trigger.OnHover)
		{
			this.OnHover(true);
		}
	}

	// Token: 0x060030F7 RID: 12535 RVA: 0x000F005C File Offset: 0x000EE45C
	private void OnHover(bool isOver)
	{
		if (base.enabled && (this.trigger == Trigger.OnHover || (this.trigger == Trigger.OnHoverTrue && isOver) || (this.trigger == Trigger.OnHoverFalse && !isOver)))
		{
			this.mActivated = (isOver && this.trigger == Trigger.OnHover);
			this.Play(isOver);
		}
	}

	// Token: 0x060030F8 RID: 12536 RVA: 0x000F00C3 File Offset: 0x000EE4C3
	private void OnDragOut()
	{
		if (base.enabled && this.mActivated)
		{
			this.mActivated = false;
			this.Play(false);
		}
	}

	// Token: 0x060030F9 RID: 12537 RVA: 0x000F00EC File Offset: 0x000EE4EC
	private void OnPress(bool isPressed)
	{
		if (base.enabled && (this.trigger == Trigger.OnPress || (this.trigger == Trigger.OnPressTrue && isPressed) || (this.trigger == Trigger.OnPressFalse && !isPressed)))
		{
			this.mActivated = (isPressed && this.trigger == Trigger.OnPress);
			this.Play(isPressed);
		}
	}

	// Token: 0x060030FA RID: 12538 RVA: 0x000F0153 File Offset: 0x000EE553
	private void OnClick()
	{
		if (base.enabled && this.trigger == Trigger.OnClick)
		{
			this.Play(true);
		}
	}

	// Token: 0x060030FB RID: 12539 RVA: 0x000F0172 File Offset: 0x000EE572
	private void OnDoubleClick()
	{
		if (base.enabled && this.trigger == Trigger.OnDoubleClick)
		{
			this.Play(true);
		}
	}

	// Token: 0x060030FC RID: 12540 RVA: 0x000F0194 File Offset: 0x000EE594
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (this.trigger == Trigger.OnSelect || (this.trigger == Trigger.OnSelectTrue && isSelected) || (this.trigger == Trigger.OnSelectFalse && !isSelected)))
		{
			this.mActivated = (isSelected && this.trigger == Trigger.OnSelect);
			this.Play(isSelected);
		}
	}

	// Token: 0x060030FD RID: 12541 RVA: 0x000F0200 File Offset: 0x000EE600
	private void OnToggle()
	{
		if (!base.enabled || UIToggle.current == null)
		{
			return;
		}
		if (this.trigger == Trigger.OnActivate || (this.trigger == Trigger.OnActivateTrue && UIToggle.current.value) || (this.trigger == Trigger.OnActivateFalse && !UIToggle.current.value))
		{
			this.Play(UIToggle.current.value);
		}
	}

	// Token: 0x060030FE RID: 12542 RVA: 0x000F027C File Offset: 0x000EE67C
	private void Update()
	{
		if (this.disableWhenFinished != DisableCondition.DoNotDisable && this.mTweens != null)
		{
			bool flag = true;
			bool flag2 = true;
			int i = 0;
			int num = this.mTweens.Length;
			while (i < num)
			{
				UITweener uitweener = this.mTweens[i];
				if (uitweener.tweenGroup == this.tweenGroup)
				{
					if (uitweener.enabled)
					{
						flag = false;
						break;
					}
					if (uitweener.direction != (Direction)this.disableWhenFinished)
					{
						flag2 = false;
					}
				}
				i++;
			}
			if (flag)
			{
				if (flag2)
				{
					NGUITools.SetActive(this.tweenTarget, false);
				}
				this.mTweens = null;
			}
		}
	}

	// Token: 0x060030FF RID: 12543 RVA: 0x000F0328 File Offset: 0x000EE728
	public void Play(bool forward)
	{
		this.mActive = 0;
		GameObject gameObject = (!(this.tweenTarget == null)) ? this.tweenTarget : base.gameObject;
		if (!NGUITools.GetActive(gameObject))
		{
			if (this.ifDisabledOnPlay != EnableCondition.EnableThenPlay)
			{
				return;
			}
			NGUITools.SetActive(gameObject, true);
		}
		this.mTweens = ((!this.includeChildren) ? gameObject.GetComponents<UITweener>() : gameObject.GetComponentsInChildren<UITweener>());
		if (this.mTweens.Length == 0)
		{
			if (this.disableWhenFinished != DisableCondition.DoNotDisable)
			{
				NGUITools.SetActive(this.tweenTarget, false);
			}
		}
		else
		{
			bool flag = false;
			if (this.playDirection == Direction.Reverse)
			{
				forward = !forward;
			}
			int i = 0;
			int num = this.mTweens.Length;
			while (i < num)
			{
				UITweener uitweener = this.mTweens[i];
				if (uitweener.tweenGroup == this.tweenGroup)
				{
					if (!flag && !NGUITools.GetActive(gameObject))
					{
						flag = true;
						NGUITools.SetActive(gameObject, true);
					}
					this.mActive++;
					if (this.playDirection == Direction.Toggle)
					{
						EventDelegate.Add(uitweener.onFinished, new EventDelegate.Callback(this.OnFinished), true);
						uitweener.Toggle();
					}
					else
					{
						if (this.resetOnPlay || (this.resetIfDisabled && !uitweener.enabled))
						{
							uitweener.Play(forward);
							uitweener.ResetToBeginning();
						}
						EventDelegate.Add(uitweener.onFinished, new EventDelegate.Callback(this.OnFinished), true);
						uitweener.Play(forward);
					}
				}
				i++;
			}
		}
	}

	// Token: 0x06003100 RID: 12544 RVA: 0x000F04C0 File Offset: 0x000EE8C0
	private void OnFinished()
	{
		if (--this.mActive == 0 && UIPlayTween.current == null)
		{
			UIPlayTween.current = this;
			EventDelegate.Execute(this.onFinished);
			if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
			{
				this.eventReceiver.SendMessage(this.callWhenFinished, SendMessageOptions.DontRequireReceiver);
			}
			this.eventReceiver = null;
			UIPlayTween.current = null;
		}
	}

	// Token: 0x04001B7E RID: 7038
	public static UIPlayTween current;

	// Token: 0x04001B7F RID: 7039
	public GameObject tweenTarget;

	// Token: 0x04001B80 RID: 7040
	public int tweenGroup;

	// Token: 0x04001B81 RID: 7041
	public Trigger trigger;

	// Token: 0x04001B82 RID: 7042
	public Direction playDirection = Direction.Forward;

	// Token: 0x04001B83 RID: 7043
	public bool resetOnPlay;

	// Token: 0x04001B84 RID: 7044
	public bool resetIfDisabled;

	// Token: 0x04001B85 RID: 7045
	public EnableCondition ifDisabledOnPlay;

	// Token: 0x04001B86 RID: 7046
	public DisableCondition disableWhenFinished;

	// Token: 0x04001B87 RID: 7047
	public bool includeChildren;

	// Token: 0x04001B88 RID: 7048
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x04001B89 RID: 7049
	[HideInInspector]
	[SerializeField]
	private GameObject eventReceiver;

	// Token: 0x04001B8A RID: 7050
	[HideInInspector]
	[SerializeField]
	private string callWhenFinished;

	// Token: 0x04001B8B RID: 7051
	private UITweener[] mTweens;

	// Token: 0x04001B8C RID: 7052
	private bool mStarted;

	// Token: 0x04001B8D RID: 7053
	private int mActive;

	// Token: 0x04001B8E RID: 7054
	private bool mActivated;
}
