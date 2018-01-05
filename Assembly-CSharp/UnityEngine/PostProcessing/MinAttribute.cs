using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020007C5 RID: 1989
	public sealed class MinAttribute : PropertyAttribute
	{
		// Token: 0x0600401C RID: 16412 RVA: 0x00142354 File Offset: 0x00140754
		public MinAttribute(float min)
		{
			this.min = min;
		}

		// Token: 0x0400286B RID: 10347
		public readonly float min;
	}
}
