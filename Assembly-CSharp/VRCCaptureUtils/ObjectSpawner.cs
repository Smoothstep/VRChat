using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRCSDK2;

namespace VRCCaptureUtils
{
	// Token: 0x020009EA RID: 2538
	public class ObjectSpawner : Enableable
	{
		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x06004D30 RID: 19760 RVA: 0x0019DAFD File Offset: 0x0019BEFD
		public List<SpawnableObject> spawnedObjects
		{
			get
			{
				return new List<SpawnableObject>(this.mSpawnedObjects);
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06004D31 RID: 19761 RVA: 0x0019DB0A File Offset: 0x0019BF0A
		public GameObject lastSpawnedObject
		{
			get
			{
				return this.mLastSpawnedObject;
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06004D32 RID: 19762 RVA: 0x0019DB12 File Offset: 0x0019BF12
		// (set) Token: 0x06004D33 RID: 19763 RVA: 0x0019DB1C File Offset: 0x0019BF1C
		[SerializeField]
		public override bool isEnabled
		{
			get
			{
				return this.mEnabled;
			}
			set
			{
				this.mEnabled = value;
				if (Application.isPlaying)
				{
					if (this.mEnabled)
					{
						this.timer.timer = this.timer.time;
						this.timer.StartTimer();
					}
					else
					{
						this.timer.StopTimer();
					}
				}
			}
		}

		// Token: 0x06004D34 RID: 19764 RVA: 0x0019DB78 File Offset: 0x0019BF78
		public void SpawnAllItemsFromAllSpawners()
		{
			if (VRC.Network.IsMaster)
			{
				ObjectSpawner[] array = UnityEngine.Object.FindObjectsOfType<ObjectSpawner>();
				foreach (ObjectSpawner objectSpawner in array)
				{
					objectSpawner.isEnabled = true;
					objectSpawner.SpawnObject();
				}
			}
		}

		// Token: 0x06004D35 RID: 19765 RVA: 0x0019DBBC File Offset: 0x0019BFBC
		public void DespawnAllObjects()
		{
			if (VRC.Network.IsMaster)
			{
				SpawnableObject[] array = UnityEngine.Object.FindObjectsOfType<SpawnableObject>();
				foreach (SpawnableObject spawnableObject in array)
				{
					spawnableObject.ResetSpawnableItem();
					UnityEngine.Object.Destroy(spawnableObject.gameObject);
				}
			}
		}

		// Token: 0x06004D36 RID: 19766 RVA: 0x0019DC04 File Offset: 0x0019C004
		public void DespawnAllObjectsFromAllSpawners()
		{
			if (VRC.Network.IsMaster)
			{
				ObjectSpawner[] array = UnityEngine.Object.FindObjectsOfType<ObjectSpawner>();
				foreach (ObjectSpawner objectSpawner in array)
				{
					objectSpawner.DespawnAllObjectsFromThisSpawner();
				}
			}
		}

		// Token: 0x06004D37 RID: 19767 RVA: 0x0019DC44 File Offset: 0x0019C044
		public void DespawnAllObjectsFromThisSpawner()
		{
			foreach (SpawnableObject spawnableObject in this.mSpawnedObjects)
			{
				spawnableObject.ResetSpawnableItem();
				UnityEngine.Object.Destroy(spawnableObject.gameObject);
			}
		}

		// Token: 0x06004D38 RID: 19768 RVA: 0x0019DCAC File Offset: 0x0019C0AC
		public void EnableAllSpawners()
		{
			if (VRC.Network.IsMaster)
			{
				ObjectSpawner[] array = UnityEngine.Object.FindObjectsOfType<ObjectSpawner>();
				foreach (ObjectSpawner objectSpawner in array)
				{
					objectSpawner.isEnabled = true;
				}
			}
		}

		// Token: 0x06004D39 RID: 19769 RVA: 0x0019DCEC File Offset: 0x0019C0EC
		public void DisableAllSpawners()
		{
			if (VRC.Network.IsMaster)
			{
				ObjectSpawner[] array = UnityEngine.Object.FindObjectsOfType<ObjectSpawner>();
				foreach (ObjectSpawner objectSpawner in array)
				{
					objectSpawner.isEnabled = false;
				}
			}
		}

		// Token: 0x06004D3A RID: 19770 RVA: 0x0019DD2C File Offset: 0x0019C12C
		protected void Awake()
		{
			this.timer = base.GetComponent<Timer>();
			if (this.timer == null)
			{
				this.timer = base.gameObject.AddComponent<Timer>();
			}
			this.timer.time = this.spawnRate;
			this.timer.StopTimer();
			this.timer.ResetTimer();
			this.timer.resetAndStopTimerWhenDone = false;
			this.mSpawnedObjects = new List<SpawnableObject>();
		}

		// Token: 0x06004D3B RID: 19771 RVA: 0x0019DDA5 File Offset: 0x0019C1A5
		private void OnNetworkReady()
		{
			if (!this.isInit && this.spawnType != ObjectSpawner.SpawnType.DontAutoSpawn)
			{
				this.SpawnObject();
			}
			this.isInit = true;
		}

		// Token: 0x06004D3C RID: 19772 RVA: 0x0019DDCB File Offset: 0x0019C1CB
		private void OnEnable()
		{
			if (!this.isInit && VRC.Network.IsObjectReady(base.gameObject))
			{
				if (this.spawnType != ObjectSpawner.SpawnType.DontAutoSpawn)
				{
					this.SpawnObject();
				}
				this.isInit = true;
			}
		}

		// Token: 0x06004D3D RID: 19773 RVA: 0x0019DE01 File Offset: 0x0019C201
		private void OnDisable()
		{
			if (VRC.Network.IsMaster)
			{
				this.DespawnAllObjectsFromThisSpawner();
				this.timer.StopTimer();
				this.timer.ResetTimer();
			}
		}

		// Token: 0x06004D3E RID: 19774 RVA: 0x0019DE2C File Offset: 0x0019C22C
		private void OnPlayerJoined(VRC_PlayerApi player)
		{
			if (VRC.Network.IsMaster)
			{
				VRC.Player component = player.GetComponent<VRC.Player>();
				foreach (SpawnableObject spawnableObject in this.mSpawnedObjects)
				{
					VRC.Network.RPC(component, base.gameObject, "SetupLastSpawnedObject", new object[]
					{
						spawnableObject.gameObject
					});
				}
			}
		}

		// Token: 0x06004D3F RID: 19775 RVA: 0x0019DEB4 File Offset: 0x0019C2B4
		public void SpawnObject()
		{
			if (VRC.Network.IsMaster && this.mEnabled && this.mSpawnedObjects.Count <= this.maxSpawnedObjects && this.objectPrefab != null)
			{
				Debug.Log("Spawning " + this.objectPrefab.name);
				GameObject gameObject = VRC.Network.Instantiate(VRC_EventHandler.VrcBroadcastType.Master, this.objectPrefab.name, base.transform.position, base.transform.rotation);
				if (this.parentSpawnerToObject)
				{
					gameObject.transform.SetParent(base.transform);
				}
				if (gameObject != null)
				{
					this.SetupLastSpawnedObject(gameObject, VRC.Network.LocalPlayer);
					base.StartCoroutine(this.SetupLastSpawnedObjectRemotes(gameObject));
				}
			}
		}

		// Token: 0x06004D40 RID: 19776 RVA: 0x0019DF84 File Offset: 0x0019C384
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.Others
		})]
		private void SetupLastSpawnedObject(GameObject spawnedObject, VRC.Player instigator)
		{
			if (this.spawnType == ObjectSpawner.SpawnType.OnTimer)
			{
				this.timer.ResetTimer();
				this.timer.StartTimer();
			}
			else
			{
				this.timer.StopTimer();
				this.timer.ResetTimer();
			}
			this.mLastSpawnedObject = spawnedObject;
			if (this.mLastSpawnedObject.GetComponent<VRC_Pickup>() != null)
			{
				SpawnablePickup spawnablePickup = spawnedObject.AddComponent<SpawnablePickup>();
				spawnablePickup.SetupSpawnedObject(this.despawnType, this.despawnTime, this.autoDespawnDistance, new Action<SpawnablePickup>(this.OnItemPickedUp), null, new Action<SpawnableObject>(this.OnItemDestroyed), this);
			}
			else
			{
				SpawnableObject spawnableObject = spawnedObject.AddComponent<SpawnableObject>();
				spawnableObject.SetupSpawnedObject(this.despawnType, this.despawnTime, this.autoDespawnDistance, new Action<SpawnableObject>(this.OnItemDestroyed), this);
			}
			VRC_DestructibleStandard component = this.mLastSpawnedObject.GetComponent<VRC_DestructibleStandard>();
			if (component != null)
			{
				this.SetupDestructibleStandard(component);
			}
			SpawnableObject component2 = spawnedObject.GetComponent<SpawnableObject>();
			this.mSpawnedObjects.Add(component2);
			if (this.onItemSpawned != null)
			{
				this.onItemSpawned(spawnedObject);
			}
			if (this.onObjectSpawnedTrigger != null && !string.IsNullOrEmpty(this.onObjectSpawnedEvent.ParameterString))
			{
				VRC_Trigger.TriggerCustom(this.onObjectSpawnedTrigger.gameObject, this.onObjectSpawnedEvent.ParameterString);
			}
		}

		// Token: 0x06004D41 RID: 19777 RVA: 0x0019E0EC File Offset: 0x0019C4EC
		private IEnumerator SetupLastSpawnedObjectRemotes(GameObject spawnedObject)
		{
			yield return new WaitForSeconds(1f);
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.Others, base.gameObject, "SetupLastSpawnedObject", new object[]
			{
				spawnedObject
			});
			yield break;
		}

