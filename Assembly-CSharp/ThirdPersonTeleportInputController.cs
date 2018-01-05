using System;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000A9F RID: 2719
public class ThirdPersonTeleportInputController : LocomotionInputController
{
	// Token: 0x060051BF RID: 20927 RVA: 0x001C002F File Offset: 0x001BE42F
	public void ForceTrackingLink()
	{
		this.linked = true;
	}

	// Token: 0x060051C0 RID: 20928 RVA: 0x001C0038 File Offset: 0x001BE438
	protected override void FixedUpdate()
	{
		bool flag = this.linked;
		if (this.linked)
		{
			this.UpdateIntraFrameMotion();
			this.UpdateTrackingMotion();
			InputStateController.lastPosition = base.transform.position;
			InputStateController.lastRotation = base.transform.rotation;
			this.speed = 0f;
		}
		Vector3 zero = Vector3.zero;
		Quaternion identity = Quaternion.identity;
		this.linked = true;
		if (this.immobilize)
		{
			this.motionState.IsImmobilized = true;
		}
		else
		{
			this.motionState.IsImmobilized = false;
			bool flag2 = false;
			if (this.inAxisVertical.axis > 0.5f && this.inAxisVertical.axis > Mathf.Abs(this.inAxisHorizontal.axis) && (VRCInputManager.AnyKey(VRCInputManager.InputMethod.Hydra) || VRCInputManager.AnyKey(VRCInputManager.InputMethod.Vive) || VRCInputManager.AnyKey(VRCInputManager.InputMethod.Oculus)))
			{
				flag2 = true;
			}
			if (LocomotionInputController.navigationCursorActive || flag2)
			{
				this.UpdateLocomotionFromCursor(out zero, out identity);
			}
			else
			{
				this.UpdateLocomotionFromController(out zero, out identity);
			}
		}
		this.motionState.SetLocomotion(zero, Time.deltaTime);
		if (zero.sqrMagnitude > 0f)
		{
			this.motionState.isLocomoting = true;
		}
		else
		{
			this.motionState.isLocomoting = false;
		}
		if (VRCTrackingManager.IsPlayerNearTracking() && !this.linked)
		{
			VRCTrackingManager.SetPlayerNearTracking(false);
		}
		if (this.linked)
		{
			base.transform.Rotate(this.CalculateRotation(Time.deltaTime));
			Vector3 playerWorldMotion = base.transform.position - InputStateController.lastPosition;
			playerWorldMotion.y = base.transform.position.y - VRCTrackingManager.GetTrackingWorldOrigin().y;
			Quaternion playerWorldRotation = Quaternion.Inverse(InputStateController.lastRotation) * base.transform.rotation;
			if (this.linked && !flag && !VRCInputManager.thirdPersonRotation)
			{
				playerWorldRotation = Quaternion.identity;
			}
			VRCTrackingManager.ApplyPlayerMotion(playerWorldMotion, playerWorldRotation);
			InputStateController.lastPosition = base.transform.position;
			InputStateController.lastRotation = base.transform.rotation;
		}
		else
		{
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, identity, 10f * Time.deltaTime);
			Vector3 euler = this.CalculateRotation(Time.deltaTime);
			VRCTrackingManager.ApplyPlayerMotion(Vector3.zero, Quaternion.Euler(euler));
		}
		this.IsFirstPhysicsStepThisFrame = false;
	}

	// Token: 0x060051C1 RID: 20929 RVA: 0x001C02C0 File Offset: 0x001BE6C0
	private void UpdateLocomotionFromCursor(out Vector3 desiredMove, out Quaternion desiredRotate)
	{
		desiredMove = Vector3.zero;
		desiredRotate = base.transform.rotation;
		Vector3 vector = Vector3.zero;
		float heightBasedMotionScale = this.GetHeightBasedMotionScale();
		float num = this.runSpeed * heightBasedMotionScale;
		if (this.inAxisVertical.axis != 0f)
		{
			vector = LocomotionInputController.navigationCursorPosition - base.transform.position;
			Vector3 normalized = vector.normalized;
			vector.y = 0f;
			normalized.y = 0f;
			float magnitude = vector.magnitude;
			LocomotionInputController.navigationCursorActive = true;
			if (magnitude > 0.01f)
			{
				num = Mathf.Lerp(0f, num, Mathf.Clamp01(magnitude * this.speedDistanceScale));
				this.speed = Mathf.MoveTowards(this.speed, num, this.acceleration * Time.deltaTime);
				desiredMove = vector.normalized * this.speed;
			}
			if (magnitude > 0.2f)
			{
				desiredRotate = Quaternion.LookRotation(normalized, Vector3.up);
			}
			this.linked = false;
		}
		else if (this.inAxisHorizontal.axis != 0f)
		{
			this.linked = false;
			desiredMove = Vector3.zero;
			desiredRotate = base.transform.rotation;
		}
		else
		{
			this.linked = true;
			LocomotionInputController.navigationCursorActive = false;
			desiredMove = Vector3.zero;
			desiredRotate = base.transform.rotation;
		}
	}

	// Token: 0x060051C2 RID: 20930 RVA: 0x001C0444 File Offset: 0x001BE844
	private void UpdateLocomotionFromController(out Vector3 desiredMove, out Quaternion desiredRotate)
	{
		float heightBasedMotionScale = this.GetHeightBasedMotionScale();
		float d = this.runSpeed * heightBasedMotionScale;
		float num = this.inAxisVertical.axis;
		float num2 = this.inAxisHorizontal.axis;
		LocomotionInputController.navigationCursorActive = false;
		if (Mathf.Abs(num) < 0.1f)
		{
			num = 0f;
		}
		if (Mathf.Abs(num2) < 0.1f)
		{
			num2 = 0f;
		}
		if (num != 0f || num2 != 0f)
		{
			Transform trackedTransform = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.Hmd);
			Vector3 normalized = Vector3.ProjectOnPlane(trackedTransform.forward, Vector3.up).normalized;
			Vector3 normalized2 = Vector3.ProjectOnPlane(trackedTransform.right, Vector3.up).normalized;
			desiredMove = normalized * d * num + normalized2 * d * num2;
			desiredRotate = Quaternion.LookRotation(desiredMove, Vector3.up);
			this.linked = false;
		}
		else
		{
			this.linked = true;
			desiredMove = Vector3.zero;
			desiredRotate = base.transform.rotation;
		}
	}

	// Token: 0x060051C3 RID: 20931 RVA: 0x001C056D File Offset: 0x001BE96D
	public override void Teleport(Vector3 teleportPos, Quaternion teleportRot, VRC_SceneDescriptor.SpawnOrientation teleportOrientation = VRC_SceneDescriptor.SpawnOrientation.Default)
	{
		base.Teleport(teleportPos, teleportRot, teleportOrientation);
		this.ForceTrackingLink();
	}

	// Token: 0x04003A09 RID: 14857
	private bool linked = true;

	// Token: 0x04003A0A RID: 14858
	private float speed;

	// Token: 0x04003A0B RID: 14859
	public float acceleration = 7.5f;

	// Token: 0x04003A0C RID: 14860
	public float speedDistanceScale = 0.6f;
}
