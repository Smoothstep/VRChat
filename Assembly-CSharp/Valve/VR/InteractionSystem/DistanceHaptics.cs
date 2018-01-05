using System;
using System.Collections;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BA0 RID: 2976
	public class DistanceHaptics : MonoBehaviour
	{
		// Token: 0x06005C67 RID: 23655 RVA: 0x00204598 File Offset: 0x00202998
		private IEnumerator Start()
		{
			for (;;)
			{
				float distance = Vector3.Distance(this.firstTransform.position, this.secondTransform.position);
				SteamVR_TrackedObject trackedObject = base.GetComponentInParent<SteamVR_TrackedObject>();
				if (trackedObject)
				{
					float num = this.distanceIntensityCurve.Evaluate(distance);
					SteamVR_Controller.Input((int)trackedObject.index).TriggerHapticPulse((ushort)num, EVRButtonId.k_EButton_Axis0);
				}
				float nextPulse = this.pulseIntervalCurve.Evaluate(distance);
				yield return new WaitForSeconds(nextPulse);
			}
			yield break;
		}

		// Token: 0x040041F8 RID: 16888
		public Transform firstTransform;

		// Token: 0x040041F9 RID: 16889
		public Transform secondTransform;

		// Token: 0x040041FA RID: 16890
		public AnimationCurve distanceIntensityCurve = AnimationCurve.Linear(0f, 800f, 1f, 800f);

		// Token: 0x040041FB RID: 16891
		public AnimationCurve pulseIntervalCurve = AnimationCurve.Linear(0f, 0.01f, 1f, 0f);
	}
}
