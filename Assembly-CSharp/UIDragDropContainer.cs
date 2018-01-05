using System;
using UnityEngine;

// Token: 0x020005A9 RID: 1449
[AddComponentMenu("NGUI/Interaction/Drag and Drop Container")]
public class UIDragDropContainer : MonoBehaviour
{
	// Token: 0x0600305A RID: 12378 RVA: 0x000ED3C3 File Offset: 0x000EB7C3
	protected virtual void Start()
	{
		if (this.reparentTarget == null)
		{
			this.reparentTarget = base.transform;
		}
	}

	// Token: 0x04001ABD RID: 6845
	public Transform reparentTarget;
}
