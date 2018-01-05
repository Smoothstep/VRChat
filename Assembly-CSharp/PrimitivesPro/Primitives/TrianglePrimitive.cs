using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro.Primitives
{
	// Token: 0x02000883 RID: 2179
	public class TrianglePrimitive : Primitive
	{
		// Token: 0x0600431A RID: 17178 RVA: 0x0015F4FC File Offset: 0x0015D8FC
		public static float GenerateGeometry(Mesh mesh, float side, int subdivision)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			side = Mathf.Clamp(side, 0.01f, 100f);
			subdivision = Mathf.Clamp(subdivision, 0, 7);
			mesh.Clear();
			int triCount = TrianglePrimitive.GetTriCount(subdivision);
			int vertCount = TrianglePrimitive.GetVertCount(subdivision);
			int[] array = new int[triCount * 3];
			int[] array2 = new int[triCount * 3];
			Vector3[] array3 = new Vector3[vertCount];
			Vector3[] array4 = new Vector3[vertCount];
			Vector2[] array5 = new Vector2[vertCount];
			Vector3 zero = Vector3.zero;
			Vector3 vector = new Vector3(1f, 0f, 0f);
			Vector3 vector2 = zero + vector * side;
			Vector3 a = Quaternion.AngleAxis(-60f, Vector3.up) * vector;
			Vector3 vector3 = zero + a * side;
			Vector3 b = (zero + vector2 + vector3) / 3f;
			array3[0] = zero - b;
			array3[1] = vector2 - b;
			array3[2] = vector3 - b;
			array4[0] = new Vector3(0f, 1f, 0f);
			array4[1] = new Vector3(0f, 1f, 0f);
			array4[2] = new Vector3(0f, 1f, 0f);
			array[0] = 2;
			array[1] = 1;
			array[2] = 0;
			array5[0] = new Vector2(0f, 0f);
			array5[1] = new Vector2(1f, 0f);
			array5[2] = new Vector2(0.5f, Mathf.Sin(1.04719758f));
			Dictionary<int, int> indexLookup = new Dictionary<int, int>();
			int vertCount2 = TrianglePrimitive.GetVertCount(0);
			for (int i = 0; i < subdivision; i++)
			{
				int num = 0;
				int num2 = TrianglePrimitive.GetTriCount(i) * 3;
				for (int j = 0; j < num2; j += 3)
				{
					int num3 = array[j];
					int num4 = array[j + 1];
					int num5 = array[j + 2];
					int num6 = TrianglePrimitive.AddMidPoint(array3, array5, array4, vertCount2++, num3, num4, indexLookup);
					int num7 = TrianglePrimitive.AddMidPoint(array3, array5, array4, vertCount2++, num4, num5, indexLookup);
					int num8 = TrianglePrimitive.AddMidPoint(array3, array5, array4, vertCount2++, num5, num3, indexLookup);
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
				int[] array6 = array2;
				array2 = array;
				array = array6;
			}
			mesh.vertices = array3;
			mesh.normals = array4;
			mesh.uv = array5;
			mesh.triangles = array;
			mesh.RecalculateBounds();
			MeshUtils.CalculateTangents(mesh);
			stopwatch.Stop();
			return (float)stopwatch.ElapsedMilliseconds;
		}

		// Token: 0x0600431B RID: 17179 RVA: 0x0015F854 File Offset: 0x0015DC54
		private static int AddMidPoint(Vector3[] vertices, Vector2[] uvs, Vector3[] normals, int vertIndex, int id0, int id1, Dictionary<int, int> indexLookup)
		{
			int key = (id0 >= id1) ? ((id1 << 16) + id0) : ((id0 << 16) + id1);
			int result;
			if (indexLookup.TryGetValue(key, out result))
			{
				return result;
			}
			vertices[vertIndex] = (vertices[id0] + vertices[id1]) * 0.5f;
			normals[vertIndex] = Vector3.up;
			uvs[vertIndex] = (uvs[id0] + uvs[id1]) * 0.5f;
			indexLookup.Add(key, vertIndex);
			return vertIndex;
		}

		// Token: 0x0600431C RID: 17180 RVA: 0x0015F918 File Offset: 0x0015DD18
		private static int GetVertCount(int subdivision)
		{
			return TrianglePrimitive.GetTriCount(subdivision) * 3;
		}

		// Token: 0x0600431D RID: 17181 RVA: 0x0015F922 File Offset: 0x0015DD22
		private static int GetTriCount(int subdivision)
		{
			return (int)Mathf.Pow(4f, (float)subdivision);
		}
	}
}
