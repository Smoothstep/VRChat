using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VRC.Core;

namespace VRC.UI
{
	// Token: 0x02000C27 RID: 3111
	public class PageWorldInfo : VRCUiPage
	{
		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x0600606D RID: 24685 RVA: 0x0021F3AE File Offset: 0x0021D7AE
		public ApiWorld world
		{
			get
			{
				return this.mWorld;
			}
		}

		// Token: 0x0600606E RID: 24686 RVA: 0x0021F3B6 File Offset: 0x0021D7B6
		public override void Awake()
		{
			this.defaultImageTexture = this.roomImage.texture;
		}

		// Token: 0x0600606F RID: 24687 RVA: 0x0021F3C9 File Offset: 0x0021D7C9
		public override void OnDisable()
		{
			this.ClearRoomInfo();
		}

		// Token: 0x06006070 RID: 24688 RVA: 0x0021F3D1 File Offset: 0x0021D7D1
		public override void OnEnable()
		{
			this.ConfigurePortalButton(ApiWorld.WorldInstance.AccessType.Public, false, false, true);
			this.il = base.GetComponentInChildren<UiWorldInstanceList>();
		}

		// Token: 0x06006071 RID: 24689 RVA: 0x0021F3EC File Offset: 0x0021D7EC
		private void ConfigureJoinButton(bool isInstanceFull, bool isInstancePublic)
		{
			this.joinButtonOnClickTrigger.triggers.Clear();
			bool flag = true;
			string disabledReason = string.Empty;
			if (isInstanceFull && !VRCPlayer.Instance.player.isInternal)
			{
				flag = false;
				disabledReason = "This world instance is currently filled to capacity.";
			}
			if (isInstancePublic && ModerationManager.Instance.IsBannedFromPublicOnly(APIUser.CurrentUser.id))
			{
				flag = false;
				disabledReason = ModerationManager.Instance.GetPublicOnlyBannedUserMessage();
			}
			Text component = this.joinButton.transform.Find("Text").GetComponent<Text>();
			if (flag)
			{
				this.joinButton.interactable = true;
				component.color = Color.white;
				this.joinButton.onClick.AddListener(new UnityAction(this.JoinRoom));
			}
			else
			{
				this.joinButton.interactable = false;
				component.color = Color.gray;
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.eventID = EventTriggerType.PointerClick;
				entry.callback.AddListener(delegate(BaseEventData data)
				{
					if (!string.IsNullOrEmpty(disabledReason))
					{
						VRCUiPopupManager.Instance.ShowAlert("Cannot Join World Instance", disabledReason, 10f);
					}
				});
				this.joinButtonOnClickTrigger.triggers.Add(entry);
			}
		}

		// Token: 0x06006072 RID: 24690 RVA: 0x0021F51C File Offset: 0x0021D91C
		private void ConfigurePortalButton(ApiWorld.WorldInstance.AccessType access, bool isMine, bool isInstanceFull, bool isInstancePublic)
		{
			this.portalButtonOnClickTrigger.triggers.Clear();
			if (this.isPortalMenu)
			{
				this.portalButton.gameObject.SetActive(false);
			}
			else
			{
				this.portalButton.gameObject.SetActive(true);
				Text component = this.portalButton.transform.Find("Text").GetComponent<Text>();
				bool flag = true;
				string disabledReason = string.Empty;
				if (!isMine && (access == ApiWorld.WorldInstance.AccessType.InviteOnly || access == ApiWorld.WorldInstance.AccessType.FriendsOnly || access == ApiWorld.WorldInstance.AccessType.FriendsOfGuests))
				{
					flag = false;
					disabledReason = "You cannot create a portal to a private world created by another user.";
				}
				if (isInstanceFull)
				{
					flag = false;
					disabledReason = "This world instance is currently filled to capacity.";
				}
				if (isInstancePublic && ModerationManager.Instance.IsBannedFromPublicOnly(APIUser.CurrentUser.id))
				{
					flag = false;
					disabledReason = ModerationManager.Instance.GetPublicOnlyBannedUserMessage();
				}
				if (flag)
				{
					this.portalButton.interactable = true;
					component.color = Color.white;
					if (RoomManager.inRoom)
					{
						this.portalButton.onClick.AddListener(new UnityAction(this.CreatePortalAndAskForPassword));
					}
				}
				else
				{
					this.portalButton.interactable = false;
					component.color = Color.gray;
					EventTrigger.Entry entry = new EventTrigger.Entry();
					entry.eventID = EventTriggerType.PointerClick;
					entry.callback.AddListener(delegate(BaseEventData data)
					{
						if (!string.IsNullOrEmpty(disabledReason))
						{
							VRCUiPopupManager.Instance.ShowAlert("Cannot Create Portal", disabledReason, 10f);
						}
					});
					this.portalButtonOnClickTrigger.triggers.Add(entry);
				}
			}
		}

