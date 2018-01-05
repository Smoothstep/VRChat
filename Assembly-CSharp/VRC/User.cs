using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Steamworks;
using UnityEngine;
using VRC.Core;

namespace VRC
{
	// Token: 0x02000B2C RID: 2860
	public class User : APIUser
	{
		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x06005721 RID: 22305 RVA: 0x001E04C6 File Offset: 0x001DE8C6
		public ApiAvatar apiAvatar
		{
			get
			{
				return this.mApiAvatar;
			}
		}

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x06005722 RID: 22306 RVA: 0x001E04CE File Offset: 0x001DE8CE
		public new static User CurrentUser
		{
			get
			{
				if (User.mCurrentNonApiUser == null && APIUser.CurrentUser != null)
				{
					User.mCurrentNonApiUser = new User();
					User.mCurrentNonApiUser.Init(APIUser.CurrentUser);
				}
				return User.mCurrentNonApiUser;
			}
		}

		// Token: 0x06005723 RID: 22307 RVA: 0x001E0504 File Offset: 0x001DE904
		public new static void Register(string username, string email, string password, string birthday, Action<APIUser> successCallback = null, Action<string> errorCallback = null)
		{
			APIUser.Register(username, email, password, birthday, delegate(APIUser apiUser)
			{
				ApiCredentials.SetUser(username, password);
				User.SetNetworkProperties();
				if (successCallback != null)
				{
					successCallback(apiUser);
				}
			}, errorCallback);
		}

		// Token: 0x06005724 RID: 22308 RVA: 0x001E0550 File Offset: 0x001DE950
		public new static void UpdateAccountInfo(string id, string email, string birthday, string acceptedTOSVersion, Action<APIUser> successCallback = null, Action<string> errorCallback = null)
		{
			APIUser.UpdateAccountInfo(id, email, birthday, acceptedTOSVersion, delegate(APIUser apiUser)
			{
				User.mCurrentNonApiUser = new User();
				User.mCurrentNonApiUser.Init(apiUser);
				User.SetNetworkProperties();
				if (successCallback != null)
				{
					successCallback(apiUser);
				}
			}, errorCallback);
		}

		// Token: 0x06005725 RID: 22309 RVA: 0x001E0584 File Offset: 0x001DE984
		public static void Login(string usernameOrEmail, string password, Action<APIUser> successCallback = null, Action<string> errorCallback = null)
		{
			ApiCredentials.SetUser(usernameOrEmail, password);
			APIUser.Login(delegate(APIUser user)
			{
				User.mCurrentNonApiUser = new User();
				User.mCurrentNonApiUser.Init(user);
				User.SetNetworkProperties();
				if (successCallback != null)
				{
					successCallback(user);
				}
			}, errorCallback);
		}

		// Token: 0x06005726 RID: 22310 RVA: 0x001E05B8 File Offset: 0x001DE9B8
		public static void SteamLogin(string authSessionTicket, Action<APIUser> onFetchSuccess = null, Action<string> onFetchError = null)
		{
			ApiCredentials.Clear();
			APIUser.ThirdPartyLogin("steam", new Dictionary<string, string>
			{
				{
					"steamTicket",
					authSessionTicket
				}
			}, delegate(string authToken, APIUser freshAPIUser)
			{
				ApiCredentials.SetAuthToken(authToken, "steam", SteamUser.GetSteamID().ToString());
				User.mCurrentNonApiUser = new User();
				User.mCurrentNonApiUser.Init(APIUser.CurrentUser);
				User.SetNetworkProperties();
				if (onFetchSuccess != null)
				{
					onFetchSuccess(freshAPIUser);
				}
			}, onFetchError);
		}

		// Token: 0x06005727 RID: 22311 RVA: 0x001E0604 File Offset: 0x001DEA04
		public new static void Login(Action<APIUser> onFetchSuccess = null, Action<string> onFetchError = null)
		{
			ApiCredentials.Load();
			APIUser.Login(delegate(APIUser freshAPIUser)
			{
				User.mCurrentNonApiUser = new User();
				User.mCurrentNonApiUser.Init(APIUser.CurrentUser);
				User.SetNetworkProperties();
				if (onFetchSuccess != null)
				{
					onFetchSuccess(freshAPIUser);
				}
			}, onFetchError);
		}

