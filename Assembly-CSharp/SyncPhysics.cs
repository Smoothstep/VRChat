using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000B62 RID: 2914
public class SyncPhysics : VRCPunBehaviour
{
	// Token: 0x06005960 RID: 22880 RVA: 0x001F0C0C File Offset: 0x001EF00C
	public static bool PositionEventIsBad(SyncPhysics.PositionEvent evt)
	{
		return evt.Position.IsBad() || evt.Velocity.IsBad() || evt.Rotation.IsBad() || ((float)evt.Time).IsBad();
	}

	// Token: 0x06005961 RID: 22881 RVA: 0x001F0C58 File Offset: 0x001EF058
	private SyncPhysics.PositionEvent MakeCurrentPositionEvent(double now)
	{
		return new SyncPhysics.PositionEvent
		{
			flags = this.replicateFlags,
			Time = now,
			Position = this.GetPosition(),
			Velocity = this.GetVelocity(),
			Rotation = this.GetRotation(),
			OwnerID = base.OwnerId
		};
	}

	// Token: 0x06005962 RID: 22882 RVA: 0x001F0CC0 File Offset: 0x001EF0C0
	public void OnDrawGizmos()
	{
		if (this.positionHistory.Count < 3)
		{
			return;
		}
		Vector3 zero = Vector3.zero;
		int index = this.positionHistory.Count - 1;
		SyncPhysics.PositionEvent positionEvent = this.positionHistory[0];
		for (double num = this.positionHistory[0].Time; num < this.positionHistory[index].Time; num += (double)Time.smoothDeltaTime)
		{
			SyncPhysics.PositionEvent positionEvent2 = TweenFunctions.Tween<SyncPhysics.PositionEvent>(this.positionHistory, this.InterpolateFunction, num, VRC.Network.ExpectedInterval, -1);
			if (positionEvent2 != null)
			{
				Color color = Color.white;
				if (positionEvent2.Discontinuity)
				{
					color = Color.magenta;
				}
				if (positionEvent != null)
				{
					Gizmos.color = color;
					Gizmos.DrawLine(positionEvent.Position, positionEvent2.Position + zero);
				}
				positionEvent = positionEvent2;
			}
		}
		double time = this.positionHistory[0].Time;
		double time2 = this.positionHistory[index].Time;
		double num2 = double.MinValue;
		int localInstigatorID = VRC.Network.LocalInstigatorID;
		foreach (SyncPhysics.PositionEvent positionEvent3 in this.positionHistory)
		{
			Color color2 = Color.yellow;
			if (positionEvent3.isCollision)
			{
				color2 = Color.white;
			}
			else if (positionEvent3.RecentOwnershipChange)
			{
				color2 = Color.green;
			}
			else if (positionEvent3.HeldInHand != VRC_Pickup.PickupHand.None)
			{
				color2 = Color.black;
			}
			else if (positionEvent3.isSleeping)
			{
				color2 = Color.blue;
			}
			else if (positionEvent3.Time == num2)
			{
				color2 = Color.magenta;
			}
			num2 = positionEvent3.Time;
			Gizmos.color = color2;
			if (positionEvent3.OwnerID == localInstigatorID)
			{
				Gizmos.DrawWireSphere(positionEvent3.Position, 0.05f);
			}
			else
			{
				Gizmos.DrawWireCube(positionEvent3.Position, new Vector3(0.05f, 0.05f, 0.05f));
			}
		}
	}

	// Token: 0x17000CE4 RID: 3300
	// (get) Token: 0x06005963 RID: 22883 RVA: 0x001F0EF4 File Offset: 0x001EF2F4
	public float UserJoinedUnsettlingTime
	{
		get
		{
			return (float)(0.10000000149011612 + VRC.Network.SimulationDelay(base.Owner)) * 35f;
		}
	}

	// Token: 0x17000CE5 RID: 3301
	// (get) Token: 0x06005964 RID: 22884 RVA: 0x001F0F12 File Offset: 0x001EF312
	public bool isHeld
	{
		get
		{
			return this.HeldInHand != VRC_Pickup.PickupHand.None;
		}
	}

	// Token: 0x17000CE6 RID: 3302
	// (get) Token: 0x06005965 RID: 22885 RVA: 0x001F0F20 File Offset: 0x001EF320
	// (set) Token: 0x06005966 RID: 22886 RVA: 0x001F0F28 File Offset: 0x001EF328
	public float LastUnsettledTime
	{
		get
		{
			return this._lastUnsettledTime;
		}
		set
		{
			if (this._lastUnsettledTime < value)
			{
				this._lastUnsettledTime = value;
			}
		}
	}

	// Token: 0x17000CE7 RID: 3303
	// (get) Token: 0x06005967 RID: 22887 RVA: 0x001F0F40 File Offset: 0x001EF340
	public bool WasDiscontinuousRecently
	{
		get
		{
			if (this.LastPosition == null)
			{
				return true;
			}
			double time = this.LastPosition.Time;
			double num = VRC.Network.SimulationDelay(base.Owner) * 2.0;
			for (int i = this.positionHistory.Count - 1; i > 0; i--)
			{
				SyncPhysics.PositionEvent positionEvent = this.positionHistory[i];
				if (positionEvent.OwnerID != base.OwnerId || positionEvent.RecentOwnershipChange || positionEvent.Discontinuity)
				{
					return true;
				}
				if (time - positionEvent.Time > num)
				{
					return false;
				}
			}
			return true;
		}
	}

