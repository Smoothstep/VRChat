using System;
using UnityEngine;

namespace DynamicFogAndMist
{
	// Token: 0x020009AE RID: 2478
	[ExecuteInEditMode]
	[HelpURL("http://kronnect.com/taptapgo")]
	public class DynamicFogManager : MonoBehaviour
	{
		// Token: 0x06004AC2 RID: 19138 RVA: 0x0018E241 File Offset: 0x0018C641
		private void OnEnable()
		{
			this.UpdateMaterialProperties();
		}

		// Token: 0x06004AC3 RID: 19139 RVA: 0x0018E249 File Offset: 0x0018C649
		private void Reset()
		{
			this.UpdateMaterialProperties();
		}

		// Token: 0x06004AC4 RID: 19140 RVA: 0x0018E254 File Offset: 0x0018C654
		private void Update()
		{
			if (this.sun != null)
			{
				bool flag = false;
				if (this.sun.transform.forward != this.sunDirection)
				{
					flag = true;
				}
				if (this.sunLight != null && (this.sunLight.color != this.sunColor || this.sunLight.intensity != this.sunIntensity))
				{
					flag = true;
				}
				if (flag)
				{
					this.UpdateFogColor();
				}
			}
			this.UpdateFogData();
		}

		// Token: 0x06004AC5 RID: 19141 RVA: 0x0018E2EC File Offset: 0x0018C6EC
		public void UpdateMaterialProperties()
		{
			this.UpdateFogData();
			this.UpdateFogColor();
		}

		// Token: 0x06004AC6 RID: 19142 RVA: 0x0018E2FC File Offset: 0x0018C6FC
		private void UpdateFogData()
		{
			Vector4 value = new Vector4(this.height + 0.001f, this.baselineHeight, Camera.main.farClipPlane * this.distance, this.heightFallOff);
			Shader.SetGlobalVector("_FogData", value);
			Shader.SetGlobalFloat("_FogData2", this.distanceFallOff * value.z + 0.0001f);
		}

		// Token: 0x06004AC7 RID: 19143 RVA: 0x0018E364 File Offset: 0x0018C764
		private void UpdateFogColor()
		{
			if (this.sun != null)
			{
				if (this.sunLight == null)
				{
					this.sunLight = this.sun.GetComponent<Light>();
				}
				if (this.sunLight != null && this.sunLight.transform != this.sun.transform)
				{
					this.sunLight = this.sun.GetComponent<Light>();
				}
				this.sunDirection = this.sun.transform.forward;
				if (this.sunLight != null)
				{
					this.sunColor = this.sunLight.color;
					this.sunIntensity = this.sunLight.intensity;
				}
			}
			float b = this.sunIntensity * Mathf.Clamp01(1f - this.sunDirection.y);
			Color value = this.color * this.sunColor * b;
			value.a = this.alpha;
			Shader.SetGlobalColor("_FogColor", value);
		}

		// Token: 0x040032A0 RID: 12960
		[Range(0f, 1f)]
		public float alpha = 1f;

		// Token: 0x040032A1 RID: 12961
		[Range(0f, 1f)]
		public float noiseStrength = 0.5f;

		// Token: 0x040032A2 RID: 12962
		[Range(0f, 0.999f)]
		public float distance = 0.2f;

		// Token: 0x040032A3 RID: 12963
		[Range(0f, 2f)]
		public float distanceFallOff = 1f;

		// Token: 0x040032A4 RID: 12964
		[Range(0f, 500f)]
		public float height = 1f;

		// Token: 0x040032A5 RID: 12965
		[Range(0f, 1f)]
		public float heightFallOff = 1f;

		// Token: 0x040032A6 RID: 12966
		public float baselineHeight;

		// Token: 0x040032A7 RID: 12967
		public Color color = new Color(0.89f, 0.89f, 0.89f, 1f);

		// Token: 0x040032A8 RID: 12968
		public GameObject sun;

		// Token: 0x040032A9 RID: 12969
		private Light sunLight;

		// Token: 0x040032AA RID: 12970
		private Vector3 sunDirection = Vector3.zero;

		// Token: 0x040032AB RID: 12971
		private Color sunColor = Color.white;

		// Token: 0x040032AC RID: 12972
		private float sunIntensity = 1f;
	}
}
