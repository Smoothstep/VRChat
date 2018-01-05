using System;
using UnityEngine;

namespace PrimitivesPro.MeshCutting
{
	// Token: 0x02000864 RID: 2148
	public class LSHash
	{
		// Token: 0x0600429E RID: 17054 RVA: 0x00152E49 File Offset: 0x00151249
		public LSHash(float bucketSize, int allocSize)
		{
			this.bucketSize2 = bucketSize * bucketSize;
			this.buckets = new Vector3[allocSize];
			this.count = 0;
		}

		// Token: 0x0600429F RID: 17055 RVA: 0x00152E70 File Offset: 0x00151270
		public void Clear()
		{
			for (int i = 0; i < this.buckets.Length; i++)
			{
				this.buckets[i] = Vector3.zero;
			}
			this.count = 0;
		}

		// Token: 0x060042A0 RID: 17056 RVA: 0x00152EB4 File Offset: 0x001512B4
		public int Hash(Vector3 p)
		{
			for (int i = 0; i < this.count; i++)
			{
				Vector3 vector = this.buckets[i];
				float num = p.x - vector.x;
				float num2 = p.y - vector.y;
				float num3 = p.z - vector.z;
				float num4 = num * num + num2 * num2 + num3 * num3;
				if (num4 < this.bucketSize2)
				{
					return i;
				}
			}
			this.buckets[this.count++] = p;
			return this.count - 1;
		}

		// Token: 0x060042A1 RID: 17057 RVA: 0x00152F68 File Offset: 0x00151368
		public void Hash(Vector3 p0, Vector3 p1, out int hash0, out int hash1)
		{
			float num = p0.x - p1.x;
			float num2 = p0.y - p1.y;
			float num3 = p0.z - p1.z;
			float num4 = num * num + num2 * num2 + num3 * num3;
			if (num4 < this.bucketSize2)
			{
				hash0 = this.Hash(p0);
				hash1 = hash0;
				return;
			}
			hash0 = this.Hash(p0);
			hash1 = this.Hash(p1);
		}

		// Token: 0x04002B40 RID: 11072
		private readonly Vector3[] buckets;

		// Token: 0x04002B41 RID: 11073
		private readonly float bucketSize2;

		// Token: 0x04002B42 RID: 11074
		private int count;
	}
}
