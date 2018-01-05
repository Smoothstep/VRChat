using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000AD2 RID: 2770
public class PlayerNet : VRCPunBehaviour
{
	// Token: 0x17000C34 RID: 3124
	// (get) Token: 0x0600542B RID: 21547 RVA: 0x001D12F6 File Offset: 0x001CF6F6
	public VRC.Player player
	{
		get
		{
			return (!(this.vrcPlayer == null)) ? this.vrcPlayer.player : null;
		}
	}

	// Token: 0x17000C35 RID: 3125
	// (get) Token: 0x0600542C RID: 21548 RVA: 0x001D131A File Offset: 0x001CF71A
	public PlayerNet playerNet
	{
		get
		{
			return (!(this.vrcPlayer == null)) ? this.vrcPlayer.playerNet : null;
		}
	}

	// Token: 0x17000C36 RID: 3126
	// (get) Token: 0x0600542D RID: 21549 RVA: 0x001D133E File Offset: 0x001CF73E
	public bool WasRecentlyDiscontinuous
	{
		get
		{
			return this.positionSync == null || this.positionSync.WasDiscontinuousRecently;
		}
	}

	// Token: 0x17000C37 RID: 3127
	// (get) Token: 0x0600542E RID: 21550 RVA: 0x001D135F File Offset: 0x001CF75F
	public short Ping
	{
		get
		{
			return this._ping;
		}
	}

	// Token: 0x17000C38 RID: 3128
	// (get) Token: 0x0600542F RID: 21551 RVA: 0x001D1367 File Offset: 0x001CF767
	public short PingVariance
	{
		get
		{
			return this._pingVariance;
		}
	}

	// Token: 0x17000C39 RID: 3129
	// (get) Token: 0x06005430 RID: 21552 RVA: 0x001D136F File Offset: 0x001CF76F
	public float TransitTimeAverageMS
	{
		get
		{
			return this._transitTimeAvgMS;
		}
	}

	// Token: 0x17000C3A RID: 3130
	// (get) Token: 0x06005431 RID: 21553 RVA: 0x001D1378 File Offset: 0x001CF778
	public float ConnectionQuality
	{
		get
		{
			if (this._frames.Count == 0)
			{
				return 0f;
			}
			if (this._connectionQuality != null)
			{
				return this._connectionQuality.Value;
			}
			int num = (from f in this._frames
			where f.expected
			select f).Count<PlayerNet.Frame>();
			this._connectionQuality = new float?((!this.vrcPlayer.isLocal) ? ((float)num / (float)this._frames.Count) : 1f);
			return this._connectionQuality.Value;
		}
	}

	// Token: 0x17000C3B RID: 3131
	// (get) Token: 0x06005432 RID: 21554 RVA: 0x001D1424 File Offset: 0x001CF824
	public float ConnectionDisparity
	{
		get
		{
			if (this._receiveTimes.Count < 2)
			{
				return 0f;
			}
			double num = 0.0;
			for (int i = 1; i < this._receiveTimes.Count; i++)
			{
				double num2 = this._receiveTimes[i - 1];
				double num3 = this._receiveTimes[i];
				num += num3 - num2;
			}
			double num4 = num / (double)(this._receiveTimes.Count - 1);
			return (float)(num4 - VRC.Network.ExpectedInterval);
		}
	}

	// Token: 0x06005433 RID: 21555 RVA: 0x001D14AC File Offset: 0x001CF8AC
	public override void Awake()
	{
		base.Awake();
		this.vrcPlayer = base.GetComponent<VRCPlayer>();
		if (this.vrcPlayer == null)
		{
			Debug.LogError("PlayerNet present on object without VRCPlayer in hierarchy.");
			UnityEngine.Object.Destroy(this);
			return;
		}
		VRCMotionState motionState = base.GetComponent<VRCMotionState>();
		this.positionSync = base.gameObject.GetOrAddComponent<SyncPhysics>();
		if (motionState != null)
		{
			this.positionSync.GetVelocity = delegate
			{
				if (this.isMine)
				{
					return motionState.PlayerVelocity;
				}
				return (this.positionSync.LastReplicatedPosition != null) ? this.positionSync.LastReplicatedPosition.Velocity : this.positionSync.ObservedVelocity;
			};
			this.positionSync.SetVelocity = delegate(Vector3 velocity)
			{
				if (velocity.magnitude < 0.0001f)
				{
					if (this.positionSync.ObservedVelocity.AlmostEquals(Vector3.zero, 0.0001f))
					{
						motionState.PlayerVelocity = Vector3.zero;
					}
					else
					{
						motionState.PlayerVelocity = velocity.normalized * 0.0001f;
					}
				}
				else
				{
					motionState.PlayerVelocity = velocity;
				}
			};
		}
		else
		{
			this.positionSync.GetVelocity = (() => this.positionSync.ObservedVelocity);
		}
	}

