using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

namespace VolumetricFogAndMist
{
	// Token: 0x020009BE RID: 2494
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Rendering/Volumetric Fog & Mist")]
	[HelpURL("http://kronnect.com/taptapgo")]
	public class VolumetricFog : MonoBehaviour
	{
		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x06004AE2 RID: 19170 RVA: 0x0018F3C4 File Offset: 0x0018D7C4
		public static VolumetricFog instance
		{
			get
			{
				if (VolumetricFog._fog == null)
				{
					if (Camera.main != null)
					{
						VolumetricFog._fog = Camera.main.GetComponent<VolumetricFog>();
					}
					if (VolumetricFog._fog == null)
					{
						foreach (Camera camera in Camera.allCameras)
						{
							VolumetricFog._fog = camera.GetComponent<VolumetricFog>();
							if (VolumetricFog._fog != null)
							{
								break;
							}
						}
					}
				}
				return VolumetricFog._fog;
			}
		}

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x06004AE3 RID: 19171 RVA: 0x0018F453 File Offset: 0x0018D853
		// (set) Token: 0x06004AE4 RID: 19172 RVA: 0x0018F45B File Offset: 0x0018D85B
		public FOG_PRESET preset
		{
			get
			{
				return this._preset;
			}
			set
			{
				if (value != this._preset)
				{
					this._preset = value;
					this.UpdatePreset();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06004AE5 RID: 19173 RVA: 0x0018F47D File Offset: 0x0018D87D
		// (set) Token: 0x06004AE6 RID: 19174 RVA: 0x0018F488 File Offset: 0x0018D888
		public VolumetricFogProfile profile
		{
			get
			{
				return this._profile;
			}
			set
			{
				if (value != this._profile)
				{
					this._profile = value;
					if (this._profile != null)
					{
						this._profile.Load(this);
						this._preset = FOG_PRESET.Custom;
					}
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06004AE7 RID: 19175 RVA: 0x0018F4DC File Offset: 0x0018D8DC
		// (set) Token: 0x06004AE8 RID: 19176 RVA: 0x0018F4E4 File Offset: 0x0018D8E4
		public bool useFogVolumes
		{
			get
			{
				return this._useFogVolumes;
			}
			set
			{
				if (value != this._useFogVolumes)
				{
					this._useFogVolumes = value;
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06004AE9 RID: 19177 RVA: 0x0018F500 File Offset: 0x0018D900
		// (set) Token: 0x06004AEA RID: 19178 RVA: 0x0018F508 File Offset: 0x0018D908
		public bool debugDepthPass
		{
			get
			{
				return this._debugPass;
			}
			set
			{
				if (value != this._debugPass)
				{
					this._debugPass = value;
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06004AEB RID: 19179 RVA: 0x0018F524 File Offset: 0x0018D924
		// (set) Token: 0x06004AEC RID: 19180 RVA: 0x0018F52C File Offset: 0x0018D92C
		public TRANSPARENT_MODE transparencyBlendMode
		{
			get
			{
				return this._transparencyBlendMode;
			}
			set
			{
				if (value != this._transparencyBlendMode)
				{
					this._transparencyBlendMode = value;
					this.UpdateMaterialProperties();
					this.UpdateRenderComponents();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x06004AED RID: 19181 RVA: 0x0018F554 File Offset: 0x0018D954
		// (set) Token: 0x06004AEE RID: 19182 RVA: 0x0018F55C File Offset: 0x0018D95C
		public float transparencyBlendPower
		{
			get
			{
				return this._transparencyBlendPower;
			}
			set
			{
				if (value != this._transparencyBlendPower)
				{
					this._transparencyBlendPower = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x06004AEF RID: 19183 RVA: 0x0018F57E File Offset: 0x0018D97E
		// (set) Token: 0x06004AF0 RID: 19184 RVA: 0x0018F586 File Offset: 0x0018D986
		public LayerMask transparencyLayerMask
		{
			get
			{
				return this._transparencyLayerMask;
			}
			set
			{
				if (this._transparencyLayerMask != value)
				{
					this._transparencyLayerMask = value;
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x06004AF1 RID: 19185 RVA: 0x0018F5AC File Offset: 0x0018D9AC
		// (set) Token: 0x06004AF2 RID: 19186 RVA: 0x0018F5B4 File Offset: 0x0018D9B4
		public LIGHTING_MODEL lightingModel
		{
			get
			{
				return this._lightingModel;
			}
			set
			{
				if (value != this._lightingModel)
				{
					this._lightingModel = value;
					this.UpdateMaterialProperties();
					this.UpdateTexture();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x06004AF3 RID: 19187 RVA: 0x0018F5DC File Offset: 0x0018D9DC
		// (set) Token: 0x06004AF4 RID: 19188 RVA: 0x0018F5E4 File Offset: 0x0018D9E4
		public bool computeDepth
		{
			get
			{
				return this._computeDepth;
			}
			set
			{
				if (value != this._computeDepth)
				{
					this._computeDepth = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x06004AF5 RID: 19189 RVA: 0x0018F606 File Offset: 0x0018DA06
		// (set) Token: 0x06004AF6 RID: 19190 RVA: 0x0018F60E File Offset: 0x0018DA0E
		public COMPUTE_DEPTH_SCOPE computeDepthScope
		{
			get
			{
				return this._computeDepthScope;
			}
			set
			{
				if (value != this._computeDepthScope)
				{
					this._computeDepthScope = value;
				}
			}
		}

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06004AF7 RID: 19191 RVA: 0x0018F623 File Offset: 0x0018DA23
		// (set) Token: 0x06004AF8 RID: 19192 RVA: 0x0018F62B File Offset: 0x0018DA2B
		public bool renderBeforeTransparent
		{
			get
			{
				return this._renderBeforeTransparent;
			}
			set
			{
				if (value != this._renderBeforeTransparent)
				{
					this._renderBeforeTransparent = value;
					if (this._renderBeforeTransparent)
					{
						this._transparencyBlendMode = TRANSPARENT_MODE.None;
					}
					this.UpdateMaterialProperties();
					this.UpdateRenderComponents();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06004AF9 RID: 19193 RVA: 0x0018F665 File Offset: 0x0018DA65
		// (set) Token: 0x06004AFA RID: 19194 RVA: 0x0018F66D File Offset: 0x0018DA6D
		public GameObject sun
		{
			get
			{
				return this._sun;
			}
			set
			{
				if (value != this._sun)
				{
					this._sun = value;
					this.UpdateSun();
				}
			}
		}

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06004AFB RID: 19195 RVA: 0x0018F68D File Offset: 0x0018DA8D
		// (set) Token: 0x06004AFC RID: 19196 RVA: 0x0018F695 File Offset: 0x0018DA95
		public bool sunCopyColor
		{
			get
			{
				return this._sunCopyColor;
			}
			set
			{
				if (value != this._sunCopyColor)
				{
					this._sunCopyColor = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x06004AFD RID: 19197 RVA: 0x0018F6B7 File Offset: 0x0018DAB7
		// (set) Token: 0x06004AFE RID: 19198 RVA: 0x0018F6BF File Offset: 0x0018DABF
		public float density
		{
			get
			{
				return this._density;
			}
			set
			{
				if (value != this._density)
				{
					this._preset = FOG_PRESET.Custom;
					this._density = value;
					this.UpdateMaterialProperties();
					this.UpdateTextureAlpha();
					this.UpdateTexture();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x06004AFF RID: 19199 RVA: 0x0018F6F8 File Offset: 0x0018DAF8
		// (set) Token: 0x06004B00 RID: 19200 RVA: 0x0018F700 File Offset: 0x0018DB00
		public float noiseStrength
		{
			get
			{
				return this._noiseStrength;
			}
			set
			{
				if (value != this._noiseStrength)
				{
					this._preset = FOG_PRESET.Custom;
					this._noiseStrength = value;
					this.UpdateMaterialProperties();
					this.UpdateTextureAlpha();
					this.UpdateTexture();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06004B01 RID: 19201 RVA: 0x0018F739 File Offset: 0x0018DB39
		// (set) Token: 0x06004B02 RID: 19202 RVA: 0x0018F741 File Offset: 0x0018DB41
		public float noiseFinalMultiplier
		{
			get
			{
				return this._noiseFinalMultiplier;
			}
			set
			{
				if (value != this._noiseFinalMultiplier)
				{
					this._preset = FOG_PRESET.Custom;
					this._noiseFinalMultiplier = value;
					this.UpdateMaterialProperties();
					this.UpdateTextureAlpha();
					this.UpdateTexture();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06004B03 RID: 19203 RVA: 0x0018F77A File Offset: 0x0018DB7A
		// (set) Token: 0x06004B04 RID: 19204 RVA: 0x0018F782 File Offset: 0x0018DB82
		public float noiseSparse
		{
			get
			{
				return this._noiseSparse;
			}
			set
			{
				if (value != this._noiseSparse)
				{
					this._preset = FOG_PRESET.Custom;
					this._noiseSparse = value;
					this.UpdateMaterialProperties();
					this.UpdateTextureAlpha();
					this.UpdateTexture();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x06004B05 RID: 19205 RVA: 0x0018F7BB File Offset: 0x0018DBBB
		// (set) Token: 0x06004B06 RID: 19206 RVA: 0x0018F7C3 File Offset: 0x0018DBC3
		public float distance
		{
			get
			{
				return this._distance;
			}
			set
			{
				if (value != this._distance)
				{
					this._preset = FOG_PRESET.Custom;
					this._distance = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x06004B07 RID: 19207 RVA: 0x0018F7F0 File Offset: 0x0018DBF0
		// (set) Token: 0x06004B08 RID: 19208 RVA: 0x0018F7F8 File Offset: 0x0018DBF8
		public float maxFogLength
		{
			get
			{
				return this._maxFogLength;
			}
			set
			{
				if (value != this._maxFogLength)
				{
					this._maxFogLength = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x06004B09 RID: 19209 RVA: 0x0018F81A File Offset: 0x0018DC1A
		// (set) Token: 0x06004B0A RID: 19210 RVA: 0x0018F822 File Offset: 0x0018DC22
		public float maxFogLengthFallOff
		{
			get
			{
				return this._maxFogLengthFallOff;
			}
			set
			{
				if (value != this._maxFogLengthFallOff)
				{
					this._maxFogLengthFallOff = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x06004B0B RID: 19211 RVA: 0x0018F844 File Offset: 0x0018DC44
		// (set) Token: 0x06004B0C RID: 19212 RVA: 0x0018F84C File Offset: 0x0018DC4C
		public float distanceFallOff
		{
			get
			{
				return this._distanceFallOff;
			}
			set
			{
				if (value != this._distanceFallOff)
				{
					this._preset = FOG_PRESET.Custom;
					this._distanceFallOff = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x06004B0D RID: 19213 RVA: 0x0018F879 File Offset: 0x0018DC79
		// (set) Token: 0x06004B0E RID: 19214 RVA: 0x0018F881 File Offset: 0x0018DC81
		public float height
		{
			get
			{
				return this._height;
			}
			set
			{
				if (value != this._height)
				{
					this._preset = FOG_PRESET.Custom;
					this._height = Mathf.Max(value, 1E-05f);
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x06004B0F RID: 19215 RVA: 0x0018F8B8 File Offset: 0x0018DCB8
		// (set) Token: 0x06004B10 RID: 19216 RVA: 0x0018F8C0 File Offset: 0x0018DCC0
		public float baselineHeight
		{
			get
			{
				return this._baselineHeight;
			}
			set
			{
				if (value != this._baselineHeight)
				{
					this._preset = FOG_PRESET.Custom;
					this._baselineHeight = value;
					if (this._fogAreaRadius > 0f)
					{
						this._fogAreaPosition.y = this._baselineHeight;
					}
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x06004B11 RID: 19217 RVA: 0x0018F919 File Offset: 0x0018DD19
		// (set) Token: 0x06004B12 RID: 19218 RVA: 0x0018F921 File Offset: 0x0018DD21
		public bool baselineRelativeToCamera
		{
			get
			{
				return this._baselineRelativeToCamera;
			}
			set
			{
				if (value != this._baselineRelativeToCamera)
				{
					this._preset = FOG_PRESET.Custom;
					this._baselineRelativeToCamera = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x06004B13 RID: 19219 RVA: 0x0018F94E File Offset: 0x0018DD4E
		// (set) Token: 0x06004B14 RID: 19220 RVA: 0x0018F956 File Offset: 0x0018DD56
		public float baselineRelativeToCameraDelay
		{
			get
			{
				return this._baselineRelativeToCameraDelay;
			}
			set
			{
				if (value != this._baselineRelativeToCameraDelay)
				{
					this._baselineRelativeToCameraDelay = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x06004B15 RID: 19221 RVA: 0x0018F978 File Offset: 0x0018DD78
		// (set) Token: 0x06004B16 RID: 19222 RVA: 0x0018F980 File Offset: 0x0018DD80
		public float noiseScale
		{
			get
			{
				return this._noiseScale;
			}
			set
			{
				if (value != this._noiseScale)
				{
					this._preset = FOG_PRESET.Custom;
					this._noiseScale = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x06004B17 RID: 19223 RVA: 0x0018F9AD File Offset: 0x0018DDAD
		// (set) Token: 0x06004B18 RID: 19224 RVA: 0x0018F9B5 File Offset: 0x0018DDB5
		public float alpha
		{
			get
			{
				return this._alpha;
			}
			set
			{
				if (value != this._alpha)
				{
					this._preset = FOG_PRESET.Custom;
					this._alpha = value;
					this.currentFogAlpha = this._alpha;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x06004B19 RID: 19225 RVA: 0x0018F9EE File Offset: 0x0018DDEE
		// (set) Token: 0x06004B1A RID: 19226 RVA: 0x0018F9F6 File Offset: 0x0018DDF6
		public Color color
		{
			get
			{
				return this._color;
			}
			set
			{
				if (value != this._color)
				{
					this._preset = FOG_PRESET.Custom;
					this._color = value;
					this.currentFogColor = this._color;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06004B1B RID: 19227 RVA: 0x0018FA34 File Offset: 0x0018DE34
		// (set) Token: 0x06004B1C RID: 19228 RVA: 0x0018FA3C File Offset: 0x0018DE3C
		public Color specularColor
		{
			get
			{
				return this._specularColor;
			}
			set
			{
				if (value != this._specularColor)
				{
					this._preset = FOG_PRESET.Custom;
					this._specularColor = value;
					this.currentFogSpecularColor = this._specularColor;
					this.UpdateMaterialProperties();
					this.UpdateTexture();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06004B1D RID: 19229 RVA: 0x0018FA8B File Offset: 0x0018DE8B
		// (set) Token: 0x06004B1E RID: 19230 RVA: 0x0018FA93 File Offset: 0x0018DE93
		public float specularThreshold
		{
			get
			{
				return this._specularThreshold;
			}
			set
			{
				if (value != this._specularThreshold)
				{
					this._preset = FOG_PRESET.Custom;
					this._specularThreshold = value;
					this.UpdateTexture();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06004B1F RID: 19231 RVA: 0x0018FAC0 File Offset: 0x0018DEC0
		// (set) Token: 0x06004B20 RID: 19232 RVA: 0x0018FAC8 File Offset: 0x0018DEC8
		public float specularIntensity
		{
			get
			{
				return this._specularIntensity;
			}
			set
			{
				if (value != this._specularIntensity)
				{
					this._preset = FOG_PRESET.Custom;
					this._specularIntensity = value;
					this.UpdateMaterialProperties();
					this.UpdateTexture();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x06004B21 RID: 19233 RVA: 0x0018FAFB File Offset: 0x0018DEFB
		// (set) Token: 0x06004B22 RID: 19234 RVA: 0x0018FB03 File Offset: 0x0018DF03
		public Vector3 lightDirection
		{
			get
			{
				return this._lightDirection;
			}
			set
			{
				if (value != this._lightDirection)
				{
					this._preset = FOG_PRESET.Custom;
					this._lightDirection = value;
					this.UpdateMaterialProperties();
					this.UpdateTexture();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06004B23 RID: 19235 RVA: 0x0018FB3B File Offset: 0x0018DF3B
		// (set) Token: 0x06004B24 RID: 19236 RVA: 0x0018FB43 File Offset: 0x0018DF43
		public float lightIntensity
		{
			get
			{
				return this._lightIntensity;
			}
			set
			{
				if (value != this._lightIntensity)
				{
					this._preset = FOG_PRESET.Custom;
					this._lightIntensity = value;
					this.UpdateMaterialProperties();
					this.UpdateTexture();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06004B25 RID: 19237 RVA: 0x0018FB76 File Offset: 0x0018DF76
		// (set) Token: 0x06004B26 RID: 19238 RVA: 0x0018FB80 File Offset: 0x0018DF80
		public Color lightColor
		{
			get
			{
				return this._lightColor;
			}
			set
			{
				if (value != this._lightColor)
				{
					this._preset = FOG_PRESET.Custom;
					this._lightColor = value;
					this.currentLightColor = this._lightColor;
					this.UpdateMaterialProperties();
					this.UpdateTexture();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x06004B27 RID: 19239 RVA: 0x0018FBCF File Offset: 0x0018DFCF
		// (set) Token: 0x06004B28 RID: 19240 RVA: 0x0018FBD7 File Offset: 0x0018DFD7
		public int updateTextureSpread
		{
			get
			{
				return this._updateTextureSpread;
			}
			set
			{
				if (value != this._updateTextureSpread)
				{
					this._updateTextureSpread = value;
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06004B29 RID: 19241 RVA: 0x0018FBF3 File Offset: 0x0018DFF3
		// (set) Token: 0x06004B2A RID: 19242 RVA: 0x0018FBFB File Offset: 0x0018DFFB
		public float speed
		{
			get
			{
				return this._speed;
			}
			set
			{
				if (value != this._speed)
				{
					this._preset = FOG_PRESET.Custom;
					this._speed = value;
					if (!Application.isPlaying)
					{
						this.UpdateWindSpeedQuick();
					}
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06004B2B RID: 19243 RVA: 0x0018FC32 File Offset: 0x0018E032
		// (set) Token: 0x06004B2C RID: 19244 RVA: 0x0018FC3C File Offset: 0x0018E03C
		public Vector3 windDirection
		{
			get
			{
				return this._windDirection;
			}
			set
			{
				if (value != this._windDirection)
				{
					this._preset = FOG_PRESET.Custom;
					this._windDirection = value.normalized;
					if (!Application.isPlaying)
					{
						this.UpdateWindSpeedQuick();
					}
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x06004B2D RID: 19245 RVA: 0x0018FC89 File Offset: 0x0018E089
		// (set) Token: 0x06004B2E RID: 19246 RVA: 0x0018FC91 File Offset: 0x0018E091
		public Color skyColor
		{
			get
			{
				return this._skyColor;
			}
			set
			{
				if (value != this._skyColor)
				{
					this._preset = FOG_PRESET.Custom;
					this._skyColor = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x06004B2F RID: 19247 RVA: 0x0018FCC3 File Offset: 0x0018E0C3
		// (set) Token: 0x06004B30 RID: 19248 RVA: 0x0018FCCB File Offset: 0x0018E0CB
		public float skyHaze
		{
			get
			{
				return this._skyHaze;
			}
			set
			{
				if (value != this._skyHaze)
				{
					this._preset = FOG_PRESET.Custom;
					this._skyHaze = value;
					if (!Application.isPlaying)
					{
						this.UpdateWindSpeedQuick();
					}
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x06004B31 RID: 19249 RVA: 0x0018FD02 File Offset: 0x0018E102
		// (set) Token: 0x06004B32 RID: 19250 RVA: 0x0018FD0A File Offset: 0x0018E10A
		public float skySpeed
		{
			get
			{
				return this._skySpeed;
			}
			set
			{
				if (value != this._skySpeed)
				{
					this._preset = FOG_PRESET.Custom;
					this._skySpeed = value;
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x06004B33 RID: 19251 RVA: 0x0018FD31 File Offset: 0x0018E131
		// (set) Token: 0x06004B34 RID: 19252 RVA: 0x0018FD39 File Offset: 0x0018E139
		public float skyNoiseStrength
		{
			get
			{
				return this._skyNoiseStrength;
			}
			set
			{
				if (value != this._skyNoiseStrength)
				{
					this._preset = FOG_PRESET.Custom;
					this._skyNoiseStrength = value;
					if (!Application.isPlaying)
					{
						this.UpdateWindSpeedQuick();
					}
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x06004B35 RID: 19253 RVA: 0x0018FD70 File Offset: 0x0018E170
		// (set) Token: 0x06004B36 RID: 19254 RVA: 0x0018FD78 File Offset: 0x0018E178
		public float skyAlpha
		{
			get
			{
				return this._skyAlpha;
			}
			set
			{
				if (value != this._skyAlpha)
				{
					this._preset = FOG_PRESET.Custom;
					this._skyAlpha = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x06004B37 RID: 19255 RVA: 0x0018FDA5 File Offset: 0x0018E1A5
		// (set) Token: 0x06004B38 RID: 19256 RVA: 0x0018FDAD File Offset: 0x0018E1AD
		public float skyDepth
		{
			get
			{
				return this._skyDepth;
			}
			set
			{
				if (value != this._skyDepth)
				{
					this._skyDepth = value;
					if (!Application.isPlaying)
					{
						this.UpdateWindSpeedQuick();
					}
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x06004B39 RID: 19257 RVA: 0x0018FDD9 File Offset: 0x0018E1D9
		// (set) Token: 0x06004B3A RID: 19258 RVA: 0x0018FDE1 File Offset: 0x0018E1E1
		public GameObject character
		{
			get
			{
				return this._character;
			}
			set
			{
				if (value != this._character)
				{
					this._character = value;
					this.isDirty = true;
					if (this._fogVoidRadius < 20f)
					{
						this.fogVoidRadius = 20f;
					}
				}
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06004B3B RID: 19259 RVA: 0x0018FE1D File Offset: 0x0018E21D
		// (set) Token: 0x06004B3C RID: 19260 RVA: 0x0018FE25 File Offset: 0x0018E225
		public FOG_VOID_TOPOLOGY fogVoidTopology
		{
			get
			{
				return this._fogVoidTopology;
			}
			set
			{
				if (value != this._fogVoidTopology)
				{
					this._fogVoidTopology = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x06004B3D RID: 19261 RVA: 0x0018FE47 File Offset: 0x0018E247
		// (set) Token: 0x06004B3E RID: 19262 RVA: 0x0018FE4F File Offset: 0x0018E24F
		public float fogVoidFallOff
		{
			get
			{
				return this._fogVoidFallOff;
			}
			set
			{
				if (value != this._fogVoidFallOff)
				{
					this._preset = FOG_PRESET.Custom;
					this._fogVoidFallOff = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x06004B3F RID: 19263 RVA: 0x0018FE7C File Offset: 0x0018E27C
		// (set) Token: 0x06004B40 RID: 19264 RVA: 0x0018FE84 File Offset: 0x0018E284
		public float fogVoidRadius
		{
			get
			{
				return this._fogVoidRadius;
			}
			set
			{
				if (value != this._fogVoidRadius)
				{
					this._preset = FOG_PRESET.Custom;
					this._fogVoidRadius = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x06004B41 RID: 19265 RVA: 0x0018FEB1 File Offset: 0x0018E2B1
		// (set) Token: 0x06004B42 RID: 19266 RVA: 0x0018FEB9 File Offset: 0x0018E2B9
		public Vector3 fogVoidPosition
		{
			get
			{
				return this._fogVoidPosition;
			}
			set
			{
				if (value != this._fogVoidPosition)
				{
					this._preset = FOG_PRESET.Custom;
					this._fogVoidPosition = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06004B43 RID: 19267 RVA: 0x0018FEEB File Offset: 0x0018E2EB
		// (set) Token: 0x06004B44 RID: 19268 RVA: 0x0018FEF3 File Offset: 0x0018E2F3
		public float fogVoidDepth
		{
			get
			{
				return this._fogVoidDepth;
			}
			set
			{
				if (value != this._fogVoidDepth)
				{
					this._preset = FOG_PRESET.Custom;
					this._fogVoidDepth = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x06004B45 RID: 19269 RVA: 0x0018FF20 File Offset: 0x0018E320
		// (set) Token: 0x06004B46 RID: 19270 RVA: 0x0018FF28 File Offset: 0x0018E328
		public float fogVoidHeight
		{
			get
			{
				return this._fogVoidHeight;
			}
			set
			{
				if (value != this._fogVoidHeight)
				{
					this._preset = FOG_PRESET.Custom;
					this._fogVoidHeight = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x06004B47 RID: 19271 RVA: 0x0018FF55 File Offset: 0x0018E355
		// (set) Token: 0x06004B48 RID: 19272 RVA: 0x0018FF5D File Offset: 0x0018E35D
		[Obsolete("Fog Void inverted is now deprecated. Use Fog Area settings.")]
		public bool fogVoidInverted
		{
			get
			{
				return this._fogVoidInverted;
			}
			set
			{
				this._fogVoidInverted = value;
			}
		}

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x06004B49 RID: 19273 RVA: 0x0018FF66 File Offset: 0x0018E366
		// (set) Token: 0x06004B4A RID: 19274 RVA: 0x0018FF6E File Offset: 0x0018E36E
		public GameObject fogAreaCenter
		{
			get
			{
				return this._fogAreaCenter;
			}
			set
			{
				if (value != this._character)
				{
					this._fogAreaCenter = value;
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x06004B4B RID: 19275 RVA: 0x0018FF8F File Offset: 0x0018E38F
		// (set) Token: 0x06004B4C RID: 19276 RVA: 0x0018FF97 File Offset: 0x0018E397
		public float fogAreaFallOff
		{
			get
			{
				return this._fogAreaFallOff;
			}
			set
			{
				if (value != this._fogAreaFallOff)
				{
					this._fogAreaFallOff = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x06004B4D RID: 19277 RVA: 0x0018FFB9 File Offset: 0x0018E3B9
		// (set) Token: 0x06004B4E RID: 19278 RVA: 0x0018FFC1 File Offset: 0x0018E3C1
		public FOG_AREA_FOLLOW_MODE fogAreaFollowMode
		{
			get
			{
				return this._fogAreaFollowMode;
			}
			set
			{
				if (value != this._fogAreaFollowMode)
				{
					this._fogAreaFollowMode = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x06004B4F RID: 19279 RVA: 0x0018FFE3 File Offset: 0x0018E3E3
		// (set) Token: 0x06004B50 RID: 19280 RVA: 0x0018FFEB File Offset: 0x0018E3EB
		public FOG_AREA_TOPOLOGY fogAreaTopology
		{
			get
			{
				return this._fogAreaTopology;
			}
			set
			{
				if (value != this._fogAreaTopology)
				{
					this._fogAreaTopology = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x06004B51 RID: 19281 RVA: 0x0019000D File Offset: 0x0018E40D
		// (set) Token: 0x06004B52 RID: 19282 RVA: 0x00190015 File Offset: 0x0018E415
		public float fogAreaRadius
		{
			get
			{
				return this._fogAreaRadius;
			}
			set
			{
				if (value != this._fogAreaRadius)
				{
					this._fogAreaRadius = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06004B53 RID: 19283 RVA: 0x00190037 File Offset: 0x0018E437
		// (set) Token: 0x06004B54 RID: 19284 RVA: 0x00190040 File Offset: 0x0018E440
		public Vector3 fogAreaPosition
		{
			get
			{
				return this._fogAreaPosition;
			}
			set
			{
				if (value != this._fogAreaPosition)
				{
					this._fogAreaPosition = value;
					if (this._fogAreaCenter == null || this._fogAreaFollowMode == FOG_AREA_FOLLOW_MODE.RestrictToXZPlane)
					{
						this._baselineHeight = this._fogAreaPosition.y;
					}
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x06004B55 RID: 19285 RVA: 0x001900A0 File Offset: 0x0018E4A0
		// (set) Token: 0x06004B56 RID: 19286 RVA: 0x001900A8 File Offset: 0x0018E4A8
		public float fogAreaDepth
		{
			get
			{
				return this._fogAreaDepth;
			}
			set
			{
				if (value != this._fogAreaDepth)
				{
					this._fogAreaDepth = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x06004B57 RID: 19287 RVA: 0x001900CA File Offset: 0x0018E4CA
		// (set) Token: 0x06004B58 RID: 19288 RVA: 0x001900D2 File Offset: 0x0018E4D2
		public float fogAreaHeight
		{
			get
			{
				return this._fogAreaHeight;
			}
			set
			{
				if (value != this._fogAreaHeight)
				{
					this._fogAreaHeight = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x06004B59 RID: 19289 RVA: 0x001900F4 File Offset: 0x0018E4F4
		// (set) Token: 0x06004B5A RID: 19290 RVA: 0x001900FC File Offset: 0x0018E4FC
		public FOG_AREA_SORTING_MODE fogAreaSortingMode
		{
			get
			{
				return this._fogAreaSortingMode;
			}
			set
			{
				if (value != this._fogAreaSortingMode)
				{
					this._fogAreaSortingMode = value;
					this.lastTimeSortInstances = 0f;
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x06004B5B RID: 19291 RVA: 0x00190123 File Offset: 0x0018E523
		// (set) Token: 0x06004B5C RID: 19292 RVA: 0x0019012B File Offset: 0x0018E52B
		public int fogAreaRenderOrder
		{
			get
			{
				return this._fogAreaRenderOrder;
			}
			set
			{
				if (value != this._fogAreaRenderOrder)
				{
					this._fogAreaRenderOrder = value;
					this.lastTimeSortInstances = 0f;
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x06004B5D RID: 19293 RVA: 0x00190152 File Offset: 0x0018E552
		// (set) Token: 0x06004B5E RID: 19294 RVA: 0x0019015A File Offset: 0x0018E55A
		public bool pointLightTrackAuto
		{
			get
			{
				return this._pointLightTrackingAuto;
			}
			set
			{
				if (value != this._pointLightTrackingAuto)
				{
					this._pointLightTrackingAuto = value;
					this.TrackPointLights();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06004B5F RID: 19295 RVA: 0x0019017C File Offset: 0x0018E57C
		// (set) Token: 0x06004B60 RID: 19296 RVA: 0x00190184 File Offset: 0x0018E584
		public int pointLightTrackingCount
		{
			get
			{
				return this._pointLightTrackingCount;
			}
			set
			{
				if (value != this._pointLightTrackingCount)
				{
					this._pointLightTrackingCount = Mathf.Clamp(value, 0, 6);
					this.TrackPointLights();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x06004B61 RID: 19297 RVA: 0x001901AD File Offset: 0x0018E5AD
		// (set) Token: 0x06004B62 RID: 19298 RVA: 0x001901B5 File Offset: 0x0018E5B5
		public float pointLightTrackingCheckInterval
		{
			get
			{
				return this._pointLightTrackingCheckInterval;
			}
			set
			{
				if (value != this._pointLightTrackingCheckInterval)
				{
					this._pointLightTrackingCheckInterval = value;
					this.TrackPointLights();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06004B63 RID: 19299 RVA: 0x001901D7 File Offset: 0x0018E5D7
		// (set) Token: 0x06004B64 RID: 19300 RVA: 0x001901DF File Offset: 0x0018E5DF
		public int downsampling
		{
			get
			{
				return this._downsampling;
			}
			set
			{
				if (value != this._downsampling)
				{
					this._preset = FOG_PRESET.Custom;
					this._downsampling = value;
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06004B65 RID: 19301 RVA: 0x00190206 File Offset: 0x0018E606
		// (set) Token: 0x06004B66 RID: 19302 RVA: 0x0019020E File Offset: 0x0018E60E
		public bool edgeImprove
		{
			get
			{
				return this._edgeImprove;
			}
			set
			{
				if (value != this._edgeImprove)
				{
					this._preset = FOG_PRESET.Custom;
					this._edgeImprove = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06004B67 RID: 19303 RVA: 0x0019023B File Offset: 0x0018E63B
		// (set) Token: 0x06004B68 RID: 19304 RVA: 0x00190243 File Offset: 0x0018E643
		public float edgeThreshold
		{
			get
			{
				return this._edgeThreshold;
			}
			set
			{
				if (value != this._edgeThreshold)
				{
					this._preset = FOG_PRESET.Custom;
					this._edgeThreshold = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06004B69 RID: 19305 RVA: 0x00190270 File Offset: 0x0018E670
		// (set) Token: 0x06004B6A RID: 19306 RVA: 0x00190278 File Offset: 0x0018E678
		public float stepping
		{
			get
			{
				return this._stepping;
			}
			set
			{
				if (value != this._stepping)
				{
					this._preset = FOG_PRESET.Custom;
					this._stepping = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06004B6B RID: 19307 RVA: 0x001902A5 File Offset: 0x0018E6A5
		// (set) Token: 0x06004B6C RID: 19308 RVA: 0x001902AD File Offset: 0x0018E6AD
		public float steppingNear
		{
			get
			{
				return this._steppingNear;
			}
			set
			{
				if (value != this._steppingNear)
				{
					this._preset = FOG_PRESET.Custom;
					this._steppingNear = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x06004B6D RID: 19309 RVA: 0x001902DA File Offset: 0x0018E6DA
		// (set) Token: 0x06004B6E RID: 19310 RVA: 0x001902E2 File Offset: 0x0018E6E2
		public bool dithering
		{
			get
			{
				return this._dithering;
			}
			set
			{
				if (value != this._dithering)
				{
					this._dithering = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x06004B6F RID: 19311 RVA: 0x00190304 File Offset: 0x0018E704
		// (set) Token: 0x06004B70 RID: 19312 RVA: 0x0019030C File Offset: 0x0018E70C
		public float ditherStrength
		{
			get
			{
				return this._ditherStrength;
			}
			set
			{
				if (value != this._ditherStrength)
				{
					this._ditherStrength = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x06004B71 RID: 19313 RVA: 0x0019032E File Offset: 0x0018E72E
		// (set) Token: 0x06004B72 RID: 19314 RVA: 0x00190336 File Offset: 0x0018E736
		public bool lightScatteringEnabled
		{
			get
			{
				return this._lightScatteringEnabled;
			}
			set
			{
				if (value != this._lightScatteringEnabled)
				{
					this._lightScatteringEnabled = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06004B73 RID: 19315 RVA: 0x00190358 File Offset: 0x0018E758
		// (set) Token: 0x06004B74 RID: 19316 RVA: 0x00190360 File Offset: 0x0018E760
		public float lightScatteringDiffusion
		{
			get
			{
				return this._lightScatteringDiffusion;
			}
			set
			{
				if (value != this._lightScatteringDiffusion)
				{
					this._lightScatteringDiffusion = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x06004B75 RID: 19317 RVA: 0x00190382 File Offset: 0x0018E782
		// (set) Token: 0x06004B76 RID: 19318 RVA: 0x0019038A File Offset: 0x0018E78A
		public float lightScatteringSpread
		{
			get
			{
				return this._lightScatteringSpread;
			}
			set
			{
				if (value != this._lightScatteringSpread)
				{
					this._lightScatteringSpread = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06004B77 RID: 19319 RVA: 0x001903AC File Offset: 0x0018E7AC
		// (set) Token: 0x06004B78 RID: 19320 RVA: 0x001903B4 File Offset: 0x0018E7B4
		public int lightScatteringSamples
		{
			get
			{
				return this._lightScatteringSamples;
			}
			set
			{
				if (value != this._lightScatteringSamples)
				{
					this._lightScatteringSamples = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06004B79 RID: 19321 RVA: 0x001903D6 File Offset: 0x0018E7D6
		// (set) Token: 0x06004B7A RID: 19322 RVA: 0x001903DE File Offset: 0x0018E7DE
		public float lightScatteringWeight
		{
			get
			{
				return this._lightScatteringWeight;
			}
			set
			{
				if (value != this._lightScatteringWeight)
				{
					this._lightScatteringWeight = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06004B7B RID: 19323 RVA: 0x00190400 File Offset: 0x0018E800
		// (set) Token: 0x06004B7C RID: 19324 RVA: 0x00190408 File Offset: 0x0018E808
		public float lightScatteringIllumination
		{
			get
			{
				return this._lightScatteringIllumination;
			}
			set
			{
				if (value != this._lightScatteringIllumination)
				{
					this._lightScatteringIllumination = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06004B7D RID: 19325 RVA: 0x0019042A File Offset: 0x0018E82A
		// (set) Token: 0x06004B7E RID: 19326 RVA: 0x00190432 File Offset: 0x0018E832
		public float lightScatteringDecay
		{
			get
			{
				return this._lightScatteringDecay;
			}
			set
			{
				if (value != this._lightScatteringDecay)
				{
					this._lightScatteringDecay = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06004B7F RID: 19327 RVA: 0x00190454 File Offset: 0x0018E854
		// (set) Token: 0x06004B80 RID: 19328 RVA: 0x0019045C File Offset: 0x0018E85C
		public float lightScatteringExposure
		{
			get
			{
				return this._lightScatteringExposure;
			}
			set
			{
				if (value != this._lightScatteringExposure)
				{
					this._lightScatteringExposure = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06004B81 RID: 19329 RVA: 0x0019047E File Offset: 0x0018E87E
		// (set) Token: 0x06004B82 RID: 19330 RVA: 0x00190486 File Offset: 0x0018E886
		public float lightScatteringJittering
		{
			get
			{
				return this._lightScatteringJittering;
			}
			set
			{
				if (value != this._lightScatteringJittering)
				{
					this._lightScatteringJittering = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06004B83 RID: 19331 RVA: 0x001904A8 File Offset: 0x0018E8A8
		// (set) Token: 0x06004B84 RID: 19332 RVA: 0x001904B0 File Offset: 0x0018E8B0
		public bool fogBlur
		{
			get
			{
				return this._fogBlur;
			}
			set
			{
				if (value != this._fogBlur)
				{
					this._fogBlur = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06004B85 RID: 19333 RVA: 0x001904D2 File Offset: 0x0018E8D2
		// (set) Token: 0x06004B86 RID: 19334 RVA: 0x001904DA File Offset: 0x0018E8DA
		public float fogBlurDepth
		{
			get
			{
				return this._fogBlurDepth;
			}
			set
			{
				if (value != this._fogBlurDepth)
				{
					this._fogBlurDepth = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06004B87 RID: 19335 RVA: 0x001904FC File Offset: 0x0018E8FC
		// (set) Token: 0x06004B88 RID: 19336 RVA: 0x00190504 File Offset: 0x0018E904
		public bool sunShadows
		{
			get
			{
				return this._sunShadows;
			}
			set
			{
				if (value != this._sunShadows)
				{
					this._sunShadows = value;
					this.CleanUpTextureDepthSun();
					if (this._sunShadows)
					{
						this.needUpdateDepthSunTexture = true;
					}
					else
					{
						this.DestroySunShadowsDependencies();
					}
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x06004B89 RID: 19337 RVA: 0x00190554 File Offset: 0x0018E954
		// (set) Token: 0x06004B8A RID: 19338 RVA: 0x0019055C File Offset: 0x0018E95C
		public LayerMask sunShadowsLayerMask
		{
			get
			{
				return this._sunShadowsLayerMask;
			}
			set
			{
				if (this._sunShadowsLayerMask != value)
				{
					this._sunShadowsLayerMask = value;
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06004B8B RID: 19339 RVA: 0x00190582 File Offset: 0x0018E982
		// (set) Token: 0x06004B8C RID: 19340 RVA: 0x0019058A File Offset: 0x0018E98A
		public float sunShadowsStrength
		{
			get
			{
				return this._sunShadowsStrength;
			}
			set
			{
				if (value != this._sunShadowsStrength)
				{
					this._sunShadowsStrength = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06004B8D RID: 19341 RVA: 0x001905AC File Offset: 0x0018E9AC
		// (set) Token: 0x06004B8E RID: 19342 RVA: 0x001905B4 File Offset: 0x0018E9B4
		public float sunShadowsBias
		{
			get
			{
				return this._sunShadowsBias;
			}
			set
			{
				if (value != this._sunShadowsBias)
				{
					this._sunShadowsBias = value;
					this.needUpdateDepthSunTexture = true;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06004B8F RID: 19343 RVA: 0x001905DD File Offset: 0x0018E9DD
		// (set) Token: 0x06004B90 RID: 19344 RVA: 0x001905E5 File Offset: 0x0018E9E5
		public float sunShadowsJitterStrength
		{
			get
			{
				return this._sunShadowsJitterStrength;
			}
			set
			{
				if (value != this._sunShadowsJitterStrength)
				{
					this._sunShadowsJitterStrength = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06004B91 RID: 19345 RVA: 0x00190607 File Offset: 0x0018EA07
		// (set) Token: 0x06004B92 RID: 19346 RVA: 0x0019060F File Offset: 0x0018EA0F
		public int sunShadowsResolution
		{
			get
			{
				return this._sunShadowsResolution;
			}
			set
			{
				if (value != this._sunShadowsResolution)
				{
					this._sunShadowsResolution = value;
					this.needUpdateDepthSunTexture = true;
					this.CleanUpTextureDepthSun();
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06004B93 RID: 19347 RVA: 0x0019063E File Offset: 0x0018EA3E
		// (set) Token: 0x06004B94 RID: 19348 RVA: 0x00190646 File Offset: 0x0018EA46
		public float sunShadowsMaxDistance
		{
			get
			{
				return this._sunShadowsMaxDistance;
			}
			set
			{
				if (value != this._sunShadowsMaxDistance)
				{
					this._sunShadowsMaxDistance = value;
					this.needUpdateDepthSunTexture = true;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06004B95 RID: 19349 RVA: 0x0019066F File Offset: 0x0018EA6F
		// (set) Token: 0x06004B96 RID: 19350 RVA: 0x00190677 File Offset: 0x0018EA77
		public SUN_SHADOWS_BAKE_MODE sunShadowsBakeMode
		{
			get
			{
				return this._sunShadowsBakeMode;
			}
			set
			{
				if (value != this._sunShadowsBakeMode)
				{
					this._sunShadowsBakeMode = value;
					this.needUpdateDepthSunTexture = true;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06004B97 RID: 19351 RVA: 0x001906A0 File Offset: 0x0018EAA0
		// (set) Token: 0x06004B98 RID: 19352 RVA: 0x001906A8 File Offset: 0x0018EAA8
		public float sunShadowsRefreshInterval
		{
			get
			{
				return this._sunShadowsRefreshInterval;
			}
			set
			{
				if (value != this._sunShadowsRefreshInterval)
				{
					this._sunShadowsRefreshInterval = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x06004B99 RID: 19353 RVA: 0x001906CA File Offset: 0x0018EACA
		// (set) Token: 0x06004B9A RID: 19354 RVA: 0x001906D2 File Offset: 0x0018EAD2
		public float sunShadowsCancellation
		{
			get
			{
				return this._sunShadowsCancellation;
			}
			set
			{
				if (value != this._sunShadowsCancellation)
				{
					this._sunShadowsCancellation = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06004B9B RID: 19355 RVA: 0x001906F4 File Offset: 0x0018EAF4
		// (set) Token: 0x06004B9C RID: 19356 RVA: 0x001906FC File Offset: 0x0018EAFC
		public float turbulenceStrength
		{
			get
			{
				return this._turbulenceStrength;
			}
			set
			{
				if (value != this._turbulenceStrength)
				{
					this._turbulenceStrength = value;
					if (this._turbulenceStrength <= 0f)
					{
						this.UpdateTexture();
					}
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06004B9D RID: 19357 RVA: 0x00190734 File Offset: 0x0018EB34
		// (set) Token: 0x06004B9E RID: 19358 RVA: 0x0019073C File Offset: 0x0018EB3C
		public bool useXYPlane
		{
			get
			{
				return this._useXYPlane;
			}
			set
			{
				if (value != this._useXYPlane)
				{
					this._useXYPlane = value;
					if (this._sunShadows)
					{
						this.needUpdateDepthSunTexture = true;
					}
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06004B9F RID: 19359 RVA: 0x00190770 File Offset: 0x0018EB70
		// (set) Token: 0x06004BA0 RID: 19360 RVA: 0x00190778 File Offset: 0x0018EB78
		public bool useSinglePassStereoRenderingMatrix
		{
			get
			{
				return this._useSinglePassStereoRenderingMatrix;
			}
			set
			{
				if (value != this._useSinglePassStereoRenderingMatrix)
				{
					this._useSinglePassStereoRenderingMatrix = value;
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06004BA1 RID: 19361 RVA: 0x00190794 File Offset: 0x0018EB94
		// (set) Token: 0x06004BA2 RID: 19362 RVA: 0x0019079C File Offset: 0x0018EB9C
		public SPSR_BEHAVIOUR spsrBehaviour
		{
			get
			{
				return this._spsrBehaviour;
			}
			set
			{
				if (value != this._spsrBehaviour)
				{
					this._spsrBehaviour = value;
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06004BA3 RID: 19363 RVA: 0x001907B8 File Offset: 0x0018EBB8
		public Camera fogCamera
		{
			get
			{
				return this.mainCamera;
			}
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06004BA4 RID: 19364 RVA: 0x001907C0 File Offset: 0x0018EBC0
		public int renderingInstancesCount
		{
			get
			{
				return this._renderingInstancesCount;
			}
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x06004BA5 RID: 19365 RVA: 0x001907C8 File Offset: 0x0018EBC8
		public bool hasCamera
		{
			get
			{
				return this._hasCamera;
			}
		}

		// Token: 0x06004BA6 RID: 19366 RVA: 0x001907D0 File Offset: 0x0018EBD0
		private void OnEnable()
		{
			this.isPartOfScene = (this.isPartOfScene || this.IsPartOfScene());
			if (!this.isPartOfScene)
			{
				return;
			}
			if (this._fogVoidInverted)
			{
				this._fogVoidInverted = false;
				this._fogAreaCenter = this._character;
				this._fogAreaDepth = this._fogVoidDepth;
				this._fogAreaFallOff = this._fogVoidFallOff;
				this._fogAreaHeight = this._fogVoidHeight;
				this._fogAreaPosition = this._fogVoidPosition;
				this._fogAreaRadius = this._fogVoidRadius;
				this._fogVoidRadius = 0f;
				this._character = null;
			}
			this.mainCamera = base.gameObject.GetComponent<Camera>();
			this._hasCamera = (this.mainCamera != null);
			if (this._hasCamera)
			{
				this.fogRenderer = this;
				if (this.mainCamera.depthTextureMode == DepthTextureMode.None)
				{
					this.mainCamera.depthTextureMode = DepthTextureMode.Depth;
				}
			}
			else if (this.fogRenderer == null)
			{
				this.mainCamera = Camera.main;
				if (this.mainCamera == null)
				{
					return;
				}
				this.fogRenderer = this.mainCamera.GetComponent<VolumetricFog>();
				if (this.fogRenderer == null)
				{
					this.fogRenderer = this.mainCamera.gameObject.AddComponent<VolumetricFog>();
					this.fogRenderer.density = 0f;
				}
			}
			else
			{
				this.mainCamera = this.fogRenderer.mainCamera;
				if (this.mainCamera == null)
				{
					this.mainCamera = this.fogRenderer.GetComponent<Camera>();
				}
			}
			if (this._pointLights.Length < 6)
			{
				this._pointLights = new GameObject[6];
			}
			if (this._pointLightColors.Length < 6)
			{
				this._pointLightColors = new Color[6];
			}
			if (this._pointLightIntensities.Length < 6)
			{
				this._pointLightIntensities = new float[6];
			}
			if (this._pointLightIntensitiesMultiplier.Length < 6)
			{
				this._pointLightIntensitiesMultiplier = new float[6];
			}
			if (this._pointLightPositions.Length < 6)
			{
				this._pointLightPositions = new Vector3[6];
			}
			if (this._pointLightRanges.Length < 6)
			{
				this._pointLightRanges = new float[6];
			}
			if (this.fogMat == null)
			{
				this.InitFogMaterial();
				if (this._profile != null)
				{
					this._profile.Load(this);
				}
			}
			else
			{
				this.UpdateMaterialPropertiesNow();
			}
			this.RegisterWithRenderer();
		}

		// Token: 0x06004BA7 RID: 19367 RVA: 0x00190A54 File Offset: 0x0018EE54
		private void OnDestroy()
		{
			if (!this._hasCamera && this.fogRenderer != null)
			{
				this.fogRenderer.UnregisterFogArea(this);
			}
			else
			{
				this.UnregisterFogArea(this);
			}
			if (this.depthCamObj != null)
			{
				UnityEngine.Object.DestroyImmediate(this.depthCamObj);
				this.depthCamObj = null;
			}
			if (this.adjustedTexture != null)
			{
				UnityEngine.Object.DestroyImmediate(this.adjustedTexture);
				this.adjustedTexture = null;
			}
			if (this.chaosLerpMat != null)
			{
				UnityEngine.Object.DestroyImmediate(this.chaosLerpMat);
				this.chaosLerpMat = null;
			}
			if (this.adjustedChaosTexture != null)
			{
				UnityEngine.Object.DestroyImmediate(this.adjustedChaosTexture);
				this.adjustedChaosTexture = null;
			}
			if (this.blurMat != null)
			{
				UnityEngine.Object.DestroyImmediate(this.blurMat);
				this.blurMat = null;
			}
			if (this.fogMat != null)
			{
				UnityEngine.Object.DestroyImmediate(this.fogMat);
				this.fogMat = null;
			}
			this.CleanUpDepthTexture();
			this.DestroySunShadowsDependencies();
		}

		// Token: 0x06004BA8 RID: 19368 RVA: 0x00190B73 File Offset: 0x0018EF73
		public void DestroySelf()
		{
			this.DestroyRenderComponent<VolumetricFogPreT>();
			this.DestroyRenderComponent<VolumetricFogPosT>();
			UnityEngine.Object.DestroyImmediate(this);
		}

		// Token: 0x06004BA9 RID: 19369 RVA: 0x00190B87 File Offset: 0x0018EF87
		private void Start()
		{
			this.currentFogAlpha = this._alpha;
			this.currentSkyHazeAlpha = this._skyAlpha;
			this.lastTextureUpdate = Time.time + 0.2f;
			this.RegisterWithRenderer();
			this.Update();
		}

		// Token: 0x06004BAA RID: 19370 RVA: 0x00190BC0 File Offset: 0x0018EFC0
		private void Update()
		{
			if (!this.isPartOfScene)
			{
				return;
			}
			if (this.fogRenderer.sun != null)
			{
				Vector3 forward = this.fogRenderer.sun.transform.forward;
				bool flag = !Application.isPlaying || (this.updatingTextureSlice < 0 && Time.time - this.lastTextureUpdate >= 0.2f);
				if (flag)
				{
					if (forward != this._lightDirection)
					{
						this._lightDirection = forward;
						this.needUpdateTexture = true;
						this.needUpdateDepthSunTexture = true;
					}
					if (this.sunLight != null)
					{
						if (this._sunCopyColor && this.sunLight.color != this._lightColor)
						{
							this._lightColor = this.sunLight.color;
							this.needUpdateTexture = true;
						}
						if (this.sunLightIntensity != this.sunLight.intensity)
						{
							this.sunLightIntensity = this.sunLight.intensity;
							this.needUpdateTexture = true;
						}
					}
				}
			}
			if (!this.needUpdateTexture)
			{
				if (this._lightingModel == LIGHTING_MODEL.Classic)
				{
					if (this.lastRenderSettingsAmbientIntensity != RenderSettings.ambientIntensity)
					{
						this.needUpdateTexture = true;
					}
					else if (this.lastRenderSettingsAmbientLight != RenderSettings.ambientLight)
					{
						this.needUpdateTexture = true;
					}
				}
				else if (this._lightingModel == LIGHTING_MODEL.Natural && this.lastRenderSettingsAmbientLight != RenderSettings.ambientLight)
				{
					this.needUpdateTexture = true;
				}
			}
			if (this.transitionProfile)
			{
				float num = (Time.time - this.transitionStartTime) / this.transitionDuration;
				if (num > 1f)
				{
					num = 1f;
				}
				VolumetricFogProfile.Lerp(this.initialProfile, this.targetProfile, num, this);
				if (num >= 1f)
				{
					this.transitionProfile = false;
				}
			}
			if (this.transitionAlpha)
			{
				if (this.targetFogAlpha >= 0f || this.targetSkyHazeAlpha >= 0f)
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
							this.transitionAlpha = false;
						}
						this.fogMat.SetFloat("_FogAlpha", this.currentFogAlpha);
						this.UpdateSkyColor(this.currentSkyHazeAlpha);
					}
				}
				else if (this.currentFogAlpha != this._alpha || this.currentSkyHazeAlpha != this._skyAlpha)
				{
					if (this.transitionDuration > 0f)
					{
						this.currentFogAlpha = Mathf.Lerp(this.initialFogAlpha, this._alpha, (Time.time - this.transitionStartTime) / this.transitionDuration);
						this.currentSkyHazeAlpha = Mathf.Lerp(this.initialSkyHazeAlpha, this.alpha, (Time.time - this.transitionStartTime) / this.transitionDuration);
					}
					else
					{
						this.currentFogAlpha = this._alpha;
						this.currentSkyHazeAlpha = this._skyAlpha;
						this.transitionAlpha = false;
					}
					this.fogMat.SetFloat("_FogAlpha", this.currentFogAlpha);
					this.UpdateSkyColor(this.currentSkyHazeAlpha);
				}
			}
			if (this.transitionColor)
			{
				if (this.targetColorActive)
				{
					if (this.targetFogColor != this.currentFogColor)
					{
						if (this.transitionDuration > 0f)
						{
							this.currentFogColor = Color.Lerp(this.initialFogColor, this.targetFogColor, (Time.time - this.transitionStartTime) / this.transitionDuration);
						}
						else
						{
							this.currentFogColor = this.targetFogColor;
							this.transitionColor = false;
						}
					}
				}
				else if (this.currentFogColor != this._color)
				{
					if (this.transitionDuration > 0f)
					{
						this.currentFogColor = Color.Lerp(this.initialFogColor, this._color, (Time.time - this.transitionStartTime) / this.transitionDuration);
					}
					else
					{
						this.currentFogColor = this._color;
						this.transitionColor = false;
					}
				}
				this.UpdateMaterialFogColor();
			}
			if (this.transitionSpecularColor)
			{
				if (this.targetSpecularColorActive)
				{
					if (this.targetFogSpecularColor != this.currentFogSpecularColor)
					{
						if (this.transitionDuration > 0f)
						{
							this.currentFogSpecularColor = Color.Lerp(this.initialFogSpecularColor, this.targetFogSpecularColor, (Time.time - this.transitionStartTime) / this.transitionDuration);
						}
						else
						{
							this.currentFogSpecularColor = this.targetFogSpecularColor;
							this.transitionSpecularColor = false;
						}
						this.needUpdateTexture = true;
					}
				}
				else if (this.currentFogSpecularColor != this._specularColor)
				{
					if (this.transitionDuration > 0f)
					{
						this.currentFogSpecularColor = Color.Lerp(this.initialFogSpecularColor, this._specularColor, (Time.time - this.transitionStartTime) / this.transitionDuration);
					}
					else
					{
						this.currentFogSpecularColor = this._specularColor;
						this.transitionSpecularColor = false;
					}
					this.needUpdateTexture = true;
				}
			}
			if (this.transitionLightColor)
			{
				if (this.targetLightColorActive)
				{
					if (this.targetLightColor != this.currentLightColor)
					{
						if (this.transitionDuration > 0f)
						{
							this.currentLightColor = Color.Lerp(this.initialLightColor, this.targetLightColor, (Time.time - this.transitionStartTime) / this.transitionDuration);
						}
						else
						{
							this.currentLightColor = this.targetLightColor;
							this.transitionLightColor = false;
						}
						this.needUpdateTexture = true;
					}
				}
				else if (this.currentLightColor != this._lightColor)
				{
					if (this.transitionDuration > 0f)
					{
						this.currentLightColor = Color.Lerp(this.initialLightColor, this._lightColor, (Time.time - this.transitionStartTime) / this.transitionDuration);
					}
					else
					{
						this.currentLightColor = this._lightColor;
						this.transitionLightColor = false;
					}
					this.needUpdateTexture = true;
				}
			}
			if (this._baselineRelativeToCamera)
			{
				this.UpdateMaterialHeights();
			}
			else if (this._character != null)
			{
				this._fogVoidPosition = this._character.transform.position;
				this.UpdateMaterialHeights();
			}
			if (this._fogAreaCenter != null)
			{
				if (this._fogAreaFollowMode == FOG_AREA_FOLLOW_MODE.FullXYZ)
				{
					this._fogAreaPosition = this._fogAreaCenter.transform.position;
				}
				else
				{
					this._fogAreaPosition.x = this._fogAreaCenter.transform.position.x;
					this._fogAreaPosition.z = this._fogAreaCenter.transform.position.z;
				}
				this.UpdateMaterialHeights();
			}
			if (this._pointLightTrackingAuto && (!Application.isPlaying || Time.time - this.trackPointAutoLastTime > this._pointLightTrackingCheckInterval))
			{
				this.trackPointAutoLastTime = Time.time;
				this.TrackPointLights();
			}
			if (this.updatingTextureSlice >= 0)
			{
				this.UpdateTextureColors(this.adjustedColors, false);
			}
			else if (this.needUpdateTexture)
			{
				this.UpdateTexture();
			}
			if (this._hasCamera)
			{
				if (this._fogOfWarEnabled)
				{
					this.FogOfWarUpdate();
				}
				if (this._sunShadows && this.fogRenderer.sun)
				{
					this.CastSunShadows();
				}
				int count = this.fogInstances.Count;
				if (count > 1)
				{
					Vector3 position = this.mainCamera.transform.position;
					bool flag2 = !Application.isPlaying || Time.time - this.lastTimeSortInstances >= 2f;
					if (!flag2)
					{
						float num2 = (position.x - this.lastCamPos.x) * (position.x - this.lastCamPos.x) + (position.y - this.lastCamPos.y) * (position.y - this.lastCamPos.y) + (position.z - this.lastCamPos.z) * (position.z - this.lastCamPos.z);
						if (num2 > 625f)
						{
							this.lastCamPos = position;
							flag2 = true;
						}
					}
					if (flag2)
					{
						this.lastTimeSortInstances = Time.time;
						float x2 = position.x;
						float y2 = position.y;
						float z = position.z;
						for (int i = 0; i < count; i++)
						{
							VolumetricFog volumetricFog = this.fogInstances[i];
							if (volumetricFog != null)
							{
								Vector3 position2 = volumetricFog.transform.position;
								position2.y = volumetricFog.currentFogAltitude;
								float num3 = x2 - position2.x;
								float num4 = y2 - position2.y;
								float num5 = num4 * num4;
								float num6 = y2 - (position2.y + volumetricFog.height);
								float num7 = num6 * num6;
								volumetricFog.distanceToCameraYAxis = ((num5 >= num7) ? num7 : num5);
								float num8 = z - position2.z;
								float num9 = num3 * num3 + num4 * num4 + num8 * num8;
								volumetricFog.distanceToCamera = num9;
								Vector3 position3 = position2 - volumetricFog.transform.localScale * 0.5f;
								Vector3 position4 = position2 + volumetricFog.transform.localScale * 0.5f;
								volumetricFog.distanceToCameraMin = this.mainCamera.WorldToScreenPoint(position3).z;
								volumetricFog.distanceToCameraMax = this.mainCamera.WorldToScreenPoint(position4).z;
							}
						}
						this.fogInstances.Sort(delegate(VolumetricFog x, VolumetricFog y)
						{
							if (!x || !y)
							{
								return 0;
							}
							if (x.fogAreaSortingMode == FOG_AREA_SORTING_MODE.Fixed || y.fogAreaSortingMode == FOG_AREA_SORTING_MODE.Fixed)
							{
								if (x.fogAreaRenderOrder < y.fogAreaRenderOrder)
								{
									return -1;
								}
								if (x.fogAreaRenderOrder > y.fogAreaRenderOrder)
								{
									return 1;
								}
								return 0;
							}
							else if ((x.distanceToCameraMin < y.distanceToCameraMin && x.distanceToCameraMax > y.distanceToCameraMax) || (y.distanceToCameraMin < x.distanceToCameraMin && y.distanceToCameraMax > x.distanceToCameraMax) || x.fogAreaSortingMode == FOG_AREA_SORTING_MODE.Altitude || y.fogAreaSortingMode == FOG_AREA_SORTING_MODE.Altitude)
							{
								if (x.distanceToCameraYAxis < y.distanceToCameraYAxis)
								{
									return 1;
								}
								if (x.distanceToCameraYAxis > y.distanceToCameraYAxis)
								{
									return -1;
								}
								return 0;
							}
							else
							{
								if (x.distanceToCamera < y.distanceToCamera)
								{
									return 1;
								}
								if (x.distanceToCamera > y.distanceToCamera)
								{
									return -1;
								}
								return 0;
							}
						});
					}
				}
			}
		}

		// Token: 0x06004BAB RID: 19371 RVA: 0x0019164C File Offset: 0x0018FA4C
		private void OnPreCull()
		{
			if (!base.enabled || !base.gameObject.activeSelf || this.fogMat == null || !this._hasCamera || this.mainCamera == null)
			{
				return;
			}
			if (this.mainCamera.depthTextureMode == DepthTextureMode.None)
			{
				this.mainCamera.depthTextureMode = DepthTextureMode.Depth;
			}
			if (this._computeDepth)
			{
				this.GetTransparentDepth();
			}
		}

		// Token: 0x06004BAC RID: 19372 RVA: 0x001916D0 File Offset: 0x0018FAD0
		private bool IsPartOfScene()
		{
			VolumetricFog[] array = UnityEngine.Object.FindObjectsOfType<VolumetricFog>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == this)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004BAD RID: 19373 RVA: 0x00191708 File Offset: 0x0018FB08
		private void InitFogMaterial()
		{
			this.targetFogAlpha = -1f;
			this.targetSkyHazeAlpha = -1f;
			this._skyColor.a = this._skyAlpha;
			this.updatingTextureSlice = -1;
			this.fogMat = new Material(Shader.Find("VolumetricFogAndMist/VolumetricFog"));
			this.fogMat.hideFlags = HideFlags.DontSave;
			Texture2D texture2D = Resources.Load<Texture2D>("Textures/Noise3");
			this.noiseColors = texture2D.GetPixels();
			this.adjustedColors = new Color[this.noiseColors.Length];
			this.adjustedTexture = new Texture2D(texture2D.width, texture2D.height, TextureFormat.RGBA32, false);
			this.adjustedTexture.hideFlags = HideFlags.DontSave;
			this.UpdateTextureAlpha();
			this.UpdateSun();
			if (this._pointLightTrackingAuto)
			{
				this.TrackPointLights();
			}
			else
			{
				this.UpdatePointLights();
			}
			this.FogOfWarInit();
			if (this.fogOfWarTexture == null)
			{
				this.FogOfWarUpdateTexture();
			}
			this.CopyTransitionValues();
			this.UpdatePreset();
			this.oldBaselineRelativeCameraY = this.mainCamera.transform.position.y;
			if (this._sunShadows)
			{
				this.needUpdateDepthSunTexture = true;
			}
		}

		// Token: 0x06004BAE RID: 19374 RVA: 0x00191838 File Offset: 0x0018FC38
		private void UpdateRenderComponents()
		{
			if (!this._hasCamera)
			{
				return;
			}
			if (this._renderBeforeTransparent)
			{
				this.AssignRenderComponent<VolumetricFogPreT>();
				this.DestroyRenderComponent<VolumetricFogPosT>();
			}
			else if (this._transparencyBlendMode == TRANSPARENT_MODE.Blend)
			{
				this.AssignRenderComponent<VolumetricFogPreT>();
				this.AssignRenderComponent<VolumetricFogPosT>();
			}
			else
			{
				this.AssignRenderComponent<VolumetricFogPosT>();
				this.DestroyRenderComponent<VolumetricFogPreT>();
			}
		}

		// Token: 0x06004BAF RID: 19375 RVA: 0x00191898 File Offset: 0x0018FC98
		private void DestroyRenderComponent<T>() where T : IVolumetricFogRenderComponent
		{
			T[] componentsInChildren = base.GetComponentsInChildren<T>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].fog == this || componentsInChildren[i].fog == null)
				{
					componentsInChildren[i].DestroySelf();
				}
			}
		}

		// Token: 0x06004BB0 RID: 19376 RVA: 0x00191914 File Offset: 0x0018FD14
		private void AssignRenderComponent<T>() where T : Component, IVolumetricFogRenderComponent
		{
			T[] componentsInChildren = base.GetComponentsInChildren<T>(true);
			int num = -1;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].fog == this)
				{
					return;
				}
				if (componentsInChildren[i].fog == null)
				{
					num = i;
				}
			}
			if (num < 0)
			{
				T t = base.gameObject.AddComponent<T>();
				t.fog = this;
			}
			else
			{
				componentsInChildren[num].fog = this;
			}
		}

		// Token: 0x06004BB1 RID: 19377 RVA: 0x001919BB File Offset: 0x0018FDBB
		private void RegisterFogArea(VolumetricFog fog)
		{
			if (this.fogInstances.Contains(fog))
			{
				return;
			}
			this.fogInstances.Add(fog);
		}

		// Token: 0x06004BB2 RID: 19378 RVA: 0x001919DB File Offset: 0x0018FDDB
		private void UnregisterFogArea(VolumetricFog fog)
		{
			if (!this.fogInstances.Contains(fog))
			{
				return;
			}
			this.fogInstances.Remove(fog);
		}

		// Token: 0x06004BB3 RID: 19379 RVA: 0x001919FC File Offset: 0x0018FDFC
		private void RegisterWithRenderer()
		{
			if (!this._hasCamera && this.fogRenderer != null)
			{
				this.fogRenderer.RegisterFogArea(this);
			}
			else
			{
				this.RegisterFogArea(this);
			}
			this.lastTimeSortInstances = 0f;
		}

		// Token: 0x06004BB4 RID: 19380 RVA: 0x00191A48 File Offset: 0x0018FE48
		internal void DoOnRenderImage(RenderTexture source, RenderTexture destination)
		{
			int count = this.fogInstances.Count;
			this.fogRenderInstances.Clear();
			for (int i = 0; i < count; i++)
			{
				if (this.fogInstances[i] != null && this.fogInstances[i].isActiveAndEnabled && this.fogInstances[i].density > 0f)
				{
					this.fogRenderInstances.Add(this.fogInstances[i]);
				}
			}
			this._renderingInstancesCount = this.fogRenderInstances.Count;
			if (this._renderingInstancesCount == 0 || this.mainCamera == null)
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this._hasCamera && this._density <= 0f && this.shouldUpdateMaterialProperties)
			{
				this.UpdateMaterialPropertiesNow();
			}
			if (this._renderingInstancesCount == 1)
			{
				this.fogRenderInstances[0].DoOnRenderImageInstance(source, destination);
			}
			else
			{
				RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);
				this.fogRenderInstances[0].DoOnRenderImageInstance(source, temporary);
				if (this._renderingInstancesCount == 2)
				{
					this.fogRenderInstances[1].DoOnRenderImageInstance(temporary, destination);
				}
				if (this._renderingInstancesCount >= 3)
				{
					RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);
					RenderTexture source2 = temporary;
					RenderTexture renderTexture = temporary2;
					int num = this._renderingInstancesCount - 1;
					for (int j = 1; j < num; j++)
					{
						if (j > 1)
						{
							renderTexture.DiscardContents();
						}
						this.fogRenderInstances[j].DoOnRenderImageInstance(source2, renderTexture);
						if (renderTexture == temporary2)
						{
							source2 = temporary2;
							renderTexture = temporary;
						}
						else
						{
							source2 = temporary;
							renderTexture = temporary2;
						}
					}
					this.fogRenderInstances[num].DoOnRenderImageInstance(source2, destination);
					RenderTexture.ReleaseTemporary(temporary2);
				}
				RenderTexture.ReleaseTemporary(temporary);
			}
		}

		// Token: 0x06004BB5 RID: 19381 RVA: 0x00191C54 File Offset: 0x00190054
		internal void DoOnRenderImageInstance(RenderTexture source, RenderTexture destination)
		{
			if (this.mainCamera == null || this.fogMat == null)
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (!this._hasCamera)
			{
				this.CheckFogAreaDimensions();
				if (this._sunShadows && !this.fogRenderer.sunShadows)
				{
					this.fogRenderer.sunShadows = true;
				}
			}
			if (this.shouldUpdateMaterialProperties)
			{
				this.UpdateMaterialPropertiesNow();
			}
			if (Application.isPlaying)
			{
				this.UpdateWindSpeedQuick();
			}
			if (this._hasCamera)
			{
				if (this._spsrBehaviour == SPSR_BEHAVIOUR.ForcedOn && !this._useSinglePassStereoRenderingMatrix)
				{
					this.useSinglePassStereoRenderingMatrix = true;
				}
				else if (this._spsrBehaviour == SPSR_BEHAVIOUR.ForcedOff && this._useSinglePassStereoRenderingMatrix)
				{
					this.useSinglePassStereoRenderingMatrix = false;
				}
			}
			if (this.fogRenderer.useSinglePassStereoRenderingMatrix && VRSettings.enabled)
			{
				this.fogMat.SetMatrix("_ClipToWorld", this.mainCamera.cameraToWorldMatrix);
			}
			else
			{
				this.fogMat.SetMatrix("_ClipToWorld", this.mainCamera.cameraToWorldMatrix * this.mainCamera.projectionMatrix.inverse);
			}
			if (this.mainCamera.orthographic)
			{
				this.fogMat.SetVector("_ClipDir", this.mainCamera.transform.forward);
			}
			if (this.fogRenderer.sun && this._lightScatteringEnabled)
			{
				this.UpdateScatteringData();
			}
			for (int i = 0; i < 6; i++)
			{
				Light light = this.pointLightComponents[i];
				if (light != null)
				{
					if (this._pointLightColors[i] != light.color)
					{
						this._pointLightColors[i] = light.color;
						this.isDirty = true;
					}
					if (this._pointLightRanges[i] != light.range)
					{
						this._pointLightRanges[i] = light.range;
						this.isDirty = true;
					}
					if (this._pointLightPositions[i] != light.transform.position)
					{
						this._pointLightPositions[i] = light.transform.position;
						this.isDirty = true;
					}
					if (this._pointLightIntensities[i] != light.intensity)
					{
						this._pointLightIntensities[i] = light.intensity;
						this.isDirty = true;
					}
				}
				if (this._pointLightRanges[i] * this._pointLightIntensities[i] > 0f)
				{
					this.SetMaterialLightData(i, light);
				}
			}
			if (Application.isPlaying && this._turbulenceStrength > 0f)
			{
				this.ApplyChaos();
			}
			if ((float)this._downsampling > 1f)
			{
				int scaledSize = this.GetScaledSize(source.width, (float)this._downsampling);
				int scaledSize2 = this.GetScaledSize(source.width, (float)this._downsampling);
				this.reducedDestination = RenderTexture.GetTemporary(scaledSize, scaledSize2, 0, RenderTextureFormat.ARGB32);
				RenderTextureFormat format = (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RFloat)) ? RenderTextureFormat.ARGBFloat : RenderTextureFormat.RFloat;
				RenderTexture temporary = RenderTexture.GetTemporary(scaledSize, scaledSize2, 0, format);
				if (this._fogBlur)
				{
					RenderTexture temporary2 = RenderTexture.GetTemporary(scaledSize, scaledSize2, 0, RenderTextureFormat.ARGB32);
					Graphics.Blit(source, temporary2);
					this.SetBlurTexture(temporary2);
					RenderTexture.ReleaseTemporary(temporary2);
				}
				if (!this._edgeImprove || VRSettings.enabled || SystemInfo.supportedRenderTargetCount < 2)
				{
					Graphics.Blit(source, this.reducedDestination, this.fogMat, 3);
					if (this._edgeImprove)
					{
						Graphics.Blit(source, temporary, this.fogMat, 4);
					}
				}
				else
				{
					this.fogMat.SetTexture("_MainTex", source);
					if (this.mrt == null)
					{
						this.mrt = new RenderBuffer[2];
					}
					this.mrt[0] = this.reducedDestination.colorBuffer;
					this.mrt[1] = temporary.colorBuffer;
					Graphics.SetRenderTarget(this.mrt, this.reducedDestination.depthBuffer);
					Graphics.Blit(null, this.fogMat, 1);
				}
				this.fogMat.SetTexture("_FogDownsampled", this.reducedDestination);
				this.fogMat.SetTexture("_DownsampledDepth", temporary);
				Graphics.Blit(source, destination, this.fogMat, 2);
				RenderTexture.ReleaseTemporary(temporary);
				RenderTexture.ReleaseTemporary(this.reducedDestination);
			}
			else
			{
				if (this._fogBlur)
				{
					RenderTexture temporary3 = RenderTexture.GetTemporary(256, 256, 0, RenderTextureFormat.ARGB32);
					Graphics.Blit(source, temporary3);
					this.SetBlurTexture(temporary3);
					RenderTexture.ReleaseTemporary(temporary3);
				}
				Graphics.Blit(source, destination, this.fogMat, 0);
			}
		}

		// Token: 0x06004BB6 RID: 19382 RVA: 0x00192137 File Offset: 0x00190537
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

		// Token: 0x06004BB7 RID: 19383 RVA: 0x00192152 File Offset: 0x00190552
		private void CleanUpDepthTexture()
		{
			if (this.depthTexture)
			{
				RenderTexture.ReleaseTemporary(this.depthTexture);
				this.depthTexture = null;
			}
		}

		// Token: 0x06004BB8 RID: 19384 RVA: 0x00192178 File Offset: 0x00190578
		private void GetTransparentDepth()
		{
			this.CleanUpDepthTexture();
			if (this.depthCam == null)
			{
				if (this.depthCamObj == null)
				{
					this.depthCamObj = GameObject.Find("VFMDepthCamera");
				}
				if (this.depthCamObj == null)
				{
					this.depthCamObj = new GameObject("VFMDepthCamera");
					this.depthCam = this.depthCamObj.AddComponent<Camera>();
					this.depthCam.enabled = false;
					this.depthCamObj.hideFlags = HideFlags.HideAndDontSave;
				}
				else
				{
					this.depthCam = this.depthCamObj.GetComponent<Camera>();
					if (this.depthCam == null)
					{
						UnityEngine.Object.DestroyImmediate(this.depthCamObj);
						this.depthCamObj = null;
						return;
					}
				}
			}
			this.depthCam.CopyFrom(this.mainCamera);
			this.depthCam.depthTextureMode = DepthTextureMode.None;
			this.depthTexture = RenderTexture.GetTemporary(this.mainCamera.pixelWidth, this.mainCamera.pixelHeight, 24, RenderTextureFormat.Depth, RenderTextureReadWrite.Linear);
			this.depthCam.backgroundColor = new Color(0f, 0f, 0f, 0f);
			this.depthCam.clearFlags = CameraClearFlags.Color;
			this.depthCam.cullingMask = this._transparencyLayerMask;
			this.depthCam.targetTexture = this.depthTexture;
			this.depthCam.renderingPath = RenderingPath.Forward;
			if (this.depthShader == null)
			{
				this.depthShader = Shader.Find("VolumetricFogAndMist/CopyDepth");
			}
			if (this._computeDepthScope == COMPUTE_DEPTH_SCOPE.OnlyTreeBillboards)
			{
				this.depthCam.RenderWithShader(this.depthShader, "RenderType");
			}
			else
			{
				this.depthCam.RenderWithShader(this.depthShader, null);
			}
			Shader.SetGlobalTexture("_VolumetricFogDepthTexture", this.depthTexture);
		}

		// Token: 0x06004BB9 RID: 19385 RVA: 0x00192350 File Offset: 0x00190750
		private void CastSunShadows()
		{
			if (!base.enabled || !base.gameObject.activeSelf || this.fogMat == null)
			{
				return;
			}
			if (this._sunShadowsBakeMode == SUN_SHADOWS_BAKE_MODE.Discrete && this._sunShadowsRefreshInterval > 0f && Time.time > this.lastShadowUpdateFrame + this._sunShadowsRefreshInterval)
			{
				this.needUpdateDepthSunTexture = true;
			}
			if (!Application.isPlaying || this.needUpdateDepthSunTexture || this.depthSunTexture == null || !this.depthSunTexture.IsCreated())
			{
				this.needUpdateDepthSunTexture = false;
				this.lastShadowUpdateFrame = Time.time;
				this.GetSunShadows();
			}
		}

		// Token: 0x06004BBA RID: 19386 RVA: 0x00192414 File Offset: 0x00190814
		private void GetSunShadows()
		{
			if (this._sun == null || !this._sunShadows)
			{
				return;
			}
			if (this.depthSunCam == null)
			{
				if (this.depthSunCamObj == null)
				{
					this.depthSunCamObj = GameObject.Find("VFMDepthSunCamera");
				}
				if (this.depthSunCamObj == null)
				{
					this.depthSunCamObj = new GameObject("VFMDepthSunCamera");
					this.depthSunCamObj.hideFlags = HideFlags.HideAndDontSave;
					this.depthSunCam = this.depthSunCamObj.AddComponent<Camera>();
				}
				else
				{
					this.depthSunCam = this.depthSunCamObj.GetComponent<Camera>();
					if (this.depthSunCam == null)
					{
						UnityEngine.Object.DestroyImmediate(this.depthSunCamObj);
						this.depthSunCamObj = null;
						return;
					}
				}
				if (this.depthSunShader == null)
				{
					this.depthSunShader = Shader.Find("VolumetricFogAndMist/CopySunDepth");
				}
				this.depthSunCam.SetReplacementShader(this.depthSunShader, "RenderType");
				this.depthSunCam.nearClipPlane = 1f;
				this.depthSunCam.renderingPath = RenderingPath.Forward;
				this.depthSunCam.orthographic = true;
				this.depthSunCam.aspect = 1f;
				this.depthSunCam.backgroundColor = new Color(0f, 0f, 0.5f, 0f);
				this.depthSunCam.clearFlags = CameraClearFlags.Color;
				this.depthSunCam.depthTextureMode = DepthTextureMode.None;
			}
			float orthographicSize = this._sunShadowsMaxDistance / 0.95f;
			this.depthSunCam.transform.position = this.mainCamera.transform.position - this._sun.transform.forward * 2000f;
			this.depthSunCam.transform.rotation = this._sun.transform.rotation;
			this.depthSunCam.farClipPlane = 4000f;
			this.depthSunCam.orthographicSize = orthographicSize;
			if (this.sunLight != null)
			{
				this.depthSunCam.cullingMask = this._sunShadowsLayerMask;
			}
			if (this.depthSunTexture == null)
			{
				int num = (int)Mathf.Pow(2f, (float)(this._sunShadowsResolution + 9));
				this.depthSunTexture = new RenderTexture(num, num, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
				this.depthSunTexture.hideFlags = HideFlags.DontSave;
				this.depthSunTexture.filterMode = FilterMode.Point;
				this.depthSunTexture.wrapMode = TextureWrapMode.Clamp;
				this.depthSunTexture.Create();
			}
			this.depthSunCam.targetTexture = this.depthSunTexture;
			Shader.SetGlobalFloat("_VF_ShadowBias", this._sunShadowsBias);
			if (Application.isPlaying && this._sunShadowsBakeMode == SUN_SHADOWS_BAKE_MODE.Realtime)
			{
				if (!this.depthSunCam.enabled)
				{
					this.depthSunCam.enabled = true;
				}
			}
			else
			{
				if (this.depthSunCam.enabled)
				{
					this.depthSunCam.enabled = false;
				}
				this.depthSunCam.Render();
			}
			Shader.SetGlobalMatrix("_VolumetricFogSunProj", this.depthSunCam.projectionMatrix * this.depthSunCam.worldToCameraMatrix);
			Shader.SetGlobalTexture("_VolumetricFogSunDepthTexture", this.depthSunTexture);
			Vector4 value = this.depthSunCam.transform.position;
			value.w = Mathf.Min(this._sunShadowsMaxDistance, this._maxFogLength);
			Shader.SetGlobalVector("_VolumetricFogSunWorldPos", value);
			this.UpdateSunShadowsData();
		}

		// Token: 0x06004BBB RID: 19387 RVA: 0x001927A0 File Offset: 0x00190BA0
		private void SetBlurTexture(RenderTexture source)
		{
			if (this.blurMat == null)
			{
				Shader shader = Shader.Find("VolumetricFogAndMist/Blur");
				this.blurMat = new Material(shader);
				this.blurMat.hideFlags = HideFlags.DontSave;
			}
			if (this.blurMat == null)
			{
				return;
			}
			this.blurMat.SetFloat("_BlurDepth", this._fogBlurDepth);
			RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
			Graphics.Blit(source, temporary, this.blurMat, 0);
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
			Graphics.Blit(temporary, temporary2, this.blurMat, 1);
			this.blurMat.SetFloat("_BlurDepth", this._fogBlurDepth * 2f);
			temporary.DiscardContents();
			Graphics.Blit(temporary2, temporary, this.blurMat, 0);
			temporary2.DiscardContents();
			Graphics.Blit(temporary, temporary2, this.blurMat, 1);
			this.fogMat.SetTexture("_BlurTex", temporary2);
			RenderTexture.ReleaseTemporary(temporary2);
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x06004BBC RID: 19388 RVA: 0x001928B9 File Offset: 0x00190CB9
		private void DestroySunShadowsDependencies()
		{
			if (this.depthSunCamObj != null)
			{
				UnityEngine.Object.DestroyImmediate(this.depthSunCamObj);
				this.depthSunCamObj = null;
			}
			this.CleanUpTextureDepthSun();
		}

		// Token: 0x06004BBD RID: 19389 RVA: 0x001928E4 File Offset: 0x00190CE4
		private void CleanUpTextureDepthSun()
		{
			if (this.depthSunTexture != null)
			{
				this.depthSunTexture.Release();
				this.depthSunTexture = null;
			}
		}

		// Token: 0x06004BBE RID: 19390 RVA: 0x00192909 File Offset: 0x00190D09
		public string GetCurrentPresetName()
		{
			return Enum.GetName(typeof(FOG_PRESET), this._preset);
		}

		// Token: 0x06004BBF RID: 19391 RVA: 0x00192928 File Offset: 0x00190D28
		private void UpdatePreset()
		{
			FOG_PRESET preset = this._preset;
			switch (preset)
			{
			case FOG_PRESET.SandStorm1:
				this._skySpeed = 0.35f;
				this._skyHaze = 388f;
				this._skyNoiseStrength = 0.847f;
				this._skyAlpha = 1f;
				this._density = 0.487f;
				this._noiseStrength = 0.758f;
				this._noiseScale = 1.71f;
				this._noiseSparse = 0f;
				this._distance = 0f;
				this._distanceFallOff = 0f;
				this._height = 16f;
				this._stepping = 6f;
				this._steppingNear = 0f;
				this._alpha = 1f;
				this._color = new Color(0.505f, 0.505f, 0.505f, 1f);
				this._skyColor = this._color;
				this._specularColor = new Color(1f, 1f, 0.8f, 1f);
				this._specularIntensity = 0f;
				this._specularThreshold = 0.6f;
				this._lightColor = Color.white;
				this._lightIntensity = 0f;
				this._speed = 0.3f;
				this._windDirection = Vector3.right;
				this._downsampling = 1;
				this._baselineRelativeToCamera = false;
				this.CheckWaterLevel(false);
				this._fogVoidRadius = 0f;
				this.CopyTransitionValues();
				break;
			case FOG_PRESET.Smoke:
				this._skySpeed = 0.109f;
				this._skyHaze = 10f;
				this._skyNoiseStrength = 0.119f;
				this._skyAlpha = 1f;
				this._density = 1f;
				this._noiseStrength = 0.767f;
				this._noiseScale = 1.6f;
				this._noiseSparse = 0f;
				this._distance = 0f;
				this._distanceFallOff = 0f;
				this._height = 8f;
				this._stepping = 12f;
				this._steppingNear = 25f;
				this._alpha = 1f;
				this._color = new Color(0.125f, 0.125f, 0.125f, 1f);
				this._skyColor = this._color;
				this._specularColor = new Color(1f, 1f, 1f, 1f);
				this._specularIntensity = 0.575f;
				this._specularThreshold = 0.6f;
				this._lightColor = Color.white;
				this._lightIntensity = 1f;
				this._speed = 0.075f;
				this._windDirection = Vector3.right;
				this._downsampling = 1;
				this._baselineRelativeToCamera = false;
				this.CheckWaterLevel(false);
				this._baselineHeight += 8f;
				this._fogVoidRadius = 0f;
				this.CopyTransitionValues();
				break;
			case FOG_PRESET.ToxicSwamp:
				this._skySpeed = 0.062f;
				this._skyHaze = 22f;
				this._skyNoiseStrength = 0.694f;
				this._skyAlpha = 1f;
				this._density = 1f;
				this._noiseStrength = 1f;
				this._noiseScale = 1f;
				this._noiseSparse = 0f;
				this._distance = 0f;
				this._distanceFallOff = 0f;
				this._height = 2.5f;
				this._stepping = 20f;
				this._steppingNear = 50f;
				this._alpha = 0.95f;
				this._color = new Color(0.0238f, 0.175f, 0.109f, 1f);
				this._skyColor = this._color;
				this._specularColor = new Color(0.593f, 0.625f, 0.207f, 1f);
				this._specularIntensity = 0.735f;
				this._specularThreshold = 0.6f;
				this._lightColor = new Color(0.73f, 0.746f, 0.511f, 1f);
				this._lightIntensity = 0.492f;
				this._speed = 0.0003f;
				this._windDirection = Vector3.right;
				this._downsampling = 1;
				this._baselineRelativeToCamera = false;
				this.CheckWaterLevel(false);
				this._fogVoidRadius = 0f;
				this.CopyTransitionValues();
				break;
			case FOG_PRESET.SandStorm2:
				this._skySpeed = 0f;
				this._skyHaze = 0f;
				this._skyNoiseStrength = 0.729f;
				this._skyAlpha = 0.55f;
				this._density = 0.545f;
				this._noiseStrength = 1f;
				this._noiseScale = 3f;
				this._noiseSparse = 0f;
				this._distance = 0f;
				this._distanceFallOff = 0f;
				this._height = 12f;
				this._stepping = 5f;
				this._steppingNear = 19.6f;
				this._alpha = 0.96f;
				this._color = new Color(0.609f, 0.609f, 0.609f, 1f);
				this._skyColor = this._color;
				this._specularColor = new Color(0.589f, 0.621f, 0.207f, 1f);
				this._specularIntensity = 0.505f;
				this._specularThreshold = 0.6f;
				this._lightColor = new Color(0.726f, 0.742f, 0.507f, 1f);
				this._lightIntensity = 0.581f;
				this._speed = 0.168f;
				this._windDirection = Vector3.right;
				this._downsampling = 1;
				this._baselineRelativeToCamera = false;
				this.CheckWaterLevel(false);
				this._fogVoidRadius = 0f;
				this.CopyTransitionValues();
				break;
			default:
				switch (preset)
				{
				case FOG_PRESET.GroundFog:
					this._skySpeed = 0.3f;
					this._skyHaze = 0f;
					this._skyNoiseStrength = 0.1f;
					this._skyAlpha = 0.85f;
					this._density = 0.6f;
					this._noiseStrength = 0.479f;
					this._noiseScale = 1.15f;
					this._noiseSparse = 0f;
					this._distance = 5f;
					this._distanceFallOff = 1f;
					this._height = 1.5f;
					this._stepping = 8f;
					this._steppingNear = 0f;
					this._alpha = 0.95f;
					this._color = new Color(0.89f, 0.89f, 0.89f, 1f);
					this._skyColor = this._color;
					this._specularColor = new Color(1f, 1f, 0.8f, 1f);
					this._specularIntensity = 0.2f;
					this._specularThreshold = 0.6f;
					this._lightColor = Color.white;
					this._lightIntensity = 0.2f;
					this._speed = 0.01f;
					this._downsampling = 1;
					this._baselineRelativeToCamera = false;
					this.CheckWaterLevel(false);
					this._fogVoidRadius = 0f;
					this.CopyTransitionValues();
					break;
				case FOG_PRESET.FrostedGround:
					this._skySpeed = 0f;
					this._skyHaze = 0f;
					this._skyNoiseStrength = 0.729f;
					this._skyAlpha = 0.55f;
					this._density = 1f;
					this._noiseStrength = 0.164f;
					this._noiseScale = 1.81f;
					this._noiseSparse = 0f;
					this._distance = 0f;
					this._distanceFallOff = 0f;
					this._height = 0.5f;
					this._stepping = 20f;
					this._steppingNear = 50f;
					this._alpha = 0.97f;
					this._color = new Color(0.546f, 0.648f, 0.71f, 1f);
					this._skyColor = this._color;
					this._specularColor = new Color(0.792f, 0.792f, 0.792f, 1f);
					this._specularIntensity = 1f;
					this._specularThreshold = 0.866f;
					this._lightColor = new Color(0.972f, 0.972f, 0.972f, 1f);
					this._lightIntensity = 0.743f;
					this._speed = 0f;
					this._downsampling = 1;
					this._baselineRelativeToCamera = false;
					this.CheckWaterLevel(false);
					this._fogVoidRadius = 0f;
					this.CopyTransitionValues();
					break;
				case FOG_PRESET.FoggyLake:
					this._skySpeed = 0.3f;
					this._skyHaze = 40f;
					this._skyNoiseStrength = 0.574f;
					this._skyAlpha = 0.827f;
					this._density = 1f;
					this._noiseStrength = 0.03f;
					this._noiseScale = 5.77f;
					this._noiseSparse = 0f;
					this._distance = 0f;
					this._distanceFallOff = 0f;
					this._height = 4f;
					this._stepping = 6f;
					this._steppingNear = 14.4f;
					this._alpha = 1f;
					this._color = new Color(0f, 0.96f, 1f, 1f);
					this._skyColor = this._color;
					this._specularColor = Color.white;
					this._lightColor = Color.white;
					this._specularIntensity = 0.861f;
					this._specularThreshold = 0.907f;
					this._lightIntensity = 0.126f;
					this._speed = 0f;
					this._downsampling = 1;
					this._baselineRelativeToCamera = false;
					this.CheckWaterLevel(false);
					this._fogVoidRadius = 0f;
					this.CopyTransitionValues();
					break;
				default:
					if (preset != FOG_PRESET.Mist)
					{
						if (preset != FOG_PRESET.WindyMist)
						{
							if (preset != FOG_PRESET.LowClouds)
							{
								if (preset != FOG_PRESET.SeaClouds)
								{
									if (preset != FOG_PRESET.Fog)
									{
										if (preset != FOG_PRESET.HeavyFog)
										{
											if (preset != FOG_PRESET.Clear)
											{
												if (preset == FOG_PRESET.WorldEdge)
												{
													this._skySpeed = 0.3f;
													this._skyHaze = 60f;
													this._skyNoiseStrength = 1f;
													this._skyAlpha = 0.96f;
													this._density = 1f;
													this._noiseStrength = 1f;
													this._noiseScale = 3f;
													this._noiseSparse = 0f;
													this._distance = 0f;
													this._distanceFallOff = 0f;
													this._height = 20f;
													this._stepping = 6f;
													this._alpha = 0.98f;
													this._color = new Color(0.89f, 0.89f, 0.89f, 1f);
													this._skyColor = this._color;
													this._specularColor = new Color(1f, 1f, 0.8f, 1f);
													this._specularIntensity = 0.259f;
													this._specularThreshold = 0.6f;
													this._lightColor = Color.white;
													this._lightIntensity = 0.15f;
													this._speed = 0.03f;
													this._downsampling = 2;
													this._baselineRelativeToCamera = false;
													this.CheckWaterLevel(false);
													Terrain activeTerrain = VolumetricFog.GetActiveTerrain();
													if (activeTerrain != null)
													{
														this._fogVoidPosition = activeTerrain.transform.position + activeTerrain.terrainData.size * 0.5f;
														this._fogVoidRadius = activeTerrain.terrainData.size.x * 0.45f;
														this._fogVoidHeight = activeTerrain.terrainData.size.y;
														this._fogVoidDepth = activeTerrain.terrainData.size.z * 0.45f;
														this._fogVoidFallOff = 6f;
														this._fogAreaRadius = 0f;
														this._character = null;
														this._fogAreaCenter = null;
														float x = activeTerrain.terrainData.size.x;
														if (this.mainCamera.farClipPlane < x)
														{
															this.mainCamera.farClipPlane = x;
														}
														if (this._maxFogLength < x * 0.6f)
														{
															this._maxFogLength = x * 0.6f;
														}
													}
													this.CopyTransitionValues();
												}
											}
											else
											{
												this._density = 0f;
												this._fogOfWarEnabled = false;
												this._fogVoidRadius = 0f;
											}
										}
										else
										{
											this._skySpeed = 0.05f;
											this._skyHaze = 500f;
											this._skyNoiseStrength = 0.96f;
											this._skyAlpha = 1f;
											this._density = 0.35f;
											this._noiseStrength = 0.1f;
											this._noiseScale = 1f;
											this._noiseSparse = 0f;
											this._distance = 20f;
											this._distanceFallOff = 0.8f;
											this._height = 18f;
											this._stepping = 6f;
											this._steppingNear = 0f;
											this._alpha = 1f;
											this._color = new Color(0.91f, 0.91f, 0.91f, 1f);
											this._skyColor = this._color;
											this._specularColor = new Color(1f, 1f, 0.8f, 1f);
											this._specularIntensity = 0f;
											this._specularThreshold = 0.6f;
											this._lightColor = Color.white;
											this._lightIntensity = 0f;
											this._speed = 0.015f;
											this._downsampling = 1;
											this._baselineRelativeToCamera = false;
											this.CheckWaterLevel(false);
											this._fogVoidRadius = 0f;
											this.CopyTransitionValues();
										}
									}
									else
									{
										this._skySpeed = 0.3f;
										this._skyHaze = 144f;
										this._skyNoiseStrength = 0.7f;
										this._skyAlpha = 0.9f;
										this._density = 0.35f;
										this._noiseStrength = 0.3f;
										this._noiseScale = 1f;
										this._noiseSparse = 0f;
										this._distance = 20f;
										this._distanceFallOff = 0.7f;
										this._height = 8f;
										this._stepping = 8f;
										this._steppingNear = 0f;
										this._alpha = 0.97f;
										this._color = new Color(0.89f, 0.89f, 0.89f, 1f);
										this._skyColor = this._color;
										this._specularColor = new Color(1f, 1f, 0.8f, 1f);
										this._specularIntensity = 0f;
										this._specularThreshold = 0.6f;
										this._lightColor = Color.white;
										this._lightIntensity = 0f;
										this._speed = 0.05f;
										this._downsampling = 1;
										this._baselineRelativeToCamera = false;
										this.CheckWaterLevel(false);
										this._fogVoidRadius = 0f;
										this.CopyTransitionValues();
									}
								}
								else
								{
									this._skySpeed = 0.3f;
									this._skyHaze = 60f;
									this._skyNoiseStrength = 1f;
									this._skyAlpha = 0.96f;
									this._density = 1f;
									this._noiseStrength = 1f;
									this._noiseScale = 1.5f;
									this._noiseSparse = 0f;
									this._distance = 0f;
									this._distanceFallOff = 0f;
									this._height = 12.4f;
									this._stepping = 6f;
									this._alpha = 0.98f;
									this._color = new Color(0.89f, 0.89f, 0.89f, 1f);
									this._skyColor = this._color;
									this._specularColor = new Color(1f, 1f, 0.8f, 1f);
									this._specularIntensity = 0.259f;
									this._specularThreshold = 0.6f;
									this._lightColor = Color.white;
									this._lightIntensity = 0.15f;
									this._speed = 0.008f;
									this._downsampling = 1;
									this._baselineRelativeToCamera = false;
									this.CheckWaterLevel(false);
									this._fogVoidRadius = 0f;
									this.CopyTransitionValues();
								}
							}
							else
							{
								this._skySpeed = 0.3f;
								this._skyHaze = 60f;
								this._skyNoiseStrength = 1f;
								this._skyAlpha = 0.96f;
								this._density = 1f;
								this._noiseStrength = 0.7f;
								this._noiseScale = 1f;
								this._noiseSparse = 0f;
								this._distance = 0f;
								this._distanceFallOff = 0f;
								this._height = 4f;
								this._stepping = 12f;
								this._steppingNear = 0f;
								this._alpha = 0.98f;
								this._color = new Color(0.89f, 0.89f, 0.89f, 1f);
								this._skyColor = this._color;
								this._specularColor = new Color(1f, 1f, 0.8f, 1f);
								this._specularIntensity = 0.15f;
								this._specularThreshold = 0.6f;
								this._lightColor = Color.white;
								this._lightIntensity = 0.15f;
								this._speed = 0.008f;
								this._downsampling = 1;
								this._baselineRelativeToCamera = false;
								this.CheckWaterLevel(false);
								this._fogVoidRadius = 0f;
								this.CopyTransitionValues();
							}
						}
						else
						{
							this._skySpeed = 0.3f;
							this._skyHaze = 25f;
							this._skyNoiseStrength = 0.1f;
							this._skyAlpha = 0.85f;
							this._density = 0.3f;
							this._noiseStrength = 0.5f;
							this._noiseScale = 1.15f;
							this._noiseSparse = 0f;
							this._distance = 0f;
							this._distanceFallOff = 0f;
							this._height = 6.5f;
							this._stepping = 10f;
							this._steppingNear = 0f;
							this._alpha = 1f;
							this._color = new Color(0.89f, 0.89f, 0.89f, 1f);
							this._skyColor = this._color;
							this._specularColor = new Color(1f, 1f, 0.8f, 1f);
							this._specularIntensity = 0.1f;
							this._specularThreshold = 0.6f;
							this._lightColor = Color.white;
							this._lightIntensity = 0f;
							this._speed = 0.15f;
							this._downsampling = 1;
							this._baselineRelativeToCamera = false;
							this.CheckWaterLevel(false);
							this._fogVoidRadius = 0f;
							this.CopyTransitionValues();
						}
					}
					else
					{
						this._skySpeed = 0.3f;
						this._skyHaze = 15f;
						this._skyNoiseStrength = 0.1f;
						this._skyAlpha = 0.8f;
						this._density = 0.3f;
						this._noiseStrength = 0.6f;
						this._noiseScale = 1f;
						this._noiseSparse = 0f;
						this._distance = 0f;
						this._distanceFallOff = 0f;
						this._height = 6f;
						this._stepping = 8f;
						this._steppingNear = 0f;
						this._alpha = 1f;
						this._color = new Color(0.89f, 0.89f, 0.89f, 1f);
						this._skyColor = this._color;
						this._specularColor = new Color(1f, 1f, 0.8f, 1f);
						this._specularIntensity = 0.1f;
						this._specularThreshold = 0.6f;
						this._lightColor = Color.white;
						this._lightIntensity = 0.12f;
						this._speed = 0.01f;
						this._downsampling = 1;
						this._baselineRelativeToCamera = false;
						this.CheckWaterLevel(false);
						this._fogVoidRadius = 0f;
						this.CopyTransitionValues();
					}
					break;
				}
				break;
			}
			this.FogOfWarUpdateTexture();
			this.UpdateMaterialProperties();
			this.UpdateRenderComponents();
			this.UpdateTextureAlpha();
			this.UpdateTexture();
		}

		// Token: 0x06004BC0 RID: 19392 RVA: 0x00193D18 File Offset: 0x00192118
		public void CheckWaterLevel(bool baseZero)
		{
			if (this.mainCamera == null)
			{
				return;
			}
			if (this._baselineHeight > this.mainCamera.transform.position.y || baseZero)
			{
				this._baselineHeight = 0f;
			}
			GameObject gameObject = GameObject.Find("Water");
			if (gameObject == null)
			{
				GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null && array[i].layer == 4)
					{
						gameObject = array[i];
						break;
					}
				}
			}
			if (gameObject != null)
			{
				this._renderBeforeTransparent = false;
				if (this._baselineHeight < gameObject.transform.position.y)
				{
					this._baselineHeight = gameObject.transform.position.y;
				}
			}
			this.UpdateMaterialHeights();
		}

		// Token: 0x06004BC1 RID: 19393 RVA: 0x00193E14 File Offset: 0x00192214
		public static Terrain GetActiveTerrain()
		{
			Terrain terrain = Terrain.activeTerrain;
			if (terrain != null && terrain.isActiveAndEnabled)
			{
				return terrain;
			}
			for (int i = 0; i < Terrain.activeTerrains.Length; i++)
			{
				terrain = Terrain.activeTerrains[i];
				if (terrain != null && terrain.isActiveAndEnabled)
				{
					return terrain;
				}
			}
			return null;
		}

		// Token: 0x06004BC2 RID: 19394 RVA: 0x00193E7A File Offset: 0x0019227A
		private void UpdateMaterialFogColor()
		{
			this.fogMat.SetColor("_Color", this.currentFogColor * 2f);
		}

		// Token: 0x06004BC3 RID: 19395 RVA: 0x00193E9C File Offset: 0x0019229C
		private void UpdateMaterialHeights()
		{
			this.currentFogAltitude = this._baselineHeight;
			Vector3 fogAreaPosition = this._fogAreaPosition;
			if (this._fogAreaRadius > 0f)
			{
				if (this._fogAreaCenter != null && this._fogAreaFollowMode == FOG_AREA_FOLLOW_MODE.FullXYZ)
				{
					this.currentFogAltitude += this._fogAreaCenter.transform.position.y;
				}
				fogAreaPosition.y = 0f;
			}
			if (this._baselineRelativeToCamera && !this._useXYPlane)
			{
				this.oldBaselineRelativeCameraY += (this.mainCamera.transform.position.y - this.oldBaselineRelativeCameraY) * Mathf.Clamp01(1.001f - this._baselineRelativeToCameraDelay);
				this.currentFogAltitude += this.oldBaselineRelativeCameraY - 1f;
			}
			float w = 0.01f / this._noiseScale;
			this.fogMat.SetVector("_FogData", new Vector4(this.currentFogAltitude, this._height, 1f / this._density, w));
			this.fogMat.SetFloat("_FogSkyHaze", this._skyHaze + this.currentFogAltitude);
			Vector3 v = this._fogVoidPosition - this.currentFogAltitude * Vector3.up;
			this.fogMat.SetVector("_FogVoidPosition", v);
			this.fogMat.SetVector("_FogAreaPosition", fogAreaPosition);
		}

		// Token: 0x06004BC4 RID: 19396 RVA: 0x00194028 File Offset: 0x00192428
		private void UpdateMaterialProperties()
		{
			this.shouldUpdateMaterialProperties = true;
			if (!Application.isPlaying)
			{
				this.UpdateMaterialPropertiesNow();
			}
		}

		// Token: 0x06004BC5 RID: 19397 RVA: 0x00194044 File Offset: 0x00192444
		private void UpdateMaterialPropertiesNow()
		{
			if (this.fogMat == null)
			{
				return;
			}
			this.shouldUpdateMaterialProperties = false;
			this.UpdateSkyColor(this._skyAlpha);
			Vector4 value = new Vector4(1f / (this._stepping + 1f), 1f / (1f + this._steppingNear), this._edgeThreshold, (!this._dithering) ? 0f : (this._ditherStrength * 0.1f));
			if (!this._edgeImprove)
			{
				value.z = 0f;
			}
			this.fogMat.SetVector("_FogStepping", value);
			this.fogMat.SetFloat("_FogAlpha", this.currentFogAlpha);
			this.UpdateMaterialHeights();
			float num = 0.01f / this._noiseScale;
			float w = this._maxFogLength * this._maxFogLengthFallOff + 1f;
			this.fogMat.SetVector("_FogDistance", new Vector4(num * num * this._distance * this._distance, this._distanceFallOff * this._distanceFallOff + 0.1f, this._maxFogLength, w));
			this.UpdateMaterialFogColor();
			if (this.shaderKeywords == null)
			{
				this.shaderKeywords = new List<string>();
			}
			else
			{
				this.shaderKeywords.Clear();
			}
			if (this._distance > 0f)
			{
				this.shaderKeywords.Add("FOG_DISTANCE_ON");
			}
			if (this._fogVoidRadius > 0f && this._fogVoidFallOff > 0f)
			{
				Vector4 value2 = new Vector4(1f / (1f + this._fogVoidRadius), 1f / (1f + this._fogVoidHeight), 1f / (1f + this._fogVoidDepth), this._fogVoidFallOff);
				if (this._fogVoidTopology == FOG_VOID_TOPOLOGY.Box)
				{
					this.shaderKeywords.Add("FOG_VOID_BOX");
				}
				else
				{
					this.shaderKeywords.Add("FOG_VOID_SPHERE");
				}
				this.fogMat.SetVector("_FogVoidData", value2);
			}
			if (this._fogAreaRadius > 0f && this._fogAreaFallOff > 0f)
			{
				Vector4 value3 = new Vector4(1f / (0.0001f + this._fogAreaRadius), 1f / (0.0001f + this._fogAreaHeight), 1f / (0.0001f + this._fogAreaDepth), this._fogAreaFallOff);
				if (this._fogAreaTopology == FOG_AREA_TOPOLOGY.Box)
				{
					this.shaderKeywords.Add("FOG_AREA_BOX");
				}
				else
				{
					this.shaderKeywords.Add("FOG_AREA_SPHERE");
					value3.y = this._fogAreaRadius * this._fogAreaRadius;
					value3.x /= num;
					value3.z /= num;
				}
				this.fogMat.SetVector("_FogAreaData", value3);
			}
			if (this._skyHaze > 0f && this._skyAlpha > 0f && !this._useXYPlane)
			{
				this.shaderKeywords.Add("FOG_HAZE_ON");
			}
			if (this._fogOfWarEnabled)
			{
				this.shaderKeywords.Add("FOG_OF_WAR_ON");
				this.fogMat.SetTexture("_FogOfWar", this.fogOfWarTexture);
				this.fogMat.SetVector("_FogOfWarCenter", this._fogOfWarCenter);
				this.fogMat.SetVector("_FogOfWarSize", this._fogOfWarSize);
				Vector3 vector = this._fogOfWarCenter - 0.5f * this._fogOfWarSize;
				if (this._useXYPlane)
				{
					this.fogMat.SetVector("_FogOfWarCenterAdjusted", new Vector3(vector.x / this._fogOfWarSize.x, vector.y / (this._fogOfWarSize.y + 0.0001f), 1f));
				}
				else
				{
					this.fogMat.SetVector("_FogOfWarCenterAdjusted", new Vector3(vector.x / this._fogOfWarSize.x, 1f, vector.z / (this._fogOfWarSize.z + 0.0001f)));
				}
			}
			int num2 = -1;
			for (int i = 0; i < 6; i++)
			{
				if (this._pointLights[i] != null || this._pointLightRanges[i] * this._pointLightIntensities[i] > 0f)
				{
					num2 = i;
				}
			}
			if (num2 >= 0)
			{
				this.shaderKeywords.Add("FOG_POINT_LIGHT" + num2.ToString());
			}
			if (this.fogRenderer.sun)
			{
				this.UpdateScatteringData();
				if (this._lightScatteringEnabled && this._lightScatteringExposure > 0f)
				{
					this.shaderKeywords.Add("FOG_SCATTERING_ON");
				}
				if (this._sunShadows)
				{
					this.shaderKeywords.Add("FOG_SUN_SHADOWS_ON");
					this.UpdateSunShadowsData();
				}
			}
			if (this._fogBlur)
			{
				this.shaderKeywords.Add("FOG_BLUR_ON");
				this.fogMat.SetFloat("_FogBlurDepth", this._fogBlurDepth);
			}
			if (this._useXYPlane)
			{
				this.shaderKeywords.Add("FOG_USE_XY_PLANE");
			}
			if (!this._renderBeforeTransparent && this._transparencyBlendMode == TRANSPARENT_MODE.Blend)
			{
				this.fogMat.SetFloat("_BlendPower", this._transparencyBlendPower);
				this.shaderKeywords.Add("FOG_TRANSPARENCY_BLEND_PASS");
			}
			if (this.fogRenderer.computeDepth)
			{
				this.shaderKeywords.Add("FOG_COMPUTE_DEPTH");
			}
			this.fogMat.shaderKeywords = this.shaderKeywords.ToArray();
		}

		// Token: 0x06004BC6 RID: 19398 RVA: 0x0019463C File Offset: 0x00192A3C
		private void UpdateSunShadowsData()
		{
			if (this._sun == null || !this._sunShadows || this.fogMat == null)
			{
				return;
			}
			float num = this._sunShadowsStrength * Mathf.Clamp01(-this._sun.transform.forward.y * 10f);
			if (num < 0f)
			{
				num = 0f;
			}
			if (num > 0f && !this.fogMat.IsKeywordEnabled("FOG_SUN_SHADOWS_ON"))
			{
				this.fogMat.EnableKeyword("FOG_SUN_SHADOWS_ON");
			}
			else if (num <= 0f && this.fogMat.IsKeywordEnabled("FOG_SUN_SHADOWS_ON"))
			{
				this.fogMat.DisableKeyword("FOG_SUN_SHADOWS_ON");
			}
			if (this._hasCamera)
			{
				Shader.SetGlobalVector("_VolumetricFogSunShadowsData", new Vector4(num, this._sunShadowsJitterStrength, this._sunShadowsCancellation, 0f));
			}
		}

		// Token: 0x06004BC7 RID: 19399 RVA: 0x00194748 File Offset: 0x00192B48
		private void UpdateWindSpeedQuick()
		{
			if (this.fogMat == null)
			{
				return;
			}
			float d = 0.01f / this._noiseScale;
			this.windSpeedAcum += Time.deltaTime * this._windDirection * this._speed / d;
			this.fogMat.SetVector("_FogWindDir", this.windSpeedAcum);
			this.skyHazeSpeedAcum += Time.deltaTime * this._skySpeed / 20f;
			this.fogMat.SetVector("_FogSkyData", new Vector4(this._skyHaze, this._skyNoiseStrength, this.skyHazeSpeedAcum, this._skyDepth));
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x00194810 File Offset: 0x00192C10
		private void UpdateScatteringData()
		{
			Vector3 vector = this.mainCamera.WorldToViewportPoint(this.fogRenderer.sun.transform.forward * 10000f);
			if (vector.z < 0f)
			{
				Vector2 vector2 = new Vector2(vector.x, vector.y);
				float num = Mathf.Clamp01(1f - this._lightDirection.y);
				if (vector2 != this.oldSunPos)
				{
					this.oldSunPos = vector2;
					this.fogMat.SetVector("_SunPosition", vector2);
					this.sunFade = Mathf.SmoothStep(1f, 0f, (vector2 - Vector2.one * 0.5f).magnitude * 0.5f) * num;
				}
				if (this._lightScatteringEnabled && !this.fogMat.IsKeywordEnabled("FOG_SCATTERING_ON"))
				{
					this.fogMat.EnableKeyword("FOG_SCATTERING_ON");
				}
				float num2 = this._lightScatteringExposure * this.sunFade;
				this.fogMat.SetVector("_FogScatteringData", new Vector4(this._lightScatteringSpread / (float)this._lightScatteringSamples, (float)((num2 <= 0f) ? 0 : this._lightScatteringSamples), num2, this._lightScatteringWeight / (float)this._lightScatteringSamples));
				this.fogMat.SetVector("_FogScatteringData2", new Vector4(this._lightScatteringIllumination, this._lightScatteringDecay, this._lightScatteringJittering, (!this._lightScatteringEnabled) ? 0f : (1.2f * this._lightScatteringDiffusion * num * this.sunLightIntensity)));
				this.fogMat.SetVector("_SunDir", -this._lightDirection);
				this.fogMat.SetColor("_SunColor", this._lightColor);
			}
			else if (this.fogMat.IsKeywordEnabled("FOG_SCATTERING_ON"))
			{
				this.fogMat.DisableKeyword("FOG_SCATTERING_ON");
			}
		}

		// Token: 0x06004BC9 RID: 19401 RVA: 0x00194A29 File Offset: 0x00192E29
		private void UpdateSun()
		{
			if (this.fogRenderer.sun != null)
			{
				this.sunLight = this.fogRenderer.sun.GetComponent<Light>();
			}
			else
			{
				this.sunLight = null;
			}
		}

		// Token: 0x06004BCA RID: 19402 RVA: 0x00194A64 File Offset: 0x00192E64
		private void UpdateSkyColor(float alpha)
		{
			if (this.fogMat == null)
			{
				return;
			}
			float num = (this._lightIntensity + this.sunLightIntensity) * Mathf.Clamp01(1f - this._lightDirection.y);
			if (num < 0f)
			{
				num = 0f;
			}
			else if (num > 1f)
			{
				num = 1f;
			}
			this._skyColor.a = alpha;
			Color value = num * this._skyColor;
			this.fogMat.SetColor("_FogSkyColor", value);
		}

		// Token: 0x06004BCB RID: 19403 RVA: 0x00194AFC File Offset: 0x00192EFC
		private void UpdatePointLights()
		{
			for (int i = 0; i < this._pointLights.Length; i++)
			{
				GameObject gameObject = this._pointLights[i];
				if (gameObject != null)
				{
					this.pointLightComponents[i] = gameObject.GetComponent<Light>();
				}
				else
				{
					this.pointLightComponents[i] = null;
				}
			}
		}

		// Token: 0x06004BCC RID: 19404 RVA: 0x00194B54 File Offset: 0x00192F54
		private void UpdateTextureAlpha()
		{
			if (this.adjustedColors == null)
			{
				return;
			}
			float num = Mathf.Clamp(this._noiseStrength, 0f, 0.95f);
			for (int i = 0; i < this.adjustedColors.Length; i++)
			{
				float num2 = 1f - (this._noiseSparse + this.noiseColors[i].b) * num;
				num2 *= this._density * this._noiseFinalMultiplier;
				if (num2 < 0f)
				{
					num2 = 0f;
				}
				else if (num2 > 1f)
				{
					num2 = 1f;
				}
				this.adjustedColors[i].a = num2;
			}
			this.hasChangeAdjustedColorsAlpha = true;
		}

		// Token: 0x06004BCD RID: 19405 RVA: 0x00194C10 File Offset: 0x00193010
		private void UpdateTexture()
		{
			if (this.fogMat == null)
			{
				return;
			}
			this.UpdateSkyColor(this._skyAlpha);
			float num = this._lightIntensity + this.sunLightIntensity;
			if (!this._useXYPlane)
			{
				num *= Mathf.Clamp01(1f - this._lightDirection.y * 2f);
			}
			LIGHTING_MODEL lightingModel = this._lightingModel;
			if (lightingModel != LIGHTING_MODEL.Natural)
			{
				if (lightingModel != LIGHTING_MODEL.SingleLight)
				{
					Color a = RenderSettings.ambientLight * RenderSettings.ambientIntensity;
					this.updatingTextureLightColor = Color.Lerp(a, this.currentLightColor * num, num);
					this.lastRenderSettingsAmbientLight = RenderSettings.ambientLight;
					this.lastRenderSettingsAmbientIntensity = RenderSettings.ambientIntensity;
				}
				else
				{
					this.updatingTextureLightColor = Color.Lerp(Color.black, this.currentLightColor * num, this._lightIntensity);
				}
			}
			else
			{
				Color ambientLight = RenderSettings.ambientLight;
				this.lastRenderSettingsAmbientLight = RenderSettings.ambientLight;
				this.updatingTextureLightColor = Color.Lerp(ambientLight, this.currentLightColor * num + ambientLight, this._lightIntensity);
			}
			if (Application.isPlaying)
			{
				this.updatingTextureSlice = 0;
			}
			else
			{
				this.updatingTextureSlice = -1;
			}
			this.UpdateTextureColors(this.adjustedColors, this.hasChangeAdjustedColorsAlpha);
			this.needUpdateTexture = false;
		}

		// Token: 0x06004BCE RID: 19406 RVA: 0x00194D68 File Offset: 0x00193168
		private void UpdateTextureColors(Color[] colors, bool forceUpdateEntireTexture)
		{
			float num = 1.0001f - this._specularThreshold;
			int width = this.adjustedTexture.width;
			Vector3 vector = new Vector3(-this._lightDirection.x, 0f, -this._lightDirection.z);
			Vector3 vector2 = vector.normalized * 0.3f;
			vector2.y = ((this._lightDirection.y <= 0f) ? (1f - Mathf.Clamp01(-this._lightDirection.y)) : Mathf.Clamp01(1f - this._lightDirection.y));
			int num2 = Mathf.FloorToInt(vector2.z * (float)width) * width;
			int num3 = (int)((float)num2 + vector2.x * (float)width) + colors.Length;
			float num4 = vector2.y / num;
			Color color = this.currentFogSpecularColor * (1f + this._specularIntensity) * this._specularIntensity;
			bool flag = false;
			if (this.updatingTextureSlice >= 1 || forceUpdateEntireTexture)
			{
				flag = true;
			}
			float num5 = this.updatingTextureLightColor.r * 0.5f;
			float num6 = this.updatingTextureLightColor.g * 0.5f;
			float num7 = this.updatingTextureLightColor.b * 0.5f;
			float num8 = color.r * 0.5f;
			float num9 = color.g * 0.5f;
			float num10 = color.b * 0.5f;
			int num11 = colors.Length;
			int num12 = 0;
			int num13 = num11;
			if (this.updatingTextureSlice >= 0)
			{
				if (this.updatingTextureSlice > this._updateTextureSpread)
				{
					this.updatingTextureSlice = -1;
					this.needUpdateTexture = true;
					return;
				}
				num12 = num11 * this.updatingTextureSlice / this._updateTextureSpread;
				num13 = num11 * (this.updatingTextureSlice + 1) / this._updateTextureSpread;
			}
			int num14 = 0;
			for (int i = num12; i < num13; i++)
			{
				int num15 = (i + num3) % num11;
				float a = colors[i].a;
				float num16 = (a - colors[num15].a) * num4;
				if (num16 < 0f)
				{
					num16 = 0f;
				}
				else if (num16 > 1f)
				{
					num16 = 1f;
				}
				float num17 = num5 + num8 * num16;
				float num18 = num6 + num9 * num16;
				float num19 = num7 + num10 * num16;
				if (!flag)
				{
					if (num14++ < 100)
					{
						if (num17 != colors[i].r || num18 != colors[i].g || num19 != colors[i].b)
						{
							flag = true;
						}
					}
					else if (!flag)
					{
						break;
					}
				}
				colors[i].r = num17;
				colors[i].g = num18;
				colors[i].b = num19;
			}
			bool flag2 = forceUpdateEntireTexture;
			if (flag)
			{
				if (this.updatingTextureSlice >= 0)
				{
					this.updatingTextureSlice++;
					if (this.updatingTextureSlice >= this._updateTextureSpread)
					{
						this.updatingTextureSlice = -1;
						flag2 = true;
					}
				}
				else
				{
					flag2 = true;
				}
			}
			else
			{
				this.updatingTextureSlice = -1;
			}
			if (flag2)
			{
				if (Application.isPlaying && this._turbulenceStrength > 0f && this.adjustedChaosTexture)
				{
					this.adjustedChaosTexture.SetPixels(this.adjustedColors);
					this.adjustedChaosTexture.Apply();
				}
				else
				{
					this.adjustedTexture.SetPixels(this.adjustedColors);
					this.adjustedTexture.Apply();
					this.fogMat.SetTexture("_NoiseTex", this.adjustedTexture);
				}
				this.lastTextureUpdate = Time.time;
			}
		}

		// Token: 0x06004BCF RID: 19407 RVA: 0x0019514C File Offset: 0x0019354C
		private void ApplyChaos()
		{
			if (!this.adjustedTexture)
			{
				return;
			}
			if (this.chaosLerpMat == null)
			{
				Shader shader = Shader.Find("VolumetricFogAndMist/Chaos Lerp");
				this.chaosLerpMat = new Material(shader);
				this.chaosLerpMat.hideFlags = HideFlags.DontSave;
			}
			this.turbAcum += Time.deltaTime * this._turbulenceStrength;
			this.chaosLerpMat.SetFloat("_Amount", this.turbAcum);
			if (!this.adjustedChaosTexture)
			{
				this.adjustedChaosTexture = UnityEngine.Object.Instantiate<Texture2D>(this.adjustedTexture);
				this.adjustedChaosTexture.hideFlags = HideFlags.DontSave;
			}
			RenderTexture temporary = RenderTexture.GetTemporary(this.adjustedTexture.width, this.adjustedTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			temporary.wrapMode = TextureWrapMode.Repeat;
			Graphics.Blit(this.adjustedChaosTexture, temporary, this.chaosLerpMat);
			this.fogMat.SetTexture("_NoiseTex", temporary);
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x06004BD0 RID: 19408 RVA: 0x0019524B File Offset: 0x0019364B
		private void CopyTransitionValues()
		{
			this.currentFogAlpha = this._alpha;
			this.currentSkyHazeAlpha = this._skyAlpha;
			this.currentFogColor = this._color;
			this.currentFogSpecularColor = this._specularColor;
			this.currentLightColor = this._lightColor;
		}

		// Token: 0x06004BD1 RID: 19409 RVA: 0x0019528C File Offset: 0x0019368C
		public void SetTargetProfile(VolumetricFogProfile targetProfile, float duration)
		{
			if (!this._useFogVolumes)
			{
				return;
			}
			this.initialProfile = ScriptableObject.CreateInstance<VolumetricFogProfile>();
			this.initialProfile.Save(this);
			this.targetProfile = targetProfile;
			this.transitionDuration = duration;
			this.transitionStartTime = Time.time;
			this.transitionProfile = true;
		}

		// Token: 0x06004BD2 RID: 19410 RVA: 0x001952DC File Offset: 0x001936DC
		public void ClearTargetProfile(float duration)
		{
			this.SetTargetProfile(this.initialProfile, duration);
		}

		// Token: 0x06004BD3 RID: 19411 RVA: 0x001952EC File Offset: 0x001936EC
		public void SetTargetAlpha(float newFogAlpha, float newSkyHazeAlpha, float duration)
		{
			if (!this._useFogVolumes)
			{
				return;
			}
			this.initialFogAlpha = this.currentFogAlpha;
			this.initialSkyHazeAlpha = this.currentSkyHazeAlpha;
			this.targetFogAlpha = newFogAlpha;
			this.targetSkyHazeAlpha = newSkyHazeAlpha;
			this.transitionDuration = duration;
			this.transitionStartTime = Time.time;
			this.transitionAlpha = true;
		}

		// Token: 0x06004BD4 RID: 19412 RVA: 0x00195344 File Offset: 0x00193744
		public void ClearTargetAlpha(float duration)
		{
			this.SetTargetAlpha(-1f, -1f, duration);
		}

		// Token: 0x06004BD5 RID: 19413 RVA: 0x00195358 File Offset: 0x00193758
		public void SetTargetColor(Color newColor, float duration)
		{
			if (!this.useFogVolumes)
			{
				return;
			}
			this.initialFogColor = this.currentFogColor;
			this.targetFogColor = newColor;
			this.transitionDuration = duration;
			this.transitionStartTime = Time.time;
			this.transitionColor = true;
			this.targetColorActive = true;
		}

		// Token: 0x06004BD6 RID: 19414 RVA: 0x001953A4 File Offset: 0x001937A4
		public void ClearTargetColor(float duration)
		{
			this.SetTargetColor(this._color, duration);
			this.targetColorActive = false;
		}

		// Token: 0x06004BD7 RID: 19415 RVA: 0x001953BC File Offset: 0x001937BC
		public void SetTargetSpecularColor(Color newSpecularColor, float duration)
		{
			if (!this.useFogVolumes)
			{
				return;
			}
			this.initialFogSpecularColor = this.currentFogSpecularColor;
			this.targetFogSpecularColor = newSpecularColor;
			this.transitionDuration = duration;
			this.transitionStartTime = Time.time;
			this.transitionSpecularColor = true;
			this.targetSpecularColorActive = true;
		}

		// Token: 0x06004BD8 RID: 19416 RVA: 0x00195408 File Offset: 0x00193808
		public void ClearTargetSpecularColor(float duration)
		{
			this.SetTargetSpecularColor(this._specularColor, duration);
			this.targetSpecularColorActive = false;
		}

		// Token: 0x06004BD9 RID: 19417 RVA: 0x00195420 File Offset: 0x00193820
		public void SetTargetLightColor(Color newLightColor, float duration)
		{
			if (!this.useFogVolumes)
			{
				return;
			}
			this._sunCopyColor = false;
			this.initialLightColor = this.currentLightColor;
			this.targetLightColor = newLightColor;
			this.transitionDuration = duration;
			this.transitionStartTime = Time.time;
			this.transitionLightColor = true;
			this.targetLightColorActive = true;
		}

		// Token: 0x06004BDA RID: 19418 RVA: 0x00195473 File Offset: 0x00193873
		public void ClearTargetLightColor(float duration)
		{
			this.SetTargetLightColor(this._lightColor, duration);
			this.targetLightColorActive = false;
		}

		// Token: 0x06004BDB RID: 19419 RVA: 0x0019548C File Offset: 0x0019388C
		private void SetMaterialLightData(int k, Light lightComponent)
		{
			string str = k.ToString();
			Vector3 v = this._pointLightPositions[k];
			v.y -= this._baselineHeight;
			Vector3 v2 = new Vector3(this._pointLightColors[k].r, this._pointLightColors[k].g, this._pointLightColors[k].b) * this._pointLightIntensities[k] * 0.1f * this._pointLightIntensitiesMultiplier[k] * (this._pointLightRanges[k] * this._pointLightRanges[k]);
			this.fogMat.SetVector("_FogPointLightPosition" + str, v);
			this.fogMat.SetVector("_FogPointLightColor" + str, v2);
		}

		// Token: 0x06004BDC RID: 19420 RVA: 0x00195578 File Offset: 0x00193978
		public GameObject GetPointLight(int index)
		{
			if (index < 0 || index > this._pointLights.Length)
			{
				return null;
			}
			return this._pointLights[index];
		}

		// Token: 0x06004BDD RID: 19421 RVA: 0x0019559C File Offset: 0x0019399C
		public void SetPointLight(int index, GameObject pointLight)
		{
			if (index < 0 || index > this._pointLights.Length)
			{
				return;
			}
			if (this._pointLights[index] != pointLight)
			{
				this._pointLights[index] = pointLight;
				this.UpdatePointLights();
				this.UpdateMaterialProperties();
				this.isDirty = true;
			}
		}

		// Token: 0x06004BDE RID: 19422 RVA: 0x001955EE File Offset: 0x001939EE
		public float GetPointLightRange(int index)
		{
			if (index < 0 || index > this._pointLightRanges.Length)
			{
				return 0f;
			}
			return this._pointLightRanges[index];
		}

		// Token: 0x06004BDF RID: 19423 RVA: 0x00195613 File Offset: 0x00193A13
		public void SetPointLightRange(int index, float range)
		{
			if (index < 0 || index > this._pointLightRanges.Length)
			{
				return;
			}
			if (range != this._pointLightRanges[index])
			{
				this._pointLightRanges[index] = range;
				this.UpdateMaterialProperties();
				this.isDirty = true;
			}
		}

		// Token: 0x06004BE0 RID: 19424 RVA: 0x0019564F File Offset: 0x00193A4F
		public float GetPointLightIntensity(int index)
		{
			if (index < 0 || index > this._pointLightIntensities.Length)
			{
				return 0f;
			}
			return this._pointLightIntensities[index];
		}

		// Token: 0x06004BE1 RID: 19425 RVA: 0x00195674 File Offset: 0x00193A74
		public void SetPointLightIntensity(int index, float intensity)
		{
			if (index < 0 || index > this._pointLightIntensities.Length)
			{
				return;
			}
			if (intensity != this._pointLightIntensities[index])
			{
				this._pointLightIntensities[index] = intensity;
				this.UpdateMaterialProperties();
				this.isDirty = true;
			}
		}

		// Token: 0x06004BE2 RID: 19426 RVA: 0x001956B0 File Offset: 0x00193AB0
		public float GetPointLightIntensityMultiplier(int index)
		{
			if (index < 0 || index > this._pointLightIntensitiesMultiplier.Length)
			{
				return 0f;
			}
			return this._pointLightIntensitiesMultiplier[index];
		}

		// Token: 0x06004BE3 RID: 19427 RVA: 0x001956D5 File Offset: 0x00193AD5
		public void SetPointLightIntensityMultiplier(int index, float intensityMultiplier)
		{
			if (index < 0 || index > this._pointLightIntensitiesMultiplier.Length)
			{
				return;
			}
			if (intensityMultiplier != this._pointLightIntensitiesMultiplier[index])
			{
				this._pointLightIntensitiesMultiplier[index] = intensityMultiplier;
				this.UpdateMaterialProperties();
				this.isDirty = true;
			}
		}

		// Token: 0x06004BE4 RID: 19428 RVA: 0x00195711 File Offset: 0x00193B11
		public Vector3 GetPointLightPosition(int index)
		{
			if (index < 0 || index > this._pointLightPositions.Length)
			{
				return Vector3.zero;
			}
			return this._pointLightPositions[index];
		}

		// Token: 0x06004BE5 RID: 19429 RVA: 0x00195740 File Offset: 0x00193B40
		public void SetPointLightPosition(int index, Vector3 position)
		{
			if (index < 0 || index > this._pointLightPositions.Length)
			{
				return;
			}
			if (position != this._pointLightPositions[index])
			{
				this._pointLightPositions[index] = position;
				this.UpdateMaterialProperties();
				this.isDirty = true;
			}
		}

		// Token: 0x06004BE6 RID: 19430 RVA: 0x0019579E File Offset: 0x00193B9E
		public Color GetPointLightColor(int index)
		{
			if (index < 0 || index > this._pointLightColors.Length)
			{
				return Color.white;
			}
			return this._pointLightColors[index];
		}

		// Token: 0x06004BE7 RID: 19431 RVA: 0x001957CC File Offset: 0x00193BCC
		public void SetPointLightColor(int index, Color color)
		{
			if (index < 0 || index > this._pointLightColors.Length)
			{
				return;
			}
			if (color != this._pointLightColors[index])
			{
				this._pointLightColors[index] = color;
				this.UpdateMaterialProperties();
				this.isDirty = true;
			}
		}

		// Token: 0x06004BE8 RID: 19432 RVA: 0x0019582A File Offset: 0x00193C2A
		private void TrackNewLights()
		{
			this.lastFoundLights = UnityEngine.Object.FindObjectsOfType<Light>();
		}

		// Token: 0x06004BE9 RID: 19433 RVA: 0x00195838 File Offset: 0x00193C38
		private void TrackPointLights()
		{
			if (!this._pointLightTrackingAuto)
			{
				return;
			}
			if (this.lastFoundLights == null || !Application.isPlaying || Time.time - this.trackPointCheckNewLightsLastTime > 3f)
			{
				this.trackPointCheckNewLightsLastTime = Time.time;
				this.TrackNewLights();
			}
			int num = this.lastFoundLights.Length;
			if (this.lightBuffer == null || this.lightBuffer.Length != num)
			{
				this.lightBuffer = new Light[num];
			}
			for (int i = 0; i < num; i++)
			{
				this.lightBuffer[i] = this.lastFoundLights[i];
			}
			bool flag = false;
			for (int j = 0; j < 6; j++)
			{
				GameObject gameObject = null;
				if (j < this._pointLightTrackingCount)
				{
					gameObject = this.GetNearestLight(this.lightBuffer);
				}
				this._pointLights[j] = gameObject;
				this._pointLightRanges[j] = 0f;
				if (this.currentLights[j] != gameObject)
				{
					this.currentLights[j] = gameObject;
					flag = true;
				}
			}
			if (flag)
			{
				this.UpdatePointLights();
				this.UpdateMaterialProperties();
			}
		}

		// Token: 0x06004BEA RID: 19434 RVA: 0x00195958 File Offset: 0x00193D58
		private GameObject GetNearestLight(Light[] lights)
		{
			float num = float.MaxValue;
			Vector3 position = this.mainCamera.transform.position;
			GameObject result = null;
			int num2 = -1;
			for (int i = 0; i < lights.Length; i++)
			{
				Light light = lights[i];
				if (!(light == null) && light.enabled && light.type == LightType.Point)
				{
					GameObject gameObject = lights[i].gameObject;
					if (gameObject.activeSelf)
					{
						float sqrMagnitude = (gameObject.transform.position - position).sqrMagnitude;
						if (sqrMagnitude < num)
						{
							result = gameObject;
							num = sqrMagnitude;
							num2 = i;
						}
					}
				}
			}
			if (num2 >= 0)
			{
				lights[num2] = null;
			}
			return result;
		}

		// Token: 0x06004BEB RID: 19435 RVA: 0x00195A20 File Offset: 0x00193E20
		public static VolumetricFog CreateFogArea(Vector3 position, float radius, float height = 16f, float fallOff = 1f)
		{
			VolumetricFog volumetricFog = VolumetricFog.CreateFogAreaPlaceholder(true, position, radius, height, radius);
			volumetricFog.preset = FOG_PRESET.SeaClouds;
			volumetricFog.transform.position = position;
			volumetricFog.skyHaze = 0f;
			volumetricFog.dithering = true;
			return volumetricFog;
		}

		// Token: 0x06004BEC RID: 19436 RVA: 0x00195A60 File Offset: 0x00193E60
		public static VolumetricFog CreateFogArea(Vector3 position, Vector3 boxSize)
		{
			VolumetricFog volumetricFog = VolumetricFog.CreateFogAreaPlaceholder(false, position, boxSize.x * 0.5f, boxSize.y * 0.5f, boxSize.z * 0.5f);
			volumetricFog.preset = FOG_PRESET.SeaClouds;
			volumetricFog.transform.position = position;
			volumetricFog.height = boxSize.y * 0.98f;
			volumetricFog.skyHaze = 0f;
			return volumetricFog;
		}

		// Token: 0x06004BED RID: 19437 RVA: 0x00195AD0 File Offset: 0x00193ED0
		private static VolumetricFog CreateFogAreaPlaceholder(bool spherical, Vector3 position, float radius, float height, float depth)
		{
			GameObject original = (!spherical) ? Resources.Load<GameObject>("Prefabs/FogBoxArea") : Resources.Load<GameObject>("Prefabs/FogSphereArea");
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			gameObject.transform.position = position;
			gameObject.transform.localScale = new Vector3(radius, height, depth);
			return gameObject.GetComponent<VolumetricFog>();
		}

		// Token: 0x06004BEE RID: 19438 RVA: 0x00195B2C File Offset: 0x00193F2C
		public static void RemoveAllFogAreas()
		{
			VolumetricFog[] array = UnityEngine.Object.FindObjectsOfType<VolumetricFog>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && !array[i].hasCamera)
				{
					UnityEngine.Object.DestroyImmediate(array[i].gameObject);
				}
			}
		}

		// Token: 0x06004BEF RID: 19439 RVA: 0x00195B7C File Offset: 0x00193F7C
		private void CheckFogAreaDimensions()
		{
			if (this.mr == null)
			{
				this.mr = base.GetComponent<MeshRenderer>();
			}
			if (this.mr == null)
			{
				return;
			}
			Vector3 extents = this.mr.bounds.extents;
			FOG_AREA_TOPOLOGY fogAreaTopology = this._fogAreaTopology;
			if (fogAreaTopology != FOG_AREA_TOPOLOGY.Box)
			{
				if (fogAreaTopology == FOG_AREA_TOPOLOGY.Sphere)
				{
					this.fogAreaRadius = extents.x;
					if (base.transform.localScale.z != base.transform.localScale.x)
					{
						base.transform.localScale = new Vector3(base.transform.localScale.x, base.transform.localScale.y, base.transform.localScale.x);
					}
				}
			}
			else
			{
				this.fogAreaRadius = extents.x;
				this.fogAreaHeight = extents.y;
				this.fogAreaDepth = extents.z;
			}
			if (this._fogAreaCenter != null)
			{
				if (this._fogAreaFollowMode == FOG_AREA_FOLLOW_MODE.FullXYZ)
				{
					base.transform.position = this._fogAreaCenter.transform.position;
				}
				else
				{
					base.transform.position = new Vector3(this._fogAreaCenter.transform.position.x, base.transform.position.y, this._fogAreaCenter.transform.position.z);
				}
			}
			this.fogAreaPosition = base.transform.position;
		}

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x06004BF0 RID: 19440 RVA: 0x00195D3F File Offset: 0x0019413F
		// (set) Token: 0x06004BF1 RID: 19441 RVA: 0x00195D47 File Offset: 0x00194147
		public bool fogOfWarEnabled
		{
			get
			{
				return this._fogOfWarEnabled;
			}
			set
			{
				if (value != this._fogOfWarEnabled)
				{
					this._fogOfWarEnabled = value;
					this.FogOfWarUpdateTexture();
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x06004BF2 RID: 19442 RVA: 0x00195D6F File Offset: 0x0019416F
		// (set) Token: 0x06004BF3 RID: 19443 RVA: 0x00195D77 File Offset: 0x00194177
		public Vector3 fogOfWarCenter
		{
			get
			{
				return this._fogOfWarCenter;
			}
			set
			{
				if (value != this._fogOfWarCenter)
				{
					this._fogOfWarCenter = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06004BF4 RID: 19444 RVA: 0x00195D9E File Offset: 0x0019419E
		// (set) Token: 0x06004BF5 RID: 19445 RVA: 0x00195DA8 File Offset: 0x001941A8
		public Vector3 fogOfWarSize
		{
			get
			{
				return this._fogOfWarSize;
			}
			set
			{
				if (value != this._fogOfWarSize && value.x > 0f && value.z > 0f)
				{
					this._fogOfWarSize = value;
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06004BF6 RID: 19446 RVA: 0x00195DFC File Offset: 0x001941FC
		// (set) Token: 0x06004BF7 RID: 19447 RVA: 0x00195E04 File Offset: 0x00194204
		public int fogOfWarTextureSize
		{
			get
			{
				return this._fogOfWarTextureSize;
			}
			set
			{
				if (value != this._fogOfWarTextureSize && value > 16)
				{
					this._fogOfWarTextureSize = value;
					this.FogOfWarUpdateTexture();
					this.UpdateMaterialProperties();
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x06004BF8 RID: 19448 RVA: 0x00195E34 File Offset: 0x00194234
		// (set) Token: 0x06004BF9 RID: 19449 RVA: 0x00195E3C File Offset: 0x0019423C
		public float fogOfWarRestoreDelay
		{
			get
			{
				return this._fogOfWarRestoreDelay;
			}
			set
			{
				if (value != this._fogOfWarRestoreDelay)
				{
					this._fogOfWarRestoreDelay = value;
					this.isDirty = true;
				}
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x06004BFA RID: 19450 RVA: 0x00195E58 File Offset: 0x00194258
		// (set) Token: 0x06004BFB RID: 19451 RVA: 0x00195E60 File Offset: 0x00194260
		public float fogOfWarRestoreDuration
		{
			get
			{
				return this._fogOfWarRestoreDuration;
			}
			set
			{
				if (value != this._fogOfWarRestoreDuration)
				{
					this._fogOfWarRestoreDuration = value;
					this.isDirty = true;
				}
			}
		}

		// Token: 0x06004BFC RID: 19452 RVA: 0x00195E7C File Offset: 0x0019427C
		private void FogOfWarInit()
		{
			this.fowTransitionList = new List<VolumetricFog.FogOfWarTransition>();
		}

		// Token: 0x06004BFD RID: 19453 RVA: 0x00195E8C File Offset: 0x0019428C
		private void FogOfWarUpdateTexture()
		{
			if (!this._fogOfWarEnabled)
			{
				return;
			}
			int scaledSize = this.GetScaledSize(this._fogOfWarTextureSize, 1f);
			this.fogOfWarTexture = new Texture2D(scaledSize, scaledSize, TextureFormat.Alpha8, false);
			this.fogOfWarTexture.hideFlags = HideFlags.DontSave;
			this.fogOfWarTexture.filterMode = FilterMode.Bilinear;
			this.fogOfWarTexture.wrapMode = TextureWrapMode.Clamp;
			this.ResetFogOfWar();
		}

		// Token: 0x06004BFE RID: 19454 RVA: 0x00195EF4 File Offset: 0x001942F4
		private void FogOfWarUpdate()
		{
			if (!this._fogOfWarEnabled)
			{
				return;
			}
			int count = this.fowTransitionList.Count;
			int width = this.fogOfWarTexture.width;
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				VolumetricFog.FogOfWarTransition fogOfWarTransition = this.fowTransitionList[i];
				if (fogOfWarTransition.enabled)
				{
					float num = Time.time - fogOfWarTransition.startTime - fogOfWarTransition.startDelay;
					if (num > 0f)
					{
						float num2 = (fogOfWarTransition.duration > 0f) ? (num / fogOfWarTransition.duration) : 1f;
						num2 = Mathf.Clamp01(num2);
						byte a = (byte)Mathf.Lerp((float)fogOfWarTransition.initialAlpha, (float)fogOfWarTransition.targetAlpha, num2);
						int num3 = fogOfWarTransition.y * width + fogOfWarTransition.x;
						this.fogOfWarColorBuffer[num3].a = a;
						this.fogOfWarTexture.SetPixel(fogOfWarTransition.x, fogOfWarTransition.y, this.fogOfWarColorBuffer[num3]);
						flag = true;
						if (num2 >= 1f)
						{
							fogOfWarTransition.enabled = false;
							if (fogOfWarTransition.targetAlpha < 255 && this._fogOfWarRestoreDelay > 0f)
							{
								this.AddFowOfWarTransitionSlot(fogOfWarTransition.x, fogOfWarTransition.y, fogOfWarTransition.targetAlpha, byte.MaxValue, this._fogOfWarRestoreDelay, this._fogOfWarRestoreDuration);
							}
						}
					}
				}
			}
			if (flag)
			{
				this.fogOfWarTexture.Apply();
			}
		}

		// Token: 0x06004BFF RID: 19455 RVA: 0x00196090 File Offset: 0x00194490
		public void SetFogOfWarAlpha(Vector3 worldPosition, float radius, float fogNewAlpha)
		{
			this.SetFogOfWarAlpha(worldPosition, radius, fogNewAlpha, 1f);
		}

		// Token: 0x06004C00 RID: 19456 RVA: 0x001960A0 File Offset: 0x001944A0
		public void SetFogOfWarAlpha(Vector3 worldPosition, float radius, float fogNewAlpha, float duration)
		{
			if (this.fogOfWarTexture == null)
			{
				return;
			}
			float num = (worldPosition.x - this._fogOfWarCenter.x) / this._fogOfWarSize.x + 0.5f;
			if (num < 0f || num > 1f)
			{
				return;
			}
			float num2 = (worldPosition.z - this._fogOfWarCenter.z) / this._fogOfWarSize.z + 0.5f;
			if (num2 < 0f || num2 > 1f)
			{
				return;
			}
			int width = this.fogOfWarTexture.width;
			int height = this.fogOfWarTexture.height;
			int num3 = (int)(num * (float)width);
			int num4 = (int)(num2 * (float)height);
			int num5 = num4 * width + num3;
			byte b = (byte)(fogNewAlpha * 255f);
			Color32 color = this.fogOfWarColorBuffer[num5];
			if (b != color.a)
			{
				float num6 = radius / this._fogOfWarSize.z;
				int num7 = Mathf.FloorToInt((float)height * num6);
				for (int i = num4 - num7; i <= num4 + num7; i++)
				{
					if (i > 0 && i < height - 1)
					{
						for (int j = num3 - num7; j <= num3 + num7; j++)
						{
							if (j > 0 && j < width - 1)
							{
								int num8 = Mathf.FloorToInt(Mathf.Sqrt((float)((num4 - i) * (num4 - i) + (num3 - j) * (num3 - j))));
								if (num8 <= num7)
								{
									num5 = i * width + j;
									Color32 color2 = this.fogOfWarColorBuffer[num5];
									byte b2 = (byte)Mathf.Lerp((float)b, (float)color2.a, (float)num8 / (float)num7);
									if (duration > 0f)
									{
										this.AddFowOfWarTransitionSlot(j, i, color2.a, b2, 0f, duration);
									}
									else
									{
										color2.a = b2;
										this.fogOfWarColorBuffer[num5] = color2;
										this.fogOfWarTexture.SetPixel(j, i, color2);
										if (this._fogOfWarRestoreDuration > 0f)
										{
											this.AddFowOfWarTransitionSlot(j, i, b2, byte.MaxValue, this._fogOfWarRestoreDelay, this._fogOfWarRestoreDuration);
										}
									}
								}
							}
						}
					}
				}
				this.fogOfWarTexture.Apply();
			}
		}

		// Token: 0x06004C01 RID: 19457 RVA: 0x0019630C File Offset: 0x0019470C
		public void ResetFogOfWarAlpha(Vector3 worldPosition, float radius)
		{
			if (this.fogOfWarTexture == null)
			{
				return;
			}
			float num = (worldPosition.x - this._fogOfWarCenter.x) / this._fogOfWarSize.x + 0.5f;
			if (num < 0f || num > 1f)
			{
				return;
			}
			float num2 = (worldPosition.z - this._fogOfWarCenter.z) / this._fogOfWarSize.z + 0.5f;
			if (num2 < 0f || num2 > 1f)
			{
				return;
			}
			int width = this.fogOfWarTexture.width;
			int height = this.fogOfWarTexture.height;
			int num3 = (int)(num * (float)width);
			int num4 = (int)(num2 * (float)height);
			int num5 = num4 * width + num3;
			float num6 = radius / this._fogOfWarSize.z;
			int num7 = Mathf.FloorToInt((float)height * num6);
			for (int i = num4 - num7; i <= num4 + num7; i++)
			{
				if (i > 0 && i < height - 1)
				{
					for (int j = num3 - num7; j <= num3 + num7; j++)
					{
						if (j > 0 && j < width - 1)
						{
							int num8 = Mathf.FloorToInt(Mathf.Sqrt((float)((num4 - i) * (num4 - i) + (num3 - j) * (num3 - j))));
							if (num8 <= num7)
							{
								num5 = i * width + j;
								Color32 color = this.fogOfWarColorBuffer[num5];
								color.a = byte.MaxValue;
								this.fogOfWarColorBuffer[num5] = color;
								this.fogOfWarTexture.SetPixel(j, i, color);
							}
						}
					}
				}
				this.fogOfWarTexture.Apply();
			}
		}

		// Token: 0x06004C02 RID: 19458 RVA: 0x001964DC File Offset: 0x001948DC
		public void ResetFogOfWar()
		{
			if (this.fogOfWarTexture == null || !this.isPartOfScene)
			{
				return;
			}
			int height = this.fogOfWarTexture.height;
			int width = this.fogOfWarTexture.width;
			int num = height * width;
			if (this.fogOfWarColorBuffer == null || this.fogOfWarColorBuffer.Length != num)
			{
				this.fogOfWarColorBuffer = new Color32[num];
			}
			Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			for (int i = 0; i < num; i++)
			{
				this.fogOfWarColorBuffer[i] = color;
			}
			this.fogOfWarTexture.SetPixels32(this.fogOfWarColorBuffer);
			this.fogOfWarTexture.Apply();
			this.fowTransitionList.Clear();
			this.isDirty = true;
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06004C03 RID: 19459 RVA: 0x001965B9 File Offset: 0x001949B9
		// (set) Token: 0x06004C04 RID: 19460 RVA: 0x001965C4 File Offset: 0x001949C4
		public Color32[] fogOfWarTextureData
		{
			get
			{
				return this.fogOfWarColorBuffer;
			}
			set
			{
				this.fogOfWarEnabled = true;
				this.fogOfWarColorBuffer = value;
				if (value == null || this.fogOfWarTexture == null)
				{
					return;
				}
				if (value.Length != this.fogOfWarTexture.width * this.fogOfWarTexture.height)
				{
					return;
				}
				this.fogOfWarTexture.SetPixels32(this.fogOfWarColorBuffer);
				this.fogOfWarTexture.Apply();
			}
		}

		// Token: 0x06004C05 RID: 19461 RVA: 0x00196634 File Offset: 0x00194A34
		private void AddFowOfWarTransitionSlot(int x, int y, byte initialAlpha, byte targetAlpha, float delay, float duration)
		{
			int count = this.fowTransitionList.Count;
			VolumetricFog.FogOfWarTransition fogOfWarTransition = null;
			for (int i = 0; i < count; i++)
			{
				VolumetricFog.FogOfWarTransition fogOfWarTransition2 = this.fowTransitionList[i];
				if (fogOfWarTransition2.x == x && fogOfWarTransition2.y == y)
				{
					fogOfWarTransition = fogOfWarTransition2;
					break;
				}
				if (!fogOfWarTransition2.enabled)
				{
					fogOfWarTransition = fogOfWarTransition2;
				}
			}
			if (fogOfWarTransition == null)
			{
				fogOfWarTransition = new VolumetricFog.FogOfWarTransition();
				this.fowTransitionList.Add(fogOfWarTransition);
			}
			fogOfWarTransition.x = x;
			fogOfWarTransition.y = y;
			fogOfWarTransition.duration = duration;
			fogOfWarTransition.startTime = Time.time;
			fogOfWarTransition.startDelay = delay;
			fogOfWarTransition.initialAlpha = initialAlpha;
			fogOfWarTransition.targetAlpha = targetAlpha;
			fogOfWarTransition.enabled = true;
		}

		// Token: 0x040032FB RID: 13051
		public const string SKW_FOG_DISTANCE_ON = "FOG_DISTANCE_ON";

		// Token: 0x040032FC RID: 13052
		public const string SKW_LIGHT_SCATTERING = "FOG_SCATTERING_ON";

		// Token: 0x040032FD RID: 13053
		public const string SKW_FOG_AREA_BOX = "FOG_AREA_BOX";

		// Token: 0x040032FE RID: 13054
		public const string SKW_FOG_AREA_SPHERE = "FOG_AREA_SPHERE";

		// Token: 0x040032FF RID: 13055
		public const string SKW_FOG_VOID_BOX = "FOG_VOID_BOX";

		// Token: 0x04003300 RID: 13056
		public const string SKW_FOG_VOID_SPHERE = "FOG_VOID_SPHERE";

		// Token: 0x04003301 RID: 13057
		public const string SKW_FOG_HAZE_ON = "FOG_HAZE_ON";

		// Token: 0x04003302 RID: 13058
		public const string SKW_FOG_OF_WAR_ON = "FOG_OF_WAR_ON";

		// Token: 0x04003303 RID: 13059
		public const string SKW_FOG_BLUR = "FOG_BLUR_ON";

		// Token: 0x04003304 RID: 13060
		public const string SKW_SUN_SHADOWS = "FOG_SUN_SHADOWS_ON";

		// Token: 0x04003305 RID: 13061
		public const string SKW_FOG_USE_XY_PLANE = "FOG_USE_XY_PLANE";

		// Token: 0x04003306 RID: 13062
		public const string SKW_FOG_TRANSPARENT_BLENDPASS = "FOG_TRANSPARENCY_BLEND_PASS";

		// Token: 0x04003307 RID: 13063
		public const string SKW_FOG_COMPUTE_DEPTH = "FOG_COMPUTE_DEPTH";

		// Token: 0x04003308 RID: 13064
		private const float TIME_BETWEEN_TEXTURE_UPDATES = 0.2f;

		// Token: 0x04003309 RID: 13065
		private const string DEPTH_CAM_NAME = "VFMDepthCamera";

		// Token: 0x0400330A RID: 13066
		private const string DEPTH_SUN_CAM_NAME = "VFMDepthSunCamera";

		// Token: 0x0400330B RID: 13067
		private const string VFM_BUILD_HINT = "VFMBuildHint80";

		// Token: 0x0400330C RID: 13068
		private static VolumetricFog _fog;

		// Token: 0x0400330D RID: 13069
		[HideInInspector]
		public bool isDirty;

		// Token: 0x0400330E RID: 13070
		[SerializeField]
		private FOG_PRESET _preset = FOG_PRESET.Mist;

		// Token: 0x0400330F RID: 13071
		[SerializeField]
		private VolumetricFogProfile _profile;

		// Token: 0x04003310 RID: 13072
		[SerializeField]
		private bool _useFogVolumes;

		// Token: 0x04003311 RID: 13073
		[SerializeField]
		private bool _debugPass;

		// Token: 0x04003312 RID: 13074
		[SerializeField]
		private TRANSPARENT_MODE _transparencyBlendMode;

		// Token: 0x04003313 RID: 13075
		[SerializeField]
		private float _transparencyBlendPower = 1f;

		// Token: 0x04003314 RID: 13076
		[SerializeField]
		private LayerMask _transparencyLayerMask = -1;

		// Token: 0x04003315 RID: 13077
		[SerializeField]
		private LIGHTING_MODEL _lightingModel;

		// Token: 0x04003316 RID: 13078
		[SerializeField]
		private bool _computeDepth;

		// Token: 0x04003317 RID: 13079
		[SerializeField]
		private COMPUTE_DEPTH_SCOPE _computeDepthScope;

		// Token: 0x04003318 RID: 13080
		[SerializeField]
		private bool _renderBeforeTransparent;

		// Token: 0x04003319 RID: 13081
		[SerializeField]
		private GameObject _sun;

		// Token: 0x0400331A RID: 13082
		[SerializeField]
		private bool _sunCopyColor = true;

		// Token: 0x0400331B RID: 13083
		[SerializeField]
		private float _density = 1f;

		// Token: 0x0400331C RID: 13084
		[SerializeField]
		private float _noiseStrength = 0.8f;

		// Token: 0x0400331D RID: 13085
		[SerializeField]
		private float _noiseFinalMultiplier = 1f;

		// Token: 0x0400331E RID: 13086
		[SerializeField]
		private float _noiseSparse;

		// Token: 0x0400331F RID: 13087
		[SerializeField]
		private float _distance;

		// Token: 0x04003320 RID: 13088
		[SerializeField]
		private float _maxFogLength = 1000f;

		// Token: 0x04003321 RID: 13089
		[SerializeField]
		private float _maxFogLengthFallOff;

		// Token: 0x04003322 RID: 13090
		[SerializeField]
		private float _distanceFallOff;

		// Token: 0x04003323 RID: 13091
		[SerializeField]
		private float _height = 4f;

		// Token: 0x04003324 RID: 13092
		[SerializeField]
		private float _baselineHeight;

		// Token: 0x04003325 RID: 13093
		[SerializeField]
		private bool _baselineRelativeToCamera;

		// Token: 0x04003326 RID: 13094
		[SerializeField]
		private float _baselineRelativeToCameraDelay;

		// Token: 0x04003327 RID: 13095
		[SerializeField]
		private float _noiseScale = 1f;

		// Token: 0x04003328 RID: 13096
		[SerializeField]
		private float _alpha = 1f;

		// Token: 0x04003329 RID: 13097
		[SerializeField]
		private Color _color = new Color(0.89f, 0.89f, 0.89f, 1f);

		// Token: 0x0400332A RID: 13098
		[SerializeField]
		private Color _specularColor = new Color(1f, 1f, 0.8f, 1f);

		// Token: 0x0400332B RID: 13099
		[SerializeField]
		private float _specularThreshold = 0.6f;

		// Token: 0x0400332C RID: 13100
		[SerializeField]
		private float _specularIntensity = 0.2f;

		// Token: 0x0400332D RID: 13101
		[SerializeField]
		private Vector3 _lightDirection = new Vector3(1f, 0f, -1f);

		// Token: 0x0400332E RID: 13102
		[SerializeField]
		private float _lightIntensity = 0.2f;

		// Token: 0x0400332F RID: 13103
		[SerializeField]
		private Color _lightColor = Color.white;

		// Token: 0x04003330 RID: 13104
		[SerializeField]
		private int _updateTextureSpread = 1;

		// Token: 0x04003331 RID: 13105
		[SerializeField]
		private float _speed = 0.01f;

		// Token: 0x04003332 RID: 13106
		[SerializeField]
		private Vector3 _windDirection = new Vector3(-1f, 0f, 0f);

		// Token: 0x04003333 RID: 13107
		[SerializeField]
		private Color _skyColor = new Color(0.89f, 0.89f, 0.89f, 1f);

		// Token: 0x04003334 RID: 13108
		[SerializeField]
		private float _skyHaze = 50f;

		// Token: 0x04003335 RID: 13109
		[SerializeField]
		private float _skySpeed = 0.3f;

		// Token: 0x04003336 RID: 13110
		[SerializeField]
		private float _skyNoiseStrength = 0.1f;

		// Token: 0x04003337 RID: 13111
		[SerializeField]
		private float _skyAlpha = 1f;

		// Token: 0x04003338 RID: 13112
		[SerializeField]
		private float _skyDepth = 0.999f;

		// Token: 0x04003339 RID: 13113
		[SerializeField]
		private GameObject _character;

		// Token: 0x0400333A RID: 13114
		[SerializeField]
		private FOG_VOID_TOPOLOGY _fogVoidTopology;

		// Token: 0x0400333B RID: 13115
		[SerializeField]
		private float _fogVoidFallOff = 1f;

		// Token: 0x0400333C RID: 13116
		[SerializeField]
		private float _fogVoidRadius;

		// Token: 0x0400333D RID: 13117
		[SerializeField]
		private Vector3 _fogVoidPosition = Vector3.zero;

		// Token: 0x0400333E RID: 13118
		[SerializeField]
		private float _fogVoidDepth;

		// Token: 0x0400333F RID: 13119
		[SerializeField]
		private float _fogVoidHeight;

		// Token: 0x04003340 RID: 13120
		[SerializeField]
		private bool _fogVoidInverted;

		// Token: 0x04003341 RID: 13121
		[SerializeField]
		private GameObject _fogAreaCenter;

		// Token: 0x04003342 RID: 13122
		[SerializeField]
		private float _fogAreaFallOff = 1f;

		// Token: 0x04003343 RID: 13123
		[SerializeField]
		private FOG_AREA_FOLLOW_MODE _fogAreaFollowMode;

		// Token: 0x04003344 RID: 13124
		[SerializeField]
		private FOG_AREA_TOPOLOGY _fogAreaTopology = FOG_AREA_TOPOLOGY.Sphere;

		// Token: 0x04003345 RID: 13125
		[SerializeField]
		private float _fogAreaRadius;

		// Token: 0x04003346 RID: 13126
		[SerializeField]
		private Vector3 _fogAreaPosition = Vector3.zero;

		// Token: 0x04003347 RID: 13127
		[SerializeField]
		private float _fogAreaDepth;

		// Token: 0x04003348 RID: 13128
		[SerializeField]
		private float _fogAreaHeight;

		// Token: 0x04003349 RID: 13129
		[SerializeField]
		private FOG_AREA_SORTING_MODE _fogAreaSortingMode;

		// Token: 0x0400334A RID: 13130
		[SerializeField]
		private int _fogAreaRenderOrder = 1;

		// Token: 0x0400334B RID: 13131
		public const int MAX_POINT_LIGHTS = 6;

		// Token: 0x0400334C RID: 13132
		[SerializeField]
		private GameObject[] _pointLights = new GameObject[6];

		// Token: 0x0400334D RID: 13133
		[SerializeField]
		private float[] _pointLightRanges = new float[6];

		// Token: 0x0400334E RID: 13134
		[SerializeField]
		private float[] _pointLightIntensities = new float[]
		{
			1f,
			1f,
			1f,
			1f,
			1f,
			1f
		};

		// Token: 0x0400334F RID: 13135
		[SerializeField]
		private float[] _pointLightIntensitiesMultiplier = new float[]
		{
			1f,
			1f,
			1f,
			1f,
			1f,
			1f
		};

		// Token: 0x04003350 RID: 13136
		[SerializeField]
		private Vector3[] _pointLightPositions = new Vector3[6];

		// Token: 0x04003351 RID: 13137
		[SerializeField]
		private Color[] _pointLightColors = new Color[]
		{
			new Color(1f, 1f, 0f, 1f),
			new Color(1f, 1f, 0f, 1f),
			new Color(1f, 1f, 0f, 1f),
			new Color(1f, 1f, 0f, 1f),
			new Color(1f, 1f, 0f, 1f),
			new Color(1f, 1f, 0f, 1f)
		};

		// Token: 0x04003352 RID: 13138
		[SerializeField]
		private bool _pointLightTrackingAuto;

		// Token: 0x04003353 RID: 13139
		[SerializeField]
		private int _pointLightTrackingCount;

		// Token: 0x04003354 RID: 13140
		[SerializeField]
		private float _pointLightTrackingCheckInterval = 1f;

		// Token: 0x04003355 RID: 13141
		[SerializeField]
		private int _downsampling = 1;

		// Token: 0x04003356 RID: 13142
		[SerializeField]
		private bool _edgeImprove;

		// Token: 0x04003357 RID: 13143
		[SerializeField]
		private float _edgeThreshold = 0.0005f;

		// Token: 0x04003358 RID: 13144
		[SerializeField]
		private float _stepping = 12f;

		// Token: 0x04003359 RID: 13145
		[SerializeField]
		private float _steppingNear = 1f;

		// Token: 0x0400335A RID: 13146
		[SerializeField]
		private bool _dithering;

		// Token: 0x0400335B RID: 13147
		[SerializeField]
		private float _ditherStrength = 0.75f;

		// Token: 0x0400335C RID: 13148
		[SerializeField]
		private bool _lightScatteringEnabled;

		// Token: 0x0400335D RID: 13149
		[SerializeField]
		private float _lightScatteringDiffusion = 0.7f;

		// Token: 0x0400335E RID: 13150
		[SerializeField]
		private float _lightScatteringSpread = 0.686f;

		// Token: 0x0400335F RID: 13151
		[SerializeField]
		private int _lightScatteringSamples = 16;

		// Token: 0x04003360 RID: 13152
		[SerializeField]
		private float _lightScatteringWeight = 1.9f;

		// Token: 0x04003361 RID: 13153
		[SerializeField]
		private float _lightScatteringIllumination = 18f;

		// Token: 0x04003362 RID: 13154
		[SerializeField]
		private float _lightScatteringDecay = 0.986f;

		// Token: 0x04003363 RID: 13155
		[SerializeField]
		private float _lightScatteringExposure;

		// Token: 0x04003364 RID: 13156
		[SerializeField]
		private float _lightScatteringJittering = 0.5f;

		// Token: 0x04003365 RID: 13157
		[SerializeField]
		private bool _fogBlur;

		// Token: 0x04003366 RID: 13158
		[SerializeField]
		private float _fogBlurDepth = 0.05f;

		// Token: 0x04003367 RID: 13159
		[SerializeField]
		private bool _sunShadows;

		// Token: 0x04003368 RID: 13160
		[SerializeField]
		private LayerMask _sunShadowsLayerMask = -1;

		// Token: 0x04003369 RID: 13161
		[SerializeField]
		private float _sunShadowsStrength = 0.5f;

		// Token: 0x0400336A RID: 13162
		[SerializeField]
		private float _sunShadowsBias = 0.1f;

		// Token: 0x0400336B RID: 13163
		[SerializeField]
		private float _sunShadowsJitterStrength = 0.1f;

		// Token: 0x0400336C RID: 13164
		[SerializeField]
		private int _sunShadowsResolution = 2;

		// Token: 0x0400336D RID: 13165
		[SerializeField]
		private float _sunShadowsMaxDistance = 200f;

		// Token: 0x0400336E RID: 13166
		[SerializeField]
		private SUN_SHADOWS_BAKE_MODE _sunShadowsBakeMode = SUN_SHADOWS_BAKE_MODE.Discrete;

		// Token: 0x0400336F RID: 13167
		[SerializeField]
		private float _sunShadowsRefreshInterval;

		// Token: 0x04003370 RID: 13168
		[SerializeField]
		private float _sunShadowsCancellation;

		// Token: 0x04003371 RID: 13169
		[SerializeField]
		private float _turbulenceStrength;

		// Token: 0x04003372 RID: 13170
		[SerializeField]
		private bool _useXYPlane;

		// Token: 0x04003373 RID: 13171
		[SerializeField]
		private bool _useSinglePassStereoRenderingMatrix;

		// Token: 0x04003374 RID: 13172
		[SerializeField]
		private SPSR_BEHAVIOUR _spsrBehaviour;

		// Token: 0x04003375 RID: 13173
		[NonSerialized]
		public float distanceToCameraMin;

		// Token: 0x04003376 RID: 13174
		[NonSerialized]
		public float distanceToCameraMax;

		// Token: 0x04003377 RID: 13175
		[NonSerialized]
		public float distanceToCamera;

		// Token: 0x04003378 RID: 13176
		[NonSerialized]
		public float distanceToCameraYAxis;

		// Token: 0x04003379 RID: 13177
		public VolumetricFog fogRenderer;

		// Token: 0x0400337A RID: 13178
		private bool isPartOfScene;

		// Token: 0x0400337B RID: 13179
		private float initialFogAlpha;

		// Token: 0x0400337C RID: 13180
		private float targetFogAlpha;

		// Token: 0x0400337D RID: 13181
		private float initialSkyHazeAlpha;

		// Token: 0x0400337E RID: 13182
		private float targetSkyHazeAlpha;

		// Token: 0x0400337F RID: 13183
		private bool transitionAlpha;

		// Token: 0x04003380 RID: 13184
		private bool transitionColor;

		// Token: 0x04003381 RID: 13185
		private bool transitionSpecularColor;

		// Token: 0x04003382 RID: 13186
		private bool transitionLightColor;

		// Token: 0x04003383 RID: 13187
		private bool transitionProfile;

		// Token: 0x04003384 RID: 13188
		private bool targetColorActive;

		// Token: 0x04003385 RID: 13189
		private bool targetSpecularColorActive;

		// Token: 0x04003386 RID: 13190
		private bool targetLightColorActive;

		// Token: 0x04003387 RID: 13191
		private Color initialFogColor;

		// Token: 0x04003388 RID: 13192
		private Color targetFogColor;

		// Token: 0x04003389 RID: 13193
		private Color initialFogSpecularColor;

		// Token: 0x0400338A RID: 13194
		private Color targetFogSpecularColor;

		// Token: 0x0400338B RID: 13195
		private Color initialLightColor;

		// Token: 0x0400338C RID: 13196
		private Color targetLightColor;

		// Token: 0x0400338D RID: 13197
		private float transitionDuration;

		// Token: 0x0400338E RID: 13198
		private float transitionStartTime;

		// Token: 0x0400338F RID: 13199
		private float currentFogAlpha;

		// Token: 0x04003390 RID: 13200
		private float currentSkyHazeAlpha;

		// Token: 0x04003391 RID: 13201
		private Color currentFogColor;

		// Token: 0x04003392 RID: 13202
		private Color currentFogSpecularColor;

		// Token: 0x04003393 RID: 13203
		private Color currentLightColor;

		// Token: 0x04003394 RID: 13204
		private VolumetricFogProfile initialProfile;

		// Token: 0x04003395 RID: 13205
		private VolumetricFogProfile targetProfile;

		// Token: 0x04003396 RID: 13206
		private float oldBaselineRelativeCameraY;

		// Token: 0x04003397 RID: 13207
		private float currentFogAltitude;

		// Token: 0x04003398 RID: 13208
		private float skyHazeSpeedAcum;

		// Token: 0x04003399 RID: 13209
		private bool _hasCamera;

		// Token: 0x0400339A RID: 13210
		private Camera mainCamera;

		// Token: 0x0400339B RID: 13211
		private List<string> shaderKeywords;

		// Token: 0x0400339C RID: 13212
		private Material blurMat;

		// Token: 0x0400339D RID: 13213
		private RenderBuffer[] mrt;

		// Token: 0x0400339E RID: 13214
		private int _renderingInstancesCount;

		// Token: 0x0400339F RID: 13215
		private bool shouldUpdateMaterialProperties;

		// Token: 0x040033A0 RID: 13216
		[NonSerialized]
		public Material fogMat;

		// Token: 0x040033A1 RID: 13217
		private RenderTexture depthTexture;

		// Token: 0x040033A2 RID: 13218
		private RenderTexture depthSunTexture;

		// Token: 0x040033A3 RID: 13219
		private RenderTexture reducedDestination;

		// Token: 0x040033A4 RID: 13220
		private Light[] pointLightComponents = new Light[6];

		// Token: 0x040033A5 RID: 13221
		private Light[] lastFoundLights;

		// Token: 0x040033A6 RID: 13222
		private Light[] lightBuffer;

		// Token: 0x040033A7 RID: 13223
		private GameObject[] currentLights = new GameObject[6];

		// Token: 0x040033A8 RID: 13224
		private float trackPointAutoLastTime;

		// Token: 0x040033A9 RID: 13225
		private float trackPointCheckNewLightsLastTime;

		// Token: 0x040033AA RID: 13226
		private Shader depthShader;

		// Token: 0x040033AB RID: 13227
		private GameObject depthCamObj;

		// Token: 0x040033AC RID: 13228
		private Camera depthCam;

		// Token: 0x040033AD RID: 13229
		private float lastTextureUpdate;

		// Token: 0x040033AE RID: 13230
		private Vector3 windSpeedAcum;

		// Token: 0x040033AF RID: 13231
		private Texture2D adjustedTexture;

		// Token: 0x040033B0 RID: 13232
		private Color[] noiseColors;

		// Token: 0x040033B1 RID: 13233
		private Color[] adjustedColors;

		// Token: 0x040033B2 RID: 13234
		private float sunLightIntensity = 1f;

		// Token: 0x040033B3 RID: 13235
		private bool needUpdateTexture;

		// Token: 0x040033B4 RID: 13236
		private bool hasChangeAdjustedColorsAlpha;

		// Token: 0x040033B5 RID: 13237
		private int updatingTextureSlice;

		// Token: 0x040033B6 RID: 13238
		private Color updatingTextureLightColor;

		// Token: 0x040033B7 RID: 13239
		private Color lastRenderSettingsAmbientLight;

		// Token: 0x040033B8 RID: 13240
		private float lastRenderSettingsAmbientIntensity;

		// Token: 0x040033B9 RID: 13241
		private Light sunLight;

		// Token: 0x040033BA RID: 13242
		private Vector2 oldSunPos;

		// Token: 0x040033BB RID: 13243
		private float sunFade = 1f;

		// Token: 0x040033BC RID: 13244
		private GameObject depthSunCamObj;

		// Token: 0x040033BD RID: 13245
		private Camera depthSunCam;

		// Token: 0x040033BE RID: 13246
		private Shader depthSunShader;

		// Token: 0x040033BF RID: 13247
		private bool needUpdateDepthSunTexture;

		// Token: 0x040033C0 RID: 13248
		private float lastShadowUpdateFrame;

		// Token: 0x040033C1 RID: 13249
		private Texture2D adjustedChaosTexture;

		// Token: 0x040033C2 RID: 13250
		private Material chaosLerpMat;

		// Token: 0x040033C3 RID: 13251
		private float turbAcum;

		// Token: 0x040033C4 RID: 13252
		private List<VolumetricFog> fogInstances = new List<VolumetricFog>();

		// Token: 0x040033C5 RID: 13253
		private List<VolumetricFog> fogRenderInstances = new List<VolumetricFog>();

		// Token: 0x040033C6 RID: 13254
		private MeshRenderer mr;

		// Token: 0x040033C7 RID: 13255
		private float lastTimeSortInstances;

		// Token: 0x040033C8 RID: 13256
		private const float FOG_INSTANCES_SORT_INTERVAL = 2f;

		// Token: 0x040033C9 RID: 13257
		private Vector3 lastCamPos;

		// Token: 0x040033CA RID: 13258
		[SerializeField]
		private bool _fogOfWarEnabled;

		// Token: 0x040033CB RID: 13259
		[SerializeField]
		private Vector3 _fogOfWarCenter;

		// Token: 0x040033CC RID: 13260
		[SerializeField]
		private Vector3 _fogOfWarSize = new Vector3(1024f, 0f, 1024f);

		// Token: 0x040033CD RID: 13261
		[SerializeField]
		private int _fogOfWarTextureSize = 256;

		// Token: 0x040033CE RID: 13262
		[SerializeField]
		private float _fogOfWarRestoreDelay;

		// Token: 0x040033CF RID: 13263
		[SerializeField]
		private float _fogOfWarRestoreDuration = 2f;

		// Token: 0x040033D0 RID: 13264
		private Texture2D fogOfWarTexture;

		// Token: 0x040033D1 RID: 13265
		private Color32[] fogOfWarColorBuffer;

		// Token: 0x040033D2 RID: 13266
		private List<VolumetricFog.FogOfWarTransition> fowTransitionList;

		// Token: 0x020009BF RID: 2495
		private class FogOfWarTransition
		{
			// Token: 0x040033D4 RID: 13268
			public bool enabled;

			// Token: 0x040033D5 RID: 13269
			public int x;

			// Token: 0x040033D6 RID: 13270
			public int y;

			// Token: 0x040033D7 RID: 13271
			public float startTime;

			// Token: 0x040033D8 RID: 13272
			public float startDelay;

			// Token: 0x040033D9 RID: 13273
			public float duration;

			// Token: 0x040033DA RID: 13274
			public byte initialAlpha;

			// Token: 0x040033DB RID: 13275
			public byte targetAlpha;
		}
	}
}
