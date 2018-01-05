using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using UnityEngine;
using UnityEngine.VR;
using VRC.Core;

// Token: 0x02000B2F RID: 2863
public class VRCApplicationSetup : MonoBehaviour
{
	// Token: 0x17000CA4 RID: 3236
	// (get) Token: 0x06005741 RID: 22337 RVA: 0x001E0D41 File Offset: 0x001DF141
	public string gameServerVersion
	{
		get
		{
			return this.GetGameServerVersion();
		}
	}

	// Token: 0x17000CA5 RID: 3237
	// (get) Token: 0x06005742 RID: 22338 RVA: 0x001E0D49 File Offset: 0x001DF149
	public static VRCApplicationSetup Instance
	{
		get
		{
			if (VRCApplicationSetup._instance == null)
			{
				VRCApplicationSetup._instance = UnityEngine.Object.FindObjectOfType<VRCApplicationSetup>();
			}
			return VRCApplicationSetup._instance;
		}
	}

	// Token: 0x17000CA6 RID: 3238
	// (get) Token: 0x06005743 RID: 22339 RVA: 0x001E0D6A File Offset: 0x001DF16A
	public static bool IsVRSDKInitialized
	{
		get
		{
			return VRCApplicationSetup._isVRSDKInitialized;
		}
	}

	// Token: 0x17000CA7 RID: 3239
	// (get) Token: 0x06005744 RID: 22340 RVA: 0x001E0D71 File Offset: 0x001DF171
	public static bool LaunchedInDesktopMode
	{
		get
		{
			return VRCApplicationSetup._launchedInDesktopMode;
		}
	}

	// Token: 0x06005745 RID: 22341 RVA: 0x001E0D78 File Offset: 0x001DF178
	private void Awake()
    {
        UnityEngine.Debug.developerConsoleVisible = true;
        if (VRCApplicationSetup._instance != null && VRCApplicationSetup._instance != this)
		{
            UnityEngine.Debug.Log("Destroying me.");
			UnityEngine.Object.Destroy(VRCApplicationSetup._instance);
		}
		VRCApplicationSetup._instance = this;
		VRCApplicationSetup.commandLine = base.GetComponent<VRCFlowCommandLine>();
		VRCFlowManager component = base.gameObject.GetComponent<VRCFlowManager>();
		if (component != null)
		{
			component.enabled = true;
		}
		VRCApplication.AppType = this.appType;
		VRCApplication.appVersion = this.appVersion;
		VRCApplication.shouldAutoUpdate = this.shouldAutoUpdate;
		ApiModel.SetApiUrlFromEnvironment(this.ServerEnvironment);
		VRCApplicationSetup._sMimicStandaloneBuild = this.MimicStandaloneBuildSettingsInEditor;
		string empty = string.Empty;
		VRCApplication.clientId = this.clientId;
        
		UnityEngine.Debug.Log("Client ID: " + VRCApplication.clientId);
		UnityEngine.Debug.Log("Prefix: " + empty);
		VRCApplication.clientVersion = empty + this.appVersion;
		VRCApplication.clientVersion = empty + "w_" + this.appVersion;
		VRCApplicationSetup.commandLine.ReadCommandLine();
		PhotonCustomTypes.Register();
		PhotonNetwork.networkingPeer.ChannelCount = 51;
		this.InitVRSDK();
	}

	// Token: 0x06005746 RID: 22342 RVA: 0x001E0E94 File Offset: 0x001DF294
	public static bool CheckForUpdates()
	{
		bool flag = !string.IsNullOrEmpty(VRCApplicationSetup.parseReleaseAppVersion = RemoteConfig.GetString("releaseAppVersionStandalone"));
		flag = !string.IsNullOrEmpty(VRCApplicationSetup.releaseUnityUrl = RemoteConfig.GetString("downloadLinkWindows"));
		flag = !string.IsNullOrEmpty(VRCApplicationSetup.releaseOculusUrl = RemoteConfig.GetString("downloadLinkWindows"));
		flag = !string.IsNullOrEmpty(VRCApplicationSetup.releaseMacUrl = RemoteConfig.GetString("downloadLinkMac"));
		flag = !string.IsNullOrEmpty(VRCApplicationSetup.releaseViveUrl = RemoteConfig.GetString("viveWindowsUrl"));
		if (flag)
		{
			if ((VRCApplication.clientId == "vrc" || VRCApplication.clientId == "vrc5") && VRCApplication.shouldAutoUpdate && VRCApplicationSetup.parseReleaseAppVersion != VRCApplication.appVersion)
			{
				VRCApplicationSetup.needsUpdate = true;
			}
		}
		else
		{
			VRCApplicationSetup.releaseViveUrl = (VRCApplicationSetup.devViveUrl = (VRCApplicationSetup.releaseUnityUrl = (VRCApplicationSetup.releaseOculusUrl = (VRCApplicationSetup.releaseMacUrl = (VRCApplicationSetup.devOculusUrl = (VRCApplicationSetup.devMacUrl = "http://vrchat.com"))))));
			VRCApplicationSetup.needsUpdate = false;
			UnityEngine.Debug.LogError("Unable to fetch update info from parse.");
		}
		return VRCApplicationSetup.needsUpdate;
	}

