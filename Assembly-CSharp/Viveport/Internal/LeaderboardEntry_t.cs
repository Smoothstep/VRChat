using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x020009A0 RID: 2464
	internal struct LeaderboardEntry_t
	{
		// Token: 0x04003248 RID: 12872
		internal int m_nGlobalRank;

		// Token: 0x04003249 RID: 12873
		internal int m_nScore;

		// Token: 0x0400324A RID: 12874
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		internal string m_pUserName;
	}
}
