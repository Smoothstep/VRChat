using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000535 RID: 1333
	[AddComponentMenu("Klak/Wiring/Convertion/Vector")]
	public class Vector : NodeBase
	{
		// Token: 0x1700070E RID: 1806
		// (set) Token: 0x06002E9B RID: 11931 RVA: 0x000E3416 File Offset: 0x000E1816
		[Inlet]
		public float scale
		{
			set
			{
				if (!base.enabled)
				{
					return;
				}
				this._vectorEvent.Invoke(this._baseVector * value);
			}
		}

		// Token: 0x040018C1 RID: 6337
		[SerializeField]
		private Vector3 _baseVector = Vector3.up;

		// Token: 0x040018C2 RID: 6338
		[SerializeField]
		[Outlet]
		private NodeBase.Vector3Event _vectorEvent = new NodeBase.Vector3Event();
	}
}
