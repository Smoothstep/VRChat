using System;
using System.Collections.Generic;
using UnityEngine;

namespace PhysSound
{
	// Token: 0x020007B9 RID: 1977
	public class PhysSoundMaterial : ScriptableObject
	{
		// Token: 0x06003FC9 RID: 16329 RVA: 0x001403EC File Offset: 0x0013E7EC
		private void OnEnable()
		{
			if (this.AudioSets.Count <= 0)
			{
				return;
			}
			this.audioSetDic = new Dictionary<int, PhysSoundAudioSet>();
			foreach (PhysSoundAudioSet physSoundAudioSet in this.AudioSets)
			{
				if (this.audioSetDic.ContainsKey(physSoundAudioSet.Key))
				{
					Debug.LogError(string.Concat(new string[]
					{
						"PhysSound Material ",
						base.name,
						" has duplicate audio set for Material Type \"",
						PhysSoundTypeList.GetKey(physSoundAudioSet.Key),
						"\". It will not be used during runtime."
					}));
				}
				else
				{
					this.audioSetDic.Add(physSoundAudioSet.Key, physSoundAudioSet);
				}
			}
			if (this.FallbackTypeIndex == 0)
			{
				this.FallbackTypeKey = -1;
			}
			else
			{
				this.FallbackTypeKey = this.AudioSets[this.FallbackTypeIndex - 1].Key;
			}
		}

		// Token: 0x06003FCA RID: 16330 RVA: 0x00140500 File Offset: 0x0013E900
		public KeyValuePair<int, int>? GetImpactAudio(GameObject otherObject, Vector3 relativeVel, Vector3 norm, Vector3 contact, int layer = -1)
		{
			if (this.audioSetDic == null)
			{
				return null;
			}
			if (!this.CollideWith(otherObject))
			{
				return null;
			}
			PhysSoundMaterial physSoundMaterial = null;
			PhysSoundBase component = otherObject.GetComponent<PhysSoundBase>();
			if (component)
			{
				physSoundMaterial = component.GetPhysSoundMaterial(contact);
			}
			int key = 0;
			int? num = null;
			if (this.UseCollisionVelocity)
			{
				float impactVolume = this.GetImpactVolume(relativeVel, norm);
				if (impactVolume < 0f)
				{
					return null;
				}
				if (physSoundMaterial)
				{
					PhysSoundAudioSet physSoundAudioSet;
					if (this.audioSetDic.TryGetValue(physSoundMaterial.MaterialTypeKey, out physSoundAudioSet))
					{
						key = physSoundMaterial.MaterialTypeKey;
						num = physSoundAudioSet.GetImpact(impactVolume, false);
					}
					else if (this.FallbackTypeKey != -1)
					{
						key = this.FallbackTypeKey;
						num = physSoundAudioSet.GetImpact(impactVolume, false);
					}
				}
				else if (this.FallbackTypeKey != -1)
				{
					key = this.FallbackTypeKey;
					num = this.audioSetDic[this.FallbackTypeKey].GetImpact(impactVolume, false);
				}
			}
			else if (physSoundMaterial)
			{
				PhysSoundAudioSet physSoundAudioSet2;
				if (this.audioSetDic.TryGetValue(physSoundMaterial.MaterialTypeKey, out physSoundAudioSet2))
				{
					key = physSoundMaterial.MaterialTypeKey;
					num = physSoundAudioSet2.GetImpact(0f, true);
				}
				else if (this.FallbackTypeKey != -1)
				{
					key = this.FallbackTypeKey;
					num = physSoundAudioSet2.GetImpact(0f, true);
				}
			}
			else if (this.FallbackTypeKey != -1)
			{
				key = this.FallbackTypeKey;
				num = this.audioSetDic[this.FallbackTypeKey].GetImpact(0f, true);
			}
			if (num != null)
			{
				return new KeyValuePair<int, int>?(new KeyValuePair<int, int>(key, num.Value));
			}
			return null;
		}

		// Token: 0x06003FCB RID: 16331 RVA: 0x001406E0 File Offset: 0x0013EAE0
		public float GetSlideVolume(Vector3 relativeVel, Vector3 norm)
		{
			float num = 1f - Mathf.Abs(Vector3.Dot(norm, relativeVel));
			float val = num * relativeVel.magnitude * this.SlideVolMultiplier;
			return this.RelativeVelocityThreshold.Normalize(val);
		}

		// Token: 0x06003FCC RID: 16332 RVA: 0x00140720 File Offset: 0x0013EB20
		public float GetImpactVolume(Vector3 relativeVel, Vector3 norm)
		{
			float num = Mathf.Abs(Vector3.Dot(norm.normalized, relativeVel.normalized));
			float num2 = (num + (1f - num) * (1f - this.ImpactNormalBias)) * relativeVel.magnitude;
			if (num2 < this.RelativeVelocityThreshold.Min)
			{
				return -1f;
			}
			return this.RelativeVelocityThreshold.Normalize(num2);
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x00140788 File Offset: 0x0013EB88
		public bool HasAudioSet(int keyIndex)
		{
			foreach (PhysSoundAudioSet physSoundAudioSet in this.AudioSets)
			{
				if (physSoundAudioSet.CompareKeyIndex(keyIndex))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003FCE RID: 16334 RVA: 0x001407F4 File Offset: 0x0013EBF4
		public PhysSoundAudioSet GetAudioSet(int keyIndex)
		{
			foreach (PhysSoundAudioSet physSoundAudioSet in this.AudioSets)
			{
				if (physSoundAudioSet.CompareKeyIndex(keyIndex))
				{
					return physSoundAudioSet;
				}
			}
			return null;
		}

		// Token: 0x06003FCF RID: 16335 RVA: 0x00140860 File Offset: 0x0013EC60
		public string[] GetFallbackAudioSets()
		{
			string[] array = new string[this.AudioSets.Count + 1];
			array[0] = "None";
			for (int i = 0; i < this.AudioSets.Count; i++)
			{
				array[i + 1] = PhysSoundTypeList.GetKey(this.AudioSets[i].Key);
			}
			return array;
		}

		// Token: 0x06003FD0 RID: 16336 RVA: 0x001408C0 File Offset: 0x0013ECC0
		public bool CollideWith(GameObject g)
		{
			return !(g == null) && (1 << g.layer & this.CollisionMask.value) != 0;
		}

		// Token: 0x04002830 RID: 10288
		public int MaterialTypeKey;

		// Token: 0x04002831 RID: 10289
		public int FallbackTypeIndex;

		// Token: 0x04002832 RID: 10290
		public int FallbackTypeKey;

		// Token: 0x04002833 RID: 10291
		public Range RelativeVelocityThreshold;

		// Token: 0x04002834 RID: 10292
		public float PitchRandomness = 0.1f;

		// Token: 0x04002835 RID: 10293
		public float SlidePitchMod = 0.05f;

		// Token: 0x04002836 RID: 10294
		public float SlideVolMultiplier = 1f;

		// Token: 0x04002837 RID: 10295
		public float ImpactNormalBias = 1f;

		// Token: 0x04002838 RID: 10296
		public LayerMask CollisionMask = -1;

		// Token: 0x04002839 RID: 10297
		public bool UseCollisionVelocity = true;

		// Token: 0x0400283A RID: 10298
		public bool ScaleImpactVolume = true;

		// Token: 0x0400283B RID: 10299
		public List<PhysSoundAudioSet> AudioSets = new List<PhysSoundAudioSet>();

		// Token: 0x0400283C RID: 10300
		public Dictionary<int, PhysSoundAudioSet> audioSetDic;
	}
}
