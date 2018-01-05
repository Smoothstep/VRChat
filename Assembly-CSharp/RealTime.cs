using System;
using UnityEngine;

// Token: 0x020005F9 RID: 1529
public class RealTime : MonoBehaviour
{
	// Token: 0x1700079D RID: 1949
	// (get) Token: 0x06003327 RID: 13095 RVA: 0x001011F7 File Offset: 0x000FF5F7
	public static float time
	{
		get
		{
			return Time.unscaledTime;
		}
	}

	// Token: 0x1700079E RID: 1950
	// (get) Token: 0x06003328 RID: 13096 RVA: 0x001011FE File Offset: 0x000FF5FE
	public static float deltaTime
	{
		get
		{
			return Time.unscaledDeltaTime;
		}
	}
}
