using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

namespace DynamicFogAndMist
{
	// Token: 0x020009AD RID: 2477
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[HelpURL("http://kronnect.com/taptapgo")]
	[ImageEffectAllowedInSceneView]
	public class DynamicFog : MonoBehaviour
	{
		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x06004AA8 RID: 19112 RVA: 0x0018C64C File Offset: 0x0018AA4C
		public static DynamicFog instance
		{
			get
			{
				if (DynamicFog._fog == null)
				{
					foreach (Camera camera in Camera.allCameras)
					{
						DynamicFog._fog = camera.GetComponent<DynamicFog>();
						if (DynamicFog._fog != null)
						{
							break;
						}
					}
				}
				return DynamicFog._fog;
			}
		}

		// Token: 0x06004AA9 RID: 19113 RVA: 0x0018C6AC File Offset: 0x0018AAAC
		public string GetCurrentPresetName()
		{
			return Enum.GetName(typeof(FOG_PRESET), this.preset);
		}

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x06004AAA RID: 19114 RVA: 0x0018C6C8 File Offset: 0x0018AAC8
		public Camera fogCamera
		{
			get
			{
				return this.currentCamera;
			}
		}

		// Token: 0x06004AAB RID: 19115 RVA: 0x0018C6D0 File Offset: 0x0018AAD0
		private void OnEnable()
		{
			this.Init();
			this.UpdateMaterialProperties();
		}

