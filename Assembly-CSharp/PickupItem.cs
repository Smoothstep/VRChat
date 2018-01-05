using System;
using System.Collections.Generic;
using Photon;
using UnityEngine;

// Token: 0x02000795 RID: 1941
[RequireComponent(typeof(PhotonView))]
public class PickupItem : Photon.MonoBehaviour, IPunObservable
{
    // Token: 0x17000A03 RID: 2563
    // (get) Token: 0x06003EEA RID: 16106 RVA: 0x0013D3C6 File Offset: 0x0013B7C6
    public static List<PickupItem> All = new List<PickupItem>();
    public PickupItem()
    {
        All.Add(this);
    }

	public int ViewID
	{
		get
		{
			return base.photonView.viewID;
		}
	}

	// Token: 0x06003EEB RID: 16107 RVA: 0x0013D3D4 File Offset: 0x0013B7D4
	public void OnTriggerEnter(Collider other)
	{
		PhotonView component = other.GetComponent<PhotonView>();
		if (this.PickupOnTrigger && component != null && component.isMine)
		{
			this.Pickup();
		}
	}

	// Token: 0x06003EEC RID: 16108 RVA: 0x0013D410 File Offset: 0x0013B810
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting && this.SecondsBeforeRespawn <= 0f)
		{
			stream.SendNext(base.gameObject.transform.position);
		}
		else
		{
			Vector3 position = (Vector3)stream.ReceiveNext();
			base.gameObject.transform.position = position;
		}
	}

	// Token: 0x06003EED RID: 16109 RVA: 0x0013D475 File Offset: 0x0013B875
	public void Pickup()
	{
		if (this.SentPickup)
		{
			return;
		}
		this.SentPickup = true;
		base.photonView.RPC("PunPickup", PhotonTargets.AllViaServer, new object[0]);
	}

	// Token: 0x06003EEE RID: 16110 RVA: 0x0013D4A1 File Offset: 0x0013B8A1
	public void Drop()
	{
		if (this.PickupIsMine)
		{
			base.photonView.RPC("PunRespawn", PhotonTargets.AllViaServer, new object[0]);
		}
	}

	// Token: 0x06003EEF RID: 16111 RVA: 0x0013D4C5 File Offset: 0x0013B8C5
	public void Drop(Vector3 newPosition)
	{
		if (this.PickupIsMine)
		{
			base.photonView.RPC("PunRespawn", PhotonTargets.AllViaServer, new object[]
			{
				newPosition
			});
		}
	}

	// Token: 0x06003EF0 RID: 16112 RVA: 0x0013D4F4 File Offset: 0x0013B8F4
	[PunRPC]
	public void PunPickup(PhotonMessageInfo msgInfo)
	{
		if (msgInfo.sender.IsLocal)
		{
			this.SentPickup = false;
		}
		if (!base.gameObject.GetActive())
		{
			Debug.Log(string.Concat(new object[]
			{
				"Ignored PU RPC, cause item is inactive. ",
				base.gameObject,
				" SecondsBeforeRespawn: ",
				this.SecondsBeforeRespawn,
				" TimeOfRespawn: ",
				this.TimeOfRespawn,
				" respawn in future: ",
				this.TimeOfRespawn > PhotonNetwork.time
			}));
			return;
		}
		this.PickupIsMine = msgInfo.sender.IsLocal;
		if (this.OnPickedUpCall != null)
		{
			this.OnPickedUpCall.SendMessage("OnPickedUp", this);
		}
		if (this.SecondsBeforeRespawn <= 0f)
		{
			this.PickedUp(0f);
		}
		else
		{
			double num = PhotonNetwork.time - msgInfo.timestamp;
			double num2 = (double)this.SecondsBeforeRespawn - num;
			if (num2 > 0.0)
			{
				this.PickedUp((float)num2);
			}
		}
	}

	// Token: 0x06003EF1 RID: 16113 RVA: 0x0013D618 File Offset: 0x0013BA18
	internal void PickedUp(float timeUntilRespawn)
	{
		base.gameObject.SetActive(false);
		PickupItem.DisabledPickupItems.Add(this);
		this.TimeOfRespawn = 0.0;
		if (timeUntilRespawn > 0f)
		{
			this.TimeOfRespawn = PhotonNetwork.time + (double)timeUntilRespawn;
			base.Invoke("PunRespawn", timeUntilRespawn);
		}
	}

	// Token: 0x06003EF2 RID: 16114 RVA: 0x0013D671 File Offset: 0x0013BA71
	[PunRPC]
	internal void PunRespawn(Vector3 pos)
	{
		Debug.Log("PunRespawn with Position.");
		this.PunRespawn();
		base.gameObject.transform.position = pos;
	}

	// Token: 0x06003EF3 RID: 16115 RVA: 0x0013D694 File Offset: 0x0013BA94
	[PunRPC]
	internal void PunRespawn()
	{
		PickupItem.DisabledPickupItems.Remove(this);
		this.TimeOfRespawn = 0.0;
		this.PickupIsMine = false;
		if (base.gameObject != null)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x0400277D RID: 10109
	public float SecondsBeforeRespawn = 2f;

	// Token: 0x0400277E RID: 10110
	public bool PickupOnTrigger;

	// Token: 0x0400277F RID: 10111
	public bool PickupIsMine;

	// Token: 0x04002780 RID: 10112
	public UnityEngine.MonoBehaviour OnPickedUpCall;

	// Token: 0x04002781 RID: 10113
	public bool SentPickup;

	// Token: 0x04002782 RID: 10114
	public double TimeOfRespawn;

	// Token: 0x04002783 RID: 10115
	public static HashSet<PickupItem> DisabledPickupItems = new HashSet<PickupItem>();
}
