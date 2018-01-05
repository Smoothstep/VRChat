using System;
using System.Collections;
using UnityEngine;

namespace VRC.Core
{
	// Token: 0x02000A53 RID: 2643
	public class AudioClipWarmer : MonoBehaviour
	{
		// Token: 0x0600500D RID: 20493 RVA: 0x001B5AF4 File Offset: 0x001B3EF4
		public static void Warm(AudioClip clip)
		{
			if (clip == null)
			{
				return;
			}
			new GameObject("Audio Warmer for " + clip.name, new Type[]
			{
				typeof(AudioSource),
				typeof(AudioClipWarmer)
			})
			{
				hideFlags = HideFlags.HideInHierarchy
			}.GetComponent<AudioClipWarmer>().clip = clip;
		}

		// Token: 0x0600500E RID: 20494 RVA: 0x001B5B58 File Offset: 0x001B3F58
		private IEnumerator Start()
		{
			if (this.clip == null)
			{
				UnityEngine.Object.Destroy(this);
				yield break;
			}
			AudioSource audioSource = base.GetComponent<AudioSource>();
			audioSource.clip = this.clip;
			audioSource.volume = 0f;
			audioSource.Play();
			yield return new WaitUntil(() => audioSource.isPlaying);
			audioSource.Stop();
			yield return new WaitUntil(() => !audioSource.isPlaying);
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x040038E5 RID: 14565
		public AudioClip clip;
	}
}
