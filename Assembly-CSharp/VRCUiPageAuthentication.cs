using System;
using VRC.Core;

// Token: 0x02000C73 RID: 3187
public class VRCUiPageAuthentication : VRCUiPage
{
	// Token: 0x060062F8 RID: 25336 RVA: 0x00233676 File Offset: 0x00231A76
	public override void Start()
	{
		base.Start();
		this.flowManager = VRCFlowManager.Instance;
	}

	// Token: 0x060062F9 RID: 25337 RVA: 0x0023368C File Offset: 0x00231A8C
	public void AccountLoginPressed()
	{
		if (InputFieldValidator.IsFormInputValid(base.gameObject))
		{
			this.flowManager.StartCoroutine(this.flowManager.LoginAccount(this.loginUserName.text, this.loginPassword.text));
		}
		else
		{
			VRCUiManager.Instance.popups.ShowStandardPopup("Cannot Login", "Please fill out valid data for each input.", "Close", delegate
			{
				VRCUiManager.Instance.popups.HideCurrentPopup();
			}, null);
			Analytics.Send(ApiAnalyticEvent.EventType.error, "LoginFailed: form entries invalid", null, null);
		}
	}

	// Token: 0x060062FA RID: 25338 RVA: 0x0023372C File Offset: 0x00231B2C
	public void AccountCreatePressed()
	{
		if (InputFieldValidator.IsFormInputValid(base.gameObject))
		{
			if (InputFieldValidator.ValidateFormPostEntry(base.gameObject))
			{
				this.flowManager.StartCoroutine(this.flowManager.CreateAccount(this.createUserName.text, this.createPassword.text, this.createEmail.text, DateTime.Parse(this.createDateOfBirth.text).ToString("yyyy-MM-dd")));
			}
			else
			{
				VRCUiManager.Instance.popups.ShowStandardPopup("Cannot Register Account", "Account creation failed.", "Close", delegate
				{
					VRCUiManager.Instance.popups.HideCurrentPopup();
				}, null);
				Analytics.Send(ApiAnalyticEvent.EventType.error, "CreateAccountFailed: dob check failed", null, null);
			}
		}
		else
		{
			VRCUiManager.Instance.popups.ShowStandardPopup("Cannot Register Account", "Please fill out valid data for each input.", "Close", delegate
			{
				VRCUiManager.Instance.popups.HideCurrentPopup();
			}, null);
			Analytics.Send(ApiAnalyticEvent.EventType.error, "CreateAccountFailed: form entries invalid", null, null);
		}
	}

	// Token: 0x060062FB RID: 25339 RVA: 0x0023385A File Offset: 0x00231C5A
	public void AccountLoginAnonymous()
	{
		this.flowManager.StartCoroutine(this.flowManager.LoginAccount(null, null));
	}

	// Token: 0x060062FC RID: 25340 RVA: 0x00233875 File Offset: 0x00231C75
	public void AuthenticateWithSteam()
	{
		VRCFlowManager.Instance.AuthenticateWithSteam(true);
	}

	// Token: 0x060062FD RID: 25341 RVA: 0x00233882 File Offset: 0x00231C82
	public void AuthenticateWithViveport()
	{
	}

	// Token: 0x060062FE RID: 25342 RVA: 0x00233884 File Offset: 0x00231C84
	public static void ShowAppropriateLoginPromptScreenInternal()
	{
		if (!VRCFlowManager.Instance.AllowThirdPartyLogin)
		{
			VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Screens/Authentication/LoginPrompt");
		}
		else
		{
			VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Screens/Authentication/StoreLoginPrompt");
		}
	}

	// Token: 0x060062FF RID: 25343 RVA: 0x002338B8 File Offset: 0x00231CB8
	public void ShowAppropriateLoginPromptScreen()
	{
		VRCUiPageAuthentication.ShowAppropriateLoginPromptScreenInternal();
	}

	// Token: 0x04004881 RID: 18561
	private VRCFlowManager flowManager;

	// Token: 0x04004882 RID: 18562
	public UiInputField loginUserName;

	// Token: 0x04004883 RID: 18563
	public UiInputField loginPassword;

	// Token: 0x04004884 RID: 18564
	public UiInputField createUserName;

	// Token: 0x04004885 RID: 18565
	public UiInputField createPassword;

	// Token: 0x04004886 RID: 18566
	public UiInputField createEmail;

	// Token: 0x04004887 RID: 18567
	public UiInputField createPasswordHint;

	// Token: 0x04004888 RID: 18568
	public UiInputField createDateOfBirth;
}
