using System;
using UnityEngine;

// Token: 0x02000A0C RID: 2572
public class StandaloneInput : VirtualInput
{
	// Token: 0x06004E1D RID: 19997 RVA: 0x001A25C5 File Offset: 0x001A09C5
	public override float GetAxis(string name, bool raw)
	{
		return (!this.alwaysUseVirtual.Contains(name)) ? Input.GetAxis(name) : this.virtualAxes[name].GetValue;
	}

	// Token: 0x06004E1E RID: 19998 RVA: 0x001A25F4 File Offset: 0x001A09F4
	public override bool GetButton(string name, CrossPlatformInput.ButtonAction action)
	{
		switch (action)
		{
		case CrossPlatformInput.ButtonAction.GetButtonDown:
			return (!this.alwaysUseVirtual.Contains(name)) ? Input.GetButtonDown(name) : this.virtualButtons[name].GetButtonDown;
		case CrossPlatformInput.ButtonAction.GetButtonUp:
			return (!this.alwaysUseVirtual.Contains(name)) ? Input.GetButtonUp(name) : this.virtualButtons[name].GetButtonUp;
		case CrossPlatformInput.ButtonAction.GetButton:
			return (!this.alwaysUseVirtual.Contains(name)) ? Input.GetButton(name) : this.virtualButtons[name].GetButton;
		default:
			throw new Exception("Invalid button action.");
		}
	}

	// Token: 0x06004E1F RID: 19999 RVA: 0x001A26AC File Offset: 0x001A0AAC
	public override Vector3 MousePosition()
	{
		return Input.mousePosition;
	}
}
