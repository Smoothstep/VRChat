using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x020005BE RID: 1470
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Play Animation")]
public class UIPlayAnimation : MonoBehaviour
{
	// Token: 0x17000750 RID: 1872
	// (get) Token: 0x060030D6 RID: 12502 RVA: 0x000EF637 File Offset: 0x000EDA37
	private bool dualState
	{
		get
		{
			return this.trigger == Trigger.OnPress || this.trigger == Trigger.OnHover;
		}
	}

	// Token: 0x060030D7 RID: 12503 RVA: 0x000EF654 File Offset: 0x000EDA54
	private void Awake()
	{
		UIButton component = base.GetComponent<UIButton>();
		if (component != null)
		{
			this.dragHighlight = component.dragHighlight;
		}
		if (this.eventReceiver != null && EventDelegate.IsValid(this.onFinished))
		{
			this.eventReceiver = null;
			this.callWhenFinished = null;
		}
	}

	// Token: 0x060030D8 RID: 12504 RVA: 0x000EF6B0 File Offset: 0x000EDAB0
	private void Start()
	{
		this.mStarted = true;
		if (this.target == null && this.animator == null)
		{
			this.animator = base.GetComponentInChildren<Animator>();
		}
		if (this.animator != null)
		{
			if (this.animator.enabled)
			{
				this.animator.enabled = false;
			}
			return;
		}
		if (this.target == null)
		{
			this.target = base.GetComponentInChildren<Animation>();
		}
		if (this.target != null && this.target.enabled)
		{
			this.target.enabled = false;
		}
	}

	// Token: 0x060030D9 RID: 12505 RVA: 0x000EF76C File Offset: 0x000EDB6C
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

	// Token: 0x060030DA RID: 12506 RVA: 0x000EF830 File Offset: 0x000EDC30
	private void OnDisable()
	{
		UIToggle component = base.GetComponent<UIToggle>();
		if (component != null)
		{
			EventDelegate.Remove(component.onChange, new EventDelegate.Callback(this.OnToggle));
		}
	}

