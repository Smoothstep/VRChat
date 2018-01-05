using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BB5 RID: 2997
	public class LinearAnimation : MonoBehaviour
	{
		// Token: 0x06005CCC RID: 23756 RVA: 0x0020681C File Offset: 0x00204C1C
		private void Awake()
		{
			if (this.animation == null)
			{
				this.animation = base.GetComponent<Animation>();
			}
			if (this.linearMapping == null)
			{
				this.linearMapping = base.GetComponent<LinearMapping>();
			}
			this.animation.playAutomatically = true;
			this.animState = this.animation[this.animation.clip.name];
			this.animState.wrapMode = WrapMode.PingPong;
			this.animState.speed = 0f;
			this.animLength = this.animState.length;
		}

		// Token: 0x06005CCD RID: 23757 RVA: 0x002068C0 File Offset: 0x00204CC0
		private void Update()
		{
			float value = this.linearMapping.value;
			if (value != this.lastValue)
			{
				this.animState.time = value / this.animLength;
			}
			this.lastValue = value;
		}

		// Token: 0x0400425C RID: 16988
		public LinearMapping linearMapping;

		// Token: 0x0400425D RID: 16989
		public Animation animation;

		// Token: 0x0400425E RID: 16990
		private AnimationState animState;

		// Token: 0x0400425F RID: 16991
		private float animLength;

		// Token: 0x04004260 RID: 16992
		private float lastValue;
	}
}
