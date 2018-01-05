using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BC2 RID: 3010
	public class SpawnAndAttachAfterControllerIsTracking : MonoBehaviour
	{
		// Token: 0x06005D11 RID: 23825 RVA: 0x00207E82 File Offset: 0x00206282
		private void Start()
		{
			this.hand = base.GetComponentInParent<Hand>();
		}

		// Token: 0x06005D12 RID: 23826 RVA: 0x00207E90 File Offset: 0x00206290
		private void Update()
		{
			if (this.itemPrefab != null && this.hand.controller != null && this.hand.controller.hasTracking)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.itemPrefab);
				gameObject.SetActive(true);
				this.hand.AttachObject(gameObject, Hand.AttachmentFlags.SnapOnAttach | Hand.AttachmentFlags.DetachOthers | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.ParentToHand, string.Empty);
				this.hand.controller.TriggerHapticPulse(800, EVRButtonId.k_EButton_Axis0);
				UnityEngine.Object.Destroy(base.gameObject);
				gameObject.transform.localScale = this.itemPrefab.transform.localScale;
			}
		}

		// Token: 0x040042AA RID: 17066
		private Hand hand;

		// Token: 0x040042AB RID: 17067
		public GameObject itemPrefab;
	}
}
