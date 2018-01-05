using System;
using UnityEngine;

// Token: 0x02000AC1 RID: 2753
public static class PhysicsUtil
{
	// Token: 0x0600538E RID: 21390 RVA: 0x001CD1B4 File Offset: 0x001CB5B4
	public static Bounds GetCompoundColliderAABB(GameObject obj)
	{
		Bounds result = default(Bounds);
		bool flag = true;
		Collider[] componentsInChildren = obj.GetComponentsInChildren<Collider>();
		foreach (Collider collider in componentsInChildren)
		{
			if (!collider.isTrigger)
			{
				if (flag)
				{
					result = collider.bounds;
				}
				else
				{
					result.Encapsulate(collider.bounds);
				}
				flag = false;
			}
		}
		return result;
	}

	// Token: 0x0600538F RID: 21391 RVA: 0x001CD224 File Offset: 0x001CB624
	public static Vector3 ClosestPointOnCollider(Collider col, Vector3 pt)
	{
		bool flag = false;
		return PhysicsUtil.ClosestPointOnCollider(col, pt, ref flag);
	}

	// Token: 0x06005390 RID: 21392 RVA: 0x001CD23C File Offset: 0x001CB63C
	public static Vector3 ClosestPointOnCollider(Collider col, Vector3 pt, ref bool isPointInside)
	{
		if (col == null)
		{
			return Vector3.zero;
		}
		SphereCollider sphereCollider = col as SphereCollider;
		if (sphereCollider != null)
		{
			return PhysicsUtil.ClosestPointOnCollider(sphereCollider, pt, ref isPointInside);
		}
		BoxCollider boxCollider = col as BoxCollider;
		if (boxCollider != null)
		{
			return PhysicsUtil.ClosestPointOnCollider(boxCollider, pt, ref isPointInside);
		}
		CapsuleCollider capsuleCollider = col as CapsuleCollider;
		if (capsuleCollider != null)
		{
			return PhysicsUtil.ClosestPointOnCollider(capsuleCollider, pt, ref isPointInside);
		}
		Bounds bounds = col.bounds;
		bool flag = pt.x >= bounds.min.x && pt.x <= bounds.max.x && pt.y >= bounds.min.y && pt.y <= bounds.max.y && pt.z >= bounds.min.z && pt.z <= bounds.max.z;
		Vector3 vector = (!flag) ? col.ClosestPointOnBounds(pt) : pt;
		Vector3 direction = bounds.center - vector;
		Ray ray = new Ray(vector, direction);
		RaycastHit raycastHit;
		if (col.Raycast(ray, out raycastHit, direction.magnitude))
		{
			isPointInside = false;
			return raycastHit.point;
		}
		isPointInside = flag;
		return col.ClosestPointOnBounds(pt);
	}

	// Token: 0x06005391 RID: 21393 RVA: 0x001CD3C0 File Offset: 0x001CB7C0
	public static Vector3 ClosestPointOnCollider(SphereCollider col, Vector3 pt, ref bool isPointInside)
	{
		Vector3 a = col.transform.InverseTransformPoint(pt);
		Vector3 a2 = a - col.center;
		float magnitude = a2.magnitude;
		if (Mathf.Approximately(magnitude, 0f))
		{
			isPointInside = true;
			return col.transform.TransformPoint(col.center + Vector3.forward * col.radius);
		}
		isPointInside = (magnitude <= col.radius);
		a2 /= magnitude;
		return col.transform.TransformPoint(col.center + a2 * col.radius);
	}

	// Token: 0x06005392 RID: 21394 RVA: 0x001CD464 File Offset: 0x001CB864
	public static Vector3 ClosestPointOnCollider(BoxCollider col, Vector3 pt, ref bool isPointInside)
	{
		Vector3 position = col.transform.InverseTransformPoint(pt);
		Vector3 vector = col.center - col.size / 2f;
		Vector3 vector2 = col.center + col.size / 2f;
		isPointInside = (position.x >= vector.x && position.x <= vector2.x && position.y >= vector.y && position.y <= vector2.y && position.z >= vector.z && position.z <= vector2.z);
		position = new Vector3(Mathf.Clamp(position.x, vector.x, vector2.x), Mathf.Clamp(position.y, vector.y, vector2.y), Mathf.Clamp(position.z, vector.z, vector2.z));
		float num = Mathf.Abs(position.x - vector.x);
		int num2 = 0;
		float num3 = Mathf.Abs(position.x - vector2.x);
		if (num3 < num)
		{
			num2 = 1;
			num = num3;
		}
		num3 = Mathf.Abs(position.y - vector.y);
		if (num3 < num)
		{
			num2 = 2;
			num = num3;
		}
		num3 = Mathf.Abs(position.y - vector2.y);
		if (num3 < num)
		{
			num2 = 3;
			num = num3;
		}
		num3 = Mathf.Abs(position.z - vector.z);
		if (num3 < num)
		{
			num2 = 4;
			num = num3;
		}
		num3 = Mathf.Abs(position.z - vector2.z);
		if (num3 < num)
		{
			num2 = 5;
		}
		switch (num2)
		{
		case 0:
			position.x = vector.x;
			break;
		case 1:
			position.x = vector2.x;
			break;
		case 2:
			position.y = vector.y;
			break;
		case 3:
			position.y = vector2.y;
			break;
		case 4:
			position.z = vector.z;
			break;
		default:
			position.z = vector2.z;
			break;
		}
		return col.transform.TransformPoint(position);
	}

	// Token: 0x06005393 RID: 21395 RVA: 0x001CD6E9 File Offset: 0x001CBAE9
	public static Vector3 ClosestPointOnCollider(CapsuleCollider col, Vector3 pt, ref bool isPointInside)
	{
		return PhysicsUtil.ClosestPointOnCapsule(col.transform.localToWorldMatrix, col.center, col.height, col.radius, col.direction, pt, ref isPointInside);
	}

	// Token: 0x06005394 RID: 21396 RVA: 0x001CD718 File Offset: 0x001CBB18
	public static Vector3 ClosestPointOnCapsule(Matrix4x4 localToWorldTransform, Vector3 capsuleCenter, float capsuleHeight, float capsuleRadius, int capsuleDirection, Vector3 pt, ref bool isPointInside)
	{
		Quaternion q = Quaternion.identity;
		if (capsuleDirection == 0)
		{
			q = Quaternion.Euler(0f, 0f, -90f);
		}
		else if (capsuleDirection == 2)
		{
			q = Quaternion.Euler(90f, 0f, 0f);
		}
		Matrix4x4 matrix4x = localToWorldTransform * Matrix4x4.TRS(capsuleCenter, q, Vector3.one);
		Vector3 vector = matrix4x.inverse.MultiplyPoint(pt);
		float num = Mathf.Max(capsuleHeight / 2f - capsuleRadius, 0f);
		Vector3 vector2 = new Vector3(0f, vector.y, 0f);
		if (vector.y > num)
		{
			vector2 = new Vector3(0f, num, 0f);
		}
		else if (vector.y < -num)
		{
			vector2 = new Vector3(0f, -num, 0f);
		}
		Vector3 a = vector - vector2;
		float magnitude = a.magnitude;
		if (Mathf.Approximately(magnitude, 0f))
		{
			isPointInside = true;
			vector = vector2 + Vector3.forward * capsuleRadius;
			return matrix4x.MultiplyPoint(vector);
		}
		isPointInside = (magnitude <= capsuleRadius);
		a /= magnitude;
		vector = vector2 + a * capsuleRadius;
		return matrix4x.MultiplyPoint(vector);
	}
}
