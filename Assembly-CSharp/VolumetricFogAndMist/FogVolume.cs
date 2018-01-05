using System;
using UnityEngine;

namespace VolumetricFogAndMist
{
	// Token: 0x020009B2 RID: 2482
	public class FogVolume : MonoBehaviour
	{
		// Token: 0x06004ADB RID: 19163 RVA: 0x0018ECCB File Offset: 0x0018D0CB
		private void Start()
		{
			if (this.targetFog == null)
			{
				this.targetFog = VolumetricFog.instance;
			}
			if (this.targetFog != null)
			{
				this.targetFog.useFogVolumes = true;
			}
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x0018ED08 File Offset: 0x0018D108
		private void OnTriggerEnter(Collider other)
		{
			if (this.cameraInside || this.targetFog == null)
			{
				return;
			}
			if (other == this.targetCollider || other.gameObject.transform.GetComponentInChildren<Camera>() == this.targetFog.fogCamera)
			{
				this.cameraInside = true;
				if (this.enableProfileTransition && this.targetProfile != null)
				{
					this.targetFog.SetTargetProfile(this.targetProfile, this.transitionDuration);
				}
				if (this.enableAlphaTransition)
				{
					this.targetFog.SetTargetAlpha(this.targetFogAlpha, this.targetSkyHazeAlpha, this.transitionDuration);
				}
				if (this.enableFogColorTransition)
				{
					this.targetFog.SetTargetColor(this.targetFogColor, this.transitionDuration);
				}
				if (this.enableFogSpecularColorTransition)
				{
					this.targetFog.SetTargetSpecularColor(this.targetFogSpecularColor, this.transitionDuration);
				}
				if (this.enableLightColorTransition)
				{
					this.targetFog.SetTargetLightColor(this.targetLightColor, this.transitionDuration);
				}
				if (this.debugMode)
				{
					Debug.Log("Fog Volume entered by " + other.name);
				}
			}
		}

		// Token: 0x06004ADD RID: 19165 RVA: 0x0018EE50 File Offset: 0x0018D250
		private void OnTriggerExit(Collider other)
		{
			if (!this.cameraInside || this.targetFog == null)
			{
				return;
			}
			if (other == this.targetCollider || other.gameObject.transform.GetComponentInChildren<Camera>() == this.targetFog.fogCamera)
			{
				this.cameraInside = false;
				if (this.enableProfileTransition && this.targetProfile != null)
				{
					this.targetFog.ClearTargetProfile(this.transitionDuration);
				}
				if (this.enableAlphaTransition)
				{
					this.targetFog.ClearTargetAlpha(this.transitionDuration);
				}
				if (this.enableFogColorTransition)
				{
					this.targetFog.ClearTargetColor(this.transitionDuration);
				}
				if (this.enableFogSpecularColorTransition)
				{
					this.targetFog.ClearTargetSpecularColor(this.transitionDuration);
				}
				if (this.enableLightColorTransition)
				{
					this.targetFog.ClearTargetLightColor(this.transitionDuration);
				}
				if (this.debugMode)
				{
					Debug.Log("Fog Volume exited by " + other.name);
				}
			}
		}

		// Token: 0x040032BB RID: 12987
		private const float GRAY = 0.8901961f;

		// Token: 0x040032BC RID: 12988
		[Tooltip("Enables transition to a given profile.")]
		public bool enableProfileTransition;

		// Token: 0x040032BD RID: 12989
		[Tooltip("Assign the transition profile.")]
		public VolumetricFogProfile targetProfile;

		// Token: 0x040032BE RID: 12990
		[Tooltip("Enables alpha transition.")]
		public bool enableAlphaTransition;

		// Token: 0x040032BF RID: 12991
		[Tooltip("Target alpha for fog when camera enters this fog volume")]
		[Range(0f, 1f)]
		public float targetFogAlpha = 0.5f;

		// Token: 0x040032C0 RID: 12992
		[Tooltip("Target alpha for sky haze when camera enters this fog volume")]
		[Range(0f, 1f)]
		public float targetSkyHazeAlpha = 0.5f;

		// Token: 0x040032C1 RID: 12993
		[Tooltip("Enables fog color transition.")]
		public bool enableFogColorTransition;

		// Token: 0x040032C2 RID: 12994
		[Tooltip("Target fog color 1 when gamera enters this fog folume")]
		public Color targetFogColor = new Color(0.8901961f, 0.8901961f, 0.8901961f);

		// Token: 0x040032C3 RID: 12995
		[Tooltip("Enables fog specular color transition.")]
		public bool enableFogSpecularColorTransition;

		// Token: 0x040032C4 RID: 12996
		[Tooltip("Target fog color 2 when gamera enters this fog folume")]
		public Color targetFogSpecularColor = new Color(0.8901961f, 0.8901961f, 0.8901961f);

		// Token: 0x040032C5 RID: 12997
		[Tooltip("Enables light color transition.")]
		public bool enableLightColorTransition;

		// Token: 0x040032C6 RID: 12998
		[Tooltip("Target light color when gamera enters this fog folume")]
		public Color targetLightColor = Color.white;

		// Token: 0x040032C7 RID: 12999
		[Tooltip("Set this to zero for changing fog alpha immediately upon enter/exit fog volume.")]
		public float transitionDuration = 3f;

		// Token: 0x040032C8 RID: 13000
		[Tooltip("Set collider that will trigger this fog volume. If not set, this fog volume will react to any collider which has the main camera. If you use a third person controller, assign the character collider here.")]
		public Collider targetCollider;

		// Token: 0x040032C9 RID: 13001
		[Tooltip("When enabled, a console message will be printed whenever this fog volume is entered or exited.")]
		public bool debugMode;

		// Token: 0x040032CA RID: 13002
		[Tooltip("Assign target Volumetric Fog component that will be affected by this volume.")]
		public VolumetricFog targetFog;

		// Token: 0x040032CB RID: 13003
		private bool cameraInside;
	}
}
