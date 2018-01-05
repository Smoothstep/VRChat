using System;
using System.Text;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x02000981 RID: 2433
	public class User
	{
		// Token: 0x060049AF RID: 18863 RVA: 0x0018A8E8 File Offset: 0x00188CE8
		public static string GetUserId()
		{
			StringBuilder stringBuilder = new StringBuilder(256);

            Viveport.Internal.User.GetUserID(stringBuilder, 256);
			return stringBuilder.ToString();
		}

		// Token: 0x060049B0 RID: 18864 RVA: 0x0018A914 File Offset: 0x00188D14
		public static string GetUserName()
		{
			StringBuilder stringBuilder = new StringBuilder(256);
            Viveport.Internal.User.GetUserName(stringBuilder, 256);
			return stringBuilder.ToString();
		}

		// Token: 0x060049B1 RID: 18865 RVA: 0x0018A940 File Offset: 0x00188D40
		public static string GetUserAvatarUrl()
		{
			StringBuilder stringBuilder = new StringBuilder(512);
            Viveport.Internal.User.GetUserAvatarUrl(stringBuilder, 512);
			return stringBuilder.ToString();
		}

		// Token: 0x040031F5 RID: 12789
		private const int MaxIdLength = 256;

		// Token: 0x040031F6 RID: 12790
		private const int MaxNameLength = 256;

		// Token: 0x040031F7 RID: 12791
		private const int MaxUrlLength = 512;
	}
}
