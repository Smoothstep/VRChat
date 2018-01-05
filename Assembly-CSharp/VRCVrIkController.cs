using System;
using System.Collections;
using RootMotion.FinalIK;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000A4F RID: 2639
public class VRCVrIkController : MonoBehaviour, IVRCTrackedIk
{
	// Token: 0x17000BD2 RID: 3026
	// (get) Token: 0x06004FB1 RID: 20401 RVA: 0x001B11FB File Offset: 0x001AF5FB
	// (set) Token: 0x06004FB2 RID: 20402 RVA: 0x001B1204 File Offset: 0x001AF604
	public bool enableIk
	{
		get
		{
			return this._isIkEnabled;
		}
		set
		{
			this._isIkEnabled = value;
			if (this._vrik != null)
			{
				this._vrik.enabled = value;
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

	// Token: 0x17000BD3 RID: 3027
	// (get) Token: 0x06004FB3 RID: 20403 RVA: 0x001B126F File Offset: 0x001AF66F
	// (set) Token: 0x06004FB4 RID: 20404 RVA: 0x001B1277 File Offset: 0x001AF677
	public bool hasLowerBodyTracking
	{
		get
		{
			return this._enableLowerBodyTracking;
		}
		set
		{
			if (this._calibrated)
			{
				this._enableLowerBodyTracking = value;
				this.Reset(true);
				this.CheckPoseChange(true);
			}
		}
	}

	// Token: 0x17000BD4 RID: 3028
	// (get) Token: 0x06004FB5 RID: 20405 RVA: 0x001B1299 File Offset: 0x001AF699
	public bool isCalibrated
	{
		get
		{
			return this._calibrated;
		}
	}

	// Token: 0x17000BD5 RID: 3029
	// (get) Token: 0x06004FB6 RID: 20406 RVA: 0x001B12A1 File Offset: 0x001AF6A1
	// (set) Token: 0x06004FB7 RID: 20407 RVA: 0x001B12A9 File Offset: 0x001AF6A9
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

	// Token: 0x06004FB8 RID: 20408 RVA: 0x001B12B4 File Offset: 0x001AF6B4
	private void SyncLowerBodyTargetsWithSkeleton()
	{
		Transform boneTransform = this._anim.GetBoneTransform(HumanBodyBones.Hips);
		if (boneTransform != null && this._hipTarget != null)
		{
			this._hipTarget.position = boneTransform.position;
			this._hipTarget.rotation = boneTransform.rotation;
		}
		if (this._hasToes)
		{
			if (this._leftFootTarget != null && this._anim.GetBoneTransform(HumanBodyBones.LeftToes) != null)
			{
				this._leftFootTarget.position = this._anim.GetBoneTransform(HumanBodyBones.LeftToes).position;
			}
			if (this._rightFootTarget != null && this._anim.GetBoneTransform(HumanBodyBones.RightToes) != null)
			{
				this._rightFootTarget.position = this._anim.GetBoneTransform(HumanBodyBones.RightToes).position;
			}
		}
		else
		{
			if (this._leftFootTarget != null && this._anim.GetBoneTransform(HumanBodyBones.LeftFoot) != null)
			{
				this._leftFootTarget.position = this._anim.GetBoneTransform(HumanBodyBones.LeftFoot).position;
			}
			if (this._rightFootTarget != null && this._anim.GetBoneTransform(HumanBodyBones.RightFoot) != null)
			{
				this._rightFootTarget.position = this._anim.GetBoneTransform(HumanBodyBones.RightFoot).position;
			}
		}
	}

	// Token: 0x06004FB9 RID: 20409 RVA: 0x001B1434 File Offset: 0x001AF834
	private IEnumerator ResetHipWithFrameDelay(bool init, int frames)
	{
		for (int frameCount = frames; frameCount > 0; frameCount--)
		{
			yield return null;
		}
		if (this._playerT != null && this._anim != null)
		{
			Vector3 position = this._anim.GetBoneTransform(HumanBodyBones.Hips).position;
			Quaternion rotation = this._anim.GetBoneTransform(HumanBodyBones.Hips).rotation;
			this._pseudoHipPosGoal = this._playerT.InverseTransformPoint(position);
			this._pseudoHipRotGoal = rotation;
			if (init)
			{
				this._pseudoHipPos = this._pseudoHipPosGoal;
				this._pseudoHipRot = this._pseudoHipRotGoal;
			}
		}
		yield break;
	}

	// Token: 0x06004FBA RID: 20410 RVA: 0x001B145D File Offset: 0x001AF85D
	private void ResetPseudoHipDelayed(bool init = false, int afterFrameWait = 0)
	{
		base.StartCoroutine(this.ResetHipWithFrameDelay(init, afterFrameWait));
	}

	// Token: 0x06004FBB RID: 20411 RVA: 0x001B1470 File Offset: 0x001AF870
	private void ResetPseudoHip(bool init = false)
	{
		Vector3 position = this._anim.GetBoneTransform(HumanBodyBones.Hips).position;
		Quaternion rotation = this._anim.GetBoneTransform(HumanBodyBones.Hips).rotation;
		this._pseudoHipPosGoal = this._playerT.InverseTransformPoint(position);
		this._pseudoHipRotGoal = rotation;
		if (init)
		{
			this._pseudoHipPos = this._pseudoHipPosGoal;
			this._pseudoHipRot = this._pseudoHipRotGoal;
		}
	}

	// Token: 0x06004FBC RID: 20412 RVA: 0x001B14D8 File Offset: 0x001AF8D8
	private void UpdatePseudoHip()
	{
		if (!this._enableLowerBodyTracking && !this._motion.IsSeated && !this._motion.isLocomoting)
		{
			this._hipTarget.position = this._playerT.TransformPoint(this._pseudoHipPos);
			this._hipTarget.rotation = this._pseudoHipRot;
			this.ResetPseudoHip(false);
			this._pseudoHipPos = Vector3.Lerp(this._pseudoHipPos, this._pseudoHipPosGoal, Time.deltaTime * 6f);
			float num = Quaternion.Angle(this._pseudoHipRot, this._pseudoHipRotGoal);
			if (num > 15f)
			{
				this._pseudoHipRot = Quaternion.Slerp(this._pseudoHipRot, this._pseudoHipRotGoal, Time.deltaTime * (num - 25f) * 0.1f);
			}
		}
	}

	// Token: 0x06004FBD RID: 20413 RVA: 0x001B15B0 File Offset: 0x001AF9B0
	private void CheckPoseChange(bool force = false)
	{
		if (this._vrik == null)
		{
			return;
		}
		if (this._vrik.solver == null)
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
				this._framesSeated = 0;
			}
			if (this._seatedRAC != this._anim.runtimeAnimatorController)
			{
				flag2 = true;
				this._framesSeated = 0;
			}
		}
		int num = 10;
		int num2 = 10;
		int num3 = num + num2;
		if (isSeated != this._lastSeated || flag || flag2 || (this._framesSeated > 0 && this._framesSeated < num3))
		{
			if (isSeated)
			{
				this._anim.transform.localPosition = Vector3.zero;
				this._anim.transform.localRotation = Quaternion.identity;
				if (this._framesSeated < num)
				{
					this._seatedRAC = this._anim.runtimeAnimatorController;
					this._anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
					this.enableIk = false;
					this.SolverWeight = 0f;
					this._anim.Update(0f);
					this.SyncLowerBodyTargetsWithSkeleton();
					if (!this._enableLowerBodyTracking)
					{
						this.ResetPseudoHip(true);
					}
				}
				else
				{
					if (!this._local)
					{
						this._anim.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
					}
					this.SolverWeight = (float)(this._framesSeated - num) / (float)num2;
					this.enableIk = true;
					this._vrik.solver.spine.pelvisTarget = this._hipTarget;
					this._vrik.solver.leftLeg.target = this._leftFootTarget;
					this._vrik.solver.rightLeg.target = this._rightFootTarget;
					this._vrik.solver.leftLeg.bendGoal = null;
					this._vrik.solver.rightLeg.bendGoal = null;
				}
				this._vrik.solver.spine.pelvisPositionWeight = 1f;
				this._vrik.solver.spine.pelvisRotationWeight = 0.1f;
				this._vrik.solver.spine.positionWeight = 0.8f;
				this._vrik.solver.spine.rotationWeight = 1f;
				this._vrik.solver.spine.maintainPelvisPosition = 1f;
				this._vrik.solver.spine.bodyRotStiffness = 0.4f;
				this._vrik.solver.spine.neckStiffness = 0.5f;
				this._vrik.solver.spine.chestClampWeight = 0f;
				this._vrik.solver.spine.headClampWeight = 0f;
				this._vrik.solver.leftLeg.positionWeight = 1f;
				this._vrik.solver.rightLeg.positionWeight = 1f;
				this._vrik.solver.leftLeg.rotationWeight = 0f;
				this._vrik.solver.rightLeg.rotationWeight = 0f;
				this._vrik.solver.leftLeg.bendGoalWeight = 0f;
				this._vrik.solver.rightLeg.bendGoalWeight = 0f;
				this._vrik.solver.spine.maxRootAngle = 180f;
				this.HeadPosWeight = 0f;
				this.HeadRotWeight = 0f;
				this.SetAutoFootstep(false);
				this._lastSeatedPos = preIkHipPos;
				this._framesSeated++;
			}
			else
			{
				this._framesSeated = 0;
				this._seatedRAC = null;
				this.enableIk = true;
				this._anim.transform.localPosition = Vector3.zero;
				this._anim.transform.localRotation = Quaternion.identity;
				this._anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
				this._anim.Update(0f);
				if (!this._local)
				{
					this._anim.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
				}
				this._vrik.solver.spine.positionWeight = 1f;
				this._vrik.solver.spine.rotationWeight = 1f;
				this._vrik.solver.spine.bodyRotStiffness = 0.1f;
				this._vrik.solver.spine.neckStiffness = 0.2f;
				this._vrik.solver.spine.chestClampWeight = 0.5f;
				this._vrik.solver.spine.headClampWeight = 0.6f;
				this._vrik.solver.leftLeg.target = null;
				this._vrik.solver.rightLeg.target = null;
				this._vrik.solver.leftLeg.bendGoal = null;
				this._vrik.solver.rightLeg.bendGoal = null;
				this._vrik.solver.leftLeg.positionWeight = 0f;
				this._vrik.solver.rightLeg.positionWeight = 0f;
				this._vrik.solver.leftLeg.rotationWeight = 0f;
				this._vrik.solver.rightLeg.rotationWeight = 0f;
				this._vrik.solver.leftLeg.bendGoalWeight = 0f;
				this._vrik.solver.rightLeg.bendGoalWeight = 0f;
				this._vrik.solver.spine.maxRootAngle = 150f;
				this.SetAutoFootstep(!this._motion.isLocomoting);
				if (this._enableLowerBodyTracking)
				{
					this._vrik.solver.spine.pelvisTarget = this._hipTarget;
					this._vrik.solver.spine.pelvisPositionWeight = 0.5f;
					this._vrik.solver.spine.pelvisRotationWeight = 0.8f;
					this._vrik.solver.spine.maintainPelvisPosition = 0f;
				}
				else
				{
					this.ResetPseudoHip(true);
					this._vrik.solver.spine.pelvisTarget = this._hipTarget;
					if (this._player.GetVRMode())
					{
						this._vrik.solver.spine.pelvisPositionWeight = 0f;
						this._vrik.solver.spine.pelvisRotationWeight = 0.8f;
					}
					else
					{
						this._vrik.solver.spine.pelvisPositionWeight = 0.5f;
						this._vrik.solver.spine.pelvisRotationWeight = 0.9f;
					}
					this._vrik.solver.spine.maintainPelvisPosition = 0.05f;
				}
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

	// Token: 0x06004FBE RID: 20414 RVA: 0x001B1DAC File Offset: 0x001B01AC
	private void SetAutoFootstep(bool doAutoStep)
	{
		if (doAutoStep)
		{
			this.autoFootWtGoal = 1f;
			this.autoFootResetOnce = true;
		}
		else
		{
			this.autoFootWtGoal = 0f;
			this._vrik.solver.locomotion.weight = 0f;
		}
	}

	// Token: 0x06004FBF RID: 20415 RVA: 0x001B1DFC File Offset: 0x001B01FC
	private void UpdateAutoFootstep()
	{
		if (this._ik == null)
		{
			return;
		}
		if (!this._inited)
		{
			return;
		}
		if (this._ik.GetLocomotionLayerWeight() < 0.01f && this.autoFootWtGoal == 1f)
		{
			float num = this._vrik.solver.locomotion.weight;
			num = Mathf.Lerp(num, this.autoFootWtGoal, Time.deltaTime * 5f);
			if (this.autoFootResetOnce)
			{
				this._vrik.solver.Reset();
				this.autoFootResetOnce = false;
			}
			this._vrik.solver.locomotion.weight = num;
		}
	}

	// Token: 0x06004FC0 RID: 20416 RVA: 0x001B1EB2 File Offset: 0x001B02B2
	public void SeatedChange(bool sitting)
	{
		if (sitting)
		{
			this.SyncLowerBodyTargetsWithSkeleton();
		}
	}

	// Token: 0x17000BD6 RID: 3030
	// (get) Token: 0x06004FC1 RID: 20417 RVA: 0x001B1EC0 File Offset: 0x001B02C0
	// (set) Token: 0x06004FC2 RID: 20418 RVA: 0x001B1EE0 File Offset: 0x001B02E0
	public float LeftHandWeight
	{
		get
		{
			return this._vrik.solver.leftArm.positionWeight / 1f;
		}
		set
		{
			if (!this._ready)
			{
				return;
			}
			float num = Mathf.Clamp01(value);
			this._vrik.solver.leftArm.positionWeight = num * 1f;
			this._vrik.solver.leftArm.rotationWeight = num * 0.5f;
		}
	}

	// Token: 0x17000BD7 RID: 3031
	// (get) Token: 0x06004FC3 RID: 20419 RVA: 0x001B1F38 File Offset: 0x001B0338
	// (set) Token: 0x06004FC4 RID: 20420 RVA: 0x001B1F58 File Offset: 0x001B0358
	public float RightHandWeight
	{
		get
		{
			return this._vrik.solver.rightArm.positionWeight / 1f;
		}
		set
		{
			if (!this._ready)
			{
				return;
			}
			float num = Mathf.Clamp01(value);
			this._vrik.solver.rightArm.positionWeight = num * 1f;
			this._vrik.solver.rightArm.rotationWeight = num * 0.5f;
		}
	}

	// Token: 0x17000BD8 RID: 3032
	// (get) Token: 0x06004FC5 RID: 20421 RVA: 0x001B1FB0 File Offset: 0x001B03B0
	// (set) Token: 0x06004FC6 RID: 20422 RVA: 0x001B1FB8 File Offset: 0x001B03B8
	public float HeadPosWeight
	{
		get
		{
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
			this._vrik.solver.spine.positionWeight = num * this._baseSpinePosWeight;
		}
	}

	// Token: 0x17000BD9 RID: 3033
	// (get) Token: 0x06004FC7 RID: 20423 RVA: 0x001B1FFC File Offset: 0x001B03FC
	// (set) Token: 0x06004FC8 RID: 20424 RVA: 0x001B2004 File Offset: 0x001B0404
	public float HeadRotWeight
	{
		get
		{
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
			this._vrik.solver.spine.rotationWeight = num * this._baseSpineRotWeight;
		}
	}

	// Token: 0x17000BDA RID: 3034
	// (get) Token: 0x06004FC9 RID: 20425 RVA: 0x001B2048 File Offset: 0x001B0448
	// (set) Token: 0x06004FCA RID: 20426 RVA: 0x001B2050 File Offset: 0x001B0450
	public float LowerBodyWeight
	{
		get
		{
			return this._lowerBodyWeight;
		}
		set
		{
			if (!this._ready)
			{
				return;
			}
			float num = Mathf.Clamp01(value);
			if (num != this._lowerBodyWeight)
			{
				this._lowerBodyWeight = num;
				if (this._enableLowerBodyTracking)
				{
					this._vrik.solver.spine.pelvisPositionWeight = 0.5f * num;
					this._vrik.solver.spine.pelvisRotationWeight = 0.8f * num;
					this._vrik.solver.spine.maintainPelvisPosition = 0f;
				}
				else
				{
					if (this._player.GetVRMode())
					{
						this._vrik.solver.spine.pelvisPositionWeight = 0f * num;
						this._vrik.solver.spine.pelvisRotationWeight = 0.8f * num;
					}
					else
					{
						this._vrik.solver.spine.pelvisPositionWeight = 0.5f * num;
						this._vrik.solver.spine.pelvisRotationWeight = 0.9f * num;
					}
					this._vrik.solver.spine.maintainPelvisPosition = 0.05f;
				}
			}
		}
	}

	// Token: 0x17000BDB RID: 3035
	// (get) Token: 0x06004FCB RID: 20427 RVA: 0x001B2184 File Offset: 0x001B0584
	// (set) Token: 0x06004FCC RID: 20428 RVA: 0x001B2196 File Offset: 0x001B0596
	public float SolverWeight
	{
		get
		{
			return this._vrik.solver.IKPositionWeight;
		}
		set
		{
			if (value != this._vrik.solver.IKPositionWeight)
			{
				this._vrik.solver.IKPositionWeight = value;
			}
		}
	}

	// Token: 0x06004FCD RID: 20429 RVA: 0x001B21C0 File Offset: 0x001B05C0
	public void LocomotionChange(bool loco)
	{
		if (loco)
		{
			this._vrik.solver.spine.maxRootAngle = 0f;
			this.SetAutoFootstep(false);
		}
		else
		{
			this._vrik.solver.Reset();
			this.SetAutoFootstep(true);
			this.ResetPseudoHip(true);
		}
	}

	// Token: 0x06004FCE RID: 20430 RVA: 0x001B2218 File Offset: 0x001B0618
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

	// Token: 0x06004FCF RID: 20431 RVA: 0x001B229C File Offset: 0x001B069C
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

	// Token: 0x06004FD0 RID: 20432 RVA: 0x001B234C File Offset: 0x001B074C
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

	// Token: 0x06004FD1 RID: 20433 RVA: 0x001B23E8 File Offset: 0x001B07E8
	public bool Initialize(VRC_AnimationController animControl, Animator anim, VRCPlayer player, bool local)
	{
		if (this._inited)
		{
			this.Uninitialize();
		}
		this._frameSinceInit = 0;
		this._framesSeated = 0;
		this._enableLowerBodyTracking = false;
		this._animCtl = animControl;
		this._animMgr = this._animCtl.GetComponent<AnimatorControllerManager>();
		this._anim = anim;
		this._player = player;
		this._playerT = player.transform;
		this._avatarMgr = this._player.GetComponentInChildren<VRCAvatarManager>();
		this._motion = this._playerT.GetComponent<VRCMotionState>();
		this._local = local;
		this._vrik = this._anim.GetComponent<VRIK>();
		this._ik = base.GetComponent<IkController>();
		this._inGrabL = VRCInputManager.FindInput("GrabLeft");
		this._inGrabR = VRCInputManager.FindInput("GrabRight");
		if (this._local)
		{
			this._ik.HeadControl(true);
		}
		if (this._avatarMgr.CurrentAvatar == null)
		{
			if (this._avatarMgr.currentAvatarKind == VRCAvatarManager.AvatarKind.Custom)
			{
				Debug.LogError("IK[" + this._player.name + "] (3/4pt) init, but Loading was not finished!");
				this._isCustomAvatar = true;
			}
			else
			{
				Debug.Log(string.Concat(new object[]
				{
					"IK[",
					this._player.name,
					"] (3/4pt) (",
					this._avatarMgr.currentAvatarKind,
					")"
				}));
				this._isCustomAvatar = false;
			}
		}
		else if (this._avatarMgr.currentAvatarKind == VRCAvatarManager.AvatarKind.Custom)
		{
			Debug.Log("IK[" + this._player.name + "] (3/4pt) init avatar = " + this._avatarMgr.CurrentAvatar.id);
			this._isCustomAvatar = true;
		}
		else
		{
			Debug.Log(string.Concat(new object[]
			{
				"IK[",
				this._player.name,
				"] (3/4pt) (",
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
			Transform boneTransform = this._anim.GetBoneTransform(HumanBodyBones.Head);
			this._headTarget = this._ik.HeadEffector.transform.Find("HeadBoneOffset");
			Transform transform = this._ik.LeftEffector.transform;
			Transform transform2 = this._ik.RightEffector.transform;
			this._hipTarget = this._ik.HipEffector.transform;
			this._leftFootTarget = this._ik.LeftFootEffector.transform;
			this._rightFootTarget = this._ik.RightFootEffector.transform;
			this._poseHeadRot = Quaternion.Inverse(this._ik.HeadEffector.transform.rotation) * boneTransform.rotation;
			if (this._hipTarget == null || this._leftFootTarget == null || this._rightFootTarget == null)
			{
				Debug.LogError("Hip and/or Foot IK Targets are null");
			}
			if (this._leftKneeTarget == null || this._rightKneeTarget == null)
			{
				Debug.LogError("Knee bend IK Targets are null");
			}
			this._vrik.fixTransforms = true;
			this._vrik.solver.spine.pelvisTarget = null;
			this._vrik.solver.leftLeg.target = null;
			this._vrik.solver.rightLeg.target = null;
			this.CheckFingers();
			Transform boneTransform2 = this._anim.GetBoneTransform(HumanBodyBones.LeftHand);
			Transform boneTransform3 = this._anim.GetBoneTransform(HumanBodyBones.RightHand);
			this._poseWristRotL = Quaternion.Inverse(this._anim.transform.rotation) * boneTransform2.rotation;
			this._poseWristRotR = Quaternion.Inverse(this._anim.transform.rotation) * boneTransform3.rotation;
			this.ResetPseudoHip(true);
			this._vrik.solver.spine.positionWeight = 0f;
			this._vrik.solver.spine.rotationWeight = 0f;
			this._vrik.solver.spine.pelvisPositionWeight = 0f;
			this._vrik.solver.spine.chestGoalWeight = 0.01f;
			this._vrik.solver.spine.bodyPosStiffness = 0.55f;
			this._vrik.solver.spine.bodyRotStiffness = 0.2f;
			this._vrik.solver.spine.neckStiffness = 0.2f;
			this._vrik.solver.spine.chestClampWeight = 0.5f;
			this._vrik.solver.spine.headClampWeight = 0.6f;
			this._vrik.solver.spine.maintainPelvisPosition = 0.05f;
			this._vrik.solver.spine.maxRootAngle = 180f;
			this._vrik.solver.spine.headTarget = this._headTarget;
			this._vrik.solver.spine.minHeadHeight = 1f;
			this._vrik.solver.leftArm.positionWeight = 0f;
			this._vrik.solver.leftArm.rotationWeight = 0f;
			this._vrik.solver.leftArm.shoulderRotationMode = IKSolverVR.Arm.ShoulderRotationMode.FromTo;
			this._vrik.solver.leftArm.shoulderRotationWeight = 0.8f;
			this._vrik.solver.leftArm.swivelOffset = 0f;
			this._vrik.solver.leftArm.target = transform;
			this._vrik.solver.rightArm.positionWeight = 0f;
			this._vrik.solver.rightArm.rotationWeight = 0f;
			this._vrik.solver.rightArm.shoulderRotationMode = IKSolverVR.Arm.ShoulderRotationMode.FromTo;
			this._vrik.solver.rightArm.shoulderRotationWeight = 0.8f;
			this._vrik.solver.rightArm.swivelOffset = 0f;
			this._vrik.solver.rightArm.target = transform2;
			this._poseElbowBendDirL = boneTransform2.InverseTransformDirection(this._playerT.TransformDirection(Vector3.back));
			this._poseElbowBendDirR = boneTransform3.InverseTransformDirection(this._playerT.TransformDirection(Vector3.back));
			this._vrik.solver.leftArm.bendGoalWeight = 0.25f;
			this._vrik.solver.rightArm.bendGoalWeight = 0.25f;
			this.SetAutoFootstep(false);
			Transform boneTransform4 = this._anim.GetBoneTransform(HumanBodyBones.LeftToes);
			Transform boneTransform5 = this._anim.GetBoneTransform(HumanBodyBones.RightToes);
			this._hasToes = (boneTransform4 != null && boneTransform5 != null);
			this._vrik.solver.locomotion.footDistance = 0.2f;
			this._vrik.solver.locomotion.stepThreshold = 0.25f;
			this._avatarScale = 1f;
			this._measured = false;
			this._vrik.solver.locomotion.angleThreshold = 70f;
			this._vrik.solver.leftLeg.positionWeight = 0f;
			this._vrik.solver.rightLeg.positionWeight = 0f;
			this._vrik.solver.leftLeg.rotationWeight = 0f;
			this._vrik.solver.rightLeg.rotationWeight = 0f;
			this._vrik.solver.leftLeg.bendGoalWeight = 0f;
			this._vrik.solver.rightLeg.bendGoalWeight = 0f;
			this._vrik.AutoDetectReferences();
			this.CheckReferences();
			GameObject gameObject = this._vrik.references.leftForearm.gameObject;
			if (gameObject != null)
			{
				this._forearmLTwistRelaxer = gameObject.AddComponent<TwistRelaxer>();
				this._forearmLTwistRelaxer.weight = 0.5f;
				this._forearmLTwistRelaxer.parentChildCrossfade = 0.8f;
			}
			GameObject gameObject2 = this._vrik.references.rightForearm.gameObject;
			if (gameObject2 != null)
			{
				this._forearmRTwistRelaxer = gameObject2.AddComponent<TwistRelaxer>();
				this._forearmRTwistRelaxer.weight = 0.5f;
				this._forearmRTwistRelaxer.parentChildCrossfade = 0.8f;
			}
			this._vrik.solver.GuessHandOrientations(this._vrik.references, false);
		}
		catch (Exception ex)
		{
			if (!this._local)
			{
				this.RemoveTPose();
			}
			Debug.LogWarning("FinalIK Disabled because of setup error: " + ex.Message);
			return false;
		}
		IKSolverVR solver = this._vrik.solver;
		solver.OnPreUpdate = (IKSolver.UpdateDelegate)Delegate.Combine(solver.OnPreUpdate, new IKSolver.UpdateDelegate(this.OnPreUpdate));
		IKSolverVR solver2 = this._vrik.solver;
		solver2.OnPostUpdate = (IKSolver.UpdateDelegate)Delegate.Combine(solver2.OnPostUpdate, new IKSolver.UpdateDelegate(this.OnPostUpdate));
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
		this.SolverWeight = 0f;
		if (!this._local)
		{
			this.SyncLowerBodyTargetsWithSkeleton();
			this.RemoveTPose();
		}
		this.Reset(true);
		return true;
	}

	// Token: 0x06004FD2 RID: 20434 RVA: 0x001B2E5C File Offset: 0x001B125C
	private void ForceTPose()
	{
		this._animMgr.Push(this._animMgr.tPoseController);
		this._anim.updateMode = AnimatorUpdateMode.UnscaledTime;
		this._anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
		this._anim.Update(0.01f);
		this._anim.transform.localPosition = Vector3.zero;
		this._anim.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x06004FD3 RID: 20435 RVA: 0x001B2ED1 File Offset: 0x001B12D1
	private void RemoveTPose()
	{
		this._animMgr.Pop();
		this._anim.updateMode = AnimatorUpdateMode.Normal;
		this._anim.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
	}

	// Token: 0x06004FD4 RID: 20436 RVA: 0x001B2EF8 File Offset: 0x001B12F8
	private void ScaleSettings()
	{
		this._avatarScale = this._avatarMgr.GetAvatarArmLength() / VRCTracking.DefaultArmLength;
		if (this._avatarMgr.IsFemale())
		{
			this._vrik.solver.locomotion.footDistance = 0.12f * this._avatarScale;
			this._vrik.solver.locomotion.stepThreshold = 0.25f * this._avatarScale;
		}
		else
		{
			this._vrik.solver.locomotion.footDistance = 0.19f * this._avatarScale;
			this._vrik.solver.locomotion.stepThreshold = 0.28f * this._avatarScale;
		}
		this._vrik.solver.spine.minHeadHeight = this._avatarMgr.GetAvatarEyeHeight() / 4f;
	}

	// Token: 0x06004FD5 RID: 20437 RVA: 0x001B2FDC File Offset: 0x001B13DC
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

	// Token: 0x06004FD6 RID: 20438 RVA: 0x001B3030 File Offset: 0x001B1430
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

	// Token: 0x06004FD7 RID: 20439 RVA: 0x001B30C0 File Offset: 0x001B14C0
	private void CheckFingers()
	{
		if (this._anim != null && this._anim.GetBoneTransform(HumanBodyBones.LeftThumbProximal) != null && this._anim.GetBoneTransform(HumanBodyBones.LeftIndexProximal) != null && this._anim.GetBoneTransform(HumanBodyBones.LeftMiddleProximal) != null && this._anim.GetBoneTransform(HumanBodyBones.RightThumbProximal) != null && this._anim.GetBoneTransform(HumanBodyBones.RightIndexProximal) != null && this._anim.GetBoneTransform(HumanBodyBones.RightMiddleProximal) != null)
		{
			return;
		}
		throw new NotSupportedException("Avatar doesn't have enough fingers.");
	}

	// Token: 0x06004FD8 RID: 20440 RVA: 0x001B3179 File Offset: 0x001B1579
	private void OnDestroy()
	{
		this.Uninitialize();
	}

	// Token: 0x06004FD9 RID: 20441 RVA: 0x001B3184 File Offset: 0x001B1584
	public void Uninitialize()
	{
		if (this._inited)
		{
			this._inited = false;
			IKSolverVR solver = this._vrik.solver;
			solver.OnPreUpdate = (IKSolver.UpdateDelegate)Delegate.Remove(solver.OnPreUpdate, new IKSolver.UpdateDelegate(this.OnPreUpdate));
			IKSolverVR solver2 = this._vrik.solver;
			solver2.OnPostUpdate = (IKSolver.UpdateDelegate)Delegate.Remove(solver2.OnPostUpdate, new IKSolver.UpdateDelegate(this.OnPostUpdate));
			VRCUiManager.Instance.onUiEnabled -= this.OnUIEnabled;
			VRCUiManager.Instance.onUiDisabled -= this.OnUIDisabled;
			this._calibrated = false;
			this._ready = false;
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

	// Token: 0x06004FDA RID: 20442 RVA: 0x001B3274 File Offset: 0x001B1674
	public void NeedsReset()
	{
		this._needReset = true;
		this.enableIk = false;
		this.ResetPseudoHip(true);
	}

	// Token: 0x06004FDB RID: 20443 RVA: 0x001B328C File Offset: 0x001B168C
	public void Reset(bool restore = true)
	{
		if (!this._inited)
		{
			return;
		}
		if (this._vrik.solver.initiated && this._anim != null)
		{
			if (this._anim.transform != null)
			{
				if (this._anim.transform.parent != null)
				{
					this._anim.transform.parent.localPosition = Vector3.zero;
					this._anim.transform.parent.localRotation = Quaternion.identity;
				}
				this._anim.transform.localRotation = Quaternion.identity;
				this._anim.transform.localPosition = Vector3.zero;
			}
			this._anim.Update(0f);
			this._vrik.solver.Reset();
			if (restore)
			{
				this.enableIk = true;
			}
			this._needReset = false;
		}
	}

	// Token: 0x06004FDC RID: 20444 RVA: 0x001B338E File Offset: 0x001B178E
	private void ProneStart()
	{
		this.HeadPosWeight = 0f;
		this.SolverWeight = 0.755555f;
		this._crawling = true;
		this.SetAutoFootstep(false);
	}

	// Token: 0x06004FDD RID: 20445 RVA: 0x001B33B4 File Offset: 0x001B17B4
	private IEnumerator ProneEnd()
	{
		this.LeftHandWeight = 1f;
		this.RightHandWeight = 1f;
		this.HeadPosWeight = 1f;
		this.SetAutoFootstep(true);
		yield return null;
		this.SolverWeight = 0f;
		this._anim.Update(0f);
		yield return null;
		yield return null;
		this._vrik.solver.Reset();
		this._gettingUp = false;
		yield break;
	}

	// Token: 0x06004FDE RID: 20446 RVA: 0x001B33D0 File Offset: 0x001B17D0
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
		if (this._ready)
		{
			float standingHeight = this._motion.StandingHeight;
			if (!this._motion.IsSeated)
			{
				if (this._crawling && standingHeight > LocomotionInputController.ProneSpeedStart + 0.1f)
				{
					this._crawling = false;
					this._gettingUp = true;
					base.StartCoroutine(this.ProneEnd());
				}
				if (standingHeight < LocomotionInputController.ProneSpeedStart)
				{
					if (!this._gettingUp)
					{
						this.ProneStart();
						this._anim.SetFloat("HeightScaleNOMOVE", 0f);
					}
				}
				else if (standingHeight < LocomotionInputController.CrouchSpeedStart)
				{
					this._anim.SetFloat("HeightScaleNOMOVE", 0.5f);
				}
				else
				{
					this._anim.SetFloat("HeightScaleNOMOVE", 1f);
				}
			}
		}
		if (!this._motion.InVehicle)
		{
			float sqrMagnitude = (this._playerT.position - this._lastPos).sqrMagnitude;
			float num = Quaternion.Angle(this._playerT.rotation, this._lastRot);
			if (sqrMagnitude > 1f || num > 45f)
			{
				base.StartCoroutine(this.PlayerTeleportedCoroutine());
			}
		}
		this._lastPos = this._playerT.position;
		this._lastRot = this._playerT.rotation;
	}

	// Token: 0x06004FDF RID: 20447 RVA: 0x001B3568 File Offset: 0x001B1968
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
				if (this._vrik != null)
				{
					this._vrik.enabled = false;
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
			this._wasCulled = true;
			return;
		}
		if (this._vrik != null)
		{
			this._vrik.enabled = this.enableIk;
		}
		if (this._forearmLTwistRelaxer != null)
		{
			this._forearmLTwistRelaxer.enabled = this.enableIk;
		}
		if (this._forearmRTwistRelaxer != null)
		{
			this._forearmRTwistRelaxer.enabled = this.enableIk;
		}
		if (this._wasCulled)
		{
			this.LocomotionChange(this._motion.isLocomoting);
			this._wasCulled = false;
		}
		if (!this._measured && this._avatarMgr.WasMeasured)
		{
			this.ScaleSettings();
			this._measured = true;
		}
		if (this._needReset)
		{
			this.Reset(true);
		}
		if (this._frameSinceInit == 1)
		{
			this._vrik.solver.leftArm.wristToPalmAxis = Vector3.zero;
			this._vrik.solver.leftArm.palmToThumbAxis = Vector3.zero;
			this._vrik.solver.rightArm.wristToPalmAxis = Vector3.zero;
			this._vrik.solver.rightArm.palmToThumbAxis = Vector3.zero;
			if (this._local && this._isCustomAvatar)
			{
				this._enableLowerBodyTracking = VRCTrackingManager.CanSupportHipTracking();
				if (this._enableLowerBodyTracking)
				{
					VRCTrackingManager.SetSeatedPlayMode(false);
					Debug.LogWarning("Hip Tracking: Hip tracker found. tracking enabled.");
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
				Debug.LogWarning("Hip Tracking: in T-pose for calibration");
				this._anim.transform.localPosition = Vector3.zero;
				this._anim.transform.localRotation = Quaternion.identity;
				if (this._anim.transform.parent != null)
				{
					this._anim.transform.parent.localPosition = Vector3.zero;
					this._anim.transform.parent.localRotation = Quaternion.identity;
				}
			}
			else
			{
				this._animCtl.SetupHandGestures(this._anim, false);
				this._calibrated = true;
			}
		}
		if (!this._calibrated && this._local && this._enableLowerBodyTracking && this._inGrabR.button && this._inGrabL.button)
		{
			VRCTrackingManager.PerformCalibration(this._anim, true, false);
			VRCTrackingManager.RestoreTrackingAfterCalibration();
			this.enableIk = true;
			this._animMgr.Pop();
			this._animCtl.SetupHandGestures(this._anim, false);
			this.CheckPoseChange(true);
			this._inputState.PopInputController();
			this._frozenInput = false;
			this._calibrated = true;
			Debug.LogWarning("Hip Tracking: Calibrated");
		}
		if (this._ready)
		{
			this.CheckPoseChange(false);
		}
		this.UpdateAutoFootstep();
		if (!this._ready && this._calibrated)
		{
			this._ready = true;
			this.HeadPosWeight = 1f;
			this.HeadRotWeight = 1f;
			this.LeftHandWeight = 1f;
			this.RightHandWeight = 1f;
			this.CheckPoseChange(true);
		}
		if (this._frameSinceInit == 10 && this._frozenInput && this._calibrated)
		{
			this._inputState.PopInputController();
			this._frozenInput = false;
		}
		if (Networking.IsObjectReady(this._player.gameObject) && this._frameSinceInit < 300)
		{
			this._frameSinceInit++;
		}
	}

	// Token: 0x06004FE0 RID: 20448 RVA: 0x001B3A2C File Offset: 0x001B1E2C
	private void UpdateTargets()
	{
		if (!this._inited || this._culled)
		{
			return;
		}
		this.UpdatePseudoHip();
		if (this._enableLowerBodyTracking && this._calibrated && this._local && !this._motion.IsSeated && VRCTrackingManager.IsPlayerNearTracking() && VRCTrackingManager.IsTracked(VRCTracking.ID.BodyTracker_Hip))
		{
			Transform trackedTransform = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.BodyTracker_Hip);
			this._hipTarget.position = trackedTransform.position;
			this._hipTarget.rotation = trackedTransform.rotation;
		}
	}

	// Token: 0x06004FE1 RID: 20449 RVA: 0x001B3AC8 File Offset: 0x001B1EC8
	private void LimitHeadRotation()
	{
		Transform boneTransform = this._anim.GetBoneTransform(HumanBodyBones.Head);
		Transform transform = this._ik.HeadEffector.transform;
		Vector3 eulerAngles = transform.localRotation.eulerAngles;
		Vector3 euler = eulerAngles;
		if (eulerAngles.x > 180f)
		{
			eulerAngles.x -= 360f;
		}
		euler.x = Mathf.Clamp(eulerAngles.x, -45f, 40f);
		boneTransform.rotation = transform.parent.rotation * Quaternion.Euler(euler) * this._poseHeadRot;
	}

	// Token: 0x06004FE2 RID: 20450 RVA: 0x001B3B70 File Offset: 0x001B1F70
	private void LateUpdate()
	{
		if (!this._inited)
		{
			return;
		}
		if (this._culled)
		{
			return;
		}
		if (this._player == null || this._anim == null || this._vrik == null)
		{
			return;
		}
		if (this._motion.isGrounded)
		{
			this._vrik.solver.plantFeet = true;
		}
		else
		{
			this._vrik.solver.plantFeet = false;
		}
		this.UpdateTargets();
	}

	// Token: 0x06004FE3 RID: 20451 RVA: 0x001B3C08 File Offset: 0x001B2008
	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(this.PostIkLeftHandPos, this.PostIkLeftHandPos + 0.1f * this.lThumbDir);
		Gizmos.DrawLine(this.PostIkRightHandPos, this.PostIkRightHandPos + 0.1f * this.rThumbDir);
	}

	// Token: 0x06004FE4 RID: 20452 RVA: 0x001B3C64 File Offset: 0x001B2064
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

	// Token: 0x06004FE5 RID: 20453 RVA: 0x001B3C80 File Offset: 0x001B2080
	private IEnumerator PlayerTeleportedCoroutine()
	{
		this.enableIk = false;
		this._anim.transform.localPosition = Vector3.zero;
		this._vrik.solver.Reset();
		yield return null;
		this._anim.transform.localPosition = Vector3.zero;
		this._vrik.solver.Reset();
		yield return null;
		this._anim.transform.localPosition = Vector3.zero;
		this._vrik.solver.Reset();
		yield return null;
		this._anim.transform.localPosition = Vector3.zero;
		this._vrik.solver.Reset();
		yield return null;
		this._anim.transform.localPosition = Vector3.zero;
		this.enableIk = true;
		yield return null;
		this._vrik.solver.Reset();
		yield break;
	}

	// Token: 0x06004FE6 RID: 20454 RVA: 0x001B3C9B File Offset: 0x001B209B
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

	// Token: 0x06004FE7 RID: 20455 RVA: 0x001B3CC8 File Offset: 0x001B20C8
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
		if (this._anim != null && this._anim.transform != null)
		{
			this._anim.transform.SetParent(this._forwardT, true);
		}
		this._forwardT = null;
		this.NeedsReset();
	}

	// Token: 0x06004FE8 RID: 20456 RVA: 0x001B3D40 File Offset: 0x001B2140
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
			if (this._vrik != null && this._vrik.solver != null && this._vrik.solver.IKPositionWeight > 0f && this._vrik.solver.leftArm != null && this._vrik.solver.leftArm.target != null && this._vrik.solver.rightArm != null && this._vrik.solver.rightArm.target != null)
			{
				Quaternion rotation = this._vrik.solver.leftArm.target.rotation;
				Quaternion rotation2 = this._vrik.solver.rightArm.target.rotation;
				if (this.LeftHandWeight > 0f)
				{
					boneTransform.rotation = rotation * this.LeftWristAlign * this._poseWristRotL;
					Vector3 b = boneTransform.TransformDirection(this._poseElbowBendDirL);
					this._vrik.solver.leftArm.bendDirection = Vector3.Lerp(this._vrik.solver.leftArm.bendDirection, b, Time.deltaTime * 10f);
				}
				if (this.RightHandWeight > 0f)
				{
					boneTransform2.rotation = rotation2 * this.RightWristAlign * this._poseWristRotR;
					Vector3 b2 = boneTransform2.TransformDirection(this._poseElbowBendDirR);
					this._vrik.solver.rightArm.bendDirection = Vector3.Lerp(this._vrik.solver.rightArm.bendDirection, b2, Time.deltaTime * 10f);
				}
				if (!this._player.GetVRMode())
				{
					this.LimitHeadRotation();
				}
			}
		}
	}

	// Token: 0x06004FE9 RID: 20457 RVA: 0x001B3FA5 File Offset: 0x001B23A5
	private void OnEnable()
	{
		this.enableIk = true;
	}

	// Token: 0x06004FEA RID: 20458 RVA: 0x001B3FAE File Offset: 0x001B23AE
	private void OnDisable()
	{
		this.enableIk = false;
	}

	// Token: 0x04003858 RID: 14424
	public RuntimeAnimatorController ikPoseController;

	// Token: 0x04003859 RID: 14425
	public Vector3 PostIkHeadPos;

	// Token: 0x0400385A RID: 14426
	public Vector3 PostIkLeftHandPos;

	// Token: 0x0400385B RID: 14427
	public Vector3 PostIkRightHandPos;

	// Token: 0x0400385C RID: 14428
	private Vector3 PreIkHipPos;

	// Token: 0x0400385D RID: 14429
	private bool _inited;

	// Token: 0x0400385E RID: 14430
	private bool _culled;

	// Token: 0x0400385F RID: 14431
	private bool _wasCulled;

	// Token: 0x04003860 RID: 14432
	private int _frameSinceInit;

	// Token: 0x04003861 RID: 14433
	private bool _local;

	// Token: 0x04003862 RID: 14434
	private bool _calibrated;

	// Token: 0x04003863 RID: 14435
	private bool _frozenInput;

	// Token: 0x04003864 RID: 14436
	private bool _ready;

	// Token: 0x04003865 RID: 14437
	private bool _enableLowerBodyTracking;

	// Token: 0x04003866 RID: 14438
	private bool _isCustomAvatar;

	// Token: 0x04003867 RID: 14439
	private bool _measured;

	// Token: 0x04003868 RID: 14440
	private VRCPlayer _player;

	// Token: 0x04003869 RID: 14441
	private Transform _playerT;

	// Token: 0x0400386A RID: 14442
	private Transform _forwardT;

	// Token: 0x0400386B RID: 14443
	private Animator _anim;

	// Token: 0x0400386C RID: 14444
	private VRC_AnimationController _animCtl;

	// Token: 0x0400386D RID: 14445
	private AnimatorControllerManager _animMgr;

	// Token: 0x0400386E RID: 14446
	private VRCAvatarManager _avatarMgr;

	// Token: 0x0400386F RID: 14447
	private VRIK _vrik;

	// Token: 0x04003870 RID: 14448
	private IkController _ik;

	// Token: 0x04003871 RID: 14449
	private VRCMotionState _motion;

	// Token: 0x04003872 RID: 14450
	private InputStateControllerManager _inputState;

	// Token: 0x04003873 RID: 14451
	private Quaternion LeftWristAlign = Quaternion.Euler(new Vector3(0f, 90f, 0f));

	// Token: 0x04003874 RID: 14452
	private Quaternion RightWristAlign = Quaternion.Euler(new Vector3(0f, -90f, 0f));

	// Token: 0x04003875 RID: 14453
	private Quaternion _poseWristRotL;

	// Token: 0x04003876 RID: 14454
	private Quaternion _poseWristRotR;

	// Token: 0x04003877 RID: 14455
	private Vector3 _poseElbowBendDirL;

	// Token: 0x04003878 RID: 14456
	private Vector3 _poseElbowBendDirR;

	// Token: 0x04003879 RID: 14457
	private TwistRelaxer _forearmLTwistRelaxer;

	// Token: 0x0400387A RID: 14458
	private TwistRelaxer _forearmRTwistRelaxer;

	// Token: 0x0400387B RID: 14459
	private Vector3 _lastPos;

	// Token: 0x0400387C RID: 14460
	private Quaternion _lastRot;

	// Token: 0x0400387D RID: 14461
	private Transform _hipTarget;

	// Token: 0x0400387E RID: 14462
	private Transform _leftFootTarget;

	// Token: 0x0400387F RID: 14463
	private Transform _rightFootTarget;

	// Token: 0x04003880 RID: 14464
	public Transform _leftKneeTarget;

	// Token: 0x04003881 RID: 14465
	public Transform _rightKneeTarget;

	// Token: 0x04003882 RID: 14466
	private bool _hasToes;

	// Token: 0x04003883 RID: 14467
	private Transform _headTarget;

	// Token: 0x04003884 RID: 14468
	private Quaternion _poseHeadRot;

	// Token: 0x04003885 RID: 14469
	private Vector3 _pseudoHipPos;

	// Token: 0x04003886 RID: 14470
	private Vector3 _pseudoHipPosGoal;

	// Token: 0x04003887 RID: 14471
	private Quaternion _pseudoHipRot;

	// Token: 0x04003888 RID: 14472
	private Quaternion _pseudoHipRotGoal;

	// Token: 0x04003889 RID: 14473
	private Quaternion _pseudoHipLastRot;

	// Token: 0x0400388A RID: 14474
	private float _avatarScale = 1f;

	// Token: 0x0400388B RID: 14475
	private const float _baseArmPosWeight = 1f;

	// Token: 0x0400388C RID: 14476
	private const float _baseArmRotWeight = 0.5f;

	// Token: 0x0400388D RID: 14477
	private const float _baseLegPosWeight = 1f;

	// Token: 0x0400388E RID: 14478
	private const float _baseLegRotWeight = 1f;

	// Token: 0x0400388F RID: 14479
	private float _headPosWeight;

	// Token: 0x04003890 RID: 14480
	private float _headRotWeight;

	// Token: 0x04003891 RID: 14481
	private float _lowerBodyWeight;

	// Token: 0x04003892 RID: 14482
	private float _baseSpinePosWeight = 1f;

	// Token: 0x04003893 RID: 14483
	private float _baseSpineRotWeight = 1f;

	// Token: 0x04003894 RID: 14484
	private float _savedWeight;

	// Token: 0x04003895 RID: 14485
	private bool _lastSeated;

	// Token: 0x04003896 RID: 14486
	private int _framesSeated;

	// Token: 0x04003897 RID: 14487
	private bool _crawling;

	// Token: 0x04003898 RID: 14488
	private bool _gettingUp;

	// Token: 0x04003899 RID: 14489
	private bool _needReset;

	// Token: 0x0400389A RID: 14490
	private const int SEATING_FREEZE_TIME = 10;

	// Token: 0x0400389B RID: 14491
	private const int SEATING_BLEND_TIME = 10;

	// Token: 0x0400389C RID: 14492
	private const float MAINTAIN_PELVIS_WT = 0.05f;

	// Token: 0x0400389D RID: 14493
	private const float PELVIS_POS_WT_SEATED = 1f;

	// Token: 0x0400389E RID: 14494
	private const float PELVIS_ROT_WT_SEATED = 0.1f;

	// Token: 0x0400389F RID: 14495
	private const float PELVIS_POS_WT_UNTRACKED = 0f;

	// Token: 0x040038A0 RID: 14496
	private const float PELVIS_ROT_WT_UNTRACKED = 0.8f;

	// Token: 0x040038A1 RID: 14497
	private const float PELVIS_POS_WT_DESKTOP = 0.5f;

	// Token: 0x040038A2 RID: 14498
	private const float PELVIS_ROT_WT_DESKTOP = 0.9f;

	// Token: 0x040038A3 RID: 14499
	private const float PELVIS_POS_WT_LOWERBODY_TRACKED = 0.5f;

	// Token: 0x040038A4 RID: 14500
	private const float PELVIS_ROT_WT_LOWERBODY_TRACKED = 0.8f;

	// Token: 0x040038A5 RID: 14501
	private const float BEND_GOAL_WT_LOWERBODY_TRACKED = 0.75f;

	// Token: 0x040038A6 RID: 14502
	private const float FOOT_ROT_WT_LOWERBODY_TRACKED = 0.5f;

	// Token: 0x040038A7 RID: 14503
	private const float HEAD_UP_LIMIT_DESKTOP = -45f;

	// Token: 0x040038A8 RID: 14504
	private const float HEAD_DOWN_LIMIT_DESKTOP = 40f;

	// Token: 0x040038A9 RID: 14505
	private Vector3 _lastSeatedPos;

	// Token: 0x040038AA RID: 14506
	private Vector3 _animHeadPos;

	// Token: 0x040038AB RID: 14507
	private RuntimeAnimatorController _seatedRAC;

	// Token: 0x040038AC RID: 14508
	private bool _rotationCleared;

	// Token: 0x040038AD RID: 14509
	private VRCInput _inGrabL;

	// Token: 0x040038AE RID: 14510
	private VRCInput _inGrabR;

	// Token: 0x040038AF RID: 14511
	private bool _isIkEnabled;

	// Token: 0x040038B0 RID: 14512
	private float autoFootWtGoal;

	// Token: 0x040038B1 RID: 14513
	private bool autoFootResetOnce = true;

	// Token: 0x040038B2 RID: 14514
	private Vector3 lThumbDir = Vector3.zero;

	// Token: 0x040038B3 RID: 14515
	private Vector3 rThumbDir = Vector3.zero;
}
