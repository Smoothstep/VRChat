using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BA9 RID: 2985
	public class HideOnHandFocusLost : MonoBehaviour
	{
		// Token: 0x06005C99 RID: 23705 RVA: 0x00205DF8 File Offset: 0x002041F8
		private void OnHandFocusLost(Hand hand)
		{
			base.gameObject.SetActive(false);
		}
	}
}
