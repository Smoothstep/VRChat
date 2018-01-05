using System;
using UnityEngine;

// Token: 0x02000733 RID: 1843
public static class GameObjectExtensions
{
	// Token: 0x06003BA7 RID: 15271 RVA: 0x0012C285 File Offset: 0x0012A685
	public static bool GetActive(this GameObject target)
	{
		return target.activeInHierarchy;
	}
}
