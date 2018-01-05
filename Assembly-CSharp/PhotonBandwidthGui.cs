using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using UnityEngine;
using VRC;

// Token: 0x02000AB7 RID: 2743
public class PhotonBandwidthGui : VRGUI
{
	// Token: 0x17000C1B RID: 3099
	// (get) Token: 0x06005359 RID: 21337 RVA: 0x001CAB02 File Offset: 0x001C8F02
	private int frame
	{
		get
		{
			if (this.lastNetworkTime != PhotonNetwork.time)
			{
				this.lastNetworkTime = PhotonNetwork.time;
				this._frame++;
			}
			return this._frame;
		}
	}

	// Token: 0x17000C1C RID: 3100
	// (get) Token: 0x0600535A RID: 21338 RVA: 0x001CAB33 File Offset: 0x001C8F33
	public static PhotonBandwidthGui Instance
	{
		get
		{
			return PhotonBandwidthGui._instance;
		}
	}

	// Token: 0x17000C1D RID: 3101
	// (get) Token: 0x0600535B RID: 21339 RVA: 0x001CAB3C File Offset: 0x001C8F3C
	private static bool ShouldRecord
	{
		get
		{
			if (!PhotonBandwidthGui._shouldRecord)
			{
				PhotonBandwidthGui._shouldRecord = (PhotonBandwidthGui._instance != null && VRC.Network.IsNetworkSettled && (PhotonBandwidthGui._instance.graphVisible || PhotonBandwidthGui._instance.textVisible));
			}
			return PhotonBandwidthGui._shouldRecord;
		}
	}

	// Token: 0x0600535C RID: 21340 RVA: 0x001CAB98 File Offset: 0x001C8F98
	private static int ApproximatePing(Player player)
	{
		Player localPlayer = VRC.Network.LocalPlayer;
		if (localPlayer == null || player == null)
		{
			return 0;
		}
		return (int)(((!(player != localPlayer)) ? 0 : player.vrcPlayer.Ping) + localPlayer.vrcPlayer.Ping);
	}

	// Token: 0x0600535D RID: 21341 RVA: 0x001CABF0 File Offset: 0x001C8FF0
	public static void RecordEvent(EventData evt, PhotonPlayer originatingPlayer)
	{
		if (!PhotonBandwidthGui.ShouldRecord)
		{
			return;
		}
		try
		{
			PhotonBandwidthGui instance = PhotonBandwidthGui._instance;
			string name = Enum.GetName(typeof(PhotonBandwidthGui.EventTypes), (PhotonBandwidthGui.EventTypes)evt.Code);
			int bytes = Protocol.Serialize(evt.Parameters).Length;
			object obj = instance.serializationStatistics;
			lock (obj)
			{
				instance.serializationStatistics.Insert(0, new PhotonBandwidthGui.UsageData
				{
					name = name,
					bytes = bytes,
					frame = instance.frame,
					type = PhotonBandwidthGui.UsageType.Event,
					addToTotal = true
				});
			}
			if (evt.Code == 201 || evt.Code == 206)
			{
				Hashtable hashtable = (Hashtable)evt[245];
				byte b = 10;
				int num = 1;
				if (hashtable.ContainsKey(1))
				{
					short num2 = (short)hashtable[1];
					num = 2;
				}
				byte b2 = b;
				while ((int)(b2 - b) < hashtable.Count - num)
				{
					object[] array = hashtable[b2] as object[];
					PhotonBandwidthGui.RecordSerialization(PhotonView.Find((int)array[0]), array);
					b2 += 1;
				}
			}
			if (originatingPlayer != null)
			{
				object obj2 = instance.serializationStatistics;
				lock (obj2)
				{
					instance.serializationStatistics.Insert(0, new PhotonBandwidthGui.UsageData
					{
						name = originatingPlayer.NickName,
						bytes = bytes,
						frame = instance.frame,
						type = PhotonBandwidthGui.UsageType.Player,
						addToTotal = false
					});
				}
			}
		}
		catch
		{
		}
	}

	// Token: 0x0600535E RID: 21342 RVA: 0x001CAE08 File Offset: 0x001C9208
	public static void RecordRPC(string methodName, int totalSize)
	{
		if (!PhotonBandwidthGui.ShouldRecord)
		{
			return;
		}
		try
		{
			PhotonBandwidthGui instance = PhotonBandwidthGui._instance;
			object obj = instance.serializationStatistics;
			lock (obj)
			{
				instance.serializationStatistics.Insert(0, new PhotonBandwidthGui.UsageData
				{
					bytes = totalSize,
					frame = instance.frame,
					type = PhotonBandwidthGui.UsageType.RPC,
					name = methodName,
					addToTotal = true
				});
			}
		}
		catch
		{
		}
	}

