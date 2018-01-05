using System;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro.Primitives
{
	// Token: 0x0200087E RID: 2174
	public class SpherePrimitive : Primitive
	{
		// Token: 0x0600430F RID: 17167 RVA: 0x0015D510 File Offset: 0x0015B910
		public static float GenerateGeometry(Mesh mesh, float radius, int segments, float hemisphere, float innerRadius, float slice, NormalsType normalsType, PivotPosition pivotPosition)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			radius = Mathf.Clamp(radius, 0f, 100f);
			segments = Mathf.Clamp(segments, 4, 100);
			hemisphere = Mathf.Clamp(hemisphere, 0f, 1f);
			innerRadius = Mathf.Clamp01(innerRadius) * radius;
			slice = Mathf.Clamp(slice, 0f, 360f);
			mesh.Clear();
			int num = segments - 1;
			int num2 = segments;
			slice /= 360f;
			if ((int)((float)num2 * (1f - slice)) == 0)
			{
			}
			float num3 = -1f + hemisphere * 2f;
			int num4 = num;
			float num5 = -radius;
			float num6 = 1f / (float)(num - 1);
			float num7 = 1f / (float)(num2 - 1);
			float num8 = 0f;
			for (int i = 0; i < num; i++)
			{
				float num9 = Mathf.Cos(-6.28318548f + 3.14159274f * (float)i * num6);
				if (num9 < num3)
				{
					num4 = i;
					num5 = num8 * radius;
					break;
				}
				num8 = num9;
			}
			int num10 = num4 * num2 * 6;
			int num11 = segments + 1;
			int num12 = segments * 3;
			if (num4 == num)
			{
				num10 -= num2 * 3;
			}
			int num13;
			if (normalsType == NormalsType.Vertex)
			{
				num13 = (num4 + 1) * (num2 + 1);
			}
			else
			{
				num13 = num10;
			}
			if (num4 == num)
			{
				num13 -= num2 + 1;
			}
			if (innerRadius > 0f && num4 < num)
			{
				num13 *= 2;
				num10 *= 2;
				num13 += num2 * 2;
				num10 += num2 * 3;
			}
			Vector3 zero = Vector3.zero;
			float num14 = radius - num5;
			if (pivotPosition != PivotPosition.Botttom)
			{
				if (pivotPosition != PivotPosition.Top)
				{
					if (pivotPosition == PivotPosition.Center)
					{
						zero = new Vector3(0f, num5 + num14 / 2f, 0f);
					}
				}
				else
				{
					zero = new Vector3(0f, num5, 0f);
				}
			}
			else
			{
				zero = new Vector3(0f, radius, 0f);
			}
			if (num13 + num11 > 60000)
			{
				UnityEngine.Debug.LogError("Too much vertices!");
				return 0f;
			}
			Vector3[] array = new Vector3[num13 + num11];
			Vector3[] array2 = new Vector3[num13 + num11];
			Vector2[] array3 = new Vector2[num13 + num11];
			int[] array4 = new int[num10 + num12];
			int num15 = 0;
			int num16 = 0;
			for (int j = 0; j < num4; j++)
			{
				float y = -Mathf.Cos(-6.28318548f + 3.14159274f * (float)j * num6);
				float num17 = Mathf.Sin(3.14159274f * (float)j * num6);
				for (int k = 0; k < num2; k++)
				{
					float x = Mathf.Sin(6.28318548f * (float)k * num7) * num17;
					float z = Mathf.Cos(6.28318548f * (float)k * num7) * num17;
					array[num15] = new Vector3(x, y, z) * radius + zero;
					array2[num15] = new Vector3(x, y, z);
					array3[num15] = new Vector2(1f - (float)k * num7, (float)j * num6);
					if (j < num4 - 1 && k < num2 - 1)
					{
						array4[num16 + 5] = (j + 1) * num2 + k;
						array4[num16 + 4] = j * num2 + (k + 1);
						array4[num16 + 3] = j * num2 + k;
						array4[num16 + 2] = (j + 1) * num2 + (k + 1);
						array4[num16 + 1] = j * num2 + (k + 1);
						array4[num16] = (j + 1) * num2 + k;
						num16 += 6;
					}
					num15++;
				}
			}
			int num18 = num15;
			if (num4 < num)
			{
				if (innerRadius > 0f)
				{
					int num19 = num18;
					float num20 = -Mathf.Cos(-6.28318548f + 3.14159274f * (float)(num4 - 1) * num6) * radius;
					float num21 = -Mathf.Cos(-6.28318548f + 3.14159274f * (float)(num4 - 1) * num6) * innerRadius;
					Vector3 b = new Vector3(0f, num20 - num21, 0f);
					for (int l = 0; l < num4; l++)
					{
						float y2 = -Mathf.Cos(-6.28318548f + 3.14159274f * (float)l * num6);
						float num22 = Mathf.Sin(3.14159274f * (float)l * num6);
						for (int m = 0; m < num2; m++)
						{
							float x2 = Mathf.Sin(6.28318548f * (float)m * num7) * num22;
							float z2 = Mathf.Cos(6.28318548f * (float)m * num7) * num22;
							array[num15] = new Vector3(x2, y2, z2) * innerRadius + zero + b;
							array2[num15] = -new Vector3(x2, y2, z2);
							array3[num15] = new Vector2(1f - (float)m * num7, (float)l * num6);
							if (l < num4 - 1 && m < num2 - 1)
							{
								array4[num16] = num19 + (l + 1) * num2 + m;
								array4[num16 + 1] = num19 + l * num2 + (m + 1);
								array4[num16 + 2] = num19 + l * num2 + m;
								array4[num16 + 3] = num19 + (l + 1) * num2 + (m + 1);
								array4[num16 + 4] = num19 + l * num2 + (m + 1);
								array4[num16 + 5] = num19 + (l + 1) * num2 + m;
								num16 += 6;
							}
							num15++;
						}
					}
					num18 = num15;
					if (normalsType == NormalsType.Face)
					{
						MeshUtils.DuplicateSharedVertices(ref array, ref array3, array4, num16);
						num18 = num16;
					}
					float y3 = -Mathf.Cos(-6.28318548f + 3.14159274f * (float)(num4 - 1) * num6);
					float num23 = Mathf.Sin(3.14159274f * (float)(num4 - 1) * num6);
					int num24 = num18;
					num15 = num24;
					for (int n = 0; n < num2; n++)
					{
						float x3 = Mathf.Sin(6.28318548f * (float)n * num7) * num23;
						float z3 = Mathf.Cos(6.28318548f * (float)n * num7) * num23;
						Vector3 a = new Vector3(x3, y3, z3);
						a.Normalize();
						array[num15] = a * radius + zero;
						array[num15 + 1] = a * innerRadius + zero + b;
						array2[num15] = new Vector3(0f, 1f, 0f);
						array2[num15 + 1] = new Vector3(0f, 1f, 0f);
						Vector2 b2 = new Vector2(a.x * 0.5f, a.z * 0.5f);
						Vector2 a2 = new Vector2(0.5f, 0.5f);
						Vector2 b3 = new Vector2(a.x * innerRadius / radius * 0.5f, a.z * innerRadius / radius * 0.5f);
						array3[num15] = a2 + b2;
						array3[num15 + 1] = a2 + b3;
						num15 += 2;
					}
					for (int num25 = 0; num25 < num; num25++)
					{
						array4[num16 + 2] = num24 + 1;
						array4[num16 + 1] = num24 + 2;
						array4[num16] = num24;
						array4[num16 + 4] = num24 + 3;
						array4[num16 + 5] = num24 + 1;
						array4[num16 + 3] = num24 + 2;
						num16 += 6;
						num24 += 2;
					}
				}
				else
				{
					if (normalsType == NormalsType.Face)
					{
						MeshUtils.DuplicateSharedVertices(ref array, ref array3, array4, num16);
						num18 = num16;
					}
					int num26 = num18;
					num15 = num18;
					float y4 = -Mathf.Cos(-6.28318548f + 3.14159274f * (float)(num4 - 1) * num6);
					float num27 = Mathf.Sin(3.14159274f * (float)(num4 - 1) * num6);
					for (int num28 = 0; num28 < num2; num28++)
					{
						float x4 = Mathf.Sin(6.28318548f * (float)num28 * num7) * num27;
						float z4 = Mathf.Cos(6.28318548f * (float)num28 * num7) * num27;
						Vector3 a3 = new Vector3(x4, y4, z4);
						array[num15] = a3 * radius + zero;
						array2[num15] = new Vector3(0f, 1f, 0f);
						Vector2 b4 = new Vector2(a3.x * 0.5f, a3.z * 0.5f);
						Vector2 a4 = new Vector2(0.5f, 0.5f);
						array3[num15] = a4 + b4;
						num15++;
					}
					array[num15] = new Vector3(0f, -num5, 0f) + zero;
					array2[num15] = new Vector3(0f, 1f, 0f);
					array3[num15] = new Vector2(0.5f, 0.5f);
					num15++;
					for (int num29 = 0; num29 < num; num29++)
					{
						array4[num16 + 2] = num26;
						array4[num16 + 1] = num15 - 1;
						if (num29 == num - 1)
						{
							array4[num16] = num18;
						}
						else
						{
							array4[num16] = num26 + 1;
						}
						num16 += 3;
						num26++;
					}
				}
			}
			else if (normalsType == NormalsType.Face)
			{
				MeshUtils.DuplicateSharedVertices(ref array, ref array3, array4, num16);
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
