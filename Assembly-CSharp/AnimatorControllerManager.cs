using System;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000A51 RID: 2641
public class AnimatorControllerManager : MonoBehaviour
{
	// Token: 0x17000BDD RID: 3037
	// (get) Token: 0x06005002 RID: 20482 RVA: 0x001B5539 File Offset: 0x001B3939
	public Animator avatarAnimator
	{
		get
		{
			return this.mAvatarAnimator;
		}
	}

	// Token: 0x06005003 RID: 20483 RVA: 0x001B5544 File Offset: 0x001B3944
	public void Initialize(Animator animator, VRC_AvatarDescriptor Ad, bool local)
	{
		VRC_AnimationController component = base.GetComponent<VRC_AnimationController>();
		this.avatarManager = base.transform.parent.GetComponentInChildren<VRCAvatarManager>();
		this.avatarAnimation = base.GetComponent<AvatarAnimation>();
		this.player = base.transform.parent.GetComponent<VRC.Player>();
		this.mInitialized = true;
		this.mAvatarAnimator = animator;
		this.mAnimatorControllerStack = new List<RuntimeAnimatorController>();
		if (animator.isHuman)
		{
			this.Push(this.tPoseController);
			animator.updateMode = AnimatorUpdateMode.UnscaledTime;
			animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			animator.Update(0.01f);
			animator.transform.localPosition = Vector3.zero;
			animator.transform.localRotation = Quaternion.identity;
			if (component != null)
			{
				component.RecordTPose(animator);
			}
			this.avatarManager.MeasureHumanAvatar(animator);
			animator.updateMode = AnimatorUpdateMode.Normal;
			if (!local)
			{
				animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
			}
		}
		this.Finalize(animator, Ad, local);
	}

	// Token: 0x06005004 RID: 20484 RVA: 0x001B5638 File Offset: 0x001B3A38
	public void Finalize(Animator animator, VRC_AvatarDescriptor Ad, bool local)
	{
		VRC_AnimationController component = base.GetComponent<VRC_AnimationController>();
		component.Reset(animator, Ad);
		if (animator.isHuman)
		{
			if (this.avatarManager != null && this.player != null)
			{
				this.Push(this.avatarManager.GetStandAnimController());
				if (this.avatarAnimation != null && this.avatarAnimation.motionState.IsSeated && this.player.currentStation != null)
				{
					this.player.currentStation.ApplySeatedAnimation(this.player);
				}
				component.GenerateLocalHandCollision();
			}
			else
			{
				this.Push(animator.runtimeAnimatorController);
			}
		}
		else
		{
			this.Push(animator.runtimeAnimatorController);
		}
	}

	// Token: 0x06005005 RID: 20485 RVA: 0x001B570C File Offset: 0x001B3B0C
	public void Push(RuntimeAnimatorController animatorController)
	{
		if (this.mInitialized)
		{
			if (this.mAvatarAnimator != null)
			{
				if (this.mAvatarAnimator.isHuman)
				{
					this.mAvatarAnimator.runtimeAnimatorController = animatorController;
					this.mAvatarAnimator.Update(0f);
					this.mAnimatorControllerStack.Add(animatorController);
					this.mAvatarAnimator.transform.localPosition = Vector3.zero;
					this.mAvatarAnimator.transform.localRotation = Quaternion.identity;
				}
			}
			else
			{
				Debug.LogError("Can't push runtime controller onto a null avatar animator.");
			}
		}
		else
		{
			Debug.LogError("Cannot push animator controller. AnimatorController not initialized yet.");
		}
	}

	// Token: 0x06005006 RID: 20486 RVA: 0x001B57B8 File Offset: 0x001B3BB8
	public void Pop()
	{
		if (this.mInitialized)
		{
			if (this.mAvatarAnimator != null)
			{
				if (this.mAvatarAnimator.isHuman)
				{
					if (this.mAnimatorControllerStack.Count > 1)
					{
						this.mAnimatorControllerStack.RemoveAt(this.mAnimatorControllerStack.Count - 1);
						RuntimeAnimatorController runtimeAnimatorController = this.mAnimatorControllerStack[this.mAnimatorControllerStack.Count - 1];
						this.mAvatarAnimator.runtimeAnimatorController = runtimeAnimatorController;
						this.mAvatarAnimator.Update(0f);
						this.mAvatarAnimator.transform.localPosition = Vector3.zero;
						this.mAvatarAnimator.transform.localRotation = Quaternion.identity;
					}
					else
					{
						Debug.LogError("Trying to pop last (default) animator controller from the stack.");
					}
				}
			}
			else
			{
				Debug.LogError("Can't push runtime controller onto a null avatar animator.");
			}
		}
		else
		{
			Debug.LogError("Cannot pop animator controller. AnimatorController not initialized yet.");
		}
	}

	// Token: 0x06005007 RID: 20487 RVA: 0x001B58A8 File Offset: 0x001B3CA8
	public RuntimeAnimatorController Peek()
	{
		RuntimeAnimatorController result = null;
		if (this.mInitialized && this.mAnimatorControllerStack.Count > 0)
		{
			result = this.mAnimatorControllerStack[this.mAnimatorControllerStack.Count - 1];
		}
		return result;
	}

	// Token: 0x040038D7 RID: 14551
	private List<RuntimeAnimatorController> mAnimatorControllerStack;

	// Token: 0x040038D8 RID: 14552
	private Animator mAvatarAnimator;

	// Token: 0x040038D9 RID: 14553
	private bool mInitialized;

	// Token: 0x040038DA RID: 14554
	public RuntimeAnimatorController tPoseController;

	// Token: 0x040038DB RID: 14555
	private VRCAvatarManager avatarManager;

	// Token: 0x040038DC RID: 14556
	private AvatarAnimation avatarAnimation;

	// Token: 0x040038DD RID: 14557
	private VRC.Player player;
}
