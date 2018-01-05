using System;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000A3E RID: 2622
public class IkController : MonoBehaviour
{
	// Token: 0x17000BBF RID: 3007
	// (get) Token: 0x06004EF3 RID: 20211 RVA: 0x001A8AB8 File Offset: 0x001A6EB8
	public IkController.IkType currentIk
	{
		get
		{
			return this._currentIkType;
		}
	}

	// Token: 0x06004EF4 RID: 20212 RVA: 0x001A8AC0 File Offset: 0x001A6EC0
	public void ClearIkType()
	{
		this._currentIkType = IkController.IkType.None;
	}

	// Token: 0x06004EF5 RID: 20213 RVA: 0x001A8AC9 File Offset: 0x001A6EC9
	public void SetCull(bool flag)
	{
		this._culled = flag;
	}

	// Token: 0x17000BC0 RID: 3008
	// (get) Token: 0x06004EF6 RID: 20214 RVA: 0x001A8AD2 File Offset: 0x001A6ED2
	// (set) Token: 0x06004EF7 RID: 20215 RVA: 0x001A8ADA File Offset: 0x001A6EDA
	public float FullIKBlend
	{
		get
		{
			return this.solverWeight;
		}
		set
		{
			this.solverWeight = value;
		}
	}

	// Token: 0x17000BC1 RID: 3009
	// (get) Token: 0x06004EF8 RID: 20216 RVA: 0x001A8AE3 File Offset: 0x001A6EE3
	public bool HasLowerBodyTracking
	{
		get
		{
			return this._useVrcTrackedIk && this.vrcTrackedIk != null && this.vrcTrackedIk.hasLowerBodyTracking;
		}
	}

	// Token: 0x06004EF9 RID: 20217 RVA: 0x001A8B0C File Offset: 0x001A6F0C
	private void Start()
	{
		this._cammountT = base.transform.parent.parent.Find("CameraMount");
		this._uspeakT = this._cammountT.Find("USpeak");
		this._uspeakLocalPos = this._uspeakT.localPosition;
		this._uspeakLocalRot = this._uspeakT.localRotation;
		this._emojiT = this._cammountT.Find("EmojiGenerator");
		this._emojiLocalPos = this._emojiT.localPosition;
		this._emojiLocalRot = this._emojiT.localRotation;
	}

	// Token: 0x06004EFA RID: 20218 RVA: 0x001A8BA9 File Offset: 0x001A6FA9
	private void FixedUpdate()
	{
		if (!this._inited)
		{
			return;
		}
		if (this._culled)
		{
			return;
		}
		if (this.isLocalPlayer)
		{
			this.UpdateEffectorPositions();
		}
	}

	// Token: 0x06004EFB RID: 20219 RVA: 0x001A8BD4 File Offset: 0x001A6FD4
	private void SetFloatParameter(Animator anim, string name, float val)
	{
		foreach (AnimatorControllerParameter animatorControllerParameter in anim.parameters)
		{
			if (animatorControllerParameter.type == AnimatorControllerParameterType.Float && animatorControllerParameter.name == name)
			{
				anim.SetFloat(name, val);
			}
		}
	}

