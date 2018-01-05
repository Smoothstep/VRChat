using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon;
using UnityEngine;

namespace ExitGames.UtilityScripts
{
	// Token: 0x02000792 RID: 1938
	public class PlayerRoomIndexing : PunBehaviour
	{
		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x06003ED7 RID: 16087 RVA: 0x0013CF99 File Offset: 0x0013B399
		public int[] PlayerIds
		{
			get
			{
				return this._playerIds;
			}
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x0013CFA1 File Offset: 0x0013B3A1
		public void Awake()
		{
			if (PlayerRoomIndexing.instance != null)
			{
				Debug.LogError("Existing instance of PlayerRoomIndexing found. Only One instance is required at the most. Please correct and have only one at any time.");
			}
			PlayerRoomIndexing.instance = this;
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x0013CFC3 File Offset: 0x0013B3C3
		public override void OnJoinedRoom()
		{
			if (PhotonNetwork.isMasterClient)
			{
				this.AssignIndex(PhotonNetwork.player);
			}
			else
			{
				this.RefreshData();
			}
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x0013CFE5 File Offset: 0x0013B3E5
		public override void OnLeftRoom()
		{
			this.RefreshData();
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x0013CFED File Offset: 0x0013B3ED
		public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
		{
			if (PhotonNetwork.isMasterClient)
			{
				this.AssignIndex(newPlayer);
			}
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x0013D000 File Offset: 0x0013B400
		public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
			if (PhotonNetwork.isMasterClient)
			{
				this.UnAssignIndex(otherPlayer);
			}
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x0013D013 File Offset: 0x0013B413
		public override void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
		{
			if (propertiesThatChanged.ContainsKey("PlayerIndexes"))
			{
				this.RefreshData();
			}
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x0013D02B File Offset: 0x0013B42B
		public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
		{
			if (PhotonNetwork.isMasterClient)
			{
				this.SanitizeIndexing();
			}
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x0013D03D File Offset: 0x0013B43D
		public int GetRoomIndex(PhotonPlayer player)
		{
			if (this._indexesLUT != null && this._indexesLUT.ContainsKey(player.ID))
			{
				return this._indexesLUT[player.ID];
			}
			return -1;
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x0013D074 File Offset: 0x0013B474
		private void SanitizeIndexing()
		{
			if (!PhotonNetwork.isMasterClient)
			{
				return;
			}
			if (PhotonNetwork.room == null)
			{
				return;
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			if (PhotonNetwork.room.CustomProperties.TryGetValue("PlayerIndexes", out this._indexes))
			{
				dictionary = (this._indexes as Dictionary<int, int>);
			}
			if (dictionary.Count != PhotonNetwork.room.PlayerCount)
			{
				foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
				{
					if (!dictionary.ContainsKey(photonPlayer.ID))
					{
						this.AssignIndex(photonPlayer);
					}
				}
			}
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x0013D114 File Offset: 0x0013B514
		private void RefreshData()
		{
			if (PhotonNetwork.room != null)
			{
				this._playerIds = new int[PhotonNetwork.room.MaxPlayers];
				if (PhotonNetwork.room.CustomProperties.TryGetValue("PlayerIndexes", out this._indexes))
				{
					this._indexesLUT = (this._indexes as Dictionary<int, int>);
					foreach (KeyValuePair<int, int> keyValuePair in this._indexesLUT)
					{
						this._p = PhotonPlayer.Find(keyValuePair.Key);
						this._playerIds[keyValuePair.Value] = this._p.ID;
					}
				}
			}
			else
			{
				this._playerIds = new int[0];
			}
			if (this.OnRoomIndexingChanged != null)
			{
				this.OnRoomIndexingChanged();
			}
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x0013D20C File Offset: 0x0013B60C
		private void AssignIndex(PhotonPlayer player)
		{
			if (PhotonNetwork.room.CustomProperties.TryGetValue("PlayerIndexes", out this._indexes))
			{
				this._indexesLUT = (this._indexes as Dictionary<int, int>);
			}
			else
			{
				this._indexesLUT = new Dictionary<int, int>();
			}
			List<bool> list = new List<bool>(new bool[PhotonNetwork.room.MaxPlayers]);
			foreach (KeyValuePair<int, int> keyValuePair in this._indexesLUT)
			{
				list[keyValuePair.Value] = true;
			}
			this._indexesLUT[player.ID] = Mathf.Max(0, list.IndexOf(false));
			PhotonNetwork.room.SetCustomProperties(new Hashtable
			{
				{
					"PlayerIndexes",
					this._indexesLUT
				}
			}, null, false);
			this.RefreshData();
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x0013D30C File Offset: 0x0013B70C
		private void UnAssignIndex(PhotonPlayer player)
		{
			if (PhotonNetwork.room.CustomProperties.TryGetValue("PlayerIndexes", out this._indexes))
			{
				this._indexesLUT = (this._indexes as Dictionary<int, int>);
				this._indexesLUT.Remove(player.ID);
				PhotonNetwork.room.SetCustomProperties(new Hashtable
				{
					{
						"PlayerIndexes",
						this._indexesLUT
					}
				}, null, false);
			}
			this.RefreshData();
		}

		// Token: 0x04002775 RID: 10101
		public static PlayerRoomIndexing instance;

		// Token: 0x04002776 RID: 10102
		public PlayerRoomIndexing.RoomIndexingChanged OnRoomIndexingChanged;

		// Token: 0x04002777 RID: 10103
		public const string RoomPlayerIndexedProp = "PlayerIndexes";

		// Token: 0x04002778 RID: 10104
		private int[] _playerIds;

		// Token: 0x04002779 RID: 10105
		private object _indexes;

		// Token: 0x0400277A RID: 10106
		private Dictionary<int, int> _indexesLUT;

		// Token: 0x0400277B RID: 10107
		private List<bool> _indexesPool;

		// Token: 0x0400277C RID: 10108
		private PhotonPlayer _p;

		// Token: 0x02000793 RID: 1939
		// (Invoke) Token: 0x06003EE5 RID: 16101
		public delegate void RoomIndexingChanged();
	}
}
