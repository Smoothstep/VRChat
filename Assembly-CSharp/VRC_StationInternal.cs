using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000B73 RID: 2931
public class VRC_StationInternal : VRCPunBehaviour
{
	// Token: 0x06005AA3 RID: 23203 RVA: 0x001F9724 File Offset: 0x001F7B24
	public static bool ExitAllStations(VRC.Player target)
	{
		if (target == null)
		{
			return false;
		}
		bool flag = true;
		List<VRC_StationInternal> list = VRC_StationInternal.FindActiveStations(target);
		foreach (VRC_StationInternal vrc_StationInternal in list)
		{
			vrc_StationInternal.ExitStation(target);
			flag = (flag && vrc_StationInternal.Occupant != target);
		}
		return flag;
	}

	// Token: 0x06005AA4 RID: 23204 RVA: 0x001F97AC File Offset: 0x001F7BAC
	public static void AddActiveStation(VRC_StationInternal station)
	{
		VRC_StationInternal.activeStations.Add(station);
	}

	// Token: 0x06005AA5 RID: 23205 RVA: 0x001F97BA File Offset: 0x001F7BBA
	public static void RemoveActiveStation(VRC_StationInternal station)
	{
		VRC_StationInternal.activeStations.Remove(station);
	}

	// Token: 0x06005AA6 RID: 23206 RVA: 0x001F97C8 File Offset: 0x001F7BC8
	public static List<VRC_StationInternal> FindActiveStations(VRC.Player target)
	{
		List<VRC_StationInternal> list = new List<VRC_StationInternal>();
		if (VRC_StationInternal.activeStations != null)
		{
			foreach (VRC_StationInternal item in from s in VRC_StationInternal.activeStations
			where s != null && s.Occupant == target
			select s)
			{
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x17000D14 RID: 3348
	// (get) Token: 0x06005AA7 RID: 23207 RVA: 0x001F9850 File Offset: 0x001F7C50
	// (set) Token: 0x06005AA8 RID: 23208 RVA: 0x001F9858 File Offset: 0x001F7C58
	public VRC.Player Occupant
	{
		get
		{
			return this._occupant;
		}
		private set
		{
			this._occupant = value;
		}
	}

	// Token: 0x17000D15 RID: 3349
	// (get) Token: 0x06005AA9 RID: 23209 RVA: 0x001F9861 File Offset: 0x001F7C61
	public bool canUseStationFromStation
	{
		get
		{
			return this.baseStation.canUseStationFromStation;
		}
	}

	// Token: 0x17000D16 RID: 3350
	// (get) Token: 0x06005AAA RID: 23210 RVA: 0x001F986E File Offset: 0x001F7C6E
	public RuntimeAnimatorController animatorController
	{
		get
		{
			return this.baseStation.animatorController;
		}
	}

	// Token: 0x17000D17 RID: 3351
	// (get) Token: 0x06005AAB RID: 23211 RVA: 0x001F987B File Offset: 0x001F7C7B
	public VRC_Station.Mobility mobility
	{
		get
		{
			return this.baseStation.PlayerMobility;
		}
	}

	// Token: 0x17000D18 RID: 3352
	// (get) Token: 0x06005AAC RID: 23212 RVA: 0x001F9888 File Offset: 0x001F7C88
	public bool isImmobilized
	{
		get
		{
			return this.mobility != VRC_Station.Mobility.Mobile || this.isSeated;
		}
	}

	// Token: 0x17000D19 RID: 3353
	// (get) Token: 0x06005AAD RID: 23213 RVA: 0x001F989E File Offset: 0x001F7C9E
	public bool inVehicle
	{
		get
		{
			return this.mobility == VRC_Station.Mobility.ImmobilizeForVehicle;
		}
	}

	// Token: 0x17000D1A RID: 3354
	// (get) Token: 0x06005AAE RID: 23214 RVA: 0x001F98A9 File Offset: 0x001F7CA9
	public bool isSeated
	{
		get
		{
			return this.baseStation != null && this.baseStation.seated;
		}
	}

	// Token: 0x17000D1B RID: 3355
	// (get) Token: 0x06005AAF RID: 23215 RVA: 0x001F98CA File Offset: 0x001F7CCA
	public VRC_ObjectApi controlsObject
	{
		get
		{
			return this.baseStation.controlsObject;
		}
	}

	// Token: 0x17000D1C RID: 3356
	// (get) Token: 0x06005AB0 RID: 23216 RVA: 0x001F98D7 File Offset: 0x001F7CD7
	public bool disableStationExit
	{
		get
		{
			return this.baseStation.disableStationExit;
		}
	}

	// Token: 0x17000D1D RID: 3357
	// (get) Token: 0x06005AB1 RID: 23217 RVA: 0x001F98E4 File Offset: 0x001F7CE4
	public Transform stationExitPlayerLocation
	{
		get
		{
			return this.baseStation.stationExitPlayerLocation;
		}
	}

	// Token: 0x17000D1E RID: 3358
	// (get) Token: 0x06005AB2 RID: 23218 RVA: 0x001F98F1 File Offset: 0x001F7CF1
	public Transform stationEnterPlayerLocation
	{
		get
		{
			return this.baseStation.stationEnterPlayerLocation;
		}
	}

	// Token: 0x06005AB3 RID: 23219 RVA: 0x001F9900 File Offset: 0x001F7D00
	public override void Awake()
	{
		base.Awake();
		this.baseStation = base.GetComponentInParent<VRC_Station>();
		this.stationControls = base.GetComponentInParent<VRC_PropController>();
		Collider component = base.gameObject.GetComponent<Collider>();
		if (component == null)
		{
			BoxCollider boxCollider = base.gameObject.AddComponent<BoxCollider>();
			boxCollider.isTrigger = true;
		}
	}

	// Token: 0x06005AB4 RID: 23220 RVA: 0x001F9958 File Offset: 0x001F7D58
	protected override void OnNetworkReady()
	{
		base.OnNetworkReady();
		if (!VRC_EventHandler.HasEventTrigger(base.gameObject))
		{
			VRC_Trigger orAddComponent = base.gameObject.GetOrAddComponent<VRC_Trigger>();
			List<VRC_Trigger.TriggerEvent> triggers = orAddComponent.Triggers;
			VRC_Trigger.TriggerEvent triggerEvent = new VRC_Trigger.TriggerEvent();
			triggerEvent.BroadcastType = VRC_EventHandler.VrcBroadcastType.Local;
			triggerEvent.TriggerType = VRC_Trigger.TriggerType.OnInteract;
			triggerEvent.Events = (from evt in this.baseStation.ProvideEvents()
			where evt.Name == "UseStation"
			select evt).ToList<VRC_EventHandler.VrcEvent>();
			triggers.Add(triggerEvent);
		}
		else
		{
			VRC_Trigger component = base.GetComponent<VRC_Trigger>();
			if (component != null)
			{
				foreach (VRC_Trigger.TriggerEvent triggerEvent2 in from t in component.Triggers
				where t.Events.Any((VRC_EventHandler.VrcEvent e) => e.ParameterString == "UseStation" || e.ParameterString == "ExitStation")
				select t)
				{
					triggerEvent2.BroadcastType = VRC_EventHandler.VrcBroadcastType.Local;
				}
			}
			VRC_UseEvents component2 = base.GetComponent<VRC_UseEvents>();
			if (component2 != null)
			{
				component2.BroadcastType = VRC_EventHandler.VrcBroadcastType.Local;
				component2.EventName = "UseStation";
			}
		}
		Collider component3 = base.gameObject.GetComponent<Collider>();
		if (component3 == null)
		{
			BoxCollider boxCollider = base.gameObject.AddComponent<BoxCollider>();
			boxCollider.isTrigger = true;
		}
		VRC_EventHandler component4 = base.GetComponent<VRC_EventHandler>();
		if (component4 != null)
		{
			VRC_Station component5 = base.GetComponent<VRC_Station>();
			using (IEnumerator<VRC_EventHandler.VrcEvent> enumerator2 = component5.ProvideEvents().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					VRC_EventHandler.VrcEvent evt = enumerator2.Current;
					VRC_StationInternal t = this;
					if (!component4.Events.Any((VRC_EventHandler.VrcEvent item) => item.Name == evt.Name && item.ParameterObjects != null && item.ParameterObjects.Contains(t.gameObject)))
					{
						component4.Events.Add(evt);
					}
				}
			}
		}
	}

	// Token: 0x06005AB5 RID: 23221 RVA: 0x001F9B70 File Offset: 0x001F7F70
	private void Update()
	{
		VRC.Player occupant = this.Occupant;
		if (((occupant == null && this.Occupant != null) || (occupant != null && (this.Occupant == null || this.Occupant.userId != occupant.userId))) && (double)Time.time - base.photonView.ownershipTransferTime > VRC.Network.SendInterval * 8.0)
		{
			this.InternalExitStation(this.Occupant);
		}
		if (occupant == null)
		{
			return;
		}
		VRCPlayer component = occupant.GetComponent<VRCPlayer>();
		if (!component.isAvatarLoaded)
		{
			return;
		}
		if (occupant != null && this.Occupant == null && this.PlayerCanUseStation(occupant, false))
		{
			this.InternalUseStation(occupant);
		}
		if (this.Occupant != null && this.isImmobilized)
		{
			Vector3 position = base.transform.position;
			Quaternion rotation = base.transform.rotation;
			if (this.stationEnterPlayerLocation != null)
			{
				position = this.stationEnterPlayerLocation.position;
				rotation = this.stationEnterPlayerLocation.rotation;
			}
			Rigidbody component2 = this.Occupant.GetComponent<Rigidbody>();
			if (component2 != null && !component2.isKinematic)
			{
				component2.MovePosition(position);
				component2.MoveRotation(rotation);
			}
			else
			{
				this.Occupant.transform.position = position;
				this.Occupant.transform.rotation = rotation;
			}
			if (this.Occupant.isLocal)
			{
				VRCTrackingManager.SetTrackingWorldOrigin(position + Quaternion.Inverse(this.stationEnterInitialRotation) * rotation * this.trackingOriginDeltaPosition, rotation * this.trackingOriginDeltaRotation);
			}
		}
	}

	// Token: 0x06005AB6 RID: 23222 RVA: 0x001F9D5C File Offset: 0x001F815C
	public bool PlayerCanUseStation(VRC.Player player, bool log = false)
	{
		if (player == null)
		{
			return false;
		}
		if (this.Occupant != null && this.Occupant.playerApi.playerId != player.playerApi.playerId)
		{
			if (log)
			{
				Debug.LogError(player.name + " cannot use station because it is occupied.", this);
			}
			return false;
		}
		VRCAvatarManager componentInChildren = player.GetComponentInChildren<VRCAvatarManager>();
		if (componentInChildren == null)
		{
			if (log)
			{
				Debug.LogError(player.name + " cannot use station because they lack an AvatarManager.", this);
			}
			return false;
		}
		if (!componentInChildren.IsHuman)
		{
			if (log)
			{
				Debug.LogError(player.name + " cannot use station because their avatar is not human.", this);
			}
			return false;
		}
		if (this.isSeated)
		{
			if (player.GetComponentInChildren<AnimatorControllerManager>() == null)
			{
				if (log)
				{
					Debug.LogError(player.name + " cannot use station because they have no AnimatorControllerManager.", this);
				}
				return false;
			}
			RuntimeAnimatorController runtimeAnimatorController = this.animatorController;
			if (runtimeAnimatorController != null)
			{
				if (runtimeAnimatorController.name == "SitStation")
				{
					runtimeAnimatorController = componentInChildren.GetSitAnimController();
				}
				if (runtimeAnimatorController == null)
				{
					if (log)
					{
						Debug.LogError(player.name + " cannot use station because they lack a sit animation controller.", this);
					}
					return false;
				}
			}
			else
			{
				runtimeAnimatorController = componentInChildren.GetSitAnimController();
				if (runtimeAnimatorController == null)
				{
					if (log)
					{
						Debug.LogError(player.name + " cannot use station because they lack a sit animation controller.", this);
					}
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06005AB7 RID: 23223 RVA: 0x001F9EEC File Offset: 0x001F82EC
	public void OnPlayerJoined(VRC_PlayerApi p)
	{
		if (this.Occupant != null && this.Occupant.isLocal)
		{
			VRC.Network.RPC(PlayerManager.GetPlayer(p.playerId), base.gameObject, "InternalUseStation", new object[0]);
		}
	}

	// Token: 0x06005AB8 RID: 23224 RVA: 0x001F9F3C File Offset: 0x001F833C
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.Local,
		VRC_EventHandler.VrcTargetType.All
	})]
	public void UseStation(VRC.Player player)
	{
		if (!player.isLocal)
		{
			return;
		}
		if (this.PlayerCanUseStation(player, true) && VRC.Network.SetOwner(player, base.gameObject, VRC.Network.OwnershipModificationType.Request, false))
		{
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "InternalUseStation", new object[0]);
		}
	}

	// Token: 0x06005AB9 RID: 23225 RVA: 0x001F9F8C File Offset: 0x001F838C
	public void ApplySeatedAnimation(VRC.Player player)
	{
		RuntimeAnimatorController runtimeAnimatorController = this.animatorController;
		VRCAvatarManager componentInChildren = player.GetComponentInChildren<VRCAvatarManager>();
		if (runtimeAnimatorController != null)
		{
			if (runtimeAnimatorController.name == "SitStation")
			{
				runtimeAnimatorController = componentInChildren.GetSitAnimController();
			}
			if (runtimeAnimatorController == null)
			{
				Debug.LogError("SitAnimController on " + player.name + " is not available.", this);
			}
			else
			{
				this.AttachAnimatorControllerTo(player, runtimeAnimatorController);
			}
		}
		else
		{
			runtimeAnimatorController = componentInChildren.GetSitAnimController();
			if (runtimeAnimatorController == null)
			{
				Debug.LogError("SitAnimController on " + player.name + " is not available.", this);
			}
			else
			{
				this.AttachAnimatorControllerTo(player, runtimeAnimatorController);
			}
		}
	}

	// Token: 0x06005ABA RID: 23226 RVA: 0x001FA044 File Offset: 0x001F8444
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All,
		VRC_EventHandler.VrcTargetType.TargetPlayer
	})]
	private void InternalUseStation(VRC.Player player)
	{
		bool flag = false;
		if (player == null || (this.Occupant != null && this.Occupant != player) || player == this.Occupant)
		{
			return;
		}
		List<VRC_StationInternal> source = VRC_StationInternal.FindActiveStations(player);
		if (source.FirstOrDefault((VRC_StationInternal s) => s != this) != null)
		{
			if (!this.canUseStationFromStation)
			{
				return;
			}
			if (!VRC_StationInternal.ExitAllStations(player))
			{
				return;
			}
			flag = true;
		}
		else
		{
			VRC_StationInternal.ExitAllStations(player);
		}
		if (this.Occupant != null)
		{
			return;
		}
		VRC_StationInternal.FlagDiscontinuity(player);
		this.Occupant = player;
		if (flag)
		{
			this._occupant = null;
			return;
		}
		if (VRC_EventHandler.HasEventTrigger(base.gameObject))
		{
			this.AddUseExit(player);
		}
		if (this.stationControls != null)
		{
			this.stationControls.controllingPlayer = player.GetComponent<VRC_PlayerApi>();
		}
		Vector3 position = Vector3.zero;
		Quaternion rhs = Quaternion.identity;
		if (player.isLocal)
		{
			VRCPlayer component = player.GetComponent<VRCPlayer>();
			component.AlignTrackingToPlayer();
			Transform trackingTransform = VRCTrackingManager.GetTrackingTransform();
			Vector3 position2 = trackingTransform.position;
			Quaternion rotation = trackingTransform.rotation;
			position = player.transform.InverseTransformPoint(trackingTransform.position);
			rhs = Quaternion.Inverse(player.transform.rotation) * rotation;
		}
		if (this.isSeated)
		{
			this.ApplySeatedAnimation(player);
		}
		this.SetEnterPlayerTransform(player.gameObject);
		player.currentStation = this;
		if (player.isLocal)
		{
			VRCTrackingManager.SetTrackingWorldOrigin(player.transform.TransformPoint(position), player.transform.rotation * rhs);
			if (this.isSeated)
			{
				VRCTrackingManager.UseAvatarStationViewPoint(true);
			}
			if (this.stationEnterPlayerLocation != null)
			{
				this.stationEnterInitialRotation = this.stationEnterPlayerLocation.rotation;
			}
			else
			{
				this.stationEnterInitialRotation = base.transform.rotation;
			}
			this.trackingOriginDeltaPosition = VRCTrackingManager.GetTrackingWorldOrigin() - player.transform.position;
			this.trackingOriginDeltaRotation = Quaternion.FromToRotation(player.transform.TransformDirection(Vector3.forward), VRCTrackingManager.GetTrackingTransform().TransformDirection(Vector3.forward));
			player.GetComponent<LocomotionInputController>().immobilize = this.isImmobilized;
			player.GetComponent<VRCMotionState>().IsSeated = this.isSeated;
			player.GetComponent<VRCMotionState>().InVehicle = this.inVehicle;
		}
		if (this.isImmobilized)
		{
			Collider component2 = player.GetComponent<Collider>();
			if (component2 != null)
			{
				component2.enabled = false;
			}
			if (player.isLocal)
			{
				this.AttachInputControllerTo(player, "ImmobileInputController");
			}
		}
		if (this.controlsObject != null)
		{
			VRC.Network.SetOwner(player, this.controlsObject.gameObject, VRC.Network.OwnershipModificationType.Request, false);
		}
		if (TutorialManager.Instance != null && player.isLocal && this.isImmobilized && !this.disableStationExit)
		{
			base.StartCoroutine(this.ShowGetUpInstructions());
		}
		VRC_StationInternal.AddActiveStation(this);
		base.SendMessage("OnStationEntered", SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x06005ABB RID: 23227 RVA: 0x001FA374 File Offset: 0x001F8774
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.Local,
		VRC_EventHandler.VrcTargetType.All
	})]
	public void ExitStation(VRC.Player sender)
	{
		if (this.Occupant != null && this.Occupant.isLocal && VRCUiManager.Instance != null && VRCUiManager.Instance.IsActive())
		{
			VRCUiManager.Instance.CloseUi(false);
		}
		if (sender != null && sender.isLocal && this.Occupant != null && this.Occupant.userId == sender.userId)
		{
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "InternalExitStation", new object[0]);
		}
	}

	// Token: 0x06005ABC RID: 23228 RVA: 0x001FA428 File Offset: 0x001F8828
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void InternalExitStation(VRC.Player player)
	{
		if (this.Occupant == null || player == null || player != this._occupant)
		{
			return;
		}
		VRC_StationInternal.FlagDiscontinuity(player);
		StationUseExit component = base.GetComponent<StationUseExit>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
		this.Occupant = null;
		if (this.animatorController != null || this.isSeated)
		{
			this.DetachAnimatorControllerFrom(player);
			if (player.isLocal)
			{
				VRCTrackingManager.UseAvatarStationViewPoint(false);
				LocomotionInputController component2 = player.GetComponent<LocomotionInputController>();
				if (component2 != null)
				{
					component2.immobilize = false;
				}
				VRCMotionState component3 = player.GetComponent<VRCMotionState>();
				if (component3 != null)
				{
					component3.IsSeated = false;
					component3.InVehicle = false;
				}
			}
		}
		if (this.stationControls != null)
		{
			this.stationControls.controllingPlayer = null;
		}
		Collider component4 = player.GetComponent<Collider>();
		if (component4 != null)
		{
			component4.enabled = true;
		}
		this.DetachInputControllerFrom(player.gameObject);
		this.ReleasePlayerTransform(player.gameObject);
		player.currentStation = null;
		if (TutorialManager.Instance != null && player.isLocal && this.isImmobilized)
		{
			TutorialManager.Instance.DeactivateObjectLabel(null);
			TutorialManager.Instance.DeactivateControllerLabel(ControllerHand.Left, ControllerInputUI.TrackpadTop);
		}
		if (player.isLocal)
		{
			VRCTrackingManager.RestoreLevel();
		}
		VRC_StationInternal.RemoveActiveStation(this);
		base.SendMessage("OnStationExited", SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x06005ABD RID: 23229 RVA: 0x001FA5B0 File Offset: 0x001F89B0
	private void AddUseExit(VRC.Player player)
	{
		if (player.isLocal && !this.disableStationExit)
		{
			StationUseExit stationUseExit = base.gameObject.AddComponent<StationUseExit>();
			stationUseExit.localUser = player.gameObject;
		}
	}

	// Token: 0x06005ABE RID: 23230 RVA: 0x001FA5EC File Offset: 0x001F89EC
	private void AttachAnimatorControllerTo(VRC.Player player, RuntimeAnimatorController rac)
	{
		AnimatorControllerManager componentInChildren = player.GetComponentInChildren<AnimatorControllerManager>();
		if (componentInChildren != null && rac != null)
		{
			componentInChildren.Push(rac);
		}
		else
		{
			Debug.LogError("Failed to attach animator controller to " + player.name + ".", this);
		}
	}

	// Token: 0x06005ABF RID: 23231 RVA: 0x001FA640 File Offset: 0x001F8A40
	private void AttachInputControllerTo(VRC.Player player, string inputControllerName)
	{
		if (player.isLocal)
		{
			InputStateControllerManager component = player.GetComponent<InputStateControllerManager>();
			if (component != null && this.isImmobilized)
			{
				component.PushInputController(inputControllerName);
			}
		}
	}

	// Token: 0x06005AC0 RID: 23232 RVA: 0x001FA67D File Offset: 0x001F8A7D
	private void SetEnterPlayerTransform(GameObject player)
	{
		if (this.stationEnterPlayerLocation != null)
		{
			player.transform.position = this.stationEnterPlayerLocation.position;
			player.transform.rotation = this.stationEnterPlayerLocation.rotation;
		}
	}

	// Token: 0x06005AC1 RID: 23233 RVA: 0x001FA6BC File Offset: 0x001F8ABC
	private void ReleasePlayerTransform(GameObject player)
	{
		if (this.stationExitPlayerLocation != null)
		{
			player.transform.position = this.stationExitPlayerLocation.position;
			Vector3 eulerAngles = this.stationExitPlayerLocation.rotation.eulerAngles;
			eulerAngles.x = (eulerAngles.z = 0f);
			player.transform.eulerAngles = eulerAngles;
		}
	}

	// Token: 0x06005AC2 RID: 23234 RVA: 0x001FA728 File Offset: 0x001F8B28
	private void DetachAnimatorControllerFrom(VRC.Player player)
	{
		AnimatorControllerManager componentInChildren = player.GetComponentInChildren<AnimatorControllerManager>();
		if (componentInChildren != null)
		{
			componentInChildren.Pop();
		}
	}

	// Token: 0x06005AC3 RID: 23235 RVA: 0x001FA750 File Offset: 0x001F8B50
	private void DetachInputControllerFrom(GameObject player)
	{
		InputStateControllerManager component = player.GetComponent<InputStateControllerManager>();
		if (component != null && this.isImmobilized)
		{
			component.PopInputController();
		}
	}

	// Token: 0x06005AC4 RID: 23236 RVA: 0x001FA784 File Offset: 0x001F8B84
	private IEnumerator ShowGetUpInstructions()
	{
		yield return new WaitForSeconds(0.1f);
		if (this.Occupant != null)
		{
			if (VRCInputManager.IsUsingHandController())
			{
				TutorialManager.Instance.ActivateControllerLabel(ControllerHand.Left, ControllerInputUI.TrackpadTop, "Get Up", 10f, 0);
			}
			else
			{
				TutorialManager.Instance.ActivateObjectLabel(null, TutorialLabelType.Popup, ControllerHand.None, "Get Up", ControllerActionUI.Move, string.Empty, ControllerActionUI.None, 10f, 0, AttachMode.PositionOnly, true);
			}
		}
		yield break;
	}

	// Token: 0x06005AC5 RID: 23237 RVA: 0x001FA7A0 File Offset: 0x001F8BA0
	private static void FlagDiscontinuity(VRC.Player player)
	{
		if (player == null)
		{
			return;
		}
		foreach (SyncPhysics syncPhysics in from sync in player.GetComponentsInChildren<SyncPhysics>()
		where sync != null
		select sync)
		{
			syncPhysics.DiscontinuityHint = true;
		}
	}

	// Token: 0x06005AC6 RID: 23238 RVA: 0x001FA828 File Offset: 0x001F8C28
	private void OnDestroy()
	{
		if (this._occupant != null)
		{
			this.ExitStation(this._occupant);
		}
	}

	// Token: 0x0400407A RID: 16506
	private static HashSet<VRC_StationInternal> activeStations = new HashSet<VRC_StationInternal>();

	// Token: 0x0400407B RID: 16507
	private VRC_Station baseStation;

	// Token: 0x0400407C RID: 16508
	private VRC_PropController stationControls;

	// Token: 0x0400407D RID: 16509
	private Vector3 trackingOriginDeltaPosition;

	// Token: 0x0400407E RID: 16510
	private Quaternion trackingOriginDeltaRotation;

	// Token: 0x0400407F RID: 16511
	private Quaternion stationEnterInitialRotation;

	// Token: 0x04004080 RID: 16512
	private VRC.Player _occupant;

	// Token: 0x04004081 RID: 16513
	public float moveTolerance = 1E-05f;
}
