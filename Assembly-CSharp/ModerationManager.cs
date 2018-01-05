using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000AA7 RID: 2727
public class ModerationManager : MonoBehaviour
{
	// Token: 0x17000C06 RID: 3078
	// (get) Token: 0x06005206 RID: 20998 RVA: 0x001C1749 File Offset: 0x001BFB49
	public static ModerationManager Instance
	{
		get
		{
			return ModerationManager.instance;
		}
	}

	// Token: 0x14000052 RID: 82
	// (add) Token: 0x06005207 RID: 20999 RVA: 0x001C1750 File Offset: 0x001BFB50
	// (remove) Token: 0x06005208 RID: 21000 RVA: 0x001C1788 File Offset: 0x001BFB88
	public event ModerationManager.ModerationChangeEvent moderationsChanged;

	// Token: 0x17000C07 RID: 3079
	// (get) Token: 0x06005209 RID: 21001 RVA: 0x001C17BE File Offset: 0x001BFBBE
	public bool isModerator
	{
		get
		{
			return VRC.Player.Instance.isModerator;
		}
	}

	// Token: 0x17000C08 RID: 3080
	// (get) Token: 0x0600520A RID: 21002 RVA: 0x001C17CA File Offset: 0x001BFBCA
	public bool isRoomAuthor
	{
		get
		{
			return RoomManager.currentAuthorId == User.CurrentUser.id;
		}
	}

	// Token: 0x17000C09 RID: 3081
	// (get) Token: 0x0600520B RID: 21003 RVA: 0x001C17E0 File Offset: 0x001BFBE0
	public bool isInstanceOwner
	{
		get
		{
			return RoomManager.currentOwnerId == User.CurrentUser.id;
		}
	}

	// Token: 0x0600520C RID: 21004 RVA: 0x001C17F8 File Offset: 0x001BFBF8
	public void FetchModerations(Action onSuccess = null, Action<string> onError = null)
	{
		this.awaitingResults = 3;
		ApiModeration.LocalFetchAll(delegate(List<ApiModeration> mods)
		{
			this.globalModerations = mods;
			this.awaitingResults--;
			if (this.awaitingResults <= 0 && onSuccess != null)
			{
				onSuccess();
				if (this.moderationsChanged != null)
				{
					this.moderationsChanged();
				}
			}
		}, delegate(string message)
		{
			if (onError != null)
			{
				onError(message);
			}
		});
		ApiPlayerModeration.FetchAllMine(delegate(List<ApiPlayerModeration> mods)
		{
			this.playerModerationsMine = mods;
			this.awaitingResults--;
			if (this.awaitingResults <= 0 && onSuccess != null)
			{
				onSuccess();
				if (this.moderationsChanged != null)
				{
					this.moderationsChanged();
				}
			}
		}, delegate(string message)
		{
			if (onError != null)
			{
				onError(message);
			}
		});
		ApiPlayerModeration.FetchAllAgainstMe(delegate(List<ApiPlayerModeration> mods)
		{
			this.playerModerationsAgainstMe = mods;
			this.awaitingResults--;
			if (this.awaitingResults <= 0 && onSuccess != null)
			{
				onSuccess();
				if (this.moderationsChanged != null)
				{
					this.moderationsChanged();
				}
			}
		}, delegate(string message)
		{
			if (onError != null)
			{
				onError(message);
			}
		});
	}

	// Token: 0x0600520D RID: 21005 RVA: 0x001C1880 File Offset: 0x001BFC80
	public bool SelfCheckAndEnforceModerations()
	{
		if (VRC.Player.Instance == null || VRC.Player.Instance.isModerator)
		{
			return false;
		}
		if (APIUser.IsLoggedInWithCredentials && this.IsBanned(VRC.Player.Instance.userId))
		{
			this.EnforceBan();
			return true;
		}
		if (this.CheckAndEnforcePublicOnlyBan())
		{
			return true;
		}
		if (this.IsKicked(APIUser.CurrentUser.id))
		{
			this.ModKickUser(APIUser.CurrentUser);
			return true;
		}
		return false;
	}

	// Token: 0x0600520E RID: 21006 RVA: 0x001C1905 File Offset: 0x001BFD05
	public void OnUserLoggedIn()
	{
		this.wasLocalUserPublicOnlyBanned = false;
	}

