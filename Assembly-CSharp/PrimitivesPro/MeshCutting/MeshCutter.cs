using System;
using System.Collections.Generic;
using System.Diagnostics;
using PrimitivesPro.Utils;
using UnityEngine;

namespace PrimitivesPro.MeshCutting
{
	// Token: 0x02000865 RID: 2149
	public class MeshCutter
	{
		// Token: 0x060042A3 RID: 17059 RVA: 0x00152FF4 File Offset: 0x001513F4
		public float Cut(GameObject obj, PrimitivesPro.Utils.Plane plane, bool triangulateHoles, bool deleteOriginal, Vector4 crossSection, out GameObject cut0, out GameObject cut1, out ContourData intersectionData)
		{
			MeshFilter component = obj.GetComponent<MeshFilter>();
			if (component == null || component.sharedMesh == null)
			{
				cut0 = null;
				cut1 = null;
				intersectionData = null;
				return 0f;
			}
			Mesh sharedMesh = obj.GetComponent<MeshFilter>().sharedMesh;
			Material material = null;
			MeshRenderer component2 = obj.GetComponent<MeshRenderer>();
			if (component2 != null)
			{
				material = component2.sharedMaterial;
			}
			Mesh mesh = null;
			Mesh mesh2 = null;
			cut0 = null;
			cut1 = null;
			intersectionData = null;
			bool flag = obj.transform.localScale == Vector3.one;
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			float result;
			if (flag)
			{
				result = this.Cut(sharedMesh, obj.transform, plane, triangulateHoles, crossSection, out mesh, out mesh2, out zero, out zero2, out intersectionData);
			}
			else
			{
				result = this.Cut(sharedMesh, obj.transform, plane, triangulateHoles, crossSection, out mesh, out mesh2, out intersectionData);
			}
			if (mesh != null)
			{
				GameObject gameObject = new GameObject(obj.name + "_cut0");
				MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
				if (meshFilter != null)
				{
					meshFilter.sharedMesh = mesh;
				}
				MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
				if (meshRenderer != null && material != null)
				{
					meshRenderer.sharedMaterials = new Material[]
					{
						new Material(material),
						new Material(material)
					};
				}
				gameObject.transform.position = obj.transform.position;
				gameObject.transform.rotation = obj.transform.rotation;
				gameObject.transform.localScale = obj.transform.localScale;
				if (flag)
				{
					gameObject.transform.Translate(zero);
				}
				cut0 = gameObject;
			}
			if (mesh2 != null)
			{
				GameObject gameObject2 = new GameObject(obj.name + "_cut1");
				MeshFilter meshFilter2 = gameObject2.AddComponent<MeshFilter>();
				if (meshFilter2 != null)
				{
					meshFilter2.sharedMesh = mesh2;
				}
				MeshRenderer meshRenderer2 = gameObject2.AddComponent<MeshRenderer>();
				if (meshRenderer2 != null && material != null)
				{
					meshRenderer2.sharedMaterials = new Material[]
					{
						new Material(material),
						new Material(material)
					};
				}
				gameObject2.transform.position = obj.transform.position;
				gameObject2.transform.rotation = obj.transform.rotation;
				gameObject2.transform.localScale = obj.transform.localScale;
				if (flag)
				{
					gameObject2.transform.Translate(zero2);
				}
				cut1 = gameObject2;
				if (deleteOriginal)
				{
					UnityEngine.Object.Destroy(obj);
				}
			}
			return result;
		}

		// Token: 0x060042A4 RID: 17060 RVA: 0x001532B4 File Offset: 0x001516B4
		public ContourData GetIntersectionData(GameObject obj, PrimitivesPro.Utils.Plane plane)
		{
			MeshFilter component = obj.GetComponent<MeshFilter>();
			if (component == null || component.sharedMesh == null)
			{
				return null;
			}
			Mesh sharedMesh = obj.GetComponent<MeshFilter>().sharedMesh;
			Mesh mesh = null;
			Mesh mesh2 = null;
			Vector3 vector;
			Vector3 vector2;
			ContourData result;
			this.Cut(sharedMesh, obj.transform, plane, true, false, true, true, this.crossSectionUV, out mesh, out mesh2, out vector, out vector2, out result);
			return result;
		}

