using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C78 RID: 3192
public class VRCUiPageLoading : VRCUiPage
{
	// Token: 0x0600631C RID: 25372 RVA: 0x002340F8 File Offset: 0x002324F8
	public override void Start()
	{
		base.Start();
		this.onPageActivated = (Action)Delegate.Combine(this.onPageActivated, new Action(this.ShowExitButton));
		this.ShowExitButton();
	}

	// Token: 0x0600631D RID: 25373 RVA: 0x00234128 File Offset: 0x00232528
	public void ShowExitButton()
	{
		if (this.ExitButton)
		{
			this.ExitButton.SetActive(VRCFlowManager.Instance.CanCancelRoomLoad());
		}
	}

	// Token: 0x0600631E RID: 25374 RVA: 0x00234150 File Offset: 0x00232550
	public override void Update()
	{
		base.Update();
		float fillAmount;
		string text;
		if (VRCUiPageLoading.Progress > 2f)
		{
			fillAmount = 0f;
			text = (VRCUiPageLoading.Progress / 1048576f).ToString("0.00") + " MB";
		}
		else
		{
			fillAmount = VRCUiPageLoading.Progress;
			text = "100%";
			if (VRCUiPageLoading.Progress < 1f)
			{
				text = (VRCUiPageLoading.Progress * 100f).ToString("0.00") + "%";
			}
		}
		foreach (Image image in this.fillProgress)
		{
			image.fillAmount = fillAmount;
		}
		foreach (Text text2 in this.textProgress)
		{
			text2.text = text;
		}
	}

	// Token: 0x0600631F RID: 25375 RVA: 0x0023423E File Offset: 0x0023263E
	public void OnExitButton()
	{
		if (VRCUiPageLoading.OnExitPressed != null)
		{
			VRCUiPageLoading.OnExitPressed();
		}
	}

	// Token: 0x0400489C RID: 18588
	public Image[] fillProgress;

	// Token: 0x0400489D RID: 18589
	public Text[] textProgress;

	// Token: 0x0400489E RID: 18590
	public static float Progress;

	// Token: 0x0400489F RID: 18591
	public GameObject ExitButton;

	// Token: 0x040048A0 RID: 18592
	public static VRCUiPageLoading.ExitCallback OnExitPressed;

	// Token: 0x02000C79 RID: 3193
	// (Invoke) Token: 0x06006322 RID: 25378
	public delegate void ExitCallback();
}
