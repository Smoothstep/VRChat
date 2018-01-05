using System;
using System.Collections.Generic;
using HeathenEngineering.OSK.v2;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000C85 RID: 3205
public class VRCUiPopupInput : VRCUiPopupStandard
{
	// Token: 0x06006376 RID: 25462 RVA: 0x00235576 File Offset: 0x00233976
	public override void Start()
	{
		this.keyboard.KeyPressed += this.KeyboardKeyPressed;
	}

	// Token: 0x06006377 RID: 25463 RVA: 0x0023558F File Offset: 0x0023398F
	public override void Initialize(string title, string body)
	{
		base.Initialize(title, string.Empty);
		this.popupInputField.text = body;
		this.mHidePopupOnSubmit = false;
		this.nextInputField = null;
		this.showNextInputFieldOnDisable = false;
		this.inputtedKeyCodes = new List<KeyCode>();
	}

	// Token: 0x06006378 RID: 25464 RVA: 0x002355CC File Offset: 0x002339CC
	public void SetupInputAndSubmitButton(string placeholderText, InputField.InputType inputType, string buttonText, Action<string, List<KeyCode>, Text> onSubmit, bool hidePopupOnSubmit = true)
	{
		this.popupInputField.gameObject.transform.Find("Placeholder").GetComponent<Text>().text = placeholderText;
		this.popupInputField.inputType = inputType;
		this.RefreshInput();
		this.mOnSubmitDelegate = onSubmit;
		base.SetupMiddleButton(buttonText, new Action(this.CloseAndSubmit));
		this.mHidePopupOnSubmit = hidePopupOnSubmit;
		this.HighlightInput();
	}

	// Token: 0x06006379 RID: 25465 RVA: 0x0023563C File Offset: 0x00233A3C
	public void SetupInputAndSubmitAndCancelButton(string placeholderText, InputField.InputType inputType, string submitText, Action<string, List<KeyCode>, Text> onSubmit, string cancelText, Action onCancel, bool hidePopupOnSubmit = true)
	{
		this.popupInputField.gameObject.transform.Find("Placeholder").GetComponent<Text>().text = placeholderText;
		this.popupInputField.inputType = inputType;
		this.RefreshInput();
		this.mOnSubmitDelegate = onSubmit;
		base.SetupRightButton(submitText, new Action(this.CloseAndSubmit));
		this.mOnCancelDelegate = onCancel;
		base.SetupLeftButton(cancelText, new Action(this.CloseAndCancel));
		this.mHidePopupOnSubmit = hidePopupOnSubmit;
		this.HighlightInput();
	}

	// Token: 0x0600637A RID: 25466 RVA: 0x002356C8 File Offset: 0x00233AC8
	private void CloseAndSubmit()
	{
		if (this.mOnSubmitDelegate != null)
		{
			this.mOnSubmitDelegate(this.popupInputField.text, this.inputtedKeyCodes, this.popupTitleText);
		}
		if (this.nextInputField != null)
		{
			this.FlagShowNextInput();
		}
		else if (this.mHidePopupOnSubmit)
		{
			VRCUiManager.Instance.popups.HideCurrentPopup();
		}
	}

	// Token: 0x0600637B RID: 25467 RVA: 0x00235738 File Offset: 0x00233B38
	private void FlagShowNextInput()
	{
		VRCUiManager.Instance.popups.HideCurrentPopup();
		this.showNextInputFieldOnDisable = true;
	}

	// Token: 0x0600637C RID: 25468 RVA: 0x00235750 File Offset: 0x00233B50
	private void ShowNextInput()
	{
		if (this.nextInputField != null)
		{
			this.nextInputField.PressEdit();
		}
	}

	// Token: 0x0600637D RID: 25469 RVA: 0x0023576E File Offset: 0x00233B6E
	private void CloseAndCancel()
	{
		if (this.mOnCancelDelegate != null)
		{
			this.mOnCancelDelegate();
		}
		VRCUiManager.Instance.popups.HideCurrentPopup();
	}

	// Token: 0x0600637E RID: 25470 RVA: 0x00235795 File Offset: 0x00233B95
	public override void Update()
	{
		base.Update();
		this.UnfocusInputOnSpecialInputs();
		this.HandleSpecialInputs();
	}

	// Token: 0x0600637F RID: 25471 RVA: 0x002357A9 File Offset: 0x00233BA9
	public override void OnDisable()
	{
		base.OnDisable();
		if (this.showNextInputFieldOnDisable)
		{
			base.Invoke("ShowNextInput", 0.01f);
		}
	}

