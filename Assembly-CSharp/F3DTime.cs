using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000488 RID: 1160
public class F3DTime : MonoBehaviour
{
	// Token: 0x06002806 RID: 10246 RVA: 0x000D0595 File Offset: 0x000CE995
	private void Awake()
	{
		F3DTime.time = this;
		this.timers = new List<F3DTime.Timer>();
		this.removalPending = new List<int>();
	}

	// Token: 0x06002807 RID: 10247 RVA: 0x000D05B3 File Offset: 0x000CE9B3
	public int AddTimer(float rate, Action callBack)
	{
		return this.AddTimer(rate, 0, callBack);
	}

	// Token: 0x06002808 RID: 10248 RVA: 0x000D05C0 File Offset: 0x000CE9C0
	public int AddTimer(float rate, int ticks, Action callBack)
	{
		F3DTime.Timer timer = new F3DTime.Timer(++this.idCounter, rate, ticks, callBack);
		this.timers.Add(timer);
		return timer.id;
	}

	// Token: 0x06002809 RID: 10249 RVA: 0x000D05F9 File Offset: 0x000CE9F9
	public void RemoveTimer(int timerId)
	{
		this.removalPending.Add(timerId);
	}

	// Token: 0x0600280A RID: 10250 RVA: 0x000D0608 File Offset: 0x000CEA08
	private void Remove()
	{
		if (this.removalPending.Count > 0)
		{
			foreach (int num in this.removalPending)
			{
				for (int i = 0; i < this.timers.Count; i++)
				{
					if (this.timers[i].id == num)
					{
						this.timers.RemoveAt(i);
						break;
					}
				}
			}
			this.removalPending.Clear();
		}
	}

	// Token: 0x0600280B RID: 10251 RVA: 0x000D06C0 File Offset: 0x000CEAC0
	private void Tick()
	{
		for (int i = 0; i < this.timers.Count; i++)
		{
			this.timers[i].Tick();
		}
	}

	// Token: 0x0600280C RID: 10252 RVA: 0x000D06FA File Offset: 0x000CEAFA
	private void Update()
	{
		this.Remove();
		this.Tick();
	}

	// Token: 0x04001648 RID: 5704
	public static F3DTime time;

	// Token: 0x04001649 RID: 5705
	private List<F3DTime.Timer> timers;

	// Token: 0x0400164A RID: 5706
	private List<int> removalPending;

	// Token: 0x0400164B RID: 5707
	private int idCounter;

	// Token: 0x02000489 RID: 1161
	private class Timer
	{
		// Token: 0x0600280D RID: 10253 RVA: 0x000D0708 File Offset: 0x000CEB08
		public Timer(int id_, float rate_, int ticks_, Action callback_)
		{
			this.id = id_;
			this.rate = ((rate_ >= 0f) ? rate_ : 0f);
			this.ticks = ((ticks_ >= 0) ? ticks_ : 0);
			this.callBack = callback_;
			this.last = 0f;
			this.ticksElapsed = 0;
			this.isActive = true;
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x000D0774 File Offset: 0x000CEB74
		public void Tick()
		{
			this.last += Time.deltaTime;
			if (this.isActive && this.last >= this.rate)
			{
				this.last = 0f;
				this.ticksElapsed++;
				this.callBack();
				if (this.ticks > 0 && this.ticks == this.ticksElapsed)
				{
					this.isActive = false;
					F3DTime.time.RemoveTimer(this.id);
				}
			}
		}

		// Token: 0x0400164C RID: 5708
		public int id;

		// Token: 0x0400164D RID: 5709
		public bool isActive;

		// Token: 0x0400164E RID: 5710
		public float rate;

		// Token: 0x0400164F RID: 5711
		public int ticks;

		// Token: 0x04001650 RID: 5712
		public int ticksElapsed;

		// Token: 0x04001651 RID: 5713
		public float last;

		// Token: 0x04001652 RID: 5714
		public Action callBack;
	}
}
