using System;
using System.Collections;
using RootMotion;
using RootMotion.FinalIK;
using UnityEngine;

// Token: 0x02000A4D RID: 2637
public class VRCFbbIkController : MonoBehaviour, IVRCTrackedIk
{
	// Token: 0x17000BC8 RID: 3016
	// (get) Token: 0x06004F78 RID: 20344 RVA: 0x001ADCA9 File Offset: 0x001AC0A9
	// (set) Token: 0x06004F79 RID: 20345 RVA: 0x001ADCB4 File Offset: 0x001AC0B4
	public bool enableIk
	{
		get
		{
			return this._isIkEnabled;
		}
		set
		{
			this._isIkEnabled = value;
			if (this._fbbik != null)
			{
				this._fbbik.enabled = value;
			}
			if (this._fbbikHead != null)
			{
				this._fbbikHead.enabled = value;
			}
			if (this._forearmLTwistRelaxer != null)
			{
				this._forearmLTwistRelaxer.enabled = value;
			}
			if (this._forearmRTwistRelaxer != null)
			{
				this._forearmRTwistRelaxer.enabled = value;
			}
		}
	}

	// Token: 0x17000BC9 RID: 3017
	// (get) Token: 0x06004F7A RID: 20346 RVA: 0x001ADD3C File Offset: 0x001AC13C
	// (set) Token: 0x06004F7B RID: 20347 RVA: 0x001ADD44 File Offset: 0x001AC144
	public bool hasLowerBodyTracking
	{
		get
		{
			return this._enableLowerBodyTracking;
		}
		set
		{
			this._enableLowerBodyTracking = value;
			this.Reset(true);
			this.CheckPoseChange(true);
		}
	}

	// Token: 0x17000BCA RID: 3018
	// (get) Token: 0x06004F7C RID: 20348 RVA: 0x001ADD5B File Offset: 0x001AC15B
	public bool isCalibrated
	{
		get
		{
			return this._calibrated;
		}
	}

	// Token: 0x17000BCB RID: 3019
	// (get) Token: 0x06004F7D RID: 20349 RVA: 0x001ADD63 File Offset: 0x001AC163
	// (set) Token: 0x06004F7E RID: 20350 RVA: 0x001ADD6B File Offset: 0x001AC16B
	public bool isCulled
	{
		get
		{
			return this._culled;
		}
		set
		{
			this._culled = value;
		}
	}

	// Token: 0x06004F7F RID: 20351 RVA: 0x001ADD74 File Offset: 0x001AC174
	private void SyncLowerBodyTargetsWithSkeleton()
	{
		this._hipTarget.position = this._anim.GetBoneTransform(HumanBodyBones.Hips).position;
		this._hipTarget.rotation = this._anim.GetBoneTransform(HumanBodyBones.Hips).rotation;
		this._leftFootTarget.position = this._anim.GetBoneTransform(HumanBodyBones.LeftFoot).position;
		this._rightFootTarget.position = this._anim.GetBoneTransform(HumanBodyBones.RightFoot).position;
	}

	// Token: 0x06004F80 RID: 20352 RVA: 0x001ADDF4 File Offset: 0x001AC1F4
	private void CheckPoseChange(bool force = false)
	{
		if (this._fbbik == null)
		{
			return;
		}
		if (this._fbbik.solver == null)
		{
			return;
		}
		if (this._anim == null)
		{
			return;
		}
		if (this._anim.transform == null)
		{
			return;
		}
		if (this._motion == null)
		{
			return;
		}
		if (this._ik == null)
		{
			return;
		}
		Vector3 preIkHipPos = this.PreIkHipPos;
		bool isSeated = this._motion.IsSeated;
		if (force)
		{
			this._lastSeated = !isSeated;
		}
		bool flag = false;
		bool flag2 = false;
		if (isSeated)
		{
			if (!this._motion.InVehicle && (this._lastSeatedPos - preIkHipPos).sqrMagnitude > 1f)
			{
				flag = true;
			}
			if (this._seatedRAC != this._anim.runtimeAnimatorController)
			{
				flag2 = true;
			}
		}
		float num = 0.5f;
		float num2 = 0.5f;
		float num3 = num + num2;
		if (isSeated != this._lastSeated || flag || flag2 || (this._timeSeated > 0f && this._timeSeated < num3))
		{
			if (isSeated && !this._enableLowerBodyTracking)
			{
				if (this._timeSeated < num)
				{
					this._seatedRAC = this._anim.runtimeAnimatorController;
					this._anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
					this.enableIk = false;
					this.SolverWeight = 0f;
					this._anim.Update(0f);
					this.SyncLowerBodyTargetsWithSkeleton();
				}
				else
				{
					if (!this._local)
					{
						this._anim.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
					}
					this.SolverWeight = (this._timeSeated - num) / num2;
					this.enableIk = true;
					this._fbbik.solver.bodyEffector.target = this._hipTarget;
					this._fbbik.solver.leftFootEffector.target = this._leftFootTarget;
					this._fbbik.solver.rightFootEffector.target = this._rightFootTarget;
					this._fbbik.solver.leftLegChain.bendConstraint.bendGoal = null;
					this._fbbik.solver.rightLegChain.bendConstraint.bendGoal = null;
				}
				this._fbbik.solver.leftFootEffector.positionWeight = 0f;
				this._fbbik.solver.rightFootEffector.positionWeight = 0f;
				this._fbbik.solver.leftFootEffector.rotationWeight = 0f;
				this._fbbik.solver.rightFootEffector.rotationWeight = 0f;
				this._fbbik.solver.leftLegChain.bendConstraint.weight = 0f;
				this._fbbik.solver.rightLegChain.bendConstraint.weight = 0f;
				this.HeadPosWeight = 0f;
				this.HeadRotWeight = 0f;
				this._lastSeatedPos = preIkHipPos;
				if (this._local)
				{
					this._ik.HeadControl(false);
				}
			}
			else
			{
				this._timeSeated = 0f;
				this._seatedRAC = null;
				this._anim.transform.localPosition = Vector3.zero;
				this._anim.transform.localRotation = Quaternion.identity;
				this._anim.Update(0f);
				this._fbbik.solver.bodyEffector.target = this._hipTarget;
				this._fbbik.solver.leftFootEffector.target = this._leftFootTarget;
				this._fbbik.solver.rightFootEffector.target = this._rightFootTarget;
				this._fbbik.solver.leftLegChain.bendConstraint.bendGoal = this._leftKneeTarget;
				this._fbbik.solver.rightLegChain.bendConstraint.bendGoal = this._rightKneeTarget;
				this._fbbik.solver.leftFootEffector.positionWeight = 1f;
				this._fbbik.solver.rightFootEffector.positionWeight = 1f;
				this._fbbik.solver.leftFootEffector.rotationWeight = 0.5f;
				this._fbbik.solver.rightFootEffector.rotationWeight = 0.5f;
				this._fbbik.solver.leftLegChain.bendConstraint.weight = 0.75f;
				this._fbbik.solver.rightLegChain.bendConstraint.weight = 0.75f;
				this.HeadPosWeight = 1f;
				this.HeadRotWeight = 1f;
				if (this._local)
				{
					this._ik.HeadControl(true);
				}
			}
		}
		this._lastSeated = isSeated;
	}

