using System;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro.Primitives
{
	// Token: 0x0200086E RID: 2158
	public class ArcPrimitive : Primitive
	{
		// Token: 0x060042E9 RID: 17129 RVA: 0x00157114 File Offset: 0x00155514
		public static float GenerateGeometry(Mesh mesh, float width, float height1, float height2, float depth, int arcSegments, Vector3 controlPoint, PivotPosition pivot)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			width = Mathf.Clamp(width, 0f, 100f);
			height1 = Mathf.Clamp(height1, 0f, 100f);
			height2 = Mathf.Clamp(height2, 0f, 100f);
			depth = Mathf.Clamp(depth, 0f, 100f);
			arcSegments = Mathf.Clamp(arcSegments, 1, 100);
			float num = Mathf.Max(height1, height2);
			mesh.Clear();
			int num2 = 36 + arcSegments * 6 + arcSegments * 3 * 2 + 6;
			int num3 = 24 + arcSegments * 6 * 2 + 2 + 2;
			Vector3 zero = Vector3.zero;
			if (pivot != PivotPosition.Center)
			{
				if (pivot == PivotPosition.Top)
				{
					zero = new Vector3(0f, -num, 0f);
				}
			}
			else
			{
				zero = new Vector3(0f, -num / 2f, 0f);
			}
			if (num3 > 60000)
			{
				UnityEngine.Debug.LogError("Too much vertices!");
				return 0f;
			}
			Vector3[] array = new Vector3[num3];
			Vector2[] array2 = new Vector2[num3];
			int[] array3 = new int[num2];
			Vector3 vector = array[0] = new Vector3(-width / 2f, 0f, -depth / 2f);
			Vector3 vector2 = array[1] = new Vector3(width / 2f, 0f, -depth / 2f);
			Vector3 vector3 = array[2] = new Vector3(width / 2f, 0f, depth / 2f);
			Vector3 vector4 = array[3] = new Vector3(-width / 2f, 0f, depth / 2f);
			array2[0] = new Vector2(0f, 1f);
			array2[1] = new Vector2(1f, 1f);
			array2[2] = new Vector2(1f, 0f);
			array2[3] = new Vector2(0f, 0f);
			Vector3 p = vector;
			Vector3 p2 = vector2;
			Vector3 p3 = vector4;
			Vector3 p4 = vector3;
			array3[0] = 0;
			array3[1] = 1;
			array3[2] = 2;
			array3[3] = 0;
			array3[4] = 2;
			array3[5] = 3;
			int num4 = 4;
			int num5 = 6;
			if (height1 > 0f)
			{
				array[num4] = vector;
				array[num4 + 1] = vector4;
				p = (array[num4 + 2] = new Vector3(-width / 2f, height1, -depth / 2f));
				p3 = (array[num4 + 3] = new Vector3(-width / 2f, height1, depth / 2f));
				array2[num4 + 3] = new Vector2(0f, height1 / num);
				array2[num4 + 2] = new Vector2(1f, height1 / num);
				array2[num4 + 1] = new Vector2(0f, 0f);
				array2[num4] = new Vector2(1f, 0f);
				array3[num5] = num4 + 3;
				array3[num5 + 1] = num4 + 2;
				array3[num5 + 2] = num4;
				array3[num5 + 3] = num4 + 1;
				array3[num5 + 4] = num4 + 3;
				array3[num5 + 5] = num4;
				num4 += 4;
				num5 += 6;
			}
			if (height2 > 0f)
			{
				array[num4] = vector2;
				array[num4 + 1] = vector3;
				p2 = (array[num4 + 2] = new Vector3(width / 2f, height2, -depth / 2f));
				p4 = (array[num4 + 3] = new Vector3(width / 2f, height2, depth / 2f));
				array2[num4 + 3] = new Vector2(1f, height2 / num);
				array2[num4 + 2] = new Vector2(0f, height2 / num);
				array2[num4 + 1] = new Vector2(1f, 0f);
				array2[num4] = new Vector2(0f, 0f);
				array3[num5] = num4 + 2;
				array3[num5 + 1] = num4 + 1;
				array3[num5 + 2] = num4;
				array3[num5 + 3] = num4 + 1;
				array3[num5 + 4] = num4 + 2;
				array3[num5 + 5] = num4 + 3;
				num4 += 4;
				num5 += 6;
			}
			array[num4++] = vector;
			array[num4++] = vector2;
			array[num4++] = vector3;
			array[num4++] = vector4;
			int num6 = num4 - 4;
			int num7 = num4 - 3;
			int num8 = num4 - 2;
			int num9 = num4 - 1;
			array2[num6] = new Vector2(0f, 0f);
			array2[num7] = new Vector2(1f, 0f);
			array2[num8] = new Vector2(0f, 0f);
			array2[num9] = new Vector2(1f, 0f);
			int num10 = num4 + arcSegments * 2;
			int num11 = num5 + arcSegments * 6;
			int num12 = 0;
			int num13 = 3;
			float num14 = 0.5f;
			for (int i = 0; i <= arcSegments; i++)
			{
				float num15 = (float)i / (float)arcSegments;
				Vector3 vector5 = MeshUtils.BezierQuadratic(p, p2, new Vector3(controlPoint.x, controlPoint.y, -depth / 2f), num15);
				Vector3 vector6 = MeshUtils.BezierQuadratic(p3, p4, new Vector3(controlPoint.x, controlPoint.y, depth / 2f), num15);
				array[num4] = vector5;
				array[num4 + 1] = vector6;
				array2[num4] = new Vector2(num15, 0f);
				array2[num4 + 1] = new Vector2(num15, 1f);
				if (i < arcSegments)
				{
					array3[num5] = num4;
					array3[num5 + 1] = num4 + 1;
					array3[num5 + 2] = num4 + 3;
					array3[num5 + 3] = num4 + 3;
					array3[num5 + 4] = num4 + 2;
					array3[num5 + 5] = num4;
					array3[num11 + num5] = num10 + num4;
					array3[num11 + num5 + 1] = num10 + num4 + 2;
					array3[num11 + num5 + 5] = num10 + num4 + 1;
					array3[num11 + num5 + 4] = num10 + num4 + 3;
					if (height1 > Mathf.Epsilon && height2 > Mathf.Epsilon)
					{
						if (num15 < 0.5f)
						{
							array3[num11 + num5 + 2] = num6;
							array3[num11 + num5 + 3] = num9;
						}
						else
						{
							array3[num11 + num5 + 2] = num7;
							array3[num11 + num5 + 3] = num8;
						}
					}
					else
					{
						if (height1 > Mathf.Epsilon)
						{
							array3[num11 + num5 + 2] = num6;
							array3[num11 + num5 + 3] = num9;
						}
						if (height2 > Mathf.Epsilon)
						{
							array3[num11 + num5 + 2] = num7;
							array3[num11 + num5 + 3] = num8;
						}
					}
					num5 += 6;
				}
				array[num10 + num4] = vector5;
				array[num10 + num4 + 1] = vector6;
				array2[num10 + num4] = new Vector2(num15, vector5.y / num);
				array2[num10 + num4 + 1] = new Vector2(1f - num15, vector5.y / num);
				if (num15 < 0.5f && num15 + (float)(i + 1) / (float)arcSegments >= 0.5f)
				{
					num12 = num4 + 2;
					num13 = num4 + 3;
					num14 = num15;
					if (arcSegments % 2 == 0)
					{
						num14 = 0.5f;
					}
				}
				num4 += 2;
			}
			if (height1 > 0f && height2 > 0f)
			{
				array[num10 + num4] = array[num12];
				array[num10 + num4 + 1] = array[num13];
				array2[num10 + num4] = new Vector2(1f - num14, array[num12].y / num);
				array2[num10 + num4 + 1] = new Vector2(num14, array[num13].y / num);
				array3[num11 + num5 + 2] = num6;
				array3[num11 + num5 + 1] = num7;
				array3[num11 + num5] = num10 + num4;
				array3[num11 + num5 + 3] = num9;
				array3[num11 + num5 + 4] = num8;
				array3[num11 + num5 + 5] = num10 + num4 + 1;
			}
			for (int j = 0; j < array.Length; j++)
			{
				array[j] += zero;
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