		// Token: 0x06004AAC RID: 19116 RVA: 0x0018C6DE File Offset: 0x0018AADE
		private void Reset()
		{
			this.UpdateMaterialProperties();
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x0018C6E8 File Offset: 0x0018AAE8
		private void OnDestroy()
		{
			this.fogMat = null;
			if (this.fogMatVol != null)
			{
				UnityEngine.Object.DestroyImmediate(this.fogMatVol);
				this.fogMatVol = null;
				if (this.fogMatDesktopPlusOrthogonal != null)
				{
					UnityEngine.Object.DestroyImmediate(this.fogMatDesktopPlusOrthogonal);
					this.fogMatDesktopPlusOrthogonal = null;
				}
			}
			if (this.fogMatAdv != null)
			{
				UnityEngine.Object.DestroyImmediate(this.fogMatAdv);
				this.fogMatAdv = null;
			}
			if (this.fogMatFogSky != null)
			{
				UnityEngine.Object.DestroyImmediate(this.fogMatFogSky);
				this.fogMatFogSky = null;
			}
			if (this.fogMatOnlyFog != null)
			{
				UnityEngine.Object.DestroyImmediate(this.fogMatOnlyFog);
				this.fogMatOnlyFog = null;
			}
			if (this.fogMatSimple != null)
			{
				UnityEngine.Object.DestroyImmediate(this.fogMatSimple);
				this.fogMatSimple = null;
			}
			if (this.fogMatBasic != null)
			{
				UnityEngine.Object.DestroyImmediate(this.fogMatBasic);
				this.fogMatBasic = null;
			}
			if (this.fogMatOrthogonal != null)
			{
				UnityEngine.Object.DestroyImmediate(this.fogMatOrthogonal);
				this.fogMatOrthogonal = null;
			}
			if (this.fogOfWarTexture != null)
			{
				UnityEngine.Object.DestroyImmediate(this.fogOfWarTexture);
				this.fogOfWarTexture = null;
			}
		}

		// Token: 0x06004AAE RID: 19118 RVA: 0x0018C837 File Offset: 0x0018AC37
		private void Init()
		{
			this.targetFogAlpha = -1f;
			this.targetSkyHazeAlpha = -1f;
			this.currentCamera = base.GetComponent<Camera>();
			this.UpdateFogOfWarTexture();
		}

		// Token: 0x06004AAF RID: 19119 RVA: 0x0018C864 File Offset: 0x0018AC64
		private void Update()
		{
			if (this.fogMat == null)
			{
				return;
			}
			if (this.targetFogAlpha >= 0f)
			{
				if (this.targetFogAlpha != this.currentFogAlpha || this.targetSkyHazeAlpha != this.currentSkyHazeAlpha)
				{
					if (this.transitionDuration > 0f)
					{
						this.currentFogAlpha = Mathf.Lerp(this.initialFogAlpha, this.targetFogAlpha, (Time.time - this.transitionStartTime) / this.transitionDuration);
						this.currentSkyHazeAlpha = Mathf.Lerp(this.initialSkyHazeAlpha, this.targetSkyHazeAlpha, (Time.time - this.transitionStartTime) / this.transitionDuration);
					}
					else
					{
						this.currentFogAlpha = this.targetFogAlpha;
						this.currentSkyHazeAlpha = this.targetSkyHazeAlpha;
					}
					this.fogMat.SetFloat("_FogAlpha", this.currentFogAlpha);
					this.SetSkyData();
				}
			}
			else if (this.currentFogAlpha != this.alpha || this.targetSkyHazeAlpha != this.currentSkyHazeAlpha)
			{
				if (this.transitionDuration > 0f)
				{
					this.currentFogAlpha = Mathf.Lerp(this.initialFogAlpha, this.alpha, (Time.time - this.transitionStartTime) / this.transitionDuration);
					this.currentSkyHazeAlpha = Mathf.Lerp(this.initialSkyHazeAlpha, this.alpha, (Time.time - this.transitionStartTime) / this.transitionDuration);
				}
				else
				{
					this.currentFogAlpha = this.alpha;
					this.currentSkyHazeAlpha = this.skyAlpha;
				}
				this.fogMat.SetFloat("_FogAlpha", this.currentFogAlpha);
				this.SetSkyData();
			}
			if (this.targetFogColors)
			{
				if (this.targetFogColor1 != this.currentFogColor1 || this.targetFogColor2 != this.currentFogColor2)
				{
					if (this.transitionDuration > 0f)
					{
						this.currentFogColor1 = Color.Lerp(this.initialFogColor1, this.targetFogColor1, (Time.time - this.transitionStartTime) / this.transitionDuration);
						this.currentFogColor2 = Color.Lerp(this.initialFogColor2, this.targetFogColor2, (Time.time - this.transitionStartTime) / this.transitionDuration);
					}
					else
					{
						this.currentFogColor1 = this.targetFogColor1;
						this.currentFogColor2 = this.targetFogColor2;
					}
					this.fogMat.SetColor("_FogColor", this.currentFogColor1);
					this.fogMat.SetColor("_FogColor2", this.currentFogColor2);
				}
			}
			else if (this.currentFogColor1 != this.color || this.currentFogColor2 != this.color2)
			{
				if (this.transitionDuration > 0f)
				{
					this.currentFogColor1 = Color.Lerp(this.initialFogColor1, this.color, (Time.time - this.transitionStartTime) / this.transitionDuration);
					this.currentFogColor2 = Color.Lerp(this.initialFogColor2, this.color2, (Time.time - this.transitionStartTime) / this.transitionDuration);
				}
				else
				{
					this.currentFogColor1 = this.color;
					this.currentFogColor2 = this.color2;
				}
				this.fogMat.SetColor("_FogColor", this.currentFogColor1);
				this.fogMat.SetColor("_FogColor2", this.currentFogColor2);
			}
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
		}

		// Token: 0x06004AB0 RID: 19120 RVA: 0x0018CC60 File Offset: 0x0018B060
		public void CheckPreset()
		{
			switch (this.preset)
			{
			case FOG_PRESET.Clear:
				this.alpha = 0f;
				break;
			case FOG_PRESET.Mist:
				this.alpha = 0.75f;
				this.skySpeed = 0.11f;
				this.skyHaze = 15f;
				this.skyNoiseStrength = 1f;
				this.skyAlpha = 0.33f;
				this.distance = 0f;
				this.distanceFallOff = 0.07f;
				this.height = 4.4f;
				this.heightFallOff = 1f;
				this.turbulence = 0f;
				this.noiseStrength = 0.6f;
				this.speed = 0.01f;
				this.color = new Color(0.89f, 0.89f, 0.89f, 1f);
				this.color2 = this.color;
				this.maxDistance = 0.999f;
				this.maxDistanceFallOff = 0f;
				break;
			case FOG_PRESET.WindyMist:
				this.alpha = 0.75f;
				this.skySpeed = 0.3f;
				this.skyHaze = 35f;
				this.skyNoiseStrength = 0.32f;
				this.skyAlpha = 0.33f;
				this.distance = 0f;
				this.distanceFallOff = 0.07f;
				this.height = 2f;
				this.heightFallOff = 1f;
				this.turbulence = 2f;
				this.noiseStrength = 0.6f;
				this.speed = 0.06f;
				this.color = new Color(0.89f, 0.89f, 0.89f, 1f);
				this.color2 = this.color;
				this.maxDistance = 0.999f;
				this.maxDistanceFallOff = 0f;
				break;
			case FOG_PRESET.GroundFog:
				this.alpha = 1f;
				this.skySpeed = 0.3f;
				this.skyHaze = 35f;
				this.skyNoiseStrength = 0.32f;
				this.skyAlpha = 0.33f;
				this.distance = 0f;
				this.distanceFallOff = 0f;
				this.height = 1f;
				this.heightFallOff = 1f;
				this.turbulence = 0.4f;
				this.noiseStrength = 0.7f;
				this.speed = 0.005f;
				this.color = new Color(0.89f, 0.89f, 0.89f, 1f);
				this.color2 = this.color;
				this.maxDistance = 0.999f;
				this.maxDistanceFallOff = 0f;
				break;
			case FOG_PRESET.Fog:
				this.alpha = 0.96f;
				this.skySpeed = 0.3f;
				this.skyHaze = 155f;
				this.skyNoiseStrength = 0.6f;
				this.skyAlpha = 0.93f;
				this.distance = ((!this.effectType.isPlus()) ? 0.01f : 0.2f);
				this.distanceFallOff = 0.04f;
				this.height = 20f;
				this.heightFallOff = 1f;
				this.turbulence = 0.4f;
				this.noiseStrength = 0.4f;
				this.speed = 0.005f;
				this.color = new Color(0.89f, 0.89f, 0.89f, 1f);
				this.color2 = this.color;
				this.maxDistance = 0.999f;
				this.maxDistanceFallOff = 0f;
				break;
			case FOG_PRESET.HeavyFog:
				this.alpha = 1f;
				this.skySpeed = 0.05f;
				this.skyHaze = 350f;
				this.skyNoiseStrength = 0.8f;
				this.skyAlpha = 0.97f;
				this.distance = ((!this.effectType.isPlus()) ? 0f : 0.1f);
				this.distanceFallOff = 0.045f;
				this.height = 35f;
				this.heightFallOff = 0.88f;
				this.turbulence = 0.4f;
				this.noiseStrength = 0.24f;
				this.speed = 0.003f;
				this.color = new Color(0.86f, 0.847f, 0.847f, 1f);
				this.color2 = this.color;
				this.maxDistance = 0.999f;
				this.maxDistanceFallOff = 0f;
				break;
			case FOG_PRESET.SandStorm:
				this.alpha = 1f;
				this.skySpeed = 0.49f;
				this.skyHaze = 333f;
				this.skyNoiseStrength = 0.72f;
				this.skyAlpha = 0.97f;
				this.distance = ((!this.effectType.isPlus()) ? 0f : 0.15f);
				this.distanceFallOff = 0.028f;
				this.height = 83f;
				this.heightFallOff = 0f;
				this.turbulence = 15f;
				this.noiseStrength = 0.45f;
				this.speed = 0.2f;
				this.color = new Color(0.364f, 0.36f, 0.36f, 1f);
				this.color2 = this.color;
				this.maxDistance = 0.999f;
				this.maxDistanceFallOff = 0f;
				break;
			}
		}

		// Token: 0x06004AB1 RID: 19121 RVA: 0x0018D1B5 File Offset: 0x0018B5B5
		private void OnPreCull()
		{
			if (this.currentCamera != null && this.currentCamera.depthTextureMode == DepthTextureMode.None)
			{
				this.currentCamera.depthTextureMode = DepthTextureMode.Depth;
			}
		}

		// Token: 0x06004AB2 RID: 19122 RVA: 0x0018D1E4 File Offset: 0x0018B5E4
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.fogMat == null || this.alpha == 0f || this.currentCamera == null)
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.currentCamera.orthographic)
			{
				if (!this.matOrtho)
				{
					this.ResetMaterial();
				}
				this.fogMat.SetVector("_ClipDir", this.currentCamera.transform.forward);
			}
			else if (this.matOrtho)
			{
				this.ResetMaterial();
			}
			if (this.useSinglePassStereoRenderingMatrix && VRSettings.enabled)
			{
				this.fogMat.SetMatrix("_ClipToWorld", this.currentCamera.cameraToWorldMatrix);
			}
			else
			{
				this.fogMat.SetMatrix("_ClipToWorld", this.currentCamera.cameraToWorldMatrix * this.currentCamera.projectionMatrix.inverse);
			}
			Graphics.Blit(source, destination, this.fogMat);
		}

		// Token: 0x06004AB3 RID: 19123 RVA: 0x0018D2FC File Offset: 0x0018B6FC
		private void ResetMaterial()
		{
			this.fogMat = null;
			this.fogMatAdv = null;
			this.fogMatFogSky = null;
			this.fogMatOnlyFog = null;
			this.fogMatSimple = null;
			this.fogMatBasic = null;
			this.fogMatVol = null;
			this.UpdateMaterialProperties();
		}

		// Token: 0x06004AB4 RID: 19124 RVA: 0x0018D338 File Offset: 0x0018B738
		public void UpdateMaterialProperties()
		{
			this.CheckPreset();
			this.CopyTransitionValues();
			switch (this.effectType)
			{
			case FOG_TYPE.MobileFogWithSkyHaze:
				if (this.fogMatFogSky == null)
				{
					string path;
					if (this.currentCamera.orthographic)
					{
						this.matOrtho = true;
						path = "Materials/DFOWithSky";
					}
					else
					{
						this.matOrtho = false;
						path = "Materials/DFGWithSky";
					}
					this.fogMatFogSky = UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>(path));
					this.fogMatFogSky.hideFlags = HideFlags.DontSave;
				}
				this.fogMat = this.fogMatFogSky;
				break;
			case FOG_TYPE.MobileFogOnlyGround:
				if (this.fogMatOnlyFog == null)
				{
					string path;
					if (this.currentCamera.orthographic)
					{
						this.matOrtho = true;
						path = "Materials/DFOOnlyFog";
					}
					else
					{
						this.matOrtho = false;
						path = "Materials/DFGOnlyFog";
					}
					this.fogMatOnlyFog = UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>(path));
					this.fogMatOnlyFog.hideFlags = HideFlags.DontSave;
				}
				this.fogMat = this.fogMatOnlyFog;
				break;
			case FOG_TYPE.DesktopFogPlusWithSkyHaze:
				if (this.fogMatVol == null)
				{
					string path;
					if (this.currentCamera.orthographic)
					{
						this.matOrtho = true;
						path = "Materials/DFODesktopPlus";
					}
					else
					{
						this.matOrtho = false;
						path = "Materials/DFGDesktopPlus";
					}
					this.fogMatVol = UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>(path));
					this.fogMatVol.hideFlags = HideFlags.DontSave;
				}
				this.fogMat = this.fogMatVol;
				break;
			case FOG_TYPE.MobileFogSimple:
				if (this.fogMatSimple == null)
				{
					string path;
					if (this.currentCamera.orthographic)
					{
						this.matOrtho = true;
						path = "Materials/DFOSimple";
					}
					else
					{
						this.matOrtho = false;
						path = "Materials/DFGSimple";
					}
					this.fogMatSimple = UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>(path));
					this.fogMatSimple.hideFlags = HideFlags.DontSave;
				}
				this.fogMat = this.fogMatSimple;
				break;
			case FOG_TYPE.MobileFogBasic:
				if (this.fogMatBasic == null)
				{
					string path;
					if (this.currentCamera.orthographic)
					{
						this.matOrtho = true;
						path = "Materials/DFOBasic";
					}
					else
					{
						this.matOrtho = false;
						path = "Materials/DFGBasic";
					}
					this.fogMatBasic = UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>(path));
					this.fogMatBasic.hideFlags = HideFlags.DontSave;
				}
				this.fogMat = this.fogMatBasic;
				break;
			case FOG_TYPE.MobileFogOrthogonal:
				if (this.fogMatOrthogonal == null)
				{
					string path;
					if (this.currentCamera.orthographic)
					{
						this.matOrtho = true;
						path = "Materials/DFOOrthogonal";
					}
					else
					{
						this.matOrtho = false;
						path = "Materials/DFGOrthogonal";
					}
					this.fogMatOrthogonal = UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>(path));
					this.fogMatOrthogonal.hideFlags = HideFlags.DontSave;
				}
				this.fogMat = this.fogMatOrthogonal;
				break;
			case FOG_TYPE.DesktopFogPlusOrthogonal:
				if (this.fogMatDesktopPlusOrthogonal == null)
				{
					string path;
					if (this.currentCamera.orthographic)
					{
						this.matOrtho = true;
						path = "Materials/DFODesktopPlusOrthogonal";
					}
					else
					{
						this.matOrtho = false;
						path = "Materials/DFGDesktopPlusOrthogonal";
					}
					this.fogMatDesktopPlusOrthogonal = UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>(path));
					this.fogMatDesktopPlusOrthogonal.hideFlags = HideFlags.DontSave;
				}
				this.fogMat = this.fogMatDesktopPlusOrthogonal;
				break;
			default:
				if (this.fogMatAdv == null)
				{
					string path;
					if (this.currentCamera.orthographic)
					{
						this.matOrtho = true;
						path = "Materials/DFODesktop";
					}
					else
					{
						this.matOrtho = false;
						path = "Materials/DFGDesktop";
					}
					this.fogMatAdv = UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>(path));
					this.fogMatAdv.hideFlags = HideFlags.DontSave;
				}
				this.fogMat = this.fogMatAdv;
				break;
			}
			if (this.fogMat == null)
			{
				return;
			}
			if (this.currentCamera == null)
			{
				this.currentCamera = base.GetComponent<Camera>();
			}
			this.fogMat.SetFloat("_FogSpeed", (this.effectType != FOG_TYPE.DesktopFogPlusWithSkyHaze) ? this.speed : (this.speed * 5f));
			Vector4 value = new Vector4(this.noiseStrength, this.turbulence, this.currentCamera.farClipPlane * 15f / 1000f, this.noiseScale);
			this.fogMat.SetVector("_FogNoiseData", value);
			Vector4 value2 = new Vector4(this.height + 0.001f, this.baselineHeight, (!this.clipUnderBaseline) ? -10000f : -0.01f, this.heightFallOff);
			if (this.effectType == FOG_TYPE.MobileFogOrthogonal || this.effectType == FOG_TYPE.DesktopFogPlusOrthogonal)
			{
				value2.z = this.maxHeight;
			}
			this.fogMat.SetVector("_FogHeightData", value2);
			this.fogMat.SetFloat("_FogAlpha", this.currentFogAlpha);
			Vector4 value3 = new Vector4(this.distance, this.distanceFallOff, this.maxDistance, this.maxDistanceFallOff);
			if (this.effectType.isPlus())
			{
				value3.x = this.currentCamera.farClipPlane * this.distance;
				value3.y = this.distanceFallOff * value3.x + 0.0001f;
				value3.z *= this.currentCamera.farClipPlane;
			}
			this.fogMat.SetVector("_FogDistance", value3);
			this.UpdateFogColor();
			this.SetSkyData();
			if (this.shaderKeywords == null)
			{
				this.shaderKeywords = new List<string>();
			}
			else
			{
				this.shaderKeywords.Clear();
			}
			if (this.fogOfWarEnabled)
			{
				if (this.fogOfWarTexture == null)
				{
					this.UpdateFogOfWarTexture();
				}
				this.fogMat.SetTexture("_FogOfWar", this.fogOfWarTexture);
				this.fogMat.SetVector("_FogOfWarCenter", this.fogOfWarCenter);
				this.fogMat.SetVector("_FogOfWarSize", this.fogOfWarSize);
				Vector3 vector = this.fogOfWarCenter - 0.5f * this.fogOfWarSize;
				this.fogMat.SetVector("_FogOfWarCenterAdjusted", new Vector3(vector.x / this.fogOfWarSize.x, 1f, vector.z / this.fogOfWarSize.z));
				this.shaderKeywords.Add("FOG_OF_WAR_ON");
			}
			if (this.enableDithering)
			{
				this.shaderKeywords.Add("DITHER_ON");
			}
			this.fogMat.shaderKeywords = this.shaderKeywords.ToArray();
		}

		// Token: 0x06004AB5 RID: 19125 RVA: 0x0018D9F2 File Offset: 0x0018BDF2
		private void CopyTransitionValues()
		{
			this.currentFogAlpha = this.alpha;
			this.currentSkyHazeAlpha = this.skyAlpha;
			this.currentFogColor1 = this.color;
			this.currentFogColor2 = this.color2;
		}

		// Token: 0x06004AB6 RID: 19126 RVA: 0x0018DA24 File Offset: 0x0018BE24
		private void SetSkyData()
		{
			Vector4 value = new Vector4(this.skyHaze, this.skySpeed, this.skyNoiseStrength, this.currentSkyHazeAlpha);
			this.fogMat.SetVector("_FogSkyData", value);
		}

		// Token: 0x06004AB7 RID: 19127 RVA: 0x0018DA64 File Offset: 0x0018BE64
		private void UpdateFogColor()
		{
			if (this.fogMat == null)
			{
				return;
			}
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
			this.fogMat.SetColor("_FogColor", b * this.currentFogColor1 * this.sunColor);
			this.fogMat.SetColor("_FogColor2", b * this.currentFogColor2 * this.sunColor);
		}

		// Token: 0x06004AB8 RID: 19128 RVA: 0x0018DBB0 File Offset: 0x0018BFB0
		public void SetTargetAlpha(float newFogAlpha, float newSkyHazeAlpha, float duration)
		{
			if (!this.useFogVolumes)
			{
				return;
			}
			this.initialFogAlpha = this.currentFogAlpha;
			this.initialSkyHazeAlpha = this.currentSkyHazeAlpha;
			this.targetFogAlpha = newFogAlpha;
			this.targetSkyHazeAlpha = newSkyHazeAlpha;
			this.transitionDuration = duration;
			this.transitionStartTime = Time.time;
		}

		// Token: 0x06004AB9 RID: 19129 RVA: 0x0018DC01 File Offset: 0x0018C001
		public void ClearTargetAlpha(float duration)
		{
			this.SetTargetAlpha(-1f, -1f, duration);
		}

		// Token: 0x06004ABA RID: 19130 RVA: 0x0018DC14 File Offset: 0x0018C014
		public void SetTargetColors(Color color1, Color color2, float duration)
		{
			if (!this.useFogVolumes)
			{
				return;
			}
			this.initialFogColor1 = this.currentFogColor1;
			this.initialFogColor2 = this.currentFogColor2;
			this.targetFogColor1 = color1;
			this.targetFogColor2 = color2;
			this.transitionDuration = duration;
			this.transitionStartTime = Time.time;
			this.targetFogColors = true;
		}

		// Token: 0x06004ABB RID: 19131 RVA: 0x0018DC6C File Offset: 0x0018C06C
		public void ClearTargetColors(float duration)
		{
			this.targetFogColors = false;
			this.SetTargetColors(this.color, this.color2, duration);
		}

		// Token: 0x06004ABC RID: 19132 RVA: 0x0018DC88 File Offset: 0x0018C088
		private void UpdateFogOfWarTexture()
		{
			if (!this.fogOfWarEnabled)
			{
				return;
			}
			int scaledSize = this.GetScaledSize(this.fogOfWarTextureSize, 1f);
			this.fogOfWarTexture = new Texture2D(scaledSize, scaledSize, TextureFormat.ARGB32, false);
			this.fogOfWarTexture.hideFlags = HideFlags.DontSave;
			this.fogOfWarTexture.filterMode = FilterMode.Bilinear;
			this.fogOfWarTexture.wrapMode = TextureWrapMode.Clamp;
			this.ResetFogOfWar();
		}

		// Token: 0x06004ABD RID: 19133 RVA: 0x0018DCF0 File Offset: 0x0018C0F0
		public void SetFogOfWarAlpha(Vector3 worldPosition, float radius, float fogNewAlpha)
		{
			if (this.fogOfWarTexture == null)
			{
				return;
			}
			float num = (worldPosition.x - this.fogOfWarCenter.x) / this.fogOfWarSize.x + 0.5f;
			if (num < 0f || num > 1f)
			{
				return;
			}
			float num2 = (worldPosition.z - this.fogOfWarCenter.z) / this.fogOfWarSize.z + 0.5f;
			if (num2 < 0f || num2 > 1f)
			{
				return;
			}
			int width = this.fogOfWarTexture.width;
			int num3 = this.fogOfWarTexture.height;
			int num4 = (int)(num * (float)width);
			int num5 = (int)(num2 * (float)num3);
			int num6 = num5 * width + num4;
			byte b = (byte)(fogNewAlpha * 255f);
			Color32 color = this.fogOfWarColorBuffer[num6];
			if (b != color.a)
			{
				float num7 = radius / this.fogOfWarSize.z;
				int num8 = Mathf.FloorToInt((float)num3 * num7);
				for (int i = num5 - num8; i <= num5 + num8; i++)
				{
					if (i > 0 && i < num3 - 1)
					{
						for (int j = num4 - num8; j <= num4 + num8; j++)
						{
							if (j > 0 && j < width - 1)
							{
								int num9 = Mathf.FloorToInt(Mathf.Sqrt((float)((num5 - i) * (num5 - i) + (num4 - j) * (num4 - j))));
								if (num9 <= num8)
								{
									num6 = i * width + j;
									Color32 color2 = this.fogOfWarColorBuffer[num6];
									color2.a = (byte)Mathf.Lerp((float)b, (float)color2.a, (float)num9 / (float)num8);
									this.fogOfWarColorBuffer[num6] = color2;
									this.fogOfWarTexture.SetPixel(j, i, color2);
								}
							}
						}
					}
				}
				this.fogOfWarTexture.Apply();
			}
		}

		// Token: 0x06004ABE RID: 19134 RVA: 0x0018DF00 File Offset: 0x0018C300
		public void ResetFogOfWarAlpha(Vector3 worldPosition, float radius)
		{
			if (this.fogOfWarTexture == null)
			{
				return;
			}
			float num = (worldPosition.x - this.fogOfWarCenter.x) / this.fogOfWarSize.x + 0.5f;
			if (num < 0f || num > 1f)
			{
				return;
			}
			float num2 = (worldPosition.z - this.fogOfWarCenter.z) / this.fogOfWarSize.z + 0.5f;
			if (num2 < 0f || num2 > 1f)
			{
				return;
			}
			int width = this.fogOfWarTexture.width;
			int num3 = this.fogOfWarTexture.height;
			int num4 = (int)(num * (float)width);
			int num5 = (int)(num2 * (float)num3);
			int num6 = num5 * width + num4;
			float num7 = radius / this.fogOfWarSize.z;
			int num8 = Mathf.FloorToInt((float)num3 * num7);
			for (int i = num5 - num8; i <= num5 + num8; i++)
			{
				if (i > 0 && i < num3 - 1)
				{
					for (int j = num4 - num8; j <= num4 + num8; j++)
					{
						if (j > 0 && j < width - 1)
						{
							int num9 = Mathf.FloorToInt(Mathf.Sqrt((float)((num5 - i) * (num5 - i) + (num4 - j) * (num4 - j))));
							if (num9 <= num8)
							{
								num6 = i * width + j;
								Color32 color = this.fogOfWarColorBuffer[num6];
								color.a = byte.MaxValue;
								this.fogOfWarColorBuffer[num6] = color;
								this.fogOfWarTexture.SetPixel(j, i, color);
							}
						}
					}
				}
				this.fogOfWarTexture.Apply();
			}
		}

		// Token: 0x06004ABF RID: 19135 RVA: 0x0018E0D0 File Offset: 0x0018C4D0
		public void ResetFogOfWar()
		{
			if (this.fogOfWarTexture == null)
			{
				return;
			}
			int num = this.fogOfWarTexture.height;
			int width = this.fogOfWarTexture.width;
			int num2 = num * width;
			if (this.fogOfWarColorBuffer == null || this.fogOfWarColorBuffer.Length != num2)
			{
				this.fogOfWarColorBuffer = new Color32[num2];
			}
			Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			for (int i = 0; i < num2; i++)
			{
				this.fogOfWarColorBuffer[i] = color;
			}
			this.fogOfWarTexture.SetPixels32(this.fogOfWarColorBuffer);
			this.fogOfWarTexture.Apply();
		}

		// Token: 0x06004AC0 RID: 19136 RVA: 0x0018E190 File Offset: 0x0018C590
		private int GetScaledSize(int size, float factor)
		{
			size = (int)((float)size / factor);
			size /= 4;
			if (size < 1)
			{
				size = 1;
			}
			return size * 4;
		}

		// Token: 0x0400325F RID: 12895
		public FOG_TYPE effectType = FOG_TYPE.DesktopFogPlusWithSkyHaze;

		// Token: 0x04003260 RID: 12896
		public FOG_PRESET preset = FOG_PRESET.Mist;

		// Token: 0x04003261 RID: 12897
		public bool useFogVolumes;

		// Token: 0x04003262 RID: 12898
		public bool enableDithering;

		// Token: 0x04003263 RID: 12899
		[Range(0f, 1f)]
		public float alpha = 1f;

		// Token: 0x04003264 RID: 12900
		[Range(0f, 1f)]
		public float noiseStrength = 0.5f;

		// Token: 0x04003265 RID: 12901
		[Range(0.01f, 1f)]
		public float noiseScale = 0.1f;

		// Token: 0x04003266 RID: 12902
		[Range(0f, 0.999f)]
		public float distance = 0.1f;

		// Token: 0x04003267 RID: 12903
		[Range(0.0001f, 2f)]
		public float distanceFallOff = 0.01f;

		// Token: 0x04003268 RID: 12904
		[Range(0f, 1.2f)]
		public float maxDistance = 0.999f;

		// Token: 0x04003269 RID: 12905
		[Range(0.0001f, 0.5f)]
		public float maxDistanceFallOff;

		// Token: 0x0400326A RID: 12906
		[Range(0f, 500f)]
		public float height = 1f;

		// Token: 0x0400326B RID: 12907
		[Range(0f, 500f)]
		public float maxHeight = 100f;

		// Token: 0x0400326C RID: 12908
		[Range(0.0001f, 1f)]
		public float heightFallOff = 0.1f;

		// Token: 0x0400326D RID: 12909
		public float baselineHeight;

		// Token: 0x0400326E RID: 12910
		public bool clipUnderBaseline;

		// Token: 0x0400326F RID: 12911
		[Range(0f, 15f)]
		public float turbulence = 0.1f;

		// Token: 0x04003270 RID: 12912
		[Range(0f, 5f)]
		public float speed = 0.1f;

		// Token: 0x04003271 RID: 12913
		public Color color = Color.white;

		// Token: 0x04003272 RID: 12914
		public Color color2 = Color.gray;

		// Token: 0x04003273 RID: 12915
		[Range(0f, 500f)]
		public float skyHaze = 50f;

		// Token: 0x04003274 RID: 12916
		[Range(0f, 1f)]
		public float skySpeed = 0.3f;

		// Token: 0x04003275 RID: 12917
		[Range(0f, 1f)]
		public float skyNoiseStrength = 0.1f;

		// Token: 0x04003276 RID: 12918
		[Range(0f, 1f)]
		public float skyAlpha = 1f;

		// Token: 0x04003277 RID: 12919
		public GameObject sun;

		// Token: 0x04003278 RID: 12920
		public bool fogOfWarEnabled;

		// Token: 0x04003279 RID: 12921
		public Vector3 fogOfWarCenter;

		// Token: 0x0400327A RID: 12922
		public Vector3 fogOfWarSize = new Vector3(1024f, 0f, 1024f);

		// Token: 0x0400327B RID: 12923
		public int fogOfWarTextureSize = 256;

		// Token: 0x0400327C RID: 12924
		public bool useSinglePassStereoRenderingMatrix;

		// Token: 0x0400327D RID: 12925
		public bool useXZDistance;

		// Token: 0x0400327E RID: 12926
		private Material fogMatAdv;

		// Token: 0x0400327F RID: 12927
		private Material fogMatFogSky;

		// Token: 0x04003280 RID: 12928
		private Material fogMatOnlyFog;

		// Token: 0x04003281 RID: 12929
		private Material fogMatVol;

		// Token: 0x04003282 RID: 12930
		private Material fogMatSimple;

		// Token: 0x04003283 RID: 12931
		private Material fogMatBasic;

		// Token: 0x04003284 RID: 12932
		private Material fogMatOrthogonal;

		// Token: 0x04003285 RID: 12933
		private Material fogMatDesktopPlusOrthogonal;

		// Token: 0x04003286 RID: 12934
		[SerializeField]
		private Material fogMat;

		// Token: 0x04003287 RID: 12935
		private float initialFogAlpha;

		// Token: 0x04003288 RID: 12936
		private float targetFogAlpha;

		// Token: 0x04003289 RID: 12937
		private float initialSkyHazeAlpha;

		// Token: 0x0400328A RID: 12938
		private float targetSkyHazeAlpha;

		// Token: 0x0400328B RID: 12939
		private bool targetFogColors;

		// Token: 0x0400328C RID: 12940
		private Color initialFogColor1;

		// Token: 0x0400328D RID: 12941
		private Color targetFogColor1;

		// Token: 0x0400328E RID: 12942
		private Color initialFogColor2;

		// Token: 0x0400328F RID: 12943
		private Color targetFogColor2;

		// Token: 0x04003290 RID: 12944
		private float transitionDuration;

		// Token: 0x04003291 RID: 12945
		private float transitionStartTime;

		// Token: 0x04003292 RID: 12946
		private float currentFogAlpha;

		// Token: 0x04003293 RID: 12947
		private float currentSkyHazeAlpha;

		// Token: 0x04003294 RID: 12948
		private Color currentFogColor1;

		// Token: 0x04003295 RID: 12949
		private Color currentFogColor2;

		// Token: 0x04003296 RID: 12950
		private Camera currentCamera;

		// Token: 0x04003297 RID: 12951
		private Texture2D fogOfWarTexture;

		// Token: 0x04003298 RID: 12952
		private Color32[] fogOfWarColorBuffer;

		// Token: 0x04003299 RID: 12953
		private Light sunLight;

		// Token: 0x0400329A RID: 12954
		private Vector3 sunDirection = Vector3.zero;

		// Token: 0x0400329B RID: 12955
		private Color sunColor = Color.white;

		// Token: 0x0400329C RID: 12956
		private float sunIntensity = 1f;

		// Token: 0x0400329D RID: 12957
		private static DynamicFog _fog;

		// Token: 0x0400329E RID: 12958
		private List<string> shaderKeywords;

		// Token: 0x0400329F RID: 12959
		private bool matOrtho;
	}
}
