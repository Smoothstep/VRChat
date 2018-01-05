using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BC7 RID: 3015
	public class Unparent : MonoBehaviour
	{
		// Token: 0x06005D32 RID: 23858 RVA: 0x00208964 File Offset: 0x00206D64
		private void Start()
		{
			this.oldParent = base.transform.parent;
			base.transform.parent = null;
			base.gameObject.name = this.oldParent.gameObject.name + "." + base.gameObject.name;
		}

		// Token: 0x06005D33 RID: 23859 RVA: 0x002089BE File Offset: 0x00206DBE
		private void Update()
		{
			if (this.oldParent == null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06005D34 RID: 23860 RVA: 0x002089DC File Offset: 0x00206DDC
		public Transform GetOldParent()
		{
			return this.oldParent;
		}

		// Token: 0x040042C9 RID: 17097
		private Transform oldParent;
	}
}
