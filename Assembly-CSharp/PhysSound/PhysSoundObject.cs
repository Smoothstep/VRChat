using System;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRCSDK2;

namespace PhysSound
{
	// Token: 0x020007BB RID: 1979
	[AddComponentMenu("PhysSound/PhysSound Object")]
	public class PhysSoundObject : PhysSoundBase
	{
		// Token: 0x06003FD5 RID: 16341 RVA: 0x0014098C File Offset: 0x0013ED8C
		private void Start()
		{
			if (this.SoundMaterial == null)
			{
				return;
			}
			this.r = base.GetComponent<Rigidbody>();
			this.r2D = base.GetComponent<Rigidbody2D>();
			if (this.AutoCreateSources)
			{
				if (!this.ImpactAudio.isActiveAndEnabled && !this.PlayClipAtPoint)
				{
					this.ImpactAudio = PhysSoundTempAudioPool.GetAudioSourceCopy(this.ImpactAudio, base.gameObject);
				}
				this.baseImpactVol = this.ImpactAudio.volume;
				this.baseImpactPitch = this.ImpactAudio.pitch;
				this.audioContainersDic = new Dictionary<int, PhysSoundAudioContainer>();
				this.AudioContainers = new List<PhysSoundAudioContainer>();
				foreach (PhysSoundAudioSet physSoundAudioSet in this.SoundMaterial.AudioSets)
				{
					if (!(physSoundAudioSet.Slide == null))
					{
						PhysSoundAudioContainer physSoundAudioContainer = new PhysSoundAudioContainer(physSoundAudioSet.Key);
						physSoundAudioContainer.SlideAudio = PhysSoundTempAudioPool.GetAudioSourceCopy(this.ImpactAudio, base.gameObject);
						physSoundAudioContainer.Initialize(this.SoundMaterial);
						this.audioContainersDic.Add(physSoundAudioContainer.KeyIndex, physSoundAudioContainer);
						this.AudioContainers.Add(physSoundAudioContainer);
					}
				}
				this.ImpactAudio.loop = false;
			}
			else
			{
				if (this.ImpactAudio)
				{
					this.ImpactAudio.loop = false;
					this.baseImpactVol = this.ImpactAudio.volume;
					this.baseImpactPitch = this.ImpactAudio.pitch;
				}
				if (this.AudioContainers.Count > 0)
				{
					this.audioContainersDic = new Dictionary<int, PhysSoundAudioContainer>();
					foreach (PhysSoundAudioContainer physSoundAudioContainer2 in this.AudioContainers)
					{
						if (!this.SoundMaterial.HasAudioSet(physSoundAudioContainer2.KeyIndex))
						{
							Debug.LogError("PhysSound Object " + base.gameObject.name + " has an audio container for an invalid Material Type! Select this object in the hierarchy to update its audio container list.");
						}
						else
						{
							physSoundAudioContainer2.Initialize(this.SoundMaterial);
							this.audioContainersDic.Add(physSoundAudioContainer2.KeyIndex, physSoundAudioContainer2);
						}
					}
				}
			}
			if (this.PlayClipAtPoint)
			{
				PhysSoundTempAudioPool.Create();
			}
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x00140C00 File Offset: 0x0013F000
		private void Update()
		{
			this.isMine = VRC.Network.IsOwner(base.gameObject);
			if (this.SoundMaterial == null)
			{
				return;
			}
			for (int i = 0; i < this.AudioContainers.Count; i++)
			{
				this.AudioContainers[i].UpdateVolume();
			}
			if (this.ImpactAudio && !this.ImpactAudio.isPlaying)
			{
				this.ImpactAudio.Stop();
			}
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x00140C88 File Offset: 0x0013F088
		public void SetEnabled(bool enable)
		{
			if (enable && !base.enabled)
			{
				for (int i = 0; i < this.AudioContainers.Count; i++)
				{
					this.AudioContainers[i].Enable();
				}
				this.ImpactAudio.enabled = true;
				base.enabled = true;
			}
			else if (!enable && base.enabled)
			{
				if (this.ImpactAudio)
				{
					this.ImpactAudio.Stop();
					this.ImpactAudio.enabled = false;
				}
				for (int j = 0; j < this.AudioContainers.Count; j++)
				{
					this.AudioContainers[j].Disable();
				}
				base.enabled = false;
			}
		}

		// Token: 0x06003FD8 RID: 16344 RVA: 0x00140D57 File Offset: 0x0013F157
		public override PhysSoundMaterial GetPhysSoundMaterial(Vector3 contactPoint)
		{
			return this.SoundMaterial;
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x00140D60 File Offset: 0x0013F160
		private void playImpactSound(GameObject otherObject, Vector3 relativeVelocity, Vector3 normal, Vector3 contactPoint)
		{
			if (!this.isMine)
			{
				return;
			}
			if (!this.ImpactAudio)
			{
				return;
			}
			KeyValuePair<int, int>? impactAudio = this.SoundMaterial.GetImpactAudio(otherObject, relativeVelocity, normal, contactPoint, -1);
			if (impactAudio == null)
			{
				return;
			}
			if (Time.time - this.lastPlayTime < 0.5f)
			{
				return;
			}
			this.lastPlayTime = Time.time;
			float val = this.baseImpactPitch + UnityEngine.Random.Range(-this.SoundMaterial.PitchRandomness, this.SoundMaterial.PitchRandomness);
			float val2 = this.baseImpactVol * this.SoundMaterial.GetImpactVolume(relativeVelocity, normal);
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "_playImpactSound", new object[]
			{
				impactAudio.Value.Key,
				impactAudio.Value.Value,
				(short)Mathf.FloatToHalf(val2),
				(short)Mathf.FloatToHalf(val)
			});
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x00140E68 File Offset: 0x0013F268
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.All
		})]
		private void _playImpactSound(int key, int subKey, short volume, short pitch, VRC.Player instigator)
		{
			if (VRC.Network.GetOwner(base.gameObject) != instigator)
			{
				return;
			}
			PhysSoundAudioSet physSoundAudioSet = null;
			if (!this.SoundMaterial.audioSetDic.TryGetValue(key, out physSoundAudioSet) || physSoundAudioSet.Impacts.Count < subKey)
			{
				return;
			}
			AudioClip clip = physSoundAudioSet.Impacts[subKey];
			float num = Mathf.HalfToFloat((ushort)volume);
			float pitch2 = Mathf.HalfToFloat((ushort)pitch);
			if (this.PlayClipAtPoint)
			{
				if (!PhysSoundTempAudioPool.Instance.ClipIsPlaying(clip))
				{
					PhysSoundTempAudioPool.Instance.PlayClip(clip, base.transform.position, this.ImpactAudio, (!this.SoundMaterial.ScaleImpactVolume) ? this.ImpactAudio.volume : num, pitch2);
				}
			}
			else
			{
				this.ImpactAudio.pitch = pitch2;
				if (this.SoundMaterial.ScaleImpactVolume)
				{
					this.ImpactAudio.volume = num;
				}
				this.ImpactAudio.clip = clip;
				this.ImpactAudio.Play();
			}
		}

