using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Video;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000B74 RID: 2932
public class VrcSdk2Interface : MonoBehaviour
{
	// Token: 0x06005ACE RID: 23246 RVA: 0x001FAA54 File Offset: 0x001F8E54
	private void Awake()
	{
		if (VrcSdk2Interface.exists)
		{
			Debug.LogError("Dublicate VrcSdk2Interface detected. Destroying new one.");
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			VrcSdk2Interface.exists = true;
			VrcSdk2Interface.Instance = this;
			if (VrcSdk2Interface.f__mg0 == null)
			{
				VrcSdk2Interface.f__mg0 = new VRC_AvatarPedestal.InstantiationDelegate(AssetManagement.Instantiate);
			}
			VRC_AvatarPedestal.Instantiate = VrcSdk2Interface.f__mg0;
			if (VrcSdk2Interface.f__mg1 == null)
			{
				VrcSdk2Interface.f__mg1 = new VRC_AvatarCalibrator.InstantiationDelegate(AssetManagement.Instantiate);
			}
			VRC_AvatarCalibrator.Instantiate = VrcSdk2Interface.f__mg1;
			VRC_ObjectSpawn.Initialize = new VRC_ObjectSpawn.InitializationDelegate(this.VRC_ObjectSpawn_Initialize);
			VRC_DataStorage.Initialize = new VRC_DataStorage.InitializationDelegate(this.VRC_DataStorage_Initialize);
			if (VrcSdk2Interface.f__mg2 == null)
			{
				VrcSdk2Interface.f__mg2 = new VRC_DataStorage.SerializationDelegate(VRC_DataStorageInternal.Serialize);
			}
			VRC_DataStorage.Serialize = VrcSdk2Interface.f__mg2;
			if (VrcSdk2Interface.f__mg3 == null)
			{
				VrcSdk2Interface.f__mg3 = new VRC_DataStorage.DeserializationDelegate(VRC_DataStorageInternal.Deserialize);
			}
			VRC_DataStorage.Deserialize = VrcSdk2Interface.f__mg3;
			VRC_DataStorage._Resize = delegate(VRC_DataStorage obj, int sz)
			{
				VRC_DataStorageInternal component = obj.GetComponent<VRC_DataStorageInternal>();
				if (component != null)
				{
					component.Resize(sz);
				}
			};
			VRC_AddDamage.Initialize = new VRC_AddDamage.InitializationDelegate(this.VRC_AddDamage_Initialize);
			VRC_AddHealth.Initialize = new VRC_AddHealth.InitializationDelegate(this.VRC_AddHealth_Initialize);
			VRC_PlayerMods.Initialize = new VRC_PlayerMods.InitializationDelegate(this.VRC_PlayerMods_Initialize);
			VRC_PortalMarker.Initialize = new VRC_PortalMarker.InitializationDelegate(this.VRC_PortalMarker_Initialize);
			VRC_SyncAnimation.Initialize = new VRC_SyncAnimation.InitializationDelegate(this.VRC_SyncAnimation_Initialize);
			VRC_SyncVideoPlayer.Initialize = new Action<VRC_SyncVideoPlayer>(this.VRC_SyncVideoPlayer_Initialize);
			VRC_SyncVideoPlayer._Play = new Action<VRC_SyncVideoPlayer>(this.VRC_SyncVideoPlayer_Play);
			VRC_SyncVideoPlayer._PlayIndex = new Action<VRC_SyncVideoPlayer, int>(this.VRC_SyncVideoPlayer_PlayIndex);
			VRC_SyncVideoPlayer._Stop = new Action<VRC_SyncVideoPlayer>(this.VRC_SyncVideoPlayer_Stop);
			VRC_SyncVideoPlayer._Pause = new Action<VRC_SyncVideoPlayer>(this.VRC_SyncVideoPlayer_Pause);
			VRC_SyncVideoPlayer._Next = new Action<VRC_SyncVideoPlayer>(this.VRC_SyncVideoPlayer_Next);
			VRC_SyncVideoPlayer._Previous = new Action<VRC_SyncVideoPlayer>(this.VRC_SyncVideoPlayer_Previous);
			VRC_SyncVideoPlayer._Shuffle = new Action<VRC_SyncVideoPlayer>(this.VRC_SyncVideoPlayer_Shuffle);
			VRC_SyncVideoPlayer._Clear = new Action<VRC_SyncVideoPlayer>(this.VRC_SyncVideoPlayer_Clear);
			VRC_SyncVideoPlayer._AddURL = new Action<VRC_SyncVideoPlayer, string>(this.VRC_SyncVideoPlayer_AddURL);
			VRC_SyncVideoPlayer._SpeedDown = new Action<VRC_SyncVideoPlayer>(this.VRC_SyncVideoPlayer_SpeedDown);
			VRC_SyncVideoPlayer._SpeedUp = new Action<VRC_SyncVideoPlayer>(this.VRC_SyncVideoPlayer_SpeedUp);
			VRC_Station.Initialize = new VRC_Station.InitializationDelegate(this.VRC_Station_Initialize);
			VRC_Interactable.Initialize = new VRC_Interactable.InitializationDelegate(this.VRC_Interactable_Initialize);
			VRC_WebPanel.Initialize = new VRC_WebPanel.InitializeDelegate(this.VRC_WebPanel_Initialize);
			VRC_ObjectSync.Initialize = new VRC_ObjectSync.InitializationDelegate(this.VRC_ObjectSync_Initialize);
			VRC_ObjectSync.IsLocal = new VRC_ObjectSync.IsLocalDelegate(this.VRC_ObjectSync_IsLocal);
			VRC_ObjectSync.TeleportHandler = new VRC_ObjectSync.TeleportDelegate(this.VRC_ObjectSync_TeleportHandler);
			VRC_PropController.UpdateInputs = new VRC_PropController.UpdateDelegate(this.VRC_PropController_Update);
			VRC_PropController.Initialize = new VRC_PropController.InitializeDelegate(this.VRC_PropController_Initialize);
			VRC_StationInput.UpdateInputs = new VRC_StationInput.UpdateDelegate(this.VRC_StationInput_Update);
			VRC_StationInput.Initialize = new VRC_StationInput.InitializeDelegate(this.VRC_StationInput_Initialize);
			VRC_NPCSpawn.Initialize = new VRC_NPCSpawn.InstantiationDelegate(this.VRC_NPCSpawn_Initialize);
			VRC_KeyEvents.Initialize = new VRC_KeyEvents.InitializationDelegate(this.VRC_KeyEvents_Initialize);
			VRC_TriggerColliderEventTrigger.CollisionEnter = new VRC_TriggerColliderEventTrigger.CollisionEnterDelegate(this.VRC_TriggerColliderEventTrigger_CollisionEnter);
			VRC_TriggerColliderEventTrigger.CollisionExit = new VRC_TriggerColliderEventTrigger.CollisionExitDelegate(this.VRC_TriggerColliderEventTrigger_CollisionExit);
			this.InitializeEventHandler();
			this.InitializePlayerApi();
			this.InitializeNpcApi();
			if (VrcSdk2Interface.f__mg4 == null)
			{
				VrcSdk2Interface.f__mg4 = new VRC_StationApi.StationOccupiedDelegate(VrcSdk2Interface.VRC_StationApi_IsStationOccupiedDelegate);
			}
			VRC_StationApi.IsStationOccupiedDelegate = VrcSdk2Interface.f__mg4;
			if (VrcSdk2Interface.f__mg5 == null)
			{
				VrcSdk2Interface.f__mg5 = new VRC_StationApi.StationOccupantDelegate(VrcSdk2Interface.VRC_StationApi_GatStationOccupant);
			}
			VRC_StationApi.GetStationOccupant = VrcSdk2Interface.f__mg5;
			VRC_RainObject.Initialize = new VRC_RainObject.InitializationDelegate(this.VRC_RainObject_Initialize);
			if (VrcSdk2Interface.f__mg6 == null)
			{
				VrcSdk2Interface.f__mg6 = new VRC_Pickup.AwakeDelegate(VrcSdk2Interface.VRC_Pickup_OnAwake);
			}
			VRC_Pickup.OnAwake = VrcSdk2Interface.f__mg6;
			if (VrcSdk2Interface.f__mg7 == null)
			{
				VrcSdk2Interface.f__mg7 = new VRC_Pickup.ForceDropDelegate(VrcSdk2Interface.VRC_Pickup_ForceDrop);
			}
			VRC_Pickup.ForceDrop = VrcSdk2Interface.f__mg7;
			if (VrcSdk2Interface.f__mg8 == null)
			{
				VrcSdk2Interface.f__mg8 = new VRC_Pickup.HapticEventDelegate(VrcSdk2Interface.VRC_Pickup_HapticEvent);
			}
			VRC_Pickup.HapticEvent = VrcSdk2Interface.f__mg8;
			if (VrcSdk2Interface.f__mg9 == null)
			{
				VrcSdk2Interface.f__mg9 = new VRC_Pickup.OnDestroyedDelegate(VrcSdk2Interface.VRC_Pickup_OnDestroyed);
			}
			VRC_Pickup.OnDestroyed = VrcSdk2Interface.f__mg9;
			if (VrcSdk2Interface.f__mgA == null)
			{
				VrcSdk2Interface.f__mgA = new Func<VRC_Pickup, VRC_Pickup.PickupHand>(VRCHandGrasper.GetPickupHand);
			}
			VRC_Pickup._GetPickupHand = VrcSdk2Interface.f__mgA;
			if (VrcSdk2Interface.f__mgB == null)
			{
				VrcSdk2Interface.f__mgB = new Func<VRC_Pickup, VRC_PlayerApi>(VRCHandGrasper.GetCurrentPlayer);
			}
			VRC_Pickup._GetCurrentPlayer = VrcSdk2Interface.f__mgB;
			if (VrcSdk2Interface.f__mgC == null)
			{
				VrcSdk2Interface.f__mgC = new VRC_SceneDescriptor.IntializationDelegate(VrcSdk2Interface.VRC_SceneDescriptor_Initialize);
			}
			VRC_SceneDescriptor.Initialize = VrcSdk2Interface.f__mgC;
			VRC_UiShape.GetEventCamera = new VRC_UiShape.GetEventCameraDelegate(this.VRC_UiShape_GetEventCamera);
			if (VrcSdk2Interface.f__mgD == null)
			{
				VrcSdk2Interface.f__mgD = new Tutorial.ActivateObjectLabelDelegate(TutorialManager.VRC_Tutorial_ActivateObjectLabel);
			}
			Tutorial._ActivateObjectLabel = VrcSdk2Interface.f__mgD;
			if (VrcSdk2Interface.f__mgE == null)
			{
				VrcSdk2Interface.f__mgE = new Tutorial.DeactivateObjectLabelDelegate(TutorialManager.VRC_Tutorial_DeactivateObjectLabel);
			}
			Tutorial._DeactivateObjectLabel = VrcSdk2Interface.f__mgE;
			if (VrcSdk2Interface.f__mgF == null)
			{
				VrcSdk2Interface.f__mgF = new Tutorial.ActivateControllerLabelDelegate(TutorialManager.VRC_Tutorial_ActivateControllerLabel);
			}
			Tutorial._ActivateControllerLabel = VrcSdk2Interface.f__mgF;
			if (VrcSdk2Interface.f__mg10 == null)
			{
				VrcSdk2Interface.f__mg10 = new Tutorial.DeactivateControllerLabelDelegate(TutorialManager.VRC_Tutorial_DeactivateControllerLabel);
			}
			Tutorial._DeactivateControllerLabel = VrcSdk2Interface.f__mg10;
			if (VrcSdk2Interface.f__mg11 == null)
			{
				VrcSdk2Interface.f__mg11 = new Func<bool>(VRCInputManager.IsUsingHandController);
			}
			InputManager._IsUsingHandController = VrcSdk2Interface.f__mg11;
			if (VrcSdk2Interface.f__mg12 == null)
			{
				VrcSdk2Interface.f__mg12 = new Func<VRCInputMethod>(VRCInputManager.GetLastUsedInputMethod);
			}
			InputManager._GetLastUsedInputMethod = VrcSdk2Interface.f__mg12;
			if (VrcSdk2Interface.f__mg13 == null)
			{
				VrcSdk2Interface.f__mg13 = new Func<VRCInputSetting, bool>(VRCInputManager.GetInputSetting);
			}
			InputManager._GetInputSetting = VrcSdk2Interface.f__mg13;
			if (VrcSdk2Interface.f__mg14 == null)
			{
				VrcSdk2Interface.f__mg14 = new Action<VRCInputSetting, bool>(VRCInputManager.SetInputSetting);
			}
			InputManager._SetInputSetting = VrcSdk2Interface.f__mg14;
			if (VrcSdk2Interface.f__mg15 == null)
			{
				VrcSdk2Interface.f__mg15 = new Action<Renderer, bool>(HighlightsFX.EnableObjectHighlight);
			}
			InputManager._EnableObjectHighlight = VrcSdk2Interface.f__mg15;
			VRC_Interactable.CheckValid = new VRC_Interactable.ValidDelegate(this.VRC_Interactable_CheckValid);
			VRC_PlayerApi._isMasterDelegate = ((VRC_PlayerApi api) => !(api == null) && api.GetComponent<VRC.Player>().isMaster);
			VRC_PlayerApi._isModeratorDelegate = ((VRC_PlayerApi api) => !(api == null) && api.GetComponent<VRC.Player>().isModerator);
			VRC_YouTubeSync._init = new Action<VRC_YouTubeSync>(this.VRC_Migrate_YouTubeSync);
			VRC_MetadataListener._GetCurrentMetadata = new Func<Dictionary<string, object>>(this.VRC_MetadataListener_GetCurrentMetadata);
			if (VrcSdk2Interface.f__mg16 == null)
			{
				VrcSdk2Interface.f__mg16 = new Action<VRC_Analytics.EventType, Dictionary<string, object>, Vector3?>(Analytics.VRC_Analytics_Send);
			}
			VRC_Analytics._Send = VrcSdk2Interface.f__mg16;
			this.InitializeSDKNetworkingTools();
			this.InitializeDebugTools();
		}
	}

