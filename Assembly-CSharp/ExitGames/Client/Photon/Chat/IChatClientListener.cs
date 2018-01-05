using System;

namespace ExitGames.Client.Photon.Chat
{
	// Token: 0x020007B7 RID: 1975
	public interface IChatClientListener
	{
		// Token: 0x06003FBD RID: 16317
		void DebugReturn(DebugLevel level, string message);

		// Token: 0x06003FBE RID: 16318
		void OnDisconnected();

		// Token: 0x06003FBF RID: 16319
		void OnConnected();

		// Token: 0x06003FC0 RID: 16320
		void OnChatStateChange(ChatState state);

		// Token: 0x06003FC1 RID: 16321
		void OnGetMessages(string channelName, string[] senders, object[] messages);

		// Token: 0x06003FC2 RID: 16322
		void OnPrivateMessage(string sender, object message, string channelName);

		// Token: 0x06003FC3 RID: 16323
		void OnSubscribed(string[] channels, bool[] results);

		// Token: 0x06003FC4 RID: 16324
		void OnUnsubscribed(string[] channels);

		// Token: 0x06003FC5 RID: 16325
		void OnStatusUpdate(string user, int status, bool gotMessage, object message);
	}
}
