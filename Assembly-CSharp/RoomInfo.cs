using System;
using ExitGames.Client.Photon;

// Token: 0x0200076D RID: 1901
public class RoomInfo
{
	// Token: 0x06003E22 RID: 15906 RVA: 0x00138A4F File Offset: 0x00136E4F
	protected internal RoomInfo(string roomName, Hashtable properties)
	{
		this.InternalCacheProperties(properties);
		this.nameField = roomName;
	}

	// Token: 0x170009EB RID: 2539
	// (get) Token: 0x06003E23 RID: 15907 RVA: 0x00138A89 File Offset: 0x00136E89
	// (set) Token: 0x06003E24 RID: 15908 RVA: 0x00138A91 File Offset: 0x00136E91
	public bool removedFromList { get; internal set; }

	// Token: 0x170009EC RID: 2540
	// (get) Token: 0x06003E25 RID: 15909 RVA: 0x00138A9A File Offset: 0x00136E9A
	// (set) Token: 0x06003E26 RID: 15910 RVA: 0x00138AA2 File Offset: 0x00136EA2
	protected internal bool serverSideMasterClient { get; private set; }

	// Token: 0x170009ED RID: 2541
	// (get) Token: 0x06003E27 RID: 15911 RVA: 0x00138AAB File Offset: 0x00136EAB
	public Hashtable CustomProperties
	{
		get
		{
			return this.customPropertiesField;
		}
	}

	// Token: 0x170009EE RID: 2542
	// (get) Token: 0x06003E28 RID: 15912 RVA: 0x00138AB3 File Offset: 0x00136EB3
	public string Name
	{
		get
		{
			return this.nameField;
		}
	}

	// Token: 0x170009EF RID: 2543
	// (get) Token: 0x06003E29 RID: 15913 RVA: 0x00138ABB File Offset: 0x00136EBB
	// (set) Token: 0x06003E2A RID: 15914 RVA: 0x00138AC3 File Offset: 0x00136EC3
	public int PlayerCount { get; private set; }

	// Token: 0x170009F0 RID: 2544
	// (get) Token: 0x06003E2B RID: 15915 RVA: 0x00138ACC File Offset: 0x00136ECC
	// (set) Token: 0x06003E2C RID: 15916 RVA: 0x00138AD4 File Offset: 0x00136ED4
	public bool IsLocalClientInside { get; set; }

	// Token: 0x170009F1 RID: 2545
	// (get) Token: 0x06003E2D RID: 15917 RVA: 0x00138ADD File Offset: 0x00136EDD
	public byte MaxPlayers
	{
		get
		{
			return this.maxPlayersField;
		}
	}

	// Token: 0x170009F2 RID: 2546
	// (get) Token: 0x06003E2E RID: 15918 RVA: 0x00138AE5 File Offset: 0x00136EE5
	public bool IsOpen
	{
		get
		{
			return this.openField;
		}
	}

	// Token: 0x170009F3 RID: 2547
	// (get) Token: 0x06003E2F RID: 15919 RVA: 0x00138AED File Offset: 0x00136EED
	public bool IsVisible
	{
		get
		{
			return this.visibleField;
		}
	}

	// Token: 0x06003E30 RID: 15920 RVA: 0x00138AF8 File Offset: 0x00136EF8
	public override bool Equals(object other)
	{
		RoomInfo roomInfo = other as RoomInfo;
		return roomInfo != null && this.Name.Equals(roomInfo.nameField);
	}

	// Token: 0x06003E31 RID: 15921 RVA: 0x00138B26 File Offset: 0x00136F26
	public override int GetHashCode()
	{
		return this.nameField.GetHashCode();
	}

	// Token: 0x06003E32 RID: 15922 RVA: 0x00138B34 File Offset: 0x00136F34
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

