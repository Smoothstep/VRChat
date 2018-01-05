using System;
using System.Text;
using Steamworks;
using UnityEngine;
using VRC.Core;

// Token: 0x02000AFC RID: 2812
[DisallowMultipleComponent]
internal class SteamManager : MonoBehaviour
{
	// Token: 0x17000C4A RID: 3146
	// (get) Token: 0x0600550E RID: 21774 RVA: 0x001D52BA File Offset: 0x001D36BA
	private static SteamManager Instance
	{
		get
		{
			return SteamManager.s_instance ?? new GameObject("SteamManager").AddComponent<SteamManager>();
		}
	}

	// Token: 0x17000C4B RID: 3147
	// (get) Token: 0x0600550F RID: 21775 RVA: 0x001D52D7 File Offset: 0x001D36D7
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x06005510 RID: 21776 RVA: 0x001D52E3 File Offset: 0x001D36E3
	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x06005511 RID: 21777 RVA: 0x001D52EC File Offset: 0x001D36EC
	private void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (VRCApplicationSetup.Instance.ServerEnvironment == ApiServerEnvironment.Dev)
		{
			this.vrchatAppId = (AppId_t)326u;
		}
		if (SteamManager.s_EverInialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary(this.vrchatAppId))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException arg)
		{
			Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg, this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
		SteamManager.s_EverInialized = true;
	}

	// Token: 0x06005512 RID: 21778 RVA: 0x001D53F8 File Offset: 0x001D37F8
	private void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x06005513 RID: 21779 RVA: 0x001D544F File Offset: 0x001D384F
	private void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x06005514 RID: 21780 RVA: 0x001D5479 File Offset: 0x001D3879
	private void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x04003C0F RID: 15375
	private static SteamManager s_instance;

	// Token: 0x04003C10 RID: 15376
	private static bool s_EverInialized;

	// Token: 0x04003C11 RID: 15377
	private bool m_bInitialized;

	// Token: 0x04003C12 RID: 15378
	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

	// Token: 0x04003C13 RID: 15379
	private AppId_t vrchatAppId = (AppId_t)438u;
}
