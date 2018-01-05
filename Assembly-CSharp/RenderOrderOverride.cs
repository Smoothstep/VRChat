using System;
using UnityEngine;

// Token: 0x02000C33 RID: 3123
public class RenderOrderOverride : MonoBehaviour
{
	// Token: 0x06006110 RID: 24848 RVA: 0x00223F38 File Offset: 0x00222338
	private void Awake()
	{
		foreach (Renderer renderer in this.renderers)
		{
			foreach (Material material in renderer.materials)
			{
				material.renderQueue = this.renderQueue;
			}
		}
	}

	// Token: 0x040046B2 RID: 18098
	public Renderer[] renderers;

	// Token: 0x040046B3 RID: 18099
	public int renderQueue;
}
