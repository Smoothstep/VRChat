using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x0200053B RID: 1339
	[AddComponentMenu("Klak/Wiring/Mixing/Float Vector Mix")]
	public class FloatVectorMix : NodeBase
	{
		// Token: 0x17000713 RID: 1811
		// (set) Token: 0x06002EAA RID: 11946 RVA: 0x000E370E File Offset: 0x000E1B0E
		[Inlet]
		public float floatInput
		{
			set
			{
				if (!base.enabled)
				{
					return;
				}
				this._floatValue = value;
				this.InvokeEvent();
			}
		}

		// Token: 0x17000714 RID: 1812
		// (set) Token: 0x06002EAB RID: 11947 RVA: 0x000E3729 File Offset: 0x000E1B29
		[Inlet]
		public Vector3 vectorInput
		{
			set
			{
				if (!base.enabled)
				{
					return;
				}
				this._vectorValue = value;
				this.InvokeEvent();
			}
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x000E3744 File Offset: 0x000E1B44
		private void InvokeEvent()
		{
			this._outputEvent.Invoke(this._floatValue * this._vectorValue);
		}

		// Token: 0x040018DD RID: 6365
		[SerializeField]
		[Outlet]
		private NodeBase.Vector3Event _outputEvent = new NodeBase.Vector3Event();

		// Token: 0x040018DE RID: 6366
		private float _floatValue;

		// Token: 0x040018DF RID: 6367
		private Vector3 _vectorValue;
	}
}
