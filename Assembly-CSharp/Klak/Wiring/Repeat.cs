using System;
using System.Collections;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x0200053C RID: 1340
	[AddComponentMenu("Klak/Wiring/Switching/Repeat")]
	public class Repeat : NodeBase
	{
		// Token: 0x06002EAE RID: 11950 RVA: 0x000E3787 File Offset: 0x000E1B87
		[Inlet]
		public void Trigger()
		{
			base.StartCoroutine(this.InvokeRepeatedly());
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x000E3798 File Offset: 0x000E1B98
		private IEnumerator InvokeRepeatedly()
		{
			for (int i = 0; i < this._repeatCount; i++)
			{
				this._outputEvent.Invoke();
				yield return new WaitForSeconds(this._interval);
			}
			yield break;
		}

		// Token: 0x040018E0 RID: 6368
		[SerializeField]
		private int _repeatCount = 3;

		// Token: 0x040018E1 RID: 6369
		[SerializeField]
		private float _interval = 0.5f;

		// Token: 0x040018E2 RID: 6370
		[SerializeField]
		[Outlet]
		private NodeBase.VoidEvent _outputEvent = new NodeBase.VoidEvent();
	}
}
