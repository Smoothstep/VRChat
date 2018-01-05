using System;
using UnityEngine;

// Token: 0x020005AC RID: 1452
[AddComponentMenu("NGUI/Interaction/Drag and Drop Root")]
public class UIDragDropRoot : MonoBehaviour
{
	// Token: 0x0600306A RID: 12394 RVA: 0x000ED3EA File Offset: 0x000EB7EA
	private void OnEnable()
	{
		UIDragDropRoot.root = base.transform;
	}

	// Token: 0x0600306B RID: 12395 RVA: 0x000ED3F7 File Offset: 0x000EB7F7
	private void OnDisable()
	{
		if (UIDragDropRoot.root == base.transform)
		{
			UIDragDropRoot.root = null;
		}
	}

	// Token: 0x04001AD3 RID: 6867
	public static Transform root;
}
