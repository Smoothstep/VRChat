using System;
using UnityEngine;

namespace Klak.Math
{
	// Token: 0x02000526 RID: 1318
	internal struct DTweenQuaternion
	{
		// Token: 0x06002E3F RID: 11839 RVA: 0x000E25EE File Offset: 0x000E09EE
		public DTweenQuaternion(Quaternion rotation, float omega)
		{
			this.rotation = rotation;
			this.velocity = Vector4.zero;
			this.omega = omega;
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x000E2609 File Offset: 0x000E0A09
		public void Step(Quaternion target)
		{
			this.rotation = DTween.Step(this.rotation, target, ref this.velocity, this.omega);
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x000E2629 File Offset: 0x000E0A29
		public static implicit operator Quaternion(DTweenQuaternion m)
		{
			return m.rotation;
		}

		// Token: 0x04001870 RID: 6256
		public Quaternion rotation;

		// Token: 0x04001871 RID: 6257
		public Vector4 velocity;

		// Token: 0x04001872 RID: 6258
		public float omega;
	}
}
