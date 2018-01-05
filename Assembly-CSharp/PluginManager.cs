using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using BestHTTP;
using UnityEngine;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000B52 RID: 2898
[Serializable]
public class PluginManager : MonoBehaviour
{
	// Token: 0x17000CD6 RID: 3286
	// (get) Token: 0x060058C7 RID: 22727 RVA: 0x001EBF30 File Offset: 0x001EA330
	public static IEnumerable<Assembly> LoadedAssemblies
	{
		get
		{
			return PluginManager.loadedAssemblies.Values;
		}
	}

	// Token: 0x17000CD7 RID: 3287
	// (get) Token: 0x060058C8 RID: 22728 RVA: 0x001EBF3C File Offset: 0x001EA33C
	public static IEnumerable<Assembly> PluginAssemblies
	{
		get
		{
			IEnumerable<Assembly> values = PluginManager.loadedAssemblies.Values;
			if (PluginManager.f__mg0 == null)
			{
				PluginManager.f__mg0 = new Func<Assembly, bool>(PluginManager.IsPluginAssembly);
			}
			return values.Where(PluginManager.f__mg0);
		}
	}

	// Token: 0x060058C9 RID: 22729 RVA: 0x001EBF6C File Offset: 0x001EA36C
	public static bool IsPluginAssembly(Assembly a)
	{
		if (a == null)
		{
			return false;
		}
		string name = a.GetName().Name;
		return name.StartsWith("vrc");
	}

