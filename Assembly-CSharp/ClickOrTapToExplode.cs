using System;
using UnityEngine;

// Token: 0x0200056B RID: 1387
[RequireComponent(typeof(Collider))]
public class ClickOrTapToExplode : MonoBehaviour
{
	// Token: 0x06002F5E RID: 12126 RVA: 0x000E6178 File Offset: 0x000E4578
	private void OnMouseDown()
	{
		this.StartExplosion();
	}

	// Token: 0x06002F5F RID: 12127 RVA: 0x000E6180 File Offset: 0x000E4580
	private void Update()
	{
		foreach (Touch touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Began)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out raycastHit))
				{
					if (!(raycastHit.collider != base.GetComponent<Collider>()))
					{
						this.StartExplosion();
						return;
					}
				}
			}
		}
	}

	// Token: 0x06002F60 RID: 12128 RVA: 0x000E620F File Offset: 0x000E460F
	private void StartExplosion()
	{
		base.BroadcastMessage("Explode");
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
