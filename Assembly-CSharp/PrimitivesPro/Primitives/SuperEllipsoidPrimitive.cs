using System;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro.Primitives
{
	// Token: 0x02000880 RID: 2176
	public class SuperEllipsoidPrimitive : Primitive
	{
		// Token: 0x06004313 RID: 17171 RVA: 0x0015E534 File Offset: 0x0015C934
		public static float GenerateGeometry(Mesh mesh, float width, float height, float length, int segments, float n1, float n2, NormalsType normalsType, PivotPosition pivotPosition)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			width = Mathf.Clamp(width, 0f, 100f);
			length = Mathf.Clamp(length, 0f, 100f);
			height = Mathf.Clamp(height, 0f, 100f);
			segments = Mathf.Clamp(segments, 1, 100);
			n1 = Mathf.Clamp(n1, 0.01f, 5f);
			n2 = Mathf.Clamp(n2, 0.01f, 5f);
			mesh.Clear();
			segments = segments * 4 - 1;
			segments += 5;
			int num = (segments + 1) * (segments / 2 + 1);
			int num2 = segments * (segments / 2) * 6;
			if (normalsType == NormalsType.Face)
			{
				num = num2;
			}
			if (num > 60000)
			{
				UnityEngine.Debug.LogError("Too much vertices!");
				return 0f;
			}
			Vector3[] array = new Vector3[num];
			Vector2[] array2 = new Vector2[num];
			int[] array3 = new int[num2];
			Vector3 zero = Vector3.zero;
			if (pivotPosition != PivotPosition.Botttom)
			{
				if (pivotPosition == PivotPosition.Top)
				{
					zero = new Vector3(0f, -height, 0f);
				}
			}
			else
			{
				zero = new Vector3(0f, height, 0f);
			}
			int num3 = 0;
			for (int i = 0; i <= segments / 2; i++)
			{
				for (int j = 0; j <= segments; j++)
				{
					int num4 = i * (segments + 1) + j;
					float f = (float)j * 2f * 3.14159274f / (float)segments;
					float f2 = -1.57079637f + 3.14159274f * (float)i / ((float)segments / 2f);
					array[num4].x = SuperEllipsoidPrimitive.RPower(Mathf.Cos(f2), n1) * SuperEllipsoidPrimitive.RPower(Mathf.Cos(f), n2) * width;
					array[num4].z = SuperEllipsoidPrimitive.RPower(Mathf.Cos(f2), n1) * SuperEllipsoidPrimitive.RPower(Mathf.Sin(f), n2) * length;
					array[num4].y = SuperEllipsoidPrimitive.RPower(Mathf.Sin(f2), n1) * height;
					array2[num4].x = Mathf.Atan2(array[num4].z, array[num4].x) / 6.28318548f;
					if (array2[num4].x < 0f)
					{
						array2[num4].x = 1f + array2[num4].x;
					}
					array2[num4].y = 0.5f + Mathf.Atan2(array[num4].y, Mathf.Sqrt(array[num4].x * array[num4].x + array[num4].z * array[num4].z)) / 3.14159274f;
					if (i == 0)
					{
						array[num4].x = 0f;
						array[num4].y = -height;
						array[num4].z = 0f;
						array2[num4].y = 0f;
						array2[num4].x = 0f;
					}
					if (i == segments / 2)
					{
						array[num4].x = 0f;
						array[num4].y = height;
						array[num4].z = 0f;
						array2[num4].y = 1f;
						array2[num4].x = array2[(i - 1) * (segments + 1) + j].x;
					}
					if (j == segments)
					{
						array[num4].x = array[i * (segments + 1)].x;
						array[num4].z = array[i * (segments + 1)].z;
						array2[num4].x = 1f;
					}
					array[num4] += zero;
					if (num3 < num4)
					{
						num3 = num4;
					}
				}
			}
			for (int k = 0; k <= segments; k++)
			{
				int num5 = segments + 1 + k;
				array2[k].x = array2[num5].x;
			}
			int num6 = 0;
			for (int l = 0; l < segments / 2; l++)
			{
				for (int m = 0; m < segments; m++)
				{
					int num7 = l * (segments + 1) + m;
					int num8 = l * (segments + 1) + (m + 1);
					int num9 = (l + 1) * (segments + 1) + (m + 1);
					int num10 = (l + 1) * (segments + 1) + m;
					array3[num6] = num9;
					array3[num6 + 1] = num8;
					array3[num6 + 2] = num7;
					array3[num6 + 3] = num10;
					array3[num6 + 4] = num9;
					array3[num6 + 5] = num7;
					num6 += 6;
				}
			}
			if (normalsType == NormalsType.Face)
			{
				MeshUtils.DuplicateSharedVertices(ref array, ref array2, array3, -1);
			}
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.triangles = array3;
			if (normalsType == NormalsType.Vertex)
			{
				Vector3[] array4 = null;
				MeshUtils.ComputeVertexNormals(array, array3, out array4);
				for (int num11 = 0; num11 < segments / 2; num11++)
				{
					int num12 = num11 * (segments + 1);
					int num13 = num11 * (segments + 1) + segments;
					array4[num13] = array4[num12];
				}
				mesh.normals = array4;
			}
			else
			{
				mesh.RecalculateNormals();
			}
			mesh.RecalculateBounds();
			MeshUtils.CalculateTangents(mesh);
			stopwatch.Stop();
			return (float)stopwatch.ElapsedMilliseconds;
		}

		// Token: 0x06004314 RID: 17172 RVA: 0x0015EB0B File Offset: 0x0015CF0B
		private static float RPower(float v, float n)
		{
			if (v >= 0f)
			{
				return Mathf.Pow(v, n);
			}
			return -Mathf.Pow(-v, n);
		}
	}
}
