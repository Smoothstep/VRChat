using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC;
using VRC.Core;

// Token: 0x02000C75 RID: 3189
public class VRCUiPageEditPlayer : VRCUiSubPage
{
	// Token: 0x0600630B RID: 25355 RVA: 0x00233C50 File Offset: 0x00232050
	public override void OnEnable()
	{
		base.OnEnable();
		Text componentInChildren = this.setHeightButton.GetComponentInChildren<Text>();
		float playerHeight = VRCTrackingManager.GetPlayerHeight();
		componentInChildren.text = string.Format("Player Height: {0:F2} m", playerHeight);
		if (!this.isInitialized)
		{
			this.changeAvatarButton.onClick.AddListener(new UnityAction(this.LaunchAvatarChooserPopup));
			this.newAvatarButton.onClick.AddListener(new UnityAction(this.CreateBlueprint));
			this.setHeightButton.onClick.AddListener(new UnityAction(this.SetHeight));
			this.isInitialized = true;
		}
		if (APIUser.IsLoggedIn && this.avatarPedestal.avatarUrl != User.CurrentUser.apiAvatar.assetUrl)
		{
			this.avatarPedestal.Refresh(User.CurrentUser.apiAvatar.assetUrl);
		}
	}

	// Token: 0x0600630C RID: 25356 RVA: 0x00233D39 File Offset: 0x00232139
	public void LaunchAvatarChooserPopup()
	{
		Debug.LogError("This screeen is depricated");
	}

	// Token: 0x0600630D RID: 25357 RVA: 0x00233D45 File Offset: 0x00232145
	private void CreateBlueprint()
	{
		VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Screens/Profile/CreateBlueprint");
	}

	// Token: 0x0600630E RID: 25358 RVA: 0x00233D58 File Offset: 0x00232158
	public void SetHeight()
	{
		Text componentInChildren = this.setHeightButton.GetComponentInChildren<Text>();
		float num = VRCTrackingManager.MeasurePlayerHeight();
		componentInChildren.text = string.Format("Player Height: {0:F2} m", num);
	}

	// Token: 0x04004896 RID: 18582
	public SimpleAvatarPedestal avatarPedestal;

	// Token: 0x04004897 RID: 18583
	public Button changeAvatarButton;

	// Token: 0x04004898 RID: 18584
	public Button newAvatarButton;

	// Token: 0x04004899 RID: 18585
	public Button setHeightButton;

	// Token: 0x0400489A RID: 18586
	private bool isInitialized;
}