		// Token: 0x06006073 RID: 24691 RVA: 0x0021F69C File Offset: 0x0021DA9C
		private void ClearRoomInfo()
		{
			this.mWorld = null;
			this.roomName.text = string.Empty;
			this.roomAuthor.text = string.Empty;
			this.roomOwner.text = string.Empty;
			this.instanceStatus.text = string.Empty;
			this.worldInstanceId.text = string.Empty;
			this.playerCount.text = string.Empty;
			this.maxPlayers.text = string.Empty;
			this.SetCurrentInstanceMessage(string.Empty, false);
			this.roomImage.texture = this.defaultImageTexture;
			this.joinButton.interactable = true;
			this.portalButton.interactable = true;
			this.newInstanceButton.interactable = false;
			this.joinButton.onClick.RemoveAllListeners();
			this.joinButtonOnClickTrigger.triggers.Clear();
			this.portalButton.onClick.RemoveAllListeners();
			this.portalButtonOnClickTrigger.triggers.Clear();
			this.backButton.onClick.RemoveAllListeners();
			this.openedFromPortal = false;
		}

		// Token: 0x06006074 RID: 24692 RVA: 0x0021F7B8 File Offset: 0x0021DBB8
		public void SetupWorldInfo(ApiWorld world, ApiWorld.WorldInstance instance = null, bool newInstance = false, bool isPortal = false)
		{
			if (instance == null)
			{
				instance = world.GetBestInstance(null, false);
			}
			this.mWorld = world;
			this.worldInstance = instance;
			this.openedFromPortal = isPortal;
			ApiWorld.WorldInstance.AccessType accessType = this.worldInstance.GetAccessType();
			ApiWorld.WorldInstance.AccessDetail accessDetail = ApiWorld.WorldInstance.GetAccessDetail(accessType);
			string str = string.Empty;
			this.roomName.text = world.name;
			string instanceCreator = this.worldInstance.GetInstanceCreator();
			str = accessDetail.shortName;
			this.instanceStatus.text = accessDetail.fullName;
			if (accessType == ApiWorld.WorldInstance.AccessType.Public)
			{
				this.roomAuthor.text = world.authorName;
				this.roomOwner.text = world.authorName;
			}
			else if (accessType == ApiWorld.WorldInstance.AccessType.InviteOnly || accessType == ApiWorld.WorldInstance.AccessType.FriendsOnly || accessType == ApiWorld.WorldInstance.AccessType.FriendsOfGuests)
			{
				this.roomAuthor.text = world.authorName;
				this.roomOwner.text = "(loading...)";
				APIUser.Fetch(instanceCreator, delegate(APIUser user)
				{
					this.roomOwner.text = user.displayName;
				}, null);
			}
			this.playerCount.text = this.worldInstance.count.ToString();
			this.maxPlayers.text = world.capacity.ToString();
			this.worldInstanceId.text = "#" + this.worldInstance.idOnly + " " + str;
			Downloader.DownloadImage(world.imageUrl, delegate(string downloadedUrl, Texture2D obj)
			{
				this.roomImage.texture = obj;
			}, string.Empty);
			this.isPortalMenu = isPortal;
			this.ConfigureJoinButton(instance.count >= world.capacity, instance.isPublic);
			this.ConfigurePortalButton(accessType, instanceCreator == APIUser.CurrentUser.id, instance.count >= world.capacity, instance.isPublic);
			if (isPortal)
			{
				this.backButton.GetComponentInChildren<Text>().text = "Exit";
				this.backButton.onClick.AddListener(new UnityAction(this.Respawn));
				VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Backdrop/Backdrop");
				VRCUiManager.Instance.HideScreen("HEADER");
				QuickMenu.Instance.CanExitWithButton = false;
			}
			else
			{
				this.backButton.onClick.AddListener(new UnityAction(this.BackToWorlds));
				this.backButton.GetComponentInChildren<Text>().text = "Back";
			}
			if (this.il != null)
			{
				this.il.world = world;
				this.il.Refresh();
			}
			if (world.releaseStatus == "public" || world.authorId == APIUser.CurrentUser.id)
			{
				this.newInstanceButton.interactable = true;
			}
			if (instance.count >= world.capacity)
			{
				this.playerCount.text = "Full";
			}
			this.SetCurrentInstanceMessage(string.Empty, false);
			if (RoomManager.inRoom)
			{
				ApiWorld currentRoom = RoomManager.currentRoom;
				if (currentRoom.id == world.id && currentRoom.currentInstanceIdWithTags == this.worldInstance.idWithTags)
				{
					this.SetCurrentInstanceMessage("You are here", true);
				}
			}
			if (newInstance)
			{
				this.SetCurrentInstanceMessage("NEW INSTANCE", true);
			}
		}

