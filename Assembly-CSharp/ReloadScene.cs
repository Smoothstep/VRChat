using System;
using UnityEngine;

// Token: 0x0200056C RID: 1388
public class ReloadScene : MonoBehaviour
{
	// Token: 0x06002F62 RID: 12130 RVA: 0x000E622F File Offset: 0x000E462F
	private void OnGUI()
	{
		if (GUILayout.Button("Reload", new GUILayoutOption[0]))
		{
			this.Reload();
		}
	}

	// Token: 0x06002F63 RID: 12131 RVA: 0x000E624C File Offset: 0x000E464C
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			this.Reload();
		}
	}

	// Token: 0x06002F64 RID: 12132 RVA: 0x000E6260 File Offset: 0x000E4660
	private void Reload()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}
