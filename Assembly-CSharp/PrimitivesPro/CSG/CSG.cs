using System;
using System.Collections.Generic;
using UnityEngine;

namespace PrimitivesPro.CSG
{
	// Token: 0x02000839 RID: 2105
	internal class CSG
	{
		// Token: 0x0600416C RID: 16748 RVA: 0x0014A31F File Offset: 0x0014871F
		public CSG()
		{
		}

		// Token: 0x0600416D RID: 16749 RVA: 0x0014A328 File Offset: 0x00148728
		public CSG(CSG instance)
		{
			this.Polygons = new List<CSGPolygon>(instance.Polygons.Count);
			foreach (CSGPolygon instance2 in instance.Polygons)
			{
				this.Polygons.Add(new CSGPolygon(instance2));
			}
			this.root = new CSGNode();
			this.root.Build(this.Polygons);
		}

		// Token: 0x0600416E RID: 16750 RVA: 0x0014A3C8 File Offset: 0x001487C8
		public void Construct(Mesh mesh, Transform meshTransform, int id)
		{
			int num = mesh.triangles.Length;
			int[] triangles = mesh.triangles;
			Vector3[] vertices = mesh.vertices;
			Vector3[] normals = mesh.normals;
			Vector2[] uv = mesh.uv;
			this.Polygons = new List<CSGPolygon>(num / 3);
			for (int i = 0; i < num; i += 3)
			{
				int num2 = triangles[i];
				int num3 = triangles[i + 1];
				int num4 = triangles[i + 2];
				this.Polygons.Add(new CSGPolygon(id, new CSGVertex[]
				{
					new CSGVertex(meshTransform.TransformPoint(vertices[num2]), meshTransform.TransformDirection(normals[num2]), uv[num2]),
					new CSGVertex(meshTransform.TransformPoint(vertices[num3]), meshTransform.TransformDirection(normals[num3]), uv[num3]),
					new CSGVertex(meshTransform.TransformPoint(vertices[num4]), meshTransform.TransformDirection(normals[num4]), uv[num4])
				}));
			}
			this.root = new CSGNode();
			this.root.Build(this.Polygons);
		}

		// Token: 0x0600416F RID: 16751 RVA: 0x0014A51C File Offset: 0x0014891C
		public void TestDepth()
		{
			this.root.TestDepth();
		}

		// Token: 0x06004170 RID: 16752 RVA: 0x0014A52C File Offset: 0x0014892C
		public Mesh ToMesh()
		{
			Mesh mesh = new Mesh();
			List<CSGPolygon> list = this.root.AllPolygons();
			int num = list.Count * 3 * 2;
			List<int> list2 = new List<int>(num);
			List<int> list3 = new List<int>(num);
			List<Vector3> list4 = new List<Vector3>(num);
			List<Vector3> list5 = new List<Vector3>(num);
			List<Vector2> list6 = new List<Vector2>(num);
			Dictionary<int, int> cache = new Dictionary<int, int>(num);
			VertexHash hash = new VertexHash(0.001f, num * 3 * 2);
			int num2 = 0;
			foreach (CSGPolygon csgpolygon in list)
			{
				int count = csgpolygon.Vertices.Count;
				for (int i = 2; i < count; i++)
				{
					int item = this.AddVertex(csgpolygon.Vertices[0], list4, list5, list6, cache, hash);
					int item2 = this.AddVertex(csgpolygon.Vertices[i - 1], list4, list5, list6, cache, hash);
					int item3 = this.AddVertex(csgpolygon.Vertices[i], list4, list5, list6, cache, hash);
					if (csgpolygon.Id == 0)
					{
						list2.Add(item);
						list2.Add(item2);
						list2.Add(item3);
					}
					else
					{
						list3.Add(item);
						list3.Add(item2);
						list3.Add(item3);
					}
					num2 += 3;
				}
			}
			mesh.vertices = list4.ToArray();
			mesh.normals = list5.ToArray();
			mesh.uv = list6.ToArray();
			mesh.subMeshCount = 2;
			mesh.SetTriangles(list2.ToArray(), 0);
			mesh.SetTriangles(list3.ToArray(), 1);
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			return mesh;
		}

		// Token: 0x06004171 RID: 16753 RVA: 0x0014A704 File Offset: 0x00148B04
		private int AddVertex(CSGVertex v0, List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs, Dictionary<int, int> cache, VertexHash hash)
		{
			int key = hash.Hash(v0);
			int result;
			if (cache.TryGetValue(key, out result))
			{
				return result;
			}
			vertices.Add(v0.P);
			normals.Add(v0.N);
			uvs.Add(v0.UV);
			int num = vertices.Count - 1;
			cache.Add(key, num);
			return num;
		}

		// Token: 0x06004172 RID: 16754 RVA: 0x0014A764 File Offset: 0x00148B64
		public CSG Union(CSG csg)
		{
			CSG csg2 = new CSG(this);
			CSG csg3 = new CSG(csg);
			csg2.root.ClipTo(csg3.root);
			csg3.root.ClipTo(csg2.root);
			csg3.root.Invert();
			csg3.root.ClipTo(csg2.root);
			csg3.root.Invert();
			csg2.root.Build(csg3.root.AllPolygons());
			return csg2;
		}

		// Token: 0x06004173 RID: 16755 RVA: 0x0014A7E0 File Offset: 0x00148BE0
		public CSG Substract(CSG csg)
		{
			CSG csg2 = new CSG(this);
			CSG csg3 = new CSG(csg);
			csg2.root.Invert();
			csg2.root.ClipTo(csg3.root);
			csg3.root.ClipTo(csg2.root);
			csg3.root.Invert();
			csg3.root.ClipTo(csg2.root);
			csg3.root.Invert();
			csg2.root.Build(csg3.root.AllPolygons());
			csg2.root.Invert();
			return csg2;
		}

		// Token: 0x06004174 RID: 16756 RVA: 0x0014A874 File Offset: 0x00148C74
		public CSG Intersect(CSG csg)
		{
			CSG csg2 = new CSG(this);
			CSG csg3 = new CSG(csg);
			csg2.root.Invert();
			csg3.root.ClipTo(csg2.root);
			csg3.root.Invert();
			csg2.root.ClipTo(csg3.root);
			csg3.root.ClipTo(csg2.root);
			csg2.root.Build(csg3.root.AllPolygons());
			csg2.root.Invert();
			return csg2;
		}

		// Token: 0x06004175 RID: 16757 RVA: 0x0014A8FA File Offset: 0x00148CFA
		public CSG Test()
		{
			return this;
		}

		// Token: 0x06004176 RID: 16758 RVA: 0x0014A900 File Offset: 0x00148D00
		public CSG Inverse()
		{
			CSG result = new CSG(this);
			foreach (CSGPolygon csgpolygon in this.Polygons)
			{
				csgpolygon.Flip();
			}
			return result;
		}

		// Token: 0x04002A6F RID: 10863
		private CSGNode root;

		// Token: 0x04002A70 RID: 10864
		private List<CSGPolygon> Polygons;
	}
}
