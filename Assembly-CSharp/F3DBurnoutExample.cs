using System;
using UnityEngine;

// Token: 0x0200048E RID: 1166
public class F3DBurnoutExample : MonoBehaviour
{
	// Token: 0x06002824 RID: 10276 RVA: 0x000D0EC5 File Offset: 0x000CF2C5
	private void Start()
	{
		this.BurnoutID = Shader.PropertyToID("_BurnOut");
		this.turretParts = base.GetComponentsInChildren<MeshRenderer>();
	}

	// Token: 0x06002825 RID: 10277 RVA: 0x000D0EE4 File Offset: 0x000CF2E4
	private void Update()
	{
		for (int i = 0; i < this.turretParts.Length; i++)
		{
			this.turretParts[i].material.SetFloat(this.BurnoutID, Mathf.Lerp(0f, 2f, Mathf.Sin(Time.time) / 2f));
		}
	}

	// Token: 0x04001670 RID: 5744
	private MeshRenderer[] turretParts;

	// Token: 0x04001671 RID: 5745
	private int BurnoutID;
}
