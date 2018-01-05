using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BDB RID: 3035
	public class SoundBowClick : MonoBehaviour
	{
		// Token: 0x06005DE3 RID: 24035 RVA: 0x0020DBE2 File Offset: 0x0020BFE2
		private void Awake()
		{
			this.thisAudioSource = base.GetComponent<AudioSource>();
		}

		// Token: 0x06005DE4 RID: 24036 RVA: 0x0020DBF0 File Offset: 0x0020BFF0
		public void PlayBowTensionClicks(float normalizedTension)
		{
			float num = this.pitchTensionCurve.Evaluate(normalizedTension);
			this.thisAudioSource.pitch = (this.maxPitch - this.minPitch) * num + this.minPitch;
			this.thisAudioSource.PlayOneShot(this.bowClick);
		}

		// Token: 0x0400439C RID: 17308
		public AudioClip bowClick;

		// Token: 0x0400439D RID: 17309
		public AnimationCurve pitchTensionCurve;

		// Token: 0x0400439E RID: 17310
		public float minPitch;

		// Token: 0x0400439F RID: 17311
		public float maxPitch;

		// Token: 0x040043A0 RID: 17312
		private AudioSource thisAudioSource;
	}
}