		// Token: 0x06004D42 RID: 19778 RVA: 0x0019E110 File Offset: 0x0019C510
		private void SetupDestructibleStandard(VRC_DestructibleStandard ds)
		{
			if (this.onObjectDamagedTrigger != null && !string.IsNullOrEmpty(this.onObjectDestructedEvent.ParameterString))
			{
				ds.spawnerOnDamagedTrigger = this.onObjectDamagedTrigger;
				ds.spawnerOnDamagedEvent = this.onObjectDamagedEvent;
			}
			if (this.onObjectDestructedTrigger != null && !string.IsNullOrEmpty(this.onObjectDestructedEvent.ParameterString))
			{
				ds.spawnerOnDestructedTrigger = this.onObjectDestructedTrigger;
				ds.spawnerOnDestructedEvent = this.onObjectDestructedEvent;
			}
			if (this.onObjectHealedTrigger != null && !string.IsNullOrEmpty(this.onObjectHealedEvent.ParameterString))
			{
				ds.spawnerOnHealedTrigger = this.onObjectHealedTrigger;
				ds.spawnerOnHealedEvent = this.onObjectHealedEvent;
			}
			if (this.onObjectFullHealedTrigger != null && !string.IsNullOrEmpty(this.onObjectFullHealedEvent.ParameterString))
			{
				ds.spawnerOnFullHealedTrigger = this.onObjectFullHealedTrigger;
				ds.spawnerOnFullHealedEvent = this.onObjectFullHealedEvent;
			}
		}