		// Token: 0x060042A5 RID: 17061 RVA: 0x0015331C File Offset: 0x0015171C
		public float Cut(Mesh mesh, Transform meshTransform, PrimitivesPro.Utils.Plane plane, bool triangulateHoles, Vector4 crossSection, out Mesh mesh0, out Mesh mesh1)
		{
			Vector3 vector;
			Vector3 vector2;
			ContourData contourData;
			return this.Cut(mesh, meshTransform, plane, triangulateHoles, false, true, false, crossSection, out mesh0, out mesh1, out vector, out vector2, out contourData);
		}

		// Token: 0x060042A6 RID: 17062 RVA: 0x00153344 File Offset: 0x00151744
		public float Cut(Mesh mesh, Transform meshTransform, PrimitivesPro.Utils.Plane plane, bool triangulateHoles, Vector4 crossSection, out Mesh mesh0, out Mesh mesh1, out Vector3 centroid0, out Vector3 centroid1, out ContourData intersectionData)
		{
			return this.Cut(mesh, meshTransform, plane, triangulateHoles, true, true, false, crossSection, out mesh0, out mesh1, out centroid0, out centroid1, out intersectionData);
		}

		// Token: 0x060042A7 RID: 17063 RVA: 0x0015336C File Offset: 0x0015176C
		public float Cut(Mesh mesh, Transform meshTransform, PrimitivesPro.Utils.Plane plane, bool triangulateHoles, Vector4 crossSection, out Mesh mesh0, out Mesh mesh1, out ContourData intersectionData)
		{
			Vector3 vector;
			Vector3 vector2;
			return this.Cut(mesh, meshTransform, plane, triangulateHoles, false, true, false, crossSection, out mesh0, out mesh1, out vector, out vector2, out intersectionData);
		}

		// Token: 0x060042A8 RID: 17064 RVA: 0x00153394 File Offset: 0x00151794
		private void AllocateBuffers(int trianglesNum, int verticesNum)
		{
			if (this.triangles == null || this.triangles[0].Capacity < trianglesNum)
			{
				this.triangles = new List<int>[]
				{
					new List<int>(trianglesNum),
					new List<int>(trianglesNum)
				};
			}
			else
			{
				this.triangles[0].Clear();
				this.triangles[1].Clear();
			}
			if (this.vertices == null || this.vertices[0].Capacity < verticesNum)
			{
				this.vertices = new List<Vector3>[]
				{
					new List<Vector3>(verticesNum),
					new List<Vector3>(verticesNum)
				};
				this.normals = new List<Vector3>[]
				{
					new List<Vector3>(verticesNum),
					new List<Vector3>(verticesNum)
				};
				this.uvs = new List<Vector2>[]
				{
					new List<Vector2>(verticesNum),
					new List<Vector2>(verticesNum)
				};
				this.centroid = new Vector3[2];
				this.triCache = new int[verticesNum + 1];
				this.triCounter = new int[2];
				this.cutTris = new List<int>(verticesNum / 3);
			}
			else
			{
				for (int i = 0; i < 2; i++)
				{
					this.vertices[i].Clear();
					this.normals[i].Clear();
					this.uvs[i].Clear();
					this.centroid[i] = Vector3.zero;
					this.triCounter[i] = 0;
				}
				this.cutTris.Clear();
				for (int j = 0; j < this.triCache.Length; j++)
				{
					this.triCache[j] = 0;
				}
			}
		}

