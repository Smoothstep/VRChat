using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using VRC;
using VRCSDK2;

// Token: 0x02000B68 RID: 2920
[RequireComponent(typeof(VideoPlayer))]
public class SyncVideoPlayer : VRCPunBehaviour
{
    // Token: 0x060059C8 RID: 22984 RVA: 0x001F276C File Offset: 0x001F0B6C
    public override void Awake()
	{
		base.Awake();
		this.syncVid = base.GetComponent<VRC_SyncVideoPlayer>();
		this.vidPlayer = base.GetComponent<VideoPlayer>();
		this.BlockReady();
		this.syncVid.Videos = (from v in this.syncVid.Videos
		where v != null && v.Source != VideoSource.VideoClip
		select v).ToArray<VRC_SyncVideoPlayer.VideoEntry>();
		if (this.vidPlayer != null)
		{
			this.vidPlayer.isLooping = false;
			this.vidPlayer.errorReceived += this.VidPlayer_errorReceived;
			Transform transform = base.transform.Find("Time");
			if (transform != null)
			{
				this.timeText = transform.GetComponentInChildren<Text>();
			}
			return;
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x060059C9 RID: 22985 RVA: 0x001F2844 File Offset: 0x001F0C44
	private void VidPlayer_errorReceived(VideoPlayer source, string message)
	{
		if (this.currentPlaylistIndex >= 0 && this.currentPlaylistIndex < this.playlist.Length)
		{
			VRC_SyncVideoPlayer.VideoEntry videoEntry = this.syncVid.Videos[this.currentPlaylistIndex];
			if (videoEntry.Source == VideoSource.Url)
			{
				this.foundURIs[videoEntry.URL].RemoveAt(0);
				if (this.foundURIs[videoEntry.URL].Count > 0)
				{
					this.vidPlayer.url = this.foundURIs[videoEntry.URL].FirstOrDefault<string>();
					if (this.isPlaying)
					{
						this.vidPlayer.Play();
					}
					return;
				}
			}
		}
		Debug.LogError(message);
		source.url = SyncVideoPlayer.silence;
	}

	// Token: 0x060059CA RID: 22986 RVA: 0x001F290C File Offset: 0x001F0D0C
	private IEnumerator FindURIs(string url, Action onDone = null)
	{
		if (this.foundURIs.ContainsKey(url))
		{
			this.foundURIs.Remove(url);
		}
		yield return YoutubeDL.FindBestVideoURL(url, false, delegate(string found)
		{
			if (!this.foundURIs.ContainsKey(url))
			{
				this.foundURIs.Add(url, new List<string>
				{
					found
				});
			}
			else
			{
				this.foundURIs[url].Add(found);
			}
		}, delegate(string err)
		{

    Debug.LogError(err);
		});
		if (!this.foundURIs.ContainsKey(url))
		{
			this.foundURIs.Add(url, new List<string>());
		}
		if (onDone != null)
		{
			onDone();
		}
		yield break;
	}

	// Token: 0x060059CB RID: 22987 RVA: 0x001F2938 File Offset: 0x001F0D38
	public override IEnumerator Start()
	{
		if (string.IsNullOrEmpty(SyncVideoPlayer.silence))
		{
			yield return this.FindURIs("https://ia800301.us.archive.org/0/items/electricsheep-flock-244-32500-3/00244=32593=23650=23640.mp4", delegate
			{
				SyncVideoPlayer.silence = this.foundURIs["https://ia800301.us.archive.org/0/items/electricsheep-flock-244-32500-3/00244=32593=23650=23640.mp4"].FirstOrDefault<string>();
			});
		}
		this.vidPlayer.url = SyncVideoPlayer.silence;
		this.vidPlayer.Play();
		foreach (VRC_SyncVideoPlayer.VideoEntry videoEntry in this.syncVid.Videos)
		{
			if (videoEntry.Source == VideoSource.Url)
			{
				base.StartCoroutine(this.FindURIs(videoEntry.URL, null));
			}
		}
        yield return base.Start();
		base.ObserveThis();
		yield return new WaitUntil(() => (from v in this.syncVid.Videos
		where v.Source == VideoSource.Url
		select v).All((VRC_SyncVideoPlayer.VideoEntry v) => this.foundURIs.ContainsKey(v.URL)));
		this.UnblockReady();
		if (base.isMine)
		{
			this.playlist = new int[this.syncVid.Videos.Length];
			this.ResetPlaylist();
			if (this.vidPlayer.playOnAwake)
			{
				this.PlayIndex(0);
			}
			else
			{
				this.SetPlaylistIndex(0);
			}
		}
		yield break;
	}

	// Token: 0x060059CC RID: 22988 RVA: 0x001F2953 File Offset: 0x001F0D53
	private void ShuffleOrReset()
	{
		if (this.shuffle)
		{
			this.ShufflePlaylist();
		}
		else
		{
			this.ResetPlaylist();
		}
	}

	// Token: 0x060059CD RID: 22989 RVA: 0x001F2974 File Offset: 0x001F0D74
	private void ResetPlaylist()
	{
		this.playlist = new int[this.syncVid.Videos.Length];
		for (int i = 0; i < this.syncVid.Videos.Length; i++)
		{
			this.playlist[i] = i;
		}
		Networking.RPC(VRC_EventHandler.VrcTargetType.OthersBufferOne, base.gameObject, "RemoteSetPlaylist", new object[]
		{
			this.shuffle,
			this.playlist
		});
	}

	// Token: 0x060059CE RID: 22990 RVA: 0x001F29F0 File Offset: 0x001F0DF0
	private void ShufflePlaylist()
	{
		int num = this.syncVid.Videos.Length;
		this.playlist = new int[num];
		for (int i = 0; i < num; i++)
		{
			int num2 = UnityEngine.Random.Range(0, num);
			if (num2 != i)
			{
				int num3 = this.playlist[i];
				this.playlist[i] = this.playlist[num2];
				this.playlist[num2] = num3;
			}
		}
		Networking.RPC(VRC_EventHandler.VrcTargetType.OthersBufferOne, base.gameObject, "RemoteSetPlaylist", new object[]
		{
			this.shuffle,
			this.playlist
		});
	}

	// Token: 0x060059CF RID: 22991 RVA: 0x001F2A88 File Offset: 0x001F0E88
	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			if (this.vidPlayer != null)
			{
				this.SendVidSync(stream);
			}
		}
		else if (this.vidPlayer != null)
		{
			this.ReceiveVidSync(stream);
		}
	}