		// Token: 0x06004D43 RID: 19779 RVA: 0x0019E218 File Offset: 0x0019C618
		private void OnItemPickedUp(SpawnablePickup sp)
		{
			switch (this.spawnType)
			{
			case ObjectSpawner.SpawnType.OnDespawned:
				this.timer.StopTimer();
				break;
			case ObjectSpawner.SpawnType.OnPickedUp:
				this.timer.ResetTimer();
				this.timer.StartTimer();
				break;
			}
			if (this.mLastSpawnedObject != null && this.mLastSpawnedObject.gameObject == sp.gameObject)
			{
				this.mLastSpawnedObject = null;
			}
		}

		// Token: 0x06004D44 RID: 19780 RVA: 0x0019E2B0 File Offset: 0x0019C6B0
		private void OnItemDestroyed(SpawnableObject so)
		{
			switch (this.spawnType)
			{
			case ObjectSpawner.SpawnType.OnDespawned:
				this.timer.ResetTimer();
				this.timer.StartTimer();
				break;
			case ObjectSpawner.SpawnType.OnPickedUp:
				if (this.mLastSpawnedObject != null)
				{
					this.timer.ResetTimer();
					this.timer.StartTimer();
				}
				break;
			}
			if (this.mLastSpawnedObject == so)
			{
				this.mLastSpawnedObject = null;
			}
			this.mSpawnedObjects.Remove(so);
			if (this.onObjectDespawnedTrigger != null && !string.IsNullOrEmpty(this.onObjectDespawnedEvent.ParameterString))
			{
				VRC_Trigger.TriggerCustom(this.onObjectDespawnedTrigger.gameObject, this.onObjectDespawnedEvent.ParameterString);
			}
		}

		// Token: 0x06004D45 RID: 19781 RVA: 0x0019E396 File Offset: 0x0019C796
		private void Update()
		{
			if (!this.isInit)
			{
				return;
			}
			if (VRC.Network.IsMaster)
			{
				this.SpawnWhenReady();
			}
		}

