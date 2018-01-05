using System;
using UnityEngine;

// Token: 0x020007A1 RID: 1953
public class QuitOnEscapeOrBack : MonoBehaviour
{
	// Token: 0x06003F2E RID: 16174 RVA: 0x0013E453 File Offset: 0x0013C853
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}