	// Token: 0x06006380 RID: 25472 RVA: 0x002357CC File Offset: 0x00233BCC
	private void HandleSpecialInputs()
	{
		this.mIsControlKeyPressed = false;
		if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
		{
			Debug.Log("Pressing control");
			this.mIsControlKeyPressed = true;
			if (Input.GetKeyDown(KeyCode.V))
			{
				Debug.Log("Pasting");
				this.popupInputField.text = UniPasteBoard.GetClipBoardString();
				this.popupInputField.MoveTextEnd(false);
			}
		}
		if (this.mIsControlKeyPressed)
		{
			if (Input.GetKeyDown(KeyCode.A) && !string.IsNullOrEmpty(this.popupInputField.text))
			{
				this.HighlightInput();
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				this.CloseAndSubmit();
			}
			if (Input.GetKeyDown(KeyCode.Escape) && !this.popupInputField.isFocused)
			{
				this.CloseAndCancel();
			}
		}
	}

	// Token: 0x06006381 RID: 25473 RVA: 0x002358AC File Offset: 0x00233CAC
	private void UnfocusInputOnSpecialInputs()
	{
		if (this.popupInputField.isFocused && this.mIsControlKeyPressed && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && Input.anyKeyDown)
		{
			this.UnfocusInput();
		}
	}

	// Token: 0x06006382 RID: 25474 RVA: 0x002358FC File Offset: 0x00233CFC
	private void KeyboardKeyPressed(OnScreenKeyboard sender, OnScreenKeyboardArguments args)
	{
		switch (args.KeyPressed.type)
		{
		case KeyClass.String:
			if (this.mWasJustFocused)
			{
				this.UnfocusInput();
				this.popupInputField.text = args.KeyPressed.ToString();
				this.inputtedKeyCodes.Add(args.KeyPressed.keyCode);
			}
			else
			{
				InputField inputField = this.popupInputField;
				inputField.text += args.KeyPressed.ToString();
				this.inputtedKeyCodes.Add(args.KeyPressed.keyCode);
			}
			this.popupInputField.MoveTextEnd(false);
			break;
		case KeyClass.Shift:
			this.UnfocusInput();
			break;
		case KeyClass.Return:
		{
			InputField inputField2 = this.popupInputField;
			inputField2.text += args.KeyPressed.ToString();
			this.inputtedKeyCodes.Add(args.KeyPressed.keyCode);
			break;
		}
		case KeyClass.Backspace:
			if (this.mWasJustFocused)
			{
				this.UnfocusInput();
				this.popupInputField.text = string.Empty;
				this.inputtedKeyCodes.Clear();
			}
			else if (this.popupInputField.text.Length > 0)
			{
				this.popupInputField.text = this.popupInputField.text.Substring(0, this.popupInputField.text.Length - 1);
				this.inputtedKeyCodes.RemoveAt(this.inputtedKeyCodes.Count - 1);
			}
			break;
		}
		if (this.popupInputField.isFocused && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && Input.anyKeyDown)
		{
			this.UnfocusInput();
		}
	}

	// Token: 0x06006383 RID: 25475 RVA: 0x00235AC9 File Offset: 0x00233EC9
	private void UnfocusInput()
	{
		EventSystem.current.SetSelectedGameObject(null);
		this.mWasJustFocused = false;
	}

	// Token: 0x06006384 RID: 25476 RVA: 0x00235AE0 File Offset: 0x00233EE0
	private void HighlightInput()
	{
		if (!string.IsNullOrEmpty(this.popupInputField.text))
		{
			this.popupInputField.interactable = true;
			this.popupInputField.Select();
			this.popupInputField.interactable = false;
			this.mWasJustFocused = true;
		}
	}

	// Token: 0x06006385 RID: 25477 RVA: 0x00235B2C File Offset: 0x00233F2C
	private void RefreshInput()
	{
		this.popupInputField.gameObject.SetActive(false);
		this.popupInputField.gameObject.SetActive(true);
	}

	// Token: 0x040048D9 RID: 18649
	public InputField popupInputField;

	// Token: 0x040048DA RID: 18650
	public List<KeyCode> inputtedKeyCodes;

	// Token: 0x040048DB RID: 18651
	public OnScreenKeyboard keyboard;

	// Token: 0x040048DC RID: 18652
	public UiInputField nextInputField;

	// Token: 0x040048DD RID: 18653
	private Text mPopupButtonSubmitText;

	// Token: 0x040048DE RID: 18654
	private Action<string, List<KeyCode>, Text> mOnSubmitDelegate;

	// Token: 0x040048DF RID: 18655
	private Action mOnCancelDelegate;

	// Token: 0x040048E0 RID: 18656
	private bool mHidePopupOnSubmit;

	// Token: 0x040048E1 RID: 18657
	private bool mIsControlKeyPressed;

	// Token: 0x040048E2 RID: 18658
	private bool mWasJustFocused;

	// Token: 0x040048E3 RID: 18659
	private bool showNextInputFieldOnDisable;
}
