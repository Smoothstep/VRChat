using System;
using Klak.Math;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000548 RID: 1352
	[AddComponentMenu("Klak/Wiring/Input/Mouse Position Input")]
	public class MousePositionInput : NodeBase
	{
		// Token: 0x06002ED0 RID: 11984 RVA: 0x000E406E File Offset: 0x000E246E
		private void Start()
		{
			this._xValue = new FloatInterpolator(0f, this._interpolator);
			this._yValue = new FloatInterpolator(0f, this._interpolator);
		}

		// Token: 0x06002ED1 RID: 11985 RVA: 0x000E409C File Offset: 0x000E249C
		private void Update()
		{
			Vector3 mousePosition = Input.mousePosition;
			this._xValue.targetValue = mousePosition.x / (float)Screen.width;
			this._yValue.targetValue = mousePosition.y / (float)Screen.height;
			this._xEvent.Invoke(this._xValue.Step());
			this._yEvent.Invoke(this._yValue.Step());
		}

		// Token: 0x04001930 RID: 6448
		[SerializeField]
		private FloatInterpolator.Config _interpolator;

		// Token: 0x04001931 RID: 6449
		[SerializeField]
		[Outlet]
		private NodeBase.FloatEvent _xEvent = new NodeBase.FloatEvent();

		// Token: 0x04001932 RID: 6450
		[SerializeField]
		[Outlet]
		private NodeBase.FloatEvent _yEvent = new NodeBase.FloatEvent();

		// Token: 0x04001933 RID: 6451
		private FloatInterpolator _xValue;

		// Token: 0x04001934 RID: 6452
		private FloatInterpolator _yValue;
	}
}
