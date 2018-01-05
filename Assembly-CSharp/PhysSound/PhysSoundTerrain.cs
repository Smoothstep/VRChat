using System;
using System.Collections.Generic;
using UnityEngine;

namespace PhysSound
{
	// Token: 0x020007BF RID: 1983
	[AddComponentMenu("PhysSound/PhysSound Terrain")]
	public class PhysSoundTerrain : PhysSoundBase
	{
		// Token: 0x06003FFE RID: 16382 RVA: 0x00141DA4 File Offset: 0x001401A4
		private void Start()
		{
			this.terrainData = this.Terrain.terrainData;
			this.terrainPos = this.Terrain.transform.position;
			foreach (PhysSoundMaterial physSoundMaterial in this.SoundMaterials)
			{
				if (!this.compDic.ContainsKey(physSoundMaterial.MaterialTypeKey))
				{
					this.compDic.Add(physSoundMaterial.MaterialTypeKey, new PhysSoundComposition(physSoundMaterial.MaterialTypeKey));
				}
			}
		}

		// Token: 0x06003FFF RID: 16383 RVA: 0x00141E54 File Offset: 0x00140254
		public override PhysSoundMaterial GetPhysSoundMaterial(Vector3 contactPoint)
		{
			int dominantTexture = this.getDominantTexture(contactPoint);
			if (dominantTexture < this.SoundMaterials.Count && this.SoundMaterials[dominantTexture] != null)
			{
				return this.SoundMaterials[dominantTexture];
			}
			return null;
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x00141EA0 File Offset: 0x001402A0
		public Dictionary<int, PhysSoundComposition> GetComposition(Vector3 contactPoint)
		{
			foreach (PhysSoundComposition physSoundComposition in this.compDic.Values)
			{
				physSoundComposition.Reset();
			}
			float[] textureMix = this.getTextureMix(contactPoint);
			for (int i = 0; i < textureMix.Length; i++)
			{
				if (i >= this.SoundMaterials.Count)
				{
					break;
				}
				if (!(this.SoundMaterials[i] == null))
				{
					PhysSoundComposition physSoundComposition2;
					if (this.compDic.TryGetValue(this.SoundMaterials[i].MaterialTypeKey, out physSoundComposition2))
					{
						physSoundComposition2.Add(textureMix[i]);
					}
				}
			}
			return this.compDic;
		}

		// Token: 0x06004001 RID: 16385 RVA: 0x00141F84 File Offset: 0x00140384
		private float[] getTextureMix(Vector3 worldPos)
		{
			int x = (int)((worldPos.x - this.terrainPos.x) / this.terrainData.size.x * (float)this.terrainData.alphamapWidth);
			int y = (int)((worldPos.z - this.terrainPos.z) / this.terrainData.size.z * (float)this.terrainData.alphamapHeight);
			float[,,] alphamaps = this.terrainData.GetAlphamaps(x, y, 1, 1);
			float[] array = new float[alphamaps.GetUpperBound(2) + 1];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = alphamaps[0, 0, i];
			}
			return array;
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x0014204C File Offset: 0x0014044C
		private int getDominantTexture(Vector3 worldPos)
		{
			float[] textureMix = this.getTextureMix(worldPos);
			float num = 0f;
			int result = 0;
			for (int i = 0; i < textureMix.Length; i++)
			{
				if (textureMix[i] > num)
				{
					result = i;
					num = textureMix[i];
				}
			}
			return result;
		}

		// Token: 0x0400285B RID: 10331
		public Terrain Terrain;

		// Token: 0x0400285C RID: 10332
		public List<PhysSoundMaterial> SoundMaterials = new List<PhysSoundMaterial>();

		// Token: 0x0400285D RID: 10333
		private Dictionary<int, PhysSoundComposition> compDic = new Dictionary<int, PhysSoundComposition>();

		// Token: 0x0400285E RID: 10334
		private TerrainData terrainData;

		// Token: 0x0400285F RID: 10335
		private Vector3 terrainPos;
	}
}