		// Token: 0x060042A9 RID: 17065 RVA: 0x00153534 File Offset: 0x00151934
		private void AllocateContours(int cutTrianglesNum)
		{
			if (this.contour == null)
			{
				this.contour = new Contour(cutTrianglesNum);
				this.cutVertCache = new Dictionary<long, int>[]
				{
					new Dictionary<long, int>(cutTrianglesNum * 2),
					new Dictionary<long, int>(cutTrianglesNum * 2)
				};
				this.cornerVertCache = new Dictionary<int, int>[]
				{
					new Dictionary<int, int>(cutTrianglesNum),
					new Dictionary<int, int>(cutTrianglesNum)
				};
				this.contourBufferSize = cutTrianglesNum;
			}
			else
			{
				if (this.contourBufferSize < cutTrianglesNum)
				{
					this.cutVertCache = new Dictionary<long, int>[]
					{
						new Dictionary<long, int>(cutTrianglesNum * 2),
						new Dictionary<long, int>(cutTrianglesNum * 2)
					};
					this.cornerVertCache = new Dictionary<int, int>[]
					{
						new Dictionary<int, int>(cutTrianglesNum),
						new Dictionary<int, int>(cutTrianglesNum)
					};
					this.contourBufferSize = cutTrianglesNum;
				}
				else
				{
					for (int i = 0; i < 2; i++)
					{
						this.cutVertCache[i].Clear();
						this.cornerVertCache[i].Clear();
					}
				}
				this.contour.AllocateBuffers(cutTrianglesNum);
			}
		}

