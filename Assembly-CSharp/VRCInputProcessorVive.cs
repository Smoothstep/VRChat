using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// Token: 0x02000B1C RID: 2844
public class VRCInputProcessorVive : VRCInputProcessor
{
	// Token: 0x17000C91 RID: 3217
	// (get) Token: 0x0600566E RID: 22126 RVA: 0x001DBFB8 File Offset: 0x001DA3B8
	public override bool required
	{
		get
		{
			return this._isUsingViveControllers;
		}
	}

	// Token: 0x17000C92 RID: 3218
	// (get) Token: 0x0600566F RID: 22127 RVA: 0x001DBFC0 File Offset: 0x001DA3C0
	public override bool supported
	{
		get
		{
			return this._isUsingViveControllers;
		}
	}

	// Token: 0x17000C93 RID: 3219
	// (get) Token: 0x06005670 RID: 22128 RVA: 0x001DBFC8 File Offset: 0x001DA3C8
	public override bool platformDefaultEnable
	{
		get
		{
			return this._isUsingViveControllers;
		}
	}

	// Token: 0x17000C94 RID: 3220
	// (get) Token: 0x06005671 RID: 22129 RVA: 0x001DBFD0 File Offset: 0x001DA3D0
	public override bool present
	{
		get
		{
			return base.enabled && SteamVR.active && this._isUsingViveControllers;
		}
	}

	// Token: 0x17000C95 RID: 3221
	// (get) Token: 0x06005672 RID: 22130 RVA: 0x001DBFF0 File Offset: 0x001DA3F0
	public override string StorageTag
	{
		get
		{
			return "VRC_INPUT_VIVE";
		}
	}

	// Token: 0x06005673 RID: 22131 RVA: 0x001DBFF8 File Offset: 0x001DA3F8
	private void Start()
	{
		this._isUsingViveControllers = true;
		if (VRCTrackingManager.IsInitialized())
		{
			this.viveControllerManager = VRCTrackingManager.GetTrackingComponent<SteamVR_ControllerManager>();
		}
		if (SteamVR.instance != null)
		{
			Debug.Log("STEAM Tracking System: " + SteamVR.instance.hmd_TrackingSystemName);
		}
		this._isUsingViveControllers = (SteamVR.instance != null && !SteamVR.instance.hmd_TrackingSystemName.Contains("oculus"));
		Debug.Log("VRCInputProcessorVive: using vive controllers = " + this._isUsingViveControllers);
		base.Init();
	}