	// Token: 0x06005968 RID: 22888 RVA: 0x001F0FE4 File Offset: 0x001EF3E4
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{

	})]
	public void EnableKinematic(VRC.Player instigator)
	{
		if (base.isMine && this.hasRigidbody && (!this.isPlayer || instigator == base.Owner) && !this.rigidbody.isKinematic)
		{
			this.rigidbody.isKinematic = true;
			this.StorePhysicsState();
		}
	}

	// Token: 0x06005969 RID: 22889 RVA: 0x001F1048 File Offset: 0x001EF448
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{

	})]
	public void DisableKinematic(VRC.Player instigator)
	{
		if (base.isMine && this.hasRigidbody && (!this.isPlayer || instigator == base.Owner) && this.rigidbody.isKinematic)
		{
			this.rigidbody.isKinematic = false;
			this.StorePhysicsState();
		}
	}

	// Token: 0x0600596A RID: 22890 RVA: 0x001F10AC File Offset: 0x001EF4AC
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{

	})]
	public void EnableGravity(VRC.Player instigator)
	{
		if (base.isMine && this.hasRigidbody && (!this.isPlayer || instigator == base.Owner) && !this.rigidbody.useGravity)
		{
			this.rigidbody.useGravity = true;
			this.StorePhysicsState();
		}
	}

	// Token: 0x0600596B RID: 22891 RVA: 0x001F1110 File Offset: 0x001EF510
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{

	})]
	public void DisableGravity(VRC.Player instigator)
	{
		if (base.isMine && this.hasRigidbody && (!this.isPlayer || instigator == base.Owner) && this.rigidbody.useGravity)
		{
			this.rigidbody.useGravity = false;
			this.StorePhysicsState();
		}
	}

	// Token: 0x0600596C RID: 22892 RVA: 0x001F1171 File Offset: 0x001EF571
	public void StorePhysicsState()
	{
		if (this.hasRigidbody)
		{
			this.originalUseGravity = this.rigidbody.useGravity;
			this.originalIsKinematic = this.rigidbody.isKinematic;
		}
	}

	// Token: 0x0600596D RID: 22893 RVA: 0x001F11A0 File Offset: 0x001EF5A0
	public void RevertPhysics()
	{
		if (this.originalUseGravity)
		{
			this.EnableGravity(VRC.Network.LocalPlayer);
		}
		else
		{
			this.DisableGravity(VRC.Network.LocalPlayer);
		}
		if (this.originalIsKinematic)
		{
			this.EnableKinematic(VRC.Network.LocalPlayer);
		}
		else
		{
			this.DisableKinematic(VRC.Network.LocalPlayer);
		}
	}

	// Token: 0x17000CE8 RID: 3304
	// (get) Token: 0x0600596E RID: 22894 RVA: 0x001F11FC File Offset: 0x001EF5FC
	public Vector3 ObservedVelocity
	{
		get
		{
			if (this.positionHistory.Count < 2 || this.WasDiscontinuousRecently)
			{
				return Vector3.zero;
			}
			SyncPhysics.PositionEvent lastPosition = this.LastPosition;
			if (lastPosition == null || lastPosition.Discontinuity)
			{
				return Vector3.zero;
			}
			SyncPhysics.PositionEvent penultimatePosition = this.PenultimatePosition;
			if (penultimatePosition == null || penultimatePosition.Discontinuity)
			{
				return Vector3.zero;
			}
			return (lastPosition.Position - penultimatePosition.Position) / (float)(lastPosition.Time - penultimatePosition.Time);
		}
	}

	// Token: 0x17000CE9 RID: 3305
	// (get) Token: 0x0600596F RID: 22895 RVA: 0x001F128C File Offset: 0x001EF68C
	public Quaternion ReplicatedRotation
	{
		get
		{
			SyncPhysics.PositionEvent lastReplicatedPosition = this.LastReplicatedPosition;
			if (lastReplicatedPosition == null)
			{
				return Quaternion.identity;
			}
			return lastReplicatedPosition.Rotation;
		}
	}

	// Token: 0x17000CEA RID: 3306
	// (get) Token: 0x06005970 RID: 22896 RVA: 0x001F12B4 File Offset: 0x001EF6B4
	public Vector3 ReplicatedVelocity
	{
		get
		{
			SyncPhysics.PositionEvent lastReplicatedPosition = this.LastReplicatedPosition;
			if (lastReplicatedPosition == null)
			{
				return Vector3.zero;
			}
			return lastReplicatedPosition.Velocity;
		}
	}

	// Token: 0x17000CEB RID: 3307
	// (get) Token: 0x06005971 RID: 22897 RVA: 0x001F12DC File Offset: 0x001EF6DC
	public Vector3 ReplicatedPosition
	{
		get
		{
			SyncPhysics.PositionEvent lastReplicatedPosition = this.LastReplicatedPosition;
			if (lastReplicatedPosition == null)
			{
				return Vector3.zero;
			}
			return lastReplicatedPosition.Position;
		}
	}

	// Token: 0x17000CEC RID: 3308
	// (get) Token: 0x06005972 RID: 22898 RVA: 0x001F1304 File Offset: 0x001EF704
	public double LastReplicatedTime
	{
		get
		{
			SyncPhysics.PositionEvent lastReplicatedPosition = this.LastReplicatedPosition;
			if (lastReplicatedPosition == null)
			{
				return 0.0;
			}
			return lastReplicatedPosition.Time;
		}
	}

	// Token: 0x17000CED RID: 3309
	// (get) Token: 0x06005973 RID: 22899 RVA: 0x001F1330 File Offset: 0x001EF730
	public SyncPhysics.PositionEvent LastReplicatedPosition
	{
		get
		{
			for (int i = this.positionHistory.Count - 1; i > 0; i--)
			{
				if (!this.positionHistory[i].Skip)
				{
					return this.positionHistory[i];
				}
			}
			return null;
		}
	}

	// Token: 0x17000CEE RID: 3310
	// (get) Token: 0x06005974 RID: 22900 RVA: 0x001F1380 File Offset: 0x001EF780
	public SyncPhysics.PositionEvent PenultimateReplicatedPosition
	{
		get
		{
			int num = 1;
			for (int i = this.positionHistory.Count - 1; i > 0; i--)
			{
				if (!this.positionHistory[i].Skip)
				{
					if (num == 0)
					{
						return this.positionHistory[i];
					}
					num--;
				}
			}
			return null;
		}
	}

	// Token: 0x17000CEF RID: 3311
	// (get) Token: 0x06005975 RID: 22901 RVA: 0x001F13DC File Offset: 0x001EF7DC
	public SyncPhysics.PositionEvent LastPosition
	{
		get
		{
			if (this.positionHistory == null)
			{
				return null;
			}
			int num = this.positionHistory.Count - 1;
			if (num < 0)
			{
				return null;
			}
			return this.positionHistory[num];
		}
	}

	// Token: 0x17000CF0 RID: 3312
	// (get) Token: 0x06005976 RID: 22902 RVA: 0x001F141C File Offset: 0x001EF81C
	public SyncPhysics.PositionEvent PenultimatePosition
	{
		get
		{
			if (this.positionHistory == null)
			{
				return null;
			}
			int num = this.positionHistory.Count - 2;
			if (num < 0)
			{
				return null;
			}
			return this.positionHistory[num];
		}
	}

	// Token: 0x17000CF1 RID: 3313
	// (get) Token: 0x06005977 RID: 22903 RVA: 0x001F1459 File Offset: 0x001EF859
	public bool isSleeping
	{
		get
		{
			return (this.replicateFlags & 8) != 0;
		}
	}

	// Token: 0x17000CF2 RID: 3314
	// (get) Token: 0x06005978 RID: 22904 RVA: 0x001F1469 File Offset: 0x001EF869
	// (set) Token: 0x06005979 RID: 22905 RVA: 0x001F147D File Offset: 0x001EF87D
	public bool RecentOwnershipChange
	{
		get
		{
			return (this.replicateFlags & 512) != 0;
		}
		set
		{
			if (value)
			{
				this.replicateFlags |= 512;
			}
			else
			{
				this.replicateFlags &= -513;
			}
		}
	}

	// Token: 0x17000CF3 RID: 3315
	// (get) Token: 0x0600597A RID: 22906 RVA: 0x001F14B0 File Offset: 0x001EF8B0
	// (set) Token: 0x0600597B RID: 22907 RVA: 0x001F14C4 File Offset: 0x001EF8C4
	public bool RecentCollision
	{
		get
		{
			return (this.replicateFlags & 256) != 0;
		}
		set
		{
			if (value)
			{
				this.replicateFlags |= 256;
			}
			else
			{
				this.replicateFlags &= -257;
			}
		}
	}

	// Token: 0x17000CF4 RID: 3316
	// (get) Token: 0x0600597C RID: 22908 RVA: 0x001F14F7 File Offset: 0x001EF8F7
	// (set) Token: 0x0600597D RID: 22909 RVA: 0x001F1507 File Offset: 0x001EF907
	public bool ReplicateVelocity
	{
		get
		{
			return (this.replicateFlags & 2) != 0;
		}
		set
		{
			if (value && this.ReplicatePosition)
			{
				this.replicateFlags |= 2;
			}
			else
			{
				this.replicateFlags &= -3;
			}
		}
	}

	// Token: 0x17000CF5 RID: 3317
	// (get) Token: 0x0600597E RID: 22910 RVA: 0x001F153E File Offset: 0x001EF93E
	// (set) Token: 0x0600597F RID: 22911 RVA: 0x001F154E File Offset: 0x001EF94E
	public bool ReplicatePosition
	{
		get
		{
			return (this.replicateFlags & 1) != 0;
		}
		set
		{
			if (value)
			{
				this.replicateFlags |= 1;
			}
			else
			{
				this.replicateFlags &= -2;
			}
		}
	}

	// Token: 0x17000CF6 RID: 3318
	// (get) Token: 0x06005980 RID: 22912 RVA: 0x001F157A File Offset: 0x001EF97A
	private bool hasRigidbody
	{
		get
		{
			return this.rigidbody != null && this.rigidbody.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x06005981 RID: 22913 RVA: 0x001F15A0 File Offset: 0x001EF9A0
	public override void Awake()
	{
		base.Awake();
		this.ReplicatePosition = true;
		this.ReplicateVelocity = true;
		VRC_ObjectSync componentInSelfOrParent = base.gameObject.GetComponentInSelfOrParent<VRC_ObjectSync>();
		if (componentInSelfOrParent != null)
		{
			bool synchronizePhysics = componentInSelfOrParent.SynchronizePhysics;
			this.ReplicateVelocity = synchronizePhysics;
			this.ReplicatePosition = synchronizePhysics;
		}
		this.rigidbody = base.gameObject.GetComponent<Rigidbody>();
		if (this.rigidbody != null)
		{
			this.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		}
		if (this.GetRotation == null)
		{
			this.GetRotation = (() => (!this.hasRigidbody) ? base.transform.rotation : this.rigidbody.rotation);
		}
		if (this.SetRotation == null)
		{
			this.SetRotation = delegate(Quaternion rotation)
			{
				if (this.hasRigidbody && !this.isHeld)
				{
					this.rigidbody.MoveRotation(rotation);
				}
				else
				{
					base.transform.rotation = rotation;
				}
			};
		}
		if (this.GetPosition == null)
		{
			this.GetPosition = (() => (!this.hasRigidbody) ? base.transform.position : this.rigidbody.position);
		}
		if (this.SetPosition == null)
		{
			this.SetPosition = delegate(Vector3 position)
			{
				if (this.hasRigidbody && !this.isHeld)
				{
					this.rigidbody.MovePosition(position);
				}
				else
				{
					base.transform.position = position;
				}
			};
		}
		if (this.GetVelocity == null)
		{
			this.GetVelocity = (() => (this.hasRigidbody && !this.isHeld) ? this.rigidbody.velocity : this.ObservedVelocity);
		}
		if (this.SetVelocity == null)
		{
			this.SetVelocity = delegate(Vector3 velocity)
			{
				if (this.hasRigidbody)
				{
					this.rigidbody.velocity = velocity;
				}
			};
		}
		if (this.hasRigidbody)
		{
			this.rigidbody.sleepThreshold = Mathf.Max(0.05f, this.rigidbody.sleepThreshold);
		}
		if (this.InterpolateFunction == null)
		{
			if (SyncPhysics.f__mg0 == null)
			{
				SyncPhysics.f__mg0 = new TweenFunctions.TweenFunction<SyncPhysics.PositionEvent>(TweenFunctions.CatMullRomTween<SyncPhysics.PositionEvent>);
			}
			this.InterpolateFunction = SyncPhysics.f__mg0;
		}
		this.StorePhysicsState();
	}

	// Token: 0x06005982 RID: 22914 RVA: 0x001F172C File Offset: 0x001EFB2C
	public override IEnumerator Start()
	{
		this.isPlayer = (base.gameObject.GetComponentInSelfOrParent<VRC.Player>() != null);
		this.hasAnimatedParts = (this.isPlayer || base.gameObject.GetComponentInSelfOrParent<Animation>() != null || base.GetComponentInChildren<Animation>() != null || base.gameObject.GetComponentInSelfOrParent<Animation>() != null || base.GetComponentInChildren<Animator>() != null);
		this.hasCamera = (base.gameObject.GetComponentInChildren<Camera>() != null);
		if (!this.isPlayer && base.GetComponent<SyncPhysicsLateUpdate>() == null && base.GetComponent<SyncPhysicsFixedUpdate>() == null)
		{
			this.UpdateComponent = base.gameObject.AddComponent<SyncPhysicsLateUpdate>();
		}
        yield return base.Start();
		base.ObserveThis();
		if (SyncPhysics.f__mg1 == null)
		{
			SyncPhysics.f__mg1 = new Func<bool>(VRC.Network.Get_IsNetworkSettled);
		}
		yield return new WaitUntil(SyncPhysics.f__mg1);
		this._lastUnsettledTime = Time.time;
		yield break;
	}

	// Token: 0x06005983 RID: 22915 RVA: 0x001F1748 File Offset: 0x001EFB48
	private void LateUpdate()
	{
		if (VRC.Network.IsSadAndAlone && (this.LastPosition == null || (double)Time.time - this.LastPosition.Time > VRC.Network.SendInterval))
		{
			this.DiscontinuityHint = false;
			TweenFunctions.RecordValue<SyncPhysics.PositionEvent>(this.positionHistory, this.MakeCurrentPositionEvent((double)Time.time));
		}
		if (base.isMine)
		{
			if (this.hasRigidbody)
			{
				if (!this.isHeld)
				{
					this.rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
				}
				else
				{
					this.rigidbody.interpolation = RigidbodyInterpolation.None;
				}
				if (!this.rigidbody.IsSleeping() && !this.ShouldSleep())
				{
					this.LastUnsettledTime = Time.time;
				}
			}
			this.UpdateFlags();
		}
		else if (this.hasRigidbody)
		{
			this.rigidbody.interpolation = RigidbodyInterpolation.None;
		}
	}

	// Token: 0x06005984 RID: 22916 RVA: 0x001F182A File Offset: 0x001EFC2A
	private void OnCollisionEnter(Collision collision)
	{
		if (!base.isMine)
		{
			return;
		}
		this.RecentCollision = true;
	}

	// Token: 0x06005985 RID: 22917 RVA: 0x001F1840 File Offset: 0x001EFC40
	public void TeleportTo(Vector3 position, Quaternion rotation)
	{
		base.transform.position = position;
		base.transform.rotation = rotation;
		if (this.hasRigidbody)
		{
			this.rigidbody.velocity = Vector3.zero;
			this.rigidbody.angularVelocity = Vector3.zero;
			this.rigidbody.position = position;
			this.rigidbody.rotation = rotation;
		}
		this.DiscontinuityHint = true;
	}

	// Token: 0x06005986 RID: 22918 RVA: 0x001F18B0 File Offset: 0x001EFCB0
	public bool ShouldSleep()
	{
		if (this.isPlayer)
		{
			return false;
		}
		if ((double)Time.time < base.OwnershipTransferTime + (double)this.UserJoinedUnsettlingTime)
		{
			return false;
		}
		if (this.isHeld || this.WasDiscontinuousRecently)
		{
			return false;
		}
		if (!this.hasRigidbody || this.rigidbody.IsSleeping())
		{
			return true;
		}
		Vector3 target = this.GetVelocity();
		if (!target.AlmostEquals(Vector3.zero, 1E-05f))
		{
			return false;
		}
		SyncPhysics.PositionEvent penultimatePosition = this.PenultimatePosition;
		if (penultimatePosition == null || penultimatePosition.Discontinuity)
		{
			return false;
		}
		Vector3 target2 = this.GetPosition();
		if (!target2.AlmostEquals(penultimatePosition.Position, 1E-05f))
		{
			return false;
		}
		Quaternion target3 = this.GetRotation();
		return target3.AlmostEquals(penultimatePosition.Rotation, 0.1f);
	}

	// Token: 0x06005987 RID: 22919 RVA: 0x001F19A0 File Offset: 0x001EFDA0
	public void OnPlayerJoined(VRC_PlayerApi apiPlayer)
	{
		this.LastUnsettledTime = Time.time + this.UserJoinedUnsettlingTime;
	}

	// Token: 0x06005988 RID: 22920 RVA: 0x001F19B4 File Offset: 0x001EFDB4
	private void UpdateFlags()
	{
		if (!base.isMine)
		{
			return;
		}
		SyncPhysics.PositionEvent lastReplicatedPosition = this.LastReplicatedPosition;
		this.replicateFlags &= -513;
		if (lastReplicatedPosition != null && lastReplicatedPosition.OwnerID != base.photonView.ownerId)
		{
			this.replicateFlags |= 512;
		}
		if (!this.hasRigidbody || this.rigidbody.isKinematic)
		{
			this.replicateFlags |= 4;
		}
		else if ((this.replicateFlags & 4) != 0)
		{
			this.replicateFlags &= -5;
			this.LastUnsettledTime = Time.time;
		}
		if (this.hasRigidbody && this.rigidbody.useGravity)
		{
			this.replicateFlags |= 16;
		}
		else if ((this.replicateFlags & 16) != 0)
		{
			this.replicateFlags &= -17;
			this.LastUnsettledTime = Time.time;
		}
		if (this.isHeld)
		{
			this.replicateFlags &= -193;
			this.replicateFlags |= (short)((int)this.HeldInHand << 6);
		}
		else if ((this.replicateFlags & 192) != 0)
		{
			this.replicateFlags &= -193;
			this.LastUnsettledTime = Time.time;
		}
		if (this.DiscontinuityHint)
		{
			this.replicateFlags |= 32;
		}
		else if ((this.replicateFlags & 32) != 0)
		{
			this.replicateFlags &= -33;
			this.LastUnsettledTime = Time.time;
		}
		if (base.GetComponentInParent<VRCPlayer>() != null)
		{
			this.replicateFlags &= -9;
		}
		else
		{
			bool flag = this.ShouldSleep();
			if (flag && (double)(Time.time - this.LastUnsettledTime) > VRC.Network.SimulationDelay(base.Owner))
			{
				this.replicateFlags |= 8;
			}
			else
			{
				this.replicateFlags &= -9;
			}
		}
	}

	// Token: 0x06005989 RID: 22921 RVA: 0x001F1BEC File Offset: 0x001EFFEC
	public void DoPositionSync(double now, double delta)
	{
		if (base.isMine || (double)(Time.time - this.LastUnsettledTime) > VRC.Network.SimulationDelay(base.Owner) * 2.0)
		{
			return;
		}
		VRC.Player componentInParent = base.gameObject.GetComponentInParent<VRC.Player>();
		List<VRC_StationInternal> source = VRC_StationInternal.FindActiveStations(componentInParent);
		if (this.isPlayer)
		{
			if ((from s in source
			where s.isImmobilized
			select s).FirstOrDefault<VRC_StationInternal>() != null)
			{
				return;
			}
		}
		VRC_StationInternal componentInParent2 = base.gameObject.GetComponentInParent<VRC_StationInternal>();
		VRC_StationInternal componentInChildren = base.gameObject.GetComponentInChildren<VRC_StationInternal>();
		if (this.hasCamera || base.gameObject.IsVisible() || (componentInParent2 != null && componentInParent2.Occupant != null && componentInParent2.Occupant.isLocal) || (componentInChildren != null && componentInChildren.Occupant != null && componentInChildren.Occupant.isLocal))
		{
			if (SyncPhysics.f__mg2 == null)
			{
				SyncPhysics.f__mg2 = new TweenFunctions.TweenFunction<SyncPhysics.PositionEvent>(TweenFunctions.CatMullRomTween<SyncPhysics.PositionEvent>);
			}
			this.InterpolateFunction = SyncPhysics.f__mg2;
		}
		else
		{
			if (SyncPhysics.f__mg3 == null)
			{
				SyncPhysics.f__mg3 = new TweenFunctions.TweenFunction<SyncPhysics.PositionEvent>(TweenFunctions.NoTween<SyncPhysics.PositionEvent>);
			}
			this.InterpolateFunction = SyncPhysics.f__mg3;
		}
		double num = now - VRC.Network.SimulationDelay(base.Owner);
		SyncPhysics.PositionEvent positionEvent = TweenFunctions.Tween<SyncPhysics.PositionEvent>(this.positionHistory, this.InterpolateFunction, num, VRC.Network.ExpectedInterval, (!this.isPlayer) ? -1 : base.photonView.viewID);
		if (positionEvent == null && this.positionHistory.Full && this.positionHistory[0].Time > num)
		{
			this.positionHistory.Capacity *= 2;
		}
		if (positionEvent != null)
		{
			this.DoAdjustment(positionEvent);
		}
	}

	// Token: 0x0600598A RID: 22922 RVA: 0x001F1DEC File Offset: 0x001F01EC
	private void DoAdjustment(SyncPhysics.PositionEvent evt)
	{
		if (evt == null || base.isMine)
		{
			return;
		}
		this.replicateFlags = evt.flags;
		if (this.hasRigidbody)
		{
			this.rigidbody.isKinematic = evt.isKinematic;
			this.rigidbody.useGravity = evt.useGravity;
		}
		this.HeldInHand = (VRC_Pickup.PickupHand)((evt.flags & 192) >> 6);
		if (evt.Discontinuity)
		{
			this.SetVelocity(Vector3.zero);
		}
		else if (!this.hasAnimatedParts || (this.GetVelocity() - evt.Velocity).sqrMagnitude > 1E-05f)
		{
			this.SetVelocity(evt.Velocity);
		}
		this.SetRotation(evt.Rotation);
		this.SetPosition(evt.Position);
		this.DiscontinuityHint = evt.Discontinuity;
		if (this.hasRigidbody && !this.rigidbody.IsSleeping() && this.ShouldSleep())
		{
			this.rigidbody.Sleep();
		}
	}

	// Token: 0x0600598B RID: 22923 RVA: 0x001F1F1C File Offset: 0x001F031C
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			SyncPhysics.PositionEvent positionEvent = this.MakeCurrentPositionEvent((double)Time.time);
			positionEvent.OwnerID = info.photonView.ownerId;
			bool flag = false;
			this.RecentOwnershipChange = flag;
			this.RecentCollision = flag;
			this.SerializePositionEvent(positionEvent, stream, info);
			this.DiscontinuityHint = false;
			TweenFunctions.RecordValue<SyncPhysics.PositionEvent>(this.positionHistory, positionEvent);
		}
		else
		{
			SyncPhysics.PositionEvent positionEvent = this.DeserializePositionEvent(stream, info);
			positionEvent.OwnerID = info.sender.ID;
			if (this.positionHistory.Full && this.positionHistory[0].Time > (double)Time.time - 2.0 * VRC.Network.SimulationDelay(base.Owner))
			{
				this.positionHistory.Capacity *= 2;
			}
			if (positionEvent.Discontinuity)
			{
				this.positionHistory.Clear();
			}
			TweenFunctions.RecordValue<SyncPhysics.PositionEvent>(this.positionHistory, positionEvent);
			this.LastUnsettledTime = Time.time;
		}
	}

	// Token: 0x0600598C RID: 22924 RVA: 0x001F2023 File Offset: 0x001F0423
	private void SerializeVector(PhotonStream stream, Vector3 vect)
	{
		Serialization.SerializeVector(stream, vect);
	}

	// Token: 0x0600598D RID: 22925 RVA: 0x001F202C File Offset: 0x001F042C
	private Vector3 DeserializeVector(PhotonStream stream)
	{
		return Serialization.DeserializeVector(stream);
	}

	// Token: 0x0600598E RID: 22926 RVA: 0x001F2034 File Offset: 0x001F0434
	private void SerializeQuaternion(PhotonStream stream, Quaternion quat)
	{
		Serialization.SerializeQuaternionAsShorts(stream, quat);
	}

	// Token: 0x0600598F RID: 22927 RVA: 0x001F203D File Offset: 0x001F043D
	private Quaternion DeserializeQuaternion(PhotonStream stream)
	{
		return Serialization.DeserializeQuaternionFromShorts(stream);
	}

	// Token: 0x06005990 RID: 22928 RVA: 0x001F2048 File Offset: 0x001F0448
	private void SerializePositionEvent(SyncPhysics.PositionEvent evt, PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(evt.flags);
		if (evt.isSleeping)
		{
			return;
		}
		this.SerializeQuaternion(stream, evt.Rotation);
		if (evt.ReplicatePosition)
		{
			this.SerializeVector(stream, evt.Position);
		}
		if (evt.ReplicateVelocity)
		{
			this.SerializeVector(stream, evt.Velocity);
		}
	}

	// Token: 0x06005991 RID: 22929 RVA: 0x001F20B0 File Offset: 0x001F04B0
	private SyncPhysics.PositionEvent DeserializePositionEvent(PhotonStream stream, PhotonMessageInfo info)
	{
		SyncPhysics.PositionEvent positionEvent = this.LastReplicatedPosition;
		if (positionEvent == null)
		{
			positionEvent = this.MakeCurrentPositionEvent((double)Time.time);
		}
		short flags = (short)stream.ReceiveNext();
		SyncPhysics.PositionEvent positionEvent2 = new SyncPhysics.PositionEvent
		{
			flags = flags,
			Time = (double)Time.time,
			Position = positionEvent.Position,
			Velocity = positionEvent.Velocity,
			Rotation = positionEvent.Rotation
		};
		if (positionEvent2.isSleeping)
		{
			return positionEvent2;
		}
		positionEvent2.Rotation = this.DeserializeQuaternion(stream);
		if (positionEvent2.ReplicatePosition)
		{
			positionEvent2.Position = this.DeserializeVector(stream);
		}
		if (positionEvent2.ReplicateVelocity)
		{
			positionEvent2.Velocity = this.DeserializeVector(stream);
		}
		return positionEvent2;
	}

	// Token: 0x04003FEE RID: 16366
	private bool hasAnimatedParts;

	// Token: 0x04003FEF RID: 16367
	private bool hasCamera;

	// Token: 0x04003FF0 RID: 16368
	private bool isPlayer;

	// Token: 0x04003FF1 RID: 16369
	private const float SleepSqrDistanceEpsilon = 1E-05f;

	// Token: 0x04003FF2 RID: 16370
	private const float SleepRotationEpsilon = 0.1f;

	// Token: 0x04003FF3 RID: 16371
	public bool DiscontinuityHint;

	// Token: 0x04003FF4 RID: 16372
	public VRC_Pickup.PickupHand HeldInHand;

	// Token: 0x04003FF5 RID: 16373
	public TweenFunctions.TweenFunction<SyncPhysics.PositionEvent> InterpolateFunction;

	// Token: 0x04003FF6 RID: 16374
	public SyncPhysics.SetVector3 SetVelocity;

	// Token: 0x04003FF7 RID: 16375
	public SyncPhysics.GetVector3 GetVelocity;

	// Token: 0x04003FF8 RID: 16376
	public SyncPhysics.SetVector3 SetPosition;

	// Token: 0x04003FF9 RID: 16377
	public SyncPhysics.GetVector3 GetPosition;

	// Token: 0x04003FFA RID: 16378
	public SyncPhysics.SetQuaternion SetRotation;

	// Token: 0x04003FFB RID: 16379
	public SyncPhysics.GetQuaternion GetRotation;

	// Token: 0x04003FFC RID: 16380
	public MonoBehaviour UpdateComponent;

	// Token: 0x04003FFD RID: 16381
	private LimitedCapacityList<SyncPhysics.PositionEvent> positionHistory = new LimitedCapacityList<SyncPhysics.PositionEvent>();

	// Token: 0x04003FFE RID: 16382
	private bool originalUseGravity;

	// Token: 0x04003FFF RID: 16383
	private bool originalIsKinematic = true;

	// Token: 0x04004000 RID: 16384
	public Rigidbody rigidbody;

	// Token: 0x04004001 RID: 16385
	private short replicateFlags;

	// Token: 0x04004002 RID: 16386
	private float _lastUnsettledTime;

	// Token: 0x04004003 RID: 16387
	[CompilerGenerated]
	private static TweenFunctions.TweenFunction<SyncPhysics.PositionEvent> f__mg0;

	// Token: 0x04004004 RID: 16388
	[CompilerGenerated]
	private static Func<bool> f__mg1;

	// Token: 0x04004005 RID: 16389
	[CompilerGenerated]
	private static TweenFunctions.TweenFunction<SyncPhysics.PositionEvent> f__mg2;

	// Token: 0x04004006 RID: 16390
	[CompilerGenerated]
	private static TweenFunctions.TweenFunction<SyncPhysics.PositionEvent> f__mg3;

	// Token: 0x02000B63 RID: 2915
	public class PositionEvent : TweenFunctions.IPositionSample, ICloneable, TweenFunctions.ITimedValue
	{
		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x0600599B RID: 22939 RVA: 0x001F2286 File Offset: 0x001F0686
		// (set) Token: 0x0600599C RID: 22940 RVA: 0x001F228E File Offset: 0x001F068E
		public Vector3 Position { get; set; }

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x0600599D RID: 22941 RVA: 0x001F2297 File Offset: 0x001F0697
		// (set) Token: 0x0600599E RID: 22942 RVA: 0x001F229F File Offset: 0x001F069F
		public Vector3 Velocity { get; set; }

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x0600599F RID: 22943 RVA: 0x001F22A8 File Offset: 0x001F06A8
		// (set) Token: 0x060059A0 RID: 22944 RVA: 0x001F22B0 File Offset: 0x001F06B0
		public Quaternion Rotation { get; set; }

		// Token: 0x060059A1 RID: 22945 RVA: 0x001F22BC File Offset: 0x001F06BC
		public object Clone()
		{
			return new SyncPhysics.PositionEvent
			{
				flags = this.flags,
				Position = this.Position,
				Velocity = this.Velocity,
				Rotation = this.Rotation,
				OwnerID = this.OwnerID,
				Time = (double)UnityEngine.Time.time
			};
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x060059A2 RID: 22946 RVA: 0x001F2318 File Offset: 0x001F0718
		public bool Skip
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x060059A3 RID: 22947 RVA: 0x001F231B File Offset: 0x001F071B
		// (set) Token: 0x060059A4 RID: 22948 RVA: 0x001F2323 File Offset: 0x001F0723
		public double Time { get; set; }

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x060059A5 RID: 22949 RVA: 0x001F232C File Offset: 0x001F072C
		// (set) Token: 0x060059A6 RID: 22950 RVA: 0x001F2340 File Offset: 0x001F0740
		public bool RecentOwnershipChange
		{
			get
			{
				return (this.flags & 512) != 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 512;
				}
				else
				{
					this.flags &= -513;
				}
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x060059A7 RID: 22951 RVA: 0x001F2373 File Offset: 0x001F0773
		// (set) Token: 0x060059A8 RID: 22952 RVA: 0x001F2387 File Offset: 0x001F0787
		public bool isCollision
		{
			get
			{
				return (this.flags & 256) != 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 256;
				}
				else
				{
					this.flags &= -257;
				}
			}
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x060059A9 RID: 22953 RVA: 0x001F23BA File Offset: 0x001F07BA
		// (set) Token: 0x060059AA RID: 22954 RVA: 0x001F23CA File Offset: 0x001F07CA
		public VRC_Pickup.PickupHand HeldInHand
		{
			get
			{
				return (VRC_Pickup.PickupHand)((this.flags & 192) >> 6);
			}
			set
			{
				this.flags &= -193;
				this.flags = (short)((int)this.flags | (int)value << 6);
			}
		}

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x060059AB RID: 22955 RVA: 0x001F23F0 File Offset: 0x001F07F0
		// (set) Token: 0x060059AC RID: 22956 RVA: 0x001F2401 File Offset: 0x001F0801
		public bool Discontinuity
		{
			get
			{
				return (this.flags & 32) != 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 32;
				}
				else
				{
					this.flags &= -33;
				}
			}
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x060059AD RID: 22957 RVA: 0x001F242E File Offset: 0x001F082E
		// (set) Token: 0x060059AE RID: 22958 RVA: 0x001F243F File Offset: 0x001F083F
		public bool useGravity
		{
			get
			{
				return (this.flags & 16) != 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 16;
				}
				else
				{
					this.flags &= -17;
				}
			}
		}

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x060059AF RID: 22959 RVA: 0x001F246C File Offset: 0x001F086C
		// (set) Token: 0x060059B0 RID: 22960 RVA: 0x001F247C File Offset: 0x001F087C
		public bool isSleeping
		{
			get
			{
				return (this.flags & 8) != 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 8;
				}
				else
				{
					this.flags &= -9;
				}
			}
		}

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x060059B1 RID: 22961 RVA: 0x001F24A8 File Offset: 0x001F08A8
		// (set) Token: 0x060059B2 RID: 22962 RVA: 0x001F24B8 File Offset: 0x001F08B8
		public bool isKinematic
		{
			get
			{
				return (this.flags & 4) != 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 4;
				}
				else
				{
					this.flags &= -5;
				}
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x060059B3 RID: 22963 RVA: 0x001F24E4 File Offset: 0x001F08E4
		// (set) Token: 0x060059B4 RID: 22964 RVA: 0x001F24F4 File Offset: 0x001F08F4
		public bool ReplicateVelocity
		{
			get
			{
				return (this.flags & 2) != 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 2;
				}
				else
				{
					this.flags &= -3;
				}
			}
		}

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x060059B5 RID: 22965 RVA: 0x001F2520 File Offset: 0x001F0920
		// (set) Token: 0x060059B6 RID: 22966 RVA: 0x001F2530 File Offset: 0x001F0930
		public bool ReplicatePosition
		{
			get
			{
				return (this.flags & 1) != 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 1;
				}
				else
				{
					this.flags &= -2;
				}
			}
		}

		// Token: 0x04004008 RID: 16392
		public short flags;

		// Token: 0x0400400C RID: 16396
		public int OwnerID;
	}

	// Token: 0x02000B64 RID: 2916
	// (Invoke) Token: 0x060059B8 RID: 22968
	public delegate void SetVector3(Vector3 newValue);

	// Token: 0x02000B65 RID: 2917
	// (Invoke) Token: 0x060059BC RID: 22972
	public delegate Vector3 GetVector3();

	// Token: 0x02000B66 RID: 2918
	// (Invoke) Token: 0x060059C0 RID: 22976
	public delegate void SetQuaternion(Quaternion newValue);

	// Token: 0x02000B67 RID: 2919
	// (Invoke) Token: 0x060059C4 RID: 22980
	public delegate Quaternion GetQuaternion();
}
