using System;

namespace ExitGames.Client.Photon.Chat
{
	// Token: 0x020007AE RID: 1966
	public class ChatOperationCode
	{
		// Token: 0x040027DA RID: 10202
		public const byte Authenticate = 230;

		// Token: 0x040027DB RID: 10203
		public const byte Subscribe = 0;

		// Token: 0x040027DC RID: 10204
		public const byte Unsubscribe = 1;

		// Token: 0x040027DD RID: 10205
		public const byte Publish = 2;

		// Token: 0x040027DE RID: 10206
		public const byte SendPrivate = 3;

		// Token: 0x040027DF RID: 10207
		public const byte ChannelHistory = 4;

		// Token: 0x040027E0 RID: 10208
		public const byte UpdateStatus = 5;

		// Token: 0x040027E1 RID: 10209
		public const byte AddFriends = 6;

		// Token: 0x040027E2 RID: 10210
		public const byte RemoveFriends = 7;
	}
}
