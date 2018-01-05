using System;
using UnityEngine;

// Token: 0x020008C8 RID: 2248
public class SU_Planet : MonoBehaviour
{
	// Token: 0x060044B0 RID: 17584 RVA: 0x0016F7F1 File Offset: 0x0016DBF1
	private void Start()
	{
		this._cacheTransform = base.transform;
	}

	// Token: 0x060044B1 RID: 17585 RVA: 0x0016F7FF File Offset: 0x0016DBFF
	private void Update()
	{
		if (this._cacheTransform != null)
		{
			this._cacheTransform.Rotate(this.planetRotation * Time.deltaTime);
		}
	}

	// Token: 0x04002E90 RID: 11920
	public Vector3 planetRotation;

	// Token: 0x04002E91 RID: 11921
	private Transform _cacheTransform;
}