	// Token: 0x060030DB RID: 12507 RVA: 0x000EF868 File Offset: 0x000EDC68
	private void OnHover(bool isOver)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.trigger == Trigger.OnHover || (this.trigger == Trigger.OnHoverTrue && isOver) || (this.trigger == Trigger.OnHoverFalse && !isOver))
		{
			this.Play(isOver, this.dualState);
		}
	}

	// Token: 0x060030DC RID: 12508 RVA: 0x000EF8C0 File Offset: 0x000EDCC0
	private void OnPress(bool isPressed)
	{
		if (!base.enabled)
		{
			return;
		}
		if (UICamera.currentTouchID < -1)
		{
			return;
		}
		if (this.trigger == Trigger.OnPress || (this.trigger == Trigger.OnPressTrue && isPressed) || (this.trigger == Trigger.OnPressFalse && !isPressed))
		{
			this.Play(isPressed, this.dualState);
		}
	}

	// Token: 0x060030DD RID: 12509 RVA: 0x000EF922 File Offset: 0x000EDD22
	private void OnClick()
	{
		if (UICamera.currentTouchID < -1)
		{
			return;
		}
		if (base.enabled && this.trigger == Trigger.OnClick)
		{
			this.Play(true, false);
		}
	}

	// Token: 0x060030DE RID: 12510 RVA: 0x000EF94E File Offset: 0x000EDD4E
	private void OnDoubleClick()
	{
		if (UICamera.currentTouchID < -1)
		{
			return;
		}
		if (base.enabled && this.trigger == Trigger.OnDoubleClick)
		{
			this.Play(true, false);
		}
	}

	// Token: 0x060030DF RID: 12511 RVA: 0x000EF97C File Offset: 0x000EDD7C
	private void OnSelect(bool isSelected)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.trigger == Trigger.OnSelect || (this.trigger == Trigger.OnSelectTrue && isSelected) || (this.trigger == Trigger.OnSelectFalse && !isSelected))
		{
			this.Play(isSelected, this.dualState);
		}
	}

	// Token: 0x060030E0 RID: 12512 RVA: 0x000EF9D8 File Offset: 0x000EDDD8
	private void OnToggle()
	{
		if (!base.enabled || UIToggle.current == null)
		{
			return;
		}
		if (this.trigger == Trigger.OnActivate || (this.trigger == Trigger.OnActivateTrue && UIToggle.current.value) || (this.trigger == Trigger.OnActivateFalse && !UIToggle.current.value))
		{
			this.Play(UIToggle.current.value, this.dualState);
		}
	}

	// Token: 0x060030E1 RID: 12513 RVA: 0x000EFA5C File Offset: 0x000EDE5C
	private void OnDragOver()
	{
		if (base.enabled && this.dualState)
		{
			if (UICamera.currentTouch.dragged == base.gameObject)
			{
				this.Play(true, true);
			}
			else if (this.dragHighlight && this.trigger == Trigger.OnPress)
			{
				this.Play(true, true);
			}
		}
	}

	// Token: 0x060030E2 RID: 12514 RVA: 0x000EFAC5 File Offset: 0x000EDEC5
	private void OnDragOut()
	{
		if (base.enabled && this.dualState && UICamera.hoveredObject != base.gameObject)
		{
			this.Play(false, true);
		}
	}

	// Token: 0x060030E3 RID: 12515 RVA: 0x000EFAFA File Offset: 0x000EDEFA
	private void OnDrop(GameObject go)
	{
		if (base.enabled && this.trigger == Trigger.OnPress && UICamera.currentTouch.dragged != base.gameObject)
		{
			this.Play(false, true);
		}
	}

	// Token: 0x060030E4 RID: 12516 RVA: 0x000EFB35 File Offset: 0x000EDF35
	public void Play(bool forward)
	{
		this.Play(forward, true);
	}

	// Token: 0x060030E5 RID: 12517 RVA: 0x000EFB40 File Offset: 0x000EDF40
	public void Play(bool forward, bool onlyIfDifferent)
	{
		if (this.target || this.animator)
		{
			if (onlyIfDifferent)
			{
				if (this.mActivated == forward)
				{
					return;
				}
				this.mActivated = forward;
			}
			if (this.clearSelection && UICamera.selectedObject == base.gameObject)
			{
				UICamera.selectedObject = null;
			}
			int num = (int)(-(int)this.playDirection);
			Direction direction = (Direction)((!forward) ? num : ((int)this.playDirection));
			ActiveAnimation activeAnimation = (!this.target) ? ActiveAnimation.Play(this.animator, this.clipName, direction, this.ifDisabledOnPlay, this.disableWhenFinished) : ActiveAnimation.Play(this.target, this.clipName, direction, this.ifDisabledOnPlay, this.disableWhenFinished);
			if (activeAnimation != null)
			{
				if (this.resetOnPlay)
				{
					activeAnimation.Reset();
				}
				for (int i = 0; i < this.onFinished.Count; i++)
				{
					EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinished), true);
				}
			}
		}
	}

	// Token: 0x060030E6 RID: 12518 RVA: 0x000EFC6C File Offset: 0x000EE06C
	private void OnFinished()
	{
		if (UIPlayAnimation.current == null)
		{
			UIPlayAnimation.current = this;
			EventDelegate.Execute(this.onFinished);
			if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
			{
				this.eventReceiver.SendMessage(this.callWhenFinished, SendMessageOptions.DontRequireReceiver);
			}
			this.eventReceiver = null;
			UIPlayAnimation.current = null;
		}
	}

	// Token: 0x04001B60 RID: 7008
	public static UIPlayAnimation current;

	// Token: 0x04001B61 RID: 7009
	public Animation target;

	// Token: 0x04001B62 RID: 7010
	public Animator animator;

	// Token: 0x04001B63 RID: 7011
	public string clipName;

	// Token: 0x04001B64 RID: 7012
	public Trigger trigger;

	// Token: 0x04001B65 RID: 7013
	public Direction playDirection = Direction.Forward;

	// Token: 0x04001B66 RID: 7014
	public bool resetOnPlay;

	// Token: 0x04001B67 RID: 7015
	public bool clearSelection;

	// Token: 0x04001B68 RID: 7016
	public EnableCondition ifDisabledOnPlay;

	// Token: 0x04001B69 RID: 7017
	public DisableCondition disableWhenFinished;

	// Token: 0x04001B6A RID: 7018
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x04001B6B RID: 7019
	[HideInInspector]
	[SerializeField]
	private GameObject eventReceiver;

	// Token: 0x04001B6C RID: 7020
	[HideInInspector]
	[SerializeField]
	private string callWhenFinished;

	// Token: 0x04001B6D RID: 7021
	private bool mStarted;

	// Token: 0x04001B6E RID: 7022
	private bool mActivated;

	// Token: 0x04001B6F RID: 7023
	private bool dragHighlight;
}