	// Token: 0x0600535F RID: 21343 RVA: 0x001CAEA8 File Offset: 0x001C92A8
	public static void RecordSerialization(PhotonView view, object[] data)
	{
		if (!PhotonBandwidthGui.ShouldRecord)
		{
			return;
		}
		try
		{
			PhotonBandwidthGui instance = PhotonBandwidthGui._instance;
			List<object> list = new List<object>();
			if (data != null)
			{
				list.AddRange(data);
			}
			int num = 0;
			foreach (object obj in list)
			{
				if (obj != null)
				{
					int num2 = Protocol.Serialize(obj).Length;
					object obj2 = instance.serializationStatistics;
					lock (obj2)
					{
						instance.serializationStatistics.Insert(0, new PhotonBandwidthGui.UsageData
						{
							bytes = num2,
							frame = instance.frame,
							name = obj.GetType().Name,
							type = PhotonBandwidthGui.UsageType.Value,
							addToTotal = false
						});
					}
					num += num2;
				}
			}
			Player componentInSelfOrParent = view.gameObject.GetComponentInSelfOrParent<Player>();
			object obj3 = instance.serializationStatistics;
			lock (obj3)
			{
				instance.serializationStatistics.Insert(0, new PhotonBandwidthGui.UsageData
				{
					bytes = num,
					frame = instance.frame,
					type = ((!(componentInSelfOrParent == null)) ? PhotonBandwidthGui.UsageType.Player : PhotonBandwidthGui.UsageType.Object),
					obj = view.gameObject,
					name = PhotonBandwidthGui.GetObjectPath(view.gameObject),
					addToTotal = false
				});
			}
			object obj4 = instance.serializationStatistics;
			lock (obj4)
			{
				if (instance.serializationStatistics.Count > 1000)
				{
					instance.Flush();
				}
			}
		}
		catch
		{
		}
	}

	// Token: 0x06005360 RID: 21344 RVA: 0x001CB0E8 File Offset: 0x001C94E8
	public static void RecordPlayerStat(Player player)
	{
		if (!PhotonBandwidthGui.ShouldRecord)
		{
			return;
		}
		try
		{
			PhotonBandwidthGui instance = PhotonBandwidthGui._instance;
			object obj = instance.playerStats;
			lock (obj)
			{
				if (!instance.playerStats.ContainsKey(player))
				{
					instance.playerStats.Add(player, new List<PhotonBandwidthGui.PlayerStat>());
				}
			}
			object obj2 = instance.playerStats;
			lock (obj2)
			{
				instance.playerStats[player].Insert(0, new PhotonBandwidthGui.PlayerStat
				{
					frame = instance.frame,
					ping = (float)PhotonBandwidthGui.ApproximatePing(player),
					quality = player.vrcPlayer.ConnectionQuality
				});
			}
		}
		catch
		{
		}
	}

