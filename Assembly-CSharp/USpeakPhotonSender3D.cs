using System;
using System.Collections;
using System.Collections.Generic;
using MoPhoGames.USpeak.Core;
using MoPhoGames.USpeak.Interface;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x020009E5 RID: 2533
public class USpeakPhotonSender3D : VRCPunBehaviour, ISpeechDataHandler
{
	// Token: 0x17000B9C RID: 2972
	// (get) Token: 0x06004CF9 RID: 19705 RVA: 0x0019C8C6 File Offset: 0x0019ACC6
	public bool IsInitialized
	{
		get
		{
			return this.initialized;
		}
	}

	// Token: 0x06004CFA RID: 19706 RVA: 0x0019C8D0 File Offset: 0x0019ACD0
	public override void Awake()
	{
		base.Awake();
		this.spk = USpeaker.Get(this);
		if (base.photonView.isMine)
		{
			this.spk.SpeakerMode = SpeakerMode.Local;
		}
		else
		{
			this.spk.SpeakerMode = SpeakerMode.Remote;
		}
	}

	// Token: 0x06004CFB RID: 19707 RVA: 0x0019C91C File Offset: 0x0019AD1C
	public override IEnumerator Start()
	{
		yield return base.Start();
		this.player = base.photonView.owner;
		if (this.player != null)
		{
			USpeakPhotonManager3D.TNetSpeakerMap.Add(this.player, this.spk);
		}
		else
		{
			Debug.LogError("PhotonView.owner (player) is null. Not sure how this even happens.");
		}
		yield break;
	}

	// Token: 0x06004CFC RID: 19708 RVA: 0x0019C937 File Offset: 0x0019AD37
	private void OnEnable()
	{
		this.ResetState();
		PhotonNetwork.OnEventCall = (PhotonNetwork.EventCallback)Delegate.Combine(PhotonNetwork.OnEventCall, new PhotonNetwork.EventCallback(this.OnPhotonEventHandler));
	}

	// Token: 0x06004CFD RID: 19709 RVA: 0x0019C95F File Offset: 0x0019AD5F
	private void OnDisable()
	{
		PhotonNetwork.OnEventCall = (PhotonNetwork.EventCallback)Delegate.Remove(PhotonNetwork.OnEventCall, new PhotonNetwork.EventCallback(this.OnPhotonEventHandler));
	}

	// Token: 0x06004CFE RID: 19710 RVA: 0x0019C984 File Offset: 0x0019AD84
	private void ResetState()
	{
		this._receiveWindowStartIndex = -1;
		this._numQueuedPackets = 0;
		this._dispatchStartDelay = 0f;
		this._dispatchStartTime = 0f;
		this._firstPlaybackFrameIndex = -1;
		this._firstPacketDispatchTime = 0f;
		this._lastPacketReceivedAtTime = -1f;
		this.InitPacketBuffer();
	}

	// Token: 0x06004CFF RID: 19711 RVA: 0x0019C9D8 File Offset: 0x0019ADD8
	private void InitPacketBuffer()
	{
		this._queuedPackets = new List<byte[]>(1024);
		for (int i = 0; i < 1024; i++)
		{
			this._queuedPackets.Add(null);
		}
	}

	// Token: 0x06004D00 RID: 19712 RVA: 0x0019CA18 File Offset: 0x0019AE18
	private void Update()
	{
		if (this.DebugIsNetworkSimEnabled)
		{
			if (!PhotonNetwork.networkingPeer.IsSimulationEnabled)
			{
				PhotonNetwork.networkingPeer.IsSimulationEnabled = true;
				base.gameObject.AddMissingComponent<PhotonLagSimulationGui>();
			}
			PhotonNetwork.networkingPeer.NetworkSimulationSettings.IncomingLag = this.DebugLag;
			PhotonNetwork.networkingPeer.NetworkSimulationSettings.OutgoingLag = this.DebugLag;
			PhotonNetwork.networkingPeer.NetworkSimulationSettings.IncomingJitter = this.DebugJitter;
			PhotonNetwork.networkingPeer.NetworkSimulationSettings.OutgoingJitter = this.DebugJitter;
			PhotonNetwork.networkingPeer.NetworkSimulationSettings.IncomingLossPercentage = this.DebugLossPct;
			PhotonNetwork.networkingPeer.NetworkSimulationSettings.OutgoingLossPercentage = this.DebugLossPct;
		}
		if (this.spk.isInitialized)
		{
			this.initialized = true;
		}
		this.DispatchPackets();
		if (this.ResetPacketStats)
		{
			this._totalPackets = (this._totalPacketsMissed = 0u);
			this.ResetPacketStats = false;
		}
	}

