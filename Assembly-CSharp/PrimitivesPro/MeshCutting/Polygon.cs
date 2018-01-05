using System;
using System.Collections.Generic;
using PrimitivesPro.ThirdParty.P2T;
using UnityEngine;

namespace PrimitivesPro.MeshCutting
{
	// Token: 0x02000867 RID: 2151
	public class Polygon
	{
		// Token: 0x060042B0 RID: 17072 RVA: 0x001553F4 File Offset: 0x001537F4
		public Polygon(Vector2[] pnts)
		{
			this.Points = pnts;
			this.Area = this.GetArea();
			this.holes = new List<Polygon>();
		}

		// Token: 0x060042B1 RID: 17073 RVA: 0x0015541C File Offset: 0x0015381C
		public float GetArea()
		{
			this.Min.x = float.MaxValue;
			this.Min.y = float.MaxValue;
			this.Max.x = float.MinValue;
			this.Max.y = float.MinValue;
			int num = this.Points.Length;
			float num2 = 0f;
			int num3 = num - 1;
			int i = 0;
			while (i < num)
			{
				Vector2 vector = this.Points[num3];
				Vector2 vector2 = this.Points[i];
				num2 += vector.x * vector2.y - vector2.x * vector.y;
				if (vector.x < this.Min.x)
				{
					this.Min.x = vector.x;
				}
				if (vector.y < this.Min.y)
				{
					this.Min.y = vector.y;
				}
				if (vector.x > this.Max.x)
				{
					this.Max.x = vector.x;
				}
				if (vector.y > this.Max.y)
				{
					this.Max.y = vector.y;
				}
				num3 = i++;
			}
			return num2 * 0.5f;
		}

		// Token: 0x060042B2 RID: 17074 RVA: 0x00155584 File Offset: 0x00153984
		public bool IsPointInside(Vector3 p)
		{
			int num = this.Points.Length;
			Vector2 vector = this.Points[num - 1];
			bool flag = vector.y >= p.y;
			Vector2 vector2 = Vector2.zero;
			bool flag2 = false;
			for (int i = 0; i < num; i++)
			{
				vector2 = this.Points[i];
				bool flag3 = vector2.y >= p.y;
				if (flag != flag3 && (vector2.y - p.y) * (vector.x - vector2.x) >= (vector2.x - p.x) * (vector.y - vector2.y) == flag3)
				{
					flag2 = !flag2;
				}
				flag = flag3;
				vector = vector2;
			}
			return flag2;
		}

		// Token: 0x060042B3 RID: 17075 RVA: 0x00155669 File Offset: 0x00153A69
		public bool IsPolygonInside(Polygon polygon)
		{
			return this.Area > polygon.Area && this.IsPointInside(polygon.Points[0]);
		}

		// Token: 0x060042B4 RID: 17076 RVA: 0x0015569A File Offset: 0x00153A9A
		public void AddHole(Polygon polygon)
		{
			this.holes.Add(polygon);
		}

		// Token: 0x060042B5 RID: 17077 RVA: 0x001556A8 File Offset: 0x00153AA8
		public List<int> Triangulate()
		{
			List<PolygonPoint> list = new List<PolygonPoint>(this.Points.Length);
			foreach (Vector2 vector in this.Points)
			{
				list.Add(new PolygonPoint((double)vector.x, (double)vector.y));
			}
			ThirdParty.P2T.Polygon polygon = new ThirdParty.P2T.Polygon(list);
			foreach (Polygon polygon2 in this.holes)
			{
				List<PolygonPoint> list2 = new List<PolygonPoint>(polygon2.Points.Length);
				foreach (Vector2 vector2 in polygon2.Points)
				{
					list2.Add(new PolygonPoint((double)vector2.x, (double)vector2.y));
				}
				polygon.AddHole(new ThirdParty.P2T.Polygon(list2));
			}
			P2T.Triangulate(polygon);
			int count = polygon.Triangles.Count;
			List<int> list3 = new List<int>(count * 3);
			this.Points = new Vector2[count * 3];
			int num = 0;
			for (int k = 0; k < count; k++)
			{
				list3.Add(num);
				list3.Add(num + 1);
				list3.Add(num + 2);
				this.Points[num + 2].x = (float)polygon.Triangles[k].Points._0.X;
				this.Points[num + 2].y = (float)polygon.Triangles[k].Points._0.Y;
				this.Points[num + 1].x = (float)polygon.Triangles[k].Points._1.X;
				this.Points[num + 1].y = (float)polygon.Triangles[k].Points._1.Y;
				this.Points[num].x = (float)polygon.Triangles[k].Points._2.X;
				this.Points[num].y = (float)polygon.Triangles[k].Points._2.Y;
				num += 3;
			}
			return list3;
		}

		// Token: 0x04002B55 RID: 11093
		public Vector2[] Points;

		// Token: 0x04002B56 RID: 11094
		public readonly float Area;

		// Token: 0x04002B57 RID: 11095
		public Vector2 Min;

		// Token: 0x04002B58 RID: 11096
		public Vector2 Max;

		// Token: 0x04002B59 RID: 11097
		private readonly List<Polygon> holes;
	}
}
