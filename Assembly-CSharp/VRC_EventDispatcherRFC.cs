using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEngine;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000B6B RID: 2923
public class VRC_EventDispatcherRFC : VRC_EventDispatcher
{
	// Token: 0x17000D05 RID: 3333
	// (get) Token: 0x06005A06 RID: 23046 RVA: 0x001F4F05 File Offset: 0x001F3305
	// (set) Token: 0x06005A07 RID: 23047 RVA: 0x001F4F0C File Offset: 0x001F330C
	public static VRC_EventDispatcherRFC Instance { get; private set; }

	// Token: 0x17000D06 RID: 3334
	// (get) Token: 0x06005A08 RID: 23048 RVA: 0x001F4F14 File Offset: 0x001F3314
	private PhotonView photonView
	{
		get
		{
			if (this._photonView == null)
			{
				this._photonView = base.GetComponent<PhotonView>();
			}
			return this._photonView;
		}
	}

	// Token: 0x06005A09 RID: 23049 RVA: 0x001F4F39 File Offset: 0x001F3339
	private void Awake()
	{
		if (VRC_EventDispatcherRFC.Instance != null)
		{
			Debug.LogError("Duplicate instance of VRC_EventDispatcherRFC detected, replacing existing");
		}
		VRC_EventDispatcherRFC.Instance = this;
	}

	// Token: 0x06005A0A RID: 23050 RVA: 0x001F4F5B File Offset: 0x001F335B
	private void OnDestroy()
	{
		if (VRC_EventDispatcherRFC.Instance == this)
		{
			VRC_EventDispatcherRFC.Instance = null;
		}
	}

	// Token: 0x06005A0B RID: 23051 RVA: 0x001F4F74 File Offset: 0x001F3374
	public void Execute(VRC_EventHandler.VrcEvent e, VRC_EventHandler.VrcBroadcastType broadcast, int instagatorId, float fastForward)
	{
		string text = string.Format("<color=lightblue>Executing event {0} of type {1} ({2}, {3:F3}, {4}, {5}, {6}, {7}) for {8}</color>", new object[]
		{
			e.Name,
			e.EventType,
			e.ParameterBoolOp,
			e.ParameterFloat,
			e.ParameterInt,
			(!(e.ParameterObject == null)) ? e.ParameterObject.name : "null",
			e.ParameterString,
			(e.ParameterBytes != null) ? e.ParameterBytes.Length.ToString() : "0",
			instagatorId
		});
		Debug.Log(text);
		if (!Application.isEditor)
		{
			this.DebugPrint(text, new object[0]);
		}
		switch (e.EventType)
		{
		case VRC_EventHandler.VrcEventType.MeshVisibility:
			this.SetMeshVisibility(0L, broadcast, instagatorId, e.ParameterObject, e.ParameterBoolOp);
			break;
		case VRC_EventHandler.VrcEventType.AnimationFloat:
			this.SetAnimatorFloat(0L, broadcast, instagatorId, e.ParameterString, e.ParameterObject, e.ParameterFloat);
			break;
		case VRC_EventHandler.VrcEventType.AnimationBool:
			this.SetAnimatorBool(0L, broadcast, instagatorId, e.ParameterString, e.ParameterObject, e.ParameterBoolOp);
			break;
		case VRC_EventHandler.VrcEventType.AnimationTrigger:
			this.SetAnimatorTrigger(0L, broadcast, instagatorId, e.ParameterString, e.ParameterObject);
			break;
		case VRC_EventHandler.VrcEventType.AudioTrigger:
			this.TriggerAudioSource(0L, broadcast, instagatorId, e.ParameterObject, e.ParameterString, fastForward);
			break;
		case VRC_EventHandler.VrcEventType.PlayAnimation:
			this.PlayAnimation(0L, broadcast, instagatorId, e.ParameterString, e.ParameterObject, fastForward);
			break;
		case VRC_EventHandler.VrcEventType.SendMessage:
			this.SendMessage(0L, broadcast, instagatorId, e.ParameterObject, e.ParameterString);
			break;
		case VRC_EventHandler.VrcEventType.SetParticlePlaying:
			this.SetParticlePlaying(0L, broadcast, instagatorId, e.ParameterObject, e.ParameterBoolOp);
			break;
		case VRC_EventHandler.VrcEventType.TeleportPlayer:
			this.TeleportPlayer(0L, broadcast, instagatorId, e.ParameterObject, e.ParameterBoolOp);
			break;
		case VRC_EventHandler.VrcEventType.RunConsoleCommand:
			this.RunConsoleCommand(0L, broadcast, instagatorId, e.ParameterString);
			break;
		case VRC_EventHandler.VrcEventType.SetGameObjectActive:
			this.SetGameObjectActive(0L, broadcast, instagatorId, e.ParameterObject, e.ParameterBoolOp);
			break;
		case VRC_EventHandler.VrcEventType.SetWebPanelURI:
			this.SetWebPanelURI(0L, broadcast, instagatorId, e.ParameterObject, e.ParameterString);
			break;
		case VRC_EventHandler.VrcEventType.SetWebPanelVolume:
			this.SetWebPanelVolume(0L, broadcast, instagatorId, e.ParameterObject, e.ParameterFloat);
			break;
		case VRC_EventHandler.VrcEventType.SpawnObject:
			this.SpawnObject(0L, broadcast, instagatorId, e.ParameterObject, e.ParameterString, e.ParameterBytes);
			break;
		case VRC_EventHandler.VrcEventType.SendRPC:
			this.SendRPC(0L, broadcast, instagatorId, (VRC_EventHandler.VrcTargetType)e.ParameterInt, e.ParameterObject, e.ParameterString, e.ParameterBytes);
			break;
		case VRC_EventHandler.VrcEventType.ActivateCustomTrigger:
			this.ActivateCustomTrigger(0L, broadcast, instagatorId, e.ParameterObject, e.ParameterString);
			break;
		case VRC_EventHandler.VrcEventType.DestroyObject:
			this.DestroyObject(0L, broadcast, instagatorId, e.ParameterObject);
			break;
		case VRC_EventHandler.VrcEventType.SetLayer:
			this.SetLayer(0L, broadcast, instagatorId, e.ParameterObject, e.ParameterInt);
			break;
		case VRC_EventHandler.VrcEventType.SetMaterial:
			this.SetMaterial(0L, broadcast, instagatorId, e.ParameterObject, e.ParameterString);
			break;
		case VRC_EventHandler.VrcEventType.AddHealth:
			this.AddHealth(0L, broadcast, instagatorId, e.ParameterObject, e.ParameterFloat);
			break;
		case VRC_EventHandler.VrcEventType.AddDamage:
			this.AddDamage(0L, broadcast, instagatorId, e.ParameterObject, e.ParameterFloat);
			break;
		case VRC_EventHandler.VrcEventType.SetComponentActive:
			this.SetComponentActive(0L, broadcast, instagatorId, e.ParameterObject, e.ParameterString, e.ParameterBoolOp);
			break;
		case VRC_EventHandler.VrcEventType.AnimationInt:
			this.SetAnimatorInt(0L, broadcast, instagatorId, e.ParameterString, e.ParameterObject, e.ParameterInt);
			break;
		default:
			Debug.LogError("Unknown event type " + e.EventType.ToString());
			break;
		}
	}

