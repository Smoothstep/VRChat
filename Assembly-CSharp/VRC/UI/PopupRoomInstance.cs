using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.Core;

namespace VRC.UI
{
	// Token: 0x02000C2C RID: 3116
	public class PopupRoomInstance : VRCUiPopup
	{
		// Token: 0x060060A4 RID: 24740 RVA: 0x0022081D File Offset: 0x0021EC1D
		public override void Initialize(string title, string body)
		{
			this.ClearRoomInfo();
			base.Initialize(title, string.Empty);
		}

		// Token: 0x060060A5 RID: 24741 RVA: 0x00220834 File Offset: 0x0021EC34
		private void ClearRoomInfo()
		{
			this.mWorld = null;
			this.roomName.text = string.Empty;
			this.publicButton.onClick.RemoveAllListeners();
			this.fogButton.onClick.RemoveAllListeners();
			this.friendsButton.onClick.RemoveAllListeners();
			this.inviteButton.onClick.RemoveAllListeners();
		}

		// Token: 0x060060A6 RID: 24742 RVA: 0x00220898 File Offset: 0x0021EC98
		public void SetupRoomInfo(ApiWorld world, PageWorldInfo worldInfo)
		{
			this.mWorld = world;
			this.mWorldInfo = worldInfo;
			if (this.mWorld.releaseStatus == "private")
			{
				this.isPrivate = true;
			}
			else
			{
				this.isPrivate = false;
			}
			this.roomName.text = world.name;
			bool flag = ModerationManager.Instance.IsBannedFromPublicOnly(APIUser.CurrentUser.id);
			if (this.isPrivate || flag)
			{
				this.publicButton.gameObject.SetActive(false);
			}
			else
			{
				this.publicButton.gameObject.SetActive(true);
				this.publicButton.onClick.AddListener(new UnityAction(this.CreatePublicInstance));
			}
			if (flag)
			{
				this.fogButton.gameObject.SetActive(false);
			}
			else
			{
				this.fogButton.gameObject.SetActive(true);
				this.fogButton.onClick.AddListener(new UnityAction(this.CreateFriendsOfGuestsInstance));
			}
			this.friendsButton.onClick.AddListener(new UnityAction(this.CreateFriendsOnlyInstance));
			this.inviteButton.onClick.AddListener(new UnityAction(this.CreateInviteOnlyInstance));
		}

		// Token: 0x060060A7 RID: 24743 RVA: 0x002209DB File Offset: 0x0021EDDB
		private void CreatePublicInstance()
		{
			this.CreateNewInstanceByAccess(ApiWorld.WorldInstance.AccessType.Public);
		}

		// Token: 0x060060A8 RID: 24744 RVA: 0x002209E4 File Offset: 0x0021EDE4
		private void CreateFriendsOfGuestsInstance()
		{
			this.CreateNewInstanceByAccess(ApiWorld.WorldInstance.AccessType.FriendsOfGuests);
		}

		// Token: 0x060060A9 RID: 24745 RVA: 0x002209ED File Offset: 0x0021EDED
		private void CreateFriendsOnlyInstance()
		{
			this.CreateNewInstanceByAccess(ApiWorld.WorldInstance.AccessType.FriendsOnly);
		}

		// Token: 0x060060AA RID: 24746 RVA: 0x002209F6 File Offset: 0x0021EDF6
		private void CreateInviteOnlyInstance()
		{
			this.CreateNewInstanceByAccess(ApiWorld.WorldInstance.AccessType.InviteOnly);
		}

		// Token: 0x060060AB RID: 24747 RVA: 0x00220A00 File Offset: 0x0021EE00
		private void CreateNewInstanceByAccess(ApiWorld.WorldInstance.AccessType access)
		{
			string tags = ApiWorld.WorldInstance.BuildAccessTags(access, APIUser.CurrentUser.id);
			this.mWorldInfo.SetupWorldInfo(this.mWorld, this.mWorld.GetNewInstance(tags), true, false);
			this.Close();
		}

		// Token: 0x04004648 RID: 17992
		public Text roomName;

		// Token: 0x04004649 RID: 17993
		public Button publicButton;

		// Token: 0x0400464A RID: 17994
		public Button fogButton;

		// Token: 0x0400464B RID: 17995
		public Button friendsButton;

		// Token: 0x0400464C RID: 17996
		public Button inviteButton;

		// Token: 0x0400464D RID: 17997
		private ApiWorld mWorld;

		// Token: 0x0400464E RID: 17998
		private PageWorldInfo mWorldInfo;

		// Token: 0x0400464F RID: 17999
		private Texture defaultImageTexture;

		// Token: 0x04004650 RID: 18000
		private bool isPrivate;
	}
}