	// Token: 0x06005747 RID: 22343 RVA: 0x001E0FB8 File Offset: 0x001DF3B8
	public static void DownloadUpdate()
	{
		string url = VRCApplicationSetup.releaseViveUrl;
		Application.OpenURL(url);
	}

	// Token: 0x06005748 RID: 22344 RVA: 0x001E0FD7 File Offset: 0x001DF3D7
	public static bool IsEditor()
	{
		return Application.isEditor && !VRCApplicationSetup._sMimicStandaloneBuild;
	}

	// Token: 0x06005749 RID: 22345 RVA: 0x001E0FF0 File Offset: 0x001DF3F0
	public string GetGameServerVersion()
	{
		if (this.gameServerVersionOverride != null && !string.IsNullOrEmpty(this.gameServerVersionOverride.Trim()))
		{
			return this.gameServerVersionOverride;
		}
		return this.ServerEnvironment.ToString() + "_server_" + this.buildNumber;
	}

	// Token: 0x0600574A RID: 22346 RVA: 0x001E104C File Offset: 0x001DF44C
	private void InitVRSDK()
	{
		bool disableVR = VRCApplicationSetup.commandLine.DisableVR;
		if (disableVR)
		{
			VRCApplicationSetup._launchedInDesktopMode = true;
			VRSettings.enabled = false;
			SteamVR.enabled = false;
			this.PostInitVRSDK();
		}
		else
		{
			base.StartCoroutine(this.InitVRSDK_Routine());
		}
	}

	// Token: 0x0600574B RID: 22347 RVA: 0x001E1094 File Offset: 0x001DF494
	public static void SetBundleVersion()
	{
		UnityEngine.Debug.LogError("VRCApplicationSetup.SetBundleVersion failed - not in editor!");
	}

	// Token: 0x0600574C RID: 22348 RVA: 0x001E10A0 File Offset: 0x001DF4A0
	private IEnumerator InitVRSDK_Routine()
	{
		string deviceName = string.Empty;
		deviceName = "OpenVR";
		UnityEngine.Debug.Log("InitVRSDK: " + deviceName);
		if (Array.IndexOf<string>(VRSettings.supportedDevices, deviceName) < 0)
		{
			UnityEngine.Debug.LogError("InitVRSDK failed: '" + deviceName + "' is not in supportedDevicesList!");
			this.PostInitVRSDK();
			yield break;
		}
		VRSettings.LoadDeviceByName(new string[]
		{
			deviceName,
			"None"
		});
		yield return null;
		VRSettings.enabled = true;
		UnityEngine.Debug.Log("InitVRSDK: " + deviceName + " success!");
		if (!string.IsNullOrEmpty(deviceName) && deviceName.Contains("OpenVR") && SteamVR.instance != null)
		{
			UnityEngine.Debug.Log("Steam tracking system name: '" + SteamVR.instance.hmd_TrackingSystemName + "'");
		}
		this.PostInitVRSDK();
		yield break;
	}

	// Token: 0x0600574D RID: 22349 RVA: 0x001E10BB File Offset: 0x001DF4BB
	private void PostInitVRSDK()
	{
		VRCApplicationSetup._isVRSDKInitialized = true;
		base.StartCoroutine(this.PostInitVRSDK_Routine());
	}

