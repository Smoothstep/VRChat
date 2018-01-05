using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x0200054A RID: 1354
	[AddComponentMenu("Klak/Wiring/Input/Starter")]
	public class Starter : NodeBase
	{
		// Token: 0x06002ED7 RID: 11991 RVA: 0x000E41E2 File Offset: 0x000E25E2
		private void Start()
		{
			this._onStartEvent.Invoke();
		}

		// Token: 0x0400193B RID: 6459
		[SerializeField]
		[Outlet]
		private NodeBase.VoidEvent _onStartEvent = new NodeBase.VoidEvent();
	}
}
