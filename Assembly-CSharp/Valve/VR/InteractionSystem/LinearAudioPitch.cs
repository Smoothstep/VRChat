using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BB7 RID: 2999
	public class LinearAudioPitch : MonoBehaviour
	{
		// Token: 0x06005CD2 RID: 23762 RVA: 0x00206A00 File Offset: 0x00204E00
		private void Awake()
		{
			if (this.audioSource == null)
			{
				this.audioSource = base.GetComponent<AudioSource>();
			}
			if (this.linearMapping == null)
			{
				this.linearMapping = base.GetComponent<LinearMapping>();
			}
		}

		// Token: 0x06005CD3 RID: 23763 RVA: 0x00206A3C File Offset: 0x00204E3C
		private void Update()
		{
			if (this.applyContinuously)
			{
				this.Apply();
			}
		}

		// Token: 0x06005CD4 RID: 23764 RVA: 0x00206A50 File Offset: 0x00204E50
		private void Apply()
		{
			float t = this.pitchCurve.Evaluate(this.linearMapping.value);
			this.audioSource.pitch = Mathf.Lerp(this.minPitch, this.maxPitch, t);
		}

		// Token: 0x04004265 RID: 16997
		public LinearMapping linearMapping;

		// Token: 0x04004266 RID: 16998
		public AnimationCurve pitchCurve;

		// Token: 0x04004267 RID: 16999
		public float minPitch;

		// Token: 0x04004268 RID: 17000
		public float maxPitch;

		// Token: 0x04004269 RID: 17001
		public bool applyContinuously = true;

		// Token: 0x0400426A RID: 17002
		private AudioSource audioSource;
	}
}
