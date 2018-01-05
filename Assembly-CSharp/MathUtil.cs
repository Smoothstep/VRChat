using System;
using UnityEngine;

// Token: 0x02000AA5 RID: 2725
public static class MathUtil
{
	// Token: 0x060051FA RID: 20986 RVA: 0x001C0D84 File Offset: 0x001BF184
	public static Vector3 GetPlanarDirection(this Vector3 v)
	{
		Vector3 a = v;
		a.y = 0f;
		float magnitude = a.magnitude;
		if (Mathf.Approximately(magnitude, 0f))
		{
			return Vector3.zero;
		}
		return a / magnitude;
	}
}
