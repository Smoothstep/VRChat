using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000B9D RID: 2973
	[RequireComponent(typeof(Interactable))]
	public class DestroyOnDetachedFromHand : MonoBehaviour
	{
		// Token: 0x06005C5F RID: 23647 RVA: 0x00204474 File Offset: 0x00202874
		private void OnDetachedFromHand(Hand hand)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
