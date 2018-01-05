using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000B9F RID: 2975
	public class DestroyOnTriggerEnter : MonoBehaviour
	{
		// Token: 0x06005C64 RID: 23652 RVA: 0x002044D1 File Offset: 0x002028D1
		private void Start()
		{
			if (!string.IsNullOrEmpty(this.tagFilter))
			{
				this.useTag = true;
			}
		}

		// Token: 0x06005C65 RID: 23653 RVA: 0x002044EC File Offset: 0x002028EC
		private void OnTriggerEnter(Collider collider)
		{
			if (!this.useTag || (this.useTag && collider.gameObject.tag == this.tagFilter))
			{
				UnityEngine.Object.Destroy(collider.gameObject.transform.root.gameObject);
			}
		}

		// Token: 0x040041F6 RID: 16886
		public string tagFilter;

		// Token: 0x040041F7 RID: 16887
		private bool useTag;
	}
}