	// Token: 0x06005ACF RID: 23247 RVA: 0x001FB0A9 File Offset: 0x001F94A9
	private void Update()
	{
		this.ProcessDeferredEvents(this.mDeferredEventsNextUpdate);
	}

	// Token: 0x06005AD0 RID: 23248 RVA: 0x001FB0B7 File Offset: 0x001F94B7
	private void FixedUpdate()
	{
		this.ProcessDeferredEvents(this.mDeferredEventsNextFixedUpdate);
	}

	// Token: 0x06005AD1 RID: 23249 RVA: 0x001FB0C5 File Offset: 0x001F94C5
	private void OnDestroy()
	{
		VrcSdk2Interface.exists = false;
		VrcSdk2Interface.Instance = null;
		VrcSdk2Interface.activeScene = null;
	}

	// Token: 0x06005AD2 RID: 23250 RVA: 0x001FB0DC File Offset: 0x001F94DC
	private void Start()
	{
		VrcSdk2Interface.inAxisHorizontal = VRCInputManager.FindInput("Horizontal");
		VrcSdk2Interface.inAxisVertical = VRCInputManager.FindInput("Vertical");
		VrcSdk2Interface.inAxisLookHorizontal = VRCInputManager.FindInput("LookHorizontal");
		VrcSdk2Interface.inAxisLookVertical = VRCInputManager.FindInput("LookVertical");
		VrcSdk2Interface.inUseLeft = VRCInputManager.FindInput("UseLeft");
		VrcSdk2Interface.inUseRight = VRCInputManager.FindInput("UseRight");
	}

	// Token: 0x06005AD3 RID: 23251 RVA: 0x001FB144 File Offset: 0x001F9544
	public static IEnumerator DeferUntil(Func<bool> predicate, Action action)
	{
		while (!predicate())
		{
			yield return null;
		}
		action();
		yield break;
	}

	// Token: 0x06005AD4 RID: 23252 RVA: 0x001FB166 File Offset: 0x001F9566
	private void VRC_ObjectSpawn_Initialize(VRC_ObjectSpawn obj)
	{
		obj.gameObject.GetOrAddComponent<VRC_EventHandler>().enabled = true;
		obj.gameObject.GetOrAddComponent<ObjectInstantiator>().enabled = true;
	}

	// Token: 0x06005AD5 RID: 23253 RVA: 0x001FB18C File Offset: 0x001F958C
	private void VRC_DataStorage_Initialize(VRC_DataStorage obj)
	{
		if (obj.gameObject.GetComponent<VRC_DataStorageInternal>() == null)
		{
			VRC_DataStorageInternal orAddComponent = obj.gameObject.GetOrAddComponent<VRC_DataStorageInternal>();
			orAddComponent.Initialize(obj);
		}
	}

	// Token: 0x06005AD6 RID: 23254 RVA: 0x001FB1C4 File Offset: 0x001F95C4
	private void VRC_KeyEvents_Initialize(VRC_KeyEvents obj)
	{
		VRCPlayer x = obj.GetComponent<VRCPlayer>();
		if (x == null)
		{
			x = obj.gameObject.GetComponentInParent<VRCPlayer>();
		}
		if (x != null)
		{
			if (x != VRCPlayer.Instance)
			{
				obj.enabled = false;
			}
			else
			{
				obj.enabled = true;
			}
		}
	}

	// Token: 0x06005AD7 RID: 23255 RVA: 0x001FB220 File Offset: 0x001F9620
	private void VRC_AddDamage_Initialize(VRC_AddDamage obj)
	{
		VRCHealthAndDamageEvents vrchealthAndDamageEvents = obj.gameObject.AddMissingComponent<VRCHealthAndDamageEvents>();
		vrchealthAndDamageEvents.damageBase = obj;
	}

	// Token: 0x06005AD8 RID: 23256 RVA: 0x001FB240 File Offset: 0x001F9640
	private void VRC_PlayerMods_Initialize(VRC_PlayerMods obj)
	{
		PlayerMods orAddComponent = obj.gameObject.GetOrAddComponent<PlayerMods>();
		orAddComponent.isRoomPlayerMods = obj.isRoomPlayerMods;
		orAddComponent.playerMods = obj.playerMods;
		UnityEngine.Object.Destroy(obj);
	}

	// Token: 0x06005AD9 RID: 23257 RVA: 0x001FB278 File Offset: 0x001F9678
	private void VRC_PortalMarker_Initialize(VRC_PortalMarker obj)
	{
		if (obj.useDefaultPresentation && (obj.effectPrefabName == null || obj.effectPrefabName == string.Empty))
		{
			obj.effectPrefabName = "PortalExitEffect";
		}
		GameObject asset = (GameObject)Resources.Load("PortalInternal", typeof(GameObject));
		GameObject gameObject = (GameObject)AssetManagement.Instantiate(asset, obj.transform.position, obj.transform.rotation);
		gameObject.transform.parent = obj.transform;
		PortalTrigger orAddComponent = obj.gameObject.GetOrAddComponent<PortalTrigger>();
		orAddComponent.effectPrefabName = obj.effectPrefabName;
		PortalInternal orAddComponent2 = gameObject.GetOrAddComponent<PortalInternal>();
		orAddComponent2.RoomId = ((obj.world != VRC_PortalMarker.VRChatWorld.Hub) ? obj.roomId : RemoteConfig.GetString("hubWorldId"));
		orAddComponent2.SortHeading = (ApiWorld.SortHeading)obj.sortHeading;
		orAddComponent2.SortOrder = (ApiWorld.SortOrder)obj.sortOrder;
		orAddComponent2.Offset = obj.offset;
		orAddComponent2.SearchTerm = obj.searchTerm;
		orAddComponent2.Dynamic = false;
		orAddComponent2.FetchWorld(null);
	}

