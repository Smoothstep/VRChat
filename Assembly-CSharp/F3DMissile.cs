using System;
using UnityEngine;

// Token: 0x0200047E RID: 1150
public class F3DMissile : MonoBehaviour
{
	// Token: 0x060027DB RID: 10203 RVA: 0x000CF5B4 File Offset: 0x000CD9B4
	private void Awake()
	{
		this.transform = base.GetComponent<Transform>();
		this.particles = base.GetComponentsInChildren<ParticleSystem>();
		this.meshRenderer = base.GetComponent<MeshRenderer>();
	}

	// Token: 0x060027DC RID: 10204 RVA: 0x000CF5DA File Offset: 0x000CD9DA
	public void OnSpawned()
	{
		this.isHit = false;
		this.isFXSpawned = false;
		this.timer = 0f;
		this.targetLastPos = Vector3.zero;
		this.step = Vector3.zero;
		this.meshRenderer.enabled = true;
	}

	// Token: 0x060027DD RID: 10205 RVA: 0x000CF617 File Offset: 0x000CDA17
	public void OnDespawned()
	{
	}

	// Token: 0x060027DE RID: 10206 RVA: 0x000CF61C File Offset: 0x000CDA1C
	private void Delay()
	{
		if (this.particles.Length > 0 && this.delayedParticles.Length > 0)
		{
			for (int i = 0; i < this.particles.Length; i++)
			{
				bool flag = false;
				for (int j = 0; j < this.delayedParticles.Length; j++)
				{
					if (this.particles[i] == this.delayedParticles[j])
					{
						flag = true;
						break;
					}
				}
				this.particles[i].Stop(false);
				if (!flag)
				{
					this.particles[i].Clear(false);
				}
			}
		}
	}

	// Token: 0x060027DF RID: 10207 RVA: 0x000CF6BC File Offset: 0x000CDABC
	private void OnMissileDestroy()
	{
		if (F3DPool.instance != null)
		{
			F3DPool.instance.Despawn(this.transform);
		}
	}

	// Token: 0x060027E0 RID: 10208 RVA: 0x000CF6DE File Offset: 0x000CDADE
	private void OnHit()
	{
		this.meshRenderer.enabled = false;
		this.isHit = true;
		if (this.DelayDespawn)
		{
			this.timer = 0f;
			this.Delay();
		}
	}

	// Token: 0x060027E1 RID: 10209 RVA: 0x000CF710 File Offset: 0x000CDB10
	private void Update()
	{
		if (this.isHit)
		{
			if (!this.isFXSpawned)
			{
				F3DMissileLauncher.instance.SpawnExplosion(this.transform.position);
				this.isFXSpawned = true;
			}
			if (!this.DelayDespawn || (this.DelayDespawn && this.timer >= this.despawnDelay))
			{
				this.OnMissileDestroy();
			}
		}
		else
		{
			if (this.target != null)
			{
				if (this.missileType == F3DMissile.MissileType.Predictive)
				{
					Vector3 a = F3DPredictTrajectory.Predict(this.transform.position, this.target.position, this.targetLastPos, this.velocity);
					this.targetLastPos = this.target.position;
					this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(a - this.transform.position), Time.deltaTime * this.alignSpeed);
				}
				else if (this.missileType == F3DMissile.MissileType.Guided)
				{
					this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(this.target.position - this.transform.position), Time.deltaTime * this.alignSpeed);
				}
			}
			this.step = this.transform.forward * Time.deltaTime * this.velocity;
			if (this.target != null && this.missileType != F3DMissile.MissileType.Unguided && Vector3.SqrMagnitude(this.transform.position - this.target.position) <= this.detonationDistance)
			{
				this.OnHit();
			}
			else if (this.missileType == F3DMissile.MissileType.Unguided && Physics.Raycast(this.transform.position, this.transform.forward, this.step.magnitude * this.RaycastAdvance, this.layerMask))
			{
				this.OnHit();
			}
			else if (this.timer >= this.lifeTime)
			{
				this.isFXSpawned = true;
				this.OnHit();
			}
			this.transform.position += this.step;
		}
		this.timer += Time.deltaTime;
	}

	// Token: 0x040015FF RID: 5631
	public F3DMissile.MissileType missileType;

	// Token: 0x04001600 RID: 5632
	public Transform target;

	// Token: 0x04001601 RID: 5633
	public LayerMask layerMask;

	// Token: 0x04001602 RID: 5634
	public float detonationDistance;

	// Token: 0x04001603 RID: 5635
	public float lifeTime = 5f;

	// Token: 0x04001604 RID: 5636
	public float despawnDelay;

	// Token: 0x04001605 RID: 5637
	public float velocity = 300f;

	// Token: 0x04001606 RID: 5638
	public float alignSpeed = 1f;

	// Token: 0x04001607 RID: 5639
	public float RaycastAdvance = 2f;

	// Token: 0x04001608 RID: 5640
	public bool DelayDespawn;

	// Token: 0x04001609 RID: 5641
	public ParticleSystem[] delayedParticles;

	// Token: 0x0400160A RID: 5642
	private ParticleSystem[] particles;

	// Token: 0x0400160B RID: 5643
	private new Transform transform;

	// Token: 0x0400160C RID: 5644
	private bool isHit;

	// Token: 0x0400160D RID: 5645
	private bool isFXSpawned;

	// Token: 0x0400160E RID: 5646
	private float timer;

	// Token: 0x0400160F RID: 5647
	private Vector3 targetLastPos;

	// Token: 0x04001610 RID: 5648
	private Vector3 step;

	// Token: 0x04001611 RID: 5649
	private MeshRenderer meshRenderer;

	// Token: 0x0200047F RID: 1151
	public enum MissileType
	{
		// Token: 0x04001613 RID: 5651
		Unguided,
		// Token: 0x04001614 RID: 5652
		Guided,
		// Token: 0x04001615 RID: 5653
		Predictive
	}
}
