using System;
using Klak.Math;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000534 RID: 1332
	[AddComponentMenu("Klak/Wiring/Convertion/From To Vector")]
	public class FromToVector : NodeBase
	{
		// Token: 0x1700070D RID: 1805
		// (set) Token: 0x06002E99 RID: 11929 RVA: 0x000E33C0 File Offset: 0x000E17C0
		[Inlet]
		public float parameter
		{
			set
			{
				if (!base.enabled)
				{
					return;
				}
				Vector3 arg = BasicMath.Lerp(this._fromVector, this._toVector, value);
				this._vectorEvent.Invoke(arg);
			}
		}

		// Token: 0x040018BE RID: 6334
		[SerializeField]
		private Vector3 _fromVector = Vector3.zero;

		// Token: 0x040018BF RID: 6335
		[SerializeField]
		private Vector3 _toVector = Vector3.up;

		// Token: 0x040018C0 RID: 6336
		[SerializeField]
		[Outlet]
		private NodeBase.Vector3Event _vectorEvent = new NodeBase.Vector3Event();
	}
}