		// Token: 0x06003FDB RID: 16347 RVA: 0x00140F74 File Offset: 0x0013F374
		private void setSlideTargetVolumes(GameObject otherObject, Vector3 relativeVelocity, Vector3 normal, Vector3 contactPoint, bool exit)
		{
			if (!this.isMine)
			{
				return;
			}
			if (this.audioContainersDic == null)
			{
				return;
			}
			this._setSlideTargetVolumes(otherObject, relativeVelocity, normal, contactPoint, exit);
		}

		// Token: 0x06003FDC RID: 16348 RVA: 0x00140F9C File Offset: 0x0013F39C
		private void _setSlideTargetVolumes(GameObject otherObject, Vector3 relativeVelocity, Vector3 normal, Vector3 contactPoint, bool exit)
		{
			PhysSoundMaterial physSoundMaterial = null;
			PhysSoundBase physSoundBase = (!(otherObject == null)) ? otherObject.GetComponent<PhysSoundBase>() : null;
			if (physSoundBase)
			{
				if (!(physSoundBase is PhysSoundTerrain))
				{
					physSoundMaterial = physSoundBase.GetPhysSoundMaterial(contactPoint);
				}
			}
			PhysSoundAudioContainer physSoundAudioContainer2;
			if (physSoundMaterial)
			{
				PhysSoundAudioContainer physSoundAudioContainer;
				if (this.audioContainersDic.TryGetValue(physSoundMaterial.MaterialTypeKey, out physSoundAudioContainer))
				{
					physSoundAudioContainer.SetTargetVolumeAndPitch(base.gameObject, otherObject, relativeVelocity, normal, exit, 1f);
				}
				else if (this.SoundMaterial.FallbackTypeKey != -1 && this.audioContainersDic.TryGetValue(this.SoundMaterial.FallbackTypeKey, out physSoundAudioContainer))
				{
					physSoundAudioContainer.SetTargetVolumeAndPitch(base.gameObject, otherObject, relativeVelocity, normal, exit, 1f);
				}
			}
			else if (this.SoundMaterial.FallbackTypeKey != -1 && this.audioContainersDic.TryGetValue(this.SoundMaterial.FallbackTypeKey, out physSoundAudioContainer2))
			{
				physSoundAudioContainer2.SetTargetVolumeAndPitch(base.gameObject, otherObject, relativeVelocity, normal, exit, 1f);
			}
		}

