using System;
using UnityEngine;

namespace OvrTouch.Hands
{
	// Token: 0x02000716 RID: 1814
	public class GrabTrigger : MonoBehaviour
	{
		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x06003B36 RID: 15158 RVA: 0x0012A3C3 File Offset: 0x001287C3
		public Grabbable Grabbable
		{
			get
			{
				return this.m_grabbable;
			}
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x0012A3CB File Offset: 0x001287CB
		public void SetGrabbable(Grabbable grabbable)
		{
			if (this.m_grabbable != null)
			{
				throw new InvalidOperationException("GrabTrigger: Grabbable already set by a different grab point -- make sure multiple grab points are not referencing the same collider.");
			}
			this.m_grabbable = grabbable;
		}

		// Token: 0x040023EF RID: 9199
		private Grabbable m_grabbable;
	}
}
