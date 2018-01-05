using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x020009A6 RID: 2470
	internal class UserStats
	{
		// Token: 0x06004A80 RID: 19072 RVA: 0x0018C4DE File Offset: 0x0018A8DE
		static UserStats()
		{
			Api.LoadLibraryManually("viveport_api.dll");
		}

		// Token: 0x06004A82 RID: 19074
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_IsReady")]
		internal static extern int IsReady(StatusCallback IsReadyCallback);

		// Token: 0x06004A83 RID: 19075
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_DownloadStats")]
		internal static extern int DownloadStats(StatusCallback downloadStatsCallback);

		// Token: 0x06004A84 RID: 19076
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetStat0")]
		internal static extern int GetStat(string pchName, ref int pnData);

		// Token: 0x06004A85 RID: 19077
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetStat")]
		internal static extern int GetStat(string pchName, ref float pfData);

		// Token: 0x06004A86 RID: 19078
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_SetStat0")]
		internal static extern int SetStat(string pchName, int nData);

		// Token: 0x06004A87 RID: 19079
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_SetStat")]
		internal static extern int SetStat(string pchName, float fData);

		// Token: 0x06004A88 RID: 19080
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_UploadStats")]
		internal static extern int UploadStats(StatusCallback uploadStatsCallback);

		// Token: 0x06004A89 RID: 19081
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetAchievement")]
		internal static extern int GetAchievement(string pchName, ref int pbAchieved);

		// Token: 0x06004A8A RID: 19082
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetAchievementUnlockTime")]
		internal static extern int GetAchievementUnlockTime(string pchName, ref int punUnlockTime);

		// Token: 0x06004A8B RID: 19083
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_SetAchievement")]
		internal static extern int SetAchievement(string pchName);

		// Token: 0x06004A8C RID: 19084
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_ClearAchievement")]
		internal static extern int ClearAchievement(string pchName);

		// Token: 0x06004A8D RID: 19085
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_DownloadLeaderboardScores")]
		internal static extern int DownloadLeaderboardScores(StatusCallback downloadLeaderboardScoresCB, string pchLeaderboardName, ELeaderboardDataRequest eLeaderboardDataRequest, ELeaderboardDataTimeRange eLeaderboardDataTimeRange, int nRangeStart, int nRangeEnd);

		// Token: 0x06004A8E RID: 19086
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_UploadLeaderboardScore")]
		internal static extern int UploadLeaderboardScore(StatusCallback uploadLeaderboardScoreCB, string pchLeaderboardName, int nScore);

		// Token: 0x06004A8F RID: 19087
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetLeaderboardScore")]
		internal static extern int GetLeaderboardScore(int index, ref LeaderboardEntry_t pLeaderboardEntry);

		// Token: 0x06004A90 RID: 19088
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetLeaderboardScoreCount")]
		internal static extern int GetLeaderboardScoreCount();

		// Token: 0x06004A91 RID: 19089
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetLeaderboardSortMethod")]
		internal static extern ELeaderboardSortMethod GetLeaderboardSortMethod();

		// Token: 0x06004A92 RID: 19090
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetLeaderboardDisplayType")]
		internal static extern ELeaderboardDisplayType GetLeaderboardDisplayType();
	}
}
