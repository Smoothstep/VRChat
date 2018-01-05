using System;
using System.Collections;
using RAIN.Core;
using RAIN.Memory;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000B5C RID: 2908
public class ObjectInternal : VRCPunBehaviour
{
	// Token: 0x0600590A RID: 22794 RVA: 0x001EDDA8 File Offset: 0x001EC1A8
	private void OnDrawGizmos()
	{
		Renderer component = base.GetComponent<Renderer>();
		Gizmos.color = ((!base.gameObject.IsMine()) ? Color.yellow : Color.green);
		Gizmos.DrawWireCube(base.transform.position, (!(component != null)) ? new Vector3(0.1f, 0.1f, 0.1f) : component.bounds.size);
	}

	// Token: 0x17000CD9 RID: 3289
	// (get) Token: 0x0600590B RID: 22795 RVA: 0x001EDE23 File Offset: 0x001EC223
	// (set) Token: 0x0600590C RID: 22796 RVA: 0x001EDE44 File Offset: 0x001EC244
	public bool DiscontinuityHint
	{
		get
		{
			return this.physicsSync != null && this.physicsSync.DiscontinuityHint;
		}
		set
		{
			if (this.physicsSync != null)
			{
				this.physicsSync.DiscontinuityHint = value;
			}
		}
	}

	// Token: 0x17000CDA RID: 3290
	// (get) Token: 0x0600590D RID: 22797 RVA: 0x001EDE63 File Offset: 0x001EC263
	public bool SyncPhysics
	{
		get
		{
			return this.sync != null && this.sync.SynchronizePhysics;
		}
	}

	// Token: 0x17000CDB RID: 3291
	// (get) Token: 0x0600590E RID: 22798 RVA: 0x001EDE84 File Offset: 0x001EC284
	// (set) Token: 0x0600590F RID: 22799 RVA: 0x001EDEA5 File Offset: 0x001EC2A5
	public bool AllowCollisionTransfer
	{
		get
		{
			return this.sync == null || this.sync.AllowCollisionTransfer;
		}
		set
		{
			if (this.sync != null)
			{
				this.sync.AllowCollisionTransfer = value;
			}
		}
	}

	// Token: 0x17000CDC RID: 3292
	// (get) Token: 0x06005910 RID: 22800 RVA: 0x001EDEC4 File Offset: 0x001EC2C4
	public float LastHeldChangeTime
	{
		get
		{
			return this.lastHeldChangeTime;
		}
	}

	// Token: 0x17000CDD RID: 3293
	// (get) Token: 0x06005911 RID: 22801 RVA: 0x001EDECC File Offset: 0x001EC2CC
	public bool IsSleeping
	{
		get
		{
			return this.physicsSync != null && this.physicsSync.isSleeping;
		}
	}

	// Token: 0x17000CDE RID: 3294
	// (get) Token: 0x06005912 RID: 22802 RVA: 0x001EDEED File Offset: 0x001EC2ED
	// (set) Token: 0x06005913 RID: 22803 RVA: 0x001EDF10 File Offset: 0x001EC310
	public VRC_Pickup.PickupHand HeldInHand
	{
		get
		{
			if (this.physicsSync == null)
			{
				return VRC_Pickup.PickupHand.None;
			}
			return this.physicsSync.HeldInHand;
		}
		set
		{
			if (this.physicsSync != null)
			{
				if ((value != VRC_Pickup.PickupHand.None || this.isHeld) && value != this.physicsSync.HeldInHand)
				{
					this.lastHeldChangeTime = Time.time;
				}
				this.physicsSync.HeldInHand = value;
			}
		}
	}

	// Token: 0x17000CDF RID: 3295
	// (get) Token: 0x06005914 RID: 22804 RVA: 0x001EDF67 File Offset: 0x001EC367
	public bool isHeld
	{
		get
		{
			return !(this.physicsSync == null) && this.physicsSync.isHeld;
		}
	}

	// Token: 0x17000CE0 RID: 3296
	// (get) Token: 0x06005915 RID: 22805 RVA: 0x001EDF87 File Offset: 0x001EC387
	public bool DisallowsTheft
	{
		get
		{
			return this.sync != null && this.pickup != null && this.pickup.DisallowTheft;
		}
	}

