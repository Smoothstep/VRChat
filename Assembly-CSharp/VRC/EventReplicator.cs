using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace VRC
{
	// Token: 0x02000B4E RID: 2894
	public class EventReplicator<EventType, EventEqualityComparerType> : MonoBehaviour where EventType : IEvent, new() where EventEqualityComparerType : IEqualityComparer<EventType>, new()
	{
		// Token: 0x14000073 RID: 115
		// (add) Token: 0x060058A5 RID: 22693 RVA: 0x001EB480 File Offset: 0x001E9880
		// (remove) Token: 0x060058A6 RID: 22694 RVA: 0x001EB4B8 File Offset: 0x001E98B8
		public event EventReplicator<EventType, EventEqualityComparerType>.OnInitialSyncFinishedDelegate OnInitialSyncFinished;

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x060058A7 RID: 22695 RVA: 0x001EB4EE File Offset: 0x001E98EE
		public IEnumerable<EventType> RecordedEvents
		{
			get
			{
				return this.eventLogState.Values;
			}
		}

		// Token: 0x060058A8 RID: 22696 RVA: 0x001EB4FC File Offset: 0x001E98FC
		public int RemoveEventsIf(Func<EventType, bool> predicate)
		{
			int count = this.eventLogState.Count;
			EventType[] array = this.eventLogState.Keys.ToArray<EventType>();
			foreach (EventType eventType in array)
			{
				try
				{
					if (predicate(eventType))
					{
						this.eventLogState.Remove(eventType);
					}
				}
				catch (Exception ex)
				{
					Debug.LogErrorFormat("Exception in predicate when examining {0}:\n{1}", new object[]
					{
						eventType.ToString(),
						ex.ToString()
					});
				}
			}
			return count - this.eventLogState.Count;
		}

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x060058A9 RID: 22697 RVA: 0x001EB5B8 File Offset: 0x001E99B8
		private double UpdateRate
		{
			get
			{
				return (double)PhotonNetwork.sendRateOnSerialize * 0.001;
			}
		}

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x060058AA RID: 22698 RVA: 0x001EB5CA File Offset: 0x001E99CA
		public bool IsReceivingEvents
		{
			get
			{
				return this.receivedInitialSyncFinished || this.eventLogState.Count > 0 || Network.IsMaster;
			}
		}

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x060058AB RID: 22699 RVA: 0x001EB5F0 File Offset: 0x001E99F0
		public bool HasReceivedInitialEvents
		{
			get
			{
				return this.receivedInitialSyncFinished;
			}
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x060058AC RID: 22700 RVA: 0x001EB5F8 File Offset: 0x001E99F8
		public int EventStateSize
		{
			get
			{
				return this.eventLogState.Count;
			}
		}

		// Token: 0x060058AD RID: 22701 RVA: 0x001EB605 File Offset: 0x001E9A05
		private void Awake()
		{
			this.eventLogState = new Dictionary<EventType, EventType>(Activator.CreateInstance<EventEqualityComparerType>());
			this.receivedInitialSyncFinished = false;
		}

		// Token: 0x060058AE RID: 22702 RVA: 0x001EB624 File Offset: 0x001E9A24
		public IEnumerator Start()
		{
			yield return new WaitUntil(() => NetworkManager.Instance != null);
			NetworkManager.Instance.OnPlayerLeftEvent.AddListener(new UnityAction<Player>(this.HandlePlayerLeft));
			yield break;
		}

		// Token: 0x060058AF RID: 22703 RVA: 0x001EB63F File Offset: 0x001E9A3F
		private void OnDestroy()
		{
			if (NetworkManager.Instance != null)
			{
				NetworkManager.Instance.OnPlayerLeftEvent.RemoveListener(new UnityAction<Player>(this.HandlePlayerLeft));
			}
		}

		// Token: 0x060058B0 RID: 22704 RVA: 0x001EB66C File Offset: 0x001E9A6C
		public void HandlePlayerLeft(Player player)
		{
			int instigatorID = Network.GetInstigatorID(player);
			foreach (KeyValuePair<EventType, EventType> keyValuePair in this.eventLogState.Where(delegate(KeyValuePair<EventType, EventType> p)
			{
				EventType key2 = p.Key;
				return key2.Instigator == instigatorID;
			}))
			{
				EventType key = keyValuePair.Key;
				int instigator = 0;
				EventType value = keyValuePair.Value;
				value.Instigator = instigator;
				key.Instigator = instigator;
			}
			if (this.syncQueues.ContainsKey(player.GetPhotonPlayerId()))
			{
				this.syncQueues.Remove(player.GetPhotonPlayerId());
			}
		}

		// Token: 0x060058B1 RID: 22705 RVA: 0x001EB73C File Offset: 0x001E9B3C
		public bool RequestPastEvents()
		{
			if (Network.IsMaster)
			{
				return true;
			}
			PhotonView photonView = PhotonView.Get(this);
			if (photonView == null)
			{
				Debug.LogError("Not requesting past events: photonView == null");
				return false;
			}
			if (photonView.viewID < 1)
			{
				Debug.LogError("Not requesting past events: photonView.viewID < 1");
				return false;
			}
			if (PhotonNetwork.masterClient == null)
			{
				Debug.LogError("Not requesting past events: PhotonNetwork.masterClient == null");
				return false;
			}
			if (VRCPlayer.Instance == null)
			{
				Debug.LogError("Not requesting past events: VRCPlayer.Instance == null");
				return false;
			}
			if (VRCPlayer.Instance.photonView == null)
			{
				Debug.LogError("Not requesting past events: VRCPlayer.Instance.photonView == null");
				return false;
			}
			photonView.RpcSecure("SendPastEvents", PhotonNetwork.masterClient, true, new object[0]);
			return true;
		}

		// Token: 0x060058B2 RID: 22706 RVA: 0x001EB7F7 File Offset: 0x001E9BF7
		public void ClearRecordedEvents()
		{
			this.eventLogState.Clear();
		}

		// Token: 0x060058B3 RID: 22707 RVA: 0x001EB804 File Offset: 0x001E9C04
		protected virtual void SendPastEvents(PhotonPlayer sender)
		{
			if (!PhotonNetwork.isMasterClient || sender == null || sender.IsLocal)
			{
				return;
			}
			if (!this.syncQueues.ContainsKey(sender.ID))
			{
				this.syncQueues.Add(sender.ID, (from evt in this.eventLogState.Values
				orderby evt.Time
				select evt).ToList<EventType>());
			}
		}

		// Token: 0x060058B4 RID: 22708 RVA: 0x001EB888 File Offset: 0x001E9C88
		protected virtual void SyncEvents(EventType[] eventLog, PhotonPlayer sender)
		{
			if (!sender.IsMasterClient || sender.IsLocal || this.receivedInitialSyncFinished)
			{
				return;
			}
			foreach (EventType entry in eventLog)
			{
				this.RecordEventInternal(entry, !this.receivedInitialSyncFinished, sender);
			}
		}

		// Token: 0x060058B5 RID: 22709 RVA: 0x001EB8E8 File Offset: 0x001E9CE8
		protected virtual void InitialSyncFinished(PhotonPlayer sender)
		{
			if (!sender.IsMasterClient)
			{
				return;
			}
			Debug.Log(string.Concat(new object[]
			{
				"<color=yellow>Initial sync finished for ",
				base.name,
				", received ",
				this.EventStateSize,
				" events</color>"
			}));
			this.receivedInitialSyncFinished = true;
		}

		// Token: 0x060058B6 RID: 22710 RVA: 0x001EB948 File Offset: 0x001E9D48
		public virtual void ProcessEvent(EventType entry, PhotonPlayer sender)
		{
			if (sender.IsLocal)
			{
				PhotonView photonView = PhotonView.Get(this);
				photonView.RpcSecure("ProcessEvent", PhotonTargets.Others, true, new object[]
				{
					entry
				});
			}
			entry.Instigator = Network.GetInstigatorID(sender);
			this.RecordEventInternal(entry, false, sender);
		}

		// Token: 0x060058B7 RID: 22711 RVA: 0x001EB9A0 File Offset: 0x001E9DA0
		private void RecordEventInternal(EventType entry, bool initial, PhotonPlayer sender)
		{
			if (!sender.IsMasterClient && (Network.GetInstigatorID(sender) != entry.Instigator || entry.Instigator <= 0))
			{
				Debug.LogError("Attempted to record an event with an invalid instigator id.");
				return;
			}
			Player player = Network.GetPlayerByInstigatorID(entry.Instigator);
			if (player == null)
			{
				player = Network.MasterPlayer;
				entry.Instigator = Network.GetInstigatorID(player);
			}
			if (!entry.Store)
			{
				if (this.OnNewEvent != null)
				{
					this.OnNewEvent(entry, player);
				}
				return;
			}
			EventEqualityComparerType comparer = Activator.CreateInstance<EventEqualityComparerType>();
			int num = this.RemoveEventsIf((EventType e) => comparer.Equals(e, entry));
			if (num > 0)
			{
				Debug.LogFormat("<color=cyan>Removed {0} previous events similar to {1}.</color>", new object[]
				{
					num,
					entry.ToString()
				});
			}
			if (!this.eventLogState.ContainsKey(entry))
			{
				Debug.LogFormat("<color=cyan>Recorded {0}</color>", new object[]
				{
					entry.ToString()
				});
				this.eventLogState.Add(entry, entry);
				if (PhotonNetwork.isMasterClient)
				{
					foreach (int num2 in this.syncQueues.Keys)
					{
						if (num2 != sender.ID)
						{
							this.syncQueues[num2].Add(entry);
						}
					}
				}
				if (this.OnNewEvent != null)
				{
					this.OnNewEvent(entry, player);
				}
			}
			else
			{
				Debug.LogError("Discarding duplicate event " + entry.ToString());
			}
		}

		// Token: 0x060058B8 RID: 22712 RVA: 0x001EBBD8 File Offset: 0x001E9FD8
		private void Update()
		{
			if ((double)Time.time - this.lastUpdateTime > this.UpdateRate)
			{
				this.lastUpdateTime = (double)Time.time;
				this.SendEvents();
			}
		}

		// Token: 0x060058B9 RID: 22713 RVA: 0x001EBC04 File Offset: 0x001EA004
		private void SendEvents()
		{
			if (!Network.IsMaster)
			{
				return;
			}
			List<PhotonPlayer> list = new List<PhotonPlayer>();
			PhotonView photonView = PhotonView.Get(this);
			List<int> list2 = this.syncQueues.Keys.ToList<int>();
			foreach (int num in list2)
			{
				List<EventType> list3 = new List<EventType>();
				List<EventType> list4 = this.syncQueues[num];
				PhotonPlayer photonPlayer = PhotonPlayer.Find(num);
				if (list4.Count == 0 && this.syncQueues.ContainsKey(num))
				{
					photonView.RpcSecure("InitialSyncFinished", photonPlayer, true, new object[0]);
					list.Add(photonPlayer);
					this.syncQueues.Remove(num);
				}
				else
				{
					list3.AddRange(list4.Take(this.SyncBatchSize));
					list4.RemoveRange(0, list3.Count);
					if (list3.Count == 0 && this.syncQueues.ContainsKey(num))
					{
						photonView.RpcSecure("InitialSyncFinished", photonPlayer, true, new object[0]);
						list.Add(photonPlayer);
						this.syncQueues.Remove(num);
					}
					else if (list3.Count != 0)
					{
						photonView.RpcSecure("SyncEvents", photonPlayer, true, new object[]
						{
							list3.ToArray()
						});
					}
				}
			}
			if (this.OnInitialSyncFinished != null && list.Count > 0)
			{
				this.OnInitialSyncFinished(list);
			}
		}

		// Token: 0x04003F83 RID: 16259
		public int SyncBatchSize = 128;

		// Token: 0x04003F84 RID: 16260
		public EventReplicator<EventType, EventEqualityComparerType>.NewEventHandler OnNewEvent;

		// Token: 0x04003F85 RID: 16261
		private Dictionary<EventType, EventType> eventLogState;

		// Token: 0x04003F86 RID: 16262
		private Dictionary<int, List<EventType>> syncQueues = new Dictionary<int, List<EventType>>();

		// Token: 0x04003F87 RID: 16263
		private bool receivedInitialSyncFinished;

		// Token: 0x04003F88 RID: 16264
		private double lastUpdateTime;

		// Token: 0x02000B4F RID: 2895
		// (Invoke) Token: 0x060058BC RID: 22716
		public delegate void OnInitialSyncFinishedDelegate(IEnumerable<PhotonPlayer> players);

		// Token: 0x02000B50 RID: 2896
		// (Invoke) Token: 0x060058C0 RID: 22720
		public delegate void NewEventHandler(EventType evt, Player instigator);
	}
}
