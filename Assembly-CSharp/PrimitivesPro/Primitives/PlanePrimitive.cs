using System;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro.Primitives
{
	// Token: 0x02000877 RID: 2167
	public class PlanePrimitive : Primitive
	{
		// Token: 0x06004307 RID: 17159 RVA: 0x0015C3B0 File Offset: 0x0015A7B0
		public static float GenerateGeometry(Mesh mesh, float width, float length, int widthSegments, int lengthSegments)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			width = Mathf.Clamp(width, 0f, 100f);
			length = Mathf.Clamp(length, 0f, 100f);
			widthSegments = Mathf.Clamp(widthSegments, 1, 100);
			lengthSegments = Mathf.Clamp(lengthSegments, 1, 100);
			mesh.Clear();
			int num = widthSegments + 1;
			int num2 = lengthSegments + 1;
			int num3 = widthSegments * lengthSegments * 6;
			int num4 = num * num2;
			if (num4 > 60000)
			{
				UnityEngine.Debug.LogError("Too much vertices!");
				return 0f;
			}
			Vector3[] array = new Vector3[num4];
			Vector2[] array2 = new Vector2[num4];
			int[] array3 = new int[num3];
			int num5 = 0;
			float num6 = 1f / (float)widthSegments;
			float num7 = 1f / (float)lengthSegments;
			float num8 = width / (float)widthSegments;
			float num9 = length / (float)lengthSegments;
			for (float num10 = 0f; num10 < (float)num2; num10 += 1f)
			{
				for (float num11 = 0f; num11 < (float)num; num11 += 1f)
				{
					array[num5] = new Vector3(num11 * num8 - width / 2f, 0f, num10 * num9 - length / 2f);
					array2[num5++] = new Vector2(num11 * num6, num10 * num7);
				}
			}
			int num12 = 0;
			for (int i = 0; i < lengthSegments; i++)
			{
				for (int j = 0; j < widthSegments; j++)
				{
					array3[num12] = i * num + j;
					array3[num12 + 1] = (i + 1) * num + j;
					array3[num12 + 2] = i * num + j + 1;
					array3[num12 + 3] = (i + 1) * num + j;
					array3[num12 + 4] = (i + 1) * num + j + 1;
					array3[num12 + 5] = i * num + j + 1;
					num12 += 6;
				}
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
	}
}
