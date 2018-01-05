using System;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro.Primitives
{
	// Token: 0x0200087B RID: 2171
	public class PyramidPrimitive : Primitive
	{
		// Token: 0x0600430A RID: 17162 RVA: 0x0015C5E0 File Offset: 0x0015A9E0
		public static float GenerateGeometry(Mesh mesh, float width, float height, float depth, int widthSegments, int heightSegments, int depthSegments, bool pyramidMap, PivotPosition pivotPosition)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			width = Mathf.Clamp(width, 0f, 100f);
			height = Mathf.Clamp(height, 0f, 100f);
			width = Mathf.Clamp(width, 0f, 100f);
			widthSegments = Mathf.Clamp(widthSegments, 1, 100);
			heightSegments = Mathf.Clamp(heightSegments, 1, 100);
			depthSegments = Mathf.Clamp(depthSegments, 1, 100);
			mesh.Clear();
			int num = widthSegments * heightSegments * 6 * 2 + depthSegments * heightSegments * 6 * 2 + widthSegments * depthSegments * 6;
			int num2 = (widthSegments + 1) * (heightSegments + 1) * 2 + (depthSegments + 1) * (heightSegments + 1) * 2 + (widthSegments + 1) * (depthSegments + 1);
			if (num2 > 60000)
			{
				UnityEngine.Debug.LogError("Too much vertices!");
				return 0f;
			}
			Vector3[] array = new Vector3[num2];
			Vector2[] array2 = new Vector2[num2];
			int[] array3 = new int[num];
			Vector3 a = new Vector3(0f, height, 0f);
			Vector3 zero = Vector3.zero;
			if (pivotPosition != PivotPosition.Center)
			{
				if (pivotPosition == PivotPosition.Top)
				{
					zero = new Vector3(0f, -height, 0f);
				}
			}
			else
			{
				zero = new Vector3(0f, -height / 2f, 0f);
			}
			int num3 = 0;
			float num4 = width / (float)widthSegments;
			int num5 = widthSegments * heightSegments * 6;
			int num6 = (widthSegments + 1) * (heightSegments + 1);
			Vector3 vector = new Vector3(depth / 2f, 0f, 0f * num4 - width / 2f) + zero;
			Vector3 vector2 = new Vector3(depth / 2f, 0f, (float)widthSegments * num4 - width / 2f) + zero;
			Vector3 vector3 = new Vector3(-depth / 2f, 0f, 0f * num4 - width / 2f) + zero;
			Vector3 vector4 = new Vector3(-depth / 2f, 0f, (float)widthSegments * num4 - width / 2f) + zero;
			for (int i = 0; i < widthSegments + 1; i++)
			{
				Vector3 vector5 = new Vector3(depth / 2f, 0f, (float)i * num4 - width / 2f);
				Vector3 a2 = a - vector5;
				Vector3 vector6 = new Vector3(-depth / 2f, 0f, (float)i * num4 - width / 2f);
				Vector3 a3 = a - vector6;
				for (int j = 0; j < heightSegments + 1; j++)
				{
					array[num3] = vector5 + (float)j / (float)heightSegments * a2 + zero;
					array[num6 + num3] = vector6 + (float)j / (float)heightSegments * a3 + zero;
					if (pyramidMap)
					{
						Vector3 baryCoords = MeshUtils.ComputeBarycentricCoordinates(a + zero, vector, vector2, array[num3]);
						array2[num3] = PyramidPrimitive.GetPyramidUVMap(PyramidPrimitive.PyramidSide.side0, baryCoords);
						baryCoords = MeshUtils.ComputeBarycentricCoordinates(a + zero, vector4, vector3, array[num6 + num3]);
						array2[num6 + num3] = PyramidPrimitive.GetPyramidUVMap(PyramidPrimitive.PyramidSide.side2, baryCoords);
					}
					else
					{
						array2[num3] = new Vector2(array[num3].z / width, (float)j / (float)heightSegments);
						array2[num6 + num3] = new Vector2(array[num6 + num3].z / width, (float)j / (float)heightSegments);
					}
					num3++;
				}
			}
			int num7 = 0;
			int num8 = 0;
			for (int k = 0; k < widthSegments; k++)
			{
				int num9 = heightSegments + 1;
				for (int l = 0; l < heightSegments; l++)
				{
					array3[num7] = num8;
					array3[num7 + 1] = num8 + 1;
					array3[num7 + 2] = num8 + num9;
					array3[num7 + 3] = num8 + num9;
					array3[num7 + 4] = num8 + 1;
					array3[num7 + 5] = num8 + num9 + 1;
					array3[num5 + num7] = num6 + num8 + num9;
					array3[num5 + num7 + 1] = num6 + num8 + 1;
					array3[num5 + num7 + 2] = num6 + num8;
					array3[num5 + num7 + 3] = num6 + num8 + num9 + 1;
					array3[num5 + num7 + 4] = num6 + num8 + 1;
					array3[num5 + num7 + 5] = num6 + num8 + num9;
					num7 += 6;
					num8++;
				}
				num8++;
			}
			num4 = depth / (float)depthSegments;
			num3 = (widthSegments + 1) * (heightSegments + 1) * 2;
			num7 = widthSegments * heightSegments * 6 * 2;
			num8 = num3;
			num5 = depthSegments * heightSegments * 6;
			num6 = (depthSegments + 1) * (heightSegments + 1);
			vector = new Vector3(0f * num4 - depth / 2f, 0f, width / 2f) + zero;
			vector2 = new Vector3((float)depthSegments * num4 - depth / 2f, 0f, width / 2f) + zero;
			vector3 = new Vector3(0f * num4 - depth / 2f, 0f, -width / 2f) + zero;
			vector4 = new Vector3((float)depthSegments * num4 - depth / 2f, 0f, -width / 2f) + zero;
			for (int m = 0; m < depthSegments + 1; m++)
			{
				Vector3 vector7 = new Vector3((float)m * num4 - depth / 2f, 0f, width / 2f);
				Vector3 a4 = a - vector7;
				Vector3 vector8 = new Vector3((float)m * num4 - depth / 2f, 0f, -width / 2f);
				Vector3 a5 = a - vector8;
				for (int n = 0; n < heightSegments + 1; n++)
				{
					array[num3] = vector7 + (float)n / (float)heightSegments * a4 + zero;
					array[num6 + num3] = vector8 + (float)n / (float)heightSegments * a5 + zero;
					if (pyramidMap)
					{
						Vector3 baryCoords2 = MeshUtils.ComputeBarycentricCoordinates(a + zero, vector2, vector, array[num3]);
						array2[num3] = PyramidPrimitive.GetPyramidUVMap(PyramidPrimitive.PyramidSide.side1, baryCoords2);
						baryCoords2 = MeshUtils.ComputeBarycentricCoordinates(a + zero, vector3, vector4, array[num6 + num3]);
						array2[num6 + num3] = PyramidPrimitive.GetPyramidUVMap(PyramidPrimitive.PyramidSide.side3, baryCoords2);
					}
					else
					{
						array2[num3] = new Vector2(array[num3].x / depth, (float)n / (float)heightSegments);
						array2[num6 + num3] = new Vector2(array[num6 + num3].x / depth, (float)n / (float)heightSegments);
					}
					num3++;
				}
			}
			for (int num10 = 0; num10 < depthSegments; num10++)
			{
				int num11 = heightSegments + 1;
				for (int num12 = 0; num12 < heightSegments; num12++)
				{
					array3[num7] = num8 + num11;
					array3[num7 + 1] = num8 + 1;
					array3[num7 + 2] = num8;
					array3[num7 + 3] = num8 + num11 + 1;
					array3[num7 + 4] = num8 + 1;
					array3[num7 + 5] = num8 + num11;
					array3[num5 + num7] = num6 + num8;
					array3[num5 + num7 + 1] = num6 + num8 + 1;
					array3[num5 + num7 + 2] = num6 + num8 + num11;
					array3[num5 + num7 + 3] = num6 + num8 + num11;
					array3[num5 + num7 + 4] = num6 + num8 + 1;
					array3[num5 + num7 + 5] = num6 + num8 + num11 + 1;
					num7 += 6;
					num8++;
				}
				num8++;
			}
			num4 = width / (float)widthSegments;
			float num13 = depth / (float)depthSegments;
			num3 = (widthSegments + 1) * (heightSegments + 1) * 2 + (depthSegments + 1) * (heightSegments + 1) * 2;
			num7 = widthSegments * heightSegments * 6 * 2 + depthSegments * heightSegments * 6 * 2;
			num8 = num3;
			for (int num14 = 0; num14 < depthSegments + 1; num14++)
			{
				for (int num15 = 0; num15 < widthSegments + 1; num15++)
				{
					array[num3] = new Vector3(depth / 2f - num13 * (float)num14, 0f, width / 2f - (float)num15 * num4) + zero;
					if (pyramidMap)
					{
						array2[num3] = PyramidPrimitive.GetPyramidUVMap(PyramidPrimitive.PyramidSide.bottom, new Vector2((float)num15 / (float)widthSegments, (float)num14 / (float)depthSegments));
					}
					else
					{
						array2[num3] = new Vector2((float)num15 / (float)widthSegments, (float)num14 / (float)depthSegments);
					}
					num3++;
				}
			}
			for (int num16 = 0; num16 < depthSegments; num16++)
			{
				int num17 = widthSegments + 1;
				for (int num18 = 0; num18 < widthSegments; num18++)
				{
					array3[num7] = num8 + num17;
					array3[num7 + 1] = num8 + 1;
					array3[num7 + 2] = num8;
					array3[num7 + 3] = num8 + num17 + 1;
					array3[num7 + 4] = num8 + 1;
					array3[num7 + 5] = num8 + num17;
					num7 += 6;
					num8++;
				}
				num8++;
			}
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.triangles = array3;
			mesh.RecalculateNormals();
			MeshUtils.CalculateTangents(mesh);
			mesh.RecalculateBounds();
			stopwatch.Stop();
			return (float)stopwatch.ElapsedMilliseconds;
		}

		// Token: 0x0600430B RID: 17163 RVA: 0x0015CFFC File Offset: 0x0015B3FC
		private static Vector2 GetPyramidUVMap(PyramidPrimitive.PyramidSide side, Vector3 baryCoords)
		{
			Vector2 vector = new Vector2(2f / (2f + Mathf.Sqrt(3f)), 0.5f);
			switch (side)
			{
			case PyramidPrimitive.PyramidSide.side0:
				return new Vector2(baryCoords.x * vector.x + baryCoords.y * vector.x + baryCoords.z * 1f, baryCoords.x * vector.y + baryCoords.y * 0f + baryCoords.z * (vector.y / 2f));
			case PyramidPrimitive.PyramidSide.side1:
				return new Vector2(baryCoords.x * vector.x + baryCoords.y * 1f + baryCoords.z * 1f, baryCoords.x * vector.y + baryCoords.y * vector.y / 2f + baryCoords.z * (vector.y * 3f / 2f));
			case PyramidPrimitive.PyramidSide.side2:
				return new Vector2(baryCoords.x * vector.x + baryCoords.y * 1f + baryCoords.z * vector.x, baryCoords.x * vector.y + baryCoords.y * (vector.y * 3f / 2f) + baryCoords.z * 1f);
			case PyramidPrimitive.PyramidSide.side3:
				return new Vector2(baryCoords.x * vector.x + baryCoords.y * vector.x + baryCoords.z * (vector.x - vector.y * Mathf.Sqrt(3f) / 2f), baryCoords.x * vector.y + baryCoords.y * 1f + baryCoords.z * (vector.y * 3f / 2f));
			case PyramidPrimitive.PyramidSide.bottom:
				return new Vector2(baryCoords.x * vector.x, baryCoords.y * vector.y);
			default:
				return Vector2.zero;
			}
		}

		// Token: 0x0200087C RID: 2172
		private enum PyramidSide
		{
			// Token: 0x04002B81 RID: 11137
			side0,
			// Token: 0x04002B82 RID: 11138
			side1,
			// Token: 0x04002B83 RID: 11139
			side2,
			// Token: 0x04002B84 RID: 11140
			side3,
			// Token: 0x04002B85 RID: 11141
			bottom
		}
	}
}
