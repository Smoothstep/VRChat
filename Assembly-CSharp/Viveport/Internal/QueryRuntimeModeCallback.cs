using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x0200099A RID: 2458
	// (Invoke) Token: 0x06004A62 RID: 19042
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void QueryRuntimeModeCallback(int nResult, int nMode);
}
