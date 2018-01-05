using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x0200053D RID: 1341
	[AddComponentMenu("Klak/Wiring/Mixing/Rotation Mix")]
	public class RotationMix : NodeBase
	{
		// Token: 0x17000715 RID: 1813
		// (set) Token: 0x06002EB1 RID: 11953 RVA: 0x000E3898 File Offset: 0x000E1C98
		[Inlet]
		public Quaternion input
		{
			set
			{
				if (!base.enabled)
				{
					return;
				}
				this._inputValue = value;
				this.InvokeEvent();
			}
		}

		// Token: 0x17000716 RID: 1814
		// (set) Token: 0x06002EB2 RID: 11954 RVA: 0x000E38B3 File Offset: 0x000E1CB3
		[Inlet]
		public Quaternion modulation
		{
			set
			{
				if (!base.enabled)
				{
					return;
				}
				this._modulationValue = value;
				this.InvokeEvent();
			}
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x000E38CE File Offset: 0x000E1CCE
		private void InvokeEvent()
		{
			this._outputEvent.Invoke(this._inputValue * this._modulationValue);
		}

		// Token: 0x040018E3 RID: 6371
		[SerializeField]
		[Outlet]
		private NodeBase.QuaternionEvent _outputEvent = new NodeBase.QuaternionEvent();

		// Token: 0x040018E4 RID: 6372
		private Quaternion _inputValue;

		// Token: 0x040018E5 RID: 6373
		private Quaternion _modulationValue;
	}
}
