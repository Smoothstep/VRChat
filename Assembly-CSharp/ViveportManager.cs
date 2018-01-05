using System;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using UnityEngine;
using Viveport;
using VRC.Core;

// Token: 0x02000B7E RID: 2942
public class ViveportManager
{
	// Token: 0x17000D2D RID: 3373
	// (get) Token: 0x06005BAE RID: 23470 RVA: 0x00200371 File Offset: 0x001FE771
	public static bool IsInitialized
	{
		get
		{
			return ViveportManager._isInitialized && ViveportManager._isUserStatsReady;
		}
	}

	// Token: 0x17000D2E RID: 3374
	// (get) Token: 0x06005BAF RID: 23471 RVA: 0x00200385 File Offset: 0x001FE785
	// (set) Token: 0x06005BB0 RID: 23472 RVA: 0x0020038C File Offset: 0x001FE78C
	public static bool IsWaitingForInit { get; private set; }

	// Token: 0x06005BB1 RID: 23473 RVA: 0x00200394 File Offset: 0x001FE794
	public static void Init()
	{
		if (ViveportManager.IsWaitingForInit)
		{
			Debug.LogError("Viveport: Init already pending");
			return;
		}
		if (!ViveportManager.IsViveportAppInstalled())
		{
			Debug.LogError("Viveport: Viveport software not installed");
			ViveportManager.InitStatusHandler(-1);
			return;
		}
		Action init = delegate
		{
			ViveportManager.IsWaitingForInit = true;
			if (ViveportManager.f__mg0 == null)
			{
				ViveportManager.f__mg0 = new StatusCallback(ViveportManager.InitStatusHandler);
			}
			Api.Init(ViveportManager.f__mg0, ViveportManager.APP_ID);
		};
		if (ViveportManager._isInitialized)
		{
			ViveportManager.IsWaitingForInit = true;
			Api.Shutdown(delegate(int nResult)
			{
				ViveportManager.ShutdownHandler(nResult);
				init();
			});
		}
		else
		{
			init();
		}
	}

	// Token: 0x06005BB2 RID: 23474 RVA: 0x0020042C File Offset: 0x001FE82C
	public static void Shutdown()
	{
		if (ViveportManager._isInitialized)
		{
			if (ViveportManager.f__mg1 == null)
			{
				ViveportManager.f__mg1 = new StatusCallback(ViveportManager.ShutdownHandler);
			}
			Api.Shutdown(ViveportManager.f__mg1);
		}
	}

	// Token: 0x06005BB3 RID: 23475 RVA: 0x0020045B File Offset: 0x001FE85B
	public static bool IsUserSignedIn()
	{
		return ViveportManager.IsInitialized && ViveportManager.GetUserId() != ViveportManager.kUnknownUserId;
	}

	// Token: 0x06005BB4 RID: 23476 RVA: 0x00200479 File Offset: 0x001FE879
	public static bool DoesSignedInUserHaveLicense()
	{
		return ViveportManager.IsUserSignedIn() && ViveportManager._doesUserHaveLicense;
	}

	// Token: 0x06005BB5 RID: 23477 RVA: 0x0020048D File Offset: 0x001FE88D
	public static string GetUserId()
	{
		if (!ViveportManager.IsInitialized)
		{
			return ViveportManager.kUnknownUserId;
		}
		return User.GetUserId();
	}

	// Token: 0x06005BB6 RID: 23478 RVA: 0x002004A4 File Offset: 0x001FE8A4
	public static string GetUserName()
	{
		if (!ViveportManager.IsInitialized)
		{
			return "<unknown user>";
		}
		return User.GetUserName();
	}

	// Token: 0x06005BB7 RID: 23479 RVA: 0x002004BC File Offset: 0x001FE8BC
	public static bool IsViveportAppInstalled()
	{
		RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey("vive");
		if (registryKey == null)
		{
			return false;
		}
		object value = registryKey.GetValue(string.Empty);
		return value != null && string.Compare(value.ToString(), "URL:Vive Protocol") == 0;
	}