		// Token: 0x06006075 RID: 24693 RVA: 0x0021FB0E File Offset: 0x0021DF0E
		private void Respawn()
		{
			SpawnManager.Instance.RespawnPlayerUsingOrder(VRCPlayer.Instance);
		}

		// Token: 0x06006076 RID: 24694 RVA: 0x0021FB20 File Offset: 0x0021DF20
		private void JoinRoom()
		{
			string instanceId = string.Empty;
			if (this.worldInstance != null)
			{
				instanceId = this.worldInstance.idWithTags;
			}
			VRCFlowManager.Instance.EnterWorld(this.mWorld.id, instanceId, false, delegate(string obj)
			{
				VRCUiPopupManager.Instance.ShowAlert("Cannot Join World Instance", obj, 10f);
			});
		}

		// Token: 0x06006077 RID: 24695 RVA: 0x0021FB7E File Offset: 0x0021DF7E
		private void CreatePortalAndAskForPassword()
		{
			this.CreatePortal();
		}

		// Token: 0x06006078 RID: 24696 RVA: 0x0021FB88 File Offset: 0x0021DF88
		private void CreatePortal()
		{
			if (RoomManager.inRoom && PortalInternal.CreatePortal(this.mWorld, this.worldInstance, VRCPlayer.Instance.transform.position, VRCPlayer.Instance.transform.forward, true))
			{
				this.portalButton.gameObject.SetActive(false);
			}
		}

		// Token: 0x06006079 RID: 24697 RVA: 0x0021FBE5 File Offset: 0x0021DFE5
		private void CloseRoom()
		{
			Debug.LogError("Rooms are no longer closed this way.");
		}

		// Token: 0x0600607A RID: 24698 RVA: 0x0021FBF1 File Offset: 0x0021DFF1
		private void CreatePrivate()
		{
		}

		// Token: 0x0600607B RID: 24699 RVA: 0x0021FBF3 File Offset: 0x0021DFF3
		private void BackToWorlds()
		{
			VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Screens/Worlds");
		}

		// Token: 0x0600607C RID: 24700 RVA: 0x0021FC04 File Offset: 0x0021E004
		public void CreateNewInstance()
		{
			VRCUiPopupManager.Instance.ShowRoomInstancePopup(this.mWorld, this);
		}

		// Token: 0x0600607D RID: 24701 RVA: 0x0021FC18 File Offset: 0x0021E018
		private void SetCurrentInstanceMessage(string message, bool enabled)
		{
			Text componentInChildren = this.currentInstanceUi.GetComponentInChildren<Text>(true);
			if (componentInChildren != null)
			{
				componentInChildren.text = message;
			}
			this.currentInstanceUi.SetActive(enabled);
		}

		// Token: 0x04004615 RID: 17941
		public RawImage roomImage;

		// Token: 0x04004616 RID: 17942
		public Text roomName;

		// Token: 0x04004617 RID: 17943
		public Text roomAuthor;

		// Token: 0x04004618 RID: 17944
		public Text roomOwner;

		// Token: 0x04004619 RID: 17945
		public Text playerCount;

		// Token: 0x0400461A RID: 17946
		public Text maxPlayers;

		// Token: 0x0400461B RID: 17947
		public Text instanceStatus;

		// Token: 0x0400461C RID: 17948
		public Text likeCount;

		// Token: 0x0400461D RID: 17949
		public Text visitCount;

		// Token: 0x0400461E RID: 17950
		public Text worldInstanceId;

		// Token: 0x0400461F RID: 17951
		public GameObject currentInstanceUi;

		// Token: 0x04004620 RID: 17952
		public Button joinButton;

		// Token: 0x04004621 RID: 17953
		public EventTrigger joinButtonOnClickTrigger;

		// Token: 0x04004622 RID: 17954
		public Button portalButton;

		// Token: 0x04004623 RID: 17955
		public EventTrigger portalButtonOnClickTrigger;

		// Token: 0x04004624 RID: 17956
		public Button backButton;

		// Token: 0x04004625 RID: 17957
		public Button newInstanceButton;

		// Token: 0x04004626 RID: 17958
		private ApiWorld mWorld;

		// Token: 0x04004627 RID: 17959
		public ApiWorld.WorldInstance worldInstance;

		// Token: 0x04004628 RID: 17960
		private Texture defaultImageTexture;

		// Token: 0x04004629 RID: 17961
		private bool isPortalMenu;

		// Token: 0x0400462A RID: 17962
		private bool portalCreated;

		// Token: 0x0400462B RID: 17963
		private UiWorldInstanceList il;

		// Token: 0x0400462C RID: 17964
		public bool openedFromPortal;
	}
}
