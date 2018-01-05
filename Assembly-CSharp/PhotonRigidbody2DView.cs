using System;
using UnityEngine;

// Token: 0x02000777 RID: 1911
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody2D))]
[AddComponentMenu("Photon Networking/Photon Rigidbody 2D View")]
public class PhotonRigidbody2DView : MonoBehaviour, IPunObservable
{
	// Token: 0x06003E6B RID: 15979 RVA: 0x0013A7FF File Offset: 0x00138BFF
	private void Awake()
	{
		this.m_Body = base.GetComponent<Rigidbody2D>();
	}

	// Token: 0x06003E6C RID: 15980 RVA: 0x0013A810 File Offset: 0x00138C10
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
				this.m_Body.velocity = (Vector2)stream.ReceiveNext();
			}
			if (this.m_SynchronizeAngularVelocity)
			{
				this.m_Body.angularVelocity = (float)stream.ReceiveNext();
			}
		}
	}

	// Token: 0x040026EA RID: 9962
	[SerializeField]
	private bool m_SynchronizeVelocity = true;

	// Token: 0x040026EB RID: 9963
	[SerializeField]
	private bool m_SynchronizeAngularVelocity = true;

	// Token: 0x040026EC RID: 9964
	private Rigidbody2D m_Body;
}
