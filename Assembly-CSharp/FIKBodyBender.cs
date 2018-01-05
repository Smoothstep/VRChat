using System;
using RootMotion.FinalIK;
using UnityEngine;

// Token: 0x02000A37 RID: 2615
public class FIKBodyBender : MonoBehaviour
{
	// Token: 0x06004EA4 RID: 20132 RVA: 0x001A62CC File Offset: 0x001A46CC
	public void Initialize()
	{
		this._anim = this.ik.GetComponent<Animator>();
		this._baseLeftWristRot = Quaternion.Inverse(this._anim.transform.rotation) * this._anim.GetBoneTransform(HumanBodyBones.LeftHand).rotation;
		this._baseRightWristRot = Quaternion.Inverse(this._anim.transform.rotation) * this._anim.GetBoneTransform(HumanBodyBones.RightHand).rotation;
		this.ik.solver.rightHandEffector.rotation = this._anim.GetBoneTransform(HumanBodyBones.RightHand).rotation;
		Transform boneTransform = this._anim.GetBoneTransform(HumanBodyBones.Head);
		float y = this.ik.transform.parent.parent.position.y;
		this._avatarStandingHeadHeight = boneTransform.position.y - y;
		this.ik.solver.leftFootEffector.positionWeight = 0f;
		this.ik.solver.leftFootEffector.rotationWeight = 0f;
		this.ik.solver.leftFootEffector.maintainRelativePositionWeight = 0f;
		this.ik.solver.leftLegChain.pull = 0.2f;
		this.ik.solver.leftLegChain.reach = 0.2f;
		this.ik.solver.leftLegMapping.maintainRotationWeight = 0.6f;
		this.ik.solver.rightFootEffector.positionWeight = 0f;
		this.ik.solver.rightFootEffector.rotationWeight = 0f;
		this.ik.solver.rightFootEffector.maintainRelativePositionWeight = 0f;
		this.ik.solver.rightLegChain.pull = 0.2f;
		this.ik.solver.rightLegChain.reach = 0.2f;
		this.ik.solver.rightLegMapping.maintainRotationWeight = 0.6f;
		this.ik.solver.leftHandEffector.maintainRelativePositionWeight = 1f;
		this.ik.solver.leftArmChain.pull = 0.05f;
		this.ik.solver.leftArmChain.reach = 0.05f;
		this.ik.solver.leftArmChain.bendConstraint.weight = 0.05f;
		this.ik.solver.leftArmMapping.weight = 1f;
		this.ik.solver.leftArmMapping.maintainRotationWeight = 0f;
		this.ik.solver.rightHandEffector.maintainRelativePositionWeight = 1f;
		this.ik.solver.rightArmChain.pull = 0.05f;
		this.ik.solver.rightArmChain.reach = 0.05f;
		this.ik.solver.rightArmChain.bendConstraint.weight = 0.05f;
		this.ik.solver.rightArmMapping.weight = 1f;
		this.ik.solver.rightArmMapping.maintainRotationWeight = 0f;
		this.elbowBendDirectionOffsetLeft = new Vector3(-0.5f, -1f, -1f);
		this.elbowBendDirectionOffsetRight = new Vector3(0.5f, -1f, -1f);
		this.ik.solver.leftArmChain.bendConstraint.weight = 0.5f;
		this.ik.solver.rightArmChain.bendConstraint.weight = 0.5f;
		this.kneeBendDirectionOffsetLeft = new Vector3(-1f, 1f, 1f);
		this.kneeBendDirectionOffsetRight = new Vector3(1f, 1f, 1f);
		this.ik.solver.leftLegChain.bendConstraint.weight = 0f;
		this.ik.solver.rightLegChain.bendConstraint.weight = 0f;
		this.ik.solver.bodyEffector.positionWeight = 0f;
		this.ik.solver.bodyEffector.rotationWeight = 0f;
		this.ik.solver.spineStiffness = 0.75f;
		this.ik.solver.pullBodyVertical = 0f;
		this.ik.solver.pullBodyHorizontal = 0f;
		IKSolverFullBodyBiped solver = this.ik.solver;
		solver.OnPostUpdate = (IKSolver.UpdateDelegate)Delegate.Combine(solver.OnPostUpdate, new IKSolver.UpdateDelegate(this.OnPostFBBIK));
		this._initiated = true;
	}

