using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BB8 RID: 3000
	public class LinearBlendshape : MonoBehaviour
	{
		// Token: 0x06005CD6 RID: 23766 RVA: 0x00206A99 File Offset: 0x00204E99
		private void Awake()
		{
			if (this.skinnedMesh == null)
			{
				this.skinnedMesh = base.GetComponent<SkinnedMeshRenderer>();
			}
			if (this.linearMapping == null)
			{
				this.linearMapping = base.GetComponent<LinearMapping>();
			}
		}

		// Token: 0x06005CD7 RID: 23767 RVA: 0x00206AD8 File Offset: 0x00204ED8
		private void Update()
		{
			float value = this.linearMapping.value;
			if (value != this.lastValue)
			{
				float value2 = Util.RemapNumberClamped(value, 0f, 1f, 1f, 100f);
				this.skinnedMesh.SetBlendShapeWeight(0, value2);
			}
			this.lastValue = value;
		}

		// Token: 0x0400426B RID: 17003
		public LinearMapping linearMapping;

		// Token: 0x0400426C RID: 17004
		public SkinnedMeshRenderer skinnedMesh;

		// Token: 0x0400426D RID: 17005
		private float lastValue;
	}
}
