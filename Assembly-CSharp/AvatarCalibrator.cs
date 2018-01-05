using System;
using UnityEngine;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000B54 RID: 2900
public class AvatarCalibrator : MonoBehaviour
{
	// Token: 0x060058D9 RID: 22745 RVA: 0x001EC620 File Offset: 0x001EAA20
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
		this.calibrator = base.GetComponentInParent<VRC_AvatarCalibrator>();
		this.switcher = base.GetComponent<VRCAvatarManager>();
		VRCAvatarManager vrcavatarManager = this.switcher;
		vrcavatarManager.OnAvatarCreated = (VRCAvatarManager.AvatarCreationCallback)Delegate.Combine(vrcavatarManager.OnAvatarCreated, new VRCAvatarManager.AvatarCreationCallback(this.AvatarCreated));
		this.vrikCalib = base.GetComponent<VRCVrIkCalibrator>();
		if (!string.IsNullOrEmpty(this.calibrator.blueprintId))
		{
			ApiAvatar.Fetch(this.calibrator.blueprintId, delegate(ApiAvatar bp)
			{
				this.avatar = bp;
				this.switcher.SwitchAvatar(this.avatar, this.calibrator.scale, null);
				Tools.SetLayerRecursively(base.gameObject, base.transform.parent.gameObject.layer, -1);
			}, delegate(string message)
			{
				this.switcher.SwitchToErrorAvatar(1f);
			});
		}
	}

	// Token: 0x060058DA RID: 22746 RVA: 0x001EC737 File Offset: 0x001EAB37
	public void UpdateHeight()
	{
		if (this.vrikCalib != null)
		{
			this.vrikCalib.SetEyeHeight(this.calibrator.userEyeHeight);
		}
	}

	// Token: 0x060058DB RID: 22747 RVA: 0x001EC760 File Offset: 0x001EAB60
	private void SetAvatarUse(int Instigator)
	{
		if (this.calibrator.ChangeAvatarsOnUse)
		{
			VRC.Core.Logger.Log("Setting local user avatar to " + this.avatar.id, DebugLevel.Always);
			User.CurrentUser.SetCurrentAvatar(this.avatar);
		}
	}

	// Token: 0x060058DC RID: 22748 RVA: 0x001EC7A0 File Offset: 0x001EABA0
	private void AvatarCreated(GameObject avatar, VRC_AvatarDescriptor Desc, bool loaded)
	{
		this.switcher = base.GetComponent<VRCAvatarManager>();
		if (this.switcher.profile != null)
		{
		}
		VRC_KeyEvents[] componentsInChildren = avatar.GetComponentsInChildren<VRC_KeyEvents>();
		foreach (VRC_KeyEvents vrc_KeyEvents in componentsInChildren)
		{
			vrc_KeyEvents.enabled = false;
		}
		Tools.SetLayerRecursively(base.gameObject, base.transform.parent.gameObject.layer, -1);
		Animator component = this.switcher.currentAvatar.GetComponent<Animator>();
		this.vrikCalib.Initialize(component, this.switcher.currentAvatarDescriptor.ViewPosition.y);
	}

	// Token: 0x04003F93 RID: 16275
	private VRCAvatarManager switcher;

	// Token: 0x04003F94 RID: 16276
	private VRCVrIkCalibrator vrikCalib;

	// Token: 0x04003F95 RID: 16277
	private VRC_AvatarCalibrator calibrator;

	// Token: 0x04003F96 RID: 16278
	private ApiAvatar avatar;
}
