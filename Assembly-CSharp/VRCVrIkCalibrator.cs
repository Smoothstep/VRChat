using System;
using RootMotion.FinalIK;
using UnityEngine;

// Token: 0x02000A4E RID: 2638
public class VRCVrIkCalibrator : MonoBehaviour
{
	// Token: 0x06004FAD RID: 20397 RVA: 0x001B0BDC File Offset: 0x001AEFDC
	public void Initialize(Animator anim, float avEyeHt)
	{
		this._baseEyeHt = avEyeHt;
		this._anim = anim;
		this._vrik = this._anim.GetComponent<VRIK>();
		this._anim.GetComponent<global::LimbIK>().enabled = false;
		RuntimeAnimatorController runtimeAnimatorController = this._anim.runtimeAnimatorController;
		this._anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
		this._anim.Update(0f);
		this.Head.position = this._anim.GetBoneTransform(HumanBodyBones.Head).position;
		this.Head.rotation = this._anim.GetBoneTransform(HumanBodyBones.Head).rotation;
		this.LeftHand.position = this._anim.GetBoneTransform(HumanBodyBones.LeftHand).position;
		this.LeftHand.rotation = this._anim.GetBoneTransform(HumanBodyBones.LeftHand).rotation;
		this.RightHand.position = this._anim.GetBoneTransform(HumanBodyBones.RightHand).position;
		this.RightHand.rotation = this._anim.GetBoneTransform(HumanBodyBones.RightHand).rotation;
		this._baseHeadHt = this.Head.localPosition.y;
		this._vrik.solver.spine.positionWeight = 1f;
		this._vrik.solver.spine.rotationWeight = 1f;
		this._vrik.solver.spine.pelvisPositionWeight = 0f;
		this._vrik.solver.spine.bodyPosStiffness = 0.55f;
		this._vrik.solver.spine.bodyRotStiffness = 0.1f;
		this._vrik.solver.spine.chestGoalWeight = 0.3f;
		this._vrik.solver.spine.chestClampWeight = 0.5f;
		this._vrik.solver.spine.headClampWeight = 0.6f;
		this._vrik.solver.spine.maintainPelvisPosition = 0.1f;
		this._vrik.solver.spine.maxRootAngle = 25f;
		this._vrik.solver.leftArm.positionWeight = 0f;
		this._vrik.solver.leftArm.rotationWeight = 0f;
		this._vrik.solver.leftArm.shoulderRotationMode = IKSolverVR.Arm.ShoulderRotationMode.FromTo;
		this._vrik.solver.leftArm.shoulderRotationWeight = 0.9f;
		this._vrik.solver.leftArm.bendGoalWeight = 0.8f;
		this._vrik.solver.leftArm.swivelOffset = 0f;
		this._vrik.solver.leftArm.wristToPalmAxis = new Vector3(-1f, 0f, 0f);
		this._vrik.solver.leftArm.palmToThumbAxis = new Vector3(0f, 0f, 1f);
		this._vrik.solver.rightArm.positionWeight = 0f;
		this._vrik.solver.rightArm.rotationWeight = 0f;
		this._vrik.solver.rightArm.shoulderRotationMode = IKSolverVR.Arm.ShoulderRotationMode.FromTo;
		this._vrik.solver.rightArm.shoulderRotationWeight = 0.9f;
		this._vrik.solver.rightArm.bendGoalWeight = 0.8f;
		this._vrik.solver.rightArm.swivelOffset = 0f;
		this._vrik.solver.rightArm.wristToPalmAxis = new Vector3(1f, 0f, 0f);
		this._vrik.solver.rightArm.palmToThumbAxis = new Vector3(0f, 0f, 1f);
		this._vrik.solver.locomotion.weight = 0f;
		this._anim.runtimeAnimatorController = this.ikPoseController;
		this._anim.Update(0f);
		this._vrik.AutoDetectReferences();
		this.CheckReferences();
		this._anim.runtimeAnimatorController = runtimeAnimatorController;
		this._anim.Update(0.001f);
		this._vrik.solver.spine.headTarget = this.Head;
		this._vrik.solver.leftArm.target = this.LeftHand;
		this._vrik.solver.rightArm.target = this.RightHand;
	}

	// Token: 0x06004FAE RID: 20398 RVA: 0x001B109C File Offset: 0x001AF49C
	private void CheckReferences()
	{
		Transform boneTransform = this._anim.GetBoneTransform(HumanBodyBones.LeftShoulder);
		Transform boneTransform2 = this._anim.GetBoneTransform(HumanBodyBones.RightShoulder);
		Transform parent = boneTransform.parent;
		if (parent == null || boneTransform2.parent != parent)
		{
			return;
		}
		if (this._vrik.references.chest != parent)
		{
			this._vrik.references.chest = parent;
			this._vrik.references.spine = parent.parent;
		}
	}

	// Token: 0x06004FAF RID: 20399 RVA: 0x001B112C File Offset: 0x001AF52C
	public void SetEyeHeight(float eyeHt)
	{
		float y = this._baseHeadHt - this._baseEyeHt + eyeHt;
		Vector3 localPosition = this.Head.localPosition;
		localPosition.y = y;
		this.Head.localPosition = localPosition;
	}

	// Token: 0x04003850 RID: 14416
	public RuntimeAnimatorController ikPoseController;

	// Token: 0x04003851 RID: 14417
	public Transform Head;

	// Token: 0x04003852 RID: 14418
	public Transform LeftHand;

	// Token: 0x04003853 RID: 14419
	public Transform RightHand;

	// Token: 0x04003854 RID: 14420
	private Animator _anim;

	// Token: 0x04003855 RID: 14421
	private VRIK _vrik;

	// Token: 0x04003856 RID: 14422
	private float _baseEyeHt;

	// Token: 0x04003857 RID: 14423
	private float _baseHeadHt;
}
