using System;

namespace LitJson
{
	// Token: 0x02000409 RID: 1033
	internal enum ParserToken
	{
		// Token: 0x040012B3 RID: 4787
		None = 65536,
		// Token: 0x040012B4 RID: 4788
		Number,
		// Token: 0x040012B5 RID: 4789
		True,
		// Token: 0x040012B6 RID: 4790
		False,
		// Token: 0x040012B7 RID: 4791
		Null,
		// Token: 0x040012B8 RID: 4792
		CharSeq,
		// Token: 0x040012B9 RID: 4793
		Char,
		// Token: 0x040012BA RID: 4794
		Text,
		// Token: 0x040012BB RID: 4795
		Object,
		// Token: 0x040012BC RID: 4796
		ObjectPrime,
		// Token: 0x040012BD RID: 4797
		Pair,
		// Token: 0x040012BE RID: 4798
		PairRest,
		// Token: 0x040012BF RID: 4799
		Array,
		// Token: 0x040012C0 RID: 4800
		ArrayPrime,
		// Token: 0x040012C1 RID: 4801
		Value,
		// Token: 0x040012C2 RID: 4802
		ValueRest,
		// Token: 0x040012C3 RID: 4803
		String,
		// Token: 0x040012C4 RID: 4804
		End,
		// Token: 0x040012C5 RID: 4805
		Epsilon
	}
}
