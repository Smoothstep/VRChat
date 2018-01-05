using System;
using UnityEngine.SceneManagement;

// Token: 0x0200075B RID: 1883
public class SceneManagerHelper
{
	// Token: 0x17000989 RID: 2441
	// (get) Token: 0x06003CE5 RID: 15589 RVA: 0x00133ED8 File Offset: 0x001322D8
	public static string ActiveSceneName
	{
		get
		{
			return SceneManager.GetActiveScene().name;
		}
	}

	// Token: 0x1700098A RID: 2442
	// (get) Token: 0x06003CE6 RID: 15590 RVA: 0x00133EF4 File Offset: 0x001322F4
	public static int ActiveSceneBuildIndex
	{
		get
		{
			return SceneManager.GetActiveScene().buildIndex;
		}
	}
}
