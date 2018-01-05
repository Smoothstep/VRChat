using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BD5 RID: 3029
	public class BalloonHapticBump : MonoBehaviour
	{
		// Token: 0x06005DBE RID: 23998 RVA: 0x0020CD0C File Offset: 0x0020B10C
		private void OnCollisionEnter(Collision other)
		{
			Balloon componentInParent = other.collider.GetComponentInParent<Balloon>();
			if (componentInParent != null)
			{
				Hand componentInParent2 = this.physParent.GetComponentInParent<Hand>();
				if (componentInParent2 != null)
				{
					componentInParent2.controller.TriggerHapticPulse(500, EVRButtonId.k_EButton_Axis0);
				}
			}
		}

		// Token: 0x04004350 RID: 17232
		public GameObject physParent;
	}
}
