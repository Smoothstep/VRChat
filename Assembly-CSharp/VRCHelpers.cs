using System;
using UnityEngine;

// Token: 0x02000B41 RID: 2881
public static class VRCHelpers
{
	// Token: 0x06005867 RID: 22631 RVA: 0x001E9EC0 File Offset: 0x001E82C0
	public static T GetComponentInSelfOrParent<T>(this GameObject obj) where T : Component
	{
		T t = obj.GetComponent<T>();
		if (t == null)
		{
			t = obj.GetComponentInParent<T>();
		}
		return t;
	}
}
