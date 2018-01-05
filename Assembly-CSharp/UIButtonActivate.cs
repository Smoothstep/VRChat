using System;
using UnityEngine;

// Token: 0x0200059C RID: 1436
[AddComponentMenu("NGUI/Interaction/Button Activate")]
public class UIButtonActivate : MonoBehaviour
{
	// Token: 0x0600300E RID: 12302 RVA: 0x000EBE31 File Offset: 0x000EA231
	private void OnClick()
	{
		if (this.target != null)
		{
			NGUITools.SetActive(this.target, this.state);
		}
	}

	// Token: 0x04001A82 RID: 6786
	public GameObject target;

	// Token: 0x04001A83 RID: 6787
	public bool state = true;
}
