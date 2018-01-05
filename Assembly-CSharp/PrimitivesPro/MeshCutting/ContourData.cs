using System;
using System.Collections.Generic;
using PrimitivesPro.Utils;
using UnityEngine;

namespace PrimitivesPro.MeshCutting
{
	// Token: 0x02000863 RID: 2147
	public class ContourData
	{
		// Token: 0x06004296 RID: 17046 RVA: 0x001526A8 File Offset: 0x00150AA8
		public ContourData(List<Vector3[]> contourList, Transform transformData)
		{
			this.contours = contourList;
			this.transform = default(Matrix4x4);
			this.transform.SetTRS(transformData.position, transformData.rotation, transformData.localScale);
		}

		// Token: 0x06004297 RID: 17047 RVA: 0x001526EE File Offset: 0x00150AEE
		public List<Vector3[]> GetLocalContours()
		{
			return this.contours;
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x001526F8 File Offset: 0x00150AF8
		public List<Vector3[]> GetWorldContours()
		{
			List<Vector3[]> list = new List<Vector3[]>(this.contours.Count);
			for (int i = 0; i < this.contours.Count; i++)
			{
				Vector3[] array = new Vector3[this.contours[i].Length];
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = this.TransformPoint(this.contours[i][j]);
				}
				list.Add(array);
			}
			return list;
		}

		// Token: 0x06004299 RID: 17049 RVA: 0x0015278C File Offset: 0x00150B8C
		public GameObject CreateGameObject(bool doubleSide)
		{
			if (this.contours.Count == 0 || this.contours[0].Length < 3)
			{
				return null;
			}
			List<Vector3[]> worldContours = this.GetWorldContours();
			List<Polygon> list = new List<Polygon>(worldContours.Count);
			Mesh mesh = new Mesh();
			List<int> list2 = new List<int>();
			List<Vector3> list3 = new List<Vector3>();
			List<Vector3> list4 = new List<Vector3>();
			List<Vector2> list5 = new List<Vector2>();
			int num = 0;
			PrimitivesPro.Utils.Plane plane = new PrimitivesPro.Utils.Plane(ContourData.GetNormal(worldContours[0]), worldContours[0][0]);
			Matrix4x4 planeMatrix = plane.GetPlaneMatrix();
			Matrix4x4 inverse = planeMatrix.inverse;
			float z = (inverse * worldContours[0][0]).z;
			foreach (Vector3[] array in worldContours)
			{
				Vector2[] array2 = new Vector2[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = inverse * array[i];
				}
				list.Add(new Polygon(array2));
			}
			ContourData.CollapsePolygons(list);
			foreach (Polygon polygon in list)
			{
				List<int> list6 = polygon.Triangulate();
				float num2 = Mathf.Min(polygon.Min.x, polygon.Min.y);
				float num3 = Mathf.Max(polygon.Max.x, polygon.Max.y);
				float num4 = num2 - num3;
				foreach (Vector2 vector in polygon.Points)
				{
					Vector3 item = planeMatrix * new Vector3(vector.x, vector.y, z);
					list3.Add(item);
					list4.Add(plane.Normal);
					list5.Add(new Vector2((vector.x - num2) / num4, (vector.y - num2) / num4));
				}
				foreach (int num5 in list6)
				{
					list2.Add(num5 + num);
				}
				num += list6.Count;
			}
			if (doubleSide)
			{
				int count = list3.Count;
				for (int k = 0; k < count; k++)
				{
					list3.Add(list3[k]);
					list4.Add(-list4[0]);
					list5.Add(list5[k]);
				}
				count = list2.Count;
				for (int l = 0; l < count; l++)
				{
					list2.Add(list2[count - l - 1]);
				}
			}
			mesh.vertices = list3.ToArray();
			mesh.normals = list3.ToArray();
			mesh.uv = list5.ToArray();
			mesh.triangles = list2.ToArray();
			GameObject gameObject = GameObject.Find("ContourObject");
			if (gameObject)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
			GameObject gameObject2 = new GameObject("ContourObject");
			gameObject2.AddComponent<MeshFilter>().sharedMesh = mesh;
			gameObject2.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Diffuse"));
			return gameObject2;
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x00152BB8 File Offset: 0x00150FB8
		private Vector3 TransformPoint(Vector3 localPos)
		{
			return this.transform.MultiplyPoint3x4(localPos);
		}

		// Token: 0x0600429B RID: 17051 RVA: 0x00152BC8 File Offset: 0x00150FC8
		private static void CollapsePolygons(List<Polygon> polygon)
		{
			if (polygon.Count > 0)
			{
				bool flag = true;
				while (flag)
				{
					flag = false;
					for (int i = 0; i < polygon.Count; i++)
					{
						if (flag)
						{
							break;
						}
						for (int j = 0; j < polygon.Count; j++)
						{
							if (flag)
							{
								break;
							}
							if (i != j)
							{
								Polygon polygon2 = polygon[i];
								Polygon polygon3 = polygon[j];
								if (polygon2.IsPolygonInside(polygon3))
								{
									polygon2.AddHole(polygon3);
									polygon.Remove(polygon3);
									flag = true;
								}
								if (polygon3.IsPolygonInside(polygon2))
								{
									polygon3.AddHole(polygon2);
									polygon.Remove(polygon2);
									flag = true;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600429C RID: 17052 RVA: 0x00152C8C File Offset: 0x0015108C
		public void ShowContourDBG(float duration)
		{
			List<Vector3[]> worldContours = this.GetWorldContours();
			foreach (Vector3[] array in worldContours)
			{
				Vector3 start = array[0];
				for (int i = 1; i < array.Length; i++)
				{
					Debug.DrawLine(start, array[i], Color.red, duration);
					start = array[i];
				}
				Debug.DrawLine(start, array[0], Color.red, duration);
				ContourData.GetNormal(worldContours[0]);
			}
		}

		// Token: 0x0600429D RID: 17053 RVA: 0x00152D54 File Offset: 0x00151154
		private static Vector3 GetNormal(Vector3[] points)
		{
			Vector3 a = points[0];
			Vector3 vector = points[1];
			int num = 1;
			while ((a - vector).sqrMagnitude < 0.01f)
			{
				vector = points[num++];
				if (num == points.Length)
				{
					return Vector3.zero;
				}
			}
			Vector3 vector2 = points[num];
			while ((a - vector2).sqrMagnitude < 0.01f || (vector - vector2).sqrMagnitude < 0.01f)
			{
				vector2 = points[num++];
				if (num == points.Length)
				{
					return Vector3.zero;
				}
			}
			Vector3 result = MeshUtils.ComputePolygonNormal(a, vector, vector2);
			if (result.sqrMagnitude < 0.01f)
			{
				return Vector3.zero;
			}
			return result;
		}

		// Token: 0x04002B3E RID: 11070
		private readonly List<Vector3[]> contours;

		// Token: 0x04002B3F RID: 11071
		private Matrix4x4 transform;
	}
}
