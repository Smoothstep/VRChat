using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.Core;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

// Token: 0x02000B2D RID: 2861
public class VRCApplication : MonoBehaviour
{
	// Token: 0x17000CA3 RID: 3235
	// (get) Token: 0x06005735 RID: 22325 RVA: 0x001E0994 File Offset: 0x001DED94
	public static bool inMainMenu
	{
		get
		{
			string name = SceneManager.GetActiveScene().name;
			return name == "MainMenu 1" || name == "GearMainMenu";
		}
	}
    [DllImport("kernel32.dll")]
    static extern bool AttachConsole(UInt32 dwProcessId);
    [DllImport("kernel32.dll")]
    private static extern bool GetFileInformationByHandle(SafeFileHandle hFile, out BY_HANDLE_FILE_INFORMATION lpFileInformation);
    [DllImport("kernel32.dll")]
    private static extern SafeFileHandle GetStdHandle(UInt32 nStdHandle);
    [DllImport("kernel32.dll")]
    private static extern bool SetStdHandle(UInt32 nStdHandle, SafeFileHandle hHandle);
    [DllImport("kernel32.dll")]
    private static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, SafeFileHandle hSourceHandle, IntPtr hTargetProcessHandle, out SafeFileHandle lpTargetHandle, UInt32 dwDesiredAccess, Boolean bInheritHandle, UInt32 dwOptions);
    private const UInt32 ATTACH_PARENT_PROCESS = 0xFFFFFFFF;
    private const UInt32 STD_OUTPUT_HANDLE = 0xFFFFFFF5;
    private const UInt32 STD_ERROR_HANDLE = 0xFFFFFFF4;
    private const UInt32 DUPLICATE_SAME_ACCESS = 2;
    struct BY_HANDLE_FILE_INFORMATION
    {
        public UInt32 FileAttributes;
        public System.Runtime.InteropServices.ComTypes.FILETIME CreationTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME LastWriteTime;
        public UInt32 VolumeSerialNumber;
        public UInt32 FileSizeHigh;
        public UInt32 FileSizeLow;
        public UInt32 NumberOfLinks;
        public UInt32 FileIndexHigh;
        public UInt32 FileIndexLow;
    }
    static void InitConsoleHandles()
    {
        SafeFileHandle hStdOut, hStdErr, hStdOutDup, hStdErrDup;
        BY_HANDLE_FILE_INFORMATION bhfi;
        hStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
        hStdErr = GetStdHandle(STD_ERROR_HANDLE);
        // Get current process handle
        IntPtr hProcess = System.Diagnostics.Process.GetCurrentProcess().Handle;
        // Duplicate Stdout handle to save initial value
        DuplicateHandle(hProcess, hStdOut, hProcess, out hStdOutDup,
        0, true, DUPLICATE_SAME_ACCESS);
        // Duplicate Stderr handle to save initial value
        DuplicateHandle(hProcess, hStdErr, hProcess, out hStdErrDup,
        0, true, DUPLICATE_SAME_ACCESS);
        // Attach to console window – this may modify the standard handles
        AttachConsole(ATTACH_PARENT_PROCESS);
        // Adjust the standard handles
        if (GetFileInformationByHandle(GetStdHandle(STD_OUTPUT_HANDLE), out bhfi))
        {
            SetStdHandle(STD_OUTPUT_HANDLE, hStdOutDup);
        }
        else
        {
            SetStdHandle(STD_OUTPUT_HANDLE, hStdOut);
        }
        if (GetFileInformationByHandle(GetStdHandle(STD_ERROR_HANDLE), out bhfi))
        {
            SetStdHandle(STD_ERROR_HANDLE, hStdErrDup);
        }
        else
        {
            SetStdHandle(STD_ERROR_HANDLE, hStdErr);
        }
    }

    // Token: 0x06005736 RID: 22326 RVA: 0x001E09D0 File Offset: 0x001DEDD0
    private void Awake()
	{
        InitConsoleHandles();
        Debug.Log("Awakening");
        Debug.LogError("Test");
        Console.WriteLine("Init");
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (VRCApplication.exists)
		{
			Debug.LogError("Dublicate VRCApplication detected. Destroying new one.");
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			VRCApplication.exists = true;
		}
		VRCApplication.OriginalFixedDelta = Time.fixedDeltaTime;
		Application.targetFrameRate = 90;
		Application.backgroundLoadingPriority = ThreadPriority.Low;
		PhotonNetwork.autoJoinLobby = false;
		PhotonNetwork.automaticallySyncScene = false;
		PhotonNetwork.CrcCheckEnabled = true;
		PhotonNetwork.QuickResends = 3;
		PhotonNetwork.MaxResendsBeforeDisconnect = 7;
	}

	// Token: 0x06005737 RID: 22327 RVA: 0x001E0A48 File Offset: 0x001DEE48
	private IEnumerator DebugPrintObjectCountsEvery(float s)
	{
		for (;;)
		{
			yield return new WaitForSecondsRealtime(s);
			if (RoomManager.inRoom)
			{
				VRCApplication.DebugPrintAllObjectCounts();
			}
		}
		yield break;
	}

	// Token: 0x06005738 RID: 22328 RVA: 0x001E0A64 File Offset: 0x001DEE64
	public static void LoadMainMenu()
	{
		string sceneName = "MainMenu 1";
		VRCApplication.VRCAppType appType = VRCApplication.AppType;
		if (appType != VRCApplication.VRCAppType.Default)
		{
			if (appType != VRCApplication.VRCAppType.GearDemo)
			{
				if (appType == VRCApplication.VRCAppType.UI2)
				{
					sceneName = "Application2";
				}
			}
			else
			{
				sceneName = "GearMainMenu";
			}
		}
		AssetManagement.LoadLevel(sceneName);
	}

	// Token: 0x06005739 RID: 22329 RVA: 0x001E0AB7 File Offset: 0x001DEEB7
	public static bool IsWindows10()
	{
		return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major == 10;
	}

	// Token: 0x0600573A RID: 22330 RVA: 0x001E0AE2 File Offset: 0x001DEEE2
	public static bool IsAndroid()
	{
		return false;
	}

	// Token: 0x0600573B RID: 22331 RVA: 0x001E0AE5 File Offset: 0x001DEEE5
	public static bool IsOSX()
	{
		return false;
	}

	// Token: 0x0600573C RID: 22332 RVA: 0x001E0AE8 File Offset: 0x001DEEE8
	private void OnApplicationQuit()
	{
		SteamAPI.Shutdown();
	}

	// Token: 0x0600573D RID: 22333 RVA: 0x001E0AF0 File Offset: 0x001DEEF0
	public static void DebugPrintAllObjectCounts()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		int num = 0;
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll<UnityEngine.Object>();
		foreach (UnityEngine.Object @object in array)
		{
			if (!(@object == null))
			{
				int num2 = 0;
				if (!dictionary.TryGetValue(@object.GetType().FullName, out num2))
				{
					dictionary[@object.GetType().FullName] = 1;
				}
				else
				{
					dictionary[@object.GetType().FullName] = num2 + 1;
				}
				num++;
			}
		}
		List<KeyValuePair<string, int>> list = dictionary.ToList<KeyValuePair<string, int>>();
		list.Sort((KeyValuePair<string, int> x, KeyValuePair<string, int> y) => y.Value - x.Value);
		Debug.Log("==== Object Counts: total " + num);
		foreach (KeyValuePair<string, int> keyValuePair in list)
		{
			Debug.LogFormat("- {0}: {1}", new object[]
			{
				keyValuePair.Key,
				keyValuePair.Value
			});
		}
		Debug.Log("==");
	}

	// Token: 0x04003E22 RID: 15906
	public static VRCApplication.VRCAppType AppType;

	// Token: 0x04003E23 RID: 15907
	public static bool exists;

	// Token: 0x04003E24 RID: 15908
	public static string clientVersion = string.Empty;

	// Token: 0x04003E25 RID: 15909
	public static string appVersion = string.Empty;

	// Token: 0x04003E26 RID: 15910
	public static string clientId;

	// Token: 0x04003E27 RID: 15911
	public static bool shouldAutoUpdate;

	// Token: 0x04003E28 RID: 15912
	public static float OriginalFixedDelta = 0.0333333351f;

	// Token: 0x02000B2E RID: 2862
	public enum VRCAppType
	{
		// Token: 0x04003E2B RID: 15915
		Default,
		// Token: 0x04003E2C RID: 15916
		GearDemo,
		// Token: 0x04003E2D RID: 15917
		UI2,
		// Token: 0x04003E2E RID: 15918
		OldUi
	}
}
