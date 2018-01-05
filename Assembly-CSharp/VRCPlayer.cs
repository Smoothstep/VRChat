using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ExitGames.Client.Photon;
using Photon;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000B3A RID: 2874
public class VRCPlayer : PunBehaviour
{
    // Token: 0x17000CB7 RID: 3255
    // (get) Token: 0x060057F1 RID: 22513 RVA: 0x001E7309 File Offset: 0x001E5709
    const short nil = 0;
	public short Ping
	{
		get
		{
			return (!(this.playerNet == null)) ? this.playerNet.Ping : nil;
		}
	}

	// Token: 0x17000CB8 RID: 3256
	// (get) Token: 0x060057F2 RID: 22514 RVA: 0x001E732D File Offset: 0x001E572D
	public short PingVariance
	{
		get
		{
			return (!(this.playerNet == null)) ? this.playerNet.PingVariance : nil;
		}
	}

	// Token: 0x17000CB9 RID: 3257
	// (get) Token: 0x060057F3 RID: 22515 RVA: 0x001E7351 File Offset: 0x001E5751
	public float ConnectionQuality
	{
		get
		{
			return (!(this.playerNet == null)) ? this.playerNet.ConnectionQuality : 0f;
		}
	}

	// Token: 0x17000CBA RID: 3258
	// (get) Token: 0x060057F4 RID: 22516 RVA: 0x001E7379 File Offset: 0x001E5779
	public float ConnectionDisparity
	{
		get
		{
			return (!(this.playerNet == null)) ? this.playerNet.ConnectionDisparity : 0f;
		}
	}

	// Token: 0x17000CBB RID: 3259
	// (get) Token: 0x060057F5 RID: 22517 RVA: 0x001E73A1 File Offset: 0x001E57A1
	public VRC.Player player
	{
		get
		{
			if (this._player == null)
			{
				this._player = base.GetComponent<VRC.Player>();
			}
			return this._player;
		}
	}

	// Token: 0x17000CBC RID: 3260
	// (get) Token: 0x060057F6 RID: 22518 RVA: 0x001E73C6 File Offset: 0x001E57C6
	public PlayerNet playerNet
	{
		get
		{
			if (this._playerNet == null)
			{
				this._playerNet = base.GetComponent<PlayerNet>();
			}
			return this._playerNet;
		}
	}

	// Token: 0x17000CBD RID: 3261
	// (get) Token: 0x060057F7 RID: 22519 RVA: 0x001E73EB File Offset: 0x001E57EB
	public USpeaker uSpeaker
	{
		get
		{
			return this._uSpeaker;
		}
	}

	// Token: 0x17000CBE RID: 3262
	// (get) Token: 0x060057F8 RID: 22520 RVA: 0x001E73F3 File Offset: 0x001E57F3
	public bool initialized
	{
		get
		{
			return this._initialized;
		}
	}

	// Token: 0x17000CBF RID: 3263
	// (get) Token: 0x060057F9 RID: 22521 RVA: 0x001E73FB File Offset: 0x001E57FB
	public bool ready
	{
		get
		{
			return this.initialized && (this.isAvatarLoaded || !ModerationManager.Instance.IsBlockedEitherWay(this.player.userId));
		}
	}

	// Token: 0x14000071 RID: 113
	// (add) Token: 0x060057FA RID: 22522 RVA: 0x001E7434 File Offset: 0x001E5834
	// (remove) Token: 0x060057FB RID: 22523 RVA: 0x001E746C File Offset: 0x001E586C
	public event VRCPlayer.OnAvatarIsReady onAvatarIsReady;

	// Token: 0x17000CC0 RID: 3264
	// (get) Token: 0x060057FC RID: 22524 RVA: 0x001E74A2 File Offset: 0x001E58A2
	public VRC_PlayerApi apiPlayer
	{
		get
		{
			if (this._apiPlayer == null)
			{
				this._apiPlayer = base.GetComponent<VRC_PlayerApi>();
			}
			return this._apiPlayer;
		}
	}

	// Token: 0x17000CC1 RID: 3265
	// (get) Token: 0x060057FD RID: 22525 RVA: 0x001E74C8 File Offset: 0x001E58C8
	public List<VRCHandGrasper> hands
	{
		get
		{
			if (this._hands == null)
			{
				this._hands = new List<VRCHandGrasper>();
				this._hands.AddRange(base.gameObject.GetComponents<VRCHandGrasper>());
				this._hands.AddRange(base.gameObject.GetComponentsInChildren<VRCHandGrasper>());
				if (this._hands.Count == 0)
				{
					this._hands = null;
				}
			}
			return this._hands;
		}
	}

	// Token: 0x17000CC2 RID: 3266
	// (get) Token: 0x060057FE RID: 22526 RVA: 0x001E7534 File Offset: 0x001E5934
	public bool isLocal
	{
		get
		{
			return VRCPlayer.Instance == this;
		}
	}

	// Token: 0x17000CC3 RID: 3267
	// (get) Token: 0x060057FF RID: 22527 RVA: 0x001E7541 File Offset: 0x001E5941
	public bool isAvatarLoaded
	{
		get
		{
			return this._avatarLoaded;
		}
	}

	// Token: 0x17000CC4 RID: 3268
	// (get) Token: 0x06005800 RID: 22528 RVA: 0x001E7549 File Offset: 0x001E5949
	public static string DefaultModTag
	{
		get
		{
			return (!PlayerPrefs.HasKey("defaultModTag")) ? "mod" : PlayerPrefs.GetString("defaultModTag");
		}
	}

	// Token: 0x17000CC5 RID: 3269
	// (get) Token: 0x06005801 RID: 22529 RVA: 0x001E756E File Offset: 0x001E596E
	// (set) Token: 0x06005802 RID: 22530 RVA: 0x001E75A5 File Offset: 0x001E59A5
	public string modTag
	{
		get
		{
			if (this.isLocal)
			{
				return (!PlayerPrefs.HasKey("modTag")) ? VRCPlayer.DefaultModTag : PlayerPrefs.GetString("modTag");
			}
			return this._modTag;
		}
		set
		{
			this._modTag = value;
			if (this.isLocal)
			{
				PlayerPrefs.SetString("modTag", value);
				User.SetNetworkProperties();
			}
		}
	}

	// Token: 0x17000CC6 RID: 3270
	// (get) Token: 0x06005803 RID: 22531 RVA: 0x001E75CC File Offset: 0x001E59CC
	public static string LocalModTag
	{
		get
		{
			return (!(VRCPlayer.Instance != null)) ? ((!PlayerPrefs.HasKey("modTag")) ? VRCPlayer.DefaultModTag : PlayerPrefs.GetString("modTag")) : VRCPlayer.Instance.modTag;
		}
	}

