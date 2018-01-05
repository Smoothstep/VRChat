using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BB6 RID: 2998
	public class LinearAnimator : MonoBehaviour
	{
		// Token: 0x06005CCF RID: 23759 RVA: 0x00206914 File Offset: 0x00204D14
		private void Awake()
		{
			if (this.animator == null)
			{
				this.animator = base.GetComponent<Animator>();
			}
			this.animator.speed = 0f;
			if (this.linearMapping == null)
			{
				this.linearMapping = base.GetComponent<LinearMapping>();
			}
		}

		// Token: 0x06005CD0 RID: 23760 RVA: 0x0020696C File Offset: 0x00204D6C
		private void Update()
		{
			if (this.currentLinearMapping != this.linearMapping.value)
			{
				this.currentLinearMapping = this.linearMapping.value;
				this.animator.enabled = true;
				this.animator.Play(0, 0, this.currentLinearMapping);
				this.framesUnchanged = 0;
			}
			else
			{
				this.framesUnchanged++;
				if (this.framesUnchanged > 2)
				{
					this.animator.enabled = false;
				}
			}
		}

		// Token: 0x04004261 RID: 16993
		public LinearMapping linearMapping;

		// Token: 0x04004262 RID: 16994
		public Animator animator;

		// Token: 0x04004263 RID: 16995
		private float currentLinearMapping = float.NaN;

		// Token: 0x04004264 RID: 16996
		private int framesUnchanged;
	}
}