	// Token: 0x06004F81 RID: 20353 RVA: 0x001AE2EB File Offset: 0x001AC6EB
	public void SeatedChange(bool sitting)
	{
		if (sitting)
		{
			this.SyncLowerBodyTargetsWithSkeleton();
		}
	}

	// Token: 0x17000BCC RID: 3020
	// (get) Token: 0x06004F82 RID: 20354 RVA: 0x001AE2F9 File Offset: 0x001AC6F9
	// (set) Token: 0x06004F83 RID: 20355 RVA: 0x001AE328 File Offset: 0x001AC728
	public float LeftHandWeight
	{
		get
		{
			if (!this._ready)
			{
				return 0f;
			}
			return this._fbbik.solver.leftArmMapping.weight / 1f;
		}
		set
		{
			if (!this._ready)
			{
				return;
			}
			float num = Mathf.Clamp01(value);
			this._fbbik.solver.leftArmMapping.weight = num * 1f;
		}
	}

	// Token: 0x17000BCD RID: 3021
	// (get) Token: 0x06004F84 RID: 20356 RVA: 0x001AE364 File Offset: 0x001AC764
	// (set) Token: 0x06004F85 RID: 20357 RVA: 0x001AE394 File Offset: 0x001AC794
	public float RightHandWeight
	{
		get
		{
			if (!this._ready)
			{
				return 0f;
			}
			return this._fbbik.solver.rightArmMapping.weight / 1f;
		}
		set
		{
			if (!this._ready)
			{
				return;
			}
			float num = Mathf.Clamp01(value);
			this._fbbik.solver.rightArmMapping.weight = num * 1f;
		}
	}

	// Token: 0x17000BCE RID: 3022
	// (get) Token: 0x06004F86 RID: 20358 RVA: 0x001AE3D0 File Offset: 0x001AC7D0
	// (set) Token: 0x06004F87 RID: 20359 RVA: 0x001AE3EC File Offset: 0x001AC7EC
	public float HeadPosWeight
	{
		get
		{
			if (!this._ready)
			{
				return 0f;
			}
			return this._headPosWeight;
		}
		set
		{
			if (!this._ready)
			{
				return;
			}
			float num = Mathf.Clamp01(value);
			this._headPosWeight = num;
			if (!this._lastSeated)
			{
				this._fbbikHead.positionWeight = num * this._baseSpinePosWeight;
				this._fbbikHead.CCDWeight = num;
			}
		}
	}

	// Token: 0x17000BCF RID: 3023
	// (get) Token: 0x06004F88 RID: 20360 RVA: 0x001AE43D File Offset: 0x001AC83D
	// (set) Token: 0x06004F89 RID: 20361 RVA: 0x001AE458 File Offset: 0x001AC858
	public float HeadRotWeight
	{
		get
		{
			if (!this._ready)
			{
				return 0f;
			}
			return this._headRotWeight;
		}
		set
		{
			if (!this._ready)
			{
				return;
			}
			float num = Mathf.Clamp01(value);
			this._headRotWeight = num;
			if (!this._lastSeated)
			{
				this._fbbikHead.rotationWeight = num * this._baseSpineRotWeight;
			}
		}
	}

	// Token: 0x17000BD0 RID: 3024
	// (get) Token: 0x06004F8A RID: 20362 RVA: 0x001AE49D File Offset: 0x001AC89D
	// (set) Token: 0x06004F8B RID: 20363 RVA: 0x001AE4C8 File Offset: 0x001AC8C8
	public float LowerBodyWeight
	{
		get
		{
			if (!this._ready || this._fbbik == null)
			{
				return 0f;
			}
			return this._lowerBodyWeight;
		}
		set
		{
			if (!this._ready || this._fbbik == null)
			{
				return;
			}
			float num = Mathf.Clamp01(value);
			if (this._enableLowerBodyTracking && num != this._lowerBodyWeight)
			{
				this._fbbik.solver.bodyEffector.positionWeight = num;
				this._fbbik.solver.bodyEffector.rotationWeight = num;
				this._fbbik.solver.leftLegMapping.weight = num;
				this._fbbik.solver.leftFootEffector.positionWeight = num;
				this._fbbik.solver.leftFootEffector.rotationWeight = num;
				this._fbbik.solver.leftThighEffector.positionWeight = num;
				this._fbbik.solver.rightLegMapping.weight = num;
				this._fbbik.solver.rightFootEffector.positionWeight = num;
				this._fbbik.solver.rightFootEffector.rotationWeight = num;
				this._fbbik.solver.rightThighEffector.positionWeight = num;
				this._lowerBodyWeight = num;
			}
		}
	}

	// Token: 0x17000BD1 RID: 3025
	// (get) Token: 0x06004F8C RID: 20364 RVA: 0x001AE5F3 File Offset: 0x001AC9F3
	// (set) Token: 0x06004F8D RID: 20365 RVA: 0x001AE616 File Offset: 0x001ACA16
	public float SolverWeight
	{
		get
		{
			if (!this._ready)
			{
				return 0f;
			}
			return this._fbbik.solver.IKPositionWeight;
		}
		set
		{
			if (!this._ready)
			{
				return;
			}
			if (value != this._fbbik.solver.IKPositionWeight)
			{
				this._fbbik.solver.IKPositionWeight = value;
			}
		}
	}

	// Token: 0x06004F8E RID: 20366 RVA: 0x001AE64B File Offset: 0x001ACA4B
	public void LocomotionChange(bool loco)
	{
	}

	// Token: 0x06004F8F RID: 20367 RVA: 0x001AE650 File Offset: 0x001ACA50
	private Vector3 MatchLocalAxis(Transform t, Vector3 worldaxis)
	{
		Vector3 vector = t.InverseTransformDirection(worldaxis);
		if (Mathf.Abs(vector.x) < 0.2f)
		{
			vector.x = 0f;
		}
		if (Mathf.Abs(vector.y) < 0.2f)
		{
			vector.y = 0f;
		}
		if (Mathf.Abs(vector.z) < 0.2f)
		{
			vector.z = 0f;
		}
		return vector.normalized;
	}

	// Token: 0x06004F90 RID: 20368 RVA: 0x001AE6D4 File Offset: 0x001ACAD4
	private float RotationAlongMajorAxis(Quaternion rot, Vector3 axis)
	{
		float num = 0f;
		Vector3 eulerAngles = rot.eulerAngles;
		if (Mathf.Abs(axis.x) > 0.5f)
		{
			num = eulerAngles.x * Mathf.Sign(axis.x);
		}
		if (Mathf.Abs(axis.y) > 0.5f)
		{
			num = eulerAngles.y * Mathf.Sign(axis.y);
		}
		if (Mathf.Abs(axis.z) > 0.5f)
		{
			num = eulerAngles.z * Mathf.Sign(axis.z);
		}
		if (num > 180f)
		{
			num -= 360f;
		}
		return num;
	}

