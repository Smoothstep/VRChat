using System;
using UnityEngine;

// Token: 0x02000491 RID: 1169
public class F3DNebula : MonoBehaviour
{
	// Token: 0x0600282E RID: 10286 RVA: 0x000D10A5 File Offset: 0x000CF4A5
	private void Start()
	{
	}

	// Token: 0x0600282F RID: 10287 RVA: 0x000D10A8 File Offset: 0x000CF4A8
	private void Update()
	{
		base.transform.position -= Vector3.forward * Time.deltaTime * 1000f;
		if (base.transform.position.z < -2150f)
		{
			Vector3 position = base.transform.position;
			position.z = 2150f;
			base.transform.position = position;
			base.transform.rotation = UnityEngine.Random.rotation;
			base.transform.localScale = new Vector3(1f, 1f, 1f) * (float)UnityEngine.Random.Range(200, 800);
		}
	}
}
