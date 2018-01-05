using System;
using UnityEngine;

// Token: 0x02000AFE RID: 2814
public class TNAvatarCreate : MonoBehaviour
{
	// Token: 0x0600551A RID: 21786 RVA: 0x001D56B9 File Offset: 0x001D3AB9
	private void Start()
	{
		Debug.LogError("FIX PREVIOUS LINE");
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04003C14 RID: 15380
	private int avatarIndex;

	// Token: 0x04003C15 RID: 15381
	private GameObject[] avatarPrefabs;

	// Token: 0x04003C16 RID: 15382
	public bool persistent;
}
