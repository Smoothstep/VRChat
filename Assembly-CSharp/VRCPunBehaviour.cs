using System;
using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000A5C RID: 2652
public abstract class VRCPunBehaviour : Photon.MonoBehaviour
{
	// Token: 0x06005041 RID: 20545 RVA: 0x0019C3E8 File Offset: 0x0019A7E8
	public virtual void Awake()
	{
		if (this.photonView == null)
		{
			base.gameObject.AddComponent<PhotonView>();
		}
		this.photonView.ownershipTransfer = OwnershipOption.Takeover;
		this.photonView.ownershipTransferTime = (double)Time.time;
		this.photonView.synchronization = ViewSynchronization.Off;
	}

	// Token: 0x06005042 RID: 20546 RVA: 0x0019C43C File Offset: 0x0019A83C
	public virtual IEnumerator Start()
	{
		this.photonView.synchronization = ViewSynchronization.Unreliable;
		yield return null;
		yield break;
	}

	// Token: 0x06005043 RID: 20547 RVA: 0x0019C457 File Offset: 0x0019A857
	protected virtual void OnNetworkReady()
	{
		if (this.photonView != null)
		{
			this.photonView.ownershipTransfer = (base.gameObject.IsPlayer() ? OwnershipOption.Fixed : OwnershipOption.Takeover);
		}
	}

	// Token: 0x17000BE4 RID: 3044
	// (get) Token: 0x06005044 RID: 20548 RVA: 0x0019C48C File Offset: 0x0019A88C
	public new PhotonView photonView
	{
		get
		{
			if (base.photonView != null)
			{
				return base.photonView;
			}
			return base.GetComponentInParent<PhotonView>();
		}
	}

	// Token: 0x17000BE5 RID: 3045
	// (get) Token: 0x06005045 RID: 20549 RVA: 0x0019C4AC File Offset: 0x0019A8AC
	public VRC_EventHandler EventHandler
	{
		get
		{
			if (this._handler == null)
			{
				this._handler = base.gameObject.GetComponent<VRC_EventHandler>();
				if (this._handler == null)
				{
					this._handler = base.GetComponentInParent<VRC_EventHandler>();
				}
			}
			return this._handler;
		}
	}

	// Token: 0x06005046 RID: 20550 RVA: 0x0019C4FE File Offset: 0x0019A8FE
	protected void ObserveThis()
	{
		this.Observe(this);
	}

	// Token: 0x06005047 RID: 20551 RVA: 0x0019C508 File Offset: 0x0019A908
	protected void Observe(Component c)
	{
		if (this.photonView.ObservedComponents == null)
		{
			this.photonView.ObservedComponents = new List<Component>();
		}
		if (c != null && !this.photonView.ObservedComponents.Contains(c))
		{
			this.photonView.ObservedComponents.Add(c);
			this.photonView.ObservedComponents.RemoveAll((Component v) => v == null);
			this.photonView.ObservedComponents.Sort((Component a, Component b) => (a.name + a.GetType().Name).CompareTo(b.name + b.GetType().Name));
		}
	}

	// Token: 0x17000BE6 RID: 3046
	// (get) Token: 0x06005048 RID: 20552 RVA: 0x0019C5C3 File Offset: 0x0019A9C3
	public bool isMine
	{
		get
		{
			return this.photonView != null && this.photonView.isMine;
		}
	}

	// Token: 0x17000BE7 RID: 3047
	// (get) Token: 0x06005049 RID: 20553 RVA: 0x0019C5E4 File Offset: 0x0019A9E4
	public bool hasOwner
	{
		get
		{
			return this.photonView != null && this.photonView.ownerId > 0 && this.photonView.owner != null;
		}
	}

	// Token: 0x17000BE8 RID: 3048
	// (get) Token: 0x0600504A RID: 20554 RVA: 0x0019C61C File Offset: 0x0019AA1C
	public int OwnerId
	{
		get
		{
			return (!(this.photonView != null)) ? 0 : this.photonView.ownerId;
		}
	}

	// Token: 0x17000BE9 RID: 3049
	// (get) Token: 0x0600504B RID: 20555 RVA: 0x0019C640 File Offset: 0x0019AA40
	public string OwnerUserId
	{
		get
		{
			if (this.photonView == null || this.photonView.owner == null)
			{
				return null;
			}
			return this.photonView.owner.ID.ToString();
		}
	}

	// Token: 0x17000BEA RID: 3050
	// (get) Token: 0x0600504C RID: 20556 RVA: 0x0019C690 File Offset: 0x0019AA90
	public VRC.Player Owner
	{
		get
		{
			if (!this.hasOwner)
			{
				return VRC.Network.MasterPlayer;
			}
			if (this.ownerCache == null || this.ownerCache.vrcPlayer == null || VRC.Network.GetOwnerId(this.ownerCache.gameObject) != this.OwnerId)
			{
				this.ownerCache = PlayerManager.GetPlayer(this.photonView.owner);
			}
			if (this.ownerCache == null)
			{
				return VRC.Network.MasterPlayer;
			}
			return this.ownerCache;
		}
	}

	// Token: 0x0600504D RID: 20557 RVA: 0x0019C73A File Offset: 0x0019AB3A
	public void RequestOwnership()
	{
		if (!this.isMine && Time.time - this._lastRequestTime > 1f)
		{
			this._lastRequestTime = Time.time;
			this.photonView.RequestOwnership();
		}
	}

	// Token: 0x17000BEB RID: 3051
	// (get) Token: 0x0600504E RID: 20558 RVA: 0x0019C773 File Offset: 0x0019AB73
	public double OwnershipTransferTime
	{
		get
		{
			return (!(this.photonView == null)) ? this.photonView.ownershipTransferTime : 0.0;
		}
	}

	// Token: 0x04003914 RID: 14612
	public int ReservedID;

	// Token: 0x04003915 RID: 14613
	private VRC_EventHandler _handler;

	// Token: 0x04003916 RID: 14614
	private VRC.Player ownerCache;

	// Token: 0x04003917 RID: 14615
	private float _lastRequestTime;
}
