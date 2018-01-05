using System;
using UnityEngine;

// Token: 0x02000AB6 RID: 2742
public class PSDestroyOnCompletion : MonoBehaviour
{
	// Token: 0x06005356 RID: 21334 RVA: 0x001CA9F0 File Offset: 0x001C8DF0
	public void Start()
	{
		this.ps = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x06005357 RID: 21335 RVA: 0x001CA9FE File Offset: 0x001C8DFE
	public void Update()
	{
		if (this.ps && !this.ps.IsAlive())
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04003AAD RID: 15021
	private ParticleSystem ps;
}
