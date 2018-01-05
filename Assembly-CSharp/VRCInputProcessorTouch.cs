using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// Token: 0x02000B1B RID: 2843
public class VRCInputProcessorTouch : VRCInputProcessor
{
	// Token: 0x17000C8C RID: 3212
	// (get) Token: 0x0600565D RID: 22109 RVA: 0x001DB5CE File Offset: 0x001D99CE
	public override bool required
	{
		get
		{
			return this._isUsingTouchControllers;
		}
	}

	// Token: 0x17000C8D RID: 3213
	// (get) Token: 0x0600565E RID: 22110 RVA: 0x001DB5D6 File Offset: 0x001D99D6
	public override bool supported
	{
		get
		{
			return this._isUsingTouchControllers;
		}
	}

	// Token: 0x17000C8E RID: 3214
	// (get) Token: 0x0600565F RID: 22111 RVA: 0x001DB5DE File Offset: 0x001D99DE
	public override bool platformDefaultEnable
	{
		get
		{
			return this._isUsingTouchControllers;
		}
	}

	// Token: 0x17000C8F RID: 3215
	// (get) Token: 0x06005660 RID: 22112 RVA: 0x001DB5E6 File Offset: 0x001D99E6
	public override bool present
	{
		get
		{
			return base.enabled && this._isUsingTouchControllers && (this.IsControllerConnected(OVRInput.Controller.LTouch) || this.IsControllerConnected(OVRInput.Controller.RTouch));
		}
	}

	// Token: 0x17000C90 RID: 3216
	// (get) Token: 0x06005661 RID: 22113 RVA: 0x001DB617 File Offset: 0x001D9A17
	public override string StorageTag
	{
		get
		{
			return "VRC_INPUT_TOUCH";
		}
	}

	// Token: 0x06005662 RID: 22114 RVA: 0x001DB620 File Offset: 0x001D9A20
	private void Start()
	{
		this._isUsingTouchControllers = true;
		this._isUsingTouchControllers = (SteamVR.instance != null && SteamVR.instance.hmd_TrackingSystemName.Contains("oculus"));
		Debug.Log("VRCInputProcessorTouch: using touch controller = " + this._isUsingTouchControllers);
		base.Init();
	}

