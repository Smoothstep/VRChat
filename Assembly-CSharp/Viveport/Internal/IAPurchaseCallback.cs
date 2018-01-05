using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x020009A1 RID: 2465
	// (Invoke) Token: 0x06004A66 RID: 19046
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void IAPurchaseCallback(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
}
