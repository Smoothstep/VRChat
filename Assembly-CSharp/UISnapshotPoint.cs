using System;
using UnityEngine;

// Token: 0x0200060F RID: 1551
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Internal/Snapshot Point")]
public class UISnapshotPoint : MonoBehaviour
{
	// Token: 0x060033D8 RID: 13272 RVA: 0x0010836F File Offset: 0x0010676F
	private void Start()
	{
		if (base.tag != "EditorOnly")
		{
			base.tag = "EditorOnly";
		}
	}

	// Token: 0x04001D86 RID: 7558
	public bool isOrthographic = true;

	// Token: 0x04001D87 RID: 7559
	public float nearClip = -100f;

	// Token: 0x04001D88 RID: 7560
	public float farClip = 100f;

	// Token: 0x04001D89 RID: 7561
	[Range(10f, 80f)]
	public int fieldOfView = 35;

	// Token: 0x04001D8A RID: 7562
	public float orthoSize = 30f;

	// Token: 0x04001D8B RID: 7563
	public Texture2D thumbnail;
}
