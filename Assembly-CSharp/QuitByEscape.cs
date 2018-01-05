using System;
using UnityEngine;

// Token: 0x02000474 RID: 1140
public class QuitByEscape : MonoBehaviour
{
	// Token: 0x0600277B RID: 10107 RVA: 0x000CC5A7 File Offset: 0x000CA9A7
	private void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}