		// Token: 0x060042AA RID: 17066 RVA: 0x00153634 File Offset: 0x00151A34
		private float Cut(Mesh mesh, Transform meshTransform, PrimitivesPro.Utils.Plane plane, bool triangulateHoles, bool fixPivot, bool getContourList, bool dontCut, Vector4 crossSection, out Mesh mesh0, out Mesh mesh1, out Vector3 centroid0, out Vector3 centroid1, out ContourData intersectionData)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			int num = mesh.triangles.Length;
			int verticesNum = mesh.vertices.Length;
			int[] array = mesh.triangles;
			Vector3[] array2 = mesh.vertices;
			Vector3[] array3 = mesh.normals;
			Vector2[] uv = mesh.uv;
			this.crossSectionUV = crossSection;
			this.AllocateBuffers(num, verticesNum);
			plane.InverseTransform(meshTransform);
			for (int i = 0; i < num; i += 3)
			{
				Vector3 vector = array2[array[i]];
				Vector3 vector2 = array2[array[i + 1]];
				Vector3 vector3 = array2[array[i + 2]];
				bool sideFix = plane.GetSideFix(ref vector);
				bool sideFix2 = plane.GetSideFix(ref vector2);
				bool sideFix3 = plane.GetSideFix(ref vector3);
				array2[array[i]] = vector;
				array2[array[i + 1]] = vector2;
				array2[array[i + 2]] = vector3;
				if (sideFix == sideFix2 && sideFix2 == sideFix3)
				{
					int num2 = (!sideFix) ? 1 : 0;
					if (this.triCache[array[i]] == 0)
					{
						this.triangles[num2].Add(this.triCounter[num2]);
						this.vertices[num2].Add(array2[array[i]]);
						this.normals[num2].Add(array3[array[i]]);
						this.uvs[num2].Add(uv[array[i]]);
						this.centroid[num2] += array2[array[i]];
						this.triCache[array[i]] = this.triCounter[num2] + 1;
						this.triCounter[num2]++;
					}
					else
					{
						this.triangles[num2].Add(this.triCache[array[i]] - 1);
					}
					if (this.triCache[array[i + 1]] == 0)
					{
						this.triangles[num2].Add(this.triCounter[num2]);
						this.vertices[num2].Add(array2[array[i + 1]]);
						this.normals[num2].Add(array3[array[i + 1]]);
						this.uvs[num2].Add(uv[array[i + 1]]);
						this.centroid[num2] += array2[array[i + 1]];
						this.triCache[array[i + 1]] = this.triCounter[num2] + 1;
						this.triCounter[num2]++;
					}
					else
					{
						this.triangles[num2].Add(this.triCache[array[i + 1]] - 1);
					}
					if (this.triCache[array[i + 2]] == 0)
					{
						this.triangles[num2].Add(this.triCounter[num2]);
						this.vertices[num2].Add(array2[array[i + 2]]);
						this.normals[num2].Add(array3[array[i + 2]]);
						this.uvs[num2].Add(uv[array[i + 2]]);
						this.centroid[num2] += array2[array[i + 2]];
						this.triCache[array[i + 2]] = this.triCounter[num2] + 1;
						this.triCounter[num2]++;
					}
					else
					{
						this.triangles[num2].Add(this.triCache[array[i + 2]] - 1);
					}
				}
				else
				{
					this.cutTris.Add(i);
				}
			}
			if (this.vertices[0].Count == 0)
			{
				this.centroid[0] = array2[0];
			}
			else
			{
				this.centroid[0] /= (float)this.vertices[0].Count;
			}
			if (this.vertices[1].Count == 0)
			{
				this.centroid[1] = array2[1];
			}
			else
			{
				this.centroid[1] /= (float)this.vertices[1].Count;
			}
			mesh0 = null;
			mesh1 = null;
			centroid0 = this.centroid[0];
			centroid1 = this.centroid[1];
			intersectionData = null;
			if (this.cutTris.Count < 1)
			{
				stopwatch.Stop();
				return (float)stopwatch.ElapsedMilliseconds;
			}
			this.AllocateContours(this.cutTris.Count);
			foreach (int num3 in this.cutTris)
			{
				MeshCutter.Triangle tri = new MeshCutter.Triangle
				{
					ids = new int[]
					{
						array[num3],
						array[num3 + 1],
						array[num3 + 2]
					},
					pos = new Vector3[]
					{
						array2[array[num3]],
						array2[array[num3 + 1]],
						array2[array[num3 + 2]]
					},
					normal = new Vector3[]
					{
						array3[array[num3]],
						array3[array[num3 + 1]],
						array3[array[num3 + 2]]
					},
					uvs = new Vector2[]
					{
						uv[array[num3]],
						uv[array[num3 + 1]],
						uv[array[num3 + 2]]
					}
				};
				bool side = plane.GetSide(tri.pos[0]);
				bool side2 = plane.GetSide(tri.pos[1]);
				bool side3 = plane.GetSide(tri.pos[2]);
				int num4 = (!side) ? 1 : 0;
				int num5 = 1 - num4;
				if (side == side2)
				{
					float num6;
					Vector3 vector4;
					bool flag = plane.IntersectSegment(tri.pos[2], tri.pos[0], out num6, out vector4);
					float num7;
					Vector3 vector5;
					bool flag2 = plane.IntersectSegment(tri.pos[2], tri.pos[1], out num7, out vector5);
					int num8 = this.AddIntersectionPoint(vector4, tri, tri.ids[2], tri.ids[0], this.cutVertCache[num4], this.vertices[num4], this.normals[num4], this.uvs[num4]);
					int num9 = this.AddIntersectionPoint(vector5, tri, tri.ids[2], tri.ids[1], this.cutVertCache[num4], this.vertices[num4], this.normals[num4], this.uvs[num4]);
					int item = this.AddTrianglePoint(tri.pos[0], tri.normal[0], tri.uvs[0], tri.ids[0], this.triCache, this.cornerVertCache[num4], this.vertices[num4], this.normals[num4], this.uvs[num4]);
					int item2 = this.AddTrianglePoint(tri.pos[1], tri.normal[1], tri.uvs[1], tri.ids[1], this.triCache, this.cornerVertCache[num4], this.vertices[num4], this.normals[num4], this.uvs[num4]);
					this.triangles[num4].Add(num8);
					this.triangles[num4].Add(item);
					this.triangles[num4].Add(num9);
					this.triangles[num4].Add(num9);
					this.triangles[num4].Add(item);
					this.triangles[num4].Add(item2);
					int num10 = this.AddIntersectionPoint(vector4, tri, tri.ids[2], tri.ids[0], this.cutVertCache[num5], this.vertices[num5], this.normals[num5], this.uvs[num5]);
					int num11 = this.AddIntersectionPoint(vector5, tri, tri.ids[2], tri.ids[1], this.cutVertCache[num5], this.vertices[num5], this.normals[num5], this.uvs[num5]);
					int item3 = this.AddTrianglePoint(tri.pos[2], tri.normal[2], tri.uvs[2], tri.ids[2], this.triCache, this.cornerVertCache[num5], this.vertices[num5], this.normals[num5], this.uvs[num5]);
					this.triangles[num5].Add(item3);
					this.triangles[num5].Add(num10);
					this.triangles[num5].Add(num11);
					if (triangulateHoles)
					{
						if (num4 == 0)
						{
							this.contour.AddTriangle(num3, num8, num9, vector4, vector5);
						}
						else
						{
							this.contour.AddTriangle(num3, num10, num11, vector4, vector5);
						}
					}
				}
				else if (side == side3)
				{
					float num6;
					Vector3 vector5;
					bool flag3 = plane.IntersectSegment(tri.pos[1], tri.pos[0], out num6, out vector5);
					Vector3 vector4;
					float num7;
					bool flag4 = plane.IntersectSegment(tri.pos[1], tri.pos[2], out num7, out vector4);
					int num12 = this.AddIntersectionPoint(vector4, tri, tri.ids[1], tri.ids[2], this.cutVertCache[num4], this.vertices[num4], this.normals[num4], this.uvs[num4]);
					int num13 = this.AddIntersectionPoint(vector5, tri, tri.ids[1], tri.ids[0], this.cutVertCache[num4], this.vertices[num4], this.normals[num4], this.uvs[num4]);
					int item4 = this.AddTrianglePoint(tri.pos[0], tri.normal[0], tri.uvs[0], tri.ids[0], this.triCache, this.cornerVertCache[num4], this.vertices[num4], this.normals[num4], this.uvs[num4]);
					int item5 = this.AddTrianglePoint(tri.pos[2], tri.normal[2], tri.uvs[2], tri.ids[2], this.triCache, this.cornerVertCache[num4], this.vertices[num4], this.normals[num4], this.uvs[num4]);
					this.triangles[num4].Add(item5);
					this.triangles[num4].Add(num13);
					this.triangles[num4].Add(num12);
					this.triangles[num4].Add(item5);
					this.triangles[num4].Add(item4);
					this.triangles[num4].Add(num13);
					int num14 = this.AddIntersectionPoint(vector4, tri, tri.ids[1], tri.ids[2], this.cutVertCache[num5], this.vertices[num5], this.normals[num5], this.uvs[num5]);
					int num15 = this.AddIntersectionPoint(vector5, tri, tri.ids[1], tri.ids[0], this.cutVertCache[num5], this.vertices[num5], this.normals[num5], this.uvs[num5]);
					int item6 = this.AddTrianglePoint(tri.pos[1], tri.normal[1], tri.uvs[1], tri.ids[1], this.triCache, this.cornerVertCache[num5], this.vertices[num5], this.normals[num5], this.uvs[num5]);
					this.triangles[num5].Add(num14);
					this.triangles[num5].Add(num15);
					this.triangles[num5].Add(item6);
					if (triangulateHoles)
					{
						if (num4 == 0)
						{
							this.contour.AddTriangle(num3, num12, num13, vector4, vector5);
						}
						else
						{
							this.contour.AddTriangle(num3, num14, num15, vector4, vector5);
						}
					}
				}
				else
				{
					float num6;
					Vector3 vector4;
					bool flag5 = plane.IntersectSegment(tri.pos[0], tri.pos[1], out num6, out vector4);
					float num7;
					Vector3 vector5;
					bool flag6 = plane.IntersectSegment(tri.pos[0], tri.pos[2], out num7, out vector5);
					int num16 = this.AddIntersectionPoint(vector4, tri, tri.ids[0], tri.ids[1], this.cutVertCache[num5], this.vertices[num5], this.normals[num5], this.uvs[num5]);
					int num17 = this.AddIntersectionPoint(vector5, tri, tri.ids[0], tri.ids[2], this.cutVertCache[num5], this.vertices[num5], this.normals[num5], this.uvs[num5]);
					int item7 = this.AddTrianglePoint(tri.pos[1], tri.normal[1], tri.uvs[1], tri.ids[1], this.triCache, this.cornerVertCache[num5], this.vertices[num5], this.normals[num5], this.uvs[num5]);
					int item8 = this.AddTrianglePoint(tri.pos[2], tri.normal[2], tri.uvs[2], tri.ids[2], this.triCache, this.cornerVertCache[num5], this.vertices[num5], this.normals[num5], this.uvs[num5]);
					this.triangles[num5].Add(item8);
					this.triangles[num5].Add(num17);
					this.triangles[num5].Add(item7);
					this.triangles[num5].Add(num17);
					this.triangles[num5].Add(num16);
					this.triangles[num5].Add(item7);
					int num18 = this.AddIntersectionPoint(vector4, tri, tri.ids[0], tri.ids[1], this.cutVertCache[num4], this.vertices[num4], this.normals[num4], this.uvs[num4]);
					int num19 = this.AddIntersectionPoint(vector5, tri, tri.ids[0], tri.ids[2], this.cutVertCache[num4], this.vertices[num4], this.normals[num4], this.uvs[num4]);
					int item9 = this.AddTrianglePoint(tri.pos[0], tri.normal[0], tri.uvs[0], tri.ids[0], this.triCache, this.cornerVertCache[num4], this.vertices[num4], this.normals[num4], this.uvs[num4]);
					this.triangles[num4].Add(num19);
					this.triangles[num4].Add(item9);
					this.triangles[num4].Add(num18);
					if (triangulateHoles)
					{
						if (num4 == 0)
						{
							this.contour.AddTriangle(num3, num18, num19, vector4, vector5);
						}
						else
						{
							this.contour.AddTriangle(num3, num16, num17, vector4, vector5);
						}
					}
				}
			}
			if (triangulateHoles || getContourList)
			{
				this.contour.FindContours();
			}
			List<int>[] array4 = null;
			if (triangulateHoles)
			{
				array4 = new List<int>[]
				{
					new List<int>(this.contour.MidPointsCount),
					new List<int>(this.contour.MidPointsCount)
				};
				this.Triangulate(this.contour.contour, plane, this.vertices, this.normals, this.uvs, array4, true);
			}
			intersectionData = null;
			if (getContourList)
			{
				List<Vector3[]> contourList = null;
				this.GetContourList(this.contour.contour, this.vertices[0], out contourList);
				intersectionData = new ContourData(contourList, meshTransform);
			}
			centroid0 = this.centroid[0];
			centroid1 = this.centroid[1];
			if (dontCut)
			{
				mesh0 = null;
				mesh1 = null;
				return (float)stopwatch.ElapsedMilliseconds;
			}
			if (this.vertices[0].Count > 0 && this.vertices[1].Count > 0)
			{
				mesh0 = new Mesh();
				mesh1 = new Mesh();
				Vector3[] array5 = this.vertices[0].ToArray();
				Vector3[] array6 = this.vertices[1].ToArray();
				if (fixPivot)
				{
					MeshUtils.CenterPivot(array5, this.centroid[0]);
					MeshUtils.CenterPivot(array6, this.centroid[1]);
				}
				mesh0.vertices = array5;
				mesh0.normals = this.normals[0].ToArray();
				mesh0.uv = this.uvs[0].ToArray();
				mesh1.vertices = array6;
				mesh1.normals = this.normals[1].ToArray();
				mesh1.uv = this.uvs[1].ToArray();
				if (triangulateHoles && array4[0].Count > 0)
				{
					mesh0.subMeshCount = 2;
					mesh0.SetTriangles(this.triangles[0].ToArray(), 0);
					mesh0.SetTriangles(array4[0].ToArray(), 1);
					mesh1.subMeshCount = 2;
					mesh1.SetTriangles(this.triangles[1].ToArray(), 0);
					mesh1.SetTriangles(array4[1].ToArray(), 1);
				}
				else
				{
					mesh0.triangles = this.triangles[0].ToArray();
					mesh1.triangles = this.triangles[1].ToArray();
				}
				stopwatch.Stop();
				return (float)stopwatch.ElapsedMilliseconds;
			}
			mesh0 = null;
			mesh1 = null;
			stopwatch.Stop();
			return (float)stopwatch.ElapsedMilliseconds;
		}