	// Token: 0x17000BAD RID: 2989
	// (get) Token: 0x06004EA5 RID: 20133 RVA: 0x001A67C0 File Offset: 0x001A4BC0
	// (set) Token: 0x06004EA6 RID: 20134 RVA: 0x001A67D7 File Offset: 0x001A4BD7
	public float LeftHandWeight
	{
		get
		{
			return this.ik.solver.leftHandEffector.positionWeight;
		}
		set
		{
			this.ik.solver.leftHandEffector.positionWeight = value;
			this.ik.solver.leftHandEffector.rotationWeight = value;
		}
	}

	// Token: 0x17000BAE RID: 2990
	// (get) Token: 0x06004EA7 RID: 20135 RVA: 0x001A6805 File Offset: 0x001A4C05
	// (set) Token: 0x06004EA8 RID: 20136 RVA: 0x001A681C File Offset: 0x001A4C1C
	public Vector3 LeftHandPos
	{
		get
		{
			return this.ik.solver.leftHandEffector.position;
		}
		set
		{
			this.ik.solver.leftHandEffector.position = value;
		}
	}

	// Token: 0x17000BAF RID: 2991
	// (get) Token: 0x06004EA9 RID: 20137 RVA: 0x001A6834 File Offset: 0x001A4C34
	// (set) Token: 0x06004EAA RID: 20138 RVA: 0x001A6870 File Offset: 0x001A4C70
	public Quaternion LeftHandRot
	{
		get
		{
			return this.ik.solver.leftHandEffector.rotation * Quaternion.Inverse(this._baseLeftWristRot) * Quaternion.Inverse(Quaternion.Euler(this.LeftWristAlign));
		}
		set
		{
			this.ik.solver.leftHandEffector.rotation = value * Quaternion.Euler(this.LeftWristAlign) * this._baseLeftWristRot;
		}
	}

	// Token: 0x17000BB0 RID: 2992
	// (get) Token: 0x06004EAB RID: 20139 RVA: 0x001A68A3 File Offset: 0x001A4CA3
	// (set) Token: 0x06004EAC RID: 20140 RVA: 0x001A68BA File Offset: 0x001A4CBA
	public float RightHandWeight
	{
		get
		{
			return this.ik.solver.rightHandEffector.positionWeight;
		}
		set
		{
			this.ik.solver.rightHandEffector.positionWeight = value;
			this.ik.solver.rightHandEffector.rotationWeight = value;
		}
	}

	// Token: 0x17000BB1 RID: 2993
	// (get) Token: 0x06004EAD RID: 20141 RVA: 0x001A68E8 File Offset: 0x001A4CE8
	// (set) Token: 0x06004EAE RID: 20142 RVA: 0x001A68FF File Offset: 0x001A4CFF
	public Vector3 RightHandPos
	{
		get
		{
			return this.ik.solver.rightHandEffector.position;
		}
		set
		{
			this.ik.solver.rightHandEffector.position = value;
		}
	}

	// Token: 0x17000BB2 RID: 2994
	// (get) Token: 0x06004EAF RID: 20143 RVA: 0x001A6917 File Offset: 0x001A4D17
	// (set) Token: 0x06004EB0 RID: 20144 RVA: 0x001A6953 File Offset: 0x001A4D53
	public Quaternion RightHandRot
	{
		get
		{
			return this.ik.solver.rightHandEffector.rotation * Quaternion.Inverse(this._baseRightWristRot) * Quaternion.Inverse(Quaternion.Euler(this.RightWristAlign));
		}
		set
		{
			this.ik.solver.rightHandEffector.rotation = value * Quaternion.Euler(this.RightWristAlign) * this._baseRightWristRot;
		}
	}

	// Token: 0x06004EB1 RID: 20145 RVA: 0x001A6988 File Offset: 0x001A4D88
	public void SetHipImmobile(bool flag)
	{
		if (flag && this.HipWeight < 0.5f)
		{
			this.HipWeight = 1f;
		}
		if (!flag && this.HipWeight >= 0.5f)
		{
			this.HipWeight = 0f;
		}
	}

	// Token: 0x17000BB3 RID: 2995
	// (get) Token: 0x06004EB2 RID: 20146 RVA: 0x001A69D7 File Offset: 0x001A4DD7
	// (set) Token: 0x06004EB3 RID: 20147 RVA: 0x001A69E9 File Offset: 0x001A4DE9
	public float SolverWeight
	{
		get
		{
			return this.ik.solver.GetIKPositionWeight();
		}
		set
		{
			this.ik.solver.SetIKPositionWeight(value);
		}
	}

	// Token: 0x06004EB4 RID: 20148 RVA: 0x001A69FC File Offset: 0x001A4DFC
	public Vector3 GetCOG()
	{
		return this._computedCOG;
	}

