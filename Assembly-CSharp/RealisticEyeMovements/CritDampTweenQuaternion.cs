using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace RealisticEyeMovements
{
	// Token: 0x020008B4 RID: 2228
	internal struct CritDampTweenQuaternion
	{
		// Token: 0x06004427 RID: 17447 RVA: 0x00169E0A File Offset: 0x0016820A
		public CritDampTweenQuaternion(Quaternion rotation, float omega, float maxSpeed)
		{
			this._rotation.v = Vector4.zero;
			this._rotation.q = rotation;
			this.velocity = Vector4.zero;
			this.omega = omega;
			this.maxSpeed = maxSpeed;
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x00169E41 File Offset: 0x00168241
		private static Vector4 q2v(Quaternion q)
		{
			return new Vector4(q.x, q.y, q.z, q.w);
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06004429 RID: 17449 RVA: 0x00169E64 File Offset: 0x00168264
		// (set) Token: 0x0600442A RID: 17450 RVA: 0x00169E71 File Offset: 0x00168271
		public Quaternion rotation
		{
			get
			{
				return this._rotation.q;
			}
			set
			{
				this._rotation.q = value;
			}
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x00169E80 File Offset: 0x00168280
		public void Step(Quaternion target)
		{
			Vector4 vector = CritDampTweenQuaternion.q2v(target);
			if (Vector4.Dot(this._rotation.v, vector) < 0f)
			{
				vector = -vector;
			}
			float deltaTime = Time.deltaTime;
			Vector4 a = this.velocity - (this._rotation.v - vector) * (this.omega * this.omega * deltaTime);
			float num = 1f + this.omega * deltaTime;
			this.velocity = a / (num * num);
			float magnitude = this.velocity.magnitude;
			this.velocity = Mathf.Min(magnitude, this.maxSpeed) / magnitude * this.velocity;
			this._rotation.v = (this._rotation.v + this.velocity * deltaTime).normalized;
		}

		// Token: 0x0600442C RID: 17452 RVA: 0x00169F69 File Offset: 0x00168369
		public static implicit operator Quaternion(CritDampTweenQuaternion m)
		{
			return m.rotation;
		}

		// Token: 0x04002D85 RID: 11653
		private CritDampTweenQuaternion.QVUnion _rotation;

		// Token: 0x04002D86 RID: 11654
		public Vector4 velocity;

		// Token: 0x04002D87 RID: 11655
		public float omega;

		// Token: 0x04002D88 RID: 11656
		private readonly float maxSpeed;

		// Token: 0x020008B5 RID: 2229
		[StructLayout(LayoutKind.Explicit)]
		private struct QVUnion
		{
			// Token: 0x04002D89 RID: 11657
			[FieldOffset(0)]
			public Vector4 v;

			// Token: 0x04002D8A RID: 11658
			[FieldOffset(0)]
			public Quaternion q;
		}
	}
}