	// Token: 0x06004EFC RID: 20220 RVA: 0x001A8C28 File Offset: 0x001A7028
	public int GetMatchingLayerIndex(Animator anim, string match)
	{
		for (int i = 0; i < anim.layerCount; i++)
		{
			string layerName = anim.GetLayerName(i);
			if (layerName.ToLower().Contains(match))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06004EFD RID: 20221 RVA: 0x001A8C68 File Offset: 0x001A7068
	public float GetLocomotionLayerWeight()
	{
		if (this._modelAnimator != null || this._locoLayer >= 0)
		{
			return this.GetLayerWeight(this._modelAnimator, this._locoLayer);
		}
		return 0f;
	}

	// Token: 0x06004EFE RID: 20222 RVA: 0x001A8C9F File Offset: 0x001A709F
	private float GetLayerWeight(Animator anim, int layerIndex)
	{
		if (layerIndex >= 0)
		{
			return anim.GetLayerWeight(layerIndex);
		}
		return 0f;
	}

	// Token: 0x06004EFF RID: 20223 RVA: 0x001A8CB5 File Offset: 0x001A70B5
	private void SetLayerWeight(Animator anim, int layerIndex, float val)
	{
		if (layerIndex >= 0)
		{
			anim.SetLayerWeight(layerIndex, val);
		}
	}

	// Token: 0x06004F00 RID: 20224 RVA: 0x001A8CC8 File Offset: 0x001A70C8
	private void Update()
	{
		PoseRecorder componentInParent = base.GetComponentInParent<PoseRecorder>();
		if (componentInParent != null && componentInParent.enabled && !this._culled)
		{
			componentInParent.DoAdjustment(Time.time);
		}
		SyncPhysics componentInParent2 = base.GetComponentInParent<SyncPhysics>();
		if (componentInParent2 != null && componentInParent2.enabled)
		{
			componentInParent2.DoPositionSync((double)Time.time, (double)Time.deltaTime);
		}
		if (!this._inited)
		{
			return;
		}
		if (this._useVrcTrackedIk)
		{
			this.vrcTrackedIk.isCulled = this._culled;
		}
		if (this._culled)
		{
			return;
		}
		if (!this._reportedUSpeakMoveError && !this._completedUSpeakMove && this._uspeakT != null && Networking.IsObjectReady(this._uspeakT.gameObject))
		{
			USpeakPhotonSender3D component = this._uspeakT.GetComponent<USpeakPhotonSender3D>();
			if (this._cammountT != null && component != null && component.IsInitialized)
			{
				if (this._useLimbIK || this._useVrcTrackedIk)
				{
					if (this._uspeakT.parent != this.HeadEffector.transform)
					{
						this._uspeakT.parent = this.HeadEffector.transform;
						this._emojiT.parent = this.HeadEffector.transform;
						Debug.Log("IK[" + this._player.name + "]: USpeak moved to head effector.");
					}
				}
				else if (this._uspeakT.parent != this._cammountT)
				{
					this._uspeakT.parent = this._cammountT;
					this._emojiT.parent = this._cammountT;
					Debug.Log("IK[" + this._player.name + "]: USpeak moved to camera mount.");
				}
				this._uspeakT.localPosition = this._uspeakLocalPos;
				this._uspeakT.localRotation = this._uspeakLocalRot;
				this._emojiT.localPosition = this._emojiLocalPos;
				this._emojiT.localRotation = this._emojiLocalRot;
				this._completedUSpeakMove = true;
			}
			if (VRC.Network.IsNetworkSettled)
			{
				this._elapsedUSpeakTimeToMove += Time.deltaTime;
				if (this._elapsedUSpeakTimeToMove > 10f)
				{
					Debug.LogError("IK[" + this._player.name + "]: USpeak move took too long. aborting.");
					this._reportedUSpeakMoveError = true;
				}
			}
		}
		if (this._modelAnimator != null && this._modelAnimator.isHuman)
		{
			float num = 2f;
			float num2 = 1.5f;
			float num3 = 2f;
			float num4 = 2f;
			float num5 = 2f;
			float num6 = 10f;
			float num7 = 5f;
			bool flag = false;
			if (this.isLocalPlayer)
			{
				float num8 = VRCTrackingManager.GetPlayerUprightAmount();
				if (this._locoLayer >= 0)
				{
					if (this._useLimbIK)
					{
						num8 = 1f;
					}
					if (this.motion.IsImmobilized)
					{
						this.SetFloatParameter(this._modelAnimator, "HeightScale", 1f);
					}
					else
					{
						this.SetFloatParameter(this._modelAnimator, "HeightScale", num8);
					}
				}
				this.motion.StandingHeight = num8;
				this.UpdateTracking();
				if (this.motion.isLocomoting)
				{
					if (!this._wasLoco)
					{
						this._wasLoco = true;
						flag = true;
					}
					if (this._player.GetVRMode() && this._useVrcTrackedIk && !this.vrcTrackedIk.hasLowerBodyTracking)
					{
						this.headPosWeight = 1f;
						this.headRotWeight = 1f;
					}
					else
					{
						this.headRotWeight = 0f;
						this.headPosWeight = 0f;
					}
					this.lowerBodyWeight = 0f;
					this._locoWeight = 1f;
				}
				else
				{
					if (this._wasLoco)
					{
						this._wasLoco = false;
						flag = true;
					}
					this.lowerBodyWeight = 1f;
					this._locoWeight = 0f;
				}
				if (this._locoLayer >= 0)
				{
					float num9 = this.GetLayerWeight(this._modelAnimator, this._locoLayer);
					if (this.motion.IsImmobilized)
					{
						num9 = this._locoWeight;
					}
					else
					{
						num9 = Mathf.MoveTowards(num9, this._locoWeight, Time.deltaTime * num2);
					}
					this.SetLayerWeight(this._modelAnimator, this._locoLayer, num9);
				}
				if (this._useLimbIK)
				{
					this.limbik.LeftPosWeight = Mathf.MoveTowards(this.limbik.LeftPosWeight, this.leftWeight, Time.deltaTime * num);
					this.limbik.RightPosWeight = Mathf.MoveTowards(this.limbik.RightPosWeight, this.rightWeight, Time.deltaTime * num);
					this.limbik.HeadPosWeight = Mathf.MoveTowards(this.limbik.HeadPosWeight, this.headPosWeight, Time.deltaTime * num3);
					this.limbik.LeftRotWeight = this.limbik.LeftPosWeight;
					this.limbik.RightRotWeight = this.limbik.RightPosWeight;
					this.limbik.HeadRotWeight = this.limbik.HeadPosWeight;
				}
				if (this._useVrcTrackedIk)
				{
					if (flag)
					{
						this.vrcTrackedIk.LocomotionChange(this._wasLoco);
					}
					if (this.motion.IsImmobilized)
					{
						this.vrcTrackedIk.LeftHandWeight = this.leftWeight;
						this.vrcTrackedIk.RightHandWeight = this.rightWeight;
						this.vrcTrackedIk.HeadPosWeight = this.headPosWeight;
						this.vrcTrackedIk.HeadRotWeight = this.headRotWeight;
						this.vrcTrackedIk.LowerBodyWeight = this.lowerBodyWeight;
					}
					else
					{
						this.vrcTrackedIk.LeftHandWeight = Mathf.MoveTowards(this.vrcTrackedIk.LeftHandWeight, this.leftWeight, Time.deltaTime * num);
						this.vrcTrackedIk.RightHandWeight = Mathf.MoveTowards(this.vrcTrackedIk.RightHandWeight, this.rightWeight, Time.deltaTime * num);
						this.vrcTrackedIk.HeadRotWeight = Mathf.MoveTowards(this.vrcTrackedIk.HeadRotWeight, this.headRotWeight, Time.deltaTime * num3);
						if (this.vrcTrackedIk.hasLowerBodyTracking)
						{
							this.vrcTrackedIk.LowerBodyWeight = this.lowerBodyWeight;
							this.vrcTrackedIk.HeadPosWeight = this.headPosWeight;
						}
						else
						{
							if (this.headPosWeight == 1f)
							{
								this.vrcTrackedIk.HeadPosWeight = this.headPosWeight;
							}
							else
							{
								this.vrcTrackedIk.HeadPosWeight = Mathf.MoveTowards(this.vrcTrackedIk.HeadPosWeight, this.headPosWeight, Time.deltaTime * num3);
							}
							this.vrcTrackedIk.LowerBodyWeight = Mathf.MoveTowards(this.vrcTrackedIk.LowerBodyWeight, this.lowerBodyWeight, Time.deltaTime * num4);
						}
					}
				}
				if (this._useVrcTrackedIk)
				{
					if (this.motion.IsImmobilized)
					{
						this.vrcTrackedIk.SolverWeight = this.solverWeight;
					}
					else if (this.vrcTrackedIk.hasLowerBodyTracking)
					{
						this.vrcTrackedIk.SolverWeight = Mathf.MoveTowards(this.vrcTrackedIk.SolverWeight, this.solverWeight, Time.deltaTime * num7);
					}
					else
					{
						this.vrcTrackedIk.SolverWeight = Mathf.MoveTowards(this.vrcTrackedIk.SolverWeight, this.solverWeight, Time.deltaTime * num5);
					}
				}
			}
			else
			{
				float standingHeight = this.motion.StandingHeight;
				if (this._locoLayer >= 0)
				{
					this.SetFloatParameter(this._modelAnimator, "HeightScale", standingHeight);
				}
				if (this.motion.isLocomoting)
				{
					if (!this._wasLoco)
					{
						this._wasLoco = true;
						flag = true;
						this._locoWeight = 1f;
					}
					if (!this._player.GetVRMode() || !this._useVrcTrackedIk || this.vrcTrackedIk.hasLowerBodyTracking)
					{
						this.headPosWeight = 0f;
					}
					this.lowerBodyWeight = 0f;
				}
				else if (this._wasLoco)
				{
					this._wasLoco = false;
					flag = true;
					this._locoWeight = 0f;
				}
				Vector3 position = this._modelAnimator.GetBoneTransform(HumanBodyBones.Head).position;
				Vector3 position2 = this.LeftEffector.transform.position;
				Vector3 position3 = this.RightEffector.transform.position;
				if (this._locoLayer >= 0)
				{
					float num10 = this.GetLayerWeight(this._modelAnimator, this._locoLayer);
					num10 = Mathf.MoveTowards(num10, this._locoWeight, Time.deltaTime * num2);
					this.SetLayerWeight(this._modelAnimator, this._locoLayer, num10);
				}
				if (this._useLimbIK)
				{
					this.limbik.LeftPosWeight = Mathf.Min(this.leftWeight, 1f);
					this.limbik.LeftRotWeight = Mathf.Min(this.leftWeight, 1f);
					this.limbik.RightPosWeight = Mathf.Min(this.rightWeight, 1f);
					this.limbik.RightRotWeight = Mathf.Min(this.rightWeight, 1f);
					this.limbik.HeadPosWeight = Mathf.Min(this.headPosWeight, 1f);
					this.limbik.HeadRotWeight = Mathf.Min(this.headRotWeight, 1f);
				}
				if (this._useVrcTrackedIk)
				{
					if (flag)
					{
						this.vrcTrackedIk.LocomotionChange(this._wasLoco);
					}
					this.vrcTrackedIk.LeftHandWeight = Mathf.MoveTowards(this.vrcTrackedIk.LeftHandWeight, this.leftWeight, Time.deltaTime * num);
					this.vrcTrackedIk.RightHandWeight = Mathf.MoveTowards(this.vrcTrackedIk.RightHandWeight, this.rightWeight, Time.deltaTime * num);
					this.vrcTrackedIk.HeadRotWeight = Mathf.MoveTowards(this.vrcTrackedIk.HeadRotWeight, this.headRotWeight, Time.deltaTime * num3);
					if (this.vrcTrackedIk.hasLowerBodyTracking)
					{
						this.vrcTrackedIk.LowerBodyWeight = this.lowerBodyWeight;
						this.vrcTrackedIk.HeadPosWeight = this.headPosWeight;
					}
					else
					{
						this.vrcTrackedIk.LowerBodyWeight = Mathf.MoveTowards(this.vrcTrackedIk.LowerBodyWeight, this.lowerBodyWeight, Time.deltaTime * num4);
						this.vrcTrackedIk.HeadPosWeight = Mathf.MoveTowards(this.vrcTrackedIk.HeadPosWeight, this.headPosWeight, Time.deltaTime * num3);
					}
					if (this.motion.IsImmobilized)
					{
						this.vrcTrackedIk.SolverWeight = Mathf.MoveTowards(this.vrcTrackedIk.SolverWeight, this.solverWeight, Time.deltaTime * num6);
					}
					else if (this.vrcTrackedIk.hasLowerBodyTracking)
					{
						this.vrcTrackedIk.SolverWeight = Mathf.MoveTowards(this.vrcTrackedIk.SolverWeight, this.solverWeight, Time.deltaTime * num7);
					}
					else
					{
						this.vrcTrackedIk.SolverWeight = Mathf.MoveTowards(this.vrcTrackedIk.SolverWeight, this.solverWeight, Time.deltaTime * num5);
					}
				}
				this.leftWeight = Mathf.Max(this.leftWeight - Time.deltaTime, 0f);
				this.rightWeight = Mathf.Max(this.rightWeight - Time.deltaTime, 0f);
				this.headPosWeight = Mathf.Max(this.headPosWeight - Time.deltaTime, 0f);
				this.headRotWeight = Mathf.Max(this.headRotWeight - Time.deltaTime, 0f);
				this.lowerBodyWeight = Mathf.Max(this.lowerBodyWeight - Time.deltaTime, 0f);
			}
			if (flag)
			{
				this.motion.Reset();
				if (this._useVrcTrackedIk)
				{
					this.vrcTrackedIk.Reset(false);
				}
			}
		}
	}

	// Token: 0x06004F01 RID: 20225 RVA: 0x001A98FC File Offset: 0x001A7CFC
	public void SeatedChange(bool sitting)
	{
		if (this._useVrcTrackedIk && this.vrcTrackedIk != null)
		{
			this.vrcTrackedIk.SeatedChange(sitting);
		}
	}

	// Token: 0x06004F02 RID: 20226 RVA: 0x001A9920 File Offset: 0x001A7D20
	public void Reset(bool restoreWhenDone = true)
	{
		if (!this._inited)
		{
			return;
		}
		this.motion.Reset();
		if (this._useLimbIK && this.limbik != null)
		{
			this.limbik.LeftPosWeight = Mathf.Min(this.leftWeight, 1f);
			this.limbik.LeftRotWeight = Mathf.Min(this.leftWeight, 1f);
			this.limbik.RightPosWeight = Mathf.Min(this.rightWeight, 1f);
			this.limbik.RightRotWeight = Mathf.Min(this.rightWeight, 1f);
			this.limbik.HeadPosWeight = Mathf.Min(this.headPosWeight, 1f);
			this.limbik.HeadRotWeight = Mathf.Min(this.headRotWeight, 1f);
		}
		if (this._useVrcTrackedIk && this.vrcTrackedIk != null)
		{
			this.vrcTrackedIk.LeftHandWeight = this.leftWeight;
			this.vrcTrackedIk.RightHandWeight = this.rightWeight;
			this.vrcTrackedIk.HeadPosWeight = this.headPosWeight;
			this.vrcTrackedIk.HeadRotWeight = this.headRotWeight;
			this.vrcTrackedIk.LowerBodyWeight = this.lowerBodyWeight;
			this.vrcTrackedIk.NeedsReset();
		}
	}

	// Token: 0x06004F03 RID: 20227 RVA: 0x001A9A78 File Offset: 0x001A7E78
	public void InstantReset()
	{
		if (!this._inited)
		{
			return;
		}
		this.motion.Reset();
		if (this._useLimbIK && this.limbik != null)
		{
			this.limbik.LeftPosWeight = Mathf.Min(this.leftWeight, 1f);
			this.limbik.LeftRotWeight = Mathf.Min(this.leftWeight, 1f);
			this.limbik.RightPosWeight = Mathf.Min(this.rightWeight, 1f);
			this.limbik.RightRotWeight = Mathf.Min(this.rightWeight, 1f);
			this.limbik.HeadPosWeight = Mathf.Min(this.headPosWeight, 1f);
			this.limbik.HeadRotWeight = Mathf.Min(this.headRotWeight, 1f);
		}
		if (this._useVrcTrackedIk && this.vrcTrackedIk != null)
		{
			this.vrcTrackedIk.LeftHandWeight = Mathf.Min(this.leftWeight, 1f);
			this.vrcTrackedIk.RightHandWeight = Mathf.Min(this.rightWeight, 1f);
			this.vrcTrackedIk.HeadPosWeight = Mathf.Min(this.headPosWeight, 1f);
			this.vrcTrackedIk.HeadRotWeight = Mathf.Min(this.headRotWeight, 1f);
			this.vrcTrackedIk.LowerBodyWeight = Mathf.Min(this.lowerBodyWeight, 1f);
			this.vrcTrackedIk.Reset(false);
		}
	}

	// Token: 0x06004F04 RID: 20228 RVA: 0x001A9C04 File Offset: 0x001A8004
	public void SetRemoteHandEffectorLeft(Vector3 pos, Quaternion rot)
	{
		pos = base.transform.TransformPoint(pos);
		this.LeftEffector.transform.position = pos;
		rot = base.transform.rotation * rot;
		this.LeftEffector.transform.rotation = rot;
		this.leftWeight = 2f;
	}

	// Token: 0x06004F05 RID: 20229 RVA: 0x001A9C5F File Offset: 0x001A805F
	public void ClearRemoteHandEffectorLeft()
	{
		this.leftWeight = 0f;
	}

	// Token: 0x06004F06 RID: 20230 RVA: 0x001A9C6C File Offset: 0x001A806C
	public void SetRemoteHandEffectorRight(Vector3 pos, Quaternion rot)
	{
		pos = base.transform.TransformPoint(pos);
		this.RightEffector.transform.position = pos;
		rot = base.transform.rotation * rot;
		this.RightEffector.transform.rotation = rot;
		this.rightWeight = 2f;
	}

	// Token: 0x06004F07 RID: 20231 RVA: 0x001A9CC7 File Offset: 0x001A80C7
	public void ClearRemoteHandEffectorRight()
	{
		this.rightWeight = 0f;
	}

	// Token: 0x06004F08 RID: 20232 RVA: 0x001A9CD4 File Offset: 0x001A80D4
	public void SetRemoteHeadEffector(Vector3 pos, Quaternion rot)
	{
		pos = base.transform.TransformPoint(pos);
		this.HeadEffector.transform.position = pos;
		rot = base.transform.rotation * rot;
		this.HeadEffector.transform.rotation = rot;
		this.headPosWeight = 1f;
		this.headRotWeight = 1f;
	}

	// Token: 0x06004F09 RID: 20233 RVA: 0x001A9D3C File Offset: 0x001A813C
	public void SetRemoteFootEffectorLeft(Vector3 pos, Quaternion rot)
	{
		if (this._currentIkType != IkController.IkType.SixPoint)
		{
			this.SetIkType(IkController.IkType.SixPoint, true);
		}
		pos = base.transform.TransformPoint(pos);
		this.LeftFootEffector.transform.position = pos;
		rot = base.transform.rotation * rot;
		this.LeftFootEffector.transform.rotation = rot;
	}

	// Token: 0x06004F0A RID: 20234 RVA: 0x001A9DA0 File Offset: 0x001A81A0
	public void ClearRemoteFootEffectorLeft()
	{
		this.lowerBodyWeight = 0f;
	}

	// Token: 0x06004F0B RID: 20235 RVA: 0x001A9DB0 File Offset: 0x001A81B0
	public void SetRemoteFootEffectorRight(Vector3 pos, Quaternion rot)
	{
		if (this._currentIkType != IkController.IkType.SixPoint)
		{
			this.SetIkType(IkController.IkType.SixPoint, true);
		}
		pos = base.transform.TransformPoint(pos);
		this.RightFootEffector.transform.position = pos;
		rot = base.transform.rotation * rot;
		this.RightFootEffector.transform.rotation = rot;
	}

	// Token: 0x06004F0C RID: 20236 RVA: 0x001A9E14 File Offset: 0x001A8214
	public void ClearRemoteFootEffectorRight()
	{
		this.lowerBodyWeight = 0f;
	}

	// Token: 0x06004F0D RID: 20237 RVA: 0x001A9E24 File Offset: 0x001A8224
	public void SetRemoteHipEffector(Vector3 pos, Quaternion rot)
	{
		pos = base.transform.TransformPoint(pos);
		this.HipEffector.transform.position = pos;
		rot = base.transform.rotation * rot;
		this.HipEffector.transform.rotation = rot;
		if (this._useVrcTrackedIk && !this.vrcTrackedIk.hasLowerBodyTracking)
		{
			this.vrcTrackedIk.hasLowerBodyTracking = true;
		}
		this.lowerBodyWeight = 1f;
	}

	// Token: 0x06004F0E RID: 20238 RVA: 0x001A9EA6 File Offset: 0x001A82A6
	public void ClearRemoteHipEffector()
	{
		this.lowerBodyWeight = 0f;
	}

	// Token: 0x06004F0F RID: 20239 RVA: 0x001A9EB4 File Offset: 0x001A82B4
	public void GetHandEffectorLeft(out Vector3 pos, out Quaternion rot)
	{
		pos = this.LeftEffector.transform.position;
		pos = base.transform.InverseTransformPoint(pos);
		rot = this.LeftEffector.transform.rotation;
		rot = Quaternion.Inverse(base.transform.rotation) * rot;
	}

	// Token: 0x06004F10 RID: 20240 RVA: 0x001A9F28 File Offset: 0x001A8328
	public void GetHandEffectorRight(out Vector3 pos, out Quaternion rot)
	{
		pos = this.RightEffector.transform.position;
		pos = base.transform.InverseTransformPoint(pos);
		rot = this.RightEffector.transform.rotation;
		rot = Quaternion.Inverse(base.transform.rotation) * rot;
	}

	// Token: 0x06004F11 RID: 20241 RVA: 0x001A9F9C File Offset: 0x001A839C
	public void GetHeadEffector(out Vector3 pos, out Quaternion rot)
	{
		pos = this.HeadEffector.transform.position;
		pos = base.transform.InverseTransformPoint(pos);
		rot = this.HeadEffector.transform.rotation;
		rot = Quaternion.Inverse(base.transform.rotation) * rot;
	}

	// Token: 0x06004F12 RID: 20242 RVA: 0x001AA010 File Offset: 0x001A8410
	public void GetFootEffectorLeft(out Vector3 pos, out Quaternion rot)
	{
		pos = this.LeftFootEffector.transform.position;
		pos = base.transform.InverseTransformPoint(pos);
		rot = this.LeftFootEffector.transform.rotation;
		rot = Quaternion.Inverse(base.transform.rotation) * rot;
	}

	// Token: 0x06004F13 RID: 20243 RVA: 0x001AA084 File Offset: 0x001A8484
	public void GetFootEffectorRight(out Vector3 pos, out Quaternion rot)
	{
		pos = this.RightFootEffector.transform.position;
		pos = base.transform.InverseTransformPoint(pos);
		rot = this.RightFootEffector.transform.rotation;
		rot = Quaternion.Inverse(base.transform.rotation) * rot;
	}

	// Token: 0x06004F14 RID: 20244 RVA: 0x001AA0F8 File Offset: 0x001A84F8
	public void GetHipEffector(out Vector3 pos, out Quaternion rot)
	{
		pos = this.HipEffector.transform.position;
		pos = base.transform.InverseTransformPoint(pos);
		rot = this.HipEffector.transform.rotation;
		rot = Quaternion.Inverse(base.transform.rotation) * rot;
	}

	// Token: 0x06004F15 RID: 20245 RVA: 0x001AA16C File Offset: 0x001A856C
	private void OnDrawGizmos()
	{
		Vector3 b = new Vector3(0f, VRCTrackingManager.GetPlayerShoulderHeight(), 0f);
		Gizmos.DrawWireSphere(base.transform.position + b, 0.05f);
		if (this.rightHandGrasper != null && this.rightHandGrasper.IsHoldingObject())
		{
			Transform trackedTransform = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.HandTracker_RightWrist);
			Gizmos.DrawWireSphere(trackedTransform.position, 0.05f);
		}
		if (this.leftHandGrasper != null && this.leftHandGrasper.IsHoldingObject())
		{
			Transform trackedTransform2 = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.HandTracker_LeftWrist);
			Gizmos.DrawWireSphere(trackedTransform2.position, 0.05f);
		}
	}

