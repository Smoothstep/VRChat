using System;
using UnityEngine;

// Token: 0x020008C9 RID: 2249
public class SU_SpaceParticles : MonoBehaviour
{
	// Token: 0x060044B3 RID: 17587 RVA: 0x0016F8B0 File Offset: 0x0016DCB0
	private void Start()
	{
		this._cacheTransform = base.transform;
		this._cacheParticleSystem = base.GetComponent<ParticleSystem>();
		this._distanceToSpawn = this.range * this.distanceSpawn;
		this._distanceToFade = this.range * this.distanceFade;
		if (this._cacheParticleSystem == null)
		{
			Debug.LogError("This script must be attached to a GameObject with a particle system. It is strongly recommended that you use the SpaceParticles or SpaceFog prefab which have suitable particle systems)");
		}
		for (int i = 0; i < this.maxParticles; i++)
		{
			ParticleSystem.Particle particle = default(ParticleSystem.Particle);
			particle.position = this._cacheTransform.position + UnityEngine.Random.insideUnitSphere * this._distanceToSpawn;
			particle.remainingLifetime = float.PositiveInfinity;
			Vector3 velocity = new Vector3(UnityEngine.Random.Range(this.minParticleDriftSpeed, this.maxParticleDriftSpeed) * this.driftSpeedMultiplier, UnityEngine.Random.Range(this.minParticleDriftSpeed, this.maxParticleDriftSpeed) * this.driftSpeedMultiplier, UnityEngine.Random.Range(this.minParticleDriftSpeed, this.maxParticleDriftSpeed) * this.driftSpeedMultiplier);
			particle.velocity = velocity;
			particle.size = UnityEngine.Random.Range(this.minParticleSize, this.maxParticleSize) * this.sizeMultiplier;
			base.GetComponent<ParticleSystem>().Emit(1);
		}
	}

	// Token: 0x060044B4 RID: 17588 RVA: 0x0016F9EC File Offset: 0x0016DDEC
	private void Update()
	{
		int particleCount = base.GetComponent<ParticleSystem>().particleCount;
		ParticleSystem.Particle[] array = new ParticleSystem.Particle[particleCount];
		base.GetComponent<ParticleSystem>().GetParticles(array);
		for (int i = 0; i < array.Length; i++)
		{
			float num = Vector3.Distance(array[i].position, this._cacheTransform.position);
			if (num > this.range)
			{
				array[i].position = UnityEngine.Random.onUnitSphere * this._distanceToSpawn + this._cacheTransform.position;
				num = Vector3.Distance(array[i].position, this._cacheTransform.position);
				Vector3 velocity = new Vector3(UnityEngine.Random.Range(this.minParticleDriftSpeed, this.maxParticleDriftSpeed) * this.driftSpeedMultiplier, UnityEngine.Random.Range(this.minParticleDriftSpeed, this.maxParticleDriftSpeed) * this.driftSpeedMultiplier, UnityEngine.Random.Range(this.minParticleDriftSpeed, this.maxParticleDriftSpeed) * this.driftSpeedMultiplier);
				array[i].velocity = velocity;
				array[i].size = UnityEngine.Random.Range(this.minParticleSize, this.maxParticleSize) * this.sizeMultiplier;
			}
			if (this.fadeParticles)
			{
				Color color = array[i].color;
				if (num > this._distanceToFade)
				{
					array[i].color = new Color(color.r, color.g, color.b, Mathf.Clamp01(1f - (num - this._distanceToFade) / (this._distanceToSpawn - this._distanceToFade)));
				}
				else
				{
					array[i].color = new Color(color.r, color.g, color.b, 1f);
				}
			}
		}
		base.GetComponent<ParticleSystem>().SetParticles(array, particleCount);
	}

	// Token: 0x04002E92 RID: 11922
	public int maxParticles = 1000;

	// Token: 0x04002E93 RID: 11923
	public float range = 200f;

	// Token: 0x04002E94 RID: 11924
	public float distanceSpawn = 0.95f;

	// Token: 0x04002E95 RID: 11925
	public float minParticleSize = 0.5f;

	// Token: 0x04002E96 RID: 11926
	public float maxParticleSize = 1f;

	// Token: 0x04002E97 RID: 11927
	public float sizeMultiplier = 1f;

	// Token: 0x04002E98 RID: 11928
	public float minParticleDriftSpeed;

	// Token: 0x04002E99 RID: 11929
	public float maxParticleDriftSpeed = 1f;

	// Token: 0x04002E9A RID: 11930
	public float driftSpeedMultiplier = 1f;

	// Token: 0x04002E9B RID: 11931
	public bool fadeParticles = true;

	// Token: 0x04002E9C RID: 11932
	public float distanceFade = 0.5f;

	// Token: 0x04002E9D RID: 11933
	private float _distanceToSpawn;

	// Token: 0x04002E9E RID: 11934
	private float _distanceToFade;

	// Token: 0x04002E9F RID: 11935
	private ParticleSystem _cacheParticleSystem;

	// Token: 0x04002EA0 RID: 11936
	private Transform _cacheTransform;
}