	// Token: 0x0600574E RID: 22350 RVA: 0x001E10D0 File Offset: 0x001DF4D0
	private IEnumerator PostInitVRSDK_Routine()
	{
		if (this.EnableAfterVRInit != null)
		{
			foreach (GameObject gameObject in this.EnableAfterVRInit)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(true);
				}
			}
			yield return null;
			yield return null;
			foreach (GameObject gameObject2 in this.EnableAfterVRInit)
			{
				if (gameObject2 != null)
				{
					gameObject2.SendMessage("OnVRSDKInitialized", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		yield break;
	}

	// Token: 0x0600574F RID: 22351 RVA: 0x001E10EC File Offset: 0x001DF4EC
	private void OnApplicationQuit()
	{
		bool flag = true;
		RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey("VRChat");
		if (registryKey != null)
		{
			RegistryKey registryKey2 = registryKey.OpenSubKey("shell");
			if (registryKey2 != null)
			{
				RegistryKey registryKey3 = registryKey2.OpenSubKey("open");
				if (registryKey3 != null)
				{
					RegistryKey registryKey4 = registryKey3.OpenSubKey("command");
					if (registryKey4 != null)
					{
						string text = registryKey4.GetValue(string.Empty).ToString();
						string text2 = text.TrimStart(new char[]
						{
							'"'
						});
						text2 = text2.Substring(0, text2.IndexOf('"'));
						string a = Path.GetDirectoryName(text2).Replace('\\', '/');
						string directoryName = Path.GetDirectoryName(Application.dataPath.Replace("VRChat_Data", string.Empty));
						if (a == directoryName)
						{
							flag = false;
						}
					}
				}
			}
		}
		if (flag && !VRCApplicationSetup._hasLaunchURLProtocolInstaller)
		{
			VRCApplicationSetup._hasLaunchURLProtocolInstaller = true;
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = Application.dataPath + "/../install.exe";
			if (File.Exists(processStartInfo.FileName))
			{
				Process.Start(processStartInfo);
			}
		}
	}

	// Token: 0x06005750 RID: 22352 RVA: 0x001E120C File Offset: 0x001DF60C
	public static string GetBuildDescriptionString()
	{
		return string.Concat(new string[]
		{
			"VRChat ",
			VRCApplicationSetup.GetBuildVersionString(),
			", ",
			VRCApplicationSetup.GetBuildStoreID(),
			" ",
			Application.platform.ToString()
		});
	}

	// Token: 0x06005751 RID: 22353 RVA: 0x001E1264 File Offset: 0x001DF664
	public static string GetBuildVersionString()
	{
		return string.Concat(new object[]
		{
			VRCApplicationSetup.Instance.appVersion,
			":",
			VRCApplicationSetup.Instance.buildNumber,
			":",
			VRCApplicationSetup.Instance.ServerEnvironment
		});
	}

	// Token: 0x06005752 RID: 22354 RVA: 0x001E12BD File Offset: 0x001DF6BD
	public static string GetBuildStoreID()
	{
		return "Steam";
	}

	// Token: 0x04003E2F RID: 15919
	[HideInInspector]
	public VRCApplication.VRCAppType appType;

	// Token: 0x04003E30 RID: 15920
	public ApiServerEnvironment ServerEnvironment;

	// Token: 0x04003E31 RID: 15921
	public string appVersion = string.Empty;

	// Token: 0x04003E32 RID: 15922
	public string gameServerVersionOverride = string.Empty;

	// Token: 0x04003E33 RID: 15923
	public string clientId = "vrc5";

	// Token: 0x04003E34 RID: 15924
	public bool shouldAutoUpdate;

	// Token: 0x04003E35 RID: 15925
	public int buildNumber;

	// Token: 0x04003E36 RID: 15926
	public bool MimicStandaloneBuildSettingsInEditor;

	// Token: 0x04003E37 RID: 15927
	private static bool _sMimicStandaloneBuild;

	// Token: 0x04003E38 RID: 15928
	public GameObject[] EnableAfterVRInit;

	// Token: 0x04003E39 RID: 15929
	public static bool needsUpdate;

	// Token: 0x04003E3A RID: 15930
	public static string releaseUnityUrl = "http://vrchat.com";

	// Token: 0x04003E3B RID: 15931
	public static string releaseOculusUrl = "http://vrchat.com";

	// Token: 0x04003E3C RID: 15932
	public static string releaseMacUrl = "http://vrchat.com";

	// Token: 0x04003E3D RID: 15933
	public static string releaseViveUrl = "http://vrchat.com";

	// Token: 0x04003E3E RID: 15934
	public static string devOculusUrl = "http://vrchat.com";

	// Token: 0x04003E3F RID: 15935
	public static string devMacUrl = "http://vrchat.com";

	// Token: 0x04003E40 RID: 15936
	public static string devViveUrl = "http://vrchat.com";

	// Token: 0x04003E41 RID: 15937
	public static string parseReleaseAppVersion = string.Empty;

	// Token: 0x04003E42 RID: 15938
	public static string parseDevAppVersion = string.Empty;

	// Token: 0x04003E43 RID: 15939
	public static VRCFlowCommandLine commandLine;

	// Token: 0x04003E44 RID: 15940
	public static VRCApplicationSetup _instance;

	// Token: 0x04003E45 RID: 15941
	private static bool _isVRSDKInitialized;

	// Token: 0x04003E46 RID: 15942
	private static bool _launchedInDesktopMode;

	// Token: 0x04003E47 RID: 15943
	private static bool _hasLaunchURLProtocolInstaller;
}
