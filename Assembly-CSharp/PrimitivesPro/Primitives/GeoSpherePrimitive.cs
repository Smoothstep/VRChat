using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro.Primitives
{
	// Token: 0x02000874 RID: 2164
	public class GeoSpherePrimitive : Primitive
	{
		// Token: 0x060042F8 RID: 17144 RVA: 0x0015A1FC File Offset: 0x001585FC
		public static float GenerateGeometry(Mesh mesh, float radius, int subdivision, GeoSpherePrimitive.BaseType baseType, NormalsType normalsType, PivotPosition pivotPosition)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			radius = Mathf.Clamp(radius, 0f, 100f);
			subdivision = Mathf.Clamp(subdivision, 0, 6);
			mesh.Clear();
			bool sharedVertices = normalsType == NormalsType.Vertex;
			int i = GeoSpherePrimitive.GetVertCount(baseType, subdivision, sharedVertices);
			int triCount = GeoSpherePrimitive.GetTriCount(baseType, subdivision);
			while (i > 60000)
			{
				subdivision--;
				i = GeoSpherePrimitive.GetVertCount(baseType, subdivision, sharedVertices);
				triCount = GeoSpherePrimitive.GetTriCount(baseType, subdivision);
			}
			Vector3 zero = Vector3.zero;
			if (pivotPosition != PivotPosition.Botttom)
			{
				if (pivotPosition == PivotPosition.Top)
				{
					zero = new Vector3(0f, -radius, 0f);
				}
			}
			else
			{
				zero = new Vector3(0f, radius, 0f);
			}
			int[] array = new int[triCount * 3];
			int[] array2 = new int[triCount * 3];
			Vector3[] array3 = new Vector3[i];
			Vector2[] array4 = new Vector2[i];
			Vector3[] normals = null;
			GeoSpherePrimitive.InitBasePrimitive(radius, baseType, array3, array4, array);
			Dictionary<int, int> indexLookup = new Dictionary<int, int>();
			int vertCount = GeoSpherePrimitive.GetVertCount(baseType, 0, sharedVertices);
			for (int j = 0; j < subdivision; j++)
			{
				int num = 0;
				int num2 = GeoSpherePrimitive.GetTriCount(baseType, j) * 3;
				for (int k = 0; k < num2; k += 3)
				{
					int num3 = array[k];
					int num4 = array[k + 1];
					int num5 = array[k + 2];
					int num6 = GeoSpherePrimitive.AddMidPoint(array3, radius, vertCount++, num3, num4, indexLookup);
					int num7 = GeoSpherePrimitive.AddMidPoint(array3, radius, vertCount++, num4, num5, indexLookup);
					int num8 = GeoSpherePrimitive.AddMidPoint(array3, radius, vertCount++, num5, num3, indexLookup);
					array2[num] = num3;
					array2[num + 1] = num6;
					array2[num + 2] = num8;
					array2[num + 3] = num4;
					array2[num + 4] = num7;
					array2[num + 5] = num6;
					array2[num + 6] = num5;
					array2[num + 7] = num8;
					array2[num + 8] = num7;
					array2[num + 9] = num6;
					array2[num + 10] = num7;
					array2[num + 11] = num8;
					num += 12;
				}
				int[] array5 = array2;
				array2 = array;
				array = array5;
			}
			if (normalsType == NormalsType.Face)
			{
				MeshUtils.DuplicateSharedVertices(ref array3, ref array4, array, -1);
			}
			for (int l = 0; l < i; l++)
			{
				array4[l] = GeoSpherePrimitive.GetSphericalUV(ref array3[l]);
			}
			List<Vector3> list = new List<Vector3>(array3);
			List<Vector2> list2 = new List<Vector2>(array4);
			List<int> list3 = new List<int>(array);
			GeoSpherePrimitive.CorrectSeam(list, list2, list3);
			GeoSpherePrimitive.CorrectPoles(list, list2, ref list3, radius);
			array3 = list.ToArray();
			array = list3.ToArray();
			if (normalsType == NormalsType.Vertex)
			{
				GeoSpherePrimitive.CalculateNormals(array3, out normals);
			}
			else
			{
				MeshUtils.ComputeVertexNormals(array3, array, out normals);
			}
			GeoSpherePrimitive.CorrectPivot(list, pivotPosition, ref zero);
			if (list.Count > 60000)
			{
				UnityEngine.Debug.LogError("Too much vertices!");
				return 0f;
			}
			mesh.vertices = list.ToArray();
			mesh.uv = list2.ToArray();
			mesh.triangles = list3.ToArray();
			mesh.normals = normals;
			mesh.RecalculateBounds();
			MeshUtils.CalculateTangents(mesh);
			stopwatch.Stop();
			return (float)stopwatch.ElapsedMilliseconds;
		}

		// Token: 0x060042F9 RID: 17145 RVA: 0x0015A540 File Offset: 0x00158940
		private static int AddMidPoint(Vector3[] vertices, float radius, int vertIndex, int id0, int id1, Dictionary<int, int> indexLookup)
		{
			int key = (id0 >= id1) ? ((id1 << 16) + id0) : ((id0 << 16) + id1);
			int result;
			if (indexLookup.TryGetValue(key, out result))
			{
				return result;
			}
			Vector3 normalized = ((vertices[id0] + vertices[id1]) * 0.5f).normalized;
			Vector3 vector = normalized * radius;
			vertices[vertIndex] = vector;
			indexLookup.Add(key, vertIndex);
			return vertIndex;
		}

		// Token: 0x060042FA RID: 17146 RVA: 0x0015A5D0 File Offset: 0x001589D0
		private static void InitBasePrimitive(float radius, GeoSpherePrimitive.BaseType baseType, Vector3[] vertices, Vector2[] uvs, int[] triangles)
		{
			switch (baseType)
			{
			case GeoSpherePrimitive.BaseType.Tetrahedron:
			{
				float num = 1f / Mathf.Sqrt(2f);
				float num2 = 1f / Mathf.Sqrt(1.5f);
				float num3 = 0.67f * radius / num2;
				num = 0.67f * radius * num / num2;
				GeoSpherePrimitive.SetVertices(vertices, new Vector3[]
				{
					new Vector3(num3, 0f, -num),
					new Vector3(-num3, 0f, -num),
					new Vector3(0f, num3, num),
					new Vector3(0f, -num3, num)
				});
				GeoSpherePrimitive.SetTriangles(triangles, new int[]
				{
					0,
					1,
					2,
					1,
					3,
					2,
					0,
					2,
					3,
					0,
					3,
					1
				});
				break;
			}
			case GeoSpherePrimitive.BaseType.Octahedron:
				GeoSpherePrimitive.SetVertices(vertices, new Vector3[]
				{
					new Vector3(0f, -radius, 0f),
					new Vector3(-radius, 0f, 0f),
					new Vector3(0f, 0f, -radius),
					new Vector3(radius, 0f, 0f),
					new Vector3(0f, 0f, radius),
					new Vector3(0f, radius, 0f)
				});
				GeoSpherePrimitive.SetTriangles(triangles, new int[]
				{
					0,
					1,
					2,
					0,
					2,
					3,
					0,
					3,
					4,
					0,
					4,
					1,
					5,
					2,
					1,
					5,
					3,
					2,
					5,
					4,
					3,
					5,
					1,
					4
				});
				break;
			case GeoSpherePrimitive.BaseType.Icosahedron:
			{
				float num4 = 1f;
				float num5 = (1f + Mathf.Sqrt(5f)) / 2f;
				float num6 = radius / Mathf.Sqrt(num4 * num4 + num5 * num5);
				num4 *= num6;
				num5 *= num6;
				GeoSpherePrimitive.SetVertices(vertices, new Vector3[]
				{
					new Vector3(-num4, num5, 0f),
					new Vector3(num4, num5, 0f),
					new Vector3(-num4, -num5, 0f),
					new Vector3(num4, -num5, 0f),
					new Vector3(0f, -num4, num5),
					new Vector3(0f, num4, num5),
					new Vector3(0f, -num4, -num5),
					new Vector3(0f, num4, -num5),
					new Vector3(num5, 0f, -num4),
					new Vector3(num5, 0f, num4),
					new Vector3(-num5, 0f, -num4),
					new Vector3(-num5, 0f, num4)
				});
				GeoSpherePrimitive.SetTriangles(triangles, new int[]
				{
					0,
					11,
					5,
					0,
					5,
					1,
					0,
					1,
					7,
					0,
					7,
					10,
					0,
					10,
					11,
					1,
					5,
					9,
					5,
					11,
					4,
					11,
					10,
					2,
					10,
					7,
					6,
					7,
					1,
					8,
					3,
					9,
					4,
					3,
					4,
					2,
					3,
					2,
					6,
					3,
					6,
					8,
					3,
					8,
					9,
					4,
					9,
					5,
					2,
					4,
					11,
					6,
					2,
					10,
					8,
					6,
					7,
					9,
					8,
					1
				});
				break;
			}
			case GeoSpherePrimitive.BaseType.Icositetrahedron:
			{
				float num7 = radius / Mathf.Sqrt(3f);
				GeoSpherePrimitive.SetVertices(vertices, new Vector3[]
				{
					new Vector3(0f, radius, 0f),
					new Vector3(0f, -radius, 0f),
					new Vector3(radius, 0f, 0f),
					new Vector3(-radius, 0f, 0f),
					new Vector3(0f, 0f, radius),
					new Vector3(0f, 0f, -radius),
					new Vector3(-num7, num7, num7),
					new Vector3(-num7, num7, -num7),
					new Vector3(num7, num7, -num7),
					new Vector3(num7, num7, num7),
					new Vector3(-num7, -num7, num7),
					new Vector3(-num7, -num7, -num7),
					new Vector3(num7, -num7, -num7),
					new Vector3(num7, -num7, num7)
				});
				GeoSpherePrimitive.SetTriangles(triangles, new int[]
				{
					0,
					7,
					6,
					0,
					8,
					7,
					0,
					9,
					8,
					0,
					6,
					9,
					1,
					10,
					11,
					1,
					11,
					12,
					1,
					12,
					13,
					1,
					13,
					10,
					2,
					8,
					9,
					2,
					9,
					13,
					2,
					13,
					12,
					2,
					12,
					8,
					3,
					6,
					7,
					3,
					7,
					11,
					3,
					11,
					10,
					3,
					10,
					6,
					4,
					9,
					6,
					4,
					13,
					9,
					4,
					10,
					13,
					4,
					6,
					10,
					5,
					7,
					8,
					5,
					8,
					12,
					5,
					12,
					11,
					5,
					11,
					7
				});
				break;
			}
			}
		}

		// Token: 0x060042FB RID: 17147 RVA: 0x0015AAEC File Offset: 0x00158EEC
		private static int GetTriCount(GeoSpherePrimitive.BaseType type, int subdivision)
		{
			int num = 0;
			switch (type)
			{
			case GeoSpherePrimitive.BaseType.Tetrahedron:
				num = 4;
				break;
			case GeoSpherePrimitive.BaseType.Octahedron:
				num = 8;
				break;
			case GeoSpherePrimitive.BaseType.Icosahedron:
				num = 20;
				break;
			case GeoSpherePrimitive.BaseType.Icositetrahedron:
				num = 24;
				break;
			}
			return num * (int)Mathf.Pow(4f, (float)subdivision);
		}

		// Token: 0x060042FC RID: 17148 RVA: 0x0015AB44 File Offset: 0x00158F44
		private static int GetVertCount(GeoSpherePrimitive.BaseType type, int subdivision, bool sharedVertices)
		{
			int triCount = GeoSpherePrimitive.GetTriCount(type, subdivision);
			if (sharedVertices)
			{
				return triCount;
			}
			return triCount * 3;
		}

		// Token: 0x060042FD RID: 17149 RVA: 0x0015AB64 File Offset: 0x00158F64
		private static void SetVertices(Vector3[] vertices, params Vector3[] verts)
		{
			for (int i = 0; i < verts.Length; i++)
			{
				vertices[i] = verts[i];
			}
		}

		// Token: 0x060042FE RID: 17150 RVA: 0x0015ABA0 File Offset: 0x00158FA0
		private static void SetTriangles(int[] triangles, params int[] tris)
		{
			for (int i = 0; i < tris.Length; i++)
			{
				triangles[i] = tris[i];
			}
		}

		// Token: 0x060042FF RID: 17151 RVA: 0x0015ABC8 File Offset: 0x00158FC8
		private static void CalculateNormals(Vector3[] vertices, out Vector3[] normals)
		{
			normals = new Vector3[vertices.Length];
			for (int i = 0; i < vertices.Length; i++)
			{
				normals[i] = vertices[i].normalized;
			}
		}

		// Token: 0x06004300 RID: 17152 RVA: 0x0015AC0C File Offset: 0x0015900C
		private static Vector2 GetSphericalUV(ref Vector3 pnt)
		{
			Vector3 normalized = pnt.normalized;
			return new Vector2(0.5f + Mathf.Atan2(normalized.z, normalized.x) / 6.28318548f, 1f - (0.5f - Mathf.Asin(normalized.y) / 3.14159274f));
		}

		// Token: 0x06004301 RID: 17153 RVA: 0x0015AC64 File Offset: 0x00159064
		private static void CorrectPivot(List<Vector3> vertices, PivotPosition pivotPosition, ref Vector3 pivotOffset)
		{
			if (pivotPosition != PivotPosition.Center)
			{
				for (int i = 0; i < vertices.Count; i++)
				{
					int index;
					vertices[index = i] = vertices[index] + pivotOffset;
				}
			}
		}

		// Token: 0x06004302 RID: 17154 RVA: 0x0015ACAC File Offset: 0x001590AC
		private static void CorrectSeam(List<Vector3> vertices, List<Vector2> uvs, List<int> triangles)
		{
			List<int> list = new List<int>();
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int i = triangles.Count - 3; i >= 0; i -= 3)
			{
				Vector2 a = uvs[triangles[i]];
				Vector2 b = uvs[triangles[i + 1]];
				Vector2 a2 = uvs[triangles[i + 2]];
				if (Vector3.Cross(a - b, a2 - b).z <= 0f)
				{
					for (int j = i; j < i + 3; j++)
					{
						int num = triangles[j];
						Vector3 item = vertices[num];
						Vector2 vector = uvs[num];
						if (vector.x >= 0.8f)
						{
							if (dictionary.ContainsKey(num))
							{
								list.Add(dictionary[num]);
							}
							else
							{
								Vector2 item2 = vector;
								item2.x -= 1f;
								vertices.Add(item);
								uvs.Add(item2);
								int num2 = vertices.Count - 1;
								dictionary.Add(num, num2);
								list.Add(num2);
							}
						}
						else
						{
							list.Add(num);
						}
					}
				}
				else
				{
					list.AddRange(triangles.GetRange(i, 3));
				}
			}
			triangles.Clear();
			triangles.AddRange(list);
		}

		// Token: 0x06004303 RID: 17155 RVA: 0x0015AE18 File Offset: 0x00159218
		private static void CorrectPoles(List<Vector3> vertices, List<Vector2> uvs, ref List<int> triangles, float radius)
		{
			List<int> list = new List<int>(triangles);
			for (int i = 0; i < triangles.Count; i += 3)
			{
				int num = triangles[i];
				int num2 = triangles[i + 1];
				int num3 = triangles[i + 2];
				if (Math.Abs(vertices[num].y) == radius)
				{
					Vector3 item = vertices[num];
					Vector2 item2 = uvs[num];
					float num4 = 0.5f;
					if (vertices[num].y == radius)
					{
						if (uvs[num2].x > uvs[num3].x)
						{
							num4 = 0f;
						}
					}
					else if (uvs[num2].x < uvs[num3].x)
					{
						num4 = 0f;
					}
					item2.x = (uvs[num2].x + uvs[num3].x) * 0.5f + num4;
					item2.y = 1f - (0.5f - Mathf.Asin(Mathf.Sign(item.y)) / 3.14159274f);
					vertices.Add(item);
					uvs.Add(item2);
					list.Add(vertices.Count - 1);
					list.Add(num2);
					list.Add(num3);
				}
				else if (Math.Abs(vertices[num2].y) == radius)
				{
					Vector3 item3 = vertices[num2];
					Vector2 item4 = uvs[num2];
					item4.x = (uvs[num].x + uvs[num3].x) * 0.5f;
					item4.y = 1f - (0.5f - Mathf.Asin(Mathf.Sign(item3.y)) / 3.14159274f);
					vertices.Add(item3);
					uvs.Add(item4);
					list.Add(num);
					list.Add(vertices.Count - 1);
					list.Add(num3);
				}
				else if (Math.Abs(vertices[num3].y) == radius)
				{
					Vector3 item5 = vertices[num3];
					Vector2 item6 = uvs[num3];
					float num5 = 0.5f;
					if (vertices[num3].y == radius)
					{
						if (uvs[num].x > uvs[num2].x)
						{
							num5 = 0f;
						}
					}
					else if (uvs[num].x < uvs[num2].x)
					{
						num5 = 0f;
					}
					item6.x = (uvs[num].x + uvs[num2].x) * 0.5f + num5;
					item6.y = 1f - (0.5f - Mathf.Asin(Mathf.Sign(item5.y)) / 3.14159274f);
					vertices.Add(item5);
					uvs.Add(item6);
					list.Add(num);
					list.Add(num2);
					list.Add(vertices.Count - 1);
				}
				else
				{
					list.Add(num);
					list.Add(num2);
					list.Add(num3);
				}
			}
			triangles = list;
		}

		// Token: 0x02000875 RID: 2165
		public enum BaseType
		{
			// Token: 0x04002B75 RID: 11125
			Tetrahedron,
			// Token: 0x04002B76 RID: 11126
			Octahedron,
			// Token: 0x04002B77 RID: 11127
			Icosahedron,
			// Token: 0x04002B78 RID: 11128
			Icositetrahedron
		}
	}
}
