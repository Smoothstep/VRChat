using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BC3 RID: 3011
	public class SpawnAndAttachToHand : MonoBehaviour
	{
		// Token: 0x06005D14 RID: 23828 RVA: 0x00207F40 File Offset: 0x00206340
		public void SpawnAndAttach(Hand passedInhand)
		{
			Hand hand = passedInhand;
			if (passedInhand == null)
			{
				hand = this.hand;
			}
			if (hand == null)
			{
				return;
			}
			GameObject objectToAttach = UnityEngine.Object.Instantiate<GameObject>(this.prefab);
			hand.AttachObject(objectToAttach, Hand.AttachmentFlags.SnapOnAttach | Hand.AttachmentFlags.DetachOthers | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.ParentToHand, string.Empty);
		}

		// Token: 0x040042AC RID: 17068
		public Hand hand;

		// Token: 0x040042AD RID: 17069
		public GameObject prefab;
	}
}
