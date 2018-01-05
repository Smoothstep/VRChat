using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Steamworks;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using VRC;
using VRC.Core;
using VRCSDK2;
//System.Runtime.Remoting.Messaging.MonoM

// Token: 0x02000C57 RID: 3159
public class VRCFlowManager : MonoBehaviour
{
	// Token: 0x14000086 RID: 134
	// (add) Token: 0x060061DE RID: 25054 RVA: 0x00228B44 File Offset: 0x00226F44
	// (remove) Token: 0x060061DF RID: 25055 RVA: 0x00228B7C File Offset: 0x00226F7C
	public event Action onEnteredWorld;

	// Token: 0x17000DA3 RID: 3491
	// (get) Token: 0x060061E0 RID: 25056 RVA: 0x00228BB2 File Offset: 0x00226FB2
	public static VRCFlowManager Instance
	{
		get
		{
			return VRCFlowManager.mInstance;
		}
	}

	// Token: 0x17000DA4 RID: 3492
	// (get) Token: 0x060061E1 RID: 25057 RVA: 0x00228BB9 File Offset: 0x00226FB9
	public bool HasConnectionError
	{
		get
		{
			return this.isInConnectErrorState;
		}
	}

	// Token: 0x17000DA5 RID: 3493
	// (get) Token: 0x060061E2 RID: 25058 RVA: 0x00228BC1 File Offset: 0x00226FC1
	public bool AllowThirdPartyLogin
	{
		get
		{
			return this._allowThirdPartyLogin;
		}
	}

	// Token: 0x17000DA6 RID: 3494
	// (get) Token: 0x060061E3 RID: 25059 RVA: 0x00228BC9 File Offset: 0x00226FC9
	public VRCFlowCommandLine CommandLine
	{
		get
		{
			return this.commandLine;
		}
	}

	// Token: 0x17000DA7 RID: 3495
	// (get) Token: 0x060061E4 RID: 25060 RVA: 0x00228BD1 File Offset: 0x00226FD1
	// (set) Token: 0x060061E5 RID: 25061 RVA: 0x00228BD9 File Offset: 0x00226FD9
	private string destinationWorldId
	{
		get
		{
			return this._destinationWorldId;
		}
		set
		{
			UnityEngine.Debug.Log("Destination set: " + value);
			this._destinationWorldId = value;
			if (!string.IsNullOrEmpty(this._destinationWorldId))
			{
				RoomManager.LeaveRoom();
			}
		}
	}

	// Token: 0x17000DA8 RID: 3496
	// (get) Token: 0x060061E6 RID: 25062 RVA: 0x00228C07 File Offset: 0x00227007
	// (set) Token: 0x060061E7 RID: 25063 RVA: 0x00228C0F File Offset: 0x0022700F
	public bool HasAttemptedCachedLogin { get; private set; }

	// Token: 0x060061E8 RID: 25064 RVA: 0x00228C18 File Offset: 0x00227018
	private void Awake()
	{
		if (VRCFlowManager.mInstance != null)
		{
			UnityEngine.Debug.LogError("More than one VRCFlowManager detected!!!");
		}
		else
		{
			VRCFlowManager.mInstance = this;
		}
		this.analyticDict = new Dictionary<string, object>();
	}

	// Token: 0x060061E9 RID: 25065 RVA: 0x00228C4C File Offset: 0x0022704C
	private void Start()
	{
		string assetUrl = string.Concat(new object[]
		{
			Application.dataPath,
			Path.DirectorySeparatorChar,
			"StreamingAssets",
			Path.DirectorySeparatorChar,
			"errorworld.vrcw"
		});
		this.errorWorldId = "local:error_" + Tools.GetRandomDigits(10);
		ApiWorld.AddLocal(new ApiWorld
		{
			id = this.errorWorldId,
			assetUrl = assetUrl,
			pluginUrl = string.Empty,
			name = "Error World",
			capacity = 1
		});
		if (GameObject.Find("UserInterface") == null)
		{
			AssetManagement.LoadLevelAdditive("ui");
		}
		base.StartCoroutine(this.ManageGameFlow(new VRCFlowManager.ResetGameFlags[]
		{
			VRCFlowManager.ResetGameFlags.ShowUI,
			VRCFlowManager.ResetGameFlags.GoToDefaultWorld
		}));
		this.commandLine = base.GetComponent<VRCFlowCommandLine>();
	}

