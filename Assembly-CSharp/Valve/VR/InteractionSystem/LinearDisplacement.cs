using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BB9 RID: 3001
	public class LinearDisplacement : MonoBehaviour
	{
		// Token: 0x06005CD9 RID: 23769 RVA: 0x00206B34 File Offset: 0x00204F34
		private void Start()
		{
			this.initialPosition = base.transform.localPosition;
			if (this.linearMapping == null)
			{
				this.linearMapping = base.GetComponent<LinearMapping>();
			}
		}

		// Token: 0x06005CDA RID: 23770 RVA: 0x00206B64 File Offset: 0x00204F64
		private void Update()
		{
			if (this.linearMapping)
			{
				base.transform.localPosition = this.initialPosition + this.linearMapping.value * this.displacement;
			}
		}

		// Token: 0x0400426E RID: 17006
		public Vector3 displacement;

		// Token: 0x0400426F RID: 17007
		public LinearMapping linearMapping;

		// Token: 0x04004270 RID: 17008
		private Vector3 initialPosition;
	}
}
