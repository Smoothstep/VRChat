using System;
using System.Collections.Generic;
using PrimitivesPro.Utils;
using UnityEngine;

namespace PrimitivesPro.CSG
{
	// Token: 0x0200083B RID: 2107
	internal class CSGPlane
	{
		// Token: 0x06004180 RID: 16768 RVA: 0x0014AD98 File Offset: 0x00149198
		public CSGPlane(Vector3 a, Vector3 b, Vector3 c)
		{
			this.Normal = Vector3.Cross(b - a, c - a).normalized;
			this.Distance = Vector3.Dot(this.Normal, a);
		}

		// Token: 0x06004181 RID: 16769 RVA: 0x0014ADDE File Offset: 0x001491DE
		public CSGPlane(Vector3 normal, Vector3 p)
		{
			this.Normal = normal.normalized;
			this.Distance = Vector3.Dot(this.Normal, p);
		}

		// Token: 0x06004182 RID: 16770 RVA: 0x0014AE05 File Offset: 0x00149205
		public CSGPlane(CSGPlane instance)
		{
			this.Normal = instance.Normal;
			this.Distance = instance.Distance;
		}

		// Token: 0x06004183 RID: 16771 RVA: 0x0014AE28 File Offset: 0x00149228
		public PrimitivesPro.Utils.Plane.PointClass ClassifyPoint(Vector3 p)
		{
			float num = Vector3.Dot(p, this.Normal) - this.Distance;
			return (num >= -1E-06f) ? ((num <= 1E-06f) ? PrimitivesPro.Utils.Plane.PointClass.Coplanar : PrimitivesPro.Utils.Plane.PointClass.Front) : PrimitivesPro.Utils.Plane.PointClass.Back;
		}

		// Token: 0x06004184 RID: 16772 RVA: 0x0014AE6C File Offset: 0x0014926C
		public void Flip()
		{
			this.Normal = -this.Normal;
			this.Distance = -this.Distance;
		}

		// Token: 0x06004185 RID: 16773 RVA: 0x0014AE8C File Offset: 0x0014928C
		public void Split(CSGPolygon polygon, List<CSGPolygon> coplanarFront, List<CSGPolygon> coplanarBack, List<CSGPolygon> front, List<CSGPolygon> back)
		{
			int count = polygon.Vertices.Count;
			PrimitivesPro.Utils.Plane.PointClass[] array = new PrimitivesPro.Utils.Plane.PointClass[count];
			PrimitivesPro.Utils.Plane.PointClass pointClass = PrimitivesPro.Utils.Plane.PointClass.Coplanar;
			for (int i = 0; i < count; i++)
			{
				float num = Vector3.Dot(this.Normal, polygon.Vertices[i].P) - this.Distance;
				PrimitivesPro.Utils.Plane.PointClass pointClass2 = (num >= -1E-06f) ? ((num <= 1E-06f) ? PrimitivesPro.Utils.Plane.PointClass.Coplanar : PrimitivesPro.Utils.Plane.PointClass.Front) : PrimitivesPro.Utils.Plane.PointClass.Back;
				pointClass |= pointClass2;
				array[i] = pointClass2;
			}
			switch (pointClass)
			{
			case PrimitivesPro.Utils.Plane.PointClass.Coplanar:
				if (Vector3.Dot(this.Normal, polygon.Plane.Normal) > 0f)
				{
					coplanarFront.Add(polygon);
				}
				else
				{
					coplanarBack.Add(polygon);
				}
				break;
			case PrimitivesPro.Utils.Plane.PointClass.Front:
				front.Add(polygon);
				break;
			case PrimitivesPro.Utils.Plane.PointClass.Back:
				back.Add(polygon);
				break;
			case PrimitivesPro.Utils.Plane.PointClass.Intersection:
			{
				List<CSGVertex> list = new List<CSGVertex>(4);
				List<CSGVertex> list2 = new List<CSGVertex>(4);
				for (int j = 0; j < count; j++)
				{
					int num2 = (j + 1) % count;
					PrimitivesPro.Utils.Plane.PointClass pointClass3 = array[j];
					PrimitivesPro.Utils.Plane.PointClass pointClass4 = array[num2];
					CSGVertex csgvertex = polygon.Vertices[j];
					CSGVertex csgvertex2 = polygon.Vertices[num2];
					if (pointClass3 != PrimitivesPro.Utils.Plane.PointClass.Back)
					{
						list.Add(csgvertex);
					}
					if (pointClass3 != PrimitivesPro.Utils.Plane.PointClass.Front)
					{
						list2.Add((pointClass3 == PrimitivesPro.Utils.Plane.PointClass.Back) ? csgvertex : new CSGVertex(csgvertex));
					}
					if ((pointClass3 | pointClass4) == PrimitivesPro.Utils.Plane.PointClass.Intersection)
					{
						CSGVertex csgvertex3 = new CSGVertex(polygon.Vertices[0]);
						float t = (this.Distance - Vector3.Dot(this.Normal, csgvertex.P)) / Vector3.Dot(this.Normal, csgvertex2.P - csgvertex.P);
						csgvertex3.P = Vector3.Lerp(csgvertex.P, csgvertex2.P, t);
						csgvertex3.N = Vector3.Lerp(csgvertex.N, csgvertex2.N, t);
						csgvertex3.UV = Vector2.Lerp(csgvertex.UV, csgvertex2.UV, t);
						list.Add(csgvertex3);
						list2.Add(new CSGVertex(csgvertex3));
					}
				}
				if (list.Count >= 3)
				{
					front.Add(new CSGPolygon(polygon.Id, list));
				}
				if (list2.Count >= 3)
				{
					back.Add(new CSGPolygon(polygon.Id, list2));
				}
				break;
			}
			}
		}

		// Token: 0x04002A75 RID: 10869
		private const float epsylon = 1E-06f;

		// Token: 0x04002A76 RID: 10870
		public Vector3 Normal;

		// Token: 0x04002A77 RID: 10871
		public float Distance;

		// Token: 0x0200083C RID: 2108
		[Flags]
		public enum PointClass
		{
			// Token: 0x04002A79 RID: 10873
			Coplanar = 0,
			// Token: 0x04002A7A RID: 10874
			Front = 1,
			// Token: 0x04002A7B RID: 10875
			Back = 2,
			// Token: 0x04002A7C RID: 10876
			Intersection = 3
		}
	}
}