	// Token: 0x060061EA RID: 25066 RVA: 0x00228D2C File Offset: 0x0022712C
	private IEnumerator ManageGameFlow(params VRCFlowManager.ResetGameFlags[] flags)
	{
        UnityEngine.Debug.developerConsoleVisible = true;
		bool showUI = flags.Contains(VRCFlowManager.ResetGameFlags.ShowUI);
		bool skipLogin = flags.Contains(VRCFlowManager.ResetGameFlags.SkipLogin) && APIUser.IsLoggedIn;
		this.startTime = Time.timeSinceLevelLoad;
		this.lastShowUIRequested = showUI;
		this.lastSkipLogin = skipLogin;
		this._allowThirdPartyLogin = true;
		VRCAudioManager.EnableAllAudio(true);
		UnityEngine.Debug.Log("<color=yellow>ManageGameFlow waiting for UI Manager, Patched version</color>");
		while (VRCUiManager.Instance == null)
		{
			yield return null;
		}
		if (!skipLogin)
		{
			UnityEngine.Debug.Log("<color=yellow>ManageGameFlow get remote config and default blueprints</color>");
			this.GetOnlineData();
		}
		UnityEngine.Debug.Log("<color=yellow>ManageGameFlow connecting to online servers</color>");
		yield return base.StartCoroutine(this.BeginConnectionToPhoton(flags.Contains(VRCFlowManager.ResetGameFlags.IsConnectionRetry)));
		QuickMenu.Instance.IsAllowed = false;
		VRCUiManager.Instance.EnableHudMessages(false);
		this._hasEnteredRoom = false;
		if (showUI && !VRCUiManager.Instance.IsActive())
		{
			VRCUiManager.Instance.FadeTo("SpaceFade", 0f, null);
			VRCUiManager.Instance.FadeTo("BlackFade", 0f, null);
			yield return base.StartCoroutine(this.WaitForMinimumTimeSinceLoad(1f));
			VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Screens/Title");
			yield return base.StartCoroutine(this.WaitForMinimumTimeSinceLoad(4f));
			VRCUiManager.Instance.HideScreen("SCREEN");
			yield return base.StartCoroutine(this.WaitForMinimumTimeSinceLoad(5f));
		}
		if (!skipLogin)
		{
			UnityEngine.Debug.Log("<color=yellow>ManageGameFlow waiting for remote config</color>");
			yield return base.StartCoroutine(this.WaitForOnlineData());
		}
		UnityEngine.Debug.Log("<color=yellow>ManageGameFlow waiting for photon</color>");
		yield return base.StartCoroutine(this.WaitForPhotonConnection());
		if (!this.hasPerformedUpdateCheck)
		{
			this.hasPerformedUpdateCheck = true;
			UnityEngine.Debug.Log("<color=yellow>ManageGameFlow performing update check</color>");
			yield return base.StartCoroutine(this.PerformUpdateCheckAndFlow());
		}
		yield return base.StartCoroutine(this.ShowQueuedAlert());
		if (!skipLogin)
		{
			if (this.commandLine.CommandLineOperation == VRCFlowCommandLine.Operation.CreateRoom)
			{
				this._allowThirdPartyLogin = false;
			}
			UnityEngine.Debug.Log("<color=yellow>ManageGameFlow authenticating</color>");
			yield return base.StartCoroutine(this.Authenticate(showUI));
			UnityEngine.Analytics.Analytics.SetUserId(User.CurrentUser.id);
			this.SendFlowStepAnalytic("loggedIn");
			UnityEngine.Debug.Log("<color=yellow>ManageGameFlow checking for ban</color>");
			yield return base.StartCoroutine(this.CheckModeration());
		}
		UnityEngine.Debug.Log("<color=yellow>ManageGameFlow checking for account info update</color>");
		yield return base.StartCoroutine(this.CheckAccountInfoNeedsUpdate());
		UnityEngine.Debug.Log("<color=yellow>ManageGameFlow checking terms of service</color>");
		yield return base.StartCoroutine(this.CheckTermsOfService());
		UnityEngine.Debug.Log("<color=yellow>ManageGameFlow set connect error state</color>");
		this.SetConnectErrorState(false);
		UnityEngine.Debug.Log("<color=yellow>ManageGameFlow setup crash logging</color>");
		this.OneTimeForceSettings();
		UnityEngine.Debug.Log("<color=yellow>ManageGameFlow show first time settings</color>");
		yield return base.StartCoroutine(this.CheckAndDisplayFirstTimeSettings());
		if (flags.Contains(VRCFlowManager.ResetGameFlags.GoToDefaultWorld) && !this.IsGoingToTutorial())
		{
			this.GoToDefaultWorld();
		}
		if (this.commandLine.IsCommandLineLaunch())
		{
			this.destinationWorldId = this.commandLine.launchId;
			this.commandLine.CommandLineOperation = VRCFlowCommandLine.Operation.None;
		}
		if (string.IsNullOrEmpty(this.destinationWorldId) && !showUI)
		{
			VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Backdrop/Backdrop");
			VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Backdrop/Header");
			VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Backdrop/Footer");
		}
		VRCUiManager.Instance.CloseUi(false);
		UnityEngine.Debug.Log("<color=yellow>ManageGameFlow entering main loop</color>");
		while (APIUser.IsLoggedIn)
		{
			if (!string.IsNullOrEmpty(this.destinationWorldId))
			{
				UnityEngine.Debug.Log("<color=yellow>ManageGameFlow beginning room transition</color>");
				QuickMenu.Instance.IsAllowed = false;
				VRCUiManager.Instance.EnableHudMessages(false);
				yield return base.StartCoroutine(this.TransitionToRoom(true));
			}
			else
			{
				VRCFlowNetworkManager photonConnect = base.GetComponent<VRCFlowNetworkManager>();
				if (RoomManager.inRoom)
				{
					QuickMenu.Instance.IsAllowed = true;
				}
				else if (!photonConnect.isConnected && !photonConnect.isConnecting)
				{
					yield return base.StartCoroutine(this.BeginConnectionToPhoton(false));
				}
				else if (RoomManager.currentRoom != null)
				{
					this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[1]);
				}
				else
				{
					if (ModerationManager.Instance != null && VRC.Player.Instance != null && ModerationManager.Instance.IsKicked(VRC.Player.Instance.userId))
					{
						break;
					}
					if (!VRCUiManager.Instance.IsActive())
					{
						UnityEngine.Debug.Log("<color=yellow>ManageGameFlow has no active ui manager.</color>");
						break;
					}
				}
			}
			yield return null;
		}
		if (!APIUser.IsLoggedIn)
		{
			UnityEngine.Debug.Log("<color=yellow>User is logged out.</color>");
		}
		UnityEngine.Debug.Log("<color=yellow>ManageGameFlow resetting flow</color>");
		this.GoToDefaultWorld();
		this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[0]);
		yield break;
	}

	// Token: 0x060061EB RID: 25067 RVA: 0x00228D4E File Offset: 0x0022714E
	private void SetConnectErrorState(bool isInErrorState)
	{
		if (this.isInConnectErrorState == isInErrorState)
		{
			return;
		}
		UnityEngine.Debug.Log("SetConnectErrorState " + isInErrorState);
		this.isInConnectErrorState = isInErrorState;
	}

	// Token: 0x060061EC RID: 25068 RVA: 0x00228D7C File Offset: 0x0022717C
	private void CheckForConnectionRetry()
	{
		if (this.HasConnectionError && Time.timeSinceLevelLoad - this.startTime >= this.ConnectionFlowResetTime)
		{
			UnityEngine.Debug.LogError("Photon connection state " + PhotonNetwork.connectionStateDetailed.ToString());
			UnityEngine.Debug.Log("CheckForConnectionRetry - resetting flow");
			this.SetConnectErrorState(false);
			List<VRCFlowManager.ResetGameFlags> list = new List<VRCFlowManager.ResetGameFlags>
			{
				VRCFlowManager.ResetGameFlags.IsConnectionRetry
			};
			if (this.lastShowUIRequested || this.lastSkipLogin)
			{
				list.Add(VRCFlowManager.ResetGameFlags.ShowUI);
			}
			this.ResetGameFlow(list.ToArray());
		}
	}

	// Token: 0x060061ED RID: 25069 RVA: 0x00228E16 File Offset: 0x00227216
	private void GetOnlineData()
	{
		RemoteConfig.Init(true, delegate
		{
			UnityEngine.Debug.Log("RemoteConfig Init() succeeded");
			this._timeRemoteConfigLastRefreshed = Time.realtimeSinceStartup;
		}, delegate
		{
			UnityEngine.Debug.Log("Failed to get RemoteConfig!");
			this.SetConnectErrorState(true);
		});
	}

	// Token: 0x060061EE RID: 25070 RVA: 0x00228E38 File Offset: 0x00227238
	private IEnumerator BeginConnectionToPhoton(bool isRetry)
	{
		VRCFlowNetworkManager photonConnect = base.GetComponent<VRCFlowNetworkManager>();
		while (photonConnect.isConnecting)
		{
			yield return null;
		}
		photonConnect.BeginConnection(isRetry);
		yield break;
	}

	// Token: 0x060061EF RID: 25071 RVA: 0x00228E5C File Offset: 0x0022725C
	private IEnumerator WaitForMinimumTimeSinceLoad(float t)
	{
		while (Time.timeSinceLevelLoad < this.startTime + t)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060061F0 RID: 25072 RVA: 0x00228E80 File Offset: 0x00227280
	private IEnumerator WaitForPhotonConnection()
	{
		VRCFlowNetworkManager photonConnect = base.GetComponent<VRCFlowNetworkManager>();
		while (!photonConnect.isConnected)
		{
			if (!photonConnect.isConnecting)
			{
				photonConnect.Disconnect();
				this.SetConnectErrorState(true);
			}
			this.CheckForConnectionRetry();
			yield return null;
		}
		yield break;
	}

	// Token: 0x060061F1 RID: 25073 RVA: 0x00228E9C File Offset: 0x0022729C
	private IEnumerator PerformUpdateCheckAndFlow()
	{
		bool UpdateRequired = VRCApplicationSetup.CheckForUpdates();
		if (!UpdateRequired)
		{
			yield break;
		}
		VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Backdrop/Backdrop");
		VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Screens/UpdateRequired");
		for (;;)
		{
			yield return null;
		}
	}

	// Token: 0x060061F2 RID: 25074 RVA: 0x00228EB0 File Offset: 0x002272B0
	private IEnumerator WaitForOnlineData()
	{
		while (!RemoteConfig.IsInitialized())
		{
			this.CheckForConnectionRetry();
			yield return null;
		}
		yield break;
	}

	// Token: 0x060061F3 RID: 25075 RVA: 0x00228ECC File Offset: 0x002272CC
	private IEnumerator ShowQueuedAlert()
	{
		if (!this._hasQueuedAlert)
		{
			yield break;
		}
		VRCUiPopupManager.Instance.ShowAlert(this._queuedAlertTitle, this._queuedAlertMessage, (float)this._queuedAlertTimeout);
		while (VRCUiPopupManager.Instance.isPopupActive)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060061F4 RID: 25076 RVA: 0x00228EE8 File Offset: 0x002272E8
	private IEnumerator CheckAccountInfoNeedsUpdate()
	{
		bool isEmailRequired = true;
		bool isDoBRequired = true;
		if (ApiCredentials.GetAuthTokenProvider() == "steam")
		{
			isEmailRequired = false;
			isDoBRequired = true;
		}
		if ((!isEmailRequired || User.CurrentUser.hasEmail) && (!isDoBRequired || User.CurrentUser.hasBirthday))
		{
			yield break;
		}
		VRCUiPopupManager.Instance.ShowStandardPopup("Account Verification", (!isEmailRequired) ? "Please verify your date of birth to proceed into VRChat." : "Please verify your email address and date of birth to proceed into VRChat.", "Continue", delegate
		{
			VRCUiPopupManager.Instance.HideCurrentPopup();
			VRCUiPageAccountUpdate.IsEmailRequired = isEmailRequired;
			VRCUiPageAccountUpdate.IsDoBRequired = isDoBRequired;
			VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Screens/Authentication/LoginUpdate");
		}, null);
		for (;;)
		{
			yield return null;
		}
	}

	// Token: 0x060061F5 RID: 25077 RVA: 0x00228EFC File Offset: 0x002272FC
	private IEnumerator CheckTermsOfService()
	{
		Action showTOSScreen = delegate
		{
			VRCUiPopupManager.Instance.HideCurrentPopup();
			VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Screens/Authentication/AgreeTermsOfService");
		};
		int currentTOSVersion = 0;
		if (int.TryParse(RemoteConfig.GetString("currentTOSVersion"), out currentTOSVersion))
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"TOS current version: ",
				currentTOSVersion,
				", accepted: ",
				User.CurrentUser.acceptedTOSVersion
			}));
			if (User.CurrentUser.acceptedTOSVersion < currentTOSVersion)
			{
				if (!this.skipTOSChangedDialog)
				{
					VRCUiPopupManager.Instance.ShowStandardPopup("Terms Of Service", "Our Terms of Service have changed. Please continue to review the updated Terms of Use and End User License Agreement.", "Continue", delegate
					{
						showTOSScreen();
					}, null);
				}
				else
				{
					this.skipTOSChangedDialog = false;
					showTOSScreen();
				}
				for (;;)
				{
					yield return null;
				}
			}
		}
		yield break;
	}

	// Token: 0x060061F6 RID: 25078 RVA: 0x00228F18 File Offset: 0x00227318
	private IEnumerator CheckVerification()
	{
		if (User.CurrentUser.verified)
		{
			yield break;
		}
		VRCUiPopupManager.Instance.ShowStandardPopup("Email Verification", "You must verify your email address before you can proceed into VRChat. Log in to another account or re-send your verification email.", "Log-in", delegate
		{
			User.Logout();
			this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[0]);
			VRCUiPopupManager.Instance.HideCurrentPopup();
		}, "Re-send", delegate
		{
			APIUser.AttemptVerification();
			VRCUiPopupManager.Instance.ShowStandardPopup("Email Sent!", "Check the email address you used when creating your account. Click the link in we sent to verify your account and sign in here.", "Thanks", delegate
			{
				User.Logout();
				this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[0]);
				VRCUiPopupManager.Instance.HideCurrentPopup();
			}, null);
		}, null);
		global::Analytics.Send(ApiAnalyticEvent.EventType.error, "EmailNotVerified", null, null);
		for (;;)
		{
			yield return null;
		}
	}

	// Token: 0x060061F7 RID: 25079 RVA: 0x00228F34 File Offset: 0x00227334
	private IEnumerator CheckModeration()
	{
		if (!APIUser.IsLoggedIn)
		{
			UnityEngine.Debug.LogError("CheckModeration: User not logged in");
			yield break;
		}
		ModerationManager.Instance.OnUserLoggedIn();
		bool moderationComplete = false;
		bool moderationSuccess = false;
		ModerationManager.Instance.FetchModerations(delegate
		{
			moderationComplete = true;
			moderationSuccess = true;
		}, delegate(string err)
		{
			moderationComplete = true;
			moderationSuccess = false;
		});
		while (!moderationComplete)
		{
			yield return null;
		}
		if (!moderationSuccess)
		{
			UnityEngine.Debug.LogWarning("Failed to fetch moderation data.");
			this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[0]);
		}
		ApiModeration myBan = null;
		if (!moderationSuccess || !ModerationManager.Instance.IsBanned(APIUser.CurrentUser.id))
		{
			yield break;
		}
		moderationSuccess = false;
		myBan = ModerationManager.Instance.GetModerationOfType(ApiModeration.ModerationType.Ban);
		string dateString = myBan.expires.ToString("MMM dd, yyyy HH:mm");
		UnityEngine.Debug.LogError("You have been banned until " + dateString);
		VRCUiPopupManager.Instance.ShowStandardPopup("Moderation", "You have been banned until " + dateString, "OK", delegate
		{
			User.Logout();
			this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[0]);
			VRCUiPopupManager.Instance.HideCurrentPopup();
		}, null);
		global::Analytics.Send(ApiAnalyticEvent.EventType.error, "UserIsBanned", null, null);
		for (;;)
		{
			yield return null;
		}
	}

	// Token: 0x060061F8 RID: 25080 RVA: 0x00228F4F File Offset: 0x0022734F
	public void AuthenticateWithSteam(bool showUI)
	{
		base.StartCoroutine(this.AuthenticateWithSteamCoroutine(showUI, null));
	}

	// Token: 0x060061F9 RID: 25081 RVA: 0x00228F60 File Offset: 0x00227360
	private IEnumerator AuthenticateWithSteamCoroutine(bool showUI, Action<bool> onAuthed = null)
	{
		if (!SteamManager.Initialized)
		{
			UnityEngine.Debug.LogError("Login via Steam failed, SteamManager failed to initialize");
			yield break;
		}
		AppId_t appId = (AppId_t)438100u;
		if (VRCApplicationSetup.Instance.ServerEnvironment == ApiServerEnvironment.Dev)
		{
			appId = (AppId_t)326100u;
		}
		if (VRCApplicationSetup.Instance.ServerEnvironment == ApiServerEnvironment.Beta)
		{
			appId = (AppId_t)744530u;
		}
		bool shutdown = false;
		if (!SteamAPI.IsSteamRunning())
		{
			Process.Start("steam://rungameid/" + appId.ToString());
			shutdown = true;
		}
		else if (SteamAPI.RestartAppIfNecessary(appId))
		{
			shutdown = true;
		}
		if (!shutdown)
		{
			VRCUiManager.Instance.HideScreen("SCREEN");
			bool popupComplete = false;
			bool popupCanceled = false;
			bool LoginSuccess = false;
			bool loginComplete = false;
			if (showUI)
			{
				string personaName = SteamFriends.GetPersonaName();
				VRCUiManager.Instance.popups.ShowStandardPopup("LOGIN", "Logging in through Steam as " + personaName, "Cancel", delegate
				{
					popupCanceled = true;
					User.Logout();
					UnityEngine.Debug.Log("Steam Login Canceled");
				}, delegate(VRCUiPopup popup)
				{
					popup.SetupProgressTimer(10f, delegate
					{
						popupComplete = true;
					}, 1f);
				});
			}
			byte[] ticket = new byte[1024];
			uint pcbTicket;
			SteamUser.GetAuthSessionTicket(ticket, 1024, out pcbTicket);
			Array.Resize<byte>(ref ticket, (int)pcbTicket);
			Action<APIUser> loginSuccessDelegate = delegate(APIUser user)
			{
				if (popupCanceled)
				{
					User.Logout();
				}
				else
				{
					APIUser.FetchFriends(null, null);
				}
				loginComplete = true;
			};
			Action<string> loginFailureDelegate = delegate(string err)
			{
				UnityEngine.Debug.LogError(string.Format("Failed to call SteamLogin: {0}", err));
				User.Logout();
				VRCUiManager.Instance.popups.ShowStandardPopup("ERROR", "Could not log in. Please try again later.", "Okay", delegate
				{
					VRCUiManager.Instance.popups.HideCurrentPopup();
					popupCanceled = true;
				}, null);
				global::Analytics.Send(ApiAnalyticEvent.EventType.error, "SteamLoginFailed: " + err, null, null);
			};
			string hex = BitConverter.ToString(ticket).Replace("-", string.Empty);
			User.SteamLogin(hex, loginSuccessDelegate, loginFailureDelegate);
			while (!loginComplete && !popupCanceled)
			{
				yield return null;
			}
			if (showUI)
			{
				if (!popupCanceled)
				{
					VRCUiManager.Instance.currentPopup.ProgressComplete();
				}
				while (!popupComplete && !popupCanceled)
				{
					yield return null;
				}
				VRCUiManager.Instance.HideScreen("POPUP");
			}
			LoginSuccess = !popupCanceled;
			if (onAuthed != null)
			{
				onAuthed(LoginSuccess);
			}
			if (!LoginSuccess)
			{
				this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[0]);
			}
			yield break;
		}
		if (this.commandLine.IsCommandLineLaunch())
		{
			this.commandLine.SaveUrlForNextLaunch();
		}
		VRCUiManager.Instance.popups.ShowStandardPopup("Launching Through Steam", "VRChat is shutting down to restart within steam. It will take a moment for this to complete.", "OK", delegate
		{
			Application.Quit();
		}, delegate(VRCUiPopup popup)
		{
			popup.SetupProgressTimer(20f, delegate
			{
				Application.Quit();
			}, 0f);
		});
		global::Analytics.Send(ApiAnalyticEvent.EventType.error, "LaunchedOutsideSteamRestart", null, null);
		for (;;)
		{
			yield return null;
		}
	}

	// Token: 0x060061FA RID: 25082 RVA: 0x00228F8C File Offset: 0x0022738C
	private IEnumerator AuthenticateWithVRChat(bool showUI, Action<bool> onAuthed = null)
	{
		bool LoginSuccess = false;
		if (ApiCredentials.Load())
		{
			bool popupCanceled = false;
			bool popupComplete = false;
			bool loginComplete = false;
			VRCUiManager.Instance.HideScreen("SCREEN");
			Action<APIUser> loginSuccessDelegate = delegate(APIUser user)
			{
				APIUser.FetchFriends(null, null);
				loginComplete = true;
			};
			Action<string> loginFailureDelegate = delegate(string err)
			{
				UnityEngine.Debug.LogError(string.Format("Failed to call CachedLogin: {0}", err));
				User.Logout();
				popupCanceled = true;
			};
			User.Login(loginSuccessDelegate, loginFailureDelegate);
			if (showUI)
			{
				VRCUiManager.Instance.popups.ShowStandardPopup("LOGIN", "Logging in as " + ApiCredentials.GetUsername(), "Cancel", delegate
				{
					popupCanceled = true;
					UnityEngine.Debug.Log("Canceled");
				}, delegate(VRCUiPopup popup)
				{
					popup.SetupProgressTimer(10f, delegate
					{
						popupComplete = true;
					}, 1f);
				});
			}
			while (!loginComplete && !popupCanceled)
			{
				yield return null;
			}
			if (showUI)
			{
				if (!popupCanceled)
				{
					VRCUiManager.Instance.currentPopup.ProgressComplete();
				}
				while (!popupComplete && !popupCanceled)
				{
					yield return null;
				}
				VRCUiManager.Instance.HideScreen("POPUP");
			}
			LoginSuccess = !popupCanceled;
		}
		if (onAuthed != null)
		{
			onAuthed(LoginSuccess);
		}
		yield break;
	}

	// Token: 0x060061FB RID: 25083 RVA: 0x00228FB0 File Offset: 0x002273B0
	private IEnumerator Authenticate(bool showUI)
	{
		bool LoginSuccess = false;
		Action<bool> onAuthed = delegate(bool isSuccess)
		{
			LoginSuccess = isSuccess;
		};
		bool CachedLoginFailed = false;
		if (ApiCredentials.Load())
		{
			UnityEngine.Debug.Log("Logging in with cached vrchat creds");
			yield return base.StartCoroutine(this.AuthenticateWithVRChat(showUI, onAuthed));
		}
		else if (this.AllowThirdPartyLogin && ApiCredentials.GetAuthTokenProvider() == "steam")
		{
			if (SteamManager.Initialized)
			{
				string cachedSteamUserId = ApiCredentials.GetAuthTokenProviderUserId();
				if (string.IsNullOrEmpty(cachedSteamUserId) || cachedSteamUserId == SteamUser.GetSteamID().ToString())
				{
					UnityEngine.Debug.Log("Logging in with cached steam creds");
					yield return base.StartCoroutine(this.AuthenticateWithSteamCoroutine(showUI, onAuthed));
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Skipping cached steam login, SteamManager failed to initialize");
			}
		}
		this.HasAttemptedCachedLogin = true;
		if (!LoginSuccess)
		{
			User.Logout();
			if (CachedLoginFailed)
			{
				this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[0]);
			}
			else if (showUI)
			{
				VRCUiManager.Instance.HideAll();
				VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Backdrop/Backdrop");
				VRCUiPageAuthentication.ShowAppropriateLoginPromptScreenInternal();
				while (!APIUser.IsLoggedIn)
				{
					yield return null;
				}
			}
			else
			{
				this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[1]);
			}
		}
		yield break;
	}

	// Token: 0x060061FC RID: 25084 RVA: 0x00228FD4 File Offset: 0x002273D4
	private void OneTimeForceSettings()
	{
		if (!VRCTrackingManager.IsInVRMode())
		{
			if (this.ShouldOverrideSettingOnce("ForceSettings_MicToggle", 2))
			{
				VRCInputManager.talkToggle = false;
			}
			if (this.ShouldOverrideSettingOnce("ForceSettings_MicTalk", 2))
			{
				VRCInputManager.talkDefaultOn = false;
			}
		}
		if (this.ShouldOverrideSettingOnce("ForceSettings_Grasp", 3))
		{
			VRCInputManager.legacyGrasp = false;
		}
	}

	// Token: 0x060061FD RID: 25085 RVA: 0x00229030 File Offset: 0x00227430
	private IEnumerator CheckAndDisplayFirstTimeSettings()
	{
		yield return this.DisplaySettingScreenOnce("ForceSettings_MuteSetting", 4, "UserInterface/MenuContent/Screens/FirstLogin/MuteSetting");
		yield return this.DisplaySettingScreenOnce("ForceSettings_Tutorial", 1, "UserInterface/MenuContent/Screens/FirstLogin/TutorialLaunch");
		yield break;
	}

	// Token: 0x060061FE RID: 25086 RVA: 0x0022904C File Offset: 0x0022744C
	private bool ShouldOverrideSettingOnce(string playerPrefsForce, int version)
	{
		if (version <= 0)
		{
			UnityEngine.Debug.LogError("Must specify a setting version greater than 0");
		}
		int num = 0;
		if (PlayerPrefs.HasKey(playerPrefsForce))
		{
			num = PlayerPrefs.GetInt(playerPrefsForce);
		}
		if (num != version)
		{
			PlayerPrefs.SetInt(playerPrefsForce, version);
			return true;
		}
		return false;
	}

	// Token: 0x060061FF RID: 25087 RVA: 0x00229090 File Offset: 0x00227490
	private IEnumerator DisplaySettingScreenOnce(string playerPrefsForce, int version, string screenPath)
	{
		if (version <= 0)
		{
			UnityEngine.Debug.LogError("Must specify a setting version greater than 0");
		}
		int storedVersion = 0;
		if (PlayerPrefs.HasKey(playerPrefsForce))
		{
			storedVersion = PlayerPrefs.GetInt(playerPrefsForce);
		}
		if (storedVersion == version)
		{
			yield break;
		}
		if (screenPath != null)
		{
			VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Backdrop/Backdrop");
			VRCUiManager.Instance.ShowScreen(screenPath);
			while (VRCUiManager.Instance.IsScreenActive("SCREEN"))
			{
				yield return null;
			}
		}
		PlayerPrefs.SetInt(playerPrefsForce, version);
		yield break;
	}

	// Token: 0x06006200 RID: 25088 RVA: 0x002290BC File Offset: 0x002274BC
	public IEnumerator LoginAccount(string userName, string password)
	{
		string bodyText = "Anonymous Log In";
		if (userName != null)
		{
			bodyText = "Logging in as " + userName;
		}
		VRCUiManager.Instance.popups.ShowStandardPopup("LOGIN", bodyText, "Cancel", delegate
		{
			User.Logout();
			VRCUiManager.Instance.popups.HideCurrentPopup();
		}, delegate(VRCUiPopup popup)
		{
			popup.SetupProgressTimer(10f, delegate
			{
				VRCUiManager.Instance.popups.HideCurrentPopup();
			}, 1f);
		});
		if (userName == null)
		{
			UnityEngine.Debug.LogError("Can't log in anonymously anymore");
			yield return new WaitForSeconds(3f);
			VRCUiManager.Instance.currentPopup.ProgressComplete();
		}
		else
		{
			User.Login(userName, password, delegate
			{
				APIUser.FetchFriends(null, null);
				VRCUiManager.Instance.currentPopup.ProgressComplete();
			}, delegate(string errorMessage)
			{
				UnityEngine.Debug.Log("Error loggin in - " + errorMessage);
				VRCUiManager.Instance.popups.ShowStandardPopup("ERROR", "Could not log in - " + errorMessage, "Okay", delegate
				{
					VRCUiManager.Instance.popups.HideCurrentPopup();
				}, null);
				global::Analytics.Send(ApiAnalyticEvent.EventType.error, "UpdateAccountFailed: login api failed - " + errorMessage, null, null);
			});
		}
		yield break;
	}

	// Token: 0x06006201 RID: 25089 RVA: 0x002290E0 File Offset: 0x002274E0
	public IEnumerator CreateAccount(string userName, string password, string email, string birthday)
	{
		bool isDoneRegistering = false;
		VRCUiManager.Instance.popups.ShowStandardPopup("CREATING ACCOUNT", "Creating account " + userName, null);
		User.Register(userName, email, password, birthday, delegate(APIUser user)
		{
			isDoneRegistering = true;
			VRCUiManager.Instance.popups.HideCurrentPopup();
			this.skipTOSChangedDialog = true;
		}, delegate(string obj)
		{
			isDoneRegistering = true;
			UnityEngine.Debug.Log("Registration error - " + obj);
			VRCUiManager.Instance.popups.ShowStandardPopup("ERROR", "Could not create account - " + obj, "Okay", delegate
			{
				VRCUiManager.Instance.popups.HideCurrentPopup();
			}, null);
			global::Analytics.Send(ApiAnalyticEvent.EventType.error, "CreateAccountFailed: register api failed - " + obj, null, null);
		});
		while (!isDoneRegistering)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006202 RID: 25090 RVA: 0x00229118 File Offset: 0x00227518
	public IEnumerator UpdateAccountInfo(string email, string birthday)
	{
		bool isDone = false;
		VRCUiManager.Instance.popups.ShowStandardPopup("VERIFYING ACCOUNT", "Verifying account " + User.CurrentUser.username, null);
		User.UpdateAccountInfo(User.CurrentUser.id, email, birthday, null, delegate(APIUser user)
		{
			isDone = true;
			VRCUiManager.Instance.popups.HideCurrentPopup();
			this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[0]);
		}, delegate(string obj)
		{
			isDone = true;
			UnityEngine.Debug.Log("Verification error - " + obj);
			VRCUiManager.Instance.popups.ShowStandardPopup("ERROR", "Could not verify account - " + obj, "Okay", delegate
			{
				VRCUiManager.Instance.popups.HideCurrentPopup();
			}, null);
			global::Analytics.Send(ApiAnalyticEvent.EventType.error, "UpdateAccountFailed: update api failed - " + obj, null, null);
		});
		while (!isDone)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006203 RID: 25091 RVA: 0x00229144 File Offset: 0x00227544
	public IEnumerator AgreeToTermsOfService()
	{
		bool isDone = false;
		int currentTOSVersion = 0;
		if (int.TryParse(RemoteConfig.GetString("currentTOSVersion"), out currentTOSVersion))
		{
			VRCUiManager.Instance.popups.ShowStandardPopup("UPDATING ACCOUNT", "Updating account " + User.CurrentUser.username, null);
			User.UpdateAccountInfo(User.CurrentUser.id, null, null, currentTOSVersion.ToString(), delegate(APIUser user)
			{
				isDone = true;
				VRCUiManager.Instance.popups.HideCurrentPopup();
				this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[0]);
			}, delegate(string obj)
			{
				isDone = true;
				UnityEngine.Debug.Log("Update account error - " + obj);
				VRCUiManager.Instance.popups.ShowStandardPopup("ERROR", "Could not update account - " + obj, "Okay", delegate
				{
					VRCUiManager.Instance.popups.HideCurrentPopup();
				}, null);
				global::Analytics.Send(ApiAnalyticEvent.EventType.error, "UpdateAccountFailed: " + obj, null, null);
			});
		}
		else
		{
			VRCUiManager.Instance.popups.ShowStandardPopup("ERROR", "Could not update account. Please try again later.", "Okay", delegate
			{
				VRCUiManager.Instance.popups.HideCurrentPopup();
			}, null);
		}
		while (!isDone)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006204 RID: 25092 RVA: 0x0022915F File Offset: 0x0022755F
	protected void OnApplicationQuit()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x17000DA9 RID: 3497
	// (get) Token: 0x06006205 RID: 25093 RVA: 0x00229167 File Offset: 0x00227567
	// (set) Token: 0x06006206 RID: 25094 RVA: 0x0022916E File Offset: 0x0022756E
	[DefaultValue(false)]
	public static bool BlockResetGameFlow { get; set; }

	// Token: 0x06006207 RID: 25095 RVA: 0x00229178 File Offset: 0x00227578
	public void ResetGameFlow(params VRCFlowManager.ResetGameFlags[] flags)
	{
		if (VRCFlowManager.BlockResetGameFlow)
		{
			UnityEngine.Debug.LogError("ResetGameFlow blocked.");
			return;
		}
		if (this._resetGameFlowPending)
		{
			UnityEngine.Debug.LogError("ResetGameFlow called before last one finished, ignoring.");
			return;
		}
		this._resetGameFlowPending = true;
		this.skipTOSChangedDialog = false;
		base.StopAllCoroutines();
		UnityEngine.Debug.Log("<color=red>Resetting game flow</color>");
		if (flags.Contains(VRCFlowManager.ResetGameFlags.ShowUI) && (!flags.Contains(VRCFlowManager.ResetGameFlags.IsConnectionRetry) || this.lastSkipLogin))
		{
			VRCUiManager.Instance.CloseUi(true);
		}
		NeckMouseRotator componentInChildren = VRCVrCamera.GetInstance().GetComponentInChildren<NeckMouseRotator>();
		if (componentInChildren != null)
		{
			componentInChildren.neckLookUpDist = 0f;
			componentInChildren.neckLookDownDist = 0f;
			componentInChildren.rotationRange.x = 0f;
			componentInChildren.rotationRange.z = 0f;
			componentInChildren.rotationRange.y = 0f;
		}
		RoomManager.LeaveRoom();
		if (flags.Contains(VRCFlowManager.ResetGameFlags.UploadErrorLog))
		{
			VRCLog.UploadMiniLog("ResetGameFlow was triggered due to error!");
		}
		if (flags.Contains(VRCFlowManager.ResetGameFlags.IsConnectionRetry))
		{
			VRCFlowNetworkManager.Instance.Disconnect();
		}
		if (flags.Contains(VRCFlowManager.ResetGameFlags.ClearDestinationWorld))
		{
			if (this.IsGoingToDefaultWorld())
			{
				this.roomReloadRetryCount++;
			}
			if (this.roomReloadRetryCount >= 2)
			{
				UnityEngine.Debug.Log("<color=red>Failed to load default world " + this.roomReloadRetryCount + " times, going to error world</color>");
				this.roomReloadRetryCount = 0;
				this.GoErrorWorld();
			}
			else
			{
				this.GoToDefaultWorld();
			}
		}
		if (flags.Contains(VRCFlowManager.ResetGameFlags.TryAlternateInstance))
		{
			UnityEngine.Debug.Log("ResetGameFlow: choosing alternate instance. destid=" + this.destinationWorldId);
			string id = this.destinationWorldId;
			if (this.destinationWorldId != null)
			{
				id = this.destinationWorldId.Split(new char[]
				{
					':'
				})[0].Trim();
			}
			ApiWorld.Fetch(id, delegate(ApiWorld w)
			{
				UnityEngine.Debug.Log("ResetGameFlow: w=" + w);
				if (this.destinationWorldId == null)
				{
					this.destinationWorldId = w.id;
				}
				string[] array = this.destinationWorldId.Split(new char[]
				{
					':'
				});
				string str = array[0].Trim();
				string item = "1";
				if (array.Length > 1)
				{
					item = array[1].Trim();
				}
				List<string> list = new List<string>();
				list.Add(item);
				UnityEngine.Debug.Log("ResetGameFlow: GetBestInstance");
				ApiWorld.WorldInstance bestInstance = w.GetBestInstance(list, ModerationManager.Instance.IsBannedFromPublicOnly(APIUser.CurrentUser.id));
				UnityEngine.Debug.Log("ResetGameFlow: 1fallbackInstance=" + bestInstance.idWithTags);
				this.destinationWorldId = str + ":" + bestInstance.idWithTags;
				UnityEngine.Debug.Log("ResetGameFlow: chose new hub instance = " + this.destinationWorldId);
				this.FinishReset();
			}, delegate(string errmsg)
			{
				this.FinishReset();
			});
			return;
		}
		this.FinishReset();
	}

	// Token: 0x06006208 RID: 25096 RVA: 0x00229369 File Offset: 0x00227769
	private void FinishReset()
	{
		this._resetGameFlowPending = false;
		base.StartCoroutine(this.ManageGameFlow(new VRCFlowManager.ResetGameFlags[]
		{
			VRCFlowManager.ResetGameFlags.ShowUI,
			VRCFlowManager.ResetGameFlags.SkipLogin
		}));
	}

	// Token: 0x06006209 RID: 25097 RVA: 0x00229389 File Offset: 0x00227789
	public void QueueAlertForNextReset(string title, string message, int timeout)
	{
		this._hasQueuedAlert = true;
		this._queuedAlertTitle = title;
		this._queuedAlertMessage = message;
		this._queuedAlertTimeout = timeout;
	}

	// Token: 0x0600620A RID: 25098 RVA: 0x002293A7 File Offset: 0x002277A7
	public bool CanEnterRoom()
	{
		return !this.IsEnteringRoom() && string.IsNullOrEmpty(this.destinationWorldId);
	}

	// Token: 0x0600620B RID: 25099 RVA: 0x002293C2 File Offset: 0x002277C2
	public bool IsEnteringRoom()
	{
		return !this._hasEnteredRoom;
	}

	// Token: 0x0600620C RID: 25100 RVA: 0x002293CD File Offset: 0x002277CD
	public void EnterRoom(string roomId, Action<string> onError = null)
	{
		this.EnterRoom(roomId, true, onError);
	}

	// Token: 0x0600620D RID: 25101 RVA: 0x002293D8 File Offset: 0x002277D8
	public void EnterRoom(string roomId, bool goToDefaultOnError, Action<string> onError = null)
	{
		string instanceId = null;
		string worldId = roomId;
		string[] array = roomId.Split(new char[]
		{
			':'
		});
		if (array.Length > 1)
		{
			worldId = array[0];
			instanceId = array[1];
		}
		this.EnterWorld(worldId, instanceId, goToDefaultOnError, onError);
	}

	// Token: 0x0600620E RID: 25102 RVA: 0x00229416 File Offset: 0x00227816
	public void EnterWorld(string worldId, string instanceId, Action<string> onError = null)
	{
		this.EnterWorld(worldId, instanceId, true, onError);
	}

	// Token: 0x0600620F RID: 25103 RVA: 0x00229422 File Offset: 0x00227822
	public void EnterWorld(string worldId, string instanceId, bool goToDefaultOnError, Action<string> onError = null)
	{
		if (string.IsNullOrEmpty(worldId))
		{
			UnityEngine.Debug.LogError("EnterWorld called with an invalid world id.");
			return;
		}
		this.EnterWorldChecked(worldId, instanceId, goToDefaultOnError, onError);
	}

	// Token: 0x06006210 RID: 25104
	private void EnterWorldChecked(string worldId, string instanceId, bool goToDefaultOnError, Action<string> onError = null)
	{
		if (false && ModerationManager.Instance.IsKickedFromWorld(APIUser.CurrentUser.id, worldId, instanceId))
		{
			string text = "You have been kicked from this world for an hour.";
			UnityEngine.Debug.Log(text);
			if (onError != null)
			{
				onError(text);
			}
			if (goToDefaultOnError)
			{
				this.GoToDefaultWorld();
			}
		}
		else if (false && ModerationManager.Instance.IsPublicOnlyBannedFromWorld(APIUser.CurrentUser.id, worldId, instanceId))
		{
			string publicOnlyBannedUserMessage = ModerationManager.Instance.GetPublicOnlyBannedUserMessage();
			UnityEngine.Debug.Log(publicOnlyBannedUserMessage);
			if (onError != null)
			{
				onError(publicOnlyBannedUserMessage);
			}
			if (goToDefaultOnError)
			{
				this.GoToDefaultWorld();
			}
		}
		else
		{
			this.destinationWorldId = worldId + ":" + instanceId;
		}
	}

	// Token: 0x06006211 RID: 25105 RVA: 0x002294FC File Offset: 0x002278FC
	public void EnterNewWorldInstance(string worldId, Action onSuccess = null, Action<string> onError = null)
	{
		if (string.IsNullOrEmpty(worldId))
		{
			string text = "EnterNewWorldInstance called with an invalid world id.";
			UnityEngine.Debug.LogError(text);
			if (onError != null)
			{
				onError(text);
			}
			return;
		}
		string newInstanceTags = string.Empty;
		if (ModerationManager.Instance.IsBannedFromPublicOnly(APIUser.CurrentUser.id))
		{
			newInstanceTags = ApiWorld.WorldInstance.BuildAccessTags(ApiWorld.WorldInstance.AccessType.FriendsOnly, APIUser.CurrentUser.id);
		}
		this.EnterNewWorldInstanceWithTags(worldId, newInstanceTags, onSuccess, onError);
	}

	// Token: 0x06006212 RID: 25106 RVA: 0x00229568 File Offset: 0x00227968
	public void EnterNewWorldInstanceWithTags(string worldId, string newInstanceTags, Action onSuccess = null, Action<string> onError = null)
	{
		if (string.IsNullOrEmpty(worldId))
		{
			string text = "EnterNewWorldInstanceWithTags called with an invalid world id.";
			UnityEngine.Debug.LogError(text);
			if (onError != null)
			{
				onError(text);
			}
			return;
		}
		ApiWorld.Fetch(worldId, delegate(ApiWorld w)
		{
			string idWithTags = w.GetNewInstance(newInstanceTags).idWithTags;
			bool wasError = false;
			this.EnterWorld(worldId, idWithTags, delegate(string errorStr)
			{
				wasError = true;
				if (onError != null)
				{
					onError(errorStr);
				}
			});
			if (!wasError && onSuccess != null)
			{
				onSuccess();
			}
		}, delegate(string errorStr)
		{
			if (onError != null)
			{
				onError(errorStr);
			}
		});
	}

	// Token: 0x06006213 RID: 25107 RVA: 0x002295F6 File Offset: 0x002279F6
	public void GoHome()
	{
		UnityEngine.Debug.Log("Going Home");
		this.destinationWorldId = RemoteConfig.GetString("homeWorldId");
	}

	// Token: 0x06006214 RID: 25108 RVA: 0x00229612 File Offset: 0x00227A12
	public void GoTutorial()
	{
		UnityEngine.Debug.Log("Going to Tutorial");
		this.destinationWorldId = RemoteConfig.GetString("tutorialWorldId");
	}

	// Token: 0x06006215 RID: 25109 RVA: 0x0022962E File Offset: 0x00227A2E
	public void GoHub()
	{
		UnityEngine.Debug.Log("Going to Hub");
		this.destinationWorldId = RemoteConfig.GetString("hubWorldId");
	}

	// Token: 0x06006216 RID: 25110 RVA: 0x0022964A File Offset: 0x00227A4A
	public void GoPublicBanWorld()
	{
		UnityEngine.Debug.Log("Going to Public Ban World");
		this.destinationWorldId = RemoteConfig.GetString("timeOutWorldId");
		this.destinationWorldDefaultAccessType = ApiWorld.WorldInstance.AccessType.FriendsOnly;
	}

	// Token: 0x06006217 RID: 25111 RVA: 0x0022966D File Offset: 0x00227A6D
	public void GoErrorWorld()
	{
		UnityEngine.Debug.Log("Going to Error World");
		this.destinationWorldId = this.errorWorldId;
	}

	// Token: 0x06006218 RID: 25112 RVA: 0x00229685 File Offset: 0x00227A85
	public void GoToDefaultWorld()
	{
		if (APIUser.CurrentUser != null && ModerationManager.Instance.IsBannedFromPublicOnly(APIUser.CurrentUser.id))
		{
			this.GoPublicBanWorld();
		}
		else
		{
			this.GoHub();
		}
	}

	// Token: 0x06006219 RID: 25113 RVA: 0x002296BB File Offset: 0x00227ABB
	public string GetDefaultWorldId()
	{
		if (APIUser.CurrentUser != null && ModerationManager.Instance.IsBannedFromPublicOnly(APIUser.CurrentUser.id))
		{
			return RemoteConfig.GetString("timeOutWorldId");
		}
		return RemoteConfig.GetString("hubWorldId");
	}

	// Token: 0x0600621A RID: 25114 RVA: 0x002296F5 File Offset: 0x00227AF5
	public bool CanCancelRoomLoad()
	{
		return !this.IsGoingToDefaultWorld() && !this.IsGoingToHome() && !this.IsGoingToTutorial();
	}

	// Token: 0x0600621B RID: 25115 RVA: 0x0022971C File Offset: 0x00227B1C
	public bool IsInWorld(string worldId)
	{
		return !string.IsNullOrEmpty(worldId) && (this.destinationWorldId == worldId || (RoomManager.currentRoom != null && RoomManager.currentRoom.id == worldId));
	}

	// Token: 0x0600621C RID: 25116 RVA: 0x00229768 File Offset: 0x00227B68
	public bool IsInPublicBanWorld()
	{
		return this.IsInWorld(RemoteConfig.GetString("timeOutWorldId"));
	}

	// Token: 0x0600621D RID: 25117 RVA: 0x0022977A File Offset: 0x00227B7A
	public bool IsInHubWorld()
	{
		return this.IsInWorld(RemoteConfig.GetString("hubWorldId"));
	}

	// Token: 0x0600621E RID: 25118 RVA: 0x0022978C File Offset: 0x00227B8C
	public bool IsInDefaultWorld()
	{
		return this.IsInHubWorld() || this.IsInPublicBanWorld();
	}

	// Token: 0x0600621F RID: 25119 RVA: 0x002297A2 File Offset: 0x00227BA2
	private bool IsGoingToDefaultWorld()
	{
		return this.IsGoingToHub() || this.IsGoingToPublicBanWorld();
	}

	// Token: 0x06006220 RID: 25120 RVA: 0x002297B8 File Offset: 0x00227BB8
	private bool IsGoingToHub()
	{
		return this.destinationWorldId == RemoteConfig.GetString("hubWorldId");
	}

	// Token: 0x06006221 RID: 25121 RVA: 0x002297CF File Offset: 0x00227BCF
	private bool IsGoingToHome()
	{
		return this.destinationWorldId == RemoteConfig.GetString("homeWorldId");
	}

	// Token: 0x06006222 RID: 25122 RVA: 0x002297E6 File Offset: 0x00227BE6
	private bool IsGoingToTutorial()
	{
		return this.destinationWorldId == RemoteConfig.GetString("tutorialWorldId");
	}

	// Token: 0x06006223 RID: 25123 RVA: 0x002297FD File Offset: 0x00227BFD
	private bool IsGoingToPublicBanWorld()
	{
		return this.destinationWorldId == RemoteConfig.GetString("timeOutWorldId");
	}

	// Token: 0x06006224 RID: 25124 RVA: 0x00229814 File Offset: 0x00227C14
	private bool IsGoingToErrorWorld()
	{
		return this.destinationWorldId == this.errorWorldId;
	}

	// Token: 0x06006225 RID: 25125 RVA: 0x00229828 File Offset: 0x00227C28
	private IEnumerator TransitionToRoom(bool initialFade = true)
	{
		UnityEngine.Debug.Log("<color=yellow>Transitioning to room</color>");
		this._hasEnteredRoom = false;
		ApiModel.CleanExpiredReponseCache();
		bool waitingForRemoteConfigRefresh = false;
		if (Time.realtimeSinceStartup - this._timeRemoteConfigLastRefreshed >= 3600f)
		{
			UnityEngine.Debug.Log("<color=yellow>Refreshing RemoteConfig</color>");
			waitingForRemoteConfigRefresh = true;
			RemoteConfig.Init(true, delegate
			{
				UnityEngine.Debug.Log("<color=yellow>RemoteConfig refresh succeeded</color>");
				this._timeRemoteConfigLastRefreshed = Time.realtimeSinceStartup;
				waitingForRemoteConfigRefresh = false;
			}, delegate
			{
				UnityEngine.Debug.Log("<color=yellow>Failed to refresh RemoteConfig!</color>");
				waitingForRemoteConfigRefresh = false;
			});
		}
		if (initialFade)
		{
			bool fading = true;
			VRCUiManager.Instance.CloseUi(false);
			VRCUiManager.Instance.FadeTo("SpaceFade", 0f, null);
			VRCUiManager.Instance.FadeTo("BlackFade", 1f, delegate
			{
				fading = false;
			});
			while (fading)
			{
				yield return null;
			}
		}
		this.startTime = Time.timeSinceLevelLoad;
		float transitionStart = Time.realtimeSinceStartup;
		Coroutine reconnectToPhoton = null;
		if (!VRCFlowNetworkManager.Instance.isConnected)
		{
			UnityEngine.Debug.LogError("Lost connection to Photon, trying to auto-reconnect: - state is " + PhotonNetwork.connectionStateDetailed.ToString());
			reconnectToPhoton = base.StartCoroutine(this.TryReconnectToPhoton());
		}
		if (waitingForRemoteConfigRefresh)
		{
			UnityEngine.Debug.Log("<color=yellow>Waiting for RemoteConfig refresh...</color>");
		}
		while (waitingForRemoteConfigRefresh)
		{
			yield return null;
		}
		string instanceId = null;
		string worldId = this.destinationWorldId;
		bool isTestWorld = false;
		if (!worldId.StartsWith("local:"))
		{
			string[] array = worldId.Split(new char[]
			{
				':'
			});
			if (array.Length > 1)
			{
				worldId = array[0];
				instanceId = array[1];
			}
		}
		else
		{
			isTestWorld = true;
		}
		if (!this.IsGoingToErrorWorld() && VRCFlowManager.Instance.CommandLine.DebugFlags.Contains(VRCFlowCommandLine.DebugFlag.SimulateRoomLoadError))
		{
			UnityEngine.Debug.LogError("<color=red>Faking room load error...</color>");
			VRCFlowManager.ResetGameFlags[] array2 = new VRCFlowManager.ResetGameFlags[2];
			array2[0] = VRCFlowManager.ResetGameFlags.ClearDestinationWorld;
			this.ResetGameFlow(array2);
		}
		float apiFetchWorldStart = Time.realtimeSinceStartup;
		ApiWorld destinationWorld = null;
		ApiWorld.Fetch(worldId, delegate(ApiWorld world)
		{
			destinationWorld = world;
		}, delegate(string msg)
		{
			UnityEngine.Debug.LogError("Couldn't fetch world " + worldId + " - " + msg);
			global::Analytics.Send(ApiAnalyticEvent.EventType.error, "WorldLoadFailed: Couldn't fetch world " + worldId + " - " + msg, null, null);
			VRCFlowManager t = this;
			VRCFlowManager.ResetGameFlags[] array4 = new VRCFlowManager.ResetGameFlags[3];
			array4[0] = VRCFlowManager.ResetGameFlags.ClearDestinationWorld;
			array4[1] = VRCFlowManager.ResetGameFlags.UploadErrorLog;
			t.ResetGameFlow(array4);
		});
		if (reconnectToPhoton != null)
		{
			UnityEngine.Debug.Log("<color=yellow>Waiting for TryReconnectToPhoton()</color>");
			yield return reconnectToPhoton;
			UnityEngine.Debug.Log("<color=yellow>Spent " + (Time.realtimeSinceStartup - transitionStart).ToString() + "s reconnecting.</color>");
		}
		if (RoomManager.inRoom)
		{
			float leaveRoomStart = Time.realtimeSinceStartup;
			UnityEngine.Debug.Log("<color=yellow>Leaving the current room</color>");
			try
			{
				PhotonNetwork.SendOutgoingCommands();
				RoomManager.LeaveRoom();
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Something went wrong leaving current room:\n" + ex.ToString() + "\n" + ex.StackTrace);
				this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
				{
					VRCFlowManager.ResetGameFlags.ShowUI,
					VRCFlowManager.ResetGameFlags.ClearDestinationWorld,
					VRCFlowManager.ResetGameFlags.UploadErrorLog
				});
				yield break;
			}
			float inRoomStart = Time.realtimeSinceStartup;
			while (RoomManager.inRoom && Time.realtimeSinceStartup - inRoomStart < 10f)
			{
				RoomManager.LeaveRoom();
				float lastExitTime = Time.realtimeSinceStartup;
				while (RoomManager.inRoom && Time.realtimeSinceStartup - lastExitTime < 1f)
				{
					yield return null;
				}
			}
			if (RoomManager.inRoom)
			{
				UnityEngine.Debug.LogError("Failed to leave room.");
				this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
				{
					VRCFlowManager.ResetGameFlags.ShowUI,
					VRCFlowManager.ResetGameFlags.ClearDestinationWorld,
					VRCFlowManager.ResetGameFlags.UploadErrorLog
				});
				yield break;
			}
			UnityEngine.Debug.Log("<color=yellow>Waiting for isReadyToEnter()</color>");
			float readyToEnterStart = Time.realtimeSinceStartup;
			while (!RoomManager.isReadyToEnter() && Time.realtimeSinceStartup - readyToEnterStart < 10f)
			{
				if (!PhotonNetwork.connected)
				{
					UnityEngine.Debug.LogError("Lost connection to Photon: - state is " + PhotonNetwork.connectionStateDetailed.ToString());
					this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[1]);
					yield break;
				}
				yield return null;
			}
			UnityEngine.Debug.Log("<color=yellow>Waited " + (Time.realtimeSinceStartup - readyToEnterStart).ToString() + "s to enter.</color>");
			if (!RoomManager.isReadyToEnter())
			{
				UnityEngine.Debug.LogError("Timed out waiting to be ready to enter rooms.");
				global::Analytics.Send(ApiAnalyticEvent.EventType.error, "WorldLoadFailed: Timed out waiting to be ready to enter rooms. worldId - " + worldId, null, null);
				this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
				{
					VRCFlowManager.ResetGameFlags.ShowUI,
					VRCFlowManager.ResetGameFlags.ClearDestinationWorld,
					VRCFlowManager.ResetGameFlags.UploadErrorLog
				});
				yield break;
			}
			UnityEngine.Debug.Log("<color=yellow>Spent " + (Time.realtimeSinceStartup - leaveRoomStart).ToString() + "s leaving room.</color>");
		}
		this.ResetCameraAttributes();
		UnityEngine.Debug.Log("<color=yellow>Unloading scenes</color>");
		RoomManager.isTestRoom = isTestWorld;
		SceneManager.LoadScene("Custom");
		UnityEngine.Debug.LogFormat("<color=yellow>Waiting for ApiWorld.Fetch() for {0}</color>", new object[]
		{
			worldId
		});
		while (destinationWorld == null)
		{
			yield return null;
		}
		if (string.IsNullOrEmpty(instanceId) && this.destinationWorldDefaultAccessType != ApiWorld.WorldInstance.AccessType.Public)
		{
			instanceId = destinationWorld.GetNewInstance(ApiWorld.WorldInstance.BuildAccessTags(this.destinationWorldDefaultAccessType, APIUser.CurrentUser.id)).idWithTags;
		}
		this.analyticDict.Clear();
		this.analyticDict["worldId"] = destinationWorld.id;
		this.analyticDict["worldName"] = destinationWorld.name;
		AnalyticsResult ar = UnityEngine.Analytics.Analytics.CustomEvent("enterWorld", this.analyticDict);
		UnityEngine.Debug.Log(ar);
		try
		{
			if (AssetBundleDownloadManager.Instance != null)
			{
				AssetBundleDownloadManager.Instance.DestroyOldAssets();
			}
			UnityEngine.Object.Destroy(this.vrcObjectsInstance);
		}
		catch (Exception ex2)
		{
			UnityEngine.Debug.LogError("Something went wrong clearing old assets:\n" + ex2.ToString() + "\n" + ex2.StackTrace);
			this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
			{
				VRCFlowManager.ResetGameFlags.ShowUI,
				VRCFlowManager.ResetGameFlags.ClearDestinationWorld,
				VRCFlowManager.ResetGameFlags.UploadErrorLog
			});
			yield break;
		}
		UnityEngine.Debug.Log("<color=yellow>ApiWorld took " + (Time.realtimeSinceStartup - apiFetchWorldStart).ToString() + "s to fetch.</color>");
		RoomManager.SetWorldIdForMetadata(destinationWorld.id);
		RoomManager.FetchMetadata();
		float roomDownloadStart = Time.realtimeSinceStartup;
		AssetBundleDownload roomDownload = null;
		CustomScenes loader = null;
		bool wasLoadError = false;
		LoadErrorReason errorReason = LoadErrorReason.Unknown;
		bool exitPressed = false;
		try
		{
			VRCTrackingManager.ResetTrackingToOrigin();
			if (VRCUiManager.Instance != null && initialFade)
			{
				VRCUiManager.Instance.FadeTo("SpaceFade", 1f, null);
				VRCUiManager.Instance.FadeTo("BlackFade", 0f, null);
				VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Popups/LoadingPopup");
			}
			if (!this.IsGoingToDefaultWorld())
			{
				VRCUiPageLoading.ExitCallback b = delegate
				{
					exitPressed = true;
					UnityEngine.Debug.Log("Room load was cancelled, returning to hub..");
				};
				VRCUiPageLoading.OnExitPressed = null;
				VRCUiPageLoading.OnExitPressed = (VRCUiPageLoading.ExitCallback)Delegate.Combine(VRCUiPageLoading.OnExitPressed, b);
			}
			USpeaker.MuteAll = true;
			loader = new CustomScenes();
			CustomScenes customScenes = loader;
			customScenes.OnSceneCreated = (CustomScenes.SceneCreationCallback)Delegate.Combine(customScenes.OnSceneCreated, new CustomScenes.SceneCreationCallback(delegate(AssetBundleDownload download)
			{
				roomDownload = download;
			}));
			CustomScenes customScenes2 = loader;
			customScenes2.OnDownloadError = (CustomScenes.DownloadErrorCallback)Delegate.Combine(customScenes2.OnDownloadError, new CustomScenes.DownloadErrorCallback(delegate(string dlerror, LoadErrorReason reason)
			{
				UnityEngine.Debug.LogError("LoadCustomEnvironment returned error: " + dlerror);
				wasLoadError = true;
				errorReason = reason;
				global::Analytics.Send(ApiAnalyticEvent.EventType.error, "WorldLoadFailed: " + errorReason.ToString() + " - worldId " + worldId, null, null);
			}));
			if (!loader.LoadCustomEnvironment(destinationWorld))
			{
				wasLoadError = true;
				errorReason = LoadErrorReason.InvalidURL;
				global::Analytics.Send(ApiAnalyticEvent.EventType.error, "WorldLoadFailed: " + errorReason.ToString() + " - worldId " + worldId, null, null);
			}
		}
		catch (Exception ex3)
		{
			UnityEngine.Debug.LogError("Something went wrong loading destination room:\n" + ex3.ToString() + "\n" + ex3.StackTrace);
			global::Analytics.Send(ApiAnalyticEvent.EventType.error, "WorldLoadFailed: Something went wrong loading destination room:\n" + ex3.ToString() + " - worldId " + worldId, null, null);
			this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
			{
				VRCFlowManager.ResetGameFlags.ShowUI,
				VRCFlowManager.ResetGameFlags.ClearDestinationWorld,
				VRCFlowManager.ResetGameFlags.UploadErrorLog
			});
			yield break;
		}
		while (roomDownload == null)
		{
			if (wasLoadError)
			{
				if (errorReason != LoadErrorReason.AssetBundleCorrupt || this.commandLine.CommandLineOperation != VRCFlowCommandLine.Operation.CreateRoom)
				{
					this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
					{
						VRCFlowManager.ResetGameFlags.SkipLogin,
						VRCFlowManager.ResetGameFlags.ClearDestinationWorld
					});
					yield break;
				}
				VRCUiPopupManager.Instance.ShowStandardPopup("Error", "The room could not be loaded. Please make sure you are using the latest VRChat client and SDK version. You can select which VRChat client to use in Unity, under 'VRChat SDK -> Settings -> VRChat Client'.", "Exit VRChat", delegate
				{
					Application.Quit();
				}, null);
				global::Analytics.Send(ApiAnalyticEvent.EventType.error, "WorldLoadFailed: Local SDK room test bundle failed to load, maybe built on incompatible unity version", null, null);
				for (;;)
				{
					yield return null;
				}
			}
			else
			{
				if (exitPressed)
				{
					loader.CancelLoad(destinationWorld);
					this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
					{
						VRCFlowManager.ResetGameFlags.SkipLogin,
						VRCFlowManager.ResetGameFlags.ClearDestinationWorld
					});
					yield break;
				}
				VRCUiPageLoading.Progress = loader.Progress;
				if (loader.GetDownloadStalledTime() > 120f)
				{
					UnityEngine.Debug.LogError("Room download stalled and timed out after " + 120f + ", may have lost connection. Resetting.");
					global::Analytics.Send(ApiAnalyticEvent.EventType.error, "WorldLoadFailed: download stalled and timed out.  - worldId " + worldId, null, null);
					loader.CancelLoad(destinationWorld);
					yield return new WaitForSeconds(1f);
					if (!loader.LoadCustomEnvironment(destinationWorld))
					{
						this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
						{
							VRCFlowManager.ResetGameFlags.SkipLogin,
							VRCFlowManager.ResetGameFlags.ClearDestinationWorld
						});
					}
					VRCUiPageLoading.Progress = 0f;
				}
				yield return null;
			}
		}
		UnityEngine.Debug.Log("<color=yellow>Room download took " + (Time.realtimeSinceStartup - roomDownloadStart).ToString() + "s</color>");
		UnityEngine.Debug.Log("<color=yellow>Waiting for world metadata load to finish.. </color>");
		while (RoomManager.metadata == null)
		{
			yield return null;
		}
		float blackoutTimeStart = Time.realtimeSinceStartup;
		bool blackFading = true;
		VRCUiPageLoading.Progress = 1f;
		VRCUiManager.Instance.FadeTo("SpaceFade", 0f, null);
		VRCUiManager.Instance.FadeTo("BlackFade", 1f, delegate
		{
			blackFading = false;
		});
		VRCUiManager.Instance.CloseUi(false);
		while (blackFading)
		{
			yield return null;
		}
		UnityEngine.Debug.Log("<color=yellow>Instantiating downloaded scene</color>");
		float loadLevelStart = Time.realtimeSinceStartup;
		bool sceneSuccess = false;
		yield return loader.InstantiateDownloadedScene(roomDownload, 120f, delegate
		{
			sceneSuccess = true;
		}, delegate(string errorMsg)
		{
			global::Analytics.Send(ApiAnalyticEvent.EventType.error, "WorldLoadFailed: instantiating scene failed: " + errorMsg + " - worldId " + worldId, null, null);
		});
		roomDownload.Unload();
		UnityEngine.Debug.Log("<color=yellow>Spent " + (Time.realtimeSinceStartup - loadLevelStart) + "s loading scene</color>");
		if (!sceneSuccess)
		{
			this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
			{
				VRCFlowManager.ResetGameFlags.ShowUI,
				VRCFlowManager.ResetGameFlags.ClearDestinationWorld,
				VRCFlowManager.ResetGameFlags.UploadErrorLog
			});
			yield break;
		}
		VRCTrackingManager.ResetTrackingToOrigin();
		UnityEngine.Debug.Log("<color=yellow>Instantiating VRC_OBJECTS</color>");
		float vrcObjectStart = Time.realtimeSinceStartup;
		try
		{
			GameObject asset = Resources.Load("VRC_OBJECTS") as GameObject;
			this.vrcObjectsInstance = (AssetManagement.Instantiate(asset) as GameObject);
			this.vrcObjectsInstance.name = "VRC_OBJECTS";
			this.vrcObjectsInstance.GetComponentInChildren<PhotonView>().viewID = 1;
			UnityEngine.Object.DontDestroyOnLoad(this.vrcObjectsInstance);
			GameObject gameObject = new GameObject("SceneEventHandlerAndInstantiator", new Type[]
			{
				typeof(ObjectInstantiator),
				typeof(VRC_EventHandler),
				typeof(PhotonView)
			});
			VRC.Network.SceneInstantiator = gameObject.GetComponent<ObjectInstantiator>();
			VRC.Network.SceneEventHandler = gameObject.GetComponent<VRC_EventHandler>();
			gameObject.GetComponentInChildren<PhotonView>().viewID = 2;
		}
		catch (Exception ex4)
		{
			UnityEngine.Debug.LogError("Something went wrong creating VRC_OBJECTS root:\n" + ex4.ToString() + "\n" + ex4.StackTrace);
			this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
			{
				VRCFlowManager.ResetGameFlags.ShowUI,
				VRCFlowManager.ResetGameFlags.ClearDestinationWorld,
				VRCFlowManager.ResetGameFlags.UploadErrorLog
			});
			yield break;
		}
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		PhotonBandwidthGui.Reset();
		VRC_EventLog.Instance.Reset();
		UnityEngine.Debug.Log("<color=yellow>Spent " + (Time.realtimeSinceStartup - vrcObjectStart).ToString() + "s initializing VRC Objects and level</color>");
		float assignStart = Time.realtimeSinceStartup;
		try
		{
			UnityEngine.Debug.Log("<color=yellow>Doing some house-keeping.</color>");
			foreach (AudioSource audioSource in UnityEngine.Object.FindObjectsOfType<AudioSource>())
			{
				audioSource.priority = Mathf.Clamp(audioSource.priority, 10, 255);
			}
			UnityEngine.Debug.Log("<color=yellow>Assigning IDs to scene</color>");
			VRC.Network.AssignNetworkIDsToScene();
		}
		catch (Exception ex5)
		{
			UnityEngine.Debug.LogError("Something went wrong allocating ids:\n" + ex5.ToString() + "\n" + ex5.StackTrace);
			this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
			{
				VRCFlowManager.ResetGameFlags.ShowUI,
				VRCFlowManager.ResetGameFlags.ClearDestinationWorld,
				VRCFlowManager.ResetGameFlags.UploadErrorLog
			});
			yield break;
		}
		UnityEngine.Debug.Log("<color=yellow>Spent " + (Time.realtimeSinceStartup - assignStart).ToString() + "s assigning network IDs.</color>");
		UnityEngine.Debug.Log("<color=yellow>Processing scene objects</color>");
		float processStart = Time.realtimeSinceStartup;
		bool processSuccess = false;
		yield return loader.ProcessSceneObjects(delegate
		{
			processSuccess = true;
		});
		if (!processSuccess)
		{
			this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
			{
				VRCFlowManager.ResetGameFlags.ShowUI,
				VRCFlowManager.ResetGameFlags.ClearDestinationWorld,
				VRCFlowManager.ResetGameFlags.UploadErrorLog
			});
			yield break;
		}
		UnityEngine.Debug.Log("<color=yellow>Spent " + (Time.realtimeSinceStartup - processStart).ToString() + "s processing scene objects.</color>");
		UnityEngine.Debug.Log("<color=yellow>Fixing materials</color>");
		float fixMaterialsStart = Time.realtimeSinceStartup;
		AssetManagement.FixMaterialsInLevel();
		UnityEngine.Debug.Log("<color=yellow>Spent " + (Time.realtimeSinceStartup - fixMaterialsStart).ToString() + "s fixing materials.</color>");
		UnityEngine.Debug.Log("<color=yellow>Finalizing scene</color>");
		float finalizeStart = Time.realtimeSinceStartup;
		try
		{
			loader.FinalizeScene();
			USpeaker.MuteAll = false;
		}
		catch (Exception ex6)
		{
			UnityEngine.Debug.LogError("Something went wrong finalizing the scene:\n" + ex6.ToString() + "\n" + ex6.StackTrace);
			this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
			{
				VRCFlowManager.ResetGameFlags.ShowUI,
				VRCFlowManager.ResetGameFlags.ClearDestinationWorld,
				VRCFlowManager.ResetGameFlags.UploadErrorLog
			});
			yield break;
		}
		UnityEngine.Debug.Log("<color=yellow>Spent " + (Time.realtimeSinceStartup - finalizeStart).ToString() + "s finalizing scene.</color>");
		UnityEngine.Debug.Log("<color=yellow>Entering world</color>");
		float worldEnterStart = Time.realtimeSinceStartup;
		if (!RoomManager.EnterWorld(destinationWorld, instanceId))
		{
			if (!PhotonNetwork.connectedAndReady)
			{
				UnityEngine.Debug.LogError("Lost connection, state is now " + PhotonNetwork.connectionStateDetailed.ToString());
				this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
				{
					VRCFlowManager.ResetGameFlags.ShowUI,
					VRCFlowManager.ResetGameFlags.SkipLogin,
					VRCFlowManager.ResetGameFlags.TryAlternateInstance
				});
			}
			else
			{
				this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
				{
					VRCFlowManager.ResetGameFlags.ShowUI,
					VRCFlowManager.ResetGameFlags.ClearDestinationWorld
				});
			}
			yield break;
		}
		UnityEngine.Debug.Log("<color=yellow>Spent " + (Time.realtimeSinceStartup - worldEnterStart).ToString() + "s entering world.</color>");
		UnityEngine.Debug.Log("<color=yellow>Waiting for Photon to enter room</color>");
		float photonRoomStartTime = Time.realtimeSinceStartup;
		while ((!PhotonNetwork.inRoom || PhotonNetwork.masterClient == null) && Time.realtimeSinceStartup - photonRoomStartTime < 10f)
		{
			if (VRCFlowNetworkManager.Instance == null || !PhotonNetwork.connected)
			{
				UnityEngine.Debug.LogError("Lost connection to Photon: - state is " + PhotonNetwork.connectionStateDetailed.ToString());
				this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[1]);
				yield break;
			}
			yield return null;
		}
		UnityEngine.Debug.Log("<color=yellow>Spent " + (Time.realtimeSinceStartup - photonRoomStartTime).ToString() + "s waiting to enter room.</color>");
		if (!PhotonNetwork.inRoom || PhotonNetwork.masterClient == null)
		{
			UnityEngine.Debug.LogError("Could not enter the photon room in time.");
			global::Analytics.Send(ApiAnalyticEvent.EventType.error, "WorldLoadFailed: Could not enter the photon room in time.  - worldId " + worldId, null, null);
			this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
			{
				VRCFlowManager.ResetGameFlags.ShowUI,
				VRCFlowManager.ResetGameFlags.TryAlternateInstance
			});
			yield break;
		}
		UnityEngine.Debug.Log("<color=yellow>Time in blackness: " + (Time.realtimeSinceStartup - blackoutTimeStart).ToString() + "s</color>");
		blackFading = true;
		VRCUiManager.Instance.FadeTo("BlackFade", 0f, delegate
		{
			blackFading = false;
		});
		UnityEngine.Debug.Log("<color=yellow>Spawning players</color>");
		float spawnPlayerStart = Time.realtimeSinceStartup;
		try
		{
			SpawnManager.Instance.SpawnPlayerUsingOrder();
		}
		catch (Exception ex7)
		{
			UnityEngine.Debug.LogError("Something went wrong while spawning:\n" + ex7.ToString() + "\n" + ex7.StackTrace);
			this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
			{
				VRCFlowManager.ResetGameFlags.ShowUI,
				VRCFlowManager.ResetGameFlags.ClearDestinationWorld,
				VRCFlowManager.ResetGameFlags.UploadErrorLog
			});
			yield break;
		}
		UnityEngine.Debug.Log("<color=yellow>Spent " + (Time.realtimeSinceStartup - spawnPlayerStart).ToString() + "s spawning players.</color>");
		VRCAudioManager.EnableAllAudio(true);
		UnityEngine.Debug.Log("<color=yellow>Waiting for event log to finish initial load</color>");
		float waitForInitialLoadStart = Time.realtimeSinceStartup;
		int attempts = 0;
		while (attempts < 3 && !VRC_EventLog.IsFinishedInitialLoad)
		{
			float eventLoadStart = Time.realtimeSinceStartup;
			VRC_EventLog.Instance.RequestPastEvents();
			while (!VRC_EventLog.IsFinishedInitialLoad && Time.realtimeSinceStartup - eventLoadStart < 5f)
			{
				yield return null;
			}
			attempts++;
		}
		if (VRC_EventLog.IsFinishedInitialLoad)
		{
			UnityEngine.Debug.Log("<color=yellow>Waited " + (Time.realtimeSinceStartup - waitForInitialLoadStart).ToString() + "s for event log.</color>");
			VRC_EventLog.Instance.ShouldProcessEvents = true;
			UnityEngine.Debug.Log("<color=yellow>Waiting for network to settle</color>");
			float settleStart = Time.realtimeSinceStartup;
			while (!VRC.Network.IsNetworkSettled && Time.realtimeSinceStartup - settleStart < 10f)
			{
				yield return null;
			}
			if (!VRC.Network.IsNetworkSettled)
			{
				UnityEngine.Debug.LogError("Network did not settle.");
				global::Analytics.Send(ApiAnalyticEvent.EventType.error, "WorldLoadFailed: Network did not settle.  - worldId " + worldId, null, null);
				this.ResetGameFlow(new VRCFlowManager.ResetGameFlags[]
				{
					VRCFlowManager.ResetGameFlags.ShowUI,
					VRCFlowManager.ResetGameFlags.ClearDestinationWorld,
					VRCFlowManager.ResetGameFlags.UploadErrorLog
				});
				yield break;
			}
			UnityEngine.Debug.Log("<color=yellow>Readying all objects</color>");
			float readyStart = Time.realtimeSinceStartup;
			IEnumerable<GameObject> allObjs = UnityEngine.Object.FindObjectsOfType<GameObject>();
			foreach (GameObject obj in from o in allObjs
			where o != null
			select o)
			{
				VRC.Network.SceneEventHandler.StartCoroutine(VRC.Network.CheckReady(obj));
			}
			UnityEngine.Debug.Log("<color=yellow>Room transition time: " + (Time.realtimeSinceStartup - transitionStart).ToString() + "s</color>");
			UnityEngine.Debug.Log("<color=yellow>Spent " + (Time.realtimeSinceStartup - readyStart).ToString() + "s readying objects.</color>");
			UnityEngine.Debug.Log("<color=yellow>I am " + ((!PhotonNetwork.isMasterClient) ? "*NOT* MASTER" : "MASTER") + "</color>");
			UnityEngine.Debug.Log("<color=yellow>Photon ID: " + PhotonNetwork.player.ID + "</color>");
			VRCUiManager.Instance.CloseUi(false);
			while (blackFading)
			{
				yield return null;
			}
			this._hasEnteredRoom = true;
			if (this.onEnteredWorld != null)
			{
				this.onEnteredWorld();
			}
			this.roomReloadRetryCount = 0;
			this.destinationWorldId = null;
			this.destinationWorldDefaultAccessType = ApiWorld.WorldInstance.AccessType.Public;
		}
		else
		{
			UnityEngine.Debug.LogError("Master is not sending any events! Moving to a new instance.");
			global::Analytics.Send(ApiAnalyticEvent.EventType.error, "WorldLoadFailed: Master is not sending any events! Moving to a new instance.  - worldId " + worldId, null, null);
			ApiWorld currentWorld = RoomManager.currentRoom;
			ApiWorld.WorldInstance currentInstance = (currentWorld == null) ? null : new ApiWorld.WorldInstance(currentWorld.currentInstanceIdWithTags, 0);
			if (currentWorld != null && currentInstance.GetAccessType() == ApiWorld.WorldInstance.AccessType.Public)
			{
				string idWithTags = currentWorld.GetNewInstance(string.Empty).idWithTags;
				VRCFlowManager.Instance.EnterWorld(currentWorld.id, idWithTags, null);
			}
			else
			{
				bool waiting = true;
				this.EnterNewWorldInstance(RemoteConfig.GetString("hubWorldId"), delegate
				{
					waiting = false;
				}, delegate(string errorStr)
				{
					waiting = false;
				});
				while (waiting)
				{
					yield return null;
				}
			}
		}
		yield break;
	}

	// Token: 0x06006226 RID: 25126 RVA: 0x0022984C File Offset: 0x00227C4C
	private IEnumerator TryReconnectToPhoton()
	{
		yield return base.StartCoroutine(this.BeginConnectionToPhoton(true));
		yield return base.StartCoroutine(this.WaitForPhotonConnection());
		yield break;
	}

	// Token: 0x06006227 RID: 25127 RVA: 0x00229868 File Offset: 0x00227C68
	private void ResetCameraAttributes()
	{
		Camera[] componentsInChildren = VRCVrCamera.GetInstance().GetComponentsInChildren<Camera>(true);
		foreach (Camera camera in componentsInChildren)
		{
			if (!camera.orthographic)
			{
				camera.farClipPlane = 1000f;
				camera.clearFlags = CameraClearFlags.Skybox;
				camera.backgroundColor = Color.black;
				camera.renderingPath = RenderingPath.Forward;
				PostEffectManager.RemovePostEffects(camera.gameObject);
			}
		}
	}

	// Token: 0x06006228 RID: 25128 RVA: 0x002298DC File Offset: 0x00227CDC
	private void SendFlowStepAnalytic(string flowStep)
	{
		this.analyticDict.Clear();
		this.analyticDict["flowStep"] = flowStep;
		AnalyticsResult analyticsResult = UnityEngine.Analytics.Analytics.CustomEvent("flowStep", this.analyticDict);
		UnityEngine.Debug.Log(analyticsResult);
	}

	// Token: 0x04004787 RID: 18311
	public float ConnectionRecoveryWaitStep = 3f;

	// Token: 0x04004788 RID: 18312
	private static VRCFlowManager mInstance;

	// Token: 0x04004789 RID: 18313
	private float startTime;

	// Token: 0x0400478A RID: 18314
	private GameObject vrcObjectsInstance;

	// Token: 0x0400478B RID: 18315
	private VRCFlowCommandLine commandLine;

	// Token: 0x0400478C RID: 18316
	private bool lastShowUIRequested = true;

	// Token: 0x0400478D RID: 18317
	private bool lastSkipLogin;

	// Token: 0x0400478E RID: 18318
	private bool isInConnectErrorState;

	// Token: 0x0400478F RID: 18319
	private bool hasPerformedUpdateCheck;

	// Token: 0x04004790 RID: 18320
	public float ConnectionFlowResetTime = 30f;

	// Token: 0x04004791 RID: 18321
	private bool _allowThirdPartyLogin = true;

	// Token: 0x04004792 RID: 18322
	private bool _hasEnteredRoom;

	// Token: 0x04004793 RID: 18323
	private float _timeRemoteConfigLastRefreshed = -1f;

	// Token: 0x04004794 RID: 18324
	private const float kRemoteConfigRefreshTime = 3600f;

	// Token: 0x04004795 RID: 18325
	private string _destinationWorldId;

	// Token: 0x04004796 RID: 18326
	private ApiWorld.WorldInstance.AccessType destinationWorldDefaultAccessType;

	// Token: 0x04004798 RID: 18328
	private Dictionary<string, object> analyticDict = new Dictionary<string, object>();

	// Token: 0x04004799 RID: 18329
	private string errorWorldId;

	// Token: 0x0400479A RID: 18330
	private const int kRoomReloadMaxRetries = 2;

	// Token: 0x0400479B RID: 18331
	private int roomReloadRetryCount;

	// Token: 0x0400479C RID: 18332
	private bool _hasQueuedAlert;

	// Token: 0x0400479D RID: 18333
	private string _queuedAlertTitle;

	// Token: 0x0400479E RID: 18334
	private string _queuedAlertMessage;

	// Token: 0x0400479F RID: 18335
	private int _queuedAlertTimeout;

	// Token: 0x040047A0 RID: 18336
	private bool _resetGameFlowPending;

	// Token: 0x040047A1 RID: 18337
	private bool skipTOSChangedDialog;

	// Token: 0x02000C58 RID: 3160
	public enum ResetGameFlags
	{
		// Token: 0x040047A4 RID: 18340
		ShowUI,
		// Token: 0x040047A5 RID: 18341
		ClearDestinationWorld,
		// Token: 0x040047A6 RID: 18342
		TryAlternateInstance,
		// Token: 0x040047A7 RID: 18343
		UploadErrorLog,
		// Token: 0x040047A8 RID: 18344
		IsConnectionRetry,
		// Token: 0x040047A9 RID: 18345
		SkipLogin,
		// Token: 0x040047AA RID: 18346
		GoToDefaultWorld
	}
}
