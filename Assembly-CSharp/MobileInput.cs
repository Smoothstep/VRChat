using System;
using UnityEngine;

// Token: 0x02000A0B RID: 2571
public class MobileInput : VirtualInput
{
	// Token: 0x06004E19 RID: 19993 RVA: 0x001A24F9 File Offset: 0x001A08F9
	public override float GetAxis(string name, bool raw)
	{
		return (!this.virtualAxes.ContainsKey(name)) ? 0f : this.virtualAxes[name].GetValue;
	}

	// Token: 0x06004E1A RID: 19994 RVA: 0x001A2528 File Offset: 0x001A0928
	public override bool GetButton(string name, CrossPlatformInput.ButtonAction action)
	{
		if (!this.virtualButtons.ContainsKey(name))
		{
			throw new Exception(" Button " + name + " does not exist");
		}
		switch (action)
		{
		case CrossPlatformInput.ButtonAction.GetButtonDown:
			return this.virtualButtons[name].GetButtonDown;
		case CrossPlatformInput.ButtonAction.GetButtonUp:
			return this.virtualButtons[name].GetButtonUp;
		case CrossPlatformInput.ButtonAction.GetButton:
			return this.virtualButtons[name].GetButton;
		default:
			throw new Exception("Invalid button action.");
		}
	}

	// Token: 0x06004E1B RID: 19995 RVA: 0x001A25B5 File Offset: 0x001A09B5
	public override Vector3 MousePosition()
	{
		return base.virtualMousePosition;
	}
}
