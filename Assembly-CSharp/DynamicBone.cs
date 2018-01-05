using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200045C RID: 1116
[AddComponentMenu("Dynamic Bone/Dynamic Bone")]
public class DynamicBone : MonoBehaviour, IVRCCullable
{
	// Token: 0x06002705 RID: 9989 RVA: 0x000BFB04 File Offset: 0x000BDF04
	private void Start()
	{
		this.m_UpdateMode = DynamicBone.UpdateMode.Normal;
		this.m_UpdateRate = Mathf.Clamp(this.m_UpdateRate, 0f, 60f);
		this.m_BaseUpdateRate = this.m_UpdateRate;
		this.m_ReferenceObject = null;
		this.m_DistanceToObject = 10f;
		this.m_DistantDisable = true;
		this.SetupParticles();
	}

	// Token: 0x06002706 RID: 9990 RVA: 0x000BFB5E File Offset: 0x000BDF5E
	private void FixedUpdate()
	{
		if (this.m_UpdateMode == DynamicBone.UpdateMode.AnimatePhysics)
		{
			this.PreUpdate();
		}
	}

	// Token: 0x06002707 RID: 9991 RVA: 0x000BFB74 File Offset: 0x000BDF74
	private void Update()
	{
		if (this.m_UpdateMode != DynamicBone.UpdateMode.AnimatePhysics)
		{
			this.PreUpdate();
		}
		if (this.m_Culled || this.m_DistantDisabled)
		{
			this.m_UpdateRate = Mathf.MoveTowards(this.m_UpdateRate, 0f, this.m_BaseUpdateRate * Time.deltaTime);
			this.m_Weight = Mathf.MoveTowards(this.m_Weight, 0f, Time.deltaTime);
		}
		else
		{
			this.m_UpdateRate = Mathf.MoveTowards(this.m_UpdateRate, this.m_BaseUpdateRate, this.m_BaseUpdateRate * Time.deltaTime);
			this.m_Weight = Mathf.MoveTowards(this.m_Weight, 1f, 0.5f * Time.deltaTime);
		}
	}

	// Token: 0x06002708 RID: 9992 RVA: 0x000BFC30 File Offset: 0x000BE030
	private void LateUpdate()
	{
		if (this.m_DistantDisable)
		{
			this.CheckDistance();
		}
		if (this.m_Weight > 0f && (!this.m_DistantDisable || !this.m_DistantDisabled))
		{
			float t = (this.m_UpdateMode != DynamicBone.UpdateMode.UnscaledTime) ? Time.deltaTime : Time.unscaledDeltaTime;
			this.UpdateDynamicBones(t);
		}
	}

	// Token: 0x06002709 RID: 9993 RVA: 0x000BFC97 File Offset: 0x000BE097
	private void PreUpdate()
	{
		if (this.m_Weight > 0f && (!this.m_DistantDisable || !this.m_DistantDisabled))
		{
			this.InitTransforms();
		}
	}

	// Token: 0x0600270A RID: 9994 RVA: 0x000BFCC8 File Offset: 0x000BE0C8
	private void CheckDistance()
	{
		Transform transform = this.m_ReferenceObject;
		if (transform == null && Camera.main != null)
		{
			transform = Camera.main.transform;
		}
		if (transform != null)
		{
			float sqrMagnitude = (transform.position - base.transform.position).sqrMagnitude;
			bool flag = sqrMagnitude > this.m_DistanceToObject * this.m_DistanceToObject;
			if (flag != this.m_DistantDisabled)
			{
				if (!flag)
				{
					this.ResetParticlesPosition();
				}
				this.m_DistantDisabled = flag;
			}
		}
	}

	// Token: 0x0600270B RID: 9995 RVA: 0x000BFD5E File Offset: 0x000BE15E
	private void OnEnable()
	{
		this.ResetParticlesPosition();
	}

	// Token: 0x0600270C RID: 9996 RVA: 0x000BFD66 File Offset: 0x000BE166
	private void OnDisable()
	{
		this.InitTransforms();
	}

