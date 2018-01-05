using System;
using System.Collections;
using RootMotion.FinalIK;
using UnityEngine;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000B55 RID: 2901
public class AvatarPedestal : MonoBehaviour
{
	// Token: 0x060058E0 RID: 22752 RVA: 0x001EC8BC File Offset: 0x001EACBC
	private void Start()
	{
		VRC_EventHandler componentInParent = base.GetComponentInParent<VRC_EventHandler>();
		if (componentInParent != null)
		{
			for (int i = 0; i < componentInParent.Events.Count; i++)
			{
				if (componentInParent.Events[i].Name == "Use" && componentInParent.Events[i].ParameterObject == null)
				{
					componentInParent.Events[i].ParameterObject = base.gameObject;
				}
			}
		}
		this.pedestal = base.GetComponentInParent<VRC_AvatarPedestal>();
		this.switcher = base.GetComponent<VRCAvatarManager>();
		VRCAvatarManager vrcavatarManager = this.switcher;
		vrcavatarManager.OnAvatarCreated = (VRCAvatarManager.AvatarCreationCallback)Delegate.Combine(vrcavatarManager.OnAvatarCreated, new VRCAvatarManager.AvatarCreationCallback(this.AvatarCreated));
		this.refreshId();
	}

	// Token: 0x060058E1 RID: 22753 RVA: 0x001EC990 File Offset: 0x001EAD90
	private void refreshId()
	{
		if (!string.IsNullOrEmpty(this.pedestal.blueprintId))
		{
			ApiAvatar.Fetch(this.pedestal.blueprintId, delegate(ApiAvatar bp)
			{
				this.avatar = bp;
				this.switcher.SwitchAvatar(this.avatar, this.pedestal.scale, null);
				Tools.SetLayerRecursively(base.gameObject, base.transform.parent.gameObject.layer, -1);
			}, delegate(string message)
			{
				this.switcher.SwitchToErrorAvatar(1f);
				this.loadedErrorAvatar = true;
			});
			this.currId = this.pedestal.blueprintId;
		}
	}

	// Token: 0x060058E2 RID: 22754 RVA: 0x001EC9EC File Offset: 0x001EADEC
	private void Update()
	{
		if (this.pedestal == null || this.switcher == null)
		{
			return;
		}
		if (this.currId != this.pedestal.blueprintId)
		{
			this.refreshId();
		}
	}

	// Token: 0x060058E3 RID: 22755 RVA: 0x001ECA40 File Offset: 0x001EAE40
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	public void RefreshAvatar(VRC.Player sender)
	{
		if (this.switcher != null && VRC.Network.IsOwner(sender, this.switcher.gameObject))
		{
			if (this.switcher.profile != null)
			{
				this.switcher.profile.transform.position = base.transform.position + new Vector3(0f, 2f, 0f);
			}
			VRCAvatarManager vrcavatarManager = this.switcher;
			vrcavatarManager.OnAvatarCreated = (VRCAvatarManager.AvatarCreationCallback)Delegate.Combine(vrcavatarManager.OnAvatarCreated, new VRCAvatarManager.AvatarCreationCallback(this.AvatarCreated));
			Debug.Log("Refreshing avatar w/ scale: " + this.pedestal.scale);
			this.switcher.SwitchAvatar(this.avatar, this.pedestal.scale, null);
			Tools.SetLayerRecursively(base.gameObject, base.transform.parent.gameObject.layer, -1);
		}
	}

	// Token: 0x060058E4 RID: 22756 RVA: 0x001ECB48 File Offset: 0x001EAF48
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.Local
	})]
	private void SetAvatarUse(VRC.Player instigator)
	{
		if (this.avatar == null && !this.loadedErrorAvatar)
		{
			base.StartCoroutine(this.SetAvatarUseCoroutine());
			return;
		}
		if (this.pedestal.ChangeAvatarsOnUse && this.avatar != null)
		{
			User.CurrentUser.SetCurrentAvatar(this.avatar);
		}
	}

	// Token: 0x060058E5 RID: 22757 RVA: 0x001ECBA4 File Offset: 0x001EAFA4
	private IEnumerator SetAvatarUseCoroutine()
	{
		while (this.avatar == null && !this.loadedErrorAvatar)
		{
			yield return null;
		}
		this.SetAvatarUse(VRC.Network.LocalPlayer);
		yield break;
	}

	// Token: 0x060058E6 RID: 22758 RVA: 0x001ECBC0 File Offset: 0x001EAFC0
	private void AvatarCreated(GameObject avatar, VRC_AvatarDescriptor Desc, bool loaded)
	{
		this.switcher = base.GetComponent<VRCAvatarManager>();
		if (this.switcher.profile != null)
		{
		}
		Tools.SetLayerRecursively(base.gameObject, base.transform.parent.gameObject.layer, -1);
		this.SetAvatarPose();
	}

	// Token: 0x060058E7 RID: 22759 RVA: 0x001ECC18 File Offset: 0x001EB018
	private void SetAvatarPose()
	{
		Animator componentInChildren = base.GetComponentInChildren<Animator>();
		VRC_AvatarDescriptor componentInChildren2 = base.GetComponentInChildren<VRC_AvatarDescriptor>();
		if (componentInChildren2 == null)
		{
			componentInChildren.runtimeAnimatorController = this.switcher.animatorControllerMale;
		}
		else if (componentInChildren2.Animations == VRC_AvatarDescriptor.AnimationSet.Female)
		{
			componentInChildren.runtimeAnimatorController = this.switcher.animatorControllerFemale;
		}
		else
		{
			componentInChildren.runtimeAnimatorController = this.switcher.animatorControllerMale;
		}
		int layerIndex = componentInChildren.GetLayerIndex("Idle Layer");
		int layerIndex2 = componentInChildren.GetLayerIndex("Locomotion Layer");
		int layerIndex3 = componentInChildren.GetLayerIndex("HandLeft");
		int layerIndex4 = componentInChildren.GetLayerIndex("HandRight");
		if (layerIndex >= 0)
		{
			componentInChildren.SetLayerWeight(layerIndex, 1f);
		}
		if (layerIndex2 >= 0)
		{
			componentInChildren.SetLayerWeight(layerIndex2, 0f);
		}
		if (layerIndex3 >= 0)
		{
			componentInChildren.SetLayerWeight(layerIndex3, 0f);
		}
		if (layerIndex4 >= 0)
		{
			componentInChildren.SetLayerWeight(layerIndex4, 0f);
		}
		componentInChildren.SetFloat("HeightScale", 1f);
		componentInChildren.SetFloat("HeightScaleNOMOVE", 1f);
		VRIK component = componentInChildren.GetComponent<VRIK>();
		if (component != null)
		{
			component.enabled = false;
		}
		global::LimbIK component2 = componentInChildren.GetComponent<global::LimbIK>();
		if (component2 != null)
		{
			component2.enabled = false;
		}
	}

	// Token: 0x04003F97 RID: 16279
	private VRCAvatarManager switcher;

	// Token: 0x04003F98 RID: 16280
	private VRC_AvatarPedestal pedestal;

	// Token: 0x04003F99 RID: 16281
	private ApiAvatar avatar;

	// Token: 0x04003F9A RID: 16282
	private bool loadedErrorAvatar;

	// Token: 0x04003F9B RID: 16283
	private string currId;
}
