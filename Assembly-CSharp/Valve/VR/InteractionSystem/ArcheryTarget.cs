using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BCE RID: 3022
	public class ArcheryTarget : MonoBehaviour
	{
		// Token: 0x06005D95 RID: 23957 RVA: 0x0020B389 File Offset: 0x00209789
		private void ApplyDamage()
		{
			this.OnDamageTaken();
		}

		// Token: 0x06005D96 RID: 23958 RVA: 0x0020B391 File Offset: 0x00209791
		private void FireExposure()
		{
			this.OnDamageTaken();
		}

		// Token: 0x06005D97 RID: 23959 RVA: 0x0020B399 File Offset: 0x00209799
		private void OnDamageTaken()
		{
			if (this.targetEnabled)
			{
				this.onTakeDamage.Invoke();
				base.StartCoroutine(this.FallDown());
				if (this.onceOnly)
				{
					this.targetEnabled = false;
				}
			}
		}

		// Token: 0x06005D98 RID: 23960 RVA: 0x0020B3D0 File Offset: 0x002097D0
		private IEnumerator FallDown()
		{
			if (this.baseTransform)
			{
				Quaternion startingRot = this.baseTransform.rotation;
				float startTime = Time.time;
				float rotLerp = 0f;
				while (rotLerp < 1f)
				{
					rotLerp = Util.RemapNumberClamped(Time.time, startTime, startTime + this.fallTime, 0f, 1f);
					this.baseTransform.rotation = Quaternion.Lerp(startingRot, this.fallenDownTransform.rotation, rotLerp);
					yield return null;
				}
			}
			yield return null;
			yield break;
		}

		// Token: 0x04004303 RID: 17155
		public UnityEvent onTakeDamage;

		// Token: 0x04004304 RID: 17156
		public bool onceOnly;

		// Token: 0x04004305 RID: 17157
		public Transform targetCenter;

		// Token: 0x04004306 RID: 17158
		public Transform baseTransform;

		// Token: 0x04004307 RID: 17159
		public Transform fallenDownTransform;

		// Token: 0x04004308 RID: 17160
		public float fallTime = 0.5f;

		// Token: 0x04004309 RID: 17161
		private const float targetRadius = 0.25f;

		// Token: 0x0400430A RID: 17162
		private bool targetEnabled = true;
	}
}
