using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000539 RID: 1337
	[AddComponentMenu("Klak/Wiring/Mixing/Float Mix")]
	public class FloatMix : NodeBase
	{
		// Token: 0x17000711 RID: 1809
		// (set) Token: 0x06002EA6 RID: 11942 RVA: 0x000E3612 File Offset: 0x000E1A12
		[Inlet]
		public float input
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

		// Token: 0x17000712 RID: 1810
		// (set) Token: 0x06002EA7 RID: 11943 RVA: 0x000E3638 File Offset: 0x000E1A38
		[Inlet]
		public float modulation
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

		// Token: 0x06002EA8 RID: 11944 RVA: 0x000E3660 File Offset: 0x000E1A60
		private float MixValues()
		{
			switch (this._modulationType)
			{
			case FloatMix.ModulationType.Add:
				return this._inputValue + this._modulationValue;
			case FloatMix.ModulationType.Subtract:
				return this._inputValue - this._modulationValue;
			case FloatMix.ModulationType.Multiply:
				return this._inputValue * this._modulationValue;
			case FloatMix.ModulationType.Divide:
				return this._inputValue / this._modulationValue;
			case FloatMix.ModulationType.Minimum:
				return Mathf.Min(this._inputValue, this._modulationValue);
			case FloatMix.ModulationType.Maximum:
				return Mathf.Max(this._inputValue, this._modulationValue);
			default:
				return this._inputValue;
			}
		}

		// Token: 0x040018D1 RID: 6353
		[SerializeField]
		private FloatMix.ModulationType _modulationType = FloatMix.ModulationType.Add;

		// Token: 0x040018D2 RID: 6354
		[SerializeField]
		[Outlet]
		private NodeBase.FloatEvent _outputEvent = new NodeBase.FloatEvent();

		// Token: 0x040018D3 RID: 6355
		private float _inputValue;

		// Token: 0x040018D4 RID: 6356
		private float _modulationValue;

		// Token: 0x0200053A RID: 1338
		public enum ModulationType
		{
			// Token: 0x040018D6 RID: 6358
			Off,
			// Token: 0x040018D7 RID: 6359
			Add,
			// Token: 0x040018D8 RID: 6360
			Subtract,
			// Token: 0x040018D9 RID: 6361
			Multiply,
			// Token: 0x040018DA RID: 6362
			Divide,
			// Token: 0x040018DB RID: 6363
			Minimum,
			// Token: 0x040018DC RID: 6364
			Maximum
		}
	}
}