		// Token: 0x060042AB RID: 17067 RVA: 0x00154B1C File Offset: 0x00152F1C
		private int AddIntersectionPoint(Vector3 pos, MeshCutter.Triangle tri, int edge0, int edge1, Dictionary<long, int> cache, List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs)
		{
			int num = (edge0 >= edge1) ? ((edge1 << 16) + edge0) : ((edge0 << 16) + edge1);
			int result;
			if (cache.TryGetValue((long)num, out result))
			{
				return result;
			}
			Vector3 vector = MeshUtils.ComputeBarycentricCoordinates(tri.pos[0], tri.pos[1], tri.pos[2], pos);
			vertices.Add(pos);
			normals.Add(new Vector3(vector.x * tri.normal[0].x + vector.y * tri.normal[1].x + vector.z * tri.normal[2].x, vector.x * tri.normal[0].y + vector.y * tri.normal[1].y + vector.z * tri.normal[2].y, vector.x * tri.normal[0].z + vector.y * tri.normal[1].z + vector.z * tri.normal[2].z));
			uvs.Add(new Vector2(vector.x * tri.uvs[0].x + vector.y * tri.uvs[1].x + vector.z * tri.uvs[2].x, vector.x * tri.uvs[0].y + vector.y * tri.uvs[1].y + vector.z * tri.uvs[2].y));
			int num2 = vertices.Count - 1;
			cache.Add((long)num, num2);
			return num2;
		}

