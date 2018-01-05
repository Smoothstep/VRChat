using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000542 RID: 1346
	[AddComponentMenu("Klak/Wiring/Mixing/Vector Mix")]
	public class VectorMix : NodeBase
	{
		// Token: 0x1700071A RID: 1818
		// (set) Token: 0x06002EC0 RID: 11968 RVA: 0x000E3C79 File Offset: 0x000E2079
		[Inlet]
		public Vector3 input
		{
			set
			{
				if (!base.enabled)
				{
					return;
				}
				this._inputValue = value;
				this._outputEvent.Invoke(this.MixValues());
			}
		}

		// Token: 0x1700071B RID: 1819
		// (set) Token: 0x06002EC1 RID: 11969 RVA: 0x000E3C9F File Offset: 0x000E209F
		[Inlet]
		public Vector3 modulation
		{
			set
			{
				if (!base.enabled)
				{
					return;
				}
				this._modulationValue = value;
				this._outputEvent.Invoke(this.MixValues());
			}
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x000E3CC8 File Offset: 0x000E20C8
		private Vector3 MixValues()
		{
			switch (this._modulationType)
			{
			case VectorMix.ModulationType.Add:
				return this._inputValue + this._modulationValue;
			case VectorMix.ModulationType.Subtract:
				return this._inputValue - this._modulationValue;
			case VectorMix.ModulationType.Multiply:
				return Vector3.Scale(this._inputValue, this._modulationValue);
			case VectorMix.ModulationType.Cross:
				return Vector3.Cross(this._inputValue, this._modulationValue);
			case VectorMix.ModulationType.Minimum:
				return Vector3.Min(this._inputValue, this._modulationValue);
			case VectorMix.ModulationType.Maximum:
				return Vector3.Max(this._inputValue, this._modulationValue);
			default:
				return this._inputValue;
			}
		}

		// Token: 0x04001908 RID: 6408
		[SerializeField]
		private VectorMix.ModulationType _modulationType = VectorMix.ModulationType.Add;

		// Token: 0x04001909 RID: 6409
		[SerializeField]
		[Outlet]
		private NodeBase.Vector3Event _outputEvent = new NodeBase.Vector3Event();

		// Token: 0x0400190A RID: 6410
		private Vector3 _inputValue;

		// Token: 0x0400190B RID: 6411
		private Vector3 _modulationValue;

		// Token: 0x02000543 RID: 1347
		public enum ModulationType
		{
			// Token: 0x0400190D RID: 6413
			Off,
			// Token: 0x0400190E RID: 6414
			Add,
			// Token: 0x0400190F RID: 6415
			Subtract,
			// Token: 0x04001910 RID: 6416
			Multiply,
			// Token: 0x04001911 RID: 6417
			Cross,
			// Token: 0x04001912 RID: 6418
			Minimum,
			// Token: 0x04001913 RID: 6419
			Maximum
		}
	}
}
