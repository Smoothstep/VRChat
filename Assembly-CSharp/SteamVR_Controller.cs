using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Valve.VR;

// Token: 0x02000BEB RID: 3051
public class SteamVR_Controller
{
	// Token: 0x06005EA2 RID: 24226 RVA: 0x00212430 File Offset: 0x00210830
	public static SteamVR_Controller.Device Input(int deviceIndex)
	{
		if (SteamVR_Controller.devices == null)
		{
			SteamVR_Controller.devices = new SteamVR_Controller.Device[16];
			uint num = 0u;
			while ((ulong)num < (ulong)((long)SteamVR_Controller.devices.Length))
			{
				SteamVR_Controller.devices[(int)((UIntPtr)num)] = new SteamVR_Controller.Device(num);
				num += 1u;
			}
		}
		return SteamVR_Controller.devices[deviceIndex];
	}

	// Token: 0x06005EA3 RID: 24227 RVA: 0x00212484 File Offset: 0x00210884
	public static void Update()
	{
		int num = 0;
		while ((long)num < 16L)
		{
			SteamVR_Controller.Input(num).Update();
			num++;
		}
	}

	// Token: 0x06005EA4 RID: 24228 RVA: 0x002124B4 File Offset: 0x002108B4
	public static int GetDeviceIndex(SteamVR_Controller.DeviceRelation relation, ETrackedDeviceClass deviceClass = ETrackedDeviceClass.Controller, int relativeTo = 0)
	{
		int result = -1;
		SteamVR_Utils.RigidTransform t = (relativeTo >= 16) ? SteamVR_Utils.RigidTransform.identity : SteamVR_Controller.Input(relativeTo).transform.GetInverse();
		CVRSystem system = OpenVR.System;
		if (system == null)
		{
			return result;
		}
		float num = float.MinValue;
		int num2 = 0;
		while ((long)num2 < 16L)
		{
			if (num2 != relativeTo && system.GetTrackedDeviceClass((uint)num2) == deviceClass)
			{
				SteamVR_Controller.Device device = SteamVR_Controller.Input(num2);
				if (device.connected)
				{
					if (relation == SteamVR_Controller.DeviceRelation.First)
					{
						return num2;
					}
					Vector3 vector = t * device.transform.pos;
					float num3;
					if (relation == SteamVR_Controller.DeviceRelation.FarthestRight)
					{
						num3 = vector.x;
					}
					else if (relation == SteamVR_Controller.DeviceRelation.FarthestLeft)
					{
						num3 = -vector.x;
					}
					else
					{
						Vector3 vector2 = new Vector3(vector.x, 0f, vector.z);
						Vector3 normalized = vector2.normalized;
						float num4 = Vector3.Dot(normalized, Vector3.forward);
						Vector3 vector3 = Vector3.Cross(normalized, Vector3.forward);
						if (relation == SteamVR_Controller.DeviceRelation.Leftmost)
						{
							num3 = ((vector3.y <= 0f) ? num4 : (2f - num4));
						}
						else
						{
							num3 = ((vector3.y >= 0f) ? num4 : (2f - num4));
						}
					}
					if (num3 > num)
					{
						result = num2;
						num = num3;
					}
				}
			}
			num2++;
		}
		return result;
	}

	// Token: 0x0400444F RID: 17487
	private static SteamVR_Controller.Device[] devices;

	// Token: 0x02000BEC RID: 3052
	public class ButtonMask
	{
		// Token: 0x04004450 RID: 17488
		public const ulong System = 1UL;

		// Token: 0x04004451 RID: 17489
		public const ulong ApplicationMenu = 2UL;

		// Token: 0x04004452 RID: 17490
		public const ulong Grip = 4UL;

		// Token: 0x04004453 RID: 17491
		public const ulong Axis0 = 4294967296UL;

		// Token: 0x04004454 RID: 17492
		public const ulong Axis1 = 8589934592UL;

		// Token: 0x04004455 RID: 17493
		public const ulong Axis2 = 17179869184UL;

		// Token: 0x04004456 RID: 17494
		public const ulong Axis3 = 34359738368UL;

