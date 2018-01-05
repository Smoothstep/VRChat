using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BE4 RID: 3044
	public abstract class TeleportMarkerBase : MonoBehaviour
	{
		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06005E41 RID: 24129 RVA: 0x002108A3 File Offset: 0x0020ECA3
		public virtual bool showReticle
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005E42 RID: 24130 RVA: 0x002108A6 File Offset: 0x0020ECA6
		public void SetLocked(bool locked)
		{
			this.locked = locked;
			this.UpdateVisuals();
		}

		// Token: 0x06005E43 RID: 24131 RVA: 0x002108B5 File Offset: 0x0020ECB5
		public virtual void TeleportPlayer(Vector3 pointedAtPosition)
		{
		}

		// Token: 0x06005E44 RID: 24132
		public abstract void UpdateVisuals();

		// Token: 0x06005E45 RID: 24133
		public abstract void Highlight(bool highlight);

		// Token: 0x06005E46 RID: 24134
		public abstract void SetAlpha(float tintAlpha, float alphaPercent);

		// Token: 0x06005E47 RID: 24135
		public abstract bool ShouldActivate(Vector3 playerPosition);

		// Token: 0x06005E48 RID: 24136
		public abstract bool ShouldMovePlayer();

		// Token: 0x04004417 RID: 17431
		public bool locked;

		// Token: 0x04004418 RID: 17432
		public bool markerActive = true;
	}
}