	// Token: 0x060059D0 RID: 22992 RVA: 0x001F2AD8 File Offset: 0x001F0ED8
	private void SendVidSync(PhotonStream stream)
	{
		if (this.vidPlayer == null)
		{
			return;
		}
		stream.SendNext(this.isPlaying);
		if (this.isPlaying)
		{
			this.lastSyncTime = new double?(this.vidPlayer.time);
			stream.SendNext(this.vidPlayer.time);
			stream.SendNext((short)Mathf.FloatToHalf(this.vidPlayer.playbackSpeed));
			stream.SendNext((short)this.currentPlaylistIndex);
		}
	}

	// Token: 0x060059D1 RID: 22993 RVA: 0x001F2B70 File Offset: 0x001F0F70
	private void ReceiveVidSync(PhotonStream stream)
	{
		if (this.vidPlayer == null)
		{
			return;
		}
		bool flag = VRC.Network.IsObjectReady(base.gameObject);
		bool flag2 = (bool)stream.ReceiveNext();
		if (flag2 != this.isPlaying && flag)
		{
			if (flag2)
			{
				this.Play(true, 0.0);
			}
			if (!flag2)
			{
				this.Stop(true, true);
			}
		}
		if (flag2)
		{
			this.lastSyncTime = new double?((double)stream.ReceiveNext());
			float num = Mathf.HalfToFloat((ushort)((short)stream.ReceiveNext()));
			int num2 = (int)((short)stream.ReceiveNext());
			if (num2 != this.currentPlaylistIndex && flag)
			{
				this.Stop(true, true);
				this.SetVideoIndex(num2);
				this.Play(true, 0.0);
			}
			if (num != this.vidPlayer.playbackSpeed)
			{
				this.vidPlayer.playbackSpeed = num;
			}
		}
	}

