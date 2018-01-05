using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C82 RID: 3202
public class VRCUiPopup : VRCUiPage
{
	// Token: 0x06006350 RID: 25424 RVA: 0x002203CE File Offset: 0x0021E7CE
	public override void Awake()
	{
		base.Awake();
		if (this.closePopupButton != null)
		{
			this.closePopupButton.onClick.AddListener(delegate
			{
				this.Close();
			});
		}
	}

	// Token: 0x06006351 RID: 25425 RVA: 0x00220403 File Offset: 0x0021E803
	public override void OnEnable()
	{
		base.OnEnable();
		if (VRCUiManager.Instance != null)
		{
			VRCUiManager.Instance.currentPopup = this;
		}
	}

	// Token: 0x06006352 RID: 25426 RVA: 0x00220426 File Offset: 0x0021E826
	public override void OnDisable()
	{
		base.OnDisable();
		if (VRCUiManager.Instance != null)
		{
			VRCUiManager.Instance.currentPopup = null;
		}
	}

	// Token: 0x06006353 RID: 25427 RVA: 0x0022044C File Offset: 0x0021E84C
	public virtual void Initialize(string title, string body)
	{
		this.screenType = "POPUP";
		if (this.popupTitleText != null)
		{
			this.popupTitleText.text = title;
		}
		if (this.popupBodyText != null)
		{
			this.popupBodyText.text = body;
		}
		if (this.popupProgressFillImage != null)
		{
			this.popupProgressFillImage.enabled = false;
			this.PopupTimerExpired = null;
			this.popupTimer = 0f;
			this.popupTimerMaximum = 0f;
			this.popupTimerMinimum = 0f;
			this.popupTimerSpeed = 1f;
		}
		base.enabled = true;
	}

	// Token: 0x06006354 RID: 25428 RVA: 0x002204F8 File Offset: 0x0021E8F8
	public override void Update()
	{
		if (this.popupTimer > 0f)
		{
			this.popupTimer -= Time.deltaTime * this.popupTimerSpeed;
			if (this.popupTimer < this.popupTimerMinimum)
			{
				this.popupTimer = this.popupTimerMinimum;
			}
			this.popupProgressFillImage.fillAmount = (this.popupTimerMaximum - this.popupTimer) / this.popupTimerMaximum;
			if (this.popupTimer <= 0f)
			{
				this.PopupTimerExpired();
			}
		}
		base.Update();
	}

	// Token: 0x06006355 RID: 25429 RVA: 0x0022058B File Offset: 0x0021E98B
	public void SetupProgressTimer(float duration, Action callback, float minimum = 0f)
	{
		this.popupProgressFillImage.enabled = true;
		this.popupProgressFillImage.fillAmount = 0f;
		this.popupTimerMaximum = duration;
		this.popupTimerMinimum = minimum;
		this.PopupTimerExpired = callback;
		this.popupTimer = duration;
	}

	// Token: 0x06006356 RID: 25430 RVA: 0x002205C5 File Offset: 0x0021E9C5
	public void ProgressComplete()
	{
		this.popupTimerMinimum = 0f;
		this.popupTimerSpeed = 10f;
	}

	// Token: 0x06006357 RID: 25431 RVA: 0x002205DD File Offset: 0x0021E9DD
	public virtual void Close()
	{
		VRCUiPopupManager.Instance.HideCurrentPopup();
	}

	// Token: 0x040048B9 RID: 18617
	public Text popupTitleText;

	// Token: 0x040048BA RID: 18618
	public Text popupBodyText;

	// Token: 0x040048BB RID: 18619
	public Image popupProgressFillImage;

	// Token: 0x040048BC RID: 18620
	public Button closePopupButton;

	// Token: 0x040048BD RID: 18621
	private float popupTimer;

	// Token: 0x040048BE RID: 18622
	private float popupTimerMaximum = 1f;

	// Token: 0x040048BF RID: 18623
	private float popupTimerMinimum;

	// Token: 0x040048C0 RID: 18624
	private float popupTimerSpeed = 1f;

	// Token: 0x040048C1 RID: 18625
	private Action PopupTimerExpired;
}
