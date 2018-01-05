// Decompiled with JetBrains decompiler
// Type: Viveport.ArcadeLeaderboard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D96FA5-73D5-49AE-8538-9A130950C0D8
// Assembly location: C:\Program Files (x86)\Steam\SteamApps\common\VRChat\VRChat_Data\Managed\Assembly-CSharp.dll

using System;
using Viveport.Internal;

namespace Viveport
{
    public class ArcadeLeaderboard
    {
        public static void IsReady(StatusCallback callback)
        {
            if (callback == null)
                throw new InvalidOperationException("callback == null");
            Viveport.Internal.StatusCallback IsReadyCallback = new Viveport.Internal.StatusCallback(callback.Invoke);
            Api.InternalStatusCallbacks.Add(IsReadyCallback);
            Viveport.Internal.ArcadeLeaderboard.IsReady(IsReadyCallback);
        }

        public static void DownloadLeaderboardScores(StatusCallback callback, string pchLeaderboardName, ArcadeLeaderboard.LeaderboardTimeRange eLeaderboardDataTimeRange, int nCount)
        {
            if (callback == null)
                throw new InvalidOperationException("callback == null");
            Viveport.Internal.StatusCallback downloadLeaderboardScoresCB = new Viveport.Internal.StatusCallback(callback.Invoke);
            Api.InternalStatusCallbacks.Add(downloadLeaderboardScoresCB);
            eLeaderboardDataTimeRange = ArcadeLeaderboard.LeaderboardTimeRange.AllTime;
            Viveport.Internal.ArcadeLeaderboard.DownloadLeaderboardScores(downloadLeaderboardScoresCB, pchLeaderboardName, (ELeaderboardDataTimeRange)eLeaderboardDataTimeRange, nCount);
        }

        public static void UploadLeaderboardScore(StatusCallback callback, string pchLeaderboardName, string pchUserName, int nScore)
        {
            if (callback == null)
                throw new InvalidOperationException("callback == null");
            Viveport.Internal.StatusCallback uploadLeaderboardScoreCB = new Viveport.Internal.StatusCallback(callback.Invoke);
            Api.InternalStatusCallbacks.Add(uploadLeaderboardScoreCB);
            Viveport.Internal.ArcadeLeaderboard.UploadLeaderboardScore(uploadLeaderboardScoreCB, pchLeaderboardName, pchUserName, nScore);
        }

        public static Leaderboard GetLeaderboardScore(int index)
        {
            LeaderboardEntry_t pLeaderboardEntry;
            pLeaderboardEntry.m_nGlobalRank = 0;
            pLeaderboardEntry.m_nScore = 0;
            pLeaderboardEntry.m_pUserName = string.Empty;
            Viveport.Internal.ArcadeLeaderboard.GetLeaderboardScore(index, ref pLeaderboardEntry);
            return new Leaderboard() { Rank = pLeaderboardEntry.m_nGlobalRank, Score = pLeaderboardEntry.m_nScore, UserName = pLeaderboardEntry.m_pUserName };
        }

        public static int GetLeaderboardScoreCount()
        {
            return Viveport.Internal.ArcadeLeaderboard.GetLeaderboardScoreCount();
        }

        public static int GetLeaderboardUserRank()
        {
            return Viveport.Internal.ArcadeLeaderboard.GetLeaderboardUserRank();
        }

        public static int GetLeaderboardUserScore()
        {
            return Viveport.Internal.ArcadeLeaderboard.GetLeaderboardUserScore();
        }

        public enum LeaderboardTimeRange
        {
            AllTime,
        }
    }
}
