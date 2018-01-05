using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000A9E RID: 2718
public class LocomotionInputController : InputStateController
{
	// Token: 0x060051B3 RID: 20915 RVA: 0x001BF110 File Offset: 0x001BD510
	protected virtual void Awake()
	{
		this.motionState = base.GetComponent<VRCMotionState>();
		this.headCamMouseRotator = VRCVrCamera.GetInstance().GetComponentInChildren<NeckMouseRotator>();
		this.inAxisHorizontal = VRCInputManager.FindInput("Horizontal");
		this.inAxisVertical = VRCInputManager.FindInput("Vertical");
		this.inComfortLeft = VRCInputManager.FindInput("ComfortLeft");
		this.inComfortRight = VRCInputManager.FindInput("ComfortRight");
		this.inAxisLookHorizontal = VRCInputManager.FindInput("LookHorizontal");
		this.inAxisLookVertical = VRCInputManager.FindInput("LookVertical");
		this.inDrop = VRCInputManager.FindInput("DropRight");
		this.inRun = VRCInputManager.FindInput("Run");
		InputStateController.lastPosition = VRCTrackingManager.GetWorldTrackingPosition();
		InputStateController.lastRotation = VRCTrackingManager.GetWorldTrackingOrientation();
	}

	// Token: 0x060051B4 RID: 20916 RVA: 0x001BF1D0 File Offset: 0x001BD5D0
	protected virtual void FixedUpdate()
	{
		this.UpdateIntraFrameMotion();
		this.UpdateTrackingMotion();
		if (this.immobilize)
		{
			this.motionState.SetLocomotion(Vector3.zero, Time.deltaTime);
			this.motionState.IsImmobilized = true;
		}
		else
		{
			this.motionState.IsImmobilized = false;
		}
		InputStateController.lastPosition = base.transform.position;
		InputStateController.lastRotation = base.transform.rotation;
		this.IsFirstPhysicsStepThisFrame = false;
	}

	// Token: 0x060051B5 RID: 20917 RVA: 0x001BF24D File Offset: 0x001BD64D
	protected virtual void Update()
	{
		this.IsFirstPhysicsStepThisFrame = true;
	}

	// Token: 0x060051B6 RID: 20918 RVA: 0x001BF258 File Offset: 0x001BD658
	protected virtual void UpdateIntraFrameMotion()
	{
		Vector3 playerWorldMotion = base.transform.position - InputStateController.lastPosition;
		Quaternion playerWorldRotation = Quaternion.Inverse(InputStateController.lastRotation) * base.transform.rotation;
		VRCTrackingManager.ApplyPlayerMotion(playerWorldMotion, playerWorldRotation);
	}

	// Token: 0x060051B7 RID: 20919 RVA: 0x001BF2A0 File Offset: 0x001BD6A0
	protected virtual void UpdateTrackingMotion()
	{
		Vector3 worldTrackingMotion = VRCTrackingManager.GetWorldTrackingMotion();
		Vector3 position = base.transform.position;
		this.motionState.AddMotionOffset(worldTrackingMotion);
		Vector3 position2 = base.transform.position;
		Vector3 target = position2 - position;
		target.y = 0f;
		worldTrackingMotion.y = 0f;
		bool flag = target.AlmostEquals(worldTrackingMotion, 0.01f);
		VRCTrackingManager.SetPlayerNearTracking(flag);
		if (flag)
		{
			Quaternion worldTrackingRotation = VRCTrackingManager.GetWorldTrackingRotation();
			base.transform.rotation = base.transform.rotation * worldTrackingRotation;
		}
	}

	// Token: 0x060051B8 RID: 20920 RVA: 0x001BF33C File Offset: 0x001BD73C
	protected virtual float GetHeightBasedMotionScale()
	{
		float result;
		if (this.motionState.StandingHeight > LocomotionInputController.CrouchSpeedStart)
		{
			result = 1f;
		}
		else if (this.motionState.StandingHeight >= LocomotionInputController.ProneSpeedStart)
		{
			result = 0.5f;
		}
		else
		{
			result = 0.1f;
		}
		return result;
	}

	// Token: 0x060051B9 RID: 20921 RVA: 0x001BF390 File Offset: 0x001BD790
	protected virtual Vector3 CalculateRotation(float dt)
	{
		if (VRCInputManager.comfortTurning)
		{
			float num = this.inAxisLookHorizontal.axis;
			if (this.inComfortLeft.button)
			{
				num = -1f;
			}
			if (this.inComfortRight.button)
			{
				num = 1f;
			}
			float num2 = Mathf.Abs(num);
			if (num2 <= 0.7f || this.ComfortRotationInputActive)
			{
				if (num2 < 0.3f)
				{
					this.ComfortRotationInputActive = false;
				}
				return Vector3.zero;
			}
			this.ComfortRotationInputActive = true;
			if (num < 0f)
			{
				return new Vector3(0f, -1f * this.ComfortRotation, 0f);
			}
			return new Vector3(0f, 1f * this.ComfortRotation, 0f);
		}
		else
		{
			if (this.IsFirstPhysicsStepThisFrame)
			{
				float num3 = 0f;
				if (Cursor.lockState == CursorLockMode.Locked || QuickMenu.Instance.IsActive || VRCApplicationSetup.IsEditor())
				{
					num3 = Input.GetAxis("Mouse X") * this.mouseRotationSpeed;
				}
				num3 += this.inAxisLookHorizontal.axis * this.controllerRotationSpeed;
				return new Vector3(0f, num3, 0f);
			}
			return Vector3.zero;
		}
	}

