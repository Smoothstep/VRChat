using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BestHTTP.JSON;
using ExitGames.Client.Photon;
using UnityEngine;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000AE8 RID: 2792
public class RoomManager : MonoBehaviour
{
	// Token: 0x06005493 RID: 21651 RVA: 0x001D2F5F File Offset: 0x001D135F
	public static bool isReadyToEnter()
	{
		return RoomManager.enterRoomReady;
	}

	// Token: 0x17000C40 RID: 3136
	// (get) Token: 0x06005494 RID: 21652 RVA: 0x001D2F66 File Offset: 0x001D1366
	public static bool inRoom
	{
		get
		{
			return PhotonNetwork.inRoom;
		}
	}

	// Token: 0x17000C41 RID: 3137
	// (get) Token: 0x06005495 RID: 21653 RVA: 0x001D2F6D File Offset: 0x001D136D
	public static string currentAuthorId
	{
		get
		{
			if (!RoomManager.inRoom)
			{
				return null;
			}
			if (RoomManager.currentRoom == null)
			{
				return null;
			}
			return RoomManager.currentRoom.authorId;
		}
	}

	// Token: 0x17000C42 RID: 3138
	// (get) Token: 0x06005496 RID: 21654 RVA: 0x001D2F94 File Offset: 0x001D1394
	public static string currentOwnerId
	{
		get
		{
			if (!RoomManager.inRoom)
			{
				return null;
			}
			if (!RoomManager.inRoom)
			{
				return null;
			}
			ApiWorld.WorldInstance worldInstance = new ApiWorld.WorldInstance(RoomManager.currentRoom.currentInstanceIdWithTags, 0);
			return worldInstance.GetInstanceCreator();
		}
	}

	// Token: 0x17000C43 RID: 3139
	// (get) Token: 0x06005497 RID: 21655 RVA: 0x001D2FD0 File Offset: 0x001D13D0
	public static Dictionary<string, object> metadata
	{
		get
		{
			return RoomManager.roomMetadata;
		}
	}

	// Token: 0x06005498 RID: 21656 RVA: 0x001D2FD7 File Offset: 0x001D13D7
	private void OnDestroy()
	{
		if (NetworkManager.Instance != null)
		{
			NetworkManager.Instance.OnJoinedRoomEvent -= this.OnJoinedRoom;
			NetworkManager.Instance.OnConnectedToMasterEvent -= this.OnConnectedToMaster;
		}
	}

	// Token: 0x06005499 RID: 21657 RVA: 0x001D3018 File Offset: 0x001D1418
	private IEnumerator Start()
	{
		if (RoomManager._instance != null)
		{
			Debug.LogError("Extra RoomManager Instance");
		}
		RoomManager._instance = this;
		RoomManager.userPortals = new Dictionary<int, PortalInternal>();
		yield return new WaitUntil(() => NetworkManager.Instance != null);
		NetworkManager.Instance.OnJoinedRoomEvent += this.OnJoinedRoom;
		NetworkManager.Instance.OnConnectedToMasterEvent += this.OnConnectedToMaster;
		yield break;
	}

	// Token: 0x0600549A RID: 21658 RVA: 0x001D3034 File Offset: 0x001D1434
	private void Update()
	{
		if (!RoomManager.inRoom)
		{
			return;
		}
		int minute = DateTime.Now.Minute;
		if (RoomManager.lastMetadataFetchMinute == -1 || (minute != RoomManager.lastMetadataFetchMinute && minute % 5 == 0))
		{
			RoomManager.FetchMetadata();
		}
		RoomManager.lastMetadataFetchMinute = minute;
		if (RoomManager._metadataUpdateNotifyNeeded)
		{
			if (RoomManager._metadataWorldId == RoomManager.currentRoom.id)
			{
				Debug.Log("Room metadata updated, notifying listeners.");
				this.TriggerMetadataUpdateInternal(RoomManager.roomMetadata);
				VRC_MetadataListener.TriggerUpdate(RoomManager.roomMetadata);
			}
			RoomManager._metadataUpdateNotifyNeeded = false;
		}
	}

