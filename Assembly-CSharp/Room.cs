using System;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x0200076C RID: 1900
public class Room : RoomInfo
{
	// Token: 0x06003DFE RID: 15870 RVA: 0x00138E80 File Offset: 0x00137280
	internal Room(string roomName, RoomOptions options) : base(roomName, null)
	{
		if (options == null)
		{
			options = new RoomOptions();
		}
		this.visibleField = options.IsVisible;
		this.openField = options.IsOpen;
		this.maxPlayersField = options.MaxPlayers;
		this.autoCleanUpField = false;
		base.InternalCacheProperties(options.CustomRoomProperties);
		this.PropertiesListedInLobby = options.CustomRoomPropertiesForLobby;
	}

	// Token: 0x170009D9 RID: 2521
	// (get) Token: 0x06003DFF RID: 15871 RVA: 0x00138EE5 File Offset: 0x001372E5
	// (set) Token: 0x06003E00 RID: 15872 RVA: 0x00138EED File Offset: 0x001372ED
	public new string Name
	{
		get
		{
			return this.nameField;
		}
		internal set
		{
			this.nameField = value;
		}
	}

	// Token: 0x170009DA RID: 2522
	// (get) Token: 0x06003E01 RID: 15873 RVA: 0x00138EF6 File Offset: 0x001372F6
	// (set) Token: 0x06003E02 RID: 15874 RVA: 0x00138F00 File Offset: 0x00137300
	public new bool IsOpen
	{
		get
		{
			return this.openField;
		}
		set
		{
			if (!this.Equals(PhotonNetwork.room))
			{
				Debug.LogWarning("Can't set open when not in that room.");
			}
			if (value != this.openField && !PhotonNetwork.offlineMode)
			{
				PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(new Hashtable
				{
					{
						253,
						value
					}
				}, null, false);
			}
			this.openField = value;
		}
	}

	// Token: 0x170009DB RID: 2523
	// (get) Token: 0x06003E03 RID: 15875 RVA: 0x00138F6E File Offset: 0x0013736E
	// (set) Token: 0x06003E04 RID: 15876 RVA: 0x00138F78 File Offset: 0x00137378
	public new bool IsVisible
	{
		get
		{
			return this.visibleField;
		}
		set
		{
			if (!this.Equals(PhotonNetwork.room))
			{
				Debug.LogWarning("Can't set visible when not in that room.");
			}
			if (value != this.visibleField && !PhotonNetwork.offlineMode)
			{
				PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(new Hashtable
				{
					{
						254,
						value
					}
				}, null, false);
			}
			this.visibleField = value;
		}
	}

	// Token: 0x170009DC RID: 2524
	// (get) Token: 0x06003E05 RID: 15877 RVA: 0x00138FE6 File Offset: 0x001373E6
	// (set) Token: 0x06003E06 RID: 15878 RVA: 0x00138FEE File Offset: 0x001373EE
	public string[] PropertiesListedInLobby { get; private set; }

	// Token: 0x170009DD RID: 2525
	// (get) Token: 0x06003E07 RID: 15879 RVA: 0x00138FF7 File Offset: 0x001373F7
	public bool AutoCleanUp
	{
		get
		{
			return this.autoCleanUpField;
		}
	}

	// Token: 0x170009DE RID: 2526
	// (get) Token: 0x06003E08 RID: 15880 RVA: 0x00138FFF File Offset: 0x001373FF
	// (set) Token: 0x06003E09 RID: 15881 RVA: 0x00139008 File Offset: 0x00137408
	public new int MaxPlayers
	{
		get
		{
			return (int)this.maxPlayersField;
		}
		set
		{
			if (!this.Equals(PhotonNetwork.room))
			{
				Debug.LogWarning("Can't set MaxPlayers when not in that room.");
			}
			if (value > 255)
			{
				Debug.LogWarning("Can't set Room.MaxPlayers to: " + value + ". Using max value: 255.");
				value = 255;
			}
			if (value != (int)this.maxPlayersField && !PhotonNetwork.offlineMode)
			{
				PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(new Hashtable
				{
					{
						byte.MaxValue,
						(byte)value
					}
				}, null, false);
			}
			this.maxPlayersField = (byte)value;
		}
	}

