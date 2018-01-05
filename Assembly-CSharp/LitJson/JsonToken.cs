using System;

namespace LitJson
{
	// Token: 0x02000401 RID: 1025
	public enum JsonToken
	{
		// Token: 0x04001253 RID: 4691
		None,
		// Token: 0x04001254 RID: 4692
		ObjectStart,
		// Token: 0x04001255 RID: 4693
		PropertyName,
		// Token: 0x04001256 RID: 4694
		ObjectEnd,
		// Token: 0x04001257 RID: 4695
		ArrayStart,
		// Token: 0x04001258 RID: 4696
		ArrayEnd,
		// Token: 0x04001259 RID: 4697
		Int,
		// Token: 0x0400125A RID: 4698
		Long,
		// Token: 0x0400125B RID: 4699
		Double,
		// Token: 0x0400125C RID: 4700
		String,
		// Token: 0x0400125D RID: 4701
		Boolean,
		// Token: 0x0400125E RID: 4702
		Null
	}
}
