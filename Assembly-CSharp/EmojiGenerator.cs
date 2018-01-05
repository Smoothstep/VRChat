using System;
using UnityEngine;

// Token: 0x02000AC3 RID: 2755
public class EmojiGenerator : MonoBehaviour
{
	// Token: 0x060053B0 RID: 21424 RVA: 0x001CDD84 File Offset: 0x001CC184
	public void Spawn(int n)
	{
		GameObject gameObject = this.emojiPrefabs[n];
		if (gameObject == null)
		{
			return;
		}
		UnityEngine.Object.Instantiate<GameObject>(gameObject, base.transform.position, base.transform.rotation);
	}

	// Token: 0x04003B03 RID: 15107
	public GameObject[] emojiPrefabs;
}
