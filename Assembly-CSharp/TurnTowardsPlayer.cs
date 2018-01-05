using System;
using UnityEngine;

// Token: 0x02000B00 RID: 2816
public class TurnTowardsPlayer : MonoBehaviour
{
	// Token: 0x06005522 RID: 21794 RVA: 0x001D5738 File Offset: 0x001D3B38
	private void Update()
	{
		if (this.player == null)
		{
			Vector3 worldCameraPos = VRCVrCamera.GetInstance().GetWorldCameraPos();
			if (this.isBackwards)
			{
				base.transform.LookAt(2f * base.transform.position - worldCameraPos);
			}
			else
			{
				base.transform.LookAt(worldCameraPos);
			}
		}
		else if (this.isBackwards)
		{
			base.transform.LookAt(2f * base.transform.position - this.player.transform.position);
		}
		else
		{
			base.transform.LookAt(this.player.transform.position);
		}
	}

	// Token: 0x04003C19 RID: 15385
	public GameObject player;

	// Token: 0x04003C1A RID: 15386
	public bool isBackwards;
}
