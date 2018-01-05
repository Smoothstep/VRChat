using System;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro.Primitives
{
	// Token: 0x02000884 RID: 2180
	public class TubePrimitive : Primitive
	{
		// Token: 0x0600431F RID: 17183 RVA: 0x0015F93C File Offset: 0x0015DD3C
		public static float GenerateGeometry(Mesh mesh, float radius0, float radius1, float height, int sides, int heightSegments, float slice, bool radialMapping, NormalsType normalsType, PivotPosition pivotPosition)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			radius0 = Mathf.Clamp(radius0, 0f, 100f);
			radius1 = Mathf.Clamp(radius1, 0f, 100f);
			height = Mathf.Clamp(height, 0f, 1000f);
			sides = Mathf.Clamp(sides, 3, 100);
			heightSegments = Mathf.Clamp(heightSegments, 1, 100);
			slice = Mathf.Clamp(slice, 0f, 360f);
			slice /= 360f;
			mesh.Clear();
			heightSegments = Mathf.Clamp(heightSegments, 1, 250);
			sides = Mathf.Clamp(sides, 3, 250);
			int num = (int)((float)sides * (1f - slice));
			float num2 = (1f - slice) * 2f * 3.14159274f;
			if (num == 0)
			{
				num = 1;
			}
			int num3 = num * 6 * heightSegments * 2;
			int num4 = (num + 1) * 2 * 2;
			int num5 = num * 6 * 2;
			int num6 = 0;
			int num7 = 0;
			if (num < sides)
			{
				num6 = 12;
				num7 = 8;
			}
			int num8;
			if (normalsType == NormalsType.Face)
			{
				num8 = num * (4 + (heightSegments - 1) * 2) * 2;
			}
			else
			{
				num8 = (num + 1) * (heightSegments + 1) * 2;
			}
			if (num8 + num4 > 60000)
			{
				UnityEngine.Debug.LogError("Too much vertices!");
				return 0f;
			}
			Vector3[] array = new Vector3[num8 + num4 + num7];
			Vector3[] array2 = new Vector3[num8 + num4 + num7];
			Vector2[] array3 = new Vector2[num8 + num4 + num7];
			int[] array4 = new int[num3 + num5 + num6];
			Vector3 zero = Vector3.zero;
			int num9 = num8 / 2;
			int num10 = num3 / 2;
			Vector3 zero2 = Vector3.zero;
			if (pivotPosition != PivotPosition.Center)
			{
				if (pivotPosition == PivotPosition.Top)
				{
					zero2 = new Vector3(0f, -height, 0f);
				}
			}
			else
			{
				zero2 = new Vector3(0f, -height / 2f, 0f);
			}
			if (normalsType == NormalsType.Face)
			{
				int num11 = 0;
				int num12 = 0;
				float num13 = height / (float)heightSegments;
				for (int i = 0; i < num; i++)
				{
					float f = (float)i / (float)sides * 3.14159274f * 2f;
					Vector3 vector = new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f));
					Vector3 normalized = vector.normalized;
					float f2 = (float)(i + 1) / (float)sides * 3.14159274f * 2f;
					if (i + 1 == num)
					{
						f2 = num2;
					}
					Vector3 vector2 = new Vector3(Mathf.Cos(f2), 0f, Mathf.Sin(f2));
					Vector3 normalized2 = vector2.normalized;
					float num14 = 0f;
					int num15 = num11;
					Vector3 normalized3 = (normalized + normalized2).normalized;
					for (int j = 0; j <= heightSegments; j++)
					{
						array[num11] = zero + normalized * radius1 + new Vector3(0f, num14, 0f) + zero2;
						array[num11 + 1] = zero + normalized2 * radius1 + new Vector3(0f, num14, 0f) + zero2;
						array2[num11] = normalized3;
						array2[num11 + 1] = normalized3;
						array3[num11] = new Vector2((float)i / (float)sides, (float)j / (float)heightSegments);
						array3[num11 + 1] = new Vector2((float)(i + 1) / (float)sides, (float)j / (float)heightSegments);
						array[num11 + num9] = zero + normalized * radius0 + new Vector3(0f, num14, 0f) + zero2;
						array[num11 + num9 + 1] = zero + normalized2 * radius0 + new Vector3(0f, num14, 0f) + zero2;
						array2[num11 + num9] = -normalized3;
						array2[num11 + num9 + 1] = -normalized3;
						array3[num11 + num9] = new Vector2((float)i / (float)sides, (float)j / (float)heightSegments);
						array3[num11 + num9 + 1] = new Vector2((float)(i + 1) / (float)sides, (float)j / (float)heightSegments);
						num11 += 2;
						num14 += num13;
					}
					for (int k = 0; k < heightSegments; k++)
					{
						array4[num12] = num15;
						array4[num12 + 1] = num15 + 2;
						array4[num12 + 2] = num15 + 1;
						array4[num12 + 3] = num15 + 2;
						array4[num12 + 4] = num15 + 3;
						array4[num12 + 5] = num15 + 1;
						array4[num12 + num10] = num15 + num9 + 1;
						array4[num12 + num10 + 1] = num15 + num9 + 2;
						array4[num12 + num10 + 2] = num15 + num9;
						array4[num12 + num10 + 3] = num15 + num9 + 1;
						array4[num12 + num10 + 4] = num15 + num9 + 3;
						array4[num12 + num10 + 5] = num15 + num9 + 2;
						num12 += 6;
						num15 += 2;
					}
				}
			}
			else
			{
				int num16 = 0;
				int num17 = 0;
				int num18 = 0;
				float num19 = height / (float)heightSegments;
				for (int l = 0; l <= num; l++)
				{
					float f3 = (float)l / (float)sides * 3.14159274f * 2f;
					if (l == num)
					{
						f3 = num2;
					}
					Vector3 vector3 = new Vector3(Mathf.Cos(f3), 0f, Mathf.Sin(f3));
					Vector3 normalized4 = vector3.normalized;
					float num20 = 0f;
					for (int m = 0; m <= heightSegments; m++)
					{
						array[num16] = zero + normalized4 * radius1 + new Vector3(0f, num20, 0f) + zero2;
						array2[num16] = normalized4;
						array3[num16] = new Vector2((float)l / (float)sides, (float)m / (float)heightSegments);
						array[num16 + num9] = zero + normalized4 * radius0 + new Vector3(0f, num20, 0f) + zero2;
						array2[num16 + num9] = -normalized4;
						array3[num16 + num9] = new Vector2((float)l / (float)sides, (float)m / (float)heightSegments);
						num16++;
						num20 += num19;
					}
				}
				for (int n = 0; n < num; n++)
				{
					int num21 = (n + 1) * (heightSegments + 1);
					for (int num22 = 0; num22 < heightSegments; num22++)
					{
						array4[num17] = num18;
						array4[num17 + 1] = num18 + 1;
						array4[num17 + 2] = num21;
						array4[num17 + 3] = num21;
						array4[num17 + 4] = num18 + 1;
						array4[num17 + 5] = num21 + 1;
						array4[num17 + num10] = num21 + num9;
						array4[num17 + num10 + 1] = num18 + num9 + 1;
						array4[num17 + num10 + 2] = num18 + num9;
						array4[num17 + num10 + 3] = num21 + num9 + 1;
						array4[num17 + num10 + 4] = num18 + num9 + 1;
						array4[num17 + num10 + 5] = num21 + num9;
						num17 += 6;
						num18++;
						num21++;
					}
					num18++;
				}
			}
			int num23 = num8;
			int num24 = num3;
			int num25 = num23;
			int num26 = num4 / 2;
			int num27 = num5 / 2;
			float num28 = 0.5f * (radius0 / radius1);
			for (int num29 = 0; num29 <= num; num29++)
			{
				float f4 = (float)num29 / (float)sides * 3.14159274f * 2f;
				if (num29 == num)
				{
					f4 = num2;
				}
				Vector3 vector4 = new Vector3(Mathf.Cos(f4), 0f, Mathf.Sin(f4));
				Vector3 normalized5 = vector4.normalized;
				array[num23] = zero + normalized5 * radius0 + new Vector3(0f, height, 0f) + zero2;
				array2[num23] = new Vector3(0f, 1f, 0f);
				array[num23 + 1] = zero + normalized5 * radius1 + new Vector3(0f, height, 0f) + zero2;
				array2[num23 + 1] = new Vector3(0f, 1f, 0f);
				Vector2 b = new Vector2(normalized5.x * 0.5f, normalized5.z * 0.5f);
				Vector2 b2 = new Vector2(normalized5.x * num28, normalized5.z * num28);
				Vector2 a = new Vector2(0.5f, 0.5f);
				float x = (float)num29 / (float)sides;
				if (radialMapping)
				{
					array3[num23] = new Vector2(x, 1f);
					array3[num23 + 1] = new Vector2(x, 0f);
				}
				else
				{
					array3[num23] = a + b2;
					array3[num23 + 1] = a + b;
				}
				array[num23 + num26] = zero + normalized5 * radius0 + zero2;
				array2[num23 + num26] = new Vector3(0f, -1f, 0f);
				array[num23 + num26 + 1] = zero + normalized5 * radius1 + zero2;
				array2[num23 + num26 + 1] = new Vector3(0f, -1f, 0f);
				if (radialMapping)
				{
					array3[num23 + num26] = new Vector2(x, 1f);
					array3[num23 + num26 + 1] = new Vector2(x, 0f);
				}
				else
				{
					array3[num23 + num26] = a + b2;
					array3[num23 + num26 + 1] = a + b;
				}
				num23 += 2;
			}
			for (int num30 = 0; num30 < num; num30++)
			{
				int num31 = num8 + (num30 + 1) * 2;
				int num32 = num8 + num26 + (num30 + 1) * 2;
				array4[num24] = num31;
				array4[num24 + 1] = num25 + 1;
				array4[num24 + 2] = num25;
				array4[num24 + 3] = num31 + 1;
				array4[num24 + 4] = num25 + 1;
				array4[num24 + 5] = num31;
				array4[num24 + num27] = num25 + num26;
				array4[num24 + num27 + 1] = num25 + num26 + 1;
				array4[num24 + num27 + 2] = num32;
				array4[num24 + num27 + 3] = num32;
				array4[num24 + num27 + 4] = num25 + num26 + 1;
				array4[num24 + num27 + 5] = num32 + 1;
				num24 += 6;
				num25 += 2;
			}
			if (num < sides)
			{
				int num33 = num3 + num5;
				int num34 = num8 + num4;
				if (normalsType == NormalsType.Vertex)
				{
					array[num34] = array[0];
					array[num34 + 1] = array[num9];
					array[num34 + 2] = array[heightSegments];
					array[num34 + 3] = array[num9 + heightSegments];
					array[num34 + 4] = array[num * (heightSegments + 1) + heightSegments];
					array[num34 + 5] = array[num * (heightSegments + 1) + num9];
					array[num34 + 6] = array[num * (heightSegments + 1)];
					array[num34 + 7] = array[num * (heightSegments + 1) + num9 + heightSegments];
				}
				else
				{
					array[num34] = array[0];
					array[num34 + 1] = array[num9];
					array[num34 + 2] = array[heightSegments * 2];
					array[num34 + 3] = array[num9 + heightSegments * 2];
					array[num34 + 4] = array[(num - 1) * ((heightSegments + 1) * 2) + heightSegments * 2 + 1];
					array[num34 + 5] = array[(num - 1) * ((heightSegments + 1) * 2) + num9 + 1];
					array[num34 + 6] = array[(num - 1) * ((heightSegments + 1) * 2) + 1];
					array[num34 + 7] = array[(num - 1) * ((heightSegments + 1) * 2) + num9 + heightSegments * 2 + 1];
				}
				array4[num33] = num34;
				array4[num33 + 1] = num34 + 1;
				array4[num33 + 2] = num34 + 2;
				array4[num33 + 3] = num34 + 3;
				array4[num33 + 4] = num34 + 2;
				array4[num33 + 5] = num34 + 1;
				array4[num33 + 6] = num34 + 4;
				array4[num33 + 7] = num34 + 5;
				array4[num33 + 8] = num34 + 6;
				array4[num33 + 9] = num34 + 7;
				array4[num33 + 10] = num34 + 5;
				array4[num33 + 11] = num34 + 4;
				Vector3 vector5 = Vector3.Cross(array[num34 + 1] - array[num34], array[num34 + 2] - array[num34]);
				array2[num34] = vector5;
				array2[num34 + 1] = vector5;
				array2[num34 + 2] = vector5;
				array2[num34 + 3] = vector5;
				Vector3 vector6 = Vector3.Cross(array[num34 + 5] - array[num34 + 4], array[num34 + 6] - array[num34 + 4]);
				array2[num34 + 4] = vector6;
				array2[num34 + 5] = vector6;
				array2[num34 + 6] = vector6;
				array2[num34 + 7] = vector6;
				array3[num34] = new Vector2(1f, 1f);
				array3[num34 + 1] = new Vector2(0f, 1f);
				array3[num34 + 2] = new Vector2(1f, 0f);
				array3[num34 + 3] = new Vector2(0f, 0f);
				array3[num34 + 4] = new Vector2(1f, 1f);
				array3[num34 + 5] = new Vector2(0f, 0f);
				array3[num34 + 6] = new Vector2(1f, 0f);
				array3[num34 + 7] = new Vector2(0f, 1f);
			}
			mesh.vertices = array;
			mesh.normals = array2;
			mesh.uv = array3;
			mesh.triangles = array4;
			mesh.RecalculateBounds();
			MeshUtils.CalculateTangents(mesh);
			stopwatch.Stop();
			return (float)stopwatch.ElapsedMilliseconds;
		}
	}
}