	// Token: 0x06004F16 RID: 20246 RVA: 0x001AA21C File Offset: 0x001A861C
	public void InitHeadEffector(Animator anim, bool local)
	{
		this._ikHeadBone = null;
		if (anim == null || !anim.isHuman)
		{
			return;
		}
		Transform boneTransform = anim.GetBoneTransform(HumanBodyBones.Head);
		Transform transform = boneTransform.Find("HmdPivot");
		IKHeadAlignment componentInChildren = this.HeadEffector.GetComponentInChildren<IKHeadAlignment>();
		if (boneTransform != null && transform != null && componentInChildren != null)
		{
			this.HeadEffector.transform.position = transform.position;
			this.HeadEffector.transform.localRotation = Quaternion.identity;
			componentInChildren.Initialize(anim);
			this._ikHeadBone = componentInChildren.transform;
		}
	}

	// Token: 0x06004F17 RID: 20247 RVA: 0x001AA2CC File Offset: 0x001A86CC
	public void Initialize(IkController.IkType ikType, Animator modelAnimator, VRCPlayer player, bool local)
	{
		this._player = player;
		this.isLocalPlayer = local;
		this.animationController = base.GetComponentInParent<VRC_AnimationController>();
		this._completedUSpeakMove = false;
		this._reportedUSpeakMoveError = false;
		this._elapsedUSpeakTimeToMove = 0f;
		this._useLimbIK = false;
		this._useVrcTrackedIk = false;
		if (modelAnimator != null && modelAnimator.isHuman && modelAnimator.runtimeAnimatorController != null)
		{
			this._modelAnimator = modelAnimator;
			this.SetIkType(ikType, false);
			this._locoLayer = this.GetMatchingLayerIndex(this._modelAnimator, "locomotion");
		}
		else
		{
			this._modelAnimator = null;
			this._locoLayer = -1;
		}
		if (local)
		{
			this.leftHandGrasper = this.LeftEffector.gameObject.AddMissingComponent<VRCHandGrasper>();
			this.leftHandGrasper.DropInput = VRCInputManager.FindInput("DropLeft");
			this.leftHandGrasper.UseInput = VRCInputManager.FindInput("UseLeft");
			this.leftHandGrasper.GrabInput = VRCInputManager.FindInput("GrabLeft");
			this.leftHandGrasper.RightHand = false;
			this.rightHandGrasper = this.RightEffector.gameObject.AddMissingComponent<VRCHandGrasper>();
			this.rightHandGrasper.DropInput = VRCInputManager.FindInput("DropRight");
			this.rightHandGrasper.UseInput = VRCInputManager.FindInput("UseRight");
			this.rightHandGrasper.GrabInput = VRCInputManager.FindInput("GrabRight");
			this.rightHandGrasper.RightHand = true;
		}
		this.motion = this.animationController.GetComponentInParent<VRCMotionState>();
		this.motion.StandingHeight = 1f;
		this._inited = true;
		this._completedUSpeakMove = false;
	}

