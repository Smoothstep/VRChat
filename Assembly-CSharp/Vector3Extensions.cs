using System;
using UnityEngine;

// Token: 0x02000A6C RID: 2668
public static class Vector3Extensions
{
	// Token: 0x060050B0 RID: 20656 RVA: 0x001B9344 File Offset: 0x001B7744
	public static bool IsBad(this Vector3 vec)
	{
		return vec.x.IsBad() || vec.y.IsBad() || vec.z.IsBad();
	}
}
