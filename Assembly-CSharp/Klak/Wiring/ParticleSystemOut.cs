using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x02000553 RID: 1363
	[AddComponentMenu("Klak/Wiring/Output/Component/Particle System Out")]
	public class ParticleSystemOut : NodeBase
	{
		// Token: 0x17000722 RID: 1826
		// (set) Token: 0x06002EF2 RID: 12018 RVA: 0x000E45C1 File Offset: 0x000E29C1
		[Inlet]
		public float emissionRate
		{
			set
			{
				if (!base.enabled || this._particleSystem == null)
				{
					return;
				}
				this._emission.rateOverTime = new ParticleSystem.MinMaxCurve(value);
			}
		}

		// Token: 0x06002EF3 RID: 12019 RVA: 0x000E45F1 File Offset: 0x000E29F1
		[Inlet]
		public void Emit(float count)
		{
			this._particleSystem.Emit((int)count);
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x000E4600 File Offset: 0x000E2A00
		private void Start()
		{
			this._emission = this._particleSystem.emission;
		}

		// Token: 0x04001951 RID: 6481
		[SerializeField]
		private ParticleSystem _particleSystem;

		// Token: 0x04001952 RID: 6482
		private ParticleSystem.EmissionModule _emission;
	}
}
