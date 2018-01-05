using System;
using UnityEngine;

// Token: 0x02000A8F RID: 2703
public static class GLOBAL
{
	// Token: 0x17000BFE RID: 3070
	// (get) Token: 0x0600516D RID: 20845 RVA: 0x001BE5E4 File Offset: 0x001BC9E4
	// (set) Token: 0x0600516E RID: 20846 RVA: 0x001BE5EB File Offset: 0x001BC9EB
	public static bool networkInitialized
	{
		get
		{
			return GLOBAL.m_NetworkInitalized;
		}
		set
		{
			GLOBAL.m_NetworkInitalized = value;
		}
	}

	// Token: 0x0600516F RID: 20847 RVA: 0x001BE5F3 File Offset: 0x001BC9F3
	public static void HandleException(string message)
	{
		Debug.LogError(message);
	}

	// Token: 0x040039B3 RID: 14771
	public static string K_PLAYER_NAME = "k_player_name";

	// Token: 0x040039B4 RID: 14772
	public static string K_AVATAR_NAME = "k_avatar_name";

	// Token: 0x040039B5 RID: 14773
	private static bool m_NetworkInitalized;

	// Token: 0x02000A90 RID: 2704
	public enum RFC
	{
		// Token: 0x040039B7 RID: 14775
		NetworkTimeRequest = 1,
		// Token: 0x040039B8 RID: 14776
		NetworkTimeResponse,
		// Token: 0x040039B9 RID: 14777
		SendPlayerState,
		// Token: 0x040039BA RID: 14778
		SendPlayerStateImmediately,
		// Token: 0x040039BB RID: 14779
		USpeakInit,
		// Token: 0x040039BC RID: 14780
		USpeakReceiveVoice
	}

	// Token: 0x02000A91 RID: 2705
	public enum WorldChannel
	{
		// Token: 0x040039BE RID: 14782
		Hub = 1,
		// Token: 0x040039BF RID: 14783
		CoffeeShop,
		// Token: 0x040039C0 RID: 14784
		Palace
	}
}
