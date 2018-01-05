using System;
using System.Collections.Generic;
using UnityEngine;

namespace PrimitivesPro.MeshCutting
{
	// Token: 0x02000861 RID: 2145
	public class Contour
	{
		// Token: 0x06004290 RID: 17040 RVA: 0x0015228B File Offset: 0x0015068B
		public Contour(int trianglesNum)
		{
			this.AllocateBuffers(trianglesNum);
		}

		// Token: 0x06004291 RID: 17041 RVA: 0x0015229C File Offset: 0x0015069C
		public void AllocateBuffers(int trianglesNum)
		{
			if (this.lsHash == null)
			{
				this.midPoints = new ArrayDictionary<Contour.MidPoint>(trianglesNum * 2);
				this.contour = new List<Dictionary<int, int>>();
				this.lsHash = new LSHash(0.001f, trianglesNum * 2);
			}
			else
			{
				this.lsHash.Clear();
				foreach (Dictionary<int, int> dictionary in this.contour)
				{
					dictionary.Clear();
				}
				this.contour.Clear();
				if (this.midPoints.Size < trianglesNum * 2)
				{
					this.midPoints = new ArrayDictionary<Contour.MidPoint>(trianglesNum * 2);
				}
				else
				{
					this.midPoints.Clear();
				}
			}
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06004292 RID: 17042 RVA: 0x0015237C File Offset: 0x0015077C
		// (set) Token: 0x06004293 RID: 17043 RVA: 0x00152384 File Offset: 0x00150784
		public int MidPointsCount { get; private set; }

		// Token: 0x06004294 RID: 17044 RVA: 0x00152390 File Offset: 0x00150790
		public void AddTriangle(int triangleID, int id0, int id1, Vector3 v0, Vector3 v1)
		{
			int num;
			int num2;
			this.lsHash.Hash(v0, v1, out num, out num2);
			if (num == num2)
			{
				return;
			}
			Contour.MidPoint value;
			if (this.midPoints.TryGetValue(num, out value))
			{
				if (value.idNext == 2147483647 && value.idPrev != num2)
				{
					value.idNext = num2;
				}
				else if (value.idPrev == 2147483647 && value.idNext != num2)
				{
					value.idPrev = num2;
				}
				this.midPoints[num] = value;
			}
			else
			{
				this.midPoints.Add(num, new Contour.MidPoint
				{
					id = num,
					vertexId = id0,
					idNext = num2,
					idPrev = int.MaxValue
				});
			}
			if (this.midPoints.TryGetValue(num2, out value))
			{
				if (value.idNext == 2147483647 && value.idPrev != num)
				{
					value.idNext = num;
				}
				else if (value.idPrev == 2147483647 && value.idNext != num)
				{
					value.idPrev = num;
				}
				this.midPoints[num2] = value;
			}
			else
			{
				this.midPoints.Add(num2, new Contour.MidPoint
				{
					id = num2,
					vertexId = id1,
					idPrev = num,
					idNext = int.MaxValue
				});
			}
			this.MidPointsCount = this.midPoints.Count;
		}

		// Token: 0x06004295 RID: 17045 RVA: 0x00152528 File Offset: 0x00150928
		public bool FindContours()
		{
			if (this.midPoints.Count == 0)
			{
				return false;
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>(this.midPoints.Count);
			int num = 10000;
			Contour.MidPoint firstValue = this.midPoints.GetFirstValue();
			dictionary.Add(firstValue.id, firstValue.vertexId);
			this.midPoints.Remove(firstValue.id);
			int num2 = firstValue.idNext;
			while (this.midPoints.Count > 0)
			{
				if (num2 == 2147483647)
				{
					return false;
				}
				Contour.MidPoint midPoint;
				if (!this.midPoints.TryGetValue(num2, out midPoint))
				{
					return false;
				}
				dictionary.Add(midPoint.id, midPoint.vertexId);
				this.midPoints.Remove(midPoint.id);
				if (dictionary.ContainsKey(midPoint.idNext))
				{
					if (dictionary.ContainsKey(midPoint.idPrev))
					{
						this.contour.Add(new Dictionary<int, int>(dictionary));
						dictionary.Clear();
						if (this.midPoints.Count == 0)
						{
							break;
						}
						firstValue = this.midPoints.GetFirstValue();
						dictionary.Add(firstValue.id, firstValue.vertexId);
						this.midPoints.Remove(firstValue.id);
						num2 = firstValue.idNext;
						continue;
					}
					else
					{
						num2 = midPoint.idPrev;
					}
				}
				else
				{
					num2 = midPoint.idNext;
				}
				num--;
				if (num == 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04002B36 RID: 11062
		public List<Dictionary<int, int>> contour;

		// Token: 0x04002B37 RID: 11063
		private ArrayDictionary<Contour.MidPoint> midPoints;

		// Token: 0x04002B38 RID: 11064
		private LSHash lsHash;

		// Token: 0x02000862 RID: 2146
		private struct MidPoint
		{
			// Token: 0x04002B3A RID: 11066
			public int id;

			// Token: 0x04002B3B RID: 11067
			public int vertexId;

			// Token: 0x04002B3C RID: 11068
			public int idNext;

			// Token: 0x04002B3D RID: 11069
			public int idPrev;
		}
	}
}
