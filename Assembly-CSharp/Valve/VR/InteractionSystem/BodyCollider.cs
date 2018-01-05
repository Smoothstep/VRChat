using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000B93 RID: 2963
	[RequireComponent(typeof(CapsuleCollider))]
	public class BodyCollider : MonoBehaviour
	{
		// Token: 0x06005C30 RID: 23600 RVA: 0x00202ECE File Offset: 0x002012CE
		private void Awake()
		{
			this.capsuleCollider = base.GetComponent<CapsuleCollider>();
		}

		// Token: 0x06005C31 RID: 23601 RVA: 0x00202EDC File Offset: 0x002012DC
		private void FixedUpdate()
		{
			float num = Vector3.Dot(this.head.localPosition, Vector3.up);
			this.capsuleCollider.height = Mathf.Max(this.capsuleCollider.radius, num);
			base.transform.localPosition = this.head.localPosition - 0.5f * num * Vector3.up;
		}

		// Token: 0x040041B4 RID: 16820
		public Transform head;

		// Token: 0x040041B5 RID: 16821
		private CapsuleCollider capsuleCollider;
	}
}
