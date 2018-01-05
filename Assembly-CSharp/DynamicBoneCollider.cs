using System;
using UnityEngine;

// Token: 0x02000460 RID: 1120
[AddComponentMenu("Dynamic Bone/Dynamic Bone Collider")]
public class DynamicBoneCollider : MonoBehaviour
{
	// Token: 0x0600271E RID: 10014 RVA: 0x000C0E77 File Offset: 0x000BF277
	private void OnValidate()
	{
		this.m_Radius = Mathf.Max(this.m_Radius, 0f);
		this.m_Height = Mathf.Max(this.m_Height, 0f);
	}

	// Token: 0x0600271F RID: 10015 RVA: 0x000C0EA8 File Offset: 0x000BF2A8
	public void Collide(ref Vector3 particlePosition, float particleRadius)
	{
		float num = this.m_Radius * Mathf.Abs(base.transform.lossyScale.x);
		float num2 = this.m_Height * 0.5f - this.m_Radius;
		if (num2 <= 0f)
		{
			if (this.m_Bound == DynamicBoneCollider.Bound.Outside)
			{
				DynamicBoneCollider.OutsideSphere(ref particlePosition, particleRadius, base.transform.TransformPoint(this.m_Center), num);
			}
			else
			{
				DynamicBoneCollider.InsideSphere(ref particlePosition, particleRadius, base.transform.TransformPoint(this.m_Center), num);
			}
		}
		else
		{
			Vector3 center = this.m_Center;
			Vector3 center2 = this.m_Center;
			DynamicBoneCollider.Direction direction = this.m_Direction;
			if (direction != DynamicBoneCollider.Direction.X)
			{
				if (direction != DynamicBoneCollider.Direction.Y)
				{
					if (direction == DynamicBoneCollider.Direction.Z)
					{
						center.z -= num2;
						center2.z += num2;
					}
				}
				else
				{
					center.y -= num2;
					center2.y += num2;
				}
			}
			else
			{
				center.x -= num2;
				center2.x += num2;
			}
			if (this.m_Bound == DynamicBoneCollider.Bound.Outside)
			{
				DynamicBoneCollider.OutsideCapsule(ref particlePosition, particleRadius, base.transform.TransformPoint(center), base.transform.TransformPoint(center2), num);
			}
			else
			{
				DynamicBoneCollider.InsideCapsule(ref particlePosition, particleRadius, base.transform.TransformPoint(center), base.transform.TransformPoint(center2), num);
			}
		}
	}

	// Token: 0x06002720 RID: 10016 RVA: 0x000C102C File Offset: 0x000BF42C
	private static void OutsideSphere(ref Vector3 particlePosition, float particleRadius, Vector3 sphereCenter, float sphereRadius)
	{
		float num = sphereRadius + particleRadius;
		float num2 = num * num;
		Vector3 a = particlePosition - sphereCenter;
		float sqrMagnitude = a.sqrMagnitude;
		if (sqrMagnitude > 0f && sqrMagnitude < num2)
		{
			float num3 = Mathf.Sqrt(sqrMagnitude);
			particlePosition = sphereCenter + a * (num / num3);
		}
	}

	// Token: 0x06002721 RID: 10017 RVA: 0x000C1088 File Offset: 0x000BF488
	private static void InsideSphere(ref Vector3 particlePosition, float particleRadius, Vector3 sphereCenter, float sphereRadius)
	{
		float num = sphereRadius - particleRadius;
		float num2 = num * num;
		Vector3 a = particlePosition - sphereCenter;
		float sqrMagnitude = a.sqrMagnitude;
		if (sqrMagnitude > num2)
		{
			float num3 = Mathf.Sqrt(sqrMagnitude);
			particlePosition = sphereCenter + a * (num / num3);
		}
	}

	// Token: 0x06002722 RID: 10018 RVA: 0x000C10D8 File Offset: 0x000BF4D8
	private static void OutsideCapsule(ref Vector3 particlePosition, float particleRadius, Vector3 capsuleP0, Vector3 capsuleP1, float capsuleRadius)
	{
		float num = capsuleRadius + particleRadius;
		float num2 = num * num;
		Vector3 vector = capsuleP1 - capsuleP0;
		Vector3 vector2 = particlePosition - capsuleP0;
		float num3 = Vector3.Dot(vector2, vector);
		if (num3 <= 0f)
		{
			float sqrMagnitude = vector2.sqrMagnitude;
			if (sqrMagnitude > 0f && sqrMagnitude < num2)
			{
				float num4 = Mathf.Sqrt(sqrMagnitude);
				particlePosition = capsuleP0 + vector2 * (num / num4);
			}
		}
		else
		{
			float sqrMagnitude2 = vector.sqrMagnitude;
			if (num3 >= sqrMagnitude2)
			{
				vector2 = particlePosition - capsuleP1;
				float sqrMagnitude3 = vector2.sqrMagnitude;
				if (sqrMagnitude3 > 0f && sqrMagnitude3 < num2)
				{
					float num5 = Mathf.Sqrt(sqrMagnitude3);
					particlePosition = capsuleP1 + vector2 * (num / num5);
				}
			}
			else if (sqrMagnitude2 > 0f)
			{
				num3 /= sqrMagnitude2;
				vector2 -= vector * num3;
				float sqrMagnitude4 = vector2.sqrMagnitude;
				if (sqrMagnitude4 > 0f && sqrMagnitude4 < num2)
				{
					float num6 = Mathf.Sqrt(sqrMagnitude4);
					particlePosition += vector2 * ((num - num6) / num6);
				}
			}
		}
	}

