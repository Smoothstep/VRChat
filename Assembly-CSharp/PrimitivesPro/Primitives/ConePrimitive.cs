using System;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro.Primitives
{
	// Token: 0x02000871 RID: 2161
	public class ConePrimitive : Primitive
	{
		// Token: 0x060042F2 RID: 17138 RVA: 0x001593B8 File Offset: 0x001577B8
		public static float GenerateGeometry(Mesh mesh, float radius0, float radius1, float height, int sides, int heightSegments, NormalsType normalsType, PivotPosition pivotPosition)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			mesh.Clear();
			radius0 = Mathf.Clamp(radius0, 0f, 100f);
			radius1 = Mathf.Clamp(radius1, 0f, 100f);
			height = Mathf.Clamp(height, 0f, 100f);
			sides = Mathf.Clamp(sides, 3, 100);
			heightSegments = Mathf.Clamp(heightSegments, 1, 100);
			int num = sides * 6 * heightSegments;
			int num2 = 2 * (sides + 1);
			int num3 = 2 * (3 * sides);
			int num4;
			if (normalsType == NormalsType.Face)
			{
				num4 = sides * (4 + (heightSegments - 1) * 2);
			}
			else
			{
				num4 = (sides + 1) * (heightSegments + 1);
			}
			if (num4 + num2 > 60000)
			{
				UnityEngine.Debug.LogError("Too much vertices!");
				return 0f;
			}
			Vector3[] array = new Vector3[num4 + num2];
			Vector3[] array2 = new Vector3[num4 + num2];
			Vector2[] array3 = new Vector2[num4 + num2];
			int[] array4 = new int[num + num3];
			Vector3 zero = Vector3.zero;
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
				int num5 = 0;
				int num6 = 0;
				float num7 = magnitude / (float)heightSegments;
				for (int i = 0; i < sides; i++)
				{
					float f = (float)i / (float)sides * 3.14159274f * 2f;
					Vector3 vector = new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f));
					Vector3 normalized = vector.normalized;
					float f2 = (float)(i + 1) / (float)sides * 3.14159274f * 2f;
					Vector3 vector2 = new Vector3(Mathf.Cos(f2), 0f, Mathf.Sin(f2));
					Vector3 normalized2 = vector2.normalized;
					float num8 = 0f;
					int num9 = num5;
					Vector3 a = zero + new Vector3(0f, height, 0f) + normalized * radius1 - (zero + normalized * radius0);
					Vector3 a2 = zero + new Vector3(0f, height, 0f) + normalized2 * radius1 - (zero + normalized2 * radius0);
					a.Normalize();
					a2.Normalize();
					Vector3 normalized3 = (normalized + normalized2).normalized;
					for (int j = 0; j <= heightSegments; j++)
					{
						array[num5] = zero + normalized * radius0 + a * num8 + zero2;
						array[num5 + 1] = zero + normalized2 * radius0 + a2 * num8 + zero2;
						array2[num5] = normalized3;
						array2[num5 + 1] = normalized3;
						array3[num5] = new Vector2((float)i / (float)sides, (float)j / (float)heightSegments);
						array3[num5 + 1] = new Vector2((float)(i + 1) / (float)sides, (float)j / (float)heightSegments);
						num5 += 2;
						num8 += num7;
					}
					for (int k = 0; k < heightSegments; k++)
					{
						array4[num6] = num9;
						array4[num6 + 1] = num9 + 2;
						array4[num6 + 2] = num9 + 1;
						array4[num6 + 3] = num9 + 2;
						array4[num6 + 4] = num9 + 3;
						array4[num6 + 5] = num9 + 1;
						num6 += 6;
						num9 += 2;
					}
				}
			}
			else
			{
				int num10 = 0;
				int num11 = 0;
				int num12 = 0;
				float num13 = magnitude / (float)heightSegments;
				for (int l = 0; l <= sides; l++)
				{
					float f3 = (float)l / (float)sides * 3.14159274f * 2f;
					Vector3 vector3 = new Vector3(Mathf.Cos(f3), 0f, Mathf.Sin(f3));
					Vector3 normalized4 = vector3.normalized;
					Vector3 a3 = zero + new Vector3(0f, height, 0f) + normalized4 * radius1 - (zero + normalized4 * radius0);
					a3.Normalize();
					float num14 = 0f;
					for (int m = 0; m <= heightSegments; m++)
					{
						array[num10] = zero + normalized4 * radius0 + a3 * num14 + zero2;
						array2[num10] = normalized4;
						array3[num10] = new Vector2((float)l / (float)sides, (float)m / (float)heightSegments);
						num10++;
						num14 += num13;
					}
				}
				for (int n = 0; n < sides; n++)
				{
					int num15 = (n + 1) * (heightSegments + 1);
					for (int num16 = 0; num16 < heightSegments; num16++)
					{
						array4[num11] = num12;
						array4[num11 + 1] = num12 + 1;
						array4[num11 + 2] = num15;
						array4[num11 + 3] = num15;
						array4[num11 + 4] = num12 + 1;
						array4[num11 + 5] = num15 + 1;
						num11 += 6;
						num12++;
						num15++;
					}
					num12++;
				}
			}
			int num17 = num4;
			int num18 = num;
			int num19 = num17;
			for (int num20 = 0; num20 < sides; num20++)
			{
				float f4 = (float)num20 / (float)sides * 3.14159274f * 2f;
				Vector3 vector4 = new Vector3(Mathf.Cos(f4), 0f, Mathf.Sin(f4));
				Vector3 normalized5 = vector4.normalized;
				array[num17] = zero + normalized5 * radius0 + zero2;
				array2[num17] = new Vector3(0f, -1f, 0f);
				array[num17 + 1] = zero + normalized5 * radius1 + new Vector3(0f, height, 0f) + zero2;
				array2[num17 + 1] = new Vector3(0f, 1f, 0f);
				Vector2 b = new Vector2(normalized5.x * 0.5f, normalized5.z * 0.5f);
				Vector2 a4 = new Vector2(0.5f, 0.5f);
				array3[num17] = a4 + b;
				array3[num17 + 1] = a4 + b;
				num17 += 2;
			}
			array[num17] = new Vector3(0f, 0f, 0f) + zero2;
			array[num17 + 1] = new Vector3(0f, height, 0f) + zero2;
			array2[num17] = new Vector3(0f, -1f, 0f);
			array2[num17 + 1] = new Vector3(0f, 1f, 0f);
			array3[num17] = new Vector2(0.5f, 0.5f);
			array3[num17 + 1] = new Vector2(0.5f, 0.5f);
			num17 += 2;
			for (int num21 = 0; num21 < sides; num21++)
			{
				array4[num18] = num19;
				array4[num18 + 2] = num17 - 2;
				array4[num18 + 3] = num19 + 1;
				array4[num18 + 4] = num17 - 1;
				if (num21 == sides - 1)
				{
					array4[num18 + 1] = num4;
					array4[num18 + 5] = num4 + 1;
				}
				else
				{
					array4[num18 + 1] = num19 + 2;
					array4[num18 + 5] = num19 + 3;
				}
				num18 += 6;
				num19 += 2;
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
