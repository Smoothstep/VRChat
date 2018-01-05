using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B1A RID: 2842
public class VRCInputProcessorMouse : VRCInputProcessor
{
	// Token: 0x17000C87 RID: 3207
	// (get) Token: 0x06005655 RID: 22101 RVA: 0x001DB490 File Offset: 0x001D9890
	public override bool required
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000C88 RID: 3208
	// (get) Token: 0x06005656 RID: 22102 RVA: 0x001DB493 File Offset: 0x001D9893
	public override bool supported
	{
		get
		{
			return Input.mousePresent;
		}
	}

	// Token: 0x17000C89 RID: 3209
	// (get) Token: 0x06005657 RID: 22103 RVA: 0x001DB49A File Offset: 0x001D989A
	public override bool platformDefaultEnable
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000C8A RID: 3210
	// (get) Token: 0x06005658 RID: 22104 RVA: 0x001DB49D File Offset: 0x001D989D
	public override bool present
	{
		get
		{
			return base.enabled && Input.mousePresent;
		}
	}

	// Token: 0x17000C8B RID: 3211
	// (get) Token: 0x06005659 RID: 22105 RVA: 0x001DB4B2 File Offset: 0x001D98B2
	public override string StorageTag
	{
		get
		{
			return "VRC_INPUT_MOUSE";
		}
	}

	// Token: 0x0600565A RID: 22106 RVA: 0x001DB4BC File Offset: 0x001D98BC
	public override void Apply()
	{
		VRCInput.ClearChanges();
		this.mouseUse.ApplyButton(Input.GetMouseButton(0));
		this.mouseGrab.ApplyButton(Input.GetMouseButton(0));
		this.mouseDrop.ApplyButton(Input.GetMouseButton(1));
		this.mouseX.ApplyAxis(Input.GetAxis("Mouse X"));
		this.mouseY.ApplyAxis(Input.GetAxis("Mouse Y"));
		this.mouseZ.ApplyAxis(Input.GetAxis("Mouse Wheel"));
		this._anyKey = VRCInput.IsChanged();
	}

	// Token: 0x0600565B RID: 22107 RVA: 0x001DB54C File Offset: 0x001D994C
	public override void ConnectInputs(Dictionary<string, VRCInput> inputs)
	{
		this.mouseUse = inputs["UseRight"];
		this.mouseGrab = inputs["GrabRight"];
		this.mouseDrop = inputs["DropRight"];
		this.mouseX = inputs["MouseX"];
		this.mouseY = inputs["MouseY"];
		this.mouseZ = inputs["MouseZ"];
	}

	// Token: 0x04003D1B RID: 15643
	private VRCInput mouseUse;

	// Token: 0x04003D1C RID: 15644
	private VRCInput mouseGrab;

	// Token: 0x04003D1D RID: 15645
	private VRCInput mouseDrop;

	// Token: 0x04003D1E RID: 15646
	private VRCInput mouseX;

	// Token: 0x04003D1F RID: 15647
	private VRCInput mouseY;

	// Token: 0x04003D20 RID: 15648
	private VRCInput mouseZ;
}