	// Token: 0x06002723 RID: 10019 RVA: 0x000C1220 File Offset: 0x000BF620
	private static void InsideCapsule(ref Vector3 particlePosition, float particleRadius, Vector3 capsuleP0, Vector3 capsuleP1, float capsuleRadius)
	{
		float num = capsuleRadius - particleRadius;
		float num2 = num * num;
		Vector3 vector = capsuleP1 - capsuleP0;
		Vector3 vector2 = particlePosition - capsuleP0;
		float num3 = Vector3.Dot(vector2, vector);
		if (num3 <= 0f)
		{
			float sqrMagnitude = vector2.sqrMagnitude;
			if (sqrMagnitude > num2)
			{
				float num4 = Mathf.Sqrt(sqrMagnitude);
				particlePosition = capsuleP0 + vector2 * (num / num4);
			}
		}
		else
		{
			float sqrMagnitude2 = vector.sqrMagnitude;
			if (num3 >= sqrMagnitude2)
			{
				vector2 = particlePosition - capsuleP1;
				float sqrMagnitude3 = vector2.sqrMagnitude;
				if (sqrMagnitude3 > num2)
				{
					float num5 = Mathf.Sqrt(sqrMagnitude3);
					particlePosition = capsuleP1 + vector2 * (num / num5);
				}
			}
			else if (sqrMagnitude2 > 0f)
			{
				num3 /= sqrMagnitude2;
				vector2 -= vector * num3;
				float sqrMagnitude4 = vector2.sqrMagnitude;
				if (sqrMagnitude4 > num2)
				{
					float num6 = Mathf.Sqrt(sqrMagnitude4);
					particlePosition += vector2 * ((num - num6) / num6);
				}
			}
		}
	}

	// Token: 0x06002724 RID: 10020 RVA: 0x000C1344 File Offset: 0x000BF744
	private void OnDrawGizmosSelected()
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.m_Bound == DynamicBoneCollider.Bound.Outside)
		{
			Gizmos.color = Color.yellow;
		}
		else
		{
			Gizmos.color = Color.magenta;
		}
		float radius = this.m_Radius * Mathf.Abs(base.transform.lossyScale.x);
		float num = this.m_Height * 0.5f - this.m_Radius;
		if (num <= 0f)
		{
			Gizmos.DrawWireSphere(base.transform.TransformPoint(this.m_Center), radius);
		}
		else
		{
			Vector3 center = this.m_Center;
			Vector3 center2 = this.m_Center;
			DynamicBoneCollider.Direction direction = this.m_Direction;
			if (direction != DynamicBoneCollider.Direction.X)
			{
				if (direction != DynamicBoneCollider.Direction.Y)
				{
					if (direction == DynamicBoneCollider.Direction.Z)
					{
						center.z -= num;
						center2.z += num;
					}
				}
				else
				{
					center.y -= num;
					center2.y += num;
				}
			}
			else
			{
				center.x -= num;
				center2.x += num;
			}
			Gizmos.DrawWireSphere(base.transform.TransformPoint(center), radius);
			Gizmos.DrawWireSphere(base.transform.TransformPoint(center2), radius);
		}
	}

	// Token: 0x0400147B RID: 5243
	public Vector3 m_Center = Vector3.zero;

	// Token: 0x0400147C RID: 5244
	public float m_Radius = 0.5f;

	// Token: 0x0400147D RID: 5245
	public float m_Height;

	// Token: 0x0400147E RID: 5246
	public DynamicBoneCollider.Direction m_Direction;

	// Token: 0x0400147F RID: 5247
	public DynamicBoneCollider.Bound m_Bound;

	// Token: 0x02000461 RID: 1121
	public enum Direction
	{
		// Token: 0x04001481 RID: 5249
		X,
		// Token: 0x04001482 RID: 5250
		Y,
		// Token: 0x04001483 RID: 5251
		Z
	}

	// Token: 0x02000462 RID: 1122
	public enum Bound
	{
		// Token: 0x04001485 RID: 5253
		Outside,
		// Token: 0x04001486 RID: 5254
		Inside
	}
}
