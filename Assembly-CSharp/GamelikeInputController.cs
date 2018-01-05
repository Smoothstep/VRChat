using System;
using UnityEngine;

// Token: 0x02000A9B RID: 2715
public class GamelikeInputController : LocomotionInputController
{
	// Token: 0x060051A1 RID: 20897 RVA: 0x001BFA50 File Offset: 0x001BDE50
	protected override void FixedUpdate()
	{
		this.UpdateIntraFrameMotion();
		this.UpdateTrackingMotion();
		InputStateController.lastPosition = base.transform.position;
		InputStateController.lastRotation = base.transform.rotation;
		base.transform.Rotate(this.CalculateRotation(Time.deltaTime));
		float heightBasedMotionScale = this.GetHeightBasedMotionScale();
		Vector3 a = this.CalculateMoveVelocity(true);
		if (a.sqrMagnitude > 0f)
		{
			this.motionState.isLocomoting = true;
		}
		else
		{
			this.motionState.isLocomoting = false;
		}
		if (this.immobilize)
		{
			this.motionState.SetLocomotion(Vector3.zero, Time.deltaTime);
			this.motionState.IsImmobilized = true;
		}
		else
		{
			this.motionState.SetLocomotion(a * heightBasedMotionScale, Time.deltaTime);
			this.motionState.IsImmobilized = false;
		}
		Vector3 playerWorldMotion = base.transform.position - InputStateController.lastPosition;
		playerWorldMotion.y = base.transform.position.y - VRCTrackingManager.GetTrackingWorldOrigin().y;
		VRCTrackingManager.ApplyPlayerMotion(playerWorldMotion, Quaternion.Inverse(InputStateController.lastRotation) * base.transform.rotation);
		InputStateController.lastPosition = base.transform.position;
		InputStateController.lastRotation = base.transform.rotation;
		this.IsFirstPhysicsStepThisFrame = false;
	}
}