	// Token: 0x06004EB5 RID: 20149 RVA: 0x001A6A04 File Offset: 0x001A4E04
	private void ComputeCOG()
	{
		if (this.ik == null || this.ik.transform.parent == null || this.ik.transform.parent.parent == null)
		{
			return;
		}
		Transform parent = this.ik.transform.parent.parent;
		if (this.SolverWeight >= 0.5f)
		{
			Vector3 position = this.ik.references.head.position;
			Vector3 position2 = this.ik.references.pelvis.position;
			Vector3 position3 = this.ik.references.leftFoot.position;
			Vector3 position4 = this.ik.references.rightFoot.position;
			this._computedCOG = (position2 * 4f + position * 2f + position3 + position4) / 8f;
			this._computedCOG.y = parent.position.y;
		}
		else
		{
			this._computedCOG = parent.position;
		}
	}

	// Token: 0x06004EB6 RID: 20150 RVA: 0x001A6B40 File Offset: 0x001A4F40
	private void ComputeUpright()
	{
		float y = this._anim.GetBoneTransform(HumanBodyBones.Head).position.y;
		float y2 = this.ik.transform.parent.parent.position.y;
		this._avatarUpright = Mathf.Clamp01((y - y2) / this._avatarStandingHeadHeight);
	}

	// Token: 0x06004EB7 RID: 20151 RVA: 0x001A6BA0 File Offset: 0x001A4FA0
	private void ComputeFootLocking(Vector3 lFootPos, Vector3 rFootPos)
	{
		if ((this._computedCOG - this._lockedCOG).magnitude > 0.3f)
		{
			this._feetLocked = false;
			this._lockedCOG = Vector3.MoveTowards(this._lockedCOG, this._computedCOG, Time.deltaTime * 2f);
			this._lFootLockPos = Vector3.MoveTowards(this._lFootLockPos, lFootPos, Time.deltaTime * 2f);
			this._rFootLockPos = Vector3.MoveTowards(this._rFootLockPos, rFootPos, Time.deltaTime * 2f);
		}
		else
		{
			this._feetLocked = true;
			this._lockedCOG = Vector3.MoveTowards(this._lockedCOG, this._computedCOG, Time.deltaTime / 10f);
		}
	}

	// Token: 0x06004EB8 RID: 20152 RVA: 0x001A6C64 File Offset: 0x001A5064
	private void LateUpdate()
	{
		if (this.ik == null || this.ik.transform.parent == null || this.ik.transform.parent.parent == null)
		{
			return;
		}
		if (!this.ik.solver.IsValid())
		{
			return;
		}
		Vector3 left = Vector3.left;
		Vector3 right = Vector3.right;
		Quaternion rotation = this.ik.transform.parent.parent.rotation;
		this.ik.solver.leftArmChain.bendConstraint.direction = rotation * left + rotation * this.elbowBendDirectionOffsetLeft;
		this.ik.solver.rightArmChain.bendConstraint.direction = rotation * right + rotation * this.elbowBendDirectionOffsetRight;
		this.ik.solver.leftLegChain.bendConstraint.direction = rotation * left + rotation * this.kneeBendDirectionOffsetLeft;
		this.ik.solver.rightLegChain.bendConstraint.direction = rotation * right + rotation * this.kneeBendDirectionOffsetRight;
		float num = 1f - this._avatarUpright;
		this.ik.solver.leftLegChain.bendConstraint.weight = 0.1f * num;
		this.ik.solver.rightLegChain.bendConstraint.weight = 0.1f * num;
		if (this.HipWeight == 1f)
		{
			this.ik.solver.leftFootEffector.positionWeight = 0f;
			this.ik.solver.rightFootEffector.positionWeight = 0f;
			this.ik.solver.bodyEffector.positionWeight = 1f;
			this.ik.solver.bodyEffector.position = this.ik.references.pelvis.position;
		}
		else
		{
			float positionWeight = Mathf.Max(this.ik.solver.leftHandEffector.positionWeight, this.ik.solver.rightHandEffector.positionWeight);
			Transform parent = this.ik.transform.parent.parent;
			Vector3 position = this.ik.references.head.position;
			Vector3 position2 = this.ik.references.leftFoot.position;
			Vector3 position3 = this.ik.references.rightFoot.position;
			Vector3 b = position;
			b.y = parent.position.y;
			Vector3 computedCOG = this._computedCOG;
			computedCOG.y = parent.position.y + this._avatarStandingHeadHeight / 5f;
			this.ik.solver.bodyEffector.position = computedCOG;
			this.ik.solver.bodyEffector.positionWeight = Mathf.Clamp01(1f - 2f * this._avatarUpright);
			Vector3 vector = this.ik.transform.parent.parent.TransformDirection(Vector3.forward);
			vector *= 0.5f * this._avatarStandingHeadHeight * Mathf.Clamp01(1f - this._avatarUpright);
			this.ik.solver.leftFootEffector.position = position2 - b + this._computedCOG + vector;
			this.ik.solver.rightFootEffector.position = position3 - b + this._computedCOG + vector;
			this.ik.solver.leftFootEffector.positionWeight = positionWeight;
			this.ik.solver.rightFootEffector.positionWeight = positionWeight;
		}
	}

