using System;
using Klak.Math;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000544 RID: 1348
	[AddComponentMenu("Klak/Wiring/Input/Axis Input")]
	public class AxisInput : NodeBase
	{
		// Token: 0x06002EC4 RID: 11972 RVA: 0x000E3D91 File Offset: 0x000E2191
		private void Start()
		{
			this._axisValue = new FloatInterpolator(0f, this._interpolator);
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x000E3DA9 File Offset: 0x000E21A9
		private void Update()
		{
			this._axisValue.targetValue = Input.GetAxis(this._axisName);
			this._valueEvent.Invoke(this._axisValue.Step());
		}

		// Token: 0x04001914 RID: 6420
		[SerializeField]
		private string _axisName = "Horizontal";

		// Token: 0x04001915 RID: 6421
		[SerializeField]
		private FloatInterpolator.Config _interpolator;

		// Token: 0x04001916 RID: 6422
		[SerializeField]
		[Outlet]
		private NodeBase.FloatEvent _valueEvent = new NodeBase.FloatEvent();

		// Token: 0x04001917 RID: 6423
		private FloatInterpolator _axisValue;
	}
}