	// Token: 0x060051BA RID: 20922 RVA: 0x001BF4D0 File Offset: 0x001BD8D0
	public virtual Vector3 CalculateMoveVelocity(bool obeyHeadlookSetting = true)
	{
		Vector2 zero = Vector2.zero;
		float axis = this.inAxisHorizontal.axis;
		float y = this.inAxisVertical.axis;
		if (VRCInputManager.comfortTurning && this.inComfortLeft.button && this.inComfortRight.button)
		{
			y = 1f;
		}
		zero = new Vector2(axis, y);
		float d = this.runSpeed;
		bool button = this.inRun.button;
		d = ((!this.walkByDefault) ? ((!button) ? this.runSpeed : this.walkSpeed) : ((!button) ? this.walkSpeed : this.runSpeed));
        //d *= 10.0f;
        if (zero.sqrMagnitude > 1f)
		{
			zero.Normalize();
		}
		Quaternion rotation = VRCTrackingManager.GetMotionOrientation();
		if (!obeyHeadlookSetting)
		{
			rotation = base.transform.rotation;
		}
		Vector3 a = rotation * Vector3.forward;
		Vector3 a2 = rotation * Vector3.right;
		return a * zero.y * d + a2 * zero.x * this.strafeSpeed;
	}

	// Token: 0x060051BB RID: 20923 RVA: 0x001BF610 File Offset: 0x001BDA10
	public override void OnActivate()
	{
		VRCTrackingManager.ImmobilizePlayer(false);
		if (this.headCamMouseRotator != null)
		{
			float trackingScale = VRCTrackingManager.GetTrackingScale();
			this.headCamMouseRotator.neckLookUpDist = -0.1f * trackingScale;
			this.headCamMouseRotator.neckLookDownDist = 0.15f * trackingScale;
			this.headCamMouseRotator.rotationRange.x = -70f;
			this.headCamMouseRotator.rotationRange.z = 80f;
			this.headCamMouseRotator.rotationRange.y = 0f;
		}
	}

	// Token: 0x060051BC RID: 20924 RVA: 0x001BF6A0 File Offset: 0x001BDAA0
	public virtual void Teleport(Vector3 teleportPos, Quaternion teleportRot, VRC_SceneDescriptor.SpawnOrientation teleportOrientation = VRC_SceneDescriptor.SpawnOrientation.Default)
	{
		Vector3 eulerAngles = teleportRot.eulerAngles;
		eulerAngles.x = (eulerAngles.z = 0f);
		IEnumerable<SyncPhysics> componentsInChildren = base.GetComponentsInChildren<SyncPhysics>();
		foreach (SyncPhysics syncPhysics in componentsInChildren)
		{
			syncPhysics.TeleportTo(teleportPos, Quaternion.Euler(eulerAngles));
		}
		if (componentsInChildren.FirstOrDefault<SyncPhysics>() == null)
		{
			base.transform.position = teleportPos;
			base.transform.rotation = Quaternion.Euler(eulerAngles);
		}
		SpawnManager.Instance.AlignTrackingToPlayer(base.GetComponentInParent<VRCPlayer>());
	}

	// Token: 0x040039EF RID: 14831
	protected VRCInput inAxisHorizontal;

	// Token: 0x040039F0 RID: 14832
	protected VRCInput inAxisVertical;

	// Token: 0x040039F1 RID: 14833
	protected VRCInput inComfortLeft;

	// Token: 0x040039F2 RID: 14834
	protected VRCInput inComfortRight;

	// Token: 0x040039F3 RID: 14835
	protected VRCInput inAxisLookHorizontal;

	// Token: 0x040039F4 RID: 14836
	protected VRCInput inAxisLookVertical;

	// Token: 0x040039F5 RID: 14837
	protected VRCInput inRun;

	// Token: 0x040039F6 RID: 14838
	protected VRCInput inDrop;

	// Token: 0x040039F7 RID: 14839
	public float runSpeed = 4f;

	// Token: 0x040039F8 RID: 14840
	public float strafeSpeed = 2f;

	// Token: 0x040039F9 RID: 14841
	public bool walkByDefault = true;

	// Token: 0x040039FA RID: 14842
	public float walkSpeed = 2f;

	// Token: 0x040039FB RID: 14843
	public static Vector3 navigationCursorPosition = Vector3.zero;

	// Token: 0x040039FC RID: 14844
	public static bool navigationCursorActive = false;

	// Token: 0x040039FD RID: 14845
	protected VRCMotionState motionState;

	// Token: 0x040039FE RID: 14846
	protected NeckMouseRotator headCamMouseRotator;

	// Token: 0x040039FF RID: 14847
	public float mouseRotationSpeed = 10f;

	// Token: 0x04003A00 RID: 14848
	public float controllerRotationSpeed = 2f;

	// Token: 0x04003A01 RID: 14849
	public float ComfortRotation = 30f;

	// Token: 0x04003A02 RID: 14850
	protected bool ComfortRotationInputActive;

	// Token: 0x04003A03 RID: 14851
	public bool immobilize;

	// Token: 0x04003A04 RID: 14852
	protected bool IsFirstPhysicsStepThisFrame = true;

	// Token: 0x04003A05 RID: 14853
	public static float CrouchSpeedStart = 0.7f;

	// Token: 0x04003A06 RID: 14854
	private const float CrouchSpeedScale = 0.5f;

	// Token: 0x04003A07 RID: 14855
	public static float ProneSpeedStart = 0.3f;

	// Token: 0x04003A08 RID: 14856
	private const float ProneSpeedScale = 0.1f;
}
