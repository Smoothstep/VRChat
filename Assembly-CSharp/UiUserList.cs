using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.UI;

// Token: 0x02000C4E RID: 3150
public class UiUserList : UiVRCList
{
	// Token: 0x060061A1 RID: 24993 RVA: 0x002273B4 File Offset: 0x002257B4
	protected override void FetchAndRenderElements(int page)
	{
		List<ApiModel> am = new List<ApiModel>();
		switch (this.listType)
		{
		case UiUserList.ListType.InWorld:
			if (RoomManager.inRoom)
			{
				Player[] allPlayers = PlayerManager.GetAllPlayers();
				List<string> inRoomUserIds = (from p in allPlayers
				select p.userId).ToList<string>();
				PlayerManager.FetchUsersInWorldInstance(delegate(List<APIUser> users)
				{
					foreach (APIUser apiuser in users)
					{
						Player player = PlayerManager.GetPlayer(apiuser.id);
						bool flag = player != null && player.vrcPlayer != null && player.vrcPlayer.isInvisible;
						if (inRoomUserIds.Contains(apiuser.id) && !flag)
						{
							am.Add(apiuser);
						}
					}
					this.RenderPaginatedUsers(am.Cast<APIUser>().ToList<APIUser>(), page);
				}, delegate(string message)
				{
					Debug.LogError("Could not fetch users in current world - " + message);
				});
			}
			break;
		case UiUserList.ListType.FriendRequests:
			ApiNotification.FetchAll(ApiNotification.NotificationType.Friendrequest, false, string.Empty, delegate(List<ApiNotification> notifications)
			{
				foreach (ApiNotification item in notifications)
				{
					am.Add(item);
				}
				this.RenderPaginatedNotifs(am.Cast<ApiNotification>().ToList<ApiNotification>(), page);
			}, delegate(string message)
			{
				Debug.LogError("Could not fetch users in current world - " + message);
			});
			break;
		case UiUserList.ListType.OnlineFriends:
			APIUser.FetchFriends(delegate(List<APIUser> users)
			{
				foreach (APIUser apiuser in users)
				{
					Player player = PlayerManager.GetPlayer(apiuser.id);
					bool flag = player != null && player.vrcPlayer != null && player.vrcPlayer.isInvisible;
					if (apiuser.location != "offline" && !flag)
					{
						am.Add(apiuser);
					}
				}
				this.RenderPaginatedUsers(am.Cast<APIUser>().ToList<APIUser>(), page);
			}, delegate(string message)
			{
				Debug.LogError("Could not fetch users in current world - " + message);
			});
			break;
		case UiUserList.ListType.OfflineFriends:
			APIUser.FetchFriends(delegate(List<APIUser> users)
			{
				foreach (APIUser apiuser in users)
				{
					Player player = PlayerManager.GetPlayer(apiuser.id);
					bool flag = player != null && player.vrcPlayer != null && player.vrcPlayer.isInvisible;
					if (apiuser.location == "offline" || flag)
					{
						am.Add(apiuser);
					}
				}
				this.RenderPaginatedUsers(am.Cast<APIUser>().ToList<APIUser>(), page);
			}, delegate(string message)
			{
				Debug.LogError("Could not fetch users in current world - " + message);
			});
			break;
		case UiUserList.ListType.HelpRequests:
			ApiNotification.FetchAll(ApiNotification.NotificationType.Halp, false, string.Empty, delegate(List<ApiNotification> notifications)
			{
				foreach (ApiNotification item in notifications)
				{
					am.Add(item);
				}
				this.RenderPaginatedNotifs(am.Cast<ApiNotification>().ToList<ApiNotification>(), page);
			}, delegate(string message)
			{
				Debug.LogError("Could not fetch users in current world - " + message);
			});
			break;
		case UiUserList.ListType.Search:
			APIUser.FetchUsers(this.searchQuery, delegate(List<APIUser> users)
			{
				foreach (APIUser item in users)
				{
					am.Add(item);
				}
				this.RenderPaginatedUsers(am.Cast<APIUser>().ToList<APIUser>(), page);
			}, delegate(string message)
			{
				Debug.LogError("Could not fetch users in current world - " + message);
			});
			break;
		}
	}

	// Token: 0x060061A2 RID: 24994 RVA: 0x002275A1 File Offset: 0x002259A1
	private void RenderPaginatedUsers(List<APIUser> users, int page)
	{
		base.RenderElements<APIUser>(users, page);
	}

	// Token: 0x060061A3 RID: 24995 RVA: 0x002275AB File Offset: 0x002259AB
	private void RenderPaginatedNotifs(List<ApiNotification> notifs, int page)
	{
		base.RenderElements<ApiNotification>(notifs, page);
	}