	// Token: 0x060058CA RID: 22730 RVA: 0x001EBF98 File Offset: 0x001EA398
	private PluginManager.DomainConfiguration CreateBasicConfiguration()
	{
		PluginManager.DomainConfiguration result = default(PluginManager.DomainConfiguration);
		string dataPath = Application.dataPath;
		result.permissions = new PermissionSet(PermissionState.None);
		result.permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.PathDiscovery, dataPath));
		result.permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read, dataPath));
		result.setup = new AppDomainSetup();
		result.setup.ApplicationBase = dataPath;
		result.setup.ApplicationName = "Sandbox";
		result.evidence = AppDomain.CurrentDomain.Evidence;
		return result;
	}

	// Token: 0x060058CB RID: 22731 RVA: 0x001EC024 File Offset: 0x001EA424
	private bool LoadInAppDomain(AppDomain target, byte[] bytes)
	{
		try
		{
			AssemblyLoader @object = new AssemblyLoader
			{
				DataBytes = bytes
			};
			target.DoCallBack(new CrossAppDomainDelegate(@object.Load));
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.ToString());
			Debug.LogError("Failed to load Assembly");
			return false;
		}
		return true;
	}

	// Token: 0x060058CC RID: 22732 RVA: 0x001EC088 File Offset: 0x001EA488
	private AppDomain CreateDomain(string name, PluginManager.DomainConfiguration config)
	{
		AppDomain appDomain = null;
		AppDomain result;
		try
		{
			appDomain = AppDomain.CreateDomain(name, config.evidence, config.setup, config.permissions, new StrongName[]
			{
				this.GetStrongName()
			});
			appDomain.Load(typeof(AssemblyLoader).Assembly.GetName());
			result = appDomain;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			if (appDomain != null)
			{
				AppDomain.Unload(appDomain);
			}
			result = null;
		}
		return result;
	}

	// Token: 0x060058CD RID: 22733 RVA: 0x001EC110 File Offset: 0x001EA510
	[SecurityCritical]
	public AppDomain Load(string url, byte[] bytes)
	{
		if (!PluginManager.IsAllowedUrl(url) || bytes == null || bytes.Length == 0)
		{
			Debug.LogError("Invalid plugin: " + url);
			return null;
		}
		try
		{
			Assembly assembly = Assembly.Load(bytes);
			if (!PluginManager.loadedAssemblies.ContainsKey(assembly.GetName().Name))
			{
				PluginManager.loadedAssemblies.Add(assembly.GetName().Name, assembly);
			}
			Debug.Log("<color=red>Loaded Assembly " + base.name + "</color>");
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			Debug.LogError("Failed to load plugin at url " + url);
		}
		return null;
	}

	// Token: 0x060058CE RID: 22734 RVA: 0x001EC1CC File Offset: 0x001EA5CC
	private StrongName GetStrongName()
	{
		return this.GetStrongName(typeof(VRC_SceneDescriptor).Assembly);
	}

	// Token: 0x060058CF RID: 22735 RVA: 0x001EC1E4 File Offset: 0x001EA5E4
	private StrongName GetStrongName(Assembly assembly)
	{
		if (assembly == null)
		{
			return null;
		}
		AssemblyName name = assembly.GetName();
		byte[] publicKey = name.GetPublicKey();
		if (publicKey == null || publicKey.Length == 0)
		{
			Debug.LogError("Assembly is not strongly named");
			return null;
		}
		StrongNamePublicKeyBlob blob = new StrongNamePublicKeyBlob(publicKey);
		return new StrongName(blob, name.Name, name.Version);
	}

	// Token: 0x060058D0 RID: 22736 RVA: 0x001EC23C File Offset: 0x001EA63C
	private void CleanupAppDomains()
	{
		foreach (AppDomain domain in PluginManager.loadedDomains)
		{
			try
			{
				AppDomain.Unload(domain);
			}
			catch
			{
			}
		}
		PluginManager.loadedDomains.Clear();
		PluginManager.loadedAssemblies.Clear();
	}

	// Token: 0x060058D1 RID: 22737 RVA: 0x001EC2C4 File Offset: 0x001EA6C4
	private void OnDestroy()
	{
		this.CleanupAppDomains();
	}

	// Token: 0x060058D2 RID: 22738 RVA: 0x001EC2CC File Offset: 0x001EA6CC
	public static bool IsAllowedUrl(string url)
	{
		if (string.IsNullOrEmpty(url))
		{
			Debug.LogError("Plugin URL is empty");
			return false;
		}
		if (url.ToLower().StartsWith("file:///"))
		{
			return true;
		}
		List<string> list = RemoteConfig.GetList("whiteListedAssetUrls");
		if (list == null || list.Count == 0)
		{
			Debug.LogError("No plugins are allowed");
			return false;
		}
		if (list.Any((string allowed) => url.ToLower().StartsWith(allowed.ToLower())))
		{
			return true;
		}
		Debug.LogError("Will not load plugin at " + url);
		return false;
	}

	// Token: 0x17000CD8 RID: 3288
	// (get) Token: 0x060058D3 RID: 22739 RVA: 0x001EC378 File Offset: 0x001EA778
	public static PluginManager Instance
	{
		get
		{
			if (PluginManager.instance == null)
			{
				VRCApplication vrcapplication = UnityEngine.Object.FindObjectOfType<VRCApplication>();
				if (vrcapplication != null)
				{
					PluginManager.instance = vrcapplication.gameObject.AddComponent<PluginManager>();
				}
			}
			return PluginManager.instance;
		}
	}

	// Token: 0x060058D4 RID: 22740 RVA: 0x001EC3BC File Offset: 0x001EA7BC
	public string GetPluginUrl(ApiWorld bp)
	{
		this.whiteListedDllUrls = RemoteConfig.GetList("whiteListedAssetUrls");
		bool flag = this.whiteListedDllUrls.Any((string url) => bp.pluginUrl.ToLower().StartsWith(url.ToLower()));
		if (!this.IsValidURL(bp.pluginUrl) || (!flag && !bp.pluginUrl.StartsWith("file")))
		{
			return null;
		}
		return bp.pluginUrl;
	}

	// Token: 0x060058D5 RID: 22741 RVA: 0x001EC444 File Offset: 0x001EA844
	public string GetPluginUrl(string assetUrl)
	{
		if (!this.IsValidURL(assetUrl))
		{
			return null;
		}
		if (!RemoteConfig.IsInitialized())
		{
			Debug.LogWarning("Remote Config is not set. Not downloading DLL.");
			return null;
		}
		this.whiteListedDllUrls = RemoteConfig.GetList("whiteListedAssetUrls");
		bool flag = this.whiteListedDllUrls.Any((string url) => assetUrl.ToLower().StartsWith(url));
		if (!assetUrl.ToLower().StartsWith("file://") && !flag)
		{
			return null;
		}
		int num = assetUrl.ToLower().IndexOf(".vrcs");
		if (num == -1)
		{
			num = assetUrl.ToLower().IndexOf(".vrca");
		}
		if (num == -1)
		{
			num = assetUrl.ToLower().IndexOf(".vrcp");
		}
		if (num == -1)
		{
			return null;
		}
		assetUrl = assetUrl.Remove(num, 5);
		assetUrl = assetUrl.Insert(num, ".dll");
		return assetUrl;
	}

	// Token: 0x060058D6 RID: 22742 RVA: 0x001EC560 File Offset: 0x001EA960
	private bool IsValidURL(string url)
	{
		bool result = true;
		try
		{
			HTTPRequest httprequest = new HTTPRequest(new Uri(url), null);
		}
		catch
		{
			result = false;
		}
		return result;
	}

	// Token: 0x04003F8B RID: 16267
	private static Dictionary<string, Assembly> loadedAssemblies = new Dictionary<string, Assembly>();

	// Token: 0x04003F8C RID: 16268
	private static List<AppDomain> loadedDomains = new List<AppDomain>();

	// Token: 0x04003F8D RID: 16269
	private List<string> whiteListedDllUrls = new List<string>();

	// Token: 0x04003F8E RID: 16270
	private static PluginManager instance = null;

	// Token: 0x04003F8F RID: 16271
	[CompilerGenerated]
	private static Func<Assembly, bool> f__mg0;

	// Token: 0x02000B53 RID: 2899
	private struct DomainConfiguration
	{
		// Token: 0x04003F90 RID: 16272
		public PermissionSet permissions;

		// Token: 0x04003F91 RID: 16273
		public AppDomainSetup setup;

		// Token: 0x04003F92 RID: 16274
		public Evidence evidence;
	}
}
