using System;
using UnityEngine;

// Token: 0x0200069F RID: 1695
public class OVRPlatformMenu : MonoBehaviour
{
	// Token: 0x060038F4 RID: 14580 RVA: 0x00122928 File Offset: 0x00120D28
	private OVRPlatformMenu.eBackButtonAction ResetAndSendAction(OVRPlatformMenu.eBackButtonAction action)
	{
		MonoBehaviour.print("ResetAndSendAction( " + action + " );");
		this.downCount = 0;
		this.upCount = 0;
		this.initialDownTime = -1f;
		this.waitForUp = false;
		this.ResetCursor();
		if (action == OVRPlatformMenu.eBackButtonAction.LONG_PRESS)
		{
			this.waitForUp = true;
		}
		return action;
	}

	// Token: 0x060038F5 RID: 14581 RVA: 0x00122984 File Offset: 0x00120D84
	private OVRPlatformMenu.eBackButtonAction HandleBackButtonState()
	{
		if (this.waitForUp)
		{
			if (Input.GetKeyDown(this.keyCode) || Input.GetKey(this.keyCode))
			{
				return OVRPlatformMenu.eBackButtonAction.NONE;
			}
			this.waitForUp = false;
		}
		if (Input.GetKeyDown(this.keyCode))
		{
			this.downCount++;
			if (this.downCount == 1)
			{
				this.initialDownTime = Time.realtimeSinceStartup;
			}
		}
		else if (this.downCount > 0)
		{
			if (Input.GetKey(this.keyCode))
			{
				if (this.downCount <= this.upCount)
				{
					this.downCount++;
				}
				float num = Time.realtimeSinceStartup - this.initialDownTime;
				if (num > this.shortPressDelay)
				{
					float timerRotateRatio = (num - this.shortPressDelay) / (this.longPressDelay - this.shortPressDelay);
					this.UpdateCursor(timerRotateRatio);
				}
				if (num > this.longPressDelay)
				{
					return this.ResetAndSendAction(OVRPlatformMenu.eBackButtonAction.LONG_PRESS);
				}
			}
			else
			{
				bool flag = this.initialDownTime >= 0f;
				if (flag)
				{
					if (this.upCount < this.downCount)
					{
						this.upCount++;
					}
					float num2 = Time.realtimeSinceStartup - this.initialDownTime;
					if (num2 < this.doubleTapDelay)
					{
						if (this.downCount == 2 && this.upCount == 2)
						{
							return this.ResetAndSendAction(OVRPlatformMenu.eBackButtonAction.DOUBLE_TAP);
						}
					}
					else if (num2 > this.shortPressDelay)
					{
						if (this.downCount == 1 && this.upCount == 1)
						{
							return this.ResetAndSendAction(OVRPlatformMenu.eBackButtonAction.SHORT_PRESS);
						}
					}
					else if (num2 < this.longPressDelay)
					{
						return this.ResetAndSendAction(OVRPlatformMenu.eBackButtonAction.NONE);
					}
				}
			}
		}
		return OVRPlatformMenu.eBackButtonAction.NONE;
	}

	// Token: 0x060038F6 RID: 14582 RVA: 0x00122B4C File Offset: 0x00120F4C
	private void Awake()
	{
		if (!OVRManager.isHmdPresent)
		{
			base.enabled = false;
			return;
		}
		if (this.cursorTimer != null && this.instantiatedCursorTimer == null)
		{
			this.instantiatedCursorTimer = UnityEngine.Object.Instantiate<GameObject>(this.cursorTimer);
			if (this.instantiatedCursorTimer != null)
			{
				this.cursorTimerMaterial = this.instantiatedCursorTimer.GetComponent<Renderer>().material;
				this.cursorTimerMaterial.SetColor("_Color", this.cursorTimerColor);
				this.instantiatedCursorTimer.GetComponent<Renderer>().enabled = false;
			}
		}
	}

	// Token: 0x060038F7 RID: 14583 RVA: 0x00122BEC File Offset: 0x00120FEC
	private void OnDestroy()
	{
		if (this.cursorTimerMaterial != null)
		{
			UnityEngine.Object.Destroy(this.cursorTimerMaterial);
		}
	}

	// Token: 0x060038F8 RID: 14584 RVA: 0x00122C0A File Offset: 0x0012100A
	private void OnApplicationFocus(bool focusState)
	{
	}

	// Token: 0x060038F9 RID: 14585 RVA: 0x00122C0C File Offset: 0x0012100C
	private void OnApplicationPause(bool pauseStatus)
	{
		if (!pauseStatus)
		{
			Input.ResetInputAxes();
		}
	}

