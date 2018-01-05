using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BAC RID: 2988
	public class Interactable : MonoBehaviour
	{
		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06005CA3 RID: 23715 RVA: 0x00205F08 File Offset: 0x00204308
		// (remove) Token: 0x06005CA4 RID: 23716 RVA: 0x00205F40 File Offset: 0x00204340
		[HideInInspector]
		public event Interactable.OnAttachedToHandDelegate onAttachedToHand;

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x06005CA5 RID: 23717 RVA: 0x00205F78 File Offset: 0x00204378
		// (remove) Token: 0x06005CA6 RID: 23718 RVA: 0x00205FB0 File Offset: 0x002043B0
		[HideInInspector]
		public event Interactable.OnDetachedFromHandDelegate onDetachedFromHand;

		// Token: 0x06005CA7 RID: 23719 RVA: 0x00205FE6 File Offset: 0x002043E6
		private void OnAttachedToHand(Hand hand)
		{
			if (this.onAttachedToHand != null)
			{
				this.onAttachedToHand(hand);
			}
		}

		// Token: 0x06005CA8 RID: 23720 RVA: 0x00205FFF File Offset: 0x002043FF
		private void OnDetachedFromHand(Hand hand)
		{
			if (this.onDetachedFromHand != null)
			{
				this.onDetachedFromHand(hand);
			}
		}

		// Token: 0x02000BAD RID: 2989
		// (Invoke) Token: 0x06005CAA RID: 23722
		public delegate void OnAttachedToHandDelegate(Hand hand);

		// Token: 0x02000BAE RID: 2990
		// (Invoke) Token: 0x06005CAE RID: 23726
		public delegate void OnDetachedFromHandDelegate(Hand hand);
	}
}
