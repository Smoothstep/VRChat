using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000998 RID: 2456
	// (Invoke) Token: 0x06004A5A RID: 19034
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void GetLicenseCallback([MarshalAs(UnmanagedType.LPStr)] string message, [MarshalAs(UnmanagedType.LPStr)] string signature);
}
