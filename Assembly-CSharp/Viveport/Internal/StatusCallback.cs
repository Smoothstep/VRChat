using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000999 RID: 2457
	// (Invoke) Token: 0x06004A5E RID: 19038
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void StatusCallback(int nResult);
}
