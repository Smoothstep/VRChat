using System;
using Klak.VectorMathExtension;
using UnityEngine;

namespace Klak.Math
{
	// Token: 0x02000523 RID: 1315
	internal struct DTween
	{
		// Token: 0x06002E31 RID: 11825 RVA: 0x000E234D File Offset: 0x000E074D
		public DTween(float position, float omega)
		{
			this.position = position;
			this.velocity = 0f;
			this.omega = omega;
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x000E2368 File Offset: 0x000E0768
		public static float Step(float current, float target, ref float velocity, float omega)
		{
			float deltaTime = Time.deltaTime;
			float num = velocity - (current - target) * (omega * omega * deltaTime);
			float num2 = 1f + omega * deltaTime;
			velocity = num / (num2 * num2);
			return current + velocity * deltaTime;
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x000E23A0 File Offset: 0x000E07A0
		public static Vector2 Step(Vector2 current, Vector2 target, ref Vector2 velocity, float omega)
		{
			float deltaTime = Time.deltaTime;
			Vector2 a = velocity - (current - target) * (omega * omega * deltaTime);
			float num = 1f + omega * deltaTime;
			velocity = a / (num * num);
			return current + velocity * deltaTime;
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x000E23FC File Offset: 0x000E07FC
		public static Vector3 Step(Vector3 current, Vector3 target, ref Vector3 velocity, float omega)
		{
			float deltaTime = Time.deltaTime;
			Vector3 a = velocity - (current - target) * (omega * omega * deltaTime);
			float num = 1f + omega * deltaTime;
			velocity = a / (num * num);
			return current + velocity * deltaTime;
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x000E2458 File Offset: 0x000E0858
		public static Vector4 Step(Vector4 current, Vector4 target, ref Vector4 velocity, float omega)
		{
			float deltaTime = Time.deltaTime;
			Vector4 a = velocity - (current - target) * (omega * omega * deltaTime);
			float num = 1f + omega * deltaTime;
			velocity = a / (num * num);
			return current + velocity * deltaTime;
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x000E24B4 File Offset: 0x000E08B4
		public static Quaternion Step(Quaternion current, Quaternion target, ref Vector4 velocity, float omega)
		{
			Vector4 a = current.ToVector4();
			Vector4 vector = target.ToVector4();
			if (Vector4.Dot(a, vector) < 0f)
			{
				vector = -vector;
			}
			float deltaTime = Time.deltaTime;
			Vector4 a2 = velocity - (a - vector) * (omega * omega * deltaTime);
			float num = 1f + omega * deltaTime;
			velocity = a2 / (num * num);
			return (a + velocity * deltaTime).ToNormalizedQuaternion();
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x000E253D File Offset: 0x000E093D
		public void Step(float target)
		{
			this.position = DTween.Step(this.position, target, ref this.velocity, this.omega);
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x000E255D File Offset: 0x000E095D
		public static implicit operator float(DTween m)
		{
			return m.position;
		}

		// Token: 0x04001867 RID: 6247
		public float position;

		// Token: 0x04001868 RID: 6248
		public float velocity;

		// Token: 0x04001869 RID: 6249
		public float omega;
	}
}
