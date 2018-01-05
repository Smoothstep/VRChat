using System;
using System.Text.RegularExpressions;
using BestHTTP;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000C1F RID: 3103
[RequireComponent(typeof(UiInputField))]
public class InputFieldValidator : MonoBehaviour
{
	// Token: 0x17000D91 RID: 3473
	// (get) Token: 0x0600600A RID: 24586 RVA: 0x0021CCC6 File Offset: 0x0021B0C6
	// (set) Token: 0x0600600B RID: 24587 RVA: 0x0021CCCE File Offset: 0x0021B0CE
	public bool isValid
	{
		get
		{
			return this.mIsValid;
		}
		set
		{
			this.mIsValid = value;
		}
	}

	// Token: 0x0600600C RID: 24588 RVA: 0x0021CCD7 File Offset: 0x0021B0D7
	private void Awake()
	{
		this.HideValidateTexture();
		this.inputField = base.GetComponent<UiInputField>();
		this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.Validate));
	}

	// Token: 0x0600600D RID: 24589 RVA: 0x0021CD08 File Offset: 0x0021B108
	public static bool IsFormInputValid(GameObject form)
	{
		bool result = true;
		InputFieldValidator[] componentsInChildren = form.GetComponentsInChildren<InputFieldValidator>();
		foreach (InputFieldValidator inputFieldValidator in componentsInChildren)
		{
			if (inputFieldValidator.gameObject.activeInHierarchy && inputFieldValidator.enabled)
			{
				if (!inputFieldValidator.isValid)
				{
					inputFieldValidator.ShowInvalidSprite();
					result = false;
				}
				else
				{
					inputFieldValidator.ShowValidSprite();
				}
			}
		}
		return result;
	}

	// Token: 0x0600600E RID: 24590 RVA: 0x0021CD7C File Offset: 0x0021B17C
	public static bool ValidateFormPostEntry(GameObject form)
	{
		InputFieldValidator[] componentsInChildren = form.GetComponentsInChildren<InputFieldValidator>();
		foreach (InputFieldValidator inputFieldValidator in componentsInChildren)
		{
			if (inputFieldValidator.gameObject.activeInHierarchy && inputFieldValidator.enabled)
			{
				if (!inputFieldValidator.ValidatePostEntry())
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600600F RID: 24591 RVA: 0x0021CDD8 File Offset: 0x0021B1D8
	public static void ResetFormInput(GameObject form)
	{
		InputFieldValidator[] componentsInChildren = form.GetComponentsInChildren<InputFieldValidator>();
		foreach (InputFieldValidator inputFieldValidator in componentsInChildren)
		{
			inputFieldValidator.HideValidateTexture();
			inputFieldValidator.isValid = false;
			inputFieldValidator.inputField.text = string.Empty;
		}
	}

	// Token: 0x06006010 RID: 24592 RVA: 0x0021CE24 File Offset: 0x0021B224
	public void Validate(string text)
	{
		this.mIsValid = false;
		bool flag = false;
		switch (this.type)
		{
		case InputFieldValidator.ValidatorType.Exists:
			if (!string.IsNullOrEmpty(text))
			{
				this.mIsValid = true;
			}
			break;
		case InputFieldValidator.ValidatorType.Email:
		{
			Regex regex = new Regex("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$");
			Match match = regex.Match(text);
			this.mIsValid = match.Success;
			break;
		}
		case InputFieldValidator.ValidatorType.Blueprint:
			flag = true;
			this.ValidateBlueprintInput(text);
			break;
		case InputFieldValidator.ValidatorType.RemoteBlueprint:
			if (text.StartsWith("file:"))
			{
				this.mIsValid = false;
			}
			else
			{
				flag = true;
				this.ValidateBlueprintInput(text);
			}
			break;
		case InputFieldValidator.ValidatorType.DateOfBirth:
		{
			DateTime t = new DateTime(0L);
			if (DateTime.TryParse(text, out t))
			{
				if (t < this.kMinDateOfBirth || t > DateTime.Now)
				{
					this.mIsValid = false;
				}
				else
				{
					this.mIsValid = true;
				}
			}
			else
			{
				this.mIsValid = false;
			}
			break;
		}
		}
		if (!flag)
		{
			if (this.mIsValid)
			{
				this.ShowValidSprite();
				if (this.onValidate != null)
				{
					this.onValidate(true, text);
				}
			}
			else
			{
				this.ShowInvalidSprite();
				if (this.onValidate != null)
				{
					this.onValidate(false, text);
				}
			}
		}
	}

	// Token: 0x06006011 RID: 24593 RVA: 0x0021CF82 File Offset: 0x0021B382
	private void ValidateBlueprintInput(string text)
	{
		Debug.LogError("I'm pretty sure this is no longer necessary.  If we don't hit this by Nov 10, 2016, then just delete the whole function.");
	}

	// Token: 0x06006012 RID: 24594 RVA: 0x0021CF90 File Offset: 0x0021B390
	private bool ValidatePostEntry()
	{
		if (!this.isValid)
		{
			return false;
		}
		InputFieldValidator.ValidatorType validatorType = this.type;
		if (validatorType == InputFieldValidator.ValidatorType.DateOfBirth)
		{
			DateTime t = new DateTime(0L);
			if (!DateTime.TryParse(this.inputField.text, out t))
			{
				return false;
			}
			DateTime today = DateTime.Today;
			int num = today.Year - t.Year;
			if (t > today.AddYears(-num))
			{
				num--;
			}
			if (num < this.kMinAge)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06006013 RID: 24595 RVA: 0x0021D021 File Offset: 0x0021B421
	private void ShowValidSprite()
	{
		this.validateImage.type = Image.Type.Sliced;
		this.ShowSprite(this.isValidTexture);
	}

	// Token: 0x06006014 RID: 24596 RVA: 0x0021D03B File Offset: 0x0021B43B
	private void ShowInvalidSprite()
	{
		this.validateImage.type = Image.Type.Sliced;
		this.ShowSprite(this.isInvalidTexture);
	}

	// Token: 0x06006015 RID: 24597 RVA: 0x0021D055 File Offset: 0x0021B455
	private void ShowWaitingSprite()
	{
		this.validateImage.type = Image.Type.Filled;
		this.validateImage.fillAmount = 0f;
		this.ShowSprite(this.waitingTexture);
	}

	// Token: 0x06006016 RID: 24598 RVA: 0x0021D080 File Offset: 0x0021B480
	private void ShowSprite(Sprite sprite)
	{
		if (this.validateImage != null)
		{
			Color color = this.validateImage.color;
			color.a = 1f;
			this.validateImage.color = color;
			this.validateImage.sprite = sprite;
		}
	}

	// Token: 0x06006017 RID: 24599 RVA: 0x0021D0D0 File Offset: 0x0021B4D0
	private void HideValidateTexture()
	{
		if (this.validateImage != null)
		{
			Color color = this.validateImage.color;
			color.a = 0f;
			this.validateImage.color = color;
		}
	}

	// Token: 0x06006018 RID: 24600 RVA: 0x0021D114 File Offset: 0x0021B514
	private void OnDownloadProgress(HTTPRequest request, int downloaded, int length)
	{
		float fillAmount = (float)downloaded / (float)length;
		this.validateImage.fillAmount = fillAmount;
	}

	// Token: 0x040045C2 RID: 17858
	public InputFieldValidator.ValidatorType type;

	// Token: 0x040045C3 RID: 17859
	public Image validateImage;

	// Token: 0x040045C4 RID: 17860
	public Sprite isValidTexture;

	// Token: 0x040045C5 RID: 17861
	public Sprite isInvalidTexture;

	// Token: 0x040045C6 RID: 17862
	public Sprite waitingTexture;

	// Token: 0x040045C7 RID: 17863
	private UiInputField inputField;

	// Token: 0x040045C8 RID: 17864
	private bool mIsValid;

	// Token: 0x040045C9 RID: 17865
	public Action<bool, string> onValidate;

	// Token: 0x040045CA RID: 17866
	private readonly DateTime kMinDateOfBirth = new DateTime(1900, 1, 1);

	// Token: 0x040045CB RID: 17867
	private readonly int kMinAge = 13;

	// Token: 0x02000C20 RID: 3104
	public enum ValidatorType
	{
		// Token: 0x040045CD RID: 17869
		Exists,
		// Token: 0x040045CE RID: 17870
		Email,
		// Token: 0x040045CF RID: 17871
		Blueprint,
		// Token: 0x040045D0 RID: 17872
		RemoteBlueprint,
		// Token: 0x040045D1 RID: 17873
		DateOfBirth
	}
}