	// Token: 0x06004F91 RID: 20369 RVA: 0x001AE784 File Offset: 0x001ACB84
	private void SetMajorAxisLocalRotation(Transform t, Vector3 axis, float angle)
	{
		Vector3 localEulerAngles = t.localEulerAngles;
		if (Mathf.Abs(axis.x) > 0.5f)
		{
			localEulerAngles.x = angle * Mathf.Sign(axis.x);
		}
		if (Mathf.Abs(axis.y) > 0.5f)
		{
			localEulerAngles.y = angle * Mathf.Sign(axis.y);
		}
		if (Mathf.Abs(axis.z) > 0.5f)
		{
			localEulerAngles.z = angle * Mathf.Sign(axis.z);
		}
		t.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06004F92 RID: 20370 RVA: 0x001AE820 File Offset: 0x001ACC20
	public bool Initialize(VRC_AnimationController animControl, Animator anim, VRCPlayer player, bool local)
	{
		if (this._inited)
		{
			this.Uninitialize();
		}
		this._frameSinceInit = 0;
		this._enableLowerBodyTracking = false;
		this._animCtl = animControl;
		this._animMgr = this._animCtl.GetComponent<AnimatorControllerManager>();
		this._anim = anim;
		this._player = player;
		this._playerT = player.transform;
		this._avatarMgr = this._player.GetComponentInChildren<VRCAvatarManager>();
		this._motion = this._playerT.GetComponent<VRCMotionState>();
		this._local = local;
		this._fbbik = this._anim.GetComponent<FullBodyBipedIK>();
		this._ik = base.GetComponent<IkController>();
		this._inGrabL = VRCInputManager.FindInput("GrabLeft");
		this._inGrabR = VRCInputManager.FindInput("GrabRight");
		if (this._avatarMgr.CurrentAvatar == null)
		{
			if (this._avatarMgr.currentAvatarKind == VRCAvatarManager.AvatarKind.Custom)
			{
				Debug.LogError("IK[" + this._player.name + "] (6pt) init, but Loading was not finished!");
				this._isCustomAvatar = true;
			}
			else
			{
				Debug.Log(string.Concat(new object[]
				{
					"IK[",
					this._player.name,
					"] (6pt) (",
					this._avatarMgr.currentAvatarKind,
					")"
				}));
				this._isCustomAvatar = false;
			}
		}
		else if (this._avatarMgr.currentAvatarKind == VRCAvatarManager.AvatarKind.Custom)
		{
			Debug.Log("IK[" + this._player.name + "] (6pt) init avatar = " + this._avatarMgr.CurrentAvatar.id);
			this._isCustomAvatar = true;
		}
		else
		{
			Debug.Log(string.Concat(new object[]
			{
				"IK[",
				this._player.name,
				"] (6pt) (",
				this._avatarMgr.currentAvatarKind,
				")"
			}));
			this._isCustomAvatar = false;
		}
		if (!this._local)
		{
			this.ForceTPose();
		}
		try
		{
			this._headTarget = this._ik.HeadEffector.transform.Find("HeadBoneOffset");
			Transform transform = this._ik.LeftEffector.transform;
			Transform transform2 = this._ik.RightEffector.transform;
			this._hipTarget = this._ik.HipEffector.transform;
			this._leftFootTarget = this._ik.LeftFootEffector.transform;
			this._rightFootTarget = this._ik.RightFootEffector.transform;
			if (this._hipTarget == null || this._leftFootTarget == null || this._rightFootTarget == null)
			{
				Debug.LogError("Hip and/or Foot IK Targets are null");
			}
			if (this._leftKneeTarget == null || this._rightKneeTarget == null)
			{
				Debug.LogError("Knee bend IK Targets are null");
			}
			BipedReferences references = null;
			BipedReferences.AutoDetectReferences(ref references, this._anim.transform, BipedReferences.AutoDetectParams.Default);
			this._fbbik.SetReferences(references, this._anim.GetBoneTransform(HumanBodyBones.Spine));
			this._fbbik.fixTransforms = true;
			this._fbbik.solver.bodyEffector.target = null;
			this._fbbik.solver.leftFootEffector.target = null;
			this._fbbik.solver.rightFootEffector.target = null;
			this._fbbik.solver.leftLegChain.bendConstraint.bendGoal = null;
			this._fbbik.solver.rightLegChain.bendConstraint.bendGoal = null;
			this.CheckFingers();
			Transform boneTransform = this._anim.GetBoneTransform(HumanBodyBones.LeftHand);
			Transform boneTransform2 = this._anim.GetBoneTransform(HumanBodyBones.RightHand);
			this._poseWristRotL = Quaternion.Inverse(this._anim.transform.rotation) * boneTransform.rotation;
			this._poseWristRotR = Quaternion.Inverse(this._anim.transform.rotation) * boneTransform2.rotation;
			this._fbbik.solver.bodyEffector.target = this._hipTarget;
			this._fbbik.solver.bodyEffector.positionWeight = 0.8f;
			this._fbbik.solver.bodyEffector.rotationWeight = 0.8f;
			this._fbbik.solver.bodyEffector.effectChildNodes = true;
			this._fbbik.solver.spineStiffness = 0.5f;
			this._fbbik.solver.pullBodyVertical = 0.5f;
			this._fbbik.solver.pullBodyHorizontal = 0.5f;
			this._fbbik.solver.spineMapping.iterations = 3;
			this._fbbik.solver.spineMapping.twistWeight = 0.5f;
			this._hipForward = this._hipTarget.InverseTransformVector(this._anim.transform.parent.TransformVector(new Vector3(0f, 0f, 1f)));
			Transform transform3 = this._anim.GetBoneTransform(HumanBodyBones.Hips).transform;
			this._hipTarget.position = transform3.position;
			this._hipTarget.rotation = transform3.rotation;
			Transform transform4 = this._anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg).transform;
			Transform transform5 = this._anim.GetBoneTransform(HumanBodyBones.RightUpperLeg).transform;
			Vector3 b = transform4.position - transform3.position;
			Vector3 b2 = transform5.position - transform3.position;
			if (this._thighLTarget == null)
			{
				this._thighLTarget = new GameObject().transform;
			}
			this._thighLTarget.name = "LeftThighTarget";
			if (this._thighRTarget == null)
			{
				this._thighRTarget = new GameObject().transform;
			}
			this._thighRTarget.name = "RightThighTarget";
			this._thighLTarget.position = this._hipTarget.position + b;
			this._thighRTarget.position = this._hipTarget.position + b2;
			this._thighLTarget.rotation = transform4.rotation;
			this._thighRTarget.rotation = transform5.rotation;
			this._thighLTarget.SetParent(this._hipTarget, true);
			this._thighRTarget.SetParent(this._hipTarget, true);
			this._fbbik.solver.leftThighEffector.target = this._thighLTarget;
			this._fbbik.solver.rightThighEffector.target = this._thighRTarget;
			this._fbbik.solver.leftThighEffector.positionWeight = 1f;
			this._fbbik.solver.rightThighEffector.positionWeight = 1f;
			this._fbbik.solver.headMapping.maintainRotationWeight = 0f;
			this._fbbik.solver.leftHandEffector.target = transform;
			this._fbbik.solver.leftHandEffector.positionWeight = 1f;
			this._fbbik.solver.leftHandEffector.rotationWeight = 1f;
			this._fbbik.solver.leftHandEffector.maintainRelativePositionWeight = 0f;
			this._fbbik.solver.leftArmChain.pull = 0f;
			this._fbbik.solver.leftArmChain.push = 0f;
			this._fbbik.solver.leftArmChain.reach = 0f;
			this._fbbik.solver.leftArmChain.pushParent = 0f;
			this._fbbik.solver.leftArmChain.reachSmoothing = FBIKChain.Smoothing.None;
			this._fbbik.solver.leftArmChain.pushSmoothing = FBIKChain.Smoothing.None;
			this._fbbik.solver.leftArmMapping.weight = 0f;
			this._fbbik.solver.leftArmMapping.maintainRotationWeight = 0f;
			this._fbbik.solver.rightHandEffector.target = transform2;
			this._fbbik.solver.rightHandEffector.positionWeight = 1f;
			this._fbbik.solver.rightHandEffector.rotationWeight = 1f;
			this._fbbik.solver.rightHandEffector.maintainRelativePositionWeight = 0f;
			this._fbbik.solver.rightArmChain.pull = 0f;
			this._fbbik.solver.rightArmChain.push = 0f;
			this._fbbik.solver.rightArmChain.reach = 0f;
			this._fbbik.solver.rightArmChain.pushParent = 0f;
			this._fbbik.solver.rightArmChain.reachSmoothing = FBIKChain.Smoothing.None;
			this._fbbik.solver.rightArmChain.pushSmoothing = FBIKChain.Smoothing.None;
			this._fbbik.solver.rightArmMapping.weight = 0f;
			this._fbbik.solver.rightArmMapping.maintainRotationWeight = 0f;
			this._fbbikShoulder = this._fbbik.gameObject.AddComponent<ShoulderRotator>();
			this._fbbikShoulder.weight = 1.2f;
			this._fbbikShoulder.offset = 0.2f;
			this._fbbikArm = this._fbbik.gameObject.AddComponent<FBBIKArmBending>();
			this._fbbikArm.ik = this._fbbik;
			this._fbbikArm.bendDirectionOffsetLeft = new Vector3(0f, 0f, -1f);
			this._fbbikArm.bendDirectionOffsetRight = new Vector3(0f, 0f, -1f);
			this._fbbikArm.characterSpaceBendOffsetLeft = new Vector3(-0.8f, -1f, -1f);
			this._fbbikArm.characterSpaceBendOffsetRight = new Vector3(0.8f, -1f, -1f);
			this._fbbik.solver.leftFootEffector.positionWeight = 0f;
			this._fbbik.solver.leftFootEffector.rotationWeight = 0f;
			this._fbbik.solver.leftFootEffector.maintainRelativePositionWeight = 0f;
			this._fbbik.solver.leftLegChain.bendConstraint.weight = 0f;
			this._fbbik.solver.leftLegChain.pull = 1f;
			this._fbbik.solver.leftLegChain.push = 0f;
			this._fbbik.solver.leftLegChain.reach = 0f;
			this._fbbik.solver.leftLegChain.pushParent = 0f;
			this._fbbik.solver.leftLegChain.reachSmoothing = FBIKChain.Smoothing.None;
			this._fbbik.solver.leftLegMapping.weight = 0f;
			this._fbbik.solver.leftLegMapping.maintainRotationWeight = 0f;
			this._fbbik.solver.rightFootEffector.positionWeight = 0f;
			this._fbbik.solver.rightFootEffector.rotationWeight = 0f;
			this._fbbik.solver.rightFootEffector.maintainRelativePositionWeight = 0f;
			this._fbbik.solver.rightLegChain.bendConstraint.weight = 0f;
			this._fbbik.solver.rightLegChain.pull = 1f;
			this._fbbik.solver.rightLegChain.push = 0f;
			this._fbbik.solver.rightLegChain.reach = 0f;
			this._fbbik.solver.rightLegChain.pushParent = 0f;
			this._fbbik.solver.rightLegChain.reachSmoothing = FBIKChain.Smoothing.None;
			this._fbbik.solver.rightLegMapping.weight = 0f;
			this._fbbik.solver.rightLegMapping.maintainRotationWeight = 0f;
			GameObject gameObject = this._anim.GetBoneTransform(HumanBodyBones.LeftLowerArm).gameObject;
			if (gameObject != null)
			{
				this._forearmLTwistRelaxer = gameObject.AddComponent<TwistRelaxer>();
				this._forearmLTwistRelaxer.weight = 0.6f;
				this._forearmLTwistRelaxer.parentChildCrossfade = 0.8f;
			}
			GameObject gameObject2 = this._anim.GetBoneTransform(HumanBodyBones.RightLowerArm).gameObject;
			if (gameObject2 != null)
			{
				this._forearmRTwistRelaxer = gameObject2.AddComponent<TwistRelaxer>();
				this._forearmRTwistRelaxer.weight = 0.6f;
				this._forearmRTwistRelaxer.parentChildCrossfade = 0.8f;
			}
			Transform boneTransform3 = this._anim.GetBoneTransform(HumanBodyBones.LeftFoot);
			Transform boneTransform4 = this._anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
			Transform boneTransform5 = this._anim.GetBoneTransform(HumanBodyBones.RightFoot);
			Transform boneTransform6 = this._anim.GetBoneTransform(HumanBodyBones.RightLowerLeg);
			this._kneeBendDist = Mathf.Max((boneTransform3.position - boneTransform4.position).magnitude, (boneTransform5.position - boneTransform6.position).magnitude);
			Vector3 vector = new Vector3(0f, 1f, 1f);
			Vector3 normalized = vector.normalized;
			this._kneeLBendDir = boneTransform3.InverseTransformDirection(this._anim.transform.TransformDirection(normalized));
			this._kneeRBendDir = boneTransform5.InverseTransformDirection(this._anim.transform.TransformDirection(normalized));
			this._fbbik.solver.StoreDefaultLocalState();
			this.SetupFBBIKHead();
		}
		catch (Exception ex)
		{
			if (!this._local)
			{
				this.RemoveTPose();
			}
			Debug.LogWarning("FBBIK disabled because of setup error: " + ex.Message);
			return false;
		}
		IKSolverFullBodyBiped solver = this._fbbik.solver;
		solver.OnPreUpdate = (IKSolver.UpdateDelegate)Delegate.Combine(solver.OnPreUpdate, new IKSolver.UpdateDelegate(this.OnPreUpdate));
		IKSolverFullBodyBiped solver2 = this._fbbik.solver;
		solver2.OnPostUpdate = (IKSolver.UpdateDelegate)Delegate.Combine(solver2.OnPostUpdate, new IKSolver.UpdateDelegate(this.OnPostUpdate));
		if (this._fbbikArm != null)
		{
			FBBIKArmBending fbbikArm = this._fbbikArm;
			fbbikArm.OnPostArmBend = (IKSolver.UpdateDelegate)Delegate.Combine(fbbikArm.OnPostArmBend, new IKSolver.UpdateDelegate(this.OnPostArmBend));
		}
		VRCUiManager.Instance.onUiEnabled += this.OnUIEnabled;
		VRCUiManager.Instance.onUiDisabled += this.OnUIDisabled;
		if (this._local)
		{
			this._anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
		}
		else
		{
			this._anim.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
		}
		this._lastPos = this._playerT.position;
		this._lastRot = this._playerT.rotation;
		this._inited = true;
		this._ready = false;
		this._calibrated = false;
		this.SolverWeight = 1f;
		if (!this._local)
		{
			this.SyncLowerBodyTargetsWithSkeleton();
			this.RemoveTPose();
		}
		this.Reset(true);
		return true;
	}

