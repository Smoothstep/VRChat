using System;
using UnityEngine;

// Token: 0x020008D1 RID: 2257
public class SU_LaserImpact : MonoBehaviour
{
	// Token: 0x060044C7 RID: 17607 RVA: 0x0017023C File Offset: 0x0016E63C
	private void Start()
	{
		if (base.transform.Find("light") != null)
		{
			this._cacheLight = base.transform.Find("light");
			this._cacheLight.GetComponent<Light>().intensity = 1f;
			this._cacheLight.transform.Translate(Vector3.up * 5f, Space.Self);
		}
		else
		{
			Debug.LogWarning("Missing required child light. Impact light effect won't be visible");
		}
	}

	// Token: 0x060044C8 RID: 17608 RVA: 0x001702BE File Offset: 0x0016E6BE
	private void Update()
	{
		if (this._cacheLight != null)
		{
			this._cacheLight.GetComponent<Light>().intensity = (float)base.transform.GetComponent<ParticleEmitter>().particleCount / 50f;
		}
	}

	// Token: 0x04002EC2 RID: 11970
	public Transform _cacheLight;
}
