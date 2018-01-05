using System;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BB0 RID: 2992
	[RequireComponent(typeof(Interactable))]
	public class InteractableHoverEvents : MonoBehaviour
	{
		// Token: 0x06005CB4 RID: 23732 RVA: 0x0020614D File Offset: 0x0020454D
		private void OnHandHoverBegin()
		{
			this.onHandHoverBegin.Invoke();
		}

		// Token: 0x06005CB5 RID: 23733 RVA: 0x0020615A File Offset: 0x0020455A
		private void OnHandHoverEnd()
		{
			this.onHandHoverEnd.Invoke();
		}

		// Token: 0x06005CB6 RID: 23734 RVA: 0x00206167 File Offset: 0x00204567
		private void OnAttachedToHand(Hand hand)
		{
			this.onAttachedToHand.Invoke();
		}

		// Token: 0x06005CB7 RID: 23735 RVA: 0x00206174 File Offset: 0x00204574
		private void OnDetachedFromHand(Hand hand)
		{
			this.onDetachedFromHand.Invoke();
		}

		// Token: 0x0400423D RID: 16957
		public UnityEvent onHandHoverBegin;

		// Token: 0x0400423E RID: 16958
		public UnityEvent onHandHoverEnd;

		// Token: 0x0400423F RID: 16959
		public UnityEvent onAttachedToHand;

		// Token: 0x04004240 RID: 16960
		public UnityEvent onDetachedFromHand;
	}
}
