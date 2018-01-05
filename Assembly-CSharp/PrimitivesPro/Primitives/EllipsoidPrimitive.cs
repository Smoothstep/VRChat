using System;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro.Primitives
{
	// Token: 0x02000873 RID: 2163
	public class EllipsoidPrimitive : Primitive
	{
		// Token: 0x060042F6 RID: 17142 RVA: 0x00159EFC File Offset: 0x001582FC
		public static float GenerateGeometry(Mesh mesh, float width, float height, float length, int segments, NormalsType normalsType, PivotPosition pivotPosition)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			width = Mathf.Clamp(width, 0f, 100f);
			height = Mathf.Clamp(height, 0f, 100f);
			length = Mathf.Clamp(length, 0f, 100f);
			segments = Mathf.Clamp(segments, 4, 100);
			mesh.Clear();
			int num = segments - 1;
			int num2 = segments;
			float num3 = 1f / (float)(num - 1);
			float num4 = 1f / (float)(num2 - 1);
			int num5 = num * num2;
			int num6 = (num - 1) * (num2 - 1) * 6;
			if (normalsType == NormalsType.Face)
			{
				num5 = num6;
			}
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
			if (num5 > 60000)
			{
				UnityEngine.Debug.LogError("Too much vertices!");
				return 0f;
			}
			Vector3[] array = new Vector3[num5];
			Vector3[] array2 = new Vector3[num5];
			Vector2[] array3 = new Vector2[num5];
			int[] array4 = new int[num6];
			int num7 = 0;
			int num8 = 0;
			for (int i = 0; i < num; i++)
			{
				float num9 = Mathf.Cos(-6.28318548f + 3.14159274f * (float)i * num3);
				float num10 = Mathf.Sin(3.14159274f * (float)i * num3);
				for (int j = 0; j < num2; j++)
				{
					float num11 = Mathf.Sin(6.28318548f * (float)j * num4) * num10;
					float num12 = Mathf.Cos(6.28318548f * (float)j * num4) * num10;
					array[num7] = new Vector3(num11 * width, num9 * height, num12 * length) + zero;
					array2[num7] = new Vector3(num11, num9, num12);
					array3[num7] = new Vector2(1f - (float)j * num4, 1f - (float)i * num3);
					if (i < num - 1 && j < num2 - 1)
					{
						array4[num8] = (i + 1) * num2 + j;
						array4[num8 + 1] = i * num2 + (j + 1);
						array4[num8 + 2] = i * num2 + j;
						array4[num8 + 3] = (i + 1) * num2 + (j + 1);
						array4[num8 + 4] = i * num2 + (j + 1);
						array4[num8 + 5] = (i + 1) * num2 + j;
						num8 += 6;
					}
					num7++;
				}
			}
			if (normalsType == NormalsType.Face)
			{
				MeshUtils.DuplicateSharedVertices(ref array, ref array3, array4, -1);
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