		// Token: 0x060042AC RID: 17068 RVA: 0x00154D5C File Offset: 0x0015315C
		private int AddTrianglePoint(Vector3 pos, Vector3 normal, Vector2 uv, int idx, int[] triCache, Dictionary<int, int> cache, List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs)
		{
			if (triCache[idx] != 0)
			{
				return triCache[idx] - 1;
			}
			int result;
			if (cache.TryGetValue(idx, out result))
			{
				return result;
			}
			vertices.Add(pos);
			normals.Add(normal);
			uvs.Add(uv);
			int num = vertices.Count - 1;
			cache.Add(idx, num);
			return num;
		}

		// Token: 0x060042AD RID: 17069 RVA: 0x00154DBC File Offset: 0x001531BC
		private void GetContourList(List<Dictionary<int, int>> contours, List<Vector3> vertices, out List<Vector3[]> contoursList)
		{
			contoursList = null;
			if (contours.Count == 0 || contours[0].Count < 3)
			{
				return;
			}
			contoursList = new List<Vector3[]>(contours.Count);
			int count = contours.Count;
			for (int i = 0; i < count; i++)
			{
				contoursList.Add(new Vector3[contours[i].Count]);
				int num = 0;
				foreach (KeyValuePair<int, int> keyValuePair in contours[i])
				{
					contoursList[i][num++] = vertices[keyValuePair.Value];
				}
			}
		}

