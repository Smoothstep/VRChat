using System;
using UnityEngine;

// Token: 0x020006E8 RID: 1768
public class LipSyncDemo_SetCurrentTarget : MonoBehaviour
{
	// Token: 0x06003A3D RID: 14909 RVA: 0x00126962 File Offset: 0x00124D62
	private void Start()
	{
		OVRMessenger.AddListener<OVRTouchpad.TouchEvent>("Touchpad", new OVRCallback<OVRTouchpad.TouchEvent>(this.LocalTouchEventCallback));
		this.targetSet = 0;
		this.SwitchTargets[0].SetActive(0);
		this.SwitchTargets[1].SetActive(0);
	}

	// Token: 0x06003A3E RID: 14910 RVA: 0x001269A0 File Offset: 0x00124DA0
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.targetSet = 0;
			this.SetCurrentTarget();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.targetSet = 1;
			this.SetCurrentTarget();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			this.targetSet = 2;
			this.SetCurrentTarget();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			this.targetSet = 3;
			this.SetCurrentTarget();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	// Token: 0x06003A3F RID: 14911 RVA: 0x00126A34 File Offset: 0x00124E34
	private void SetCurrentTarget()
	{
		switch (this.targetSet)
		{
		case 0:
			this.SwitchTargets[0].SetActive(0);
			this.SwitchTargets[1].SetActive(0);
			break;
		case 1:
			this.SwitchTargets[0].SetActive(0);
			this.SwitchTargets[1].SetActive(1);
			break;
		case 2:
			this.SwitchTargets[0].SetActive(1);
			this.SwitchTargets[1].SetActive(2);
			break;
		case 3:
			this.SwitchTargets[0].SetActive(1);
			this.SwitchTargets[1].SetActive(3);
			break;
		}
	}

	// Token: 0x06003A40 RID: 14912 RVA: 0x00126AF0 File Offset: 0x00124EF0
	private void LocalTouchEventCallback(OVRTouchpad.TouchEvent touchEvent)
	{
		if (touchEvent != OVRTouchpad.TouchEvent.Left)
		{
			if (touchEvent == OVRTouchpad.TouchEvent.Right)
			{
				this.targetSet++;
				if (this.targetSet > 3)
				{
					this.targetSet = 0;
				}
				this.SetCurrentTarget();
			}
		}
		else
		{
			this.targetSet--;
			if (this.targetSet < 0)
			{
				this.targetSet = 3;
			}
			this.SetCurrentTarget();
		}
	}

	// Token: 0x04002320 RID: 8992
	public EnableSwitch[] SwitchTargets;

	// Token: 0x04002321 RID: 8993
	private int targetSet;
}
