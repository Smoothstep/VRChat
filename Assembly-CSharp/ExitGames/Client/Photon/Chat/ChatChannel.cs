using System;
using System.Collections.Generic;
using System.Text;

namespace ExitGames.Client.Photon.Chat
{
	// Token: 0x020007AA RID: 1962
	public class ChatChannel
	{
		// Token: 0x06003F5B RID: 16219 RVA: 0x0013EC39 File Offset: 0x0013D039
		public ChatChannel(string name)
		{
			this.Name = name;
		}

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x06003F5C RID: 16220 RVA: 0x0013EC5E File Offset: 0x0013D05E
		// (set) Token: 0x06003F5D RID: 16221 RVA: 0x0013EC66 File Offset: 0x0013D066
		public bool IsPrivate { get; protected internal set; }

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x06003F5E RID: 16222 RVA: 0x0013EC6F File Offset: 0x0013D06F
		public int MessageCount
		{
			get
			{
				return this.Messages.Count;
			}
		}

		// Token: 0x06003F5F RID: 16223 RVA: 0x0013EC7C File Offset: 0x0013D07C
		public void Add(string sender, object message)
		{
			this.Senders.Add(sender);
			this.Messages.Add(message);
			this.TruncateMessages();
		}

		// Token: 0x06003F60 RID: 16224 RVA: 0x0013EC9C File Offset: 0x0013D09C
		public void Add(string[] senders, object[] messages)
		{
			this.Senders.AddRange(senders);
			this.Messages.AddRange(messages);
			this.TruncateMessages();
		}

		// Token: 0x06003F61 RID: 16225 RVA: 0x0013ECBC File Offset: 0x0013D0BC
		public void TruncateMessages()
		{
			if (this.MessageLimit <= 0 || this.Messages.Count <= this.MessageLimit)
			{
				return;
			}
			int count = this.Messages.Count - this.MessageLimit;
			this.Senders.RemoveRange(0, count);
			this.Messages.RemoveRange(0, count);
		}

		// Token: 0x06003F62 RID: 16226 RVA: 0x0013ED19 File Offset: 0x0013D119
		public void ClearMessages()
		{
			this.Senders.Clear();
			this.Messages.Clear();
		}

		// Token: 0x06003F63 RID: 16227 RVA: 0x0013ED34 File Offset: 0x0013D134
		public string ToStringMessages()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.Messages.Count; i++)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", this.Senders[i], this.Messages[i]));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040027AF RID: 10159
		public readonly string Name;

		// Token: 0x040027B0 RID: 10160
		public readonly List<string> Senders = new List<string>();

		// Token: 0x040027B1 RID: 10161
		public readonly List<object> Messages = new List<object>();

		// Token: 0x040027B2 RID: 10162
		public int MessageLimit;
	}
}