	// Token: 0x06004F18 RID: 20248 RVA: 0x001AA474 File Offset: 0x001A8874
	private void SetIkType(IkController.IkType iktype, bool reinit = false)
	{
		if (iktype == IkController.IkType.SixPoint)
		{
			this._currentIkType = iktype;
			this.vrcTrackedIk = base.GetComponent<VRCFbbIkController>();
			this._useVrcTrackedIk = true;
			this._useLimbIK = false;
		}
		else if (iktype == IkController.IkType.ThreeOrFourPoint)
		{
			this._currentIkType = iktype;
			this.vrcTrackedIk = base.GetComponent<VRCVrIkController>();
			this._useVrcTrackedIk = true;
			this._useLimbIK = false;
		}
		else if (iktype == IkController.IkType.Limb)
		{
			this._currentIkType = iktype;
			this._useLimbIK = true;
			this._useVrcTrackedIk = false;
			this.vrcTrackedIk = null;
			this.limbik = this._modelAnimator.GetComponent<LimbIK>();
			if (this._ikHeadBone != null)
			{
				this.limbik.HeadTarget = this._ikHeadBone;
			}
			this.limbik.LeftHandTarget = this.LeftEffector.transform;
			this.limbik.RightHandTarget = this.RightEffector.transform;
		}
		else
		{
			this._currentIkType = IkController.IkType.None;
			Debug.Log("IkController: Invalid IK Type specified:" + iktype);
		}
		if (reinit && this.animationController != null)
		{
			Debug.Log(string.Concat(new object[]
			{
				"IK[",
				this._player.name,
				"] Reset To:",
				this._currentIkType
			}));
			this.animationController.ResetIKSystem(this._modelAnimator, true, this._currentIkType);
		}
	}

