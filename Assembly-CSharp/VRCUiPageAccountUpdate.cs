using System;
using System.Collections;
using VRC;
using VRC.Core;

// Token: 0x02000C72 RID: 3186
public class VRCUiPageAccountUpdate : VRCUiPage
{
	// Token: 0x060062EF RID: 25327 RVA: 0x00233411 File Offset: 0x00231811
	public override void Start()
	{
		base.Start();
		this.flowManager = VRCFlowManager.Instance;
	}

	// Token: 0x060062F0 RID: 25328 RVA: 0x00233424 File Offset: 0x00231824
	public override void OnEnable()
	{
		this.updateEmail.gameObject.SetActive(VRCUiPageAccountUpdate.IsEmailRequired);
		this.updateDateOfBirth.gameObject.SetActive(VRCUiPageAccountUpdate.IsDoBRequired);
		base.StartCoroutine(this.ResetFormInput());
	}

	// Token: 0x060062F1 RID: 25329 RVA: 0x00233460 File Offset: 0x00231860
	private IEnumerator ResetFormInput()
	{
		yield return null;
		InputFieldValidator.ResetFormInput(base.gameObject);
		yield break;
	}

	// Token: 0x060062F2 RID: 25330 RVA: 0x0023347C File Offset: 0x0023187C
	public void UpdatePressed()
	{
		if (InputFieldValidator.IsFormInputValid(base.gameObject))
		{
			if (InputFieldValidator.ValidateFormPostEntry(base.gameObject))
			{
				this.flowManager.StartCoroutine(this.flowManager.UpdateAccountInfo(this.updateEmail.text, DateTime.Parse(this.updateDateOfBirth.text).ToString("yyyy-MM-dd")));
			}
			else
			{
				VRCUiManager.Instance.popups.ShowStandardPopup("Cannot Verify Account", "Account verification failed.", "Close", delegate
				{
					VRCUiManager.Instance.popups.HideCurrentPopup();
				}, null);
				Analytics.Send(ApiAnalyticEvent.EventType.error, "UpdateAccountFailed: dob check failed", null, null);
			}
		}
		else
		{
			VRCUiManager.Instance.popups.ShowStandardPopup("Cannot Verify Account", "Please fill out valid data for each input.", "Close", delegate
			{
				VRCUiManager.Instance.popups.HideCurrentPopup();
			}, null);
			Analytics.Send(ApiAnalyticEvent.EventType.error, "UpdateAccountFailed: form entries invalid", null, null);
		}
	}

	// Token: 0x060062F3 RID: 25331 RVA: 0x00233594 File Offset: 0x00231994
	public void BackPressed()
	{
		User.Logout();
		this.flowManager.ResetGameFlow(new VRCFlowManager.ResetGameFlags[0]);
	}

	// Token: 0x0400487A RID: 18554
	private VRCFlowManager flowManager;

	// Token: 0x0400487B RID: 18555
	public UiInputField updateEmail;

	// Token: 0x0400487C RID: 18556
	public UiInputField updateDateOfBirth;

	// Token: 0x0400487D RID: 18557
	public static bool IsEmailRequired = true;

	// Token: 0x0400487E RID: 18558
	public static bool IsDoBRequired = true;
}
