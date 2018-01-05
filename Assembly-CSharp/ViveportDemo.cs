using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Viveport;
using Viveport.Core;

// Token: 0x02000973 RID: 2419
public class ViveportDemo : MonoBehaviour
{
	// Token: 0x0600495A RID: 18778 RVA: 0x00187FBD File Offset: 0x001863BD
	private void Start()
	{
		if (ViveportDemo.f__mg0 == null)
		{
			ViveportDemo.f__mg0 = new StatusCallback(ViveportDemo.InitStatusHandler);
		}
		Api.Init(ViveportDemo.f__mg0, ViveportDemo.APP_ID);
	}

	// Token: 0x0600495B RID: 18779 RVA: 0x00187FE7 File Offset: 0x001863E7
	private void Update()
	{
	}

	// Token: 0x0600495C RID: 18780 RVA: 0x00187FEC File Offset: 0x001863EC
	private void OnGUI()
	{
		if (!ViveportDemo.bInit)
		{
			GUI.contentColor = Color.white;
		}
		else
		{
			GUI.contentColor = Color.grey;
		}
		if (GUI.Button(new Rect((float)this.nXStart, (float)this.nYStart, (float)this.nWidth, (float)this.nHeight), "Init") && !ViveportDemo.bInit)
		{
			if (ViveportDemo.f__mg1 == null)
			{
				ViveportDemo.f__mg1 = new StatusCallback(ViveportDemo.InitStatusHandler);
			}
			Api.Init(ViveportDemo.f__mg1, ViveportDemo.APP_ID);
		}
		if (ViveportDemo.bInit)
		{
			GUI.contentColor = Color.white;
		}
		else
		{
			GUI.contentColor = Color.grey;
		}
		if (GUI.Button(new Rect((float)(this.nXStart + (this.nWidth + 10)), (float)this.nYStart, (float)this.nWidth, (float)this.nHeight), "Shutdown") && ViveportDemo.bInit)
		{
			if (ViveportDemo.f__mg2 == null)
			{
				ViveportDemo.f__mg2 = new StatusCallback(ViveportDemo.ShutdownHandler);
			}
			Api.Shutdown(ViveportDemo.f__mg2);
		}
		if (GUI.Button(new Rect((float)(this.nXStart + 2 * (this.nWidth + 10)), (float)this.nYStart, (float)this.nWidth, (float)this.nHeight), "Version") && ViveportDemo.bInit)
		{
			Viveport.Core.Logger.Log("Version: " + Api.Version());
		}
		if (GUI.Button(new Rect((float)(this.nXStart + 3 * (this.nWidth + 10)), (float)this.nYStart, (float)this.nWidth, (float)this.nHeight), "QueryRunMode") && ViveportDemo.bInit)
		{
			if (ViveportDemo.f__mg3 == null)
			{
				ViveportDemo.f__mg3 = new QueryRuntimeModeCallback(ViveportDemo.QueryRunTimeHandler);
			}
			Api.QueryRuntimeMode(ViveportDemo.f__mg3);
		}
		if (GUI.Button(new Rect((float)(this.nXStart + 4 * (this.nWidth + 10)), (float)this.nYStart, (float)this.nWidth, (float)this.nHeight), "StatIsReady") && ViveportDemo.bInit)
		{
			if (ViveportDemo.f__mg4 == null)
			{
				ViveportDemo.f__mg4 = new StatusCallback(ViveportDemo.IsReadyHandler);
			}
			UserStats.IsReady(ViveportDemo.f__mg4);
		}
		if (GUI.Button(new Rect((float)(this.nXStart + 6 * (this.nWidth + 10)), (float)this.nYStart, (float)this.nWidth, (float)this.nHeight), "DRM"))
		{
			if (ViveportDemo.bInit)
			{
				Api.GetLicense(new ViveportDemo.MyLicenseChecker(), ViveportDemo.APP_ID, ViveportDemo.APP_KEY);
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		if (GUI.Button(new Rect((float)(this.nXStart + 5 * (this.nWidth + 10)), (float)this.nYStart, (float)this.nWidth, (float)this.nHeight), "ArcadeIsReady") && ViveportDemo.bInit)
		{
			if (ViveportDemo.f__mg5 == null)
			{
				ViveportDemo.f__mg5 = new StatusCallback(ViveportDemo.IsArcadeLeaderboardReadyHandler);
			}
			ArcadeLeaderboard.IsReady(ViveportDemo.f__mg5);
		}
		if (ViveportDemo.bInit && ViveportDemo.bIsReady)
		{
			GUI.contentColor = Color.white;
		}
		else
		{
			GUI.contentColor = Color.grey;
		}
		if (GUI.Button(new Rect((float)(this.nXStart + 7 * (this.nWidth + 10)), (float)this.nYStart, (float)this.nWidth, (float)this.nHeight), "UserProfile"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bIsReady)
			{
				Viveport.Core.Logger.Log("UserId: " + User.GetUserId());
				Viveport.Core.Logger.Log("userName: " + User.GetUserName());
				Viveport.Core.Logger.Log("userAvatarUrl: " + User.GetUserAvatarUrl());
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		this.stringToEdit = GUI.TextField(new Rect(10f, (float)(this.nWidth + 10), 120f, 20f), this.stringToEdit, 50);
		this.StatsCount = GUI.TextField(new Rect(130f, (float)(this.nWidth + 10), 220f, 20f), this.StatsCount, 50);
		if (GUI.Button(new Rect((float)this.nXStart, (float)(this.nYStart + this.nWidth + 10), (float)this.nWidth, (float)this.nHeight), "DownloadStat"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bIsReady)
			{
				if (ViveportDemo.f__mg6 == null)
				{
					ViveportDemo.f__mg6 = new StatusCallback(ViveportDemo.DownloadStatsHandler);
				}
				UserStats.DownloadStats(ViveportDemo.f__mg6);
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		if (GUI.Button(new Rect((float)(this.nXStart + (this.nWidth + 10)), (float)(this.nYStart + this.nWidth + 10), (float)this.nWidth, (float)this.nHeight), "UploadStat"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bIsReady)
			{
				if (ViveportDemo.f__mg7 == null)
				{
					ViveportDemo.f__mg7 = new StatusCallback(ViveportDemo.UploadStatsHandler);
				}
				UserStats.UploadStats(ViveportDemo.f__mg7);
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		if (GUI.Button(new Rect((float)(this.nXStart + 2 * (this.nWidth + 10)), (float)(this.nYStart + this.nWidth + 10), (float)this.nWidth, (float)this.nHeight), "GetStat"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bIsReady)
			{
				this.nResult = UserStats.GetStat(this.stringToEdit, this.nInitValue);
				Viveport.Core.Logger.Log(string.Concat(new object[]
				{
					"Get ",
					this.stringToEdit,
					" stat name as => ",
					this.nResult
				}));
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		if (GUI.Button(new Rect((float)(this.nXStart + 3 * (this.nWidth + 10)), (float)(this.nYStart + this.nWidth + 10), (float)this.nWidth, (float)this.nHeight), "SetStat"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bIsReady)
			{
				Viveport.Core.Logger.Log("MaxStep is => " + int.Parse(this.StatsCount));
				this.nResult = int.Parse(this.StatsCount);
				UserStats.SetStat(this.stringToEdit, this.nResult);
				Viveport.Core.Logger.Log(string.Concat(new object[]
				{
					"Set",
					this.stringToEdit,
					" stat name as =>",
					this.nResult
				}));
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		this.achivToEdit = GUI.TextField(new Rect(10f, (float)(2 * this.nWidth + 15), 120f, 20f), this.achivToEdit, 50);
		if (GUI.Button(new Rect((float)this.nXStart, (float)(this.nYStart + 2 * this.nWidth + 10), (float)this.nWidth, (float)this.nHeight), "GetAchieve"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bIsReady)
			{
				bool achievement = UserStats.GetAchievement(this.achivToEdit);
				Viveport.Core.Logger.Log(string.Concat(new object[]
				{
					"Get achievement => ",
					this.achivToEdit,
					" , and value is => ",
					achievement
				}));
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		if (GUI.Button(new Rect((float)(this.nXStart + this.nWidth + 10), (float)(this.nYStart + 2 * this.nWidth + 10), (float)this.nWidth, (float)this.nHeight), "SetAchieve"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bIsReady)
			{
				UserStats.SetAchievement(this.achivToEdit);
				Viveport.Core.Logger.Log("Set achievement => " + this.achivToEdit);
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		if (GUI.Button(new Rect((float)(this.nXStart + 2 * (this.nWidth + 10)), (float)(this.nYStart + 2 * this.nWidth + 10), (float)this.nWidth, (float)this.nHeight), "ClearAchieve"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bIsReady)
			{
				UserStats.ClearAchievement(this.achivToEdit);
				Viveport.Core.Logger.Log("Clear achievement => " + this.achivToEdit);
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		if (GUI.Button(new Rect((float)(this.nXStart + 3 * (this.nWidth + 10)), (float)(this.nYStart + 2 * this.nWidth + 10), (float)this.nWidth, (float)this.nHeight), "Achieve&Time"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bIsReady)
			{
				int achievementUnlockTime = UserStats.GetAchievementUnlockTime(this.achivToEdit);
				Viveport.Core.Logger.Log("The achievement's unlock time is =>" + achievementUnlockTime);
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		this.leaderboardToEdit = GUI.TextField(new Rect(10f, (float)(3 * this.nWidth + 20), 160f, 20f), this.leaderboardToEdit, 150);
		if (GUI.Button(new Rect((float)this.nXStart, (float)(this.nYStart + 3 * this.nWidth + 20), (float)this.nWidth, (float)this.nHeight), "DL Around"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bIsReady)
			{
				if (ViveportDemo.f__mg8 == null)
				{
					ViveportDemo.f__mg8 = new StatusCallback(ViveportDemo.DownloadLeaderboardHandler);
				}
				UserStats.DownloadLeaderboardScores(ViveportDemo.f__mg8, this.leaderboardToEdit, UserStats.LeaderBoardRequestType.GlobalDataAroundUser, UserStats.LeaderBoardTimeRange.AllTime, -5, 5);
				Viveport.Core.Logger.Log("DownloadLeaderboardScores");
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		if (GUI.Button(new Rect((float)(this.nXStart + (this.nWidth + 10)), (float)(this.nYStart + 3 * this.nWidth + 20), (float)this.nWidth, (float)this.nHeight), "DL not Around"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bIsReady)
			{
				if (ViveportDemo.f__mg9 == null)
				{
					ViveportDemo.f__mg9 = new StatusCallback(ViveportDemo.DownloadLeaderboardHandler);
				}
				UserStats.DownloadLeaderboardScores(ViveportDemo.f__mg9, this.leaderboardToEdit, UserStats.LeaderBoardRequestType.GlobalData, UserStats.LeaderBoardTimeRange.AllTime, 0, 10);
				Viveport.Core.Logger.Log("DownloadLeaderboardScores");
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		this.leaderboardScore = GUI.TextField(new Rect(170f, (float)(3 * this.nWidth + 20), 160f, 20f), this.leaderboardScore, 50);
		if (GUI.Button(new Rect((float)(this.nXStart + 2 * (this.nWidth + 10)), (float)(this.nYStart + 3 * this.nWidth + 20), (float)this.nWidth, (float)this.nHeight), "Upload LB"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bIsReady)
			{
				if (ViveportDemo.f__mgA == null)
				{
					ViveportDemo.f__mgA = new StatusCallback(ViveportDemo.UploadLeaderboardScoreHandler);
				}
				UserStats.UploadLeaderboardScore(ViveportDemo.f__mgA, this.leaderboardToEdit, int.Parse(this.leaderboardScore));
				Viveport.Core.Logger.Log("UploadLeaderboardScore");
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		if (GUI.Button(new Rect((float)(this.nXStart + 3 * (this.nWidth + 10)), (float)(this.nYStart + 3 * this.nWidth + 20), (float)this.nWidth, (float)this.nHeight), "Get LB count"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bIsReady)
			{
				this.nResult = UserStats.GetLeaderboardScoreCount();
				Viveport.Core.Logger.Log("GetLeaderboardScoreCount=> " + this.nResult);
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		if (GUI.Button(new Rect((float)(this.nXStart + 4 * (this.nWidth + 10)), (float)(this.nYStart + 3 * this.nWidth + 20), (float)this.nWidth, (float)this.nHeight), "Get LB Score"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bIsReady)
			{
				int leaderboardScoreCount = UserStats.GetLeaderboardScoreCount();
				Viveport.Core.Logger.Log("GetLeaderboardScoreCount => " + leaderboardScoreCount);
				for (int i = 0; i < leaderboardScoreCount; i++)
				{
					Leaderboard leaderboard = UserStats.GetLeaderboardScore(i);
					Viveport.Core.Logger.Log(string.Concat(new object[]
					{
						"UserName = ",
						leaderboard.UserName,
						", Score = ",
						leaderboard.Score,
						", Rank = ",
						leaderboard.Rank
					}));
				}
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		if (GUI.Button(new Rect((float)(this.nXStart + 5 * (this.nWidth + 10)), (float)(this.nYStart + 3 * this.nWidth + 20), (float)this.nWidth, (float)this.nHeight), "Get Sort Method"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bIsReady)
			{
				int leaderboardSortMethod = (int)UserStats.GetLeaderboardSortMethod();
				Viveport.Core.Logger.Log("GetLeaderboardSortMethod=> " + leaderboardSortMethod);
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		if (GUI.Button(new Rect((float)(this.nXStart + 6 * (this.nWidth + 10)), (float)(this.nYStart + 3 * this.nWidth + 20), (float)this.nWidth, (float)this.nHeight), "Get Disp Type"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bIsReady)
			{
				int leaderboardDisplayType = (int)UserStats.GetLeaderboardDisplayType();
				Viveport.Core.Logger.Log("GetLeaderboardDisplayType=> " + leaderboardDisplayType);
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		if (ViveportDemo.bInit && ViveportDemo.bArcadeIsReady)
		{
			GUI.contentColor = Color.white;
		}
		else
		{
			GUI.contentColor = Color.grey;
		}
		this.leaderboardToEdit = GUI.TextField(new Rect(10f, (float)(4 * this.nWidth + 20), 160f, 20f), this.leaderboardToEdit, 150);
		if (GUI.Button(new Rect((float)this.nXStart, (float)(this.nYStart + 4 * this.nWidth + 20), (float)this.nWidth, (float)this.nHeight), "DL Arca LB"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bArcadeIsReady)
			{
				if (ViveportDemo.f__mgB == null)
				{
					ViveportDemo.f__mgB = new StatusCallback(ViveportDemo.DownloadLeaderboardHandler);
				}
				ArcadeLeaderboard.DownloadLeaderboardScores(ViveportDemo.f__mgB, this.leaderboardToEdit, ArcadeLeaderboard.LeaderboardTimeRange.AllTime, 10);
				Viveport.Core.Logger.Log("DownloadLeaderboardScores");
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		this.leaderboardUserName = GUI.TextField(new Rect(170f, (float)(4 * this.nWidth + 20), 160f, 20f), this.leaderboardUserName, 150);
		this.leaderboardScore = GUI.TextField(new Rect(330f, (float)(4 * this.nWidth + 20), 160f, 20f), this.leaderboardScore, 50);
		if (GUI.Button(new Rect((float)(this.nXStart + (this.nWidth + 10)), (float)(this.nYStart + 4 * this.nWidth + 20), (float)this.nWidth, (float)this.nHeight), "UL Arca LB"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bArcadeIsReady)
			{
				if (ViveportDemo.f__mgC == null)
				{
					ViveportDemo.f__mgC = new StatusCallback(ViveportDemo.UploadLeaderboardScoreHandler);
				}
				ArcadeLeaderboard.UploadLeaderboardScore(ViveportDemo.f__mgC, this.leaderboardToEdit, this.leaderboardUserName, int.Parse(this.leaderboardScore));
				Viveport.Core.Logger.Log("UploadLeaderboardScore");
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		if (GUI.Button(new Rect((float)(this.nXStart + 2 * (this.nWidth + 10)), (float)(this.nYStart + 4 * this.nWidth + 20), (float)this.nWidth, (float)this.nHeight), "Get Arca Count"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bArcadeIsReady)
			{
				this.nResult = ArcadeLeaderboard.GetLeaderboardScoreCount();
				Viveport.Core.Logger.Log("GetLeaderboardScoreCount=> " + this.nResult);
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & Arcade isReady are successful.");
			}
		}
		if (GUI.Button(new Rect((float)(this.nXStart + 3 * (this.nWidth + 10)), (float)(this.nYStart + 4 * this.nWidth + 20), (float)this.nWidth, (float)this.nHeight), "Get Arca Score"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bArcadeIsReady)
			{
				int leaderboardScoreCount2 = ArcadeLeaderboard.GetLeaderboardScoreCount();
				Viveport.Core.Logger.Log("GetLeaderboardScoreCount => " + leaderboardScoreCount2);
				for (int j = 0; j < leaderboardScoreCount2; j++)
				{
					Leaderboard leaderboard2 = ArcadeLeaderboard.GetLeaderboardScore(j);
					Viveport.Core.Logger.Log(string.Concat(new object[]
					{
						"UserName = ",
						leaderboard2.UserName,
						", Score = ",
						leaderboard2.Score,
						", Rank = ",
						leaderboard2.Rank
					}));
				}
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		if (GUI.Button(new Rect((float)(this.nXStart + 4 * (this.nWidth + 10)), (float)(this.nYStart + 4 * this.nWidth + 20), (float)this.nWidth, (float)this.nHeight), "Get AC UScore"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bArcadeIsReady)
			{
				int leaderboardUserScore = ArcadeLeaderboard.GetLeaderboardUserScore();
				Viveport.Core.Logger.Log("GetLeaderboardUserScore=> " + leaderboardUserScore);
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
		if (GUI.Button(new Rect((float)(this.nXStart + 5 * (this.nWidth + 10)), (float)(this.nYStart + 4 * this.nWidth + 20), (float)this.nWidth, (float)this.nHeight), "Get AC URank"))
		{
			if (ViveportDemo.bInit && ViveportDemo.bArcadeIsReady)
			{
				int leaderboardUserRank = ArcadeLeaderboard.GetLeaderboardUserRank();
				Viveport.Core.Logger.Log("GetLeaderboardUserRank=> " + leaderboardUserRank);
			}
			else
			{
				Viveport.Core.Logger.Log("Please make sure init & isReady are successful.");
			}
		}
	}

	// Token: 0x0600495D RID: 18781 RVA: 0x001892C4 File Offset: 0x001876C4
	private static void InitStatusHandler(int nResult)
	{
		if (nResult == 0)
		{
			ViveportDemo.bInit = true;
			ViveportDemo.bIsReady = false;
			ViveportDemo.bArcadeIsReady = false;
			Viveport.Core.Logger.Log("InitStatusHandler is successful");
		}
		else
		{
			ViveportDemo.bInit = false;
			Viveport.Core.Logger.Log("InitStatusHandler error : " + nResult);
		}
	}

	// Token: 0x0600495E RID: 18782 RVA: 0x00189313 File Offset: 0x00187713
	private static void IsReadyHandler(int nResult)
	{
		if (nResult == 0)
		{
			ViveportDemo.bIsReady = true;
			ViveportDemo.bArcadeIsReady = false;
			Viveport.Core.Logger.Log("IsReadyHandler is successful");
		}
		else
		{
			ViveportDemo.bIsReady = false;
			Viveport.Core.Logger.Log("IsReadyHandler error: " + nResult);
		}
	}

	// Token: 0x0600495F RID: 18783 RVA: 0x00189354 File Offset: 0x00187754
	private static void QueryRunTimeHandler(int nResult, int nMode)
	{
		if (nResult == 0)
		{
			Viveport.Core.Logger.Log(string.Concat(new object[]
			{
				"QueryRunTimeHandler is successful",
				nResult,
				"Running mode is ",
				nMode
			}));
		}
		else
		{
			Viveport.Core.Logger.Log("QueryRunTimeHandler error: " + nResult);
		}
	}

	// Token: 0x06004960 RID: 18784 RVA: 0x001893B3 File Offset: 0x001877B3
	private static void IsArcadeLeaderboardReadyHandler(int nResult)
	{
		if (nResult == 0)
		{
			ViveportDemo.bArcadeIsReady = true;
			ViveportDemo.bIsReady = false;
			Viveport.Core.Logger.Log("IsArcadeLeaderboardReadyHandler is successful");
		}
		else
		{
			ViveportDemo.bArcadeIsReady = false;
			Viveport.Core.Logger.Log("IsArcadeLeaderboardReadyHandler error: " + nResult);
		}
	}

	// Token: 0x06004961 RID: 18785 RVA: 0x001893F1 File Offset: 0x001877F1
	private static void ShutdownHandler(int nResult)
	{
		if (nResult == 0)
		{
			ViveportDemo.bInit = false;
			ViveportDemo.bIsReady = false;
			Viveport.Core.Logger.Log("ShutdownHandler is successful");
		}
		else
		{
			Viveport.Core.Logger.Log("ShutdownHandler error: " + nResult);
		}
	}

	// Token: 0x06004962 RID: 18786 RVA: 0x00189429 File Offset: 0x00187829
	private static void DownloadStatsHandler(int nResult)
	{
		if (nResult == 0)
		{
			Viveport.Core.Logger.Log("DownloadStatsHandler is successful ");
		}
		else
		{
			Viveport.Core.Logger.Log("DownloadStatsHandler error: " + nResult);
		}
	}

	// Token: 0x06004963 RID: 18787 RVA: 0x00189455 File Offset: 0x00187855
	private static void UploadStatsHandler(int nResult)
	{
		if (nResult == 0)
		{
			Viveport.Core.Logger.Log("UploadStatsHandler is successful");
		}
		else
		{
			Viveport.Core.Logger.Log("UploadStatsHandler error: " + nResult);
		}
	}

	// Token: 0x06004964 RID: 18788 RVA: 0x00189481 File Offset: 0x00187881
	private static void DownloadLeaderboardHandler(int nResult)
	{
		if (nResult == 0)
		{
			Viveport.Core.Logger.Log("DownloadLeaderboardHandler is successful");
		}
		else
		{
			Viveport.Core.Logger.Log("DownloadLeaderboardHandler error: " + nResult);
		}
	}

	// Token: 0x06004965 RID: 18789 RVA: 0x001894AD File Offset: 0x001878AD
	private static void UploadLeaderboardScoreHandler(int nResult)
	{
		if (nResult == 0)
		{
			Viveport.Core.Logger.Log("UploadLeaderboardScoreHandler is successful.");
		}
		else
		{
			Viveport.Core.Logger.Log("UploadLeaderboardScoreHandler error : " + nResult);
		}
	}

	// Token: 0x040031B3 RID: 12723
	private int nInitValue;

	// Token: 0x040031B4 RID: 12724
	private int nResult;

	// Token: 0x040031B5 RID: 12725
	private int nWidth = 110;

	// Token: 0x040031B6 RID: 12726
	private int nHeight = 40;

	// Token: 0x040031B7 RID: 12727
	private int nXStart = 10;

	// Token: 0x040031B8 RID: 12728
	private int nYStart = 35;

	// Token: 0x040031B9 RID: 12729
	private string stringToEdit = "Input Stats name";

	// Token: 0x040031BA RID: 12730
	private string StatsCount = "Input max index";

	// Token: 0x040031BB RID: 12731
	private string achivToEdit = "Input achieve name";

	// Token: 0x040031BC RID: 12732
	private string leaderboardToEdit = "Input leaderboard name";

	// Token: 0x040031BD RID: 12733
	private string leaderboardUserName = "Input user name";

	// Token: 0x040031BE RID: 12734
	private string leaderboardScore = "Input score";

	// Token: 0x040031BF RID: 12735
	private static bool bInit = true;

	// Token: 0x040031C0 RID: 12736
	private static bool bIsReady;

	// Token: 0x040031C1 RID: 12737
	private static bool bArcadeIsReady;

	// Token: 0x040031C2 RID: 12738
	private static string APP_ID = "469fbcbb-bfde-40b5-a7d4-381249d387cd";

	// Token: 0x040031C3 RID: 12739
	private static string APP_KEY = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCFEqngsUauMSig+loxAmP0LuL2iYBhe5ao6MajJXLO1ed7YP1MCgIphivmMWiNBxfHqjw8ktStydmp/m/p//74hC0m3LlGfk1yQAABBqyqItS2snn9aqRiuXV8ne/QAng3hpgZ7H4p6XIUVWZ1mKamDiEwrPjtbf3T/2YE+HAvJQIDAQAB";

	// Token: 0x040031C4 RID: 12740
	[CompilerGenerated]
	private static StatusCallback f__mg0;

	// Token: 0x040031C5 RID: 12741
	[CompilerGenerated]
	private static StatusCallback f__mg1;

	// Token: 0x040031C6 RID: 12742
	[CompilerGenerated]
	private static StatusCallback f__mg2;

	// Token: 0x040031C7 RID: 12743
	[CompilerGenerated]
	private static QueryRuntimeModeCallback f__mg3;

	// Token: 0x040031C8 RID: 12744
	[CompilerGenerated]
	private static StatusCallback f__mg4;

	// Token: 0x040031C9 RID: 12745
	[CompilerGenerated]
	private static StatusCallback f__mg5;

	// Token: 0x040031CA RID: 12746
	[CompilerGenerated]
	private static StatusCallback f__mg6;

	// Token: 0x040031CB RID: 12747
	[CompilerGenerated]
	private static StatusCallback f__mg7;

	// Token: 0x040031CC RID: 12748
	[CompilerGenerated]
	private static StatusCallback f__mg8;

	// Token: 0x040031CD RID: 12749
	[CompilerGenerated]
	private static StatusCallback f__mg9;

	// Token: 0x040031CE RID: 12750
	[CompilerGenerated]
	private static StatusCallback f__mgA;

	// Token: 0x040031CF RID: 12751
	[CompilerGenerated]
	private static StatusCallback f__mgB;

	// Token: 0x040031D0 RID: 12752
	[CompilerGenerated]
	private static StatusCallback f__mgC;

	// Token: 0x02000974 RID: 2420
	private class MyLicenseChecker : Api.LicenseChecker
	{
		// Token: 0x06004968 RID: 18792 RVA: 0x00189508 File Offset: 0x00187908
		public override void OnSuccess(long issueTime, long expirationTime, int latestVersion, bool updateRequired)
		{
			Viveport.Core.Logger.Log("[MyLicenseChecker] issueTime: " + issueTime);
			Viveport.Core.Logger.Log("[MyLicenseChecker] expirationTime: " + expirationTime);
			Viveport.Core.Logger.Log("[MyLicenseChecker] latestVersion: " + latestVersion);
			Viveport.Core.Logger.Log("[MyLicenseChecker] updateRequired: " + updateRequired);
		}

		// Token: 0x06004969 RID: 18793 RVA: 0x0018956A File Offset: 0x0018796A
		public override void OnFailure(int errorCode, string errorMessage)
		{
			Viveport.Core.Logger.Log("[MyLicenseChecker] errorCode: " + errorCode);
			Viveport.Core.Logger.Log("[MyLicenseChecker] errorMessage: " + errorMessage);
		}
	}
}
