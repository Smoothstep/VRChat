using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B19 RID: 2841
public class VRCInputProcessorKeyboard : VRCInputProcessor
{
	// Token: 0x17000C82 RID: 3202
	// (get) Token: 0x0600564D RID: 22093 RVA: 0x001DB22A File Offset: 0x001D962A
	public override bool required
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000C83 RID: 3203
	// (get) Token: 0x0600564E RID: 22094 RVA: 0x001DB22D File Offset: 0x001D962D
	public override bool supported
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000C84 RID: 3204
	// (get) Token: 0x0600564F RID: 22095 RVA: 0x001DB230 File Offset: 0x001D9630
	public override bool platformDefaultEnable
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000C85 RID: 3205
	// (get) Token: 0x06005650 RID: 22096 RVA: 0x001DB233 File Offset: 0x001D9633
	public override bool present
	{
		get
		{
			return base.enabled;
		}
	}

	// Token: 0x17000C86 RID: 3206
	// (get) Token: 0x06005651 RID: 22097 RVA: 0x001DB244 File Offset: 0x001D9644
	public override string StorageTag
	{
		get
		{
			return "VRC_INPUT_KEYBOARD";
		}
	}

	// Token: 0x06005652 RID: 22098 RVA: 0x001DB24C File Offset: 0x001D964C
	public override void Apply()
	{
		VRCInput.ClearChanges();
		this.inMoveForward.ApplyButton(Input.GetKey(KeyCode.W));
		this.inMoveBackward.ApplyButton(Input.GetKey(KeyCode.S));
		this.inMoveLeft.ApplyButton(Input.GetKey(KeyCode.A));
		this.inMoveRight.ApplyButton(Input.GetKey(KeyCode.D));
		this.inVoice.ApplyButton(Input.GetKey(KeyCode.V));
		this.inSelect.ApplyButton(Input.GetKey(KeyCode.Return));
		this.inJump.ApplyButton(Input.GetKey(KeyCode.Space));
		this.inRun.ApplyButton(Input.GetKey(KeyCode.LeftShift));
		this.inBack.ApplyButton(Input.GetKey(KeyCode.Escape));
		this.inMenu.ApplyButton(Input.GetKey(KeyCode.Escape));
		this.inResetOrientation.ApplyButton(Input.GetKey(KeyCode.R));
		this.inToggleSitStand.ApplyButton(Input.GetKey(KeyCode.T));
		this.inComfortLeft.ApplyButton(Input.GetKey(KeyCode.Comma));
		this.inComfortRight.ApplyButton(Input.GetKey(KeyCode.Period));
		this.inCapturePanoramo.ApplyButton(Input.GetKey(KeyCode.P));
		this._anyKey = VRCInput.IsChanged();
	}

	// Token: 0x06005653 RID: 22099 RVA: 0x001DB37C File Offset: 0x001D977C
	public override void ConnectInputs(Dictionary<string, VRCInput> inputs)
	{
		this.inMoveForward = inputs["MoveForward"];
		this.inMoveBackward = inputs["MoveBackward"];
		this.inMoveLeft = inputs["MoveLeft"];
		this.inMoveRight = inputs["MoveRight"];
		this.inVoice = inputs["Voice"];
		this.inSelect = inputs["Select"];
		this.inJump = inputs["Jump"];
		this.inRun = inputs["Run"];
		this.inBack = inputs["Back"];
		this.inMenu = inputs["Menu"];
		this.inResetOrientation = inputs["Reset Orientation"];
		this.inToggleSitStand = inputs["ToggleSitStand"];
		this.inComfortLeft = inputs["ComfortLeft"];
		this.inComfortRight = inputs["ComfortRight"];
		this.inCapturePanoramo = inputs["CapturePanorama"];
	}

	// Token: 0x04003D0C RID: 15628
	private VRCInput inMoveForward;

	// Token: 0x04003D0D RID: 15629
	private VRCInput inMoveBackward;

	// Token: 0x04003D0E RID: 15630
	private VRCInput inMoveLeft;

	// Token: 0x04003D0F RID: 15631
	private VRCInput inMoveRight;

	// Token: 0x04003D10 RID: 15632
	private VRCInput inVoice;

	// Token: 0x04003D11 RID: 15633
	private VRCInput inSelect;

	// Token: 0x04003D12 RID: 15634
	private VRCInput inJump;

	// Token: 0x04003D13 RID: 15635
	private VRCInput inRun;

	// Token: 0x04003D14 RID: 15636
	private VRCInput inBack;

	// Token: 0x04003D15 RID: 15637
	private VRCInput inMenu;

	// Token: 0x04003D16 RID: 15638
	private VRCInput inResetOrientation;

	// Token: 0x04003D17 RID: 15639
	private VRCInput inToggleSitStand;

	// Token: 0x04003D18 RID: 15640
	private VRCInput inComfortLeft;

	// Token: 0x04003D19 RID: 15641
	private VRCInput inComfortRight;

	// Token: 0x04003D1A RID: 15642
	private VRCInput inCapturePanoramo;
}