	// Token: 0x060038FA RID: 14586 RVA: 0x00122C19 File Offset: 0x00121019
	private void ShowConfirmQuitMenu()
	{
		this.ResetCursor();
	}

	// Token: 0x060038FB RID: 14587 RVA: 0x00122C21 File Offset: 0x00121021
	private void ShowGlobalMenu()
	{
	}

	// Token: 0x060038FC RID: 14588 RVA: 0x00122C23 File Offset: 0x00121023
	private void DoHandler(OVRPlatformMenu.eHandler handler)
	{
		if (handler == OVRPlatformMenu.eHandler.ResetCursor)
		{
			this.ResetCursor();
		}
		if (handler == OVRPlatformMenu.eHandler.ShowConfirmQuit)
		{
			this.ShowConfirmQuitMenu();
		}
		if (handler == OVRPlatformMenu.eHandler.ShowGlobalMenu)
		{
			this.ShowGlobalMenu();
		}
	}

	// Token: 0x060038FD RID: 14589 RVA: 0x00122C4B File Offset: 0x0012104B
	private void Update()
	{
	}

	// Token: 0x060038FE RID: 14590 RVA: 0x00122C50 File Offset: 0x00121050
	private void UpdateCursor(float timerRotateRatio)
	{
		timerRotateRatio = Mathf.Clamp(timerRotateRatio, 0f, 1f);
		if (this.instantiatedCursorTimer != null)
		{
			this.instantiatedCursorTimer.GetComponent<Renderer>().enabled = true;
			float value = Mathf.Clamp(1f - timerRotateRatio, 0f, 1f);
			this.cursorTimerMaterial.SetFloat("_ColorRampOffset", value);
			Vector3 forward = Camera.main.transform.forward;
			Vector3 position = Camera.main.transform.position;
			this.instantiatedCursorTimer.transform.position = position + forward * this.fixedDepth;
			this.instantiatedCursorTimer.transform.forward = forward;
		}
	}

	// Token: 0x060038FF RID: 14591 RVA: 0x00122D0C File Offset: 0x0012110C
	private void ResetCursor()
	{
		if (this.instantiatedCursorTimer != null)
		{
			this.cursorTimerMaterial.SetFloat("_ColorRampOffset", 1f);
			this.instantiatedCursorTimer.GetComponent<Renderer>().enabled = false;
		}
	}

	// Token: 0x040021CD RID: 8653
	public GameObject cursorTimer;

	// Token: 0x040021CE RID: 8654
	public Color cursorTimerColor = new Color(0f, 0.643f, 1f, 1f);

	// Token: 0x040021CF RID: 8655
	public float fixedDepth = 3f;

	// Token: 0x040021D0 RID: 8656
	public KeyCode keyCode = KeyCode.Escape;

	// Token: 0x040021D1 RID: 8657
	public OVRPlatformMenu.eHandler doubleTapHandler;

	// Token: 0x040021D2 RID: 8658
	public OVRPlatformMenu.eHandler shortPressHandler = OVRPlatformMenu.eHandler.ShowConfirmQuit;

	// Token: 0x040021D3 RID: 8659
	public OVRPlatformMenu.eHandler longPressHandler = OVRPlatformMenu.eHandler.ShowGlobalMenu;

	// Token: 0x040021D4 RID: 8660
	private GameObject instantiatedCursorTimer;

	// Token: 0x040021D5 RID: 8661
	private Material cursorTimerMaterial;

	// Token: 0x040021D6 RID: 8662
	private float doubleTapDelay = 0.25f;

	// Token: 0x040021D7 RID: 8663
	private float shortPressDelay = 0.25f;

	// Token: 0x040021D8 RID: 8664
	private float longPressDelay = 0.75f;

	// Token: 0x040021D9 RID: 8665
	private int downCount;

	// Token: 0x040021DA RID: 8666
	private int upCount;

	// Token: 0x040021DB RID: 8667
	private float initialDownTime = -1f;

	// Token: 0x040021DC RID: 8668
	private bool waitForUp;

	// Token: 0x020006A0 RID: 1696
	public enum eHandler
	{
		// Token: 0x040021DE RID: 8670
		ResetCursor,
		// Token: 0x040021DF RID: 8671
		ShowGlobalMenu,
		// Token: 0x040021E0 RID: 8672
		ShowConfirmQuit
	}

	// Token: 0x020006A1 RID: 1697
	private enum eBackButtonAction
	{
		// Token: 0x040021E2 RID: 8674
		NONE,
		// Token: 0x040021E3 RID: 8675
		DOUBLE_TAP,
		// Token: 0x040021E4 RID: 8676
		SHORT_PRESS,
		// Token: 0x040021E5 RID: 8677
		LONG_PRESS
	}
}
