using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC;
using VRC.Core;
using VRC.UI;
using VRCCaptureUtils;
using VRCSDK2;

// Token: 0x02000C2D RID: 3117
public class QuickMenu : MonoBehaviour
{
	// Token: 0x17000D98 RID: 3480
	// (get) Token: 0x060060AD RID: 24749 RVA: 0x00220BCD File Offset: 0x0021EFCD
	// (set) Token: 0x060060AE RID: 24750 RVA: 0x00220BD4 File Offset: 0x0021EFD4
	public static QuickMenu Instance { get; private set; }

	// Token: 0x17000D99 RID: 3481
	// (get) Token: 0x060060AF RID: 24751 RVA: 0x00220BDC File Offset: 0x0021EFDC
	public bool IsActive
	{
		get
		{
			return this._isActive;
		}
	}

	// Token: 0x17000D9A RID: 3482
	// (get) Token: 0x060060B0 RID: 24752 RVA: 0x00220BE4 File Offset: 0x0021EFE4
	public bool IsOnRightHand
	{
		get
		{
			return this._currentHandIsRight;
		}
	}

	// Token: 0x17000D9B RID: 3483
	// (get) Token: 0x060060B1 RID: 24753 RVA: 0x00220BEC File Offset: 0x0021EFEC
	public APIUser SelectedUser
	{
		get
		{
			return this.selectedUser;
		}
	}

	// Token: 0x17000D9C RID: 3484
	// (get) Token: 0x060060B2 RID: 24754 RVA: 0x00220BF4 File Offset: 0x0021EFF4
	public bool isModerator
	{
		get
		{
			return VRC.Player.Instance.isModerator;
		}
	}

	// Token: 0x17000D9D RID: 3485
	// (get) Token: 0x060060B3 RID: 24755 RVA: 0x00220C00 File Offset: 0x0021F000
	public bool isRoomAuthor
	{
		get
		{
			return RoomManager.currentAuthorId == User.CurrentUser.id;
		}
	}

	// Token: 0x17000D9E RID: 3486
	// (get) Token: 0x060060B4 RID: 24756 RVA: 0x00220C16 File Offset: 0x0021F016
	public bool isInstanceOwner
	{
		get
		{
			return RoomManager.currentOwnerId == User.CurrentUser.id;
		}
	}

	// Token: 0x060060B5 RID: 24757 RVA: 0x00220C2C File Offset: 0x0021F02C
	private void Awake()
	{
		if (QuickMenu.Instance != null)
		{
			Debug.LogError("Duplicate QuickMenu instance detected");
			UnityEngine.Object.Destroy(this);
			return;
		}
		QuickMenu.Instance = this;
	}

	// Token: 0x060060B6 RID: 24758 RVA: 0x00220C55 File Offset: 0x0021F055
	private void OnDestroy()
	{
		VRCUiManager.Instance.onUiEnabled -= this.OnMainMenuUp;
		VRCUiManager.Instance.onUiDisabled -= this.OnMainMenuDown;
		QuickMenu.Instance = null;
	}

	// Token: 0x060060B7 RID: 24759 RVA: 0x00220C8C File Offset: 0x0021F08C
	private void Start()
	{
		this._menuRect = base.GetComponent<RectTransform>();
		this._shortcutMenu = base.transform.Find("ShortcutMenu").gameObject;
		this._emoteMenu = base.transform.Find("EmoteMenu").gameObject;
		this._emojiMenu = base.transform.Find("EmojiMenu").gameObject;
		this._userInteractMenu = base.transform.Find("UserInteractMenu").gameObject;
		this._notificationInteractMenu = base.transform.Find("NotificationInteractMenu").gameObject;
		this._cameraMenu = base.transform.Find("CameraMenu").gameObject;
		this._moderationMenu = base.transform.Find("ModerationMenu").gameObject;
		this._micControls = base.transform.Find("MicControls").gameObject;
		this._volumeSlider = base.transform.Find("MicControls/VolSlider").GetComponent<Slider>();
		this._menuButton = VRCInputManager.FindInput("Menu");
		this._backButton = VRCInputManager.FindInput("Back");
		this._useButtonL = VRCInputManager.FindInput("UseLeft");
		this._useButtonR = VRCInputManager.FindInput("UseRight");
		this._menuAudio = base.GetComponentInChildren<AudioSource>();
		this._micControls.SetActive(false);
		this._shortcutMenu.SetActive(false);
		this._emoteMenu.SetActive(false);
		this._emojiMenu.SetActive(false);
		this._userInteractMenu.SetActive(false);
		this._cameraMenu.SetActive(false);
		this.SetupForDesktopOrHMD();
		this.SetupModInvisibilityButton();
		this.SetupModTagButton();
	}

	// Token: 0x060060B8 RID: 24760 RVA: 0x00220E40 File Offset: 0x0021F240
	private void SetupCameraDeveloperOptions()
	{
		if (this.IsAValidDev())
		{
			this._cameraMenu.transform.Find("Polaroid").gameObject.SetActive(true);
			this._cameraMenu.transform.Find("RemoteVid").gameObject.SetActive(true);
			this._cameraMenu.transform.Find("PlayerVid").gameObject.SetActive(true);
			this._cameraMenu.transform.Find("Light").gameObject.SetActive(true);
		}
		else
		{
			this._cameraMenu.transform.Find("Polaroid").gameObject.SetActive(false);
			this._cameraMenu.transform.Find("RemoteVid").gameObject.SetActive(false);
			this._cameraMenu.transform.Find("PlayerVid").gameObject.SetActive(false);
			this._cameraMenu.transform.Find("Light").gameObject.SetActive(false);
		}
	}

	// Token: 0x060060B9 RID: 24761 RVA: 0x00220F60 File Offset: 0x0021F360
	private void SetupModerationButton()
	{
		Button component = this._shortcutMenu.transform.Find("ModButton").GetComponent<Button>();
		Text componentInChildren = this._shortcutMenu.transform.Find("ModButton").GetComponentInChildren<Text>();
		if (this.IsAValidDev())
		{
			componentInChildren.text = "Mod Tools";
			component.onClick.RemoveAllListeners();
			component.onClick.AddListener(new UnityAction(this.SetModMenu));
		}
		else
		{
			componentInChildren.text = "Report Abuse";
			component.onClick.RemoveAllListeners();
			component.onClick.AddListener(new UnityAction(this.RequestHost));
		}
	}

	// Token: 0x060060BA RID: 24762 RVA: 0x00221010 File Offset: 0x0021F410
	private void BuzzAndBeep()
	{
		VRC.Player localPlayer = VRC.Network.LocalPlayer;
		if (null == localPlayer || null == localPlayer.playerApi)
		{
			return;
		}
		if (null != this._menuAudio)
		{
			this._menuAudio.PlayOneShot(this.AttentionSound, 0.5f);
		}
		VRC_Pickup.PickupHand hand = VRC_Pickup.PickupHand.Left;
		if (this._rightHand)
		{
			hand = VRC_Pickup.PickupHand.Right;
		}
		localPlayer.playerApi.PlayHapticEventInHand(hand, 0.4f, 0.5f, 100f);
	}

