using System;
using UnityEngine;
using VRCSDK2;

namespace VRCCaptureUtils
{
	// Token: 0x020009F5 RID: 2549
	public class SpawnableObject : MonoBehaviour
	{
		// Token: 0x06004D80 RID: 19840 RVA: 0x001A02A8 File Offset: 0x0019E6A8
		protected virtual void Awake()
		{
			this.mTimer = base.GetComponent<Timer>();
			if (this.mTimer == null)
			{
				this.mTimer = base.gameObject.AddComponent<Timer>();
			}
			this.mTimer.time = this.mDespawnTime;
			this.mTimer.StopTimer();
			this.mTimer.ResetTimer();
			this.mTimer.resetAndStopTimerWhenDone = false;
			this.mTimer.startTimerOnAwake = false;
		}

		// Token: 0x06004D81 RID: 19841 RVA: 0x001A0322 File Offset: 0x0019E722
		protected virtual void Update()
		{
			if (this.ShouldStartTimer())
			{
				this.mTimer.StartTimer();
			}
			if (Networking.IsOwner(base.gameObject))
			{
				this.DespawnWhenReady();
			}
		}

		// Token: 0x06004D82 RID: 19842 RVA: 0x001A0350 File Offset: 0x0019E750
		public virtual void ResetSpawnableItem()
		{
			this.mOnDespawned = null;
			this.mTimer.StopTimer();
			this.mTimer.ResetTimer();
		}

		// Token: 0x06004D83 RID: 19843 RVA: 0x001A036F File Offset: 0x0019E76F
		protected virtual bool ShouldStartTimer()
		{
			return this.mDespawnType == ObjectSpawner.DespawnType.OnTimer || (this.mSpawner.useAutoDespawnDistance && this.IsObjectFurtherThanDespawnDistance());
		}

		// Token: 0x06004D84 RID: 19844 RVA: 0x001A039D File Offset: 0x0019E79D
		public bool IsObjectFurtherThanDespawnDistance()
		{
			return Vector3.Distance(base.transform.position, this.mSpawner.transform.position) > this.mDespawnMoveDistance;
		}

		// Token: 0x06004D85 RID: 19845 RVA: 0x001A03C7 File Offset: 0x0019E7C7
		public void Despawn(bool wasDestroyed = false)
		{
			if (this.mOnDespawned != null)
			{
				this.mOnDespawned(this);
			}
			this.ClearCallbacks();
			if (!wasDestroyed && Networking.IsOwner(base.gameObject))
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06004D86 RID: 19846 RVA: 0x001A0407 File Offset: 0x0019E807
		public void OnDestroy()
		{
			this.Despawn(true);
		}

		// Token: 0x06004D87 RID: 19847 RVA: 0x001A0410 File Offset: 0x0019E810
		protected void DespawnWhenReady()
		{
			switch (this.mDespawnType)
			{
			case ObjectSpawner.DespawnType.OnTimer:
				if (this.mTimer.IsTimerDone())
				{
					this.Despawn(false);
				}
				break;
			case ObjectSpawner.DespawnType.OnDropped:
				if (this.mTimer.IsTimerDone())
				{
					this.Despawn(false);
				}
				break;
			}
		}

		// Token: 0x06004D88 RID: 19848 RVA: 0x001A0481 File Offset: 0x0019E881
		public virtual void SetupSpawnedObject(ObjectSpawner.DespawnType despawnType, float despawnTime, float autoDespawnDistance, Action<SpawnableObject> onDespawned, ObjectSpawner spawner)
		{
			this.mDespawnType = despawnType;
			this.mDespawnTime = despawnTime;
			this.mDespawnMoveDistance = autoDespawnDistance;
			this.mOnDespawned = onDespawned;
			this.mSpawner = spawner;
			this.mTimer.time = this.mDespawnTime;
		}

		// Token: 0x06004D89 RID: 19849 RVA: 0x001A04B9 File Offset: 0x0019E8B9
		public virtual void ClearCallbacks()
		{
			this.mOnDespawned = null;
		}

		// Token: 0x04003592 RID: 13714
		protected ObjectSpawner mSpawner;

		// Token: 0x04003593 RID: 13715
		protected Action<SpawnableObject> mOnDespawned;

		// Token: 0x04003594 RID: 13716
		protected Timer mTimer;

		// Token: 0x04003595 RID: 13717
		protected ObjectSpawner.DespawnType mDespawnType;

		// Token: 0x04003596 RID: 13718
		protected float mDespawnTime = 3f;

		// Token: 0x04003597 RID: 13719
		protected float mDespawnMoveDistance = 1f;
	}
}