	// Token: 0x0600549B RID: 21659 RVA: 0x001D30CB File Offset: 0x001D14CB
	private void TriggerMetadataUpdateInternal(Dictionary<string, object> data)
	{
		if (RoomManager.metadataUpdateCallbacksInternal != null)
		{
			RoomManager.metadataUpdateCallbacksInternal(data);
		}
	}

	// Token: 0x0600549C RID: 21660 RVA: 0x001D30E4 File Offset: 0x001D14E4
	public static void FetchMetadata()
	{
		if (string.IsNullOrEmpty(RoomManager._metadataWorldId))
		{
			return;
		}
		ApiWorld.FetchData(RoomManager._metadataWorldId, ApiWorld.WorldData.Metadata, delegate(Dictionary<string, object> obj)
		{
			string a = obj["id"] as string;
			if (string.IsNullOrEmpty(RoomManager._metadataWorldId) || a != RoomManager._metadataWorldId)
			{
				return;
			}
			RoomManager.SetMetadata(obj["metadata"] as Dictionary<string, object>);
		}, delegate(string ErrorCode)
		{
			Debug.LogWarning("World Metadata failed to download for world " + ((RoomManager._metadataWorldId == null) ? "(null world id)" : RoomManager._metadataWorldId));
			if (RoomManager.roomMetadata == null)
			{
				RoomManager.SetMetadata(new Dictionary<string, object>());
			}
		});
	}

	// Token: 0x0600549D RID: 21661 RVA: 0x001D3148 File Offset: 0x001D1548
	public static bool EnterWorld(ApiWorld world, string instanceId = "")
	{
		Debug.Log("Entering Room: " + world.name);
		if (VRCFlowNetworkManager.Instance == null || !VRCFlowNetworkManager.Instance.isConnected || !RoomManager.enterRoomReady)
		{
			string message = "Cannot join room. Connection not ready for join operations";
			UserMessage.SetMessage(message);
			Debug.LogError(message);
			return false;
		}
		try
		{
			RoomManager.LockdownOverride = false;
			Analytics.Send(ApiAnalyticEvent.EventType.joinsWorld, world.id, null, null);
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable["scene"] = "Custom";
			hashtable["url"] = world.assetUrl;
			hashtable["name"] = world.name;
			hashtable["blueprint"] = world;
			string[] customRoomPropertiesForLobby = new string[]
			{
				"scene",
				"url",
				"name"
			};
			RoomOptions roomOptions = new RoomOptions();
			roomOptions.IsOpen = true;
			roomOptions.IsVisible = true;
			roomOptions.MaxPlayers = (byte)((world.capacity * 2 >= 255) ? 255 : (world.capacity * 2));
			roomOptions.CustomRoomProperties = hashtable;
			roomOptions.CustomRoomPropertiesForLobby = customRoomPropertiesForLobby;
			List<string> list = (from m in ModerationManager.Instance.GetModerationsOfType(ApiModeration.ModerationType.Kick)
			where m.worldId == world.id
			select m.instanceId).ToList<string>();
			if (world.capacity == 1)
			{
				instanceId = User.CurrentUser.id + ((!ModerationManager.Instance.IsBannedFromPublicOnly(APIUser.CurrentUser.id)) ? string.Empty : ApiWorld.WorldInstance.BuildAccessTags(ApiWorld.WorldInstance.AccessType.FriendsOnly, APIUser.CurrentUser.id));
			}
			else if (string.IsNullOrEmpty(instanceId) || list.Contains(instanceId))
			{
				instanceId = world.GetBestInstance(list, ModerationManager.Instance.IsBannedFromPublicOnly(APIUser.CurrentUser.id)).idWithTags;
			}
			string text = world.id + ":" + instanceId;
			Debug.Log("Joining " + text);
			Debug.Log("Joining or Creating Room: " + world.name);
			bool flag = PhotonNetwork.JoinOrCreateRoom(text, roomOptions, TypedLobby.Default);
			if (!flag)
			{
				RoomManager.currentRoom = null;
				RoomManager.ClearMetadata();
				throw new Exception("JoinOrCreateRoom failed!");
			}
			RoomManager.currentRoom = world;
			RoomManager.currentRoom.currentInstanceIdWithTags = instanceId;
			ApiWorld.WorldInstance worldInstance = new ApiWorld.WorldInstance(instanceId, 0);
			RoomManager.currentRoom.currentInstanceIdOnly = worldInstance.idOnly;
			RoomManager.currentRoom.currentInstanceAccess = worldInstance.GetAccessType();
			Debug.Log("Successfully joined room");
			RoomManager.lastMetadataFetchMinute = -1;
		}
		catch (Exception ex)
		{
			Debug.LogError("Something went entering room:\n" + ex.ToString() + "\n" + ex.StackTrace);
			return false;
		}
		return true;
	}

