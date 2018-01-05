using System;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro.Primitives
{
	// Token: 0x02000882 RID: 2178
	public class TorusPrimitive : Primitive
	{
		// Token: 0x06004318 RID: 17176 RVA: 0x0015EF24 File Offset: 0x0015D324
		public static float GenerateGeometry(Mesh mesh, float torusRadius, float coneRadius, int torusSegments, int coneSegments, float slice, NormalsType normalsType, PivotPosition pivotPosition)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			torusRadius = Mathf.Clamp(torusRadius, 0f, 100f);
			coneRadius = Mathf.Clamp(coneRadius, 0f, 100f);
			torusSegments = Mathf.Clamp(torusSegments, 3, 250);
			coneSegments = Mathf.Clamp(coneSegments, 3, 100);
			slice = Mathf.Clamp(slice, 0f, 360f);
			mesh.Clear();
			slice /= 360f;
			int num = (int)((float)torusSegments * (1f - slice));
			if (num == 0)
			{
				num = 1;
			}
			int num2 = 2 * coneSegments * num;
			int num3;
			if (normalsType == NormalsType.Vertex)
			{
				num3 = (num + 1) * (coneSegments + 1);
			}
			else
			{
				num3 = num2 * 3;
			}
			int num4 = 0;
			int num5 = 0;
			if (num < torusSegments)
			{
				num4 = (coneSegments + 1) * 2;
				num5 = (coneSegments + 2) * 2;
			}
			if (num3 > 60000)
			{
				UnityEngine.Debug.LogError("Too much vertices!");
				return 0f;
			}
			Vector3[] array = new Vector3[num3 + num5];
			Vector3[] array2 = new Vector3[num3 + num5];
			Vector2[] array3 = new Vector2[num3 + num5];
			int[] array4 = new int[num2 * 3 + num4 * 3];
			float num6 = 0f;
			float num7 = 6.28318548f / (float)torusSegments;
			Vector3 b = default(Vector3);
			Vector3 vector = default(Vector3);
			int num8 = 0;
			int num9 = 0;
			int num10 = num2 * 3;
			int num11 = num3 + 1;
			int num12 = array.Length - 1;
			Vector3 zero = Vector3.zero;
			if (pivotPosition != PivotPosition.Botttom)
			{
				if (pivotPosition == PivotPosition.Top)
				{
					zero = new Vector3(0f, -coneRadius, 0f);
				}
			}
			else
			{
				zero = new Vector3(0f, coneRadius, 0f);
			}
			for (int i = 0; i <= num + 1; i++)
			{
				num6 += num7;
				b = vector;
				vector = new Vector3(torusRadius * Mathf.Cos(num6), 0f, torusRadius * Mathf.Sin(num6));
				if (i > 0)
				{
					Vector3 vector2 = vector - b;
					Vector3 vector3 = vector + b;
					Vector3 vector4 = Vector3.Cross(vector2, vector3);
					vector3 = Vector3.Cross(vector4, vector2);
					vector3.Normalize();
					vector4.Normalize();
					float num13 = 0f;
					float num14 = 6.28318548f / (float)coneSegments;
					for (int j = 0; j <= coneSegments; j++)
					{
						num13 += num14;
						float d = coneRadius * Mathf.Sin(num13);
						float d2 = coneRadius * Mathf.Cos(num13);
						Vector3 b2 = vector3 * d + vector4 * d2;
						array[num8] = vector + b2 + zero;
						array2[num8] = b2.normalized;
						array3[num8] = new Vector2(1f - (float)(i - 1) / (float)torusSegments, (float)j / (float)coneSegments);
						num8++;
						if (num < torusSegments)
						{
							if (i == num + 1)
							{
								array[num3] = vector + zero;
								array3[num3] = new Vector2(0.5f, 0.5f);
								array2[num3] = vector2.normalized;
								array[num11] = vector + b2 + zero;
								array2[num11] = vector2.normalized;
								array3[num11] = new Vector2(0.5f, 0.5f) + new Vector2(b2.x * 0.5f, b2.y * 0.5f);
								if (j < coneSegments)
								{
									array4[num10] = num11 + 1;
									array4[num10 + 1] = num11;
									array4[num10 + 2] = num3;
									num10 += 3;
								}
								num11++;
							}
							else if (i == 1)
							{
								Vector3 a = vector;
								array[num12] = a + zero;
								array3[num12] = new Vector2(0.5f, 0.5f);
								array2[num12] = -vector2.normalized;
								array[num11] = a + b2 + zero;
								array2[num11] = -vector2.normalized;
								array3[num11] = new Vector2(0.5f, 0.5f) + new Vector2(b2.x * 0.5f, b2.y * 0.5f);
								if (j < coneSegments)
								{
									array4[num10] = num12;
									array4[num10 + 1] = num11;
									array4[num10 + 2] = num11 + 1;
									num10 += 3;
								}
								num11++;
							}
						}
					}
					if (i <= num)
					{
						int num15 = (i - 1) * (coneSegments + 1);
						int num16 = i * (coneSegments + 1);
						int num17 = 0;
						for (int k = 0; k < coneSegments; k++)
						{
							array4[num9] = num16 + num17;
							array4[num9 + 1] = num15 + 1 + num17;
							array4[num9 + 2] = num15 + num17;
							array4[num9 + 3] = num16 + 1 + num17;
							array4[num9 + 4] = num15 + 1 + num17;
							array4[num9 + 5] = num16 + num17;
							num9 += 6;
							num17++;
						}
					}
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