	// Token: 0x17000CC7 RID: 3271
	// (get) Token: 0x06005804 RID: 22532 RVA: 0x001E761C File Offset: 0x001E5A1C
	// (set) Token: 0x06005805 RID: 22533 RVA: 0x001E76E8 File Offset: 0x001E5AE8
	public bool isInvisible
	{
		get
		{
			if (this._isInvisible == null)
			{
				this._isInvisible = new bool?(!this.isLocal || (PlayerPrefs.HasKey("isInvisible") && PlayerPrefs.GetInt("isInvisible") > 0));
			}
			bool flag = this._isInvisible.Value && (this.player == null || this.player.user == null || this.player.isModerator);
			if (Networking.LocalPlayer != null)
			{
				return flag || !this.GameVisibleTo(Networking.LocalPlayer.playerId);
			}
			return flag;
		}
		set
		{
			this._isInvisible = new bool?(value);
			if (this.isLocal)
			{
				PlayerPrefs.SetInt("isInvisible", (!value) ? 0 : 1);
				User.SetNetworkProperties();
			}
		}
	}

	// Token: 0x17000CC8 RID: 3272
	// (get) Token: 0x06005806 RID: 22534 RVA: 0x001E7720 File Offset: 0x001E5B20
	public static bool LocalIsInvisible
	{
		get
		{
			return (!(VRCPlayer.Instance != null)) ? (PlayerPrefs.HasKey("isInvisible") && PlayerPrefs.GetInt("isInvisible") > 0) : VRCPlayer.Instance.isInvisible;
		}
	}

	// Token: 0x06005807 RID: 22535 RVA: 0x001E7770 File Offset: 0x001E5B70
	private void Awake()
	{
		if (!PlayerPrefs.HasKey("defaultModTag"))
		{
			PlayerPrefs.SetString("defaultModTag", "mod");
		}
		this.SpawnPosition = base.transform.position;
		this.SpawnRotation = base.transform.rotation;
		this.animationController = base.GetComponentInChildren<VRC_AnimationController>();
		this.animControlManager = base.GetComponentInChildren<AnimatorControllerManager>();
		this.cameraMount = base.transform.Find("CameraMount");
		this.playerSelector = base.GetComponentInChildren<PlayerSelector>(true);
		this.namePlateColor = this.defaultNamePlateColor;
		this.UpdateDefaultNameplateVisibilityFromMetadata();
		this.RestoreNamePlateVisibility();
		this._uSpeaker = base.GetComponentInChildren<USpeaker>();
		this._emojiGen = base.GetComponentInChildren<EmojiGenerator>();
		this._emotePlayer = base.GetComponentInChildren<EmotePlayer>();
		this._avatarSwitcher = base.GetComponentInChildren<VRCAvatarManager>();
		this._lastAvatarKind = VRCAvatarManager.AvatarKind.Undefined;
		VRCAvatarManager avatarSwitcher = this._avatarSwitcher;
		avatarSwitcher.OnAvatarCreated = (VRCAvatarManager.AvatarCreationCallback)Delegate.Combine(avatarSwitcher.OnAvatarCreated, new VRCAvatarManager.AvatarCreationCallback(this.AvatarIsReady));
		VRCAvatarManager avatarSwitcher2 = this._avatarSwitcher;
		avatarSwitcher2.OnLastAvatarLoaded = (VRCAvatarManager.AvatarCreationCallback)Delegate.Combine(avatarSwitcher2.OnLastAvatarLoaded, new VRCAvatarManager.AvatarCreationCallback(this.AvatarFinishedLoading));
		this.theCombatSystem = VRC_CombatSystem.GetInstance();
		base.gameObject.AddComponent<VRCTriggerRelay>();
		RoomManager.metadataUpdateCallbacksInternal = (VRC_MetadataListener.MetadataCallback)Delegate.Combine(RoomManager.metadataUpdateCallbacksInternal, new VRC_MetadataListener.MetadataCallback(this.OnWorldMetadataUpdated));
	}

	// Token: 0x06005808 RID: 22536 RVA: 0x001E78CC File Offset: 0x001E5CCC
	public void UpdateModerations()
	{
		if (ModerationManager.Instance.IsKicked(this.userId))
		{
			VRCFlowManager.Instance.GoHub();
			return;
		}
		if (this.isLocal)
		{
			return;
		}
		this.canHear = !ModerationManager.Instance.IsMutedByUser(this.userId);
		this.canSpeak = !ModerationManager.Instance.IsMuted(this.userId);
		if (this.player.isModerator)
		{
			this.canHear = (this.canSpeak = true);
		}
		this.SetupVIP(this.player.isVIP);
		this.player.UpdateModerations();
		this._isBlockedEitherWay = ModerationManager.Instance.IsBlockedEitherWay(this.userId);
		if (QuickMenu.Instance != null)
		{
			QuickMenu.Instance.RefreshMenuState();
		}
	}

