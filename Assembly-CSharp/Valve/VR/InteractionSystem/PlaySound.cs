using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BBC RID: 3004
	[RequireComponent(typeof(AudioSource))]
	public class PlaySound : MonoBehaviour
	{
		// Token: 0x06005CE5 RID: 23781 RVA: 0x00206F60 File Offset: 0x00205360
		private void Awake()
		{
			this.audioSource = base.GetComponent<AudioSource>();
			this.clip = this.audioSource.clip;
			if (this.audioSource.playOnAwake)
			{
				if (this.useRetriggerTime)
				{
					base.InvokeRepeating("Play", this.timeInitial, UnityEngine.Random.Range(this.timeMin, this.timeMax));
				}
				else
				{
					this.Play();
				}
			}
			else if (!this.audioSource.playOnAwake && this.playOnAwakeWithDelay)
			{
				this.PlayWithDelay(this.delayOffsetTime);
				if (this.useRetriggerTime)
				{
					base.InvokeRepeating("Play", this.timeInitial, UnityEngine.Random.Range(this.timeMin, this.timeMax));
				}
			}
			else if (this.audioSource.playOnAwake && this.playOnAwakeWithDelay)
			{
				this.PlayWithDelay(this.delayOffsetTime);
				if (this.useRetriggerTime)
				{
					base.InvokeRepeating("Play", this.timeInitial, UnityEngine.Random.Range(this.timeMin, this.timeMax));
				}
			}
		}

		// Token: 0x06005CE6 RID: 23782 RVA: 0x00207084 File Offset: 0x00205484
		public void Play()
		{
			if (this.looping)
			{
				this.PlayLooping();
			}
			else
			{
				this.PlayOneShotSound();
			}
		}

		// Token: 0x06005CE7 RID: 23783 RVA: 0x002070A4 File Offset: 0x002054A4
		public void PlayWithDelay(float delayTime)
		{
			if (this.looping)
			{
				base.Invoke("PlayLooping", delayTime);
			}
			else
			{
				base.Invoke("PlayOneShotSound", delayTime);
			}
		}

		// Token: 0x06005CE8 RID: 23784 RVA: 0x002070D0 File Offset: 0x002054D0
		public AudioClip PlayOneShotSound()
		{
			if (!this.audioSource.isActiveAndEnabled)
			{
				return null;
			}
			this.SetAudioSource();
			if (this.stopOnPlay)
			{
				this.audioSource.Stop();
			}
			if (this.disableOnEnd)
			{
				base.Invoke("Disable", this.clip.length);
			}
			this.audioSource.PlayOneShot(this.clip);
			return this.clip;
		}

		// Token: 0x06005CE9 RID: 23785 RVA: 0x00207144 File Offset: 0x00205544
		public AudioClip PlayLooping()
		{
			this.SetAudioSource();
			if (!this.audioSource.loop)
			{
				this.audioSource.loop = true;
			}
			this.audioSource.Play();
			if (this.stopOnEnd)
			{
				base.Invoke("Stop", this.audioSource.clip.length);
			}
			return this.clip;
		}

		// Token: 0x06005CEA RID: 23786 RVA: 0x002071AA File Offset: 0x002055AA
		public void Disable()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06005CEB RID: 23787 RVA: 0x002071B8 File Offset: 0x002055B8
		public void Stop()
		{
			this.audioSource.Stop();
		}

		// Token: 0x06005CEC RID: 23788 RVA: 0x002071C8 File Offset: 0x002055C8
		private void SetAudioSource()
		{
			if (this.useRandomVolume)
			{
				this.audioSource.volume = UnityEngine.Random.Range(this.volMin, this.volMax);
				if (this.useRandomSilence && (float)UnityEngine.Random.Range(0, 1) < this.percentToNotPlay)
				{
					this.audioSource.volume = 0f;
				}
			}
			if (this.useRandomPitch)
			{
				this.audioSource.pitch = UnityEngine.Random.Range(this.pitchMin, this.pitchMax);
			}
			if (this.waveFile.Length > 0)
			{
				this.audioSource.clip = this.waveFile[UnityEngine.Random.Range(0, this.waveFile.Length)];
				this.clip = this.audioSource.clip;
			}
		}

		// Token: 0x0400427E RID: 17022
		[Tooltip("List of audio clips to play.")]
		public AudioClip[] waveFile;

		// Token: 0x0400427F RID: 17023
		[Tooltip("Stops the currently playing clip in the audioSource. Otherwise clips will overlap/mix.")]
		public bool stopOnPlay;

		// Token: 0x04004280 RID: 17024
		[Tooltip("After the audio clip finishes playing, disable the game object the sound is on.")]
		public bool disableOnEnd;

		// Token: 0x04004281 RID: 17025
		[Tooltip("Loop the sound after the wave file variation has been chosen.")]
		public bool looping;

		// Token: 0x04004282 RID: 17026
		[Tooltip("If the sound is looping and updating it's position every frame, stop the sound at the end of the wav/clip length. ")]
		public bool stopOnEnd;

		// Token: 0x04004283 RID: 17027
		[Tooltip("Start a wave file playing on awake, but after a delay.")]
		public bool playOnAwakeWithDelay;

		// Token: 0x04004284 RID: 17028
		[Header("Random Volume")]
		public bool useRandomVolume = true;

		// Token: 0x04004285 RID: 17029
		[Tooltip("Minimum volume that will be used when randomly set.")]
		[Range(0f, 1f)]
		public float volMin = 1f;

		// Token: 0x04004286 RID: 17030
		[Tooltip("Maximum volume that will be used when randomly set.")]
		[Range(0f, 1f)]
		public float volMax = 1f;

		// Token: 0x04004287 RID: 17031
		[Header("Random Pitch")]
		[Tooltip("Use min and max random pitch levels when playing sounds.")]
		public bool useRandomPitch = true;

		// Token: 0x04004288 RID: 17032
		[Tooltip("Minimum pitch that will be used when randomly set.")]
		[Range(-3f, 3f)]
		public float pitchMin = 1f;

		// Token: 0x04004289 RID: 17033
		[Tooltip("Maximum pitch that will be used when randomly set.")]
		[Range(-3f, 3f)]
		public float pitchMax = 1f;

		// Token: 0x0400428A RID: 17034
		[Header("Random Time")]
		[Tooltip("Use Retrigger Time to repeat the sound within a time range")]
		public bool useRetriggerTime;

		// Token: 0x0400428B RID: 17035
		[Tooltip("Inital time before the first repetion starts")]
		[Range(0f, 360f)]
		public float timeInitial;

		// Token: 0x0400428C RID: 17036
		[Tooltip("Minimum time that will pass before the sound is retriggered")]
		[Range(0f, 360f)]
		public float timeMin;

		// Token: 0x0400428D RID: 17037
		[Tooltip("Maximum pitch that will be used when randomly set.")]
		[Range(0f, 360f)]
		public float timeMax;

		// Token: 0x0400428E RID: 17038
		[Header("Random Silence")]
		[Tooltip("Use Retrigger Time to repeat the sound within a time range")]
		public bool useRandomSilence;

		// Token: 0x0400428F RID: 17039
		[Tooltip("Percent chance that the wave file will not play")]
		[Range(0f, 1f)]
		public float percentToNotPlay;

		// Token: 0x04004290 RID: 17040
		[Header("Delay Time")]
		[Tooltip("Time to offset playback of sound")]
		public float delayOffsetTime;

		// Token: 0x04004291 RID: 17041
		private AudioSource audioSource;

		// Token: 0x04004292 RID: 17042
		private AudioClip clip;
	}
}
