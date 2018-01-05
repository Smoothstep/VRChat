using System;
using Klak.Math;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000545 RID: 1349
	[AddComponentMenu("Klak/Wiring/Input/Button Input")]
	public class ButtonInput : NodeBase
	{
		// Token: 0x06002EC7 RID: 11975 RVA: 0x000E3E16 File Offset: 0x000E2216
		private void Start()
		{
			this._floatValue = new FloatInterpolator(0f, this._interpolator);
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x000E3E30 File Offset: 0x000E2230
		private void Update()
		{
			if (Input.GetButtonDown(this._buttonName))
			{
				this._buttonDownEvent.Invoke();
				this._floatValue.targetValue = this._onValue;
			}
			else if (Input.GetButtonUp(this._buttonName))
			{
				this._buttonUpEvent.Invoke();
				this._floatValue.targetValue = this._offValue;
			}
			this._valueEvent.Invoke(this._floatValue.Step());
		}

		// Token: 0x04001918 RID: 6424
		[SerializeField]
		private string _buttonName = "Jump";

		// Token: 0x04001919 RID: 6425
		[SerializeField]
		private float _offValue;

		// Token: 0x0400191A RID: 6426
		[SerializeField]
		private float _onValue = 1f;

		// Token: 0x0400191B RID: 6427
		[SerializeField]
		private FloatInterpolator.Config _interpolator;

		// Token: 0x0400191C RID: 6428
		[SerializeField]
		[Outlet]
		private NodeBase.VoidEvent _buttonDownEvent = new NodeBase.VoidEvent();

		// Token: 0x0400191D RID: 6429
		[SerializeField]
		[Outlet]
		private NodeBase.VoidEvent _buttonUpEvent = new NodeBase.VoidEvent();

		// Token: 0x0400191E RID: 6430
		[SerializeField]
		[Outlet]
		private NodeBase.FloatEvent _valueEvent = new NodeBase.FloatEvent();

		// Token: 0x0400191F RID: 6431
		private FloatInterpolator _floatValue;
	}
}
