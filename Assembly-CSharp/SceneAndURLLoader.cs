using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000A20 RID: 2592
public class SceneAndURLLoader : MonoBehaviour
{
	// Token: 0x06004E53 RID: 20051 RVA: 0x001A40DB File Offset: 0x001A24DB
	private void Awake()
	{
		this.m_PauseMenu = base.GetComponentInChildren<PauseMenu>();
	}

	// Token: 0x06004E54 RID: 20052 RVA: 0x001A40E9 File Offset: 0x001A24E9
	public void SceneLoad(string sceneName)
	{
		this.m_PauseMenu.MenuOff();
		SceneManager.LoadScene(sceneName);
	}

	// Token: 0x06004E55 RID: 20053 RVA: 0x001A40FC File Offset: 0x001A24FC
	public void LoadURL(string url)
	{
		Application.OpenURL(url);
	}

	// Token: 0x04003663 RID: 13923
	private PauseMenu m_PauseMenu;
}