	// Token: 0x06005674 RID: 22132 RVA: 0x001DC090 File Offset: 0x001DA490
	public override void Apply()
	{
		VRCInput.ClearChanges();
		this._anyKey = false;
		bool flag = false;
		if (this.viveControllerManager == null && VRCTrackingManager.IsInitialized())
		{
			this.viveControllerManager = VRCTrackingManager.GetTrackingComponent<SteamVR_ControllerManager>();
			return;
		}
		if (!SteamVR.active)
		{
			return;
		}
		if (!this._isUsingViveControllers)
		{
			return;
		}
		SteamVR_TrackedObject component = this.viveControllerManager.left.GetComponent<SteamVR_TrackedObject>();
		int index = (int)component.index;
		if (index >= 0 && component.isValid)
		{
			SteamVR_Controller.Device device = SteamVR_Controller.Input(index);
			Vector2 axis = device.GetAxis(EVRButtonId.k_EButton_Axis0);
			if (device.GetPress(EVRButtonId.k_EButton_Axis0))
			{
				axis.Normalize();
				this.inVertical.ApplyAxis(axis.y);
				this.inHorizontal.ApplyAxis(axis.x);
				this.inTouchpadLeftClick.ApplyButton(true);
			}
			else
			{
				this.inTouchpadLeftClick.ApplyButton(false);
			}
			if (device.GetTouch(EVRButtonId.k_EButton_Axis0))
			{
				this.inTouchpadLeftX.ApplyAxis(axis.x);
				this.inTouchpadLeftY.ApplyAxis(axis.y);
			}
			else
			{
				this.inTouchpadLeftX.ApplyAxis(0f);
				this.inTouchpadLeftY.ApplyAxis(0f);
			}
			this.inBack.ApplyButton(device.GetPress(EVRButtonId.k_EButton_ApplicationMenu));
			flag = VRCInput.IsChanged();
			this.inRun.ApplyButton(true);
			VRCInput.ClearChanges();
			this.inDropLeft.ApplyButton(device.GetPress(EVRButtonId.k_EButton_Grip));
			this.inUseLeft.ApplyButton(device.GetHairTrigger());
			this.inGrabLeft.ApplyButton(device.GetHairTrigger());
			Vector2 axis2 = device.GetAxis(EVRButtonId.k_EButton_Axis1);
			this.inUseAxisLeft.ApplyAxis(axis2.x);
			this.inGrabAxisLeft.ApplyAxis(axis2.x);
			if (device.velocity.sqrMagnitude > 0.1f)
			{
				VRCInput.MarkChanged();
			}
		}
		SteamVR_TrackedObject component2 = this.viveControllerManager.right.GetComponent<SteamVR_TrackedObject>();
		int index2 = (int)component2.index;
		if (index2 >= 0 && component2.isValid)
		{
			SteamVR_Controller.Device device2 = SteamVR_Controller.Input(index2);
			Vector2 axis3 = device2.GetAxis(EVRButtonId.k_EButton_Axis0);
			if (device2.GetPress(EVRButtonId.k_EButton_Axis0))
			{
				if (axis3.y > 0.7f)
				{
					if (device2.GetPressDown(EVRButtonId.k_EButton_Axis0))
					{
						this.inJump.ApplyButton(true);
					}
				}
				else if (axis3.x > 0.7f)
				{
					this.inLookHorizontal.ApplyAxis(1f);
					this.inComfortRight.ApplyButton(true);
				}
				else if (axis3.x < -0.7f)
				{
					this.inLookHorizontal.ApplyAxis(-1f);
					this.inComfortLeft.ApplyButton(true);
				}
				this.inTouchpadRightClick.ApplyButton(true);
			}
			else
			{
				this.inTouchpadRightClick.ApplyButton(false);
			}
			if (device2.GetTouch(EVRButtonId.k_EButton_Axis0))
			{
				this.inTouchpadRightX.ApplyAxis(axis3.x);
				this.inTouchpadRightY.ApplyAxis(axis3.y);
			}
			else
			{
				this.inTouchpadRightX.ApplyAxis(0f);
				this.inTouchpadRightY.ApplyAxis(0f);
			}
			this.inMenu.ApplyButton(device2.GetPress(EVRButtonId.k_EButton_ApplicationMenu));
			this.inDropRight.ApplyButton(device2.GetPress(EVRButtonId.k_EButton_Grip));
			this.inUseRight.ApplyButton(device2.GetHairTrigger());
			this.inGrabRight.ApplyButton(device2.GetHairTrigger());
			Vector2 axis4 = device2.GetAxis(EVRButtonId.k_EButton_Axis1);
			this.inUseAxisRight.ApplyAxis(axis4.x);
			this.inGrabAxisRight.ApplyAxis(axis4.x);
			if (device2.velocity.sqrMagnitude > 0.1f)
			{
				VRCInput.MarkChanged();
			}
		}
		this._anyKey = (flag || VRCInput.IsChanged());
	}