	// Token: 0x060060BB RID: 24763 RVA: 0x00221094 File Offset: 0x0021F494
	private void SetupForDesktopOrHMD()
	{
		if (HMDManager.IsHmdDetected() && VRCTrackingManager.AreHandsTracked())
		{
			this._menuRect.localPosition = Vector3.zero;
			this._menuRect.localRotation = Quaternion.identity;
			this._menuRect.localScale = this._hmdMenuScale;
			this._hmd = true;
		}
		else
		{
			this._menuRect.localPosition = this._desktopMenuPos;
			this._menuRect.localRotation = Quaternion.Euler(this._desktopMenuRot);
			this._menuRect.localScale = this._desktopMenuScale;
			this._hmd = false;
		}
	}

	// Token: 0x060060BC RID: 24764 RVA: 0x00221134 File Offset: 0x0021F534
	private void SetEmoteLabels()
	{
		Animator anim = this.GetAnim();
		if (this._emoteLabelsNeedUpdate || this.IsSeated() != this._currentlySeated || (anim != null && anim != this._currentAnim))
		{
			this._currentlySeated = this.IsSeated();
			this._currentAnim = anim;
			for (int i = 0; i < 8; i++)
			{
				Transform transform = this._emoteMenu.transform.Find("EmoteButton" + i);
				Text component = transform.transform.Find("Text").GetComponent<Text>();
				AnimatorOverrideController animatorOverrideController = this._currentAnim.runtimeAnimatorController as AnimatorOverrideController;
				if (animatorOverrideController == null)
				{
					if (this._currentlySeated)
					{
						component.text = this._sitEmotes[i];
					}
					else
					{
						component.text = this._standEmotes[i];
					}
				}
				else
				{
					component.text = animatorOverrideController[string.Format("EMOTE{0}", i + 1)].name;
				}
			}
		}
	}

	// Token: 0x060060BD RID: 24765 RVA: 0x00221254 File Offset: 0x0021F654
	private void SetEmoteStatus(string status)
	{
		for (int i = 0; i < 8; i++)
		{
			Transform transform = this._emoteMenu.transform.Find("EmoteButton" + i);
			Text component = transform.transform.Find("Text").GetComponent<Text>();
			component.text = status;
		}
		this._emoteLabelsNeedUpdate = true;
	}

	// Token: 0x060060BE RID: 24766 RVA: 0x002212B8 File Offset: 0x0021F6B8
	private bool IsAValidDev()
	{
		return true;
	}

	// Token: 0x060060BF RID: 24767 RVA: 0x002212DC File Offset: 0x0021F6DC
	public void SetMenuIndex(int n)
	{
		if (n < 0 || n > 6)
		{
			return;
		}
		if (this._currentMenu != null)
		{
			this._currentMenu.SetActive(false);
		}
		switch (n)
		{
		case 0:
			this._currentMenu = this._shortcutMenu;
			this.SetMic();
			this.RefreshSitButton();
			this.SetupModerationButton();
			this.DeselectUser();
			break;
		case 1:
			this._currentMenu = this._emoteMenu;
			break;
		case 2:
			this._currentMenu = this._emojiMenu;
			break;
		case 3:
			if (this.selectedUser != null)
			{
				this._currentMenu = this._userInteractMenu;
				QuickMenuSocialElement componentInChildren = this._userInteractMenu.GetComponentInChildren<QuickMenuSocialElement>();
				componentInChildren.SetupElement(this.selectedUser, QuickMenuSocialElement.IconType.None);
				this.SetupUserInteractButtons();
			}
			else
			{
				this.CloseMenu();
			}
			break;
		case 4:
			if (this.selectedSocialElement != null)
			{
				this._currentMenu = this._notificationInteractMenu;
				QuickMenuSocialElement componentInChildren2 = this._notificationInteractMenu.GetComponentInChildren<QuickMenuSocialElement>();
				this._notificationInteractMenu.transform.Find("Message").GetComponentInChildren<Text>().text = this.selectedSocialElement.notification.message;
				componentInChildren2.SetupElement(this.selectedSocialElement);
			}
			else
			{
				this.CloseMenu();
			}
			break;
		case 5:
			this._currentMenu = this._cameraMenu;
			this.SetupCameraDeveloperOptions();
			break;
		case 6:
			this._currentMenu = this._moderationMenu;
			this.SetupModInvisibilityButton();
			break;
		}
		this._currentMenu.SetActive(true);
		this._currentMenuIndex = n;
	}

	// Token: 0x060060C0 RID: 24768 RVA: 0x00221482 File Offset: 0x0021F882
	private void SetModMenu()
	{
		this.SetMenuIndex(6);
	}

	// Token: 0x060060C1 RID: 24769 RVA: 0x0022148C File Offset: 0x0021F88C
	private IEnumerator PlaceUiAfterPause()
	{
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		VRCUiManager.Instance.PlaceUi();
		yield break;
	}

	// Token: 0x060060C2 RID: 24770 RVA: 0x002214A0 File Offset: 0x0021F8A0
	public void MainMenu(int screen)
	{
		APIUser laseredUser = this.selectedUser;
		this.CloseMenu();
		VRCUiManager.Instance.ShowUi(false, true);
		base.StartCoroutine(this.PlaceUiAfterPause());
		string text;
		switch (screen)
		{
		case 1:
			text = "UserInterface/MenuContent/Screens/Avatar";
			break;
		case 2:
			text = "UserInterface/MenuContent/Screens/Social";
			break;
		case 3:
			text = "UserInterface/MenuContent/Screens/Settings";
			break;
		case 4:
			if (laseredUser != null)
			{
				Debug.LogError("Details for " + laseredUser.username);
				PageUserInfo pageUserInfo = VRCUiManager.Instance.GetPage("UserInterface/MenuContent/Screens/UserInfo") as PageUserInfo;
				VRCUiManager.Instance.ShowScreen(pageUserInfo);
				PageUserInfo.InfoType infoType = PageUserInfo.InfoType.NotFriends;
				if (APIUser.CurrentUser.friends != null && APIUser.CurrentUser.friends.Find((APIUser u) => u.id == laseredUser.id) != null)
				{
					infoType = PageUserInfo.InfoType.OnlineFriend;
				}
				pageUserInfo.SetupUserInfo(laseredUser, infoType, UiUserList.ListType.InWorld);
			}
			text = string.Empty;
			break;
		default:
			text = "UserInterface/MenuContent/Screens/Worlds";
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			VRCUiManager.Instance.ShowScreen(text);
		}
	}

	// Token: 0x060060C3 RID: 24771 RVA: 0x002215CE File Offset: 0x0021F9CE
	public void RefreshMenuState()
	{
		if (!this._isActive)
		{
			return;
		}
		this.SetMenuIndex(this._currentMenuIndex);
	}

	// Token: 0x060060C4 RID: 24772 RVA: 0x002215E8 File Offset: 0x0021F9E8
	private bool IsLocomoting()
	{
		VRC.Player localPlayer = VRC.Network.LocalPlayer;
		if (localPlayer == null)
		{
			return true;
		}
		VRCMotionState component = localPlayer.GetComponent<VRCMotionState>();
		return component == null || component.isLocomoting;
	}

	// Token: 0x060060C5 RID: 24773 RVA: 0x00221624 File Offset: 0x0021FA24
	private bool IsSeated()
	{
		VRC.Player localPlayer = VRC.Network.LocalPlayer;
		VRCMotionState component = localPlayer.GetComponent<VRCMotionState>();
		return component.IsSeated;
	}

