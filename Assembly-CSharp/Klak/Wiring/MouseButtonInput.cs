using System;
using Klak.Math;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000547 RID: 1351
	[AddComponentMenu("Klak/Wiring/Input/Mouse Button Input")]
	public class MouseButtonInput : NodeBase
	{
		// Token: 0x06002ECD RID: 11981 RVA: 0x000E3FB8 File Offset: 0x000E23B8
		private void Start()
		{
			this._floatValue = new FloatInterpolator(0f, this._interpolator);
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x000E3FD0 File Offset: 0x000E23D0
		private void Update()
		{
			if (Input.GetMouseButtonDown(this._buttonIndex))
			{
				this._buttonDownEvent.Invoke();
				this._floatValue.targetValue = this._onValue;
			}
			else if (Input.GetMouseButtonUp(this._buttonIndex))
			{
				this._buttonUpEvent.Invoke();
				this._floatValue.targetValue = this._offValue;
			}
			this._valueEvent.Invoke(this._floatValue.Step());
		}

		// Token: 0x04001928 RID: 6440
		[SerializeField]
		private int _buttonIndex;

		// Token: 0x04001929 RID: 6441
		[SerializeField]
		private float _offValue;

		// Token: 0x0400192A RID: 6442
		[SerializeField]
		private float _onValue = 1f;

		// Token: 0x0400192B RID: 6443
		[SerializeField]
		private FloatInterpolator.Config _interpolator;

		// Token: 0x0400192C RID: 6444
		[SerializeField]
		[Outlet]
		private NodeBase.VoidEvent _buttonDownEvent = new NodeBase.VoidEvent();

		// Token: 0x0400192D RID: 6445
		[SerializeField]
		[Outlet]
		private NodeBase.VoidEvent _buttonUpEvent = new NodeBase.VoidEvent();

		// Token: 0x0400192E RID: 6446
		[SerializeField]
		[Outlet]
		private NodeBase.FloatEvent _valueEvent = new NodeBase.FloatEvent();

		// Token: 0x0400192F RID: 6447
		private FloatInterpolator _floatValue;
	}
}
