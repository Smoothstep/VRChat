using System;
using UnityEngine;

// Token: 0x020008C7 RID: 2247
public class SU_Moon : MonoBehaviour
{
	// Token: 0x060044AD RID: 17581 RVA: 0x0016F74C File Offset: 0x0016DB4C
	private void Start()
	{
		this._cacheTransform = base.transform;
		this._cacheMeshTransform = base.transform.Find("MoonObject");
	}

	// Token: 0x060044AE RID: 17582 RVA: 0x0016F770 File Offset: 0x0016DB70
	private void Update()
	{
		if (this._cacheTransform != null)
		{
			this._cacheTransform.Rotate(Vector3.up * this.orbitSpeed * Time.deltaTime);
		}
		if (this._cacheMeshTransform != null)
		{
			this._cacheMeshTransform.Rotate(Vector3.up * this.rotationSpeed * Time.deltaTime);
		}
	}

	// Token: 0x04002E8C RID: 11916
	public float orbitSpeed;

	// Token: 0x04002E8D RID: 11917
	public float rotationSpeed;

	// Token: 0x04002E8E RID: 11918
	private Transform _cacheTransform;

	// Token: 0x04002E8F RID: 11919
	private Transform _cacheMeshTransform;
}
