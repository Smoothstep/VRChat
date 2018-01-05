using System;
using System.Collections.Generic;
using PrimitivesPro.Utils;
using UnityEngine;

namespace PrimitivesPro.MeshEditor
{
	// Token: 0x02000868 RID: 2152
	public class PPMesh
	{
		// Token: 0x060042B6 RID: 17078 RVA: 0x0015595C File Offset: 0x00153D5C
		public PPMesh(MeshFilter meshFilter, Transform transform)
		{
			if (meshFilter.sharedMesh.name.Contains(PPMesh.meshName))
			{
				this.FromUnityMesh(meshFilter.sharedMesh, transform);
			}
			else
			{
				Mesh mesh = UnityEngine.Object.Instantiate<Mesh>(meshFilter.sharedMesh);
				Mesh mesh2 = mesh;
				mesh2.name += "[pp_mesh_edit]";
				meshFilter.mesh = mesh;
				this.FromUnityMesh(mesh, transform);
			}
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x060042B7 RID: 17079 RVA: 0x001559CC File Offset: 0x00153DCC
		// (set) Token: 0x060042B8 RID: 17080 RVA: 0x001559D4 File Offset: 0x00153DD4
		public Transform Transform { get; private set; }

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x060042B9 RID: 17081 RVA: 0x001559DD File Offset: 0x00153DDD
		// (set) Token: 0x060042BA RID: 17082 RVA: 0x001559E5 File Offset: 0x00153DE5
		public Mesh OriginalMesh { get; private set; }

		// Token: 0x060042BB RID: 17083 RVA: 0x001559EE File Offset: 0x00153DEE
		public void FromUnityMesh(Mesh mesh, Transform transform)
		{
			if (!mesh.isReadable)
			{
				Debug.LogWarning("Unable to edit mesh, mesh is not readable!");
			}
			this.Transform = transform;
			this.OriginalMesh = mesh;
			this.Populate(mesh);
			this.undoSystem = new UndoSystem();
		}

		// Token: 0x060042BC RID: 17084 RVA: 0x00155A28 File Offset: 0x00153E28
		public void ApplyToMesh()
		{
			this.OriginalMesh.vertices = this.vertices;
			int[] array = new int[this.faces.Count * 3];
			int num = 0;
			foreach (PPMesh.Face face in this.faces)
			{
				array[num] = face.p0;
				array[num + 1] = face.p1;
				array[num + 2] = face.p2;
				num += 3;
			}
			this.OriginalMesh.triangles = array;
			this.OriginalMesh.RecalculateNormals();
			MeshUtils.CalculateTangents(this.OriginalMesh);
			this.OriginalMesh.RecalculateBounds();
		}

		// Token: 0x060042BD RID: 17085 RVA: 0x00155AF4 File Offset: 0x00153EF4
		public Mesh ToUnityMesh()
		{
			Mesh mesh = new Mesh
			{
				vertices = this.vertices,
				normals = this.normals,
				uv = this.uvs
			};
			int[] array = new int[this.faces.Count];
			int num = 0;
			foreach (PPMesh.Face face in this.faces)
			{
				array[num] = face.p0;
				array[num + 1] = face.p1;
				array[num + 2] = face.p2;
				num += 3;
			}
			mesh.triangles = array;
			return mesh;
		}

		// Token: 0x060042BE RID: 17086 RVA: 0x00155BB8 File Offset: 0x00153FB8
		public void OnUndo()
		{
			Dictionary<int, Vector3> dictionary = this.undoSystem.UndoVertex();
			if (dictionary != null)
			{
				foreach (KeyValuePair<int, Vector3> keyValuePair in dictionary)
				{
					this.vertices[keyValuePair.Key] -= keyValuePair.Value;
				}
			}
			this.ApplyToMesh();
		}

		// Token: 0x060042BF RID: 17087 RVA: 0x00155C4C File Offset: 0x0015404C
		public void OnRedo()
		{
			Dictionary<int, Vector3> dictionary = this.undoSystem.RedoVertex();
			if (dictionary != null)
			{
				foreach (KeyValuePair<int, Vector3> keyValuePair in dictionary)
				{
					this.vertices[keyValuePair.Key] += keyValuePair.Value;
				}
			}
			this.ApplyToMesh();
		}

		// Token: 0x060042C0 RID: 17088 RVA: 0x00155CE0 File Offset: 0x001540E0
		public bool CanUndo()
		{
			return this.undoSystem.CanUndo();
		}

		// Token: 0x060042C1 RID: 17089 RVA: 0x00155CED File Offset: 0x001540ED
		public bool CanRedo()
		{
			return this.undoSystem.CanRedo();
		}

		// Token: 0x060042C2 RID: 17090 RVA: 0x00155CFA File Offset: 0x001540FA
		public void SaveUndo()
		{
			this.undoSystem.CreateRestorePoint();
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x00155D08 File Offset: 0x00154108
		public void StartUpdateVerticesDelta(HashSet<int> verts, bool includeOverlaps)
		{
			this.verticesDelta.Clear();
			foreach (int num in verts)
			{
				Vector3 b = this.vertices[num];
				if (includeOverlaps)
				{
					for (int i = 0; i < this.vertices.Length; i++)
					{
						if ((this.vertices[i] - b).sqrMagnitude < 0.0001f)
						{
							this.verticesDelta.Add(i);
						}
					}
				}
				else
				{
					this.verticesDelta.Add(num);
				}
			}
		}

		// Token: 0x060042C4 RID: 17092 RVA: 0x00155DE0 File Offset: 0x001541E0
		public void UpdateVerticesDelta(Vector3 delta)
		{
			foreach (int num in this.verticesDelta)
			{
				this.vertices[num] += delta;
				this.undoSystem.OnVertexChanged(num, delta);
			}
		}

		// Token: 0x060042C5 RID: 17093 RVA: 0x00155E60 File Offset: 0x00154260
		public void StartUpdateFacesDelta(HashSet<PPMesh.Face> changeFaces, bool includeOverlaps)
		{
			this.verticesDelta.Clear();
			foreach (PPMesh.Face face in changeFaces)
			{
				this.verticesDelta.Add(face.p0);
				this.verticesDelta.Add(face.p1);
				this.verticesDelta.Add(face.p2);
			}
			if (includeOverlaps)
			{
				HashSet<int> hashSet = new HashSet<int>();
				foreach (int num in this.verticesDelta)
				{
					for (int i = 0; i < this.vertices.Length; i++)
					{
						Vector3 a = this.vertices[i];
						if ((a - this.vertices[num]).sqrMagnitude < 0.0001f)
						{
							hashSet.Add(i);
						}
					}
				}
				this.verticesDelta.UnionWith(hashSet);
			}
		}

		// Token: 0x060042C6 RID: 17094 RVA: 0x00155FB4 File Offset: 0x001543B4
		public void UpdateFacesDelta(Vector3 delta)
		{
			foreach (int num in this.verticesDelta)
			{
				this.vertices[num] += delta;
				this.undoSystem.OnVertexChanged(num, delta);
			}
		}

		// Token: 0x060042C7 RID: 17095 RVA: 0x00156034 File Offset: 0x00154434
		public void StartUpdateEdgesDelta(HashSet<PPMesh.HalfEdge> changeEdges, bool includeOverlaps)
		{
			this.verticesDelta.Clear();
			foreach (PPMesh.HalfEdge halfEdge in changeEdges)
			{
				this.verticesDelta.Add(halfEdge.point);
				this.verticesDelta.Add(halfEdge.nextEdge.point);
			}
			if (includeOverlaps)
			{
				HashSet<int> hashSet = new HashSet<int>();
				foreach (int num in this.verticesDelta)
				{
					for (int i = 0; i < this.vertices.Length; i++)
					{
						Vector3 a = this.vertices[i];
						if ((a - this.vertices[num]).sqrMagnitude < 0.0001f)
						{
							hashSet.Add(i);
						}
					}
				}
                verticesDelta.UnionWith(hashSet);
			}
		}

		// Token: 0x060042C8 RID: 17096 RVA: 0x0015617C File Offset: 0x0015457C
		public void UpdateEdgesDelta(Vector3 delta)
		{
			foreach (int num in this.verticesDelta)
			{
				this.vertices[num] += delta;
				this.undoSystem.OnVertexChanged(num, delta);
			}
		}

		// Token: 0x060042C9 RID: 17097 RVA: 0x001561FC File Offset: 0x001545FC
		private void Populate(Mesh mesh)
		{
			this.edges = new Dictionary<long, PPMesh.HalfEdge>(mesh.triangles.Length);
			this.faces = new HashSet<PPMesh.Face>();
			this.vertexFaces = new PPMesh.Face[mesh.vertexCount];
			this.vertices = new Vector3[mesh.vertexCount];
			this.normals = new Vector3[mesh.vertexCount];
			this.uvs = new Vector2[mesh.vertexCount];
			this.verticesDelta = new HashSet<int>();
			Array.Copy(mesh.vertices, this.vertices, mesh.vertexCount);
			Array.Copy(mesh.normals, this.normals, mesh.vertexCount);
			Array.Copy(mesh.uv, this.uvs, mesh.vertexCount);
			int[] triangles = mesh.triangles;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				long key = this.HashEdge(triangles[i], triangles[i + 1]);
				long key2 = this.HashEdge(triangles[i + 1], triangles[i + 2]);
				long key3 = this.HashEdge(triangles[i + 2], triangles[i]);
				PPMesh.Face face = new PPMesh.Face(triangles[i], triangles[i + 1], triangles[i + 2]);
				PPMesh.HalfEdge halfEdge = new PPMesh.HalfEdge
				{
					face = face,
					point = triangles[i]
				};
				PPMesh.HalfEdge halfEdge2 = new PPMesh.HalfEdge
				{
					face = face,
					point = triangles[i + 1]
				};
				PPMesh.HalfEdge halfEdge3 = new PPMesh.HalfEdge
				{
					face = face,
					point = triangles[i + 2]
				};
				this.vertexFaces[triangles[i]] = face;
				this.vertexFaces[triangles[i + 1]] = face;
				this.vertexFaces[triangles[i + 2]] = face;
				try
				{
					halfEdge.nextEdge = halfEdge2;
					halfEdge2.nextEdge = halfEdge3;
					halfEdge3.nextEdge = halfEdge;
					this.edges.Add(key, halfEdge);
					this.edges.Add(key2, halfEdge2);
					this.edges.Add(key3, halfEdge3);
					this.faces.Add(face);
				}
				catch
				{
				}
			}
			foreach (PPMesh.HalfEdge halfEdge4 in this.edges.Values)
			{
				int point = halfEdge4.point;
				int point2 = halfEdge4.nextEdge.point;
				if (halfEdge4.oppositeEdge == null)
				{
					long key4 = this.HashEdge(point2, point);
					PPMesh.HalfEdge halfEdge5;
					if (this.edges.TryGetValue(key4, out halfEdge5))
					{
						halfEdge4.oppositeEdge = halfEdge5;
						halfEdge5.oppositeEdge = halfEdge4;
					}
				}
			}
		}

		// Token: 0x060042CA RID: 17098 RVA: 0x001564B0 File Offset: 0x001548B0
		private long HashEdge(int v0, int v1)
		{
			return (long)((ulong)v0 | (ulong)((ulong)((long)v1) << 32));
		}

		// Token: 0x060042CB RID: 17099 RVA: 0x001564BC File Offset: 0x001548BC
		public float GetNearestPoint(Ray ray, out int index, bool excludeBackSidePolygons)
		{
			Vector3 origin = this.Transform.InverseTransformPoint(ray.origin);
			Vector3 direction = this.Transform.InverseTransformDirection(ray.direction);
			Ray ray2 = new Ray(origin, direction);
			float num = float.MaxValue;
			int num2 = -1;
			for (int i = 0; i < this.vertices.Length; i++)
			{
				Vector3 point = this.vertices[i];
				float num3 = MeshUtils.DistanceToLine2(ray2, point);
				if (num3 < num)
				{
					if (excludeBackSidePolygons)
					{
						PPMesh.Face face = this.vertexFaces[i];
						if (face != null && !PrimitivesPro.Utils.Plane.GetSide(this.vertices[face.p0], this.vertices[face.p1], this.vertices[face.p2], ray2.origin))
						{
							goto IL_E0;
						}
					}
					num = num3;
					num2 = i;
				}
				IL_E0:;
			}
			index = num2;
			return num;
		}

		// Token: 0x060042CC RID: 17100 RVA: 0x001565C4 File Offset: 0x001549C4
		public PPMesh.Face GetIntersectingPolygon(Ray ray, bool excludeBackSidePolygons)
		{
			Vector3 origin = this.Transform.InverseTransformPoint(ray.origin);
			Vector3 direction = this.Transform.InverseTransformDirection(ray.direction);
			Ray ray2 = new Ray(origin, direction);
			PPMesh.Face result = null;
			float num = float.MaxValue;
			foreach (PPMesh.Face face in this.faces)
			{
				Vector3 vector = this.vertices[face.p0];
				Vector3 vector2 = this.vertices[face.p1];
				Vector3 vector3 = this.vertices[face.p2];
				float num2;
				if (MeshUtils.RayTriangleIntersection(vector, vector2, vector3, ray2.origin, ray2.direction, out num2) && num2 < num)
				{
					if (!excludeBackSidePolygons || PrimitivesPro.Utils.Plane.GetSide(vector, vector2, vector3, ray2.origin))
					{
						num = num2;
						result = face;
					}
				}
			}
			return result;
		}

		// Token: 0x060042CD RID: 17101 RVA: 0x001566F4 File Offset: 0x00154AF4
		public float GetNearestEdge(Ray ray, out PPMesh.HalfEdge minEdge, bool excludeBackSidePolygons)
		{
			Vector3 origin = this.Transform.InverseTransformPoint(ray.origin);
			Vector3 direction = this.Transform.InverseTransformDirection(ray.direction);
			Ray ray2 = new Ray(origin, direction);
			float num = float.MaxValue;
			minEdge = null;
			foreach (PPMesh.HalfEdge halfEdge in this.edges.Values)
			{
				Vector3 vector = this.vertices[halfEdge.point];
				Vector3 vector2 = this.vertices[halfEdge.nextEdge.point];
				float num2 = MeshUtils.SegmentSegmentDistance2(vector, vector2, ray2.origin, ray2.origin + ray2.direction * 10000f);
				if (num2 < num)
				{
					if (excludeBackSidePolygons)
					{
						Vector3 c = this.vertices[halfEdge.nextEdge.nextEdge.point];
						if (!PrimitivesPro.Utils.Plane.GetSide(vector, vector2, c, ray2.origin))
						{
							continue;
						}
					}
					num = num2;
					minEdge = halfEdge;
				}
			}
			return num;
		}

		// Token: 0x060042CE RID: 17102 RVA: 0x00156844 File Offset: 0x00154C44
		public Vector3 GetVertexByIndexWorld(int index)
		{
			return this.Transform.TransformPoint(this.vertices[index]);
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x00156864 File Offset: 0x00154C64
		public Vector3[] GetFaceRectangle(PPMesh.Face face)
		{
			Vector3 vector = this.Transform.TransformPoint(this.vertices[face.p0]);
			Vector3 vector2 = this.Transform.TransformPoint(this.vertices[face.p1]);
			Vector3 vector3 = this.Transform.TransformPoint(this.vertices[face.p2]);
			return new Vector3[]
			{
				vector,
				vector2,
				vector3,
				vector3
			};
		}

		// Token: 0x060042D0 RID: 17104 RVA: 0x00156914 File Offset: 0x00154D14
		public Vector3[] GetEdgeLine(PPMesh.HalfEdge edge)
		{
			Vector3 vector = this.Transform.TransformPoint(this.vertices[edge.point]);
			Vector3 vector2 = this.Transform.TransformPoint(this.vertices[edge.nextEdge.point]);
			return new Vector3[]
			{
				vector,
				vector2
			};
		}

		// Token: 0x060042D1 RID: 17105 RVA: 0x0015698C File Offset: 0x00154D8C
		public Vector3 GetAveragePos(int[] list)
		{
			Vector3 a = Vector3.zero;
			foreach (int num in list)
			{
				a += this.Transform.TransformPoint(this.vertices[num]);
			}
			return a / (float)list.Length;
		}

		// Token: 0x060042D2 RID: 17106 RVA: 0x001569E8 File Offset: 0x00154DE8
		public Quaternion GetLocalRotation(HashSet<PPMesh.Face> faceSet)
		{
			Vector3 a = Vector3.zero;
			foreach (PPMesh.Face face in faceSet)
			{
				a += this.GetFaceNormalWorld(face);
			}
			return Quaternion.LookRotation(a.normalized);
		}

		// Token: 0x060042D3 RID: 17107 RVA: 0x00156A58 File Offset: 0x00154E58
		public Quaternion GetLocalRotation(HashSet<int> vertexSet)
		{
			Vector3 a = Vector3.zero;
			foreach (int num in vertexSet)
			{
				a += this.Transform.TransformDirection(this.normals[num]);
			}
			return Quaternion.LookRotation(a.normalized);
		}

		// Token: 0x060042D4 RID: 17108 RVA: 0x00156AE0 File Offset: 0x00154EE0
		public Quaternion GetLocalRotation(HashSet<PPMesh.HalfEdge> edgeSet)
		{
			Vector3 a = Vector3.zero;
			foreach (PPMesh.HalfEdge halfEdge in edgeSet)
			{
				a += this.GetFaceNormalWorld(halfEdge.face);
				if (halfEdge.oppositeEdge != null)
				{
					a += this.GetFaceNormalWorld(halfEdge.oppositeEdge.face);
				}
			}
			return Quaternion.LookRotation(a.normalized);
		}

		// Token: 0x060042D5 RID: 17109 RVA: 0x00156B78 File Offset: 0x00154F78
		public Vector3 GetFaceNormalWorld(PPMesh.Face face)
		{
			return MeshUtils.ComputePolygonNormal(this.Transform.TransformPoint(this.vertices[face.p0]), this.Transform.TransformPoint(this.vertices[face.p1]), this.Transform.TransformPoint(this.vertices[face.p2]));
		}

		// Token: 0x060042D6 RID: 17110 RVA: 0x00156BF0 File Offset: 0x00154FF0
		public HashSet<int> GetPointsInRect2D(Rect rect)
		{
			return new HashSet<int>();
		}

		// Token: 0x060042D7 RID: 17111 RVA: 0x00156C04 File Offset: 0x00155004
		public HashSet<PPMesh.Face> GetPolygonsInRect2D(Rect rect)
		{
			HashSet<PPMesh.Face> hashSet = new HashSet<PPMesh.Face>();
			HashSet<int> pointsInRect2D = this.GetPointsInRect2D(rect);
			foreach (PPMesh.Face face in this.faces)
			{
				if (pointsInRect2D.Contains(face.p0) && pointsInRect2D.Contains(face.p1) && pointsInRect2D.Contains(face.p2))
				{
					hashSet.Add(face);
				}
			}
			return hashSet;
		}

		// Token: 0x060042D8 RID: 17112 RVA: 0x00156CA4 File Offset: 0x001550A4
		public HashSet<PPMesh.HalfEdge> GetEdgesInRect2D(Rect rect)
		{
			HashSet<PPMesh.HalfEdge> hashSet = new HashSet<PPMesh.HalfEdge>();
			HashSet<int> pointsInRect2D = this.GetPointsInRect2D(rect);
			foreach (PPMesh.HalfEdge halfEdge in this.edges.Values)
			{
				if (pointsInRect2D.Contains(halfEdge.point) && pointsInRect2D.Contains(halfEdge.nextEdge.point))
				{
					hashSet.Add(halfEdge);
				}
			}
			return hashSet;
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x00156D3C File Offset: 0x0015513C
		public bool FindClosestPoint(Vector3 p, HashSet<int> exclude, float tolerance2, ref Vector3 ret)
		{
			int num = -1;
			float num2 = float.MaxValue;
			Vector3 b = this.Transform.InverseTransformPoint(p);
			for (int i = 0; i < this.vertices.Length; i++)
			{
				if (!exclude.Contains(i))
				{
					Vector3 a = this.vertices[i];
					float sqrMagnitude = (a - b).sqrMagnitude;
					if (sqrMagnitude < tolerance2 && sqrMagnitude < num2)
					{
						num = i;
						num2 = sqrMagnitude;
					}
				}
			}
			if (num != -1)
			{
				ret = this.Transform.TransformPoint(this.vertices[num]);
				return true;
			}
			return false;
		}

		// Token: 0x060042DA RID: 17114 RVA: 0x00156DF0 File Offset: 0x001551F0
		public void AddOverlappingVertices(HashSet<int> verts, int vertex)
		{
			if (vertex != -1)
			{
				Vector3 b = this.vertices[vertex];
				for (int i = 0; i < this.vertices.Length; i++)
				{
					if (i != vertex)
					{
						if ((this.vertices[i] - b).sqrMagnitude < 0.0001f)
						{
							verts.Add(i);
						}
					}
				}
			}
		}

		// Token: 0x04002B5C RID: 11100
		private Dictionary<long, PPMesh.HalfEdge> edges;

		// Token: 0x04002B5D RID: 11101
		private HashSet<PPMesh.Face> faces;

		// Token: 0x04002B5E RID: 11102
		private PPMesh.Face[] vertexFaces;

		// Token: 0x04002B5F RID: 11103
		private Vector3[] vertices;

		// Token: 0x04002B60 RID: 11104
		private Vector3[] normals;

		// Token: 0x04002B61 RID: 11105
		private Vector2[] uvs;

		// Token: 0x04002B62 RID: 11106
		private HashSet<int> verticesDelta;

		// Token: 0x04002B63 RID: 11107
		private static string meshName = "[pp_mesh_edit]";

		// Token: 0x04002B64 RID: 11108
		private UndoSystem undoSystem;

		// Token: 0x04002B65 RID: 11109
		private const float overlapEpsylon = 0.0001f;

		// Token: 0x02000869 RID: 2153
		public class HalfEdge
		{
			// Token: 0x04002B66 RID: 11110
			public PPMesh.HalfEdge oppositeEdge;

			// Token: 0x04002B67 RID: 11111
			public PPMesh.HalfEdge nextEdge;

			// Token: 0x04002B68 RID: 11112
			public PPMesh.Face face;

			// Token: 0x04002B69 RID: 11113
			public int point;
		}

		// Token: 0x0200086A RID: 2154
		public class Face
		{
			// Token: 0x060042DD RID: 17117 RVA: 0x00156E80 File Offset: 0x00155280
			public Face(int p0, int p1, int p2)
			{
				this.p0 = p0;
				this.p1 = p1;
				this.p2 = p2;
			}

			// Token: 0x060042DE RID: 17118 RVA: 0x00156EA0 File Offset: 0x001552A0
			public override string ToString()
			{
				return string.Concat(new string[]
				{
					this.p0.ToString(),
					" ",
					this.p1.ToString(),
					" ",
					this.p2.ToString()
				});
			}

			// Token: 0x04002B6A RID: 11114
			public readonly int p0;

			// Token: 0x04002B6B RID: 11115
			public readonly int p1;

			// Token: 0x04002B6C RID: 11116
			public readonly int p2;
		}

		// Token: 0x0200086B RID: 2155
		public struct Vertex
		{
			// Token: 0x060042DF RID: 17119 RVA: 0x00156F0D File Offset: 0x0015530D
			public void Update(Vector3 pos)
			{
				this.point = pos;
			}

			// Token: 0x04002B6D RID: 11117
			public Vector3 point;

			// Token: 0x04002B6E RID: 11118
			public int index;
		}
	}
}