	// Token: 0x06004F19 RID: 20249 RVA: 0x001AA5EC File Offset: 0x001A89EC
	private void UpdateEffectorPositions()
	{
		if (VRCTrackingManager.IsPlayerNearTracking())
		{
			Transform trackedTransform = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.HandTracker_LeftWrist);
			this.LeftEffector.transform.position = trackedTransform.position;
			this.LeftEffector.transform.rotation = trackedTransform.rotation;
			Transform trackedTransform2 = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.HandTracker_RightWrist);
			this.RightEffector.transform.position = trackedTransform2.position;
			this.RightEffector.transform.rotation = trackedTransform2.rotation;
			Transform trackedTransform3 = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.Hmd);
			this.HeadEffector.transform.position = trackedTransform3.position;
			this.HeadEffector.transform.rotation = trackedTransform3.rotation;
		}
	}

	// Token: 0x06004F1A RID: 20250 RVA: 0x001AA69C File Offset: 0x001A8A9C
	public void HeadControl(bool flag)
	{
		if (this.animationController != null)
		{
			this.animationController.shouldMeatHook = flag;
		}
		if (flag)
		{
			this.headPosWeight = 1f;
			this.headRotWeight = 1f;
		}
		else
		{
			this.headPosWeight = 0f;
			this.headRotWeight = 0f;
		}
	}

	// Token: 0x06004F1B RID: 20251 RVA: 0x001AA700 File Offset: 0x001A8B00
	public bool isGrasping(bool rightHand)
	{
		if (rightHand)
		{
			if (this.rightHandGrasper != null && this.rightHandGrasper.IsHoldingObject())
			{
				return true;
			}
		}
		else if (this.leftHandGrasper != null && this.leftHandGrasper.IsHoldingObject())
		{
			return true;
		}
		return false;
	}

	// Token: 0x06004F1C RID: 20252 RVA: 0x001AA760 File Offset: 0x001A8B60
	private void UpdateTracking()
	{
		this.leftHandGrasper.longReachMode = false;
		this.rightHandGrasper.longReachMode = false;
		bool flag = this.isGrasping(false);
		bool flag2 = this.isGrasping(true);
		if (flag || flag2 || VRCTrackingManager.IsPlayerNearTracking())
		{
			this.UpdateEffectorPositions();
			this.SetHandGraspersNearTracking(flag, flag2);
			if (VRCTrackingManager.IsTracked(VRCTracking.ID.Hmd))
			{
				PoseRecorder.poseContents |= 32;
			}
			this.headPosWeight = 1f;
			this.headRotWeight = 1f;
			if (this._useVrcTrackedIk && this.vrcTrackedIk.hasLowerBodyTracking && this.vrcTrackedIk.isCalibrated)
			{
				if (VRCTrackingManager.IsTracked(VRCTracking.ID.BodyTracker_Hip))
				{
					PoseRecorder.poseContents |= 64;
				}
				if (VRCTrackingManager.IsTracked(VRCTracking.ID.FootTracker_LeftFoot))
				{
					PoseRecorder.poseContents |= 128;
				}
				if (VRCTrackingManager.IsTracked(VRCTracking.ID.FootTracker_RightFoot))
				{
					PoseRecorder.poseContents |= 256;
				}
			}
			float trackingScale = VRCTrackingManager.GetTrackingScale();
			Vector3 position = this.HeadEffector.transform.position;
			if (this.motion.IsSeated || this.motion.IsImmobilized)
			{
				position = this._modelAnimator.GetBoneTransform(HumanBodyBones.Head).position;
			}
			if (VRCTrackingManager.IsTracked(VRCTracking.ID.HandTracker_LeftWrist) || flag)
			{
				Transform transform = this.LeftEffector.transform;
				float num = Vector3.Distance(position, transform.position) / trackingScale;
				this.leftWeight = Mathf.Clamp01(7f - num * 7f);
				PoseRecorder.poseContents |= 4;
			}
			else
			{
				this.leftWeight = 0f;
			}
			if (VRCTrackingManager.IsTracked(VRCTracking.ID.HandTracker_RightWrist) || flag2)
			{
				Transform transform2 = this.RightEffector.transform;
				float num2 = Vector3.Distance(position, transform2.position) / trackingScale;
				this.rightWeight = Mathf.Clamp01(7f - num2 * 7f);
				PoseRecorder.poseContents |= 8;
			}
			else
			{
				this.rightWeight = 0f;
			}
		}
		else
		{
			this.SetHandGraspersNearTracking(false, false);
			if (this._modelAnimator != null && this._modelAnimator.isHuman)
			{
				float trackingScale2 = VRCTrackingManager.GetTrackingScale();
				if (VRCTrackingManager.IsPlayerNearTracking())
				{
					PoseRecorder.poseContents |= 32;
					this.headPosWeight = 1f;
					this.headRotWeight = 1f;
				}
				if (this.leftHandGrasper.IsHoldingObject())
				{
					Transform boneTransform = this._modelAnimator.GetBoneTransform(HumanBodyBones.LeftHand);
					Quaternion tposeRotation = this.animationController.GetTPoseRotation(HumanBodyBones.LeftHand);
					Quaternion rhs = Quaternion.Inverse(tposeRotation) * Quaternion.AngleAxis(180f, Vector3.forward);
					this.LeftEffector.transform.rotation = boneTransform.rotation * rhs;
					Vector3 b = boneTransform.rotation * rhs * (this.EffectorWristOffset / trackingScale2);
					this.LeftEffector.transform.position = boneTransform.position + b;
					PoseRecorder.poseContents |= 4;
					this.leftWeight = 1f;
				}
				if (this.rightHandGrasper.IsHoldingObject())
				{
					Transform boneTransform2 = this._modelAnimator.GetBoneTransform(HumanBodyBones.RightHand);
					Quaternion tposeRotation2 = this.animationController.GetTPoseRotation(HumanBodyBones.RightHand);
					Quaternion rhs2 = Quaternion.Inverse(tposeRotation2);
					this.RightEffector.transform.rotation = boneTransform2.rotation * rhs2;
					Vector3 b2 = boneTransform2.rotation * rhs2 * (this.EffectorWristOffset / trackingScale2);
					this.RightEffector.transform.position = boneTransform2.position + b2;
					PoseRecorder.poseContents |= 8;
					this.rightWeight = 1f;
				}
			}
		}
	}

	// Token: 0x06004F1D RID: 20253 RVA: 0x001AAB43 File Offset: 0x001A8F43
	private void SetHandGraspersNearTracking(bool nearTrackingL, bool nearTrackingR)
	{
		if (this.leftHandGrasper != null)
		{
			this.leftHandGrasper.SetIsNearTracking(nearTrackingL);
		}
		if (this.rightHandGrasper != null)
		{
			this.rightHandGrasper.SetIsNearTracking(nearTrackingR);
		}
	}

	// Token: 0x06004F1E RID: 20254 RVA: 0x001AAB7F File Offset: 0x001A8F7F
	private void OnEnable()
	{
		if (this._useVrcTrackedIk && this.vrcTrackedIk != null)
		{
			this.vrcTrackedIk.enableIk = true;
		}
	}

	// Token: 0x06004F1F RID: 20255 RVA: 0x001AABA3 File Offset: 0x001A8FA3
	private void OnDisable()
	{
		if (this._useVrcTrackedIk && this.vrcTrackedIk != null)
		{
			this.vrcTrackedIk.enableIk = false;
		}
	}

	// Token: 0x04003753 RID: 14163
	private bool isLocalPlayer;

	// Token: 0x04003754 RID: 14164
	private VRCPlayer _player;

	// Token: 0x04003755 RID: 14165
	private Animator _modelAnimator;

	// Token: 0x04003756 RID: 14166
	private LimbIK limbik;

	// Token: 0x04003757 RID: 14167
	private bool _useLimbIK;

	// Token: 0x04003758 RID: 14168
	private bool _useVrcTrackedIk;

	// Token: 0x04003759 RID: 14169
	private VRCMotionState motion;

	// Token: 0x0400375A RID: 14170
	private IVRCTrackedIk vrcTrackedIk;

	// Token: 0x0400375B RID: 14171
	private IkController.IkType _currentIkType;

	// Token: 0x0400375C RID: 14172
	public GameObject LeftEffector;

	// Token: 0x0400375D RID: 14173
	public GameObject RightEffector;

	// Token: 0x0400375E RID: 14174
	public GameObject HeadEffector;

	// Token: 0x0400375F RID: 14175
	public GameObject LeftFootEffector;

	// Token: 0x04003760 RID: 14176
	public GameObject RightFootEffector;

	// Token: 0x04003761 RID: 14177
	public GameObject HipEffector;

	// Token: 0x04003762 RID: 14178
	public Vector3 EffectorWristOffset = new Vector3(0.075f, -0.02f, 0f);

	// Token: 0x04003763 RID: 14179
	public RuntimeAnimatorController flexedPoseAnimation;

	// Token: 0x04003764 RID: 14180
	private bool _disableEffectors;

	// Token: 0x04003765 RID: 14181
	private VRCHandGrasper leftHandGrasper;

	// Token: 0x04003766 RID: 14182
	private VRCHandGrasper rightHandGrasper;

	// Token: 0x04003767 RID: 14183
	private VRC_AnimationController animationController;

	// Token: 0x04003768 RID: 14184
	private Transform _ikHeadBone;

	// Token: 0x04003769 RID: 14185
	private float leftWeight;

	// Token: 0x0400376A RID: 14186
	private float rightWeight;

	// Token: 0x0400376B RID: 14187
	private float headPosWeight;

	// Token: 0x0400376C RID: 14188
	private float headRotWeight;

	// Token: 0x0400376D RID: 14189
	private float lowerBodyWeight;

	// Token: 0x0400376E RID: 14190
	private float solverWeight = 1f;

	// Token: 0x0400376F RID: 14191
	private bool _wasLoco;

	// Token: 0x04003770 RID: 14192
	private float _locoWeight;

	// Token: 0x04003771 RID: 14193
	private int _locoLayer = -1;

	// Token: 0x04003772 RID: 14194
	private Transform _cammountT;

	// Token: 0x04003773 RID: 14195
	private Transform _uspeakT;

	// Token: 0x04003774 RID: 14196
	private Vector3 _uspeakLocalPos;

	// Token: 0x04003775 RID: 14197
	private Quaternion _uspeakLocalRot;

	// Token: 0x04003776 RID: 14198
	private Transform _emojiT;

	// Token: 0x04003777 RID: 14199
	private Vector3 _emojiLocalPos;

	// Token: 0x04003778 RID: 14200
	private Quaternion _emojiLocalRot;

	// Token: 0x04003779 RID: 14201
	private const float BLEND_OUT_DIST_MULT = 7f;

	// Token: 0x0400377A RID: 14202
	private bool _inited;

	// Token: 0x0400377B RID: 14203
	private bool _culled;

	// Token: 0x0400377C RID: 14204
	private bool _completedUSpeakMove;

	// Token: 0x0400377D RID: 14205
	private bool _reportedUSpeakMoveError;

	// Token: 0x0400377E RID: 14206
	private float _elapsedUSpeakTimeToMove;

	// Token: 0x02000A3F RID: 2623
	public enum IkType
	{
		// Token: 0x04003780 RID: 14208
		None,
		// Token: 0x04003781 RID: 14209
		Limb,
		// Token: 0x04003782 RID: 14210
		ThreeOrFourPoint,
		// Token: 0x04003783 RID: 14211
		SixPoint
	}
}
