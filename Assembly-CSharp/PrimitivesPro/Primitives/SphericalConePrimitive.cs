using System;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro.Primitives
{
	// Token: 0x0200087F RID: 2175
	public class SphericalConePrimitive : Primitive
	{
		// Token: 0x06004311 RID: 17169 RVA: 0x0015DF5C File Offset: 0x0015C35C
		public static float GenerateGeometry(Mesh mesh, float radius, int segments, float coneAngle, NormalsType normalsType, PivotPosition pivotPosition)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			radius = Mathf.Clamp(radius, 0f, 100f);
			segments = Mathf.Clamp(segments, 4, 100);
			coneAngle = Mathf.Clamp(coneAngle, 0f, 360f);
			float num = 1f - coneAngle / 360f;
			mesh.Clear();
			int num2 = segments - 1;
			int num3 = segments;
			float num4 = -1f + num * 2f;
			int num5 = num2;
			float num6 = 1f / (float)(num2 - 1);
			float num7 = 1f / (float)(num3 - 1);
			for (int i = 0; i < num2; i++)
			{
				float num8 = Mathf.Cos(-6.28318548f + 3.14159274f * (float)i * num6);
				if (num8 < num4)
				{
					num5 = i;
					break;
				}
			}
			int num9 = num5 * num3 * 6;
			int num10 = segments + 1;
			int num11 = segments * 3;
			if (num5 == num2)
			{
				num9 -= num3 * 3;
			}
			int num12;
			if (normalsType == NormalsType.Vertex)
			{
				num12 = (num5 + 1) * (num3 + 1);
			}
			else
			{
				num12 = num9;
			}
			if (num5 == num2)
			{
				num12 -= num3 + 1;
			}
			Vector3 zero = Vector3.zero;
			if (pivotPosition != PivotPosition.Botttom)
			{
				if (pivotPosition != PivotPosition.Top)
				{
					if (pivotPosition == PivotPosition.Center)
					{
						zero = Vector3.zero;
					}
				}
				else
				{
					zero = new Vector3(0f, -radius, 0f);
				}
			}
			else
			{
				zero = new Vector3(0f, radius, 0f);
			}
			if (num12 + num10 > 60000)
			{
				UnityEngine.Debug.LogError("Too much vertices!");
				return 0f;
			}
			Vector3[] array = new Vector3[num12 + num10];
			Vector3[] array2 = new Vector3[num12 + num10];
			Vector2[] array3 = new Vector2[num12 + num10];
			int[] array4 = new int[num9 + num11];
			int num13 = 0;
			int num14 = 0;
			for (int j = 0; j < num5; j++)
			{
				float y = -Mathf.Cos(-6.28318548f + 3.14159274f * (float)j * num6);
				float num15 = Mathf.Sin(3.14159274f * (float)j * num6);
				for (int k = 0; k < num3; k++)
				{
					float x = Mathf.Sin(6.28318548f * (float)k * num7) * num15;
					float z = Mathf.Cos(6.28318548f * (float)k * num7) * num15;
					array[num13] = new Vector3(x, y, z) * radius + zero;
					array2[num13] = new Vector3(x, y, z);
					array3[num13] = new Vector2(1f - (float)k * num7, (float)j * num6);
					if (j < num5 - 1 && k < num3 - 1)
					{
						array4[num14 + 5] = (j + 1) * num3 + k;
						array4[num14 + 4] = j * num3 + (k + 1);
						array4[num14 + 3] = j * num3 + k;
						array4[num14 + 2] = (j + 1) * num3 + (k + 1);
						array4[num14 + 1] = j * num3 + (k + 1);
						array4[num14] = (j + 1) * num3 + k;
						num14 += 6;
					}
					num13++;
				}
			}
			int num16 = num13;
			if (num5 < num2)
			{
				if (normalsType == NormalsType.Face)
				{
					MeshUtils.DuplicateSharedVertices(ref array, ref array3, array4, num14);
					num16 = num14;
				}
				int num17 = num16;
				num13 = num16;
				float y2 = -Mathf.Cos(-6.28318548f + 3.14159274f * (float)(num5 - 1) * num6);
				float num18 = Mathf.Sin(3.14159274f * (float)(num5 - 1) * num6);
				int num19 = 0;
				for (int l = 0; l < num3; l++)
				{
					float x2 = Mathf.Sin(6.28318548f * (float)l * num7) * num18;
					float z2 = Mathf.Cos(6.28318548f * (float)l * num7) * num18;
					Vector3 a = new Vector3(x2, y2, z2);
					array[num13] = a * radius + zero;
					if (l > 0)
					{
						array2[num13] = -MeshUtils.ComputePolygonNormal(zero, array[num13], array[num13 - 1]);
						if (l == num3 - 1)
						{
							array2[num19] = MeshUtils.ComputePolygonNormal(zero, array[num19], array[num19 + 1]);
						}
					}
					else
					{
						num19 = num13;
					}
					Vector2 b = new Vector2(a.x * 0.5f, a.z * 0.5f);
					Vector2 a2 = new Vector2(0.5f, 0.5f);
					array3[num13] = a2 + b;
					num13++;
				}
				array[num13] = zero;
				array2[num13] = new Vector3(0f, 1f, 0f);
				array3[num13] = new Vector2(0.5f, 0.5f);
				num13++;
				for (int m = 0; m < num2; m++)
				{
					array4[num14 + 2] = num17;
					array4[num14 + 1] = num13 - 1;
					if (m == num2 - 1)
					{
						array4[num14] = num16;
					}
					else
					{
						array4[num14] = num17 + 1;
					}
					num14 += 3;
					num17++;
				}
			}
			mesh.vertices = array;
			mesh.uv = array3;
			mesh.triangles = array4;
			if (normalsType == NormalsType.Face)
			{
				mesh.RecalculateNormals();
			}
			else
			{
				mesh.normals = array2;
			}
			mesh.RecalculateBounds();
			MeshUtils.CalculateTangents(mesh);
			stopwatch.Stop();
			return (float)stopwatch.ElapsedMilliseconds;
		}
	}
}
