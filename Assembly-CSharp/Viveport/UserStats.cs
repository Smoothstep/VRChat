using System;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x02000982 RID: 2434
	public class UserStats
	{
		// Token: 0x060049B3 RID: 18867 RVA: 0x0018A974 File Offset: 0x00188D74
		public static int IsReady(StatusCallback callback)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
            Viveport.Internal.StatusCallback statusCallback = new Viveport.Internal.StatusCallback(callback.Invoke);
			Api.InternalStatusCallbacks.Add(statusCallback);
			return Viveport.Internal.UserStats.IsReady(statusCallback);
		}

		// Token: 0x060049B4 RID: 18868 RVA: 0x0018A9B0 File Offset: 0x00188DB0
		public static int DownloadStats(StatusCallback callback)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
            Viveport.Internal.StatusCallback statusCallback = new Viveport.Internal.StatusCallback(callback.Invoke);
			Api.InternalStatusCallbacks.Add(statusCallback);
			return Viveport.Internal.UserStats.DownloadStats(statusCallback);
		}

		// Token: 0x060049B5 RID: 18869 RVA: 0x0018A9EC File Offset: 0x00188DEC
		public static int GetStat(string name, int defaultValue)
		{
			int result = defaultValue;
            Viveport.Internal.UserStats.GetStat(name, ref result);
			return result;
		}

		// Token: 0x060049B6 RID: 18870 RVA: 0x0018AA08 File Offset: 0x00188E08
		public static float GetStat(string name, float defaultValue)
		{
			float result = defaultValue;
            Viveport.Internal.UserStats.GetStat(name, ref result);
			return result;
		}

		// Token: 0x060049B7 RID: 18871 RVA: 0x0018AA21 File Offset: 0x00188E21
		public static void SetStat(string name, int value)
		{
            Viveport.Internal.UserStats.SetStat(name, value);
		}

		// Token: 0x060049B8 RID: 18872 RVA: 0x0018AA2B File Offset: 0x00188E2B
		public static void SetStat(string name, float value)
		{
            Viveport.Internal.UserStats.SetStat(name, value);
		}

		// Token: 0x060049B9 RID: 18873 RVA: 0x0018AA38 File Offset: 0x00188E38
		public static int UploadStats(StatusCallback callback)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
            Viveport.Internal.StatusCallback statusCallback = new Viveport.Internal.StatusCallback(callback.Invoke);
			Api.InternalStatusCallbacks.Add(statusCallback);
			return Viveport.Internal.UserStats.UploadStats(statusCallback);
		}

		// Token: 0x060049BA RID: 18874 RVA: 0x0018AA74 File Offset: 0x00188E74
		public static bool GetAchievement(string pchName)
		{
			int num = 0;
            Viveport.Internal.UserStats.GetAchievement(pchName, ref num);
			return num == 1;
		}

		// Token: 0x060049BB RID: 18875 RVA: 0x0018AA90 File Offset: 0x00188E90
		public static int GetAchievementUnlockTime(string pchName)
		{
			int result = 0;
            Viveport.Internal.UserStats.GetAchievementUnlockTime(pchName, ref result);
			return result;
		}

		// Token: 0x060049BC RID: 18876 RVA: 0x0018AAA9 File Offset: 0x00188EA9
		public static int SetAchievement(string pchName)
		{
			return Viveport.Internal.UserStats.SetAchievement(pchName);
		}

		// Token: 0x060049BD RID: 18877 RVA: 0x0018AAB1 File Offset: 0x00188EB1
		public static int ClearAchievement(string pchName)
		{
			return Viveport.Internal.UserStats.ClearAchievement(pchName);
		}

		// Token: 0x060049BE RID: 18878 RVA: 0x0018AABC File Offset: 0x00188EBC
		public static int DownloadLeaderboardScores(StatusCallback callback, string pchLeaderboardName, UserStats.LeaderBoardRequestType eLeaderboardDataRequest, UserStats.LeaderBoardTimeRange eLeaderboardDataTimeRange, int nRangeStart, int nRangeEnd)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
            Viveport.Internal.StatusCallback statusCallback = new Viveport.Internal.StatusCallback(callback.Invoke);
			Api.InternalStatusCallbacks.Add(statusCallback);
			return Viveport.Internal.UserStats.DownloadLeaderboardScores(statusCallback, pchLeaderboardName, (ELeaderboardDataRequest)eLeaderboardDataRequest, (ELeaderboardDataTimeRange)eLeaderboardDataTimeRange, nRangeStart, nRangeEnd);
		}

		// Token: 0x060049BF RID: 18879 RVA: 0x0018AB00 File Offset: 0x00188F00
		public static int UploadLeaderboardScore(StatusCallback callback, string pchLeaderboardName, int nScore)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
            Viveport.Internal.StatusCallback statusCallback = new Viveport.Internal.StatusCallback(callback.Invoke);
			Api.InternalStatusCallbacks.Add(statusCallback);
			return Viveport.Internal.UserStats.UploadLeaderboardScore(statusCallback, pchLeaderboardName, nScore);
		}

		// Token: 0x060049C0 RID: 18880 RVA: 0x0018AB40 File Offset: 0x00188F40
		public static Leaderboard GetLeaderboardScore(int index)
		{
			LeaderboardEntry_t leaderboardEntry_t;
			leaderboardEntry_t.m_nGlobalRank = 0;
			leaderboardEntry_t.m_nScore = 0;
			leaderboardEntry_t.m_pUserName = string.Empty;
			Viveport.Internal.UserStats.GetLeaderboardScore(index, ref leaderboardEntry_t);
			return new Leaderboard
			{
				Rank = leaderboardEntry_t.m_nGlobalRank,
				Score = leaderboardEntry_t.m_nScore,
				UserName = leaderboardEntry_t.m_pUserName
			};
		}

		// Token: 0x060049C1 RID: 18881 RVA: 0x0018ABA0 File Offset: 0x00188FA0
		public static int GetLeaderboardScoreCount()
		{
			return UserStats.GetLeaderboardScoreCount();
		}

		// Token: 0x060049C2 RID: 18882 RVA: 0x0018ABA7 File Offset: 0x00188FA7
		public static UserStats.LeaderBoardSortMethod GetLeaderboardSortMethod()
		{
			return (UserStats.LeaderBoardSortMethod)UserStats.GetLeaderboardSortMethod();
		}

		// Token: 0x060049C3 RID: 18883 RVA: 0x0018ABAE File Offset: 0x00188FAE
		public static UserStats.LeaderBoardDiaplayType GetLeaderboardDisplayType()
		{
			return (UserStats.LeaderBoardDiaplayType)UserStats.GetLeaderboardDisplayType();
		}

		// Token: 0x02000983 RID: 2435
		public enum LeaderBoardRequestType
		{
			// Token: 0x040031F9 RID: 12793
			GlobalData,
			// Token: 0x040031FA RID: 12794
			GlobalDataAroundUser,
			// Token: 0x040031FB RID: 12795
			LocalData,
			// Token: 0x040031FC RID: 12796
			LocalDataAroundUser
		}

		// Token: 0x02000984 RID: 2436
		public enum LeaderBoardTimeRange
		{
			// Token: 0x040031FE RID: 12798
			AllTime,
			// Token: 0x040031FF RID: 12799
			Daily,
			// Token: 0x04003200 RID: 12800
			Weekly,
			// Token: 0x04003201 RID: 12801
			Monthly
		}

		// Token: 0x02000985 RID: 2437
		public enum LeaderBoardSortMethod
		{
			// Token: 0x04003203 RID: 12803
			None,
			// Token: 0x04003204 RID: 12804
			Ascending,
			// Token: 0x04003205 RID: 12805
			Descending
		}

		// Token: 0x02000986 RID: 2438
		public enum LeaderBoardDiaplayType
		{
			// Token: 0x04003207 RID: 12807
			None,
			// Token: 0x04003208 RID: 12808
			Numeric,
			// Token: 0x04003209 RID: 12809
			TimeSeconds,
			// Token: 0x0400320A RID: 12810
			TimeMilliSeconds
		}

		// Token: 0x02000987 RID: 2439
		public enum LeaderBoardScoreMethod
		{
			// Token: 0x0400320C RID: 12812
			None,
			// Token: 0x0400320D RID: 12813
			KeepBest,
			// Token: 0x0400320E RID: 12814
			ForceUpdate
		}
	}
}
