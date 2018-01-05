using System;
using UnityEngine;

// Token: 0x0200089C RID: 2204
public class FadeInOutParticles : MonoBehaviour
{
	// Token: 0x060043A5 RID: 17317 RVA: 0x00165338 File Offset: 0x00163738
	private void GetEffectSettingsComponent(Transform tr)
	{
		Transform parent = tr.parent;
		if (parent != null)
		{
			this.effectSettings = parent.GetComponentInChildren<EffectSettings>();
			if (this.effectSettings == null)
			{
				this.GetEffectSettingsComponent(parent.transform);
			}
		}
	}

	// Token: 0x060043A6 RID: 17318 RVA: 0x00165381 File Offset: 0x00163781
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		this.particles = this.effectSettings.GetComponentsInChildren<ParticleSystem>();
		this.oldVisibleStat = this.effectSettings.IsVisible;
	}

	// Token: 0x060043A7 RID: 17319 RVA: 0x001653B4 File Offset: 0x001637B4
	private void Update()
	{
		if (this.effectSettings.IsVisible != this.oldVisibleStat)
		{
			if (this.effectSettings.IsVisible)
			{
				foreach (ParticleSystem particleSystem in this.particles)
				{
					if (this.effectSettings.IsVisible)
					{
						particleSystem.Play();
                        particleSystem.enableEmission = true;
					}
				}
			}
			else
			{
				foreach (ParticleSystem particleSystem2 in this.particles)
				{
					if (!this.effectSettings.IsVisible)
					{
						particleSystem2.Stop();
                        particleSystem2.enableEmission = true;
                    }
				}
			}
		}
		this.oldVisibleStat = this.effectSettings.IsVisible;
	}

	// Token: 0x04002C6F RID: 11375
	private EffectSettings effectSettings;

	// Token: 0x04002C70 RID: 11376
	private ParticleSystem[] particles;

	// Token: 0x04002C71 RID: 11377
	private bool oldVisibleStat;
}
