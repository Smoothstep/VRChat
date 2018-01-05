using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using ExitGames.Client.Photon;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000B6C RID: 2924
[RequireComponent(typeof(VRC_EventDispatcherRFC))]
[RequireComponent(typeof(ObjectInstantiator))]
public class VRC_EventLog : VRCPunBehaviour
{
	// Token: 0x17000D07 RID: 3335
	// (get) Token: 0x06005A46 RID: 23110 RVA: 0x001F6EA9 File Offset: 0x001F52A9
	// (set) Token: 0x06005A47 RID: 23111 RVA: 0x001F6EB4 File Offset: 0x001F52B4
	public bool ShouldProcessEvents
	{
		get
		{
			return this._shouldProcessEvents;
		}
		set
		{
			if (value && !this._shouldProcessEvents)
			{
				foreach (VRC_EventLog.EventLogEntry eventLogEntry in this.eventsToProcess)
				{
					eventLogEntry.TimeReceived = (double)Time.time;
				}
			}
			this._shouldProcessEvents = value;
		}
	}

	// Token: 0x17000D08 RID: 3336
	// (get) Token: 0x06005A48 RID: 23112 RVA: 0x001F6F30 File Offset: 0x001F5330
	public float DeferNewEventsTolerance
	{
		get
		{
			return 60f;
		}
	}

	// Token: 0x17000D09 RID: 3337
	// (get) Token: 0x06005A49 RID: 23113 RVA: 0x001F6F38 File Offset: 0x001F5338
	public static bool IsFinishedInitialLoad
	{
		get
		{
			return VRC_EventLog.Instance != null && PhotonNetwork.isMessageQueueRunning && PhotonNetwork.inRoom && (PhotonNetwork.isMasterClient || VRC_EventLog.Instance.Replicator.HasReceivedInitialEvents);
		}
	}

	// Token: 0x17000D0A RID: 3338
	// (get) Token: 0x06005A4A RID: 23114 RVA: 0x001F6F88 File Offset: 0x001F5388
	public List<VRC_EventLog.EventLogEntry> RecordedEvents
	{
		get
		{
			List<VRC_EventLog.EventLogEntry> list = this.Replicator.RecordedEvents.Cast<VRC_EventLog.EventLogEntry>().ToList<VRC_EventLog.EventLogEntry>();
			list.Sort((VRC_EventLog.EventLogEntry a, VRC_EventLog.EventLogEntry b) => a.Time.CompareTo(b.Time));
			return list;
		}
	}

	// Token: 0x06005A4B RID: 23115 RVA: 0x001F6FCF File Offset: 0x001F53CF
	public override void Awake()
	{
		base.Awake();
		if (VRC_EventLog.Instance != null)
		{
			Debug.LogError("Two Event Logs!");
			UnityEngine.Object.Destroy(this);
			return;
		}
		VRC_EventLog.Instance = this;
		this.Reset();
	}

	// Token: 0x06005A4C RID: 23116 RVA: 0x001F7004 File Offset: 0x001F5404
	public void Reset()
	{
		this.Replicator = base.gameObject.GetComponent<VRC_EventLog.EventReplicator>();
		if (this.Replicator != null)
		{
			UnityEngine.Object.DestroyImmediate(this.Replicator);
		}
		this.Replicator = base.gameObject.AddComponent<VRC_EventLog.EventReplicator>();
		VRC_EventLog.EventReplicator replicator = this.Replicator;
		replicator.OnNewEvent = (EventReplicator<VRC_EventLog.EventLogEntry, VRC_EventLog.EventLogEntry.BufferedEqualityComparer>.NewEventHandler)Delegate.Combine(replicator.OnNewEvent, new EventReplicator<VRC_EventLog.EventLogEntry, VRC_EventLog.EventLogEntry.BufferedEqualityComparer>.NewEventHandler(this.OnNewEvent));
		this.Dispatcher = base.GetComponent<VRC_EventDispatcherRFC>();
		this.Replicator.Dispatcher = this.Dispatcher;
		this.eventsToProcess = new List<VRC_EventLog.EventLogEntry>();
		VRC_EventLog.EventLogEntry.RegisterSerialization();
		this.ShouldProcessEvents = false;
	}

	// Token: 0x06005A4D RID: 23117 RVA: 0x001F70AA File Offset: 0x001F54AA
	public void OnSceneWasLoaded()
	{
		this.Reset();
	}

