using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000B9E RID: 2974
	[RequireComponent(typeof(ParticleSystem))]
	public class DestroyOnParticleSystemDeath : MonoBehaviour
	{
		// Token: 0x06005C61 RID: 23649 RVA: 0x00204489 File Offset: 0x00202889
		private void Awake()
		{
			this.particles = base.GetComponent<ParticleSystem>();
			base.InvokeRepeating("CheckParticleSystem", 0.1f, 0.1f);
		}

		// Token: 0x06005C62 RID: 23650 RVA: 0x002044AC File Offset: 0x002028AC
		private void CheckParticleSystem()
		{
			if (!this.particles.IsAlive())
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x040041F5 RID: 16885
		private ParticleSystem particles;
	}
}
