using System;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro.Primitives
{
	// Token: 0x0200087D RID: 2173
	public class RingPrimitive : Primitive
	{
		// Token: 0x0600430D RID: 17165 RVA: 0x0015D248 File Offset: 0x0015B648
		public static float GenerateGeometry(Mesh mesh, float radius0, float radius1, int segments)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			radius0 = Mathf.Clamp(radius0, 0f, 100f);
			radius1 = Mathf.Clamp(radius1, 0f, 100f);
			segments = Mathf.Clamp(segments, 3, 100);
			mesh.Clear();
			int num = segments * 2;
			int num2 = segments * 2;
			if (num > 60000)
			{
				UnityEngine.Debug.LogError("Too much vertices!");
				return 0f;
			}
			Vector3[] array = new Vector3[num];
			Vector3[] array2 = new Vector3[num];
			Vector2[] array3 = new Vector2[num];
			int[] array4 = new int[num2 * 3];
			int num3 = 0;
			for (int i = 0; i < segments; i++)
			{
				float f = (float)i / (float)segments * 3.14159274f * 2f;
				Vector3 vector = new Vector3(Mathf.Sin(f), 0f, Mathf.Cos(f));
				float num4 = 0.5f * (radius0 / radius1);
				Vector2 b = new Vector2(vector.x * 0.5f, vector.z * 0.5f);
				Vector2 b2 = new Vector2(vector.x * num4, vector.z * num4);
				Vector2 a = new Vector2(0.5f, 0.5f);
				array[num3] = new Vector3(vector.x * radius0, 0f, vector.z * radius0);
				array2[num3] = new Vector3(0f, 1f, 0f);
				array3[num3] = a + b2;
				array[num3 + 1] = new Vector3(vector.x * radius1, 0f, vector.z * radius1);
				array2[num3 + 1] = new Vector3(0f, 1f, 0f);
				array3[num3 + 1] = a + b;
				num3 += 2;
			}
			int num5 = 0;
			int num6 = 0;
			for (int j = 0; j < segments; j++)
			{
				array4[num6] = num5;
				array4[num6 + 1] = num5 + 1;
				array4[num6 + 2] = num5 + 3;
				array4[num6 + 3] = num5 + 2;
				array4[num6 + 4] = num5;
				array4[num6 + 5] = num5 + 3;
				if (j == segments - 1)
				{
					array4[num6 + 2] = 1;
					array4[num6 + 3] = 0;
					array4[num6 + 5] = 1;
				}
				num5 += 2;
				num6 += 6;
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
