using System;
using System.Diagnostics;
using UnityEngine;

namespace PrimitivesPro
{
	// Token: 0x02000888 RID: 2184
	public static class MeshUtils
	{
		// Token: 0x06004334 RID: 17204 RVA: 0x001613F8 File Offset: 0x0015F7F8
		public static void CalculateTangents(Mesh mesh)
		{
			int vertexCount = mesh.vertexCount;
			Vector3[] vertices = mesh.vertices;
			Vector3[] normals = mesh.normals;
			Vector2[] uv = mesh.uv;
			int[] triangles = mesh.triangles;
			int num = triangles.Length / 3;
			Vector4[] array = new Vector4[vertexCount];
			Vector3[] array2 = new Vector3[vertexCount];
			Vector3[] array3 = new Vector3[vertexCount];
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				int num3 = triangles[num2];
				int num4 = triangles[num2 + 1];
				int num5 = triangles[num2 + 2];
				Vector3 vector = vertices[num3];
				Vector3 vector2 = vertices[num4];
				Vector3 vector3 = vertices[num5];
				Vector2 vector4 = uv[num3];
				Vector2 vector5 = uv[num4];
				Vector2 vector6 = uv[num5];
				float num6 = vector2.x - vector.x;
				float num7 = vector3.x - vector.x;
				float num8 = vector2.y - vector.y;
				float num9 = vector3.y - vector.y;
				float num10 = vector2.z - vector.z;
				float num11 = vector3.z - vector.z;
				float num12 = vector5.x - vector4.x;
				float num13 = vector6.x - vector4.x;
				float num14 = vector5.y - vector4.y;
				float num15 = vector6.y - vector4.y;
				float num16 = num12 * num15 - num13 * num14;
				float num17 = (Math.Abs(num16) >= 0.0001f) ? (1f / num16) : 0f;
				Vector3 b = new Vector3((num15 * num6 - num14 * num7) * num17, (num15 * num8 - num14 * num9) * num17, (num15 * num10 - num14 * num11) * num17);
				Vector3 b2 = new Vector3((num12 * num7 - num13 * num6) * num17, (num12 * num9 - num13 * num8) * num17, (num12 * num11 - num13 * num10) * num17);
				array2[num3] += b;
				array2[num4] += b;
				array2[num5] += b;
				array3[num3] += b2;
				array3[num4] += b2;
				array3[num5] += b2;
				num2 += 3;
			}
			for (int j = 0; j < vertexCount; j++)
			{
				Vector3 lhs = normals[j];
				Vector3 rhs = array2[j];
				Vector3.OrthoNormalize(ref lhs, ref rhs);
				array[j].x = rhs.x;
				array[j].y = rhs.y;
				array[j].z = rhs.z;
				array[j].w = (((double)Vector3.Dot(Vector3.Cross(lhs, rhs), array3[j]) >= 0.0) ? 1f : -1f);
			}
			mesh.tangents = array;
		}

		// Token: 0x06004335 RID: 17205 RVA: 0x0016178C File Offset: 0x0015FB8C
		public static void ReverseNormals(Mesh mesh)
		{
			Vector3[] normals = mesh.normals;
			for (int i = 0; i < normals.Length; i++)
			{
				normals[i] = -normals[i];
			}
			mesh.normals = normals;
			for (int j = 0; j < mesh.subMeshCount; j++)
			{
				int[] triangles = mesh.GetTriangles(j);
				for (int k = 0; k < triangles.Length; k += 3)
				{
					int num = triangles[k];
					triangles[k] = triangles[k + 1];
					triangles[k + 1] = num;
				}
				mesh.SetTriangles(triangles, j);
			}
		}

		// Token: 0x06004336 RID: 17206 RVA: 0x00161830 File Offset: 0x0015FC30
		public static void ComputeVertexNormals(Vector3[] vertices, int[] triangles, out Vector3[] normals)
		{
			normals = new Vector3[vertices.Length];
			for (int i = 0; i < triangles.Length; i += 3)
			{
				Vector3 b = vertices[triangles[i]];
				Vector3 a = vertices[triangles[i + 1]];
				Vector3 a2 = vertices[triangles[i + 2]];
				Vector3 b2 = Vector3.Cross(a - b, a2 - b);
				normals[triangles[i]] += b2;
				normals[triangles[i + 1]] += b2;
				normals[triangles[i + 2]] += b2;
			}
			for (int j = 0; j < normals.Length; j++)
			{
				normals[j].Normalize();
			}
		}

		// Token: 0x06004337 RID: 17207 RVA: 0x00161920 File Offset: 0x0015FD20
		public static Vector3 ComputePolygonNormal(Vector3 a, Vector3 b, Vector3 c)
		{
			return Vector3.Cross(b - a, c - a).normalized;
		}

