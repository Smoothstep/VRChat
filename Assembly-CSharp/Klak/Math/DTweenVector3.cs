using System;
using UnityEngine;

namespace Klak.Math
{
	// Token: 0x02000525 RID: 1317
	internal struct DTweenVector3
	{
		// Token: 0x06002E3C RID: 11836 RVA: 0x000E25AA File Offset: 0x000E09AA
		public DTweenVector3(Vector3 position, float omega)
		{
			this.position = position;
			this.velocity = Vector3.zero;
			this.omega = omega;
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x000E25C5 File Offset: 0x000E09C5
		public void Step(Vector3 target)
		{
			this.position = DTween.Step(this.position, target, ref this.velocity, this.omega);
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x000E25E5 File Offset: 0x000E09E5
		public static implicit operator Vector3(DTweenVector3 m)
		{
			return m.position;
		}

		// Token: 0x0400186D RID: 6253
		public Vector3 position;

		// Token: 0x0400186E RID: 6254
		public Vector3 velocity;

		// Token: 0x0400186F RID: 6255
		public float omega;
	}
}
