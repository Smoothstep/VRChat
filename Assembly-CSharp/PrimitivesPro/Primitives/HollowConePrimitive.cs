using System;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro.Primitives
{
	// Token: 0x02000876 RID: 2166
	public class HollowConePrimitive : Primitive
	{
		// Token: 0x06004305 RID: 17157 RVA: 0x0015B1AC File Offset: 0x001595AC
		public static float GenerateGeometry(Mesh mesh, float radius0, float radius1, float thickness, float height, int sides, int heightSegments, NormalsType normalsType, PivotPosition pivotPosition)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			radius0 = Mathf.Clamp(radius0, 0f, 100f);
			radius1 = Mathf.Clamp(radius1, 0f, 100f);
			height = Mathf.Clamp(height, 0f, 100f);
			sides = Mathf.Clamp(sides, 3, 100);
			heightSegments = Mathf.Clamp(heightSegments, 1, 100);
			float num = 0f;
			num /= 360f;
			mesh.Clear();
			heightSegments = Mathf.Clamp(heightSegments, 1, 250);
			sides = Mathf.Clamp(sides, 3, 250);
			float num2 = Mathf.Min(radius0, radius1);
			if (thickness > num2)
			{
				thickness = num2;
			}
			float d = radius0 - thickness;
			bool flag = false;
			int num3 = (int)((float)sides * (1f - num));
			if (num3 == 0)
			{
				num3 = 1;
			}
			int num4 = num3 * 6 * heightSegments * 2;
			int num5 = (num3 + 1) * 2 * 2;
			int num6 = num3 * 6 * 2;
			int num7 = 0;
			int num8 = 0;
			if (num3 < sides)
			{
				num7 = 12;
				num8 = 8;
			}
			int num9;
			if (normalsType == NormalsType.Face)
			{
				num9 = num3 * (4 + (heightSegments - 1) * 2) * 2;
			}
			else
			{
				num9 = (num3 + 1) * (heightSegments + 1) * 2;
			}
			if (num9 + num5 > 60000)
			{
				UnityEngine.Debug.LogError("Too much vertices!");
				return 0f;
			}
			Vector3[] array = new Vector3[num9 + num5 + num8];
			Vector3[] array2 = new Vector3[num9 + num5 + num8];
			Vector2[] array3 = new Vector2[num9 + num5 + num8];
			int[] array4 = new int[num4 + num6 + num7];
			Vector3 zero = Vector3.zero;
			int num10 = num9 / 2;
			int num11 = num4 / 2;
			float magnitude = (new Vector3(zero.x + radius0, zero.y, zero.z) - new Vector3(zero.x + radius1, zero.y + height, zero.z)).magnitude;
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
				int num12 = 0;
				int num13 = 0;
				float num14 = magnitude / (float)heightSegments;
				for (int i = 0; i < num3; i++)
				{
					float f = (float)i / (float)sides * 3.14159274f * 2f;
					Vector3 vector = new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f));
					Vector3 normalized = vector.normalized;
					Vector3 a = zero + new Vector3(0f, height, 0f) + normalized * radius1 - (zero + normalized * radius0);
					a.Normalize();
					float f2 = (float)(i + 1) / (float)sides * 3.14159274f * 2f;
					Vector3 vector2 = new Vector3(Mathf.Cos(f2), 0f, Mathf.Sin(f2));
					Vector3 normalized2 = vector2.normalized;
					Vector3 a2 = zero + new Vector3(0f, height, 0f) + normalized2 * radius1 - (zero + normalized2 * radius0);
					a2.Normalize();
					float num15 = 0f;
					int num16 = num12;
					Vector3 normalized3 = (normalized + normalized2).normalized;
					for (int j = 0; j <= heightSegments; j++)
					{
						array[num12] = zero + normalized * radius0 + a * num15 + zero2;
						array[num12 + 1] = zero + normalized2 * radius0 + a2 * num15 + zero2;
						array2[num12] = normalized3;
						array2[num12 + 1] = normalized3;
						array3[num12] = new Vector2((float)i / (float)sides, (float)j / (float)heightSegments);
						array3[num12 + 1] = new Vector2((float)(i + 1) / (float)sides, (float)j / (float)heightSegments);
						array[num12 + num10] = zero + normalized * d + a * num15 + zero2;
						array[num12 + num10 + 1] = zero + normalized2 * d + a2 * num15 + zero2;
						array2[num12 + num10] = -normalized3;
						array2[num12 + num10 + 1] = -normalized3;
						array3[num12 + num10] = new Vector2((float)i / (float)sides, (float)j / (float)heightSegments);
						array3[num12 + num10 + 1] = new Vector2((float)(i + 1) / (float)sides, (float)j / (float)heightSegments);
						num12 += 2;
						num15 += num14;
					}
					for (int k = 0; k < heightSegments; k++)
					{
						array4[num13] = num16;
						array4[num13 + 1] = num16 + 2;
						array4[num13 + 2] = num16 + 1;
						array4[num13 + 3] = num16 + 2;
						array4[num13 + 4] = num16 + 3;
						array4[num13 + 5] = num16 + 1;
						array4[num13 + num11] = num16 + num10 + 1;
						array4[num13 + num11 + 1] = num16 + num10 + 2;
						array4[num13 + num11 + 2] = num16 + num10;
						array4[num13 + num11 + 3] = num16 + num10 + 1;
						array4[num13 + num11 + 4] = num16 + num10 + 3;
						array4[num13 + num11 + 5] = num16 + num10 + 2;
						num13 += 6;
						num16 += 2;
					}
				}
			}
			else
			{
				int num17 = 0;
				int num18 = 0;
				int num19 = 0;
				float num20 = magnitude / (float)heightSegments;
				for (int l = 0; l <= num3; l++)
				{
					float f3 = (float)l / (float)sides * 3.14159274f * 2f;
					Vector3 vector3 = new Vector3(Mathf.Cos(f3), 0f, Mathf.Sin(f3));
					Vector3 normalized4 = vector3.normalized;
					Vector3 a3 = zero + new Vector3(0f, height, 0f) + normalized4 * radius1 - (zero + normalized4 * radius0);
					a3.Normalize();
					float num21 = 0f;
					for (int m = 0; m <= heightSegments; m++)
					{
						array[num17] = zero + normalized4 * radius0 + a3 * num21 + zero2;
						array2[num17] = normalized4;
						array3[num17] = new Vector2((float)l / (float)sides, (float)m / (float)heightSegments);
						array[num17 + num10] = zero + normalized4 * d + a3 * num21 + zero2;
						array2[num17 + num10] = -normalized4;
						array3[num17 + num10] = new Vector2((float)l / (float)sides, (float)m / (float)heightSegments);
						num17++;
						num21 += num20;
					}
				}
				for (int n = 0; n < num3; n++)
				{
					int num22 = (n + 1) * (heightSegments + 1);
					for (int num23 = 0; num23 < heightSegments; num23++)
					{
						array4[num18] = num19;
						array4[num18 + 1] = num19 + 1;
						array4[num18 + 2] = num22;
						array4[num18 + 3] = num22;
						array4[num18 + 4] = num19 + 1;
						array4[num18 + 5] = num22 + 1;
						array4[num18 + num11] = num22 + num10;
						array4[num18 + num11 + 1] = num19 + num10 + 1;
						array4[num18 + num11 + 2] = num19 + num10;
						array4[num18 + num11 + 3] = num22 + num10 + 1;
						array4[num18 + num11 + 4] = num19 + num10 + 1;
						array4[num18 + num11 + 5] = num22 + num10;
						num18 += 6;
						num19++;
						num22++;
					}
					num19++;
				}
			}
			int num24 = num9;
			int num25 = num4;
			int num26 = num24;
			int num27 = num5 / 2;
			int num28 = num6 / 2;
			float num29 = 0.5f * (radius0 / radius1);
			for (int num30 = 0; num30 <= num3; num30++)
			{
				float f4 = (float)num30 / (float)sides * 3.14159274f * 2f;
				Vector3 vector4 = new Vector3(Mathf.Cos(f4), 0f, Mathf.Sin(f4));
				Vector3 normalized5 = vector4.normalized;
				Vector3 a4 = zero + new Vector3(0f, height, 0f) + normalized5 * radius1 - (zero + normalized5 * radius0);
				a4.Normalize();
				array[num24 + 1] = zero + normalized5 * radius0 + a4 * magnitude + zero2;
				array2[num24 + 1] = new Vector3(0f, 1f, 0f);
				array[num24] = zero + normalized5 * d + a4 * magnitude + zero2;
				array2[num24] = new Vector3(0f, 1f, 0f);
				Vector2 b = new Vector2(normalized5.x * 0.5f, normalized5.z * 0.5f);
				Vector2 b2 = new Vector2(normalized5.x * num29, normalized5.z * num29);
				Vector2 a5 = new Vector2(0.5f, 0.5f);
				float x = (float)num30 / (float)sides;
				if (flag)
				{
					array3[num24] = new Vector2(x, 1f);
					array3[num24 + 1] = new Vector2(x, 0f);
				}
				else
				{
					array3[num24] = a5 + b2;
					array3[num24 + 1] = a5 + b;
				}
				array[num24 + num27 + 1] = zero + normalized5 * radius0 + zero2;
				array2[num24 + num27 + 1] = new Vector3(0f, -1f, 0f);
				array[num24 + num27] = zero + normalized5 * d + zero2;
				array2[num24 + num27] = new Vector3(0f, -1f, 0f);
				if (flag)
				{
					array3[num24 + num27] = new Vector2(x, 1f);
					array3[num24 + num27 + 1] = new Vector2(x, 0f);
				}
				else
				{
					array3[num24 + num27] = a5 + b2;
					array3[num24 + num27 + 1] = a5 + b;
				}
				num24 += 2;
			}
			for (int num31 = 0; num31 < num3; num31++)
			{
				int num32 = num9 + (num31 + 1) * 2;
				int num33 = num9 + num27 + (num31 + 1) * 2;
				array4[num25] = num32;
				array4[num25 + 1] = num26 + 1;
				array4[num25 + 2] = num26;
				array4[num25 + 3] = num32 + 1;
				array4[num25 + 4] = num26 + 1;
				array4[num25 + 5] = num32;
				array4[num25 + num28] = num26 + num27;
				array4[num25 + num28 + 1] = num26 + num27 + 1;
				array4[num25 + num28 + 2] = num33;
				array4[num25 + num28 + 3] = num33;
				array4[num25 + num28 + 4] = num26 + num27 + 1;
				array4[num25 + num28 + 5] = num33 + 1;
				num25 += 6;
				num26 += 2;
			}
			if (num3 < sides)
			{
				int num34 = num4 + num6;
				int num35 = num9 + num5;
				if (normalsType == NormalsType.Vertex)
				{
					array[num35] = array[0];
					array[num35 + 1] = array[num10];
					array[num35 + 2] = array[heightSegments];
					array[num35 + 3] = array[num10 + heightSegments];
					array[num35 + 4] = array[num3 * (heightSegments + 1) + heightSegments];
					array[num35 + 5] = array[num3 * (heightSegments + 1) + num10];
					array[num35 + 6] = array[num3 * (heightSegments + 1)];
					array[num35 + 7] = array[num3 * (heightSegments + 1) + num10 + heightSegments];
				}
				else
				{
					array[num35] = array[0];
					array[num35 + 1] = array[num10];
					array[num35 + 2] = array[heightSegments * 2];
					array[num35 + 3] = array[num10 + heightSegments * 2];
					array[num35 + 4] = array[(num3 - 1) * ((heightSegments + 1) * 2) + heightSegments * 2 + 1];
					array[num35 + 5] = array[(num3 - 1) * ((heightSegments + 1) * 2) + num10 + 1];
					array[num35 + 6] = array[(num3 - 1) * ((heightSegments + 1) * 2) + 1];
					array[num35 + 7] = array[(num3 - 1) * ((heightSegments + 1) * 2) + num10 + heightSegments * 2 + 1];
				}
				array4[num34] = num35;
				array4[num34 + 1] = num35 + 1;
				array4[num34 + 2] = num35 + 2;
				array4[num34 + 3] = num35 + 3;
				array4[num34 + 4] = num35 + 2;
				array4[num34 + 5] = num35 + 1;
				array4[num34 + 6] = num35 + 4;
				array4[num34 + 7] = num35 + 5;
				array4[num34 + 8] = num35 + 6;
				array4[num34 + 9] = num35 + 7;
				array4[num34 + 10] = num35 + 5;
				array4[num34 + 11] = num35 + 4;
				Vector3 vector5 = Vector3.Cross(array[num35 + 1] - array[num35], array[num35 + 2] - array[num35]);
				array2[num35] = vector5;
				array2[num35 + 1] = vector5;
				array2[num35 + 2] = vector5;
				array2[num35 + 3] = vector5;
				Vector3 vector6 = Vector3.Cross(array[num35 + 5] - array[num35 + 4], array[num35 + 6] - array[num35 + 4]);
				array2[num35 + 4] = vector6;
				array2[num35 + 5] = vector6;
				array2[num35 + 6] = vector6;
				array2[num35 + 7] = vector6;
				array3[num35] = new Vector2(1f, 1f);
				array3[num35 + 1] = new Vector2(0f, 1f);
				array3[num35 + 2] = new Vector2(1f, 0f);
				array3[num35 + 3] = new Vector2(0f, 0f);
				array3[num35 + 4] = new Vector2(1f, 1f);
				array3[num35 + 5] = new Vector2(0f, 0f);
				array3[num35 + 6] = new Vector2(1f, 0f);
				array3[num35 + 7] = new Vector2(0f, 1f);
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
