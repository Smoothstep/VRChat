using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Photon;
using UnityEngine;
using VRC.Core;

namespace VRC
{
	// Token: 0x02000AC6 RID: 2758
	public class PlayerManager : Photon.MonoBehaviour
	{
		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x060053C1 RID: 21441 RVA: 0x001CE37C File Offset: 0x001CC77C
		public static PlayerManager Instance
		{
			get
			{
				if (PlayerManager.mInstance == null)
				{
					PlayerManager.mInstance = new GameObject
					{
						name = "PlayerManager"
					}.AddComponent<PlayerManager>();
					PlayerManager.mInstance.Init();
				}
				return PlayerManager.mInstance;
			}
		}

		// Token: 0x060053C2 RID: 21442 RVA: 0x001CE3C4 File Offset: 0x001CC7C4
		public static void MonitorPlayer(Player p)
		{
			PlayerManager.Instance.StartCoroutine(PlayerManager.Instance.MonitorPlayerEnumerator(p));
		}

		// Token: 0x060053C3 RID: 21443 RVA: 0x001CE3DC File Offset: 0x001CC7DC
		private IEnumerator MonitorPlayerEnumerator(Player p)
		{
			object obj = this.mPlayers;
			lock (obj)
			{
				if (this.mPlayers.Contains(p))
				{
					yield break;
				}
				this.mPlayers.Add(p);
			}
			yield return Network.CheckReady(p.gameObject);
			if (PlayerManager.f__mg0 == null)
			{
				PlayerManager.f__mg0 = new Func<bool>(Network.Get_IsNetworkSettled);
			}
			yield return new WaitUntil(PlayerManager.f__mg0);
			NetworkManager.Instance.OnVRCPlayerJoined(p);
			yield break;
		}

		// Token: 0x060053C4 RID: 21444 RVA: 0x001CE400 File Offset: 0x001CC800
		public static Player[] GetAllPlayers()
		{
			object obj = PlayerManager.Instance.mPlayers;
			Player[] result;
			lock (obj)
			{
				result = PlayerManager.Instance.mPlayers.ToArray();
			}
			return result;
		}

		// Token: 0x060053C5 RID: 21445 RVA: 0x001CE44C File Offset: 0x001CC84C
		public static Player GetPlayer(int playerId)
		{
			object obj = PlayerManager.Instance.mPlayers;
			Player result;
			lock (obj)
			{
				result = PlayerManager.Instance.mPlayers.FirstOrDefault((Player p) => p != null && p.playerApi.playerId == playerId);
			}
			return result;
		}

		// Token: 0x060053C6 RID: 21446 RVA: 0x001CE4B0 File Offset: 0x001CC8B0
		public static Player GetPlayer(PhotonPlayer photonPlayer)
		{
			if (photonPlayer == null)
			{
				return null;
			}
			object obj = PlayerManager.Instance.mPlayers;
			Player result;
			lock (obj)
			{
				result = PlayerManager.Instance.mPlayers.FirstOrDefault((Player p) => p != null && p.GetPhotonPlayerId() == photonPlayer.ID);
			}
			return result;
		}

		// Token: 0x060053C7 RID: 21447 RVA: 0x001CE524 File Offset: 0x001CC924
		public static Player GetPlayer(string userId)
		{
			object obj = PlayerManager.Instance.mPlayers;
			Player result;
			lock (obj)
			{
				result = PlayerManager.Instance.mPlayers.FirstOrDefault((Player p) => p != null && p.userId == userId);
			}
			return result;
		}

		// Token: 0x060053C8 RID: 21448 RVA: 0x001CE588 File Offset: 0x001CC988
		public static Player GetCurrentPlayer()
		{
			return PlayerManager.Instance.mCurrentPlayer;
		}

		// Token: 0x060053C9 RID: 21449 RVA: 0x001CE594 File Offset: 0x001CC994
		public static float GetDistanceBetween(Player p1, Player p2)
		{
			return Vector3.Distance(p1.transform.position, p2.transform.position);
		}

		// Token: 0x060053CA RID: 21450 RVA: 0x001CE5B1 File Offset: 0x001CC9B1
		public static List<Player> GetAllPlayersByDistance()
		{
			return (from p in PlayerManager.Instance.mPlayers
			orderby Vector3.Distance(p.transform.position, PlayerManager.Instance.mCurrentPlayer.transform.position)
			select p).ToList<Player>();
		}

		// Token: 0x060053CB RID: 21451 RVA: 0x001CE5E4 File Offset: 0x001CC9E4
		public static List<Player> GetAllPlayersWithinRange(Vector3 pos, float radius)
		{
			return (from p in PlayerManager.Instance.mPlayers
			where Vector3.Distance(p.transform.position, pos) <= radius
			orderby Vector3.Distance(p.transform.position, pos)
			select p).ToList<Player>();
		}