	// Token: 0x060059D2 RID: 22994 RVA: 0x001F2C68 File Offset: 0x001F1068
	public void SetPlaylistIndex(int i)
	{
		if (i >= 0 && i < this.syncVid.Videos.Length)
		{
			int videoIndex = this.playlist[i];
			this.SetVideoIndex(videoIndex);
			this.currentPlaylistIndex = i;
		}
	}

	// Token: 0x060059D3 RID: 22995 RVA: 0x001F2CA8 File Offset: 0x001F10A8
	public void SetVideoIndex(int i)
	{
		if (i >= 0 && i < this.syncVid.Videos.Length)
		{
			if (this.vidPlayer.isPlaying)
			{
				this.vidPlayer.Stop();
			}
			Debug.Log(base.name + " play index: " + i);
			this.currentPlaylistIndex = i;
			VRC_SyncVideoPlayer.VideoEntry videoEntry = this.syncVid.Videos[i];
			if (videoEntry.Source == VideoSource.Url)
			{
				this.vidPlayer.url = this.foundURIs[videoEntry.URL].FirstOrDefault<string>();
			}
			this.vidPlayer.source = videoEntry.Source;
			this.vidPlayer.playbackSpeed = videoEntry.PlaybackSpeed;
			this.vidPlayer.loopPointReached += this.TrackFinished;
			if (this.isPlaying)
			{
				this.vidPlayer.Play();
			}
		}
	}

	// Token: 0x060059D4 RID: 22996 RVA: 0x001F2D98 File Offset: 0x001F1198
	public void Pause()
	{
		if (base.isMine)
		{
			if (this.isPlaying)
			{
				this.Stop(false, false);
			}
			else
			{
				this.Play(false, (this.lastSyncTime == null) ? 0.0 : this.lastSyncTime.Value);
			}
		}
	}

	// Token: 0x060059D5 RID: 22997 RVA: 0x001F2DF8 File Offset: 0x001F11F8
	public void SpeedUp()
	{
		if (base.isMine)
		{
			this.vidPlayer.playbackSpeed += 0.1f;
		}
	}

	// Token: 0x060059D6 RID: 22998 RVA: 0x001F2E1C File Offset: 0x001F121C
	public void SpeedDown()
	{
		if (base.isMine)
		{
			this.vidPlayer.playbackSpeed -= 0.1f;
		}
	}

	// Token: 0x060059D7 RID: 22999 RVA: 0x001F2E40 File Offset: 0x001F1240
	public void PlayIndex(int i)
	{
		if (base.isMine)
		{
			this.Stop(false, true);
			this.SetPlaylistIndex(i);
			this.Play(false, 0.0);
		}
	}

	// Token: 0x060059D8 RID: 23000 RVA: 0x001F2E6C File Offset: 0x001F126C
	public void Play(bool fromRemote, double startTime = 0.0)
	{
		if ((fromRemote || base.isMine) && !this.isPlaying)
		{
			this.isPlaying = true;
			Debug.LogFormat("Playing because {0}, starting with index {1}.", new object[]
			{
				(!base.isMine) ? "I was told to" : "it's mine",
				this.currentPlaylistIndex
			});
			this.SetVideoIndex(this.currentPlaylistIndex);
			this.vidPlayer.Play();
			if (startTime > 0.0)
			{
                
				base.StartCoroutine(this.SetPlaybackTime(startTime));
			}
		}
	}

	// Token: 0x060059D9 RID: 23001 RVA: 0x001F2F0C File Offset: 0x001F130C
	private IEnumerator SetPlaybackTime(double time)
	{
		yield return new WaitUntil(() => this.vidPlayer.isPlaying && this.vidPlayer.time > 0.0);
		this.vidPlayer.time = time;
		yield break;
	}

	// Token: 0x060059DA RID: 23002 RVA: 0x001F2F30 File Offset: 0x001F1330
	public void Stop(bool fromRemote, bool resetTime = true)
	{
		if (base.isMine || fromRemote)
		{
			this.isPlaying = false;
			this.vidPlayer.Stop();
			if (resetTime)
			{
				this.lastSyncTime = null;
			}
		}
	}

