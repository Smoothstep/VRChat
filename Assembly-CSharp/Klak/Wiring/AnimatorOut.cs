using System;
using UnityEngine;

namespace Klak.Wiring
{
	// Token: 0x0200054C RID: 1356
	[AddComponentMenu("Klak/Wiring/Output/Component/Animator Out")]
	public class AnimatorOut : NodeBase
	{
		// Token: 0x1700071C RID: 1820
		// (set) Token: 0x06002EDE RID: 11998 RVA: 0x000E42B6 File Offset: 0x000E26B6
		[Inlet]
		public float speed
		{
			set
			{
				if (!base.enabled || this._animator == null)
				{
					return;
				}
				this._animator.speed = value;
			}
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x000E42E1 File Offset: 0x000E26E1
		[Inlet]
		public void ChangeState()
		{
			this._animator.Play(this._changeStateTo);
		}

		// Token: 0x0400193F RID: 6463
		[SerializeField]
		private Animator _animator;

		// Token: 0x04001940 RID: 6464
		[SerializeField]
		private string _changeStateTo;
	}
}
