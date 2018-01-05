using System;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BA8 RID: 2984
	[RequireComponent(typeof(Interactable))]
	public class HapticRack : MonoBehaviour
	{
		// Token: 0x06005C93 RID: 23699 RVA: 0x00205D05 File Offset: 0x00204105
		private void Awake()
		{
			if (this.linearMapping == null)
			{
				this.linearMapping = base.GetComponent<LinearMapping>();
			}
		}

		// Token: 0x06005C94 RID: 23700 RVA: 0x00205D24 File Offset: 0x00204124
		private void OnHandHoverBegin(Hand hand)
		{
			this.hand = hand;
		}

		// Token: 0x06005C95 RID: 23701 RVA: 0x00205D2D File Offset: 0x0020412D
		private void OnHandHoverEnd(Hand hand)
		{
			this.hand = null;
		}

		// Token: 0x06005C96 RID: 23702 RVA: 0x00205D38 File Offset: 0x00204138
		private void Update()
		{
			int num = Mathf.RoundToInt(this.linearMapping.value * (float)this.teethCount - 0.5f);
			if (num != this.previousToothIndex)
			{
				this.Pulse();
				this.previousToothIndex = num;
			}
		}

		// Token: 0x06005C97 RID: 23703 RVA: 0x00205D80 File Offset: 0x00204180
		private void Pulse()
		{
			if (this.hand && this.hand.controller != null && this.hand.GetStandardInteractionButton())
			{
				ushort durationMicroSec = (ushort)UnityEngine.Random.Range(this.minimumPulseDuration, this.maximumPulseDuration + 1);
				this.hand.controller.TriggerHapticPulse(durationMicroSec, EVRButtonId.k_EButton_Axis0);
				this.onPulse.Invoke();
			}
		}

		// Token: 0x04004229 RID: 16937
		[Tooltip("The linear mapping driving the haptic rack")]
		public LinearMapping linearMapping;

		// Token: 0x0400422A RID: 16938
		[Tooltip("The number of haptic pulses evenly distributed along the mapping")]
		public int teethCount = 128;

		// Token: 0x0400422B RID: 16939
		[Tooltip("Minimum duration of the haptic pulse")]
		public int minimumPulseDuration = 500;

		// Token: 0x0400422C RID: 16940
		[Tooltip("Maximum duration of the haptic pulse")]
		public int maximumPulseDuration = 900;

		// Token: 0x0400422D RID: 16941
		[Tooltip("This event is triggered every time a haptic pulse is made")]
		public UnityEvent onPulse;

		// Token: 0x0400422E RID: 16942
		private Hand hand;

		// Token: 0x0400422F RID: 16943
		private int previousToothIndex = -1;
	}
}