	// Token: 0x06005434 RID: 21556 RVA: 0x001D1574 File Offset: 0x001CF974
	public override IEnumerator Start()
	{
        yield return base.Start();
		base.ObserveThis();
		yield break;
	}

	// Token: 0x06005435 RID: 21557 RVA: 0x001D158F File Offset: 0x001CF98F
	private void Update()
	{
		this.positionSync.ReplicateVelocity = (base.transform.parent == null);
	}

	// Token: 0x06005436 RID: 21558 RVA: 0x001D15AD File Offset: 0x001CF9AD
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		VRC_Trigger.Trigger(hit.collider.gameObject, VRC_Trigger.TriggerType.OnAvatarHit);
	}

	// Token: 0x06005437 RID: 21559 RVA: 0x001D15C4 File Offset: 0x001CF9C4
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.Master
	})]
	private void KillPortal(Vector3 pos, Vector3 fwd)
	{
		RaycastHit raycastHit;
		if (!Physics.Raycast(pos, fwd, out raycastHit, 100f))
		{
			return;
		}
		PortalInternal component = raycastHit.collider.gameObject.GetComponent<PortalInternal>();
		if (component == null)
		{
			return;
		}
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.Master, component.gameObject, "KillPortal", new object[0]);
	}

	// Token: 0x06005438 RID: 21560 RVA: 0x001D161C File Offset: 0x001CFA1C
	public void SetAnimatorBool(string name, bool value)
	{
		if (!base.isMine)
		{
			return;
		}
		if (this.vrcPlayer.avatarAnimator != null)
		{
			this.vrcPlayer.avatarAnimator.SetBool(name, value);
		}
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.Others, base.gameObject, "_SetAnimatorBool", new object[]
		{
			name,
			value
		});
	}

	// Token: 0x06005439 RID: 21561 RVA: 0x001D1681 File Offset: 0x001CFA81
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.Others
	})]
	private void _SetAnimatorBool(string name, bool value, VRC.Player instigator)
	{
		if (instigator == base.Owner && this.vrcPlayer.avatarAnimator != null)
		{
			this.vrcPlayer.avatarAnimator.SetBool(name, value);
		}
	}

	// Token: 0x0600543A RID: 21562 RVA: 0x001D16BC File Offset: 0x001CFABC
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			this._ping = (short)PhotonNetwork.GetPing();
			this._pingVariance = (short)PhotonNetwork.networkingPeer.RoundTripTimeVariance;
			this._qualityCounter += 1;
			stream.SendNext(this._ping);
			stream.SendNext(this._pingVariance);
			stream.SendNext(this._qualityCounter);
			stream.SendNext(PhotonNetwork.ServerTimestamp);
		}
		else
		{
			this._ping = (short)stream.ReceiveNext();
			this._pingVariance = (short)stream.ReceiveNext();
			short num = (short)stream.ReceiveNext();
			int num2 = (int)stream.ReceiveNext();
			if (this._qualityCounter + 1 != num || Time.time.AlmostEquals(this.lastTime, Time.smoothDeltaTime))
			{
				this._frames.Add(new PlayerNet.Frame
				{
					expected = false,
					time = Time.time
				});
			}
			else
			{
				this._frames.Add(new PlayerNet.Frame
				{
					expected = true,
					time = Time.time
				});
				this.lastTime = Time.time;
			}
			this._connectionQuality = null;
			this._qualityCounter = num;
			this._receiveTimes.Add(info.timestamp);
			int transitTimeMs = PhotonNetwork.ServerTimestamp - num2;
			this.AccumulateTransitTimeAverage(transitTimeMs);
			int timestampDeltaMS = info.timestampInMilliseconds - num2;
			this.AccumulateTimestampDeltaAverage(timestampDeltaMS);
		}
		PhotonBandwidthGui.RecordPlayerStat(this.vrcPlayer.player);
	}

	// Token: 0x0600543B RID: 21563 RVA: 0x001D1860 File Offset: 0x001CFC60
	private void AccumulateTransitTimeAverage(int transitTimeMs)
	{
		this._transitTimeAvgFrameCount = Mathf.Min(this._transitTimeAvgFrameCount + 1, this.GetNumExpectedNetUpdatesForTimeInterval(10f));
		this._transitTimeAvgMS = (float)(this._transitTimeAvgFrameCount - 1) / (float)this._transitTimeAvgFrameCount * this._transitTimeAvgMS + 1f / (float)this._transitTimeAvgFrameCount * (float)transitTimeMs;
	}

	// Token: 0x0600543C RID: 21564 RVA: 0x001D18BC File Offset: 0x001CFCBC
	private void AccumulateTimestampDeltaAverage(int timestampDeltaMS)
	{
		this._timestampDeltaAvgFrameCount = Mathf.Min(this._timestampDeltaAvgFrameCount + 1, this.GetNumExpectedNetUpdatesForTimeInterval(10f));
		this._timestampDeltaAvgMS = (float)(this._timestampDeltaAvgFrameCount - 1) / (float)this._timestampDeltaAvgFrameCount * this._timestampDeltaAvgMS + 1f / (float)this._timestampDeltaAvgFrameCount * (float)timestampDeltaMS;
	}

	// Token: 0x0600543D RID: 21565 RVA: 0x001D1916 File Offset: 0x001CFD16
	private int GetNumExpectedNetUpdatesForTimeInterval(float interval)
	{
		return Mathf.CeilToInt((float)((double)interval / VRC.Network.SendInterval));
	}

	// Token: 0x0600543E RID: 21566 RVA: 0x001D1928 File Offset: 0x001CFD28
	private void RemoveOldFrames(List<float> frames)
	{
		float num = Time.time - 10f;
		for (int i = 0; i < frames.Count; i++)
		{
			if (frames[i] < num)
			{
				frames.RemoveRange(0, i);
				return;
			}
		}
	}

	// Token: 0x04003B61 RID: 15201
	public VRCPlayer vrcPlayer;

	// Token: 0x04003B62 RID: 15202
	private short _ping;

	// Token: 0x04003B63 RID: 15203
	private short _pingVariance;

	// Token: 0x04003B64 RID: 15204
	private short _qualityCounter;

	// Token: 0x04003B65 RID: 15205
	private LimitedCapacityList<PlayerNet.Frame> _frames = new LimitedCapacityList<PlayerNet.Frame>(100);

	// Token: 0x04003B66 RID: 15206
	private LimitedCapacityList<double> _receiveTimes = new LimitedCapacityList<double>(100);

	// Token: 0x04003B67 RID: 15207
	private const float _qualityAgeLimit = 10f;

	// Token: 0x04003B68 RID: 15208
	private int _transitTimeAvgFrameCount;

	// Token: 0x04003B69 RID: 15209
	private float _transitTimeAvgMS;

	// Token: 0x04003B6A RID: 15210
	private int _timestampDeltaAvgFrameCount;

	// Token: 0x04003B6B RID: 15211
	private float _timestampDeltaAvgMS;

	// Token: 0x04003B6C RID: 15212
	private float? _connectionQuality;

	// Token: 0x04003B6D RID: 15213
	private SyncPhysics positionSync;

	// Token: 0x04003B6E RID: 15214
	private float lastTime;

	// Token: 0x02000AD3 RID: 2771
	private class Frame
	{
		// Token: 0x04003B70 RID: 15216
		public float time;

		// Token: 0x04003B71 RID: 15217
		public bool expected;
	}
}
