using System;
using Klak.Math;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000541 RID: 1345
	[AddComponentMenu("Klak/Wiring/Switching/Toggle Four")]
	public class ToggleFour : NodeBase
	{
		// Token: 0x17000719 RID: 1817
		// (set) Token: 0x06002EBC RID: 11964 RVA: 0x000E3B48 File Offset: 0x000E1F48
		[Inlet]
		public float trigger
		{
			set
			{
				if (!base.enabled)
				{
					return;
				}
				this._state = (this._state + 1) % this._stateCount;
				switch (this._state)
				{
				case 0:
					this._value.targetValue = this._value1;
					this._state1Event.Invoke();
					break;
				case 1:
					this._value.targetValue = this._value2;
					this._state2Event.Invoke();
					break;
				case 2:
					this._value.targetValue = this._value3;
					this._state3Event.Invoke();
					break;
				default:
					this._value.targetValue = this._value4;
					this._state4Event.Invoke();
					break;
				}
			}
		}

		// Token: 0x06002EBD RID: 11965 RVA: 0x000E3C18 File Offset: 0x000E2018
		private void Start()
		{
			this._value = new FloatInterpolator(this._value1, this._interpolator);
			if (this._sendOnStartUp)
			{
				this._state1Event.Invoke();
			}
		}

		// Token: 0x06002EBE RID: 11966 RVA: 0x000E3C47 File Offset: 0x000E2047
		private void Update()
		{
			this._valueEvent.Invoke(this._value.Step());
		}

		// Token: 0x040018FA RID: 6394
		[SerializeField]
		[Range(2f, 4f)]
		private int _stateCount = 4;

		// Token: 0x040018FB RID: 6395
		[SerializeField]
		private float _value1;

		// Token: 0x040018FC RID: 6396
		[SerializeField]
		private float _value2 = 1f;

		// Token: 0x040018FD RID: 6397
		[SerializeField]
		private float _value3 = 2f;

		// Token: 0x040018FE RID: 6398
		[SerializeField]
		private float _value4 = 3f;

		// Token: 0x040018FF RID: 6399
		[SerializeField]
		private FloatInterpolator.Config _interpolator;

		// Token: 0x04001900 RID: 6400
		[SerializeField]
		private bool _sendOnStartUp;

		// Token: 0x04001901 RID: 6401
		[SerializeField]
		[Outlet]
		private NodeBase.VoidEvent _state1Event = new NodeBase.VoidEvent();

		// Token: 0x04001902 RID: 6402
		[SerializeField]
		[Outlet]
		private NodeBase.VoidEvent _state2Event = new NodeBase.VoidEvent();

		// Token: 0x04001903 RID: 6403
		[SerializeField]
		[Outlet]
		private NodeBase.VoidEvent _state3Event = new NodeBase.VoidEvent();

		// Token: 0x04001904 RID: 6404
		[SerializeField]
		[Outlet]
		private NodeBase.VoidEvent _state4Event = new NodeBase.VoidEvent();

		// Token: 0x04001905 RID: 6405
		[SerializeField]
		[Outlet]
		private NodeBase.FloatEvent _valueEvent = new NodeBase.FloatEvent();

		// Token: 0x04001906 RID: 6406
		private int _state;

		// Token: 0x04001907 RID: 6407
		private FloatInterpolator _value;
	}
}
