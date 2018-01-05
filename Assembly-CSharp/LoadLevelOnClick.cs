using System;
using UnityEngine;

// Token: 0x0200058C RID: 1420
[AddComponentMenu("NGUI/Examples/Load Level On Click")]
public class LoadLevelOnClick : MonoBehaviour
{
	// Token: 0x06002FD3 RID: 12243 RVA: 0x000EA036 File Offset: 0x000E8436
	private void OnClick()
	{
		if (!string.IsNullOrEmpty(this.levelName))
		{
			Application.LoadLevel(this.levelName);
		}
	}

	// Token: 0x04001A3C RID: 6716
	public string levelName;
}
