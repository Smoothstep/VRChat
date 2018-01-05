using System;
using UnityEngine;

// Token: 0x02000670 RID: 1648
[Serializable]
public struct OVRPose
{
	// Token: 0x170008C7 RID: 2247
	// (get) Token: 0x060037C8 RID: 14280 RVA: 0x0011C84C File Offset: 0x0011AC4C
	public static OVRPose identity
	{
		get
		{
			return new OVRPose
			{
				position = Vector3.zero,
				orientation = Quaternion.identity
			};
		}
	}

	// Token: 0x060037C9 RID: 14281 RVA: 0x0011C87A File Offset: 0x0011AC7A
	public override bool Equals(object obj)
	{
		return obj is OVRPose && this == (OVRPose)obj;
	}

	// Token: 0x060037CA RID: 14282 RVA: 0x0011C89B File Offset: 0x0011AC9B
	public override int GetHashCode()
	{
		return this.position.GetHashCode() ^ this.orientation.GetHashCode();
	}

	// Token: 0x060037CB RID: 14283 RVA: 0x0011C8C0 File Offset: 0x0011ACC0
	public static bool operator ==(OVRPose x, OVRPose y)
	{
		return x.position == y.position && x.orientation == y.orientation;
	}

	// Token: 0x060037CC RID: 14284 RVA: 0x0011C8F0 File Offset: 0x0011ACF0
	public static bool operator !=(OVRPose x, OVRPose y)
	{
		return !(x == y);
	}

	// Token: 0x060037CD RID: 14285 RVA: 0x0011C8FC File Offset: 0x0011ACFC
	public static OVRPose operator *(OVRPose lhs, OVRPose rhs)
	{
		return new OVRPose
		{
			position = lhs.position + lhs.orientation * rhs.position,
			orientation = lhs.orientation * rhs.orientation
		};
	}

	// Token: 0x060037CE RID: 14286 RVA: 0x0011C954 File Offset: 0x0011AD54
	public OVRPose Inverse()
	{
		OVRPose result;
		result.orientation = Quaternion.Inverse(this.orientation);
		result.position = result.orientation * -this.position;
		return result;
	}

	// Token: 0x060037CF RID: 14287 RVA: 0x0011C994 File Offset: 0x0011AD94
	internal OVRPose flipZ()
	{
		OVRPose result = this;
		result.position.z = -result.position.z;
		result.orientation.z = -result.orientation.z;
		result.orientation.w = -result.orientation.w;
		return result;
	}

	// Token: 0x060037D0 RID: 14288 RVA: 0x0011C9F4 File Offset: 0x0011ADF4
	internal OVRPlugin.Posef ToPosef()
	{
		return new OVRPlugin.Posef
		{
			Position = this.position.ToVector3f(),
			Orientation = this.orientation.ToQuatf()
		};
	}

	// Token: 0x04002054 RID: 8276
	public Vector3 position;

	// Token: 0x04002055 RID: 8277
	public Quaternion orientation;
}
