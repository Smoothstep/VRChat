using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x0200052F RID: 1327
	[AddComponentMenu("Klak/Wiring/Convertion/Axis Rotation")]
	public class AxisRotation : NodeBase
	{
		// Token: 0x17000709 RID: 1801
		// (set) Token: 0x06002E8C RID: 11916 RVA: 0x000E3138 File Offset: 0x000E1538
		[Inlet]
		public float angle
		{
			set
			{
				if (!base.enabled)
				{
					return;
				}
				float angle = value * this._angleMultiplier;
				Quaternion arg = Quaternion.AngleAxis(angle, this._rotationAxis);
				this._rotationEvent.Invoke(arg);
			}
		}

		// Token: 0x040018AC RID: 6316
		[SerializeField]
		private Vector3 _rotationAxis = Vector3.up;

		// Token: 0x040018AD RID: 6317
		[SerializeField]
		private float _angleMultiplier = 90f;

		// Token: 0x040018AE RID: 6318
		[SerializeField]
		[Outlet]
		private NodeBase.QuaternionEvent _rotationEvent = new NodeBase.QuaternionEvent();
	}
}
