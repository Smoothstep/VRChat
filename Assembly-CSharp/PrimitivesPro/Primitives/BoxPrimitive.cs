using System;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro.Primitives
{
	// Token: 0x0200086F RID: 2159
	public class BoxPrimitive : Primitive
	{
		// Token: 0x060042EB RID: 17131 RVA: 0x00157B40 File Offset: 0x00155F40
		public static float GenerateGeometry(Mesh mesh, float width, float height, float depth, int widthSegments, int heightSegments, int depthSegments, bool cubeMap, float[] edgeOffsets, bool flipUV, PivotPosition pivot)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			width = Mathf.Clamp(width, 0f, 100f);
			height = Mathf.Clamp(height, 0f, 100f);
			depth = Mathf.Clamp(depth, 0f, 100f);
			heightSegments = Mathf.Clamp(heightSegments, 1, 100);
			widthSegments = Mathf.Clamp(widthSegments, 1, 100);
			depthSegments = Mathf.Clamp(depthSegments, 1, 100);
			mesh.Clear();
			int num = widthSegments * depthSegments * 6 + widthSegments * heightSegments * 6 + depthSegments * heightSegments * 6;
			int num2 = (widthSegments + 1) * (depthSegments + 1) + (widthSegments + 1) * (heightSegments + 1) + (depthSegments + 1) * (heightSegments + 1);
			num *= 2;
			num2 *= 2;
			Vector3 zero = Vector3.zero;
			if (pivot != PivotPosition.Top)
			{
				if (pivot == PivotPosition.Botttom)
				{
					zero = new Vector3(0f, height / 2f, 0f);
				}
			}
			else
			{
				zero = new Vector3(0f, -height / 2f, 0f);
			}
			if (num2 > 60000)
			{
				UnityEngine.Debug.LogError("Too much vertices!");
				return 0f;
			}
			Vector3[] vertices = new Vector3[num2];
			Vector2[] array = new Vector2[num2];
			int[] triangles = new int[num];
			int num3 = 0;
			int num4 = 0;
			Vector3 vector = new Vector3(-width / 2f, zero.y - height / 2f, -depth / 2f);
			Vector3 vector2 = new Vector3(-width / 2f, zero.y - height / 2f, depth / 2f);
			Vector3 vector3 = new Vector3(width / 2f, zero.y - height / 2f, depth / 2f);
			Vector3 vector4 = new Vector3(width / 2f, zero.y - height / 2f, -depth / 2f);
			Vector3 vector5 = new Vector3(-width / 2f, height / 2f + zero.y, -depth / 2f);
			Vector3 vector6 = new Vector3(-width / 2f, height / 2f + zero.y, depth / 2f);
			Vector3 vector7 = new Vector3(width / 2f, height / 2f + zero.y, depth / 2f);
			Vector3 vector8 = new Vector3(width / 2f, height / 2f + zero.y, -depth / 2f);
			if (edgeOffsets != null && edgeOffsets.Length > 3)
			{
				vector6.x += edgeOffsets[0];
				vector5.x += edgeOffsets[0];
				vector2.x += edgeOffsets[1];
				vector.x += edgeOffsets[1];
				vector3.x += edgeOffsets[3];
				vector7.x += edgeOffsets[2];
				vector4.x += edgeOffsets[3];
				vector8.x += edgeOffsets[2];
			}
			BoxPrimitive.CreatePlane(0, vector, vector2, vector3, vector4, widthSegments, depthSegments, cubeMap, ref vertices, ref array, ref triangles, ref num3, ref num4);
			BoxPrimitive.CreatePlane(1, vector6, vector5, vector8, vector7, widthSegments, depthSegments, cubeMap, ref vertices, ref array, ref triangles, ref num3, ref num4);
			BoxPrimitive.CreatePlane(2, vector2, vector6, vector7, vector3, widthSegments, heightSegments, cubeMap, ref vertices, ref array, ref triangles, ref num3, ref num4);
			BoxPrimitive.CreatePlane(3, vector4, vector8, vector5, vector, widthSegments, heightSegments, cubeMap, ref vertices, ref array, ref triangles, ref num3, ref num4);
			BoxPrimitive.CreatePlane(4, vector, vector5, vector6, vector2, depthSegments, heightSegments, cubeMap, ref vertices, ref array, ref triangles, ref num3, ref num4);
			BoxPrimitive.CreatePlane(5, vector3, vector7, vector8, vector4, depthSegments, heightSegments, cubeMap, ref vertices, ref array, ref triangles, ref num3, ref num4);
			if (flipUV)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].x = 1f - array[i].x;
				}
			}
			mesh.vertices = vertices;
			mesh.uv = array;
			mesh.triangles = triangles;
			mesh.RecalculateNormals();
			MeshUtils.CalculateTangents(mesh);
			mesh.RecalculateBounds();
			stopwatch.Stop();
			return (float)stopwatch.ElapsedMilliseconds;
		}

		// Token: 0x060042EC RID: 17132 RVA: 0x00157F7C File Offset: 0x0015637C
		private static void CreatePlane(int id, Vector3 a, Vector3 b, Vector3 c, Vector3 d, int segX, int segY, bool cubeMap, ref Vector3[] vertices, ref Vector2[] uvs, ref int[] triangles, ref int vertIndex, ref int triIndex)
		{
			float num = 1f / (float)segX;
			float num2 = 1f / (float)segY;
			Vector3 a2 = d - a;
			Vector3 a3 = c - b;
			int num3 = vertIndex;
			for (float num4 = 0f; num4 < (float)(segY + 1); num4 += 1f)
			{
				for (float num5 = 0f; num5 < (float)(segX + 1); num5 += 1f)
				{
					Vector3 vector = a + a2 * num4 * num2;
					Vector3 a4 = b + a3 * num4 * num2;
					Vector3 vector2 = vector + (a4 - vector) * num5 * num;
					vertices[vertIndex] = vector2;
					Vector2 vector3 = new Vector2(num5 * num, num4 * num2);
					if (cubeMap)
					{
						uvs[vertIndex] = BoxPrimitive.GetCube6UV(id / 2, id % 2, vector3);
					}
					else
					{
						uvs[vertIndex] = vector3;
					}
					vertIndex++;
				}
			}
			int num6 = segX + 1;
			for (int i = 0; i < segY; i++)
			{
				for (int j = 0; j < segX; j++)
				{
					triangles[triIndex] = num3 + i * num6 + j;
					triangles[triIndex + 1] = num3 + (i + 1) * num6 + j;
					triangles[triIndex + 2] = num3 + i * num6 + j + 1;
					triangles[triIndex + 3] = num3 + (i + 1) * num6 + j;
					triangles[triIndex + 4] = num3 + (i + 1) * num6 + j + 1;
					triangles[triIndex + 5] = num3 + i * num6 + j + 1;
					triIndex += 6;
				}
			}
		}

		// Token: 0x060042ED RID: 17133 RVA: 0x00158164 File Offset: 0x00156564
		private static Vector2 GetCube6UV(int sideID, int paralel, Vector2 factor)
		{
			factor.x *= 0.3f;
			factor.y *= 0.5f;
			if (sideID != 0)
			{
				if (sideID != 1)
				{
					if (sideID != 2)
					{
						return Vector2.zero;
					}
					if (paralel == 0)
					{
						factor.y += 0.5f;
						factor.x += 0.333333343f;
						return factor;
					}
					return factor;
				}
				else
				{
					if (paralel == 0)
					{
						factor.x += 0.333333343f;
						return factor;
					}
					factor.x += 0.6666667f;
					return factor;
				}
			}
			else
			{
				if (paralel == 0)
				{
					factor.y += 0.5f;
					return factor;
				}
				factor.y += 0.5f;
				factor.x += 0.6666667f;
				return factor;
			}
		}

		// Token: 0x04002B73 RID: 11123
		private static bool dbg;
	}
}
