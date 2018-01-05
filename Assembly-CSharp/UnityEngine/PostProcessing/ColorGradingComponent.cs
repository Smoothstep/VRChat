using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020007D3 RID: 2003
	public sealed class ColorGradingComponent : PostProcessingComponentRenderTexture<ColorGradingModel>
	{
		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06004047 RID: 16455 RVA: 0x001437C3 File Offset: 0x00141BC3
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x001437E6 File Offset: 0x00141BE6
		private float StandardIlluminantY(float x)
		{
			return 2.87f * x - 3f * x * x - 0.275095075f;
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x00143800 File Offset: 0x00141C00
		private Vector3 CIExyToLMS(float x, float y)
		{
			float num = 1f;
			float num2 = num * x / y;
			float num3 = num * (1f - x - y) / y;
			float x2 = 0.7328f * num2 + 0.4296f * num - 0.1624f * num3;
			float y2 = -0.7036f * num2 + 1.6975f * num + 0.0061f * num3;
			float z = 0.003f * num2 + 0.0136f * num + 0.9834f * num3;
			return new Vector3(x2, y2, z);
		}

		// Token: 0x0600404A RID: 16458 RVA: 0x0014387C File Offset: 0x00141C7C
		private Vector3 CalculateColorBalance(float temperature, float tint)
		{
			float num = temperature / 55f;
			float num2 = tint / 55f;
			float x = 0.31271f - num * ((num >= 0f) ? 0.05f : 0.1f);
			float y = this.StandardIlluminantY(x) + num2 * 0.05f;
			Vector3 vector = new Vector3(0.949237f, 1.03542f, 1.08728f);
			Vector3 vector2 = this.CIExyToLMS(x, y);
			return new Vector3(vector.x / vector2.x, vector.y / vector2.y, vector.z / vector2.z);
		}

		// Token: 0x0600404B RID: 16459 RVA: 0x00143920 File Offset: 0x00141D20
		private static Color NormalizeColor(Color c)
		{
			float num = (c.r + c.g + c.b) / 3f;
			if (Mathf.Approximately(num, 0f))
			{
				return new Color(1f, 1f, 1f, c.a);
			}
			return new Color
			{
				r = c.r / num,
				g = c.g / num,
				b = c.b / num,
				a = c.a
			};
		}

		// Token: 0x0600404C RID: 16460 RVA: 0x001439BE File Offset: 0x00141DBE
		private static Vector3 ClampVector(Vector3 v, float min, float max)
		{
			return new Vector3(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max), Mathf.Clamp(v.z, min, max));
		}

		// Token: 0x0600404D RID: 16461 RVA: 0x001439F0 File Offset: 0x00141DF0
		public static Vector3 GetLiftValue(Color lift)
		{
			Color color = ColorGradingComponent.NormalizeColor(lift);
			float num = (color.r + color.g + color.b) / 3f;
			float x = (color.r - num) * 0.1f + lift.a;
			float y = (color.g - num) * 0.1f + lift.a;
			float z = (color.b - num) * 0.1f + lift.a;
			return ColorGradingComponent.ClampVector(new Vector3(x, y, z), -1f, 1f);
		}

		// Token: 0x0600404E RID: 16462 RVA: 0x00143A84 File Offset: 0x00141E84
		public static Vector3 GetGammaValue(Color gamma)
		{
			Color color = ColorGradingComponent.NormalizeColor(gamma);
			float num = (color.r + color.g + color.b) / 3f;
			gamma.a *= ((gamma.a >= 0f) ? 5f : 0.8f);
			float b = Mathf.Pow(2f, (color.r - num) * 0.5f) + gamma.a;
			float b2 = Mathf.Pow(2f, (color.g - num) * 0.5f) + gamma.a;
			float b3 = Mathf.Pow(2f, (color.b - num) * 0.5f) + gamma.a;
			float x = 1f / Mathf.Max(0.01f, b);
			float y = 1f / Mathf.Max(0.01f, b2);
			float z = 1f / Mathf.Max(0.01f, b3);
			return ColorGradingComponent.ClampVector(new Vector3(x, y, z), 0f, 5f);
		}

		// Token: 0x0600404F RID: 16463 RVA: 0x00143BA0 File Offset: 0x00141FA0
		public static Vector3 GetGainValue(Color gain)
		{
			Color color = ColorGradingComponent.NormalizeColor(gain);
			float num = (color.r + color.g + color.b) / 3f;
			gain.a *= ((gain.a <= 0f) ? 1f : 3f);
			float x = Mathf.Pow(2f, (color.r - num) * 0.5f) + gain.a;
			float y = Mathf.Pow(2f, (color.g - num) * 0.5f) + gain.a;
			float z = Mathf.Pow(2f, (color.b - num) * 0.5f) + gain.a;
			return ColorGradingComponent.ClampVector(new Vector3(x, y, z), 0f, 4f);
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x00143C7F File Offset: 0x0014207F
		public static void CalculateLiftGammaGain(Color lift, Color gamma, Color gain, out Vector3 outLift, out Vector3 outGamma, out Vector3 outGain)
		{
			outLift = ColorGradingComponent.GetLiftValue(lift);
			outGamma = ColorGradingComponent.GetGammaValue(gamma);
			outGain = ColorGradingComponent.GetGainValue(gain);
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x00143CA8 File Offset: 0x001420A8
		public static Vector3 GetSlopeValue(Color slope)
		{
			Color color = ColorGradingComponent.NormalizeColor(slope);
			float num = (color.r + color.g + color.b) / 3f;
			slope.a *= 0.5f;
			float x = (color.r - num) * 0.1f + slope.a + 1f;
			float y = (color.g - num) * 0.1f + slope.a + 1f;
			float z = (color.b - num) * 0.1f + slope.a + 1f;
			return ColorGradingComponent.ClampVector(new Vector3(x, y, z), 0f, 2f);
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x00143D60 File Offset: 0x00142160
		public static Vector3 GetPowerValue(Color power)
		{
			Color color = ColorGradingComponent.NormalizeColor(power);
			float num = (color.r + color.g + color.b) / 3f;
			power.a *= 0.5f;
			float b = (color.r - num) * 0.1f + power.a + 1f;
			float b2 = (color.g - num) * 0.1f + power.a + 1f;
			float b3 = (color.b - num) * 0.1f + power.a + 1f;
			float x = 1f / Mathf.Max(0.01f, b);
			float y = 1f / Mathf.Max(0.01f, b2);
			float z = 1f / Mathf.Max(0.01f, b3);
			return ColorGradingComponent.ClampVector(new Vector3(x, y, z), 0.5f, 2.5f);
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x00143E54 File Offset: 0x00142254
		public static Vector3 GetOffsetValue(Color offset)
		{
			Color color = ColorGradingComponent.NormalizeColor(offset);
			float num = (color.r + color.g + color.b) / 3f;
			offset.a *= 0.5f;
			float x = (color.r - num) * 0.05f + offset.a;
			float y = (color.g - num) * 0.05f + offset.a;
			float z = (color.b - num) * 0.05f + offset.a;
			return ColorGradingComponent.ClampVector(new Vector3(x, y, z), -0.8f, 0.8f);
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x00143EFA File Offset: 0x001422FA
		public static void CalculateSlopePowerOffset(Color slope, Color power, Color offset, out Vector3 outSlope, out Vector3 outPower, out Vector3 outOffset)
		{
			outSlope = ColorGradingComponent.GetSlopeValue(slope);
			outPower = ColorGradingComponent.GetPowerValue(power);
			outOffset = ColorGradingComponent.GetOffsetValue(offset);
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x00143F24 File Offset: 0x00142324
		private Texture2D GetCurveTexture()
		{
			if (this.m_GradingCurves == null)
			{
				this.m_GradingCurves = new Texture2D(128, 2, TextureFormat.RGBAHalf, false, true)
				{
					name = "Internal Curves Texture",
					hideFlags = HideFlags.DontSave,
					anisoLevel = 0,
					wrapMode = TextureWrapMode.Clamp,
					filterMode = FilterMode.Bilinear
				};
			}
			ColorGradingModel.CurvesSettings curves = base.model.settings.curves;
			curves.hueVShue.Cache();
			curves.hueVSsat.Cache();
			for (int i = 0; i < 128; i++)
			{
				float t = (float)i * 0.0078125f;
				float r = curves.hueVShue.Evaluate(t);
				float g = curves.hueVSsat.Evaluate(t);
				float b = curves.satVSsat.Evaluate(t);
				float a = curves.lumVSsat.Evaluate(t);
				this.m_pixels[i] = new Color(r, g, b, a);
				float a2 = curves.master.Evaluate(t);
				float r2 = curves.red.Evaluate(t);
				float g2 = curves.green.Evaluate(t);
				float b2 = curves.blue.Evaluate(t);
				this.m_pixels[i + 128] = new Color(r2, g2, b2, a2);
			}
			this.m_GradingCurves.SetPixels(this.m_pixels);
			this.m_GradingCurves.Apply(false, false);
			return this.m_GradingCurves;
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x001440B3 File Offset: 0x001424B3
		private bool IsLogLutValid(RenderTexture lut)
		{
			return lut != null && lut.IsCreated() && lut.height == 32;
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x001440DC File Offset: 0x001424DC
		private void GenerateLut()
		{
			ColorGradingModel.Settings settings = base.model.settings;
			if (!this.IsLogLutValid(base.model.bakedLut))
			{
				GraphicsUtils.Destroy(base.model.bakedLut);
				base.model.bakedLut = new RenderTexture(1024, 32, 0, RenderTextureFormat.ARGBHalf)
				{
					name = "Color Grading Log LUT",
					hideFlags = HideFlags.DontSave,
					filterMode = FilterMode.Bilinear,
					wrapMode = TextureWrapMode.Clamp,
					anisoLevel = 0
				};
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Lut Generator");
			material.SetVector(ColorGradingComponent.Uniforms._LutParams, new Vector4(32f, 0.00048828125f, 0.015625f, 1.032258f));
			material.shaderKeywords = null;
			ColorGradingModel.TonemappingSettings tonemapping = settings.tonemapping;
			ColorGradingModel.Tonemapper tonemapper = tonemapping.tonemapper;
			if (tonemapper != ColorGradingModel.Tonemapper.Neutral)
			{
				if (tonemapper == ColorGradingModel.Tonemapper.ACES)
				{
					material.EnableKeyword("TONEMAPPING_FILMIC");
				}
			}
			else
			{
				material.EnableKeyword("TONEMAPPING_NEUTRAL");
				float num = tonemapping.neutralBlackIn * 20f + 1f;
				float num2 = tonemapping.neutralBlackOut * 10f + 1f;
				float num3 = tonemapping.neutralWhiteIn / 20f;
				float num4 = 1f - tonemapping.neutralWhiteOut / 20f;
				float t = num / num2;
				float t2 = num3 / num4;
				float y = Mathf.Max(0f, Mathf.LerpUnclamped(0.57f, 0.37f, t));
				float z = Mathf.LerpUnclamped(0.01f, 0.24f, t2);
				float w = Mathf.Max(0f, Mathf.LerpUnclamped(0.02f, 0.2f, t));
				material.SetVector(ColorGradingComponent.Uniforms._NeutralTonemapperParams1, new Vector4(0.2f, y, z, w));
				material.SetVector(ColorGradingComponent.Uniforms._NeutralTonemapperParams2, new Vector4(0.02f, 0.3f, tonemapping.neutralWhiteLevel, tonemapping.neutralWhiteClip / 10f));
			}
			material.SetFloat(ColorGradingComponent.Uniforms._HueShift, settings.basic.hueShift / 360f);
			material.SetFloat(ColorGradingComponent.Uniforms._Saturation, settings.basic.saturation);
			material.SetFloat(ColorGradingComponent.Uniforms._Contrast, settings.basic.contrast);
			material.SetVector(ColorGradingComponent.Uniforms._Balance, this.CalculateColorBalance(settings.basic.temperature, settings.basic.tint));
			Vector3 v;
			Vector3 v2;
			Vector3 v3;
			ColorGradingComponent.CalculateLiftGammaGain(settings.colorWheels.linear.lift, settings.colorWheels.linear.gamma, settings.colorWheels.linear.gain, out v, out v2, out v3);
			material.SetVector(ColorGradingComponent.Uniforms._Lift, v);
			material.SetVector(ColorGradingComponent.Uniforms._InvGamma, v2);
			material.SetVector(ColorGradingComponent.Uniforms._Gain, v3);
			Vector3 v4;
			Vector3 v5;
			Vector3 v6;
			ColorGradingComponent.CalculateSlopePowerOffset(settings.colorWheels.log.slope, settings.colorWheels.log.power, settings.colorWheels.log.offset, out v4, out v5, out v6);
			material.SetVector(ColorGradingComponent.Uniforms._Slope, v4);
			material.SetVector(ColorGradingComponent.Uniforms._Power, v5);
			material.SetVector(ColorGradingComponent.Uniforms._Offset, v6);
			material.SetVector(ColorGradingComponent.Uniforms._ChannelMixerRed, settings.channelMixer.red);
			material.SetVector(ColorGradingComponent.Uniforms._ChannelMixerGreen, settings.channelMixer.green);
			material.SetVector(ColorGradingComponent.Uniforms._ChannelMixerBlue, settings.channelMixer.blue);
			material.SetTexture(ColorGradingComponent.Uniforms._Curves, this.GetCurveTexture());
			Graphics.Blit(null, base.model.bakedLut, material, 0);
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x001444B0 File Offset: 0x001428B0
		public override void Prepare(Material uberMaterial)
		{
			if (base.model.isDirty || !this.IsLogLutValid(base.model.bakedLut))
			{
				this.GenerateLut();
				base.model.isDirty = false;
			}
			uberMaterial.EnableKeyword((!this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.PreGradingLog)) ? "COLOR_GRADING" : "COLOR_GRADING_LOG_VIEW");
			RenderTexture bakedLut = base.model.bakedLut;
			uberMaterial.SetTexture(ColorGradingComponent.Uniforms._LogLut, bakedLut);
			uberMaterial.SetVector(ColorGradingComponent.Uniforms._LogLut_Params, new Vector3(1f / (float)bakedLut.width, 1f / (float)bakedLut.height, (float)bakedLut.height - 1f));
			float value = Mathf.Exp(base.model.settings.basic.postExposure * 0.6931472f);
			uberMaterial.SetFloat(ColorGradingComponent.Uniforms._ExposureEV, value);
		}

		// Token: 0x06004059 RID: 16473 RVA: 0x001445AC File Offset: 0x001429AC
		public void OnGUI()
		{
			RenderTexture bakedLut = base.model.bakedLut;
			Rect position = new Rect(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)bakedLut.width, (float)bakedLut.height);
			GUI.DrawTexture(position, bakedLut);
		}

		// Token: 0x0600405A RID: 16474 RVA: 0x00144606 File Offset: 0x00142A06
		public override void OnDisable()
		{
			GraphicsUtils.Destroy(this.m_GradingCurves);
			GraphicsUtils.Destroy(base.model.bakedLut);
			this.m_GradingCurves = null;
			base.model.bakedLut = null;
		}

		// Token: 0x040028A0 RID: 10400
		private const int k_InternalLogLutSize = 32;

		// Token: 0x040028A1 RID: 10401
		private const int k_CurvePrecision = 128;

		// Token: 0x040028A2 RID: 10402
		private const float k_CurveStep = 0.0078125f;

		// Token: 0x040028A3 RID: 10403
		private Texture2D m_GradingCurves;

		// Token: 0x040028A4 RID: 10404
		private Color[] m_pixels = new Color[256];

		// Token: 0x020007D4 RID: 2004
		private static class Uniforms
		{
			// Token: 0x040028A5 RID: 10405
			internal static readonly int _LutParams = Shader.PropertyToID("_LutParams");

			// Token: 0x040028A6 RID: 10406
			internal static readonly int _NeutralTonemapperParams1 = Shader.PropertyToID("_NeutralTonemapperParams1");

			// Token: 0x040028A7 RID: 10407
			internal static readonly int _NeutralTonemapperParams2 = Shader.PropertyToID("_NeutralTonemapperParams2");

			// Token: 0x040028A8 RID: 10408
			internal static readonly int _HueShift = Shader.PropertyToID("_HueShift");

			// Token: 0x040028A9 RID: 10409
			internal static readonly int _Saturation = Shader.PropertyToID("_Saturation");

			// Token: 0x040028AA RID: 10410
			internal static readonly int _Contrast = Shader.PropertyToID("_Contrast");

			// Token: 0x040028AB RID: 10411
			internal static readonly int _Balance = Shader.PropertyToID("_Balance");

			// Token: 0x040028AC RID: 10412
			internal static readonly int _Lift = Shader.PropertyToID("_Lift");

			// Token: 0x040028AD RID: 10413
			internal static readonly int _InvGamma = Shader.PropertyToID("_InvGamma");

			// Token: 0x040028AE RID: 10414
			internal static readonly int _Gain = Shader.PropertyToID("_Gain");

			// Token: 0x040028AF RID: 10415
			internal static readonly int _Slope = Shader.PropertyToID("_Slope");

			// Token: 0x040028B0 RID: 10416
			internal static readonly int _Power = Shader.PropertyToID("_Power");

			// Token: 0x040028B1 RID: 10417
			internal static readonly int _Offset = Shader.PropertyToID("_Offset");

			// Token: 0x040028B2 RID: 10418
			internal static readonly int _ChannelMixerRed = Shader.PropertyToID("_ChannelMixerRed");

			// Token: 0x040028B3 RID: 10419
			internal static readonly int _ChannelMixerGreen = Shader.PropertyToID("_ChannelMixerGreen");

			// Token: 0x040028B4 RID: 10420
			internal static readonly int _ChannelMixerBlue = Shader.PropertyToID("_ChannelMixerBlue");

			// Token: 0x040028B5 RID: 10421
			internal static readonly int _Curves = Shader.PropertyToID("_Curves");

			// Token: 0x040028B6 RID: 10422
			internal static readonly int _LogLut = Shader.PropertyToID("_LogLut");

			// Token: 0x040028B7 RID: 10423
			internal static readonly int _LogLut_Params = Shader.PropertyToID("_LogLut_Params");

			// Token: 0x040028B8 RID: 10424
			internal static readonly int _ExposureEV = Shader.PropertyToID("_ExposureEV");
		}
	}
}
