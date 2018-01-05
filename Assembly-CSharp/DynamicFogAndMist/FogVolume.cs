using System;
using UnityEngine;

namespace DynamicFogAndMist
{
	// Token: 0x020009B0 RID: 2480
	public class FogVolume : MonoBehaviour
	{
		// Token: 0x06004AD3 RID: 19155 RVA: 0x0018EACC File Offset: 0x0018CECC
		private void Start()
		{
			this.fog = DynamicFog.instance;
		}

		// Token: 0x06004AD4 RID: 19156 RVA: 0x0018EADC File Offset: 0x0018CEDC
		private void OnTriggerEnter(Collider other)
		{
			if (this.cameraInside)
			{
				return;
			}
			if (other.gameObject.transform.GetComponentInChildren<Camera>() == this.fog.fogCamera)
			{
				this.cameraInside = true;
				this.fog.SetTargetAlpha(this.targetFogAlpha, this.targetSkyHazeAlpha, this.transitionDuration);
				this.fog.SetTargetColors(this.targetFogColor1, this.targetFogColor2, this.transitionDuration);
			}
		}

		// Token: 0x06004AD5 RID: 19157 RVA: 0x0018EB5C File Offset: 0x0018CF5C
		private void OnTriggerExit(Collider other)
		{
			if (!this.cameraInside)
			{
				return;
			}
			if (other.gameObject.transform.GetComponentInChildren<Camera>() == this.fog.fogCamera)
			{
				this.cameraInside = false;
				this.fog.ClearTargetAlpha(this.transitionDuration);
				this.fog.ClearTargetColors(this.transitionDuration);
			}
		}

		// Token: 0x040032B2 RID: 12978
		private const float GRAY = 0.8901961f;

		// Token: 0x040032B3 RID: 12979
		[Tooltip("Target alpha for fog when camera enters this fog volume")]
		[Range(0f, 1f)]
		public float targetFogAlpha = 0.5f;

		// Token: 0x040032B4 RID: 12980
		[Tooltip("Target alpha for sky haze when camera enters this fog volume")]
		[Range(0f, 1f)]
		public float targetSkyHazeAlpha = 0.5f;

		// Token: 0x040032B5 RID: 12981
		[Tooltip("Target fog color 1 when gamera enters this fog folume")]
		public Color targetFogColor1 = new Color(0.8901961f, 0.8901961f, 0.8901961f);

		// Token: 0x040032B6 RID: 12982
		[Tooltip("Target fog color 2 when gamera enters this fog folume")]
		public Color targetFogColor2 = new Color(0.8901961f, 0.8901961f, 0.8901961f);

		// Token: 0x040032B7 RID: 12983
		[Tooltip("Set this to zero for changing fog alpha immediately upon enter/exit fog volume.")]
		public float transitionDuration = 3f;

		// Token: 0x040032B8 RID: 12984
		private DynamicFog fog;

		// Token: 0x040032B9 RID: 12985
		private bool cameraInside;
	}
}
