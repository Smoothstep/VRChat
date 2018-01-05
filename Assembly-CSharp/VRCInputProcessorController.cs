using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B16 RID: 2838
public class VRCInputProcessorController : VRCInputProcessor
{
	// Token: 0x17000C77 RID: 3191
	// (get) Token: 0x06005637 RID: 22071 RVA: 0x001DACA5 File Offset: 0x001D90A5
	public override bool required
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000C78 RID: 3192
	// (get) Token: 0x06005638 RID: 22072 RVA: 0x001DACA8 File Offset: 0x001D90A8
	public override bool supported
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000C79 RID: 3193
	// (get) Token: 0x06005639 RID: 22073 RVA: 0x001DACAB File Offset: 0x001D90AB
	public override bool platformDefaultEnable
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000C7A RID: 3194
	// (get) Token: 0x0600563A RID: 22074 RVA: 0x001DACB0 File Offset: 0x001D90B0
	public override bool present
	{
		get
		{
			string[] joystickNames = Input.GetJoystickNames();
			return joystickNames != null && joystickNames.Length != 0;
		}
	}

	// Token: 0x17000C7B RID: 3195
	// (get) Token: 0x0600563B RID: 22075 RVA: 0x001DACD5 File Offset: 0x001D90D5
	public override string StorageTag
	{
		get
		{
			return "VRC_INPUT_CONTROLLER";
		}
	}

	// Token: 0x0600563C RID: 22076 RVA: 0x001DACDC File Offset: 0x001D90DC
	private void Start()
	{
		if (SteamVR.instance != null && SteamVR.instance.hmd_TrackingSystemName == "oculus")
		{
			this._isOculusWithSteam = true;
		}
	}

	// Token: 0x0600563D RID: 22077 RVA: 0x001DAD08 File Offset: 0x001D9108
	public override void Apply()
	{
		VRCInput.ClearChanges();
		if (this.IsValidJoystickConnected())
		{
			if (Input.GetAxis("Joy1 Axis 9") > 0.3f)
			{
				this.inMoveHoldFB.ApplyAxis(Input.GetAxis("Joy1 Axis 2"));
				this.inSpinHoldCwCcw.ApplyAxis(Input.GetAxis("Joy1 Axis 1"));
				this.inSpinHoldUD.ApplyAxis(Input.GetAxis("Joy1 Axis 5"));
				this.inSpinHoldLR.ApplyAxis(Input.GetAxis("Joy1 Axis 4"));
			}
			else
			{
				this.inHorizontal.ApplyAxis(Input.GetAxis("Joy1 Axis 1"));
				this.inVertical.ApplyAxis(-Input.GetAxis("Joy1 Axis 2"));
				this.inLookHorizontal.ApplyAxis(Input.GetAxis("Joy1 Axis 4"));
				this.inLookVertical.ApplyAxis(Input.GetAxis("Joy1 Axis 5"));
				float axis = Input.GetAxis("Joy1 Axis 4");
				if (Mathf.Abs(axis) > 0.5f)
				{
					this.inComfortLeft.ApplyButton(axis < 0f);
					this.inComfortRight.ApplyButton(axis > 0f);
				}
			}
			this.inVoice.ApplyButton(Input.GetKey(KeyCode.Joystick1Button1));
			this.inSelect.ApplyButton(Input.GetKey(KeyCode.Joystick1Button0));
			this.inJump.ApplyButton(Input.GetKey(KeyCode.Joystick1Button3));
			this.inRun.ApplyButton(Input.GetKey(KeyCode.Joystick1Button4));
			if (!this._isOculusWithSteam)
			{
				this.inBack.ApplyButton(Input.GetKey(KeyCode.Joystick1Button6));
			}
			this.inMenu.ApplyButton(Input.GetKey(KeyCode.Joystick1Button7));
			this.inResetOrientation.ApplyButton(Input.GetKey(KeyCode.Joystick1Button6));
			this.inDrop.ApplyButton(Input.GetKey(KeyCode.Joystick1Button5));
			float axis2 = Input.GetAxis("Joy1 Axis 10");
			this.inUse.ApplyButton(axis2 > 0.3f);
			this.inUse.ApplyButton(Input.GetKey(KeyCode.Joystick1Button0));
			this.inGrab.ApplyButton(axis2 > 0.3f);
			this.inGrab.ApplyButton(Input.GetKey(KeyCode.Joystick1Button0));
			this.inDrop.ApplyButton(Input.GetKey(KeyCode.Joystick1Button2));
			this.inUseAxisRight.ApplyAxis(axis2);
			this.inGrabAxisRight.ApplyAxis(axis2);
		}
		this._anyKey = VRCInput.IsChanged();
	}

	// Token: 0x0600563E RID: 22078 RVA: 0x001DAF84 File Offset: 0x001D9384
	private bool IsValidJoystickConnected()
	{
		string[] joystickNames = Input.GetJoystickNames();
		return joystickNames.Length > 0 && !joystickNames[0].Contains("OpenVR") && !joystickNames[0].Contains("Oculus");
	}