		// Token: 0x060042AE RID: 17070 RVA: 0x00154E98 File Offset: 0x00153298
		private void Triangulate(List<Dictionary<int, int>> contours, PrimitivesPro.Utils.Plane plane, List<Vector3>[] vertices, List<Vector3>[] normals, List<Vector2>[] uvs, List<int>[] triangles, bool uvCutMesh)
		{
			if (contours.Count == 0 || contours[0].Count < 3)
			{
				return;
			}
			Matrix4x4 planeMatrix = plane.GetPlaneMatrix();
			Matrix4x4 inverse = planeMatrix.inverse;
			float z = 0f;
			List<Polygon> list = new List<Polygon>(contours.Count);
			Polygon polygon = null;
			foreach (Dictionary<int, int> dictionary in contours)
			{
				Vector2[] array = new Vector2[dictionary.Count];
				int num = 0;
				foreach (int index in dictionary.Values)
				{
					Vector4 v = inverse * vertices[0][index];
					array[num++] = v;
					z = v.z;
				}
				Polygon polygon2 = new Polygon(array);
				list.Add(polygon2);
				if (polygon == null || polygon.Area < polygon2.Area)
				{
					polygon = polygon2;
				}
			}
			if (list.Count > 0)
			{
				List<Polygon> list2 = new List<Polygon>();
				foreach (Polygon polygon3 in list)
				{
					if (polygon3 != polygon && polygon.IsPointInside(polygon3.Points[0]))
					{
						polygon.AddHole(polygon3);
						list2.Add(polygon3);
					}
				}
				foreach (Polygon item in list2)
				{
					list.Remove(item);
				}
			}
			int num2 = vertices[0].Count;
			int num3 = vertices[1].Count;
			foreach (Polygon polygon4 in list)
			{
				List<int> list3 = polygon4.Triangulate();
				float num4 = Mathf.Min(polygon4.Min.x, polygon4.Min.y);
				float num5 = Mathf.Max(polygon4.Max.x, polygon4.Max.y);
				float num6 = num4 - num5;
				foreach (Vector2 vector in polygon4.Points)
				{
					Vector4 v2 = planeMatrix * new Vector3(vector.x, vector.y, z);
					vertices[0].Add(v2);
					vertices[1].Add(v2);
					normals[0].Add(-plane.Normal);
					normals[1].Add(plane.Normal);
					if (uvCutMesh)
					{
						Vector2 item2 = new Vector2((vector.x - num4) / num6, (vector.y - num4) / num6);
						Vector2 item3 = new Vector2((vector.x - num4) / num6, (vector.y - num4) / num6);
						float num7 = this.crossSectionUV.z - this.crossSectionUV.x;
						float num8 = this.crossSectionUV.w - this.crossSectionUV.y;
						item2.x = this.crossSectionUV.x + item2.x * num7;
						item2.y = this.crossSectionUV.y + item2.y * num8;
						item3.x = this.crossSectionUV.x + item3.x * num7;
						item3.y = this.crossSectionUV.y + item3.y * num8;
						uvs[0].Add(item2);
						uvs[1].Add(item3);
					}
					else
					{
						uvs[0].Add(Vector2.zero);
						uvs[1].Add(Vector2.zero);
					}
				}
				int count = list3.Count;
				int num9 = count - 1;
				for (int j = 0; j < count; j++)
				{
					triangles[0].Add(num2 + list3[j]);
					triangles[1].Add(num3 + list3[num9]);
					num9--;
				}
				num2 += polygon4.Points.Length;
				num3 += polygon4.Points.Length;
			}
		}

