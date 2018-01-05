using System;
using UnityEngine;

// Token: 0x0200078B RID: 1931
[RequireComponent(typeof(PhotonView))]
public class ManualPhotonViewAllocator : MonoBehaviour
{
	// Token: 0x06003EBF RID: 16063 RVA: 0x0013C458 File Offset: 0x0013A858
	public void AllocateManualPhotonView()
	{
		PhotonView photonView = base.gameObject.GetPhotonView();
		if (photonView == null)
		{
			Debug.LogError("Can't do manual instantiation without PhotonView component.");
			return;
		}
		int num = PhotonNetwork.AllocateViewID();
		photonView.RPC("InstantiateRpc", PhotonTargets.AllBuffered, new object[]
		{
			num
		});
	}

	// Token: 0x06003EC0 RID: 16064 RVA: 0x0013C4AC File Offset: 0x0013A8AC
	[PunRPC]
	public void InstantiateRpc(int viewID)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Prefab, InputToEvent.inputHitPos + new Vector3(0f, 5f, 0f), Quaternion.identity);
		gameObject.GetPhotonView().viewID = viewID;
		OnClickDestroy component = gameObject.GetComponent<OnClickDestroy>();
		component.DestroyByRpc = true;
	}

	// Token: 0x0400275E RID: 10078
	public GameObject Prefab;
}
