using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x020009A3 RID: 2467
	internal class ArcadeLeaderboard
	{
		// Token: 0x06004A69 RID: 19049 RVA: 0x0018C4A0 File Offset: 0x0018A8A0
		static ArcadeLeaderboard()
		{
			Api.LoadLibraryManually("viveport_api.dll");
		}

		// Token: 0x06004A6B RID: 19051
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_IsReady")]
		internal static extern void IsReady(StatusCallback IsReadyCallback);

		// Token: 0x06004A6C RID: 19052
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_DownloadLeaderboardScores")]
		internal static extern void DownloadLeaderboardScores(StatusCallback downloadLeaderboardScoresCB, string pchLeaderboardName, ELeaderboardDataTimeRange eLeaderboardDataTimeRange, int nCount);

		// Token: 0x06004A6D RID: 19053
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_UploadLeaderboardScore")]
		internal static extern void UploadLeaderboardScore(StatusCallback uploadLeaderboardScoreCB, string pchLeaderboardName, string pchUserName, int nScore);

		// Token: 0x06004A6E RID: 19054
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_GetLeaderboardScore")]
		internal static extern void GetLeaderboardScore(int index, ref LeaderboardEntry_t pLeaderboardEntry);

		// Token: 0x06004A6F RID: 19055
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_GetLeaderboardScoreCount")]
		internal static extern int GetLeaderboardScoreCount();

		// Token: 0x06004A70 RID: 19056
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_GetLeaderboardUserRank")]
		internal static extern int GetLeaderboardUserRank();

		// Token: 0x06004A71 RID: 19057
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_GetLeaderboardUserScore")]
		internal static extern int GetLeaderboardUserScore();
	}
}
