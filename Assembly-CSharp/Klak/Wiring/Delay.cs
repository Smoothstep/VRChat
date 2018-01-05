using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000536 RID: 1334
	[AddComponentMenu("Klak/Wiring/Switching/Delay")]
	public class Delay : NodeBase
	{
		// Token: 0x06002E9D RID: 11933 RVA: 0x000E3464 File Offset: 0x000E1864
		[Inlet]
		public void Trigger()
		{
			this._timeQueue.Enqueue(this.CurrentTime);
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06002E9E RID: 11934 RVA: 0x000E3477 File Offset: 0x000E1877
		private float CurrentTime
		{
			get
			{
				return (this._timeUnit != Delay.TimeUnit.Second) ? ((float)Time.frameCount) : Time.time;
			}
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x000E3494 File Offset: 0x000E1894
		private void Update()
		{
			while (this._timeQueue.Count > 0 && this._timeQueue.Peek() + this._interval < this.CurrentTime)
			{
				this._outputEvent.Invoke();
				this._timeQueue.Dequeue();
			}
		}

		// Token: 0x040018C3 RID: 6339
		[SerializeField]
		private Delay.TimeUnit _timeUnit;

		// Token: 0x040018C4 RID: 6340
		[SerializeField]
		private float _interval = 1f;

		// Token: 0x040018C5 RID: 6341
		[SerializeField]
		[Outlet]
		private NodeBase.VoidEvent _outputEvent = new NodeBase.VoidEvent();

		// Token: 0x040018C6 RID: 6342
		private Queue<float> _timeQueue = new Queue<float>();

		// Token: 0x02000537 RID: 1335
		public enum TimeUnit
		{
			// Token: 0x040018C8 RID: 6344
			Second,
			// Token: 0x040018C9 RID: 6345
			Frame
		}
	}
}