	// Token: 0x06004EB9 RID: 20153 RVA: 0x001A708C File Offset: 0x001A548C
	private void OnPostFBBIK()
	{
		if (this.ik == null)
		{
			return;
		}
		this.ComputeCOG();
		this.ComputeUpright();
		if (this.ik.solver.leftHandEffector.rotationWeight > 0f)
		{
			this.ik.references.leftHand.rotation = this.ik.solver.leftHandEffector.rotation;
		}
		if (this.ik.solver.rightHandEffector.rotationWeight > 0f)
		{
			this.ik.references.rightHand.rotation = this.ik.solver.rightHandEffector.rotation;
		}
	}

	// Token: 0x06004EBA RID: 20154 RVA: 0x001A7149 File Offset: 0x001A5549
	private void OnDestroy()
	{
		if (this.ik != null)
		{
			IKSolverFullBodyBiped solver = this.ik.solver;
			solver.OnPostUpdate = (IKSolver.UpdateDelegate)Delegate.Remove(solver.OnPostUpdate, new IKSolver.UpdateDelegate(this.OnPostFBBIK));
		}
	}

	// Token: 0x040036EE RID: 14062
	public FullBodyBipedIK ik;

	// Token: 0x040036EF RID: 14063
	public FBBIKHeadEffector trackerHead;

	// Token: 0x040036F0 RID: 14064
	public Vector3 elbowBendDirectionOffsetLeft;

	// Token: 0x040036F1 RID: 14065
	public Vector3 elbowBendDirectionOffsetRight;

	// Token: 0x040036F2 RID: 14066
	public Vector3 kneeBendDirectionOffsetLeft;

	// Token: 0x040036F3 RID: 14067
	public Vector3 kneeBendDirectionOffsetRight;

	// Token: 0x040036F4 RID: 14068
	public bool isLocal;

	// Token: 0x040036F5 RID: 14069
	public float HipWeight;

	// Token: 0x040036F6 RID: 14070
	private Vector3 LeftWristAlign = new Vector3(0f, 90f, 0f);

	// Token: 0x040036F7 RID: 14071
	private Vector3 RightWristAlign = new Vector3(0f, -90f, 0f);

	// Token: 0x040036F8 RID: 14072
	private bool _initiated;

	// Token: 0x040036F9 RID: 14073
	private bool _feetLocked;

	// Token: 0x040036FA RID: 14074
	private Animator _anim;

	// Token: 0x040036FB RID: 14075
	private Quaternion _getLHandRotOffset;

	// Token: 0x040036FC RID: 14076
	private Quaternion _setLHandRotOffset;

	// Token: 0x040036FD RID: 14077
	private Quaternion _getRHandRotOffset;

	// Token: 0x040036FE RID: 14078
	private Quaternion _setRHandRotOffset;

	// Token: 0x040036FF RID: 14079
	private Quaternion _baseLeftWristRot;

	// Token: 0x04003700 RID: 14080
	private Quaternion _baseRightWristRot;

	// Token: 0x04003701 RID: 14081
	private Vector3 _lFootLockPos;

	// Token: 0x04003702 RID: 14082
	private Vector3 _rFootLockPos;

	// Token: 0x04003703 RID: 14083
	private Quaternion _lFootLockRot;

	// Token: 0x04003704 RID: 14084
	private Quaternion _rFootLockRot;

	// Token: 0x04003705 RID: 14085
	private Vector3 _lockedCOG;

	// Token: 0x04003706 RID: 14086
	private Vector3 _computedCOG;

	// Token: 0x04003707 RID: 14087
	private float _avatarStandingHeadHeight;

	// Token: 0x04003708 RID: 14088
	private float _avatarUpright;
}
