using System;
using System.Collections.Generic;
using UnityEngine;

namespace PhysSound
{
	// Token: 0x020007BA RID: 1978
	[Serializable]
	public class PhysSoundAudioSet
	{
		// Token: 0x06003FD2 RID: 16338 RVA: 0x00140900 File Offset: 0x0013ED00
		public int? GetImpact(float vel, bool random)
		{
			if (this.Impacts.Count == 0)
			{
				return null;
			}
			if (random)
			{
				return new int?(UnityEngine.Random.Range(0, this.Impacts.Count));
			}
			return new int?((int)(vel * (float)(this.Impacts.Count - 1)));
		}

		// Token: 0x06003FD3 RID: 16339 RVA: 0x0014095A File Offset: 0x0013ED5A
		public bool CompareKeyIndex(int k)
		{
			return this.Key == k;
		}

		// Token: 0x0400283D RID: 10301
		public int Key;

		// Token: 0x0400283E RID: 10302
		public List<AudioClip> Impacts = new List<AudioClip>();

		// Token: 0x0400283F RID: 10303
		public AudioClip Slide;
	}
}
