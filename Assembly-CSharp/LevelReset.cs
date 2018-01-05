using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Token: 0x02000A22 RID: 2594
public class LevelReset : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06004E5A RID: 20058 RVA: 0x001A41B4 File Offset: 0x001A25B4
	public void OnPointerClick(PointerEventData data)
	{
		SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
	}

	// Token: 0x06004E5B RID: 20059 RVA: 0x001A41D4 File Offset: 0x001A25D4
	private void Update()
	{
	}
}