	// Token: 0x06005663 RID: 22115 RVA: 0x001DB67C File Offset: 0x001D9A7C
	public override void Apply()
	{
		VRCInput.ClearChanges();
		this._anyKey = false;
		bool flag = false;
		if (!this._isUsingTouchControllers)
		{
			return;
		}
		if (this.viveControllerManager == null && VRCTrackingManager.IsInitialized())
		{
			this.viveControllerManager = VRCTrackingManager.GetTrackingComponent<SteamVR_ControllerManager>();
			return;
		}
		SteamVR_TrackedObject component = this.viveControllerManager.left.GetComponent<SteamVR_TrackedObject>();
		int index = (int)component.index;
		this.lControl = ((index < 0 || !component.isValid) ? null : SteamVR_Controller.Input(index));
		SteamVR_TrackedObject component2 = this.viveControllerManager.right.GetComponent<SteamVR_TrackedObject>();
		int index2 = (int)component2.index;
		this.rControl = ((index2 < 0 || !component2.isValid) ? null : SteamVR_Controller.Input(index2));
		if (this.IsControllerConnected(OVRInput.Controller.LTouch))
		{
			Vector2 axis2D = this.GetAxis2D(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
			this.inVertical.ApplyAxis(axis2D.y);
			this.inHorizontal.ApplyAxis(axis2D.x);
			this.inTouchpadLeftClick.ApplyButton(false);
			this.inTouchpadLeftX.ApplyAxis(0f);
			this.inTouchpadLeftY.ApplyAxis(0f);
			this.inBack.ApplyButton(this.GetButton(OVRInput.Button.Two, OVRInput.Controller.LTouch) || this.GetButton(OVRInput.Button.Start, OVRInput.Controller.LTouch));
			this.inGrabLeft.ApplyButton(this.GetButton(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch));
			this.inUseLeft.ApplyButton(this.GetButton(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch));
			this.inUseAxisLeft.ApplyAxis(this.GetAxis1D(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch));
			this.inGrabAxisLeft.ApplyAxis(this.GetAxis1D(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch));
			this._inFaceTouchL.ApplyButton(this.GetNearTouch(OVRInput.NearTouch.PrimaryThumbButtons, OVRInput.Controller.LTouch));
			this._inFaceTouchL.ApplyButton(this.GetTouch(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.LTouch) || this.GetTouch(OVRInput.Touch.One, OVRInput.Controller.LTouch) || this.GetTouch(OVRInput.Touch.Two, OVRInput.Controller.LTouch) || this.GetTouch(OVRInput.Touch.PrimaryThumbRest, OVRInput.Controller.LTouch));
			this._inFaceButtonTouchL.ApplyButton(this.GetTouch(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.LTouch) || this.GetTouch(OVRInput.Touch.One, OVRInput.Controller.LTouch) || this.GetTouch(OVRInput.Touch.Two, OVRInput.Controller.LTouch));
			this._inTriggerTouchL.ApplyButton(this.GetNearTouch(OVRInput.NearTouch.PrimaryIndexTrigger, OVRInput.Controller.LTouch));
			this._inThumbRestTouchL.ApplyButton(this.GetTouch(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.LTouch) || this.GetTouch(OVRInput.Touch.One, OVRInput.Controller.LTouch) || this.GetTouch(OVRInput.Touch.Two, OVRInput.Controller.LTouch));
			flag = VRCInput.IsChanged();
			this.inRun.ApplyButton(true);
			VRCInput.ClearChanges();
			if (this.GetVelocity(OVRInput.Controller.LTouch).sqrMagnitude > 0.1f)
			{
				VRCInput.MarkChanged();
			}
		}
		if (this.IsControllerConnected(OVRInput.Controller.RTouch))
		{
			Vector2 axis2D2 = this.GetAxis2D(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
			if (axis2D2.x > 0.7f)
			{
				this.inLookHorizontal.ApplyAxis(1f);
				this.inComfortRight.ApplyButton(true);
			}
			else if (axis2D2.x < -0.7f)
			{
				this.inLookHorizontal.ApplyAxis(-1f);
				this.inComfortLeft.ApplyButton(true);
			}
			this.inTouchpadRightClick.ApplyButton(false);
			this.inTouchpadRightX.ApplyAxis(0f);
			this.inTouchpadRightY.ApplyAxis(0f);
			this.inMenu.ApplyButton(this.GetButton(OVRInput.Button.Two, OVRInput.Controller.RTouch));
			this.inGrabRight.ApplyButton(this.GetButton(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch));
			this.inUseRight.ApplyButton(this.GetButton(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch));
			this.inUseAxisRight.ApplyAxis(this.GetAxis1D(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch));
			this.inGrabAxisRight.ApplyAxis(this.GetAxis1D(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch));
			this._inFaceTouchR.ApplyButton(this.GetNearTouch(OVRInput.NearTouch.PrimaryThumbButtons, OVRInput.Controller.RTouch));
			this._inFaceTouchR.ApplyButton(this.GetTouch(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.RTouch) || this.GetTouch(OVRInput.Touch.One, OVRInput.Controller.RTouch) || this.GetTouch(OVRInput.Touch.Two, OVRInput.Controller.RTouch) || this.GetTouch(OVRInput.Touch.PrimaryThumbRest, OVRInput.Controller.RTouch));
			this._inFaceButtonTouchR.ApplyButton(this.GetTouch(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.RTouch) || this.GetTouch(OVRInput.Touch.One, OVRInput.Controller.RTouch) || this.GetTouch(OVRInput.Touch.Two, OVRInput.Controller.RTouch));
			this._inTriggerTouchR.ApplyButton(this.GetNearTouch(OVRInput.NearTouch.PrimaryIndexTrigger, OVRInput.Controller.RTouch));
			this._inThumbRestTouchR.ApplyButton(this.GetTouch(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.RTouch) || this.GetTouch(OVRInput.Touch.One, OVRInput.Controller.RTouch) || this.GetTouch(OVRInput.Touch.Two, OVRInput.Controller.RTouch));
			this.inJump.ApplyButton(this.GetButton(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch));
			if (this.GetVelocity(OVRInput.Controller.RTouch).sqrMagnitude > 0.1f)
			{
				VRCInput.MarkChanged();
			}
		}
		this._anyKey = (flag || VRCInput.IsChanged());
	}

	// Token: 0x06005664 RID: 22116 RVA: 0x001DBB54 File Offset: 0x001D9F54
	private SteamVR_Controller.Device GetSteamController(OVRInput.Controller controller)
	{
		return (controller != OVRInput.Controller.LTouch) ? this.rControl : this.lControl;
	}

	// Token: 0x06005665 RID: 22117 RVA: 0x001DBB6E File Offset: 0x001D9F6E
	private bool IsControllerConnected(OVRInput.Controller controller)
	{
		return this.GetSteamController(controller) != null;
	}

	// Token: 0x06005666 RID: 22118 RVA: 0x001DBB80 File Offset: 0x001D9F80
	private float GetAxis1D(OVRInput.Axis1D axis, OVRInput.Controller controller)
	{
		EVRButtonId buttonId;
		if (axis != OVRInput.Axis1D.PrimaryHandTrigger)
		{
			if (axis != OVRInput.Axis1D.PrimaryIndexTrigger)
			{
				buttonId = EVRButtonId.k_EButton_Max;
			}
			else
			{
				buttonId = EVRButtonId.k_EButton_Axis1;
			}
		}
		else
		{
			buttonId = EVRButtonId.k_EButton_Axis2;
		}
		return this.GetSteamController(controller).GetAxis(buttonId).x;
	}

	// Token: 0x06005667 RID: 22119 RVA: 0x001DBBD0 File Offset: 0x001D9FD0
	private Vector2 GetAxis2D(OVRInput.Axis2D axis, OVRInput.Controller controller)
	{
		EVRButtonId buttonId;
		if (axis != OVRInput.Axis2D.PrimaryThumbstick)
		{
			buttonId = EVRButtonId.k_EButton_Max;
		}
		else
		{
			buttonId = EVRButtonId.k_EButton_Axis0;
		}
		return this.GetSteamController(controller).GetAxis(buttonId);
	}

	// Token: 0x06005668 RID: 22120 RVA: 0x001DBC0C File Offset: 0x001DA00C
	private bool GetButton(OVRInput.Button button, OVRInput.Controller controller)
	{
		EVRButtonId buttonId;
		if (button != OVRInput.Button.One)
		{
			if (button != OVRInput.Button.Two)
			{
				if (button != OVRInput.Button.Start)
				{
					if (button != OVRInput.Button.PrimaryIndexTrigger)
					{
						if (button != OVRInput.Button.PrimaryHandTrigger)
						{
							if (button != OVRInput.Button.PrimaryThumbstick)
							{
								buttonId = EVRButtonId.k_EButton_Max;
							}
							else
							{
								buttonId = EVRButtonId.k_EButton_Axis0;
							}
						}
						else
						{
							buttonId = EVRButtonId.k_EButton_Grip;
						}
					}
					else
					{
						buttonId = EVRButtonId.k_EButton_Axis1;
					}
				}
				else
				{
					buttonId = EVRButtonId.k_EButton_Max;
				}
			}
			else
			{
				buttonId = EVRButtonId.k_EButton_ApplicationMenu;
			}
		}
		else
		{
			buttonId = EVRButtonId.k_EButton_A;
		}
		return this.GetSteamController(controller).GetPress(buttonId);
	}

	// Token: 0x06005669 RID: 22121 RVA: 0x001DBCA0 File Offset: 0x001DA0A0
	private bool GetTouch(OVRInput.Touch touch, OVRInput.Controller controller)
	{
		EVRButtonId buttonId;
		if (touch != OVRInput.Touch.One)
		{
			if (touch != OVRInput.Touch.Two)
			{
				if (touch != OVRInput.Touch.PrimaryThumbRest)
				{
					if (touch != OVRInput.Touch.PrimaryIndexTrigger)
					{
						if (touch != OVRInput.Touch.PrimaryThumbstick)
						{
							buttonId = EVRButtonId.k_EButton_Max;
						}
						else
						{
							buttonId = EVRButtonId.k_EButton_Axis0;
						}
					}
					else
					{
						buttonId = EVRButtonId.k_EButton_Axis1;
					}
				}
				else
				{
					buttonId = EVRButtonId.k_EButton_Max;
				}
			}
			else
			{
				buttonId = EVRButtonId.k_EButton_ApplicationMenu;
			}
		}
		else
		{
			buttonId = EVRButtonId.k_EButton_A;
		}
		return this.GetSteamController(controller).GetTouch(buttonId);
	}

	// Token: 0x0600566A RID: 22122 RVA: 0x001DBD20 File Offset: 0x001DA120
	private bool GetNearTouch(OVRInput.NearTouch nearTouch, OVRInput.Controller controller)
	{
		if (nearTouch == OVRInput.NearTouch.PrimaryThumbButtons)
		{
			return this.GetTouch(OVRInput.Touch.PrimaryThumbstick, controller) || this.GetTouch(OVRInput.Touch.One, controller) || this.GetTouch(OVRInput.Touch.Two, controller);
		}
		return nearTouch == OVRInput.NearTouch.PrimaryIndexTrigger && this.GetTouch(OVRInput.Touch.PrimaryIndexTrigger, controller);
	}

	// Token: 0x0600566B RID: 22123 RVA: 0x001DBD73 File Offset: 0x001DA173
	private Vector3 GetVelocity(OVRInput.Controller controller)
	{
		return this.GetSteamController(controller).velocity;
	}

	// Token: 0x0600566C RID: 22124 RVA: 0x001DBD84 File Offset: 0x001DA184
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
		this.inGrabRight = inputs["GrabRight"];
		this.inUseRight = inputs["UseRight"];
		this.inGrabLeft = inputs["GrabLeft"];
		this.inUseLeft = inputs["UseLeft"];
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
		this._inFaceTouchL = VRCInputManager.FindInput("FaceTouchLeft");
		this._inFaceTouchR = VRCInputManager.FindInput("FaceTouchRight");
		this._inFaceButtonTouchL = VRCInputManager.FindInput("FaceButtonTouchLeft");
		this._inFaceButtonTouchR = VRCInputManager.FindInput("FaceButtonTouchRight");
		this._inTriggerTouchL = VRCInputManager.FindInput("TriggerTouchLeft");
		this._inTriggerTouchR = VRCInputManager.FindInput("TriggerTouchRight");
		this._inThumbRestTouchL = VRCInputManager.FindInput("ThumbRestTouchLeft");
		this._inThumbRestTouchR = VRCInputManager.FindInput("ThumbRestTouchRight");
	}

	// Token: 0x04003D21 RID: 15649
	private SteamVR_ControllerManager viveControllerManager;

	// Token: 0x04003D22 RID: 15650
	private SteamVR_Controller.Device lControl;

	// Token: 0x04003D23 RID: 15651
	private SteamVR_Controller.Device rControl;

	// Token: 0x04003D24 RID: 15652
	private bool _isUsingTouchControllers = true;

	// Token: 0x04003D25 RID: 15653
	private VRCInput inVertical;

	// Token: 0x04003D26 RID: 15654
	private VRCInput inHorizontal;

	// Token: 0x04003D27 RID: 15655
	private VRCInput inLookHorizontal;

	// Token: 0x04003D28 RID: 15656
	private VRCInput inLookVertical;

	// Token: 0x04003D29 RID: 15657
	private VRCInput inJump;

	// Token: 0x04003D2A RID: 15658
	private VRCInput inRun;

	// Token: 0x04003D2B RID: 15659
	private VRCInput inBack;

	// Token: 0x04003D2C RID: 15660
	private VRCInput inMenu;

	// Token: 0x04003D2D RID: 15661
	private VRCInput inComfortLeft;

	// Token: 0x04003D2E RID: 15662
	private VRCInput inComfortRight;

	// Token: 0x04003D2F RID: 15663
	private VRCInput inUseRight;

	// Token: 0x04003D30 RID: 15664
	private VRCInput inGrabRight;

	// Token: 0x04003D31 RID: 15665
	private VRCInput inUseLeft;

	// Token: 0x04003D32 RID: 15666
	private VRCInput inGrabLeft;

	// Token: 0x04003D33 RID: 15667
	private VRCInput inTouchpadLeftClick;

	// Token: 0x04003D34 RID: 15668
	private VRCInput inTouchpadLeftX;

	// Token: 0x04003D35 RID: 15669
	private VRCInput inTouchpadLeftY;

	// Token: 0x04003D36 RID: 15670
	private VRCInput inTouchpadRightClick;

	// Token: 0x04003D37 RID: 15671
	private VRCInput inTouchpadRightX;

	// Token: 0x04003D38 RID: 15672
	private VRCInput inTouchpadRightY;

	// Token: 0x04003D39 RID: 15673
	private VRCInput inUseAxisLeft;

	// Token: 0x04003D3A RID: 15674
	private VRCInput inUseAxisRight;

	// Token: 0x04003D3B RID: 15675
	private VRCInput inGrabAxisLeft;

	// Token: 0x04003D3C RID: 15676
	private VRCInput inGrabAxisRight;

	// Token: 0x04003D3D RID: 15677
	private VRCInput _inFaceTouchL;

	// Token: 0x04003D3E RID: 15678
	private VRCInput _inFaceTouchR;

	// Token: 0x04003D3F RID: 15679
	private VRCInput _inFaceButtonTouchL;

	// Token: 0x04003D40 RID: 15680
	private VRCInput _inFaceButtonTouchR;

	// Token: 0x04003D41 RID: 15681
	private VRCInput _inTriggerTouchL;

	// Token: 0x04003D42 RID: 15682
	private VRCInput _inTriggerTouchR;

	// Token: 0x04003D43 RID: 15683
	private VRCInput _inThumbRestTouchL;

	// Token: 0x04003D44 RID: 15684
	private VRCInput _inThumbRestTouchR;
}
