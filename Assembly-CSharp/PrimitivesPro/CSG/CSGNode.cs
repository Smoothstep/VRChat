using System;
using System.Collections.Generic;

namespace PrimitivesPro.CSG
{
	// Token: 0x0200083A RID: 2106
	internal class CSGNode
	{
		// Token: 0x06004177 RID: 16759 RVA: 0x0014A964 File Offset: 0x00148D64
		public CSGNode()
		{
			this.Polygons = new List<CSGPolygon>();
		}

		// Token: 0x06004178 RID: 16760 RVA: 0x0014A978 File Offset: 0x00148D78
		public CSGNode(CSGNode instance)
		{
			if (this.Plane != null)
			{
				this.Plane = new CSGPlane(instance.Plane);
			}
			if (instance.FrontChild != null)
			{
				this.FrontChild = new CSGNode(instance.FrontChild);
			}
			if (instance.BackChild != null)
			{
				this.BackChild = new CSGNode(instance.BackChild);
			}
			this.Polygons = new List<CSGPolygon>(instance.Polygons.Count);
			foreach (CSGPolygon instance2 in instance.Polygons)
			{
				this.Polygons.Add(new CSGPolygon(instance2));
			}
		}

		// Token: 0x06004179 RID: 16761 RVA: 0x0014AA50 File Offset: 0x00148E50
		public void Invert()
		{
			foreach (CSGPolygon csgpolygon in this.Polygons)
			{
				csgpolygon.Flip();
			}
			this.Plane.Flip();
			if (this.FrontChild != null)
			{
				this.FrontChild.Invert();
			}
			if (this.BackChild != null)
			{
				this.BackChild.Invert();
			}
			MeshUtils.Swap<CSGNode>(ref this.FrontChild, ref this.BackChild);
		}

		// Token: 0x0600417A RID: 16762 RVA: 0x0014AAF4 File Offset: 0x00148EF4
		private List<CSGPolygon> ClipPolygons(List<CSGPolygon> polygons)
		{
			if (this.Plane == null)
			{
				return new List<CSGPolygon>(polygons);
			}
			List<CSGPolygon> list = new List<CSGPolygon>();
			List<CSGPolygon> list2 = new List<CSGPolygon>();
			foreach (CSGPolygon polygon in polygons)
			{
				this.Plane.Split(polygon, list, list2, list, list2);
			}
			if (this.FrontChild != null)
			{
				list = this.FrontChild.ClipPolygons(list);
			}
			if (this.BackChild != null)
			{
				list2 = this.BackChild.ClipPolygons(list2);
			}
			else
			{
				list2.Clear();
			}
			List<CSGPolygon> list3 = new List<CSGPolygon>(list);
			list3.AddRange(list2);
			return list3;
		}

		// Token: 0x0600417B RID: 16763 RVA: 0x0014ABC0 File Offset: 0x00148FC0
		public void ClipTo(CSGNode node)
		{
			this.Polygons = node.ClipPolygons(this.Polygons);
			if (this.FrontChild != null)
			{
				this.FrontChild.ClipTo(node);
			}
			if (this.BackChild != null)
			{
				this.BackChild.ClipTo(node);
			}
		}

		// Token: 0x0600417C RID: 16764 RVA: 0x0014AC10 File Offset: 0x00149010
		public List<CSGPolygon> AllPolygons()
		{
			List<CSGPolygon> list = new List<CSGPolygon>();
			this.GetPolygons(list);
			return list;
		}

		// Token: 0x0600417D RID: 16765 RVA: 0x0014AC2B File Offset: 0x0014902B
		private void GetPolygons(List<CSGPolygon> polygons)
		{
			polygons.AddRange(this.Polygons);
			if (this.BackChild != null)
			{
				this.BackChild.GetPolygons(polygons);
			}
			if (this.FrontChild != null)
			{
				this.FrontChild.GetPolygons(polygons);
			}
		}

		// Token: 0x0600417E RID: 16766 RVA: 0x0014AC67 File Offset: 0x00149067
		public void TestDepth()
		{
			if (this.BackChild != null)
			{
				this.BackChild.TestDepth();
			}
			if (this.FrontChild != null)
			{
				this.FrontChild.TestDepth();
			}
		}

		// Token: 0x0600417F RID: 16767 RVA: 0x0014AC98 File Offset: 0x00149098
		public void Build(List<CSGPolygon> polygons)
		{
			if (polygons.Count == 0)
			{
				return;
			}
			if (this.Plane == null)
			{
				this.Plane = new CSGPlane(polygons[0].Plane);
			}
			List<CSGPolygon> list = new List<CSGPolygon>();
			List<CSGPolygon> list2 = new List<CSGPolygon>();
			foreach (CSGPolygon polygon in polygons)
			{
				this.Plane.Split(polygon, this.Polygons, this.Polygons, list, list2);
			}
			if (list.Count > 0)
			{
				if (this.FrontChild == null)
				{
					this.FrontChild = new CSGNode();
				}
				this.FrontChild.Build(list);
			}
			if (list2.Count > 0)
			{
				if (this.BackChild == null)
				{
					this.BackChild = new CSGNode();
				}
				this.BackChild.Build(list2);
			}
		}

		// Token: 0x04002A71 RID: 10865
		private List<CSGPolygon> Polygons;

		// Token: 0x04002A72 RID: 10866
		private CSGPlane Plane;

		// Token: 0x04002A73 RID: 10867
		private CSGNode FrontChild;

		// Token: 0x04002A74 RID: 10868
		private CSGNode BackChild;
	}
}
