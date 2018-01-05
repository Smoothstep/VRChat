using System;
using UnityEngine;

// Token: 0x02000A65 RID: 2661
public static class QuaternionExtensions
{
	// Token: 0x0600508F RID: 20623 RVA: 0x001B8764 File Offset: 0x001B6B64
	public static Quaternion Normalize(this Quaternion q)
	{
		float num = q.x * q.x;
		num += q.y * q.y;
		num += q.z * q.z;
		num += q.w * q.w;
		if (num <= 0.1f)
		{
			return q;
		}
		float num2 = 1f / Mathf.Sqrt(num);
		Quaternion result;
		result.x = q.x * num2;
		result.y = q.y * num2;
		result.z = q.z * num2;
		result.w = q.w * num2;
		return result;
	}

	// Token: 0x06005090 RID: 20624 RVA: 0x001B8814 File Offset: 0x001B6C14
	public static bool IsBad(this Quaternion quat)
	{
		return quat.w.IsBad() || quat.x.IsBad() || quat.y.IsBad() || quat.z.IsBad();
	}
}
