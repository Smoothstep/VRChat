using System;
using System.Text;
using UnityEngine;

// Token: 0x020007A5 RID: 1957
public class SupportLogging : MonoBehaviour
{
	// Token: 0x06003F39 RID: 16185 RVA: 0x0013E778 File Offset: 0x0013CB78
	public void Start()
	{
		if (this.LogTrafficStats)
		{
			base.InvokeRepeating("LogStats", 10f, 10f);
		}
	}

	// Token: 0x06003F3A RID: 16186 RVA: 0x0013E79A File Offset: 0x0013CB9A
	protected void OnApplicationPause(bool pause)
	{
		Debug.Log(string.Concat(new object[]
		{
			"SupportLogger OnApplicationPause: ",
			pause,
			" connected: ",
			PhotonNetwork.connected
		}));
	}

	// Token: 0x06003F3B RID: 16187 RVA: 0x0013E7D2 File Offset: 0x0013CBD2
	public void OnApplicationQuit()
	{
		base.CancelInvoke();
	}

	// Token: 0x06003F3C RID: 16188 RVA: 0x0013E7DA File Offset: 0x0013CBDA
	public void LogStats()
	{
		if (this.LogTrafficStats)
		{
			Debug.Log("SupportLogger " + PhotonNetwork.NetworkStatisticsToString());
		}
	}

	// Token: 0x06003F3D RID: 16189 RVA: 0x0013E7FC File Offset: 0x0013CBFC
	private void LogBasics()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("SupportLogger Info: PUN {0}: ", "1.83");
		stringBuilder.AppendFormat("AppID: {0}*** GameVersion: {1} ", PhotonNetwork.networkingPeer.AppId.Substring(0, 8), PhotonNetwork.networkingPeer.AppVersion);
		stringBuilder.AppendFormat("Server: {0}. Region: {1} ", PhotonNetwork.ServerAddress, PhotonNetwork.networkingPeer.CloudRegion);
		stringBuilder.AppendFormat("HostType: {0} ", PhotonNetwork.PhotonServerSettings.HostType);
		Debug.Log(stringBuilder.ToString());
	}

	// Token: 0x06003F3E RID: 16190 RVA: 0x0013E88D File Offset: 0x0013CC8D
	public void OnConnectedToPhoton()
	{
		Debug.Log("SupportLogger OnConnectedToPhoton().");
		this.LogBasics();
		if (this.LogTrafficStats)
		{
			PhotonNetwork.NetworkStatisticsEnabled = true;
		}
	}

	// Token: 0x06003F3F RID: 16191 RVA: 0x0013E8B0 File Offset: 0x0013CCB0
	public void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.Log("SupportLogger OnFailedToConnectToPhoton(" + cause + ").");
		this.LogBasics();
	}

	// Token: 0x06003F40 RID: 16192 RVA: 0x0013E8D2 File Offset: 0x0013CCD2
	public void OnJoinedLobby()
	{
		Debug.Log("SupportLogger OnJoinedLobby(" + PhotonNetwork.lobby + ").");
	}

	// Token: 0x06003F41 RID: 16193 RVA: 0x0013E8F0 File Offset: 0x0013CCF0
	public void OnJoinedRoom()
	{
		Debug.Log(string.Concat(new object[]
		{
			"SupportLogger OnJoinedRoom(",
			PhotonNetwork.room,
			"). ",
			PhotonNetwork.lobby,
			" GameServer:",
			PhotonNetwork.ServerAddress
		}));
	}

	// Token: 0x06003F42 RID: 16194 RVA: 0x0013E940 File Offset: 0x0013CD40
	public void OnCreatedRoom()
	{
		Debug.Log(string.Concat(new object[]
		{
			"SupportLogger OnCreatedRoom(",
			PhotonNetwork.room,
			"). ",
			PhotonNetwork.lobby,
			" GameServer:",
			PhotonNetwork.ServerAddress
		}));
	}

	// Token: 0x06003F43 RID: 16195 RVA: 0x0013E98D File Offset: 0x0013CD8D
	public void OnLeftRoom()
	{
		Debug.Log("SupportLogger OnLeftRoom().");
	}

	// Token: 0x06003F44 RID: 16196 RVA: 0x0013E999 File Offset: 0x0013CD99
	public void OnDisconnectedFromPhoton()
	{
		Debug.Log("SupportLogger OnDisconnectedFromPhoton().");
	}

	// Token: 0x0400279F RID: 10143
	public bool LogTrafficStats;
}