	// Token: 0x06003E33 RID: 15923 RVA: 0x00138BB0 File Offset: 0x00136FB0
	public string ToStringFull()
	{
		return string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", new object[]
		{
			this.nameField,
			(!this.visibleField) ? "hidden" : "visible",
			(!this.openField) ? "closed" : "open",
			this.maxPlayersField,
			this.PlayerCount,
			this.customPropertiesField.ToStringFull()
		});
	}

	// Token: 0x06003E34 RID: 15924 RVA: 0x00138C3C File Offset: 0x0013703C
	protected internal void InternalCacheProperties(Hashtable propertiesToCache)
	{
		if (propertiesToCache == null || propertiesToCache.Count == 0 || this.customPropertiesField.Equals(propertiesToCache))
		{
			return;
		}
		if (propertiesToCache.ContainsKey(251))
		{
			this.removedFromList = (bool)propertiesToCache[251];
			if (this.removedFromList)
			{
				return;
			}
		}
		if (propertiesToCache.ContainsKey(255))
		{
			this.maxPlayersField = (byte)propertiesToCache[byte.MaxValue];
		}
		if (propertiesToCache.ContainsKey(253))
		{
			this.openField = (bool)propertiesToCache[253];
		}
		if (propertiesToCache.ContainsKey(254))
		{
			this.visibleField = (bool)propertiesToCache[254];
		}
		if (propertiesToCache.ContainsKey(252))
		{
			this.PlayerCount = (int)((byte)propertiesToCache[252]);
		}
		if (propertiesToCache.ContainsKey(249))
		{
			this.autoCleanUpField = (bool)propertiesToCache[249];
		}
		if (propertiesToCache.ContainsKey(248))
		{
			this.serverSideMasterClient = true;
			bool flag = this.masterClientIdField != 0;
			this.masterClientIdField = (int)propertiesToCache[248];
			if (flag)
			{
				PhotonNetwork.networkingPeer.UpdateMasterClient();
			}
		}
		if (propertiesToCache.ContainsKey(247))
		{
			this.expectedUsersField = (string[])propertiesToCache[247];
		}
		this.customPropertiesField.MergeStringKeys(propertiesToCache);
		this.customPropertiesField.StripKeysWithNullValues();
	}

	// Token: 0x170009F4 RID: 2548
	// (get) Token: 0x06003E35 RID: 15925 RVA: 0x00138E33 File Offset: 0x00137233
	[Obsolete("Please use CustomProperties (updated case for naming).")]
	public Hashtable customProperties
	{
		get
		{
			return this.CustomProperties;
		}
	}

	// Token: 0x170009F5 RID: 2549
	// (get) Token: 0x06003E36 RID: 15926 RVA: 0x00138E3B File Offset: 0x0013723B
	[Obsolete("Please use Name (updated case for naming).")]
	public string name
	{
		get
		{
			return this.Name;
		}
	}

	// Token: 0x170009F6 RID: 2550
	// (get) Token: 0x06003E37 RID: 15927 RVA: 0x00138E43 File Offset: 0x00137243
	// (set) Token: 0x06003E38 RID: 15928 RVA: 0x00138E4B File Offset: 0x0013724B
	[Obsolete("Please use PlayerCount (updated case for naming).")]
	public int playerCount
	{
		get
		{
			return this.PlayerCount;
		}
		set
		{
			this.PlayerCount = value;
		}
	}

	// Token: 0x170009F7 RID: 2551
	// (get) Token: 0x06003E39 RID: 15929 RVA: 0x00138E54 File Offset: 0x00137254
	// (set) Token: 0x06003E3A RID: 15930 RVA: 0x00138E5C File Offset: 0x0013725C
	[Obsolete("Please use IsLocalClientInside (updated case for naming).")]
	public bool isLocalClientInside
	{
		get
		{
			return this.IsLocalClientInside;
		}
		set
		{
			this.IsLocalClientInside = value;
		}
	}

	// Token: 0x170009F8 RID: 2552
	// (get) Token: 0x06003E3B RID: 15931 RVA: 0x00138E65 File Offset: 0x00137265
	[Obsolete("Please use MaxPlayers (updated case for naming).")]
	public byte maxPlayers
	{
		get
		{
			return this.MaxPlayers;
		}
	}

	// Token: 0x170009F9 RID: 2553
	// (get) Token: 0x06003E3C RID: 15932 RVA: 0x00138E6D File Offset: 0x0013726D
	[Obsolete("Please use IsOpen (updated case for naming).")]
	public bool open
	{
		get
		{
			return this.IsOpen;
		}
	}

	// Token: 0x170009FA RID: 2554
	// (get) Token: 0x06003E3D RID: 15933 RVA: 0x00138E75 File Offset: 0x00137275
	[Obsolete("Please use IsVisible (updated case for naming).")]
	public bool visible
	{
		get
		{
			return this.IsVisible;
		}
	}

	// Token: 0x040026A8 RID: 9896
	private Hashtable customPropertiesField = new Hashtable();

	// Token: 0x040026A9 RID: 9897
	protected byte maxPlayersField;

	// Token: 0x040026AA RID: 9898
	protected string[] expectedUsersField;

	// Token: 0x040026AB RID: 9899
	protected bool openField = true;

	// Token: 0x040026AC RID: 9900
	protected bool visibleField = true;

	// Token: 0x040026AD RID: 9901
	protected bool autoCleanUpField = PhotonNetwork.autoCleanUpPlayerObjects;

	// Token: 0x040026AE RID: 9902
	protected string nameField;

	// Token: 0x040026AF RID: 9903
	protected internal int masterClientIdField;
}
