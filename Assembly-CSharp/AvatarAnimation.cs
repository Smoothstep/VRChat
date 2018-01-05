using System;
using UnityEngine;

// Token: 0x02000A2E RID: 2606
public class AvatarAnimation : MonoBehaviour
{
	// Token: 0x06004E81 RID: 20097 RVA: 0x001A4DAB File Offset: 0x001A31AB
	private void Start()
	{
		if (this.CurrentAnimator == null)
		{
			return;
		}
		this.CurrentAnimator.updateMode = AnimatorUpdateMode.Normal;
		this.CurrentAnimator.stabilizeFeet = true;
		this.CurrentAnimator.feetPivotActive = 0.25f;
	}

	// Token: 0x06004E82 RID: 20098 RVA: 0x001A4DE8 File Offset: 0x001A31E8
	private void Update()
	{
		if (this.CurrentAnimator == null)
		{
			return;
		}
		if (this.CurrentAnimator.runtimeAnimatorController == null)
		{
			return;
		}
		if (this.hasMovementX || this.hasMovementZ)
		{
			Vector3 walkingVelocity = this.motionState.walkingVelocity;
			if (this.hasMovementX)
			{
				float a = Mathf.Clamp(this.CurrentAnimator.GetFloat("MovementX"), -4f, 4f);
				float value = Mathf.Lerp(a, walkingVelocity.x, 5f * Time.deltaTime);
				this.CurrentAnimator.SetFloat("MovementX", value);
			}
			if (this.hasMovementZ)
			{
				float a2 = Mathf.Clamp(this.CurrentAnimator.GetFloat("MovementZ"), -4f, 4f);
				float value2 = Mathf.Lerp(a2, walkingVelocity.z, 5f * Time.deltaTime);
				this.CurrentAnimator.SetFloat("MovementZ", value2);
			}
		}
		if (this.hasGrounded)
		{
			this.CurrentAnimator.SetBool("Grounded", this.motionState.isGrounded);
		}
		if (!this.CurrentAnimator.isHuman && this.CurrentAnimator.layerCount >= 2)
		{
			if (this.motionState.walkingVelocity.sqrMagnitude > 0.01f)
			{
				this.locomotionWeight = Mathf.Lerp(this.locomotionWeight, 1f, 2f * Time.deltaTime);
			}
			else
			{
				this.locomotionWeight = Mathf.Lerp(this.locomotionWeight, 0f, 3f * Time.deltaTime);
			}
			this.CurrentAnimator.SetLayerWeight(1, this.locomotionWeight);
		}
	}

	// Token: 0x06004E83 RID: 20099 RVA: 0x001A4FAC File Offset: 0x001A33AC
	private bool HasParameter(string name, AnimatorControllerParameterType type)
	{
		foreach (AnimatorControllerParameter animatorControllerParameter in this.CurrentParameters)
		{
			if (animatorControllerParameter.type == type && animatorControllerParameter.name == name)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004E84 RID: 20100 RVA: 0x001A4FF8 File Offset: 0x001A33F8
	public void Initialize(Animator a, bool isLocal)
	{
		this.CurrentAnimator = a;
		this.CurrentParameters = a.parameters;
		if (this.HasParameter("MovementX", AnimatorControllerParameterType.Float))
		{
			this.hasMovementX = true;
		}
		if (this.HasParameter("MovementZ", AnimatorControllerParameterType.Float))
		{
			this.hasMovementZ = true;
		}
		if (this.HasParameter("Grounded", AnimatorControllerParameterType.Bool))
		{
			this.hasGrounded = true;
		}
	}

	// Token: 0x06004E85 RID: 20101 RVA: 0x001A5060 File Offset: 0x001A3460
	public void RunAnimationState(string state)
	{
		if (this.CurrentAnimator != null)
		{
			this.CurrentAnimator.Play(state);
		}
	}

	// Token: 0x04003699 RID: 13977
	public VRCMotionState motionState;

	// Token: 0x0400369A RID: 13978
	private Animator CurrentAnimator;

	// Token: 0x0400369B RID: 13979
	private AnimatorControllerParameter[] CurrentParameters;

	// Token: 0x0400369C RID: 13980
	private bool hasMovementX;

	// Token: 0x0400369D RID: 13981
	private bool hasMovementZ;

	// Token: 0x0400369E RID: 13982
	private bool hasGrounded;

	// Token: 0x0400369F RID: 13983
	private float locomotionWeight;

	// Token: 0x040036A0 RID: 13984
	private const float MAX_SPEED = 4f;
}
