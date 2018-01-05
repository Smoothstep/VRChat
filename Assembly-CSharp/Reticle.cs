using System;
using UnityEngine;

// Token: 0x02000AE6 RID: 2790
public class Reticle : MonoBehaviour
{
	// Token: 0x0600548D RID: 21645 RVA: 0x001D2DEF File Offset: 0x001D11EF
	private void Start()
	{
		this.originalScale = base.transform.localScale;
	}

	// Token: 0x0600548E RID: 21646 RVA: 0x001D2E04 File Offset: 0x001D1204
	private void Update()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(this.CameraFacing.transform.position, this.CameraFacing.transform.rotation * Vector3.forward), out raycastHit))
		{
			this.distance = raycastHit.distance - 0.1f;
		}
		base.transform.position = this.CameraFacing.transform.position + this.CameraFacing.transform.rotation * Vector3.forward * this.distance;
		base.transform.LookAt(this.CameraFacing.transform.position);
		base.transform.Rotate(0f, 180f, 0f);
		base.transform.localScale = this.originalScale * this.distance;
	}

	// Token: 0x04003BA7 RID: 15271
	public Camera CameraFacing;

	// Token: 0x04003BA8 RID: 15272
	private Vector3 originalScale;

	// Token: 0x04003BA9 RID: 15273
	private float distance;
}
