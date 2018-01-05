using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using LitJson;
using PublicKeyConvert;
using Viveport.Core;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x0200097F RID: 2431
	public class Api
	{
		// Token: 0x060049A3 RID: 18851 RVA: 0x0018A3B0 File Offset: 0x001887B0
		public static void GetLicense(Api.LicenseChecker checker, string appId, string appKey)
		{
			if (checker == null || string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(appKey))
			{
				throw new InvalidOperationException("checker == null || string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(appKey)");
			}
			Api._appId = appId;
			Api._appKey = appKey;
			Api.InternalLicenseCheckers.Add(checker);
			if (Api.f__mg0 == null)
			{
				Api.f__mg0 = new GetLicenseCallback(Api.GetLicenseHandler);
			}
            // HMM.
			Api.GetLicense(checker, Api._appId, Api._appKey);
		}

        public static int Init(StatusCallback callback, string appId)
        {
            if (callback == null || string.IsNullOrEmpty(appId))
                throw new InvalidOperationException("callback == null || string.IsNullOrEmpty(appId)");
            Viveport.Internal.StatusCallback initCallback = new Viveport.Internal.StatusCallback(callback.Invoke);
            Api.InternalStatusCallbacks.Add(initCallback);
            return Viveport.Internal.Api.Init(initCallback, appId);
        }

        public static int Shutdown(StatusCallback callback)
        {
            if (callback == null)
                throw new InvalidOperationException("callback == null");
            Viveport.Internal.StatusCallback initCallback = new Viveport.Internal.StatusCallback(callback.Invoke);
            Api.InternalStatusCallbacks.Add(initCallback);
            return Viveport.Internal.Api.Shutdown(initCallback);
        }

        // Token: 0x060049A6 RID: 18854 RVA: 0x0018A4AC File Offset: 0x001888AC
        public static string Version()
		{
			string text = string.Empty;
			try
			{
				text += Api.Version();
			}
			catch (Exception)
			{
				Logger.Log("Can not load version from native library");
			}
			return "C# version: " + Api.VERSION + ", Native version: " + text;
		}

        // Token: 0x060049A7 RID: 18855 RVA: 0x0018A50C File Offset: 0x0018890C
        public static void QueryRuntimeMode(QueryRuntimeModeCallback callback)
        {
            if (callback == null)
                throw new InvalidOperationException("callback == null");
            Viveport.Internal.QueryRuntimeModeCallback queryRunTimeCallback = new Viveport.Internal.QueryRuntimeModeCallback(callback.Invoke);
            Api.InternalQueryRunTimeCallbacks.Add(queryRunTimeCallback);
            Viveport.Internal.Api.QueryRuntimeMode(queryRunTimeCallback);
        }

        // Token: 0x060049A8 RID: 18856 RVA: 0x0018A548 File Offset: 0x00188948
        private static void GetLicenseHandler([MarshalAs(UnmanagedType.LPStr)] string message, [MarshalAs(UnmanagedType.LPStr)] string signature)
		{
			if (string.IsNullOrEmpty(message))
			{
				for (int i = Api.InternalLicenseCheckers.Count - 1; i >= 0; i--)
				{
					Api.LicenseChecker licenseChecker = Api.InternalLicenseCheckers[i];
					licenseChecker.OnFailure(90003, "License message is empty");
					Api.InternalLicenseCheckers.Remove(licenseChecker);
				}
				return;
			}
			if (string.IsNullOrEmpty(signature))
			{
				JsonData jsonData = JsonMapper.ToObject(message);
				int errorCode = 99999;
				string errorMessage = string.Empty;
				try
				{
					errorCode = int.Parse((string)jsonData["code"]);
				}
				catch
				{
				}
				try
				{
					errorMessage = (string)jsonData["message"];
				}
				catch
				{
				}
				for (int j = Api.InternalLicenseCheckers.Count - 1; j >= 0; j--)
				{
					Api.LicenseChecker licenseChecker2 = Api.InternalLicenseCheckers[j];
					licenseChecker2.OnFailure(errorCode, errorMessage);
					Api.InternalLicenseCheckers.Remove(licenseChecker2);
				}
				return;
			}
			if (!Api.VerifyMessage(Api._appId, Api._appKey, message, signature))
			{
				for (int k = Api.InternalLicenseCheckers.Count - 1; k >= 0; k--)
				{
					Api.LicenseChecker licenseChecker3 = Api.InternalLicenseCheckers[k];
					licenseChecker3.OnFailure(90001, "License verification failed");
					Api.InternalLicenseCheckers.Remove(licenseChecker3);
				}
				return;
			}
			string @string = Encoding.UTF8.GetString(Convert.FromBase64String(message.Substring(message.IndexOf("\n", StringComparison.Ordinal) + 1)));
			JsonData jsonData2 = JsonMapper.ToObject(@string);
			Logger.Log("License: " + @string);
			long issueTime = -1L;
			long expirationTime = -1L;
			int latestVersion = -1;
			bool updateRequired = false;
			try
			{
				issueTime = (long)jsonData2["issueTime"];
			}
			catch
			{
			}
			try
			{
				expirationTime = (long)jsonData2["expirationTime"];
			}
			catch
			{
			}
			try
			{
				latestVersion = (int)jsonData2["latestVersion"];
			}
			catch
			{
			}
			try
			{
				updateRequired = (bool)jsonData2["updateRequired"];
			}
			catch
			{
			}
			for (int l = Api.InternalLicenseCheckers.Count - 1; l >= 0; l--)
			{
				Api.LicenseChecker licenseChecker4 = Api.InternalLicenseCheckers[l];
				licenseChecker4.OnSuccess(issueTime, expirationTime, latestVersion, updateRequired);
				Api.InternalLicenseCheckers.Remove(licenseChecker4);
			}
		}

		// Token: 0x060049A9 RID: 18857 RVA: 0x0018A81C File Offset: 0x00188C1C
		private static bool VerifyMessage(string appId, string appKey, string message, string signature)
		{
			try
			{
				RSACryptoServiceProvider rsacryptoServiceProvider = PEMKeyLoader.CryptoServiceProviderFromPublicKeyInfo(appKey);
				byte[] signature2 = Convert.FromBase64String(signature);
				SHA1Managed halg = new SHA1Managed();
				byte[] bytes = Encoding.UTF8.GetBytes(appId + "\n" + message);
				return rsacryptoServiceProvider.VerifyData(bytes, halg, signature2);
			}
			catch (Exception ex)
			{
				Logger.Log(ex.ToString());
			}
			return false;
		}

		// Token: 0x040031ED RID: 12781
		internal static readonly List<GetLicenseCallback> InternalGetLicenseCallbacks = new List<GetLicenseCallback>();

        // Token: 0x040031EE RID: 12782
        internal static readonly List<Viveport.Internal.StatusCallback> InternalStatusCallbacks = new List<Viveport.Internal.StatusCallback>();
        internal static readonly List<Viveport.Internal.QueryRuntimeModeCallback> InternalQueryRunTimeCallbacks = new List<Viveport.Internal.QueryRuntimeModeCallback>();

        // Token: 0x040031EF RID: 12783

		// Token: 0x040031F0 RID: 12784
		internal static readonly List<Api.LicenseChecker> InternalLicenseCheckers = new List<Api.LicenseChecker>();

		// Token: 0x040031F1 RID: 12785
		private static readonly string VERSION = "1.6.3.6";

		// Token: 0x040031F2 RID: 12786
		private static string _appId = string.Empty;

		// Token: 0x040031F3 RID: 12787
		private static string _appKey = string.Empty;

		// Token: 0x040031F4 RID: 12788
		[CompilerGenerated]
		private static GetLicenseCallback f__mg0;

		// Token: 0x02000980 RID: 2432
		public abstract class LicenseChecker
		{
			// Token: 0x060049AC RID: 18860
			public abstract void OnSuccess(long issueTime, long expirationTime, int latestVersion, bool updateRequired);

			// Token: 0x060049AD RID: 18861
			public abstract void OnFailure(int errorCode, string errorMessage);
		}
	}
}
