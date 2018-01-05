using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BC6 RID: 3014
	[RequireComponent(typeof(Interactable))]
	public class UIElement : MonoBehaviour
	{
		// Token: 0x06005D2C RID: 23852 RVA: 0x00208890 File Offset: 0x00206C90
		private void Awake()
		{
			Button component = base.GetComponent<Button>();
			if (component)
			{
				component.onClick.AddListener(new UnityAction(this.OnButtonClick));
			}
		}

		// Token: 0x06005D2D RID: 23853 RVA: 0x002088C6 File Offset: 0x00206CC6
		private void OnHandHoverBegin(Hand hand)
		{
			this.currentHand = hand;
			InputModule.instance.HoverBegin(base.gameObject);
			ControllerButtonHints.ShowButtonHint(hand, new EVRButtonId[]
			{
				EVRButtonId.k_EButton_Axis1
			});
		}

		// Token: 0x06005D2E RID: 23854 RVA: 0x002088F0 File Offset: 0x00206CF0
		private void OnHandHoverEnd(Hand hand)
		{
			InputModule.instance.HoverEnd(base.gameObject);
			ControllerButtonHints.HideButtonHint(hand, new EVRButtonId[]
			{
				EVRButtonId.k_EButton_Axis1
			});
			this.currentHand = null;
		}

		// Token: 0x06005D2F RID: 23855 RVA: 0x0020891A File Offset: 0x00206D1A
		private void HandHoverUpdate(Hand hand)
		{
			if (hand.GetStandardInteractionButtonDown())
			{
				InputModule.instance.Submit(base.gameObject);
				ControllerButtonHints.HideButtonHint(hand, new EVRButtonId[]
				{
					EVRButtonId.k_EButton_Axis1
				});
			}
		}

		// Token: 0x06005D30 RID: 23856 RVA: 0x00208948 File Offset: 0x00206D48
		private void OnButtonClick()
		{
			this.onHandClick.Invoke(this.currentHand);
		}

		// Token: 0x040042C7 RID: 17095
		public CustomEvents.UnityEventHand onHandClick;

		// Token: 0x040042C8 RID: 17096
		private Hand currentHand;
	}
}
