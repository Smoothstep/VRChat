using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x020009A2 RID: 2466
	internal struct IAPCurrency_t
	{
		// Token: 0x0400324B RID: 12875
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		internal string m_pName;

		// Token: 0x0400324C RID: 12876
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		internal string m_pSymbol;
	}
}
