using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BC1 RID: 3009
	public class SoundPlayOneshot : MonoBehaviour
	{
		// Token: 0x06005D0C RID: 23820 RVA: 0x00207D87 File Offset: 0x00206187
		private void Awake()
		{
			this.thisAudioSource = base.GetComponent<AudioSource>();
			if (this.playOnAwake)
			{
				this.Play();
			}
		}

		// Token: 0x06005D0D RID: 23821 RVA: 0x00207DA8 File Offset: 0x002061A8
		public void Play()
		{
			if (this.thisAudioSource != null && this.thisAudioSource.isActiveAndEnabled && !Util.IsNullOrEmpty<AudioClip>(this.waveFiles))
			{
				this.thisAudioSource.volume = UnityEngine.Random.Range(this.volMin, this.volMax);
				this.thisAudioSource.pitch = UnityEngine.Random.Range(this.pitchMin, this.pitchMax);
				this.thisAudioSource.PlayOneShot(this.waveFiles[UnityEngine.Random.Range(0, this.waveFiles.Length)]);
			}
		}

		// Token: 0x06005D0E RID: 23822 RVA: 0x00207E3E File Offset: 0x0020623E
		public void Pause()
		{
			if (this.thisAudioSource != null)
			{
				this.thisAudioSource.Pause();
			}
		}

		// Token: 0x06005D0F RID: 23823 RVA: 0x00207E5C File Offset: 0x0020625C
		public void UnPause()
		{
			if (this.thisAudioSource != null)
			{
				this.thisAudioSource.UnPause();
			}
		}

		// Token: 0x040042A3 RID: 17059
		public AudioClip[] waveFiles;

		// Token: 0x040042A4 RID: 17060
		private AudioSource thisAudioSource;

		// Token: 0x040042A5 RID: 17061
		public float volMin;

		// Token: 0x040042A6 RID: 17062
		public float volMax;

		// Token: 0x040042A7 RID: 17063
		public float pitchMin;

		// Token: 0x040042A8 RID: 17064
		public float pitchMax;

		// Token: 0x040042A9 RID: 17065
		public bool playOnAwake;
	}
}
