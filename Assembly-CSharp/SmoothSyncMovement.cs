using System;
using Photon;
using UnityEngine;

// Token: 0x020007A3 RID: 1955
[RequireComponent(typeof(PhotonView))]
public class SmoothSyncMovement : Photon.MonoBehaviour, IPunObservable
{
	// Token: 0x06003F33 RID: 16179 RVA: 0x0013E5AC File Offset: 0x0013C9AC
	public void Awake()
	{
		bool flag = false;
		foreach (Component x in base.photonView.ObservedComponents)
		{
			if (x == this)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Debug.LogWarning(this + " is not observed by this object's photonView! OnPhotonSerializeView() in this class won't be used.");
		}
	}

	// Token: 0x06003F34 RID: 16180 RVA: 0x0013E634 File Offset: 0x0013CA34
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(base.transform.position);
			stream.SendNext(base.transform.rotation);
		}
		else
		{
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
		}
	}

	// Token: 0x06003F35 RID: 16181 RVA: 0x0013E6A0 File Offset: 0x0013CAA0
	public void Update()
	{
		if (!base.photonView.isMine)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.correctPlayerPos, Time.deltaTime * this.SmoothingDelay);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.correctPlayerRot, Time.deltaTime * this.SmoothingDelay);
		}
	}

	// Token: 0x0400279B RID: 10139
	public float SmoothingDelay = 5f;

	// Token: 0x0400279C RID: 10140
	private Vector3 correctPlayerPos = Vector3.zero;

	// Token: 0x0400279D RID: 10141
	private Quaternion correctPlayerRot = Quaternion.identity;
}
