using System;
using UnityEngine;

namespace PrimitivesPro.Utils
{
	// Token: 0x02000886 RID: 2182
	public class Plane
	{
		// Token: 0x06004325 RID: 17189 RVA: 0x00160FE0 File Offset: 0x0015F3E0
		public Plane(Vector3 a, Vector3 b, Vector3 c)
		{
			this.Normal = Vector3.Cross(b - a, c - a).normalized;
			this.Distance = Vector3.Dot(this.Normal, a);
			this.Pnt = a;
		}

		// Token: 0x06004326 RID: 17190 RVA: 0x0016102D File Offset: 0x0015F42D
		public Plane(Vector3 normal, Vector3 p)
		{
			this.Normal = normal.normalized;
			this.Distance = Vector3.Dot(this.Normal, p);
			this.Pnt = p;
		}

		// Token: 0x06004327 RID: 17191 RVA: 0x0016105B File Offset: 0x0015F45B
		public Plane(Plane instance)
		{
			this.Normal = instance.Normal;
			this.Distance = instance.Distance;
			this.Pnt = instance.Pnt;
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06004328 RID: 17192 RVA: 0x00161087 File Offset: 0x0015F487
		// (set) Token: 0x06004329 RID: 17193 RVA: 0x0016108F File Offset: 0x0015F48F
		public Vector3 Pnt { get; private set; }

		// Token: 0x0600432A RID: 17194 RVA: 0x00161098 File Offset: 0x0015F498
		public Plane.PointClass ClassifyPoint(Vector3 p)
		{
			float num = Vector3.Dot(p, this.Normal) - this.Distance;
			return (num >= -0.0001f) ? ((num <= 0.0001f) ? Plane.PointClass.Coplanar : Plane.PointClass.Front) : Plane.PointClass.Back;
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x001610DC File Offset: 0x0015F4DC
		public bool GetSide(Vector3 n)
		{
			return Vector3.Dot(n, this.Normal) - this.Distance > 0.0001f;
		}

		// Token: 0x0600432C RID: 17196 RVA: 0x001610F8 File Offset: 0x0015F4F8
		public static bool GetSide(Vector3 a, Vector3 b, Vector3 c, Vector3 n)
		{
			Vector3 normalized = Vector3.Cross(b - a, c - a).normalized;
			float num = normalized.x * a.x + normalized.y * a.y + normalized.z * a.z;
			return n.x * normalized.x + n.y * normalized.y + n.z * normalized.z - num > 0.0001f;
		}

		// Token: 0x0600432D RID: 17197 RVA: 0x00161189 File Offset: 0x0015F589
		public void Flip()
		{
			this.Normal = -this.Normal;
			this.Distance = -this.Distance;
		}

		// Token: 0x0600432E RID: 17198 RVA: 0x001611AC File Offset: 0x0015F5AC
		public bool GetSideFix(ref Vector3 n)
		{
			float num = Vector3.Dot(n, this.Normal) - this.Distance;
			float num2 = 1f;
			float num3 = num;
			if (num < 0f)
			{
				num2 = -1f;
				num3 = -num;
			}
			if (num3 < 0.0011f)
			{
				n.x += this.Normal.x * 0.001f * num2;
				n.y += this.Normal.y * 0.001f * num2;
				n.z += this.Normal.z * 0.001f * num2;
				n += this.Normal * 0.001f * Mathf.Sign(num);
			}
			return Vector3.Dot(n, this.Normal) - this.Distance > 0.0001f;
		}

		// Token: 0x0600432F RID: 17199 RVA: 0x001612A4 File Offset: 0x0015F6A4
		public bool SameSide(Vector3 a, Vector3 b)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004330 RID: 17200 RVA: 0x001612AC File Offset: 0x0015F6AC
		public bool IntersectSegment(Vector3 a, Vector3 b, out float t, out Vector3 q)
		{
			Vector3 vector = b - a;
			if (Mathf.Abs(Vector3.Dot(this.Normal, vector)) < 0.0001f)
			{
			}
			t = (this.Distance - Vector3.Dot(this.Normal, a)) / Vector3.Dot(this.Normal, vector);
			if (t >= -0.0001f && t <= 1.0001f)
			{
				q = a + t * vector;
				return true;
			}
			q = Vector3.zero;
			return false;
		}

		// Token: 0x06004331 RID: 17201 RVA: 0x0016133C File Offset: 0x0015F73C
		public void InverseTransform(Transform transform)
		{
			Vector3 vector = transform.InverseTransformDirection(this.Normal);
			Vector3 rhs = transform.InverseTransformPoint(this.Pnt);
			this.Normal = vector;
			this.Distance = Vector3.Dot(vector, rhs);
		}

		// Token: 0x06004332 RID: 17202 RVA: 0x00161378 File Offset: 0x0015F778
		public void InverseTransform(Matrix4x4 matrix)
		{
			Matrix4x4 inverse = matrix.inverse;
			Vector3 vector = inverse.MultiplyVector(this.Normal);
			Vector3 rhs = inverse.MultiplyPoint(this.Pnt);
			this.Normal = vector;
			this.Distance = Vector3.Dot(vector, rhs);
		}

		// Token: 0x06004333 RID: 17203 RVA: 0x001613C0 File Offset: 0x0015F7C0
		public Matrix4x4 GetPlaneMatrix()
		{
			Matrix4x4 result = default(Matrix4x4);
			Quaternion q = Quaternion.LookRotation(this.Normal);
			result.SetTRS(this.Pnt, q, Vector3.one);
			return result;
		}

		// Token: 0x04002B86 RID: 11142
		private const float epsylon = 0.0001f;

		// Token: 0x04002B87 RID: 11143
		public Vector3 Normal;

		// Token: 0x04002B89 RID: 11145
		public float Distance;

		// Token: 0x02000887 RID: 2183
		[Flags]
		public enum PointClass
		{
			// Token: 0x04002B8B RID: 11147
			Coplanar = 0,
			// Token: 0x04002B8C RID: 11148
			Front = 1,
			// Token: 0x04002B8D RID: 11149
			Back = 2,
			// Token: 0x04002B8E RID: 11150
			Intersection = 3
		}
	}
}
