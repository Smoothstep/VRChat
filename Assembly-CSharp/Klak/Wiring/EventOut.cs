using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x0200054E RID: 1358
	[AddComponentMenu("Klak/Wiring/Output/Generic/Event Out")]
	public class EventOut : NodeBase
	{
		// Token: 0x06002EE4 RID: 12004 RVA: 0x000E4391 File Offset: 0x000E2791
		[Inlet]
		public void Bang()
		{
			this._event.Invoke();
		}

		// Token: 0x04001944 RID: 6468
		[SerializeField]
		private NodeBase.VoidEvent _event;
	}
}