	// Token: 0x06004D01 RID: 19713 RVA: 0x0019CB13 File Offset: 0x0019AF13
	private void OnLeftRoom()
	{
		if (this.player != null && USpeakPhotonManager3D.TNetSpeakerMap.ContainsKey(this.player))
		{
			USpeakPhotonManager3D.TNetSpeakerMap.Remove(this.player);
		}
	}

	// Token: 0x06004D02 RID: 19714 RVA: 0x0019CB48 File Offset: 0x0019AF48
	public void USpeakOnSerializeAudio(byte[] data)
	{
		int[] playersThatCanHearMe = this.GetPlayersThatCanHearMe();
		if (playersThatCanHearMe.Length > 0)
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
			raiseEventOptions.TargetActors = playersThatCanHearMe;
			raiseEventOptions.CachingOption = EventCaching.DoNotCache;
			raiseEventOptions.InterestGroup = 0;
			raiseEventOptions.Receivers = ((!this.spk.DebugPlayback) ? ReceiverGroup.Others : ReceiverGroup.All);
			raiseEventOptions.SequenceChannel = VRCPhotonEventSequence.GetNextVoiceChannel();
			raiseEventOptions.ForwardToWebhook = false;
			raiseEventOptions.Encrypt = false;
			byte[] array = new byte[data.Length + 2];
			int num = 0;
			ushort num2 = this.NextPacketID();
			Array.Copy(BitConverter.GetBytes(num2), array, 2);
			num += 2;
			Array.Copy(data, 0, array, num, data.Length);
			bool sendReliable = true;
			PhotonNetwork.RaiseEvent(1, array, sendReliable, raiseEventOptions);
			if (this.spk.DebugLevel >= USpeakDebugLevel.Default)
			{
				string text = "{ ";
				foreach (int num3 in raiseEventOptions.TargetActors)
				{
					text = text + num3 + ",";
				}
				text += " }";
				this.spk.Log(string.Concat(new object[]
				{
					"USpeakOnSerializeAudio packet ",
					num2,
					" sent to ",
					raiseEventOptions.TargetActors.Length,
					" receivers: ",
					text,
					", ",
					data.Length,
					" bytes"
				}), 30f, USpeakDebugLevel.Default);
			}
		}
		else
		{
			this.spk.Log("USpeakOnSerializeAudio: no receivers, not sending! " + data.Length + " bytes", 30f, USpeakDebugLevel.Default);
		}
	}

	// Token: 0x06004D03 RID: 19715 RVA: 0x0019CCF8 File Offset: 0x0019B0F8
	public void USpeakInitializeSettings(int data)
	{
		if (!Networking.IsObjectReady(base.gameObject))
		{
			return;
		}
		if (this.settings == null || this.settings.Value != data)
		{
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.AllBufferOne, base.gameObject, "init", new object[]
			{
				data
			});
		}
	}

	// Token: 0x06004D04 RID: 19716 RVA: 0x0019CD57 File Offset: 0x0019B157
	public void USpeakInitializeSettingsForPlayer(int data, VRC.Player p)
	{
		if (!Networking.IsObjectReady(base.gameObject))
		{
			return;
		}
		VRC.Network.RPC(p, base.gameObject, "init", new object[]
		{
			data
		});
	}

	// Token: 0x06004D05 RID: 19717 RVA: 0x0019CD8C File Offset: 0x0019B18C
	private int[] GetPlayersThatCanHearMe()
	{
		List<VRC.Player> allPlayersWithinRange = PlayerManager.GetAllPlayersWithinRange(base.transform.position, this.distanceThreshold);
		if (!this.spk.DebugPlayback)
		{
			allPlayersWithinRange.RemoveAll((VRC.Player p) => p.GetPhotonPlayerId() == PhotonNetwork.player.ID);
		}
		if (VRCPlayer.Instance != null)
		{
			allPlayersWithinRange.RemoveAll((VRC.Player p) => VRCPlayer.Instance.GetSilencedLevelToPlayer(p.GetPhotonPlayerId()) > 0);
		}
		int[] array = new int[allPlayersWithinRange.Count];
		for (int i = 0; i < allPlayersWithinRange.Count; i++)
		{
			array[i] = allPlayersWithinRange[i].GetPhotonPlayerId();
		}
		return array;
	}

	// Token: 0x06004D06 RID: 19718 RVA: 0x0019CE50 File Offset: 0x0019B250
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.TargetPlayer,
		VRC_EventHandler.VrcTargetType.AllBufferOne
	})]
	public void init(int data, VRC.Player sender)
	{
		if (!VRC.Network.IsOwner(sender, base.gameObject))
		{
			return;
		}
		if (this.settings == null || this.settings.Value != data)
		{
			this.settings = new int?(data);
			this.spk.InitializeSettings(data);
			this.initialized = true;
		}
	}

	// Token: 0x06004D07 RID: 19719 RVA: 0x0019CEAF File Offset: 0x0019B2AF
	[PunRPC]
	public void receiveVoice(byte[] data, PhotonPlayer sender)
	{
		this.ReceiveVoiceInternal(data);
	}

	// Token: 0x06004D08 RID: 19720 RVA: 0x0019CEB8 File Offset: 0x0019B2B8
	private void ReceiveVoiceInternal(byte[] data)
	{
		if (!this.initialized)
		{
			this.spk.Log("ReceiveVoiceInternal: Not receiving voice. Not initialized", -1f, USpeakDebugLevel.Default);
			return;
		}
		if (VRCPlayer.Instance == null)
		{
			this.spk.Log("ReceiveVoiceInternal: Not receiving voice. VRCPlayer is null", -1f, USpeakDebugLevel.Default);
			return;
		}
		if (this == null)
		{
			this.spk.Log("ReceiveVoiceInternal: <this> pointer is null in recieve voice", -1f, USpeakDebugLevel.Default);
			return;
		}
		if (base.photonView == null || base.photonView.owner == null)
		{
			this.spk.Log("ReceiveVoiceInternal: PhotonView or owner null", -1f, USpeakDebugLevel.Default);
			return;
		}
		if (Vector3.Distance(base.transform.position, VRCPlayer.Instance.transform.position) > this.distanceThreshold)
		{
			this.spk.Log("ReceiveVoiceInternal: Not receiving voice. Distance too far", -1f, USpeakDebugLevel.Default);
			return;
		}
		if (this.spk == null || data == null)
		{
			this.spk.Log("ReceiveVoiceInternal: uSpeaker or data is null", -1f, USpeakDebugLevel.Default);
			return;
		}
		if (USpeakPhotonSender3D.MutedPlayers.Contains(base.photonView.owner.NickName))
		{
			this.spk.Log("ReceiveVoiceInternal: Player muted " + base.photonView.owner.ID, 20f, USpeakDebugLevel.Default);
			this.spk.MutedAudio(data);
			return;
		}
		this.spk.ReceiveAudio(data);
	}

	// Token: 0x06004D09 RID: 19721 RVA: 0x0019D040 File Offset: 0x0019B440
	private void OnPhotonEventHandler(byte eventCode, object content, int senderId)
	{
		if (eventCode != 1)
		{
			Debug.Log(string.Concat(new object[]
			{
				"uSpeak: OnPhotonEventHandler: not voice data ",
				eventCode,
				", sender ",
				senderId
			}));
			return;
		}
		if (content == null)
		{
			Debug.Log("uSpeak: OnPhotonEventHandler: content is null, sender " + senderId);
			return;
		}
		if (this == null)
		{
			Debug.Log("uSpeak: OnPhotonEventHandler: this component is null! sender " + senderId);
			return;
		}
		if (base.photonView == null || base.photonView.owner == null)
		{
			Debug.Log("uSpeak: OnPhotonEventHandler: photonView or owner is null! " + senderId);
			return;
		}
		if (senderId != base.photonView.owner.ID)
		{
			return;
		}
		byte[] array = (byte[])content;
		int num = 0;
		ushort packetID = BitConverter.ToUInt16(array, num);
		num += 2;
		byte[] array2 = new byte[array.Length - num];
		Array.Copy(array, num, array2, 0, array2.Length);
		this.QueuePacket(packetID, array2);
	}

	// Token: 0x06004D0A RID: 19722 RVA: 0x0019D148 File Offset: 0x0019B548
	private ushort NextPacketID()
	{
		ushort nextPacketID;
		this._nextPacketID = (nextPacketID = this._nextPacketID);
        this._nextPacketID += 1;
		return nextPacketID;
	}

	// Token: 0x06004D0B RID: 19723 RVA: 0x0019D167 File Offset: 0x0019B567
	private float GetPacketLossPct()
	{
		return (this._totalPackets <= 0u) ? 0f : (this._totalPacketsMissed / this._totalPackets);
	}

	// Token: 0x06004D0C RID: 19724 RVA: 0x0019D190 File Offset: 0x0019B590
	private void QueuePacket(ushort packetID, byte[] data)
	{
		int num = (int)(packetID % 1024);
		int num2 = BitConverter.ToInt32(data, 0);
		int num3 = Mathf.Max(PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds - num2, 0);
		if (this.spk.GetMaxValidPacketAgeMS() < (float)num3)
		{
			this.spk.Log(string.Concat(new object[]
			{
				"<color=white>QueuePacket: discarding old timestamp packet ",
				packetID,
				" at index ",
				num,
				", ",
				(float)num3 / 1000f,
				"s old > max age ",
				this.spk.GetMaxValidPacketAgeMS() / 1000f,
				"</color>"
			}), -1f, USpeakDebugLevel.Default);
			return;
		}
		if (this._lastPacketReceivedAtTime > 0f && Time.realtimeSinceStartup - this._lastPacketReceivedAtTime >= this.spk.GetMaxValidPacketAgeMS() / 1000f)
		{
			this._receiveWindowStartIndex = -1;
		}
		if (this._receiveWindowStartIndex < 0)
		{
			this.InitReceiveWindowFromIndex(num);
		}
		int num4 = num - this._receiveWindowStartIndex;
		if (num4 < 0)
		{
			num4 += 1024;
		}
		if (num4 >= 512)
		{
			int num5 = this._receiveWindowStartIndex - num;
			if (num5 < 0)
			{
				num5 += 1024;
			}
			float num6 = (float)num5 * this.spk.GetEstimatedPacketAudioLength();
			this.spk.Log(string.Concat(new object[]
			{
				"<color=white>QueuePacket (discarding old packet ",
				packetID,
				" at index ",
				num,
				", estimated ",
				num6,
				"s late, ",
				num5,
				" packets</color>"
			}), -1f, USpeakDebugLevel.Default);
			return;
		}
		if (this._queuedPackets[num] != null)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"<color=brown>uSpeak (",
				base.photonView.owner.ID,
				"): QueuePacket: already a valid packet at index ",
				num,
				", discarding new one"
			}));
			return;
		}
		this._queuedPackets[num] = data;
		this._numQueuedPackets++;
		this._lastPacketReceivedAtTime = Time.realtimeSinceStartup;
		this.spk.Log(string.Concat(new object[]
		{
			"<color=brown>QueuePacket: ",
			packetID,
			" at index ",
			num,
			", ",
			this._numQueuedPackets,
			" total, data length ",
			data.Length,
			", next packet index to send ",
			this._receiveWindowStartIndex,
			"</color>"
		}), -1f, USpeakDebugLevel.Full);
		this.spk.Log(string.Concat(new object[]
		{
			"Packet latency (",
			data.Length,
			" bytes): combined ping / actual timestamp delta: ",
			this.spk.GetTransitTimeFromRemoteMS(),
			" / ",
			num3,
			", ping ours/theirs: ",
			PhotonNetwork.GetPing(),
			"+-",
			PhotonNetwork.networkingPeer.RoundTripTimeVariance,
			" / ",
			this.spk.GetRemotePing(),
			"+-",
			this.spk.GetRemotePingVariance()
		}), 20f, USpeakDebugLevel.Default);
		if (!this.spk.IsVoicePlaying() && this._numQueuedPackets == 1)
		{
			this._dispatchStartTime = Time.realtimeSinceStartup;
			this._dispatchStartDelay = this.GetInitialPlaybackDelay();
			this._firstPlaybackFrameIndex = -1;
			this.spk.Log("<color=brown> - reset time until packet dispatch " + this._dispatchStartDelay + "</color>", -1f, USpeakDebugLevel.Full);
		}
	}

	// Token: 0x06004D0D RID: 19725 RVA: 0x0019D590 File Offset: 0x0019B990
	private void InitReceiveWindowFromIndex(int index)
	{
		this._receiveWindowStartIndex = index - Mathf.Min(100, Mathf.CeilToInt(this.spk.GetMaxValidPacketAgeMS() / (this.spk.GetEstimatedPacketAudioLength() * 1000f)));
		if (this._receiveWindowStartIndex < 0)
		{
			this._receiveWindowStartIndex += 1024;
		}
	}

	// Token: 0x06004D0E RID: 19726 RVA: 0x0019D5EC File Offset: 0x0019B9EC
	private void DispatchPackets()
	{
		while (this._numQueuedPackets > 0)
		{
			if (Time.realtimeSinceStartup - this._dispatchStartTime < this._dispatchStartDelay)
			{
				break;
			}
			int index = this.FindNextValidPacketIndex();
			byte[] source = this._queuedPackets[index];
			int num = (int)USpeakFrameContainer.ParseFrameIndex(source, 4);
			if (this._firstPlaybackFrameIndex < 0)
			{
				this.DispatchPacketAtIndex(index);
				this._firstPlaybackFrameIndex = num;
				this._firstPacketDispatchTime = Time.realtimeSinceStartup;
			}
			else
			{
				int num2 = Mathf.FloorToInt((Time.realtimeSinceStartup - this._firstPacketDispatchTime) * 1000f);
				int num3 = num - this._firstPlaybackFrameIndex;
				if (num3 < 0)
				{
					num3 += 65536;
				}
				int a = num3 * this.spk.DurationMs;
				int num4 = Mathf.Min(a, 500);
				if (num2 < num4)
				{
					break;
				}
				this.DispatchPacketAtIndex(index);
			}
		}
	}

	// Token: 0x06004D0F RID: 19727 RVA: 0x0019D6DC File Offset: 0x0019BADC
	private int FindNextValidPacketIndex()
	{
		byte[] array = null;
		int num = this._receiveWindowStartIndex;
		for (int i = 0; i < 1024; i++)
		{
			array = this._queuedPackets[num];
			if (array != null)
			{
				break;
			}
			num = (num + 1) % 1024;
		}
		return (array == null) ? -1 : num;
	}

	// Token: 0x06004D10 RID: 19728 RVA: 0x0019D738 File Offset: 0x0019BB38
	private void DispatchPacketAtIndex(int index)
	{
		byte[] data = this._queuedPackets[index];
		this._totalPackets += 1u;
		this.spk.Log(string.Concat(new object[]
		{
			"<color=lime>DispatchNextPacket: index ",
			index,
			", ",
			this._numQueuedPackets - 1,
			" remaining</color>"
		}), -1f, USpeakDebugLevel.Full);
		this.ReceiveVoiceInternal(data);
		this._queuedPackets[index] = null;
		this._receiveWindowStartIndex = (index + 1) % 1024;
		this._numQueuedPackets--;
	}

	// Token: 0x06004D11 RID: 19729 RVA: 0x0019D7DE File Offset: 0x0019BBDE
	private float GetInitialPlaybackDelay()
	{
		return this.spk.GetPlaybackDelayForLatency();
	}

	// Token: 0x040034F0 RID: 13552
	private USpeaker spk;

	// Token: 0x040034F1 RID: 13553
	private bool initialized;

	// Token: 0x040034F2 RID: 13554
	private PhotonPlayer player;

	// Token: 0x040034F3 RID: 13555
	public float distanceThreshold = 25f;

	// Token: 0x040034F4 RID: 13556
	public bool DebugIsNetworkSimEnabled;

	// Token: 0x040034F5 RID: 13557
	public int DebugLag = 100;

	// Token: 0x040034F6 RID: 13558
	public int DebugJitter;

	// Token: 0x040034F7 RID: 13559
	public int DebugLossPct = 2;

	// Token: 0x040034F8 RID: 13560
	public bool ResetPacketStats;

	// Token: 0x040034F9 RID: 13561
	private int? settings;

	// Token: 0x040034FA RID: 13562
	public static List<string> MutedPlayers = new List<string>();

	// Token: 0x040034FB RID: 13563
	private ushort _nextPacketID;

	// Token: 0x040034FC RID: 13564
	private uint _totalPackets;

	// Token: 0x040034FD RID: 13565
	private uint _totalPacketsMissed;

	// Token: 0x040034FE RID: 13566
	private const int PACKET_BUFFER_SIZE = 1024;

	// Token: 0x040034FF RID: 13567
	private List<byte[]> _queuedPackets = new List<byte[]>(1024);

	// Token: 0x04003500 RID: 13568
	private const int RECEIVE_WINDOW_SIZE = 512;

	// Token: 0x04003501 RID: 13569
	private const int PACKET_TIMESTAMP_OFFSET = 0;

	// Token: 0x04003502 RID: 13570
	private const int PACKET_FIRST_FRAME_OFFSET = 4;

	// Token: 0x04003503 RID: 13571
	private int _receiveWindowStartIndex = -1;

	// Token: 0x04003504 RID: 13572
	private int _numQueuedPackets;

	// Token: 0x04003505 RID: 13573
	private float _dispatchStartDelay;

	// Token: 0x04003506 RID: 13574
	private float _dispatchStartTime;

	// Token: 0x04003507 RID: 13575
	private int _firstPlaybackFrameIndex = -1;

	// Token: 0x04003508 RID: 13576
	private float _firstPacketDispatchTime;

	// Token: 0x04003509 RID: 13577
	private float _lastPacketReceivedAtTime = -1f;
}