		// Token: 0x060053CC RID: 21452 RVA: 0x001CE638 File Offset: 0x001CCA38
		public static void FetchUsersInWorldInstance(Action<List<APIUser>> successCallback, Action<string> errorCallback)
		{
			if (RoomManager.currentRoom.id.StartsWith("local:"))
			{
				List<APIUser> list = new List<APIUser>();
				list.Add(APIUser.CurrentUser);
				if (successCallback != null)
				{
					successCallback(list);
				}
				return;
			}
			APIUser.FetchUsersInWorldInstance(RoomManager.currentRoom.id, RoomManager.currentRoom.currentInstanceIdWithTags, delegate(List<APIUser> users)
			{
				PlayerManager.lastUserListFetch = new List<APIUser>(users);
				if (successCallback != null)
				{
					successCallback(users);
				}
			}, errorCallback);
		}

		// Token: 0x060053CD RID: 21453 RVA: 0x001CE6BA File Offset: 0x001CCABA
		public static List<APIUser> GetLastUserListFetch()
		{
			if (PlayerManager.lastUserListFetch == null)
			{
				PlayerManager.lastUserListFetch = new List<APIUser>();
			}
			return PlayerManager.lastUserListFetch;
		}

		// Token: 0x060053CE RID: 21454 RVA: 0x001CE6D8 File Offset: 0x001CCAD8
		public static List<int> GetPlayersWithTag(string tagName, string tagValue, bool inverted)
		{
			List<int> list = new List<int>();
			foreach (Player player in PlayerManager.GetAllPlayers())
			{
				VRCPlayer component = player.GetComponent<VRCPlayer>();
				string playerTag = component.GetPlayerTag(tagName);
				if (!inverted)
				{
					if (playerTag != null && tagValue == playerTag)
					{
						list.Add(component.apiPlayer.playerId);
					}
				}
				else if (playerTag == null || tagValue != playerTag)
				{
					list.Add(component.apiPlayer.playerId);
				}
			}
			return list;
		}

		// Token: 0x060053CF RID: 21455 RVA: 0x001CE774 File Offset: 0x001CCB74
		private IEnumerator Start()
		{
			yield return new WaitUntil(() => NetworkManager.Instance != null);
			NetworkManager.Instance.OnPhotonPlayerDisconnectedEvent += this.OnPhotonPlayerDisconnected;
			yield break;
		}

		// Token: 0x060053D0 RID: 21456 RVA: 0x001CE78F File Offset: 0x001CCB8F
		private void OnDestroy()
		{
			if (NetworkManager.Instance != null)
			{
				NetworkManager.Instance.OnPhotonPlayerDisconnectedEvent -= this.OnPhotonPlayerDisconnected;
			}
		}

		// Token: 0x060053D1 RID: 21457 RVA: 0x001CE7B7 File Offset: 0x001CCBB7
		private void Init()
		{
			this.mPlayers = new List<Player>();
		}

		// Token: 0x060053D2 RID: 21458 RVA: 0x001CE7C4 File Offset: 0x001CCBC4
		private void Update()
		{
			if (this.mCurrentPlayer == null && VRCPlayer.Instance != null)
			{
				this.SetCurrentPlayer();
			}
		}

		// Token: 0x060053D3 RID: 21459 RVA: 0x001CE7ED File Offset: 0x001CCBED
		private void SetCurrentPlayer()
		{
			this.mCurrentPlayer = VRCPlayer.Instance.gameObject.GetComponent<Player>();
		}

		// Token: 0x060053D4 RID: 21460 RVA: 0x001CE804 File Offset: 0x001CCC04
		private void RemovePlayer(Player p)
		{
			object obj = PlayerManager.Instance.mPlayers;
			lock (obj)
			{
				this.mPlayers.Remove(p);
			}
		}

		// Token: 0x060053D5 RID: 21461 RVA: 0x001CE84C File Offset: 0x001CCC4C
		private void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
			Player player = PlayerManager.GetPlayer(otherPlayer);
			object obj = PlayerManager.Instance.mPlayers;
			lock (obj)
			{
				if (!this.mPlayers.Contains(player))
				{
					return;
				}
				this.mPlayers.Remove(player);
			}
			if (PhotonNetwork.isMasterClient)
			{
				PhotonNetwork.RemoveRPCs(otherPlayer);
			}
			NetworkManager.Instance.OnVRCPlayerLeft(player);
		}

		// Token: 0x04003B18 RID: 15128
		private static List<APIUser> lastUserListFetch;

		// Token: 0x04003B19 RID: 15129
		private static PlayerManager mInstance;

		// Token: 0x04003B1A RID: 15130
		private Player mCurrentPlayer;

		// Token: 0x04003B1B RID: 15131
		private List<Player> mPlayers;

		// Token: 0x04003B1C RID: 15132
		[CompilerGenerated]
		private static Func<bool> f__mg0;
	}
}
