using System;
using UnityEngine;
using UnityEngine.VR;

// Token: 0x0200066F RID: 1647
public static class OVRExtensions
{
	// Token: 0x060037BA RID: 14266 RVA: 0x0011C40C File Offset: 0x0011A80C
	public static OVRPose ToTrackingSpacePose(this Transform transform)
	{
		OVRPose lhs;
		lhs.position = InputTracking.GetLocalPosition(VRNode.Head);
		lhs.orientation = InputTracking.GetLocalRotation(VRNode.Head);
		return lhs * transform.ToHeadSpacePose();
	}

	// Token: 0x060037BB RID: 14267 RVA: 0x0011C444 File Offset: 0x0011A844
	public static OVRPose ToHeadSpacePose(this Transform transform)
	{
		return Camera.current.transform.ToOVRPose(false).Inverse() * transform.ToOVRPose(false);
	}

	// Token: 0x060037BC RID: 14268 RVA: 0x0011C478 File Offset: 0x0011A878
	internal static OVRPose ToOVRPose(this Transform t, bool isLocal = false)
	{
		OVRPose result;
		result.orientation = ((!isLocal) ? t.rotation : t.localRotation);
		result.position = ((!isLocal) ? t.position : t.localPosition);
		return result;
	}

	// Token: 0x060037BD RID: 14269 RVA: 0x0011C4C4 File Offset: 0x0011A8C4
	internal static void FromOVRPose(this Transform t, OVRPose pose, bool isLocal = false)
	{
		if (isLocal)
		{
			t.localRotation = pose.orientation;
			t.localPosition = pose.position;
		}
		else
		{
			t.rotation = pose.orientation;
			t.position = pose.position;
		}
	}

	// Token: 0x060037BE RID: 14270 RVA: 0x0011C510 File Offset: 0x0011A910
	internal static OVRPose ToOVRPose(this OVRPlugin.Posef p)
	{
		return new OVRPose
		{
			position = new Vector3(p.Position.x, p.Position.y, -p.Position.z),
			orientation = new Quaternion(-p.Orientation.x, -p.Orientation.y, p.Orientation.z, p.Orientation.w)
		};
	}

	// Token: 0x060037BF RID: 14271 RVA: 0x0011C598 File Offset: 0x0011A998
	internal static OVRTracker.Frustum ToFrustum(this OVRPlugin.Frustumf f)
	{
		return new OVRTracker.Frustum
		{
			nearZ = f.zNear,
			farZ = f.zFar,
			fov = new Vector2
			{
				x = 57.29578f * f.fovX,
				y = 57.29578f * f.fovY
			}
		};
	}

	// Token: 0x060037C0 RID: 14272 RVA: 0x0011C604 File Offset: 0x0011AA04
	internal static Color FromColorf(this OVRPlugin.Colorf c)
	{
		return new Color
		{
			r = c.r,
			g = c.g,
			b = c.b,
			a = c.a
		};
	}

	// Token: 0x060037C1 RID: 14273 RVA: 0x0011C654 File Offset: 0x0011AA54
	internal static OVRPlugin.Colorf ToColorf(this Color c)
	{
		return new OVRPlugin.Colorf
		{
			r = c.r,
			g = c.g,
			b = c.b,
			a = c.a
		};
	}

	// Token: 0x060037C2 RID: 14274 RVA: 0x0011C6A4 File Offset: 0x0011AAA4
	internal static Vector3 FromVector3f(this OVRPlugin.Vector3f v)
	{
		return new Vector3
		{
			x = v.x,
			y = v.y,
			z = v.z
		};
	}

	// Token: 0x060037C3 RID: 14275 RVA: 0x0011C6E4 File Offset: 0x0011AAE4
	internal static Vector3 FromFlippedZVector3f(this OVRPlugin.Vector3f v)
	{
		return new Vector3
		{
			x = v.x,
			y = v.y,
			z = -v.z
		};
	}

	// Token: 0x060037C4 RID: 14276 RVA: 0x0011C728 File Offset: 0x0011AB28
	internal static OVRPlugin.Vector3f ToVector3f(this Vector3 v)
	{
		return new OVRPlugin.Vector3f
		{
			x = v.x,
			y = v.y,
			z = v.z
		};
	}

	// Token: 0x060037C5 RID: 14277 RVA: 0x0011C768 File Offset: 0x0011AB68
	internal static OVRPlugin.Vector3f ToFlippedZVector3f(this Vector3 v)
	{
		return new OVRPlugin.Vector3f
		{
			x = v.x,
			y = v.y,
			z = -v.z
		};
	}

	// Token: 0x060037C6 RID: 14278 RVA: 0x0011C7AC File Offset: 0x0011ABAC
	internal static Quaternion FromQuatf(this OVRPlugin.Quatf q)
	{
		return new Quaternion
		{
			x = q.x,
			y = q.y,
			z = q.z,
			w = q.w
		};
	}

	// Token: 0x060037C7 RID: 14279 RVA: 0x0011C7FC File Offset: 0x0011ABFC
	internal static OVRPlugin.Quatf ToQuatf(this Quaternion q)
	{
		return new OVRPlugin.Quatf
		{
			x = q.x,
			y = q.y,
			z = q.z,
			w = q.w
		};
	}
}
