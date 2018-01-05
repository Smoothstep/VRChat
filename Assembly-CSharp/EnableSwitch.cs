using System;
using UnityEngine;

// Token: 0x020006E7 RID: 1767
public class EnableSwitch : MonoBehaviour
{
	// Token: 0x06003A3B RID: 14907 RVA: 0x00126900 File Offset: 0x00124D00
	public bool SetActive(int target)
	{
		if (target < 0 || target >= this.SwitchTargets.Length)
		{
			return false;
		}
		for (int i = 0; i < this.SwitchTargets.Length; i++)
		{
			this.SwitchTargets[i].SetActive(false);
		}
		this.SwitchTargets[target].SetActive(true);
		return true;
	}

	// Token: 0x0400231F RID: 8991
	public GameObject[] SwitchTargets;
}
