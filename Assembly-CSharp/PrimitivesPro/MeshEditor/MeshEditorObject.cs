using System;
using UnityEngine;

namespace PrimitivesPro.MeshEditor
{
	// Token: 0x02000853 RID: 2131
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class MeshEditorObject : MonoBehaviour
	{
		// Token: 0x06004225 RID: 16933 RVA: 0x001502F3 File Offset: 0x0014E6F3
		public void UpdateMesh()
		{
			this.ppMesh.ApplyToMesh();
		}

		// Token: 0x06004226 RID: 16934 RVA: 0x00150300 File Offset: 0x0014E700
		public void Init()
		{
			if (this.ppMesh == null)
			{
				MeshFilter component = base.GetComponent<MeshFilter>();
				if (component && component.sharedMesh)
				{
					this.ppMesh = new PPMesh(component, base.transform);
				}
			}
		}

		// Token: 0x04002AFB RID: 11003
		public PPMesh ppMesh;
	}
}