	// Token: 0x06005BB8 RID: 23480 RVA: 0x0020050C File Offset: 0x001FE90C
	private static void InitStatusHandler(int nResult)
	{
		if (nResult == 0)
		{
			ViveportManager._isInitialized = true;
			ViveportManager._isUserStatsReady = false;
			ViveportManager._doesUserHaveLicense = false;
			Debug.Log("Viveport: InitStatusHandler is successful");
			if (ViveportManager.f__mg2 == null)
			{
				ViveportManager.f__mg2 = new StatusCallback(ViveportManager.IsReadyHandler);
			}
			UserStats.IsReady(ViveportManager.f__mg2);
		}
		else
		{
			ViveportManager._isInitialized = false;
			ViveportManager._isUserStatsReady = false;
			ViveportManager._doesUserHaveLicense = false;
			ViveportManager.IsWaitingForInit = false;
			Debug.Log("Viveport: InitStatusHandler error : " + nResult);
		}
	}

	// Token: 0x06005BB9 RID: 23481 RVA: 0x00200590 File Offset: 0x001FE990
	private static void IsReadyHandler(int nResult)
	{
		if (nResult == 0)
		{
			ViveportManager._isUserStatsReady = true;
			ViveportManager._doesUserHaveLicense = false;
			Debug.Log("Viveport: IsReadyHandler is successful");
			ViveportManager.CheckLicense();
		}
		else
		{
			ViveportManager._isUserStatsReady = false;
			ViveportManager._doesUserHaveLicense = false;
			ViveportManager.IsWaitingForInit = false;
			Debug.Log("Viveport: IsReadyHandler error: " + nResult);
		}
	}

	// Token: 0x06005BBA RID: 23482 RVA: 0x002005EC File Offset: 0x001FE9EC
	private static void CheckLicense()
	{
		if (ViveportManager.f__mg3 == null)
		{
			ViveportManager.f__mg3 = new ViveportManager.LicenseChecker.OnSuccessDelegate(ViveportManager.OnLicenseCheckSuccess);
		}
		ViveportManager.LicenseChecker.OnSuccessDelegate onSuccess = ViveportManager.f__mg3;
		if (ViveportManager.f__mg4 == null)
		{
			ViveportManager.f__mg4 = new ViveportManager.LicenseChecker.OnFailureDelegate(ViveportManager.OnLicenseCheckFailure);
		}
		Api.GetLicense(new ViveportManager.LicenseChecker(onSuccess, ViveportManager.f__mg4), ViveportManager.APP_ID, ViveportManager.APP_KEY);
	}

	// Token: 0x06005BBB RID: 23483 RVA: 0x00200647 File Offset: 0x001FEA47
	private static void OnLicenseCheckSuccess(long issueTime, long expirationTime, int latestVersion, bool updateRequired)
	{
		Debug.Log("Viveport: CheckLicense is successful");
		ViveportManager._doesUserHaveLicense = true;
		ViveportManager.IsWaitingForInit = false;
	}

	// Token: 0x06005BBC RID: 23484 RVA: 0x00200660 File Offset: 0x001FEA60
	private static void OnLicenseCheckFailure(int errorCode, string errorMessage)
	{
		Debug.LogError(string.Concat(new object[]
		{
			"Viveport: License check failed: [",
			errorCode,
			"] ",
			errorMessage
		}));
		if (VRCApplicationSetup.Instance.ServerEnvironment == ApiServerEnvironment.Dev)
		{
			Debug.LogError("Viveport:  (enabling license anyway for dev build)");
			ViveportManager._doesUserHaveLicense = true;
			ViveportManager.IsWaitingForInit = false;
		}
		else
		{
			ViveportManager._doesUserHaveLicense = false;
			ViveportManager.IsWaitingForInit = false;
		}
	}

