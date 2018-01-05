using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRCSDK2;

// Token: 0x02000C86 RID: 3206
public class VRCUiPopupJsKeyboard : VRCUiPopupStandard
{
	// Token: 0x06006387 RID: 25479 RVA: 0x00235B58 File Offset: 0x00233F58
	public override void Start()
	{
	}

	// Token: 0x06006388 RID: 25480 RVA: 0x00235B5C File Offset: 0x00233F5C
	public override void Initialize(string title, string body)
	{
		base.Initialize(title, string.Empty);
		this.inputTitle = title;
		this.popupInputField.text = body;
		this.mHidePopupOnSubmit = true;
		this.popupInputField.Select();
		this.nextInputField = null;
		this.showNextInputFieldOnDisable = false;
		this.inputtedKeyCodes = new List<KeyCode>();
		this.isInitialized = true;
		this.pageLoaded = false;
	}

	// Token: 0x06006389 RID: 25481 RVA: 0x00235BC1 File Offset: 0x00233FC1
	public void SetupInputAndSubmitButton(string placeholderText, InputField.InputType inputType, string buttonText, Action<string, List<KeyCode>, Text> onSubmit, bool hidePopupOnSubmit = true)
	{
		this.popupInputField.inputType = inputType;
		this.RefreshInput();
		this.mOnSubmitDelegate = onSubmit;
		this.mHidePopupOnSubmit = hidePopupOnSubmit;
		this.pageUp = true;
	}

	// Token: 0x0600638A RID: 25482 RVA: 0x00235BEC File Offset: 0x00233FEC
	public void SetupInputAndSubmitAndCancelButton(string placeholderText, InputField.InputType inputType, string submitText, Action<string, List<KeyCode>, Text> onSubmit, string cancelText, Action onCancel, bool hidePopupOnSubmit = true)
	{
		this.popupInputField.inputType = inputType;
		this.RefreshInput();
		this.mOnSubmitDelegate = onSubmit;
		this.mOnCancelDelegate = onCancel;
		this.mHidePopupOnSubmit = hidePopupOnSubmit;
		this.pageUp = true;
	}

	// Token: 0x0600638B RID: 25483 RVA: 0x00235C20 File Offset: 0x00234020
	private void CloseAndSubmit()
	{
		this.webpanelinternal.ExecuteScript("var el=document.getElementById('inputfield'); el.value='';");
		this.webpanelinternal.HandleFocusLoss();
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
		this.pageUp = false;
	}

	// Token: 0x0600638C RID: 25484 RVA: 0x00235CB2 File Offset: 0x002340B2
	private void FlagShowNextInput()
	{
		VRCUiManager.Instance.popups.HideCurrentPopup();
		this.showNextInputFieldOnDisable = true;
	}

	// Token: 0x0600638D RID: 25485 RVA: 0x00235CCA File Offset: 0x002340CA
	private void ShowNextInput()
	{
		if (this.nextInputField != null)
		{
			this.nextInputField.PressEdit();
		}
	}

	// Token: 0x0600638E RID: 25486 RVA: 0x00235CE8 File Offset: 0x002340E8
	private void CloseAndCancel()
	{
		if (this.mOnCancelDelegate != null)
		{
			this.mOnCancelDelegate();
		}
		this.popupInputField.text = string.Empty;
		this.webpanelinternal.ExecuteScript("var el=document.getElementById('inputfield'); el.value='';");
		this.webpanelinternal.HandleFocusLoss();
		VRCUiManager.Instance.popups.HideCurrentPopup();
		this.pageUp = false;
	}

	// Token: 0x0600638F RID: 25487 RVA: 0x00235D4C File Offset: 0x0023414C
	private void OnPageLoaded()
	{
		this.webpanelinternal.ExecuteScript("var el=document.getElementById('title'); el.innerHTML='" + this.inputTitle + "';");
		this.webpanelinternal.ExecuteScript("var el=document.getElementById('inputfield'); el.value='" + this.popupInputField.text + "';");
		if (this.popupInputField.inputType == InputField.InputType.Password)
		{
			this.webpanelinternal.ExecuteScript("var el=document.getElementById('inputfield'); el.type='password';");
		}
		else
		{
			this.webpanelinternal.ExecuteScript("var el=document.getElementById('inputfield'); el.type='text';");
		}
		this.webpanelinternal.ExecuteScript("var el=document.getElementById('inputfield'); el.value='" + this.popupInputField.text + "';");
		this.webpanelinternal.ExecuteScript("ForceFocus();");
		this.webpanelinternal.SetFocus();
		this.webpanelinternal.BindCall("AcceptKeyInput", new Action<string>(this.AcceptKeyInput));
		this.webpanelinternal.BindCall("CancelKeyInput", new Action(this.CloseAndCancel));
		this.pageLoaded = true;
	}

	// Token: 0x06006390 RID: 25488 RVA: 0x00235E55 File Offset: 0x00234255
	private void AcceptKeyInput(string val)
	{
		this.popupInputField.text = val;
		this.CloseAndSubmit();
	}

	// Token: 0x06006391 RID: 25489 RVA: 0x00235E6C File Offset: 0x0023426C
	private void ConnectToWebpage()
	{
		this.webpanel = base.GetComponent<VRC_WebPanel>();
		this.webpanelinternal = base.GetComponent<WebPanelInternal>();
		WebPanelInternal webPanelInternal = this.webpanelinternal;
		webPanelInternal.OnWebPageLoaded = (Action)Delegate.Combine(webPanelInternal.OnWebPageLoaded, new Action(this.OnPageLoaded));
		this.webpanelinternal.IsVirtualKeyboard = true;
	}

