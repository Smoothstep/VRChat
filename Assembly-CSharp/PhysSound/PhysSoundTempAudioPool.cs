using System;
using System.Linq;
using UnityEngine;

namespace PhysSound
{
	// Token: 0x020007BE RID: 1982
	public class PhysSoundTempAudioPool : MonoBehaviour
	{
		// Token: 0x06003FF6 RID: 16374 RVA: 0x00141ABC File Offset: 0x0013FEBC
		public static void Create()
		{
			if (PhysSoundTempAudioPool.Instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("PhysSound Temp Audio Pool");
			PhysSoundTempAudioPool physSoundTempAudioPool = gameObject.AddComponent<PhysSoundTempAudioPool>();
			physSoundTempAudioPool.Initialize();
		}

		// Token: 0x06003FF7 RID: 16375 RVA: 0x00141AF4 File Offset: 0x0013FEF4
		public static AudioSource GetAudioSourceCopy(AudioSource template, GameObject g)
		{
			AudioSource audioSource = g.AddComponent<AudioSource>();
			if (!template)
			{
				return audioSource;
			}
			PhysSoundTempAudioPool.GetAudioSourceCopy(template, audioSource);
			return audioSource;
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x00141B20 File Offset: 0x0013FF20
		public static void GetAudioSourceCopy(AudioSource template, AudioSource target)
		{
			target.bypassEffects = template.bypassEffects;
			target.bypassListenerEffects = template.bypassListenerEffects;
			target.bypassReverbZones = template.bypassReverbZones;
			target.dopplerLevel = template.dopplerLevel;
			target.ignoreListenerPause = template.ignoreListenerPause;
			target.ignoreListenerVolume = template.ignoreListenerVolume;
			target.loop = template.loop;
			target.maxDistance = template.maxDistance;
			target.minDistance = template.minDistance;
			target.mute = template.mute;
			target.outputAudioMixerGroup = template.outputAudioMixerGroup;
			target.panStereo = template.panStereo;
			target.pitch = template.pitch;
			target.playOnAwake = template.playOnAwake;
			target.priority = template.priority;
			target.reverbZoneMix = template.reverbZoneMix;
			target.rolloffMode = template.rolloffMode;
			target.spatialBlend = template.spatialBlend;
			target.spread = template.spread;
			target.time = template.time;
			target.timeSamples = template.timeSamples;
			target.velocityUpdateMode = template.velocityUpdateMode;
			target.volume = template.volume;
		}

		// Token: 0x06003FF9 RID: 16377 RVA: 0x00141C44 File Offset: 0x00140044
		public void Initialize()
		{
			PhysSoundTempAudioPool.Instance = this;
			this.audioSources = new PhysSoundTempAudio[PhysSoundTempAudioPool.TempAudioPoolSize];
			for (int i = 0; i < PhysSoundTempAudioPool.TempAudioPoolSize; i++)
			{
				GameObject gameObject = new GameObject("Temp Audio Source");
				PhysSoundTempAudio physSoundTempAudio = gameObject.AddComponent<PhysSoundTempAudio>();
				physSoundTempAudio.Initialize(this);
				this.audioSources[i] = physSoundTempAudio;
			}
		}

		// Token: 0x06003FFA RID: 16378 RVA: 0x00141CA0 File Offset: 0x001400A0
		public void PlayClip(AudioClip clip, Vector3 point, AudioSource template, float volume, float pitch)
		{
			int i = 0;
			int num = this.lastAvailable;
			while (i < PhysSoundTempAudioPool.TempAudioPoolSize)
			{
				PhysSoundTempAudio physSoundTempAudio = this.audioSources[num];
				if (!physSoundTempAudio.gameObject.activeInHierarchy)
				{
					physSoundTempAudio.PlayClip(clip, point, template, volume, pitch);
					this.lastAvailable = num;
					return;
				}
				num++;
				i++;
				if (num >= PhysSoundTempAudioPool.TempAudioPoolSize)
				{
					num = 0;
				}
			}
		}

		// Token: 0x06003FFB RID: 16379 RVA: 0x00141D0C File Offset: 0x0014010C
		public bool ClipIsPlaying(AudioClip clip)
		{
			return this.audioSources.Any((PhysSoundTempAudio source) => source.gameObject.activeInHierarchy && source.isPlaying && source.ClipName == clip.name);
		}

		// Token: 0x04002857 RID: 10327
		public static int TempAudioPoolSize = 10;

		// Token: 0x04002858 RID: 10328
		public static PhysSoundTempAudioPool Instance;

		// Token: 0x04002859 RID: 10329
		private PhysSoundTempAudio[] audioSources;

		// Token: 0x0400285A RID: 10330
		private int lastAvailable;
	}
}
