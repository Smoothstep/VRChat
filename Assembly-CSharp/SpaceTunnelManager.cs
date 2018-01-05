using System;
using UnityEngine;

// Token: 0x02000C38 RID: 3128
public class SpaceTunnelManager : MonoBehaviour
{
	// Token: 0x0600612E RID: 24878 RVA: 0x00224708 File Offset: 0x00222B08
	private void OnEnable()
	{
		foreach (GameObject gameObject in this.hideObjects)
		{
			gameObject.SetActive(PlayerPrefs.GetInt("SETTING_LOADING_ANIMATION") != 0);
		}
	}

	// Token: 0x040046CD RID: 18125
	public GameObject[] hideObjects;
}
