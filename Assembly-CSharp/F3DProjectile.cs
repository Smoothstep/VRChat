using System;
using UnityEngine;

// Token: 0x02000483 RID: 1155
public class F3DProjectile : MonoBehaviour
{
	// Token: 0x060027EF RID: 10223 RVA: 0x000CFD7F File Offset: 0x000CE17F
	private void Awake()
	{
		this.transform = base.GetComponent<Transform>();
		this.particles = base.GetComponentsInChildren<ParticleSystem>();
	}

	// Token: 0x060027F0 RID: 10224 RVA: 0x000CFD9C File Offset: 0x000CE19C
	public void OnSpawned()
	{
		this.isHit = false;
		this.isFXSpawned = false;
		this.timer = 0f;
		this.hitPoint = default(RaycastHit);
	}

	// Token: 0x060027F1 RID: 10225 RVA: 0x000CFDD1 File Offset: 0x000CE1D1
	public void OnDespawned()
	{
	}

	// Token: 0x060027F2 RID: 10226 RVA: 0x000CFDD4 File Offset: 0x000CE1D4
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

	// Token: 0x060027F3 RID: 10227 RVA: 0x000CFE74 File Offset: 0x000CE274
	private void OnProjectileDestroy()
	{
		F3DPool.instance.Despawn(this.transform);
	}

	// Token: 0x060027F4 RID: 10228 RVA: 0x000CFE88 File Offset: 0x000CE288
	private void ApplyForce(float force)
	{
		if (this.hitPoint.rigidbody != null)
		{
			this.hitPoint.rigidbody.AddForceAtPosition(this.transform.forward * force, this.hitPoint.point, ForceMode.VelocityChange);
		}
	}

	// Token: 0x060027F5 RID: 10229 RVA: 0x000CFED8 File Offset: 0x000CE2D8
	private void Update()
	{
		if (this.isHit)
		{
			if (!this.isFXSpawned)
			{
				F3DFXType f3DFXType = this.fxType;
				switch (f3DFXType)
				{
				case F3DFXType.Vulcan:
					F3DFXController.instance.VulcanImpact(this.hitPoint.point + this.hitPoint.normal * 0.2f);
					this.ApplyForce(2.5f);
					break;
				case F3DFXType.SoloGun:
					F3DFXController.instance.SoloGunImpact(this.hitPoint.point + this.hitPoint.normal * 0.2f);
					this.ApplyForce(25f);
					break;
				default:
					if (f3DFXType == F3DFXType.LaserImpulse)
					{
						F3DFXController.instance.LaserImpulseImpact(this.hitPoint.point + this.hitPoint.normal * 0.2f);
						this.ApplyForce(25f);
					}
					break;
				case F3DFXType.Seeker:
					F3DFXController.instance.SeekerImpact(this.hitPoint.point + this.hitPoint.normal * 1f);
					this.ApplyForce(30f);
					break;
				case F3DFXType.PlasmaGun:
					F3DFXController.instance.PlasmaGunImpact(this.hitPoint.point + this.hitPoint.normal * 0.2f);
					this.ApplyForce(25f);
					break;
				}
				this.isFXSpawned = true;
			}
			if (!this.DelayDespawn || (this.DelayDespawn && this.timer >= this.despawnDelay))
			{
				this.OnProjectileDestroy();
			}
		}
		else
		{
			Vector3 b = this.transform.forward * Time.deltaTime * this.velocity;
			if (Physics.Raycast(this.transform.position, this.transform.forward, out this.hitPoint, b.magnitude * this.RaycastAdvance, this.layerMask))
			{
				this.isHit = true;
				if (this.DelayDespawn)
				{
					this.timer = 0f;
					this.Delay();
				}
			}
			else if (this.timer >= this.lifeTime)
			{
				this.OnProjectileDestroy();
			}
			this.transform.position += b;
		}
		this.timer += Time.deltaTime;
	}

	// Token: 0x04001620 RID: 5664
	public F3DFXType fxType;

	// Token: 0x04001621 RID: 5665
	public LayerMask layerMask;

	// Token: 0x04001622 RID: 5666
	public float lifeTime = 5f;

	// Token: 0x04001623 RID: 5667
	public float despawnDelay;

	// Token: 0x04001624 RID: 5668
	public float velocity = 300f;

	// Token: 0x04001625 RID: 5669
	public float RaycastAdvance = 2f;

	// Token: 0x04001626 RID: 5670
	public bool DelayDespawn;

	// Token: 0x04001627 RID: 5671
	public ParticleSystem[] delayedParticles;

	// Token: 0x04001628 RID: 5672
	private ParticleSystem[] particles;

	// Token: 0x04001629 RID: 5673
	private new Transform transform;

	// Token: 0x0400162A RID: 5674
	private RaycastHit hitPoint;

	// Token: 0x0400162B RID: 5675
	private bool isHit;

	// Token: 0x0400162C RID: 5676
	private bool isFXSpawned;

	// Token: 0x0400162D RID: 5677
	private float timer;
}
