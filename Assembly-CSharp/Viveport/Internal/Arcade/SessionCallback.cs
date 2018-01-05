using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal.Arcade
{
	// Token: 0x020009A8 RID: 2472
	// (Invoke) Token: 0x06004A9F RID: 19103
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void SessionCallback(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
}
