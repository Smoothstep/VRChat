using System;
using UnityEngine;

// Token: 0x02000480 RID: 1152
[ExecuteInEditMode]
public class F3DParticleScale : MonoBehaviour
{
	// Token: 0x060027E3 RID: 10211 RVA: 0x000CF99D File Offset: 0x000CDD9D
	private void ScaleShurikenSystems(float scaleFactor)
	{
	}

	// Token: 0x060027E4 RID: 10212 RVA: 0x000CF9A0 File Offset: 0x000CDDA0
	private void ScaleTrailRenderers(float scaleFactor)
	{
		TrailRenderer[] componentsInChildren = base.GetComponentsInChildren<TrailRenderer>();
		foreach (TrailRenderer trailRenderer in componentsInChildren)
		{
			trailRenderer.startWidth *= scaleFactor;
			trailRenderer.endWidth *= scaleFactor;
		}
	}

	// Token: 0x060027E5 RID: 10213 RVA: 0x000CF9EA File Offset: 0x000CDDEA
	private void Update()
	{
	}

	// Token: 0x04001616 RID: 5654
	[Range(0f, 20f)]
	public float ParticleScale = 1f;

	// Token: 0x04001617 RID: 5655
	public bool ScaleGameobject = true;
}
