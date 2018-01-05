using System;
using Photon;
using UnityEngine;

// Token: 0x02000796 RID: 1942
[RequireComponent(typeof(PhotonView))]
public class PickupItemSimple : Photon.MonoBehaviour
{
	// Token: 0x06003EF6 RID: 16118 RVA: 0x0013D700 File Offset: 0x0013BB00
	public void OnTriggerEnter(Collider other)
	{
		PhotonView component = other.GetComponent<PhotonView>();
		if (this.PickupOnCollide && component != null && component.isMine)
		{
			this.Pickup();
		}
	}

	// Token: 0x06003EF7 RID: 16119 RVA: 0x0013D73C File Offset: 0x0013BB3C
	public void Pickup()
	{
		if (this.SentPickup)
		{
			return;
		}
		this.SentPickup = true;
		base.photonView.RPC("PunPickupSimple", PhotonTargets.AllViaServer, new object[0]);
	}

	// Token: 0x06003EF8 RID: 16120 RVA: 0x0013D768 File Offset: 0x0013BB68
	[PunRPC]
	public void PunPickupSimple(PhotonMessageInfo msgInfo)
	{
		if (!this.SentPickup || !msgInfo.sender.IsLocal || base.gameObject.GetActive())
		{
		}
		this.SentPickup = false;
		if (!base.gameObject.GetActive())
		{
			Debug.Log("Ignored PU RPC, cause item is inactive. " + base.gameObject);
			return;
		}
		double num = PhotonNetwork.time - msgInfo.timestamp;
		float num2 = this.SecondsBeforeRespawn - (float)num;
		if (num2 > 0f)
		{
			base.gameObject.SetActive(false);
			base.Invoke("RespawnAfter", num2);
		}
	}

	// Token: 0x06003EF9 RID: 16121 RVA: 0x0013D80E File Offset: 0x0013BC0E
	public void RespawnAfter()
	{
		if (base.gameObject != null)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x04002784 RID: 10116
	public float SecondsBeforeRespawn = 2f;

	// Token: 0x04002785 RID: 10117
	public bool PickupOnCollide;

	// Token: 0x04002786 RID: 10118
	public bool SentPickup;
}
