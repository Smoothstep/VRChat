using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BDD RID: 3037
	[RequireComponent(typeof(Interactable))]
	public class InteractableExample : MonoBehaviour
	{
		// Token: 0x06005DEC RID: 24044 RVA: 0x0020E0FE File Offset: 0x0020C4FE
		private void Awake()
		{
			this.textMesh = base.GetComponentInChildren<TextMesh>();
			this.textMesh.text = "No Hand Hovering";
		}

		// Token: 0x06005DED RID: 24045 RVA: 0x0020E11C File Offset: 0x0020C51C
		private void OnHandHoverBegin(Hand hand)
		{
			this.textMesh.text = "Hovering hand: " + hand.name;
		}

		// Token: 0x06005DEE RID: 24046 RVA: 0x0020E139 File Offset: 0x0020C539
		private void OnHandHoverEnd(Hand hand)
		{
			this.textMesh.text = "No Hand Hovering";
		}

		// Token: 0x06005DEF RID: 24047 RVA: 0x0020E14C File Offset: 0x0020C54C
		private void HandHoverUpdate(Hand hand)
		{
			if (hand.GetStandardInteractionButtonDown() || (hand.controller != null && hand.controller.GetPressDown(EVRButtonId.k_EButton_Grip)))
			{
				if (hand.currentAttachedObject != base.gameObject)
				{
					this.oldPosition = base.transform.position;
					this.oldRotation = base.transform.rotation;
					hand.HoverLock(base.GetComponent<Interactable>());
					hand.AttachObject(base.gameObject, this.attachmentFlags, string.Empty);
				}
				else
				{
					hand.DetachObject(base.gameObject, true);
					hand.HoverUnlock(base.GetComponent<Interactable>());
					base.transform.position = this.oldPosition;
					base.transform.rotation = this.oldRotation;
				}
			}
		}

		// Token: 0x06005DF0 RID: 24048 RVA: 0x0020E21B File Offset: 0x0020C61B
		private void OnAttachedToHand(Hand hand)
		{
			this.textMesh.text = "Attached to hand: " + hand.name;
			this.attachTime = Time.time;
		}

		// Token: 0x06005DF1 RID: 24049 RVA: 0x0020E243 File Offset: 0x0020C643
		private void OnDetachedFromHand(Hand hand)
		{
			this.textMesh.text = "Detached from hand: " + hand.name;
		}

		// Token: 0x06005DF2 RID: 24050 RVA: 0x0020E260 File Offset: 0x0020C660
		private void HandAttachedUpdate(Hand hand)
		{
			this.textMesh.text = "Attached to hand: " + hand.name + "\nAttached time: " + (Time.time - this.attachTime).ToString("F2");
		}

		// Token: 0x06005DF3 RID: 24051 RVA: 0x0020E2A6 File Offset: 0x0020C6A6
		private void OnHandFocusAcquired(Hand hand)
		{
		}

		// Token: 0x06005DF4 RID: 24052 RVA: 0x0020E2A8 File Offset: 0x0020C6A8
		private void OnHandFocusLost(Hand hand)
		{
		}

		// Token: 0x040043A3 RID: 17315
		private TextMesh textMesh;

		// Token: 0x040043A4 RID: 17316
		private Vector3 oldPosition;

		// Token: 0x040043A5 RID: 17317
		private Quaternion oldRotation;

		// Token: 0x040043A6 RID: 17318
		private float attachTime;

		// Token: 0x040043A7 RID: 17319
		private Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.ParentToHand;
	}
}