	// Token: 0x06005ADA RID: 23258 RVA: 0x001FB38C File Offset: 0x001F978C
	private void VRC_AddHealth_Initialize(VRC_AddHealth obj)
	{
		VRCHealthAndDamageEvents vrchealthAndDamageEvents = obj.gameObject.AddMissingComponent<VRCHealthAndDamageEvents>();
		vrchealthAndDamageEvents.healthBase = obj;
	}

	// Token: 0x06005ADB RID: 23259 RVA: 0x001FB3AC File Offset: 0x001F97AC
	private void VRC_PropController_Initialize(VRC_PropController obj)
	{
	}

	// Token: 0x06005ADC RID: 23260 RVA: 0x001FB3AE File Offset: 0x001F97AE
	private void VRC_StationInput_Initialize(VRC_StationInput obj)
	{
	}

	// Token: 0x06005ADD RID: 23261 RVA: 0x001FB3B0 File Offset: 0x001F97B0
	private void VRC_PropController_Update(VRC_PropController obj)
	{
		obj.inputLeftAnalog.x = VrcSdk2Interface.inAxisHorizontal.axis;
		obj.inputLeftAnalog.y = VrcSdk2Interface.inAxisVertical.axis;
		obj.inputRightAnalog.x = VrcSdk2Interface.inAxisLookHorizontal.axis;
		obj.inputRightAnalog.x = VrcSdk2Interface.inAxisLookVertical.axis;
		obj.inputUseButton = (VrcSdk2Interface.inUseLeft.button || VrcSdk2Interface.inUseRight.button);
		foreach (VRC_PropController.InputPairing inputPairing in obj.Inputs)
		{
			inputPairing.lastValue = inputPairing.value;
			inputPairing.value = false;
			foreach (KeyCode key in inputPairing.unityKeys)
			{
				inputPairing.value = (inputPairing.value || Input.GetKey(key));
			}
			foreach (string buttonName in inputPairing.cInputKeys)
			{
				inputPairing.value = (inputPairing.value || Input.GetButton(buttonName));
			}
		}
	}

	// Token: 0x06005ADE RID: 23262 RVA: 0x001FB510 File Offset: 0x001F9910
	private void VRC_StationInput_Update(VRC_StationInput obj)
	{
		obj.inputLeftAnalog.x = VrcSdk2Interface.inAxisHorizontal.axis;
		obj.inputLeftAnalog.y = VrcSdk2Interface.inAxisVertical.axis;
		obj.inputRightAnalog.x = VrcSdk2Interface.inAxisLookHorizontal.axis;
		obj.inputRightAnalog.x = VrcSdk2Interface.inAxisLookVertical.axis;
		obj.inputUseButton = (VrcSdk2Interface.inUseLeft.button || VrcSdk2Interface.inUseRight.button);
		foreach (VRC_StationInput.InputPairing inputPairing in obj.customInputs)
		{
			inputPairing.lastValue = inputPairing.value;
			inputPairing.value = false;
			foreach (KeyCode key in inputPairing.unityKeys)
			{
				inputPairing.value = (inputPairing.value || Input.GetKey(key));
			}
			foreach (string buttonName in inputPairing.cInputKeys)
			{
				inputPairing.value = (inputPairing.value || Input.GetButton(buttonName));
			}
		}
	}

	// Token: 0x06005ADF RID: 23263 RVA: 0x001FB670 File Offset: 0x001F9A70
	private void VRC_SyncAnimation_Initialize(VRC_SyncAnimation obj)
	{
		obj.gameObject.GetOrAddComponent<SyncAnimation>();
	}

	// Token: 0x06005AE0 RID: 23264 RVA: 0x001FB67E File Offset: 0x001F9A7E
	private void VRC_SyncVideoPlayer_Initialize(VRC_SyncVideoPlayer obj)
	{
		obj.gameObject.GetOrAddComponent<SyncVideoPlayer>();
	}

	// Token: 0x06005AE1 RID: 23265 RVA: 0x001FB68C File Offset: 0x001F9A8C
	private void VRC_SyncVideoPlayer_Play(VRC_SyncVideoPlayer obj)
	{
		SyncVideoPlayer component = obj.GetComponent<SyncVideoPlayer>();
		if (component != null)
		{
			component.Play(false, 0.0);
		}
	}

	// Token: 0x06005AE2 RID: 23266 RVA: 0x001FB6BC File Offset: 0x001F9ABC
	private void VRC_SyncVideoPlayer_PlayIndex(VRC_SyncVideoPlayer obj, int i)
	{
		SyncVideoPlayer component = obj.GetComponent<SyncVideoPlayer>();
		if (component != null)
		{
			component.PlayIndex(i);
		}
	}

	// Token: 0x06005AE3 RID: 23267 RVA: 0x001FB6E4 File Offset: 0x001F9AE4
	private void VRC_SyncVideoPlayer_Stop(VRC_SyncVideoPlayer obj)
	{
		SyncVideoPlayer component = obj.GetComponent<SyncVideoPlayer>();
		if (component != null)
		{
			component.Stop(false, true);
		}
	}

	// Token: 0x06005AE4 RID: 23268 RVA: 0x001FB70C File Offset: 0x001F9B0C
	private void VRC_SyncVideoPlayer_Pause(VRC_SyncVideoPlayer obj)
	{
		SyncVideoPlayer component = obj.GetComponent<SyncVideoPlayer>();
		if (component != null)
		{
			component.Pause();
		}
	}

	// Token: 0x06005AE5 RID: 23269 RVA: 0x001FB734 File Offset: 0x001F9B34
	private void VRC_SyncVideoPlayer_Next(VRC_SyncVideoPlayer obj)
	{
		SyncVideoPlayer component = obj.GetComponent<SyncVideoPlayer>();
		if (component != null)
		{
			component.Next();
		}
	}

	// Token: 0x06005AE6 RID: 23270 RVA: 0x001FB75C File Offset: 0x001F9B5C
	private void VRC_SyncVideoPlayer_Previous(VRC_SyncVideoPlayer obj)
	{
		SyncVideoPlayer component = obj.GetComponent<SyncVideoPlayer>();
		if (component != null)
		{
			component.Previous();
		}
	}

	// Token: 0x06005AE7 RID: 23271 RVA: 0x001FB784 File Offset: 0x001F9B84
	private void VRC_SyncVideoPlayer_Shuffle(VRC_SyncVideoPlayer obj)
	{
		SyncVideoPlayer component = obj.GetComponent<SyncVideoPlayer>();
		if (component != null)
		{
			component.Shuffle();
		}
	}

	// Token: 0x06005AE8 RID: 23272 RVA: 0x001FB7AC File Offset: 0x001F9BAC
	private void VRC_SyncVideoPlayer_Clear(VRC_SyncVideoPlayer obj)
	{
		SyncVideoPlayer component = obj.GetComponent<SyncVideoPlayer>();
		if (component != null)
		{
			component.Clear();
		}
	}

	// Token: 0x06005AE9 RID: 23273 RVA: 0x001FB7D4 File Offset: 0x001F9BD4
	private void VRC_SyncVideoPlayer_AddURL(VRC_SyncVideoPlayer obj, string url)
	{
		SyncVideoPlayer component = obj.GetComponent<SyncVideoPlayer>();
		if (component != null)
		{
			component.AddURL(url);
		}
	}

	// Token: 0x06005AEA RID: 23274 RVA: 0x001FB7FC File Offset: 0x001F9BFC
	private void VRC_SyncVideoPlayer_SpeedUp(VRC_SyncVideoPlayer obj)
	{
		SyncVideoPlayer component = obj.GetComponent<SyncVideoPlayer>();
		if (component != null)
		{
			component.SpeedUp();
		}
	}

	// Token: 0x06005AEB RID: 23275 RVA: 0x001FB824 File Offset: 0x001F9C24
	private void VRC_SyncVideoPlayer_SpeedDown(VRC_SyncVideoPlayer obj)
	{
		SyncVideoPlayer component = obj.GetComponent<SyncVideoPlayer>();
		if (component != null)
		{
			component.SpeedDown();
		}
	}

	// Token: 0x06005AEC RID: 23276 RVA: 0x001FB84A File Offset: 0x001F9C4A
	private void VRC_Station_Initialize(VRC_Station obj)
	{
		obj.gameObject.GetOrAddComponent<VRC_StationInternal>();
	}

	// Token: 0x06005AED RID: 23277 RVA: 0x001FB858 File Offset: 0x001F9C58
	private bool VRC_ObjectSync_IsLocal(VRC_ObjectSync obj)
	{
		return !(obj == null) && VRC.Network.GetOwner(obj.gameObject).isLocal;
	}