	// Token: 0x060059DB RID: 23003 RVA: 0x001F2F78 File Offset: 0x001F1378
	public void Next()
	{
		if (base.isMine)
		{
			this.currentPlaylistIndex++;
			if (this.currentPlaylistIndex >= this.syncVid.Videos.Length)
			{
				this.currentPlaylistIndex = 0;
			}
			this.PlayIndex(this.currentPlaylistIndex);
		}
	}

	// Token: 0x060059DC RID: 23004 RVA: 0x001F2FCC File Offset: 0x001F13CC
	public void Previous()
	{
		if (base.isMine)
		{
			this.currentPlaylistIndex--;
			if (this.currentPlaylistIndex < 0)
			{
				this.currentPlaylistIndex = this.syncVid.Videos.Length - 1;
			}
			this.PlayIndex(this.currentPlaylistIndex);
		}
	}

	// Token: 0x060059DD RID: 23005 RVA: 0x001F3020 File Offset: 0x001F1420
	public void Shuffle()
	{
		if (base.isMine)
		{
			int num = this.syncVid.Videos.Length;
			if (num < 2)
			{
				return;
			}
			this.shuffle = !this.shuffle;
			this.ShuffleOrReset();
			Networking.RPC(VRC_EventHandler.VrcTargetType.OthersBufferOne, base.gameObject, "RemoteSetPlaylist", new object[]
			{
				this.shuffle,
				this.playlist
			});
			this.currentPlaylistIndex = 0;
			this.PlayIndex(this.currentPlaylistIndex);
		}
		else
		{
			VRC.Network.RPC(base.Owner, base.gameObject, "ShuffleRPC", new object[0]);
		}
	}

	// Token: 0x060059DE RID: 23006 RVA: 0x001F30C5 File Offset: 0x001F14C5
	public void Clear()
	{
		if (base.isMine)
		{
			Networking.RPC(VRC_EventHandler.VrcTargetType.AllBufferOne, base.gameObject, "RemoteClear", new object[0]);
		}
	}

	// Token: 0x060059DF RID: 23007 RVA: 0x001F30EC File Offset: 0x001F14EC
	private IEnumerator InternalAddURL(string url)
	{
		yield return this.FindURIs(url, null);
		List<VRC_SyncVideoPlayer.VideoEntry> entries = new List<VRC_SyncVideoPlayer.VideoEntry>(this.syncVid.Videos);
		entries.Add(new VRC_SyncVideoPlayer.VideoEntry
		{
			AspectRatio = VideoAspectRatio.FitInside,
			PlaybackSpeed = 1f,
			Source = VideoSource.Url,
			URL = url,
			VideoClip = null
		});
		this.syncVid.Videos = entries.ToArray();
		this.ShuffleOrReset();
		Networking.RPC(VRC_EventHandler.VrcTargetType.OthersBufferOne, base.gameObject, "RemoteSetVideos", new object[]
		{
			this.syncVid.Videos,
			this.shuffle,
			this.playlist
		});
		yield break;
	}

	// Token: 0x060059E0 RID: 23008 RVA: 0x001F3110 File Offset: 0x001F1510
	public void AddURL(string url)
	{
		if (!base.isMine)
		{
			return;
		}
		if (this.syncVid.Videos.Any((VRC_SyncVideoPlayer.VideoEntry entry) => entry.Source == VideoSource.VideoClip))
		{
			Debug.LogError("Cannot add URLs to a synchronized video player if VideoClip entries are present.");
			return;
		}
		base.StartCoroutine(this.InternalAddURL(url));
	}

