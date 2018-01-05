using System;
using UnityEngine;

namespace Klak.Math
{
	// Token: 0x02000524 RID: 1316
	internal struct DTweenVector2
	{
		// Token: 0x06002E39 RID: 11833 RVA: 0x000E2566 File Offset: 0x000E0966
		public DTweenVector2(Vector2 position, float omega)
		{
			this.position = position;
			this.velocity = Vector2.zero;
			this.omega = omega;
		}

		// Token: 0x06002E3A RID: 11834 RVA: 0x000E2581 File Offset: 0x000E0981
		public void Step(Vector2 target)
		{
			this.position = DTween.Step(this.position, target, ref this.velocity, this.omega);
		}

		// Token: 0x06002E3B RID: 11835 RVA: 0x000E25A1 File Offset: 0x000E09A1
		public static implicit operator Vector2(DTweenVector2 m)
		{
			return m.position;
		}

		// Token: 0x0400186A RID: 6250
		public Vector2 position;

		// Token: 0x0400186B RID: 6251
		public Vector2 velocity;

		// Token: 0x0400186C RID: 6252
		public float omega;
	}
}