	// Token: 0x0600549E RID: 21662 RVA: 0x001D34A4 File Offset: 0x001D18A4
	private static void SetMetadata(Dictionary<string, object> data)
	{
		RoomManager.roomMetadata = data;
		int num = (data == null) ? 0 : Json.Encode(data).GetHashCode();
		if (RoomManager.roomMetadataHash != num)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Room metadata was updated, old hash ",
				RoomManager.roomMetadataHash,
				", new hash ",
				num
			}));
			RoomManager.roomMetadataHash = num;
			RoomManager._metadataUpdateNotifyNeeded = true;
		}
		else
		{
			Debug.Log("Room metadata is unchanged, skipping update.");
		}
	}

	// Token: 0x0600549F RID: 21663 RVA: 0x001D352B File Offset: 0x001D192B
	private static void ClearMetadata()
	{
		RoomManager.roomMetadata = null;
		RoomManager.roomMetadataHash = 0;
		RoomManager._metadataWorldId = null;
		RoomManager._metadataUpdateNotifyNeeded = false;
	}

	// Token: 0x060054A0 RID: 21664 RVA: 0x001D3545 File Offset: 0x001D1945
	private void OnJoinedRoom()
	{
	}

	// Token: 0x060054A1 RID: 21665 RVA: 0x001D3547 File Offset: 0x001D1947
	private void OnConnectedToMaster()
	{
		RoomManager.enterRoomReady = true;
	}

	// Token: 0x060054A2 RID: 21666 RVA: 0x001D3550 File Offset: 0x001D1950
	public static void LeaveRoom()
	{
		if (RoomManager.currentRoom != null)
		{
			Analytics.Send(ApiAnalyticEvent.EventType.leavesWorld, RoomManager.currentRoom.id, null, null);
		}
		RoomManager.ClearMetadata();
		RoomManager.currentRoom = null;
		RoomManager.userPortals.Clear();
		if (PhotonNetwork.inRoom)
		{
			RoomManager.enterRoomReady = false;
			if (PhotonNetwork.LeaveRoom())
			{
				Debug.Log("Successfully left room");
			}
			else
			{
				Debug.Log("Unsuccessfully left room");
			}
		}
	}

	// Token: 0x060054A3 RID: 21667 RVA: 0x001D35C9 File Offset: 0x001D19C9
	public static string GetRoomId()
	{
		if (PhotonNetwork.room != null)
		{
			return PhotonNetwork.room.Name;
		}
		return null;
	}

	// Token: 0x17000C44 RID: 3140
	// (get) Token: 0x060054A4 RID: 21668 RVA: 0x001D35E1 File Offset: 0x001D19E1
	public static bool insideLobby
	{
		get
		{
			return PhotonNetwork.insideLobby;
		}
	}

	// Token: 0x060054A5 RID: 21669 RVA: 0x001D35E8 File Offset: 0x001D19E8
	public static bool IsLockdown()
	{
		return RoomManager.currentRoom != null && RoomManager.LockdownOverride;
	}

	// Token: 0x060054A6 RID: 21670 RVA: 0x001D35FB File Offset: 0x001D19FB
	public static void SetLockdown(bool enable)
	{
		RoomManager.LockdownOverride = enable;
	}

	// Token: 0x060054A7 RID: 21671 RVA: 0x001D3603 File Offset: 0x001D1A03
	public static bool IsRoomOwner()
	{
		return RoomManager.currentRoom.authorId == User.CurrentUser.id;
	}

	// Token: 0x060054A8 RID: 21672 RVA: 0x001D3620 File Offset: 0x001D1A20
	public static string GetLaunchUrl(ApiWorld world)
	{
		return "http://www.vrchat.net/launch.php?id=" + world.id;
	}

	// Token: 0x060054A9 RID: 21673 RVA: 0x001D363F File Offset: 0x001D1A3F
	public static void SetUserPortalForbid(bool flag)
	{
		RoomManager.userPortalsForbidden = flag;
	}

	// Token: 0x060054AA RID: 21674 RVA: 0x001D3647 File Offset: 0x001D1A47
	public static bool IsUserPortalForbidden()
	{
		return RoomManager.userPortalsForbidden;
	}

	// Token: 0x060054AB RID: 21675 RVA: 0x001D3650 File Offset: 0x001D1A50
	public static void PortalCreated(PortalInternal p)
	{
		PortalInternal portalInternal = null;
		RoomManager.userPortals.TryGetValue(p.CreatorId, out portalInternal);
		if (portalInternal != null)
		{
			if (portalInternal == p)
			{
				return;
			}
			portalInternal.ForceShutdown();
		}
		RoomManager.userPortals.Remove(p.CreatorId);
		RoomManager.userPortals.Add(p.CreatorId, p);
	}

	// Token: 0x060054AC RID: 21676 RVA: 0x001D36B3 File Offset: 0x001D1AB3
	public static void PortalShutdown(PortalInternal p)
	{
		RoomManager.userPortals.Remove(p.CreatorId);
	}

	// Token: 0x060054AD RID: 21677 RVA: 0x001D36C6 File Offset: 0x001D1AC6
	public static void SetWorldIdForMetadata(string id)
	{
		if (id != RoomManager._metadataWorldId)
		{
			RoomManager.ClearMetadata();
		}
		RoomManager._metadataWorldId = id;
	}

	// Token: 0x04003BAC RID: 15276
	private static bool LockdownOverride = false;

	// Token: 0x04003BAD RID: 15277
	private static bool enterRoomReady = false;

	// Token: 0x04003BAE RID: 15278
	public static ApiWorld currentRoom;

	// Token: 0x04003BAF RID: 15279
	public static bool isTestRoom = false;

	// Token: 0x04003BB0 RID: 15280
	private static bool userPortalsForbidden;

	// Token: 0x04003BB1 RID: 15281
	private static Dictionary<int, PortalInternal> userPortals = new Dictionary<int, PortalInternal>();

	// Token: 0x04003BB2 RID: 15282
	private static Dictionary<string, object> roomMetadata = null;

	// Token: 0x04003BB3 RID: 15283
	private static int roomMetadataHash = 0;

	// Token: 0x04003BB4 RID: 15284
	public static VRC_MetadataListener.MetadataCallback metadataUpdateCallbacksInternal;

	// Token: 0x04003BB5 RID: 15285
	public bool RealTimeList = true;

	// Token: 0x04003BB6 RID: 15286
	public static float roomFetchPeriod = 20f;

	// Token: 0x04003BB7 RID: 15287
	public static float minFetchPeriod = 5f;

	// Token: 0x04003BB8 RID: 15288
	private static RoomManager _instance = null;

	// Token: 0x04003BB9 RID: 15289
	private static int lastMetadataFetchMinute = -1;

	// Token: 0x04003BBA RID: 15290
	private static string _metadataWorldId = null;

	// Token: 0x04003BBB RID: 15291
	private static bool _metadataUpdateNotifyNeeded = false;

	// Token: 0x02000AE9 RID: 2793
	private enum RegionServerConnectionStatus
	{
		// Token: 0x04003BC0 RID: 15296
		None,
		// Token: 0x04003BC1 RID: 15297
		Connecting,
		// Token: 0x04003BC2 RID: 15298
		Success,
		// Token: 0x04003BC3 RID: 15299
		Failure
	}
}
