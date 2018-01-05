using System;
using UnityEngine;

namespace RealisticEyeMovements
{
	// Token: 0x020008C1 RID: 2241
	[Serializable]
	public struct SerializableQuaternion
	{
		// Token: 0x0600448C RID: 17548 RVA: 0x0016E677 File Offset: 0x0016CA77
		public SerializableQuaternion(float rX, float rY, float rZ, float rW)
		{
			this.x = rX;
			this.y = rY;
			this.z = rZ;
			this.w = rW;
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x0016E698 File Offset: 0x0016CA98
		public override string ToString()
		{
			return string.Format("[{0}, {1}, {2}, {3}]", new object[]
			{
				this.x,
				this.y,
				this.z,
				this.w
			});
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x0016E6ED File Offset: 0x0016CAED
		public static implicit operator Quaternion(SerializableQuaternion rValue)
		{
			return new Quaternion(rValue.x, rValue.y, rValue.z, rValue.w);
		}

		// Token: 0x0600448F RID: 17551 RVA: 0x0016E710 File Offset: 0x0016CB10
		public static implicit operator SerializableQuaternion(Quaternion rValue)
		{
			return new SerializableQuaternion(rValue.x, rValue.y, rValue.z, rValue.w);
		}

		// Token: 0x04002E4F RID: 11855
		public float x;

		// Token: 0x04002E50 RID: 11856
		public float y;

		// Token: 0x04002E51 RID: 11857
		public float z;

		// Token: 0x04002E52 RID: 11858
		public float w;
	}
}
