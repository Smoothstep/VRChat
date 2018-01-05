using System;
using UnityEngine;

// Token: 0x02000A9A RID: 2714
public class BlinkInputController : LocomotionInputController
{
	// Token: 0x0600519F RID: 20895 RVA: 0x001BF7A0 File Offset: 0x001BDBA0
	protected override void FixedUpdate()
	{
		bool flag = false;
		this.UpdateIntraFrameMotion();
		this.UpdateTrackingMotion();
		InputStateController.lastPosition = base.transform.position;
		InputStateController.lastRotation = base.transform.rotation;
		base.transform.Rotate(this.CalculateRotation(Time.deltaTime));
		Vector2 vector = new Vector2(this.inAxisHorizontal.axis, this.inAxisVertical.axis);
		if (vector.sqrMagnitude > 0.1f)
		{
			flag = true;
			this.motionState.isLocomoting = true;
		}
		if (this.immobilize)
		{
			this.motionState.IsImmobilized = true;
			this.motionState.SetLocomotion(Vector3.zero, Time.deltaTime);
		}
		else
		{
			this.motionState.IsImmobilized = false;
			bool flag2 = false;
			this.timeSinceBlink += Time.deltaTime;
			if (this.timeSinceBlink >= this.blinkPeriod && this.motionState.isGrounded && flag)
			{
				bool isGrounded = this.motionState.isGrounded;
				this.timeSinceBlink = 0f;
				Vector3 a = this.CalculateMoveVelocity(true);
				float num = this.blinkPeriod / Time.deltaTime;
				for (float num2 = 0f; num2 < num; num2 += 10f)
				{
					this.motionState.SetLocomotion(a * ((num - num2 <= 10f) ? (num - num2) : 10f), Time.deltaTime / (num / 10f));
				}
				this.motionState.SetLocomotion(a * num, Time.deltaTime);
				if (isGrounded && !this.motionState.isGrounded)
				{
					this.motionState.AddMotionOffset(Vector3.up * -1f);
				}
				this.motionState.Stop();
				flag2 = true;
			}
			if (!flag2)
			{
				Vector3 velo = Vector3.zero;
				if (!this.motionState.isGrounded)
				{
					velo = this.CalculateMoveVelocity(true);
				}
				this.motionState.SetLocomotion(velo, Time.deltaTime);
			}
		}
		Vector3 playerWorldMotion = base.transform.position - InputStateController.lastPosition;
		playerWorldMotion.y = base.transform.position.y - VRCTrackingManager.GetTrackingWorldOrigin().y;
		VRCTrackingManager.ApplyPlayerMotion(playerWorldMotion, Quaternion.Inverse(InputStateController.lastRotation) * base.transform.rotation);
		InputStateController.lastPosition = base.transform.position;
		InputStateController.lastRotation = base.transform.rotation;
		this.IsFirstPhysicsStepThisFrame = false;
	}

	// Token: 0x040039E9 RID: 14825
	private float blinkPeriod = 0.75f;

	// Token: 0x040039EA RID: 14826
	private float timeSinceBlink;
}
