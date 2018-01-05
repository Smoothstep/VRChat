using System;
using UnityEngine;
using VRC;
using VRCSDK2;

namespace PhysSound
{
	// Token: 0x020007BC RID: 1980
	[Serializable]
	public class PhysSoundAudioContainer
	{
		// Token: 0x06003FE7 RID: 16359 RVA: 0x00141671 File Offset: 0x0013FA71
		public PhysSoundAudioContainer(int k)
		{
			this.KeyIndex = k;
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x00141680 File Offset: 0x0013FA80
		public void Initialize(PhysSoundMaterial m)
		{
			if (this.SlideAudio == null)
			{
				return;
			}
			this.mat = m;
			this.SlideAudio.clip = this.mat.GetAudioSet(this.KeyIndex).Slide;
			this.baseVol = this.SlideAudio.volume;
			this.basePitch = this.SlideAudio.pitch;
			this.basePitchRand = this.basePitch;
			this.SlideAudio.loop = true;
			this.SlideAudio.volume = 0f;
		}

		// Token: 0x06003FE9 RID: 16361 RVA: 0x00141714 File Offset: 0x0013FB14
		public void SetTargetVolumeAndPitch(GameObject thisObject, GameObject otherObject, Vector3 relativeVelocity, Vector3 normal, bool exit, float mod = 1f)
		{
			if (this.SlideAudio == null)
			{
				return;
			}
			float num = this.basePitchRand + relativeVelocity.magnitude * this.mat.SlidePitchMod;
			float num2 = (!exit && this.mat.CollideWith(otherObject)) ? (this.mat.GetSlideVolume(relativeVelocity, normal) * this.baseVol * mod) : 0f;
			bool flag = exit || ((Mathf.Approximately(num, 0f) || Mathf.Approximately(num, 1f)) && !Mathf.Approximately(this.SlideAudio.pitch, num)) || ((Mathf.Approximately(num2, 0f) || Mathf.Approximately(num2, 1f)) && !Mathf.Approximately(this.targetVolume, num2));
			if (flag)
			{
				VRC.Network.RPC(VRC_EventHandler.VrcTargetType.Others, thisObject, "_SetTargetVolumeAndPitch", new object[]
				{
					this.KeyIndex,
					(short)Mathf.FloatToHalf(num2),
					(short)Mathf.FloatToHalf(num)
				});
			}
			thisObject.GetComponent<PhysSoundObject>()._SetTargetVolumeAndPitch(this.KeyIndex, (short)Mathf.FloatToHalf(num2), (short)Mathf.FloatToHalf(num), VRC.Network.LocalPlayer);
		}

		// Token: 0x06003FEA RID: 16362 RVA: 0x00141864 File Offset: 0x0013FC64
		public void _SetTargetVolumeAndPitch(float volume, float pitch)
		{
			if (this.SlideAudio == null)
			{
				return;
			}
			if (!this.SlideAudio.isPlaying)
			{
				this.SlideAudio.Play();
			}
			this.SlideAudio.pitch = pitch;
			this.targetVolume = volume;
		}

		// Token: 0x06003FEB RID: 16363 RVA: 0x001418B4 File Offset: 0x0013FCB4
		public void UpdateVolume()
		{
			if (this.SlideAudio == null)
			{
				return;
			}
			this.SlideAudio.volume = Mathf.MoveTowards(this.SlideAudio.volume, this.targetVolume, 0.06f);
			if (this.SlideAudio.volume < 0.01f)
			{
				this.SlideAudio.Stop();
			}
		}

		// Token: 0x06003FEC RID: 16364 RVA: 0x00141919 File Offset: 0x0013FD19
		public bool CompareKeyIndex(int k)
		{
			return k == this.KeyIndex;
		}

		// Token: 0x06003FED RID: 16365 RVA: 0x00141924 File Offset: 0x0013FD24
		public void Disable()
		{
			if (this.SlideAudio)
			{
				this.SlideAudio.Stop();
				this.SlideAudio.enabled = false;
			}
		}

		// Token: 0x06003FEE RID: 16366 RVA: 0x0014194D File Offset: 0x0013FD4D
		public void Enable()
		{
			if (this.SlideAudio)
			{
				this.SlideAudio.enabled = true;
			}
		}

		// Token: 0x0400284F RID: 10319
		public int KeyIndex;

		// Token: 0x04002850 RID: 10320
		public AudioSource SlideAudio;

		// Token: 0x04002851 RID: 10321
		private PhysSoundMaterial mat;

		// Token: 0x04002852 RID: 10322
		private float targetVolume;

		// Token: 0x04002853 RID: 10323
		private float baseVol;

		// Token: 0x04002854 RID: 10324
		private float basePitch;

		// Token: 0x04002855 RID: 10325
		private float basePitchRand;
	}
}