	// Token: 0x060060C6 RID: 24774 RVA: 0x00221644 File Offset: 0x0021FA44
	private Animator GetAnim()
	{
		VRC.Player localPlayer = VRC.Network.LocalPlayer;
		if (localPlayer != null)
		{
			return localPlayer.GetComponent<VRCPlayer>().avatarAnimator;
		}
		return null;
	}

	// Token: 0x060060C7 RID: 24775 RVA: 0x00221674 File Offset: 0x0021FA74
	public void OpenMenu(bool useRight)
	{
		this.DeselectUser();
		this.SetupForDesktopOrHMD();
		if (this._hmd)
		{
			this.PlaceMenuHmd(useRight);
		}
		else
		{
			VRCUiManager.Instance.PlaceUi();
		}
		this._visible = true;
		this._currentHandIsRight = useRight;
		this.SetMenuIndex(0);
		this._micControls.SetActive(true);
		VRCUiCursorManager.SetUiActive(true);
		VRCUiCursorManager.BlockCursor(useRight, true);
		VRCUiCursorManager.BlockCursor(!useRight, false);
		VRCUiCursorManager.SetDominantHand((!useRight) ? VRCUiCursor.CursorHandedness.Right : VRCUiCursor.CursorHandedness.Left);
		if (this._collider == null)
		{
			this._collider = base.GetComponent<Collider>();
		}
		if (this._collider != null)
		{
			this._collider.enabled = true;
		}
		this._isActive = true;
		if (TutorialManager.Instance != null)
		{
			TutorialManager.Instance.QuickMenuOpened(true);
		}
	}

	// Token: 0x060060C8 RID: 24776 RVA: 0x00221754 File Offset: 0x0021FB54
	public void CloseMenu()
	{
		if (this._currentMenu != null)
		{
			this._currentMenu.SetActive(false);
			this._micControls.SetActive(false);
		}
		this._visible = false;
		VRCUiCursorManager.SetUiActive(false);
		VRCUiCursorManager.BlockCursor(true, false);
		VRCUiCursorManager.BlockCursor(false, false);
		this.DeselectUser();
		if (this._collider == null)
		{
			this._collider = base.GetComponent<Collider>();
		}
		if (this._collider != null)
		{
			this._collider.enabled = false;
		}
		this._isActive = false;
		this._currentMenuIndex = -1;
		if (TutorialManager.Instance != null)
		{
			TutorialManager.Instance.QuickMenuOpened(false);
		}
	}

	// Token: 0x060060C9 RID: 24777 RVA: 0x0022180E File Offset: 0x0021FC0E
	public void ToggleMic()
	{
		DefaultTalkController.ToggleMute();
		this.SetMic();
		this.CloseMenu();
	}

	// Token: 0x060060CA RID: 24778 RVA: 0x00221821 File Offset: 0x0021FC21
	public void Respawn()
	{
		if (RoomManager.inRoom)
		{
			SpawnManager.Instance.RespawnPlayerUsingOrder(VRCPlayer.Instance);
			this.CloseMenu();
		}
	}

	// Token: 0x060060CB RID: 24779 RVA: 0x00221844 File Offset: 0x0021FC44
	private void SetMic()
	{
		Transform transform = this._micControls.transform.Find("MicButton");
		Transform transform2 = transform.Find("MicEnabled");
		Transform transform3 = transform.Find("MicDisabled");
		if (DefaultTalkController.IsLive())
		{
			transform2.gameObject.SetActive(true);
			transform3.gameObject.SetActive(false);
		}
		else
		{
			transform2.gameObject.SetActive(false);
			transform3.gameObject.SetActive(true);
		}
		this._volumeSlider.value = USpeaker.LocalGain;
	}

	// Token: 0x060060CC RID: 24780 RVA: 0x002218D0 File Offset: 0x0021FCD0
	public void VolumeUp()
	{
		float num = this._volumeSlider.value;
		num += 0.05f;
		if (num > 1f)
		{
			num = 1f;
		}
		this._volumeSlider.value = num;
	}

	// Token: 0x060060CD RID: 24781 RVA: 0x00221910 File Offset: 0x0021FD10
	public void VolumeDown()
	{
		float num = this._volumeSlider.value;
		num -= 0.05f;
		if (num < 0f)
		{
			num = 0f;
		}
		this._volumeSlider.value = num;
	}

	// Token: 0x060060CE RID: 24782 RVA: 0x00221950 File Offset: 0x0021FD50
	public void VolumeChange(float delta)
	{
		float num = this._volumeSlider.value;
		num += delta;
		if (num < 0f)
		{
			num = 0f;
		}
		if (num > 1f)
		{
			num = 1f;
		}
		this._volumeSlider.value = num;
	}

	// Token: 0x060060CF RID: 24783 RVA: 0x0022199C File Offset: 0x0021FD9C
	public void VolumeSliderChange()
	{
		float value = this._volumeSlider.value;
		if (HMDManager.IsHmdDetected())
		{
			VRCInputManager.micLevelVr = value;
		}
		else
		{
			VRCInputManager.micLevelDesk = value;
		}
		VRCPlayer.SettingsChanged();
	}

	// Token: 0x060060D0 RID: 24784 RVA: 0x002219D8 File Offset: 0x0021FDD8
	public void RequestHost()
	{
		this.CloseMenu();
		if (this._halpTimer <= 0f)
		{
			VRCUiPopupManager.Instance.ShowStandardPopup("Request Mod", "Request a mod to join you here?\nUrgent issues only, please.", "Yes", delegate
			{
				Debug.Log("<color=purple>User: " + APIUser.CurrentUser.displayName + " requests a mod.</color>");
				VRCUiPopupManager.Instance.HideCurrentPopup();
				this.SendHelpNotifyToOnlineHosts();
				this._halpTimer = 300f;
			}, "No", delegate
			{
				VRCUiPopupManager.Instance.HideCurrentPopup();
			}, null);
		}
		else
		{
			int num = Mathf.FloorToInt(5f);
			int num2 = Mathf.FloorToInt(this._halpTimer / 60f);
			int num3 = Mathf.FloorToInt(this._halpTimer) % 60;
			VRCUiPopupManager.Instance.ShowAlert("Please Wait", string.Concat(new object[]
			{
				"Sorry, but you must wait ",
				num,
				" minutes between requests.\nYou have ",
				string.Format("{0:0}:{1:00}", num2, num3),
				" remaining."
			}), 0f);
		}
	}

