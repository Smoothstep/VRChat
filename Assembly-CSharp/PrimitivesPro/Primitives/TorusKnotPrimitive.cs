using System;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro.Primitives
{
	// Token: 0x02000881 RID: 2177
	public class TorusKnotPrimitive : Primitive
	{
		// Token: 0x06004316 RID: 17174 RVA: 0x0015EB34 File Offset: 0x0015CF34
		public static float GenerateGeometry(Mesh mesh, float torusRadius, float coneRadius, int torusSegments, int coneSegments, int P, int Q, NormalsType normalsType, PivotPosition pivotPosition)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			torusRadius = Mathf.Clamp(torusRadius, 0f, 100f);
			coneRadius = Mathf.Clamp(coneRadius, 0f, 100f);
			torusSegments = Mathf.Clamp(torusSegments, 3, 250);
			coneSegments = Mathf.Clamp(coneSegments, 3, 100);
			P = Mathf.Clamp(P, 1, 20);
			Q = Mathf.Clamp(Q, 1, 20);
			mesh.Clear();
			int num = 2 * coneSegments * torusSegments;
			int num2;
			if (normalsType == NormalsType.Vertex)
			{
				num2 = (torusSegments + 1) * (coneSegments + 1);
			}
			else
			{
				num2 = num * 3;
			}
			if (num2 > 60000)
			{
				UnityEngine.Debug.LogError("Too much vertices!");
				return 0f;
			}
			Vector3[] array = new Vector3[num2];
			Vector3[] array2 = new Vector3[num2];
			Vector2[] array3 = new Vector2[num2];
			int[] array4 = new int[num * 3];
			float num3 = 0f;
			float num4 = 6.28318548f / (float)torusSegments;
			Vector3 b = default(Vector3);
			Vector3 vector = default(Vector3);
			int num5 = 0;
			int num6 = 0;
			float num7 = float.MaxValue;
			float num8 = float.MinValue;
			for (int i = 0; i <= torusSegments + 1; i++)
			{
				num3 += num4;
				float num9 = torusRadius * 0.5f * (2f + Mathf.Sin((float)Q * num3));
				b = vector;
				vector = new Vector3(num9 * Mathf.Cos((float)P * num3), num9 * Mathf.Cos((float)Q * num3), num9 * Mathf.Sin((float)P * num3));
				if (i > 0)
				{
					Vector3 vector2 = vector - b;
					Vector3 vector3 = vector + b;
					Vector3 vector4 = Vector3.Cross(vector2, vector3);
					vector3 = Vector3.Cross(vector4, vector2);
					vector3.Normalize();
					vector4.Normalize();
					float num10 = 0f;
					float num11 = 6.28318548f / (float)coneSegments;
					for (int j = 0; j <= coneSegments; j++)
					{
						num10 += num11;
						float d = coneRadius * Mathf.Sin(num10);
						float d2 = coneRadius * Mathf.Cos(num10);
						Vector3 b2 = vector3 * d + vector4 * d2;
						array[num5] = vector + b2;
						array2[num5] = b2.normalized;
						array3[num5] = new Vector2((float)(i - 1) / (float)torusSegments, (float)j / (float)coneSegments);
						if (array[num5].y < num7)
						{
							num7 = array[num5].y;
						}
						if (array[num5].y > num8)
						{
							num8 = array[num5].y;
						}
						num5++;
					}
					if (i <= torusSegments)
					{
						int num12 = (i - 1) * (coneSegments + 1);
						int num13 = i * (coneSegments + 1);
						int num14 = 0;
						for (int k = 0; k < coneSegments; k++)
						{
							array4[num6] = num13 + num14;
							array4[num6 + 1] = num12 + 1 + num14;
							array4[num6 + 2] = num12 + num14;
							array4[num6 + 3] = num13 + 1 + num14;
							array4[num6 + 4] = num12 + 1 + num14;
							array4[num6 + 5] = num13 + num14;
							num6 += 6;
							num14++;
						}
					}
				}
			}
			if (pivotPosition != PivotPosition.Center)
			{
				float num15 = (pivotPosition != PivotPosition.Botttom) ? (-num8) : (-num7);
				for (int l = 0; l < array.Length; l++)
				{
					Vector3[] array5 = array;
					int num16 = l;
					array5[num16].y = array5[num16].y + num15;
				}
			}
			if (normalsType == NormalsType.Face)
			{
				MeshUtils.DuplicateSharedVertices(ref array, ref array3, array4, -1);
			}
			mesh.vertices = array;
			mesh.triangles = array4;
			if (normalsType == NormalsType.Vertex)
			{
				mesh.normals = array2;
			}
			else
			{
				mesh.RecalculateNormals();
			}
			mesh.uv = array3;
			mesh.RecalculateBounds();
			MeshUtils.CalculateTangents(mesh);
			stopwatch.Stop();
			return (float)stopwatch.ElapsedMilliseconds;
		}
	}
}