	// Token: 0x0600270D RID: 9997 RVA: 0x000BFD70 File Offset: 0x000BE170
	private void OnValidate()
	{
		this.m_UpdateRate = Mathf.Max(this.m_UpdateRate, 0f);
		this.m_Damping = Mathf.Clamp01(this.m_Damping);
		this.m_Elasticity = Mathf.Clamp01(this.m_Elasticity);
		this.m_Stiffness = Mathf.Clamp01(this.m_Stiffness);
		this.m_Inert = Mathf.Clamp01(this.m_Inert);
		this.m_Radius = Mathf.Max(this.m_Radius, 0f);
		if (Application.isEditor && Application.isPlaying)
		{
			this.InitTransforms();
			this.SetupParticles();
		}
	}

	// Token: 0x0600270E RID: 9998 RVA: 0x000BFE10 File Offset: 0x000BE210
	private void OnDrawGizmosSelected()
	{
		if (!base.enabled || this.m_Root == null)
		{
			return;
		}
		if (Application.isEditor && !Application.isPlaying && base.transform.hasChanged)
		{
			this.InitTransforms();
			this.SetupParticles();
		}
		Gizmos.color = Color.white;
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			if (particle.m_ParentIndex >= 0)
			{
				DynamicBone.Particle particle2 = this.m_Particles[particle.m_ParentIndex];
				Gizmos.DrawLine(particle.m_Position, particle2.m_Position);
			}
			if (particle.m_Radius > 0f)
			{
				Gizmos.DrawWireSphere(particle.m_Position, particle.m_Radius * this.m_ObjectScale);
			}
		}
	}

	// Token: 0x0600270F RID: 9999 RVA: 0x000BFEF4 File Offset: 0x000BE2F4
	public void SetWeight(float w)
	{
		if (this.m_Weight != w)
		{
			if (w == 0f)
			{
				this.InitTransforms();
			}
			else if (this.m_Weight == 0f)
			{
				this.ResetParticlesPosition();
			}
			this.m_Weight = w;
		}
	}

	// Token: 0x06002710 RID: 10000 RVA: 0x000BFF40 File Offset: 0x000BE340
	public float GetWeight()
	{
		return this.m_Weight;
	}

	// Token: 0x06002711 RID: 10001 RVA: 0x000BFF48 File Offset: 0x000BE348
	private void UpdateDynamicBones(float t)
	{
		if (this.m_Root == null)
		{
			return;
		}
		this.m_ObjectScale = Mathf.Abs(base.transform.lossyScale.x);
		this.m_ObjectMove = base.transform.position - this.m_ObjectPrevPosition;
		this.m_ObjectPrevPosition = base.transform.position;
		int num = 1;
		if (this.m_UpdateRate > 0.1f)
		{
			float num2 = 1f / this.m_UpdateRate;
			this.m_Time += t;
			num = 0;
			while (this.m_Time >= num2)
			{
				this.m_Time -= num2;
				if (++num >= 3)
				{
					this.m_Time = 0f;
					break;
				}
			}
		}
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				this.UpdateParticles1();
				this.UpdateParticles2();
				this.m_ObjectMove = Vector3.zero;
			}
		}
		else
		{
			this.SkipUpdateParticles();
		}
		this.ApplyParticlesToTransforms();
	}

	// Token: 0x06002712 RID: 10002 RVA: 0x000C005C File Offset: 0x000BE45C
	private void SetupParticles()
	{
		this.m_Particles.Clear();
		if (this.m_Root == null)
		{
			return;
		}
		this.m_LocalGravity = this.m_Root.InverseTransformDirection(this.m_Gravity);
		this.m_ObjectScale = Mathf.Abs(base.transform.lossyScale.x);
		this.m_ObjectPrevPosition = base.transform.position;
		this.m_ObjectMove = Vector3.zero;
		this.m_BoneTotalLength = 0f;
		this.AppendParticles(this.m_Root, -1, 0f);
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			particle.m_Damping = this.m_Damping;
			particle.m_Elasticity = this.m_Elasticity;
			particle.m_Stiffness = this.m_Stiffness;
			particle.m_Inert = this.m_Inert;
			particle.m_Radius = this.m_Radius;
			if (this.m_BoneTotalLength > 0f)
			{
				float time = particle.m_BoneLength / this.m_BoneTotalLength;
				if (this.m_DampingDistrib != null && this.m_DampingDistrib.keys.Length > 0)
				{
					particle.m_Damping *= this.m_DampingDistrib.Evaluate(time);
				}
				if (this.m_ElasticityDistrib != null && this.m_ElasticityDistrib.keys.Length > 0)
				{
					particle.m_Elasticity *= this.m_ElasticityDistrib.Evaluate(time);
				}
				if (this.m_StiffnessDistrib != null && this.m_StiffnessDistrib.keys.Length > 0)
				{
					particle.m_Stiffness *= this.m_StiffnessDistrib.Evaluate(time);
				}
				if (this.m_InertDistrib != null && this.m_InertDistrib.keys.Length > 0)
				{
					particle.m_Inert *= this.m_InertDistrib.Evaluate(time);
				}
				if (this.m_RadiusDistrib != null && this.m_RadiusDistrib.keys.Length > 0)
				{
					particle.m_Radius *= this.m_RadiusDistrib.Evaluate(time);
				}
			}
			particle.m_Damping = Mathf.Clamp01(particle.m_Damping);
			particle.m_Elasticity = Mathf.Clamp01(particle.m_Elasticity);
			particle.m_Stiffness = Mathf.Clamp01(particle.m_Stiffness);
			particle.m_Inert = Mathf.Clamp01(particle.m_Inert);
			particle.m_Radius = Mathf.Max(particle.m_Radius, 0f);
		}
	}

	// Token: 0x06002713 RID: 10003 RVA: 0x000C02E4 File Offset: 0x000BE6E4
	private void AppendParticles(Transform b, int parentIndex, float boneLength)
	{
		DynamicBone.Particle particle = new DynamicBone.Particle();
		particle.m_Transform = b;
		particle.m_ParentIndex = parentIndex;
		if (b != null)
		{
			particle.m_Position = (particle.m_PrevPosition = b.position);
			particle.m_InitLocalPosition = b.localPosition;
			particle.m_InitLocalRotation = b.localRotation;
		}
		else
		{
			Transform transform = this.m_Particles[parentIndex].m_Transform;
			if (this.m_EndLength > 0f)
			{
				Transform parent = transform.parent;
				if (parent != null)
				{
					particle.m_EndOffset = transform.InverseTransformPoint(transform.position * 2f - parent.position) * this.m_EndLength;
				}
				else
				{
					particle.m_EndOffset = new Vector3(this.m_EndLength, 0f, 0f);
				}
			}
			else
			{
				particle.m_EndOffset = transform.InverseTransformPoint(base.transform.TransformDirection(this.m_EndOffset) + transform.position);
			}
			particle.m_Position = (particle.m_PrevPosition = transform.TransformPoint(particle.m_EndOffset));
		}
		if (parentIndex >= 0)
		{
			boneLength += (this.m_Particles[parentIndex].m_Transform.position - particle.m_Position).magnitude;
			particle.m_BoneLength = boneLength;
			this.m_BoneTotalLength = Mathf.Max(this.m_BoneTotalLength, boneLength);
		}
		int count = this.m_Particles.Count;
		this.m_Particles.Add(particle);
		if (b != null)
		{
			for (int i = 0; i < b.childCount; i++)
			{
				bool flag = false;
				if (this.m_Exclusions != null)
				{
					for (int j = 0; j < this.m_Exclusions.Count; j++)
					{
						Transform x = this.m_Exclusions[j];
						if (x == b.GetChild(i))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					this.AppendParticles(b.GetChild(i), count, boneLength);
				}
			}
			if (b.childCount == 0 && (this.m_EndLength > 0f || this.m_EndOffset != Vector3.zero))
			{
				this.AppendParticles(null, count, boneLength);
			}
		}
	}

	// Token: 0x06002714 RID: 10004 RVA: 0x000C054C File Offset: 0x000BE94C
	private void InitTransforms()
	{
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			if (particle.m_Transform != null)
			{
				particle.m_Transform.localPosition = particle.m_InitLocalPosition;
				particle.m_Transform.localRotation = particle.m_InitLocalRotation;
			}
		}
	}

	// Token: 0x06002715 RID: 10005 RVA: 0x000C05B8 File Offset: 0x000BE9B8
	private void ResetParticlesPosition()
	{
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			if (particle.m_Transform != null)
			{
				particle.m_Position = (particle.m_PrevPosition = particle.m_Transform.position);
			}
			else
			{
				Transform transform = this.m_Particles[particle.m_ParentIndex].m_Transform;
				particle.m_Position = (particle.m_PrevPosition = transform.TransformPoint(particle.m_EndOffset));
			}
		}
		this.m_ObjectPrevPosition = base.transform.position;
	}

	// Token: 0x06002716 RID: 10006 RVA: 0x000C0664 File Offset: 0x000BEA64
	private void UpdateParticles1()
	{
		Vector3 vector = this.m_Gravity;
		Vector3 normalized = this.m_Gravity.normalized;
		Vector3 lhs = this.m_Root.TransformDirection(this.m_LocalGravity);
		Vector3 b = normalized * Mathf.Max(Vector3.Dot(lhs, normalized), 0f);
		vector -= b;
		vector = (vector + this.m_Force) * this.m_ObjectScale;
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			if (particle.m_ParentIndex >= 0)
			{
				Vector3 a = particle.m_Position - particle.m_PrevPosition;
				Vector3 b2 = this.m_ObjectMove * particle.m_Inert;
				particle.m_PrevPosition = particle.m_Position + b2;
				particle.m_Position += a * (1f - particle.m_Damping) + vector + b2;
			}
			else
			{
				particle.m_PrevPosition = particle.m_Position;
				particle.m_Position = particle.m_Transform.position;
			}
		}
	}

	// Token: 0x06002717 RID: 10007 RVA: 0x000C07A4 File Offset: 0x000BEBA4
	private void UpdateParticles2()
	{
		Plane plane = default(Plane);
		for (int i = 1; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			DynamicBone.Particle particle2 = this.m_Particles[particle.m_ParentIndex];
			float magnitude;
			if (particle.m_Transform != null)
			{
				magnitude = (particle2.m_Transform.position - particle.m_Transform.position).magnitude;
			}
			else
			{
				magnitude = particle2.m_Transform.localToWorldMatrix.MultiplyVector(particle.m_EndOffset).magnitude;
			}
			float num = Mathf.Lerp(1f, particle.m_Stiffness, this.m_Weight);
			if (num > 0f || particle.m_Elasticity > 0f)
			{
				Matrix4x4 localToWorldMatrix = particle2.m_Transform.localToWorldMatrix;
				localToWorldMatrix.SetColumn(3, particle2.m_Position);
				Vector3 a;
				if (particle.m_Transform != null)
				{
					a = localToWorldMatrix.MultiplyPoint3x4(particle.m_Transform.localPosition);
				}
				else
				{
					a = localToWorldMatrix.MultiplyPoint3x4(particle.m_EndOffset);
				}
				Vector3 a2 = a - particle.m_Position;
				particle.m_Position += a2 * particle.m_Elasticity;
				if (num > 0f)
				{
					a2 = a - particle.m_Position;
					float magnitude2 = a2.magnitude;
					float num2 = magnitude * (1f - num) * 2f;
					if (magnitude2 > num2)
					{
						particle.m_Position += a2 * ((magnitude2 - num2) / magnitude2);
					}
				}
			}
			if (this.m_Colliders != null)
			{
				float particleRadius = particle.m_Radius * this.m_ObjectScale;
				for (int j = 0; j < this.m_Colliders.Count; j++)
				{
					DynamicBoneCollider dynamicBoneCollider = this.m_Colliders[j];
					if (dynamicBoneCollider != null && dynamicBoneCollider.enabled)
					{
						dynamicBoneCollider.Collide(ref particle.m_Position, particleRadius);
					}
				}
			}
			if (this.m_FreezeAxis != DynamicBone.FreezeAxis.None)
			{
				DynamicBone.FreezeAxis freezeAxis = this.m_FreezeAxis;
				if (freezeAxis != DynamicBone.FreezeAxis.X)
				{
					if (freezeAxis != DynamicBone.FreezeAxis.Y)
					{
						if (freezeAxis == DynamicBone.FreezeAxis.Z)
						{
							plane.SetNormalAndPosition(particle2.m_Transform.forward, particle2.m_Position);
						}
					}
					else
					{
						plane.SetNormalAndPosition(particle2.m_Transform.up, particle2.m_Position);
					}
				}
				else
				{
					plane.SetNormalAndPosition(particle2.m_Transform.right, particle2.m_Position);
				}
				particle.m_Position -= plane.normal * plane.GetDistanceToPoint(particle.m_Position);
			}
			Vector3 a3 = particle2.m_Position - particle.m_Position;
			float magnitude3 = a3.magnitude;
			if (magnitude3 > 0f)
			{
				particle.m_Position += a3 * ((magnitude3 - magnitude) / magnitude3);
			}
		}
	}

	// Token: 0x06002718 RID: 10008 RVA: 0x000C0ADC File Offset: 0x000BEEDC
	private void SkipUpdateParticles()
	{
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			if (particle.m_ParentIndex >= 0)
			{
				particle.m_PrevPosition += this.m_ObjectMove;
				particle.m_Position += this.m_ObjectMove;
				DynamicBone.Particle particle2 = this.m_Particles[particle.m_ParentIndex];
				float magnitude;
				if (particle.m_Transform != null)
				{
					magnitude = (particle2.m_Transform.position - particle.m_Transform.position).magnitude;
				}
				else
				{
					magnitude = particle2.m_Transform.localToWorldMatrix.MultiplyVector(particle.m_EndOffset).magnitude;
				}
				float num = Mathf.Lerp(1f, particle.m_Stiffness, this.m_Weight);
				if (num > 0f)
				{
					Matrix4x4 localToWorldMatrix = particle2.m_Transform.localToWorldMatrix;
					localToWorldMatrix.SetColumn(3, particle2.m_Position);
					Vector3 a;
					if (particle.m_Transform != null)
					{
						a = localToWorldMatrix.MultiplyPoint3x4(particle.m_Transform.localPosition);
					}
					else
					{
						a = localToWorldMatrix.MultiplyPoint3x4(particle.m_EndOffset);
					}
					Vector3 a2 = a - particle.m_Position;
					float magnitude2 = a2.magnitude;
					float num2 = magnitude * (1f - num) * 2f;
					if (magnitude2 > num2)
					{
						particle.m_Position += a2 * ((magnitude2 - num2) / magnitude2);
					}
				}
				Vector3 a3 = particle2.m_Position - particle.m_Position;
				float magnitude3 = a3.magnitude;
				if (magnitude3 > 0f)
				{
					particle.m_Position += a3 * ((magnitude3 - magnitude) / magnitude3);
				}
			}
			else
			{
				particle.m_PrevPosition = particle.m_Position;
				particle.m_Position = particle.m_Transform.position;
			}
		}
	}

	// Token: 0x06002719 RID: 10009 RVA: 0x000C0CF6 File Offset: 0x000BF0F6
	private static Vector3 MirrorVector(Vector3 v, Vector3 axis)
	{
		return v - axis * (Vector3.Dot(v, axis) * 2f);
	}

	// Token: 0x0600271A RID: 10010 RVA: 0x000C0D14 File Offset: 0x000BF114
	private void ApplyParticlesToTransforms()
	{
		for (int i = 1; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			DynamicBone.Particle particle2 = this.m_Particles[particle.m_ParentIndex];
			if (particle2.m_Transform.childCount <= 1)
			{
				Vector3 direction;
				if (particle.m_Transform != null)
				{
					direction = particle.m_Transform.localPosition;
				}
				else
				{
					direction = particle.m_EndOffset;
				}
				Vector3 toDirection = particle.m_Position - particle2.m_Position;
				Quaternion lhs = Quaternion.FromToRotation(particle2.m_Transform.TransformDirection(direction), toDirection);
				particle2.m_Transform.rotation = lhs * particle2.m_Transform.rotation;
			}
			if (particle.m_Transform != null)
			{
				particle.m_Transform.position = particle.m_Position;
			}
		}
	}

	// Token: 0x0600271B RID: 10011 RVA: 0x000C0DFD File Offset: 0x000BF1FD
	public void CullFunction(bool isCulled)
	{
		this.m_Culled = isCulled;
	}

	// Token: 0x04001443 RID: 5187
	public Transform m_Root;

	// Token: 0x04001444 RID: 5188
	public float m_UpdateRate = 60f;

	// Token: 0x04001445 RID: 5189
	public DynamicBone.UpdateMode m_UpdateMode;

	// Token: 0x04001446 RID: 5190
	[Range(0f, 1f)]
	public float m_Damping = 0.1f;

	// Token: 0x04001447 RID: 5191
	public AnimationCurve m_DampingDistrib;

	// Token: 0x04001448 RID: 5192
	[Range(0f, 1f)]
	public float m_Elasticity = 0.1f;

	// Token: 0x04001449 RID: 5193
	public AnimationCurve m_ElasticityDistrib;

	// Token: 0x0400144A RID: 5194
	[Range(0f, 1f)]
	public float m_Stiffness = 0.1f;

	// Token: 0x0400144B RID: 5195
	public AnimationCurve m_StiffnessDistrib;

	// Token: 0x0400144C RID: 5196
	[Range(0f, 1f)]
	public float m_Inert;

	// Token: 0x0400144D RID: 5197
	public AnimationCurve m_InertDistrib;

	// Token: 0x0400144E RID: 5198
	public float m_Radius;

	// Token: 0x0400144F RID: 5199
	public AnimationCurve m_RadiusDistrib;

	// Token: 0x04001450 RID: 5200
	public float m_EndLength;

	// Token: 0x04001451 RID: 5201
	public Vector3 m_EndOffset = Vector3.zero;

	// Token: 0x04001452 RID: 5202
	public Vector3 m_Gravity = Vector3.zero;

	// Token: 0x04001453 RID: 5203
	public Vector3 m_Force = Vector3.zero;

	// Token: 0x04001454 RID: 5204
	public List<DynamicBoneCollider> m_Colliders;

	// Token: 0x04001455 RID: 5205
	public List<Transform> m_Exclusions;

	// Token: 0x04001456 RID: 5206
	public DynamicBone.FreezeAxis m_FreezeAxis;

	// Token: 0x04001457 RID: 5207
	public bool m_DistantDisable;

	// Token: 0x04001458 RID: 5208
	public Transform m_ReferenceObject;

	// Token: 0x04001459 RID: 5209
	public float m_DistanceToObject = 20f;

	// Token: 0x0400145A RID: 5210
	private Vector3 m_LocalGravity = Vector3.zero;

	// Token: 0x0400145B RID: 5211
	private Vector3 m_ObjectMove = Vector3.zero;

	// Token: 0x0400145C RID: 5212
	private Vector3 m_ObjectPrevPosition = Vector3.zero;

	// Token: 0x0400145D RID: 5213
	private float m_BoneTotalLength;

	// Token: 0x0400145E RID: 5214
	private float m_ObjectScale = 1f;

	// Token: 0x0400145F RID: 5215
	private float m_Time;

	// Token: 0x04001460 RID: 5216
	private float m_Weight = 1f;

	// Token: 0x04001461 RID: 5217
	private bool m_DistantDisabled;

	// Token: 0x04001462 RID: 5218
	private List<DynamicBone.Particle> m_Particles = new List<DynamicBone.Particle>();

	// Token: 0x04001463 RID: 5219
	private float m_BaseUpdateRate;

	// Token: 0x04001464 RID: 5220
	private bool m_Culled;

	// Token: 0x0200045D RID: 1117
	public enum UpdateMode
	{
		// Token: 0x04001466 RID: 5222
		Normal,
		// Token: 0x04001467 RID: 5223
		AnimatePhysics,
		// Token: 0x04001468 RID: 5224
		UnscaledTime
	}

	// Token: 0x0200045E RID: 1118
	public enum FreezeAxis
	{
		// Token: 0x0400146A RID: 5226
		None,
		// Token: 0x0400146B RID: 5227
		X,
		// Token: 0x0400146C RID: 5228
		Y,
		// Token: 0x0400146D RID: 5229
		Z
	}

	// Token: 0x0200045F RID: 1119
	private class Particle
	{
		// Token: 0x0400146E RID: 5230
		public Transform m_Transform;

		// Token: 0x0400146F RID: 5231
		public int m_ParentIndex = -1;

		// Token: 0x04001470 RID: 5232
		public float m_Damping;

		// Token: 0x04001471 RID: 5233
		public float m_Elasticity;

		// Token: 0x04001472 RID: 5234
		public float m_Stiffness;

		// Token: 0x04001473 RID: 5235
		public float m_Inert;

		// Token: 0x04001474 RID: 5236
		public float m_Radius;

		// Token: 0x04001475 RID: 5237
		public float m_BoneLength;

		// Token: 0x04001476 RID: 5238
		public Vector3 m_Position = Vector3.zero;

		// Token: 0x04001477 RID: 5239
		public Vector3 m_PrevPosition = Vector3.zero;

		// Token: 0x04001478 RID: 5240
		public Vector3 m_EndOffset = Vector3.zero;

		// Token: 0x04001479 RID: 5241
		public Vector3 m_InitLocalPosition = Vector3.zero;

		// Token: 0x0400147A RID: 5242
		public Quaternion m_InitLocalRotation = Quaternion.identity;
	}
}