	// Token: 0x060060D1 RID: 24785 RVA: 0x00221AD0 File Offset: 0x0021FED0
	private void SendHelpNotifyToOnlineHosts()
	{
		APIUser.FetchOnlineModerators(true, delegate(List<APIUser> users)
		{
			ApiWorld.WorldInstance.AccessType currentInstanceAccess = RoomManager.currentRoom.currentInstanceAccess;
			ApiWorld.WorldInstance.AccessDetail accessDetail = ApiWorld.WorldInstance.GetAccessDetail(currentInstanceAccess);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["userId"] = APIUser.CurrentUser.id;
			dictionary["userName"] = APIUser.CurrentUser.displayName;
			dictionary["worldId"] = RoomManager.currentRoom.id + ":" + RoomManager.currentRoom.currentInstanceIdWithTags;
			dictionary["worldName"] = string.Concat(new string[]
			{
				RoomManager.currentRoom.name,
				" #",
				RoomManager.currentRoom.currentInstanceIdOnly,
				" ",
				accessDetail.shortName
			});
			dictionary["halpId"] = DateTime.UtcNow.ToString("yyyyMMddHHmmss-") + APIUser.CurrentUser.id;
			foreach (APIUser apiuser in users)
			{
				ApiNotification.SendNotification(apiuser.id, ApiNotification.NotificationType.Halp, string.Concat(new string[]
				{
					APIUser.CurrentUser.displayName,
					" needs help in \n",
					RoomManager.currentRoom.name,
					":",
					RoomManager.currentRoom.currentInstanceIdWithTags
				}), dictionary, null, null);
				VRCUiPopupManager.Instance.ShowAlert("Request Sent", "A mod will join you soon.", 10f);
				Debug.Log("<color=purple>QuickMenu/Sending a mod req to " + apiuser.displayName + "</color>");
			}
			if (users == null || users.Count == 0)
			{
				APIUser.PostHelpRequest(RoomManager.currentRoom.id, RoomManager.currentRoom.currentInstanceIdWithTags, null, null);
				VRCUiPopupManager.Instance.ShowAlert("No Mods Online", "Sorry, there are no mods online.\nYour request has been sent to our Slack channel.", 0f);
				Debug.Log("<color=purple>QuickMenu/SendHelpNotify: No mods online, sending to Halp (Slack).</color>");
			}
		}, delegate(string msg)
		{
			Debug.LogError("<QuickMenu/SendHelpNotify: Fetch Users Error: " + msg);
		});
	}

	// Token: 0x060060D2 RID: 24786 RVA: 0x00221B1D File Offset: 0x0021FF1D
	public void SelectUser(QuickMenuSocialElement e)
	{
		this.selectedUser = e.user;
		this.selectedNotification = e.notification;
		this.selectedSocialElement = e;
		if (e.isNotification)
		{
			this.SetMenuIndex(4);
		}
		else
		{
			this.SetMenuIndex(3);
		}
	}

	// Token: 0x060060D3 RID: 24787 RVA: 0x00221B5C File Offset: 0x0021FF5C
	public void FriendUser()
	{
		Text componentInChildren = this._userInteractMenu.transform.Find("FriendButton").GetComponentInChildren<Text>();
		if (APIUser.IsFriendsWith(this.selectedUser.id))
		{
			APIUser.UnfriendUser(this.selectedUser.id, null, null);
			ModerationManager.Instance.NotifyFriendStateChanged(this.selectedUser.id, false);
			componentInChildren.text = "Friend";
			QuickMenuSocialElement componentInChildren2 = this._userInteractMenu.GetComponentInChildren<QuickMenuSocialElement>();
			componentInChildren2.RefreshIcon();
		}
		else
		{
			ModerationManager.Instance.UnmuteUser(this.selectedUser.id);
			ModerationManager.Instance.UnblockUser(this.selectedUser.id);
			APIUser.SendFriendRequest(this.selectedUser.id, null, null);
			componentInChildren.text = "Friend Request Sent";
		}
	}

	// Token: 0x060060D4 RID: 24788 RVA: 0x00221C2C File Offset: 0x0022002C
	public void BlockUser()
	{
		if (this.selectedUser == null || this.selectedUser.developerType == null || this.selectedUser.developerType.Value != APIUser.DeveloperType.None || this.selectedUser.id == User.CurrentUser.id)
		{
			return;
		}
		VRC.Player player = PlayerManager.GetPlayer(this.selectedUser.id);
		if (ModerationManager.Instance.IsBlocked(this.selectedUser.id))
		{
			if (player != null)
			{
				player.ApplyBlock(false);
			}
			ModerationManager.Instance.UnblockUser(this.selectedUser.id);
		}
		else
		{
			if (player != null)
			{
				player.ApplyBlock(true);
			}
			ModerationManager.Instance.BlockUser(this.selectedUser.id);
		}
		this.RefreshMenuState();
	}

	// Token: 0x060060D5 RID: 24789 RVA: 0x00221D1C File Offset: 0x0022011C
	public void ToggleMuteUser()
	{
		if (this.selectedUser == null || this.selectedUser.developerType == null || this.selectedUser.developerType.Value != APIUser.DeveloperType.None || this.selectedUser.id == User.CurrentUser.id)
		{
			return;
		}
		VRC.Player player = PlayerManager.GetPlayer(this.selectedUser.id);
		if (ModerationManager.Instance.IsMuted(this.selectedUser.id))
		{
			if (player != null)
			{
				player.ApplyMute(false);
			}
			ModerationManager.Instance.UnmuteUser(this.selectedUser.id);
		}
		else
		{
			if (player != null)
			{
				player.ApplyMute(true);
			}
			ModerationManager.Instance.MuteUser(this.selectedUser.id);
		}
		this.RefreshMenuState();
		if (!ModerationManager.Instance.IsMuted(this.selectedUser.id) && TutorialManager.Instance != null)
		{
			TutorialManager.Instance.PlayerUnmuted();
		}
	}

	// Token: 0x060060D6 RID: 24790 RVA: 0x00221E40 File Offset: 0x00220240
	public void OnPlayerSelectedByLaser(VRCPlayer player)
	{
		this.selectedPlayer = player;
		VRC.Player component = player.GetComponent<VRC.Player>();
		APIUser cachedUser = APIUser.GetCachedUser(component.userId);
		if (cachedUser != null)
		{
			this.SelectUserByLaser(cachedUser);
		}
		else
		{
			APIUser.Fetch(component.userId, delegate(APIUser u)
			{
				this.SelectUserByLaser(u);
			}, delegate(string msg)
			{
				Debug.LogError("QuickMenu:OnPlayerSelected error on fetch:" + msg);
			});
		}
	}

	// Token: 0x060060D7 RID: 24791 RVA: 0x00221EAD File Offset: 0x002202AD
	private void SelectUserByLaser(APIUser u)
	{
		this.selectedUser = u;
		this.SetMenuIndex(3);
		if (TutorialManager.Instance != null)
		{
			TutorialManager.Instance.PlayerSelected();
		}
	}