	// Token: 0x17000CE1 RID: 3297
	// (get) Token: 0x06005916 RID: 22806 RVA: 0x001EDFB9 File Offset: 0x001EC3B9
	private bool hasRigidbody
	{
		get
		{
			return this.rigidbody != null && this.rigidbody.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x17000CE2 RID: 3298
	// (get) Token: 0x06005917 RID: 22807 RVA: 0x001EDFDF File Offset: 0x001EC3DF
	private AIRig AI
	{
		get
		{
			if (this._ai == null)
			{
				this._ai = base.gameObject.GetComponentInChildren<AIRig>();
			}
			return this._ai;
		}
	}

	// Token: 0x17000CE3 RID: 3299
	// (get) Token: 0x06005918 RID: 22808 RVA: 0x001EE009 File Offset: 0x001EC409
	private RAINMemory AIMemory
	{
		get
		{
			if (this.AI != null && this.AI.AI != null)
			{
				return this.AI.AI.WorkingMemory;
			}
			return null;
		}
	}

	// Token: 0x06005919 RID: 22809 RVA: 0x001EE040 File Offset: 0x001EC440
	public void StorePickupState()
	{
		if (this.physicsSync != null)
		{
			this.physicsSync.StorePhysicsState();
		}
		if (this.pickup != null)
		{
			if (this.hasRigidbody)
			{
				this.pickup.originalGravity = this.rigidbody.useGravity;
				this.pickup.originalKinematic = this.rigidbody.isKinematic;
				this.pickup.physicalRoot = this.rigidbody;
				if (this.collider != null)
				{
					this.pickup.originalTrigger = this.collider.isTrigger;
				}
			}
			this.pickup.originalParent = this.pickup.transform.parent;
		}
	}

	// Token: 0x0600591A RID: 22810 RVA: 0x001EE104 File Offset: 0x001EC504
	public void UpdateEnableCollisionWithPlayer()
	{
		if (this.pickup != null && this.isHeld == this.isCollisionWithPlayersEnabled && this.sentOnNetworkReady)
		{
			this.TryEnableCollisionWithPlayer(!this.isHeld);
		}
	}

	// Token: 0x0600591B RID: 22811 RVA: 0x001EE144 File Offset: 0x001EC544
	private void TryEnableCollisionWithPlayer(bool enable)
	{
		if (VRCPlayer.Instance == null)
		{
			return;
		}
		Collider component = VRCPlayer.Instance.GetComponent<CharacterController>();
		if (component == null)
		{
			Debug.LogError("EnableCollisionWithPlayer: Local VRCPlayer does not have a CharacterController!");
			return;
		}
		Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return;
		}
		if (enable)
		{
			bool flag = false;
			foreach (Collider collider in componentsInChildren)
			{
				if (!collider.isTrigger && collider.bounds.Intersects(component.bounds))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				return;
			}
		}
		int num = LayerMask.NameToLayer("Player");
		int num2 = LayerMask.NameToLayer("PlayerLocal");
		Collider[] componentsInChildren2 = VRCPlayer.Instance.GetComponentsInChildren<Collider>();
		foreach (Collider collider2 in componentsInChildren)
		{
			if (collider2 != null && !collider2.isTrigger)
			{
				foreach (Collider collider3 in componentsInChildren2)
				{
					int layer = collider3.gameObject.layer;
					if (!collider3.isTrigger && (layer == num || layer == num2))
					{
						Physics.IgnoreCollision(collider2, collider3, !enable);
					}
				}
			}
		}
		this.isCollisionWithPlayersEnabled = enable;
	}

	// Token: 0x0600591C RID: 22812 RVA: 0x001EE2C0 File Offset: 0x001EC6C0
	public override void Awake()
	{
		base.Awake();
		this.sync = base.GetComponent<VRC_ObjectSync>();
		this.animator = base.GetComponent<Animator>();
		this.targetAnimation = base.GetComponent<Animation>();
		this.destructible = base.GetComponent<IVRC_Destructible>();
		this.ConfigurePhysics();
		this.ConfigureAI();
		if (VRC_SceneDescriptor.Instance != null)
		{
			this.DestroyHeightY = VRC_SceneDescriptor.Instance.RespawnHeightY;
			this.killBehaviour = VRC_SceneDescriptor.Instance.ObjectBehaviourAtRespawnHeight;
		}
		if (this.animator != null || this.targetAnimation != null)
		{
			base.gameObject.GetOrAddComponent<VRC_SyncAnimation>();
		}
	}

	// Token: 0x0600591D RID: 22813 RVA: 0x001EE370 File Offset: 0x001EC770
	public override IEnumerator Start()
	{
		yield return base.Start();
		base.ObserveThis();
		this.pickup = base.GetComponent<VRC_Pickup>();
		this.rigidbody = base.GetComponent<Rigidbody>();
		this.collider = base.GetComponent<Collider>();
		this.StorePickupState();
		this.initialLocation = base.transform.position;
		this.initialRotation = base.transform.rotation;
		NetworkManager.Instance.OnOwnershipTransferedEvent += this.OnOwnershipTransfered;
		if (this.physicsSync != null)
		{
			this.physicsSync.enabled = (this.sync != null && this.sync.SynchronizePhysics);
		}
		yield break;
	}

	// Token: 0x0600591E RID: 22814 RVA: 0x001EE38C File Offset: 0x001EC78C
	protected override void OnNetworkReady()
	{
		base.OnNetworkReady();
		if (base.photonView != null)
		{
			base.photonView.ownershipTransferTime = (double)Time.time;
			this.initialSync = base.photonView.synchronization;
		}
		if (this.animator != null && !base.isMine)
		{
			this.animator.applyRootMotion = false;
		}
		this.sentOnNetworkReady = true;
	}

	// Token: 0x0600591F RID: 22815 RVA: 0x001EE404 File Offset: 0x001EC804
	private void DoSyncUpdate()
	{
		bool isMine = base.isMine;
		bool isHeld = this.isHeld;
		if (base.photonView != null && this.physicsSync != null && this.physicsSync.UpdateComponent != null && this.initialSync == ViewSynchronization.Unreliable)
		{
			if (isMine && (double)(Time.time - this.physicsSync.LastUnsettledTime) > VRC.Network.SimulationDelay(base.Owner) * 2.0)
			{
				base.photonView.synchronization = ViewSynchronization.Off;
				this.physicsSync.UpdateComponent.enabled = false;
			}
			else
			{
				base.photonView.synchronization = this.initialSync;
				this.physicsSync.UpdateComponent.enabled = true;
			}
		}
		if (this.pickup != null)
		{
			if (isHeld && isMine && this.pickup.currentLocalPlayer == null)
			{
				this.HeldInHand = VRC_Pickup.PickupHand.None;
			}
			isHeld = this.isHeld;
			if (isHeld && this.hasRigidbody)
			{
				if (this.rigidbody.useGravity)
				{
					this.rigidbody.useGravity = false;
				}
				this.rigidbody.interpolation = RigidbodyInterpolation.None;
			}
			else
			{
				if (this.physicsSync != null)
				{
					this.physicsSync.RevertPhysics();
				}
				this.pickup.RevertPhysics();
			}
		}
		else if (this.physicsSync != null)
		{
			this.physicsSync.RevertPhysics();
		}
	}

	// Token: 0x06005920 RID: 22816 RVA: 0x001EE5A0 File Offset: 0x001EC9A0
	private void Update()
	{
		if (this.sync != null && base.gameObject.IsReady())
		{
			this.DoSyncUpdate();
		}
		this.UpdateEnableCollisionWithPlayer();
		if (base.transform.position.y < this.DestroyHeightY)
		{
			VRC_SceneDescriptor.RespawnHeightBehaviour respawnHeightBehaviour = this.killBehaviour;
			if (respawnHeightBehaviour != VRC_SceneDescriptor.RespawnHeightBehaviour.Destroy)
			{
				if (respawnHeightBehaviour == VRC_SceneDescriptor.RespawnHeightBehaviour.Respawn)
				{
					if (this.sync != null)
					{
						this.sync.TeleportTo(this.initialLocation, this.initialRotation);
					}
				}
			}
			else
			{
				VRC.Network.Destroy(base.gameObject);
			}
		}
		if (this.pickup != null && !base.isMine && this.isHeld)
		{
			bool hidden = false;
			string ownerUserId = base.OwnerUserId;
			if (ownerUserId != null && ModerationManager.Instance != null && ModerationManager.Instance.IsBlocked(ownerUserId))
			{
				VRCVrCamera instance = VRCVrCamera.GetInstance();
				if (instance != null)
				{
					float sqrMagnitude = (instance.GetWorldCameraPos() - base.transform.position).sqrMagnitude;
					if (sqrMagnitude < 2.25f)
					{
						hidden = true;
					}
				}
			}
			this.SetHidden(hidden);
		}
	}

	// Token: 0x06005921 RID: 22817 RVA: 0x001EE6F0 File Offset: 0x001ECAF0
	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			if (this.AI != null)
			{
				this.AI.enabled = true;
			}
			if (this.destructible != null)
			{
				object[] state = this.destructible.GetState();
				stream.SendNext(state.Length);
				foreach (object obj in state)
				{
					stream.SendNext(obj);
				}
			}
		}
		else
		{
			if (this.AI != null)
			{
				this.AI.enabled = false;
			}
			if (this.destructible != null)
			{
				int num = (int)stream.ReceiveNext();
				object[] array2 = new object[num];
				for (int j = 0; j < num; j++)
				{
					array2[j] = stream.ReceiveNext();
				}
				this.destructible.SetState(array2);
			}
		}
	}

	// Token: 0x06005922 RID: 22818 RVA: 0x001EE7E0 File Offset: 0x001ECBE0
	private void ConfigurePhysics()
	{
		if (this.animator != null)
		{
			this.animatorAppliesRootMotionWhenOwned = this.animator.applyRootMotion;
		}
		this.physicsSync = base.gameObject.GetComponent<SyncPhysics>();
		if (this.sync != null && this.physicsSync == null && this.sync.SynchronizePhysics)
		{
			this.physicsSync = base.gameObject.AddComponent<SyncPhysics>();
		}
	}

	// Token: 0x06005923 RID: 22819 RVA: 0x001EE864 File Offset: 0x001ECC64
	private void ConfigureAI()
	{
		if (this.AIMemory != null)
		{
			if (this.AIMemory.ItemExists("health"))
			{
				NpcMortality npcMortality = base.gameObject.AddComponent<NpcMortality>();
				npcMortality.memory = this.AIMemory;
			}
			if (this.AIMemory.ItemExists("attack"))
			{
				NpcAttack npcAttack = base.gameObject.AddComponent<NpcAttack>();
				npcAttack.damage = this.AIMemory.GetItem<int>("attack");
			}
		}
	}

	// Token: 0x06005924 RID: 22820 RVA: 0x001EE8E0 File Offset: 0x001ECCE0
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit == null || hit.collider == null || hit.collider.gameObject == null)
		{
			return;
		}
		if (hit.collider.gameObject.IsPlayer())
		{
			ObjectInternal.DoColliderOwnershipTransfer(hit.collider.gameObject, base.gameObject);
		}
		else
		{
			ObjectInternal.DoColliderOwnershipTransfer(base.gameObject, hit.collider.gameObject);
		}
	}

	// Token: 0x06005925 RID: 22821 RVA: 0x001EE964 File Offset: 0x001ECD64
	private void OnCollisionEnter(Collision collision)
	{
		if (collision != null && collision.collider != null && ObjectInternal.DoColliderOwnershipTransfer(base.gameObject, collision.collider.gameObject))
		{
			this.doCollisionStayCheck = true;
			this.DoMomentumTransfer(collision);
		}
	}

	// Token: 0x06005926 RID: 22822 RVA: 0x001EE9B4 File Offset: 0x001ECDB4
	private void OnCollisionExit(Collision collision)
	{
		if (collision != null && collision.collider != null && ObjectInternal.DoColliderOwnershipTransfer(base.gameObject, collision.collider.gameObject))
		{
			this.doCollisionStayCheck = true;
			this.DoMomentumTransfer(collision);
		}
	}

	// Token: 0x06005927 RID: 22823 RVA: 0x001EEA04 File Offset: 0x001ECE04
	private void OnCollisionStay(Collision collision)
	{
		if (this.doCollisionStayCheck && collision != null && collision.collider != null)
		{
			ObjectInternal.DoColliderOwnershipTransfer(base.gameObject, collision.collider.gameObject);
			this.doCollisionStayCheck = false;
		}
	}

	// Token: 0x06005928 RID: 22824 RVA: 0x001EEA54 File Offset: 0x001ECE54
	private static bool DoColliderOwnershipTransfer(GameObject thisObject, GameObject thatObject)
	{
		if (thisObject == null || thatObject == null)
		{
			return false;
		}
		bool flag = thisObject.IsMine();
		bool flag2 = thatObject.IsMine();
		if (!flag && !flag2)
		{
			return false;
		}
		bool flag3 = thisObject.IsPlayer();
		bool flag4 = thatObject.IsPlayer();
		if (flag3 && flag4)
		{
			return false;
		}
		bool flag5 = thisObject.IsHeld();
		bool flag6 = thatObject.IsHeld();
		VRC.Player player = thisObject.Owner();
		if (player == null)
		{
			return false;
		}
		VRC.Player player2 = thatObject.Owner();
		if (player2 == null)
		{
			return false;
		}
		if (flag5 && !flag6 && !flag4)
		{
			return VRC.Network.SetOwner(player, thatObject, VRC.Network.OwnershipModificationType.Collision, false);
		}
		if (!flag5 && flag6 && !flag3)
		{
			return VRC.Network.SetOwner(player2, thisObject, VRC.Network.OwnershipModificationType.Collision, false);
		}
		bool flag7 = thisObject.IsPickup();
		bool flag8 = thatObject.IsPickup();
		if (flag7 && !flag8 && !flag4)
		{
			return VRC.Network.SetOwner(player, thatObject, VRC.Network.OwnershipModificationType.Collision, false);
		}
		if (!flag7 && flag8 && !flag3)
		{
			return VRC.Network.SetOwner(player2, thisObject, VRC.Network.OwnershipModificationType.Collision, false);
		}
		double ownershipTransferTime = VRC.Network.GetOwnershipTransferTime(thisObject);
		double ownershipTransferTime2 = VRC.Network.GetOwnershipTransferTime(thatObject);
		double num = VRC.Network.SimulationDelay(player2) * 2.0;
		if ((double)Time.time < ownershipTransferTime2 + num)
		{
			return false;
		}
		if (ownershipTransferTime > ownershipTransferTime2 + num && !flag4)
		{
			return VRC.Network.SetOwner(player, thatObject, VRC.Network.OwnershipModificationType.Collision, false);
		}
		return ownershipTransferTime + num < ownershipTransferTime2 && !flag3 && VRC.Network.SetOwner(player2, thisObject, VRC.Network.OwnershipModificationType.Collision, false);
	}

	// Token: 0x06005929 RID: 22825 RVA: 0x001EEBF0 File Offset: 0x001ECFF0
	private void OnOwnershipTransfered(GameObject obj, VRC.Player newOwner, VRC.Player oldOwner)
	{
		ObjectInternal componentInSelfOrParent = obj.GetComponentInSelfOrParent<ObjectInternal>();
		if (componentInSelfOrParent != null && componentInSelfOrParent.animator != null)
		{
			bool flag = componentInSelfOrParent.isMine && componentInSelfOrParent.animatorAppliesRootMotionWhenOwned;
			if (componentInSelfOrParent.animator.applyRootMotion != flag)
			{
				componentInSelfOrParent.animator.applyRootMotion = flag;
			}
		}
	}

	// Token: 0x0600592A RID: 22826 RVA: 0x001EEC58 File Offset: 0x001ED058
	private void DoMomentumTransfer(Collision collision)
	{
		if (this.physicsSync == null || collision == null || collision.collider == null || collision.contacts == null || collision.contacts.Length == 0)
		{
			return;
		}
		SyncPhysics component = collision.collider.GetComponent<SyncPhysics>();
		if (component != null && (!(this.isHeld ^ component.isHeld) || (!base.isMine && !component.isMine)))
		{
			return;
		}
		Rigidbody component2 = base.GetComponent<Rigidbody>();
		Rigidbody component3 = collision.collider.GetComponent<Rigidbody>();
		if (component2 == null || component3 == null)
		{
			return;
		}
		float num = (!(component2 != null)) ? 1f : component2.mass;
		Vector3 observedVelocity = this.physicsSync.ObservedVelocity;
		float num2 = (!(component3 != null)) ? 1f : component2.mass;
		Vector3 vector = (!(component != null)) ? component3.velocity : component.ObservedVelocity;
		if (this.isHeld)
		{
			Vector3 inNormal = (!observedVelocity.AlmostEquals(Vector3.zero, 0.001f)) ? observedVelocity.normalized : (vector * -1f).normalized;
			Vector3 a = Vector3.Reflect(vector, inNormal) + observedVelocity;
			component3.velocity = Vector3.zero;
			component3.angularVelocity = Vector3.zero;
			component3.AddForceAtPosition(a * (num + num2), collision.contacts[0].point, ForceMode.Force);
		}
		if (component != null && component.isHeld)
		{
			Vector3 inNormal2 = (!vector.AlmostEquals(Vector3.zero, 0.001f)) ? vector.normalized : (observedVelocity * -1f).normalized;
			component2.velocity = Vector3.zero;
			component2.angularVelocity = Vector3.zero;
			Vector3 a2 = Vector3.Reflect(observedVelocity, inNormal2) + vector;
			component2.AddForceAtPosition(a2 * (num + num2), collision.contacts[0].point, ForceMode.Force);
		}
	}

	// Token: 0x0600592B RID: 22827 RVA: 0x001EEEA4 File Offset: 0x001ED2A4
	private void SetHidden(bool hidden)
	{
		if (hidden != this.currentlyHidden)
		{
			if (hidden)
			{
				this.hiddenRenderers = base.GetComponentsInChildren<Renderer>();
				for (int i = 0; i < this.hiddenRenderers.Length; i++)
				{
					if (!this.hiddenRenderers[i].enabled)
					{
						this.hiddenRenderers[i] = null;
					}
					else
					{
						this.hiddenRenderers[i].enabled = false;
					}
				}
			}
			else
			{
				foreach (Renderer renderer in this.hiddenRenderers)
				{
					if (renderer != null)
					{
						renderer.enabled = true;
					}
				}
			}
			this.currentlyHidden = hidden;
		}
	}

	// Token: 0x04003FB1 RID: 16305
	private float lastHeldChangeTime;

	// Token: 0x04003FB2 RID: 16306
	private bool isCollisionWithPlayersEnabled;

	// Token: 0x04003FB3 RID: 16307
	private Rigidbody rigidbody;

	// Token: 0x04003FB4 RID: 16308
	private Collider collider;

	// Token: 0x04003FB5 RID: 16309
	private VRC_ObjectSync sync;

	// Token: 0x04003FB6 RID: 16310
	private VRC_Pickup pickup;

	// Token: 0x04003FB7 RID: 16311
	private IVRC_Destructible destructible;

	// Token: 0x04003FB8 RID: 16312
	private SyncPhysics physicsSync;

	// Token: 0x04003FB9 RID: 16313
	private Animator animator;

	// Token: 0x04003FBA RID: 16314
	private Animation targetAnimation;

	// Token: 0x04003FBB RID: 16315
	private const float blockHiddenRange = 1.5f;

	// Token: 0x04003FBC RID: 16316
	private bool currentlyHidden;

	// Token: 0x04003FBD RID: 16317
	private Renderer[] hiddenRenderers;

	// Token: 0x04003FBE RID: 16318
	private bool doCollisionStayCheck;

	// Token: 0x04003FBF RID: 16319
	public bool sentOnNetworkReady;

	// Token: 0x04003FC0 RID: 16320
	private VRC_SceneDescriptor.RespawnHeightBehaviour killBehaviour = VRC_SceneDescriptor.RespawnHeightBehaviour.Destroy;

	// Token: 0x04003FC1 RID: 16321
	private float DestroyHeightY = -100f;

	// Token: 0x04003FC2 RID: 16322
	private bool animatorAppliesRootMotionWhenOwned;

	// Token: 0x04003FC3 RID: 16323
	private AIRig _ai;

	// Token: 0x04003FC4 RID: 16324
	private Vector3 initialLocation = Vector3.zero;

	// Token: 0x04003FC5 RID: 16325
	private Quaternion initialRotation = Quaternion.identity;

	// Token: 0x04003FC6 RID: 16326
	private ViewSynchronization initialSync;
}
