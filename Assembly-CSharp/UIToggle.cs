using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x020005D8 RID: 1496
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Toggle")]
public class UIToggle : UIWidgetContainer
{
	// Token: 0x17000774 RID: 1908
	// (get) Token: 0x0600319F RID: 12703 RVA: 0x000F561F File Offset: 0x000F3A1F
	// (set) Token: 0x060031A0 RID: 12704 RVA: 0x000F5640 File Offset: 0x000F3A40
	public bool value
	{
		get
		{
			return (!this.mStarted) ? this.startsActive : this.mIsActive;
		}
		set
		{
			if (!this.mStarted)
			{
				this.startsActive = value;
			}
			else if (this.group == 0 || value || this.optionCanBeNone || !this.mStarted)
			{
				this.Set(value);
			}
		}
	}

	// Token: 0x17000775 RID: 1909
	// (get) Token: 0x060031A1 RID: 12705 RVA: 0x000F5692 File Offset: 0x000F3A92
	// (set) Token: 0x060031A2 RID: 12706 RVA: 0x000F569A File Offset: 0x000F3A9A
	[Obsolete("Use 'value' instead")]
	public bool isChecked
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

	// Token: 0x060031A3 RID: 12707 RVA: 0x000F56A4 File Offset: 0x000F3AA4
	public static UIToggle GetActiveToggle(int group)
	{
		for (int i = 0; i < UIToggle.list.size; i++)
		{
			UIToggle uitoggle = UIToggle.list[i];
			if (uitoggle != null && uitoggle.group == group && uitoggle.mIsActive)
			{
				return uitoggle;
			}
		}
		return null;
	}

	// Token: 0x060031A4 RID: 12708 RVA: 0x000F56FE File Offset: 0x000F3AFE
	private void OnEnable()
	{
		UIToggle.list.Add(this);
	}

	// Token: 0x060031A5 RID: 12709 RVA: 0x000F570B File Offset: 0x000F3B0B
	private void OnDisable()
	{
		UIToggle.list.Remove(this);
	}

	// Token: 0x060031A6 RID: 12710 RVA: 0x000F571C File Offset: 0x000F3B1C
	private void Start()
	{
		if (this.startsChecked)
		{
			this.startsChecked = false;
			this.startsActive = true;
		}
		if (!Application.isPlaying)
		{
			if (this.checkSprite != null && this.activeSprite == null)
			{
				this.activeSprite = this.checkSprite;
				this.checkSprite = null;
			}
			if (this.checkAnimation != null && this.activeAnimation == null)
			{
				this.activeAnimation = this.checkAnimation;
				this.checkAnimation = null;
			}
			if (Application.isPlaying && this.activeSprite != null)
			{
				this.activeSprite.alpha = ((!this.startsActive) ? 0f : 1f);
			}
			if (EventDelegate.IsValid(this.onChange))
			{
				this.eventReceiver = null;
				this.functionName = null;
			}
		}
		else
		{
			this.mIsActive = !this.startsActive;
			this.mStarted = true;
			bool flag = this.instantTween;
			this.instantTween = true;
			this.Set(this.startsActive);
			this.instantTween = flag;
		}
	}

	// Token: 0x060031A7 RID: 12711 RVA: 0x000F5850 File Offset: 0x000F3C50
	private void OnClick()
	{
		if (base.enabled)
		{
			this.value = !this.value;
		}
	}

	// Token: 0x060031A8 RID: 12712 RVA: 0x000F586C File Offset: 0x000F3C6C
	private void Set(bool state)
	{
		if (!this.mStarted)
		{
			this.mIsActive = state;
			this.startsActive = state;
			if (this.activeSprite != null)
			{
				this.activeSprite.alpha = ((!state) ? 0f : 1f);
			}
		}
		else if (this.mIsActive != state)
		{
			if (this.group != 0 && state)
			{
				int i = 0;
				int size = UIToggle.list.size;
				while (i < size)
				{
					UIToggle uitoggle = UIToggle.list[i];
					if (uitoggle != this && uitoggle.group == this.group)
					{
						uitoggle.Set(false);
					}
					if (UIToggle.list.size != size)
					{
						size = UIToggle.list.size;
						i = 0;
					}
					else
					{
						i++;
					}
				}
			}
			this.mIsActive = state;
			if (this.activeSprite != null)
			{
				if (this.instantTween || !NGUITools.GetActive(this))
				{
					this.activeSprite.alpha = ((!this.mIsActive) ? 0f : 1f);
				}
				else
				{
					TweenAlpha.Begin(this.activeSprite.gameObject, 0.15f, (!this.mIsActive) ? 0f : 1f);
				}
			}
			if (UIToggle.current == null)
			{
				UIToggle uitoggle2 = UIToggle.current;
				UIToggle.current = this;
				if (EventDelegate.IsValid(this.onChange))
				{
					EventDelegate.Execute(this.onChange);
				}
				else if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName))
				{
					this.eventReceiver.SendMessage(this.functionName, this.mIsActive, SendMessageOptions.DontRequireReceiver);
				}
				UIToggle.current = uitoggle2;
			}
			if (this.activeAnimation != null)
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(this.activeAnimation, null, (!state) ? Direction.Reverse : Direction.Forward, EnableCondition.IgnoreDisabledState, DisableCondition.DoNotDisable);
				if (activeAnimation != null && (this.instantTween || !NGUITools.GetActive(this)))
				{
					activeAnimation.Finish();
				}
			}
		}
	}

	// Token: 0x04001C2C RID: 7212
	public static BetterList<UIToggle> list = new BetterList<UIToggle>();

	// Token: 0x04001C2D RID: 7213
	public static UIToggle current;

	// Token: 0x04001C2E RID: 7214
	public int group;

	// Token: 0x04001C2F RID: 7215
	public UIWidget activeSprite;

	// Token: 0x04001C30 RID: 7216
	public Animation activeAnimation;

	// Token: 0x04001C31 RID: 7217
	public bool startsActive;

	// Token: 0x04001C32 RID: 7218
	public bool instantTween;

	// Token: 0x04001C33 RID: 7219
	public bool optionCanBeNone;

	// Token: 0x04001C34 RID: 7220
	public List<EventDelegate> onChange = new List<EventDelegate>();

	// Token: 0x04001C35 RID: 7221
	[HideInInspector]
	[SerializeField]
	private UISprite checkSprite;

	// Token: 0x04001C36 RID: 7222
	[HideInInspector]
	[SerializeField]
	private Animation checkAnimation;

	// Token: 0x04001C37 RID: 7223
	[HideInInspector]
	[SerializeField]
	private GameObject eventReceiver;

	// Token: 0x04001C38 RID: 7224
	[HideInInspector]
	[SerializeField]
	private string functionName = "OnActivate";

	// Token: 0x04001C39 RID: 7225
	[HideInInspector]
	[SerializeField]
	private bool startsChecked;

	// Token: 0x04001C3A RID: 7226
	private bool mIsActive = true;

	// Token: 0x04001C3B RID: 7227
	private bool mStarted;
}
