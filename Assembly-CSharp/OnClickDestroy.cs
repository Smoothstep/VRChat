using System;
using System.Collections;
using Photon;
using UnityEngine;

// Token: 0x0200078E RID: 1934
[RequireComponent(typeof(PhotonView))]
public class OnClickDestroy : Photon.MonoBehaviour
{
	// Token: 0x06003ECD RID: 16077 RVA: 0x0013CC8B File Offset: 0x0013B08B
	public void OnClick()
	{
		if (!this.DestroyByRpc)
		{
			PhotonNetwork.Destroy(base.gameObject);
		}
		else
		{
			base.photonView.RPC("DestroyRpc", PhotonTargets.AllBuffered, new object[0]);
		}
	}

	// Token: 0x06003ECE RID: 16078 RVA: 0x0013CCC0 File Offset: 0x0013B0C0
	[PunRPC]
	public IEnumerator DestroyRpc()
	{
		UnityEngine.Object.Destroy(base.gameObject);
		yield return 0;
		PhotonNetwork.UnAllocateViewID(base.photonView.viewID);
		yield break;
	}

	// Token: 0x0400276D RID: 10093
	public bool DestroyByRpc;
}
