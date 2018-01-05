using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000532 RID: 1330
	[AddComponentMenu("Klak/Wiring/Convertion/Euler Rotation")]
	public class EulerRotation : NodeBase
	{
		// Token: 0x1700070B RID: 1803
		// (set) Token: 0x06002E90 RID: 11920 RVA: 0x000E3280 File Offset: 0x000E1680
		[Inlet]
		public Vector3 eulerAngles
		{
			set
			{
				if (!base.enabled)
				{
					return;
				}
				this._rotationEvent.Invoke(Quaternion.Euler(value));
			}
		}

		// Token: 0x040018B6 RID: 6326
		[SerializeField]
		[Outlet]
		private NodeBase.QuaternionEvent _rotationEvent = new NodeBase.QuaternionEvent();
	}
}
