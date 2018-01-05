using System;
using UnityEngine;

namespace CapturePanorama
{
	// Token: 0x0200046C RID: 1132
	public static class Icosphere
	{
		// Token: 0x06002767 RID: 10087 RVA: 0x000CB050 File Offset: 0x000C9450
		public static Mesh BuildIcosphere(float radius, int iterations)
		{
			Mesh mesh = Icosphere.BuildIcosahedron(radius);
			for (int i = 0; i < iterations; i++)
			{
				Icosphere.Refine(mesh);
			}
			return mesh;
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x000CB080 File Offset: 0x000C9480
		public static Mesh BuildIcosahedron(float radius)
		{
			Mesh mesh = new Mesh();
			float num = (float)((1.0 + Math.Sqrt(5.0)) / 2.0);
			Vector3[] array = new Vector3[]
			{
				new Vector3(-1f, num, 0f),
				new Vector3(1f, num, 0f),
				new Vector3(-1f, -num, 0f),
				new Vector3(1f, -num, 0f),
				new Vector3(0f, -1f, num),
				new Vector3(0f, 1f, num),
				new Vector3(0f, -1f, -num),
				new Vector3(0f, 1f, -num),
				new Vector3(num, 0f, -1f),
				new Vector3(num, 0f, 1f),
				new Vector3(-num, 0f, -1f),
				new Vector3(-num, 0f, 1f)
			};
			Vector3 vector = new Vector3(1f, num, 0f);
			float d = radius / vector.magnitude;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] *= d;
			}
			mesh.vertices = array;
			mesh.triangles = new int[]
			{
				0,
				11,
				5,
				0,
				5,
				1,
				0,
				1,
				7,
				0,
				7,
				10,
				0,
				10,
				11,
				1,
				5,
				9,
				5,
				11,
				4,
				11,
				10,
				2,
				10,
				7,
				6,
				7,
				1,
				8,
				3,
				9,
				4,
				3,
				4,
				2,
				3,
				2,
				6,
				3,
				6,
				8,
				3,
				8,
				9,
				4,
				9,
				5,
				2,
				4,
				11,
				6,
				2,
				10,
				8,
				6,
				7,
				9,
				8,
				1
			};
			return mesh;
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000CB285 File Offset: 0x000C9685
		private static void Refine(Mesh m)
		{
			throw new Exception("TODO");
		}
	}
}