		// Token: 0x04004457 RID: 17495
		public const ulong Axis4 = 68719476736UL;

		// Token: 0x04004458 RID: 17496
		public const ulong Touchpad = 4294967296UL;

		// Token: 0x04004459 RID: 17497
		public const ulong Trigger = 8589934592UL;
	}

	// Token: 0x02000BED RID: 3053
	public class Device
	{
		// Token: 0x06005EA6 RID: 24230 RVA: 0x00212640 File Offset: 0x00210A40
		public Device(uint i)
		{
			this.index = i;
		}

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x06005EA7 RID: 24231 RVA: 0x00212661 File Offset: 0x00210A61
		// (set) Token: 0x06005EA8 RID: 24232 RVA: 0x00212669 File Offset: 0x00210A69
		public uint index { get; private set; }

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x06005EA9 RID: 24233 RVA: 0x00212672 File Offset: 0x00210A72
		// (set) Token: 0x06005EAA RID: 24234 RVA: 0x0021267A File Offset: 0x00210A7A
		public bool valid { get; private set; }

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x06005EAB RID: 24235 RVA: 0x00212683 File Offset: 0x00210A83
		public bool connected
		{
			get
			{
				this.Update();
				return this.pose.bDeviceIsConnected;
			}
		}

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x06005EAC RID: 24236 RVA: 0x00212696 File Offset: 0x00210A96
		public bool hasTracking
		{
			get
			{
				this.Update();
				return this.pose.bPoseIsValid;
			}
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x06005EAD RID: 24237 RVA: 0x002126A9 File Offset: 0x00210AA9
		public bool outOfRange
		{
			get
			{
				this.Update();
				return this.pose.eTrackingResult == ETrackingResult.Running_OutOfRange || this.pose.eTrackingResult == ETrackingResult.Calibrating_OutOfRange;
			}
		}

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x06005EAE RID: 24238 RVA: 0x002126D8 File Offset: 0x00210AD8
		public bool calibrating
		{
			get
			{
				this.Update();
				return this.pose.eTrackingResult == ETrackingResult.Calibrating_InProgress || this.pose.eTrackingResult == ETrackingResult.Calibrating_OutOfRange;
			}
		}

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x06005EAF RID: 24239 RVA: 0x00212704 File Offset: 0x00210B04
		public bool uninitialized
		{
			get
			{
				this.Update();
				return this.pose.eTrackingResult == ETrackingResult.Uninitialized;
			}
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x06005EB0 RID: 24240 RVA: 0x0021271A File Offset: 0x00210B1A
		public SteamVR_Utils.RigidTransform transform
		{
			get
			{
				this.Update();
				return new SteamVR_Utils.RigidTransform(this.pose.mDeviceToAbsoluteTracking);
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x06005EB1 RID: 24241 RVA: 0x00212732 File Offset: 0x00210B32
		public Vector3 velocity
		{
			get
			{
				this.Update();
				return new Vector3(this.pose.vVelocity.v0, this.pose.vVelocity.v1, -this.pose.vVelocity.v2);
			}
		}

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x06005EB2 RID: 24242 RVA: 0x00212770 File Offset: 0x00210B70
		public Vector3 angularVelocity
		{
			get
			{
				this.Update();
				return new Vector3(-this.pose.vAngularVelocity.v0, -this.pose.vAngularVelocity.v1, this.pose.vAngularVelocity.v2);
			}
		}

		// Token: 0x06005EB3 RID: 24243 RVA: 0x002127AF File Offset: 0x00210BAF
		public VRControllerState_t GetState()
		{
			this.Update();
			return this.state;
		}

		// Token: 0x06005EB4 RID: 24244 RVA: 0x002127BD File Offset: 0x00210BBD
		public VRControllerState_t GetPrevState()
		{
			this.Update();
			return this.prevState;
		}

		// Token: 0x06005EB5 RID: 24245 RVA: 0x002127CB File Offset: 0x00210BCB
		public TrackedDevicePose_t GetPose()
		{
			this.Update();
			return this.pose;
		}

		// Token: 0x06005EB6 RID: 24246 RVA: 0x002127DC File Offset: 0x00210BDC
		public void Update()
		{
			if (Time.frameCount != this.prevFrameCount)
			{
				this.prevFrameCount = Time.frameCount;
				this.prevState = this.state;
				CVRSystem system = OpenVR.System;
				if (system != null)
				{
					this.valid = system.GetControllerStateWithPose(SteamVR_Render.instance.trackingSpace, this.index, ref this.state, (uint)Marshal.SizeOf(typeof(VRControllerState_t)), ref this.pose);
					this.UpdateHairTrigger();
				}
			}
		}

		// Token: 0x06005EB7 RID: 24247 RVA: 0x00212859 File Offset: 0x00210C59
		public bool GetPress(ulong buttonMask)
		{
			this.Update();
			return (this.state.ulButtonPressed & buttonMask) != 0UL;
		}

		// Token: 0x06005EB8 RID: 24248 RVA: 0x00212875 File Offset: 0x00210C75
		public bool GetPressDown(ulong buttonMask)
		{
			this.Update();
			return (this.state.ulButtonPressed & buttonMask) != 0UL && (this.prevState.ulButtonPressed & buttonMask) == 0UL;
		}

		// Token: 0x06005EB9 RID: 24249 RVA: 0x002128A5 File Offset: 0x00210CA5
		public bool GetPressUp(ulong buttonMask)
		{
			this.Update();
			return (this.state.ulButtonPressed & buttonMask) == 0UL && (this.prevState.ulButtonPressed & buttonMask) != 0UL;
		}

		// Token: 0x06005EBA RID: 24250 RVA: 0x002128D8 File Offset: 0x00210CD8
		public bool GetPress(EVRButtonId buttonId)
		{
			return this.GetPress(1UL << (int)buttonId);
		}

		// Token: 0x06005EBB RID: 24251 RVA: 0x002128E7 File Offset: 0x00210CE7
		public bool GetPressDown(EVRButtonId buttonId)
		{
			return this.GetPressDown(1UL << (int)buttonId);
		}

		// Token: 0x06005EBC RID: 24252 RVA: 0x002128F6 File Offset: 0x00210CF6
		public bool GetPressUp(EVRButtonId buttonId)
		{
			return this.GetPressUp(1UL << (int)buttonId);
		}

		// Token: 0x06005EBD RID: 24253 RVA: 0x00212905 File Offset: 0x00210D05
		public bool GetTouch(ulong buttonMask)
		{
			this.Update();
			return (this.state.ulButtonTouched & buttonMask) != 0UL;
		}

		// Token: 0x06005EBE RID: 24254 RVA: 0x00212921 File Offset: 0x00210D21
		public bool GetTouchDown(ulong buttonMask)
		{
			this.Update();
			return (this.state.ulButtonTouched & buttonMask) != 0UL && (this.prevState.ulButtonTouched & buttonMask) == 0UL;
		}

		// Token: 0x06005EBF RID: 24255 RVA: 0x00212951 File Offset: 0x00210D51
		public bool GetTouchUp(ulong buttonMask)
		{
			this.Update();
			return (this.state.ulButtonTouched & buttonMask) == 0UL && (this.prevState.ulButtonTouched & buttonMask) != 0UL;
		}

		// Token: 0x06005EC0 RID: 24256 RVA: 0x00212984 File Offset: 0x00210D84
		public bool GetTouch(EVRButtonId buttonId)
		{
			return this.GetTouch(1UL << (int)buttonId);
		}

		// Token: 0x06005EC1 RID: 24257 RVA: 0x00212993 File Offset: 0x00210D93
		public bool GetTouchDown(EVRButtonId buttonId)
		{
			return this.GetTouchDown(1UL << (int)buttonId);
		}

		// Token: 0x06005EC2 RID: 24258 RVA: 0x002129A2 File Offset: 0x00210DA2
		public bool GetTouchUp(EVRButtonId buttonId)
		{
			return this.GetTouchUp(1UL << (int)buttonId);
		}

		// Token: 0x06005EC3 RID: 24259 RVA: 0x002129B4 File Offset: 0x00210DB4
		public Vector2 GetAxis(EVRButtonId buttonId = EVRButtonId.k_EButton_Axis0)
		{
			this.Update();
			switch (buttonId)
			{
			case EVRButtonId.k_EButton_Axis0:
				return new Vector2(this.state.rAxis0.x, this.state.rAxis0.y);
			case EVRButtonId.k_EButton_Axis1:
				return new Vector2(this.state.rAxis1.x, this.state.rAxis1.y);
			case EVRButtonId.k_EButton_Axis2:
				return new Vector2(this.state.rAxis2.x, this.state.rAxis2.y);
			case EVRButtonId.k_EButton_Axis3:
				return new Vector2(this.state.rAxis3.x, this.state.rAxis3.y);
			case EVRButtonId.k_EButton_Axis4:
				return new Vector2(this.state.rAxis4.x, this.state.rAxis4.y);
			default:
				return Vector2.zero;
			}
		}

		// Token: 0x06005EC4 RID: 24260 RVA: 0x00212AB0 File Offset: 0x00210EB0
		public void TriggerHapticPulse(ushort durationMicroSec = 500, EVRButtonId buttonId = EVRButtonId.k_EButton_Axis0)
		{
			CVRSystem system = OpenVR.System;
			if (system != null)
			{
				uint unAxisId = (uint)(buttonId - EVRButtonId.k_EButton_Axis0);
				system.TriggerHapticPulse(this.index, unAxisId, (char)durationMicroSec);
			}
		}

		// Token: 0x06005EC5 RID: 24261 RVA: 0x00212ADC File Offset: 0x00210EDC
		private void UpdateHairTrigger()
		{
			this.hairTriggerPrevState = this.hairTriggerState;
			float x = this.state.rAxis1.x;
			if (this.hairTriggerState)
			{
				if (x < this.hairTriggerLimit - this.hairTriggerDelta || x <= 0f)
				{
					this.hairTriggerState = false;
				}
			}
			else if (x > this.hairTriggerLimit + this.hairTriggerDelta || x >= 1f)
			{
				this.hairTriggerState = true;
			}
			this.hairTriggerLimit = ((!this.hairTriggerState) ? Mathf.Min(this.hairTriggerLimit, x) : Mathf.Max(this.hairTriggerLimit, x));
		}

		// Token: 0x06005EC6 RID: 24262 RVA: 0x00212B8E File Offset: 0x00210F8E
		public bool GetHairTrigger()
		{
			this.Update();
			return this.hairTriggerState;
		}

		// Token: 0x06005EC7 RID: 24263 RVA: 0x00212B9C File Offset: 0x00210F9C
		public bool GetHairTriggerDown()
		{
			this.Update();
			return this.hairTriggerState && !this.hairTriggerPrevState;
		}

		// Token: 0x06005EC8 RID: 24264 RVA: 0x00212BBB File Offset: 0x00210FBB
		public bool GetHairTriggerUp()
		{
			this.Update();
			return !this.hairTriggerState && this.hairTriggerPrevState;
		}

		// Token: 0x0400445C RID: 17500
		private VRControllerState_t state;

		// Token: 0x0400445D RID: 17501
		private VRControllerState_t prevState;

		// Token: 0x0400445E RID: 17502
		private TrackedDevicePose_t pose;

		// Token: 0x0400445F RID: 17503
		private int prevFrameCount = -1;

		// Token: 0x04004460 RID: 17504
		public float hairTriggerDelta = 0.1f;

		// Token: 0x04004461 RID: 17505
		private float hairTriggerLimit;

		// Token: 0x04004462 RID: 17506
		private bool hairTriggerState;

		// Token: 0x04004463 RID: 17507
		private bool hairTriggerPrevState;
	}

	// Token: 0x02000BEE RID: 3054
	public enum DeviceRelation
	{
		// Token: 0x04004465 RID: 17509
		First,
		// Token: 0x04004466 RID: 17510
		Leftmost,
		// Token: 0x04004467 RID: 17511
		Rightmost,
		// Token: 0x04004468 RID: 17512
		FarthestLeft,
		// Token: 0x04004469 RID: 17513
		FarthestRight
	}
}