	// Token: 0x06005809 RID: 22537 RVA: 0x001E79A4 File Offset: 0x001E5DA4
	public IEnumerator Start()
	{
		if (!base.photonView.isMine)
		{
			this.RemoveLocalInputs();
		}
		if (base.photonView.isMine)
		{
			VRCPlayer.Instance = this;
			this.defaultMute = VRCInputManager.GetSetting(VRCInputManager.InputSetting.DefaultMute);
			Tools.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("PlayerLocal"), LayerMask.NameToLayer("UiMenu"));
			this.namePlate.transform.parent.gameObject.SetActive(false);
		}
		this._avatarSwitcher.localPlayer = this.isLocal;
		if (this.isLocal)
		{
			this.SetupUserSettings();
			this.SetupCameraAttributes();
			base.transform.Find("Profile/NameTag").gameObject.SetActive(false);
		}
		this.SetupRemotePhotonPlayer(base.photonView.owner, base.photonView.owner.CustomProperties);
		if (this.theCombatSystem != null)
		{
			this.SetupDestructiblePlayer();
		}
		if (!this.isLocal)
		{
			ModerationManager.Instance.moderationsChanged += this.UpdateModerations;
		}
		yield return new WaitUntil(() => NetworkManager.Instance != null);
		NetworkManager.Instance.OnPhotonPlayerPropertiesChangedEvent += this.OnPhotonPlayerPropertiesChanged;
		yield return new WaitUntil(() => this.player.user != null);
		this._initialized = true;
		this.AssignNetworkIDs();
		float readyStart = Time.realtimeSinceStartup;
		while (!VRC.Network.IsObjectReady(base.gameObject))
		{
			if (Time.realtimeSinceStartup - readyStart > 25f)
			{
				Debug.LogError(string.Format("Could not ready player {0}", base.gameObject.name));
				yield break;
			}
			yield return null;
		}
		Debug.Log("Successfully readied player " + base.gameObject.name);
		yield break;
	}

	// Token: 0x0600580A RID: 22538 RVA: 0x001E79BF File Offset: 0x001E5DBF
	private void OnNetworkReady()
	{
		if (base.photonView.isMine)
		{
			Debug.Log("Local VRCPlayer OnNetworkReady");
		}
	}

	// Token: 0x0600580B RID: 22539 RVA: 0x001E79DC File Offset: 0x001E5DDC
	private void Update()
	{
		if (this.isLocal)
		{
			if (Input.GetKeyDown(KeyCode.N) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
			{
				VRCPlayer.NamePlatesVisible = !VRCPlayer.NamePlatesVisible;
			}
			if (Input.GetKeyDown(KeyCode.A) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && this._avatarSwitcher != null)
			{
				this._avatarSwitcher.SwitchToFallbackAvatar(1f, new VRCAvatarManager.AvatarCreationCallback(this.AvatarFinishedLoading));
			}
		}
		this.isSpeaking = this._uSpeaker.IsTalking;
		if (this.player != null && this.player.isValidUser && ((VRCPlayer.NamePlatesVisible && this.namePlateEnable) || this.ShouldForceOnNameplateForMod()) && !this._isBlockedEitherWay && !this.isInvisible)
		{
			bool flag = this.player.isModerator && !string.IsNullOrEmpty(this.modTag);
			this.SetupVIP(flag && this.player.isVIP);
			this.namePlate.gameObject.SetActive(true);
			if (flag)
			{
				this.vipPlate.gameObject.SetActive(true);
				this.vipPlate.text = this.modTag;
			}
			else
			{
				this.vipPlate.gameObject.SetActive(false);
			}
			this.statusPlate.gameObject.SetActive(false);
			if (APIUser.IsFriendsWith(this.userId))
			{
				this.friendSprite.gameObject.SetActive(true);
			}
			else
			{
				this.friendSprite.gameObject.SetActive(false);
			}
			this.speakingSprite.enabled = false;
			this.mutedSprite.enabled = false;
			this.speakingMutedSprite.enabled = false;
			this.listeningSprite.enabled = false;
			this.listeningMutedSprite.enabled = false;
			Color color;
			if (this.canSpeak)
			{
				this.namePlate.image.color = Color.white;
				color = this.namePlateColor;
			}
			else
			{
				this.namePlate.image.color = this.mutedNamePlateColor;
				color = ((!this.isNamePlateColorOverriden) ? this.mutedNamePlateColor : this.namePlateColor);
			}
			if (this.isSpeaking)
			{
				this.namePlate.image.sprite = this.namePlateTalkSprite;
				this.vipPlate.image.sprite = this.namePlateTalkSprite;
				this.statusPlate.image.sprite = this.namePlateTalkSprite;
				if (this.canSpeak || flag || VRCPlayer.Instance.player.isModerator)
				{
					this.speakingSprite.enabled = true;
					this.speakingSprite.color = Color.white;
				}
				else
				{
					this.speakingMutedSprite.enabled = true;
					this.speakingMutedSprite.color = this.mutedNamePlateColor;
				}
			}
			else
			{
				this.namePlate.image.sprite = this.namePlateSilentSprite;
				this.vipPlate.image.sprite = this.namePlateSilentSprite;
				this.statusPlate.image.sprite = this.namePlateSilentSprite;
				if (!this.canSpeak && !flag && !VRCPlayer.Instance.player.isModerator)
				{
					this.mutedSprite.enabled = true;
					this.mutedSprite.color = this.mutedNamePlateColor;
				}
			}
			if (!this.canHear && !flag && !VRCPlayer.Instance.player.isModerator)
			{
				this.listeningMutedSprite.enabled = true;
				this.listeningMutedSprite.color = ((!this.canSpeak && !flag && !VRCPlayer.Instance.player.isModerator) ? this.mutedNamePlateColor : Color.white);
			}
			this.namePlate.image.color = color;
			this.vipPlate.image.color = color;
			this.statusPlate.image.color = color;
		}
		else
		{
			this.namePlate.gameObject.SetActive(false);
			this.vipPlate.gameObject.SetActive(false);
			this.statusPlate.gameObject.SetActive(false);
			this.speakingSprite.enabled = false;
			this.mutedSprite.enabled = false;
			this.speakingMutedSprite.enabled = false;
			this.listeningSprite.enabled = false;
			this.listeningMutedSprite.enabled = false;
			this.friendSprite.gameObject.SetActive(false);
		}
		if (this.isLocal && base.transform.position.y < this.RespawnHeightY)
		{
			SpawnManager.Instance.RespawnPlayerUsingOrder(this);
		}
		if (this._emojiCoolTimer >= 0f)
		{
			this._emojiCoolTimer -= Time.deltaTime;
		}
		if (this._emoteCoolTimer >= 0f)
		{
			this._emoteCoolTimer -= Time.deltaTime;
		}
	}

	// Token: 0x0600580C RID: 22540 RVA: 0x001E7F3C File Offset: 0x001E633C
	private bool ShouldForceOnNameplateForMod()
	{
		return (VRC.Player.Instance == null || VRC.Player.Instance.isModerator) && !this._areNameplatesVisibleByDefault && !VRCPlayer.NamePlatesVisible && !this.isNamePlateVisibilityOverriden;
	}

	// Token: 0x0600580D RID: 22541 RVA: 0x001E7F8B File Offset: 0x001E638B
	public int LocalPhotonViewId()
	{
		return base.photonView.viewID;
	}

	// Token: 0x0600580E RID: 22542 RVA: 0x001E7F98 File Offset: 0x001E6398
	public void SetupUserSettings()
	{
		this.SetVoiceToggleMode((!VRCInputManager.talkToggle) ? 0 : 1);
		if (HMDManager.IsHmdDetected())
		{
			USpeaker.LocalGain = VRCInputManager.micLevelVr;
		}
		else
		{
			USpeaker.LocalGain = VRCInputManager.micLevelDesk;
		}
	}

	// Token: 0x0600580F RID: 22543 RVA: 0x001E7FD4 File Offset: 0x001E63D4
	private void RequestVoiceSetupFromAllPlayersIfNotSetup()
	{
		Debug.Log("RequestVoiceSetupFromAllPlayersIfNotSetup");
		List<VRC_PlayerApi> allPlayers = VRC_PlayerApi.AllPlayers;
		foreach (VRC_PlayerApi vrc_PlayerApi in allPlayers)
		{
			VRCPlayer component = vrc_PlayerApi.GetComponent<VRCPlayer>();
			if (component != null && component != this)
			{
				USpeaker uSpeaker = component.uSpeaker;
				if (uSpeaker != null && !uSpeaker.isInitialized)
				{
					Debug.LogError("Voice has not been setup for " + vrc_PlayerApi.playerId + ". Requesting setup.");
					this.RequestVoiceSetupFromPlayer(component.player);
				}
			}
		}
	}

	// Token: 0x06005810 RID: 22544 RVA: 0x001E80A0 File Offset: 0x001E64A0
	private void RequestVoiceSetupFromPlayer(VRC.Player p)
	{
		if (p != null)
		{
			VRC.Network.RPC(p, base.gameObject, "SendVoiceSetupToPlayer", new object[]
			{
				VRC.Network.LocalPlayer.playerApi.playerId
			});
			Debug.Log(string.Concat(new object[]
			{
				"RequestVoiceSetupFromPlayer ",
				p.name,
				" (",
				p.playerApi.playerId,
				")"
			}));
		}
		else
		{
			Debug.LogError("RequestVoiceSetupFromPlayer failed bc player is null");
		}
	}

	// Token: 0x06005811 RID: 22545 RVA: 0x001E813C File Offset: 0x001E653C
	private void SendVoiceSetupToPlayer(int playerId)
	{
		VRC_PlayerApi playerById = VRC_PlayerApi.GetPlayerById(playerId);
		if (playerById != null)
		{
			this._uSpeaker.SendSettingsToPlayer(playerById.GetComponent<VRC.Player>());
			Debug.Log(string.Concat(new object[]
			{
				"SendVoiceSetupToPlayer ",
				playerById.name,
				" (",
				playerById.playerId,
				")"
			}));
		}
		else
		{
			Debug.LogError("SendVoiceSetupToPlayer(" + playerId + ") failed bc player is null");
		}
	}

	// Token: 0x06005812 RID: 22546 RVA: 0x001E81CC File Offset: 0x001E65CC
	private void SetupVIP(bool isVIP)
	{
		if (isVIP)
		{
			this.defaultNamePlateColor = this.vipNamePlateColor;
			if (!this.isNamePlateColorOverriden)
			{
				this.namePlateColor = this.defaultNamePlateColor;
			}
		}
		else
		{
			this.defaultNamePlateColor = Color.green;
			if (!this.isNamePlateColorOverriden)
			{
				this.namePlateColor = this.defaultNamePlateColor;
			}
		}
	}

	// Token: 0x06005813 RID: 22547 RVA: 0x001E822C File Offset: 0x001E662C
	private void SetupDestructiblePlayer()
	{
		VRC_DestructiblePlayer vrc_DestructiblePlayer = base.gameObject.AddComponent<VRC_DestructiblePlayer>();
		vrc_DestructiblePlayer.maxHealth = this.theCombatSystem.maxPlayerHealth;
	}

	// Token: 0x06005814 RID: 22548 RVA: 0x001E8258 File Offset: 0x001E6658
	private void SetVoiceToggleMode(int mode)
	{
		DefaultTalkController componentInChildren = base.GetComponentInChildren<DefaultTalkController>();
		if (componentInChildren != null && (mode == 0 || mode == 1))
		{
			componentInChildren.ToggleMode = mode;
		}
	}

	// Token: 0x06005815 RID: 22549 RVA: 0x001E828C File Offset: 0x001E668C
	private void SetupCameraAttributes()
	{
		VRC_SceneDescriptor instance = VRC_SceneDescriptor.Instance;
		if (instance != null)
		{
			Camera[] componentsInChildren = VRCVrCamera.GetInstance().GetComponentsInChildren<Camera>(true);
			foreach (Camera camera in componentsInChildren)
			{
				if (!camera.orthographic)
				{
					if (instance.ReferenceCamera != null)
					{
						Camera component = instance.ReferenceCamera.GetComponent<Camera>();
						if (component != null)
						{
							camera.nearClipPlane = Mathf.Clamp(component.nearClipPlane, 0.01f, 0.05f);
							camera.farClipPlane = component.farClipPlane;
							camera.clearFlags = component.clearFlags;
							camera.backgroundColor = component.backgroundColor;
							camera.allowHDR = component.allowHDR;
						}
						PostEffectManager.RemovePostEffects(camera.gameObject);
						PostEffectManager.InstallPostEffects(camera.gameObject, instance.ReferenceCamera);
					}
					else
					{
						PostEffectManager.RemovePostEffects(camera.gameObject);
					}
				}
			}
			this.RespawnHeightY = instance.RespawnHeightY;
		}
	}

	// Token: 0x06005816 RID: 22550 RVA: 0x001E839C File Offset: 0x001E679C
	private void RemoveLocalInputs()
	{
		USpeaker componentInChildren = base.GetComponentInChildren<USpeaker>();
		UnityEngine.Object.Destroy(componentInChildren.gameObject.GetComponent<DefaultTalkController>());
	}

	// Token: 0x06005817 RID: 22551 RVA: 0x001E83C0 File Offset: 0x001E67C0
	private void SetupMyAvatar()
	{
		Debug.Log("Setup my avatar");
		VRC_AnimationController componentInChildren = base.GetComponentInChildren<VRC_AnimationController>();
		if (componentInChildren)
		{
			componentInChildren.Detach();
		}
	}

	// Token: 0x06005818 RID: 22552 RVA: 0x001E83F0 File Offset: 0x001E67F0
	private void AssignNetworkIDs()
	{
		this.lastUsedLocalID = 0;
		foreach (INetworkID networkID in (from c in VRC.Network.GetAllComponents<Component>(base.gameObject, true)
		where c is INetworkID
		select c).Cast<INetworkID>())
		{
			if (networkID.NetworkID <= 0)
			{
				this.lastUsedLocalID++;
				networkID.NetworkID = this.lastUsedLocalID;
			}
		}
	}

	// Token: 0x06005819 RID: 22553 RVA: 0x001E849C File Offset: 0x001E689C
	private void AvatarIsReady(GameObject avatar, VRC_AvatarDescriptor Ad, bool loaded)
	{
		this.AssignNetworkIDs();
		if (this._avatarSwitcher.currentAvatarKind == VRCAvatarManager.AvatarKind.Custom && !loaded)
		{
			return;
		}
		if (this._avatarSwitcher.currentAvatarKind != VRCAvatarManager.AvatarKind.Custom && this._lastAvatarKind == this._avatarSwitcher.currentAvatarKind)
		{
			return;
		}
		this._lastAvatarKind = this._avatarSwitcher.currentAvatarKind;
		this.avatarGameObject = avatar;
		this.avatarAnimator = avatar.GetComponent<Animator>();
		if (this.avatarAnimator != null)
		{
			this.animControlManager.Initialize(this.avatarAnimator, Ad, this.isLocal);
		}
		if (this.isLocal)
		{
			VRCTrackingManager.RefreshAvatarViewPoint();
			VRCUiManager.Instance.PlaceUi();
		}
		base.GetComponent<PlayerModManager>().Reset();
		if (this.onAvatarIsReady != null)
		{
			this.onAvatarIsReady();
		}
	}

	// Token: 0x0600581A RID: 22554 RVA: 0x001E8577 File Offset: 0x001E6977
	private void ExitRoom(string cause, string prefabName)
	{
		if (cause != null)
		{
			if (!(cause == "portal"))
			{
			}
		}
	}

	// Token: 0x0600581B RID: 22555 RVA: 0x001E859E File Offset: 0x001E699E
	public void MoveToSpawn()
	{
		VRC_StationInternal.ExitAllStations(this.player);
		base.transform.position = this.SpawnPosition;
		base.transform.rotation = this.SpawnRotation;
		base.transform.GetComponent<VRCMotionState>().Reset();
	}

	// Token: 0x0600581C RID: 22556 RVA: 0x001E85E0 File Offset: 0x001E69E0
	public void OnDestroy()
	{
		if (NetworkManager.Instance != null)
		{
			NetworkManager.Instance.OnPhotonPlayerPropertiesChangedEvent -= this.OnPhotonPlayerPropertiesChanged;
		}
		if (!this.isLocal)
		{
			ModerationManager.Instance.moderationsChanged -= this.UpdateModerations;
		}
		RoomManager.metadataUpdateCallbacksInternal = (VRC_MetadataListener.MetadataCallback)Delegate.Remove(RoomManager.metadataUpdateCallbacksInternal, new VRC_MetadataListener.MetadataCallback(this.OnWorldMetadataUpdated));
		if (VRCPlayer.Instance == this)
		{
			VRCPlayer.Instance = null;
		}
	}

	// Token: 0x0600581D RID: 22557 RVA: 0x001E866C File Offset: 0x001E6A6C
	private void SetupRemotePhotonPlayer(PhotonPlayer player, ExitGames.Client.Photon.Hashtable properties)
	{
		string a = (this.apiAvatar == null) ? string.Empty : this.apiAvatar.id;
		this._avatarLoaded = false;
		VRC.Player player2 = base.GetComponent<VRC.Player>();
		if (player2 == null)
		{
			player2 = PlayerManager.GetPlayer(player);
		}
		if (this.isLocal)
		{
			this.apiAvatar = User.CurrentUser.apiAvatar;
			this.playerName = User.CurrentUser.displayName;
			player2.userId = (properties["userId"] as string);
			this.userId = User.CurrentUser.id;
			this._inVRMode = VRCTrackingManager.IsInVRMode();
		}
		else
		{
			this.isInvisible = (properties.ContainsKey("isInvisible") && (bool)properties["isInvisible"]);
			this.modTag = ((!properties.ContainsKey("modTag")) ? string.Empty : ((string)properties["modTag"]));
			this.apiAvatar = ((!properties.ContainsKey("avatarBlueprint")) ? null : ((ApiAvatar)properties["avatarBlueprint"]));
			this.playerName = player.NickName;
			if (this.namePlate != null)
			{
				this.namePlate.text = player.NickName;
			}
			if (properties.ContainsKey("userId"))
			{
				this.userId = (properties["userId"] as string);
				player2.userId = this.userId;
			}
			if (properties.ContainsKey("defaultMute"))
			{
				this.defaultMute = (bool)properties["defaultMute"];
			}
			if (properties.ContainsKey("inVRMode"))
			{
				this._inVRMode = (bool)properties["inVRMode"];
			}
			Debug.Log(string.Concat(new object[]
			{
				"SetupRemotePhotonPlayer (",
				this.userId,
				") VRMode = ",
				this._inVRMode
			}));
		}
		if (player2 != null)
		{
			player2.vrcPlayer = this;
			if (string.IsNullOrEmpty(this.userId))
			{
				this.userId = player2.userId;
			}
			player2.user = null;
			this.SetupRemoteApiUser(player2, 25);
		}
		if (ModerationManager.Instance.IsBlockedEitherWay(this.userId))
		{
			this._avatarLoaded = false;
			this._avatarSwitcher.SetLastApiAvatar(this.apiAvatar);
		}
		else if (a != this.apiAvatar.id)
		{
			this._avatarLoaded = false;
			this.animationController.Detach();
			this._avatarSwitcher.SwitchAvatar(this.apiAvatar, 1f, new VRCAvatarManager.AvatarCreationCallback(this.AvatarFinishedLoading));
		}
		else
		{
			this._avatarLoaded = true;
		}
	}

	// Token: 0x0600581E RID: 22558 RVA: 0x001E894F File Offset: 0x001E6D4F
	private void AvatarFinishedLoading(GameObject Avatar, VRC_AvatarDescriptor Ad, bool loaded)
	{
		if (loaded)
		{
			this._avatarLoaded = true;
			if (this._avatarSwitcher.currentAvatarKind == VRCAvatarManager.AvatarKind.Custom)
			{
				this.AvatarIsReady(Avatar, Ad, true);
			}
		}
	}

	// Token: 0x0600581F RID: 22559 RVA: 0x001E8978 File Offset: 0x001E6D78
	private void SetupRemoteApiUser(VRC.Player vrcPlayer, int retry = 25)
	{
		base.StartCoroutine(this.SetupRemoteApiUserCoroutine(vrcPlayer, retry));
	}

	// Token: 0x06005820 RID: 22560 RVA: 0x001E898C File Offset: 0x001E6D8C
	private void AttemptUserMatch(VRC.Player vrcPlayer)
	{
		PlayerManager.FetchUsersInWorldInstance(delegate(List<APIUser> APIUserList)
		{
			foreach (APIUser apiuser in APIUserList)
			{
				if (vrcPlayer.userId == apiuser.id)
				{
					vrcPlayer.user = apiuser;
					break;
				}
			}
			if (vrcPlayer.user == null)
			{
				Debug.LogError("[" + this.photonView.viewID + "] SetupRemoteApiUserCoroutine: FetchUsersInWorldInstance returned success, but user id not found");
			}
		}, delegate(string failList)
		{
			Debug.LogError("Could not fetch users in world: " + failList);
		});
	}

	// Token: 0x06005821 RID: 22561 RVA: 0x001E89DC File Offset: 0x001E6DDC
	private IEnumerator SetupRemoteApiUserCoroutine(VRC.Player vrcPlayer, int retry)
	{
		if (VRCPlayer.f__mg0 == null)
		{
			VRCPlayer.f__mg0 = new Func<bool>(VRC.Network.Get_IsNetworkSettled);
		}
		yield return new WaitUntil(VRCPlayer.f__mg0);
		bool shouldRetry = false;
		int attempt = 0;
		for (;;)
		{
			if (vrcPlayer.user == null || vrcPlayer.user.developerType == null)
			{
				this.AttemptUserMatch(vrcPlayer);
			}
			if (vrcPlayer.user != null)
			{
				break;
			}
			shouldRetry = true;
			Debug.LogError(string.Concat(new object[]
			{
				"[",
				base.photonView.viewID,
				"] VRCPlayer.SetupRemoteApiUserCoroutine failed, attempt / retry ",
				attempt,
				"/",
				retry,
				" ",
				Time.time
			}));
			yield return new WaitForSeconds(5f);
			if (!shouldRetry || ++attempt >= retry)
			{
				goto IL_1CB;
			}
		}
		vrcPlayer.user.defaultMute = this.defaultMute;
		this.SetupVIP(this.player.isVIP);
		this.UpdateModerations();
		IL_1CB:
		if (attempt >= retry)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"SetupRemoteApiUser ",
				base.photonView.viewID,
				" failed all ",
				attempt,
				" attempts!!!"
			}));
		}
		yield break;
	}

	// Token: 0x06005822 RID: 22562 RVA: 0x001E8A08 File Offset: 0x001E6E08
	public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
	{
		PhotonPlayer photonPlayer = playerAndUpdatedProps[0] as PhotonPlayer;
		if (photonPlayer == base.photonView.owner)
		{
			ExitGames.Client.Photon.Hashtable properties = playerAndUpdatedProps[1] as ExitGames.Client.Photon.Hashtable;
			this.SetupRemotePhotonPlayer(photonPlayer, properties);
			this.UpdateModerations();
		}
	}

	// Token: 0x06005823 RID: 22563 RVA: 0x001E8A48 File Offset: 0x001E6E48
	public VRCHandGrasper GetHandGrasper(ControllerHand hand)
	{
		if (this.hands != null)
		{
			foreach (VRCHandGrasper vrchandGrasper in this.hands)
			{
				if ((vrchandGrasper.RightHand && hand == ControllerHand.Right) || (!vrchandGrasper.RightHand && hand == ControllerHand.Left))
				{
					return vrchandGrasper;
				}
			}
		}
		return null;
	}

	// Token: 0x06005824 RID: 22564 RVA: 0x001E8AD8 File Offset: 0x001E6ED8
	public Vector3 GetHeadFloorPos()
	{
		return new Vector3(this.cameraMount.position.x, base.transform.position.y, this.cameraMount.position.z);
	}

	// Token: 0x06005825 RID: 22565 RVA: 0x001E8B23 File Offset: 0x001E6F23
	public void SpawnEmoji(int n)
	{
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "SpawnEmojiRPC", new object[]
		{
			n
		});
	}

	// Token: 0x06005826 RID: 22566 RVA: 0x001E8B48 File Offset: 0x001E6F48
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void SpawnEmojiRPC(int n, VRC.Player sender)
	{
		if (VRC.Network.GetOwner(base.gameObject) == sender)
		{
			if (this._emojiCoolTimer > 0f)
			{
				return;
			}
			this._emojiGen.Spawn(n);
			this._emojiCoolTimer = this._emojiCooldownTime;
		}
	}

	// Token: 0x06005827 RID: 22567 RVA: 0x001E8B94 File Offset: 0x001E6F94
	public void PlayEmote(int n)
	{
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "PlayEmoteRPC", new object[]
		{
			n
		});
	}

	// Token: 0x06005828 RID: 22568 RVA: 0x001E8BB8 File Offset: 0x001E6FB8
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void PlayEmoteRPC(int n, VRC.Player sender)
	{
		if (VRC.Network.GetOwner(base.gameObject) == sender)
		{
			if (this._emoteCoolTimer > 0f)
			{
				return;
			}
			this._emotePlayer.Play(n);
			this._emoteCoolTimer = this._emoteCooldownTime;
		}
	}

	// Token: 0x06005829 RID: 22569 RVA: 0x001E8C04 File Offset: 0x001E7004
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.TargetPlayer
	})]
	private void Teleport(Vector3 teleportPos, Quaternion teleportRot, VRC_SceneDescriptor.SpawnOrientation teleportOrientation, VRC.Player sender)
	{
		if (this.player.isLocal)
		{
			LocomotionInputController component = base.GetComponent<LocomotionInputController>();
			component.Teleport(teleportPos, teleportRot, teleportOrientation);
		}
	}

	// Token: 0x0600582A RID: 22570 RVA: 0x001E8C31 File Offset: 0x001E7031
	public bool IsPlayingEmote()
	{
		return this._emotePlayer.IsPlaying();
	}

	// Token: 0x0600582B RID: 22571 RVA: 0x001E8C40 File Offset: 0x001E7040
	public void ResetIK()
	{
		if (this.animationController != null)
		{
			IkController component = this.animationController.HeadAndHandsIkController.GetComponent<IkController>();
			component.Reset(true);
		}
	}

	// Token: 0x0600582C RID: 22572 RVA: 0x001E8C76 File Offset: 0x001E7076
	public static void SettingsChanged()
	{
		if (VRCPlayer.Instance != null)
		{
			VRCPlayer.Instance.SetupUserSettings();
		}
	}

	// Token: 0x0600582D RID: 22573 RVA: 0x001E8C92 File Offset: 0x001E7092
	public void SetNamePlateVisibility(bool flag)
	{
		this.namePlateEnable = flag;
		this.isNamePlateVisibilityOverriden = true;
	}

	// Token: 0x0600582E RID: 22574 RVA: 0x001E8CA2 File Offset: 0x001E70A2
	public void RestoreNamePlateVisibility()
	{
		this.namePlateEnable = this._areNameplatesVisibleByDefault;
		this.isNamePlateVisibilityOverriden = false;
	}

	// Token: 0x0600582F RID: 22575 RVA: 0x001E8CB7 File Offset: 0x001E70B7
	public void SetNamePlateColor(Color col)
	{
		this.namePlateColor = col;
		this.isNamePlateColorOverriden = true;
	}

	// Token: 0x06005830 RID: 22576 RVA: 0x001E8CC7 File Offset: 0x001E70C7
	public void RestoreNamePlateColor()
	{
		this.namePlateColor = this.defaultNamePlateColor;
		this.isNamePlateColorOverriden = false;
	}

	// Token: 0x06005831 RID: 22577 RVA: 0x001E8CDC File Offset: 0x001E70DC
	public bool GetVRMode()
	{
		return this._inVRMode;
	}

	// Token: 0x06005832 RID: 22578 RVA: 0x001E8CE4 File Offset: 0x001E70E4
	public void ModClearRoom(int type)
	{
		Debug.Log("ModClearRoom: type " + type);
		string text = null;
		if (type == 0)
		{
			text = RemoteConfig.GetString("hubWorldId").Trim();
		}
		else if (type == 1)
		{
			text = RoomManager.currentRoom.id.Trim();
		}
		string text2 = ApiWorld.WorldInstance.ParseTagsFromIDWithTags(RoomManager.currentRoom.currentInstanceIdWithTags);
		Debug.Log("Mod warp to room: " + text + ", tags: " + text2);
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.Others, base.gameObject, "GotoRoomRPC", new object[]
		{
			text,
			text2
		});
		base.StartCoroutine(this.LeaveRoomOnceEmpty(text, text2));
	}

	// Token: 0x06005833 RID: 22579 RVA: 0x001E8D90 File Offset: 0x001E7190
	private IEnumerator LeaveRoomOnceEmpty(string worldId, string newInstanceTags)
	{
		float timeStarted = Time.realtimeSinceStartup;
		while (PhotonNetwork.playerList.Length > 1 && Time.realtimeSinceStartup - timeStarted < 10f)
		{
			yield return null;
		}
		VRCFlowManager.Instance.EnterNewWorldInstanceWithTags(worldId, newInstanceTags, null, null);
		yield break;
	}

	// Token: 0x06005834 RID: 22580 RVA: 0x001E8DB2 File Offset: 0x001E71B2
	public void SetPlayerTag(string tagName, string tagValue)
	{
		if (this._playerTags == null)
		{
			this._playerTags = new Dictionary<string, string>();
		}
		this._playerTags[tagName] = tagValue;
	}

	// Token: 0x06005835 RID: 22581 RVA: 0x001E8DD8 File Offset: 0x001E71D8
	public string GetPlayerTag(string tagName)
	{
		string result = null;
		if (this._playerTags != null && this._playerTags.ContainsKey(tagName))
		{
			result = this._playerTags[tagName];
		}
		return result;
	}

	// Token: 0x06005836 RID: 22582 RVA: 0x001E8E11 File Offset: 0x001E7211
	public void ClearPlayerTags()
	{
		if (this._playerTags != null)
		{
			this._playerTags.Clear();
		}
	}

	// Token: 0x06005837 RID: 22583 RVA: 0x001E8E2C File Offset: 0x001E722C
	public void SetInvisibleToTagged(bool invisible, string tagName, string tagValue, bool inverted)
	{
		if (this._invisibleToPlayers == null)
		{
			this._invisibleToPlayers = new HashSet<int>();
		}
		List<int> playersWithTag = PlayerManager.GetPlayersWithTag(tagName, tagValue, inverted);
		foreach (int item in playersWithTag)
		{
			if (invisible)
			{
				this._invisibleToPlayers.Add(item);
			}
			else
			{
				this._invisibleToPlayers.Remove(item);
			}
		}
	}

	// Token: 0x06005838 RID: 22584 RVA: 0x001E8EC4 File Offset: 0x001E72C4
	private bool GameVisibleTo(int playerId)
	{
		return this._invisibleToPlayers == null || !this._invisibleToPlayers.Contains(playerId);
	}

	// Token: 0x06005839 RID: 22585 RVA: 0x001E8EE5 File Offset: 0x001E72E5
	public int GetSilencedLevelToPlayer(int playerId)
	{
		return this.GameSilenceLevelTo(playerId);
	}

	// Token: 0x0600583A RID: 22586 RVA: 0x001E8EEE File Offset: 0x001E72EE
	private int GameSilenceLevelTo(int playerId)
	{
		if (this._silencedToPlayers != null && this._silencedToPlayers.ContainsKey(playerId))
		{
			return this._silencedToPlayers[playerId];
		}
		return 0;
	}

	// Token: 0x0600583B RID: 22587 RVA: 0x001E8F1A File Offset: 0x001E731A
	public void ClearInvisible()
	{
		if (this._invisibleToPlayers != null)
		{
			this._invisibleToPlayers.Clear();
		}
	}

	// Token: 0x0600583C RID: 22588 RVA: 0x001E8F34 File Offset: 0x001E7334
	public void SetSilencedToTagged(int level, string tagName, string tagValue, bool inverted)
	{
		if (this._silencedToPlayers == null)
		{
			this._silencedToPlayers = new Dictionary<int, int>();
		}
		List<int> playersWithTag = PlayerManager.GetPlayersWithTag(tagName, tagValue, inverted);
		foreach (int key in playersWithTag)
		{
			if (level > 0)
			{
				this._silencedToPlayers[key] = level;
			}
			else
			{
				this._silencedToPlayers.Remove(key);
			}
		}
	}

	// Token: 0x0600583D RID: 22589 RVA: 0x001E8FCC File Offset: 0x001E73CC
	public void ClearSilence()
	{
		if (this._silencedToPlayers != null)
		{
			this._silencedToPlayers.Clear();
		}
	}

	// Token: 0x0600583E RID: 22590 RVA: 0x001E8FE4 File Offset: 0x001E73E4
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All,
		VRC_EventHandler.VrcTargetType.Others
	})]
	private void GotoRoomRPC(string worldId, string newInstanceTags, VRC.Player instigator)
	{
		Debug.Log(string.Concat(new object[]
		{
			"GotoRoomRPC ",
			worldId,
			", tags '",
			newInstanceTags,
			"', instigator ",
			instigator.userId,
			", isMod = ",
			instigator.isModerator
		}));
		if (instigator.isModerator)
		{
			VRCFlowManager.Instance.EnterNewWorldInstanceWithTags(worldId, newInstanceTags, null, null);
		}
		else
		{
			Debug.LogError(instigator.name + " attempted to warp everyone to " + worldId);
		}
	}

	// Token: 0x0600583F RID: 22591 RVA: 0x001E9074 File Offset: 0x001E7474
	public void AlignTrackingToPlayer()
	{
		if (!this.isLocal)
		{
			return;
		}
		VRCTrackingManager.SetTrackingWorldOrigin(base.transform.position, base.transform.rotation);
		base.transform.position = VRCTrackingManager.GetWorldTrackingPosition();
		base.transform.rotation = VRCTrackingManager.GetWorldTrackingOrientation();
		if (base.GetComponent<InputStateController>() != null)
		{
			base.GetComponent<InputStateController>().ResetLastPosition();
		}
	}

	// Token: 0x06005840 RID: 22592 RVA: 0x001E90E4 File Offset: 0x001E74E4
	private void UpdateDefaultNameplateVisibilityFromMetadata()
	{
		bool areNameplatesVisibleByDefault = true;
		Dictionary<string, object> metadata = RoomManager.metadata;
		if (metadata != null)
		{
			object obj = null;
			if (metadata.TryGetValue("NameplatesVisibleByDefault", out obj) && obj is bool)
			{
				areNameplatesVisibleByDefault = (bool)obj;
			}
		}
		this._areNameplatesVisibleByDefault = areNameplatesVisibleByDefault;
	}

	// Token: 0x06005841 RID: 22593 RVA: 0x001E912C File Offset: 0x001E752C
	private void OnWorldMetadataUpdated(Dictionary<string, object> data)
	{
		this.UpdateDefaultNameplateVisibilityFromMetadata();
		this.RefreshNameplateVisibility();
	}

	// Token: 0x06005842 RID: 22594 RVA: 0x001E913A File Offset: 0x001E753A
	private void RefreshNameplateVisibility()
	{
		if (!this.isNamePlateVisibilityOverriden)
		{
			this.RestoreNamePlateVisibility();
		}
	}

	// Token: 0x04003F0D RID: 16141
	public static VRCPlayer Instance;

	// Token: 0x04003F0E RID: 16142
	private VRC.Player _player;

	// Token: 0x04003F0F RID: 16143
	private PlayerNet _playerNet;

	// Token: 0x04003F10 RID: 16144
	private static bool NamePlatesVisible = true;

	// Token: 0x04003F11 RID: 16145
	public TextMesh nameTag_old;

	// Token: 0x04003F12 RID: 16146
	public Color defaultNamePlateColor = Color.green;

	// Token: 0x04003F13 RID: 16147
	public Color vipNamePlateColor = Color.yellow;

	// Token: 0x04003F14 RID: 16148
	public Color mutedNamePlateColor = new Color(0.5f, 0.5f, 0.5f, 0.6f);

	// Token: 0x04003F15 RID: 16149
	public Sprite namePlateSilentSprite;

	// Token: 0x04003F16 RID: 16150
	public Sprite namePlateTalkSprite;

	// Token: 0x04003F17 RID: 16151
	public Image speakingSprite;

	// Token: 0x04003F18 RID: 16152
	public Image speakingMutedSprite;

	// Token: 0x04003F19 RID: 16153
	public Image mutedSprite;

	// Token: 0x04003F1A RID: 16154
	public Image listeningSprite;

	// Token: 0x04003F1B RID: 16155
	public Image listeningMutedSprite;

	// Token: 0x04003F1C RID: 16156
	public Image friendSprite;

	// Token: 0x04003F1D RID: 16157
	public VRCUiShadowPlate namePlate;

	// Token: 0x04003F1E RID: 16158
	public VRCUiShadowPlate vipPlate;

	// Token: 0x04003F1F RID: 16159
	public VRCUiShadowPlate statusPlate;

	// Token: 0x04003F20 RID: 16160
	private Color namePlateColor;

	// Token: 0x04003F21 RID: 16161
	private bool namePlateEnable;

	// Token: 0x04003F22 RID: 16162
	private bool isNamePlateVisibilityOverriden;

	// Token: 0x04003F23 RID: 16163
	private bool isNamePlateColorOverriden;

	// Token: 0x04003F24 RID: 16164
	private bool _areNameplatesVisibleByDefault = true;

	// Token: 0x04003F25 RID: 16165
	private USpeaker _uSpeaker;

	// Token: 0x04003F26 RID: 16166
	private EmojiGenerator _emojiGen;

	// Token: 0x04003F27 RID: 16167
	private EmotePlayer _emotePlayer;

	// Token: 0x04003F28 RID: 16168
	public bool isSpeaking;

	// Token: 0x04003F29 RID: 16169
	public bool canSpeak = true;

	// Token: 0x04003F2A RID: 16170
	public bool canHear = true;

	// Token: 0x04003F2B RID: 16171
	public string playerName;

	// Token: 0x04003F2C RID: 16172
	private int lastUsedLocalID;

	// Token: 0x04003F2D RID: 16173
	private VRCAvatarManager _avatarSwitcher;

	// Token: 0x04003F2E RID: 16174
	private bool _initialized;

	// Token: 0x04003F2F RID: 16175
	private Vector3 SpawnPosition;

	// Token: 0x04003F30 RID: 16176
	private Quaternion SpawnRotation;

	// Token: 0x04003F31 RID: 16177
	private float RespawnHeightY = -100f;

	// Token: 0x04003F32 RID: 16178
	private AnimatorControllerManager animControlManager;

	// Token: 0x04003F33 RID: 16179
	private Transform cameraMount;

	// Token: 0x04003F35 RID: 16181
	public GameObject avatarGameObject;

	// Token: 0x04003F36 RID: 16182
	public Animator avatarAnimator;

	// Token: 0x04003F37 RID: 16183
	public PlayerSelector playerSelector;

	// Token: 0x04003F38 RID: 16184
	private Dictionary<string, string> _playerTags;

	// Token: 0x04003F39 RID: 16185
	private HashSet<int> _invisibleToPlayers;

	// Token: 0x04003F3A RID: 16186
	private Dictionary<int, int> _silencedToPlayers;

	// Token: 0x04003F3B RID: 16187
	private VRC_PlayerApi _apiPlayer;

	// Token: 0x04003F3C RID: 16188
	private List<VRCHandGrasper> _hands;

	// Token: 0x04003F3D RID: 16189
	private bool _avatarLoaded;

	// Token: 0x04003F3E RID: 16190
	private VRCAvatarManager.AvatarKind _lastAvatarKind;

	// Token: 0x04003F3F RID: 16191
	private VRC_AnimationController animationController;

	// Token: 0x04003F40 RID: 16192
	private ApiAvatar apiAvatar;

	// Token: 0x04003F41 RID: 16193
	private VRC_CombatSystem theCombatSystem;

	// Token: 0x04003F42 RID: 16194
	public bool canPickupObjects = true;

	// Token: 0x04003F43 RID: 16195
	private string userId;

	// Token: 0x04003F44 RID: 16196
	private bool _inVRMode;

	// Token: 0x04003F45 RID: 16197
	private bool _isBlockedEitherWay;

	// Token: 0x04003F46 RID: 16198
	private bool defaultMute = true;

	// Token: 0x04003F47 RID: 16199
	private float _emojiCooldownTime = 5f;

	// Token: 0x04003F48 RID: 16200
	private float _emoteCooldownTime;

	// Token: 0x04003F49 RID: 16201
	private float _emojiCoolTimer;

	// Token: 0x04003F4A RID: 16202
	private float _emoteCoolTimer;

	// Token: 0x04003F4B RID: 16203
	private string _modTag;

	// Token: 0x04003F4C RID: 16204
	private bool? _isInvisible;

	// Token: 0x04003F4F RID: 16207
	[CompilerGenerated]
	private static Func<bool> f__mg0;

	// Token: 0x02000B3B RID: 2875
	// (Invoke) Token: 0x06005847 RID: 22599
	public delegate void OnAvatarIsReady();
}
