using System;
using UnityEngine;

// Token: 0x02000A1E RID: 2590
public class MenuSceneLoader : MonoBehaviour
{
	// Token: 0x06004E4B RID: 20043 RVA: 0x001A3FA5 File Offset: 0x001A23A5
	private void Awake()
	{
		if (this.m_Go == null)
		{
			this.m_Go = UnityEngine.Object.Instantiate<GameObject>(this.menuUI);
		}
	}

	// Token: 0x0400365D RID: 13917
	public GameObject menuUI;

	// Token: 0x0400365E RID: 13918
	private GameObject m_Go;
}