		// Token: 0x04002B43 RID: 11075
		public static Vector4 defaultCrossSection = new Vector4(0f, 0f, 1f, 1f);

		// Token: 0x04002B44 RID: 11076
		private List<int>[] triangles;

		// Token: 0x04002B45 RID: 11077
		private List<Vector3>[] vertices;

		// Token: 0x04002B46 RID: 11078
		private List<Vector3>[] normals;

		// Token: 0x04002B47 RID: 11079
		private List<Vector2>[] uvs;

		// Token: 0x04002B48 RID: 11080
		private List<int> cutTris;

		// Token: 0x04002B49 RID: 11081
		private int[] triCache;

		// Token: 0x04002B4A RID: 11082
		private Vector3[] centroid;

		// Token: 0x04002B4B RID: 11083
		private int[] triCounter;

		// Token: 0x04002B4C RID: 11084
		private Contour contour;

		// Token: 0x04002B4D RID: 11085
		private Dictionary<long, int>[] cutVertCache;

		// Token: 0x04002B4E RID: 11086
		private Dictionary<int, int>[] cornerVertCache;

		// Token: 0x04002B4F RID: 11087
		private int contourBufferSize;

		// Token: 0x04002B50 RID: 11088
		private Vector4 crossSectionUV = MeshCutter.defaultCrossSection;

		// Token: 0x02000866 RID: 2150
		private struct Triangle
		{
			// Token: 0x04002B51 RID: 11089
			public int[] ids;

			// Token: 0x04002B52 RID: 11090
			public Vector3[] pos;

			// Token: 0x04002B53 RID: 11091
			public Vector3[] normal;

			// Token: 0x04002B54 RID: 11092
			public Vector2[] uvs;
		}
	}
}