	// Token: 0x060060D8 RID: 24792 RVA: 0x00221ED8 File Offset: 0x002202D8
	private void SetupUserInteractButtons()
	{
		if (this.selectedUser.developerType == null || this.selectedUser.developerType.Value != APIUser.DeveloperType.None)
		{
			this.muteButton.interactable = false;
			this.muteButtonMute.enabled = true;
			this.muteButtonMute.color = new Color(0.5f, 0f, 0f);
			this.muteButtonUnmute.enabled = false;
		}
		else
		{
			if (ModerationManager.Instance.IsMuted(this.selectedUser.id))
			{
				this.muteButtonMute.enabled = false;
				this.muteButtonUnmute.enabled = true;
			}
			else
			{
				this.muteButtonMute.enabled = true;
				this.muteButtonMute.color = new Color(1f, 0f, 0f);
				this.muteButtonUnmute.enabled = false;
			}
			this.muteButton.interactable = true;
		}
		Button component = this._userInteractMenu.transform.Find("MicOffButton").GetComponent<Button>();
		Button component2 = this._userInteractMenu.transform.Find("WarnButton").GetComponent<Button>();
		Button component3 = this._userInteractMenu.transform.Find("KickButton").GetComponent<Button>();
		Button component4 = this._userInteractMenu.transform.Find("BanButton").GetComponent<Button>();
		if (this.isModerator || this.isRoomAuthor || this.isInstanceOwner)
		{
			component.transform.gameObject.SetActive(true);
			component2.transform.gameObject.SetActive(true);
			component3.transform.gameObject.SetActive(true);
			if (this.isModerator)
			{
				component4.transform.gameObject.SetActive(true);
			}
			else
			{
				component4.transform.gameObject.SetActive(false);
			}
		}
		else
		{
			component4.transform.gameObject.SetActive(false);
			component.transform.gameObject.SetActive(false);
			component2.transform.gameObject.SetActive(false);
			component3.transform.gameObject.SetActive(false);
		}
		Button component5 = this._userInteractMenu.transform.Find("BlockButton").GetComponent<Button>();
		if (this.selectedUser.developerType == null || this.selectedUser.developerType.Value == APIUser.DeveloperType.Internal || User.CurrentUser.developerType == null || User.CurrentUser.developerType.Value == APIUser.DeveloperType.Internal || this.selectedUser.id == User.CurrentUser.id)
		{
			component5.interactable = false;
		}
		else
		{
			component5.interactable = true;
		}
		Text componentInChildren = component5.gameObject.GetComponentInChildren<Text>();
		if (ModerationManager.Instance.IsBlocked(this.selectedUser.id))
		{
			componentInChildren.text = "Unblock";
		}
		else
		{
			componentInChildren.text = "Block";
		}
		Transform transform = this._userInteractMenu.transform.Find("FriendButton");
		Text componentInChildren2 = transform.GetComponentInChildren<Text>();
		bool interactable = true;
		if (APIUser.IsFriendsWith(this.selectedUser.id))
		{
			componentInChildren2.text = "Unfriend";
		}
		else
		{
			componentInChildren2.text = "Friend";
			if (ModerationManager.Instance.IsBlockedEitherWay(this.selectedUser.id))
			{
				interactable = false;
			}
		}
		Button componentInChildren3 = transform.GetComponentInChildren<Button>();
		componentInChildren3.interactable = interactable;
		this._userInteractMenu.GetComponentInChildren<QuickMenuSocialElement>().RefreshIcon();
	}

	// Token: 0x060060D9 RID: 24793 RVA: 0x002222A6 File Offset: 0x002206A6
	public void ModeratorWarn()
	{
		ModerationManager.Instance.AskModeratorWarnUI(this.selectedUser);
	}

	// Token: 0x060060DA RID: 24794 RVA: 0x002222B8 File Offset: 0x002206B8
	public void ModeratorTurnOffMic()
	{
		ModerationManager.Instance.AskModeratorTurnOffMicUI(this.selectedUser);
	}

	// Token: 0x060060DB RID: 24795 RVA: 0x002222CA File Offset: 0x002206CA
	public void ModeratorKick()
	{
		ModerationManager.Instance.AskModeratorKickUI(this.selectedUser);
	}

	// Token: 0x060060DC RID: 24796 RVA: 0x002222DC File Offset: 0x002206DC
	public void ModeratorBan()
	{
		ModerationManager.Instance.AskModeratorBanUI(this.selectedUser);
	}

	// Token: 0x060060DD RID: 24797 RVA: 0x002222EE File Offset: 0x002206EE
	private void DeselectUser()
	{
		VRCUiCursorManager.ClearSelectedPlayer();
		this.selectedUser = null;
		this.selectedPlayer = null;
		this.selectedSocialElement = null;
	}

	// Token: 0x060060DE RID: 24798 RVA: 0x0022230C File Offset: 0x0022070C
	public void AcceptNotification()
	{
		if (this.selectedNotification == null)
		{
			Debug.LogError("Could not accept notif bc notif is null");
			return;
		}
		if (this.selectedNotification.details == null)
		{
			Debug.LogError("Could not accept notif bc notif details is null");
			return;
		}
		bool flag = false;
		switch (this.selectedNotification.notificationType)
		{
		case ApiNotification.NotificationType.Friendrequest:
			ModerationManager.Instance.UnmuteUser(this.selectedNotification.senderUserId);
			ModerationManager.Instance.UnblockUser(this.selectedNotification.senderUserId);
			APIUser.AcceptFriendRequest(this.selectedNotification.id, null, null);
			APIUser.LocalAddFriend(this.selectedUser);
			ModerationManager.Instance.NotifyFriendStateChanged(this.selectedNotification.senderUserId, true);
			QuickMenuSocial.Instance.dontFetchNotificationsForOnePeriod = true;
			ApiNotification.DeleteNotification(this.selectedNotification.id, null, null);
			flag = true;
			break;
		case ApiNotification.NotificationType.Invite:
			if (this.selectedNotification.details.ContainsKey("worldId"))
			{
				string roomId = this.selectedNotification.details["worldId"] as string;
				VRCFlowManager.Instance.EnterRoom(roomId, null);
				ApiNotification.DeleteNotification(this.selectedNotification.id, null, null);
			}
			break;
		case ApiNotification.NotificationType.Requestinvite:
		{
			string senderUserId = this.selectedNotification.senderUserId;
			string message = "Join me in " + RoomManager.currentRoom.name;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["worldId"] = RoomManager.currentRoom.id + ":" + RoomManager.currentRoom.currentInstanceIdWithTags;
			ApiNotification.SendNotification(senderUserId, ApiNotification.NotificationType.Invite, message, dictionary, null, null);
			ApiNotification.DeleteNotification(this.selectedNotification.id, null, null);
			flag = true;
			break;
		}
		case ApiNotification.NotificationType.VoteToKick:
		{
			string userToKickId = this.selectedNotification.details["userToKickId"] as string;
			string initiatorUserId = this.selectedNotification.details["initiatorUserId"] as string;
			ModerationManager.Instance.SendVoteKick(initiatorUserId, userToKickId, true);
			NotificationManager.Instance.RemoveNotification(this.selectedNotification, NotificationManager.HistoryRange.Local);
			ApiNotification.DeleteNotification(this.selectedNotification.id, null, null);
			flag = true;
			break;
		}
		case ApiNotification.NotificationType.Halp:
			if (this.selectedNotification.details.ContainsKey("worldId") && this.selectedNotification.details.ContainsKey("halpId"))
			{
				string roomId2 = this.selectedNotification.details["worldId"] as string;
				VRCFlowManager.Instance.EnterRoom(roomId2, null);
				ApiNotification.DeleteNotification(this.selectedNotification.id, null, null);
				APIUser.FetchOnlineModerators(true, delegate(List<APIUser> mods)
				{
					foreach (APIUser apiuser in mods)
					{
						if (apiuser.id != APIUser.CurrentUser.id)
						{
							Debug.Log(string.Concat(new string[]
							{
								"<color=purple>Sending help recvd to other mod:",
								apiuser.id,
								" != responder:",
								APIUser.CurrentUser.id,
								"</color>"
							}));
							string displayName = APIUser.CurrentUser.displayName;
							string text = this.selectedNotification.details["userName"] as string;
							string text2 = this.selectedNotification.details["worldName"] as string;
							Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
							dictionary2["halpId"] = this.selectedNotification.details["halpId"];
							dictionary2["userId"] = this.selectedNotification.details["userId"];
							dictionary2["worldId"] = this.selectedNotification.details["worldId"];
							ApiNotification.SendNotification(apiuser.id, ApiNotification.NotificationType.Hidden, string.Concat(new string[]
							{
								displayName,
								" is helping ",
								text,
								"\nin ",
								text2
							}), dictionary2, null, null);
						}
					}
				}, delegate(string msg)
				{
					Debug.LogError("QuickMenu::AcceptNotification: Fetching Moderator request failed.");
				});
			}
			break;
		}
		NotificationManager.Instance.RemoveNotification(this.selectedNotification, NotificationManager.HistoryRange.Recent);
		if (flag)
		{
			this.SetMenuIndex(0);
		}
	}