	// Token: 0x06005A4E RID: 23118 RVA: 0x001F70B2 File Offset: 0x001F54B2
	private void OnDestroy()
	{
		Debug.Log("<color=green>Event Log destroyed</color>");
		if (VRC_EventLog.Instance == this)
		{
			VRC_EventLog.Instance = null;
		}
	}

	// Token: 0x06005A4F RID: 23119 RVA: 0x001F70D4 File Offset: 0x001F54D4
	private void OnNewEvent(VRC_EventLog.EventLogEntry entry, VRC.Player sender)
	{
		VRC_EventLog.AssignObjectParameter(entry, this.Dispatcher);
		entry.TimeReceived = (double)Time.time;
		if (VRC.Network.IsNetworkSettled && entry.Instigator == VRC.Network.LocalInstigatorID && VRC_EventLog.isEventReady(entry, false))
		{
			this.Dispatcher.Execute(entry.Event, entry.OriginalBroadcast, entry.Instigator, entry.FastForward);
		}
		else
		{
			this.eventsToProcess.Add(entry);
		}
	}

	// Token: 0x06005A50 RID: 23120 RVA: 0x001F7154 File Offset: 0x001F5554
	public void LogEvent(VRC_EventHandler eventHandler, VRC_EventHandler.VrcEvent vrcEvent, long combinedNetworkId, VRC_EventHandler.VrcBroadcastType broadcast, int instagatorId, float fastForward)
	{
		if (instagatorId <= 0)
		{
			instagatorId = VRC.Network.LocalInstigatorID;
		}
		if (vrcEvent.EventType == VRC_EventHandler.VrcEventType.SpawnObject || broadcast == VRC_EventHandler.VrcBroadcastType.Local)
		{
			this.Dispatcher.Execute(vrcEvent, broadcast, instagatorId, fastForward);
			return;
		}
		VRC_EventLog.EventLogEntry eventLogEntry = new VRC_EventLog.EventLogEntry
		{
			Time = PhotonNetwork.time,
			InstagatorPhotonId = instagatorId,
			CombinedNetworkId = combinedNetworkId,
			FastForward = fastForward,
			Event = vrcEvent,
			OriginalBroadcast = broadcast
		}.DeepClone();
		if (vrcEvent.ParameterObject != null)
		{
			eventLogEntry.ObjectPath = VRC.Network.GetGameObjectPath(vrcEvent.ParameterObject);
			eventLogEntry.Event.ParameterObject = vrcEvent.ParameterObject;
		}
		this.FixBooleanValues(eventLogEntry.Event, this.Dispatcher.GetEventHandler(combinedNetworkId));
		VRC_EventLog.AssignObjectParameter(eventLogEntry, this.Dispatcher);
		Debug.Log("Replicating " + eventLogEntry.ToString());
		this.Replicator.ProcessEvent(eventLogEntry, PhotonNetwork.player);
	}

	// Token: 0x06005A51 RID: 23121 RVA: 0x001F7254 File Offset: 0x001F5654
	public int RemoveEventsIf(Func<VRC_EventLog.EventLogEntry, bool> predicate)
	{
		return this.Replicator.RemoveEventsIf((VRC_EventLog.EventLogEntry entry) => predicate(entry));
	}

	// Token: 0x06005A52 RID: 23122 RVA: 0x001F7285 File Offset: 0x001F5685
	public bool RequestPastEvents()
	{
		return this.Replicator.RequestPastEvents();
	}

	// Token: 0x06005A53 RID: 23123 RVA: 0x001F7292 File Offset: 0x001F5692
	private void Update()
	{
		if (this.ShouldProcessEvents)
		{
			this.ProcessEvents();
		}
	}

