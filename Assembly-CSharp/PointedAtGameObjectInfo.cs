using System;
using UnityEngine;

// Token: 0x02000798 RID: 1944
[RequireComponent(typeof(InputToEvent))]
public class PointedAtGameObjectInfo : MonoBehaviour
{
	// Token: 0x06003F03 RID: 16131 RVA: 0x0013DC30 File Offset: 0x0013C030
	private void OnGUI()
	{
		if (InputToEvent.goPointedAt != null)
		{
			PhotonView photonView = InputToEvent.goPointedAt.GetPhotonView();
			if (photonView != null)
			{
				GUI.Label(new Rect(Input.mousePosition.x + 5f, (float)Screen.height - Input.mousePosition.y - 15f, 300f, 30f), string.Format("ViewID {0} {1}{2}", photonView.viewID, (!photonView.isSceneView) ? string.Empty : "scene ", (!photonView.isMine) ? ("owner: " + photonView.ownerId) : "mine"));
			}
		}
	}
}