	// Token: 0x0600563F RID: 22079 RVA: 0x001DAFC8 File Offset: 0x001D93C8
	public override void ConnectInputs(Dictionary<string, VRCInput> inputs)
	{
		this.inHorizontal = inputs["Horizontal"];
		this.inVertical = inputs["Vertical"];
		this.inLookHorizontal = inputs["LookHorizontal"];
		this.inLookVertical = inputs["LookVertical"];
		this.inVoice = inputs["Voice"];
		this.inSelect = inputs["Select"];
		this.inJump = inputs["Jump"];
		this.inRun = inputs["Run"];
		this.inBack = inputs["Back"];
		this.inMenu = inputs["Menu"];
		this.inResetOrientation = inputs["Reset Orientation"];
		this.inComfortLeft = inputs["ComfortLeft"];
		this.inComfortRight = inputs["ComfortRight"];
		this.inDrop = inputs["DropRight"];
		this.inUse = inputs["UseRight"];
		this.inGrab = inputs["GrabRight"];
		this.inUseAxisRight = inputs["UseAxisRight"];
		this.inGrabAxisRight = inputs["GrabAxisRight"];
		this.inMoveHoldFB = inputs["MoveHoldFB"];
		this.inSpinHoldCwCcw = inputs["SpinHoldCwCcw"];
		this.inSpinHoldUD = inputs["SpinHoldUD"];
		this.inSpinHoldLR = inputs["SpinHoldLR"];
	}

	// Token: 0x04003CDF RID: 15583
	private const KeyCode XboxButtonA = KeyCode.Joystick1Button0;

	// Token: 0x04003CE0 RID: 15584
	private const KeyCode XboxButtonB = KeyCode.Joystick1Button1;

	// Token: 0x04003CE1 RID: 15585
	private const KeyCode XboxButtonX = KeyCode.Joystick1Button2;

	// Token: 0x04003CE2 RID: 15586
	private const KeyCode XboxButtonY = KeyCode.Joystick1Button3;

	// Token: 0x04003CE3 RID: 15587
	private const KeyCode XboxButtonBumperLeft = KeyCode.Joystick1Button4;

	// Token: 0x04003CE4 RID: 15588
	private const KeyCode XboxButtonBumperRight = KeyCode.Joystick1Button5;

	// Token: 0x04003CE5 RID: 15589
	private const KeyCode XboxButtonBack = KeyCode.Joystick1Button6;

	// Token: 0x04003CE6 RID: 15590
	private const KeyCode XboxButtonStart = KeyCode.Joystick1Button7;

	// Token: 0x04003CE7 RID: 15591
	private const KeyCode XboxButtonStickLeft = KeyCode.Joystick1Button8;

	// Token: 0x04003CE8 RID: 15592
	private const KeyCode XboxButtonStickRight = KeyCode.Joystick1Button9;

	// Token: 0x04003CE9 RID: 15593
	private const string XboxAxisLeftStickHorizontal = "Joy1 Axis 1";

	// Token: 0x04003CEA RID: 15594
	private const string XboxAxisLeftStickVertical = "Joy1 Axis 2";

	// Token: 0x04003CEB RID: 15595
	private const string XboxAxisRightStickHorizontal = "Joy1 Axis 4";

	// Token: 0x04003CEC RID: 15596
	private const string XboxAxisRightStickVertical = "Joy1 Axis 5";

	// Token: 0x04003CED RID: 15597
	private const string XboxAxisLeftTrigger = "Joy1 Axis 9";

	// Token: 0x04003CEE RID: 15598
	private const string XboxAxisRightTrigger = "Joy1 Axis 10";

	// Token: 0x04003CEF RID: 15599
	private VRCInput inHorizontal;

	// Token: 0x04003CF0 RID: 15600
	private VRCInput inVertical;

	// Token: 0x04003CF1 RID: 15601
	private VRCInput inLookHorizontal;

	// Token: 0x04003CF2 RID: 15602
	private VRCInput inLookVertical;

	// Token: 0x04003CF3 RID: 15603
	private VRCInput inVoice;

	// Token: 0x04003CF4 RID: 15604
	private VRCInput inSelect;

	// Token: 0x04003CF5 RID: 15605
	private VRCInput inJump;

	// Token: 0x04003CF6 RID: 15606
	private VRCInput inRun;

	// Token: 0x04003CF7 RID: 15607
	private VRCInput inBack;

	// Token: 0x04003CF8 RID: 15608
	private VRCInput inMenu;

	// Token: 0x04003CF9 RID: 15609
	private VRCInput inResetOrientation;

	// Token: 0x04003CFA RID: 15610
	private VRCInput inComfortLeft;

	// Token: 0x04003CFB RID: 15611
	private VRCInput inComfortRight;

	// Token: 0x04003CFC RID: 15612
	private VRCInput inDrop;

	// Token: 0x04003CFD RID: 15613
	private VRCInput inUse;

	// Token: 0x04003CFE RID: 15614
	private VRCInput inGrab;

	// Token: 0x04003CFF RID: 15615
	private VRCInput inUseAxisRight;

	// Token: 0x04003D00 RID: 15616
	private VRCInput inGrabAxisRight;

	// Token: 0x04003D01 RID: 15617
	private VRCInput inMoveHoldFB;

	// Token: 0x04003D02 RID: 15618
	private VRCInput inSpinHoldCwCcw;

	// Token: 0x04003D03 RID: 15619
	private VRCInput inSpinHoldUD;

	// Token: 0x04003D04 RID: 15620
	private VRCInput inSpinHoldLR;

	// Token: 0x04003D05 RID: 15621
	private bool _isOculusWithSteam;
}
