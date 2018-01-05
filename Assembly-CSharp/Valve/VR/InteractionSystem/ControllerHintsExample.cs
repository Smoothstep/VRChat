using System;
using System.Collections;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BDC RID: 3036
	public class ControllerHintsExample : MonoBehaviour
	{
		// Token: 0x06005DE6 RID: 24038 RVA: 0x0020DC44 File Offset: 0x0020C044
		public void ShowButtonHints(Hand hand)
		{
			if (this.buttonHintCoroutine != null)
			{
				base.StopCoroutine(this.buttonHintCoroutine);
			}
			this.buttonHintCoroutine = base.StartCoroutine(this.TestButtonHints(hand));
		}

		// Token: 0x06005DE7 RID: 24039 RVA: 0x0020DC70 File Offset: 0x0020C070
		public void ShowTextHints(Hand hand)
		{
			if (this.textHintCoroutine != null)
			{
				base.StopCoroutine(this.textHintCoroutine);
			}
			this.textHintCoroutine = base.StartCoroutine(this.TestTextHints(hand));
		}

		// Token: 0x06005DE8 RID: 24040 RVA: 0x0020DC9C File Offset: 0x0020C09C
		public void DisableHints()
		{
			if (this.buttonHintCoroutine != null)
			{
				base.StopCoroutine(this.buttonHintCoroutine);
				this.buttonHintCoroutine = null;
			}
			if (this.textHintCoroutine != null)
			{
				base.StopCoroutine(this.textHintCoroutine);
				this.textHintCoroutine = null;
			}
			foreach (Hand hand in Player.instance.hands)
			{
				ControllerButtonHints.HideAllButtonHints(hand);
				ControllerButtonHints.HideAllTextHints(hand);
			}
		}

		// Token: 0x06005DE9 RID: 24041 RVA: 0x0020DD14 File Offset: 0x0020C114
		private IEnumerator TestButtonHints(Hand hand)
		{
			ControllerButtonHints.HideAllButtonHints(hand);
			for (;;)
			{
				ControllerButtonHints.ShowButtonHint(hand, new EVRButtonId[]
				{
					EVRButtonId.k_EButton_ApplicationMenu
				});
				yield return new WaitForSeconds(1f);
				ControllerButtonHints.ShowButtonHint(hand, new EVRButtonId[1]);
				yield return new WaitForSeconds(1f);
				ControllerButtonHints.ShowButtonHint(hand, new EVRButtonId[]
				{
					EVRButtonId.k_EButton_Grip
				});
				yield return new WaitForSeconds(1f);
				ControllerButtonHints.ShowButtonHint(hand, new EVRButtonId[]
				{
					EVRButtonId.k_EButton_Axis1
				});
				yield return new WaitForSeconds(1f);
				ControllerButtonHints.ShowButtonHint(hand, new EVRButtonId[]
				{
					EVRButtonId.k_EButton_Axis0
				});
				yield return new WaitForSeconds(1f);
				ControllerButtonHints.HideAllButtonHints(hand);
				yield return new WaitForSeconds(1f);
			}
			yield break;
		}

		// Token: 0x06005DEA RID: 24042 RVA: 0x0020DD30 File Offset: 0x0020C130
		private IEnumerator TestTextHints(Hand hand)
		{
			ControllerButtonHints.HideAllTextHints(hand);
			for (;;)
			{
				ControllerButtonHints.ShowTextHint(hand, EVRButtonId.k_EButton_ApplicationMenu, "Application", true);
				yield return new WaitForSeconds(3f);
				ControllerButtonHints.ShowTextHint(hand, EVRButtonId.k_EButton_System, "System", true);
				yield return new WaitForSeconds(3f);
				ControllerButtonHints.ShowTextHint(hand, EVRButtonId.k_EButton_Grip, "Grip", true);
				yield return new WaitForSeconds(3f);
				ControllerButtonHints.ShowTextHint(hand, EVRButtonId.k_EButton_Axis1, "Trigger", true);
				yield return new WaitForSeconds(3f);
				ControllerButtonHints.ShowTextHint(hand, EVRButtonId.k_EButton_Axis0, "Touchpad", true);
				yield return new WaitForSeconds(3f);
				ControllerButtonHints.HideAllTextHints(hand);
				yield return new WaitForSeconds(3f);
			}
			yield break;
		}

		// Token: 0x040043A1 RID: 17313
		private Coroutine buttonHintCoroutine;

		// Token: 0x040043A2 RID: 17314
		private Coroutine textHintCoroutine;
	}
}