	// Token: 0x06005AEE RID: 23278 RVA: 0x001FB878 File Offset: 0x001F9C78
	private void VRC_ObjectSync_Initialize(VRC_ObjectSync obj)
	{
		obj.gameObject.GetOrAddComponent<ObjectInternal>();
		VRC_ObjectSync.GetIsKinematic = delegate(VRC_ObjectSync sync)
		{
			if (sync == null)
			{
				return false;
			}
			Rigidbody component = sync.GetComponent<Rigidbody>();
			return component != null && component.isKinematic;
		};
		VRC_ObjectSync.GetUseGravity = delegate(VRC_ObjectSync sync)
		{
			if (sync == null)
			{
				return false;
			}
			Rigidbody component = sync.GetComponent<Rigidbody>();
			return component != null && component.useGravity;
		};
		VRC_ObjectSync.SetUseGravity = delegate(VRC_ObjectSync sync, bool v)
		{
			if (sync == null)
			{
				return;
			}
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.Owner, sync.gameObject, (!v) ? "DisableGravity" : "EnableGravity", new object[0]);
		};
		VRC_ObjectSync.SetIsKinematic = delegate(VRC_ObjectSync sync, bool v)
		{
			if (sync == null)
			{
				return;
			}
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.Owner, sync.gameObject, (!v) ? "DisableKinematic" : "EnableKinematic", new object[0]);
		};
	}

	// Token: 0x06005AEF RID: 23279 RVA: 0x001FB91C File Offset: 0x001F9D1C
	private void VRC_ObjectSync_TeleportHandler(VRC_ObjectSync obj, Vector3 position, Quaternion rotation)
	{
		if (obj == null)
		{
			return;
		}
		LocomotionInputController componentInParent = obj.GetComponentInParent<LocomotionInputController>();
		if (componentInParent != null)
		{
			componentInParent.Teleport(position, rotation, VRC_SceneDescriptor.SpawnOrientation.Default);
		}
		else
		{
			IEnumerable<SyncPhysics> componentsInChildren = obj.GetComponentsInChildren<SyncPhysics>();
			foreach (SyncPhysics syncPhysics in componentsInChildren)
			{
				syncPhysics.TeleportTo(position, rotation);
			}
			if (componentsInChildren.FirstOrDefault<SyncPhysics>() == null)
			{
				obj.transform.position = position;
				obj.transform.rotation = rotation;
			}
		}
	}

	// Token: 0x06005AF0 RID: 23280 RVA: 0x001FB9D0 File Offset: 0x001F9DD0
	private void VRC_TriggerColliderEventTrigger_CollisionEnter(VRC_TriggerColliderEventTrigger obj, Collider other)
	{
		if (obj.Handler != null)
		{
			VRCPlayer componentInParent = other.gameObject.GetComponentInParent<VRCPlayer>();
			if (componentInParent == VRCPlayer.Instance)
			{
				VrcSdk2Interface.Instance.QueueDeferredEvent(true, obj.Handler, obj.EnterEventName, VRC_EventHandler.VrcBroadcastType.Always, componentInParent.gameObject, 0, 0f);
			}
		}
		else
		{
			Debug.LogError("Could not find VRC_EventHander on " + obj.gameObject.name + " or in a parent.");
		}
	}

	// Token: 0x06005AF1 RID: 23281 RVA: 0x001FBA54 File Offset: 0x001F9E54
	private void VRC_TriggerColliderEventTrigger_CollisionExit(VRC_TriggerColliderEventTrigger obj, Collider other)
	{
		if (obj.Handler != null)
		{
			VRCPlayer componentInParent = other.gameObject.GetComponentInParent<VRCPlayer>();
			if (componentInParent)
			{
				VrcSdk2Interface.Instance.QueueDeferredEvent(true, obj.Handler, obj.ExitEventName, VRC_EventHandler.VrcBroadcastType.Always, componentInParent.gameObject, 0, 0f);
			}
		}
		else
		{
			Debug.LogError("Could not find VRC_EventHander on " + obj.gameObject.name + " or in a parent.");
		}
	}

	// Token: 0x06005AF2 RID: 23282 RVA: 0x001FBAD4 File Offset: 0x001F9ED4
	private void VRC_Interactable_Initialize(VRC_Interactable obj)
	{
		if (obj.interactTextPlacement != null)
		{
			GameObject asset = Resources.Load<GameObject>("UseText");
			obj.interactTextGO = (GameObject)AssetManagement.Instantiate(asset);
			obj.interactTextGO.transform.parent = obj.gameObject.transform;
			obj.interactTextGO.transform.position = Vector3.zero;
			obj.interactTextGO.transform.localPosition = Vector3.zero;
			TextMesh component = obj.interactTextGO.transform.Find("TextMesh").GetComponent<TextMesh>();
			component.text = obj.interactText;
		}
	}

	// Token: 0x06005AF3 RID: 23283 RVA: 0x001FBB7C File Offset: 0x001F9F7C
	private void LogEvent(VRC_EventHandler eventHandler, VRC_EventHandler.VrcEvent vrcEvent, long combinedNetworkId, VRC_EventHandler.VrcBroadcastType broadcast, int instagatorId, float fastForward)
	{
		VRC_EventLog instance = VRC_EventLog.Instance;
		if (instance != null)
		{
			instance.LogEvent(eventHandler, vrcEvent, combinedNetworkId, broadcast, instagatorId, fastForward);
		}
		else
		{
			Debug.LogWarning("Logger is null");
		}
	}

	// Token: 0x06005AF4 RID: 23284 RVA: 0x001FBBBC File Offset: 0x001F9FBC
	private void VRC_PlayerApi_Initialize(VRC_PlayerApi obj)
	{
		if (obj == null)
		{
			return;
		}
		obj.isLocal = VRC.Network.GetOwner(obj.gameObject).isLocal;
		VRCPlayer component = obj.GetComponent<VRCPlayer>();
		if (component != null)
		{
			obj.name = component.playerName;
		}
	}

	// Token: 0x06005AF5 RID: 23285 RVA: 0x001FBC0C File Offset: 0x001FA00C
	private void VRC_PlayerApi_Update(VRC_PlayerApi obj)
	{
		VRCPlayer component = obj.GetComponent<VRCPlayer>();
		if (component != null)
		{
			obj.name = component.playerName;
		}
	}

	// Token: 0x06005AF6 RID: 23286 RVA: 0x001FBC38 File Offset: 0x001FA038
	private Ray VRC_PlayerApi_GetLookRay(VRC_PlayerApi player)
	{
		if (player.isLocal)
		{
			return VRCVrCamera.GetInstance().GetWorldLookRay();
		}
		return new Ray(player.transform.position, player.transform.forward);
	}

	// Token: 0x06005AF7 RID: 23287 RVA: 0x001FBC6B File Offset: 0x001FA06B
	private bool VRC_PLayerApi_IsGrounded(VRC_PlayerApi player)
	{
		return player.GetComponent<VRCMotionState>().isGrounded;
	}

	// Token: 0x06005AF8 RID: 23288 RVA: 0x001FBC78 File Offset: 0x001FA078
	private void VRC_PlayerApi_SetAnimatorBool(VRC_PlayerApi obj, string name, bool value)
	{
		if (obj.GetComponent<PlayerNet>() != null)
		{
			obj.GetComponent<PlayerNet>().SetAnimatorBool(name, value);
		}
	}

	// Token: 0x06005AF9 RID: 23289 RVA: 0x001FBC98 File Offset: 0x001FA098
	private void VRC_NpcApi_Initialize(VRC_NpcApi obj)
	{
		if (obj == null)
		{
			return;
		}
	}

	// Token: 0x06005AFA RID: 23290 RVA: 0x001FBCA7 File Offset: 0x001FA0A7
	private void VRC_RainObject_Initialize(VRC_RainObject obj)
	{
		obj.attackComponent = obj.GetComponentInParent<NpcAttack>();
	}

	// Token: 0x06005AFB RID: 23291 RVA: 0x001FBCB8 File Offset: 0x001FA0B8
	private void VRC_WebPanel_Initialize(VRC_WebPanel obj)
	{
		WebPanelInternal orAddComponent = obj.gameObject.GetOrAddComponent<WebPanelInternal>();
		orAddComponent.web = obj;
		obj._BindCall = new Func<string, Delegate, bool>(orAddComponent.BindCall);
	}

	// Token: 0x06005AFC RID: 23292 RVA: 0x001FBCEC File Offset: 0x001FA0EC
	private static bool VRC_StationApi_IsStationOccupiedDelegate(VRC_StationApi obj)
	{
		VRC_StationInternal component = obj.GetComponent<VRC_StationInternal>();
		return component.Occupant != null;
	}

	// Token: 0x06005AFD RID: 23293 RVA: 0x001FBD0C File Offset: 0x001FA10C
	private static VRC_PlayerApi VRC_StationApi_GatStationOccupant(VRC_StationApi obj)
	{
		VRC_StationInternal component = obj.GetComponent<VRC_StationInternal>();
		return component.Occupant.GetComponent<VRC_PlayerApi>();
	}

	// Token: 0x06005AFE RID: 23294 RVA: 0x001FBD2C File Offset: 0x001FA12C
	private static void VRC_Pickup_OnAwake(VRC_Pickup obj)
	{
		Rigidbody component = obj.GetComponent<Rigidbody>();
		if (component != null)
		{
			component.maxAngularVelocity = 40f;
		}
		obj.gameObject.GetOrAddComponent<ObjectInternal>();
	}

	// Token: 0x06005AFF RID: 23295 RVA: 0x001FBD63 File Offset: 0x001FA163
	private static void VRC_Pickup_ForceDrop(VRC_Pickup obj)
	{
		if (obj.currentlyHeldBy != null)
		{
			(obj.currentlyHeldBy as VRCHandGrasper).ForceDrop();
		}
	}

	// Token: 0x06005B00 RID: 23296 RVA: 0x001FBD86 File Offset: 0x001FA186
	private static void VRC_Pickup_HapticEvent(VRC_Pickup obj, float duration, float amplitude, float frequency)
	{
		if (obj.currentlyHeldBy != null)
		{
			(obj.currentlyHeldBy as VRCHandGrasper).HapticEvent(duration, amplitude, frequency);
		}
	}

	// Token: 0x06005B01 RID: 23297 RVA: 0x001FBDAC File Offset: 0x001FA1AC
	private static void VRC_Pickup_OnDestroyed(VRC_Pickup obj)
	{
		if (obj.currentlyHeldBy != null)
		{
			obj.Drop();
		}
	}

	// Token: 0x06005B02 RID: 23298 RVA: 0x001FBDC8 File Offset: 0x001FA1C8
	private static void VRC_SceneDescriptor_Initialize(VRC_SceneDescriptor sceneDescriptor)
	{
		if (sceneDescriptor == null)
		{
			Debug.LogError("Was handed a null VRC_SceneDescriptor");
			return;
		}
		if (VrcSdk2Interface.activeScene != null)
		{
			Debug.LogError("There are more than one VRCSDK2.VRC_SceneDescriptor objects present.");
		}
		VrcSdk2Interface.activeScene = sceneDescriptor;
		int num = Mathf.Clamp(VrcSdk2Interface.activeScene.UpdateTimeInMS, 10, 150);
		PhotonNetwork.sendRate = 1000 / num;
		Debug.Log("<color=magenta>Room is running with an update rate of " + num.ToString() + "</color>");
	}

	// Token: 0x06005B03 RID: 23299 RVA: 0x001FBE50 File Offset: 0x001FA250
	private void VRC_NPCSpawn_Initialize(VRC_NPCSpawn obj)
	{
		GameObject asset = (GameObject)Resources.Load("VRCNpc", typeof(GameObject));
		GameObject gameObject = (GameObject)AssetManagement.Instantiate(asset, obj.transform.position, obj.transform.rotation);
		VRC_NpcInternal component = gameObject.GetComponent<VRC_NpcInternal>();
		component.LoadAvatar(obj.npcName, obj.blueprintId, obj.scale);
		obj.npcGameObject = gameObject;
	}

	// Token: 0x06005B04 RID: 23300 RVA: 0x001FBEC0 File Offset: 0x001FA2C0
	private void InitializePlayerApi()
	{
		VRC_PlayerApi.Initialize = new VRC_PlayerApi.InitializeDelegate(this.VRC_PlayerApi_Initialize);
		VRC_PlayerApi.UpdateNow = new VRC_PlayerApi.UpdateDelegate(this.VRC_PlayerApi_Update);
		VRC_PlayerApi.SetAnimatorBool = new VRC_PlayerApi.SetAnimatorBoolDelegate(this.VRC_PlayerApi_SetAnimatorBool);
		VRC_PlayerApi.GetLookRay = new VRC_PlayerApi.GetLookRayDelegate(this.VRC_PlayerApi_GetLookRay);
		VRC_PlayerApi.IsGrounded = new VRC_PlayerApi.BoolDelegate(this.VRC_PLayerApi_IsGrounded);
		VRC_PlayerApi.ClaimNetworkControl = delegate(VRC_PlayerApi player, VRC_ObjectApi obj)
		{
			if (player != null)
			{
				VRC.Network.SetOwner(player.GetComponent<VRC.Player>(), obj.gameObject, VRC.Network.OwnershipModificationType.Request, true);
			}
		};
		if (VrcSdk2Interface.f__mg17 == null)
		{
			VrcSdk2Interface.f__mg17 = new Func<VRC_PlayerApi, int>(InternalSDKPlayer.GetPlayerId);
		}
		VRC_PlayerApi._GetPlayerId = VrcSdk2Interface.f__mg17;
		if (VrcSdk2Interface.f__mg18 == null)
		{
			VrcSdk2Interface.f__mg18 = new Func<int, VRC_PlayerApi>(InternalSDKPlayer.GetPlayerById);
		}
		VRC_PlayerApi._GetPlayerById = VrcSdk2Interface.f__mg18;
		if (VrcSdk2Interface.f__mg19 == null)
		{
			VrcSdk2Interface.f__mg19 = new Func<GameObject, VRC_PlayerApi>(InternalSDKPlayer.GetPlayerByGameObject);
		}
		VRC_PlayerApi._GetPlayerByGameObject = VrcSdk2Interface.f__mg19;
		if (VrcSdk2Interface.f__mg1A == null)
		{
			VrcSdk2Interface.f__mg1A = new Func<VRC_PlayerApi, GameObject, bool>(InternalSDKPlayer.IsOwner);
		}
		VRC_PlayerApi._IsOwner = VrcSdk2Interface.f__mg1A;
		if (VrcSdk2Interface.f__mg1B == null)
		{
			VrcSdk2Interface.f__mg1B = new Action<VRC_PlayerApi, GameObject>(InternalSDKPlayer.TakeOwnership);
		}
		VRC_PlayerApi._TakeOwnership = VrcSdk2Interface.f__mg1B;
		if (VrcSdk2Interface.f__mg1C == null)
		{
			VrcSdk2Interface.f__mg1C = new Func<VRC_PlayerApi, VRC_PlayerApi.TrackingDataType, VRC_PlayerApi.TrackingData>(InternalSDKPlayer.GetTrackingData);
		}
		VRC_PlayerApi._GetTrackingData = VrcSdk2Interface.f__mg1C;
		if (VrcSdk2Interface.f__mg1D == null)
		{
			VrcSdk2Interface.f__mg1D = new Func<VRC_PlayerApi, HumanBodyBones, Transform>(InternalSDKPlayer.GetBoneTransform);
		}
		VRC_PlayerApi._GetBoneTransform = VrcSdk2Interface.f__mg1D;
		if (VrcSdk2Interface.f__mg1E == null)
		{
			VrcSdk2Interface.f__mg1E = new Func<VRC_PlayerApi, VRC_Pickup.PickupHand, VRC_Pickup>(InternalSDKPlayer.GetPickupInHand);
		}
		VRC_PlayerApi._GetPickupInHand = VrcSdk2Interface.f__mg1E;
		if (VrcSdk2Interface.f__mg1F == null)
		{
			VrcSdk2Interface.f__mg1F = new VRC_PlayerApi.Action<VRC_PlayerApi, VRC_Pickup.PickupHand, float, float, float>(InternalSDKPlayer.PlayHapticEventInHand);
		}
		VRC_PlayerApi._PlayHapticEventInHand = VrcSdk2Interface.f__mg1F;
		if (VrcSdk2Interface.f__mg20 == null)
		{
			VrcSdk2Interface.f__mg20 = new Action<VRC_PlayerApi, Vector3, Quaternion>(InternalSDKPlayer.TeleportTo);
		}
		VRC_PlayerApi._TeleportTo = VrcSdk2Interface.f__mg20;
		if (VrcSdk2Interface.f__mg21 == null)
		{
			VrcSdk2Interface.f__mg21 = new Action<VRC_PlayerApi, Vector3, Quaternion, VRC_SceneDescriptor.SpawnOrientation>(InternalSDKPlayer.TeleportToOrientation);
		}
		VRC_PlayerApi._TeleportToOrientation = VrcSdk2Interface.f__mg21;
		VRC_PlayerApi._PushAnimations = new Action<VRC_PlayerApi, RuntimeAnimatorController>(this.VRC_PlayerApi_PushAnimations);
		VRC_PlayerApi._PopAnimations = new Action<VRC_PlayerApi>(this.VRC_PlayerApi_PopAnimations);
		VRC_PlayerApi._Immobilize = new Action<VRC_PlayerApi, bool>(this.VRC_PlayerApi_Immobilize);
		VRC_PlayerApi._GetVelocity = new Func<VRC_PlayerApi, Vector3>(this.VRC_PlayerApi_GetVelocity);
		VRC_PlayerApi._SetVelocity = new Action<VRC_PlayerApi, Vector3>(this.VRC_PlayerApi_SetVelocity);
		if (VrcSdk2Interface.f__mg22 == null)
		{
			VrcSdk2Interface.f__mg22 = new Action<VRC_PlayerApi, bool>(InternalSDKPlayer.EnablePickups);
		}
		VRC_PlayerApi._EnablePickups = VrcSdk2Interface.f__mg22;
		if (VrcSdk2Interface.f__mg23 == null)
		{
			VrcSdk2Interface.f__mg23 = new Action<VRC_PlayerApi, Color>(InternalSDKPlayer.SetNamePlateColor);
		}
		VRC_PlayerApi._SetNamePlateColor = VrcSdk2Interface.f__mg23;
		if (VrcSdk2Interface.f__mg24 == null)
		{
			VrcSdk2Interface.f__mg24 = new Action<VRC_PlayerApi>(InternalSDKPlayer.RestoreNamePlateColor);
		}
		VRC_PlayerApi._RestoreNamePlateColor = VrcSdk2Interface.f__mg24;
		if (VrcSdk2Interface.f__mg25 == null)
		{
			VrcSdk2Interface.f__mg25 = new Action<VRC_PlayerApi, bool>(InternalSDKPlayer.SetNamePlateVisibility);
		}
		VRC_PlayerApi._SetNamePlateVisibility = VrcSdk2Interface.f__mg25;
		if (VrcSdk2Interface.f__mg26 == null)
		{
			VrcSdk2Interface.f__mg26 = new Action<VRC_PlayerApi>(InternalSDKPlayer.RestoreNamePlateVisibility);
		}
		VRC_PlayerApi._RestoreNamePlateVisibility = VrcSdk2Interface.f__mg26;
		if (VrcSdk2Interface.f__mg27 == null)
		{
			VrcSdk2Interface.f__mg27 = new Action<VRC_PlayerApi, string, string>(InternalSDKPlayer.SetPlayerTag);
		}
		VRC_PlayerApi._SetPlayerTag = VrcSdk2Interface.f__mg27;
		if (VrcSdk2Interface.f__mg28 == null)
		{
			VrcSdk2Interface.f__mg28 = new Func<VRC_PlayerApi, string, string>(InternalSDKPlayer.GetPlayerTag);
		}
		VRC_PlayerApi._GetPlayerTag = VrcSdk2Interface.f__mg28;
		if (VrcSdk2Interface.f__mg29 == null)
		{
			VrcSdk2Interface.f__mg29 = new Func<string, string, List<int>>(InternalSDKPlayer.GetPlayersWithTag);
		}
		VRC_PlayerApi._GetPlayersWithTag = VrcSdk2Interface.f__mg29;
		if (VrcSdk2Interface.f__mg2A == null)
		{
			VrcSdk2Interface.f__mg2A = new Action<VRC_PlayerApi>(InternalSDKPlayer.ClearPlayerTags);
		}
		VRC_PlayerApi._ClearPlayerTags = VrcSdk2Interface.f__mg2A;
		if (VrcSdk2Interface.f__mg2B == null)
		{
			VrcSdk2Interface.f__mg2B = new Action<VRC_PlayerApi, bool, string, string>(InternalSDKPlayer.SetInvisibleToTagged);
		}
		VRC_PlayerApi._SetInvisibleToTagged = VrcSdk2Interface.f__mg2B;
		if (VrcSdk2Interface.f__mg2C == null)
		{
			VrcSdk2Interface.f__mg2C = new Action<VRC_PlayerApi, bool, string, string>(InternalSDKPlayer.SetInvisibleToUntagged);
		}
		VRC_PlayerApi._SetInvisibleToUntagged = VrcSdk2Interface.f__mg2C;
		if (VrcSdk2Interface.f__mg2D == null)
		{
			VrcSdk2Interface.f__mg2D = new Action<VRC_PlayerApi, int, string, string>(InternalSDKPlayer.SetSilencedToTagged);
		}
		VRC_PlayerApi._SetSilencedToTagged = VrcSdk2Interface.f__mg2D;
		if (VrcSdk2Interface.f__mg2E == null)
		{
			VrcSdk2Interface.f__mg2E = new Action<VRC_PlayerApi, int, string, string>(InternalSDKPlayer.SetSilencedToUntagged);
		}
		VRC_PlayerApi._SetSilencedToUntagged = VrcSdk2Interface.f__mg2E;
		if (VrcSdk2Interface.f__mg2F == null)
		{
			VrcSdk2Interface.f__mg2F = new Action<VRC_PlayerApi>(InternalSDKPlayer.ClearInvisible);
		}
		VRC_PlayerApi._ClearInvisible = VrcSdk2Interface.f__mg2F;
		if (VrcSdk2Interface.f__mg30 == null)
		{
			VrcSdk2Interface.f__mg30 = new Action<VRC_PlayerApi>(InternalSDKPlayer.ClearSilence);
		}
		VRC_PlayerApi._ClearSilence = VrcSdk2Interface.f__mg30;
		if (VrcSdk2Interface.f__mg31 == null)
		{
			VrcSdk2Interface.f__mg31 = new Action(VRCTrackingManager.IncreasePlayerHeight);
		}
		VRC_PlayerApi._IncreasePlayerHeight = VrcSdk2Interface.f__mg31;
		if (VrcSdk2Interface.f__mg32 == null)
		{
			VrcSdk2Interface.f__mg32 = new Action(VRCTrackingManager.DecreasePlayerHeight);
		}
		VRC_PlayerApi._DecreasePlayerHeight = VrcSdk2Interface.f__mg32;
		if (VrcSdk2Interface.f__mg33 == null)
		{
			VrcSdk2Interface.f__mg33 = new Action<float>(VRCTrackingManager.SetPlayerHeight);
		}
		VRC_PlayerApi._SetPlayerHeight = VrcSdk2Interface.f__mg33;
		if (VrcSdk2Interface.f__mg34 == null)
		{
			VrcSdk2Interface.f__mg34 = new Func<float>(VRCTrackingManager.GetPlayerHeight);
		}
		VRC_PlayerApi._GetPlayerHeight = VrcSdk2Interface.f__mg34;
		if (VrcSdk2Interface.f__mg35 == null)
		{
			VrcSdk2Interface.f__mg35 = new Func<float>(VRCTrackingManager.MeasurePlayerHeight);
		}
		VRC_PlayerApi._MeasurePlayerHeight = VrcSdk2Interface.f__mg35;
	}

	// Token: 0x06005B05 RID: 23301 RVA: 0x001FC3B8 File Offset: 0x001FA7B8
	private void InitializeNpcApi()
	{
		VRC_NpcApi.Initialize = new VRC_NpcApi.InitializeDelegate(this.VRC_NpcApi_Initialize);
		if (VrcSdk2Interface.f__mg36 == null)
		{
			VrcSdk2Interface.f__mg36 = new Func<GameObject, VRC_NpcApi>(InternalSDKNpc.GetApiByGameObject);
		}
		VRC_NpcApi._GetApiByGameObject = VrcSdk2Interface.f__mg36;
		if (VrcSdk2Interface.f__mg37 == null)
		{
			VrcSdk2Interface.f__mg37 = new Action<VRC_NpcApi, int, bool>(InternalSDKNpc.ActThis);
		}
		VRC_NpcApi._ActThis = VrcSdk2Interface.f__mg37;
		if (VrcSdk2Interface.f__mg38 == null)
		{
			VrcSdk2Interface.f__mg38 = new Action<VRC_NpcApi, AudioClip, float>(InternalSDKNpc.SayThis);
		}
		VRC_NpcApi._SayThis = VrcSdk2Interface.f__mg38;
		if (VrcSdk2Interface.f__mg39 == null)
		{
			VrcSdk2Interface.f__mg39 = new Action<VRC_NpcApi, bool, string, string>(InternalSDKNpc.SetNamePlate);
		}
		VRC_NpcApi._SetNamePlate = VrcSdk2Interface.f__mg39;
		if (VrcSdk2Interface.f__mg3A == null)
		{
			VrcSdk2Interface.f__mg3A = new Action<VRC_NpcApi, bool, bool, bool>(InternalSDKNpc.SetSocialStatus);
		}
		VRC_NpcApi._SetSocialStatus = VrcSdk2Interface.f__mg3A;
		if (VrcSdk2Interface.f__mg3B == null)
		{
			VrcSdk2Interface.f__mg3B = new Action<VRC_NpcApi, bool, bool>(InternalSDKNpc.SetMuteStatus);
		}
		VRC_NpcApi._SetMuteStatus = VrcSdk2Interface.f__mg3B;
	}

	// Token: 0x06005B06 RID: 23302 RVA: 0x001FC4A4 File Offset: 0x001FA8A4
	private void InitializeSDKNetworkingTools()
	{
		Networking._LocalPlayer = delegate
		{
			VRC.Player localPlayer = VRC.Network.LocalPlayer;
			return (!(localPlayer == null)) ? localPlayer.playerApi : null;
		};
		Networking._Instantiate = ((VRC_EventHandler.VrcBroadcastType a, string b, Vector3 c, Quaternion d) => VRC.Network.Instantiate(a, b, c, d));
		Networking._IsMaster = (() => VRC.Network.IsMaster);
		Networking._IsNetworkSettled = (() => VRC.Network.IsNetworkSettled);
		Networking._IsOwner = ((VRC_PlayerApi p, GameObject obj) => !(p == null) && VRC.Network.IsOwner(p.GetComponent<VRC.Player>(), obj));
		Networking._IsObjectReady = ((GameObject obj) => VRC.Network.IsObjectReady(obj));
		Networking._RPC = delegate(VRC_EventHandler.VrcTargetType clients, GameObject obj, string meth, object[] ps)
		{
			VRC.Network.RPC(clients, obj, meth, ps);
		};
		Networking._RPCtoPlayer = delegate(VRC_PlayerApi plyr, GameObject obj, string meth, object[] ps)
		{
			if (plyr != null)
			{
				VRC.Network.RPC(plyr.GetComponent<VRC.Player>(), obj, meth, ps);
			}
		};
		Networking._Message = delegate(VRC_EventHandler.VrcBroadcastType a, GameObject b, string c)
		{
			VRC.Network.Message(a, b, c);
		};
		Networking._SetOwner = delegate(VRC_PlayerApi api, GameObject obj)
		{
			if (api != null)
			{
				VRC.Network.SetOwner(api.GetComponent<VRC.Player>(), obj, VRC.Network.OwnershipModificationType.Request, false);
			}
		};
		Networking._GetOwner = delegate(GameObject obj)
		{
			VRC.Player owner = VRC.Network.GetOwner(obj);
			return (!(owner == null)) ? owner.playerApi : null;
		};
		Networking._ParameterEncoder = ((object[] a) => VRC_Serialization.ParameterEncoder(a));
		Networking._ParameterDecoder = ((byte[] a) => VRC_Serialization.ParameterDecoder(a, false));
		Networking._GoToRoom = ((string a) => InternalSDKPlayer.GoToRoom(a));
		Networking._Destroy = delegate(GameObject a)
		{
			VRC.Network.Destroy(a);
		};
		Networking._GetUniqueName = ((GameObject a) => VRC.Network.GetUniqueName(a));
		Networking._GetNetworkDateTime = (() => VRC.Network.GetNetworkDateTime());
		if (VrcSdk2Interface.f__mg3C == null)
		{
			VrcSdk2Interface.f__mg3C = new Func<double>(VRC.Network.GetServerTimeInSeconds);
		}
		Networking._GetServerTimeInSeconds = VrcSdk2Interface.f__mg3C;
		if (VrcSdk2Interface.f__mg3D == null)
		{
			VrcSdk2Interface.f__mg3D = new Func<int>(VRC.Network.GetServerTimeInMilliseconds);
		}
		Networking._GetServerTimeInMilliseconds = VrcSdk2Interface.f__mg3D;
		if (VrcSdk2Interface.f__mg3E == null)
		{
			VrcSdk2Interface.f__mg3E = new Func<double, double, double>(VRC.Network.CalculateServerDeltaTime);
		}
		Networking._CalculateServerDeltaTime = VrcSdk2Interface.f__mg3E;
	}

	// Token: 0x06005B07 RID: 23303 RVA: 0x001FC75C File Offset: 0x001FAB5C
	private void InitializeEventHandler()
	{
		VRC_EventHandler.AssignCombinedNetworkId = delegate(VRC_EventHandler s)
		{
			VRCFlowManager.Instance.StartCoroutine(VRC.Network.ConfigureEventHandler(s));
			return s.CombinedNetworkId;
		};
		VRC_EventHandler.GetInstigatorId = ((GameObject go) => VRC.Network.LocalInstigatorID);
		VRC_EventHandler.LogEvent = new VRC_EventHandler.LogEventDelegate(this.LogEvent);
	}

	// Token: 0x06005B08 RID: 23304 RVA: 0x001FC7BE File Offset: 0x001FABBE
	private void InitializeDebugTools()
	{
		VRCDebugCommand.OnAwake = new VRCDebugCommand.AwakeDelegate(this.VRC_DebugCommand_OnAwake);
	}

	// Token: 0x06005B09 RID: 23305 RVA: 0x001FC7D4 File Offset: 0x001FABD4
	private void QueueDeferredEvent(bool deferToFixedUpdate, VRC_EventHandler eventHandler, string eventName, VRC_EventHandler.VrcBroadcastType broadcast, GameObject instagator = null, int instagatorId = 0, float fastForward = 0f)
	{
		VrcSdk2Interface.DeferredEvent deferredEvent = new VrcSdk2Interface.DeferredEvent();
		deferredEvent.eventHandler = eventHandler;
		deferredEvent.eventName = eventName;
		deferredEvent.broadcast = broadcast;
		deferredEvent.instagator = instagator;
		deferredEvent.instagatorId = instagatorId;
		deferredEvent.fastForward = fastForward;
		if (deferToFixedUpdate)
		{
			this.mDeferredEventsNextFixedUpdate.Enqueue(deferredEvent);
		}
		else
		{
			this.mDeferredEventsNextUpdate.Enqueue(deferredEvent);
		}
	}

	// Token: 0x06005B0A RID: 23306 RVA: 0x001FC838 File Offset: 0x001FAC38
	private void ProcessDeferredEvents(Queue<VrcSdk2Interface.DeferredEvent> eventQueue)
	{
		while (eventQueue.Count > 0)
		{
			VrcSdk2Interface.DeferredEvent e = eventQueue.Dequeue();
			foreach (VRC_EventHandler.VrcEvent e2 in from t in e.eventHandler.Events
			where t.Name == e.eventName
			select t)
			{
				e.eventHandler.TriggerEvent(e2, e.broadcast, e.instagator, e.fastForward);
			}
		}
	}

	// Token: 0x06005B0B RID: 23307 RVA: 0x001FC8FC File Offset: 0x001FACFC
	private Camera VRC_UiShape_GetEventCamera()
	{
		return VRCVrCamera.GetInstance().screenCamera;
	}

	// Token: 0x06005B0C RID: 23308 RVA: 0x001FC908 File Offset: 0x001FAD08
	private void VRC_PlayerApi_PushAnimations(VRC_PlayerApi player, RuntimeAnimatorController animations)
	{
		player.GetComponentInChildren<AnimatorControllerManager>().Push(animations);
	}

	// Token: 0x06005B0D RID: 23309 RVA: 0x001FC916 File Offset: 0x001FAD16
	private void VRC_PlayerApi_PopAnimations(VRC_PlayerApi player)
	{
		player.GetComponentInChildren<AnimatorControllerManager>().Pop();
	}

	// Token: 0x06005B0E RID: 23310 RVA: 0x001FC923 File Offset: 0x001FAD23
	private void VRC_PlayerApi_Immobilize(VRC_PlayerApi player, bool immobile)
	{
		player.GetComponent<LocomotionInputController>().immobilize = immobile;
	}

	// Token: 0x06005B0F RID: 23311 RVA: 0x001FC931 File Offset: 0x001FAD31
	private Vector3 VRC_PlayerApi_GetVelocity(VRC_PlayerApi player)
	{
		return player.GetComponent<VRCMotionState>().PlayerVelocity;
	}

	// Token: 0x06005B10 RID: 23312 RVA: 0x001FC93E File Offset: 0x001FAD3E
	private void VRC_PlayerApi_SetVelocity(VRC_PlayerApi player, Vector3 velocity)
	{
		player.GetComponent<VRCMotionState>().PlayerVelocity = velocity;
	}

	// Token: 0x06005B11 RID: 23313 RVA: 0x001FC94C File Offset: 0x001FAD4C
	private bool VRC_Interactable_CheckValid(VRC_Interactable obj, VRC_PlayerApi apiPlayer)
	{
		if (obj == null || apiPlayer == null)
		{
			return false;
		}
		VRC.Player componentInParent = apiPlayer.GetComponentInParent<VRC.Player>();
		if (componentInParent == null)
		{
			return false;
		}
		VRC_StationInternal componentInParent2 = obj.GetComponentInParent<VRC_StationInternal>();
		if (componentInParent2 != null)
		{
			bool result;
			if (componentInParent2.PlayerCanUseStation(componentInParent, false))
			{
				result = VRC_StationInternal.FindActiveStations(componentInParent).All((VRC_StationInternal s) => s.canUseStationFromStation);
			}
			else
			{
				result = false;
			}
			return result;
		}
		return true;
	}

	// Token: 0x06005B12 RID: 23314 RVA: 0x001FC9D5 File Offset: 0x001FADD5
	private void VRC_DebugCommand_OnAwake(VRCDebugCommand obj)
	{
		Debug.Log("VRCDebugCommand: " + obj.Command);
		if (obj.Command == "SpawnUSpeakBot")
		{
			base.StartCoroutine(this.SpawnUSpeakBotDelayed(obj));
		}
	}

	// Token: 0x06005B13 RID: 23315 RVA: 0x001FCA10 File Offset: 0x001FAE10
	private IEnumerator SpawnUSpeakBotDelayed(VRCDebugCommand obj)
	{
		yield return new WaitForSeconds((float)UnityEngine.Random.Range(1, 5));
		USpeaker.DebugSpawnUSpeakBot(obj.gameObject, obj.ParamObject as AudioClip);
		yield break;
	}

	// Token: 0x06005B14 RID: 23316 RVA: 0x001FCA2C File Offset: 0x001FAE2C
	private void VRC_Migrate_YouTubeSync(VRC_YouTubeSync yt)
	{
		if (yt == null)
		{
			return;
		}
		foreach (VRC_WebPanel obj in yt.gameObject.GetComponents<VRC_WebPanel>())
		{
			UnityEngine.Object.DestroyImmediate(obj);
		}
		foreach (WebPanelInternal obj2 in yt.gameObject.GetComponents<WebPanelInternal>())
		{
			UnityEngine.Object.DestroyImmediate(obj2);
		}
		VRC_SyncVideoPlayer orAddComponent = yt.gameObject.GetOrAddComponent<VRC_SyncVideoPlayer>();
		List<VRC_SyncVideoPlayer.VideoEntry> list = orAddComponent.Videos.ToList<VRC_SyncVideoPlayer.VideoEntry>();
		List<string> list2 = new List<string>();
		if (!string.IsNullOrEmpty(yt.videoID))
		{
			list2.Add("http://youtube.com/watch?v=" + yt.videoID);
		}
		foreach (string text in yt.playlist)
		{
			if (!string.IsNullOrEmpty(text))
			{
				list2.Add("http://youtube.com/watch?v=" + text);
			}
		}
		foreach (string url in list2)
		{
			list.Add(new VRC_SyncVideoPlayer.VideoEntry
			{
				AspectRatio = VideoAspectRatio.FitInside,
				PlaybackSpeed = 1f,
				VideoClip = null,
				URL = url,
				Source = VideoSource.Url
			});
		}
		orAddComponent.Videos = list.ToArray();
	}

	// Token: 0x06005B15 RID: 23317 RVA: 0x001FCBC4 File Offset: 0x001FAFC4
	private Dictionary<string, object> VRC_MetadataListener_GetCurrentMetadata()
	{
		return RoomManager.metadata;
	}

	// Token: 0x04004086 RID: 16518
	public static VrcSdk2Interface Instance;

	// Token: 0x04004087 RID: 16519
	private static VRC_SceneDescriptor activeScene;

	// Token: 0x04004088 RID: 16520
	private static bool exists;

	// Token: 0x04004089 RID: 16521
	private Queue<VrcSdk2Interface.DeferredEvent> mDeferredEventsNextUpdate = new Queue<VrcSdk2Interface.DeferredEvent>();

	// Token: 0x0400408A RID: 16522
	private Queue<VrcSdk2Interface.DeferredEvent> mDeferredEventsNextFixedUpdate = new Queue<VrcSdk2Interface.DeferredEvent>();

	// Token: 0x0400408B RID: 16523
	private static VRCInput inAxisHorizontal;

	// Token: 0x0400408C RID: 16524
	private static VRCInput inAxisVertical;

	// Token: 0x0400408D RID: 16525
	private static VRCInput inAxisLookHorizontal;

	// Token: 0x0400408E RID: 16526
	private static VRCInput inAxisLookVertical;

	// Token: 0x0400408F RID: 16527
	private static VRCInput inUseLeft;

	// Token: 0x04004090 RID: 16528
	private static VRCInput inUseRight;

	// Token: 0x04004091 RID: 16529
	[CompilerGenerated]
	private static VRC_AvatarPedestal.InstantiationDelegate f__mg0;

	// Token: 0x04004092 RID: 16530
	[CompilerGenerated]
	private static VRC_AvatarCalibrator.InstantiationDelegate f__mg1;

	// Token: 0x04004093 RID: 16531
	[CompilerGenerated]
	private static VRC_DataStorage.SerializationDelegate f__mg2;

	// Token: 0x04004094 RID: 16532
	[CompilerGenerated]
	private static VRC_DataStorage.DeserializationDelegate f__mg3;

	// Token: 0x04004095 RID: 16533
	[CompilerGenerated]
	private static VRC_StationApi.StationOccupiedDelegate f__mg4;

	// Token: 0x04004096 RID: 16534
	[CompilerGenerated]
	private static VRC_StationApi.StationOccupantDelegate f__mg5;

	// Token: 0x04004097 RID: 16535
	[CompilerGenerated]
	private static VRC_Pickup.AwakeDelegate f__mg6;

	// Token: 0x04004098 RID: 16536
	[CompilerGenerated]
	private static VRC_Pickup.ForceDropDelegate f__mg7;

	// Token: 0x04004099 RID: 16537
	[CompilerGenerated]
	private static VRC_Pickup.HapticEventDelegate f__mg8;

	// Token: 0x0400409A RID: 16538
	[CompilerGenerated]
	private static VRC_Pickup.OnDestroyedDelegate f__mg9;

	// Token: 0x0400409B RID: 16539
	[CompilerGenerated]
	private static Func<VRC_Pickup, VRC_Pickup.PickupHand> f__mgA;

	// Token: 0x0400409C RID: 16540
	[CompilerGenerated]
	private static Func<VRC_Pickup, VRC_PlayerApi> f__mgB;

	// Token: 0x0400409D RID: 16541
	[CompilerGenerated]
	private static VRC_SceneDescriptor.IntializationDelegate f__mgC;

	// Token: 0x0400409E RID: 16542
	[CompilerGenerated]
	private static Tutorial.ActivateObjectLabelDelegate f__mgD;

	// Token: 0x0400409F RID: 16543
	[CompilerGenerated]
	private static Tutorial.DeactivateObjectLabelDelegate f__mgE;

	// Token: 0x040040A0 RID: 16544
	[CompilerGenerated]
	private static Tutorial.ActivateControllerLabelDelegate f__mgF;

	// Token: 0x040040A1 RID: 16545
	[CompilerGenerated]
	private static Tutorial.DeactivateControllerLabelDelegate f__mg10;

	// Token: 0x040040A2 RID: 16546
	[CompilerGenerated]
	private static Func<bool> f__mg11;

	// Token: 0x040040A3 RID: 16547
	[CompilerGenerated]
	private static Func<VRCInputMethod> f__mg12;

	// Token: 0x040040A4 RID: 16548
	[CompilerGenerated]
	private static Func<VRCInputSetting, bool> f__mg13;

	// Token: 0x040040A5 RID: 16549
	[CompilerGenerated]
	private static Action<VRCInputSetting, bool> f__mg14;

	// Token: 0x040040A6 RID: 16550
	[CompilerGenerated]
	private static Action<Renderer, bool> f__mg15;

	// Token: 0x040040A7 RID: 16551
	[CompilerGenerated]
	private static Action<VRC_Analytics.EventType, Dictionary<string, object>, Vector3?> f__mg16;

	// Token: 0x040040AF RID: 16559
	[CompilerGenerated]
	private static Func<VRC_PlayerApi, int> f__mg17;

	// Token: 0x040040B0 RID: 16560
	[CompilerGenerated]
	private static Func<int, VRC_PlayerApi> f__mg18;

	// Token: 0x040040B1 RID: 16561
	[CompilerGenerated]
	private static Func<GameObject, VRC_PlayerApi> f__mg19;

	// Token: 0x040040B2 RID: 16562
	[CompilerGenerated]
	private static Func<VRC_PlayerApi, GameObject, bool> f__mg1A;

	// Token: 0x040040B3 RID: 16563
	[CompilerGenerated]
	private static Action<VRC_PlayerApi, GameObject> f__mg1B;

	// Token: 0x040040B4 RID: 16564
	[CompilerGenerated]
	private static Func<VRC_PlayerApi, VRC_PlayerApi.TrackingDataType, VRC_PlayerApi.TrackingData> f__mg1C;

	// Token: 0x040040B5 RID: 16565
	[CompilerGenerated]
	private static Func<VRC_PlayerApi, HumanBodyBones, Transform> f__mg1D;

	// Token: 0x040040B6 RID: 16566
	[CompilerGenerated]
	private static Func<VRC_PlayerApi, VRC_Pickup.PickupHand, VRC_Pickup> f__mg1E;

	// Token: 0x040040B7 RID: 16567
	[CompilerGenerated]
	private static VRC_PlayerApi.Action<VRC_PlayerApi, VRC_Pickup.PickupHand, float, float, float> f__mg1F;

	// Token: 0x040040B8 RID: 16568
	[CompilerGenerated]
	private static Action<VRC_PlayerApi, Vector3, Quaternion> f__mg20;

	// Token: 0x040040B9 RID: 16569
	[CompilerGenerated]
	private static Action<VRC_PlayerApi, Vector3, Quaternion, VRC_SceneDescriptor.SpawnOrientation> f__mg21;

	// Token: 0x040040BA RID: 16570
	[CompilerGenerated]
	private static Action<VRC_PlayerApi, bool> f__mg22;

	// Token: 0x040040BB RID: 16571
	[CompilerGenerated]
	private static Action<VRC_PlayerApi, Color> f__mg23;

	// Token: 0x040040BC RID: 16572
	[CompilerGenerated]
	private static Action<VRC_PlayerApi> f__mg24;

	// Token: 0x040040BD RID: 16573
	[CompilerGenerated]
	private static Action<VRC_PlayerApi, bool> f__mg25;

	// Token: 0x040040BE RID: 16574
	[CompilerGenerated]
	private static Action<VRC_PlayerApi> f__mg26;

	// Token: 0x040040BF RID: 16575
	[CompilerGenerated]
	private static Action<VRC_PlayerApi, string, string> f__mg27;

	// Token: 0x040040C0 RID: 16576
	[CompilerGenerated]
	private static Func<VRC_PlayerApi, string, string> f__mg28;

	// Token: 0x040040C1 RID: 16577
	[CompilerGenerated]
	private static Func<string, string, List<int>> f__mg29;

	// Token: 0x040040C2 RID: 16578
	[CompilerGenerated]
	private static Action<VRC_PlayerApi> f__mg2A;

	// Token: 0x040040C3 RID: 16579
	[CompilerGenerated]
	private static Action<VRC_PlayerApi, bool, string, string> f__mg2B;

	// Token: 0x040040C4 RID: 16580
	[CompilerGenerated]
	private static Action<VRC_PlayerApi, bool, string, string> f__mg2C;

	// Token: 0x040040C5 RID: 16581
	[CompilerGenerated]
	private static Action<VRC_PlayerApi, int, string, string> f__mg2D;

	// Token: 0x040040C6 RID: 16582
	[CompilerGenerated]
	private static Action<VRC_PlayerApi, int, string, string> f__mg2E;

	// Token: 0x040040C7 RID: 16583
	[CompilerGenerated]
	private static Action<VRC_PlayerApi> f__mg2F;

	// Token: 0x040040C8 RID: 16584
	[CompilerGenerated]
	private static Action<VRC_PlayerApi> f__mg30;

	// Token: 0x040040C9 RID: 16585
	[CompilerGenerated]
	private static Action f__mg31;

	// Token: 0x040040CA RID: 16586
	[CompilerGenerated]
	private static Action f__mg32;

	// Token: 0x040040CB RID: 16587
	[CompilerGenerated]
	private static Action<float> f__mg33;

	// Token: 0x040040CC RID: 16588
	[CompilerGenerated]
	private static Func<float> f__mg34;

	// Token: 0x040040CD RID: 16589
	[CompilerGenerated]
	private static Func<float> f__mg35;

	// Token: 0x040040CF RID: 16591
	[CompilerGenerated]
	private static Func<GameObject, VRC_NpcApi> f__mg36;

	// Token: 0x040040D0 RID: 16592
	[CompilerGenerated]
	private static Action<VRC_NpcApi, int, bool> f__mg37;

	// Token: 0x040040D1 RID: 16593
	[CompilerGenerated]
	private static Action<VRC_NpcApi, AudioClip, float> f__mg38;

	// Token: 0x040040D2 RID: 16594
	[CompilerGenerated]
	private static Action<VRC_NpcApi, bool, string, string> f__mg39;

	// Token: 0x040040D3 RID: 16595
	[CompilerGenerated]
	private static Action<VRC_NpcApi, bool, bool, bool> f__mg3A;

	// Token: 0x040040D4 RID: 16596
	[CompilerGenerated]
	private static Action<VRC_NpcApi, bool, bool> f__mg3B;

	// Token: 0x040040D5 RID: 16597
	[CompilerGenerated]
	private static Func<double> f__mg3C;

	// Token: 0x040040D6 RID: 16598
	[CompilerGenerated]
	private static Func<int> f__mg3D;

	// Token: 0x040040D7 RID: 16599
	[CompilerGenerated]
	private static Func<double, double, double> f__mg3E;

	// Token: 0x02000B75 RID: 2933
	private class DeferredEvent
	{
		// Token: 0x040040EC RID: 16620
		public VRC_EventHandler eventHandler;

		// Token: 0x040040ED RID: 16621
		public string eventName = string.Empty;

		// Token: 0x040040EE RID: 16622
		public VRC_EventHandler.VrcBroadcastType broadcast;

		// Token: 0x040040EF RID: 16623
		public GameObject instagator;

		// Token: 0x040040F0 RID: 16624
		public int instagatorId;

		// Token: 0x040040F1 RID: 16625
		public float fastForward;
	}
}
