using System;
using UnityEngine;

// Token: 0x02000A42 RID: 2626
public class LimbIK : MonoBehaviour
{
	// Token: 0x06004F36 RID: 20278 RVA: 0x001ABEB8 File Offset: 0x001AA2B8
	private void OnAnimatorIK(int LayerIndex)
	{
		if (this.ModelAnimator == null)
		{
			this.ModelAnimator = base.GetComponent<Animator>();
		}
		if (this.ModelAnimator == null)
		{
			return;
		}
		if (this.LeftHandTarget == null)
		{
			return;
		}
		this.ModelAnimator.SetIKPosition(AvatarIKGoal.LeftHand, this.LeftHandTarget.position);
		this.ModelAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, this.LeftPosWeight);
		this.ModelAnimator.SetIKRotation(AvatarIKGoal.LeftHand, this.LeftHandTarget.rotation);
		this.ModelAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, this.LeftRotWeight);
		if (this.RightHandTarget == null)
		{
			return;
		}
		this.ModelAnimator.SetIKPosition(AvatarIKGoal.RightHand, this.RightHandTarget.position);
		this.ModelAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, this.RightPosWeight);
		this.ModelAnimator.SetIKRotation(AvatarIKGoal.RightHand, this.RightHandTarget.rotation);
		this.ModelAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, this.RightRotWeight);
	}

	// Token: 0x06004F37 RID: 20279 RVA: 0x001ABFBC File Offset: 0x001AA3BC
	private void LateUpdate()
	{
		if (this.HeadTarget != null && this.ModelAnimator != null && this.ModelAnimator.isHuman)
		{
			Transform boneTransform = this.ModelAnimator.GetBoneTransform(HumanBodyBones.Head);
			boneTransform.rotation = Quaternion.Slerp(boneTransform.rotation, this.HeadTarget.rotation, this.HeadRotWeight);
		}
	}

	// Token: 0x06004F38 RID: 20280 RVA: 0x001AC02B File Offset: 0x001AA42B
	private void Start()
	{
	}

	// Token: 0x06004F39 RID: 20281 RVA: 0x001AC02D File Offset: 0x001AA42D
	private void Update()
	{
	}

	// Token: 0x040037A6 RID: 14246
	public float LeftPosWeight;

	// Token: 0x040037A7 RID: 14247
	public float LeftRotWeight;

	// Token: 0x040037A8 RID: 14248
	public float RightPosWeight;

	// Token: 0x040037A9 RID: 14249
	public float RightRotWeight;

	// Token: 0x040037AA RID: 14250
	public float HeadPosWeight;

	// Token: 0x040037AB RID: 14251
	public float HeadRotWeight;

	// Token: 0x040037AC RID: 14252
	public Transform LeftHandTarget;

	// Token: 0x040037AD RID: 14253
	public Transform RightHandTarget;

	// Token: 0x040037AE RID: 14254
	public Transform HeadTarget;

	// Token: 0x040037AF RID: 14255
	private Animator ModelAnimator;
}
