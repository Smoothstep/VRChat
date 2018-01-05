using System;
using UnityEngine;

// Token: 0x02000893 RID: 2195
public class LineRendererBehaviour : MonoBehaviour
{
	// Token: 0x06004376 RID: 17270 RVA: 0x00163EF8 File Offset: 0x001622F8
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

	// Token: 0x06004377 RID: 17271 RVA: 0x00163F44 File Offset: 0x00162344
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		if (this.effectSettings == null)
		{
			Debug.Log("Prefab root have not script \"PrefabSettings\"");
		}
		this.tRoot = this.effectSettings.transform;
		this.line = base.GetComponent<LineRenderer>();
		this.InitializeDefault();
		this.isInitializedOnStart = true;
	}

	// Token: 0x06004378 RID: 17272 RVA: 0x00163FA4 File Offset: 0x001623A4
	private void InitializeDefault()
	{
		base.GetComponent<Renderer>().material.SetFloat("_Chanel", (float)this.currentShaderIndex);
		this.currentShaderIndex++;
		if (this.currentShaderIndex == 3)
		{
			this.currentShaderIndex = 0;
		}
		this.line.SetPosition(0, this.tRoot.position);
		if (this.IsVertical)
		{
			if (Physics.Raycast(this.tRoot.position, Vector3.down, out this.hit))
			{
				this.line.SetPosition(1, this.hit.point);
				if (this.StartGlow != null)
				{
					this.StartGlow.transform.position = this.tRoot.position;
				}
				if (this.HitGlow != null)
				{
					this.HitGlow.transform.position = this.hit.point;
				}
				if (this.GoLight != null)
				{
					this.GoLight.transform.position = this.hit.point + new Vector3(0f, this.LightHeightOffset, 0f);
				}
				if (this.Particles != null)
				{
					this.Particles.transform.position = this.hit.point + new Vector3(0f, this.ParticlesHeightOffset, 0f);
				}
				if (this.Explosion != null)
				{
					this.Explosion.transform.position = this.hit.point + new Vector3(0f, this.ParticlesHeightOffset, 0f);
				}
			}
		}
		else
		{
			if (this.effectSettings.Target != null)
			{
				this.tTarget = this.effectSettings.Target.transform;
			}
			else if (!this.effectSettings.UseMoveVector)
			{
				Debug.Log("You must setup the the target or the motion vector");
			}
			Vector3 vector;
			if (!this.effectSettings.UseMoveVector)
			{
				vector = (this.tTarget.position - this.tRoot.position).normalized;
			}
			else
			{
				vector = this.tRoot.position + this.effectSettings.MoveVector * this.effectSettings.MoveDistance;
			}
			Vector3 a = this.tRoot.position + vector * this.effectSettings.MoveDistance;
			if (Physics.Raycast(this.tRoot.position, vector, out this.hit, this.effectSettings.MoveDistance + 1f, this.effectSettings.LayerMask))
			{
				a = (this.tRoot.position + Vector3.Normalize(this.hit.point - this.tRoot.position) * (this.effectSettings.MoveDistance + 1f)).normalized;
			}
			this.line.SetPosition(1, this.hit.point - this.effectSettings.ColliderRadius * a);
			Vector3 vector2 = this.hit.point - a * this.ParticlesHeightOffset;
			if (this.StartGlow != null)
			{
				this.StartGlow.transform.position = this.tRoot.position;
			}
			if (this.HitGlow != null)
			{
				this.HitGlow.transform.position = vector2;
			}
			if (this.GoLight != null)
			{
				this.GoLight.transform.position = this.hit.point - a * this.LightHeightOffset;
			}
			if (this.Particles != null)
			{
				this.Particles.transform.position = vector2;
			}
			if (this.Explosion != null)
			{
				this.Explosion.transform.position = vector2;
				this.Explosion.transform.LookAt(vector2 + this.hit.normal);
			}
		}
		CollisionInfo e = new CollisionInfo
		{
			Hit = this.hit
		};
		this.effectSettings.OnCollisionHandler(e);
		if (this.hit.transform != null)
		{
			ShieldCollisionBehaviour component = this.hit.transform.GetComponent<ShieldCollisionBehaviour>();
			if (component != null)
			{
				component.ShieldCollisionEnter(e);
			}
		}
	}

	// Token: 0x06004379 RID: 17273 RVA: 0x00164476 File Offset: 0x00162876
	private void OnEnable()
	{
		if (this.isInitializedOnStart)
		{
			this.InitializeDefault();
		}
	}

	// Token: 0x04002C0A RID: 11274
	public bool IsVertical;

	// Token: 0x04002C0B RID: 11275
	public float LightHeightOffset = 0.3f;

	// Token: 0x04002C0C RID: 11276
	public float ParticlesHeightOffset = 0.2f;

	// Token: 0x04002C0D RID: 11277
	public float TimeDestroyLightAfterCollision = 4f;

	// Token: 0x04002C0E RID: 11278
	public float TimeDestroyThisAfterCollision = 4f;

	// Token: 0x04002C0F RID: 11279
	public float TimeDestroyRootAfterCollision = 4f;

	// Token: 0x04002C10 RID: 11280
	public GameObject EffectOnHitObject;

	// Token: 0x04002C11 RID: 11281
	public GameObject Explosion;

	// Token: 0x04002C12 RID: 11282
	public GameObject StartGlow;

	// Token: 0x04002C13 RID: 11283
	public GameObject HitGlow;

	// Token: 0x04002C14 RID: 11284
	public GameObject Particles;

	// Token: 0x04002C15 RID: 11285
	public GameObject GoLight;

	// Token: 0x04002C16 RID: 11286
	private EffectSettings effectSettings;

	// Token: 0x04002C17 RID: 11287
	private Transform tRoot;

	// Token: 0x04002C18 RID: 11288
	private Transform tTarget;

	// Token: 0x04002C19 RID: 11289
	private bool isInitializedOnStart;

	// Token: 0x04002C1A RID: 11290
	private LineRenderer line;

	// Token: 0x04002C1B RID: 11291
	private int currentShaderIndex;

	// Token: 0x04002C1C RID: 11292
	private RaycastHit hit;
}
