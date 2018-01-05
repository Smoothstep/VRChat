using System;
using UnityEngine;

// Token: 0x020005B9 RID: 1465
[AddComponentMenu("NGUI/Interaction/Key Binding")]
public class UIKeyBinding : MonoBehaviour
{
	// Token: 0x060030C5 RID: 12485 RVA: 0x000EF3B4 File Offset: 0x000ED7B4
	private void Start()
	{
		UIInput component = base.GetComponent<UIInput>();
		this.mIsInput = (component != null);
		if (component != null)
		{
			EventDelegate.Add(component.onSubmit, new EventDelegate.Callback(this.OnSubmit));
		}
	}

	// Token: 0x060030C6 RID: 12486 RVA: 0x000EF3F9 File Offset: 0x000ED7F9
	private void OnSubmit()
	{
		if (UICamera.currentKey == this.keyCode && this.IsModifierActive())
		{
			this.mIgnoreUp = true;
		}
	}

	// Token: 0x060030C7 RID: 12487 RVA: 0x000EF420 File Offset: 0x000ED820
	private bool IsModifierActive()
	{
		if (this.modifier == UIKeyBinding.Modifier.None)
		{
			return true;
		}
		if (this.modifier == UIKeyBinding.Modifier.Alt)
		{
			if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
			{
				return true;
			}
		}
		else if (this.modifier == UIKeyBinding.Modifier.Control)
		{
			if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			{
				return true;
			}
		}
		else if (this.modifier == UIKeyBinding.Modifier.Shift && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
		{
			return true;
		}
		return false;
	}

	// Token: 0x060030C8 RID: 12488 RVA: 0x000EF4CC File Offset: 0x000ED8CC
	private void Update()
	{
		if (this.keyCode == KeyCode.None || !this.IsModifierActive())
		{
			return;
		}
		if (this.action == UIKeyBinding.Action.PressAndClick || this.action == UIKeyBinding.Action.All)
		{
			if (UICamera.inputHasFocus)
			{
				return;
			}
			UICamera.currentTouch = UICamera.controller;
			UICamera.currentScheme = UICamera.ControlScheme.Mouse;
			UICamera.currentTouch.current = base.gameObject;
			if (Input.GetKeyDown(this.keyCode))
			{
				this.mPress = true;
				UICamera.Notify(base.gameObject, "OnPress", true);
			}
			if (Input.GetKeyUp(this.keyCode))
			{
				UICamera.Notify(base.gameObject, "OnPress", false);
				if (this.mPress)
				{
					UICamera.Notify(base.gameObject, "OnClick", null);
					this.mPress = false;
				}
			}
			UICamera.currentTouch.current = null;
		}
		if ((this.action == UIKeyBinding.Action.Select || this.action == UIKeyBinding.Action.All) && Input.GetKeyUp(this.keyCode))
		{
			if (this.mIsInput)
			{
				if (!this.mIgnoreUp && !UICamera.inputHasFocus)
				{
					UICamera.selectedObject = base.gameObject;
				}
				this.mIgnoreUp = false;
			}
			else
			{
				UICamera.selectedObject = base.gameObject;
			}
		}
	}

	// Token: 0x04001B44 RID: 6980
	public KeyCode keyCode;

	// Token: 0x04001B45 RID: 6981
	public UIKeyBinding.Modifier modifier;

	// Token: 0x04001B46 RID: 6982
	public UIKeyBinding.Action action;

	// Token: 0x04001B47 RID: 6983
	private bool mIgnoreUp;

	// Token: 0x04001B48 RID: 6984
	private bool mIsInput;

	// Token: 0x04001B49 RID: 6985
	private bool mPress;

	// Token: 0x020005BA RID: 1466
	public enum Action
	{
		// Token: 0x04001B4B RID: 6987
		PressAndClick,
		// Token: 0x04001B4C RID: 6988
		Select,
		// Token: 0x04001B4D RID: 6989
		All
	}

	// Token: 0x020005BB RID: 1467
	public enum Modifier
	{
		// Token: 0x04001B4F RID: 6991
		None,
		// Token: 0x04001B50 RID: 6992
		Shift,
		// Token: 0x04001B51 RID: 6993
		Control,
		// Token: 0x04001B52 RID: 6994
		Alt
	}
}
