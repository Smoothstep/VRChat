using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x0200053E RID: 1342
	[AddComponentMenu("Klak/Wiring/Switching/Threshold")]
	public class Threshold : NodeBase
	{
		// Token: 0x17000717 RID: 1815
		// (set) Token: 0x06002EB5 RID: 11957 RVA: 0x000E3918 File Offset: 0x000E1D18
		[Inlet]
		public float input
		{
			set
			{
				if (!base.enabled)
				{
					return;
				}
				this._currentValue = value;
				if (this._currentValue >= this._threshold && this._currentState != Threshold.State.Enabled)
				{
					this._onEvent.Invoke();
					this._currentState = Threshold.State.Enabled;
				}
			}
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x000E3968 File Offset: 0x000E1D68
		private void Update()
		{
			if (this._currentValue >= this._threshold)
			{
				this._delayTimer = 0f;
			}
			else if (this._currentValue < this._threshold && this._currentState != Threshold.State.Disabled)
			{
				this._delayTimer += Time.deltaTime;
				if (this._delayTimer >= this._delayToOff)
				{
					this._offEvent.Invoke();
					this._currentState = Threshold.State.Disabled;
				}
			}
		}

		// Token: 0x040018E6 RID: 6374
		[SerializeField]
		private float _threshold = 0.01f;

		// Token: 0x040018E7 RID: 6375
		[SerializeField]
		private float _delayToOff;

		// Token: 0x040018E8 RID: 6376
		[SerializeField]
		[Outlet]
		private NodeBase.VoidEvent _onEvent = new NodeBase.VoidEvent();

		// Token: 0x040018E9 RID: 6377
		[SerializeField]
		[Outlet]
		private NodeBase.VoidEvent _offEvent = new NodeBase.VoidEvent();

		// Token: 0x040018EA RID: 6378
		private Threshold.State _currentState;

		// Token: 0x040018EB RID: 6379
		private float _currentValue;

		// Token: 0x040018EC RID: 6380
		private float _delayTimer;

		// Token: 0x0200053F RID: 1343
		private enum State
		{
			// Token: 0x040018EE RID: 6382
			Dormant,
			// Token: 0x040018EF RID: 6383
			Enabled,
			// Token: 0x040018F0 RID: 6384
			Disabled
		}
	}
}
