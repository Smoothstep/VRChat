using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.Dynamics;
using UnityEngine;

// Token: 0x02000A4A RID: 2634
public class RagdollController : MonoBehaviour
{
	// Token: 0x17000BC7 RID: 3015
	// (get) Token: 0x06004F6A RID: 20330 RVA: 0x001AD65A File Offset: 0x001ABA5A
	public bool IsRagDolled
	{
		get
		{
			return this.isRagDolled;
		}
	}

	// Token: 0x06004F6B RID: 20331 RVA: 0x001AD662 File Offset: 0x001ABA62
	public void Prepare()
	{
		if (!this.RagdollMayBeTriggered)
		{
			this.RagdollMayBeTriggered = true;
			this.Initialize(this.avatarAnimator, this.isLocal);
		}
	}

	// Token: 0x06004F6C RID: 20332 RVA: 0x001AD688 File Offset: 0x001ABA88
	public void Initialize(Animator anim, bool local)
	{
		this.rigidBodies.Clear();
		if (anim == null)
		{
			return;
		}
		if (this.RagdollSetup != null)
		{
			base.StopCoroutine(this.RagdollSetup);
		}
		if (this.isRagDolled)
		{
			this.EndRagdoll();
		}
		this.isLocal = local;
		if (anim == null || !anim.isHuman)
		{
			this.avatarAnimator = null;
			this.avatarBase = null;
			this.avatarIk = null;
			return;
		}
		this.avatarAnimator = anim;
		this.avatarBase = anim.gameObject;
		this.avatarManager = this.avatarBase.GetComponentInParent<VRCAvatarManager>();
		this.animationController = base.GetComponent<VRC_AnimationController>();
		this.avatarIk = this.animationController.HeadAndHandsIkController.GetComponent<IkController>();
		this.motion = base.GetComponentInParent<VRCMotionState>();
		if (this.RagdollMayBeTriggered)
		{
			this.RagdollSetup = base.StartCoroutine(this.RagdollSetupCoroutine());
		}
	}

	// Token: 0x06004F6D RID: 20333 RVA: 0x001AD778 File Offset: 0x001ABB78
	private IEnumerator RagdollSetupCoroutine()
	{
		yield return null;
		if (this.avatarAnimator == null)
		{
			this.RagdollSetup = null;
			yield break;
		}
		AnimatorControllerManager acm = base.GetComponent<AnimatorControllerManager>();
		RuntimeAnimatorController rac = this.avatarAnimator.runtimeAnimatorController;
		this.avatarAnimator.runtimeAnimatorController = acm.tPoseController;
		this.avatarAnimator.Update(0f);
		BipedRagdollReferences r = BipedRagdollReferences.FromAvatar(this.avatarAnimator);
		Vector3 headScale = r.head.localScale;
		r.head.localScale = Vector3.one;
		BipedRagdollCreator.Options options = BipedRagdollCreator.AutodetectOptions(r);
		BipedRagdollCreator.Create(r, options);
		r.head.localScale = headScale;
		foreach (Transform transform in r.GetRagdollTransforms())
		{
			if (!(transform == null))
			{
				Rigidbody component = transform.GetComponent<Rigidbody>();
				if (component != null)
				{
					this.rigidBodies.Add(component);
					component.isKinematic = true;
					component.collisionDetectionMode = CollisionDetectionMode.Continuous;
				}
			}
		}
		this.avatarAnimator.runtimeAnimatorController = rac;
		this.avatarAnimator.Update(0f);
		this.RagdollSetup = null;
		yield break;
	}

	// Token: 0x06004F6E RID: 20334 RVA: 0x001AD794 File Offset: 0x001ABB94
	public void Ragdoll()
	{
		if (!this.isRagDolled && this.RagdollSetup == null && this.RagdollMayBeTriggered)
		{
			this.motion.isLocomoting = false;
			if (this.avatarIk != null)
			{
				this.avatarIk.FullIKBlend = 0f;
				this.avatarIk.enabled = false;
			}
			this.avatarBase.transform.SetParent(null, true);
			foreach (Rigidbody rigidbody in this.rigidBodies)
			{
				rigidbody.isKinematic = false;
				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
			}
			this.avatarAnimator.enabled = false;
			if (this.isLocal && this.avatarIk != null)
			{
				this.avatarIk.HeadControl(false);
			}
			this.isRagDolled = true;
		}
	}

	// Token: 0x06004F6F RID: 20335 RVA: 0x001AD8B0 File Offset: 0x001ABCB0
	public void EndRagdoll()
	{
		foreach (Rigidbody rigidbody in this.rigidBodies)
		{
			if (rigidbody != null)
			{
				rigidbody.isKinematic = true;
			}
		}
		this.avatarAnimator.enabled = true;
		if (this.isLocal && this.avatarIk != null)
		{
			this.avatarIk.HeadControl(true);
		}
		this.avatarBase.transform.SetParent(this.avatarManager.transform, false);
		this.avatarBase.transform.localPosition = Vector3.zero;
		this.avatarBase.transform.localRotation = Quaternion.identity;
		if (this.avatarIk != null)
		{
			this.avatarIk.enabled = true;
			this.avatarIk.FullIKBlend = 1f;
			this.avatarIk.InstantReset();
		}
		this.motion.isLocomoting = true;
		this.isRagDolled = false;
	}

	// Token: 0x040037EA RID: 14314
	public static float DeathTime = 5f;

	// Token: 0x040037EB RID: 14315
	private Coroutine RagdollSetup;

	// Token: 0x040037EC RID: 14316
	private bool isRagDolled;

	// Token: 0x040037ED RID: 14317
	private bool isLocal;

	// Token: 0x040037EE RID: 14318
	private GameObject avatarBase;

	// Token: 0x040037EF RID: 14319
	private Animator avatarAnimator;

	// Token: 0x040037F0 RID: 14320
	private VRCAvatarManager avatarManager;

	// Token: 0x040037F1 RID: 14321
	private IkController avatarIk;

	// Token: 0x040037F2 RID: 14322
	private VRCMotionState motion;

	// Token: 0x040037F3 RID: 14323
	private VRC_AnimationController animationController;

	// Token: 0x040037F4 RID: 14324
	private bool RagdollMayBeTriggered;

	// Token: 0x040037F5 RID: 14325
	private List<Rigidbody> rigidBodies = new List<Rigidbody>();
}