	// Token: 0x060060DF RID: 24799 RVA: 0x00222600 File Offset: 0x00220A00
	public void DeclineNotification()
	{
		if (this.selectedNotification == null)
		{
			Debug.LogError("Could not accept notif bc notif is null");
			return;
		}
		switch (this.selectedNotification.notificationType)
		{
		case ApiNotification.NotificationType.Friendrequest:
			APIUser.DeclineFriendRequest(this.selectedNotification.id, null, null);
			QuickMenuSocial.Instance.dontFetchNotificationsForOnePeriod = true;
			break;
		case ApiNotification.NotificationType.Invite:
		case ApiNotification.NotificationType.Requestinvite:
		case ApiNotification.NotificationType.Halp:
		case ApiNotification.NotificationType.Hidden:
			ApiNotification.DeleteNotification(this.selectedNotification.id, null, null);
			break;
		case ApiNotification.NotificationType.VoteToKick:
		{
			string userToKickId = this.selectedNotification.details["userToKickId"] as string;
			string initiatorUserId = this.selectedNotification.details["initiatorUserId"] as string;
			ModerationManager.Instance.SendVoteKick(initiatorUserId, userToKickId, false);
			NotificationManager.Instance.RemoveNotification(this.selectedNotification, NotificationManager.HistoryRange.Local);
			break;
		}
		}
		NotificationManager.Instance.RemoveNotification(this.selectedNotification, NotificationManager.HistoryRange.Recent);
		this.SetMenuIndex(0);
	}

	// Token: 0x060060E0 RID: 24800 RVA: 0x002226FE File Offset: 0x00220AFE
	public void ToggleSitting()
	{
		this._seatedPlay = VRCTrackingManager.SetSeatedPlayMode(!this._seatedPlay);
		this.RefreshSitButton();
	}

	// Token: 0x060060E1 RID: 24801 RVA: 0x0022271C File Offset: 0x00220B1C
	private void SetImmobileInput(bool flag)
	{
		if (flag && !this._immobileInput)
		{
			Debug.LogError("IMMOBILE INPUT TRUE");
			this._immobileInput = true;
			InputStateControllerManager.localInstance.PushInputController("UIInputController");
		}
		else if (!flag && this._immobileInput)
		{
			Debug.LogError("IMMOBILE INPUT FALSE");
			this._immobileInput = false;
			InputStateControllerManager.localInstance.PopInputController();
		}
	}

	// Token: 0x060060E2 RID: 24802 RVA: 0x0022278C File Offset: 0x00220B8C
	public void RefreshSitButton()
	{
		Transform transform = this._shortcutMenu.transform.Find("SitButton");
		Text componentInChildren = transform.GetComponentInChildren<Text>();
		GameObject gameObject = transform.Find("SeatedIcon").gameObject;
		GameObject gameObject2 = transform.Find("StandingIcon").gameObject;
		this._seatedPlay = VRCTrackingManager.GetSeatedPlayMode();
		if (this._seatedPlay)
		{
			componentInChildren.text = "Seated Play";
			gameObject.SetActive(true);
			gameObject2.SetActive(false);
		}
		else
		{
			componentInChildren.text = "Standing Play";
			gameObject.SetActive(false);
			gameObject2.SetActive(true);
		}
	}

	// Token: 0x060060E3 RID: 24803 RVA: 0x00222825 File Offset: 0x00220C25
	public void TriggerEmote(int n)
	{
		VRCPlayer.Instance.PlayEmote(n);
	}

	// Token: 0x060060E4 RID: 24804 RVA: 0x00222832 File Offset: 0x00220C32
	public void TriggerEmoji(int n)
	{
		VRCPlayer.Instance.SpawnEmoji(n);
	}

	// Token: 0x060060E5 RID: 24805 RVA: 0x0022283F File Offset: 0x00220C3F
	private void PlaceMenuHmd(bool useRight)
	{
		this._rightHand = useRight;
	}

	// Token: 0x060060E6 RID: 24806 RVA: 0x00222848 File Offset: 0x00220C48
	private void CheckAndSignalDisabledUse()
	{
		VRCUiCursor vrcuiCursor = null;
		if (this._useButtonL.down)
		{
			vrcuiCursor = VRCUiCursorManager.GetInstance().handLeftCursor;
		}
		if (this._useButtonR.down)
		{
			vrcuiCursor = VRCUiCursorManager.GetInstance().handRightCursor;
		}
		if (vrcuiCursor != null && !vrcuiCursor.over.Contains(VRCUiCursor.CursorOver.Ui))
		{
			this.BuzzAndBeep();
		}
	}

