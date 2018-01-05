using System;
using UnityEngine;
using VRCSDK2;

namespace VRCCaptureUtils
{
	// Token: 0x020009F6 RID: 2550
	[RequireComponent(typeof(VRC_Pickup), typeof(VRC_EventHandler))]
	public class SpawnablePickup : SpawnableObject
	{
		// Token: 0x06004D8B RID: 19851 RVA: 0x001A04CA File Offset: 0x0019E8CA
		protected override void Awake()
		{
			base.Awake();
			this.pickup = base.gameObject.GetComponent<VRC_Pickup>();
			this.SetupTriggers();
		}

		// Token: 0x06004D8C RID: 19852 RVA: 0x001A04EC File Offset: 0x0019E8EC
		public void SetupSpawnedObject(ObjectSpawner.DespawnType despawnType, float despawnTime, float autoDespawnDistance, Action<SpawnablePickup> onPickedUp, Action<SpawnablePickup> onDropped, Action<SpawnableObject> onDespawned, ObjectSpawner spawner)
		{
			this.mDespawnType = despawnType;
			this.mDespawnTime = despawnTime;
			this.mDespawnMoveDistance = autoDespawnDistance;
			this.mOnDespawned = onDespawned;
			this.mOnPickedUp = onPickedUp;
			this.mOnDropped = onDropped;
			this.mSpawner = spawner;
			this.mTimer.time = this.mDespawnTime;
		}

		// Token: 0x06004D8D RID: 19853 RVA: 0x001A0540 File Offset: 0x0019E940
		public override void ResetSpawnableItem()
		{
			base.ResetSpawnableItem();
			this.mOnPickedUp = (this.mOnDropped = null);
			this.mOnDespawned = null;
		}

		// Token: 0x06004D8E RID: 19854 RVA: 0x001A056A File Offset: 0x0019E96A
		public void CleanupSpawnablePickup()
		{
			this.eventHandler.Events.Clear();
		}

		// Token: 0x06004D8F RID: 19855 RVA: 0x001A057C File Offset: 0x0019E97C
		protected override bool ShouldStartTimer()
		{
			bool result = false;
			if (this.pickup.currentPlayer == null)
			{
				if (this.mSpawner == null)
				{
					Debug.Log("spawner null for : " + base.gameObject.name);
				}
				if (this.mDespawnType == ObjectSpawner.DespawnType.OnTimer)
				{
					result = true;
				}
				else if (this.mSpawner.useAutoDespawnDistance && base.IsObjectFurtherThanDespawnDistance())
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06004D90 RID: 19856 RVA: 0x001A0600 File Offset: 0x0019EA00
		public void OnPickedUp()
		{
			Debug.Log("OnPickedUp");
			ObjectSpawner.DespawnType mDespawnType = this.mDespawnType;
			if (mDespawnType != ObjectSpawner.DespawnType.DontAutoDespawn)
			{
				this.mTimer.StopTimer();
				this.mTimer.ResetTimer();
			}
			if (this.mOnPickedUp != null)
			{
				this.mOnPickedUp(this);
			}
		}

		// Token: 0x06004D91 RID: 19857 RVA: 0x001A0664 File Offset: 0x0019EA64
		public void OnDropped()
		{
			Debug.Log("OnDropped");
			ObjectSpawner.DespawnType mDespawnType = this.mDespawnType;
			if (mDespawnType != ObjectSpawner.DespawnType.DontAutoDespawn)
			{
				if (mDespawnType == ObjectSpawner.DespawnType.OnDropped)
				{
					this.mTimer.ResetTimer();
					this.mTimer.StartTimer();
				}
			}
			if (this.mOnDropped != null)
			{
				this.mOnDropped(this);
			}
		}

		// Token: 0x06004D92 RID: 19858 RVA: 0x001A06CC File Offset: 0x0019EACC
		public override void ClearCallbacks()
		{
			base.ClearCallbacks();
			this.mOnPickedUp = (this.mOnDropped = null);
		}

		// Token: 0x06004D93 RID: 19859 RVA: 0x001A06F0 File Offset: 0x0019EAF0
		private void SetupTriggers()
		{
			VRC_Trigger vrc_Trigger = base.GetComponent<VRC_Trigger>();
			if (vrc_Trigger == null)
			{
				vrc_Trigger = base.gameObject.AddComponent<VRC_Trigger>();
			}
			VRC_Trigger.TriggerEvent triggerEvent = new VRC_Trigger.TriggerEvent();
			triggerEvent.TriggerType = VRC_Trigger.TriggerType.OnPickup;
			triggerEvent.BroadcastType = VRC_EventHandler.VrcBroadcastType.AlwaysUnbuffered;
			VRC_EventHandler.VrcEvent vrcEvent = new VRC_EventHandler.VrcEvent();
			vrcEvent.EventType = VRC_EventHandler.VrcEventType.SendRPC;
			vrcEvent.ParameterObjects = new GameObject[1];
			vrcEvent.ParameterObjects[0] = base.gameObject;
			vrcEvent.ParameterString = "OnPickedUp";
			vrcEvent.ParameterInt = 0;
			vrcEvent.ParameterBytes = VRC_Serialization.ParameterEncoder(new object[0]);
			triggerEvent.Events.Add(vrcEvent);
			vrc_Trigger.Triggers.Add(triggerEvent);
			VRC_Trigger.TriggerEvent triggerEvent2 = new VRC_Trigger.TriggerEvent();
			triggerEvent2.TriggerType = VRC_Trigger.TriggerType.OnDrop;
			triggerEvent2.BroadcastType = VRC_EventHandler.VrcBroadcastType.AlwaysUnbuffered;
			VRC_EventHandler.VrcEvent vrcEvent2 = new VRC_EventHandler.VrcEvent();
			vrcEvent2.EventType = VRC_EventHandler.VrcEventType.SendRPC;
			vrcEvent2.ParameterObjects = new GameObject[1];
			vrcEvent2.ParameterObjects[0] = base.gameObject;
			vrcEvent2.ParameterString = "OnDropped";
			vrcEvent2.ParameterInt = 0;
			vrcEvent2.ParameterBytes = VRC_Serialization.ParameterEncoder(new object[0]);
			triggerEvent2.Events.Add(vrcEvent2);
			vrc_Trigger.Triggers.Add(triggerEvent2);
		}

		// Token: 0x04003598 RID: 13720
		protected VRC_Pickup pickup;

		// Token: 0x04003599 RID: 13721
		protected VRC_EventHandler eventHandler;

		// Token: 0x0400359A RID: 13722
		public Action<SpawnablePickup> mOnPickedUp;

		// Token: 0x0400359B RID: 13723
		public Action<SpawnablePickup> mOnDropped;
	}
}