	// Token: 0x06005A0C RID: 23052 RVA: 0x001F537C File Offset: 0x001F377C
	public static bool IsValidExecutor(VRC.Player player, VRC_EventHandler.VrcBroadcastType Broadcast, GameObject targetObject)
	{
		return Broadcast == VRC_EventHandler.VrcBroadcastType.Always || (Broadcast == VRC_EventHandler.VrcBroadcastType.Master && player != null && player.isMaster) || (Broadcast == VRC_EventHandler.VrcBroadcastType.Owner && player != null && VRC.Network.IsOwner(player, targetObject)) || Broadcast == VRC_EventHandler.VrcBroadcastType.AlwaysUnbuffered || (Broadcast == VRC_EventHandler.VrcBroadcastType.MasterUnbuffered && player != null && player.isMaster) || (Broadcast == VRC_EventHandler.VrcBroadcastType.OwnerUnbuffered && player != null && VRC.Network.IsOwner(player, targetObject)) || Broadcast == VRC_EventHandler.VrcBroadcastType.AlwaysBufferOne || (Broadcast == VRC_EventHandler.VrcBroadcastType.MasterBufferOne && player != null && player.isMaster) || (Broadcast == VRC_EventHandler.VrcBroadcastType.OwnerBufferOne && player != null && VRC.Network.IsOwner(player, targetObject)) || (Broadcast == VRC_EventHandler.VrcBroadcastType.Local && player != null && player.isLocal);
	}

	// Token: 0x06005A0D RID: 23053 RVA: 0x001F547C File Offset: 0x001F387C
	public VRC_EventHandler GetEventHandler(long CombinedNetworkId)
	{
		GameObject gameObject = VRC.Network.FindObjectByCombinedID(CombinedNetworkId);
		if (gameObject != null)
		{
			return VRC.Network.FindNearestEventHandler(gameObject);
		}
		return null;
	}

