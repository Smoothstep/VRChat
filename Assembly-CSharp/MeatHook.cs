using System;
using UnityEngine;

// Token: 0x02000A44 RID: 2628
public class MeatHook : VRCAnimationLayer
{
	// Token: 0x06004F43 RID: 20291 RVA: 0x001AC369 File Offset: 0x001AA769
	public override void Initialize(bool local, Transform avatarRoot, Transform cameraRoot)
	{
		this._local = local;
		this._avatarRoot = avatarRoot;
		this._cameraRoot = cameraRoot;
		if (this._avatarRoot != null)
		{
			this._switcher = this._avatarRoot.GetComponent<VRCAvatarManager>();
		}
	}

	// Token: 0x06004F44 RID: 20292 RVA: 0x001AC3A4 File Offset: 0x001AA7A4
	public override void Attach(Animator a)
	{
		if (a == null || !a.isHuman)
		{
			this.avatarHead = null;
			this._hmdPivot = null;
			return;
		}
		this.avatarHead = a.GetBoneTransform(HumanBodyBones.Head);
		if (this.avatarHead)
		{
			this._hmdPivot = this.avatarHead.Find("HmdPivot");
		}
		if (VRCPlayer.Instance != null)
		{
			this._motion = VRCPlayer.Instance.GetComponent<VRCMotionState>();
		}
	}

	// Token: 0x06004F45 RID: 20293 RVA: 0x001AC42B File Offset: 0x001AA82B
	public override void Detach()
	{
		this._hmdPivot = null;
	}

	// Token: 0x06004F46 RID: 20294 RVA: 0x001AC434 File Offset: 0x001AA834
	public override VRCAnimationLayer.LimbSet GetLimbSet()
	{
		return VRCAnimationLayer.LimbSet.Root;
	}

	// Token: 0x06004F47 RID: 20295 RVA: 0x001AC43C File Offset: 0x001AA83C
	public override void Apply(bool usingFinalIK)
	{
		if (!this._local || this._hmdPivot == null)
		{
			if (this._avatarRoot != null)
			{
				this._avatarRoot.localPosition = Vector3.zero;
			}
			return;
		}
		if (VRCTrackingManager.IsPlayerNearTracking() && this._switcher != null && this._switcher.WasMeasured)
		{
			Vector3 worldCameraNeckPos = VRCVrCamera.GetInstance().GetWorldCameraNeckPos();
			this.neckPivot = worldCameraNeckPos;
			Vector3 b = worldCameraNeckPos - this.avatarHead.position;
			if (VRCPlayer.Instance != null && VRCPlayer.Instance.GetVRMode())
			{
				if (this._motion.isLocomoting || this._motion.StandingHeight < LocomotionInputController.ProneSpeedStart)
				{
					if (usingFinalIK)
					{
						b.y = 0f;
					}
					this._avatarRoot.position += b;
					return;
				}
			}
			else if (this._motion.isLocomoting)
			{
				if (usingFinalIK)
				{
					b.y = 0f;
				}
				this._avatarRoot.position += b;
				return;
			}
		}
		this._avatarRoot.localPosition = Vector3.zero;
	}

	// Token: 0x06004F48 RID: 20296 RVA: 0x001AC594 File Offset: 0x001AA994
	private void OnDrawGizmos()
	{
		if (this._cameraRoot == null)
		{
			return;
		}
		Gizmos.DrawSphere(this._cameraRoot.position, 0.01f);
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(this.neckPivot, 0.01f);
	}

	// Token: 0x040037B8 RID: 14264
	private bool _local;

	// Token: 0x040037B9 RID: 14265
	private Transform _avatarRoot;

	// Token: 0x040037BA RID: 14266
	private Transform _hmdPivot;

	// Token: 0x040037BB RID: 14267
	private Transform _cameraRoot;

	// Token: 0x040037BC RID: 14268
	private Transform avatarHead;

	// Token: 0x040037BD RID: 14269
	private VRCMotionState _motion;

	// Token: 0x040037BE RID: 14270
	private VRCAvatarManager _switcher;

	// Token: 0x040037BF RID: 14271
	private Vector3 neckPivot;
}