	// Token: 0x06005A54 RID: 23124 RVA: 0x001F72A8 File Offset: 0x001F56A8
	private static bool isEventReady(VRC_EventLog.EventLogEntry e, bool logError)
	{
		if (!string.IsNullOrEmpty(e.ObjectPath) && e.Event.ParameterObject == null)
		{
			if (logError)
			{
				Debug.LogError("Could not find target " + e.ObjectPath + " for " + e.ToString());
			}
			return false;
		}
		if (e.Event.ParameterObject != null && !VRC.Network.IsObjectReady(e.Event.ParameterObject))
		{
			if (logError)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Object did not ready in time ",
					e.ObjectPath,
					" for ",
					e.ToString(),
					" because ",
					e.Event.ParameterObject.WhyNotReady()
				}));
			}
			return false;
		}
		if (e.Event.EventType == VRC_EventHandler.VrcEventType.SendRPC)
		{
			object[] array = null;
			try
			{
				array = VRC_Serialization.ParameterDecoder(e.Event.ParameterBytes, true);
			}
			catch (KeyNotFoundException ex)
			{
				if (logError)
				{
					Debug.LogError(string.Concat(new string[]
					{
						"RPC Parameters did not ready in time ",
						e.ObjectPath,
						" for ",
						e.ToString(),
						" because ",
						ex.Message
					}));
				}
				return false;
			}
			if (array == null || array.Length <= 0)
			{
				return true;
			}
			if (array.Any((object p) => p == null || (p is GameObject && !(p as GameObject).IsReady())))
			{
				if (logError)
				{
					string[] value = (from p in array
					where p == null || (p is GameObject && !(p as GameObject).IsReady())
					select (p != null) ? ((p as GameObject).name + ": " + (p as GameObject).WhyNotReady()) : "parameter is null").ToArray<string>();
					Debug.LogError(string.Concat(new string[]
					{
						"RPC Parameters did not ready in time ",
						e.ObjectPath,
						" for ",
						e.ToString(),
						" because ",
						string.Join(", ", value)
					}));
				}
				return false;
			}
			return true;
		}
		return true;
	}

	// Token: 0x06005A55 RID: 23125 RVA: 0x001F74E4 File Offset: 0x001F58E4
	private void ProcessEvents()
	{
		int count = this.eventsToProcess.Count;
		for (int i = 0; i < count; i++)
		{
			VRC_EventLog.EventLogEntry eventLogEntry = this.eventsToProcess[0];
			this.eventsToProcess.RemoveAt(0);
			if (eventLogEntry != null)
			{
				try
				{
					VRC_EventLog.AssignObjectParameter(eventLogEntry, this.Dispatcher);
					bool flag = (double)Time.time - eventLogEntry.TimeReceived < (double)this.DeferNewEventsTolerance;
					if (!VRC_EventLog.isEventReady(eventLogEntry, !flag))
					{
						if (flag)
						{
							this.eventsToProcess.Add(eventLogEntry);
						}
					}
					else
					{
						try
						{
							float fastForward = eventLogEntry.FastForward + (float)(PhotonNetwork.time - eventLogEntry.Time);
							this.Dispatcher.Execute(eventLogEntry.Event, eventLogEntry.OriginalBroadcast, eventLogEntry.InstagatorPhotonId, fastForward);
						}
						catch (Exception exception)
						{
							Debug.LogException(exception);
						}
					}
				}
				catch (Exception exception2)
				{
					Debug.LogException(exception2, this);
				}
			}
		}
	}

	// Token: 0x06005A56 RID: 23126 RVA: 0x001F75F8 File Offset: 0x001F59F8
	private void FixBooleanValues(VRC_EventHandler.VrcEvent evt, VRC_EventHandler handler)
	{
		if (evt == null || handler == null)
		{
			return;
		}
		if (evt.ParameterObject != null)
		{
			VRC_EventHandler.VrcEventType eventType = evt.EventType;
			switch (eventType)
			{
			case VRC_EventHandler.VrcEventType.SetParticlePlaying:
			{
				Transform transform = evt.ParameterObject.transform;
				if (transform != null)
				{
					ParticleSystem component = transform.GetComponent<ParticleSystem>();
					if (component != null)
					{
						bool flag = VRC_EventHandler.BooleanOp(evt.ParameterBoolOp, component.isPlaying);
						evt.ParameterBoolOp = ((!flag) ? VRC_EventHandler.VrcBooleanOp.False : VRC_EventHandler.VrcBooleanOp.True);
					}
				}
				break;
			}
			case VRC_EventHandler.VrcEventType.TeleportPlayer:
			{
				Transform transform2 = evt.ParameterObject.transform;
				if (transform2 != null)
				{
					bool flag2 = VRC_EventHandler.BooleanOp(evt.ParameterBoolOp, false);
					evt.ParameterBoolOp = ((!flag2) ? VRC_EventHandler.VrcBooleanOp.False : VRC_EventHandler.VrcBooleanOp.True);
				}
				break;
			}
			default:
				switch (eventType)
				{
				case VRC_EventHandler.VrcEventType.MeshVisibility:
				{
					Transform transform3 = evt.ParameterObject.transform;
					if (transform3 != null)
					{
						MeshRenderer component2 = transform3.GetComponent<MeshRenderer>();
						if (component2 != null)
						{
							bool flag3 = VRC_EventHandler.BooleanOp(evt.ParameterBoolOp, component2.enabled);
							evt.ParameterBoolOp = ((!flag3) ? VRC_EventHandler.VrcBooleanOp.False : VRC_EventHandler.VrcBooleanOp.True);
						}
					}
					break;
				}
				case VRC_EventHandler.VrcEventType.AnimationBool:
				{
					Animator animator = (!(evt.ParameterObject != null)) ? null : evt.ParameterObject.GetComponent<Animator>();
					if (animator == null && handler != null)
					{
						animator = handler.gameObject.GetComponent<Animator>();
					}
					if (animator != null)
					{
						bool flag4 = VRC_EventHandler.BooleanOp(evt.ParameterBoolOp, animator.GetBool(evt.ParameterString));
						evt.ParameterBoolOp = ((!flag4) ? VRC_EventHandler.VrcBooleanOp.False : VRC_EventHandler.VrcBooleanOp.True);
					}
					break;
				}
				}
				break;
			case VRC_EventHandler.VrcEventType.SetGameObjectActive:
			{
				Transform transform4 = evt.ParameterObject.transform;
				if (transform4 != null)
				{
					bool flag5 = VRC_EventHandler.BooleanOp(evt.ParameterBoolOp, transform4.gameObject.activeSelf);
					evt.ParameterBoolOp = ((!flag5) ? VRC_EventHandler.VrcBooleanOp.False : VRC_EventHandler.VrcBooleanOp.True);
				}
				break;
			}
			}
		}
	}

	// Token: 0x06005A57 RID: 23127 RVA: 0x001F7830 File Offset: 0x001F5C30
	private static void AssignObjectParameter(VRC_EventLog.EventLogEntry entry, VRC_EventDispatcherRFC Dispatcher)
	{
		if (entry != null && entry.Event != null && entry.Event.ParameterObject == null && !string.IsNullOrEmpty(entry.ObjectPath))
		{
			GameObject gameObject = VRC.Network.FindGameObject(entry.ObjectPath, true);
			if (gameObject != null)
			{
				entry.Event.ParameterObject = gameObject;
			}
		}
	}

	// Token: 0x04004028 RID: 16424
	public bool _shouldProcessEvents;

	// Token: 0x04004029 RID: 16425
	public VRC_EventLog.EventReplicator Replicator;

	// Token: 0x0400402A RID: 16426
	public VRC_EventDispatcherRFC Dispatcher;

	// Token: 0x0400402B RID: 16427
	private List<VRC_EventLog.EventLogEntry> eventsToProcess = new List<VRC_EventLog.EventLogEntry>();

	// Token: 0x0400402C RID: 16428
	public static VRC_EventLog Instance;

	// Token: 0x02000B6D RID: 2925
	public class EventReplicator : EventReplicator<VRC_EventLog.EventLogEntry, VRC_EventLog.EventLogEntry.BufferedEqualityComparer>
	{
		// Token: 0x06005A5E RID: 23134 RVA: 0x001F7945 File Offset: 0x001F5D45
		[PunRPC]
		protected override void SendPastEvents(PhotonPlayer sender)
		{
			base.SendPastEvents(sender);
		}

		// Token: 0x06005A5F RID: 23135 RVA: 0x001F7950 File Offset: 0x001F5D50
		[PunRPC]
		protected override void SyncEvents(VRC_EventLog.EventLogEntry[] eventLog, PhotonPlayer sender)
		{
			foreach (VRC_EventLog.EventLogEntry entry in eventLog)
			{
				VRC_EventLog.AssignObjectParameter(entry, this.Dispatcher);
			}
			base.SyncEvents(eventLog, sender);
		}

		// Token: 0x06005A60 RID: 23136 RVA: 0x001F798B File Offset: 0x001F5D8B
		[PunRPC]
		protected override void InitialSyncFinished(PhotonPlayer sender)
		{
			base.InitialSyncFinished(sender);
		}

		// Token: 0x06005A61 RID: 23137 RVA: 0x001F7994 File Offset: 0x001F5D94
		[PunRPC]
		public override void ProcessEvent(VRC_EventLog.EventLogEntry entry, PhotonPlayer sender)
		{
			base.ProcessEvent(entry, sender);
		}

		// Token: 0x04004031 RID: 16433
		public VRC_EventDispatcherRFC Dispatcher;
	}

	// Token: 0x02000B6E RID: 2926
	public class EventLogEntry : IEvent
	{
		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x06005A63 RID: 23139 RVA: 0x001F79A6 File Offset: 0x001F5DA6
		public object[] rpcParameters
		{
			get
			{
				if (this.rpcParametersCache == null)
				{
					this.rpcParametersCache = VRC_Serialization.ParameterDecoder(this.Event.ParameterBytes, false);
				}
				return this.rpcParametersCache;
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x06005A64 RID: 23140 RVA: 0x001F79D0 File Offset: 0x001F5DD0
		// (set) Token: 0x06005A65 RID: 23141 RVA: 0x001F79D9 File Offset: 0x001F5DD9
		public double Time
		{
			get
			{
				return (double)this.time;
			}
			set
			{
				this.time = (float)value;
				this.serialized = null;
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x06005A66 RID: 23142 RVA: 0x001F79EA File Offset: 0x001F5DEA
		// (set) Token: 0x06005A67 RID: 23143 RVA: 0x001F79F2 File Offset: 0x001F5DF2
		public int InstagatorPhotonId
		{
			get
			{
				return this.instagatorPhotonId;
			}
			set
			{
				this.instagatorPhotonId = value;
				this.serialized = null;
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x06005A68 RID: 23144 RVA: 0x001F7A02 File Offset: 0x001F5E02
		// (set) Token: 0x06005A69 RID: 23145 RVA: 0x001F7A0A File Offset: 0x001F5E0A
		public long CombinedNetworkId
		{
			get
			{
				return this.combinedNetworkId;
			}
			set
			{
				this.combinedNetworkId = value;
				this.serialized = null;
			}
		}

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x06005A6A RID: 23146 RVA: 0x001F7A1A File Offset: 0x001F5E1A
		// (set) Token: 0x06005A6B RID: 23147 RVA: 0x001F7A22 File Offset: 0x001F5E22
		public VRC_EventHandler.VrcEvent Event
		{
			get
			{
				return this.theEvent;
			}
			set
			{
				this.theEvent = value;
				this.serialized = null;
			}
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x06005A6C RID: 23148 RVA: 0x001F7A32 File Offset: 0x001F5E32
		// (set) Token: 0x06005A6D RID: 23149 RVA: 0x001F7A3A File Offset: 0x001F5E3A
		public string ObjectPath
		{
			get
			{
				return this.objectPath;
			}
			set
			{
				this.objectPath = value;
				this.serialized = null;
			}
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x06005A6E RID: 23150 RVA: 0x001F7A4A File Offset: 0x001F5E4A
		// (set) Token: 0x06005A6F RID: 23151 RVA: 0x001F7A52 File Offset: 0x001F5E52
		public float FastForward
		{
			get
			{
				return this.fastForward;
			}
			set
			{
				this.fastForward = value;
				this.serialized = null;
			}
		}

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x06005A70 RID: 23152 RVA: 0x001F7A62 File Offset: 0x001F5E62
		// (set) Token: 0x06005A71 RID: 23153 RVA: 0x001F7A6A File Offset: 0x001F5E6A
		public int Instigator
		{
			get
			{
				return this.InstagatorPhotonId;
			}
			set
			{
				this.InstagatorPhotonId = value;
			}
		}

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x06005A72 RID: 23154 RVA: 0x001F7A74 File Offset: 0x001F5E74
		public bool Store
		{
			get
			{
				return this.OriginalBroadcast != VRC_EventHandler.VrcBroadcastType.AlwaysUnbuffered && this.OriginalBroadcast != VRC_EventHandler.VrcBroadcastType.MasterUnbuffered && this.OriginalBroadcast != VRC_EventHandler.VrcBroadcastType.OwnerUnbuffered && this.OriginalBroadcast != VRC_EventHandler.VrcBroadcastType.Local && this.theEvent.EventType != VRC_EventHandler.VrcEventType.SpawnObject;
			}
		}

		// Token: 0x06005A73 RID: 23155 RVA: 0x001F7AC6 File Offset: 0x001F5EC6
		public VRC_EventLog.EventLogEntry DeepClone()
		{
			this.serialized = VRC_EventLog.EventLogEntry.SerializeForPhoton(this);
			return (VRC_EventLog.EventLogEntry)VRC_EventLog.EventLogEntry.DeserializeForPhoton(this.serialized);
		}

		// Token: 0x06005A74 RID: 23156 RVA: 0x001F7AE4 File Offset: 0x001F5EE4
		private static byte[] SerializeForPhoton(object customobject)
		{
			VRC_EventLog.EventLogEntry eventLogEntry = (VRC_EventLog.EventLogEntry)customobject;
			if (eventLogEntry.serialized != null)
			{
				return eventLogEntry.serialized;
			}
			byte[] array = new byte[32768];
			int num = 0;
			Protocol.Serialize(eventLogEntry.time, array, ref num);
			Protocol.Serialize(eventLogEntry.instagatorPhotonId, array, ref num);
			int value = (int)(eventLogEntry.combinedNetworkId >> 32);
            // HMM.
			int value2 = (int)(eventLogEntry.combinedNetworkId & (long)(-1));
			Protocol.Serialize(value, array, ref num);
			Protocol.Serialize(value2, array, ref num);
			if (eventLogEntry.ObjectPath == null)
			{
				eventLogEntry.ObjectPath = string.Empty;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(eventLogEntry.ObjectPath);
			Protocol.Serialize((short)bytes.Length, array, ref num);
			bytes.CopyTo(array, num);
			num += bytes.Length;
			Protocol.Serialize((int)eventLogEntry.theEvent.EventType, array, ref num);
			Protocol.Serialize((int)eventLogEntry.theEvent.ParameterBoolOp, array, ref num);
			Protocol.Serialize(eventLogEntry.theEvent.ParameterFloat, array, ref num);
			Protocol.Serialize(eventLogEntry.theEvent.ParameterInt, array, ref num);
			bytes = Encoding.UTF8.GetBytes(eventLogEntry.theEvent.ParameterString);
			Protocol.Serialize((short)bytes.Length, array, ref num);
			bytes.CopyTo(array, num);
			num += bytes.Length;
			Protocol.Serialize(eventLogEntry.fastForward, array, ref num);
			Protocol.Serialize((short)eventLogEntry.OriginalBroadcast, array, ref num);
			if (eventLogEntry.theEvent.ParameterBytes == null)
			{
				eventLogEntry.theEvent.ParameterBytes = new byte[0];
			}
			Protocol.Serialize((short)eventLogEntry.theEvent.ParameterBytes.Length, array, ref num);
			eventLogEntry.theEvent.ParameterBytes.CopyTo(array, num);
			num += eventLogEntry.theEvent.ParameterBytes.Length;
			byte[] array2 = new byte[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = array[i];
			}
			eventLogEntry.serialized = array2;
			return array2;
		}

		// Token: 0x06005A75 RID: 23157 RVA: 0x001F7CC4 File Offset: 0x001F60C4
		private static object DeserializeForPhoton(byte[] bytes)
		{
			VRC_EventLog.EventLogEntry eventLogEntry = new VRC_EventLog.EventLogEntry();
			eventLogEntry.Event = new VRC_EventHandler.VrcEvent();
			int num = 0;
			Protocol.Deserialize(out eventLogEntry.time, bytes, ref num);
			Protocol.Deserialize(out eventLogEntry.instagatorPhotonId, bytes, ref num);
			int num2;
			Protocol.Deserialize(out num2, bytes, ref num);
			int num3;
			Protocol.Deserialize(out num3, bytes, ref num);
			eventLogEntry.combinedNetworkId = ((long)num2 << 32 | (long)((ulong)num3));
			short num4;
			Protocol.Deserialize(out num4, bytes, ref num);
			eventLogEntry.objectPath = Encoding.UTF8.GetString(bytes, num, (int)num4);
			num += (int)num4;
			int value;
			Protocol.Deserialize(out value, bytes, ref num);
			eventLogEntry.theEvent.EventType = (VRC_EventHandler.VrcEventType)Enum.ToObject(typeof(VRC_EventHandler.VrcEventType), value);
			int value2;
			Protocol.Deserialize(out value2, bytes, ref num);
			eventLogEntry.theEvent.ParameterBoolOp = (VRC_EventHandler.VrcBooleanOp)Enum.ToObject(typeof(VRC_EventHandler.VrcBooleanOp), value2);
			Protocol.Deserialize(out eventLogEntry.theEvent.ParameterFloat, bytes, ref num);
			Protocol.Deserialize(out eventLogEntry.theEvent.ParameterInt, bytes, ref num);
			Protocol.Deserialize(out num4, bytes, ref num);
			eventLogEntry.theEvent.ParameterString = Encoding.UTF8.GetString(bytes, num, (int)num4);
			num += (int)num4;
			try
			{
				Protocol.Deserialize(out eventLogEntry.fastForward, bytes, ref num);
				short originalBroadcast;
				Protocol.Deserialize(out originalBroadcast, bytes, ref num);
				eventLogEntry.OriginalBroadcast = (VRC_EventHandler.VrcBroadcastType)originalBroadcast;
				short num5 = 0;
				Protocol.Deserialize(out num5, bytes, ref num);
				eventLogEntry.theEvent.ParameterBytes = new byte[(int)num5];
				if (num5 > 0)
				{
					Array.Copy(bytes, num, eventLogEntry.theEvent.ParameterBytes, 0, (int)num5);
				}
				num += (int)num5;
			}
			catch (IndexOutOfRangeException)
			{
				eventLogEntry.fastForward = 0f;
				eventLogEntry.theEvent.ParameterBytes = new byte[0];
			}
			return eventLogEntry;
		}

		// Token: 0x06005A76 RID: 23158 RVA: 0x001F7E80 File Offset: 0x001F6280
		public static void RegisterSerialization()
		{
			if (VRC_EventLog.EventLogEntry._serializationRegistered)
			{
				return;
			}
			Type typeFromHandle = typeof(VRC_EventLog.EventLogEntry);
			byte code = 69;
			if (VRC_EventLog.EventLogEntry.f__mg0 == null)
			{
				VRC_EventLog.EventLogEntry.f__mg0 = new SerializeMethod(VRC_EventLog.EventLogEntry.SerializeForPhoton);
			}
			SerializeMethod serializeMethod = VRC_EventLog.EventLogEntry.f__mg0;
			if (VRC_EventLog.EventLogEntry.f__mg1 == null)
			{
				VRC_EventLog.EventLogEntry.f__mg1 = new DeserializeMethod(VRC_EventLog.EventLogEntry.DeserializeForPhoton);
			}
			PhotonPeer.RegisterType(typeFromHandle, code, serializeMethod, VRC_EventLog.EventLogEntry.f__mg1);
			VRC_EventLog.EventLogEntry._serializationRegistered = true;
		}

		// Token: 0x06005A77 RID: 23159 RVA: 0x001F7EEC File Offset: 0x001F62EC
		public override string ToString()
		{
			return string.Format("[{8} {0}/{1} w: {2} s: {3} i: {4} f: {5} b: {6} B: {7} p: {9}]", new object[]
			{
				this.theEvent.EventType.ToString(),
				this.OriginalBroadcast.ToString(),
				this.instagatorPhotonId.ToString(),
				this.theEvent.ParameterString,
				this.theEvent.ParameterInt.ToString(),
				this.theEvent.ParameterFloat.ToString(),
				this.theEvent.ParameterBoolOp.ToString(),
				(this.theEvent.ParameterBytes != null) ? this.theEvent.ParameterBytes.Length.ToString() : "None",
				this.combinedNetworkId.ToString("X"),
				this.ObjectPath
			});
		}

		// Token: 0x04004032 RID: 16434
		private float time;

		// Token: 0x04004033 RID: 16435
		private int instagatorPhotonId;

		// Token: 0x04004034 RID: 16436
		private long combinedNetworkId;

		// Token: 0x04004035 RID: 16437
		private VRC_EventHandler.VrcEvent theEvent;

		// Token: 0x04004036 RID: 16438
		private string objectPath;

		// Token: 0x04004037 RID: 16439
		private float fastForward;

		// Token: 0x04004038 RID: 16440
		public object[] rpcParametersCache;

		// Token: 0x04004039 RID: 16441
		public double TimeReceived;

		// Token: 0x0400403A RID: 16442
		public VRC_EventHandler.VrcBroadcastType OriginalBroadcast;

		// Token: 0x0400403B RID: 16443
		private byte[] serialized;

		// Token: 0x0400403C RID: 16444
		private static bool _serializationRegistered;

		// Token: 0x0400403D RID: 16445
		[CompilerGenerated]
		private static SerializeMethod f__mg0;

		// Token: 0x0400403E RID: 16446
		[CompilerGenerated]
		private static DeserializeMethod f__mg1;

		// Token: 0x02000B6F RID: 2927
		public class BufferedEqualityComparer : IEqualityComparer<VRC_EventLog.EventLogEntry>
		{
			// Token: 0x06005A7A RID: 23162 RVA: 0x001F8004 File Offset: 0x001F6404
			public bool Equals(VRC_EventLog.EventLogEntry x, VRC_EventLog.EventLogEntry y)
			{
				if (object.ReferenceEquals(x, y))
				{
					return true;
				}
				if (x.OriginalBroadcast == VRC_EventHandler.VrcBroadcastType.AlwaysBufferOne || x.OriginalBroadcast == VRC_EventHandler.VrcBroadcastType.MasterBufferOne || x.OriginalBroadcast == VRC_EventHandler.VrcBroadcastType.OwnerBufferOne)
				{
					if (x.ObjectPath == y.ObjectPath && x.Event.EventType == y.Event.EventType)
					{
						switch (x.Event.EventType)
						{
						case VRC_EventHandler.VrcEventType.MeshVisibility:
						case VRC_EventHandler.VrcEventType.AudioTrigger:
						case VRC_EventHandler.VrcEventType.PlayAnimation:
						case VRC_EventHandler.VrcEventType.SetParticlePlaying:
						case VRC_EventHandler.VrcEventType.TeleportPlayer:
						case VRC_EventHandler.VrcEventType.SetGameObjectActive:
						case VRC_EventHandler.VrcEventType.SetWebPanelURI:
							return true;
						case VRC_EventHandler.VrcEventType.AnimationFloat:
						case VRC_EventHandler.VrcEventType.AnimationBool:
						case VRC_EventHandler.VrcEventType.AnimationTrigger:
							return x.Event.ParameterString == y.Event.ParameterString;
						case VRC_EventHandler.VrcEventType.SendMessage:
						case VRC_EventHandler.VrcEventType.RunConsoleCommand:
							return x.Event.ParameterString == y.Event.ParameterString;
						case VRC_EventHandler.VrcEventType.SpawnObject:
							return x.Event.ParameterString == y.Event.ParameterString && x.Event.ParameterInt == y.Event.ParameterInt && x.Event.ParameterBool == y.Event.ParameterBool;
						case VRC_EventHandler.VrcEventType.SendRPC:
						{
							if (x.Instigator != y.Instigator || !(x.Event.ParameterString == y.Event.ParameterString))
							{
								return false;
							}
							byte[] parameterBytes = x.Event.ParameterBytes;
							byte[] parameterBytes2 = y.Event.ParameterBytes;
							if (parameterBytes.Length != parameterBytes2.Length)
							{
								return false;
							}
							for (int i = 0; i < parameterBytes.Length; i++)
							{
								if (parameterBytes[i] != parameterBytes2[i])
								{
									return false;
								}
							}
							return true;
						}
						}
						return false;
					}
					return false;
				}
				else
				{
					if (x.ObjectPath == y.ObjectPath && x.Event.EventType == y.Event.EventType)
					{
						switch (x.Event.EventType)
						{
						case VRC_EventHandler.VrcEventType.MeshVisibility:
						case VRC_EventHandler.VrcEventType.AudioTrigger:
						case VRC_EventHandler.VrcEventType.PlayAnimation:
						case VRC_EventHandler.VrcEventType.SetParticlePlaying:
						case VRC_EventHandler.VrcEventType.TeleportPlayer:
						case VRC_EventHandler.VrcEventType.SetGameObjectActive:
						case VRC_EventHandler.VrcEventType.SetWebPanelURI:
							return true;
						case VRC_EventHandler.VrcEventType.AnimationFloat:
						case VRC_EventHandler.VrcEventType.AnimationBool:
						case VRC_EventHandler.VrcEventType.AnimationTrigger:
							return x.Event.ParameterString == y.Event.ParameterString;
						}
						return false;
					}
					return false;
				}
			}

			// Token: 0x06005A7B RID: 23163 RVA: 0x001F827F File Offset: 0x001F667F
			public int GetHashCode(VRC_EventLog.EventLogEntry obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}
