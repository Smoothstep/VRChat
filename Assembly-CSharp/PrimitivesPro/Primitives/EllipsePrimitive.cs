using System;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro.Primitives
{
	// Token: 0x02000872 RID: 2162
	public class EllipsePrimitive : Primitive
	{
		// Token: 0x060042F4 RID: 17140 RVA: 0x00159CA0 File Offset: 0x001580A0
		public static float GenerateGeometry(Mesh mesh, float radius0, float radius1, int segments)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			radius0 = Mathf.Clamp(radius0, 0f, 100f);
			radius1 = Mathf.Clamp(radius1, 0f, 100f);
			segments = Mathf.Clamp(segments, 3, 100);
			mesh.Clear();
			int num = 1 + segments;
			int num2 = segments;
			if (num > 60000)
			{
				UnityEngine.Debug.LogError("Too much vertices!");
				return 0f;
			}
			Vector3[] array = new Vector3[num];
			Vector3[] array2 = new Vector3[num];
			Vector2[] array3 = new Vector2[num];
			int[] array4 = new int[num2 * 3];
			for (int i = 0; i < segments; i++)
			{
				float f = (float)i / (float)segments * 3.14159274f * 2f;
				Vector3 vector = new Vector3(Mathf.Sin(f), 0f, Mathf.Cos(f));
				array[i] = new Vector3(vector.x * radius0, 0f, vector.z * radius1);
				array2[i] = new Vector3(0f, 1f, 0f);
				float num3 = radius0 / radius1;
				array3[i] = new Vector2(0.5f + vector.x * 0.5f * num3, 0.5f + vector.z * 0.5f * 1f / num3);
			}
			array[segments] = new Vector3(0f, 0f, 0f);
			array2[segments] = new Vector3(0f, 1f, 0f);
			array3[segments] = new Vector2(0.5f, 0.5f);
			int num4 = 0;
			int num5 = 0;
			for (int j = 0; j < segments; j++)
			{
				array4[num5] = num4;
				array4[num5 + 1] = num4 + 1;
				array4[num5 + 2] = segments;
				if (j == segments - 1)
				{
					array4[num5 + 1] = 0;
				}
				num4++;
				num5 += 3;
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