	// Token: 0x06004F93 RID: 20371 RVA: 0x001AF7D8 File Offset: 0x001ADBD8
	private void ForceTPose()
	{
		this._animMgr.Push(this._animMgr.tPoseController);
		this._anim.updateMode = AnimatorUpdateMode.UnscaledTime;
		this._anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
		this._anim.Update(0.01f);
		this._anim.transform.localPosition = Vector3.zero;
		this._anim.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x06004F94 RID: 20372 RVA: 0x001AF84D File Offset: 0x001ADC4D
	private void RemoveTPose()
	{
		this._animMgr.Pop();
		this._anim.updateMode = AnimatorUpdateMode.Normal;
		this._anim.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
	}

	// Token: 0x06004F95 RID: 20373 RVA: 0x001AF874 File Offset: 0x001ADC74
	private void SetupFBBIKHead()
	{
		this._fbbikHead = this._headTarget.gameObject.AddComponent<FBBIKHeadEffector>();
		this._fbbikHead.ik = this._fbbik;
		this._fbbikHead.positionWeight = 0.5f;
		this._fbbikHead.rotationWeight = 0.5f;
		this._fbbikHead.bodyWeight = 0f;
		this._fbbikHead.thighWeight = 0f;
		this._fbbikHead.handsPullBody = true;
		this._fbbikHead.bodyClampWeight = 1f;
		this._fbbikHead.headClampWeight = 0f;
		this._fbbikHead.bendWeight = 0f;
		this._fbbikHead.postStretchWeight = 0f;
		this._fbbikHead.maxStretch = 0.05f;
		this._fbbikHead.stretchDamper = 0f;
		this._fbbikHead.fixHead = false;
		this._fbbikHead.chestDirectionWeight = 0f;
		this._fbbikHead.CCDWeight = 1f;
		this._fbbikHead.roll = 0f;
		this._fbbikHead.damper = 0f;
		this._fbbikHead.CCDBones = new Transform[4];
		this._fbbikHead.CCDBones[0] = this._anim.GetBoneTransform(HumanBodyBones.Neck);
		this._fbbikHead.CCDBones[1] = this._anim.GetBoneTransform(HumanBodyBones.Chest);
		this._fbbikHead.CCDBones[2] = this._anim.GetBoneTransform(HumanBodyBones.Spine);
		this._fbbikHead.CCDBones[3] = this._anim.GetBoneTransform(HumanBodyBones.Hips);
		this._fbbikHead.Initialize();
	}

	// Token: 0x06004F96 RID: 20374 RVA: 0x001AFA24 File Offset: 0x001ADE24
	private void ClearAvatarRotation()
	{
		if (!VRCTrackingManager.IsInitialized())
		{
			return;
		}
		Vector3 eulerAngles = VRCTrackingManager.GetWorldTrackingRotation().eulerAngles;
		eulerAngles.x = (eulerAngles.z = 0f);
		this._playerT.rotation = Quaternion.Euler(eulerAngles);
		this.NeedsReset();
	}

	// Token: 0x06004F97 RID: 20375 RVA: 0x001AFA78 File Offset: 0x001ADE78
	private BipedReferences AssignBipedReferences()
	{
		BipedReferences bipedReferences = new BipedReferences();
		bipedReferences.root = this._anim.transform;
		bipedReferences.pelvis = this._anim.GetBoneTransform(HumanBodyBones.Hips);
		bipedReferences.spine = new Transform[2];
		bipedReferences.spine[0] = this._anim.GetBoneTransform(HumanBodyBones.Spine);
		bipedReferences.spine[1] = this._anim.GetBoneTransform(HumanBodyBones.Chest);
		bipedReferences.leftUpperArm = this._anim.GetBoneTransform(HumanBodyBones.LeftUpperArm);
		bipedReferences.leftForearm = this._anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
		bipedReferences.leftHand = this._anim.GetBoneTransform(HumanBodyBones.LeftHand);
		bipedReferences.rightUpperArm = this._anim.GetBoneTransform(HumanBodyBones.RightUpperArm);
		bipedReferences.rightForearm = this._anim.GetBoneTransform(HumanBodyBones.RightLowerArm);
		bipedReferences.rightHand = this._anim.GetBoneTransform(HumanBodyBones.RightHand);
		bipedReferences.leftThigh = this._anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
		bipedReferences.leftForearm = this._anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
		bipedReferences.leftFoot = this._anim.GetBoneTransform(HumanBodyBones.LeftFoot);
		bipedReferences.rightThigh = this._anim.GetBoneTransform(HumanBodyBones.RightUpperLeg);
		bipedReferences.rightForearm = this._anim.GetBoneTransform(HumanBodyBones.RightLowerLeg);
		bipedReferences.rightFoot = this._anim.GetBoneTransform(HumanBodyBones.RightFoot);
		bipedReferences.head = this._anim.GetBoneTransform(HumanBodyBones.Head);
		return bipedReferences;
	}

	// Token: 0x06004F98 RID: 20376 RVA: 0x001AFBD4 File Offset: 0x001ADFD4
	private void CheckReferences(ref BipedReferences refs)
	{
		Transform boneTransform = this._anim.GetBoneTransform(HumanBodyBones.LeftShoulder);
		Transform boneTransform2 = this._anim.GetBoneTransform(HumanBodyBones.RightShoulder);
		Transform parent = boneTransform.parent;
		if (parent == null || boneTransform2.parent != parent)
		{
			return;
		}
		int num = refs.spine.Length;
		if (num > 1 && refs.spine[num - 1] != parent)
		{
			refs.spine[num - 1] = parent;
		}
	}

	// Token: 0x06004F99 RID: 20377 RVA: 0x001AFC58 File Offset: 0x001AE058
	private void CheckFingers()
	{
		if (this._anim != null && this._anim.GetBoneTransform(HumanBodyBones.LeftThumbProximal) != null && this._anim.GetBoneTransform(HumanBodyBones.LeftIndexProximal) != null && this._anim.GetBoneTransform(HumanBodyBones.LeftMiddleProximal) != null && this._anim.GetBoneTransform(HumanBodyBones.RightThumbProximal) != null && this._anim.GetBoneTransform(HumanBodyBones.RightIndexProximal) != null && this._anim.GetBoneTransform(HumanBodyBones.RightMiddleProximal) != null)
		{
			return;
		}
		throw new NotSupportedException("Avatar doesn't have enough fingers.");
	}

	// Token: 0x06004F9A RID: 20378 RVA: 0x001AFD11 File Offset: 0x001AE111
	private void OnDestroy()
	{
		this.Uninitialize();
	}

	// Token: 0x06004F9B RID: 20379 RVA: 0x001AFD1C File Offset: 0x001AE11C
	public void Uninitialize()
	{
		if (this._inited)
		{
			this._inited = false;
			this._ready = false;
			if (this._frozenInput)
			{
				this._inputState.PopInputController();
				this._frozenInput = false;
			}
			IKSolverFullBodyBiped solver = this._fbbik.solver;
			solver.OnPreUpdate = (IKSolver.UpdateDelegate)Delegate.Remove(solver.OnPreUpdate, new IKSolver.UpdateDelegate(this.OnPreUpdate));
			IKSolverFullBodyBiped solver2 = this._fbbik.solver;
			solver2.OnPostUpdate = (IKSolver.UpdateDelegate)Delegate.Remove(solver2.OnPostUpdate, new IKSolver.UpdateDelegate(this.OnPostUpdate));
			VRCUiManager.Instance.onUiEnabled -= this.OnUIEnabled;
			VRCUiManager.Instance.onUiDisabled -= this.OnUIDisabled;
			if (this._fbbikHead != null)
			{
				UnityEngine.Object.Destroy(this._fbbikHead);
				this._fbbikHead = null;
			}
			if (this._fbbikArm != null)
			{
				FBBIKArmBending fbbikArm = this._fbbikArm;
				fbbikArm.OnPostArmBend = (IKSolver.UpdateDelegate)Delegate.Remove(fbbikArm.OnPostArmBend, new IKSolver.UpdateDelegate(this.OnPostArmBend));
				UnityEngine.Object.Destroy(this._fbbikArm);
				this._fbbikArm = null;
			}
			if (this._fbbikShoulder != null)
			{
				UnityEngine.Object.Destroy(this._fbbikShoulder);
				this._fbbikShoulder = null;
			}
			this._calibrated = false;
			if (this._forearmLTwistRelaxer != null)
			{
				UnityEngine.Object.Destroy(this._forearmLTwistRelaxer);
			}
			if (this._forearmLTwistRelaxer != null)
			{
				UnityEngine.Object.Destroy(this._forearmRTwistRelaxer);
			}
			this._frameSinceInit = 0;
		}
	}

	// Token: 0x06004F9C RID: 20380 RVA: 0x001AFEBA File Offset: 0x001AE2BA
	public void NeedsReset()
	{
		this._needReset = true;
		this.enableIk = false;
	}

	// Token: 0x06004F9D RID: 20381 RVA: 0x001AFECC File Offset: 0x001AE2CC
	public void Reset(bool restore = true)
	{
		if (!this._inited)
		{
			return;
		}
		if (this._fbbik.solver.initiated && this._anim != null)
		{
			if (this._anim.transform != null)
			{
				this._anim.transform.localPosition = Vector3.zero;
				this._anim.transform.localRotation = Quaternion.identity;
				if (this._anim.transform.parent != null)
				{
					this._anim.transform.parent.localPosition = Vector3.zero;
					this._anim.transform.parent.localRotation = Quaternion.identity;
				}
			}
			this._anim.Update(0f);
			if (restore)
			{
				this.enableIk = true;
			}
			this._needReset = false;
		}
	}

	// Token: 0x06004F9E RID: 20382 RVA: 0x001AFFC0 File Offset: 0x001AE3C0
	private IEnumerator AlignPlayerTracking()
	{
		yield return null;
		yield return null;
		yield return null;
		if (this._local)
		{
			this._playerT.position = VRCTrackingManager.GetWorldTrackingPosition();
		}
		yield break;
	}

	// Token: 0x06004F9F RID: 20383 RVA: 0x001AFFDC File Offset: 0x001AE3DC
	private void OnPreUpdate()
	{
		if (!this._inited)
		{
			return;
		}
		if (this._culled)
		{
			return;
		}
		this.PreIkHipPos = this._anim.GetBoneTransform(HumanBodyBones.Hips).position;
		if (!this._motion.InVehicle)
		{
			float sqrMagnitude = (this._playerT.position - this._lastPos).sqrMagnitude;
			float num = Quaternion.Angle(this._playerT.rotation, this._lastRot);
			if (sqrMagnitude > 1f || num > 45f)
			{
				this.NeedsReset();
			}
		}
		this._lastPos = this._playerT.position;
		this._lastRot = this._playerT.rotation;
	}

	// Token: 0x06004FA0 RID: 20384 RVA: 0x001B009C File Offset: 0x001AE49C
	private void Update()
	{
		if (!this._inited)
		{
			return;
		}
		if (this._isCustomAvatar && this._avatarMgr.CurrentAvatar == null)
		{
			return;
		}
		if (this._culled)
		{
			if (this.enableIk)
			{
				if (this._fbbik != null)
				{
					this._fbbik.enabled = false;
				}
				if (this._fbbikHead != null)
				{
					this._fbbikHead.enabled = false;
				}
				if (this._forearmLTwistRelaxer != null)
				{
					this._forearmLTwistRelaxer.enabled = false;
				}
				if (this._forearmRTwistRelaxer != null)
				{
					this._forearmRTwistRelaxer.enabled = false;
				}
			}
			return;
		}
		if (this._fbbik != null)
		{
			this._fbbik.enabled = this.enableIk;
		}
		if (this._fbbikHead != null)
		{
			this._fbbikHead.enabled = this.enableIk;
		}
		if (this._forearmLTwistRelaxer != null)
		{
			this._forearmLTwistRelaxer.enabled = this.enableIk;
		}
		if (this._forearmRTwistRelaxer != null)
		{
			this._forearmRTwistRelaxer.enabled = this.enableIk;
		}
		if (this._needReset)
		{
			this.Reset(true);
		}
		if (this._frameSinceInit == 1)
		{
			if (this._local && this._isCustomAvatar)
			{
				this._enableLowerBodyTracking = VRCTrackingManager.CanSupportHipAndFeetTracking();
				if (this._enableLowerBodyTracking)
				{
					VRCTrackingManager.SetSeatedPlayMode(false);
					Debug.LogWarning("Hip+Feet Tracking: 3 trackers found, tracking enabled.");
				}
				this._inputState = this._player.GetComponent<InputStateControllerManager>();
				this._inputState.PushInputController("ImmobileInputController");
				this._frozenInput = true;
			}
			if (this._local && this._isCustomAvatar && this._enableLowerBodyTracking && !VRCTrackingManager.IsCalibratedForAvatar(this._avatarMgr.CurrentAvatar.id))
			{
				VRCUiManager.Instance.CloseUi(false);
				this.enableIk = false;
				this._animMgr.Push(this._animMgr.tPoseController);
				VRCTrackingManager.SetTrackingForCalibration();
				Debug.LogWarning("Hip+Feet Tracking: in T-Pose for calibration");
			}
			else
			{
				this._animCtl.SetupHandGestures(this._anim, false);
				this._enableLowerBodyTracking = true;
				this._calibrated = true;
			}
		}
		if (!this._calibrated && this._local && this._enableLowerBodyTracking)
		{
			this._anim.transform.localPosition = Vector3.zero;
			this._anim.transform.localRotation = Quaternion.identity;
			if (this._anim.transform.parent != null)
			{
				this._anim.transform.parent.localPosition = Vector3.zero;
				this._anim.transform.parent.localRotation = Quaternion.identity;
			}
			this._anim.Update(0f);
			if (this._inGrabR.button && this._inGrabL.button)
			{
				VRCTrackingManager.PerformCalibration(this._anim, true, true);
				VRCTrackingManager.RestoreTrackingAfterCalibration();
				this.Reset(true);
				this._animMgr.Pop();
				this._animCtl.SetupHandGestures(this._anim, false);
				this._inputState.PopInputController();
				this._frozenInput = false;
				this._calibrated = true;
				this.CheckPoseChange(true);
				Debug.LogWarning("Hip+Feet Tracking: Calibrated");
			}
		}
		if (this._ready)
		{
			this.CheckPoseChange(false);
		}
		if (!this._ready && this._calibrated)
		{
			this._ready = true;
			this.HeadPosWeight = 1f;
			this.HeadRotWeight = 1f;
			this.LeftHandWeight = 1f;
			this.RightHandWeight = 1f;
			this.LowerBodyWeight = 1f;
			this.CheckPoseChange(true);
		}
		if (this._frameSinceInit == 10 && this._frozenInput && this._calibrated)
		{
			this._inputState.PopInputController();
			this._frozenInput = false;
		}
		if (this._frameSinceInit < 300)
		{
			this._frameSinceInit++;
		}
		this.UpdateTargets();
	}

	// Token: 0x06004FA1 RID: 20385 RVA: 0x001B04F0 File Offset: 0x001AE8F0
	private void UpdateTargets()
	{
		if (!this._inited || this._culled)
		{
			return;
		}
		if (this._enableLowerBodyTracking)
		{
			if (this._local && VRCTrackingManager.IsPlayerNearTracking() && VRCTrackingManager.IsTracked(VRCTracking.ID.BodyTracker_Hip) && VRCTrackingManager.IsTracked(VRCTracking.ID.FootTracker_LeftFoot) && VRCTrackingManager.IsTracked(VRCTracking.ID.FootTracker_RightFoot))
			{
				Transform trackedTransform = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.BodyTracker_Hip);
				Transform trackedTransform2 = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.FootTracker_LeftFoot);
				Transform trackedTransform3 = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.FootTracker_RightFoot);
				this._hipTarget.position = trackedTransform.position;
				this._hipTarget.rotation = trackedTransform.rotation;
				this._leftFootTarget.position = trackedTransform2.position;
				this._leftFootTarget.rotation = trackedTransform2.rotation;
				this._rightFootTarget.position = trackedTransform3.position;
				this._rightFootTarget.rotation = trackedTransform3.rotation;
			}
			if (this._anim.transform != null)
			{
				Transform leftFootTarget = this._leftFootTarget;
				Transform rightFootTarget = this._rightFootTarget;
				Vector3 position = leftFootTarget.position + this._kneeBendDist * leftFootTarget.TransformDirection(this._kneeLBendDir);
				Vector3 position2 = rightFootTarget.position + this._kneeBendDist * rightFootTarget.TransformDirection(this._kneeRBendDir);
				this._leftKneeTarget.position = position;
				this._rightKneeTarget.position = position2;
			}
			if (this._fbbikHead != null)
			{
				Vector3 chestDirection = this._hipTarget.TransformVector(this._hipForward);
				this._fbbikHead.chestDirection = chestDirection;
			}
		}
	}

	// Token: 0x06004FA2 RID: 20386 RVA: 0x001B0690 File Offset: 0x001AEA90
	private void LateUpdate()
	{
	}

	// Token: 0x06004FA3 RID: 20387 RVA: 0x001B0694 File Offset: 0x001AEA94
	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(this.PostIkLeftHandPos, this.PostIkLeftHandPos + 0.1f * this.lThumbDir);
		Gizmos.DrawLine(this.PostIkRightHandPos, this.PostIkRightHandPos + 0.1f * this.rThumbDir);
	}

	// Token: 0x06004FA4 RID: 20388 RVA: 0x001B06F0 File Offset: 0x001AEAF0
	private IEnumerator PlayerDetachCoroutine()
	{
		this.NeedsReset();
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		this._forwardT = this._anim.transform.parent;
		this._anim.transform.SetParent(null, true);
		yield break;
	}

	// Token: 0x06004FA5 RID: 20389 RVA: 0x001B070B File Offset: 0x001AEB0B
	private void OnUIEnabled()
	{
		if (!this._local)
		{
			return;
		}
		if (this._forwardT != null)
		{
			return;
		}
		base.StartCoroutine(this.PlayerDetachCoroutine());
	}

	// Token: 0x06004FA6 RID: 20390 RVA: 0x001B0738 File Offset: 0x001AEB38
	private void OnUIDisabled()
	{
		if (!this._local)
		{
			return;
		}
		if (this._forwardT == null)
		{
			return;
		}
		this._anim.transform.SetParent(this._forwardT, true);
		this._forwardT = null;
		this.NeedsReset();
	}

	// Token: 0x06004FA7 RID: 20391 RVA: 0x001B0788 File Offset: 0x001AEB88
	private void OnPostUpdate()
	{
		if (!this._inited)
		{
			return;
		}
		if (this._culled)
		{
			return;
		}
		if (this._anim != null)
		{
			Transform boneTransform = this._anim.GetBoneTransform(HumanBodyBones.LeftHand);
			Transform boneTransform2 = this._anim.GetBoneTransform(HumanBodyBones.RightHand);
			Transform boneTransform3 = this._anim.GetBoneTransform(HumanBodyBones.Head);
			this.PostIkHeadPos = boneTransform3.position;
			this.PostIkLeftHandPos = boneTransform.position;
			this.PostIkRightHandPos = boneTransform2.position;
		}
	}

	// Token: 0x06004FA8 RID: 20392 RVA: 0x001B080C File Offset: 0x001AEC0C
	private void OnPostArmBend()
	{
		if (!this._inited)
		{
			return;
		}
		if (this._culled)
		{
			return;
		}
		this.FixWrists();
	}

	// Token: 0x06004FA9 RID: 20393 RVA: 0x001B082C File Offset: 0x001AEC2C
	private void FixWrists()
	{
		if (!this._inited)
		{
			return;
		}
		if (this._culled)
		{
			return;
		}
		if (this._anim != null && this._fbbik != null && this._fbbik.solver != null && this._fbbik.solver.IKPositionWeight > 0f && this._fbbik.solver.leftHandEffector != null && this._fbbik.solver.leftHandEffector.target != null && this._fbbik.solver.rightHandEffector != null && this._fbbik.solver.rightHandEffector.target != null)
		{
			Transform boneTransform = this._anim.GetBoneTransform(HumanBodyBones.LeftHand);
			Transform boneTransform2 = this._anim.GetBoneTransform(HumanBodyBones.RightHand);
			Quaternion rotation = this._fbbik.solver.leftHandEffector.target.rotation;
			Quaternion rotation2 = this._fbbik.solver.rightHandEffector.target.rotation;
			if (this.LeftHandWeight > 0f)
			{
				boneTransform.rotation = rotation * this.LeftWristAlign * this._poseWristRotL;
			}
			if (this.RightHandWeight > 0f)
			{
				boneTransform2.rotation = rotation2 * this.RightWristAlign * this._poseWristRotR;
			}
		}
	}

	// Token: 0x06004FAA RID: 20394 RVA: 0x001B09B3 File Offset: 0x001AEDB3
	private void OnEnable()
	{
		this.enableIk = true;
	}

	// Token: 0x06004FAB RID: 20395 RVA: 0x001B09BC File Offset: 0x001AEDBC
	private void OnDisable()
	{
		this.enableIk = false;
	}

	// Token: 0x04003802 RID: 14338
	public RuntimeAnimatorController ikPoseController;

	// Token: 0x04003803 RID: 14339
	public Vector3 PostIkHeadPos;

	// Token: 0x04003804 RID: 14340
	public Vector3 PostIkLeftHandPos;

	// Token: 0x04003805 RID: 14341
	public Vector3 PostIkRightHandPos;

	// Token: 0x04003806 RID: 14342
	private Vector3 PreIkHipPos;

	// Token: 0x04003807 RID: 14343
	private bool _inited;

	// Token: 0x04003808 RID: 14344
	private bool _culled;

	// Token: 0x04003809 RID: 14345
	private int _frameSinceInit;

	// Token: 0x0400380A RID: 14346
	private bool _local;

	// Token: 0x0400380B RID: 14347
	private bool _calibrated;

	// Token: 0x0400380C RID: 14348
	private bool _frozenInput;

	// Token: 0x0400380D RID: 14349
	private bool _ready;

	// Token: 0x0400380E RID: 14350
	private bool _enableLowerBodyTracking;

	// Token: 0x0400380F RID: 14351
	private bool _isCustomAvatar;

	// Token: 0x04003810 RID: 14352
	private VRCPlayer _player;

	// Token: 0x04003811 RID: 14353
	private Transform _playerT;

	// Token: 0x04003812 RID: 14354
	private Transform _forwardT;

	// Token: 0x04003813 RID: 14355
	private Animator _anim;

	// Token: 0x04003814 RID: 14356
	private VRC_AnimationController _animCtl;

	// Token: 0x04003815 RID: 14357
	private AnimatorControllerManager _animMgr;

	// Token: 0x04003816 RID: 14358
	private VRCAvatarManager _avatarMgr;

	// Token: 0x04003817 RID: 14359
	private FullBodyBipedIK _fbbik;

	// Token: 0x04003818 RID: 14360
	private FBBIKHeadEffector _fbbikHead;

	// Token: 0x04003819 RID: 14361
	private FBBIKArmBending _fbbikArm;

	// Token: 0x0400381A RID: 14362
	private ShoulderRotator _fbbikShoulder;

	// Token: 0x0400381B RID: 14363
	private IkController _ik;

	// Token: 0x0400381C RID: 14364
	private VRCMotionState _motion;

	// Token: 0x0400381D RID: 14365
	private InputStateControllerManager _inputState;

	// Token: 0x0400381E RID: 14366
	private Quaternion LeftWristAlign = Quaternion.Euler(new Vector3(0f, 90f, 0f));

	// Token: 0x0400381F RID: 14367
	private Quaternion RightWristAlign = Quaternion.Euler(new Vector3(0f, -90f, 0f));

	// Token: 0x04003820 RID: 14368
	private Quaternion _poseWristRotL;

	// Token: 0x04003821 RID: 14369
	private Quaternion _poseWristRotR;

	// Token: 0x04003822 RID: 14370
	private TwistRelaxer _forearmLTwistRelaxer;

	// Token: 0x04003823 RID: 14371
	private TwistRelaxer _forearmRTwistRelaxer;

	// Token: 0x04003824 RID: 14372
	private Vector3 _lastPos;

	// Token: 0x04003825 RID: 14373
	private Quaternion _lastRot;

	// Token: 0x04003826 RID: 14374
	private Transform _hipTarget;

	// Token: 0x04003827 RID: 14375
	private Transform _leftFootTarget;

	// Token: 0x04003828 RID: 14376
	private Transform _rightFootTarget;

	// Token: 0x04003829 RID: 14377
	public Transform _leftKneeTarget;

	// Token: 0x0400382A RID: 14378
	public Transform _rightKneeTarget;

	// Token: 0x0400382B RID: 14379
	private Vector3 _hipForward;

	// Token: 0x0400382C RID: 14380
	private float _kneeBendDist;

	// Token: 0x0400382D RID: 14381
	private Vector3 _kneeLBendDir;

	// Token: 0x0400382E RID: 14382
	private Vector3 _kneeRBendDir;

	// Token: 0x0400382F RID: 14383
	private Transform _headTarget;

	// Token: 0x04003830 RID: 14384
	private Transform _thighLTarget;

	// Token: 0x04003831 RID: 14385
	private Transform _thighRTarget;

	// Token: 0x04003832 RID: 14386
	private const float _baseArmPosWeight = 1f;

	// Token: 0x04003833 RID: 14387
	private const float _baseArmRotWeight = 0.5f;

	// Token: 0x04003834 RID: 14388
	private const float _baseLegPosWeight = 1f;

	// Token: 0x04003835 RID: 14389
	private const float _baseLegRotWeight = 1f;

	// Token: 0x04003836 RID: 14390
	private float _headPosWeight;

	// Token: 0x04003837 RID: 14391
	private float _headRotWeight;

	// Token: 0x04003838 RID: 14392
	private float _lowerBodyWeight;

	// Token: 0x04003839 RID: 14393
	private float _baseSpinePosWeight = 1f;

	// Token: 0x0400383A RID: 14394
	private float _baseSpineRotWeight = 1f;

	// Token: 0x0400383B RID: 14395
	private float _savedWeight;

	// Token: 0x0400383C RID: 14396
	private bool _lastSeated;

	// Token: 0x0400383D RID: 14397
	private float _timeSeated;

	// Token: 0x0400383E RID: 14398
	private RuntimeAnimatorController _seatedRAC;

	// Token: 0x0400383F RID: 14399
	private bool _needReset;

	// Token: 0x04003840 RID: 14400
	private const float SEATING_FREEZE_TIME = 0.5f;

	// Token: 0x04003841 RID: 14401
	private const float SEATING_BLEND_TIME = 0.5f;

	// Token: 0x04003842 RID: 14402
	private const float MAINTAIN_PELVIS_WT = 0.05f;

	// Token: 0x04003843 RID: 14403
	private const float PELVIS_POS_WT_SEATED = 1f;

	// Token: 0x04003844 RID: 14404
	private const float PELVIS_ROT_WT_SEATED = 0.5f;

	// Token: 0x04003845 RID: 14405
	private const float PELVIS_POS_WT_LOWERBODY_TRACKED = 0.8f;

	// Token: 0x04003846 RID: 14406
	private const float PELVIS_ROT_WT_LOWERBODY_TRACKED = 1f;

	// Token: 0x04003847 RID: 14407
	private const float BEND_GOAL_WT_LOWERBODY_TRACKED = 0.75f;

	// Token: 0x04003848 RID: 14408
	private const float FOOT_ROT_WT_LOWERBODY_TRACKED = 0.5f;

	// Token: 0x04003849 RID: 14409
	private Vector3 _lastSeatedPos;

	// Token: 0x0400384A RID: 14410
	private Vector3 _animHeadPos;

	// Token: 0x0400384B RID: 14411
	private VRCInput _inGrabL;

	// Token: 0x0400384C RID: 14412
	private VRCInput _inGrabR;

	// Token: 0x0400384D RID: 14413
	private bool _isIkEnabled;

	// Token: 0x0400384E RID: 14414
	private Vector3 lThumbDir = Vector3.zero;

	// Token: 0x0400384F RID: 14415
	private Vector3 rThumbDir = Vector3.zero;
}