		// Token: 0x06004338 RID: 17208 RVA: 0x00161948 File Offset: 0x0015FD48
		public static void DuplicateSharedVertices(ref Vector3[] vertices, ref Vector2[] uvs, int[] triangles, int trianglesMax)
		{
			Vector3[] array = new Vector3[triangles.Length];
			Vector2[] array2 = new Vector2[triangles.Length];
			if (trianglesMax == -1)
			{
				trianglesMax = triangles.Length;
			}
			int num = 0;
			for (int i = 0; i < trianglesMax; i += 3)
			{
				array[num] = vertices[triangles[i]];
				array[num + 1] = vertices[triangles[i + 1]];
				array[num + 2] = vertices[triangles[i + 2]];
				array2[num] = uvs[triangles[i]];
				array2[num + 1] = uvs[triangles[i + 1]];
				array2[num + 2] = uvs[triangles[i + 2]];
				triangles[i] = num;
				triangles[i + 1] = num + 1;
				triangles[i + 2] = num + 2;
				num += 3;
			}
			for (int j = trianglesMax; j < triangles.Length; j++)
			{
				array[j] = vertices[triangles[j]];
				array2[j] = uvs[triangles[j]];
			}
			vertices = array;
			uvs = array2;
		}

		// Token: 0x06004339 RID: 17209 RVA: 0x00161AAC File Offset: 0x0015FEAC
		public static Vector3 ComputeBarycentricCoordinates(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
		{
			Vector3 vector = b - a;
			Vector3 vector2 = c - a;
			Vector3 lhs = p - a;
			float num = Vector3.Dot(vector, vector);
			float num2 = Vector3.Dot(vector, vector2);
			float num3 = Vector3.Dot(vector2, vector2);
			float num4 = Vector3.Dot(lhs, vector);
			float num5 = Vector3.Dot(lhs, vector2);
			float num6 = num * num3 - num2 * num2;
			float num7 = (num3 * num4 - num2 * num5) / num6;
			float num8 = (num * num5 - num2 * num4) / num6;
			float x = 1f - num7 - num8;
			return new Vector3(x, num7, num8);
		}

		// Token: 0x0600433A RID: 17210 RVA: 0x00161B40 File Offset: 0x0015FF40
		public static void CenterPivot(Vector3[] vertices, Vector3 centroid)
		{
			int num = vertices.Length;
			for (int i = 0; i < num; i++)
			{
				Vector3 vector = vertices[i];
				vector.x -= centroid.x;
				vector.y -= centroid.y;
				vector.z -= centroid.z;
				vertices[i] = vector;
			}
		}

		// Token: 0x0600433B RID: 17211 RVA: 0x00161BBC File Offset: 0x0015FFBC
		public static void Swap<T>(ref T a, ref T b)
		{
			T t = a;
			a = b;
			b = t;
		}

		// Token: 0x0600433C RID: 17212 RVA: 0x00161BE4 File Offset: 0x0015FFE4
		public static Mesh CopyMesh(Mesh orginalMesh)
		{
			return new Mesh
			{
				vertices = orginalMesh.vertices,
				triangles = orginalMesh.triangles,
				uv = orginalMesh.uv,
				normals = orginalMesh.normals,
				colors = orginalMesh.colors,
				tangents = orginalMesh.tangents
			};
		}

		// Token: 0x0600433D RID: 17213 RVA: 0x00161C44 File Offset: 0x00160044
		public static Material[] CopyMaterials(Material[] materials)
		{
			Material[] array = new Material[materials.Length];
			for (int i = 0; i < materials.Length; i++)
			{
				array[i] = new Material(materials[i]);
			}
			return array;
		}

		// Token: 0x0600433E RID: 17214 RVA: 0x00161C7A File Offset: 0x0016007A
		public static Vector3 BezierQuadratic(Vector3 p0, Vector3 p1, Vector3 cp, float t)
		{
			return (1f - t) * (1f - t) * p0 + 2f * (1f - t) * t * cp + t * t * p1;
		}

		// Token: 0x0600433F RID: 17215 RVA: 0x00161CB9 File Offset: 0x001600B9
		[Conditional("UNITY_EDITOR")]
		public static void Assert(bool condition, string message)
		{
			if (!condition)
			{
				UnityEngine.Debug.LogError("Assert! " + message);
				UnityEngine.Debug.Break();
			}
		}

		// Token: 0x06004340 RID: 17216 RVA: 0x00161CD6 File Offset: 0x001600D6
		[Conditional("UNITY_EDITOR")]
		public static void Assert(bool condition)
		{
			if (!condition)
			{
				UnityEngine.Debug.LogError("Assert!");
				UnityEngine.Debug.Break();
			}
		}

		// Token: 0x06004341 RID: 17217 RVA: 0x00161CED File Offset: 0x001600ED
		[Conditional("UNITY_EDITOR")]
		public static void Log(string format, params object[] args)
		{
			UnityEngine.Debug.Log(string.Format(format, args));
		}

		// Token: 0x06004342 RID: 17218 RVA: 0x00161CFB File Offset: 0x001600FB
		[Conditional("UNITY_EDITOR")]
		public static void Log(object arg)
		{
			UnityEngine.Debug.Log(arg.ToString());
		}

		// Token: 0x06004343 RID: 17219 RVA: 0x00161D08 File Offset: 0x00160108
		public static bool IsGameObjectActive(GameObject obj)
		{
			return obj.activeSelf;
		}

		// Token: 0x06004344 RID: 17220 RVA: 0x00161D10 File Offset: 0x00160110
		public static void SetGameObjectActive(GameObject obj, bool status)
		{
			obj.SetActive(status);
		}

		// Token: 0x06004345 RID: 17221 RVA: 0x00161D1C File Offset: 0x0016011C
		public static float DistanceToLine2(Ray ray, Vector3 point)
		{
			return Vector3.Cross(ray.direction, point - ray.origin).sqrMagnitude;
		}

		// Token: 0x06004346 RID: 17222 RVA: 0x00161D4C File Offset: 0x0016014C
		public static bool RayTriangleIntersection(Vector3 V1, Vector3 V2, Vector3 V3, Vector3 O, Vector3 D, out float ot)
		{
			ot = -1f;
			Vector3 vector = V2 - V1;
			Vector3 vector2 = V3 - V1;
			Vector3 rhs = Vector3.Cross(D, vector2);
			float num = Vector3.Dot(vector, rhs);
			if (num > -Mathf.Epsilon && num < Mathf.Epsilon)
			{
				return false;
			}
			float num2 = 1f / num;
			Vector3 lhs = O - V1;
			float num3 = Vector3.Dot(lhs, rhs) * num2;
			if (num3 < 0f || num3 > 1f)
			{
				return false;
			}
			Vector3 rhs2 = Vector3.Cross(lhs, vector);
			float num4 = Vector3.Dot(D, rhs2) * num2;
			if (num4 < 0f || num3 + num4 > 1f)
			{
				return false;
			}
			float num5 = Vector3.Dot(vector2, rhs2) * num2;
			if (num5 > Mathf.Epsilon)
			{
				ot = num5;
				return true;
			}
			return false;
		}

		// Token: 0x06004347 RID: 17223 RVA: 0x00161E2C File Offset: 0x0016022C
		public static float LineLineDistance2(Vector3 p0, Vector3 p1, Vector3 q0, Vector3 q1)
		{
			Vector3 vector = p1 - p0;
			Vector3 vector2 = q1 - q0;
			Vector3 vector3 = p0 - q0;
			float num = Vector3.Dot(vector, vector);
			float num2 = Vector3.Dot(vector, vector2);
			float num3 = Vector3.Dot(vector2, vector2);
			float num4 = Vector3.Dot(vector, vector3);
			float num5 = Vector3.Dot(vector2, vector3);
			float num6 = num * num3 - num2 * num2;
			float d;
			float d2;
			if (num6 < Mathf.Epsilon)
			{
				d = 0f;
				d2 = ((num2 <= num3) ? (num5 / num3) : (num4 / num2));
			}
			else
			{
				d = (num2 * num5 - num3 * num4) / num6;
				d2 = (num * num5 - num2 * num4) / num6;
			}
			return (vector3 + d * vector - d2 * vector2).sqrMagnitude;
		}

		// Token: 0x06004348 RID: 17224 RVA: 0x00161F00 File Offset: 0x00160300
		public static float SegmentSegmentDistance2(Vector3 p0, Vector3 p1, Vector3 q0, Vector3 q1)
		{
			Vector3 vector = p1 - p0;
			Vector3 vector2 = q1 - q0;
			Vector3 vector3 = p0 - q0;
			float num = Vector3.Dot(vector, vector);
			float num2 = Vector3.Dot(vector, vector2);
			float num3 = Vector3.Dot(vector2, vector2);
			float num4 = Vector3.Dot(vector, vector3);
			float num5 = Vector3.Dot(vector2, vector3);
			float num6 = num * num3 - num2 * num2;
			float num7 = num6;
			float num8 = num6;
			float num9;
			float num10;
			if (num6 < Mathf.Epsilon)
			{
				num9 = 0f;
				num7 = 1f;
				num10 = num5;
				num8 = num3;
			}
			else
			{
				num9 = num2 * num5 - num3 * num4;
				num10 = num * num5 - num2 * num4;
				if ((double)num9 < 0.0)
				{
					num9 = 0f;
					num10 = num5;
					num8 = num3;
				}
				else if (num9 > num7)
				{
					num9 = num7;
					num10 = num5 + num2;
					num8 = num3;
				}
			}
			if (num10 < 0f)
			{
				num10 = 0f;
				if ((double)(-(double)num4) < 0.0)
				{
					num9 = 0f;
				}
				else if (-num4 > num)
				{
					num9 = num7;
				}
				else
				{
					num9 = -num4;
					num7 = num;
				}
			}
			else if (num10 > num8)
			{
				num10 = num8;
				if ((double)(-(double)num4 + num2) < 0.0)
				{
					num9 = 0f;
				}
				else if (-num4 + num2 > num)
				{
					num9 = num7;
				}
				else
				{
					num9 = -num4 + num2;
					num7 = num;
				}
			}
			float d = (Mathf.Abs(num9) >= Mathf.Epsilon) ? (num9 / num7) : 0f;
			float d2 = (Math.Abs(num10) >= Mathf.Epsilon) ? (num10 / num8) : 0f;
			return (vector3 + d * vector - d2 * vector2).sqrMagnitude;
		}
	}
}
