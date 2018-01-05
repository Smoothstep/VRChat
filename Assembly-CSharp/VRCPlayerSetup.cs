using System;
using Photon;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000B47 RID: 2887
public class VRCPlayerSetup : Photon.MonoBehaviour
{
	// Token: 0x06005889 RID: 22665 RVA: 0x001EACC9 File Offset: 0x001E90C9
	private void Awake()
	{
		if (base.photonView.isMine)
		{
			this.SetupLocal();
		}
		else
		{
			this.SetupRemote();
		}
		this.SetupBoth();
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x0600588A RID: 22666 RVA: 0x001EACF8 File Offset: 0x001E90F8
	private void SetupLocal()
	{
		if (base.photonView != null && base.photonView.owner != null)
		{
			base.gameObject.name = string.Concat(new object[]
			{
				"VRCPlayer[Local] ",
				base.photonView.owner.NickName,
				" ",
				base.photonView.owner.ID
			});
		}
		else
		{
			base.gameObject.name = "VRCPlayer[Local]";
		}
		base.gameObject.AddComponent<InteractivePlayer>();
		base.gameObject.AddComponent<InputStateControllerManager>();
		base.gameObject.AddComponent<PlayerWebViewControls>();
		GameObject gameObject = base.transform.Find("SelectRegion").gameObject;
		UnityEngine.Object.Destroy(gameObject);
	}

	// Token: 0x0600588B RID: 22667 RVA: 0x001EADCC File Offset: 0x001E91CC
	private void SetupRemote()
	{
		if (base.photonView != null && base.photonView.owner != null)
		{
			base.gameObject.name = string.Concat(new object[]
			{
				"VRCPlayer[Remote] ",
				base.photonView.owner.NickName,
				" ",
				base.photonView.owner.ID
			});
		}
		else
		{
			base.gameObject.name = "VRCPlayer[Remote]";
		}
		GameObject gameObject = base.transform.Find("SelectRegion").gameObject;
		gameObject.SetActive(true);
	}

	// Token: 0x0600588C RID: 22668 RVA: 0x001EAE7D File Offset: 0x001E927D
	private void SetupBoth()
	{
		base.gameObject.AddComponent<PlayerModManager>();
		base.gameObject.AddComponent<VRCSDK2.Player>();
		base.gameObject.AddComponent<VRC.Player>();
		base.gameObject.AddComponent<InternalSDKPlayer>();
	}
}
