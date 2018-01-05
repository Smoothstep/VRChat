using System;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro.Primitives
{
	// Token: 0x02000870 RID: 2160
	public class CapsulePrimitive : Primitive
	{
		// Token: 0x060042EF RID: 17135 RVA: 0x00158264 File Offset: 0x00156664
		public static float GenerateGeometry(Mesh mesh, float radius, float height, int sides, int heightSegments, bool preserveHeight, NormalsType normalsType, PivotPosition pivotPosition)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			radius = Mathf.Clamp(radius, 0f, 100f);
			height = Mathf.Clamp(height, 0f, 100f);
			heightSegments = Mathf.Clamp(heightSegments, 1, 250);
			sides = Mathf.Clamp(sides, 4, 250);
			mesh.Clear();
			if (preserveHeight)
			{
				height -= radius * 2f;
				if (height < 0f)
				{
					height = 0f;
				}
			}
			int num = sides;
			int num2 = sides + 1;
			if ((num & 1) == 0)
			{
				num++;
				num2 = sides + 1;
			}
			float num3 = 1f / (float)(num - 1);
			float num4 = 1f / (float)(num2 - 1);
			int num5 = num / 2 + 1;
			int num6 = (num - 1) * (num2 - 1) * 6;
			int num7 = (sides + 1) * (heightSegments + 1);
			int num8 = sides * 6 * heightSegments;
			int num9;
			if (normalsType == NormalsType.Vertex)
			{
				num9 = num * num2 + num2;
			}
			else
			{
				num9 = (num5 - 1) * (num2 - 1) * 4 + (num - 1 - (num5 - 1)) * (num2 - 1) * 4;
				num7 = sides * (4 + (heightSegments - 1) * 2);
			}
			if (num9 + num7 > 60000)
			{
				UnityEngine.Debug.LogError("Too much vertices!");
				return 0f;
			}
			Vector3[] array = new Vector3[num9 + num7];
			Vector3[] array2 = new Vector3[num9 + num7];
			Vector2[] array3 = new Vector2[num9 + num7];
			int[] array4 = new int[num6 + num8];
			float num10 = radius + height / 2f;
			Vector3 zero = Vector3.zero;
			if (pivotPosition != PivotPosition.Botttom)
			{
				if (pivotPosition == PivotPosition.Top)
				{
					zero = new Vector3(0f, -num10, 0f);
				}
			}
			else
			{
				zero = new Vector3(0f, num10, 0f);
			}
			int num11 = 0;
			int num12 = 0;
			if (normalsType == NormalsType.Vertex)
			{
				for (int i = 0; i < num5; i++)
				{
					float y = Mathf.Cos(-6.28318548f + 3.14159274f * (float)i * num3);
					float num13 = Mathf.Sin(3.14159274f * (float)i * num3);
					for (int j = 0; j < num2; j++)
					{
						float x = Mathf.Sin(6.28318548f * (float)j * num4) * num13;
						float z = Mathf.Cos(6.28318548f * (float)j * num4) * num13;
						array[num11] = new Vector3(x, y, z) * radius + zero;
						array2[num11] = new Vector3(x, y, z);
						Vector3[] array5 = array;
						int num14 = num11;
						array5[num14].y = array5[num14].y + height / 2f;
						Vector2 sphericalUV = CapsulePrimitive.GetSphericalUV(array[num11] - zero);
						array3[num11] = new Vector2(1f - (float)j * num4, sphericalUV.y);
						if (i < num5 - 1 && j < num2 - 1)
						{
							array4[num12] = (i + 1) * num2 + j;
							array4[num12 + 1] = i * num2 + (j + 1);
							array4[num12 + 2] = i * num2 + j;
							array4[num12 + 3] = (i + 1) * num2 + (j + 1);
							array4[num12 + 4] = i * num2 + (j + 1);
							array4[num12 + 5] = (i + 1) * num2 + j;
							num12 += 6;
						}
						num11++;
					}
				}
				if (height > 0f)
				{
					int num15 = num9;
					int num16 = num6;
					int num17 = num9;
					float num18 = height / (float)heightSegments;
					Vector3 a = new Vector3(0f, -height / 2f, 0f);
					float num19 = Mathf.Sin(3.14159274f * (float)(num5 - 1) * num3);
					for (int k = 0; k <= sides; k++)
					{
						float x2 = Mathf.Sin(6.28318548f * (float)k * num4) * num19;
						float z2 = Mathf.Cos(6.28318548f * (float)k * num4) * num19;
						Vector3 vector = new Vector3(x2, 0f, z2);
						float num20 = 0f;
						for (int l = 0; l <= heightSegments; l++)
						{
							array[num15] = a + vector * radius + new Vector3(0f, num20, 0f) + zero;
							array2[num15] = vector;
							Vector2 sphericalUV2 = CapsulePrimitive.GetSphericalUV(array[num15] - zero);
							array3[num15] = new Vector2(1f - (float)k * num4, sphericalUV2.y);
							num15++;
							num20 += num18;
						}
					}
					for (int m = 0; m < sides; m++)
					{
						int num21 = num9 + (m + 1) * (heightSegments + 1);
						for (int n = 0; n < heightSegments; n++)
						{
							array4[num16] = num21;
							array4[num16 + 1] = num21 + 1;
							array4[num16 + 2] = num17;
							array4[num16 + 3] = num21 + 1;
							array4[num16 + 4] = num17 + 1;
							array4[num16 + 5] = num17;
							num16 += 6;
							num17++;
							num21++;
						}
						num17++;
					}
				}
				for (int num22 = num5 - 1; num22 < num; num22++)
				{
					float y2 = Mathf.Cos(-6.28318548f + 3.14159274f * (float)num22 * num3);
					float num23 = Mathf.Sin(3.14159274f * (float)num22 * num3);
					for (int num24 = 0; num24 < num2; num24++)
					{
						float x3 = Mathf.Sin(6.28318548f * (float)num24 * num4) * num23;
						float z3 = Mathf.Cos(6.28318548f * (float)num24 * num4) * num23;
						array[num11] = new Vector3(x3, y2, z3) * radius;
						array2[num11] = new Vector3(x3, y2, z3);
						array[num11] += zero;
						Vector3[] array6 = array;
						int num25 = num11;
						array6[num25].y = array6[num25].y - height / 2f;
						Vector2 sphericalUV3 = CapsulePrimitive.GetSphericalUV(array[num11] - zero);
						array3[num11] = new Vector2(1f - (float)num24 * num4, sphericalUV3.y);
						if (num22 < num - 1 && num24 < num2 - 1)
						{
							array4[num12] = (num22 + 1 + 1) * num2 + num24;
							array4[num12 + 1] = (num22 + 1) * num2 + (num24 + 1);
							array4[num12 + 2] = (num22 + 1) * num2 + num24;
							array4[num12 + 3] = (num22 + 1 + 1) * num2 + (num24 + 1);
							array4[num12 + 4] = (num22 + 1) * num2 + (num24 + 1);
							array4[num12 + 5] = (num22 + 1 + 1) * num2 + num24;
							num12 += 6;
						}
						num11++;
					}
				}
			}
			else
			{
				for (int num26 = 0; num26 < num5 - 1; num26++)
				{
					float y3 = Mathf.Cos(-6.28318548f + 3.14159274f * (float)num26 * num3);
					float y4 = Mathf.Cos(-6.28318548f + 3.14159274f * (float)(num26 + 1) * num3);
					float num27 = Mathf.Sin(3.14159274f * (float)num26 * num3);
					float num28 = Mathf.Sin(3.14159274f * (float)(num26 + 1) * num3);
					for (int num29 = 0; num29 < num2 - 1; num29++)
					{
						float num30 = Mathf.Sin(6.28318548f * (float)num29 * num4);
						float num31 = Mathf.Sin(6.28318548f * (float)(num29 + 1) * num4);
						float num32 = Mathf.Cos(6.28318548f * (float)num29 * num4);
						float num33 = Mathf.Cos(6.28318548f * (float)(num29 + 1) * num4);
						float x4 = num30 * num27;
						float x5 = num31 * num27;
						float x6 = num30 * num28;
						float x7 = num31 * num28;
						float z4 = Mathf.Cos(6.28318548f * (float)num29 * num4) * num27;
						float z5 = num33 * num27;
						float z6 = num32 * num28;
						float z7 = num33 * num28;
						array[num11] = new Vector3(x4, y3, z4) * radius + zero;
						Vector3[] array7 = array;
						int num34 = num11;
						array7[num34].y = array7[num34].y + height / 2f;
						Vector2 sphericalUV4 = CapsulePrimitive.GetSphericalUV(array[num11] - zero);
						array3[num11] = new Vector2(1f - (float)num29 * num4, sphericalUV4.y);
						array[num11 + 1] = new Vector3(x6, y4, z6) * radius + zero;
						Vector3[] array8 = array;
						int num35 = num11 + 1;
						array8[num35].y = array8[num35].y + height / 2f;
						sphericalUV4 = CapsulePrimitive.GetSphericalUV(array[num11 + 1] - zero);
						array3[num11 + 1] = new Vector2(1f - (float)num29 * num4, sphericalUV4.y);
						array[num11 + 2] = new Vector3(x5, y3, z5) * radius + zero;
						Vector3[] array9 = array;
						int num36 = num11 + 2;
						array9[num36].y = array9[num36].y + height / 2f;
						sphericalUV4 = CapsulePrimitive.GetSphericalUV(array[num11 + 2] - zero);
						array3[num11 + 2] = new Vector2(1f - (float)(num29 + 1) * num4, sphericalUV4.y);
						array[num11 + 3] = new Vector3(x7, y4, z7) * radius + zero;
						Vector3[] array10 = array;
						int num37 = num11 + 3;
						array10[num37].y = array10[num37].y + height / 2f;
						sphericalUV4 = CapsulePrimitive.GetSphericalUV(array[num11 + 3] - zero);
						array3[num11 + 3] = new Vector2(1f - (float)(num29 + 1) * num4, sphericalUV4.y);
						array4[num12] = num11 + 1;
						array4[num12 + 1] = num11 + 2;
						array4[num12 + 2] = num11;
						array4[num12 + 3] = num11 + 3;
						array4[num12 + 4] = num11 + 2;
						array4[num12 + 5] = num11 + 1;
						num12 += 6;
						num11 += 4;
					}
				}
				if (height > 0f)
				{
					int num15 = num9;
					int num16 = num6;
					float num38 = height / (float)heightSegments;
					Vector3 a2 = new Vector3(0f, -height / 2f, 0f);
					float num39 = Mathf.Sin(3.14159274f * (float)(num5 - 1) * num3);
					for (int num40 = 0; num40 < sides; num40++)
					{
						Vector3 a3 = new Vector3(Mathf.Sin(6.28318548f * (float)num40 * num4) * num39, 0f, Mathf.Cos(6.28318548f * (float)num40 * num4) * num39);
						Vector3 vector2 = new Vector3(Mathf.Sin(6.28318548f * (float)(num40 + 1) * num4) * num39, 0f, Mathf.Cos(6.28318548f * (float)(num40 + 1) * num4) * num39);
						float num41 = 0f;
						int num42 = num15;
						Vector3 normalized = (a3 + vector2).normalized;
						for (int num43 = 0; num43 <= heightSegments; num43++)
						{
							array[num15] = a2 + a3 * radius + new Vector3(0f, num41, 0f) + zero;
							array[num15 + 1] = a2 + vector2 * radius + new Vector3(0f, num41, 0f) + zero;
							array2[num15] = normalized;
							array2[num15 + 1] = normalized;
							Vector2 sphericalUV5 = CapsulePrimitive.GetSphericalUV(array[num15] - zero);
							array3[num15] = new Vector2(1f - (float)num40 * num4, sphericalUV5.y);
							array3[num15 + 1] = new Vector2(1f - (float)(num40 + 1) * num4, sphericalUV5.y);
							num15 += 2;
							num41 += num38;
						}
						for (int num44 = 0; num44 < heightSegments; num44++)
						{
							array4[num16] = num42;
							array4[num16 + 1] = num42 + 1;
							array4[num16 + 2] = num42 + 3;
							array4[num16 + 3] = num42 + 3;
							array4[num16 + 4] = num42 + 2;
							array4[num16 + 5] = num42;
							num16 += 6;
							num42 += 2;
						}
					}
				}
				for (int num45 = num5 - 1; num45 < num - 1; num45++)
				{
					float y5 = Mathf.Cos(-6.28318548f + 3.14159274f * (float)num45 * num3);
					float y6 = Mathf.Cos(-6.28318548f + 3.14159274f * (float)(num45 + 1) * num3);
					float num46 = Mathf.Sin(3.14159274f * (float)num45 * num3);
					float num47 = Mathf.Sin(3.14159274f * (float)(num45 + 1) * num3);
					for (int num48 = 0; num48 < num2 - 1; num48++)
					{
						float num49 = Mathf.Sin(6.28318548f * (float)num48 * num4);
						float num50 = Mathf.Sin(6.28318548f * (float)(num48 + 1) * num4);
						float num51 = Mathf.Cos(6.28318548f * (float)num48 * num4);
						float num52 = Mathf.Cos(6.28318548f * (float)(num48 + 1) * num4);
						float x8 = num49 * num46;
						float x9 = num50 * num46;
						float x10 = num49 * num47;
						float x11 = num50 * num47;
						float z8 = Mathf.Cos(6.28318548f * (float)num48 * num4) * num46;
						float z9 = num52 * num46;
						float z10 = num51 * num47;
						float z11 = num52 * num47;
						array[num11] = new Vector3(x8, y5, z8) * radius + zero;
						Vector3[] array11 = array;
						int num53 = num11;
						array11[num53].y = array11[num53].y - height / 2f;
						Vector2 sphericalUV6 = CapsulePrimitive.GetSphericalUV(array[num11] - zero);
						array3[num11] = new Vector2(1f - (float)num48 * num4, sphericalUV6.y);
						array[num11 + 1] = new Vector3(x10, y6, z10) * radius + zero;
						Vector3[] array12 = array;
						int num54 = num11 + 1;
						array12[num54].y = array12[num54].y - height / 2f;
						sphericalUV6 = CapsulePrimitive.GetSphericalUV(array[num11 + 1] - zero);
						array3[num11 + 1] = new Vector2(1f - (float)num48 * num4, sphericalUV6.y);
						array[num11 + 2] = new Vector3(x9, y5, z9) * radius + zero;
						Vector3[] array13 = array;
						int num55 = num11 + 2;
						array13[num55].y = array13[num55].y - height / 2f;
						sphericalUV6 = CapsulePrimitive.GetSphericalUV(array[num11 + 2] - zero);
						array3[num11 + 2] = new Vector2(1f - (float)(num48 + 1) * num4, sphericalUV6.y);
						array[num11 + 3] = new Vector3(x11, y6, z11) * radius + zero;
						Vector3[] array14 = array;
						int num56 = num11 + 3;
						array14[num56].y = array14[num56].y - height / 2f;
						sphericalUV6 = CapsulePrimitive.GetSphericalUV(array[num11 + 3] - zero);
						array3[num11 + 3] = new Vector2(1f - (float)(num48 + 1) * num4, sphericalUV6.y);
						array4[num12] = num11 + 1;
						array4[num12 + 1] = num11 + 2;
						array4[num12 + 2] = num11;
						array4[num12 + 3] = num11 + 3;
						array4[num12 + 4] = num11 + 2;
						array4[num12 + 5] = num11 + 1;
						num12 += 6;
						num11 += 4;
					}
				}
			}
			mesh.vertices = array;
			mesh.normals = array2;
			mesh.uv = array3;
			mesh.triangles = array4;
			if (normalsType == NormalsType.Face)
			{
				mesh.RecalculateNormals();
			}
			mesh.RecalculateBounds();
			MeshUtils.CalculateTangents(mesh);
			stopwatch.Stop();
			return (float)stopwatch.ElapsedMilliseconds;
		}

		// Token: 0x060042F0 RID: 17136 RVA: 0x00159358 File Offset: 0x00157758
		private static Vector2 GetSphericalUV(Vector3 pnt)
		{
			Vector3 normalized = pnt.normalized;
			return new Vector2(0.5f + Mathf.Atan2(normalized.z, normalized.x) / 6.28318548f, 1f - (0.5f - Mathf.Asin(normalized.y) / 3.14159274f));
		}
	}
}
