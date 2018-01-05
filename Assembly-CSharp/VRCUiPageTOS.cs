using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.Core;

// Token: 0x02000C7C RID: 3196
public class VRCUiPageTOS : VRCUiPage
{
	// Token: 0x06006329 RID: 25385 RVA: 0x0023429B File Offset: 0x0023269B
	public override void Start()
	{
		base.Start();
		this.flowManager = VRCFlowManager.Instance;
	}

	// Token: 0x0600632A RID: 25386 RVA: 0x002342AE File Offset: 0x002326AE
	public override void OnEnable()
	{
		base.OnEnable();
		this.continueButton.interactable = false;
		this.loadingIcon.SetActive(false);
		base.StartCoroutine(this.LoadTOS());
	}

	// Token: 0x0600632B RID: 25387 RVA: 0x002342DB File Offset: 0x002326DB
	public override void OnDisable()
	{
		base.OnDisable();
		base.StopAllCoroutines();
		this.Cleanup();
	}

	// Token: 0x0600632C RID: 25388 RVA: 0x002342F0 File Offset: 0x002326F0
	private static string GetTOSUrl()
	{
		string api_URL = ApiModel.API_URL;
		Uri uri = new Uri(api_URL);
		string leftPart = uri.GetLeftPart(UriPartial.Authority);
		return leftPart + "/legal?plaintext=true";
	}

	// Token: 0x0600632D RID: 25389 RVA: 0x00234320 File Offset: 0x00232720
	private IEnumerator LoadTOS()
	{
		this.loadingIcon.SetActive(true);
		WWW www = new WWW(VRCUiPageTOS.GetTOSUrl());
		float startTime = Time.realtimeSinceStartup;
		while (!www.isDone)
		{
			if (Time.realtimeSinceStartup - startTime > 10f)
			{
				this.OnLoadError("Request Timed Out");
				base.StartCoroutine(this.RetryLoadTOS());
				yield break;
			}
			yield return null;
		}
		if (!string.IsNullOrEmpty(www.error))
		{
			this.OnLoadError(www.error);
			base.StartCoroutine(this.RetryLoadTOS());
		}
		else
		{
			this.OnLoadSuccess(www.text);
		}
		yield break;
	}

	// Token: 0x0600632E RID: 25390 RVA: 0x0023433C File Offset: 0x0023273C
	private IEnumerator RetryLoadTOS()
	{
		this.loadingIcon.SetActive(true);
		yield return new WaitForSeconds(4f);
		base.StartCoroutine(this.LoadTOS());
		yield break;
	}

	// Token: 0x0600632F RID: 25391 RVA: 0x00234357 File Offset: 0x00232757
	private void OnLoadSuccess(string wwwText)
	{
		this.loadingIcon.SetActive(false);
		this.LayoutTextView(wwwText);
		this.continueButton.interactable = true;
	}

	// Token: 0x06006330 RID: 25392 RVA: 0x00234378 File Offset: 0x00232778
	private void LayoutTextView(string wwwText)
	{
		string text = this.CleanupRawText(wwwText);
		int num = 0;
		int num2 = 0;
		for (;;)
		{
			int num3 = text.IndexOf("\n\n", num2);
			if (num3 < 0)
			{
				break;
			}
			num2 = num3 + 2;
			if (num3 - num + 2 >= 5000)
			{
				this.MakeTextBox(text.Substring(num, num3 - num + 2));
				num = num2;
			}
		}
		this.MakeTextBox(text.Substring(num));
	}

	// Token: 0x06006331 RID: 25393 RVA: 0x002343E4 File Offset: 0x002327E4
	private void MakeTextBox(string substring)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.textTemplate);
		gameObject.GetComponent<Text>().text = substring;
		gameObject.transform.SetParent(this.textTemplate.transform.parent);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		gameObject.SetActive(true);
		this.textFields.Add(gameObject);
	}

	// Token: 0x06006332 RID: 25394 RVA: 0x00234467 File Offset: 0x00232867
	private string CleanupRawText(string wwwText)
	{
		return wwwText;
	}

	// Token: 0x06006333 RID: 25395 RVA: 0x0023446C File Offset: 0x0023286C
	private void OnLoadError(string error)
	{
		Debug.LogError("TOS load error (" + VRCUiPageTOS.GetTOSUrl() + "): " + error);
		this.loadingIcon.SetActive(false);
		if (!this.shownLoadErrorPopup)
		{
			this.shownLoadErrorPopup = true;
			VRCUiManager.Instance.popups.ShowStandardPopup("Error", "Couldn't load Terms of Service.  Please try again later.", "Close", delegate
			{
				VRCUiManager.Instance.popups.HideCurrentPopup();
			}, null);
		}
	}

	// Token: 0x06006334 RID: 25396 RVA: 0x002344F0 File Offset: 0x002328F0
	private void Cleanup()
	{
		foreach (GameObject obj in this.textFields)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.textFields.Clear();
	}

	// Token: 0x06006335 RID: 25397 RVA: 0x00234558 File Offset: 0x00232958
	public void ContinuePressed()
	{
		this.flowManager.StartCoroutine(this.flowManager.AgreeToTermsOfService());
	}

	// Token: 0x06006336 RID: 25398 RVA: 0x00234571 File Offset: 0x00232971
	public void BackPressed()
	{
		User.Logout();
		this.flowManager.ResetGameFlow(new VRCFlowManager.ResetGameFlags[0]);
	}

	// Token: 0x040048A2 RID: 18594
	private VRCFlowManager flowManager;

	// Token: 0x040048A3 RID: 18595
	public Button continueButton;

	// Token: 0x040048A4 RID: 18596
	public GameObject textTemplate;

	// Token: 0x040048A5 RID: 18597
	public GameObject loadingIcon;

	// Token: 0x040048A6 RID: 18598
	private bool shownLoadErrorPopup;

	// Token: 0x040048A7 RID: 18599
	private List<GameObject> textFields = new List<GameObject>();
}
