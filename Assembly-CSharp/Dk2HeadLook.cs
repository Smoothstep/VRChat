using System;
using UnityEngine;

// Token: 0x02000A32 RID: 2610
public class Dk2HeadLook : MonoBehaviour
{
	// Token: 0x06004E8F RID: 20111 RVA: 0x001A5710 File Offset: 0x001A3B10
	public void Initialize(Animator animator, bool local)
	{
		if (animator == null)
		{
			this._local = local;
			this._animator = null;
			this._neck = null;
			this._initialNeckRotation = Quaternion.identity;
			return;
		}
		this._local = local;
		this._animator = animator;
		this._neck = animator.GetBoneTransform(HumanBodyBones.Head);
		if (this._neck != null)
		{
			this._initialNeckRotation = Quaternion.Inverse(base.transform.rotation) * this._neck.rotation;
		}
	}

	// Token: 0x06004E90 RID: 20112 RVA: 0x001A57A0 File Offset: 0x001A3BA0
	private void LateUpdate()
	{
		if (this._neck == null)
		{
			return;
		}
		if (this._local && VRCTrackingManager.IsPlayerNearTracking())
		{
			this.HeadRot = Quaternion.Inverse(base.transform.rotation) * VRCVrCamera.GetInstance().GetWorldCameraRot();
		}
		if (VRCTrackingManager.IsPlayerNearTracking())
		{
			this._neck.rotation = base.transform.rotation * this.HeadRot * this._initialNeckRotation;
		}
	}

	// Token: 0x040036AF RID: 13999
	public Quaternion HeadRot = Quaternion.identity;

	// Token: 0x040036B0 RID: 14000
	private Animator _animator;

	// Token: 0x040036B1 RID: 14001
	private Transform _neck;

	// Token: 0x040036B2 RID: 14002
	private bool _local;

	// Token: 0x040036B3 RID: 14003
	private Quaternion _initialNeckRotation;
}