	// Token: 0x06005BBD RID: 23485 RVA: 0x002006D0 File Offset: 0x001FEAD0
	private static void ShutdownHandler(int nResult)
	{
		if (nResult == 0)
		{
			ViveportManager._isInitialized = false;
			ViveportManager._isUserStatsReady = false;
			ViveportManager._doesUserHaveLicense = false;
			ViveportManager.IsWaitingForInit = false;
			Debug.Log("Viveport: ShutdownHandler is successful");
		}
		else
		{
			Debug.Log("Viveport: ShutdownHandler error: " + nResult + ", shutting down anyway");
			ViveportManager._isInitialized = false;
			ViveportManager._isUserStatsReady = false;
			ViveportManager._doesUserHaveLicense = false;
			ViveportManager.IsWaitingForInit = false;
		}
	}

	// Token: 0x04004159 RID: 16729
	private static string APP_ID = "469fbcbb-bfde-40b5-a7d4-381249d387cd";

	// Token: 0x0400415A RID: 16730
	private static string APP_KEY = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCFEqngsUauMSig+loxAmP0LuL2iYBhe5ao6MajJXLO1ed7YP1MCgIphivmMWiNBxfHqjw8ktStydmp/m/p//74hC0m3LlGfk1yQAABBqyqItS2snn9aqRiuXV8ne/QAng3hpgZ7H4p6XIUVWZ1mKamDiEwrPjtbf3T/2YE+HAvJQIDAQAB";

	// Token: 0x0400415B RID: 16731
	private static string kUnknownUserId = "UnknownU-serI-dUnk-nown-UserIdUnknow";

	// Token: 0x0400415C RID: 16732
	private static bool _isInitialized;

	// Token: 0x0400415D RID: 16733
	private static bool _isUserStatsReady;

	// Token: 0x0400415E RID: 16734
	private static bool _doesUserHaveLicense;

	// Token: 0x04004160 RID: 16736
	[CompilerGenerated]
	private static StatusCallback f__mg0;

	// Token: 0x04004162 RID: 16738
	[CompilerGenerated]
	private static StatusCallback f__mg1;

	// Token: 0x04004163 RID: 16739
	[CompilerGenerated]
	private static StatusCallback f__mg2;

	// Token: 0x04004164 RID: 16740
	[CompilerGenerated]
	private static ViveportManager.LicenseChecker.OnSuccessDelegate f__mg3;

	// Token: 0x04004165 RID: 16741
	[CompilerGenerated]
	private static ViveportManager.LicenseChecker.OnFailureDelegate f__mg4;

	// Token: 0x02000B7F RID: 2943
	private class LicenseChecker : Api.LicenseChecker
	{
		// Token: 0x06005BC0 RID: 23488 RVA: 0x0020078C File Offset: 0x001FEB8C
		public LicenseChecker(ViveportManager.LicenseChecker.OnSuccessDelegate onSuccess, ViveportManager.LicenseChecker.OnFailureDelegate onFailure)
		{
			this._onSuccess = onSuccess;
			this._onFailure = onFailure;
		}

		// Token: 0x06005BC1 RID: 23489 RVA: 0x002007A2 File Offset: 0x001FEBA2
		public override void OnSuccess(long issueTime, long expirationTime, int latestVersion, bool updateRequired)
		{
			if (this._onSuccess != null)
			{
				this._onSuccess(issueTime, expirationTime, latestVersion, updateRequired);
			}
		}

		// Token: 0x06005BC2 RID: 23490 RVA: 0x002007BF File Offset: 0x001FEBBF
		public override void OnFailure(int errorCode, string errorMessage)
		{
			if (this._onFailure != null)
			{
				this._onFailure(errorCode, errorMessage);
			}
		}

		// Token: 0x04004166 RID: 16742
		private ViveportManager.LicenseChecker.OnSuccessDelegate _onSuccess;

		// Token: 0x04004167 RID: 16743
		private ViveportManager.LicenseChecker.OnFailureDelegate _onFailure;

		// Token: 0x02000B80 RID: 2944
		// (Invoke) Token: 0x06005BC4 RID: 23492
		public delegate void OnSuccessDelegate(long issueTime, long expirationTime, int latestVersion, bool updateRequired);

		// Token: 0x02000B81 RID: 2945
		// (Invoke) Token: 0x06005BC8 RID: 23496
		public delegate void OnFailureDelegate(int errorCode, string errorMessage);
	}
}