	// Token: 0x06005675 RID: 22133 RVA: 0x001DC478 File Offset: 0x001DA878
	public override void ConnectInputs(Dictionary<string, VRCInput> inputs)
	{
		this.inVertical = inputs["Vertical"];
		this.inHorizontal = inputs["Horizontal"];
		this.inLookHorizontal = inputs["LookHorizontal"];
		this.inLookVertical = inputs["LookVertical"];
		this.inJump = inputs["Jump"];
		this.inRun = inputs["Run"];
		this.inBack = inputs["Back"];
		this.inMenu = inputs["Menu"];
		this.inComfortLeft = inputs["ComfortLeft"];
		this.inComfortRight = inputs["ComfortRight"];
		this.inDropRight = inputs["DropRight"];
		this.inUseRight = inputs["UseRight"];
		this.inGrabRight = inputs["GrabRight"];
		this.inDropLeft = inputs["DropLeft"];
		this.inUseLeft = inputs["UseLeft"];
		this.inGrabLeft = inputs["GrabLeft"];
		this.inTouchpadLeftClick = inputs["TouchpadLeftClick"];
		this.inTouchpadLeftX = inputs["TouchpadLeftX"];
		this.inTouchpadLeftY = inputs["TouchpadLeftY"];
		this.inTouchpadRightClick = inputs["TouchpadRightClick"];
		this.inTouchpadRightX = inputs["TouchpadRightX"];
		this.inTouchpadRightY = inputs["TouchpadRightY"];
		this.inUseAxisLeft = inputs["UseAxisLeft"];
		this.inUseAxisRight = inputs["UseAxisRight"];
		this.inGrabAxisLeft = inputs["GrabAxisLeft"];
		this.inGrabAxisRight = inputs["GrabAxisRight"];
	}

	// Token: 0x04003D45 RID: 15685
	private SteamVR_ControllerManager viveControllerManager;

	// Token: 0x04003D46 RID: 15686
	private bool _isUsingViveControllers = true;

	// Token: 0x04003D47 RID: 15687
	private VRCInput inVertical;

	// Token: 0x04003D48 RID: 15688
	private VRCInput inHorizontal;

	// Token: 0x04003D49 RID: 15689
	private VRCInput inLookHorizontal;

	// Token: 0x04003D4A RID: 15690
	private VRCInput inLookVertical;

	// Token: 0x04003D4B RID: 15691
	private VRCInput inJump;

	// Token: 0x04003D4C RID: 15692
	private VRCInput inRun;

	// Token: 0x04003D4D RID: 15693
	private VRCInput inBack;

	// Token: 0x04003D4E RID: 15694
	private VRCInput inMenu;

	// Token: 0x04003D4F RID: 15695
	private VRCInput inComfortLeft;

	// Token: 0x04003D50 RID: 15696
	private VRCInput inComfortRight;

	// Token: 0x04003D51 RID: 15697
	private VRCInput inDropRight;

	// Token: 0x04003D52 RID: 15698
	private VRCInput inUseRight;

	// Token: 0x04003D53 RID: 15699
	private VRCInput inGrabRight;

	// Token: 0x04003D54 RID: 15700
	private VRCInput inDropLeft;

	// Token: 0x04003D55 RID: 15701
	private VRCInput inUseLeft;

	// Token: 0x04003D56 RID: 15702
	private VRCInput inGrabLeft;

	// Token: 0x04003D57 RID: 15703
	private VRCInput inTouchpadLeftClick;

	// Token: 0x04003D58 RID: 15704
	private VRCInput inTouchpadLeftX;

	// Token: 0x04003D59 RID: 15705
	private VRCInput inTouchpadLeftY;

	// Token: 0x04003D5A RID: 15706
	private VRCInput inTouchpadRightClick;

	// Token: 0x04003D5B RID: 15707
	private VRCInput inTouchpadRightX;

	// Token: 0x04003D5C RID: 15708
	private VRCInput inTouchpadRightY;

	// Token: 0x04003D5D RID: 15709
	private VRCInput inUseAxisLeft;

	// Token: 0x04003D5E RID: 15710
	private VRCInput inUseAxisRight;

	// Token: 0x04003D5F RID: 15711
	private VRCInput inGrabAxisLeft;

	// Token: 0x04003D60 RID: 15712
	private VRCInput inGrabAxisRight;
}
