using System;
using UnityEngine;

namespace RealisticEyeMovements
{
	// Token: 0x020008C2 RID: 2242
	[Serializable]
	public struct SerializableVector3
	{
		// Token: 0x06004490 RID: 17552 RVA: 0x0016E733 File Offset: 0x0016CB33
		public SerializableVector3(float rX, float rY, float rZ)
		{
			this.x = rX;
			this.y = rY;
			this.z = rZ;
		}

		// Token: 0x06004491 RID: 17553 RVA: 0x0016E74A File Offset: 0x0016CB4A
		public override string ToString()
		{
			return string.Format("[{0}, {1}, {2}]", this.x, this.y, this.z);
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x0016E777 File Offset: 0x0016CB77
		public static implicit operator Vector3(SerializableVector3 rValue)
		{
			return new Vector3(rValue.x, rValue.y, rValue.z);
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x0016E793 File Offset: 0x0016CB93
		public static implicit operator SerializableVector3(Vector3 rValue)
		{
			return new SerializableVector3(rValue.x, rValue.y, rValue.z);
		}

		// Token: 0x04002E53 RID: 11859
		public float x;

		// Token: 0x04002E54 RID: 11860
		public float y;

		// Token: 0x04002E55 RID: 11861
		public float z;
	}
}