		// Token: 0x06004D46 RID: 19782 RVA: 0x0019E3B4 File Offset: 0x0019C7B4
		private void SpawnWhenReady()
		{
			if (this.despawnType == ObjectSpawner.DespawnType.OnMaxSpawnedObjects && this.mSpawnedObjects.Count == this.maxSpawnedObjects && this.mSpawnedObjects.Count > 0)
			{
				this.mSpawnedObjects[0].Despawn(false);
			}
			switch (this.spawnType)
			{
			case ObjectSpawner.SpawnType.OnTimer:
				if (this.timer.IsTimerDone())
				{
					this.SpawnObject();
				}
				break;
			case ObjectSpawner.SpawnType.OnDespawned:
				if (this.timer.IsTimerDone() && this.mLastSpawnedObject == null)
				{
					this.SpawnObject();
				}
				break;
			case ObjectSpawner.SpawnType.OnPickedUp:
				if (this.timer.IsTimerDone() && this.mLastSpawnedObject == null)
				{
					this.SpawnObject();
				}
				break;
			}
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x0019E4A0 File Offset: 0x0019C8A0
		private void OnDrawGizmos()
		{
			if (this.objectPrefab != null)
			{
				Gizmos.color = Color.white;
				Mesh sharedMesh = this.objectPrefab.GetComponentInChildren<MeshFilter>().sharedMesh;
				Vector3 lossyScale = this.objectPrefab.GetComponentInChildren<MeshFilter>().transform.lossyScale;
				Gizmos.DrawWireMesh(this.objectPrefab.GetComponentInChildren<MeshFilter>().sharedMesh, base.transform.position, base.transform.rotation, lossyScale);
			}
		}

		// Token: 0x04003515 RID: 13589
		public VRC_ObjectSync objectPrefab;

		// Token: 0x04003516 RID: 13590
		public ObjectSpawner.SpawnType spawnType;

		// Token: 0x04003517 RID: 13591
		public float spawnRate = 5f;

		// Token: 0x04003518 RID: 13592
		public int maxSpawnedObjects = 5;

		// Token: 0x04003519 RID: 13593
		public bool parentSpawnerToObject;

		// Token: 0x0400351A RID: 13594
		private List<SpawnableObject> mSpawnedObjects;

		// Token: 0x0400351B RID: 13595
		public ObjectSpawner.DespawnType despawnType;

		// Token: 0x0400351C RID: 13596
		public float despawnTime = 5f;

		// Token: 0x0400351D RID: 13597
		public bool useAutoDespawnDistance;

		// Token: 0x0400351E RID: 13598
		public float autoDespawnDistance = 1f;

		// Token: 0x0400351F RID: 13599
		private GameObject mLastSpawnedObject;

		// Token: 0x04003520 RID: 13600
		public Action<GameObject> onItemSpawned;

		// Token: 0x04003521 RID: 13601
		private bool isInit;

		// Token: 0x04003522 RID: 13602
		private Timer timer;

		// Token: 0x04003523 RID: 13603
		public string onObjectSpawnedEventStr = "OnObjectSpawned";

		// Token: 0x04003524 RID: 13604
		public string onObjectDespawnedEventStr = "OnObjectDespawned";

		// Token: 0x04003525 RID: 13605
		public VRC_Trigger onObjectSpawnedTrigger;

		// Token: 0x04003526 RID: 13606
		public VRC_EventHandler.VrcEvent onObjectSpawnedEvent;

		// Token: 0x04003527 RID: 13607
		public VRC_Trigger onObjectDespawnedTrigger;

		// Token: 0x04003528 RID: 13608
		public VRC_EventHandler.VrcEvent onObjectDespawnedEvent;

		// Token: 0x04003529 RID: 13609
		public VRC_Trigger onObjectDamagedTrigger;

		// Token: 0x0400352A RID: 13610
		public VRC_EventHandler.VrcEvent onObjectDamagedEvent;

		// Token: 0x0400352B RID: 13611
		public VRC_Trigger onObjectDestructedTrigger;

		// Token: 0x0400352C RID: 13612
		public VRC_EventHandler.VrcEvent onObjectDestructedEvent;

		// Token: 0x0400352D RID: 13613
		public VRC_Trigger onObjectHealedTrigger;

		// Token: 0x0400352E RID: 13614
		public VRC_EventHandler.VrcEvent onObjectHealedEvent;

		// Token: 0x0400352F RID: 13615
		public VRC_Trigger onObjectFullHealedTrigger;

		// Token: 0x04003530 RID: 13616
		public VRC_EventHandler.VrcEvent onObjectFullHealedEvent;

		// Token: 0x020009EB RID: 2539
		public enum SpawnType
		{
			// Token: 0x04003532 RID: 13618
			OnTimer,
			// Token: 0x04003533 RID: 13619
			OnDespawned,
			// Token: 0x04003534 RID: 13620
			OnPickedUp,
			// Token: 0x04003535 RID: 13621
			DontAutoSpawn
		}

		// Token: 0x020009EC RID: 2540
		public enum DespawnType
		{
			// Token: 0x04003537 RID: 13623
			OnMaxSpawnedObjects,
			// Token: 0x04003538 RID: 13624
			OnTimer,
			// Token: 0x04003539 RID: 13625
			OnDropped,
			// Token: 0x0400353A RID: 13626
			DontAutoDespawn
		}
	}
}
