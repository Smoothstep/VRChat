using System;
using UnityEngine;

namespace UnityStandardAssets.SceneUtils
{
	// Token: 0x02000A28 RID: 2600
	public class PlaceTargetWithMouse : MonoBehaviour
	{
		// Token: 0x06004E69 RID: 20073 RVA: 0x001A4768 File Offset: 0x001A2B68
		private void Update()
		{
			if (!Input.GetMouseButtonDown(0))
			{
				return;
			}
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit;
			if (!Physics.Raycast(ray, out raycastHit))
			{
				return;
			}
			base.transform.position = raycastHit.point + raycastHit.normal * this.surfaceOffset;
			if (this.setTargetOn != null)
			{
				this.setTargetOn.SendMessage("SetTarget", base.transform);
			}
		}

		// Token: 0x04003688 RID: 13960
		public float surfaceOffset = 1.5f;

		// Token: 0x04003689 RID: 13961
		public GameObject setTargetOn;
	}
}
