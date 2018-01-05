using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x020009A4 RID: 2468
	internal class Api
	{
		// Token: 0x06004A72 RID: 19058 RVA: 0x0018C4B4 File Offset: 0x0018A8B4
		static Api()
		{
			Api.LoadLibraryManually("viveport_api.dll");
		}

		// Token: 0x06004A74 RID: 19060
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportAPI_GetLicense")]
		internal static extern void GetLicense(GetLicenseCallback callback, string appId, string appKey);

		// Token: 0x06004A75 RID: 19061
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportAPI_Init")]
		internal static extern int Init(StatusCallback initCallback, string appId);

		// Token: 0x06004A76 RID: 19062
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportAPI_Shutdown")]
		internal static extern int Shutdown(StatusCallback initCallback);

		// Token: 0x06004A77 RID: 19063
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportAPI_Version")]
		internal static extern IntPtr Version();

		// Token: 0x06004A78 RID: 19064
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportAPI_QueryRuntimeMode")]
		internal static extern void QueryRuntimeMode(QueryRuntimeModeCallback queryRunTimeCallback);

		// Token: 0x06004A79 RID: 19065
		[DllImport("kernel32.dll")]
		internal static extern IntPtr LoadLibrary(string dllToLoad);

		// Token: 0x06004A7A RID: 19066 RVA: 0x0018C4C8 File Offset: 0x0018A8C8
		internal static void LoadLibraryManually(string dllName)
		{
		}
	}
}