	// Token: 0x060059E1 RID: 23009 RVA: 0x001F3174 File Offset: 0x001F1574
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.OthersBufferOne
	})]
	private void RemoteSetVideos(VRC_SyncVideoPlayer.VideoEntry[] entries, bool is_shuffled, int[] newList, VRC.Player instigator)
	{
		if (base.Owner != instigator)
		{
			return;
		}
		if (entries == null)
		{
			Debug.LogError("Remote sent a null video list");
			return;
		}
		if (newList == null)
		{
			Debug.LogError("Remote sent a null playlist");
			return;
		}
		this.syncVid.Videos = entries;
		this.shuffle = is_shuffled;
		this.playlist = newList;
	}

	// Token: 0x060059E2 RID: 23010 RVA: 0x001F31D0 File Offset: 0x001F15D0
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void RemoteClear(VRC.Player instigator)
	{
		if (base.Owner != instigator)
		{
			return;
		}
		this.syncVid.Videos = new VRC_SyncVideoPlayer.VideoEntry[0];
		this.ShuffleOrReset();
	}

	// Token: 0x060059E3 RID: 23011 RVA: 0x001F31FB File Offset: 0x001F15FB
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.OthersBufferOne
	})]
	private void RemoteSetPlaylist(bool is_shuffled, int[] newList, VRC.Player instigator)
	{
		if (base.Owner != instigator)
		{
			return;
		}
		if (newList == null)
		{
			Debug.LogError("Remote sent a null playlist");
			return;
		}
		this.shuffle = is_shuffled;
		this.playlist = newList;
	}

	// Token: 0x060059E4 RID: 23012 RVA: 0x001F322E File Offset: 0x001F162E
	private void TrackFinished(VideoPlayer source)
	{
		this.queueNext = true;
	}

	// Token: 0x060059E5 RID: 23013 RVA: 0x001F3238 File Offset: 0x001F1638
	private void Update()
	{
		if (this.vidPlayer == null)
		{
			return;
		}
		if (this.timeText != null && this.timeText.enabled)
		{
			double time = this.vidPlayer.time;
			int num = Mathf.FloorToInt((float)(time / 3600.0));
			int num2 = Mathf.FloorToInt(((float)time - (float)(3600 * num)) / 60f);
			int num3 = Mathf.FloorToInt((float)time - (float)(3600 * num) - (float)(60 * num2));
			int num4 = Mathf.RoundToInt(1000f * ((float)time - (float)(3600 * num) - (float)(60 * num2) - (float)num3));
			this.timeText.text = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", new object[]
			{
				num,
				num2,
				num3,
				num4
			});
		}
		if (this.queueNext)
		{
			this.queueNext = false;
			this.Next();
		}
		if (!base.isMine && this.isPlaying && this.lastSyncTime != null)
		{
			double num5 = VRC.Network.SimulationDelay(base.Owner);
			if (Math.Abs(this.lastSyncTime.Value - this.vidPlayer.time) > num5 + 0.5 && this.vidPlayer.time > 0.0)
			{
				double num6 = this.lastSyncTime.Value + num5 * 0.5;
				Debug.LogFormat("Advancing {0} to {1}, was at {2}.", new object[]
				{
					base.gameObject.name,
					num6,
					this.vidPlayer.time
				});
				this.vidPlayer.time = num6;
			}
			this.lastSyncTime = null;
		}
	}

	// Token: 0x0400400E RID: 16398
	private VideoPlayer vidPlayer;

	// Token: 0x0400400F RID: 16399
	private VRC_SyncVideoPlayer syncVid;

	// Token: 0x04004010 RID: 16400
	private Text timeText;

	// Token: 0x04004011 RID: 16401
	private int currentPlaylistIndex;

	// Token: 0x04004012 RID: 16402
	private int[] playlist;

	// Token: 0x04004013 RID: 16403
	private bool shuffle;

	// Token: 0x04004014 RID: 16404
	private bool queueNext;

	// Token: 0x04004015 RID: 16405
	private bool isPlaying;

	// Token: 0x04004016 RID: 16406
	private double? lastSyncTime;

	// Token: 0x04004017 RID: 16407
	private const string silenceURI = "https://ia800301.us.archive.org/0/items/electricsheep-flock-244-32500-3/00244=32593=23650=23640.mp4";

	// Token: 0x04004018 RID: 16408
	private static string silence;

	// Token: 0x04004019 RID: 16409
	private Dictionary<string, List<string>> foundURIs = new Dictionary<string, List<string>>();
}
