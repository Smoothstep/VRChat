using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C1E RID: 3102
public class HudVoiceIndicator : MonoBehaviour
{
	// Token: 0x06006004 RID: 24580 RVA: 0x0021C924 File Offset: 0x0021AD24
	private void Start()
	{
		this.dotImage = base.transform.Find("VoiceDot").GetComponent<Image>();
		this.dotDisabledImage = base.transform.Find("VoiceDotDisabled").GetComponent<Image>();
		this.dotColor = this.dotImage.color;
		this.keybdTip = base.transform.Find("PushToTalkKeybd").gameObject;
		this.xboxTip = base.transform.Find("PushToTalkXbox").gameObject;
		this.DisableTips();
		this.inVoice = VRCInputManager.FindInput("Voice");
	}

	// Token: 0x06006005 RID: 24581 RVA: 0x0021C9C4 File Offset: 0x0021ADC4
	private void SetupControlModeTip()
	{
		bool flag = false;
		if (!VRCInputManager.talkToggle && !VRCInputManager.talkDefaultOn)
		{
			flag = true;
		}
		if (VRCInputManager.showTooltips && flag)
		{
			if (VRCTrackingManager.IsInVRMode())
			{
				this.controllerMode = 0;
			}
			else if (VRCInputManager.IsPresent(VRCInputManager.InputMethod.Keyboard) && VRCInputManager.IsEnabled(VRCInputManager.InputMethod.Keyboard))
			{
				this.controllerMode = 2;
			}
			switch (this.controllerMode)
			{
			case 0:
				break;
			case 1:
				this.xboxTip.SetActive(true);
				this.tipActive = true;
				break;
			case 2:
				this.keybdTip.SetActive(true);
				this.tipActive = true;
				break;
			default:
				Debug.LogError("HudVoiceIndicator: Unknown control method=" + this.controllerMode);
				break;
			}
		}
		this.controlModeChecked = true;
	}

	// Token: 0x06006006 RID: 24582 RVA: 0x0021CAA5 File Offset: 0x0021AEA5
	private void DisableTips()
	{
		this.keybdTip.SetActive(false);
		this.xboxTip.SetActive(false);
		this.tipActive = false;
	}

	// Token: 0x06006007 RID: 24583 RVA: 0x0021CAC8 File Offset: 0x0021AEC8
	private void FlashMute()
	{
		if (this.dotDisabledImage.enabled)
		{
			this.flashTimer += Time.deltaTime;
			if (this.flashTimer > this.flashRate.x)
			{
				this.dotDisabledImage.enabled = false;
				this.flashTimer = 0f;
			}
		}
		else
		{
			this.flashTimer += Time.deltaTime;
			if (this.flashTimer > this.flashRate.y)
			{
				this.dotDisabledImage.enabled = true;
				this.flashTimer = 0f;
			}
		}
	}

	// Token: 0x06006008 RID: 24584 RVA: 0x0021CB68 File Offset: 0x0021AF68
	private void Update()
	{
		if (VRCPlayer.Instance != null)
		{
			if (!this.controlModeChecked)
			{
				this.SetupControlModeTip();
			}
			if (DefaultTalkController.IsLive())
			{
				this.dotImage.enabled = true;
				this.dotDisabledImage.enabled = false;
				if (VRCPlayer.Instance.isSpeaking)
				{
					this.dotImage.color = this.dotColor;
				}
				else
				{
					this.dotImage.color = new Color(this.dotColor.r, this.dotColor.g, this.dotColor.b, this.dotColor.a / 2f);
				}
			}
			else
			{
				this.dotImage.enabled = false;
				if (this.tipActive)
				{
					if (!VRCInputManager.talkToggle && !VRCInputManager.talkDefaultOn)
					{
						if (this.inVoice.down)
						{
							this.DisableTips();
						}
						this.FlashMute();
					}
					else
					{
						this.DisableTips();
					}
				}
				else
				{
					this.dotDisabledImage.enabled = true;
				}
			}
		}
		else
		{
			this.dotImage.enabled = false;
			this.dotDisabledImage.enabled = false;
		}
	}

	// Token: 0x040045B7 RID: 17847
	private Color dotColor;

	// Token: 0x040045B8 RID: 17848
	private Image dotImage;

	// Token: 0x040045B9 RID: 17849
	private Image dotDisabledImage;

	// Token: 0x040045BA RID: 17850
	private bool controlModeChecked;

	// Token: 0x040045BB RID: 17851
	private bool tipActive;

	// Token: 0x040045BC RID: 17852
	private GameObject keybdTip;

	// Token: 0x040045BD RID: 17853
	private GameObject xboxTip;

	// Token: 0x040045BE RID: 17854
	private int controllerMode = -1;

	// Token: 0x040045BF RID: 17855
	private VRCInput inVoice;

	// Token: 0x040045C0 RID: 17856
	private Vector2 flashRate = new Vector2(1f, 0.5f);

	// Token: 0x040045C1 RID: 17857
	private float flashTimer;
}
