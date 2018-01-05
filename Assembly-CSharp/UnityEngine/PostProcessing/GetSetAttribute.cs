using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020007C4 RID: 1988
	public sealed class GetSetAttribute : PropertyAttribute
	{
		// Token: 0x0600401B RID: 16411 RVA: 0x00142345 File Offset: 0x00140745
		public GetSetAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x04002869 RID: 10345
		public readonly string name;

		// Token: 0x0400286A RID: 10346
		public bool dirty;
	}
}
