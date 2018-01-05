using System;
using UnityEngine;

namespace PhysSound
{
	// Token: 0x020007BD RID: 1981
	public class PhysSoundTempAudio : MonoBehaviour
	{
		// Token: 0x06003FF0 RID: 16368 RVA: 0x00141973 File Offset: 0x0013FD73
		public void Initialize(PhysSoundTempAudioPool pool)
		{
			this.audioSource = base.gameObject.AddComponent<AudioSource>();
			base.transform.SetParent(pool.transform);
			base.gameObject.SetActive(false);
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x001419A4 File Offset: 0x0013FDA4
		public void PlayClip(AudioClip clip, Vector3 point, AudioSource template, float volume, float pitch)
		{
			PhysSoundTempAudioPool.GetAudioSourceCopy(template, this.audioSource);
			base.transform.position = point;
			this.audioSource.clip = clip;
			this.audioSource.volume = volume;
			this.audioSource.pitch = pitch;
			base.gameObject.SetActive(true);
			this.audioSource.Play();
		}

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x06003FF2 RID: 16370 RVA: 0x00141A08 File Offset: 0x0013FE08
		public string ClipName
		{
			get
			{
				return (!(this.audioSource == null) && base.gameObject.activeInHierarchy && !(this.audioSource.clip == null)) ? this.audioSource.clip.name : null;
			}
		}

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x06003FF3 RID: 16371 RVA: 0x00141A62 File Offset: 0x0013FE62
		public bool isPlaying
		{
			get
			{
				return this.audioSource != null && this.audioSource.isPlaying;
			}
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x00141A83 File Offset: 0x0013FE83
		private void Update()
		{
			if (!this.audioSource.isPlaying)
			{
				base.transform.position = Vector3.zero;
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x04002856 RID: 10326
		private AudioSource audioSource;
	}
}