	// Token: 0x0600520F RID: 21007 RVA: 0x001C1910 File Offset: 0x001BFD10
	public void ModWarnUser(APIUser u)
	{
		string worldId = string.Empty;
		string worldInstanceId = string.Empty;
		string text = "A moderator has sent you a warning for bad behaviour. If this continues, you may be kicked or banned.";
		if (RoomManager.inRoom)
		{
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "WarnUserRPC", new object[]
			{
				u.id,
				text
			});
			worldId = RoomManager.currentRoom.id;
			worldInstanceId = RoomManager.currentRoom.currentInstanceIdWithTags;
		}
		ApiModeration.SendModeration(u.id, ApiModeration.ModerationType.Warn, "N/A", ApiModeration.ModerationTimeRange.None, worldId, worldInstanceId, null, null);
	}

	// Token: 0x06005210 RID: 21008 RVA: 0x001C198C File Offset: 0x001BFD8C
	public void UserWarnUser(APIUser warner, string role, APIUser u)
	{
		string worldId = string.Empty;
		string worldInstanceId = string.Empty;
		string text = string.Concat(new string[]
		{
			"The ",
			role,
			", ",
			warner.displayName,
			", has sent you a warning for bad behaviour. If this continues, you may be kicked or banned."
		});
		if (RoomManager.inRoom)
		{
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "WarnUserRPC", new object[]
			{
				u.id,
				text
			});
			worldId = RoomManager.currentRoom.id;
			worldInstanceId = RoomManager.currentRoom.currentInstanceIdWithTags;
		}
		ApiModeration.SendModeration(u.id, ApiModeration.ModerationType.Warn, "N/A", ApiModeration.ModerationTimeRange.None, worldId, worldInstanceId, null, null);
	}

	// Token: 0x06005211 RID: 21009 RVA: 0x001C1A30 File Offset: 0x001BFE30
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void WarnUserRPC(string userId, string message, VRC.Player instigator)
	{
		if (APIUser.CurrentUser.id != userId)
		{
			return;
		}
		if (!instigator.isModerator && !instigator.isRoomAuthor && !instigator.isInstanceOwner)
		{
			Debug.LogError("WarnUserRPC: received unauthorized mod command from " + ((instigator.user == null) ? "(null)" : (instigator.user.displayName + " [" + instigator.user.id + "]")));
			return;
		}
		VRCUiPopupManager.Instance.ShowAlert("Moderation Warning", message, 0f);
	}

	// Token: 0x06005212 RID: 21010 RVA: 0x001C1AD4 File Offset: 0x001BFED4
	public bool TryKickUser(APIUser u)
	{
		if (User.CurrentUser == null || u == null)
		{
			return false;
		}
		if (RoomManager.currentAuthorId == User.CurrentUser.id)
		{
			this.UserKickUser(User.CurrentUser, "world author", u);
			//return true;
		}
		if (RoomManager.currentOwnerId == User.CurrentUser.id)
		{
			this.UserKickUser(User.CurrentUser, "instance owner", u);
			//return true;
		}
		if (true)
		{
			this.ModKickUser(u);
			return true;
		}
		if (VRC.Network.IsMaster)
		{
			VRC.Network.CloseConnection(PlayerManager.GetPlayer(u.id));
			return true;
		}
		return false;
	}

	// Token: 0x06005213 RID: 21011 RVA: 0x001C1BA0 File Offset: 0x001BFFA0
	public void ModKickUser(APIUser u)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = "A moderator has kicked you out of the room for bad behaviour. If this continues, you may be banned.";
		if (true)
		{
			text = RoomManager.currentRoom.id;
			text2 = RoomManager.currentRoom.currentInstanceIdWithTags;
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "KickUserRPC", new object[]
			{
				u.id,
				text3,
				text,
				text2
			});
			VRC.Network.CloseConnection(PlayerManager.GetPlayer(u.id));
			this.kickedUsers.Add(u.id, PhotonNetwork.time);
			ApiModeration.SendModeration(u.id, ApiModeration.ModerationType.Kick, text3, ApiModeration.ModerationTimeRange.OneHour, text, text2, null, null);
		}
		else
		{
			Debug.LogError("ModKickUser: can't kick mod or trusted user: " + u.id);
		}
	}

	// Token: 0x06005214 RID: 21012 RVA: 0x001C1C84 File Offset: 0x001C0084
	public void UserKickUser(APIUser kicker, string role, APIUser u)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Concat(new string[]
		{
			"The ",
			role,
			", ",
			kicker.displayName,
			", has kicked you out of the room for bad behaviour. This will be reported to the moderators."
		});
		if (true)
		{
			text = RoomManager.currentRoom.id;
			text2 = RoomManager.currentRoom.currentInstanceIdWithTags;
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "KickUserRPC", new object[]
			{
				u.id,
				text3,
				text,
				text2
			});
			VRC.Network.CloseConnection(PlayerManager.GetPlayer(u.id));
			this.kickedUsers.Add(u.id, PhotonNetwork.time);
			ApiModeration.SendModeration(u.id, ApiModeration.ModerationType.Kick, text3, ApiModeration.ModerationTimeRange.OneHour, text, text2, null, null);
		}
		else
		{
			Debug.LogError("UserKickUser: can't kick mod or trusted user: " + u.id);
		}
	}

	// Token: 0x06005215 RID: 21013 RVA: 0x001C1D94 File Offset: 0x001C0194
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void KickUserRPC(string userId, string message, string worldId, string instanceId, VRC.Player instigator)
	{
		if (!instigator.isModerator && !instigator.isRoomAuthor && !instigator.isInstanceOwner)
		{
			Debug.LogError("KickUserRPC: received unauthorized mod command from " + ((instigator.user == null) ? "(null)" : (instigator.user.displayName + " [" + instigator.user.id + "]")));
			return;
		}
		VRC.Player player = PlayerManager.GetPlayer(userId);
		if (player != null && !player.isModerator && player.isLocal)
		{
			this.CreateGlobalTempModeration(userId, ApiModeration.ModerationType.Kick, ApiModeration.ModerationTimeRange.OneHour, worldId, instanceId, string.Empty);
			this.FetchModerations(null, null);
			this.kickMessage = message;
			this.showKickMessageInNextWorld = true;
			if (RoomManager.currentRoom != null)
			{
				if (VRCFlowManager.Instance.IsInDefaultWorld())
				{
					VRCFlowManager.Instance.EnterNewWorldInstance(VRCFlowManager.Instance.GetDefaultWorldId(), null, null);
				}
				else
				{
					VRCFlowManager.Instance.GoToDefaultWorld();
				}
			}
		}
	}

	// Token: 0x06005216 RID: 21014 RVA: 0x001C1EA4 File Offset: 0x001C02A4
	public void BanUser(APIUser u, ApiModeration.ModerationType banType)
	{
		if (banType != ApiModeration.ModerationType.Ban && banType != ApiModeration.ModerationType.BanPublicOnly)
		{
			Debug.LogError("BanUser: Invalid moderation type specified: " + banType);
			return;
		}
		ApiModeration.ModerationTimeRange moderationTimeRange = ApiModeration.ModerationTimeRange.OneDay;
		ApiModeration.SendModeration(u.id, banType, "You have violated VRChat's community guidelines.", moderationTimeRange, string.Empty, string.Empty, null, null);
		if (banType == ApiModeration.ModerationType.Ban)
		{
			if (RoomManager.inRoom)
			{
				VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "BanRPC", new object[]
				{
					u.id,
					(int)moderationTimeRange
				});
			}
		}
		else if (banType == ApiModeration.ModerationType.BanPublicOnly && RoomManager.inRoom)
		{
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "BanPublicOnlyRPC", new object[]
			{
				u.id,
				(int)moderationTimeRange
			});
		}
	}

	// Token: 0x06005217 RID: 21015 RVA: 0x001C1F6E File Offset: 0x001C036E
	public void ForceUserLogout(APIUser u)
	{
		if (RoomManager.inRoom)
		{
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "ForceLogoutRPC", new object[]
			{
				u.id
			});
		}
	}

	// Token: 0x06005218 RID: 21016 RVA: 0x001C1F9C File Offset: 0x001C039C
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void ForceLogoutRPC(string userId, VRC.Player instigator)
	{
		if (!instigator.isModerator)
		{
			Debug.LogError("ForceLogoutRPC: received unauthorized mod command from " + ((instigator.user == null) ? "(null)" : (instigator.user.displayName + " [" + instigator.user.id + "]")));
			return;
		}
		if (APIUser.CurrentUser.id == userId)
		{
			User.Logout();
			VRCFlowManager.Instance.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
			{
				VRCFlowManager.ResetGameFlags.ClearDestinationWorld
			});
		}
	}

	// Token: 0x06005219 RID: 21017 RVA: 0x001C202C File Offset: 0x001C042C
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void BanRPC(string userId, int banTime, VRC.Player instigator)
	{
		if (!instigator.isModerator)
		{
			Debug.LogError("BanRPC: received unauthorized mod command from " + ((instigator.user == null) ? "(null)" : (instigator.user.displayName + " [" + instigator.user.id + "]")));
			return;
		}
		if (APIUser.CurrentUser.id == userId)
		{
			this.CreateGlobalTempModeration(userId, ApiModeration.ModerationType.Ban, (ApiModeration.ModerationTimeRange)banTime, null, null, "You have violated VRChat's community guidelines.");
			this.EnforceBan();
		}
	}

	// Token: 0x0600521A RID: 21018 RVA: 0x001C20BA File Offset: 0x001C04BA
	private void EnforceBan()
	{
		VRCFlowManager.Instance.QueueAlertForNextReset("Moderation Ban", this.GetBannedUserMessage(), 0);
		User.Logout();
		VRCFlowManager.Instance.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
		{
			VRCFlowManager.ResetGameFlags.ShowUI,
			VRCFlowManager.ResetGameFlags.ClearDestinationWorld
		});
	}

	// Token: 0x0600521B RID: 21019 RVA: 0x001C20EC File Offset: 0x001C04EC
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void BanPublicOnlyRPC(string userId, int banTime, VRC.Player instigator)
	{
		if (!instigator.isModerator)
		{
			Debug.LogError("BanPublicOnlyRPC: received unauthorized mod command from " + ((instigator.user == null) ? "(null)" : (instigator.user.displayName + " [" + instigator.user.id + "]")));
			return;
		}
		if (APIUser.CurrentUser.id == userId)
		{
			this.CreateGlobalTempModeration(userId, ApiModeration.ModerationType.BanPublicOnly, (ApiModeration.ModerationTimeRange)banTime, null, null, "You have violated VRChat's community guidelines.");
			this.FetchModerations(null, null);
			if (!this.CheckAndEnforcePublicOnlyBan())
			{
				VRCUiPopupManager.Instance.ShowAlert("Moderation Ban", this.GetPublicOnlyBannedUserMessage(), 0f);
			}
		}
	}

	// Token: 0x0600521C RID: 21020 RVA: 0x001C21A4 File Offset: 0x001C05A4
	private bool CheckAndEnforcePublicOnlyBan()
	{
		this.wasLocalUserPublicOnlyBanned = false;
		return false;
	}

	// Token: 0x0600521D RID: 21021 RVA: 0x001C2217 File Offset: 0x001C0617
	public void ModForceOffMic(APIUser u)
	{
		if (RoomManager.inRoom)
		{
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "ModForceOffMicRPC", new object[]
			{
				u.id
			});
		}
	}

	// Token: 0x0600521E RID: 21022 RVA: 0x001C2244 File Offset: 0x001C0644
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void ModForceOffMicRPC(string userId, VRC.Player instigator)
	{
		if (!instigator.isModerator && !instigator.isRoomAuthor && !instigator.isInstanceOwner)
		{
			Debug.LogError("ModForceOffMicRPC: received unauthorized mod command from " + ((instigator.user == null) ? "(null)" : (instigator.user.displayName + " [" + instigator.user.id + "]")));
			return;
		}
		if (APIUser.CurrentUser.id == userId)
		{
			DefaultTalkController.Mute();
		}
	}

	// Token: 0x0600521F RID: 21023 RVA: 0x001C22D6 File Offset: 0x001C06D6
	public void ModMicGainUp(APIUser u)
	{
		if (RoomManager.inRoom)
		{
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "ModMicGainChangeRPC", new object[]
			{
				u.id,
				0.1f
			});
		}
	}

	// Token: 0x06005220 RID: 21024 RVA: 0x001C230F File Offset: 0x001C070F
	public void ModMicGainDown(APIUser u)
	{
		if (RoomManager.inRoom)
		{
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "ModMicGainChangeRPC", new object[]
			{
				u.id,
				-0.1f
			});
		}
	}

	// Token: 0x06005221 RID: 21025 RVA: 0x001C2348 File Offset: 0x001C0748
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	public void ModMicGainChangeRPC(string userId, float deltaGain, VRC.Player instigator)
	{
		if (!instigator.isModerator)
		{
			Debug.LogError("ModMicGainChangeRPC: received unauthorized mod command from " + ((instigator.user == null) ? "(null)" : (instigator.user.displayName + " [" + instigator.user.id + "]")));
			return;
		}
		if (APIUser.CurrentUser.id == userId && QuickMenu.Instance != null)
		{
			QuickMenu.Instance.VolumeChange(deltaGain);
		}
	}

	// Token: 0x06005222 RID: 21026 RVA: 0x001C23DC File Offset: 0x001C07DC
	public void AskModeratorWarnUI(APIUser user)
	{
		if (this.isModerator || this.isRoomAuthor || this.isInstanceOwner)
		{
			VRCUiPopupManager.Instance.ShowStandardPopup("Moderator Warn", "Are you sure you want to warn user: " + user.displayName + "?", "Yes", delegate
			{
				VRCUiPopupManager.Instance.HideCurrentPopup();
				if (this.isModerator)
				{
					Debug.Log("Mod warning user");
					this.ModWarnUser(user);
				}
				else if (this.isRoomAuthor)
				{
					Debug.Log("Author warning user");
					this.UserWarnUser(User.CurrentUser, "room author", user);
				}
				else if (this.isInstanceOwner)
				{
					Debug.Log("Owner warning user");
					this.UserWarnUser(User.CurrentUser, "instance owner", user);
				}
			}, "No", delegate
			{
				VRCUiPopupManager.Instance.HideCurrentPopup();
			}, null);
		}
		else
		{
			Debug.LogError("AskModeratorWarn: not authorized to mod");
		}
	}

	// Token: 0x06005223 RID: 21027 RVA: 0x001C248C File Offset: 0x001C088C
	public void AskModeratorTurnOffMicUI(APIUser user)
	{
		if (this.isModerator || this.isRoomAuthor || this.isInstanceOwner)
		{
			VRCUiPopupManager.Instance.ShowStandardPopup("Force Mic Off", "Are you sure you want to turn off mic for user: " + user.displayName + "?", "Yes", delegate
			{
				Debug.Log("Turning off mic for user");
				VRCUiPopupManager.Instance.HideCurrentPopup();
				this.ModForceOffMic(user);
			}, "No", delegate
			{
				VRCUiPopupManager.Instance.HideCurrentPopup();
			}, null);
		}
		else
		{
			Debug.LogError("AskModeratorTurnOffMic: not authorized to mod");
		}
	}

	// Token: 0x06005224 RID: 21028 RVA: 0x001C253C File Offset: 0x001C093C
	public void AskModeratorKickUI(APIUser user)
	{
		if (this.isModerator)
		{
			VRCUiPopupManager.Instance.ShowStandardPopup("Moderator Kick", "Are you sure you want to kick user: " + user.displayName + "?", "Yes", delegate
			{
				VRCUiPopupManager.Instance.HideCurrentPopup();
				Debug.Log("Mod kicking user");
				this.ModKickUser(user);
			}, "No", delegate
			{
				VRCUiPopupManager.Instance.HideCurrentPopup();
			}, null);
		}
		else if (this.isRoomAuthor)
		{
			VRCUiPopupManager.Instance.ShowStandardPopup("Moderator Kick", "Are you sure you want to kick user: " + user.displayName + "? They will not be able to join any instance of your world for one hour.", "Yes", delegate
			{
				VRCUiPopupManager.Instance.HideCurrentPopup();
				Debug.Log("Author kicking user");
				this.UserKickUser(User.CurrentUser, "room author", user);
			}, "No", delegate
			{
				VRCUiPopupManager.Instance.HideCurrentPopup();
			}, null);
		}
		else if (this.isInstanceOwner)
		{
			VRCUiPopupManager.Instance.ShowStandardPopup("Moderator Kick", "Are you sure you want to kick user: " + user.displayName + "? They will not be able to join this instance for one hour.", "Yes", delegate
			{
				VRCUiPopupManager.Instance.HideCurrentPopup();
				Debug.Log("Owner kicking user");
				this.UserKickUser(User.CurrentUser, "instance owner", user);
			}, "No", delegate
			{
				VRCUiPopupManager.Instance.HideCurrentPopup();
			}, null);
		}
		else
		{
			Debug.LogError("AskModeratorKick: not authorized to mod");
		}
	}

	// Token: 0x06005225 RID: 21029 RVA: 0x001C26B0 File Offset: 0x001C0AB0
	public void AskModeratorForceLogoutUI(APIUser user)
	{
		if (this.isModerator)
		{
			VRCUiPopupManager.Instance.ShowStandardPopup("Moderator Force Logout", "Are you sure you want to logout user: " + user.displayName + "?", "Yes", delegate
			{
				Debug.Log("Forcing user to logout");
				VRCUiPopupManager.Instance.HideCurrentPopup();
				this.ForceUserLogout(user);
			}, "No", delegate
			{
				VRCUiPopupManager.Instance.HideCurrentPopup();
			}, null);
		}
		else
		{
			Debug.LogError("AskModeratorForceLogout: not authorized to mod");
		}
	}

	// Token: 0x06005226 RID: 21030 RVA: 0x001C2748 File Offset: 0x001C0B48
	public void AskModeratorBanUI(APIUser user)
	{
		if (this.isModerator)
		{
			Action<ApiModeration.ModerationType> banUser = delegate(ApiModeration.ModerationType type)
			{
				VRCUiPopupManager.Instance.HideCurrentPopup();
				VRCUiPopupManager.Instance.ShowStandardPopup(((type != ApiModeration.ModerationType.BanPublicOnly) ? "Full" : "Public-Only") + " Ban", "Are you sure you want to ban user: " + user.displayName + "?", "Yes", delegate
				{
					Debug.Log("Banning user: type - " + type);
					VRCUiPopupManager.Instance.HideCurrentPopup();
					this.BanUser(user, type);
				}, "No", delegate
				{
					VRCUiPopupManager.Instance.HideCurrentPopup();
				}, null);
			};
			VRCUiPopupManager.Instance.ShowStandardPopup("Moderator Ban", "What type of ban do you want to apply to user: " + user.displayName + "?", "Public Only", delegate
			{
				banUser(ApiModeration.ModerationType.BanPublicOnly);
			}, "Full", delegate
			{
				banUser(ApiModeration.ModerationType.Ban);
			}, null);
		}
		else
		{
			Debug.LogError("AskModeratorBan: not authorized to mod");
		}
	}

	// Token: 0x06005227 RID: 21031 RVA: 0x001C27F0 File Offset: 0x001C0BF0
	private void SendVoteKickToApi(string userToKickId)
	{
		if (RoomManager.currentRoom != null)
		{
			string id = RoomManager.currentRoom.id;
			string currentInstanceIdWithTags = RoomManager.currentRoom.currentInstanceIdWithTags;
			ApiModeration.SendVoteKick(userToKickId, id, currentInstanceIdWithTags, null, null, null);
		}
	}

	// Token: 0x06005228 RID: 21032 RVA: 0x001C2828 File Offset: 0x001C0C28
	public void InitiateVoteToKick(APIUser userToKick)
	{
		if (true)
		{
			this.SendVoteKickToApi(userToKick.id);
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, ModerationManager.Instance.gameObject, "ReceiveVoteToKickInitiation", new object[]
			{
				userToKick.id
			});
		}
		else
		{
			VRCUiPopupManager.Instance.ShowAlert("Could not initiate Vote to Kick", "You have already initiated a vote to kick on this player. You cannot do so more than once", 10f);
		}
	}

	// Token: 0x06005229 RID: 21033 RVA: 0x001C289C File Offset: 0x001C0C9C
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.Others,
		VRC_EventHandler.VrcTargetType.All
	})]
	private void ReceiveVoteToKickInitiation(string userToKickId, VRC.Player instigator)
	{
		VRC.Player player = PlayerManager.GetPlayer(userToKickId);
		if (player != null && !player.isModerator)
		{
			this.userVoteKicks[userToKickId] = new Dictionary<string, bool>();
			VRC.Player[] allPlayers = PlayerManager.GetAllPlayers();
			foreach (VRC.Player player2 in allPlayers)
			{
				this.userVoteKicks[userToKickId][player2.userId] = false;
			}
			this.voteInfos[userToKickId] = new ModerationManager.VoteInfo();
			this.voteInfos[userToKickId].timer = this.voteKickTime;
			this.voteInfos[userToKickId].instigator = instigator;
			if (instigator.isLocal || player.isLocal)
			{
				this.userVoteKicks[userToKickId][instigator.userId] = true;
			}
			else
			{
				string message = instigator.name + " wants to kick " + player.name + ". Do you agree?";
				ApiNotification apiNotification = new ApiNotification();
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["userToKickId"] = userToKickId;
				dictionary["initiatorUserId"] = instigator.userId;
				apiNotification.LocalInit(userToKickId, player.name, ApiNotification.NotificationType.VoteToKick, message, dictionary);
				NotificationManager.Instance.AddNotification(apiNotification, NotificationManager.HistoryRange.Local);
			}
		}
	}

	// Token: 0x0600522A RID: 21034 RVA: 0x001C29EC File Offset: 0x001C0DEC
	public void SendVoteKick(string initiatorUserId, string userToKickId, bool shouldKick)
	{
		if (shouldKick)
		{
			this.SendVoteKickToApi(userToKickId);
		}
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "ReceiveVoteKick", new object[]
		{
			initiatorUserId,
			userToKickId,
			shouldKick
		});
	}

	// Token: 0x0600522B RID: 21035 RVA: 0x001C2A24 File Offset: 0x001C0E24
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void ReceiveVoteKick(string initiatorUserId, string userToKickId, bool shouldKick, VRC.Player instigator)
	{
		VRC.Player player = PlayerManager.GetPlayer(userToKickId);
		if (player != null && !player.isModerator)
		{
			this.userVoteKicks[userToKickId][instigator.userId] = shouldKick;
			this.CountAndHandleVotes(false);
		}
	}

	// Token: 0x0600522C RID: 21036 RVA: 0x001C2A70 File Offset: 0x001C0E70
	private void CountAndHandleVotes(bool isFinalCount = false)
	{
		List<string> list = new List<string>();
		if (this.userVoteKicks == null || this.userVoteKicks.Count<KeyValuePair<string, Dictionary<string, bool>>>() <= 0)
		{
			return;
		}
		foreach (KeyValuePair<string, Dictionary<string, bool>> keyValuePair in this.userVoteKicks)
		{
			string key = keyValuePair.Key;
			ModerationManager.VoteInfo voteInfo = new ModerationManager.VoteInfo();
			voteInfo.timer = 0f;
			if (this.voteInfos.TryGetValue(key, out voteInfo) && voteInfo.timer > 0f)
			{
				List<bool> source = this.userVoteKicks[key].ToDictionary((KeyValuePair<string, bool> k) => k.Key, (KeyValuePair<string, bool> k) => k.Value).Values.ToList<bool>();
				int num = source.Count((bool v) => v);
				VRC.Player player = PlayerManager.GetPlayer(key);
				if (player != null)
				{
					if (num > PlayerManager.GetAllPlayers().Length / 2)
					{
						if (voteInfo.instigator.userId != player.userId && voteInfo.instigator.isLocal)
						{
							VRCUiPopupManager.Instance.ShowAlert("Vote To Kick Successful", "More than half the users in the world agreed to kick " + player.name + ". Kicking now.", 10f);
						}
						string text = "The users of the last world voted to kick you for bad behaviour. If this continues, you may banned.";
						VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "KickUserRPC", new object[]
						{
							player.userId,
							text
						});
						list.Add(key);
						this.voteInfos.Remove(key);
						if (!player.isLocal)
						{
							VRC.Network.CloseConnection(player);
						}
					}
					else if (isFinalCount && voteInfo.instigator.userId != player.userId && voteInfo.instigator.isLocal)
					{
						VRCUiPopupManager.Instance.ShowAlert("Vote To Kick Failed", "Less than half the users in the world agreed to kick " + player.name + ". Not kicking.", 10f);
					}
				}
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			this.userVoteKicks.Remove(list[i]);
		}
	}

	// Token: 0x0600522D RID: 21037 RVA: 0x001C2D14 File Offset: 0x001C1114
	private ApiPlayerModeration CreateTempModeration(string moderator, string target, ApiPlayerModeration.ModerationType type)
	{
		ApiPlayerModeration apiPlayerModeration = new ApiPlayerModeration();
		apiPlayerModeration.moderationType = type;
		apiPlayerModeration.moderatorUserId = moderator;
		apiPlayerModeration.targetUserId = target;
		if (moderator != null)
		{
			this.playerModerationsAgainstMe.Add(apiPlayerModeration);
		}
		if (target != null)
		{
			this.playerModerationsMine.Add(apiPlayerModeration);
		}
		return apiPlayerModeration;
	}

	// Token: 0x0600522E RID: 21038 RVA: 0x001C2D64 File Offset: 0x001C1164
	private ApiModeration CreateGlobalTempModeration(string target, ApiModeration.ModerationType type, ApiModeration.ModerationTimeRange timeRange, string worldId = "", string instanceId = "", string reason = "")
	{
		ApiModeration apiModeration = new ApiModeration();
		apiModeration.moderationType = type;
		apiModeration.targetUserId = target;
		apiModeration.expires = this.GetLocalModerationExpiresTimeFromRange(timeRange);
		if (!string.IsNullOrEmpty(worldId))
		{
			apiModeration.worldId = worldId;
		}
		if (!string.IsNullOrEmpty(instanceId))
		{
			apiModeration.instanceId = instanceId;
		}
		if (!string.IsNullOrEmpty(reason))
		{
			apiModeration.reason = reason;
		}
		this.globalModerations.Add(apiModeration);
		return apiModeration;
	}

	// Token: 0x0600522F RID: 21039 RVA: 0x001C2DDC File Offset: 0x001C11DC
	private DateTime GetLocalModerationExpiresTimeFromRange(ApiModeration.ModerationTimeRange timeRange)
	{
		DateTime networkDateTime = VRC.Network.GetNetworkDateTime();
		switch (timeRange)
		{
		case ApiModeration.ModerationTimeRange.None:
			return networkDateTime;
		case ApiModeration.ModerationTimeRange.OneHour:
			return networkDateTime.AddHours(1.0);
		case ApiModeration.ModerationTimeRange.OneDay:
			return networkDateTime.AddDays(1.0);
		default:
			Debug.LogError("Unknown time range: " + timeRange);
			return networkDateTime;
		}
	}

	// Token: 0x06005230 RID: 21040 RVA: 0x001C2E40 File Offset: 0x001C1240
	public bool IsBlockedByUser(string userId)
	{
		return this.playerModerationsAgainstMe.Exists((ApiPlayerModeration m) => m.moderationType == ApiPlayerModeration.ModerationType.Block && m.moderatorUserId == userId);
	}

	// Token: 0x06005231 RID: 21041 RVA: 0x001C2E74 File Offset: 0x001C1274
	public bool IsBlocked(string userId)
	{
		return this.playerModerationsMine.Exists((ApiPlayerModeration m) => m.moderationType == ApiPlayerModeration.ModerationType.Block && m.targetUserId == userId);
	}

	// Token: 0x06005232 RID: 21042 RVA: 0x001C2EA5 File Offset: 0x001C12A5
	public bool IsBlockedEitherWay(string userId)
	{
		return this.IsBlocked(userId) || this.IsBlockedByUser(userId);
	}

	// Token: 0x06005233 RID: 21043 RVA: 0x001C2EC0 File Offset: 0x001C12C0
	public void BlockUser(string userId)
	{
		VRC.Player player = PlayerManager.GetPlayer(userId);
		if (player == null || player.isLocal || player.isModerator)
		{
			return;
		}
		ApiPlayerModeration apm = this.CreateTempModeration(null, userId, ApiPlayerModeration.ModerationType.Block);
		ApiPlayerModeration.SendModeration(userId, ApiPlayerModeration.ModerationType.Block, delegate(ApiPlayerModeration mod)
		{
			apm.Init(mod);
		}, null);
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "BlockStateChangeRPC", new object[]
		{
			userId,
			true
		});
		if (this.moderationsChanged != null)
		{
			this.moderationsChanged();
		}
	}

	// Token: 0x06005234 RID: 21044 RVA: 0x001C2F5C File Offset: 0x001C135C
	public void UnblockUser(string userId)
	{
		ApiPlayerModeration apiPlayerModeration = this.playerModerationsMine.FirstOrDefault((ApiPlayerModeration m) => m.moderationType == ApiPlayerModeration.ModerationType.Block && m.targetUserId == userId);
		if (apiPlayerModeration == null)
		{
			return;
		}
		this.playerModerationsMine.Remove(apiPlayerModeration);
		ApiPlayerModeration.DeleteModeration(apiPlayerModeration.id, null, null);
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "BlockStateChangeRPC", new object[]
		{
			userId,
			false
		});
		if (this.moderationsChanged != null)
		{
			this.moderationsChanged();
		}
	}

	// Token: 0x06005235 RID: 21045 RVA: 0x001C2FF0 File Offset: 0x001C13F0
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void BlockStateChangeRPC(string targetId, bool blocked, VRC.Player instigator)
	{
		if (User.CurrentUser.id != targetId || VRC.Player.Instance == null)
		{
			return;
		}
		if (VRC.Player.Instance.isModerator)
		{
			Debug.LogError("Local player moderator received block request from " + instigator.userId + ", shouldn't be possible");
			return;
		}
		if (blocked)
		{
			this.NotifyOfBlockByUser(instigator.userId);
		}
		else
		{
			this.NotifyOfUnblockByUser(instigator.userId);
		}
	}

	// Token: 0x06005236 RID: 21046 RVA: 0x001C3070 File Offset: 0x001C1470
	public void NotifyOfBlockByUser(string userId)
	{
		this.CreateTempModeration(userId, null, ApiPlayerModeration.ModerationType.Block);
		if (this.moderationsChanged != null)
		{
			this.moderationsChanged();
		}
	}

	// Token: 0x06005237 RID: 21047 RVA: 0x001C3094 File Offset: 0x001C1494
	public void NotifyOfUnblockByUser(string userId)
	{
		foreach (ApiPlayerModeration apiPlayerModeration in this.playerModerationsAgainstMe)
		{
			if (apiPlayerModeration.moderationType == ApiPlayerModeration.ModerationType.Block && apiPlayerModeration.moderatorUserId == userId)
			{
				this.playerModerationsAgainstMe.Remove(apiPlayerModeration);
				if (this.moderationsChanged != null)
				{
					this.moderationsChanged();
				}
				break;
			}
		}
	}

	// Token: 0x06005238 RID: 21048 RVA: 0x001C3130 File Offset: 0x001C1530
	public void NotifyFriendStateChanged(string userId, bool isFriend)
	{
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "FriendStateChangeRPC", new object[]
		{
			userId,
			isFriend
		});
	}

	// Token: 0x06005239 RID: 21049 RVA: 0x001C3156 File Offset: 0x001C1556
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void FriendStateChangeRPC(string targetId, bool isFriend, VRC.Player instigator)
	{
		if (User.CurrentUser.id != targetId)
		{
			return;
		}
		if (isFriend)
		{
			APIUser.LocalAddFriend(instigator.user);
		}
		else
		{
			APIUser.LocalRemoveFriend(instigator.user);
		}
	}

	// Token: 0x0600523A RID: 21050 RVA: 0x001C3190 File Offset: 0x001C1590
	public bool IsMutedByUser(string userId)
	{
		if (userId == APIUser.CurrentUser.id)
		{
			return false;
		}
		bool flag = this.playerModerationsAgainstMe.Exists((ApiPlayerModeration m) => m.moderationType == ApiPlayerModeration.ModerationType.Mute && m.moderatorUserId == userId);
		if (flag)
		{
			return true;
		}
		bool flag2 = this.playerModerationsAgainstMe.Exists((ApiPlayerModeration m) => m.moderationType == ApiPlayerModeration.ModerationType.Unmute && m.moderatorUserId == userId);
		if (flag2)
		{
			return false;
		}
		APIUser cachedUser = APIUser.GetCachedUser(userId);
		return cachedUser == null || cachedUser.defaultMute;
	}

	// Token: 0x0600523B RID: 21051 RVA: 0x001C3220 File Offset: 0x001C1620
	public bool IsMuted(string userId)
	{
		if (userId == APIUser.CurrentUser.id)
		{
			return false;
		}
		bool flag = this.playerModerationsMine.Exists((ApiPlayerModeration m) => m.moderationType == ApiPlayerModeration.ModerationType.Mute && m.targetUserId == userId);
		if (flag)
		{
			return true;
		}
		bool flag2 = this.playerModerationsMine.Exists((ApiPlayerModeration m) => m.moderationType == ApiPlayerModeration.ModerationType.Unmute && m.targetUserId == userId);
		return !flag2 && VRCInputManager.GetSetting(VRCInputManager.InputSetting.DefaultMute);
	}

	// Token: 0x0600523C RID: 21052 RVA: 0x001C32A0 File Offset: 0x001C16A0
	public void MuteUser(string userId)
	{
		if (userId == APIUser.CurrentUser.id)
		{
			Debug.LogError("MuteUser: cannot block self");
			return;
		}
		APIUser cachedUser = APIUser.GetCachedUser(userId);
		if (cachedUser != null && cachedUser.developerType != null && cachedUser.developerType.Value == APIUser.DeveloperType.Internal)
		{
			Debug.LogError("MuteUser: Cannot mute moderator " + userId);
			return;
		}
		ApiPlayerModeration apiPlayerModeration = this.playerModerationsMine.FirstOrDefault((ApiPlayerModeration m) => m.moderationType == ApiPlayerModeration.ModerationType.Unmute && m.targetUserId == userId);
		if (apiPlayerModeration != null)
		{
			ApiPlayerModeration.DeleteModeration(apiPlayerModeration.id, null, null);
			this.playerModerationsMine.Remove(apiPlayerModeration);
		}
		if (this.playerModerationsMine.FirstOrDefault((ApiPlayerModeration m) => m.moderationType == ApiPlayerModeration.ModerationType.Mute && m.targetUserId == userId) == null)
		{
			ApiPlayerModeration apm = this.CreateTempModeration(null, userId, ApiPlayerModeration.ModerationType.Mute);
			ApiPlayerModeration.SendModeration(userId, ApiPlayerModeration.ModerationType.Mute, delegate(ApiPlayerModeration mod)
			{
				apm.Init(mod);
			}, null);
		}
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "MuteChangeRPC", new object[]
		{
			userId,
			true
		});
		if (this.moderationsChanged != null)
		{
			this.moderationsChanged();
		}
	}

	// Token: 0x0600523D RID: 21053 RVA: 0x001C3400 File Offset: 0x001C1800
	public void UnmuteUser(string userId)
	{
		if (this.playerModerationsMine.FirstOrDefault((ApiPlayerModeration m) => m.moderationType == ApiPlayerModeration.ModerationType.Unmute && m.targetUserId == userId) == null)
		{
			ApiPlayerModeration apm = this.CreateTempModeration(null, userId, ApiPlayerModeration.ModerationType.Unmute);
			ApiPlayerModeration.SendModeration(userId, ApiPlayerModeration.ModerationType.Unmute, delegate(ApiPlayerModeration mod)
			{
				apm.Init(mod);
			}, null);
		}
		ApiPlayerModeration apiPlayerModeration = this.playerModerationsMine.FirstOrDefault((ApiPlayerModeration m) => m.moderationType == ApiPlayerModeration.ModerationType.Mute && m.targetUserId == userId);
		if (apiPlayerModeration != null)
		{
			ApiPlayerModeration.DeleteModeration(apiPlayerModeration.id, null, null);
			this.playerModerationsMine.Remove(apiPlayerModeration);
		}
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "MuteChangeRPC", new object[]
		{
			userId,
			false
		});
		if (this.moderationsChanged != null)
		{
			this.moderationsChanged();
		}
	}

	// Token: 0x0600523E RID: 21054 RVA: 0x001C34E4 File Offset: 0x001C18E4
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void MuteChangeRPC(string targetId, bool muted, VRC.Player instigator)
	{
		if (User.CurrentUser.id != targetId)
		{
			return;
		}
		if (muted && VRC.Player.Instance != null && VRC.Player.Instance.isInternal)
		{
			Debug.LogError("MuteChangeRPC: cannot mute moderator, " + instigator.userId + " (tried to mute) " + targetId);
			return;
		}
		ApiPlayerModeration apiPlayerModeration = this.playerModerationsAgainstMe.FirstOrDefault((ApiPlayerModeration m) => m.moderationType == ApiPlayerModeration.ModerationType.Mute && m.moderatorUserId == instigator.userId);
		ApiPlayerModeration apiPlayerModeration2 = this.playerModerationsAgainstMe.FirstOrDefault((ApiPlayerModeration m) => m.moderationType == ApiPlayerModeration.ModerationType.Unmute && m.moderatorUserId == instigator.userId);
		if (muted)
		{
			if (apiPlayerModeration2 != null)
			{
				this.playerModerationsAgainstMe.Remove(apiPlayerModeration2);
			}
			if (apiPlayerModeration == null)
			{
				this.CreateTempModeration(instigator.userId, null, ApiPlayerModeration.ModerationType.Mute);
			}
		}
		else
		{
			if (apiPlayerModeration2 == null)
			{
				this.CreateTempModeration(instigator.userId, null, ApiPlayerModeration.ModerationType.Unmute);
			}
			if (apiPlayerModeration != null)
			{
				this.playerModerationsAgainstMe.Remove(apiPlayerModeration);
			}
		}
		if (this.moderationsChanged != null)
		{
			this.moderationsChanged();
		}
	}

	// Token: 0x0600523F RID: 21055 RVA: 0x001C3608 File Offset: 0x001C1A08
	public bool IsBanned(string userId)
	{
		return false;
	}

	// Token: 0x06005240 RID: 21056 RVA: 0x001C363C File Offset: 0x001C1A3C
	public bool IsBannedFromPublicOnly(string userId)
	{
		return false;
	}

	// Token: 0x06005241 RID: 21057 RVA: 0x001C36AD File Offset: 0x001C1AAD
	public bool IsPublicOnlyBannedFromCurrentWorld(string userId)
	{
		return false;
	}

	// Token: 0x06005242 RID: 21058 RVA: 0x001C36EA File Offset: 0x001C1AEA
	public bool IsPublicOnlyBannedFromWorld(string userId, string worldId, string instanceId)
	{
		return false;
	}

	// Token: 0x06005243 RID: 21059 RVA: 0x001C3720 File Offset: 0x001C1B20
	public string GetPublicOnlyBannedUserMessage()
	{
		DateTime dateTime = DateTime.UtcNow;
		string text = string.Empty;
		ApiModeration moderationOfType = this.GetModerationOfType(ApiModeration.ModerationType.BanPublicOnly);
		if (moderationOfType != null)
		{
			dateTime = moderationOfType.expires.ToLocalTime();
			text = moderationOfType.reason;
		}
		return string.Concat(new string[]
		{
			"You have been banned from public world instances until ",
			dateTime.ToString("MMM dd, yyyy HH:mm"),
			". ",
			string.IsNullOrEmpty(text) ? string.Empty : ("Reason: " + text),
			"\nCommunity Guidelines: www.vrchat.com/community"
		});
	}

	// Token: 0x06005244 RID: 21060 RVA: 0x001C37B0 File Offset: 0x001C1BB0
	public string GetBannedUserMessage()
	{
		DateTime dateTime = DateTime.UtcNow;
		string text = string.Empty;
		ApiModeration moderationOfType = this.GetModerationOfType(ApiModeration.ModerationType.Ban);
		if (moderationOfType != null)
		{
			dateTime = moderationOfType.expires.ToLocalTime();
			text = moderationOfType.reason;
		}
		return string.Concat(new string[]
		{
			"You have been banned until ",
			dateTime.ToString("MMM dd, yyyy HH:mm"),
			". ",
			string.IsNullOrEmpty(text) ? string.Empty : ("Reason: " + text),
			"\nCommunity Guidelines: www.vrchat.com/community"
		});
	}

	// Token: 0x06005245 RID: 21061 RVA: 0x001C3840 File Offset: 0x001C1C40
	public bool IsKicked(string userId)
	{
		return (this.kickedUsers.ContainsKey(userId) && PhotonNetwork.time - this.kickedUsers[userId] < this.kickTime) || (RoomManager.currentRoom != null && this.IsKickedFromWorld(userId, RoomManager.currentRoom.id, RoomManager.currentRoom.currentInstanceIdWithTags));
	}

	// Token: 0x06005246 RID: 21062 RVA: 0x001C38A8 File Offset: 0x001C1CA8
	public bool IsKickedFromWorld(string userId, string worldId, string instanceId)
	{
		return this.globalModerations.Exists((ApiModeration m) => m.moderationType == ApiModeration.ModerationType.Kick && m.worldId == worldId && m.instanceId == instanceId && m.targetUserId == userId);
	}

	// Token: 0x06005247 RID: 21063 RVA: 0x001C38E8 File Offset: 0x001C1CE8
	public ApiModeration GetModerationOfType(ApiModeration.ModerationType t)
	{
		return this.globalModerations.FirstOrDefault((ApiModeration m) => m.moderationType == t);
	}

	// Token: 0x06005248 RID: 21064 RVA: 0x001C391C File Offset: 0x001C1D1C
	public List<ApiModeration> GetModerationsOfType(ApiModeration.ModerationType t)
	{
		if (this.globalModerations == null)
		{
			Debug.LogError("early");
			this.globalModerations = new List<ApiModeration>();
		}
		return (from m in this.globalModerations
		where m.moderationType == t
		select m).ToList<ApiModeration>();
	}

	// Token: 0x06005249 RID: 21065 RVA: 0x001C3974 File Offset: 0x001C1D74
	private void Awake()
	{
		if (ModerationManager.instance == null)
		{
			ModerationManager.instance = this;
			UnityEngine.Object.DontDestroyOnLoad(this);
		}
		else
		{
			Debug.LogError("Too many instances of NotificationManage. There can only be 1.");
		}
		this.timer = this.moderationFetchPeriod;
		this.userVoteKicks = new Dictionary<string, Dictionary<string, bool>>();
		this.voteInfos = new Dictionary<string, ModerationManager.VoteInfo>();
	}

	// Token: 0x0600524A RID: 21066 RVA: 0x001C39CE File Offset: 0x001C1DCE
	private void Start()
	{
		VRCFlowManager.Instance.onEnteredWorld += this.OnEnterWorld;
	}

	// Token: 0x0600524B RID: 21067 RVA: 0x001C39E6 File Offset: 0x001C1DE6
	private void OnEnable()
	{
	}

	// Token: 0x0600524C RID: 21068 RVA: 0x001C39E8 File Offset: 0x001C1DE8
	private void OnDisable()
	{
	}

	// Token: 0x0600524D RID: 21069 RVA: 0x001C39EC File Offset: 0x001C1DEC
	private void Update()
	{
		this.timer -= Time.deltaTime;
		if (this.timer <= 0f)
		{
			this.FetchModerations(new Action(this.HandleModeration), null);
			this.timer = this.moderationFetchPeriod;
		}
		Dictionary<string, ModerationManager.VoteInfo> dictionary = new Dictionary<string, ModerationManager.VoteInfo>(this.voteInfos);
		foreach (KeyValuePair<string, ModerationManager.VoteInfo> keyValuePair in dictionary)
		{
			if (keyValuePair.Value.timer > 0f)
			{
				this.voteInfos[keyValuePair.Key].timer -= Time.deltaTime;
			}
			else
			{
				this.CountAndHandleVotes(true);
				this.voteInfos.Remove(keyValuePair.Key);
			}
		}
	}

	// Token: 0x0600524E RID: 21070 RVA: 0x001C3AE4 File Offset: 0x001C1EE4
	private void HandleModeration()
	{
		this.SelfCheckAndEnforceModerations();
	}

	// Token: 0x0600524F RID: 21071 RVA: 0x001C3AF0 File Offset: 0x001C1EF0
	private void OnEnterWorld()
	{
		if (this.showKickMessageInNextWorld)
		{
			VRCUiPopupManager.Instance.ShowAlert("Moderation Kick", this.kickMessage, 0f);
			this.showKickMessageInNextWorld = false;
			this.FetchModerations(null, null);
		}
		if (VRCFlowManager.Instance.IsInPublicBanWorld() && this.showPublicOnlyBanMessageInNextBanWorld)
		{
			VRCUiPopupManager.Instance.ShowAlert("Moderation Ban", this.GetPublicOnlyBannedUserMessage(), 0f);
			this.showPublicOnlyBanMessageInNextBanWorld = false;
			this.FetchModerations(null, null);
		}
		this.userVoteKicks = new Dictionary<string, Dictionary<string, bool>>();
		this.voteInfos = new Dictionary<string, ModerationManager.VoteInfo>();
		this.kickedUsers = new Dictionary<string, double>();
	}

	// Token: 0x06005250 RID: 21072 RVA: 0x001C3B98 File Offset: 0x001C1F98
	private void DebugPrintModerations()
	{
		if (this.globalModerations != null)
		{
			Debug.LogError("** Global moderations");
			foreach (ApiModeration apiModeration in this.globalModerations)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"----- ",
					apiModeration.moderatorDisplayName,
					" (",
					apiModeration.moderatorUserId,
					") ",
					apiModeration.moderationType,
					" => ",
					apiModeration.targetDisplayName,
					" (",
					apiModeration.targetUserId,
					") "
				}));
			}
		}
		else
		{
			Debug.LogError("*** Global moderations NULL");
		}
		if (this.playerModerationsMine != null)
		{
			Debug.LogError("** Player mods mine");
			foreach (ApiPlayerModeration apiPlayerModeration in this.playerModerationsMine)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"----- ",
					apiPlayerModeration.moderatorDisplayName,
					" (",
					apiPlayerModeration.moderatorUserId,
					") ",
					apiPlayerModeration.moderationType,
					" => ",
					apiPlayerModeration.targetDisplayName,
					" (",
					apiPlayerModeration.targetUserId,
					") "
				}));
			}
		}
		else
		{
			Debug.LogError("*** Player mods mine NULL");
		}
		if (this.playerModerationsAgainstMe != null)
		{
			Debug.LogError("** Player mods against me");
			foreach (ApiPlayerModeration apiPlayerModeration2 in this.playerModerationsAgainstMe)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"----- ",
					apiPlayerModeration2.moderatorDisplayName,
					" (",
					apiPlayerModeration2.moderatorUserId,
					") ",
					apiPlayerModeration2.moderationType,
					" => ",
					apiPlayerModeration2.targetDisplayName,
					" (",
					apiPlayerModeration2.targetUserId,
					") "
				}));
			}
		}
		else
		{
			Debug.LogError("*** Player mods against me NULL");
		}
	}

	// Token: 0x06005251 RID: 21073 RVA: 0x001C3E48 File Offset: 0x001C2248
	public void Updated()
	{
		if (this.moderationsChanged != null)
		{
			this.moderationsChanged();
		}
	}

	// Token: 0x04003A21 RID: 14881
	private static ModerationManager instance;

	// Token: 0x04003A22 RID: 14882
	private List<ApiModeration> globalModerations;

	// Token: 0x04003A23 RID: 14883
	private List<ApiPlayerModeration> playerModerationsMine;

	// Token: 0x04003A24 RID: 14884
	private List<ApiPlayerModeration> playerModerationsAgainstMe;

	// Token: 0x04003A25 RID: 14885
	public float moderationFetchPeriod = 240f;

	// Token: 0x04003A26 RID: 14886
	private float timer;

	// Token: 0x04003A27 RID: 14887
	private bool showKickMessageInNextWorld;

	// Token: 0x04003A28 RID: 14888
	private string kickMessage;

	// Token: 0x04003A29 RID: 14889
	private bool wasLocalUserPublicOnlyBanned;

	// Token: 0x04003A2A RID: 14890
	private bool showPublicOnlyBanMessageInNextBanWorld;

	// Token: 0x04003A2C RID: 14892
	private int awaitingResults;

	// Token: 0x04003A2D RID: 14893
	private Dictionary<string, Dictionary<string, bool>> userVoteKicks;

	// Token: 0x04003A2E RID: 14894
	private float voteKickTime = 120f;

	// Token: 0x04003A2F RID: 14895
	private double kickTime = 3600000.0;

	// Token: 0x04003A30 RID: 14896
	private Dictionary<string, ModerationManager.VoteInfo> voteInfos;

	// Token: 0x04003A31 RID: 14897
	private Dictionary<string, double> kickedUsers = new Dictionary<string, double>();

	// Token: 0x02000AA8 RID: 2728
	// (Invoke) Token: 0x0600525C RID: 21084
	public delegate void ModerationChangeEvent();

	// Token: 0x02000AA9 RID: 2729
	public class VoteInfo
	{
		// Token: 0x04003A3B RID: 14907
		public float timer;

		// Token: 0x04003A3C RID: 14908
		public VRC.Player instigator;
	}
}
