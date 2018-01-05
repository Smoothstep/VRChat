using System;
using UnityEngine.UI;

// Token: 0x02000C88 RID: 3208
public class VRCUiPopupStandard : VRCUiPopup
{
	// Token: 0x060063AC RID: 25516 RVA: 0x00234C2C File Offset: 0x0023302C
	public override void Initialize(string title, string body)
	{
		base.Initialize(title, body);
		if (this.popupButtonLeft != null)
		{
			this.popupButtonLeft.gameObject.SetActive(true);
			this.popupButtonLeftText = this.popupButtonLeft.GetComponentInChildren<Text>();
			this.popupButtonLeft.gameObject.SetActive(false);
		}
		if (this.popupButtonMiddle != null)
		{
			this.popupButtonMiddle.gameObject.SetActive(true);
			this.popupButtonMiddleText = this.popupButtonMiddle.GetComponentInChildren<Text>();
			this.popupButtonMiddle.gameObject.SetActive(false);
		}
		if (this.popupButtonRight != null)
		{
			this.popupButtonRight.gameObject.SetActive(true);
			this.popupButtonRightText = this.popupButtonRight.GetComponentInChildren<Text>();
			this.popupButtonRight.gameObject.SetActive(false);
		}
		this.leftButtonPressed = null;
		this.middleButtonPressed = null;
		this.rightButtonPressed = null;
	}

	// Token: 0x060063AD RID: 25517 RVA: 0x00234D22 File Offset: 0x00233122
	public void OnLeftButton()
	{
		if (this.leftButtonPressed != null)
		{
			this.leftButtonPressed();
		}
	}

	// Token: 0x060063AE RID: 25518 RVA: 0x00234D3A File Offset: 0x0023313A
	public void OnMiddleButton()
	{
		if (this.middleButtonPressed != null)
		{
			this.middleButtonPressed();
		}
	}

	// Token: 0x060063AF RID: 25519 RVA: 0x00234D52 File Offset: 0x00233152
	public void OnRightButton()
	{
		if (this.rightButtonPressed != null)
		{
			this.rightButtonPressed();
		}
	}

	// Token: 0x060063B0 RID: 25520 RVA: 0x00234D6A File Offset: 0x0023316A
	public void SetupLeftButton(string text, Action callback)
	{
		this.popupButtonLeft.gameObject.SetActive(true);
		this.popupButtonLeftText.text = text;
		this.leftButtonPressed = callback;
	}

	// Token: 0x060063B1 RID: 25521 RVA: 0x00234D90 File Offset: 0x00233190
	public void SetupMiddleButton(string text, Action callback)
	{
		if (this.popupButtonMiddle != null)
		{
			this.popupButtonMiddle.gameObject.SetActive(true);
			this.popupButtonMiddleText.text = text;
			this.middleButtonPressed = callback;
		}
	}

	// Token: 0x060063B2 RID: 25522 RVA: 0x00234DC7 File Offset: 0x002331C7
	public void SetupRightButton(string text, Action callback)
	{
		this.popupButtonRight.gameObject.SetActive(true);
		this.popupButtonRightText.text = text;
		this.rightButtonPressed = callback;
	}

	// Token: 0x040048FE RID: 18686
	public Button popupButtonLeft;

	// Token: 0x040048FF RID: 18687
	public Button popupButtonMiddle;

	// Token: 0x04004900 RID: 18688
	public Button popupButtonRight;

	// Token: 0x04004901 RID: 18689
	private Text popupButtonLeftText;

	// Token: 0x04004902 RID: 18690
	private Text popupButtonMiddleText;

	// Token: 0x04004903 RID: 18691
	private Text popupButtonRightText;

	// Token: 0x04004904 RID: 18692
	private Action leftButtonPressed;

	// Token: 0x04004905 RID: 18693
	private Action middleButtonPressed;

	// Token: 0x04004906 RID: 18694
	private Action rightButtonPressed;
}
