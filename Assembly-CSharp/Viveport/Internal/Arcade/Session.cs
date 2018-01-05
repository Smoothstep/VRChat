using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal.Arcade
{
	// Token: 0x020009A9 RID: 2473
	internal class Session
	{
		// Token: 0x06004AA3 RID: 19107
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IViveportArcadeSession_IsReady")]
		internal static extern void IsReady(SessionCallback callback);

		// Token: 0x06004AA4 RID: 19108
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IViveportArcadeSession_Start")]
		internal static extern void Start(SessionCallback callback);

		// Token: 0x06004AA5 RID: 19109
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IViveportArcadeSession_Stop")]
		internal static extern void Stop(SessionCallback callback);
	}
}