		// Token: 0x06003FDD RID: 16349 RVA: 0x001410B4 File Offset: 0x0013F4B4
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.Others
		})]
		public void _SetTargetVolumeAndPitch(int key, short volume, short pitch, VRC.Player instigator)
		{
			if (VRC.Network.GetOwner(base.gameObject) != instigator)
			{
				return;
			}
			PhysSoundAudioContainer physSoundAudioContainer;
			if (this.audioContainersDic.TryGetValue(key, out physSoundAudioContainer))
			{
				physSoundAudioContainer._SetTargetVolumeAndPitch(Mathf.HalfToFloat((ushort)volume), Mathf.HalfToFloat((ushort)pitch));
			}
		}

		// Token: 0x06003FDE RID: 16350 RVA: 0x00141100 File Offset: 0x0013F500
		public void OnCollisionEnter(Collision c)
		{
			if (this.SoundMaterial == null || !base.enabled || this.SoundMaterial.AudioSets == null || this.SoundMaterial.AudioSets.Count == 0 || c == null || c.collider == null || c.collider.gameObject == null || c.contacts == null || c.contacts.Length == 0)
			{
				return;
			}
			this.playImpactSound(c.collider.gameObject, c.relativeVelocity, c.contacts[0].normal, c.contacts[0].point);
			this.setPrev = true;
		}

		// Token: 0x06003FDF RID: 16351 RVA: 0x001411D4 File Offset: 0x0013F5D4
		public void OnCollisionStay(Collision c)
		{
			if (!base.enabled || this.SoundMaterial == null || this.SoundMaterial.AudioSets.Count == 0 || this.r == null || c.collider == null || c.collider.gameObject == null || c.contacts.Length == 0)
			{
				return;
			}
			if (this.setPrev)
			{
				this.prevVelocity = this.r.velocity;
			}
			Vector3 relativeVelocity = this.r.velocity - this.prevVelocity;
			if (this.setPrev || relativeVelocity.sqrMagnitude > 0.1f)
			{
				this.playImpactSound(c.collider.gameObject, relativeVelocity, c.contacts[0].normal, c.contacts[0].point);
				this.setSlideTargetVolumes(c.collider.gameObject, c.relativeVelocity, c.contacts[0].normal, c.contacts[0].point, false);
				this.prevVelocity = this.r.velocity;
			}
			this.setPrev = false;
		}

		// Token: 0x06003FE0 RID: 16352 RVA: 0x00141330 File Offset: 0x0013F730
		public void OnCollisionExit(Collision c)
		{
			if (this.SoundMaterial == null || !base.enabled || this.SoundMaterial.AudioSets.Count == 0)
			{
				return;
			}
			this.setSlideTargetVolumes(c.collider.gameObject, c.relativeVelocity, Vector3.up, base.transform.position, true);
			this.setPrev = true;
		}

		// Token: 0x06003FE1 RID: 16353 RVA: 0x001413A0 File Offset: 0x0013F7A0
		private void OnCollisionEnter2D(Collision2D c)
		{
			if (this.SoundMaterial == null || !base.enabled)
			{
				return;
			}
			this.playImpactSound(c.collider.gameObject, c.relativeVelocity, c.contacts[0].normal, c.contacts[0].point);
			this.setPrev = true;
		}

		// Token: 0x06003FE2 RID: 16354 RVA: 0x0014141C File Offset: 0x0013F81C
		private void OnCollisionStay2D(Collision2D c)
		{
			if (this.SoundMaterial == null || !base.enabled || this.SoundMaterial.AudioSets.Count == 0)
			{
				return;
			}
			if (this.setPrev)
			{
				this.prevVelocity = this.r2D.velocity;
				this.setPrev = false;
			}
            Vector3 relativeVelocity = this.r2D.velocity;
            relativeVelocity -= this.prevVelocity;
			this.playImpactSound(c.collider.gameObject, relativeVelocity, c.contacts[0].normal, c.contacts[0].point);
			this.setSlideTargetVolumes(c.collider.gameObject, c.relativeVelocity, c.contacts[0].normal, c.contacts[0].point, false);
			this.prevVelocity = this.r2D.velocity;
		}

		// Token: 0x06003FE3 RID: 16355 RVA: 0x00141544 File Offset: 0x0013F944
		private void OnCollisionExit2D(Collision2D c)
		{
			if (this.SoundMaterial == null || !base.enabled)
			{
				return;
			}
			this.setSlideTargetVolumes(c.collider.gameObject, c.relativeVelocity, Vector3.up, base.transform.position, true);
			this.setPrev = true;
		}

		// Token: 0x06003FE4 RID: 16356 RVA: 0x001415A4 File Offset: 0x0013F9A4
		public bool HasAudioContainer(int keyIndex)
		{
			foreach (PhysSoundAudioContainer physSoundAudioContainer in this.AudioContainers)
			{
				if (physSoundAudioContainer.CompareKeyIndex(keyIndex))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003FE5 RID: 16357 RVA: 0x00141610 File Offset: 0x0013FA10
		public void AddAudioContainer(int keyIndex)
		{
			this.AudioContainers.Add(new PhysSoundAudioContainer(keyIndex));
		}

		// Token: 0x06003FE6 RID: 16358 RVA: 0x00141624 File Offset: 0x0013FA24
		public void RemoveAudioContainer(int keyIndex)
		{
			for (int i = 0; i < this.AudioContainers.Count; i++)
			{
				if (this.AudioContainers[i].KeyIndex == keyIndex)
				{
					this.AudioContainers.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x04002840 RID: 10304
		public PhysSoundMaterial SoundMaterial;

		// Token: 0x04002841 RID: 10305
		public AudioSource ImpactAudio;

		// Token: 0x04002842 RID: 10306
		private float baseImpactVol;

		// Token: 0x04002843 RID: 10307
		private float baseImpactPitch;

		// Token: 0x04002844 RID: 10308
		public bool AutoCreateSources;

		// Token: 0x04002845 RID: 10309
		public bool PlayClipAtPoint;

		// Token: 0x04002846 RID: 10310
		public List<PhysSoundAudioContainer> AudioContainers = new List<PhysSoundAudioContainer>();

		// Token: 0x04002847 RID: 10311
		private Dictionary<int, PhysSoundAudioContainer> audioContainersDic;

		// Token: 0x04002848 RID: 10312
		private Vector3 prevVelocity = Vector3.zero;

		// Token: 0x04002849 RID: 10313
		private bool setPrev = true;

		// Token: 0x0400284A RID: 10314
		private Rigidbody r;

		// Token: 0x0400284B RID: 10315
		private Rigidbody2D r2D;

		// Token: 0x0400284C RID: 10316
		private bool isMine;

		// Token: 0x0400284D RID: 10317
		private float lastPlayTime;

		// Token: 0x0400284E RID: 10318
		private const float playInterval = 0.5f;
	}
}