	// Token: 0x06005A0E RID: 23054 RVA: 0x001F54A4 File Offset: 0x001F38A4
	public override void ActivateCustomTrigger(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, GameObject triggerObject, string customName)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, triggerObject))
		{
			this._ActivateCustomTrigger(triggerObject, customName);
		}
	}

	// Token: 0x06005A0F RID: 23055 RVA: 0x001F54D0 File Offset: 0x001F38D0
	public void _ActivateCustomTrigger(GameObject triggerObject, string customName)
	{
		if (triggerObject == null)
		{
			Debug.LogError("Activate Custom Trigger could not find target.");
			return;
		}
		this.DebugPrint("<color=orange>{0}:{2} on ({1})</color>", new object[]
		{
			"ActivateCustomTrigger",
			triggerObject.name,
			customName
		});
		VRC_Trigger.TriggerCustom(triggerObject, customName);
	}

	// Token: 0x06005A10 RID: 23056 RVA: 0x001F5524 File Offset: 0x001F3924
	public override void SetMeshVisibility(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, GameObject MeshObject, VRC_EventHandler.VrcBooleanOp Vis)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, MeshObject))
		{
			this._SetMeshVisibility(MeshObject, Vis, playerByInstigatorID);
		}
	}

	// Token: 0x06005A11 RID: 23057 RVA: 0x001F5554 File Offset: 0x001F3954
	public void _SetMeshVisibility(GameObject MeshObject, VRC_EventHandler.VrcBooleanOp Vis, VRC.Player instigator)
	{
		if (MeshObject == null)
		{
			Debug.LogError("Mesh Visibility could not find target");
			return;
		}
		if (!VRC.Network.AllowOwnershipModification(instigator, MeshObject, VRC.Network.OwnershipModificationType.Request, true))
		{
			Debug.LogError(string.Concat(new object[]
			{
				instigator,
				" cannot change mesh visibility on ",
				MeshObject.name,
				" because they cannot modify it."
			}), MeshObject);
			return;
		}
		string format = "<color=orange>{0}:{2} on ({1})</color>";
		object[] array = new object[3];
		array[0] = "SetMeshVisibility";
		array[1] = MeshObject.name;
		int num = 2;
		VRC_EventHandler.VrcBooleanOp vrcBooleanOp = Vis;
		array[num] = vrcBooleanOp.ToString();
		this.DebugPrint(format, array);
		if (MeshObject.GetComponent<MeshRenderer>() != null)
		{
			MeshObject.GetComponent<MeshRenderer>().enabled = VRC_EventHandler.BooleanOp(Vis, MeshObject.GetComponent<MeshRenderer>().enabled);
		}
		if (MeshObject.GetComponent<SkinnedMeshRenderer>() != null)
		{
			MeshObject.GetComponent<SkinnedMeshRenderer>().enabled = VRC_EventHandler.BooleanOp(Vis, MeshObject.GetComponent<SkinnedMeshRenderer>().enabled);
		}
	}

	// Token: 0x06005A12 RID: 23058 RVA: 0x001F5644 File Offset: 0x001F3A44
	public override void SetComponentActive(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, GameObject targetObject, string componentTypeName, VRC_EventHandler.VrcBooleanOp enable)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, targetObject))
		{
			this._SetComponentActive(targetObject, componentTypeName, enable, playerByInstigatorID);
		}
	}

	// Token: 0x06005A13 RID: 23059 RVA: 0x001F5674 File Offset: 0x001F3A74
	public void _SetComponentActive(GameObject targetObject, string typeName, VRC_EventHandler.VrcBooleanOp enable, VRC.Player instigator)
	{
		if (targetObject == null)
		{
			Debug.LogError("Set Component Active could not find target");
			return;
		}
		if (!VRC.Network.AllowOwnershipModification(instigator, targetObject, VRC.Network.OwnershipModificationType.Request, true))
		{
			Debug.LogError(string.Concat(new object[]
			{
				instigator,
				" cannot change active state on ",
				targetObject.name,
				".",
				typeName,
				" because they cannot modify it."
			}), targetObject);
			return;
		}
		Type type = null;
		foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			type = assembly.GetType(typeName, false, true);
			if (type != null)
			{
				break;
			}
		}
		if (type == null)
		{
			Debug.LogError("Could not find Type of name " + typeName);
			return;
		}
		PropertyInfo property = type.GetProperty("enabled");
		if (property == null)
		{
			Debug.LogError(type.FullName + " does not respond to \"enabled\"");
			return;
		}
		string format = "<color=orange>{0}:{2} on ({1}) to {3}</color>";
		object[] array = new object[4];
		array[0] = "SetComponentActive";
		array[1] = targetObject.name;
		array[2] = typeName;
		int num = 3;
		VRC_EventHandler.VrcBooleanOp vrcBooleanOp = enable;
		array[num] = vrcBooleanOp.ToString();
		this.DebugPrint(format, array);
		foreach (Component obj in targetObject.GetComponents(type))
		{
			bool current = (bool)property.GetValue(obj, null);
			bool flag = VRC_EventHandler.BooleanOp(enable, current);
			property.SetValue(obj, flag, null);
		}
	}

	// Token: 0x06005A14 RID: 23060 RVA: 0x001F57EC File Offset: 0x001F3BEC
	public override void SetAnimatorBool(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string Name, VRC_EventHandler.VrcBooleanOp Value)
	{
		this.SetAnimatorBool(0L, Broadcast, Instigator, Name, null, Value);
	}

	// Token: 0x06005A15 RID: 23061 RVA: 0x001F57FD File Offset: 0x001F3BFD
	public override void SetAnimatorTrigger(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string Name)
	{
		this.SetAnimatorTrigger(0L, Broadcast, Instigator, Name, null);
	}

	// Token: 0x06005A16 RID: 23062 RVA: 0x001F580C File Offset: 0x001F3C0C
	public override void SetAnimatorFloat(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string Name, float Value)
	{
		this.SetAnimatorFloat(0L, Broadcast, Instigator, Name, null, Value);
	}

	// Token: 0x06005A17 RID: 23063 RVA: 0x001F581D File Offset: 0x001F3C1D
	public override void SetAnimatorInt(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string Name, int Value)
	{
		this.SetAnimatorInt(0L, Broadcast, Instigator, Name, null, Value);
	}

	// Token: 0x06005A18 RID: 23064 RVA: 0x001F5830 File Offset: 0x001F3C30
	public override void SetAnimatorBool(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string Name, GameObject destObject, VRC_EventHandler.VrcBooleanOp Value)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (destObject == null)
		{
			destObject = base.gameObject;
		}
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, destObject))
		{
			this._SetAnimatorBool(Name, destObject, Value, playerByInstigatorID);
		}
	}

	// Token: 0x06005A19 RID: 23065 RVA: 0x001F5874 File Offset: 0x001F3C74
	public void _SetAnimatorBool(string Name, GameObject go, VRC_EventHandler.VrcBooleanOp Value, VRC.Player sender)
	{
		if (go == null)
		{
			Debug.LogError("Set Animator Bool could not find target");
			return;
		}
		if (!VRC.Network.AllowOwnershipModification(sender, go, VRC.Network.OwnershipModificationType.Request, true))
		{
			Debug.LogError(string.Concat(new object[]
			{
				sender,
				" cannot change animator bool on ",
				go.name,
				" because they cannot modify it."
			}), go);
			return;
		}
		string format = "<color=orange>{0}:{2} on ({1}) to {3}</color>";
		object[] array = new object[4];
		array[0] = "SetAnimatorBool";
		array[1] = go.name;
		array[2] = Name;
		int num = 3;
		VRC_EventHandler.VrcBooleanOp vrcBooleanOp = Value;
		array[num] = vrcBooleanOp.ToString();
		this.DebugPrint(format, array);
		Animator animator = go.GetComponent<Animator>();
		if (animator == null)
		{
			animator = go.GetComponentInChildren<Animator>();
		}
		if (animator != null)
		{
			bool @bool = animator.GetBool(Name);
			animator.SetBool(Name, VRC_EventHandler.BooleanOp(Value, @bool));
		}
	}

	// Token: 0x06005A1A RID: 23066 RVA: 0x001F594C File Offset: 0x001F3D4C
	public override void SetAnimatorTrigger(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string Name, GameObject destObject)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (destObject == null)
		{
			destObject = base.gameObject;
		}
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, destObject))
		{
			this._SetAnimatorTrigger(Name, destObject, playerByInstigatorID);
		}
	}

	// Token: 0x06005A1B RID: 23067 RVA: 0x001F5990 File Offset: 0x001F3D90
	public void _SetAnimatorTrigger(string Name, GameObject go, VRC.Player sender)
	{
		if (go == null)
		{
			Debug.LogError("Set Animator Trigger could not find target");
			return;
		}
		if (!VRC.Network.AllowOwnershipModification(sender, go, VRC.Network.OwnershipModificationType.Request, true))
		{
			Debug.LogError(string.Concat(new object[]
			{
				sender,
				" cannot change animator trigger on ",
				go.name,
				" because they cannot modify it."
			}), go);
			return;
		}
		this.DebugPrint("<color=orange>{0}:{2} on ({1})</color>", new object[]
		{
			"SetAnimatorTrigger",
			go.name,
			Name
		});
		Animator animator = go.GetComponent<Animator>();
		if (animator == null)
		{
			animator = go.GetComponentInChildren<Animator>();
		}
		if (animator != null)
		{
			animator.SetTrigger(Name);
		}
	}

	// Token: 0x06005A1C RID: 23068 RVA: 0x001F5A44 File Offset: 0x001F3E44
	public override void SetAnimatorFloat(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string Name, GameObject destObject, float Value)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (destObject == null)
		{
			destObject = base.gameObject;
		}
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, destObject))
		{
			this._SetAnimatorFloat(0L, Name, destObject, Value, playerByInstigatorID);
		}
	}

	// Token: 0x06005A1D RID: 23069 RVA: 0x001F5A8C File Offset: 0x001F3E8C
	public void _SetAnimatorFloat(long CombinedNetworkId, string Name, GameObject go, float Value, VRC.Player sender)
	{
		if (go == null)
		{
			Debug.LogError("Set Animator Float could not find target");
			return;
		}
		if (!VRC.Network.AllowOwnershipModification(sender, go, VRC.Network.OwnershipModificationType.Request, true))
		{
			Debug.LogError(string.Concat(new object[]
			{
				sender,
				" cannot change animator float on ",
				go.name,
				" because they cannot modify it."
			}), go);
			return;
		}
		this.DebugPrint("<color=orange>{0}:{2} on ({1})</color>", new object[]
		{
			"SetAnimatorFloat",
			go.name,
			Name
		});
		Animator animator = go.GetComponent<Animator>();
		if (animator == null)
		{
			animator = go.GetComponentInChildren<Animator>();
		}
		if (animator != null)
		{
			animator.SetFloat(Name, Value);
		}
	}

	// Token: 0x06005A1E RID: 23070 RVA: 0x001F5B44 File Offset: 0x001F3F44
	public override void SetAnimatorInt(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string Name, GameObject destObject, int Value)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (destObject == null)
		{
			destObject = base.gameObject;
		}
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, destObject))
		{
			this._SetAnimatorInt(0L, Name, destObject, Value, playerByInstigatorID);
		}
	}

	// Token: 0x06005A1F RID: 23071 RVA: 0x001F5B8C File Offset: 0x001F3F8C
	public void _SetAnimatorInt(long CombinedNetworkId, string Name, GameObject go, int Value, VRC.Player sender)
	{
		if (go == null)
		{
			Debug.LogError("Set Animator Int could not find target");
			return;
		}
		if (!VRC.Network.AllowOwnershipModification(sender, go, VRC.Network.OwnershipModificationType.Request, true))
		{
			Debug.LogError(string.Concat(new object[]
			{
				sender,
				" cannot change animator int on ",
				go.name,
				" because they cannot modify it."
			}), go);
			return;
		}
		this.DebugPrint("<color=orange>{0}:{2} on ({1})</color>", new object[]
		{
			"SetAnimatorInt",
			go.name,
			Name
		});
		Animator animator = go.GetComponent<Animator>();
		if (animator == null)
		{
			animator = go.GetComponentInChildren<Animator>();
		}
		if (animator != null)
		{
			animator.SetInteger(Name, Value);
		}
	}

	// Token: 0x06005A20 RID: 23072 RVA: 0x001F5C44 File Offset: 0x001F4044
	public override void TriggerAudioSource(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, GameObject AudioSource, float fastForward = 0f)
	{
		this.TriggerAudioSource(0L, Broadcast, Instigator, AudioSource, string.Empty, fastForward);
	}

	// Token: 0x06005A21 RID: 23073 RVA: 0x001F5C5C File Offset: 0x001F405C
	public override void TriggerAudioSource(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, GameObject AudioSource, string clipName, float fastForward = 0f)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, AudioSource))
		{
			this._TriggerAudioSource(AudioSource, clipName, fastForward, playerByInstigatorID);
		}
	}

	// Token: 0x06005A22 RID: 23074 RVA: 0x001F5C8C File Offset: 0x001F408C
	public void _TriggerAudioSource(GameObject go, string clipName, float fastForward, VRC.Player sender)
	{
		if (fastForward < 0f)
		{
			return;
		}
		if (go == null)
		{
			Debug.LogError("Trigger Audio Source could not find target");
			return;
		}
		if (!VRC.Network.AllowOwnershipModification(sender, go, VRC.Network.OwnershipModificationType.Request, true))
		{
			Debug.LogError(string.Concat(new object[]
			{
				sender,
				" cannot trigger audio source on ",
				go.name,
				" because they cannot modify it."
			}), go);
			return;
		}
		this.DebugPrint("<color=orange>{0}:{2} on ({1})</color>", new object[]
		{
			"TriggerAudioSource",
			go.name,
			clipName
		});
		if (go.GetComponent<AudioSource>() != null)
		{
			AudioSource[] components = go.GetComponents<AudioSource>();
			Action<AudioSource> action = delegate(AudioSource source)
			{
				if (source.time + fastForward < source.clip.length || source.loop)
				{
					source.time += fastForward;
					source.Play();
				}
			};
			if (components != null && components.Length > 0)
			{
				if (!string.IsNullOrEmpty(clipName))
				{
					foreach (AudioSource obj in from s in components
					where s.clip != null && s.clip.name == clipName
					select s)
					{
						action(obj);
					}
				}
				else
				{
					foreach (AudioSource obj2 in components)
					{
						action(obj2);
					}
				}
			}
		}
	}

	// Token: 0x06005A23 RID: 23075 RVA: 0x001F5E14 File Offset: 0x001F4214
	public override void PlayAnimation(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string AnimationName, GameObject destObject, float fastForward = 0f)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, destObject))
		{
			this._PlayAnimation(AnimationName, destObject, fastForward, playerByInstigatorID);
		}
	}

	// Token: 0x06005A24 RID: 23076 RVA: 0x001F5E44 File Offset: 0x001F4244
	public void _PlayAnimation(string AnimationName, GameObject destObject, float fastForward, VRC.Player sender)
	{
		if (destObject == null)
		{
			Debug.LogError("Play Animation could not find target");
			return;
		}
		if (!VRC.Network.AllowOwnershipModification(sender, destObject, VRC.Network.OwnershipModificationType.Request, true))
		{
			Debug.LogError(string.Concat(new object[]
			{
				sender,
				" cannot play animation on ",
				destObject.name,
				" because they cannot modify it."
			}), destObject);
			return;
		}
		Animation component = destObject.gameObject.GetComponent<Animation>();
		if (component == null)
		{
			Debug.Log("Handler animatoin comp is null");
			return;
		}
		AnimationClip clip = component.GetClip(AnimationName);
		if (clip == null)
		{
			Debug.Log("Could not find animation clip: " + AnimationName);
			return;
		}
		this.DebugPrint("<color=orange>{0}:{2} on ({1})</color>", new object[]
		{
			"PlayAnimation",
			destObject.name,
			AnimationName
		});
		bool flag = clip.wrapMode == WrapMode.Loop || clip.wrapMode == WrapMode.PingPong;
		if (clip.length > fastForward || flag)
		{
			component.Play(AnimationName);
			component[AnimationName].time += fastForward;
		}
	}

	// Token: 0x06005A25 RID: 23077 RVA: 0x001F5F60 File Offset: 0x001F4360
	public override void SendMessage(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, GameObject DestObject, string MessageName)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, DestObject))
		{
			VRC_EventHandler.VrcTargetType targetType;
			switch (Broadcast)
			{
			case VRC_EventHandler.VrcBroadcastType.Always:
			case VRC_EventHandler.VrcBroadcastType.AlwaysUnbuffered:
			case VRC_EventHandler.VrcBroadcastType.AlwaysBufferOne:
				targetType = VRC_EventHandler.VrcTargetType.All;
				break;
			case VRC_EventHandler.VrcBroadcastType.Master:
			case VRC_EventHandler.VrcBroadcastType.MasterUnbuffered:
			case VRC_EventHandler.VrcBroadcastType.MasterBufferOne:
				targetType = VRC_EventHandler.VrcTargetType.Master;
				break;
			default:
				targetType = VRC_EventHandler.VrcTargetType.Local;
				break;
			case VRC_EventHandler.VrcBroadcastType.Owner:
			case VRC_EventHandler.VrcBroadcastType.OwnerUnbuffered:
			case VRC_EventHandler.VrcBroadcastType.OwnerBufferOne:
				targetType = VRC_EventHandler.VrcTargetType.Owner;
				break;
			}
			this.SendRPC(CombinedNetworkId, Broadcast, Instigator, targetType, DestObject, MessageName, null);
		}
	}

	// Token: 0x06005A26 RID: 23078 RVA: 0x001F5FE0 File Offset: 0x001F43E0
	public void ExecuteConsoleCommand(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string ConsoleCommand)
	{
	}

	// Token: 0x06005A27 RID: 23079 RVA: 0x001F5FE4 File Offset: 0x001F43E4
	public override void SetParticlePlaying(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, GameObject MeshObject, VRC_EventHandler.VrcBooleanOp Vis)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, MeshObject))
		{
			this._SetParticlePlaying(MeshObject, Vis, playerByInstigatorID);
		}
	}

	// Token: 0x06005A28 RID: 23080 RVA: 0x001F6014 File Offset: 0x001F4414
	public void _SetParticlePlaying(GameObject MeshObject, VRC_EventHandler.VrcBooleanOp Vis, VRC.Player sender)
	{
		if (MeshObject == null)
		{
			Debug.LogError("Set Particle Playing could not find target");
			return;
		}
		if (!VRC.Network.AllowOwnershipModification(sender, MeshObject, VRC.Network.OwnershipModificationType.Request, true))
		{
			Debug.LogError(string.Concat(new object[]
			{
				sender,
				" cannot set particle playing on ",
				MeshObject.name,
				" because they cannot modify it."
			}), MeshObject);
			return;
		}
		string format = "<color=orange>{0}:{2} on ({1})</color>";
		object[] array = new object[3];
		array[0] = "SetParticlePlaying";
		array[1] = MeshObject.name;
		int num = 2;
		VRC_EventHandler.VrcBooleanOp vrcBooleanOp = Vis;
		array[num] = vrcBooleanOp.ToString();
		this.DebugPrint(format, array);
		ParticleSystem component = MeshObject.GetComponent<ParticleSystem>();
		if (component != null)
		{
			bool flag = VRC_EventHandler.BooleanOp(Vis, component.isPlaying);
			if (flag)
			{
				component.Play();
			}
			else
			{
				component.Stop();
			}
		}
	}

	// Token: 0x06005A29 RID: 23081 RVA: 0x001F60E4 File Offset: 0x001F44E4
	public override void TeleportPlayer(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, GameObject Destination, VRC_EventHandler.VrcBooleanOp alignRoomToDestination)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, Destination))
		{
			this._TeleportPlayer(Instigator, Destination, (int)alignRoomToDestination, playerByInstigatorID);
		}
	}

	// Token: 0x06005A2A RID: 23082 RVA: 0x001F6114 File Offset: 0x001F4514
	public void _TeleportPlayer(int Instigator, GameObject Destination, int alignRoomToDestination, VRC.Player sender)
	{
		if (Destination == null || Destination.transform == null)
		{
			Debug.LogError("Teleport Player could not find target");
			return;
		}
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (playerByInstigatorID == null)
		{
			return;
		}
		if (!VRC.Network.AllowOwnershipModification(sender, playerByInstigatorID.gameObject, VRC.Network.OwnershipModificationType.Request, true))
		{
			Debug.LogError(sender + " cannot teleport players they cannot own.");
			return;
		}
		LocomotionInputController component = playerByInstigatorID.GetComponent<LocomotionInputController>();
		if (component == null)
		{
			return;
		}
		this.DebugPrint("<color=orange>{0} on ({1})</color>", new object[]
		{
			"TeleportPlayer",
			Destination.name
		});
		bool flag = VRC_EventHandler.BooleanOp((VRC_EventHandler.VrcBooleanOp)alignRoomToDestination, false);
		component.Teleport(Destination.transform.position, Destination.transform.rotation, (!flag) ? VRC_SceneDescriptor.SpawnOrientation.Default : VRC_SceneDescriptor.SpawnOrientation.AlignRoomWithSpawnPoint);
	}

	// Token: 0x06005A2B RID: 23083 RVA: 0x001F61EC File Offset: 0x001F45EC
	public override void RunConsoleCommand(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string ConsoleCommand)
	{
	}

	// Token: 0x06005A2C RID: 23084 RVA: 0x001F61F0 File Offset: 0x001F45F0
	public override void SetGameObjectActive(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, GameObject MeshObject, VRC_EventHandler.VrcBooleanOp Vis)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, MeshObject))
		{
			this._SetGameObjectActive(MeshObject, Vis, playerByInstigatorID);
		}
	}

	// Token: 0x06005A2D RID: 23085 RVA: 0x001F6220 File Offset: 0x001F4620
	public void _SetGameObjectActive(GameObject MeshObject, VRC_EventHandler.VrcBooleanOp Vis, VRC.Player sender)
	{
		if (MeshObject == null)
		{
			Debug.LogError("Set Game Object Active could not find target");
			return;
		}
		if (!VRC.Network.AllowOwnershipModification(sender, MeshObject, VRC.Network.OwnershipModificationType.Request, true))
		{
			Debug.LogError(string.Concat(new object[]
			{
				sender,
				" cannot change active state on ",
				MeshObject.name,
				" because they cannot modify it."
			}), MeshObject);
			return;
		}
		string format = "<color=orange>{0}:{2} on ({1})</color>";
		object[] array = new object[3];
		array[0] = "SetGameObjectActive";
		array[1] = MeshObject.name;
		int num = 2;
		VRC_EventHandler.VrcBooleanOp vrcBooleanOp = Vis;
		array[num] = vrcBooleanOp.ToString();
		this.DebugPrint(format, array);
		bool activeSelf = MeshObject.activeSelf;
		MeshObject.gameObject.SetActive(VRC_EventHandler.BooleanOp(Vis, activeSelf));
		if (MeshObject.gameObject.GetActive())
		{
			base.StartCoroutine(VRC.Network.CheckReady(MeshObject.gameObject));
		}
	}

	// Token: 0x06005A2E RID: 23086 RVA: 0x001F62F4 File Offset: 0x001F46F4
	public override void SetWebPanelURI(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, GameObject webPanelObject, string uri)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, webPanelObject))
		{
			this._SetWebPanelURI(webPanelObject, uri, playerByInstigatorID);
		}
	}

	// Token: 0x06005A2F RID: 23087 RVA: 0x001F6324 File Offset: 0x001F4724
	public void _SetWebPanelURI(GameObject webPanelObject, string uri, VRC.Player sender)
	{
		if (webPanelObject == null)
		{
			Debug.LogError("Set Web Panel URI could not find target");
			return;
		}
		if (!VRC.Network.AllowOwnershipModification(sender, webPanelObject, VRC.Network.OwnershipModificationType.Request, true))
		{
			Debug.LogError(sender + " cannot change URI on web panels they cannot own.");
			return;
		}
		this.DebugPrint("<color=orange>{0}:{2} on ({1})</color>", new object[]
		{
			"SetWebPanelURI",
			webPanelObject.name,
			uri
		});
		WebPanelInternal component = webPanelObject.GetComponent<WebPanelInternal>();
		if (component == null)
		{
			Debug.LogError("Could not locate WebPanel component");
		}
		else if (!component.ShouldSync && !sender.isLocal)
		{
			Debug.LogError(sender + " cannot set Web Panel URI on unsync'd panels");
		}
		else
		{
			component.NavigateTo(uri, false);
		}
	}

	// Token: 0x06005A30 RID: 23088 RVA: 0x001F63E4 File Offset: 0x001F47E4
	public override void SetWebPanelVolume(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, GameObject webPanelObject, float volume)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, webPanelObject))
		{
			this._SetWebPanelVolume(webPanelObject, volume, playerByInstigatorID);
		}
	}

	// Token: 0x06005A31 RID: 23089 RVA: 0x001F6414 File Offset: 0x001F4814
	public void _SetWebPanelVolume(GameObject webPanelObject, float volume, VRC.Player sender)
	{
		if (webPanelObject == null)
		{
			Debug.LogError("Set Web Panel Volume could not find target");
			return;
		}
		if (!VRC.Network.AllowOwnershipModification(sender, webPanelObject, VRC.Network.OwnershipModificationType.Request, true))
		{
			Debug.LogError(string.Concat(new object[]
			{
				sender,
				" cannot set volume on ",
				webPanelObject.name,
				" because they cannot modify it."
			}), webPanelObject);
			return;
		}
		this.DebugPrint("<color=orange>{0}:{2} on ({1})</color>", new object[]
		{
			"SetWebPanelVolume",
			webPanelObject.name,
			volume
		});
		WebPanelInternal component = webPanelObject.GetComponent<WebPanelInternal>();
		if (component == null)
		{
			Debug.LogError("Could not locate WebPanel component");
		}
		else if (!component.ShouldSync && !sender.isLocal)
		{
			Debug.LogError(sender + " cannot set Web Panel Volume on unsync'd panels");
		}
		else
		{
			component.SetVolume(volume);
		}
	}

	// Token: 0x06005A32 RID: 23090 RVA: 0x001F64F8 File Offset: 0x001F48F8
	public override void SpawnObject(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, GameObject location, string prefabName, byte[] data)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (location == null)
		{
			location = ((!(base.gameObject.GetComponent<ObjectInstantiator>() != null)) ? VRC.Network.SceneEventHandler.gameObject : base.gameObject);
		}
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, location))
		{
			this._SpawnObject(Broadcast, location, prefabName, data);
		}
	}

	// Token: 0x06005A33 RID: 23091 RVA: 0x001F6564 File Offset: 0x001F4964
	public void _SpawnObject(VRC_EventHandler.VrcBroadcastType Broadcast, GameObject location, string prefabName, byte[] data)
	{
		GameObject gameObject = (!(location == null)) ? location : base.gameObject;
		this.DebugPrint("<color=orange>{0}:{2} on ({1})</color>", new object[]
		{
			"SpawnObject",
			gameObject.name,
			prefabName
		});
		Vector3 position = gameObject.transform.position;
		Quaternion rotation = gameObject.transform.rotation;
		VRC.Network.Instantiate(Broadcast, prefabName, position, rotation, gameObject.GetComponent<ObjectInstantiator>());
	}

	// Token: 0x06005A34 RID: 23092 RVA: 0x001F65D8 File Offset: 0x001F49D8
	public override void SendRPC(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, VRC_EventHandler.VrcTargetType targetType, GameObject targetObject, string rpcMethodName, byte[] parameters)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, targetObject))
		{
			try
			{
				VRC.Network.ProcessRPC(targetType, playerByInstigatorID, targetObject, rpcMethodName, parameters);
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Caught exception while sending RPC: \n{0}\n\n{1}", new object[]
				{
					ex.Message,
					ex.StackTrace
				});
			}
		}
	}

	// Token: 0x06005A35 RID: 23093 RVA: 0x001F6648 File Offset: 0x001F4A48
	public override void DestroyObject(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, GameObject targetObject)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, targetObject))
		{
			this._DestroyObject(targetObject, playerByInstigatorID);
		}
	}

	// Token: 0x06005A36 RID: 23094 RVA: 0x001F6673 File Offset: 0x001F4A73
	public void _DestroyObject(GameObject targetObject, VRC.Player sender)
	{
		if (targetObject == null)
		{
			return;
		}
		if (VRC.Network.AllowOwnershipModification(sender, targetObject, VRC.Network.OwnershipModificationType.Request, true))
		{
			UnityEngine.Object.Destroy(targetObject);
		}
		else
		{
			Debug.LogError(sender + " cannot destroy objects not owned by requester.");
		}
	}

	// Token: 0x06005A37 RID: 23095 RVA: 0x001F66AC File Offset: 0x001F4AAC
	public override void SetLayer(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, GameObject targetObject, int Layer)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, targetObject))
		{
			this._SetLayer(targetObject, Layer, playerByInstigatorID);
		}
	}

	// Token: 0x06005A38 RID: 23096 RVA: 0x001F66DC File Offset: 0x001F4ADC
	public void _SetLayer(GameObject targetObject, int Layer, VRC.Player sender)
	{
		if (targetObject == null)
		{
			Debug.LogError("Set Layer could not find target");
			return;
		}
		if (!VRC.Network.AllowOwnershipModification(sender, targetObject, VRC.Network.OwnershipModificationType.Request, true))
		{
			Debug.LogError(string.Concat(new object[]
			{
				sender,
				" cannot set layer on ",
				targetObject.name,
				" because they cannot modify it."
			}), targetObject);
			return;
		}
		this.DebugPrint("<color=orange>{0}:{2} on ({1})</color>", new object[]
		{
			"SetLayer",
			targetObject.name,
			Layer
		});
		targetObject.layer = Layer;
	}

	// Token: 0x06005A39 RID: 23097 RVA: 0x001F6770 File Offset: 0x001F4B70
	public override void SetMaterial(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, GameObject targetObject, string materialName)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, targetObject))
		{
			this._SetMaterial(targetObject, materialName, playerByInstigatorID);
		}
	}

	// Token: 0x06005A3A RID: 23098 RVA: 0x001F67A0 File Offset: 0x001F4BA0
	public void _SetMaterial(GameObject targetObject, string materialName, VRC.Player sender)
	{
		if (targetObject == null)
		{
			Debug.LogError("Set Material could not find target");
			return;
		}
		if (!VRC.Network.AllowOwnershipModification(sender, targetObject, VRC.Network.OwnershipModificationType.Request, true))
		{
			Debug.LogError(string.Concat(new object[]
			{
				sender,
				" cannot set material on ",
				targetObject.name,
				" because they cannot modify it."
			}), targetObject);
			return;
		}
		this.DebugPrint("<color=orange>{0}:{2} on ({1})</color>", new object[]
		{
			"SetMaterial",
			targetObject.name,
			materialName
		});
		Material material = VRC_SceneDescriptor.GetMaterial(materialName);
		if (material == null)
		{
			material = (Material)Resources.Load(materialName, typeof(Material));
		}
		if (material != null)
		{
			foreach (Renderer renderer in targetObject.GetComponents<Renderer>())
			{
				renderer.sharedMaterial = material;
			}
		}
	}

	// Token: 0x06005A3B RID: 23099 RVA: 0x001F6884 File Offset: 0x001F4C84
	public override void AddHealth(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, GameObject targetObject, float health)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, targetObject))
		{
			this._AddHealth(targetObject, health, playerByInstigatorID);
		}
	}

	// Token: 0x06005A3C RID: 23100 RVA: 0x001F68B4 File Offset: 0x001F4CB4
	public void _AddHealth(GameObject targetObject, float health, VRC.Player sender)
	{
		if (targetObject == null)
		{
			targetObject = VRC.Network.LocalPlayer.gameObject;
		}
		if (!VRC.Network.AllowOwnershipModification(sender, targetObject, VRC.Network.OwnershipModificationType.Request, true))
		{
			Debug.LogError(sender.name + " cannot add health on " + targetObject.name + " because they cannot modify it.", targetObject);
			return;
		}
		this.DebugPrint("<color=orange>{0}:{2} on ({1})</color>", new object[]
		{
			"AddHealth",
			targetObject.name,
			health
		});
		PlayerModComponentHealth component = targetObject.GetComponent<PlayerModComponentHealth>();
		if (component != null)
		{
			component.AddHealth(health);
		}
		VRC_DestructibleStandard component2 = targetObject.GetComponent<VRC_DestructibleStandard>();
		if (component2 != null)
		{
			component2.ApplyHealing(health);
		}
	}

	// Token: 0x06005A3D RID: 23101 RVA: 0x001F6968 File Offset: 0x001F4D68
	public override void AddDamage(long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, GameObject targetObject, float damage)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(Instigator);
		if (VRC_EventDispatcherRFC.IsValidExecutor(playerByInstigatorID, Broadcast, targetObject))
		{
			this._AddDamage(targetObject, damage, playerByInstigatorID);
		}
	}

	// Token: 0x06005A3E RID: 23102 RVA: 0x001F6998 File Offset: 0x001F4D98
	public void _AddDamage(GameObject targetObject, float damage, VRC.Player sender)
	{
		if (targetObject == null)
		{
			targetObject = VRC.Network.LocalPlayer.gameObject;
		}
		if (!VRC.Network.AllowOwnershipModification(sender, targetObject, VRC.Network.OwnershipModificationType.Request, true))
		{
			Debug.LogError(string.Concat(new object[]
			{
				sender,
				" cannot add damager on ",
				targetObject.name,
				" because they cannot modify it."
			}), targetObject);
			return;
		}
		this.DebugPrint("<color=orange>{0}:{2} on ({1})</color>", new object[]
		{
			"AddDamage",
			targetObject.name,
			damage
		});
		PlayerModComponentHealth component = targetObject.GetComponent<PlayerModComponentHealth>();
		if (component != null)
		{
			component.RemoveHealth(damage);
		}
		VRC_DestructibleStandard component2 = targetObject.GetComponent<VRC_DestructibleStandard>();
		if (component2 != null)
		{
			component2.ApplyDamage(damage);
		}
	}

	// Token: 0x06005A3F RID: 23103 RVA: 0x001F6A5C File Offset: 0x001F4E5C
	public override string GetGameObjectPath(GameObject go)
	{
		if (go == null)
		{
			return string.Empty;
		}
		string text = string.Empty;
		while (go != null)
		{
			long? combinedID = VRC.Network.GetCombinedID(go);
			if (combinedID != null)
			{
				text = ":" + combinedID.Value.ToString("X") + "/" + text;
				break;
			}
			if (text == string.Empty)
			{
				text = go.name;
			}
			else
			{
				text = go.name + "/" + text;
			}
			if (go.transform.parent == null)
			{
				text = "/" + text;
				break;
			}
			go = go.transform.parent.gameObject;
		}
		return text;
	}

	// Token: 0x06005A40 RID: 23104 RVA: 0x001F6B37 File Offset: 0x001F4F37
	public override GameObject FindGameObject(string path)
	{
		return this.FindGameObject(path, false);
	}

	// Token: 0x06005A41 RID: 23105 RVA: 0x001F6B44 File Offset: 0x001F4F44
	public override GameObject FindGameObject(string path, bool suppressErrors)
	{
		if (string.IsNullOrEmpty(path))
		{
			return null;
		}
		path = path.Trim();
		GameObject gameObject = null;
		if (path.StartsWith(":"))
		{
			try
			{
				int num = path.IndexOf("/");
				string s = path.Substring(1, num - 1);
				long id = long.Parse(s, NumberStyles.HexNumber);
				GameObject gameObject2 = VRC.Network.FindObjectByCombinedID(id);
				if (gameObject2 != null)
				{
					path = path.Substring(num + 1);
					if (string.IsNullOrEmpty(path))
					{
						gameObject = gameObject2;
					}
					else
					{
						gameObject = VRC_EventDispatcherLocal.FindChild(gameObject2, path.Split(new char[]
						{
							'/'
						}));
					}
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				return null;
			}
		}
		else if (path.StartsWith("/"))
		{
			List<string> list = path.Substring(1).Split(new char[]
			{
				'/'
			}).ToList<string>();
			IEnumerable<GameObject> rootGameObjects = AssetManagement.RootGameObjects;
			foreach (GameObject gameObject3 in rootGameObjects)
			{
				if (gameObject3.name == list[0])
				{
					gameObject = VRC_EventDispatcherLocal.FindChild(gameObject3, list.Skip(1));
					break;
				}
			}
		}
		if (gameObject == null)
		{
			gameObject = GameObject.Find(path);
		}
		if (gameObject == null && !suppressErrors)
		{
			Debug.LogError("Could not locate object with path " + path);
		}
		return gameObject;
	}

	// Token: 0x06005A42 RID: 23106 RVA: 0x001F6CF4 File Offset: 0x001F50F4
	private bool IsHandlerTarget(GameObject obj, VRC_EventHandler.VrcEventType type)
	{
		return this.handlers.Any((VRC_EventHandler h) => h.Events != null && h.Events.Any((VRC_EventHandler.VrcEvent e) => e.EventType == type && (e.ParameterObject == obj || e.ParameterObjects.Contains(obj))));
	}

	// Token: 0x06005A43 RID: 23107 RVA: 0x001F6D2C File Offset: 0x001F512C
	public override void RegisterEventHandler(VRC_EventHandler handler)
	{
		if (!this.handlers.Contains(handler))
		{
			this.handlers.Add(handler);
		}
	}

	// Token: 0x06005A44 RID: 23108 RVA: 0x001F6D4C File Offset: 0x001F514C
	public override void UnregisterEventHandler(VRC_EventHandler handler)
	{
		this.handlers.RemoveAll((VRC_EventHandler e) => e == handler || e == null);
	}

	// Token: 0x04004026 RID: 16422
	private PhotonView _photonView;

	// Token: 0x04004027 RID: 16423
	private List<VRC_EventHandler> handlers = new List<VRC_EventHandler>();
}
