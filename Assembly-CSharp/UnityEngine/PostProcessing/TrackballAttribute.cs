using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020007C6 RID: 1990
	public sealed class TrackballAttribute : PropertyAttribute
	{
		// Token: 0x0600401D RID: 16413 RVA: 0x00142363 File Offset: 0x00140763
		public TrackballAttribute(string method)
		{
			this.method = method;
		}

		// Token: 0x0400286C RID: 10348
		public readonly string method;
	}
}
