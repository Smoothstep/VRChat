using System;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000ACA RID: 2762
public class PlayerModComponentProp : MonoBehaviour, IPlayerModComponent
{
	// Token: 0x060053F4 RID: 21492 RVA: 0x001CFBE4 File Offset: 0x001CDFE4
	private void Awake()
	{
		this._animationController = base.GetComponentInChildren<VRC_AnimationController>();
		this._animControllerManager = base.GetComponentInChildren<AnimatorControllerManager>();
	}

	// Token: 0x060053F5 RID: 21493 RVA: 0x001CFBFE File Offset: 0x001CDFFE
	private void OnDestroy()
	{
		this.ResetModdedVariables();
	}

	// Token: 0x060053F6 RID: 21494 RVA: 0x001CFC08 File Offset: 0x001CE008
	public void SetProperties(List<VRCPlayerModProperty> properties)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (VRCPlayerModProperty vrcplayerModProperty in properties)
		{
			dictionary[vrcplayerModProperty.name] = vrcplayerModProperty.value();
		}
		this.propPrefab = (GameObject)Tools.GetOrDefaultFromDictionary(dictionary, "PropPrefab", null);
		this.propAnimations = (RuntimeAnimatorController)Tools.GetOrDefaultFromDictionary(dictionary, "PropAnimations", null);
		VRC_AvatarDescriptor componentInChildren = base.GetComponentInChildren<VRC_AvatarDescriptor>();
		if (componentInChildren == null)
		{
			return;
		}
		this._avatar = componentInChildren.gameObject;
		Animator component = this._avatar.GetComponent<Animator>();
		if (component.isHuman)
		{
			if (this._animator == null)
			{
				this._animator = component;
				if (this.propAnimations != null)
				{
					this._animControllerManager.Push(this.propAnimations);
				}
			}
			else if (this._animator != component)
			{
				this._animator = component;
			}
			if (this.propPrefab != null)
			{
				this.propInstance = (GameObject)AssetManagement.Instantiate(this.propPrefab);
				this.propApi = this.propInstance.GetComponent<VRC_PropApi>();
				this.propControls = this.propInstance.GetComponent<VRC_PropController>();
				if (this.propControls != null)
				{
					this.propControls.controllingPlayer = base.GetComponent<VRC_PlayerApi>();
				}
				HumanBodyBones humanBodyBones = this.propApi.mountPoint;
				this.mountPoint = this._animator.GetBoneTransform(humanBodyBones);
				Quaternion tposeRotation = this._animationController.GetTPoseRotation(humanBodyBones);
				Quaternion localRotation = Quaternion.Inverse(tposeRotation) * this.propInstance.transform.rotation;
				this.propInstance.transform.parent = this.mountPoint;
				this.propInstance.transform.localPosition = Vector3.zero;
				this.propInstance.transform.localRotation = localRotation;
			}
		}
	}

	// Token: 0x060053F7 RID: 21495 RVA: 0x001CFE28 File Offset: 0x001CE228
	private void ResetModdedVariables()
	{
		this._animator = null;
		if (this.propAnimations != null)
		{
			this._animControllerManager.Pop();
		}
		this.propApi = null;
		UnityEngine.Object.Destroy(this.propInstance);
		this.propInstance = null;
	}

	// Token: 0x04003B39 RID: 15161
	private GameObject propPrefab;

	// Token: 0x04003B3A RID: 15162
	private RuntimeAnimatorController propAnimations;

	// Token: 0x04003B3B RID: 15163
	private GameObject propInstance;

	// Token: 0x04003B3C RID: 15164
	private VRC_PropApi propApi;

	// Token: 0x04003B3D RID: 15165
	private VRC_PropController propControls;

	// Token: 0x04003B3E RID: 15166
	private Transform mountPoint;

	// Token: 0x04003B3F RID: 15167
	private Animator _animator;

	// Token: 0x04003B40 RID: 15168
	private GameObject _avatar;

	// Token: 0x04003B41 RID: 15169
	private VRC_AnimationController _animationController;

	// Token: 0x04003B42 RID: 15170
	private AnimatorControllerManager _animControllerManager;
}
