using System;
using ExitGames.Client.Photon;

// Token: 0x02000746 RID: 1862
public class RoomOptions
{
	// Token: 0x17000958 RID: 2392
	// (get) Token: 0x06003BD3 RID: 15315 RVA: 0x0012D369 File Offset: 0x0012B769
	// (set) Token: 0x06003BD4 RID: 15316 RVA: 0x0012D371 File Offset: 0x0012B771
	public bool IsVisible
	{
		get
		{
			return this.isVisibleField;
		}
		set
		{
			this.isVisibleField = value;
		}
	}

	// Token: 0x17000959 RID: 2393
	// (get) Token: 0x06003BD5 RID: 15317 RVA: 0x0012D37A File Offset: 0x0012B77A
	// (set) Token: 0x06003BD6 RID: 15318 RVA: 0x0012D382 File Offset: 0x0012B782
	public bool IsOpen
	{
		get
		{
			return this.isOpenField;
		}
		set
		{
			this.isOpenField = value;
		}
	}

	// Token: 0x1700095A RID: 2394
	// (get) Token: 0x06003BD7 RID: 15319 RVA: 0x0012D38B File Offset: 0x0012B78B
	// (set) Token: 0x06003BD8 RID: 15320 RVA: 0x0012D393 File Offset: 0x0012B793
	public bool CleanupCacheOnLeave
	{
		get
		{
			return this.cleanupCacheOnLeaveField;
		}
		set
		{
			this.cleanupCacheOnLeaveField = value;
		}
	}

	// Token: 0x1700095B RID: 2395
	// (get) Token: 0x06003BD9 RID: 15321 RVA: 0x0012D39C File Offset: 0x0012B79C
	public bool SuppressRoomEvents
	{
		get
		{
			return this.suppressRoomEventsField;
		}
	}

	// Token: 0x1700095C RID: 2396
	// (get) Token: 0x06003BDA RID: 15322 RVA: 0x0012D3A4 File Offset: 0x0012B7A4
	// (set) Token: 0x06003BDB RID: 15323 RVA: 0x0012D3AC File Offset: 0x0012B7AC
	public bool PublishUserId
	{
		get
		{
			return this.publishUserIdField;
		}
		set
		{
			this.publishUserIdField = value;
		}
	}

	// Token: 0x1700095D RID: 2397
	// (get) Token: 0x06003BDC RID: 15324 RVA: 0x0012D3B5 File Offset: 0x0012B7B5
	// (set) Token: 0x06003BDD RID: 15325 RVA: 0x0012D3BD File Offset: 0x0012B7BD
	public bool DeleteNullProperties
	{
		get
		{
			return this.deleteNullPropertiesField;
		}
		set
		{
			this.deleteNullPropertiesField = value;
		}
	}

	// Token: 0x1700095E RID: 2398
	// (get) Token: 0x06003BDE RID: 15326 RVA: 0x0012D3C6 File Offset: 0x0012B7C6
	// (set) Token: 0x06003BDF RID: 15327 RVA: 0x0012D3CE File Offset: 0x0012B7CE
	[Obsolete("Use property with uppercase naming instead.")]
	public bool isVisible
	{
		get
		{
			return this.isVisibleField;
		}
		set
		{
			this.isVisibleField = value;
		}
	}

	// Token: 0x1700095F RID: 2399
	// (get) Token: 0x06003BE0 RID: 15328 RVA: 0x0012D3D7 File Offset: 0x0012B7D7
	// (set) Token: 0x06003BE1 RID: 15329 RVA: 0x0012D3DF File Offset: 0x0012B7DF
	[Obsolete("Use property with uppercase naming instead.")]
	public bool isOpen
	{
		get
		{
			return this.isOpenField;
		}
		set
		{
			this.isOpenField = value;
		}
	}

	// Token: 0x17000960 RID: 2400
	// (get) Token: 0x06003BE2 RID: 15330 RVA: 0x0012D3E8 File Offset: 0x0012B7E8
	// (set) Token: 0x06003BE3 RID: 15331 RVA: 0x0012D3F0 File Offset: 0x0012B7F0
	[Obsolete("Use property with uppercase naming instead.")]
	public byte maxPlayers
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

