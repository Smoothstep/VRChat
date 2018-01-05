using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Viveport.Internal
{
	// Token: 0x020009A5 RID: 2469
	internal class User
	{
		// Token: 0x06004A7B RID: 19067 RVA: 0x0018C4CA File Offset: 0x0018A8CA
		static User()
		{
			Api.LoadLibraryManually("viveport_api.dll");
		}

		// Token: 0x06004A7D RID: 19069
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_GetUserID")]
		internal static extern int GetUserID(StringBuilder userId, int size);

		// Token: 0x06004A7E RID: 19070
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_GetUserName")]
		internal static extern int GetUserName(StringBuilder userName, int size);

		// Token: 0x06004A7F RID: 19071
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_GetUserAvatarUrl")]
		internal static extern int GetUserAvatarUrl(StringBuilder userAvatarUrl, int size);
	}
}
