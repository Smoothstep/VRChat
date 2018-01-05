using System;
using UnityEngine;

// Token: 0x02000778 RID: 1912
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
[AddComponentMenu("Photon Networking/Photon Rigidbody View")]
public class PhotonRigidbodyView : MonoBehaviour, IPunObservable
{
	// Token: 0x06003E6E RID: 15982 RVA: 0x0013A8C7 File Offset: 0x00138CC7
	private void Awake()
	{
		this.m_Body = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06003E6F RID: 15983 RVA: 0x0013A8D8 File Offset: 0x00138CD8
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			if (this.m_SynchronizeVelocity)
			{
				stream.SendNext(this.m_Body.velocity);
			}
			if (this.m_SynchronizeAngularVelocity)
			{
				stream.SendNext(this.m_Body.angularVelocity);
			}
		}
		else
		{
			if (this.m_SynchronizeVelocity)
			{
				this.m_Body.velocity = (Vector3)stream.ReceiveNext();
			}
			if (this.m_SynchronizeAngularVelocity)
			{
				this.m_Body.angularVelocity = (Vector3)stream.ReceiveNext();
			}
		}
	}

	// Token: 0x040026ED RID: 9965
	[SerializeField]
	private bool m_SynchronizeVelocity = true;

	// Token: 0x040026EE RID: 9966
	[SerializeField]
	private bool m_SynchronizeAngularVelocity = true;

	// Token: 0x040026EF RID: 9967
	private Rigidbody m_Body;
}
