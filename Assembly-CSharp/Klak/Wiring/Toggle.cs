using System;
using Klak.Math;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000540 RID: 1344
	[AddComponentMenu("Klak/Wiring/Switching/Toggle")]
	public class Toggle : NodeBase
	{
		// Token: 0x17000718 RID: 1816
		// (set) Token: 0x06002EB8 RID: 11960 RVA: 0x000E3A1C File Offset: 0x000E1E1C
		[Inlet]
		public float trigger
		{
			set
			{
				if (!base.enabled)
				{
					return;
				}
				this._state = !this._state;
				if (this._state)
				{
					this._value.targetValue = this._onValue;
					this._onEvent.Invoke();
				}
				else
				{
					this._value.targetValue = this._offValue;
					this._offEvent.Invoke();
				}
			}
		}

		// Token: 0x06002EB9 RID: 11961 RVA: 0x000E3A8C File Offset: 0x000E1E8C
		private void Start()
		{
			this._value = new FloatInterpolator(this._offValue, this._interpolator);
			if (this._sendOnStartUp)
			{
				this._offEvent.Invoke();
			}
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x000E3ABB File Offset: 0x000E1EBB
		private void Update()
		{
			this._valueEvent.Invoke(this._value.Step());
		}

		// Token: 0x040018F1 RID: 6385
		[SerializeField]
		private float _offValue;

		// Token: 0x040018F2 RID: 6386
		[SerializeField]
		private float _onValue = 1f;

		// Token: 0x040018F3 RID: 6387
		[SerializeField]
		private FloatInterpolator.Config _interpolator;

		// Token: 0x040018F4 RID: 6388
		[SerializeField]
		private bool _sendOnStartUp;

		// Token: 0x040018F5 RID: 6389
		[SerializeField]
		[Outlet]
		private NodeBase.VoidEvent _offEvent = new NodeBase.VoidEvent();

		// Token: 0x040018F6 RID: 6390
		[SerializeField]
		[Outlet]
		private NodeBase.VoidEvent _onEvent = new NodeBase.VoidEvent();

		// Token: 0x040018F7 RID: 6391
		[SerializeField]
		[Outlet]
		private NodeBase.FloatEvent _valueEvent = new NodeBase.FloatEvent();

		// Token: 0x040018F8 RID: 6392
		private bool _state;

		// Token: 0x040018F9 RID: 6393
		private FloatInterpolator _value;
	}
}
