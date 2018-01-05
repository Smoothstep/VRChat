using System;
using UnityEngine;

// Token: 0x02000B08 RID: 2824
public class UserMessage : MonoBehaviour
{
	// Token: 0x17000C58 RID: 3160
	// (get) Token: 0x06005588 RID: 21896 RVA: 0x001D83CF File Offset: 0x001D67CF
	public static string message
	{
		get
		{
			return UserMessage.mMessage;
		}
	}

	// Token: 0x06005589 RID: 21897 RVA: 0x001D83D6 File Offset: 0x001D67D6
	public static void SetMessage(string message)
	{
		UserMessage.mMessage = message;
	}

	// Token: 0x04003C70 RID: 15472
	private static string mMessage;
}
