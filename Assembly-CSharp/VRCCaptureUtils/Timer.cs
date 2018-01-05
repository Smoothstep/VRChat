using System;
using UnityEngine;
using VRC;
using VRCSDK2;

namespace VRCCaptureUtils
{
	// Token: 0x020009F8 RID: 2552
	public class Timer : MonoBehaviour
	{
		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06004D9C RID: 19868 RVA: 0x001A0C69 File Offset: 0x0019F069
		public bool isRunning
		{
			get
			{
				return this.shouldRunTimer;
			}
		}

		// Token: 0x06004D9D RID: 19869 RVA: 0x001A0C71 File Offset: 0x0019F071
		public void StartTimer()
		{
			this.shouldRunTimer = true;
		}

		// Token: 0x06004D9E RID: 19870 RVA: 0x001A0C7A File Offset: 0x0019F07A
		public void StopTimer()
		{
			this.shouldRunTimer = false;
		}

		// Token: 0x06004D9F RID: 19871 RVA: 0x001A0C83 File Offset: 0x0019F083
		public void ResetTimer()
		{
			this.timer = 0f;
		}

		// Token: 0x06004DA0 RID: 19872 RVA: 0x001A0C90 File Offset: 0x0019F090
		public bool IsTimerDone()
		{
			return this.timer == this.time && this.shouldRunTimer;
		}

		// Token: 0x06004DA1 RID: 19873 RVA: 0x001A0CAC File Offset: 0x0019F0AC
		public void RunTimer()
		{
			if (!base.enabled || !this.shouldRunTimer)
			{
				return;
			}
			if (this.time > 0f && this.timer != this.time)
			{
				this.timer += Time.deltaTime;
				if (this.timer > this.time)
				{
					this.timer = this.time;
					this.OnTimerDone();
				}
			}
		}

		// Token: 0x06004DA2 RID: 19874 RVA: 0x001A0D28 File Offset: 0x0019F128
		private void OnTimerDone()
		{
			if (VRC.Network.IsMaster && this.onTimerDoneTrigger != null && !string.IsNullOrEmpty(this.onTimerDoneEvent.ParameterString))
			{
				VRC_Trigger.TriggerCustom(this.onTimerDoneTrigger.gameObject, this.onTimerDoneEvent.ParameterString);
			}
			if (this.resetAndStopTimerWhenDone)
			{
				this.ResetTimer();
				this.StopTimer();
				if (this.repeat)
				{
					this.StartTimer();
				}
			}
		}

		// Token: 0x06004DA3 RID: 19875 RVA: 0x001A0DA8 File Offset: 0x0019F1A8
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.TargetPlayer
		})]
		private void SetTimer(float t, bool isTimerRunning, VRC.Player instigator)
		{
			if (instigator.isMaster)
			{
				this.timer = t;
				this.shouldRunTimer = isTimerRunning;
			}
		}

		// Token: 0x06004DA4 RID: 19876 RVA: 0x001A0DC4 File Offset: 0x0019F1C4
		public override string ToString()
		{
			string result = "Should NOT run Timer with time " + this.timer;
			if (this.shouldRunTimer)
			{
				result = "Should run Timer with time " + this.timer;
			}
			return result;
		}

		// Token: 0x06004DA5 RID: 19877 RVA: 0x001A0E09 File Offset: 0x0019F209
		private void OnNetworkReady()
		{
			this.isInit = true;
			if (this.startTimerOnAwake)
			{
				this.StartTimer();
			}
		}

		// Token: 0x06004DA6 RID: 19878 RVA: 0x001A0E23 File Offset: 0x0019F223
		private void OnEnable()
		{
			if (!this.isInit && VRC.Network.IsObjectReady(base.gameObject))
			{
				this.isInit = true;
			}
			if (this.startTimerOnAwake)
			{
				this.StartTimer();
			}
		}

		// Token: 0x06004DA7 RID: 19879 RVA: 0x001A0E58 File Offset: 0x0019F258
		private void Update()
		{
			if (!this.isInit)
			{
				return;
			}
			this.RunTimer();
		}

		// Token: 0x06004DA8 RID: 19880 RVA: 0x001A0E6C File Offset: 0x0019F26C
		private void OnPlayerJoined(VRC_PlayerApi player)
		{
			if (VRC.Network.IsMaster)
			{
				VRC.Network.RPC(player.GetComponent<VRC.Player>(), base.gameObject, "SetTimer", new object[]
				{
					this.timer,
					this.shouldRunTimer
				});
			}
		}

		// Token: 0x040035AA RID: 13738
		private VRC_EventHandler eventHandler;

		// Token: 0x040035AB RID: 13739
		public float time = 5f;

		// Token: 0x040035AC RID: 13740
		[HideInInspector]
		public float timer;

		// Token: 0x040035AD RID: 13741
		private bool shouldRunTimer;

		// Token: 0x040035AE RID: 13742
		public bool startTimerOnAwake = true;

		// Token: 0x040035AF RID: 13743
		public bool repeat;

		// Token: 0x040035B0 RID: 13744
		[HideInInspector]
		public bool resetAndStopTimerWhenDone = true;

		// Token: 0x040035B1 RID: 13745
		public VRC_Trigger onTimerDoneTrigger;

		// Token: 0x040035B2 RID: 13746
		public VRC_EventHandler.VrcEvent onTimerDoneEvent;

		// Token: 0x040035B3 RID: 13747
		private bool isInit;
	}
}