	// Token: 0x170009DF RID: 2527
	// (get) Token: 0x06003E0A RID: 15882 RVA: 0x001390A4 File Offset: 0x001374A4
	public new int PlayerCount
	{
		get
		{
			if (PhotonNetwork.playerList != null)
			{
				return PhotonNetwork.playerList.Length;
			}
			return 0;
		}
	}

	// Token: 0x170009E0 RID: 2528
	// (get) Token: 0x06003E0B RID: 15883 RVA: 0x001390B9 File Offset: 0x001374B9
	public string[] ExpectedUsers
	{
		get
		{
			return this.expectedUsersField;
		}
	}

	// Token: 0x170009E1 RID: 2529
	// (get) Token: 0x06003E0C RID: 15884 RVA: 0x001390C1 File Offset: 0x001374C1
	// (set) Token: 0x06003E0D RID: 15885 RVA: 0x001390C9 File Offset: 0x001374C9
	protected internal int MasterClientId
	{
		get
		{
			return this.masterClientIdField;
		}
		set
		{
			this.masterClientIdField = value;
		}
	}

	// Token: 0x06003E0E RID: 15886 RVA: 0x001390D4 File Offset: 0x001374D4
	public void SetCustomProperties(Hashtable propertiesToSet, Hashtable expectedValues = null, bool webForward = false)
	{
		if (propertiesToSet == null)
		{
			return;
		}
		Hashtable hashtable = propertiesToSet.StripToStringKeys();
		Hashtable hashtable2 = expectedValues.StripToStringKeys();
		bool flag = hashtable2 == null || hashtable2.Count == 0;
		if (PhotonNetwork.offlineMode || flag)
		{
			base.CustomProperties.Merge(hashtable);
			base.CustomProperties.StripKeysWithNullValues();
		}
		if (!PhotonNetwork.offlineMode)
		{
			PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable, hashtable2, webForward);
		}
		if (PhotonNetwork.offlineMode || flag)
		{
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCustomRoomPropertiesChanged, new object[]
			{
				hashtable
			});
		}
	}

	// Token: 0x06003E0F RID: 15887 RVA: 0x0013916C File Offset: 0x0013756C
	public void SetPropertiesListedInLobby(string[] propsListedInLobby)
	{
		Hashtable hashtable = new Hashtable();
		hashtable[250] = propsListedInLobby;
		PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable, null, false);
		this.PropertiesListedInLobby = propsListedInLobby;
	}

	// Token: 0x06003E10 RID: 15888 RVA: 0x001391A8 File Offset: 0x001375A8
	public void ClearExpectedUsers()
	{
		Hashtable hashtable = new Hashtable();
		hashtable[247] = new string[0];
		Hashtable hashtable2 = new Hashtable();
		hashtable2[247] = this.ExpectedUsers;
		PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable, hashtable2, false);
	}

	// Token: 0x06003E11 RID: 15889 RVA: 0x001391FC File Offset: 0x001375FC
	public override string ToString()
	{
		return string.Format("Room: '{0}' {1},{2} {4}/{3} players.", new object[]
		{
			this.nameField,
			(!this.visibleField) ? "hidden" : "visible",
			(!this.openField) ? "closed" : "open",
			this.maxPlayersField,
			this.PlayerCount
		});
	}

	// Token: 0x06003E12 RID: 15890 RVA: 0x00139278 File Offset: 0x00137678
	public new string ToStringFull()
	{
		return string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", new object[]
		{
			this.nameField,
			(!this.visibleField) ? "hidden" : "visible",
			(!this.openField) ? "closed" : "open",
			this.maxPlayersField,
			this.PlayerCount,
			base.CustomProperties.ToStringFull()
		});
	}

	// Token: 0x170009E2 RID: 2530
	// (get) Token: 0x06003E13 RID: 15891 RVA: 0x00139302 File Offset: 0x00137702
	// (set) Token: 0x06003E14 RID: 15892 RVA: 0x0013930A File Offset: 0x0013770A
	[Obsolete("Please use Name (updated case for naming).")]
	public new string name
	{
		get
		{
			return this.Name;
		}
		internal set
		{
			this.Name = value;
		}
	}

	// Token: 0x170009E3 RID: 2531
	// (get) Token: 0x06003E15 RID: 15893 RVA: 0x00139313 File Offset: 0x00137713
	// (set) Token: 0x06003E16 RID: 15894 RVA: 0x0013931B File Offset: 0x0013771B
	[Obsolete("Please use IsOpen (updated case for naming).")]
	public new bool open
	{
		get
		{
			return this.IsOpen;
		}
		set
		{
			this.IsOpen = value;
		}
	}

	// Token: 0x170009E4 RID: 2532
	// (get) Token: 0x06003E17 RID: 15895 RVA: 0x00139324 File Offset: 0x00137724
	// (set) Token: 0x06003E18 RID: 15896 RVA: 0x0013932C File Offset: 0x0013772C
	[Obsolete("Please use IsVisible (updated case for naming).")]
	public new bool visible
	{
		get
		{
			return this.IsVisible;
		}
		set
		{
			this.IsVisible = value;
		}
	}

	// Token: 0x170009E5 RID: 2533
	// (get) Token: 0x06003E19 RID: 15897 RVA: 0x00139335 File Offset: 0x00137735
	// (set) Token: 0x06003E1A RID: 15898 RVA: 0x0013933D File Offset: 0x0013773D
	[Obsolete("Please use PropertiesListedInLobby (updated case for naming).")]
	public string[] propertiesListedInLobby
	{
		get
		{
			return this.PropertiesListedInLobby;
		}
		private set
		{
			this.PropertiesListedInLobby = value;
		}
	}

	// Token: 0x170009E6 RID: 2534
	// (get) Token: 0x06003E1B RID: 15899 RVA: 0x00139346 File Offset: 0x00137746
	[Obsolete("Please use AutoCleanUp (updated case for naming).")]
	public bool autoCleanUp
	{
		get
		{
			return this.AutoCleanUp;
		}
	}

	// Token: 0x170009E7 RID: 2535
	// (get) Token: 0x06003E1C RID: 15900 RVA: 0x0013934E File Offset: 0x0013774E
	// (set) Token: 0x06003E1D RID: 15901 RVA: 0x00139356 File Offset: 0x00137756
	[Obsolete("Please use MaxPlayers (updated case for naming).")]
	public new int maxPlayers
	{
		get
		{
			return this.MaxPlayers;
		}
		set
		{
			this.MaxPlayers = value;
		}
	}

	// Token: 0x170009E8 RID: 2536
	// (get) Token: 0x06003E1E RID: 15902 RVA: 0x0013935F File Offset: 0x0013775F
	[Obsolete("Please use PlayerCount (updated case for naming).")]
	public new int playerCount
	{
		get
		{
			return this.PlayerCount;
		}
	}

	// Token: 0x170009E9 RID: 2537
	// (get) Token: 0x06003E1F RID: 15903 RVA: 0x00139367 File Offset: 0x00137767
	[Obsolete("Please use ExpectedUsers (updated case for naming).")]
	public string[] expectedUsers
	{
		get
		{
			return this.ExpectedUsers;
		}
	}

	// Token: 0x170009EA RID: 2538
	// (get) Token: 0x06003E20 RID: 15904 RVA: 0x0013936F File Offset: 0x0013776F
	// (set) Token: 0x06003E21 RID: 15905 RVA: 0x00139377 File Offset: 0x00137777
	[Obsolete("Please use MasterClientId (updated case for naming).")]
	protected internal int masterClientId
	{
		get
		{
			return this.MasterClientId;
		}
		set
		{
			this.MasterClientId = value;
		}
	}
}
