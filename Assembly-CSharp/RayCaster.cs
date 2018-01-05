using System;
using UnityEngine;

// Token: 0x02000AD9 RID: 2777
public class RayCaster
{
	// Token: 0x17000C3E RID: 3134
	// (get) Token: 0x06005456 RID: 21590 RVA: 0x001D1FD6 File Offset: 0x001D03D6
	public static RayCaster Instance
	{
		get
		{
			if (RayCaster.instance == null)
			{
				RayCaster.instance = new RayCaster();
			}
			return RayCaster.instance;
		}
	}

	// Token: 0x06005457 RID: 21591 RVA: 0x001D1FF4 File Offset: 0x001D03F4
	public void Cast(Transform rayOrigin, float rayDistance, RayCaster.RayCollisionCallBack hitCallback, RayCaster.RayNoCollisionCallBack noHitCallBack, int layerMask = 256)
	{
		Debug.DrawRay(rayOrigin.position, rayOrigin.forward * 10f, Color.red);
		if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out this._hit, rayDistance, layerMask))
		{
			hitCallback(this._hit);
		}
		else if (noHitCallBack != null)
		{
			noHitCallBack();
		}
	}

	// Token: 0x04003B79 RID: 15225
	private static RayCaster instance;

	// Token: 0x04003B7A RID: 15226
	private RaycastHit _hit;

	// Token: 0x02000ADA RID: 2778
	// (Invoke) Token: 0x06005459 RID: 21593
	public delegate void RayCollisionCallBack(RaycastHit hit);

	// Token: 0x02000ADB RID: 2779
	// (Invoke) Token: 0x0600545D RID: 21597
	public delegate void RayNoCollisionCallBack();
}