		// Token: 0x06005728 RID: 22312 RVA: 0x001E0636 File Offset: 0x001DEA36
		public new static void Logout()
		{
			if (APIUser.CurrentUser != null)
			{
				Analytics.Send(ApiAnalyticEvent.EventType.logout);
			}
			User.mCurrentNonApiUser = null;
			ApiCredentials.Clear();
			APIUser.Logout();
		}

		// Token: 0x06005729 RID: 22313 RVA: 0x001E0658 File Offset: 0x001DEA58
		public void Init(APIUser apiUser)
		{
			base.Fill(apiUser);
			this.FetchAvatar();
		}

		// Token: 0x0600572A RID: 22314 RVA: 0x001E0667 File Offset: 0x001DEA67
		protected override void Fill(Dictionary<string, object> jsonObject)
		{
			base.Fill(jsonObject);
			this.FetchAvatar();
		}

		// Token: 0x0600572B RID: 22315 RVA: 0x001E0676 File Offset: 0x001DEA76
		protected override void Fill(APIUser fromUser)
		{
			base.Fill(fromUser);
			this.FetchAvatar();
		}

		// Token: 0x0600572C RID: 22316 RVA: 0x001E0688 File Offset: 0x001DEA88
		private void FetchAvatar()
		{
			this.mApiAvatar = new ApiAvatar();
			this.mApiAvatar.Init();
			this.mApiAvatar.id = "avtr_no-id";
			ApiAvatar.Fetch(this.mAvatarId, new Action<ApiAvatar>(this.AvatarRecordReceived), new Action<string>(this.AvatarRecordFetchError));
		}

		// Token: 0x0600572D RID: 22317 RVA: 0x001E06DE File Offset: 0x001DEADE
		private void AvatarRecordReceived(ApiAvatar avatar)
		{
			this.mApiAvatar = avatar;
			User.SetNetworkProperties();
		}

		// Token: 0x0600572E RID: 22318 RVA: 0x001E06EC File Offset: 0x001DEAEC
		private void AvatarRecordFetchError(string error)
		{
			Debug.LogError("ApiAvatar: Couldn't fetch user's avatar, switching to default. Error: " + error);
			ApiAvatar.Fetch("avtr_53856003-8ff2-4002-b78f-da5d028b22bd", delegate(ApiAvatar avatar)
			{
				this.SetCurrentAvatar(avatar);
			}, delegate(string errorStr)
			{
				Debug.LogError("ApiAvatar: Couldn't set default avatar: " + errorStr);
				User.SetNetworkProperties();
			});
		}

		// Token: 0x0600572F RID: 22319 RVA: 0x001E073C File Offset: 0x001DEB3C
		public override void SetCurrentAvatar(ApiAvatar avatar)
		{
			this.mApiAvatar = avatar;
			this.mAvatarId = avatar.id;
			avatar.AssignToThisUser();
			User.SetNetworkProperties();
		}

		// Token: 0x06005730 RID: 22320 RVA: 0x001E075C File Offset: 0x001DEB5C
		public override void SetDisplayName(string name)
		{
			this.mDisplayName = name;
		}

		// Token: 0x06005731 RID: 22321 RVA: 0x001E0768 File Offset: 0x001DEB68
		public static void SetNetworkProperties()
		{
			PhotonNetwork.player.NickName = User.CurrentUser.displayName;
			Hashtable hashtable = new Hashtable();
			hashtable.Add("modTag", VRCPlayer.LocalModTag);
			hashtable.Add("isInvisible", VRCPlayer.LocalIsInvisible);
			hashtable.Add("avatarBlueprint", User.CurrentUser.mApiAvatar);
			hashtable.Add("userId", User.CurrentUser.id);
			hashtable.Add("defaultMute", VRCInputManager.GetSetting(VRCInputManager.InputSetting.DefaultMute));
			hashtable.Add("inVRMode", VRCTrackingManager.IsInVRMode());
			PhotonNetwork.player.SetCustomProperties(hashtable, null, false);
		}

		// Token: 0x04003E1E RID: 15902
		private const string kDefaultAvatarId = "avtr_53856003-8ff2-4002-b78f-da5d028b22bd";

		// Token: 0x04003E1F RID: 15903
		private ApiAvatar mApiAvatar;

		// Token: 0x04003E20 RID: 15904
		protected static User mCurrentNonApiUser;
	}
}
