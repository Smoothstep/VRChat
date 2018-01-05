using System;

namespace PrimitivesPro.CSG
{
	// Token: 0x0200083F RID: 2111
	public class VertexHash
	{
		// Token: 0x0600418D RID: 16781 RVA: 0x0014B310 File Offset: 0x00149710
		public VertexHash(float bucketSize, int allocSize)
		{
			this.bucketSize2 = bucketSize * bucketSize;
			this.buckets = new CSGVertex[allocSize];
			this.count = 0;
		}

		// Token: 0x0600418E RID: 16782 RVA: 0x0014B334 File Offset: 0x00149734
		public int Hash(CSGVertex p)
		{
			for (int i = 0; i < this.count; i++)
			{
				CSGVertex csgvertex = this.buckets[i];
				float num = p.P.x - csgvertex.P.x;
				float num2 = p.P.y - csgvertex.P.y;
				float num3 = p.P.z - csgvertex.P.z;
				float num4 = num * num + num2 * num2 + num3 * num3;
				if (num4 < this.bucketSize2)
				{
					num = p.N.x - csgvertex.N.x;
					num2 = p.N.y - csgvertex.N.y;
					num3 = p.N.z - csgvertex.N.z;
					num4 = num * num + num2 * num2 + num3 * num3;
					if (num4 < this.bucketSize2)
					{
						num = p.UV.x - csgvertex.UV.x;
						num2 = p.UV.y - csgvertex.UV.y;
						num4 = num * num + num2 * num2;
						if (num4 < this.bucketSize2)
						{
							return i;
						}
					}
				}
			}
			this.buckets[this.count++] = p;
			return this.count - 1;
		}

		// Token: 0x04002A83 RID: 10883
		private readonly CSGVertex[] buckets;

		// Token: 0x04002A84 RID: 10884
		private readonly float bucketSize2;

		// Token: 0x04002A85 RID: 10885
		private int count;
	}
}