	// Token: 0x06005361 RID: 21345 RVA: 0x001CB1D8 File Offset: 0x001C95D8
	private void Awake()
	{
		if (PhotonBandwidthGui._instance == null)
		{
			PhotonBandwidthGui._instance = this;
			this.serializationStatistics = new List<PhotonBandwidthGui.UsageData>();
			this.playerSerializationTotals = new Dictionary<string, PhotonBandwidthGui.UsageTotal>();
			this.objectSerializationTotals = new Dictionary<string, PhotonBandwidthGui.UsageTotal>();
			this.valueSerializationTotals = new Dictionary<string, PhotonBandwidthGui.UsageTotal>();
			this.rpcSerializationTotals = new Dictionary<string, PhotonBandwidthGui.UsageTotal>();
			this.eventSerializationTotals = new Dictionary<string, PhotonBandwidthGui.UsageTotal>();
			this.usageFrames = new Dictionary<string, int>();
			this.frameSizes = new Dictionary<int, int>();
			this.usageGraph = new Texture2D(50, 50);
			for (int i = 0; i < this.playerGraphs.Length; i++)
			{
				this.playerGraphs[i] = new Texture2D(50, 50);
				Color black = Color.black;
				black.a = 0.25f;
				for (int j = 0; j < 50; j++)
				{
					for (int k = 0; k < 50; k++)
					{
						this.playerGraphs[i].SetPixel(j, k, black);
					}
				}
				this.playerGraphs[i].Apply();
			}
			if (this.textStyle == null)
			{
				this.textStyle = new GUIStyle(GUI.skin.label);
				this.textStyle.fontSize = Mathf.CeilToInt((float)this.textStyle.fontSize * 0.5f);
			}
			return;
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06005362 RID: 21346 RVA: 0x001CB330 File Offset: 0x001C9730
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha4) && Input.GetKey(KeyCode.Menu))
		{
			this.textVisible = !this.textVisible;
			if (!this.textVisible && !this.graphVisible)
			{
				PhotonBandwidthGui.Reset();
			}
			PhotonNetwork.networkingPeer.TrafficStatsEnabled = (this.graphVisible || this.textVisible);
		}
		if (Input.GetKeyDown(KeyCode.Alpha5) && Input.GetKey(KeyCode.Menu))
		{
			this.graphVisible = !this.graphVisible;
			if (!this.textVisible && !this.graphVisible)
			{
				PhotonBandwidthGui.Reset();
			}
			PhotonNetwork.networkingPeer.TrafficStatsEnabled = (this.graphVisible || this.textVisible);
		}
		this.acceptMouse = this.textVisible;
		if (this.textVisible || this.graphVisible)
		{
			this.Flush();
		}
	}

	// Token: 0x06005363 RID: 21347 RVA: 0x001CB42C File Offset: 0x001C982C
	public override void OnVRGUI()
	{
		this.windowRect = base.MakeCenterRect();
		if (this.textVisible)
		{
			GUILayout.BeginArea(this.windowRect, GUI.skin.box);
			this.DrawBandwidthWindow();
			GUILayout.EndArea();
		}
		if (this.graphVisible)
		{
			GUILayout.BeginArea(this.windowRect, GUI.skin.box);
			this.DrawGraphs();
			GUILayout.EndArea();
		}
	}

	// Token: 0x06005364 RID: 21348 RVA: 0x001CB49B File Offset: 0x001C989B
	public static void Reset()
	{
		if (PhotonBandwidthGui.Instance != null)
		{
			PhotonBandwidthGui.Instance._Reset();
		}
		PhotonNetwork.networkingPeer.TrafficStatsReset();
	}

	// Token: 0x06005365 RID: 21349 RVA: 0x001CB4C4 File Offset: 0x001C98C4
	private void _Reset()
	{
		object obj = this.serializationStatistics;
		lock (obj)
		{
			this.serializationStatistics.Clear();
		}
		this._frame = 0;
		this.playerSerializationTotals.Clear();
		this.objectSerializationTotals.Clear();
		this.valueSerializationTotals.Clear();
		this.rpcSerializationTotals.Clear();
		this.eventSerializationTotals.Clear();
		this.usageFrames.Clear();
		this.frameSizes.Clear();
		object obj2 = this.playerStats;
		lock (obj2)
		{
			this.playerStats.Clear();
		}
		PhotonBandwidthGui._shouldRecord = (this.graphVisible = (this.textVisible = false));
	}

	// Token: 0x06005366 RID: 21350 RVA: 0x001CB5A4 File Offset: 0x001C99A4
	private void Flush()
	{
		object obj = this.serializationStatistics;
		lock (obj)
		{
			foreach (PhotonBandwidthGui.UsageData usageData in this.serializationStatistics.Take(100))
			{
				if (usageData.name != null)
				{
					if (usageData.type == PhotonBandwidthGui.UsageType.Player)
					{
						if (!this.playerSerializationTotals.ContainsKey(usageData.name))
						{
							this.playerSerializationTotals.Add(usageData.name, new PhotonBandwidthGui.UsageTotal
							{
								maximum = 0,
								total = 0,
								obj = usageData.obj
							});
						}
						this.playerSerializationTotals[usageData.name].total += usageData.bytes;
						this.playerSerializationTotals[usageData.name].maximum = Mathf.Max(usageData.bytes, this.playerSerializationTotals[usageData.name].maximum);
					}
					else if (usageData.type == PhotonBandwidthGui.UsageType.Object)
					{
						if (!this.objectSerializationTotals.ContainsKey(usageData.name))
						{
							this.objectSerializationTotals.Add(usageData.name, new PhotonBandwidthGui.UsageTotal
							{
								maximum = 0,
								total = 0,
								obj = usageData.obj
							});
						}
						this.objectSerializationTotals[usageData.name].total += usageData.bytes;
						this.objectSerializationTotals[usageData.name].maximum = Mathf.Max(usageData.bytes, this.objectSerializationTotals[usageData.name].maximum);
					}
					else if (usageData.type == PhotonBandwidthGui.UsageType.Value)
					{
						if (!this.valueSerializationTotals.ContainsKey(usageData.name))
						{
							this.valueSerializationTotals.Add(usageData.name, new PhotonBandwidthGui.UsageTotal
							{
								maximum = 0,
								total = 0,
								obj = usageData.obj
							});
						}
						this.valueSerializationTotals[usageData.name].total += usageData.bytes;
						this.valueSerializationTotals[usageData.name].maximum = Mathf.Max(usageData.bytes, this.valueSerializationTotals[usageData.name].maximum);
					}
					else if (usageData.type == PhotonBandwidthGui.UsageType.RPC)
					{
						if (!this.rpcSerializationTotals.ContainsKey(usageData.name))
						{
							this.rpcSerializationTotals.Add(usageData.name, new PhotonBandwidthGui.UsageTotal
							{
								maximum = 0,
								total = 0,
								obj = usageData.obj
							});
						}
						this.rpcSerializationTotals[usageData.name].total += usageData.bytes;
						this.rpcSerializationTotals[usageData.name].maximum = Mathf.Max(usageData.bytes, this.rpcSerializationTotals[usageData.name].maximum);
					}
					else if (usageData.type == PhotonBandwidthGui.UsageType.Event)
					{
						if (!this.eventSerializationTotals.ContainsKey(usageData.name))
						{
							this.eventSerializationTotals.Add(usageData.name, new PhotonBandwidthGui.UsageTotal
							{
								maximum = 0,
								total = 0,
								obj = usageData.obj
							});
						}
						this.eventSerializationTotals[usageData.name].total += usageData.bytes;
						this.eventSerializationTotals[usageData.name].maximum = Mathf.Max(usageData.bytes, this.eventSerializationTotals[usageData.name].maximum);
					}
					object obj2 = this.usageFrames;
					lock (obj2)
					{
						if (!this.usageFrames.ContainsKey(usageData.name))
						{
							this.usageFrames.Add(usageData.name, 0);
						}
						Dictionary<string, int> dictionary;
						string name;
						(dictionary = this.usageFrames)[name = usageData.name] = dictionary[name] + 1;
					}
					if (!this.frameSizes.ContainsKey(usageData.frame))
					{
						this.frameSizes.Add(usageData.frame, 0);
					}
					if (usageData.addToTotal)
					{
						Dictionary<int, int> dictionary2;
						int frame;
						(dictionary2 = this.frameSizes)[frame = usageData.frame] = dictionary2[frame] + usageData.bytes;
					}
				}
			}
			this.serializationStatistics.RemoveRange(0, Mathf.Min(100, this.serializationStatistics.Count<PhotonBandwidthGui.UsageData>()));
		}
	}

	// Token: 0x06005367 RID: 21351 RVA: 0x001CBAF0 File Offset: 0x001C9EF0
	public void DrawBandwidthWindow()
	{
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (PhotonNetwork.isMasterClient)
		{
			GUILayout.Label("Mstr", this.textStyle, new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
		}
		GUILayout.Label("Ps: " + ((PhotonNetwork.room == null) ? "1" : PhotonNetwork.room.PlayerCount.ToString()), this.textStyle, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label("Dly: " + VRC.Network.SimulationDelay(VRC.Network.LocalPlayer).ToString(), this.textStyle, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label("Rt: " + PhotonNetwork.sendRateOnSerialize.ToString(), this.textStyle, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		TrafficStats trafficStatsIncoming = PhotonNetwork.networkingPeer.TrafficStatsIncoming;
		TrafficStats trafficStatsOutgoing = PhotonNetwork.networkingPeer.TrafficStatsOutgoing;
		int totalWidth = 5;
		float num = 1f / ((float)PhotonNetwork.networkingPeer.TrafficStatsElapsedMs / 1000f);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label("In", this.textStyle, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label(string.Concat(new string[]
		{
			"Cmd: ",
			trafficStatsIncoming.TotalCommandCount.ToString().PadLeft(totalWidth, ' '),
			"/",
			((float)trafficStatsIncoming.TotalCommandCount * num).ToString("n1").PadRight(totalWidth, ' '),
			" ",
			trafficStatsIncoming.TotalCommandBytes.ToString().PadLeft(totalWidth, ' '),
			"/",
			((float)trafficStatsIncoming.TotalCommandBytes * num).ToString("n1").PadRight(totalWidth, ' ')
		}), this.textStyle, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label("Ttl: " + trafficStatsIncoming.TotalPacketBytes.ToString().PadLeft(totalWidth, ' ') + "/" + ((float)trafficStatsIncoming.TotalPacketBytes * num).ToString("n1").PadRight(totalWidth, ' '), this.textStyle, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label("Out", this.textStyle, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label(string.Concat(new string[]
		{
			"Cmd: ",
			trafficStatsOutgoing.TotalCommandCount.ToString().PadLeft(totalWidth, ' '),
			"/",
			((float)trafficStatsOutgoing.TotalCommandCount * num).ToString("n1").PadRight(totalWidth, ' '),
			" ",
			trafficStatsOutgoing.TotalCommandBytes.ToString().PadLeft(totalWidth, ' '),
			"/",
			((float)trafficStatsOutgoing.TotalCommandBytes * num).ToString("n1").PadRight(totalWidth, ' ')
		}), this.textStyle, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label("Ttl: " + trafficStatsOutgoing.TotalPacketBytes.ToString().PadLeft(totalWidth, ' ') + "/" + ((float)trafficStatsOutgoing.TotalPacketBytes * num).ToString("n1").PadRight(totalWidth, ' '), this.textStyle, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.EndVertical();
		if (this.frame == 0)
		{
			return;
		}
		IEnumerable<KeyValuePair<string, PhotonBandwidthGui.UsageTotal>> usage = (from s in this.playerSerializationTotals
		orderby s.Key
		select s).Reverse<KeyValuePair<string, PhotonBandwidthGui.UsageTotal>>();
		IEnumerable<KeyValuePair<string, PhotonBandwidthGui.UsageTotal>> usage2 = (from s in this.objectSerializationTotals
		orderby s.Value.total
		select s).Reverse<KeyValuePair<string, PhotonBandwidthGui.UsageTotal>>();
		IEnumerable<KeyValuePair<string, PhotonBandwidthGui.UsageTotal>> usage3 = (from s in this.valueSerializationTotals
		orderby s.Value.total
		select s).Reverse<KeyValuePair<string, PhotonBandwidthGui.UsageTotal>>();
		IEnumerable<KeyValuePair<string, PhotonBandwidthGui.UsageTotal>> usage4 = (from s in this.rpcSerializationTotals
		orderby s.Value.total
		select s).Reverse<KeyValuePair<string, PhotonBandwidthGui.UsageTotal>>();
		IEnumerable<KeyValuePair<string, PhotonBandwidthGui.UsageTotal>> usage5 = (from s in this.eventSerializationTotals
		orderby s.Value.total
		select s).Reverse<KeyValuePair<string, PhotonBandwidthGui.UsageTotal>>();
		GUILayout.BeginArea(new Rect(10f, 40f, this.windowRect.width - 20f, this.windowRect.height - 50f));
		this.serializationScrollPosition = GUILayout.BeginScrollView(this.serializationScrollPosition, new GUILayoutOption[0]);
		this.DrawHeader();
		object obj = this.usageFrames;
		lock (obj)
		{
			GUILayout.Label("Players", this.textStyle, new GUILayoutOption[0]);
			this.DrawUsageList(usage, this.usageFrames, PhotonBandwidthGui.UsageType.Player);
			GUILayout.Space(10f);
			GUILayout.Label("Objects", this.textStyle, new GUILayoutOption[0]);
			this.DrawUsageList(usage2, this.usageFrames, PhotonBandwidthGui.UsageType.Object);
			GUILayout.Space(10f);
			GUILayout.Label("RPCs", this.textStyle, new GUILayoutOption[0]);
			this.DrawUsageList(usage4, this.usageFrames, PhotonBandwidthGui.UsageType.RPC);
			GUILayout.Space(10f);
			GUILayout.Label("Events", this.textStyle, new GUILayoutOption[0]);
			this.DrawUsageList(usage5, this.usageFrames, PhotonBandwidthGui.UsageType.Event);
			GUILayout.Space(10f);
			GUILayout.Label("Types", this.textStyle, new GUILayoutOption[0]);
			this.DrawUsageList(usage3, this.usageFrames, PhotonBandwidthGui.UsageType.Value);
		}
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	// Token: 0x06005368 RID: 21352 RVA: 0x001CC13C File Offset: 0x001CA53C
	private void DrawGraphs()
	{
		GUILayout.BeginArea(new Rect(10f, 40f, this.windowRect.width - 20f, this.windowRect.height - 50f));
		this.serializationScrollPosition = GUILayout.BeginScrollView(this.serializationScrollPosition, new GUILayoutOption[0]);
		object obj = this.playerStats;
		lock (obj)
		{
			foreach (Player player in from p in this.playerStats.Keys
			where p != null && p.GetPhotonPlayerId() - 1 < this.playerGraphs.Length
			select p)
			{
				this.RenderPlayerGraph(player, this.playerGraphs[player.GetPhotonPlayerId() - 1]);
			}
		}
		this.RenderUsageGraph(this.usageGraph);
		int num = 20;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < this.playerGraphs.Length; i++)
		{
			PhotonPlayer photonPlayer = PhotonPlayer.Find(i + 1);
			if (photonPlayer != null)
			{
				int num4 = num2;
				num2 = num3 % Mathf.FloorToInt((this.windowRect.width - 20f) / 60f) * 60 + 10;
				if (num2 < num4)
				{
					num += 90;
				}
				num3++;
				GUILayout.BeginArea(new Rect((float)num2, (float)num, 50f, 50f), this.playerGraphs[i]);
				GUILayout.EndArea();
				GUILayout.BeginArea(new Rect((float)num2, (float)(num + 50 + 10), 50f, 20f));
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				GUILayout.Label(new GUIContent(photonPlayer.NickName + " " + (i + 1).ToString()), this.textStyle, new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}
		}
		GUILayout.BeginArea(new Rect(10f, (float)(num + 50 + 40), 50f, 50f), this.usageGraph);
		GUILayout.EndArea();
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	// Token: 0x06005369 RID: 21353 RVA: 0x001CC38C File Offset: 0x001CA78C
	private void RenderPlayerGraph(Player player, Texture2D tex)
	{
		object obj = this.playerStats;
		PhotonBandwidthGui.PlayerStat[] array;
		lock (obj)
		{
			array = (from s in this.playerStats[player]
			orderby s.frame
			select s).Reverse<PhotonBandwidthGui.PlayerStat>().Take(50).ToArray<PhotonBandwidthGui.PlayerStat>();
		}
		for (int i = 0; i < 50; i++)
		{
			int num = -1;
			int num2 = -1;
			if (i < array.Length)
			{
				PhotonBandwidthGui.PlayerStat playerStat = array[i];
				num = Mathf.Min(49, Mathf.FloorToInt(playerStat.ping / 400f * 50f));
				num2 = Mathf.Min(49, Mathf.FloorToInt(playerStat.quality * 50f));
			}
			for (int j = 0; j < 50; j++)
			{
				Color color;
				if (num == j)
				{
					color = Color.red;
				}
				else if (num2 == j)
				{
					color = Color.cyan;
				}
				else
				{
					color = Color.black;
					color.a = 0.25f;
				}
				tex.SetPixel(50 - i - 1, j, color);
			}
		}
		tex.Apply();
	}

	// Token: 0x0600536A RID: 21354 RVA: 0x001CC4D8 File Offset: 0x001CA8D8
	private void RenderUsageGraph(Texture2D tex)
	{
		KeyValuePair<int, int>[] array = (from s in this.frameSizes
		orderby s.Key
		select s).Reverse<KeyValuePair<int, int>>().Take(tex.width).ToArray<KeyValuePair<int, int>>();
		int num = 0;
		foreach (KeyValuePair<int, int> keyValuePair in array)
		{
			if (keyValuePair.Value > num)
			{
				num = keyValuePair.Value;
			}
		}
		double num2 = 1.0 / (double)this.MaximumBytesPerFrame;
		int num3 = array.Length;
		int num4 = 0;
		for (int j = 49; j >= 0; j--)
		{
			int num5 = 49 - j;
			double num6 = 0.0;
			if (num5 < num3)
			{
				num6 = (double)array[num5].Value * num2;
			}
			Color color = Color.black;
			if (num6 < 0.333)
			{
				color = Color.green;
			}
			else if (num6 < 0.666)
			{
				color = Color.yellow;
			}
			else
			{
				color = Color.red;
				num4++;
			}
			for (int k = 0; k < 50; k++)
			{
				double num7 = (double)k / 50.0;
				if (num7 > num6)
				{
					color = Color.black;
					color.a = 0.25f;
				}
				tex.SetPixel(j, k, color);
			}
		}
		tex.Apply();
	}

	// Token: 0x0600536B RID: 21355 RVA: 0x001CC664 File Offset: 0x001CAA64
	private void DrawRow(string a, string b, string c, string d, string e)
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		float num = this.windowRect.width - 60f;
		float num2 = num / 6f;
		GUILayout.BeginVertical(new GUILayoutOption[]
		{
			GUILayout.Width(num2 * 2f)
		});
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label(a, this.textStyle, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[]
		{
			GUILayout.Width(num2)
		});
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label(b, this.textStyle, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[]
		{
			GUILayout.Width(num2)
		});
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label(c, this.textStyle, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[]
		{
			GUILayout.Width(num2)
		});
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label(d, this.textStyle, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[]
		{
			GUILayout.Width(num2)
		});
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label(e, this.textStyle, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	// Token: 0x0600536C RID: 21356 RVA: 0x001CC7DE File Offset: 0x001CABDE
	private void DrawHeader()
	{
		this.DrawRow("Name", "Ttl", "Cnt", "Avg Sz", "Qlty");
	}

	// Token: 0x0600536D RID: 21357 RVA: 0x001CC800 File Offset: 0x001CAC00
	private void DrawUsageList(IEnumerable<KeyValuePair<string, PhotonBandwidthGui.UsageTotal>> usage, Dictionary<string, int> frameCounts, PhotonBandwidthGui.UsageType type)
	{
		int num = 50;
		foreach (KeyValuePair<string, PhotonBandwidthGui.UsageTotal> keyValuePair in usage)
		{
			if (type != PhotonBandwidthGui.UsageType.Player)
			{
				this.DrawRow(string.Format("{0}", keyValuePair.Key), string.Format("{0} ({1})", keyValuePair.Value.total, keyValuePair.Value.maximum), string.Format("{0}", (double)frameCounts[keyValuePair.Key]), string.Format("{0:F3}", (double)keyValuePair.Value.total / (double)frameCounts[keyValuePair.Key]), string.Empty);
			}
			else if (keyValuePair.Value.obj != null)
			{
				Player componentInSelfOrParent = keyValuePair.Value.obj.GetComponentInSelfOrParent<Player>();
				float num2 = (!(componentInSelfOrParent.playerNet == null)) ? componentInSelfOrParent.playerNet.ConnectionDisparity : 0f;
				this.DrawRow(string.Format("{0} {1}", componentInSelfOrParent.name, componentInSelfOrParent.GetPhotonPlayerId()), string.Format("{0} ({1})", keyValuePair.Value.total, keyValuePair.Value.maximum), string.Format("{0}", (double)frameCounts[keyValuePair.Key]), string.Format("{0:F3}", (double)keyValuePair.Value.total / (double)frameCounts[keyValuePair.Key]), string.Format("{0}ms, {1:F2}ms, {2}%", PhotonBandwidthGui.ApproximatePing(componentInSelfOrParent), num2, Mathf.CeilToInt(100f * componentInSelfOrParent.vrcPlayer.ConnectionQuality)));
			}
			num--;
			if (num == 0)
			{
				break;
			}
		}
	}

	// Token: 0x0600536E RID: 21358 RVA: 0x001CCA24 File Offset: 0x001CAE24
	private static string GetObjectPath(GameObject go)
	{
		if (go == null)
		{
			return null;
		}
		string text = string.Empty;
		int num = 2;
		while (go != null)
		{
			text = go.name + "/" + text;
			if (go.transform.parent != null)
			{
				go = go.transform.parent.gameObject;
			}
			else
			{
				go = null;
			}
			num--;
			if (num == 0)
			{
				if (go != null)
				{
					text = "../" + text;
				}
				break;
			}
		}
		return text.Substring(0, text.Length - 1);
	}

	// Token: 0x04003AAE RID: 15022
	public int MaximumBytesPerFrame = 60000;

	// Token: 0x04003AAF RID: 15023
	private bool textVisible;

	// Token: 0x04003AB0 RID: 15024
	private bool graphVisible;

	// Token: 0x04003AB1 RID: 15025
	[Range(10f, 100f)]
	public int MaxNameLength = 20;

	// Token: 0x04003AB2 RID: 15026
	private const int MAX_PER_FRAME = 100;

	// Token: 0x04003AB3 RID: 15027
	private Rect windowRect = new Rect(0f, 0f, 400f, 200f);

	// Token: 0x04003AB4 RID: 15028
	private List<PhotonBandwidthGui.UsageData> serializationStatistics = new List<PhotonBandwidthGui.UsageData>();

	// Token: 0x04003AB5 RID: 15029
	private static PhotonBandwidthGui _instance;

	// Token: 0x04003AB6 RID: 15030
	private Dictionary<string, PhotonBandwidthGui.UsageTotal> playerSerializationTotals = new Dictionary<string, PhotonBandwidthGui.UsageTotal>();

	// Token: 0x04003AB7 RID: 15031
	private Dictionary<string, PhotonBandwidthGui.UsageTotal> objectSerializationTotals = new Dictionary<string, PhotonBandwidthGui.UsageTotal>();

	// Token: 0x04003AB8 RID: 15032
	private Dictionary<string, PhotonBandwidthGui.UsageTotal> valueSerializationTotals = new Dictionary<string, PhotonBandwidthGui.UsageTotal>();

	// Token: 0x04003AB9 RID: 15033
	private Dictionary<string, PhotonBandwidthGui.UsageTotal> rpcSerializationTotals = new Dictionary<string, PhotonBandwidthGui.UsageTotal>();

	// Token: 0x04003ABA RID: 15034
	private Dictionary<string, PhotonBandwidthGui.UsageTotal> eventSerializationTotals = new Dictionary<string, PhotonBandwidthGui.UsageTotal>();

	// Token: 0x04003ABB RID: 15035
	private Dictionary<string, int> usageFrames = new Dictionary<string, int>();

	// Token: 0x04003ABC RID: 15036
	private Dictionary<int, int> frameSizes = new Dictionary<int, int>();

	// Token: 0x04003ABD RID: 15037
	private Dictionary<Player, List<PhotonBandwidthGui.PlayerStat>> playerStats = new Dictionary<Player, List<PhotonBandwidthGui.PlayerStat>>();

	// Token: 0x04003ABE RID: 15038
	private Vector2 serializationScrollPosition = default(Vector2);

	// Token: 0x04003ABF RID: 15039
	private Texture2D usageGraph;

	// Token: 0x04003AC0 RID: 15040
	private Texture2D[] playerGraphs = new Texture2D[128];

	// Token: 0x04003AC1 RID: 15041
	public GUIStyle textStyle;

	// Token: 0x04003AC2 RID: 15042
	private double lastNetworkTime = double.NaN;

	// Token: 0x04003AC3 RID: 15043
	private int _frame;

	// Token: 0x04003AC4 RID: 15044
	private static bool _shouldRecord;

	// Token: 0x04003AC5 RID: 15045
	private const int graphWidth = 50;

	// Token: 0x04003AC6 RID: 15046
	private const int graphHeight = 50;

	// Token: 0x02000AB8 RID: 2744
	private enum UsageType
	{
		// Token: 0x04003ACF RID: 15055
		Value,
		// Token: 0x04003AD0 RID: 15056
		Object,
		// Token: 0x04003AD1 RID: 15057
		RPC,
		// Token: 0x04003AD2 RID: 15058
		Event,
		// Token: 0x04003AD3 RID: 15059
		Player
	}

	// Token: 0x02000AB9 RID: 2745
	private struct PlayerStat
	{
		// Token: 0x04003AD4 RID: 15060
		public int frame;

		// Token: 0x04003AD5 RID: 15061
		public float quality;

		// Token: 0x04003AD6 RID: 15062
		public float ping;
	}

	// Token: 0x02000ABA RID: 2746
	private struct UsageData
	{
		// Token: 0x04003AD7 RID: 15063
		public GameObject obj;

		// Token: 0x04003AD8 RID: 15064
		public int bytes;

		// Token: 0x04003AD9 RID: 15065
		public int frame;

		// Token: 0x04003ADA RID: 15066
		public PhotonBandwidthGui.UsageType type;

		// Token: 0x04003ADB RID: 15067
		public string name;

		// Token: 0x04003ADC RID: 15068
		public bool addToTotal;
	}

	// Token: 0x02000ABB RID: 2747
	private class UsageTotal
	{
		// Token: 0x04003ADD RID: 15069
		public int total;

		// Token: 0x04003ADE RID: 15070
		public int maximum;

		// Token: 0x04003ADF RID: 15071
		public GameObject obj;
	}

	// Token: 0x02000ABC RID: 2748
	private enum EventTypes
	{
		// Token: 0x04003AE1 RID: 15073
		VoiceDataReceived = 1,
		// Token: 0x04003AE2 RID: 15074
		RPC = 200,
		// Token: 0x04003AE3 RID: 15075
		SendSerialize,
		// Token: 0x04003AE4 RID: 15076
		Instantiation,
		// Token: 0x04003AE5 RID: 15077
		CloseConnection,
		// Token: 0x04003AE6 RID: 15078
		Destroy,
		// Token: 0x04003AE7 RID: 15079
		RemoveCachedRPCs,
		// Token: 0x04003AE8 RID: 15080
		SendSerializeReliable,
		// Token: 0x04003AE9 RID: 15081
		DestroyPlayer,
		// Token: 0x04003AEA RID: 15082
		AssignMaster,
		// Token: 0x04003AEB RID: 15083
		OwnershipRequest,
		// Token: 0x04003AEC RID: 15084
		OwnershipTransfer,
		// Token: 0x04003AED RID: 15085
		VacantViewIds
	}
}
