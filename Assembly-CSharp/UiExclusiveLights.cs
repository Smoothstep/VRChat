using System;
using UnityEngine;

// Token: 0x02000C42 RID: 3138
public class UiExclusiveLights : MonoBehaviour
{
	// Token: 0x0600616B RID: 24939 RVA: 0x00226380 File Offset: 0x00224780
	private void OnPreCull()
	{
		foreach (Light light in this.exclusiveLights)
		{
			light.enabled = true;
		}
	}

	// Token: 0x0600616C RID: 24940 RVA: 0x002263B4 File Offset: 0x002247B4
	private void OnPostRender()
	{
		foreach (Light light in this.exclusiveLights)
		{
			light.enabled = false;
		}
	}

	// Token: 0x04004709 RID: 18185
	public Light[] exclusiveLights;
}