	// Token: 0x17000961 RID: 2401
	// (get) Token: 0x06003BE4 RID: 15332 RVA: 0x0012D3F9 File Offset: 0x0012B7F9
	// (set) Token: 0x06003BE5 RID: 15333 RVA: 0x0012D401 File Offset: 0x0012B801
	[Obsolete("Use property with uppercase naming instead.")]
	public bool cleanupCacheOnLeave
	{
		get
		{
			return this.cleanupCacheOnLeaveField;
		}
		set
		{
			this.cleanupCacheOnLeaveField = value;
		}
	}

	// Token: 0x17000962 RID: 2402
	// (get) Token: 0x06003BE6 RID: 15334 RVA: 0x0012D40A File Offset: 0x0012B80A
	// (set) Token: 0x06003BE7 RID: 15335 RVA: 0x0012D412 File Offset: 0x0012B812
	[Obsolete("Use property with uppercase naming instead.")]
	public Hashtable customRoomProperties
	{
		get
		{
			return this.CustomRoomProperties;
		}
		set
		{
			this.CustomRoomProperties = value;
		}
	}

	// Token: 0x17000963 RID: 2403
	// (get) Token: 0x06003BE8 RID: 15336 RVA: 0x0012D41B File Offset: 0x0012B81B
	// (set) Token: 0x06003BE9 RID: 15337 RVA: 0x0012D423 File Offset: 0x0012B823
	[Obsolete("Use property with uppercase naming instead.")]
	public string[] customRoomPropertiesForLobby
	{
		get
		{
			return this.CustomRoomPropertiesForLobby;
		}
		set
		{
			this.CustomRoomPropertiesForLobby = value;
		}
	}

	// Token: 0x17000964 RID: 2404
	// (get) Token: 0x06003BEA RID: 15338 RVA: 0x0012D42C File Offset: 0x0012B82C
	// (set) Token: 0x06003BEB RID: 15339 RVA: 0x0012D434 File Offset: 0x0012B834
	[Obsolete("Use property with uppercase naming instead.")]
	public string[] plugins
	{
		get
		{
			return this.Plugins;
		}
		set
		{
			this.Plugins = value;
		}
	}

	// Token: 0x17000965 RID: 2405
	// (get) Token: 0x06003BEC RID: 15340 RVA: 0x0012D43D File Offset: 0x0012B83D
	[Obsolete("Use property with uppercase naming instead.")]
	public bool suppressRoomEvents
	{
		get
		{
			return this.suppressRoomEventsField;
		}
	}

	// Token: 0x17000966 RID: 2406
	// (get) Token: 0x06003BED RID: 15341 RVA: 0x0012D445 File Offset: 0x0012B845
	// (set) Token: 0x06003BEE RID: 15342 RVA: 0x0012D44D File Offset: 0x0012B84D
	[Obsolete("Use property with uppercase naming instead.")]
	public bool publishUserId
	{
		get
		{
			return this.publishUserIdField;
		}
		set
		{
			this.publishUserIdField = value;
		}
	}

	// Token: 0x0400257C RID: 9596
	private bool isVisibleField = true;

	// Token: 0x0400257D RID: 9597
	private bool isOpenField = true;

	// Token: 0x0400257E RID: 9598
	public byte MaxPlayers;

	// Token: 0x0400257F RID: 9599
	public int PlayerTtl;

	// Token: 0x04002580 RID: 9600
	public int EmptyRoomTtl;

	// Token: 0x04002581 RID: 9601
	private bool cleanupCacheOnLeaveField = PhotonNetwork.autoCleanUpPlayerObjects;

	// Token: 0x04002582 RID: 9602
	public Hashtable CustomRoomProperties;

	// Token: 0x04002583 RID: 9603
	public string[] CustomRoomPropertiesForLobby = new string[0];

	// Token: 0x04002584 RID: 9604
	public string[] Plugins;

	// Token: 0x04002585 RID: 9605
	private bool suppressRoomEventsField;

	// Token: 0x04002586 RID: 9606
	private bool publishUserIdField;

	// Token: 0x04002587 RID: 9607
	private bool deleteNullPropertiesField;
}
