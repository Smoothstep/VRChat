using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x0200076F RID: 1903
[Serializable]
public class ServerSettings : ScriptableObject
{
	// Token: 0x06003E45 RID: 15941 RVA: 0x00139555 File Offset: 0x00137955
	public void UseCloudBestRegion(string cloudAppid)
	{
		this.HostType = ServerSettings.HostingOption.BestRegion;
		this.AppID = cloudAppid;
	}

	// Token: 0x06003E46 RID: 15942 RVA: 0x00139565 File Offset: 0x00137965
	public void UseCloud(string cloudAppid)
	{
		this.HostType = ServerSettings.HostingOption.PhotonCloud;
		this.AppID = cloudAppid;
	}

	// Token: 0x06003E47 RID: 15943 RVA: 0x00139575 File Offset: 0x00137975
	public void UseCloud(string cloudAppid, CloudRegionCode code)
	{
		this.HostType = ServerSettings.HostingOption.PhotonCloud;
		this.AppID = cloudAppid;
		this.PreferredRegion = code;
	}

	// Token: 0x06003E48 RID: 15944 RVA: 0x0013958C File Offset: 0x0013798C
	public void UseMyServer(string serverAddress, int serverPort, string application)
	{
		this.HostType = ServerSettings.HostingOption.SelfHosted;
		this.AppID = ((application == null) ? "master" : application);
		this.ServerAddress = serverAddress;
		this.ServerPort = serverPort;
	}

	// Token: 0x06003E49 RID: 15945 RVA: 0x001395BC File Offset: 0x001379BC
	public static bool IsAppId(string val)
	{
		try
		{
			new Guid(val);
		}
		catch
		{
			return false;
		}
		return true;
	}

	// Token: 0x170009FB RID: 2555
	// (get) Token: 0x06003E4A RID: 15946 RVA: 0x001395F0 File Offset: 0x001379F0
	public static CloudRegionCode BestRegionCodeInPreferences
	{
		get
		{
			return PhotonHandler.BestRegionCodeInPreferences;
		}
	}

	// Token: 0x06003E4B RID: 15947 RVA: 0x001395F7 File Offset: 0x001379F7
	public static void ResetBestRegionCodeInPreferences()
	{
		PhotonHandler.BestRegionCodeInPreferences = CloudRegionCode.none;
	}

	// Token: 0x06003E4C RID: 15948 RVA: 0x001395FF File Offset: 0x001379FF
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"ServerSettings: ",
			this.HostType,
			" ",
			this.ServerAddress
		});
	}

	// Token: 0x040026B7 RID: 9911
	public string AppID = string.Empty;

	// Token: 0x040026B8 RID: 9912
	public string VoiceAppID = string.Empty;

	// Token: 0x040026B9 RID: 9913
	public string ChatAppID = string.Empty;

	// Token: 0x040026BA RID: 9914
	public ServerSettings.HostingOption HostType;

	// Token: 0x040026BB RID: 9915
	public CloudRegionCode PreferredRegion;

	// Token: 0x040026BC RID: 9916
	public CloudRegionFlag EnabledRegions = (CloudRegionFlag)(-1);

	// Token: 0x040026BD RID: 9917
	public ConnectionProtocol Protocol;

	// Token: 0x040026BE RID: 9918
	public string ServerAddress = string.Empty;

	// Token: 0x040026BF RID: 9919
	public int ServerPort = 5055;

	// Token: 0x040026C0 RID: 9920
	public int VoiceServerPort = 5055;

	// Token: 0x040026C1 RID: 9921
	public bool JoinLobby;

	// Token: 0x040026C2 RID: 9922
	public bool EnableLobbyStatistics;

	// Token: 0x040026C3 RID: 9923
	public PhotonLogLevel PunLogging;

	// Token: 0x040026C4 RID: 9924
	public DebugLevel NetworkLogging = DebugLevel.ERROR;

	// Token: 0x040026C5 RID: 9925
	public bool RunInBackground = true;

	// Token: 0x040026C6 RID: 9926
	public List<string> RpcList = new List<string>();

	// Token: 0x040026C7 RID: 9927
	[HideInInspector]
	public bool DisableAutoOpenWizard;

	// Token: 0x02000770 RID: 1904
	public enum HostingOption
	{
		// Token: 0x040026C9 RID: 9929
		NotSet,
		// Token: 0x040026CA RID: 9930
		PhotonCloud,
		// Token: 0x040026CB RID: 9931
		SelfHosted,
		// Token: 0x040026CC RID: 9932
		OfflineMode,
		// Token: 0x040026CD RID: 9933
		BestRegion,
		// Token: 0x040026CE RID: 9934
		RoundRobin
	}
}
