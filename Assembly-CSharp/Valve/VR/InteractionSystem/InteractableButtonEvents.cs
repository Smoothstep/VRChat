using System;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BAF RID: 2991
	[RequireComponent(typeof(Interactable))]
	public class InteractableButtonEvents : MonoBehaviour
	{
		// Token: 0x06005CB2 RID: 23730 RVA: 0x00206020 File Offset: 0x00204420
		private void Update()
		{
			for (int i = 0; i < Player.instance.handCount; i++)
			{
				Hand hand = Player.instance.GetHand(i);
				if (hand.controller != null)
				{
					if (hand.controller.GetPressDown(EVRButtonId.k_EButton_Axis1))
					{
						this.onTriggerDown.Invoke();
					}
					if (hand.controller.GetPressUp(EVRButtonId.k_EButton_Axis1))
					{
						this.onTriggerUp.Invoke();
					}
					if (hand.controller.GetPressDown(EVRButtonId.k_EButton_Grip))
					{
						this.onGripDown.Invoke();
					}
					if (hand.controller.GetPressUp(EVRButtonId.k_EButton_Grip))
					{
						this.onGripUp.Invoke();
					}
					if (hand.controller.GetPressDown(EVRButtonId.k_EButton_Axis0))
					{
						this.onTouchpadDown.Invoke();
					}
					if (hand.controller.GetPressUp(EVRButtonId.k_EButton_Axis0))
					{
						this.onTouchpadUp.Invoke();
					}
					if (hand.controller.GetTouchDown(EVRButtonId.k_EButton_Axis0))
					{
						this.onTouchpadTouch.Invoke();
					}
					if (hand.controller.GetTouchUp(EVRButtonId.k_EButton_Axis0))
					{
						this.onTouchpadRelease.Invoke();
					}
				}
			}
		}

		// Token: 0x04004235 RID: 16949
		public UnityEvent onTriggerDown;

		// Token: 0x04004236 RID: 16950
		public UnityEvent onTriggerUp;

		// Token: 0x04004237 RID: 16951
		public UnityEvent onGripDown;

		// Token: 0x04004238 RID: 16952
		public UnityEvent onGripUp;

		// Token: 0x04004239 RID: 16953
		public UnityEvent onTouchpadDown;

		// Token: 0x0400423A RID: 16954
		public UnityEvent onTouchpadUp;

		// Token: 0x0400423B RID: 16955
		public UnityEvent onTouchpadTouch;

		// Token: 0x0400423C RID: 16956
		public UnityEvent onTouchpadRelease;
	}
}
