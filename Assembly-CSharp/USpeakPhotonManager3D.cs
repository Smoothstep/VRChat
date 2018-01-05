using System;
using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine;

// Token: 0x020009E4 RID: 2532
public class USpeakPhotonManager3D : Photon.MonoBehaviour
{
	// Token: 0x06004CEE RID: 19694 RVA: 0x0019C184 File Offset: 0x0019A584
	private void Awake()
	{
		if (!PhotonNetwork.connected)
		{
			base.enabled = false;
			return;
		}
		USpeakPhotonManager3D.TNetSpeakerMap.Clear();
	}

	// Token: 0x06004CEF RID: 19695 RVA: 0x0019C1A4 File Offset: 0x0019A5A4
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => NetworkManager.Instance != null);
		NetworkManager.Instance.OnLeftRoomEvent += this.OnLeftRoom;
		yield break;
	}

	// Token: 0x06004CF0 RID: 19696 RVA: 0x0019C1BF File Offset: 0x0019A5BF
	private void OnDestroy()
	{
		if (NetworkManager.Instance != null)
		{
			NetworkManager.Instance.OnLeftRoomEvent -= this.OnLeftRoom;
		}
	}

	// Token: 0x06004CF1 RID: 19697 RVA: 0x0019C1E7 File Offset: 0x0019A5E7
	private void OnLeftRoom()
	{
		USpeakPhotonManager3D.TNetSpeakerMap.Clear();
	}

	// Token: 0x06004CF2 RID: 19698 RVA: 0x0019C1F3 File Offset: 0x0019A5F3
	private void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		USpeakPhotonManager3D.TNetSpeakerMap.Remove(player);
	}

	// Token: 0x06004CF3 RID: 19699 RVA: 0x0019C201 File Offset: 0x0019A601
	public static void MutePlayer(PhotonPlayer player)
	{
		if (USpeakPhotonManager3D.TNetSpeakerMap.ContainsKey(player))
		{
			USpeakPhotonManager3D.TNetSpeakerMap[player].Mute = false;
		}
	}

	// Token: 0x06004CF4 RID: 19700 RVA: 0x0019C224 File Offset: 0x0019A624
	public static void UnmutePlayer(PhotonPlayer player)
	{
		if (USpeakPhotonManager3D.TNetSpeakerMap.ContainsKey(player))
		{
			USpeakPhotonManager3D.TNetSpeakerMap[player].Mute = true;
		}
	}

	// Token: 0x06004CF5 RID: 19701 RVA: 0x0019C248 File Offset: 0x0019A648
	public static void MuteAllPlayers()
	{
		foreach (KeyValuePair<PhotonPlayer, USpeaker> keyValuePair in USpeakPhotonManager3D.TNetSpeakerMap)
		{
			USpeakPhotonManager3D.MutePlayer(keyValuePair.Key);
		}
	}

	// Token: 0x06004CF6 RID: 19702 RVA: 0x0019C2A8 File Offset: 0x0019A6A8
	public static void UnmuteAllPlayers()
	{
		foreach (KeyValuePair<PhotonPlayer, USpeaker> keyValuePair in USpeakPhotonManager3D.TNetSpeakerMap)
		{
			USpeakPhotonManager3D.UnmutePlayer(keyValuePair.Key);
		}
	}

	// Token: 0x040034EF RID: 13551
	public static Dictionary<PhotonPlayer, USpeaker> TNetSpeakerMap = new Dictionary<PhotonPlayer, USpeaker>();
}
