using System;
using UnityEngine;
using VRC.Core;

namespace VRC.UI
{
	// Token: 0x02000C23 RID: 3107
	public class PageAvatar : VRCUiPage
	{
		// Token: 0x06006029 RID: 24617 RVA: 0x0021D838 File Offset: 0x0021BC38
		public override void Awake()
		{
			base.Awake();
			this.avatar.gameObject.SetActive(false);
			this.categories = base.GetComponentsInChildren<UiAvatarList>(true);
			foreach (UiAvatarList uiAvatarList in this.categories)
			{
				uiAvatarList.myPage = this;
				if (uiAvatarList.category == UiAvatarList.Category.Internal)
				{
					this.devList = uiAvatarList;
				}
			}
		}

		// Token: 0x0600602A RID: 24618 RVA: 0x0021D8A4 File Offset: 0x0021BCA4
		private void CheckAndEnableDeveloperList()
		{
			if (Player.Instance != null)
			{
				bool isInternal = Player.Instance.isInternal;
				this.devList.gameObject.SetActive(isInternal);
				if (isInternal)
				{
					this.alreadyDev = true;
				}
			}
			this.devCheckTimer = 30f;
		}

		// Token: 0x0600602B RID: 24619 RVA: 0x0021D8F8 File Offset: 0x0021BCF8
		public void ChangeToSelectedAvatar()
		{
			if (this.avatar.apiAvatar != User.CurrentUser.apiAvatar)
			{
				User.CurrentUser.SetCurrentAvatar(this.avatar.apiAvatar);
				this.changedPreview = false;
			}
			VRCUiManager.Instance.CloseUi(false);
		}

		// Token: 0x0600602C RID: 24620 RVA: 0x0021D946 File Offset: 0x0021BD46
		public void ChangedPreviewAvatar()
		{
			this.changedPreview = true;
		}

		// Token: 0x0600602D RID: 24621 RVA: 0x0021D94F File Offset: 0x0021BD4F
		private new void OnDisable()
		{
			if (APIUser.IsLoggedIn && User.CurrentUser.apiAvatar != null && this.changedPreview)
			{
				this.avatar.Refresh(User.CurrentUser.apiAvatar);
			}
		}

		// Token: 0x0600602E RID: 24622 RVA: 0x0021D98C File Offset: 0x0021BD8C
		public override void Update()
		{
			base.Update();
			if (!this.avatar.gameObject.activeSelf && this.currentlyShown && (double)this.screen.alpha == 1.0)
			{
				this.avatar.gameObject.SetActive(true);
			}
			else if (this.avatar.gameObject.activeSelf && !this.currentlyShown)
			{
				this.avatar.gameObject.SetActive(false);
			}
			if (APIUser.IsLoggedIn && User.CurrentUser.apiAvatar != null && !this.isInitialized)
			{
				this.avatar.Refresh(User.CurrentUser.apiAvatar);
				this.isInitialized = true;
			}
			if (this.isInitialized && !this.alreadyDev)
            {
                this.CheckAndEnableDeveloperList();
			}
		}

		// Token: 0x040045E0 RID: 17888
		public SimpleAvatarPedestal avatar;

		// Token: 0x040045E1 RID: 17889
		private bool isInitialized;

		// Token: 0x040045E2 RID: 17890
		private UiAvatarList[] categories;

		// Token: 0x040045E3 RID: 17891
		private UiAvatarList devList;

		// Token: 0x040045E4 RID: 17892
		private bool alreadyDev;

		// Token: 0x040045E5 RID: 17893
		private const float CHECK_PERIOD = 30f;

		// Token: 0x040045E6 RID: 17894
		private float devCheckTimer;

		// Token: 0x040045E7 RID: 17895
		private bool changedPreview;
	}
}
