using System;
using Klak.Math;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000546 RID: 1350
	[AddComponentMenu("Klak/Wiring/Input/Key Input")]
	public class KeyInput : NodeBase
	{
		// Token: 0x06002ECA RID: 11978 RVA: 0x000E3EEC File Offset: 0x000E22EC
		private void Start()
		{
			this._floatValue = new FloatInterpolator(0f, this._interpolator);
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x000E3F04 File Offset: 0x000E2304
		private void Update()
		{
			if (Input.GetKeyDown(this._keyCode))
			{
				this._keyDownEvent.Invoke();
				this._floatValue.targetValue = this._onValue;
			}
			else if (Input.GetKeyUp(this._keyCode))
			{
				this._keyUpEvent.Invoke();
				this._floatValue.targetValue = this._offValue;
			}
			this._valueEvent.Invoke(this._floatValue.Step());
		}

		// Token: 0x04001920 RID: 6432
		[SerializeField]
		private KeyCode _keyCode = KeyCode.Space;

		// Token: 0x04001921 RID: 6433
		[SerializeField]
		private float _offValue;

		// Token: 0x04001922 RID: 6434
		[SerializeField]
		private float _onValue = 1f;

		// Token: 0x04001923 RID: 6435
		[SerializeField]
		private FloatInterpolator.Config _interpolator;

		// Token: 0x04001924 RID: 6436
		[SerializeField]
		[Outlet]
		private NodeBase.VoidEvent _keyDownEvent = new NodeBase.VoidEvent();

		// Token: 0x04001925 RID: 6437
		[SerializeField]
		[Outlet]
		private NodeBase.VoidEvent _keyUpEvent = new NodeBase.VoidEvent();

		// Token: 0x04001926 RID: 6438
		[SerializeField]
		[Outlet]
		private NodeBase.FloatEvent _valueEvent = new NodeBase.FloatEvent();

		// Token: 0x04001927 RID: 6439
		private FloatInterpolator _floatValue;
	}
}
