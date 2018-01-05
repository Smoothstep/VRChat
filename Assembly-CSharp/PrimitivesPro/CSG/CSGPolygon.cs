using System;
using System.Collections.Generic;

namespace PrimitivesPro.CSG
{
	// Token: 0x0200083D RID: 2109
	internal class CSGPolygon
	{
		// Token: 0x06004186 RID: 16774 RVA: 0x0014B11C File Offset: 0x0014951C
		public CSGPolygon(int id, List<CSGVertex> vertices)
		{
			this.Vertices = vertices;
			this.Plane = new CSGPlane(vertices[0].P, vertices[1].P, vertices[2].P);
			this.Id = id;
		}

		// Token: 0x06004187 RID: 16775 RVA: 0x0014B16C File Offset: 0x0014956C
		public CSGPolygon(int id, params CSGVertex[] vertices)
		{
			this.Vertices = new List<CSGVertex>(vertices);
			this.Plane = new CSGPlane(vertices[0].P, vertices[1].P, vertices[2].P);
			this.Id = id;
		}

		// Token: 0x06004188 RID: 16776 RVA: 0x0014B1AC File Offset: 0x001495AC
		public CSGPolygon(CSGPolygon instance)
		{
			this.Vertices = new List<CSGVertex>(instance.Vertices.Count);
			foreach (CSGVertex v in instance.Vertices)
			{
				this.Vertices.Add(new CSGVertex(v));
			}
			this.Plane = new CSGPlane(instance.Plane);
			this.Id = instance.Id;
		}

		// Token: 0x06004189 RID: 16777 RVA: 0x0014B24C File Offset: 0x0014964C
		public void Flip()
		{
			this.Vertices.Reverse();
			foreach (CSGVertex csgvertex in this.Vertices)
			{
				csgvertex.Flip();
			}
		}

		// Token: 0x04002A7D RID: 10877
		public List<CSGVertex> Vertices;

		// Token: 0x04002A7E RID: 10878
		public CSGPlane Plane;

		// Token: 0x04002A7F RID: 10879
		public int Id;
	}
}