	// Token: 0x06006392 RID: 25490 RVA: 0x00235EC4 File Offset: 0x002342C4
	public override void Update()
	{
		base.Update();
		if (this.isInitialized && null == this.webpanel)
		{
			this.ConnectToWebpage();
		}
		if (this.pageUp)
		{
			if (!this.pageLoaded)
			{
				this.OnPageLoaded();
			}
			if (!Input.GetKeyDown(KeyCode.Escape) && !Input.GetKeyDown(KeyCode.Insert) && !Input.GetKeyDown(KeyCode.Tab) && !Input.GetKeyDown(KeyCode.Backspace) && !Input.GetKeyDown(KeyCode.Delete) && !Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				if (Input.GetKeyUp(KeyCode.Escape))
				{
					this.CloseAndCancel();
				}
				else if (Input.GetKeyUp(KeyCode.Insert))
				{
					string systemCopyBuffer = GUIUtility.systemCopyBuffer;
					string script = string.Concat(new object[]
					{
						" var k = VKI_Keyboard_Global;  var inp = k.VKI_target;  if (inp.selectionStart || inp.selectionStart == '0') {    inp.value = inp.value.substring(0, inp.selectionStart)                + '",
						systemCopyBuffer,
						"'               + inp.value.substring(inp.selectionEnd, inp.value.length);  } else {    inp.value += '",
						systemCopyBuffer,
						"';  }  inp.selectionStart += ",
						systemCopyBuffer.Length,
						";  inp.selectionEnd = inp.selectionStart + ",
						systemCopyBuffer.Length,
						"; "
					});
					this.webpanelinternal.ExecuteScript(script);
				}
				else if (Input.GetKeyUp(KeyCode.Tab))
				{
					string script2 = " var k = VKI_Keyboard_Global;  var inp = k.VKI_target;  inp.selectionStart = 0;  inp.selectionEnd = inp.value.length; ";
					this.webpanelinternal.ExecuteScript(script2);
				}
				else if (Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Delete))
				{
					string script3 = " var k = VKI_Keyboard_Global;  var rng = [k.VKI_target.selectionStart, k.VKI_target.selectionEnd];  if (rng[0] < rng[1]) rng[0]++;  k.VKI_target.value = k.VKI_target.value.substr(0, rng[0] - 1) + k.VKI_target.value.substr(rng[1]);  k.VKI_target.setSelectionRange(rng[0] - 1, rng[0] - 1); ";
					this.webpanelinternal.ExecuteScript(script3);
				}
				else if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
				{
					this.webpanelinternal.ExecuteScript("engine.call('AcceptKeyInput', document.getElementById('inputfield').value);");
				}
				else if (!string.IsNullOrEmpty(Input.inputString))
				{
					foreach (char c in Input.inputString)
					{
						string key = c.ToString();
						if (c == '&' || c == '^' || c == '%' || c == '#')
						{
							key = string.Empty;
						}
						Event @event = Event.KeyboardEvent(key);
						@event.character = c;
						this.webpanelinternal.ForceEvent(@event);
					}
				}
			}
		}
	}

	// Token: 0x06006393 RID: 25491 RVA: 0x00236120 File Offset: 0x00234520
	public override void OnDisable()
	{
		base.OnDisable();
		if (null != this.webpanelinternal)
		{
			WebPanelInternal webPanelInternal = this.webpanelinternal;
			webPanelInternal.OnWebPageLoaded = (Action)Delegate.Remove(webPanelInternal.OnWebPageLoaded, new Action(this.OnPageLoaded));
		}
		if (this.showNextInputFieldOnDisable)
		{
			base.Invoke("ShowNextInput", 0.01f);
		}
		this.pageLoaded = false;
		this.pageUp = false;
	}

	// Token: 0x06006394 RID: 25492 RVA: 0x00236194 File Offset: 0x00234594
	private void RefreshInput()
	{
		this.popupInputField.gameObject.SetActive(false);
		this.popupInputField.gameObject.SetActive(true);
	}

	// Token: 0x040048E4 RID: 18660
	public InputField popupInputField;

	// Token: 0x040048E5 RID: 18661
	public List<KeyCode> inputtedKeyCodes;

	// Token: 0x040048E6 RID: 18662
	public UiInputField nextInputField;

	// Token: 0x040048E7 RID: 18663
	private Text mPopupButtonSubmitText;

	// Token: 0x040048E8 RID: 18664
	private Action<string, List<KeyCode>, Text> mOnSubmitDelegate;

	// Token: 0x040048E9 RID: 18665
	private Action mOnCancelDelegate;

	// Token: 0x040048EA RID: 18666
	private bool mHidePopupOnSubmit;

	// Token: 0x040048EB RID: 18667
	private bool showNextInputFieldOnDisable;

	// Token: 0x040048EC RID: 18668
	private bool isInitialized;

	// Token: 0x040048ED RID: 18669
	private string inputTitle;

	// Token: 0x040048EE RID: 18670
	private VRC_WebPanel webpanel;

	// Token: 0x040048EF RID: 18671
	private WebPanelInternal webpanelinternal;

	// Token: 0x040048F0 RID: 18672
	private bool pageLoaded;

	// Token: 0x040048F1 RID: 18673
	private bool pageUp;
}
