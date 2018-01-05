using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000C46 RID: 3142
public class UiInputField : MonoBehaviour
{
	// Token: 0x17000DA2 RID: 3490
	// (get) Token: 0x06006176 RID: 24950 RVA: 0x002265B1 File Offset: 0x002249B1
	// (set) Token: 0x06006177 RID: 24951 RVA: 0x002265BC File Offset: 0x002249BC
	public string text
	{
		get
		{
			return this.plainText;
		}
		set
		{
			if (value == string.Empty)
			{
				this.plainText = string.Empty;
				this.textField.text = this.plainText;
				this.textField.gameObject.SetActive(false);
				if (this.placeholder != null)
				{
					this.placeholder.gameObject.SetActive(true);
				}
			}
			else
			{
				this.plainText = value;
				this.textField.text = ((this.inputType != InputField.InputType.Password) ? this.plainText : this.GetAsteriskString(this.plainText));
			}
		}
	}

	// Token: 0x06006178 RID: 24952 RVA: 0x00226664 File Offset: 0x00224A64
	private void Awake()
	{
		if (this.placeholder == null)
		{
			Transform transform = base.transform.Find("Placeholder");
			if (transform != null)
			{
				this.placeholder = transform.GetComponent<Text>();
			}
		}
		if (this.textField == null)
		{
			this.textField = base.transform.Find("Text").GetComponent<Text>();
		}
		if (this.editButton == null)
		{
			this.editButton = base.gameObject.AddMissingComponent<Button>();
			this.editButton.onClick.AddListener(new UnityAction(this.PressEdit));
		}
	}

	// Token: 0x06006179 RID: 24953 RVA: 0x00226718 File Offset: 0x00224B18
	public void PressEdit()
	{
		VRCInputManager.UseKeyboardOnlyForText(true);
		string placeholderText = string.IsNullOrEmpty(this.placeholderInputText) ? ((!(this.placeholder == null)) ? this.placeholder.text : string.Empty) : this.placeholderInputText;
		if (this.isDate)
		{
			VRCUiPopupManager.Instance.ShowDatePopupWithCancel(this.title, this.text, this.inputType, "OK", new Action<string, List<KeyCode>, Text>(this.StringAccepted), new Action(this.StringRejected), placeholderText, true, delegate(VRCUiPopup obj)
			{
				VRCUiPopupDate vrcuiPopupDate = (VRCUiPopupDate)obj;
				vrcuiPopupDate.nextInputField = this.nextInputField;
			});
		}
		else
		{
			VRCUiPopupManager.Instance.ShowInputPopupWithCancel(this.title, this.text, this.inputType, "OK", new Action<string, List<KeyCode>, Text>(this.StringAccepted), new Action(this.StringRejected), placeholderText, true, delegate(VRCUiPopup obj)
			{
				VRCUiPopupJsKeyboard vrcuiPopupJsKeyboard = (VRCUiPopupJsKeyboard)obj;
				vrcuiPopupJsKeyboard.nextInputField = this.nextInputField;
			});
		}
	}

	// Token: 0x0600617A RID: 24954 RVA: 0x0022680C File Offset: 0x00224C0C
	private void StringAccepted(string s, List<KeyCode> keyCodes, Text t)
	{
		this.text = s;
		if (!string.IsNullOrEmpty(this.textField.text))
		{
			this.textField.gameObject.SetActive(true);
			if (this.placeholder != null)
			{
				this.placeholder.gameObject.SetActive(false);
			}
		}
		else
		{
			this.textField.gameObject.SetActive(false);
			if (this.placeholder != null)
			{
				this.placeholder.gameObject.SetActive(true);
			}
		}
		VRCUiPopupManager.Instance.HideCurrentPopup();
		VRCInputManager.UseKeyboardOnlyForText(false);
		this.onEndEdit.Invoke(s);
		if (this.onDoneInputting != null)
		{
			this.onDoneInputting(s);
		}
	}

	// Token: 0x0600617B RID: 24955 RVA: 0x002268D3 File Offset: 0x00224CD3
	private void StringRejected()
	{
		VRCUiManager.Instance.popups.HideCurrentPopup();
		VRCInputManager.UseKeyboardOnlyForText(false);
	}

	// Token: 0x0600617C RID: 24956 RVA: 0x002268EA File Offset: 0x00224CEA
	private string GetAsteriskString(string s)
	{
		return new string('*', s.Length);
	}

	// Token: 0x0400470D RID: 18189
	public string title;

	// Token: 0x0400470E RID: 18190
	public Text placeholder;

	// Token: 0x0400470F RID: 18191
	public string placeholderInputText;

	// Token: 0x04004710 RID: 18192
	public Text textField;

	// Token: 0x04004711 RID: 18193
	public InputField.InputType inputType;

	// Token: 0x04004712 RID: 18194
	public bool isDate;

	// Token: 0x04004713 RID: 18195
	public UiInputField nextInputField;

	// Token: 0x04004714 RID: 18196
	public InputField.SubmitEvent onEndEdit = new InputField.SubmitEvent();

	// Token: 0x04004715 RID: 18197
	public UnityAction<string> onDoneInputting;

	// Token: 0x04004716 RID: 18198
	private string plainText = string.Empty;

	// Token: 0x04004717 RID: 18199
	public Button editButton;
}