	// Token: 0x060061A4 RID: 24996 RVA: 0x002275B8 File Offset: 0x002259B8
	protected override void SetPickerContentFromApiModel(VRCUiContentButton content, object am)
	{
		APIUser user = null;
		content.SetDetailShouldShowImage(0, false);
		content.SetDetailShouldShowImage(1, false);
		content.SetDetailShouldShowImage(2, false);
		if (am is ApiNotification)
		{
			ApiNotification notif = (ApiNotification)am;
			user = new APIUser();
			user.Init(notif);
			content.Initialize(user.currentAvatarThumbnailImageUrl, user.displayName, null, notif.id);
			APIUser.Fetch(notif.senderUserId, delegate(APIUser u)
			{
				content.Initialize(u.currentAvatarThumbnailImageUrl, u.displayName, delegate
				{
					VRCUiPage page = VRCUiManager.Instance.GetPage("UserInterface/MenuContent/Screens/UserInfo");
					VRCUiManager.Instance.ShowScreen(page);
					((PageUserInfo)page).SetupUserInfo(u, PageUserInfo.InfoType.ReceivedFriendRequest, UiUserList.ListType.None);
					((PageUserInfo)page).notification = notif;
				}, notif.id);
			}, null);
		}
		else
		{
			user = (APIUser)am;
			content.Initialize(user.currentAvatarThumbnailImageUrl, user.displayName, delegate
			{
				PageUserInfo.InfoType infoType = PageUserInfo.InfoType.NotFriends;
				VRCUiPage page = VRCUiManager.Instance.GetPage("UserInterface/MenuContent/Screens/UserInfo");
				VRCUiManager.Instance.ShowScreen(page);
				bool flag = false;
				if (APIUser.CurrentUser.friends != null)
				{
					flag = (APIUser.CurrentUser.friends.Find((APIUser u) => u.id == user.id) != null);
				}
				switch (this.listType)
				{
				case UiUserList.ListType.InWorld:
					if (flag)
					{
						infoType = PageUserInfo.InfoType.OnlineFriend;
					}
					else
					{
						infoType = PageUserInfo.InfoType.NotFriends;
					}
					break;
				case UiUserList.ListType.FriendRequests:
					infoType = PageUserInfo.InfoType.ReceivedFriendRequest;
					break;
				case UiUserList.ListType.OnlineFriends:
					infoType = PageUserInfo.InfoType.OnlineFriend;
					break;
				case UiUserList.ListType.OfflineFriends:
					infoType = PageUserInfo.InfoType.OfflineFriend;
					break;
				case UiUserList.ListType.Invites:
					infoType = PageUserInfo.InfoType.Invited;
					break;
				case UiUserList.ListType.HelpRequests:
					infoType = PageUserInfo.InfoType.ReceivedHelpRequest;
					break;
				case UiUserList.ListType.Search:
				{
					Player player = PlayerManager.GetPlayer(user.id);
					bool flag2 = player != null && player.vrcPlayer != null && player.vrcPlayer.isInvisible;
					if (flag)
					{
						infoType = ((!flag2) ? PageUserInfo.InfoType.OnlineFriend : PageUserInfo.InfoType.OfflineFriend);
					}
					else
					{
						infoType = PageUserInfo.InfoType.NotFriends;
					}
					break;
				}
				}
				((PageUserInfo)page).SetupUserInfo(user, infoType, this.listType);
			}, user.id);
		}
		if (this.listType == UiUserList.ListType.FriendRequests)
		{
			content.SetDetailShouldShowImage(2, true);
		}
		else if (ModerationManager.Instance.IsBlocked(user.id))
		{
			content.SetDetailShouldShowImage(1, true);
		}
		else if (APIUser.IsFriendsWith(user.id))
		{
			content.SetDetailShouldShowImage(0, true);
		}
	}

	// Token: 0x04004738 RID: 18232
	public UiUserList.ListType listType;

	// Token: 0x02000C4F RID: 3151
	public enum ListType
	{
		// Token: 0x04004741 RID: 18241
		None,
		// Token: 0x04004742 RID: 18242
		InWorld,
		// Token: 0x04004743 RID: 18243
		FriendRequests,
		// Token: 0x04004744 RID: 18244
		OnlineFriends,
		// Token: 0x04004745 RID: 18245
		OfflineFriends,
		// Token: 0x04004746 RID: 18246
		Invites,
		// Token: 0x04004747 RID: 18247
		HelpRequests,
		// Token: 0x04004748 RID: 18248
		Search
	}
}