	// Token: 0x060060E7 RID: 24807 RVA: 0x002228B0 File Offset: 0x00220CB0
	private void UpdateMenuTransform()
	{
		if (!this._inMainMenu && this._visible && this._hmd)
		{
			if (this._rightHand)
			{
				Transform trackedTransform = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.HandTracker_RightPalm);
				base.transform.position = trackedTransform.position + trackedTransform.TransformVector(this._hmdMenuPositionR);
				base.transform.rotation = trackedTransform.rotation * Quaternion.Euler(this._hmdMenuRotationR);
			}
			else
			{
				Transform trackedTransform2 = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.HandTracker_LeftPalm);
				base.transform.position = trackedTransform2.position + trackedTransform2.TransformVector(this._hmdMenuPositionL);
				base.transform.rotation = trackedTransform2.rotation * Quaternion.Euler(this._hmdMenuRotationL);
			}
		}
	}

	// Token: 0x060060E8 RID: 24808 RVA: 0x00222984 File Offset: 0x00220D84
	private void FixedUpdate()
	{
		this.UpdateMenuTransform();
		this._wasFixedUpdateCalled = true;
	}

	// Token: 0x060060E9 RID: 24809 RVA: 0x00222993 File Offset: 0x00220D93
	private void OnMainMenuUp()
	{
		this._inMainMenu = true;
		this.CanExitWithButton = true;
	}

	// Token: 0x060060EA RID: 24810 RVA: 0x002229A3 File Offset: 0x00220DA3
	private void OnMainMenuDown()
	{
		this._inMainMenu = false;
		this.CanExitWithButton = false;
	}

	// Token: 0x060060EB RID: 24811 RVA: 0x002229B4 File Offset: 0x00220DB4
	private void Update()
	{
		if (!this._inited)
		{
			if (this._collider == null)
			{
				this._collider = base.GetComponent<Collider>();
			}
			this._collider.enabled = false;
			this._seatedPlay = VRCTrackingManager.GetSeatedPlayMode();
			this.RefreshSitButton();
			this._inited = true;
			VRCUiManager.Instance.onUiEnabled += this.OnMainMenuUp;
			VRCUiManager.Instance.onUiDisabled += this.OnMainMenuDown;
			if (!QuickMenu.debugInitTooLongWarned && Time.timeSinceLevelLoad > 60f)
			{
				Debug.LogWarning("QuickMenu: took longer than one minute to initialize!");
				QuickMenu.debugInitTooLongWarned = true;
			}
		}
		if (!this._inited)
		{
			return;
		}
		if (this._halpTimer > 0f)
		{
			this._halpTimer -= Time.deltaTime;
		}
		bool flag = this._inMainMenu || VRCUiManager.Instance.IsActive();
		if (flag)
		{
			if (this.CanExitWithButton && (this._menuButton.down || this._backButton.down) && APIUser.IsLoggedIn)
			{
				VRCUiManager.Instance.CloseUi(true);
				VRCUiCursorManager.SetUiActive(false);
			}
		}
		else
		{
			if (!this.IsAllowed)
			{
				if (this._visible)
				{
					this.CloseMenu();
				}
				return;
			}
			if (this.IsLocomoting())
			{
				if (this._visible)
				{
					this.CloseMenu();
				}
			}
			else
			{
				if (!this._visible)
				{
					if (this._menuButton.down)
					{
						base.gameObject.DebugPrint("QuickMenu: User pressed menu button for quick (R).", new object[]
						{
							2,
							2
						});
						this.OpenMenu(true);
					}
					else if (this._backButton.down)
					{
						base.gameObject.DebugPrint("QuickMenu: User pressed menu button for quick (L).", new object[]
						{
							2,
							2
						});
						this.OpenMenu(false);
					}
				}
				else
				{
					if (!this._wasFixedUpdateCalled)
					{
						this.UpdateMenuTransform();
					}
					else
					{
						this._wasFixedUpdateCalled = false;
					}
					if (this._menuButton.down)
					{
						this.CloseMenu();
						if (!this._currentHandIsRight)
						{
							this.OpenMenu(true);
							base.gameObject.DebugPrint("QuickMenu: User pressed menu button to switch (R).", new object[]
							{
								2,
								2
							});
						}
					}
					else if (this._backButton.down)
					{
						this.CloseMenu();
						if (this._currentHandIsRight)
						{
							this.OpenMenu(false);
							base.gameObject.DebugPrint("QuickMenu: User pressed menu button to switch (L).", new object[]
							{
								2,
								2
							});
						}
					}
					VRCPlayer vrcplayer = VRCUiCursorManager.GetSelectedPlayer();
					if (vrcplayer != null && vrcplayer != this.selectedPlayer)
					{
						this.OnPlayerSelectedByLaser(vrcplayer);
					}
					else
					{
						this.CheckAndSignalDisabledUse();
					}
				}
				if (this._currentMenu == this._emoteMenu)
				{
					if (VRCPlayer.Instance.IsPlayingEmote())
					{
						this.SetEmoteStatus("[playing]");
					}
					else
					{
						this.SetEmoteLabels();
					}
				}
			}
		}
	}

	// Token: 0x060060EC RID: 24812 RVA: 0x00222D00 File Offset: 0x00221100
	public void ActivateCamera(string cameraType)
	{
		VRCCapturePanorama vrccapturePanorama = UnityEngine.Object.FindObjectOfType<VRCCapturePanorama>();
		if (cameraType != null)
		{
			if (!(cameraType == "panorama"))
			{
				if (cameraType == "vrchive")
				{
					this.CloseMenu();
					vrccapturePanorama.uploadImages = true;
					vrccapturePanorama.CapturePanorama();
				}
			}
			else
			{
				this.CloseMenu();
				vrccapturePanorama.uploadImages = false;
				vrccapturePanorama.CapturePanorama();
			}
		}
	}

	// Token: 0x060060ED RID: 24813 RVA: 0x00222D70 File Offset: 0x00221170
	public void SpawnCaptureUtil(string type)
	{
		int playerId = VRC.Network.LocalPlayer.playerApi.playerId;
		Vector3 position = VRCPlayer.Instance.transform.position + VRCPlayer.Instance.transform.up * 1.3f + VRCPlayer.Instance.transform.forward * 1f;
		Quaternion quaternion = VRCPlayer.Instance.transform.rotation;
		if (type != null)
		{
			if (!(type == "polaroid"))
			{
				if (!(type == "remotevidcam"))
				{
					if (!(type == "playervidcam"))
					{
						if (type == "dynlight")
						{
							this.CloseMenu();
							VRC.Network.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, "CapturePrefabs/DynamicSpot", position, quaternion);
						}
					}
					else
					{
						this.CloseMenu();
						GameObject gameObject = VRC.Network.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, "CapturePrefabs/PlayerVideoCamera", position, quaternion);
						GameObject remoteCamera = VRC.Network.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, "CapturePrefabs/HiddenRemoteCamera", position, quaternion);
						PlayerCamViewer component = gameObject.GetComponent<PlayerCamViewer>();
						component.SetRemoteCamera(remoteCamera);
					}
				}
				else
				{
					this.CloseMenu();
					GameObject gameObject2 = VRC.Network.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, "CapturePrefabs/RemoteVideoCamera", position, quaternion);
					GameObject inGameCamera = VRC.Network.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, "CapturePrefabs/InGameCamera", position, quaternion);
					RemoteVideoCamera component2 = gameObject2.GetComponent<RemoteVideoCamera>();
					component2.SetInGameCamera(inGameCamera);
				}
			}
			else
			{
				this.CloseMenu();
				quaternion *= Quaternion.Euler(0f, 180f, 0f);
				VRC.Network.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, "CapturePrefabs/PolaroidCamera", position, quaternion);
			}
		}
	}

	// Token: 0x060060EE RID: 24814 RVA: 0x00222EF8 File Offset: 0x002212F8
	public void ClearRoom(int type)
	{
		if (VRC.Network.LocalPlayer.isModerator)
		{
			if (VRCPlayer.Instance != null)
			{
				VRCPlayer.Instance.ModClearRoom(type);
			}
			else
			{
				Debug.LogError("ClearRoom: local player is null");
			}
		}
		else
		{
			Debug.LogError("ClearRoom: local player not moderator");
		}
	}

	// Token: 0x060060EF RID: 24815 RVA: 0x00222F4D File Offset: 0x0022134D
	public void ToggleModInvisibility()
	{
		if (this.IsAValidDev())
		{
			VRCPlayer.Instance.isInvisible = !VRCPlayer.Instance.isInvisible;
			this.SetupModInvisibilityButton();
		}
	}

	// Token: 0x060060F0 RID: 24816 RVA: 0x00222F78 File Offset: 0x00221378
	private void SetupModInvisibilityButton()
	{
		Button component = this._moderationMenu.transform.Find("InvisibilityButton").GetComponent<Button>();
		if (component != null)
		{
			Text componentInChildren = component.GetComponentInChildren<Text>();
			if (componentInChildren != null)
			{
				if (VRCPlayer.LocalIsInvisible)
				{
					componentInChildren.text = "Go\nVisible";
				}
				else
				{
					componentInChildren.text = "Go\nInvisible";
				}
			}
		}
	}

	// Token: 0x060060F1 RID: 24817 RVA: 0x00222FE4 File Offset: 0x002213E4
	public void ToggleModTag()
	{
		if (this.IsAValidDev())
		{
			if (!string.IsNullOrEmpty(VRCPlayer.LocalModTag))
			{
				VRCPlayer.Instance.modTag = string.Empty;
			}
			else
			{
				VRCPlayer.Instance.modTag = VRCPlayer.DefaultModTag;
			}
			this.SetupModTagButton();
		}
	}

	// Token: 0x060060F2 RID: 24818 RVA: 0x00223034 File Offset: 0x00221434
	private void SetupModTagButton()
	{
		Button component = this._moderationMenu.transform.Find("TagButton").GetComponent<Button>();
		if (component != null)
		{
			Text componentInChildren = component.GetComponentInChildren<Text>();
			if (componentInChildren != null)
			{
				if (string.IsNullOrEmpty(VRCPlayer.LocalModTag))
				{
					componentInChildren.text = "Show\nTag";
				}
				else
				{
					componentInChildren.text = "Hide\nTag";
				}
			}
		}
	}

	// Token: 0x060060F3 RID: 24819 RVA: 0x002230A8 File Offset: 0x002214A8
	public Transform FindUnmuteButtonIfActive()
	{
		if (this.muteButton != null && this.muteButton.gameObject.activeInHierarchy && this.muteButton.interactable)
		{
			return this.muteButton.transform;
		}
		return null;
	}

	// Token: 0x060060F4 RID: 24820 RVA: 0x002230F8 File Offset: 0x002214F8
	public bool IsActiveOnDesktop()
	{
		return this.IsActive && (!HMDManager.IsHmdDetected() || !VRCTrackingManager.AreHandsTracked());
	}

	// Token: 0x04004652 RID: 18002
	public AudioClip AttentionSound;

	// Token: 0x04004653 RID: 18003
	public DefaultTalkController talkController;

	// Token: 0x04004654 RID: 18004
	public Button muteButton;

	// Token: 0x04004655 RID: 18005
	public Image muteButtonMute;

	// Token: 0x04004656 RID: 18006
	public Image muteButtonUnmute;

	// Token: 0x04004657 RID: 18007
	public bool IsAllowed;

	// Token: 0x04004658 RID: 18008
	public bool CanExitWithButton;

	// Token: 0x04004659 RID: 18009
	private GameObject _shortcutMenu;

	// Token: 0x0400465A RID: 18010
	private GameObject _emoteMenu;

	// Token: 0x0400465B RID: 18011
	private GameObject _emojiMenu;

	// Token: 0x0400465C RID: 18012
	private GameObject _userInteractMenu;

	// Token: 0x0400465D RID: 18013
	private GameObject _notificationInteractMenu;

	// Token: 0x0400465E RID: 18014
	private GameObject _cameraMenu;

	// Token: 0x0400465F RID: 18015
	private GameObject _moderationMenu;

	// Token: 0x04004660 RID: 18016
	private GameObject _currentMenu;

	// Token: 0x04004661 RID: 18017
	private int _currentMenuIndex = -1;

	// Token: 0x04004662 RID: 18018
	private GameObject _micControls;

	// Token: 0x04004663 RID: 18019
	private Slider _volumeSlider;

	// Token: 0x04004664 RID: 18020
	private RectTransform _menuRect;

	// Token: 0x04004665 RID: 18021
	private VRCInput _menuButton;

	// Token: 0x04004666 RID: 18022
	private VRCInput _menu2Button;

	// Token: 0x04004667 RID: 18023
	private VRCInput _backButton;

	// Token: 0x04004668 RID: 18024
	private VRCInput _useButtonR;

	// Token: 0x04004669 RID: 18025
	private VRCInput _useButtonL;

	// Token: 0x0400466A RID: 18026
	private AudioSource _menuAudio;

	// Token: 0x0400466B RID: 18027
	private Animator _currentAnim;

	// Token: 0x0400466C RID: 18028
	private bool _visible;

	// Token: 0x0400466D RID: 18029
	private bool _currentHandIsRight;

	// Token: 0x0400466E RID: 18030
	private bool _currentlySeated;

	// Token: 0x0400466F RID: 18031
	private bool _inMainMenu;

	// Token: 0x04004670 RID: 18032
	private bool _immobileInput;

	// Token: 0x04004671 RID: 18033
	private bool _hmd;

	// Token: 0x04004672 RID: 18034
	private Vector3 _hmdMenuPositionL = new Vector3(0f, 1.7f, 1.1f);

	// Token: 0x04004673 RID: 18035
	private Vector3 _hmdMenuRotationL = new Vector3(180f, -80f, -76f);

	// Token: 0x04004674 RID: 18036
	private Vector3 _hmdMenuPositionR = new Vector3(0f, -1.7f, 1.1f);

	// Token: 0x04004675 RID: 18037
	private Vector3 _hmdMenuRotationR = new Vector3(180f, -80f, -104f);

	// Token: 0x04004676 RID: 18038
	private Vector3 _hmdMenuScale = new Vector3(0.0002f, 0.0002f, 0.0002f);

	// Token: 0x04004677 RID: 18039
	private Vector3 _desktopMenuPos = new Vector3(0f, -0.5f, 0.75f);

	// Token: 0x04004678 RID: 18040
	private Vector3 _desktopMenuRot = new Vector3(25f, 0f, 0f);

	// Token: 0x04004679 RID: 18041
	private Vector3 _desktopMenuScale = new Vector3(0.0003f, 0.0003f, 0.0003f);

	// Token: 0x0400467A RID: 18042
	private bool _emoteLabelsNeedUpdate = true;

	// Token: 0x0400467B RID: 18043
	private bool _isActive;

	// Token: 0x0400467C RID: 18044
	private InputStateControllerManager _inputControllerManager;

	// Token: 0x0400467D RID: 18045
	private APIUser selectedUser;

	// Token: 0x0400467E RID: 18046
	private ApiNotification selectedNotification;

	// Token: 0x0400467F RID: 18047
	private QuickMenuSocialElement selectedSocialElement;

	// Token: 0x04004680 RID: 18048
	private const int NUM_EMOTES = 8;

	// Token: 0x04004681 RID: 18049
	private const int NUM_EMOJIS = 8;

	// Token: 0x04004682 RID: 18050
	private string[] _standEmotes = new string[]
	{
		"Wave",
		"Clap",
		"Point",
		"Cheer",
		"Dance",
		"Backflip",
		"Die",
		"Sadness"
	};

	// Token: 0x04004683 RID: 18051
	private string[] _sitEmotes = new string[]
	{
		"Laugh",
		"Point",
		"Raise Hand",
		"Drum",
		"Clap",
		"Anger",
		"Disbelief",
		"Disapprove"
	};

	// Token: 0x04004684 RID: 18052
	private const float HALP_COOLDOWN_TIME = 300f;

	// Token: 0x04004685 RID: 18053
	private float _halpTimer;

	// Token: 0x04004686 RID: 18054
	private bool _inited;

	// Token: 0x04004687 RID: 18055
	private bool _seatedPlay;

	// Token: 0x04004688 RID: 18056
	private Collider _collider;

	// Token: 0x04004689 RID: 18057
	private bool _rightHand;

	// Token: 0x0400468A RID: 18058
	private bool _wasFixedUpdateCalled;

	// Token: 0x0400468B RID: 18059
	private VRCPlayer selectedPlayer;

	// Token: 0x0400468C RID: 18060
	private static bool debugInitTooLongWarned;
}
