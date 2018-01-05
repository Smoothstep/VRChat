using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

namespace VRC.UI
{
	// Token: 0x02000C25 RID: 3109
	public class PageUserInfo : VRCUiPage
	{
		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x06006034 RID: 24628 RVA: 0x0021DB68 File Offset: 0x0021BF68
		public bool isModerator
		{
			get
			{
				return Player.Instance.isModerator;
			}
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x06006035 RID: 24629 RVA: 0x0021DB74 File Offset: 0x0021BF74
		public bool isRoomAuthor
		{
			get
			{
				return RoomManager.currentAuthorId == User.CurrentUser.id;
			}
		}

		// Token: 0x17000D95 RID: 3477
		// (get) Token: 0x06006036 RID: 24630 RVA: 0x0021DB8A File Offset: 0x0021BF8A
		public bool isInstanceOwner
		{
			get
			{
				return RoomManager.currentOwnerId == User.CurrentUser.id;
			}
		}

		// Token: 0x06006037 RID: 24631 RVA: 0x0021DBA0 File Offset: 0x0021BFA0
		public override void Awake()
		{
			this.defaultImageTexture = this.avatarImage.texture;
			this.worldList = this.worldScroller.GetComponentInChildren<UiWorldList>();
		}

		// Token: 0x06006038 RID: 24632 RVA: 0x0021DBC4 File Offset: 0x0021BFC4
		private void ClearRoomInfo()
		{
			this.user = null;
			this.userName.text = string.Empty;
			this.onlineStatusText.text = string.Empty;
			this.avatarImage.texture = this.defaultImageTexture;
			this.worldImage.transform.parent.gameObject.SetActive(true);
			this.worldImage.texture = this.defaultImageTexture;
			this.worldImage.transform.parent.gameObject.SetActive(false);
			this.worldScroller.SetActive(false);
			this.worldList.ClearAll();
			this.worldList.ownerId = string.Empty;
			this.worldList.pickers.Clear();
			this.notificationMessageText.text = string.Empty;
			this.SetIcon(QuickMenuSocialElement.IconType.None, string.Empty);
			this.userLocation = null;
		}

		// Token: 0x06006039 RID: 24633 RVA: 0x0021DCAC File Offset: 0x0021C0AC
		private void SetupModButtons()
		{
			this.modButtons.SetActive(false);
			if (!this.isModerator && !this.isRoomAuthor && !this.isInstanceOwner)
			{
				return;
			}
			if (!string.IsNullOrEmpty(this.user.location) && this.user.location != "offline")
			{
				string text = this.user.location.Split(new char[]
				{
					':'
				})[0];
				if (text != "local")
				{
					ApiWorld.Fetch(text, delegate(ApiWorld world)
					{
						foreach (VRCUiButton vrcuiButton in this.modButtons.GetComponentsInChildren<VRCUiButton>(true))
						{
							if (vrcuiButton.Name == "Warn" || vrcuiButton.Name == "Kick" || vrcuiButton.Name == "MicOff")
							{
								if (this.isModerator || this.isRoomAuthor || this.isInstanceOwner)
								{
									vrcuiButton.gameObject.SetActive(true);
								}
								else
								{
									vrcuiButton.gameObject.SetActive(false);
								}
							}
							else if (this.isModerator)
							{
								vrcuiButton.gameObject.SetActive(true);
							}
							else
							{
								vrcuiButton.gameObject.SetActive(false);
							}
						}
						this.modButtons.SetActive(true);
					}, delegate(string obj)
					{
						Debug.LogError(obj);
					});
				}
			}
		}

		// Token: 0x0600603A RID: 24634 RVA: 0x0021DD74 File Offset: 0x0021C174
		public void SetupUserInfo(APIUser u, PageUserInfo.InfoType infoType, UiUserList.ListType listType = UiUserList.ListType.None)
		{
			this.SetUserRelationshipState(infoType);
			this.ClearRoomInfo();
			this.user = u;
			this.userName.text = this.user.displayName;
			if (!string.IsNullOrEmpty(this.user.currentAvatarImageUrl))
			{
				Downloader.DownloadImage(this.user.currentAvatarImageUrl, delegate(string downloadedUrl, Texture2D obj)
				{
					this.avatarImage.texture = obj;
				}, string.Empty);
			}
			this.SetupModButtons();
			this.onlineStatusText.text = string.Empty;
			switch (this.state)
			{
			case PageUserInfo.InfoType.NotFriends:
			{
				this.worldImage.transform.parent.gameObject.SetActive(true);
				Player player = PlayerManager.GetPlayer(this.user.id);
				bool flag = player != null && player.vrcPlayer != null && player.vrcPlayer.isInvisible;
				if (listType == UiUserList.ListType.InWorld && !flag)
				{
					if (RoomManager.currentRoom != null && !string.IsNullOrEmpty(RoomManager.currentRoom.imageUrl))
					{
						this.DownloadAndSetWorldImage(RoomManager.currentRoom.imageUrl);
					}
					this.onlineStatusText.text = "online in current world";
				}
				this.SetupFriendButton("Friend");
				break;
			}
			case PageUserInfo.InfoType.OnlineFriend:
				this.worldImage.transform.parent.gameObject.SetActive(true);
				this.worldList.ownerId = this.user.id;
				this.worldScroller.SetActive(true);
				this.SetIcon(QuickMenuSocialElement.IconType.Friend, u.id);
				this.SetupJoinButton("Join", false);
				this.SetupInviteButton("Invite", false);
				if (listType == UiUserList.ListType.InWorld)
				{
					if (RoomManager.currentRoom != null && !string.IsNullOrEmpty(RoomManager.currentRoom.imageUrl))
					{
						this.DownloadAndSetWorldImage(RoomManager.currentRoom.imageUrl);
					}
					this.onlineStatusText.text = "online in current world";
				}
				else if (!string.IsNullOrEmpty(this.user.location))
				{
					string text = this.user.location.Split(new char[]
					{
						':'
					})[0];
					string instanceId = this.user.location.Split(new char[]
					{
						':'
					})[1];
					if (text == "local")
					{
						this.onlineStatusText.text = "online in local test world";
					}
					else
					{
						ApiWorld.Fetch(text, delegate(ApiWorld world)
						{
							ApiWorld.WorldInstance worldInstance = new ApiWorld.WorldInstance(instanceId, 0);
							bool flag2 = APIUser.CurrentUser.id == worldInstance.GetInstanceCreator();
							ApiWorld.WorldInstance.AccessType accessType = worldInstance.GetAccessType();
							ApiWorld.WorldInstance.AccessDetail accessDetail = ApiWorld.WorldInstance.GetAccessDetail(accessType);
							if (accessType == ApiWorld.WorldInstance.AccessType.Public)
							{
								this.DownloadAndSetWorldImage(world.imageUrl);
								this.userLocation = world;
								if (world.id == RoomManager.currentRoom.id && worldInstance.idWithTags == RoomManager.currentRoom.currentInstanceIdWithTags)
								{
									this.onlineStatusText.text = "online in same world";
								}
								else
								{
									this.onlineStatusText.text = string.Concat(new string[]
									{
										"online in ",
										world.name,
										"\n<i>",
										accessDetail.fullName.ToLower(),
										"</i>"
									});
									this.SetupJoinButton("Join", true);
									this.SetupInviteButton("Invite", this.CanInviteHere());
								}
							}
							else if (accessType == ApiWorld.WorldInstance.AccessType.FriendsOfGuests)
							{
								this.DownloadAndSetWorldImage(world.imageUrl);
								this.userLocation = world;
								if (world.id == RoomManager.currentRoom.id && worldInstance.idWithTags == RoomManager.currentRoom.currentInstanceIdWithTags)
								{
									this.onlineStatusText.text = "online in same world";
								}
								else
								{
									this.onlineStatusText.text = string.Concat(new string[]
									{
										"online in ",
										world.name,
										"\n<i>",
										accessDetail.fullName.ToLower(),
										"</i>"
									});
									this.SetupJoinButton("Join", true);
									this.SetupInviteButton("Invite", this.CanInviteHere());
								}
							}
							else if (accessType == ApiWorld.WorldInstance.AccessType.FriendsOnly)
							{
								this.DownloadAndSetWorldImage(world.imageUrl);
								this.userLocation = world;
								if (world.id == RoomManager.currentRoom.id && worldInstance.idWithTags == RoomManager.currentRoom.currentInstanceIdWithTags)
								{
									this.onlineStatusText.text = "online in same world";
								}
								else
								{
									this.onlineStatusText.text = string.Concat(new string[]
									{
										"online in ",
										world.name,
										"\n<i>",
										accessDetail.fullName.ToLower(),
										"</i>"
									});
									this.SetupJoinButton("Join", flag2 || this.user.id == worldInstance.GetInstanceCreator());
									this.SetupInviteButton("Invite", this.CanInviteHere());
								}
							}
							else if (accessType == ApiWorld.WorldInstance.AccessType.InviteOnly)
							{
								if (flag2)
								{
									this.DownloadAndSetWorldImage(world.imageUrl);
									this.userLocation = world;
								}
								else
								{
									this.worldImage.texture = this.avatarImage.texture;
									this.userLocation = null;
								}
								if (world.id == RoomManager.currentRoom.id && worldInstance.idWithTags == RoomManager.currentRoom.currentInstanceIdWithTags)
								{
									this.onlineStatusText.text = "online in same world";
								}
								else
								{
									this.onlineStatusText.text = "online in " + accessDetail.fullName.ToLower() + " world";
									if (flag2)
									{
										this.SetupJoinButton("Join", true);
									}
									else
									{
										this.SetupJoinButton("Req Invite", this.user.id == worldInstance.GetInstanceCreator());
									}
									this.SetupInviteButton("Invite", this.CanInviteHere());
								}
							}
						}, delegate(string obj)
						{
							Debug.LogError(obj);
						});
					}
				}
				break;
			case PageUserInfo.InfoType.OfflineFriend:
				this.onlineStatusText.text = "offline";
				this.worldList.ownerId = this.user.id;
				this.worldScroller.SetActive(true);
				this.SetIcon(QuickMenuSocialElement.IconType.Friend, u.id);
				break;
			case PageUserInfo.InfoType.ReceivedFriendRequest:
				this.notificationMessageText.text = this.user.displayName + " wants to be your friend";
				this.SetIcon(QuickMenuSocialElement.IconType.FriendRequest, u.id);
				break;
			case PageUserInfo.InfoType.ReceivedHelpRequest:
				this.notificationMessageText.text = this.user.displayName + " needs help";
				this.SetIcon(QuickMenuSocialElement.IconType.HelpRequest, u.id);
				break;
			}
			this.SetupBlockButton();
			this.SetupVoteToKickButton();
			this.SetupMuteButton();
		}

		// Token: 0x0600603B RID: 24635 RVA: 0x0021E10C File Offset: 0x0021C50C
		private void SetupMuteButton()
		{
			VRCUiButton vrcuiButton = (from b in base.GetComponentsInChildren<VRCUiButton>()
			where b.Name == "Mute"
			select b).FirstOrDefault<VRCUiButton>();
			if (vrcuiButton != null)
			{
				if (this.user.developerType == null || this.user.developerType == APIUser.DeveloperType.Internal || this.user.id == User.CurrentUser.id)
				{
					vrcuiButton.SetButtonText("Mute");
					vrcuiButton.GetComponent<Button>().interactable = false;
				}
				else
				{
					vrcuiButton.GetComponent<Button>().interactable = true;
					if (ModerationManager.Instance.IsMuted(this.user.id))
					{
						vrcuiButton.SetButtonText("Unmute");
					}
					else
					{
						vrcuiButton.SetButtonText("Mute");
					}
				}
			}
		}

		// Token: 0x0600603C RID: 24636 RVA: 0x0021E20C File Offset: 0x0021C60C
		private void SetupBlockButton()
		{
			VRCUiButton vrcuiButton = (from b in base.GetComponentsInChildren<VRCUiButton>()
			where b.Name == "Block"
			select b).FirstOrDefault<VRCUiButton>();
			if (vrcuiButton != null)
			{
				if (!Player.Instance.isValidUser || this.isModerator || this.user.id == User.CurrentUser.id || this.user.developerType == APIUser.DeveloperType.Internal)
				{
					vrcuiButton.SetButtonText("Block");
					vrcuiButton.GetComponent<Button>().interactable = false;
				}
				else
				{
					vrcuiButton.GetComponent<Button>().interactable = true;
					if (ModerationManager.Instance.IsBlocked(this.user.id))
					{
						vrcuiButton.SetButtonText("Unblock");
					}
					else
					{
						vrcuiButton.SetButtonText("Block");
					}
				}
			}
		}

		// Token: 0x0600603D RID: 24637 RVA: 0x0021E310 File Offset: 0x0021C710
		private void SetupJoinButton(string text, bool active)
		{
			VRCUiButton vrcuiButton = (from b in base.GetComponentsInChildren<VRCUiButton>()
			where b.Name == "Join"
			select b).FirstOrDefault<VRCUiButton>();
			if (vrcuiButton != null)
			{
				Button component = vrcuiButton.GetComponent<Button>();
				if (component != null)
				{
					vrcuiButton.SetButtonText(text);
					component.interactable = active;
				}
			}
		}

		// Token: 0x0600603E RID: 24638 RVA: 0x0021E378 File Offset: 0x0021C778
		private bool CanInviteHere()
		{
			ApiWorld.WorldInstance worldInstance = new ApiWorld.WorldInstance(RoomManager.currentRoom.currentInstanceIdWithTags, 0);
			ApiWorld.WorldInstance.AccessType accessType = worldInstance.GetAccessType();
			return this.isModerator || this.isInstanceOwner || (accessType == ApiWorld.WorldInstance.AccessType.Public || accessType == ApiWorld.WorldInstance.AccessType.FriendsOfGuests);
		}

		// Token: 0x0600603F RID: 24639 RVA: 0x0021E3C8 File Offset: 0x0021C7C8
		private void SetupInviteButton(string text, bool active)
		{
			VRCUiButton vrcuiButton = (from b in base.GetComponentsInChildren<VRCUiButton>()
			where b.Name == "Invite"
			select b).FirstOrDefault<VRCUiButton>();
			if (vrcuiButton != null)
			{
				Button component = vrcuiButton.GetComponent<Button>();
				if (component != null)
				{
					vrcuiButton.SetButtonText(text);
					component.interactable = active;
				}
			}
		}

		// Token: 0x06006040 RID: 24640 RVA: 0x0021E430 File Offset: 0x0021C830
		private void SetupFriendButton(string text)
		{
			VRCUiButton vrcuiButton = (from b in base.GetComponentsInChildren<VRCUiButton>()
			where b.Name == "Friend"
			select b).FirstOrDefault<VRCUiButton>();
			if (vrcuiButton != null)
			{
				if (this.user.id == User.CurrentUser.id || ModerationManager.Instance.IsBlockedEitherWay(this.user.id))
				{
					vrcuiButton.GetComponent<Button>().interactable = false;
				}
				else
				{
					vrcuiButton.GetComponent<Button>().interactable = true;
				}
				vrcuiButton.SetButtonText(text);
			}
		}

		// Token: 0x06006041 RID: 24641 RVA: 0x0021E4D4 File Offset: 0x0021C8D4
		private void SetupVoteToKickButton()
		{
			VRCUiButton vrcuiButton = (from b in base.GetComponentsInChildren<VRCUiButton>()
			where b.Name == "VoteKick"
			select b).FirstOrDefault<VRCUiButton>();
			if (vrcuiButton == null)
			{
				return;
			}
			if (User.CurrentUser.id == this.user.id || PlayerManager.GetPlayer(this.user.id) == null)
			{
				vrcuiButton.GetComponent<Button>().interactable = false;
			}
			else
			{
				vrcuiButton.GetComponent<Button>().interactable = true;
				vrcuiButton.SetButtonText("Vote to Kick");
			}
		}

		// Token: 0x06006042 RID: 24642 RVA: 0x0021E580 File Offset: 0x0021C980
		private void DownloadAndSetWorldImage(string imageUrl)
		{
			Downloader.DownloadImage(imageUrl, delegate(string downloadedUrl, Texture2D imageTexture)
			{
				if (this.worldImage != null && downloadedUrl == imageUrl)
				{
					this.worldImage.texture = imageTexture;
				}
			}, string.Empty);
		}

		// Token: 0x06006043 RID: 24643 RVA: 0x0021E5C0 File Offset: 0x0021C9C0
		public void SetUserRelationshipState(PageUserInfo.InfoType relationshipState)
		{
			this.state = relationshipState;
			this.userButtons.SetActive(false);
			this.sentFriendRequestButtons.SetActive(false);
			this.receivedFriendRequestButtons.SetActive(false);
			this.onlineFriendButtons.SetActive(false);
			this.offlineFriendButtons.SetActive(false);
			this.invitationButtons.SetActive(false);
			switch (this.state)
			{
			case PageUserInfo.InfoType.NotFriends:
				this.userButtons.SetActive(true);
				break;
			case PageUserInfo.InfoType.OnlineFriend:
				this.onlineFriendButtons.SetActive(true);
				break;
			case PageUserInfo.InfoType.OfflineFriend:
				this.offlineFriendButtons.SetActive(true);
				break;
			case PageUserInfo.InfoType.SentFriendRequest:
				this.sentFriendRequestButtons.SetActive(true);
				break;
			case PageUserInfo.InfoType.ReceivedFriendRequest:
				this.receivedFriendRequestButtons.SetActive(true);
				break;
			case PageUserInfo.InfoType.Invited:
				this.invitationButtons.SetActive(true);
				break;
			}
		}

		// Token: 0x06006044 RID: 24644 RVA: 0x0021E6B4 File Offset: 0x0021CAB4
		public void JoinFriend()
		{
			if (this.userLocation == null)
			{
				VRCUiPopupManager.Instance.ShowStandardPopup("Invitation only", this.user.displayName + " is in a private world. Would you like to request an invitation?", "Yes", delegate
				{
					VRCUiPopupManager.Instance.HideCurrentPopup();
					this.RequestInvitation();
				}, "Cancel", delegate
				{
					VRCUiPopupManager.Instance.HideCurrentPopup();
				}, null);
			}
			else
			{
				Debug.Log("Joining friend: " + this.user.location);
				VRCFlowManager.Instance.EnterRoom(this.user.location, false, delegate(string msg)
				{
					VRCUiPopupManager.Instance.ShowAlert("Cannot Join World Instance", msg, 10f);
				});
			}
		}

		// Token: 0x06006045 RID: 24645 RVA: 0x0021E778 File Offset: 0x0021CB78
		public void InviteFriend()
		{
			string message = "Join me in " + RoomManager.currentRoom.name;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["worldId"] = RoomManager.currentRoom.id + ":" + RoomManager.currentRoom.currentInstanceIdWithTags;
			ApiNotification.SendNotification(this.user.id, ApiNotification.NotificationType.Invite, message, dictionary, null, null);
			this.SetupInviteButton("Invite Sent", false);
		}

		// Token: 0x06006046 RID: 24646 RVA: 0x0021E7EC File Offset: 0x0021CBEC
		public void RequestInvitation()
		{
			string message = "Please invite me to your world";
			ApiNotification.SendNotification(this.user.id, ApiNotification.NotificationType.Requestinvite, message, null, null, null);
			this.SetupJoinButton("Req Sent", false);
		}

		// Token: 0x06006047 RID: 24647 RVA: 0x0021E820 File Offset: 0x0021CC20
		public void SendFriendRequest()
		{
			ModerationManager.Instance.UnmuteUser(this.user.id);
			ModerationManager.Instance.UnblockUser(this.user.id);
			this.SetupFriendButton("Request Sent");
			APIUser.SendFriendRequest(this.user.id, null, null);
		}

		// Token: 0x06006048 RID: 24648 RVA: 0x0021E874 File Offset: 0x0021CC74
		public void AcceptFriendRequest()
		{
			ModerationManager.Instance.UnmuteUser(this.notification.senderUserId);
			ModerationManager.Instance.UnblockUser(this.notification.senderUserId);
			APIUser.AcceptFriendRequest(this.notification.id, null, delegate(string obj)
			{
				Debug.LogError("Something went wrong accepting friend request");
				this.SetupUserInfo(this.user, PageUserInfo.InfoType.ReceivedFriendRequest, UiUserList.ListType.None);
			});
			PageUserInfo.InfoType infoType = (!(this.user.location == "offline")) ? PageUserInfo.InfoType.OnlineFriend : PageUserInfo.InfoType.OfflineFriend;
			this.SetupUserInfo(this.user, infoType, UiUserList.ListType.None);
		}

		// Token: 0x06006049 RID: 24649 RVA: 0x0021E8F8 File Offset: 0x0021CCF8
		public void DeclineFriendRequest()
		{
			APIUser.DeclineFriendRequest(this.notification.id, null, null);
			this.Back();
		}

		// Token: 0x0600604A RID: 24650 RVA: 0x0021E912 File Offset: 0x0021CD12
		public void UnfriendUser()
		{
			APIUser.UnfriendUser(this.user.id, null, null);
			this.SetupUserInfo(this.user, PageUserInfo.InfoType.NotFriends, UiUserList.ListType.None);
		}

		// Token: 0x0600604B RID: 24651 RVA: 0x0021E934 File Offset: 0x0021CD34
		public void BlockUser()
		{
			Player player = PlayerManager.GetPlayer(this.user.id);
			if (player != null)
			{
				player.ApplyBlock(true);
			}
			ModerationManager.Instance.BlockUser(this.user.id);
			this.SetupBlockButton();
			this.RefreshIcon();
		}

		// Token: 0x0600604C RID: 24652 RVA: 0x0021E988 File Offset: 0x0021CD88
		public void UnblockUser()
		{
			Player player = PlayerManager.GetPlayer(this.user.id);
			if (player != null)
			{
				player.ApplyBlock(false);
			}
			ModerationManager.Instance.UnblockUser(this.user.id);
			this.SetupBlockButton();
			this.RefreshIcon();
		}

		// Token: 0x0600604D RID: 24653 RVA: 0x0021E9DA File Offset: 0x0021CDDA
		public void ToggleBlock()
		{
			if (ModerationManager.Instance.IsBlocked(this.user.id))
			{
				this.UnblockUser();
			}
			else
			{
				this.BlockUser();
			}
		}

		// Token: 0x0600604E RID: 24654 RVA: 0x0021EA08 File Offset: 0x0021CE08
		public void ToggleMute()
		{
			Player player = PlayerManager.GetPlayer(this.user.id);
			if (ModerationManager.Instance.IsMuted(this.user.id))
			{
				if (player != null)
				{
					player.ApplyMute(false);
				}
				ModerationManager.Instance.UnmuteUser(this.user.id);
			}
			else
			{
				if (player != null)
				{
					player.ApplyMute(true);
				}
				ModerationManager.Instance.MuteUser(this.user.id);
			}
			this.SetupMuteButton();
		}

		// Token: 0x0600604F RID: 24655 RVA: 0x0021EA9C File Offset: 0x0021CE9C
		public void InitiateVoteToKick()
		{
			VRCUiPopupManager.Instance.ShowStandardPopup("Vote to Kick", "Are you sure you want to initiate a vote to kick " + this.user.displayName + "?", "Yes", delegate
			{
				VRCUiPopupManager.Instance.HideCurrentPopup();
				ModerationManager.Instance.InitiateVoteToKick(this.user);
			}, "No", delegate
			{
				VRCUiPopupManager.Instance.HideCurrentPopup();
			}, null);
		}

		// Token: 0x06006050 RID: 24656 RVA: 0x0021EB06 File Offset: 0x0021CF06
		public void Back()
		{
			VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Screens/Social");
		}

		// Token: 0x06006051 RID: 24657 RVA: 0x0021EB17 File Offset: 0x0021CF17
		public void ModeratorWarn()
		{
			ModerationManager.Instance.AskModeratorWarnUI(this.user);
		}

		// Token: 0x06006052 RID: 24658 RVA: 0x0021EB29 File Offset: 0x0021CF29
		public void ModeratorTurnOffMic()
		{
			ModerationManager.Instance.AskModeratorTurnOffMicUI(this.user);
		}

		// Token: 0x06006053 RID: 24659 RVA: 0x0021EB3B File Offset: 0x0021CF3B
		public void ModeratorMicGainUp()
		{
			if (this.isModerator)
			{
				ModerationManager.Instance.ModMicGainUp(this.user);
			}
		}

		// Token: 0x06006054 RID: 24660 RVA: 0x0021EB58 File Offset: 0x0021CF58
		public void ModeratorMicGainDown()
		{
			if (this.isModerator)
			{
				ModerationManager.Instance.ModMicGainDown(this.user);
			}
		}

		// Token: 0x06006055 RID: 24661 RVA: 0x0021EB75 File Offset: 0x0021CF75
		public void ModeratorKick()
		{
			ModerationManager.Instance.AskModeratorKickUI(this.user);
		}

		// Token: 0x06006056 RID: 24662 RVA: 0x0021EB87 File Offset: 0x0021CF87
		public void ModeratorForceLogout()
		{
			ModerationManager.Instance.AskModeratorForceLogoutUI(this.user);
		}

		// Token: 0x06006057 RID: 24663 RVA: 0x0021EB99 File Offset: 0x0021CF99
		public void ModeratorBan()
		{
			ModerationManager.Instance.AskModeratorBanUI(this.user);
		}

		// Token: 0x06006058 RID: 24664 RVA: 0x0021EBAC File Offset: 0x0021CFAC
		public void ModeratorJoin()
		{
			if (this.isModerator)
			{
				Debug.Log("Moderator force joining: " + this.user.location);
				VRCFlowManager.Instance.EnterRoom(this.user.location, false, delegate(string msg)
				{
					VRCUiPopupManager.Instance.ShowAlert("Cannot Join World Instance", msg, 10f);
				});
			}
		}

		// Token: 0x06006059 RID: 24665 RVA: 0x0021EC14 File Offset: 0x0021D014
		private void SetIcon(QuickMenuSocialElement.IconType icon, string userId)
		{
			if (!string.IsNullOrEmpty(userId))
			{
				if (icon == QuickMenuSocialElement.IconType.None || icon == QuickMenuSocialElement.IconType.Friend)
				{
					if (ModerationManager.Instance.IsBlocked(userId))
					{
						icon = QuickMenuSocialElement.IconType.Blocked;
					}
					else if (APIUser.IsFriendsWith(userId))
					{
						icon = QuickMenuSocialElement.IconType.Friend;
					}
				}
				if (icon == QuickMenuSocialElement.IconType.Blocked && !ModerationManager.Instance.IsBlocked(userId))
				{
					if (APIUser.IsFriendsWith(userId))
					{
						icon = QuickMenuSocialElement.IconType.Friend;
					}
					else
					{
						icon = QuickMenuSocialElement.IconType.None;
					}
				}
			}
			else
			{
				icon = QuickMenuSocialElement.IconType.None;
			}
			this.currentIcon = icon;
			Texture socialIcon = QuickMenuSocial.Instance.GetSocialIcon(icon);
			if (socialIcon != null)
			{
				this.iconImage.gameObject.SetActive(true);
				this.iconImage.texture = socialIcon;
			}
			else
			{
				this.iconImage.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600605A RID: 24666 RVA: 0x0021ECE7 File Offset: 0x0021D0E7
		public void RefreshIcon()
		{
			this.SetIcon(this.currentIcon, this.user.id);
		}

		// Token: 0x040045EA RID: 17898
		public APIUser user;

		// Token: 0x040045EB RID: 17899
		public ApiNotification notification;

		// Token: 0x040045EC RID: 17900
		public RawImage avatarImage;

		// Token: 0x040045ED RID: 17901
		public Text userName;

		// Token: 0x040045EE RID: 17902
		public RawImage worldImage;

		// Token: 0x040045EF RID: 17903
		public Text onlineStatusText;

		// Token: 0x040045F0 RID: 17904
		public GameObject worldScroller;

		// Token: 0x040045F1 RID: 17905
		private UiWorldList worldList;

		// Token: 0x040045F2 RID: 17906
		public Text notificationMessageText;

		// Token: 0x040045F3 RID: 17907
		private Texture defaultImageTexture;

		// Token: 0x040045F4 RID: 17908
		public GameObject userButtons;

		// Token: 0x040045F5 RID: 17909
		public GameObject modButtons;

		// Token: 0x040045F6 RID: 17910
		public GameObject sentFriendRequestButtons;

		// Token: 0x040045F7 RID: 17911
		public GameObject receivedFriendRequestButtons;

		// Token: 0x040045F8 RID: 17912
		public GameObject receivedHelpRequestButtons;

		// Token: 0x040045F9 RID: 17913
		public GameObject onlineFriendButtons;

		// Token: 0x040045FA RID: 17914
		public GameObject offlineFriendButtons;

		// Token: 0x040045FB RID: 17915
		public GameObject invitationButtons;

		// Token: 0x040045FC RID: 17916
		private PageUserInfo.InfoType state;

		// Token: 0x040045FD RID: 17917
		private ApiWorld userLocation;

		// Token: 0x040045FE RID: 17918
		public RawImage iconImage;

		// Token: 0x040045FF RID: 17919
		private QuickMenuSocialElement.IconType currentIcon;

		// Token: 0x02000C26 RID: 3110
		public enum InfoType
		{
			// Token: 0x0400460D RID: 17933
			NotFriends,
			// Token: 0x0400460E RID: 17934
			OnlineFriend,
			// Token: 0x0400460F RID: 17935
			OfflineFriend,
			// Token: 0x04004610 RID: 17936
			SentFriendRequest,
			// Token: 0x04004611 RID: 17937
			ReceivedFriendRequest,
			// Token: 0x04004612 RID: 17938
			ReceivedHelpRequest,
			// Token: 0x04004613 RID: 17939
			Blocked,
			// Token: 0x04004614 RID: 17940
			Invited
		}
	}
}
