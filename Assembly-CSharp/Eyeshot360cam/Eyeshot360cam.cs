using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using BestHTTP.JSON;
using Eyeshot360cam.Internals;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

namespace Eyeshot360cam
{
	// Token: 0x02000463 RID: 1123
	public class Eyeshot360cam : MonoBehaviour
	{
		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06002726 RID: 10022 RVA: 0x000C16D0 File Offset: 0x000BFAD0
		public static bool IsCapturing
		{
			get
			{
				return Eyeshot360cam.capturing;
			}
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x000C16D7 File Offset: 0x000BFAD7
		protected void UpdateStatus(string message, UnityEngine.Color color)
		{
			if (this.status != null)
			{
				this.status.text = message;
				this.status.color = color;
			}
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x000C1704 File Offset: 0x000BFB04
		protected bool IsConfigurationChanged(Eyeshot360cam.Configuration config)
		{
			return config.captureStereoscopic != this.captureStereoscopic || config.panoramaWidth != this.panoramaWidth || config.interpupillaryDistance != this.interpupillaryDistance || config.numCirclePoints != this.numCirclePoints || config.ssaaFactor != this.ssaaFactor || config.antiAliasing != this.antiAliasing || config.saveCubemap != this.saveCubemap || config.useGpuTransform != this.useGPUTransform;
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x000C17A4 File Offset: 0x000BFBA4
		private System.Drawing.Imaging.ImageFormat FormatToDrawingFormat(Eyeshot360cam.ImageFormat format)
		{
			switch (format)
			{
			case Eyeshot360cam.ImageFormat.PNG:
				return System.Drawing.Imaging.ImageFormat.Png;
			case Eyeshot360cam.ImageFormat.JPEG:
				return System.Drawing.Imaging.ImageFormat.Jpeg;
			case Eyeshot360cam.ImageFormat.BMP:
				return System.Drawing.Imaging.ImageFormat.Bmp;
			default:
				return System.Drawing.Imaging.ImageFormat.Png;
			}
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x000C17D4 File Offset: 0x000BFBD4
		private string FormatMimeType(Eyeshot360cam.ImageFormat format)
		{
			switch (format)
			{
			case Eyeshot360cam.ImageFormat.PNG:
				return "image/png";
			case Eyeshot360cam.ImageFormat.JPEG:
				return "image/jpeg";
			case Eyeshot360cam.ImageFormat.BMP:
				return "image/bmp";
			default:
				return string.Empty;
			}
		}

		// Token: 0x0600272B RID: 10027 RVA: 0x000C1804 File Offset: 0x000BFC04
		private string FormatToExtension(Eyeshot360cam.ImageFormat format)
		{
			switch (format)
			{
			case Eyeshot360cam.ImageFormat.PNG:
				return "png";
			case Eyeshot360cam.ImageFormat.JPEG:
				return "jpg";
			case Eyeshot360cam.ImageFormat.BMP:
				return "bmp";
			default:
				return string.Empty;
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x0600272C RID: 10028 RVA: 0x000C1834 File Offset: 0x000BFC34
		protected bool CaptureIsLocked
		{
			get
			{
				return this.lastTimeOfCapturingDone != null && Time.realtimeSinceStartup - this.lastTimeOfCapturingDone.Value < this.lockTimeDelayAfterCaptureProcessingDone;
			}
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x000C1861 File Offset: 0x000BFC61
		public void Awake()
		{
			if (Eyeshot360cam.instance == null)
			{
				Eyeshot360cam.instance = this;
			}
			else
			{
				Debug.LogError("More than one CapturePanorama instance detected.");
			}
		}

		// Token: 0x0600272E RID: 10030 RVA: 0x000C1888 File Offset: 0x000BFC88
		public virtual void Start()
		{
			this.audioSource = base.gameObject.AddComponent<AudioSource>();
			this.audioSource.spatialBlend = 0f;
			this.audioSource.Play();
			this.Reinitialize();
		}

		// Token: 0x0600272F RID: 10031 RVA: 0x000C18BC File Offset: 0x000BFCBC
		private float IpdScaleFunction(float latitudeNormalized)
		{
			return 1.58197665f * Mathf.Exp(-latitudeNormalized * latitudeNormalized) - 0.5819767f;
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x000C18D3 File Offset: 0x000BFCD3
		public void OnDestroy()
		{
			this.Cleanup();
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x000C18DC File Offset: 0x000BFCDC
		private void Cleanup()
		{
			this.faces = null;
			UnityEngine.Object.Destroy(this.copyCameraScript);
			UnityEngine.Object.Destroy(this.renderCamera);
			if (this.camGos != null)
			{
				for (int i = this.camGos.Length - 1; i >= 0; i--)
				{
					if (this.camGos[i] != null)
					{
						UnityEngine.Object.Destroy(this.camGos[i]);
					}
				}
			}
			this.camGos = null;
			this.numCameras = -1;
			this.hFov = (this.vFov = -1f);
			if (this.cubemapRenderTexture != null)
			{
				UnityEngine.Object.Destroy(this.cubemapRenderTexture);
			}
			this.cubemapRenderTexture = null;
			if (this.cubemapDepthRenderTexture != null)
			{
				UnityEngine.Object.Destroy(this.cubemapDepthRenderTexture);
			}
			this.cubemapDepthRenderTexture = null;
			this.convertPanoramaKernelIdx = (this.renderStereoIdx = (this.textureToBufferIdx = -1));
			this.convertPanoramaKernelIdxs = null;
			this.resultPixels = (this.cameraPixels = null);
			if (this.forceWaitTexture != null)
			{
				UnityEngine.Object.Destroy(this.forceWaitTexture);
			}
			this.forceWaitTexture = new Texture2D(1, 1);
		}

		// Token: 0x06002732 RID: 10034 RVA: 0x000C1A10 File Offset: 0x000BFE10
		protected void Reinitialize()
		{
			try
			{
				this.ReinitializeBody();
			}
			catch (Exception)
			{
				this.Cleanup();
				throw;
			}
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x000C1A44 File Offset: 0x000BFE44
		private void ReinitializeBody()
		{
			this.Log("Settings changed, calling Reinitialize()");
			this.initializeFailed = true;
			if (!SystemInfo.supportsComputeShaders)
			{
				Debug.LogWarning("CapturePanorama requires compute shaders. Your system does not support them. On PC, compute shaders require DirectX 11, Windows Vista or later, and a GPU capable of Shader Model 5.0.");
				return;
			}
			this.lastConfig.captureStereoscopic = this.captureStereoscopic;
			this.lastConfig.panoramaWidth = this.panoramaWidth;
			this.lastConfig.interpupillaryDistance = this.interpupillaryDistance;
			this.lastConfig.numCirclePoints = this.numCirclePoints;
			this.lastConfig.ssaaFactor = this.ssaaFactor;
			this.lastConfig.antiAliasing = this.antiAliasing;
			this.lastConfig.saveCubemap = this.saveCubemap;
			this.lastConfig.useGpuTransform = this.useGPUTransform;
			this.Cleanup();
            this.faces = new CubemapFace[6]
            {
              CubemapFace.PositiveX,
              CubemapFace.NegativeX,
              CubemapFace.PositiveY,
              CubemapFace.NegativeY,
              CubemapFace.PositiveZ,
              CubemapFace.NegativeZ
            };
			for (int i = 0; i < this.faces.Length; i++)
			{
			}
			this.panoramaHeight = this.panoramaWidth / 2;
			this.camGos = new GameObject[3];
			for (int j = 0; j < 3; j++)
			{
				this.camGos[j] = new GameObject("PanoramaCaptureCamera" + j);
				this.camGos[j].hideFlags = HideFlags.DontSave;
				if (j > 0)
				{
					this.camGos[j].transform.parent = this.camGos[j - 1].transform;
				}
			}
			this.camGos[0].name = "OriginalPoint";
			this.camGos[1].name = "RelativeEyePoint";
			this.camGos[2].name = "RenderCamera";
			this.renderCamera = this.camGos[2].AddComponent<Camera>();
			this.renderCamera.enabled = false;
			this.renderDepth = this.camGos[2].AddComponent<RenderDepth>();
			this.renderDepth.enabled = false;
			this.copyCameraScript = this.camGos[2].AddComponent<ImageEffectCopyCamera>();
			this.copyCameraScript.enabled = false;
			this.numCameras = this.faces.Length;
			this.hFov = (this.vFov = 90f);
			if (this.captureStereoscopic)
			{
				float num = 360f / (float)this.numCirclePoints;
				float num2 = 0.001f;
				float b = 2f * (1.57079637f - Mathf.Acos(this.IpdScaleFunction(0.5f))) * 360f / 6.28318548f;
				this.hFov = Mathf.Max(90f + num, b) + num2;
				this.vFov = 90f;
				this.numCameras = 2 + this.numCirclePoints * 4;
				this.circleRadius = this.interpupillaryDistance / 2f;
				this.hFovAdjustDegrees = this.hFov / 2f;
				this.vFovAdjustDegrees = this.vFov / 2f;
			}
			double num3 = (double)this.panoramaWidth * 90.0 / 360.0;
			this.cameraWidth = (int)Math.Ceiling(Math.Tan((double)(this.hFov * 6.28318548f / 360f / 2f)) * num3 * (double)this.ssaaFactor);
			this.cameraHeight = (int)Math.Ceiling(Math.Tan((double)(this.vFov * 6.28318548f / 360f / 2f)) * num3 * (double)this.ssaaFactor);
			this.Log("Number of cameras: " + this.numCameras);
			this.Log(string.Concat(new object[]
			{
				"Camera dimensions: ",
				this.cameraWidth,
				"x",
				this.cameraHeight
			}));
			this.usingGpuTransform = (this.useGPUTransform && this.convertPanoramaShader != null && this.panoramaFormat != Eyeshot360cam.PanoramaFormat.CubeUnwrap);
			this.cubemapRenderTexture = new RenderTexture(this.cameraWidth, this.cameraHeight, 24, RenderTextureFormat.ARGB32);
			this.cubemapRenderTexture.antiAliasing = (int)this.antiAliasing;
			this.cubemapRenderTexture.Create();
			this.cubemapDepthRenderTexture = new RenderTexture(this.cameraWidth, this.cameraHeight, 24, RenderTextureFormat.ARGB32);
			this.cubemapDepthRenderTexture.Create();
			if (this.usingGpuTransform)
			{
				this.convertPanoramaKernelIdx = this.convertPanoramaShader.FindKernel("CubeMapToEquirectangular");
				this.convertPanoramaYPositiveKernelIdx = this.convertPanoramaShader.FindKernel("CubeMapToEquirectangularPositiveY");
				this.convertPanoramaYNegativeKernelIdx = this.convertPanoramaShader.FindKernel("CubeMapToEquirectangularNegativeY");
				this.convertPanoramaKernelIdxs = new int[]
				{
					this.convertPanoramaKernelIdx,
					this.convertPanoramaYPositiveKernelIdx,
					this.convertPanoramaYNegativeKernelIdx
				};
				this.convertPanoramaShader.SetInt("equirectangularWidth", this.panoramaWidth);
				this.convertPanoramaShader.SetInt("equirectangularHeight", this.panoramaHeight);
				this.convertPanoramaShader.SetInt("ssaaFactor", this.ssaaFactor);
				this.convertPanoramaShader.SetInt("cameraWidth", this.cameraWidth);
				this.convertPanoramaShader.SetInt("cameraHeight", this.cameraHeight);
				int num4 = (this.panoramaHeight + 8 - 1) / 8;
				int num5 = this.panoramaWidth;
				int num6 = (!this.captureStereoscopic) ? num4 : (2 * this.panoramaHeight);
				try
				{
					this.resultPixels = new uint[num5 * num6 + 1];
				}
				catch (Exception ex)
				{
					Debug.LogError("Error capture control intializatioi: " + ex.Message);
					this.UpdateStatus(string.Concat(new object[]
					{
						"Error capture control intializatioi: ",
						ex.Message,
						". Can't allocate memory for ",
						(!this.captureStereoscopic) ? string.Empty : "stereoscopic ",
						"bitmap with width ",
						this.panoramaWidth
					}), UnityEngine.Color.red);
					return;
				}
			}
			this.textureToBufferIdx = this.textureToBufferShader.FindKernel("TextureToBuffer");
			this.textureToBufferShader.SetInt("width", this.cameraWidth);
			this.textureToBufferShader.SetInt("height", this.cameraHeight);
			this.textureToBufferShader.SetFloat("gamma", (QualitySettings.activeColorSpace != ColorSpace.Linear) ? 1f : 0.454545438f);
			this.renderStereoIdx = this.convertPanoramaStereoShader.FindKernel("RenderStereo");
			if ((this.saveCubemap || !this.usingGpuTransform) && (this.cameraPixels == null || this.cameraPixels.Length != this.numCameras * this.cameraWidth * this.cameraHeight))
			{
				this.cameraPixels = new uint[this.numCameras * this.cameraWidth * this.cameraHeight + 1];
			}
			this.tanHalfHFov = Mathf.Tan(this.hFov * 6.28318548f / 360f / 2f);
			this.tanHalfVFov = Mathf.Tan(this.vFov * 6.28318548f / 360f / 2f);
			this.hFovAdjust = this.hFovAdjustDegrees * 6.28318548f / 360f;
			this.vFovAdjust = this.vFovAdjustDegrees * 6.28318548f / 360f;
			if (this.captureStereoscopic && this.usingGpuTransform)
			{
				this.convertPanoramaStereoShader.SetFloat("tanHalfHFov", this.tanHalfHFov);
				this.convertPanoramaStereoShader.SetFloat("tanHalfVFov", this.tanHalfVFov);
				this.convertPanoramaStereoShader.SetFloat("hFovAdjust", this.hFovAdjust);
				this.convertPanoramaStereoShader.SetFloat("vFovAdjust", this.vFovAdjust);
				this.convertPanoramaStereoShader.SetFloat("interpupillaryDistance", this.interpupillaryDistance);
				this.convertPanoramaStereoShader.SetFloat("circleRadius", this.circleRadius);
				this.convertPanoramaStereoShader.SetInt("numCirclePoints", this.numCirclePoints);
				this.convertPanoramaStereoShader.SetInt("equirectangularWidth", this.panoramaWidth);
				this.convertPanoramaStereoShader.SetInt("equirectangularHeight", this.panoramaHeight);
				this.convertPanoramaStereoShader.SetInt("cameraWidth", this.cameraWidth);
				this.convertPanoramaStereoShader.SetInt("cameraHeight", this.cameraHeight);
				this.convertPanoramaStereoShader.SetInt("ssaaFactor", this.ssaaFactor);
			}
			this.initializeFailed = false;
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x000C22C8 File Offset: 0x000C06C8
		protected void Log(string s)
		{
			if (this.enableDebugging)
			{
				Debug.Log(s, this);
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06002735 RID: 10037 RVA: 0x000C22DC File Offset: 0x000C06DC
		private bool IsDefaultCapturePressed
		{
			get
			{
				return this.defaultCaptureKey != KeyCode.None && Input.GetKeyDown(this.defaultCaptureKey);
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06002736 RID: 10038 RVA: 0x000C22FC File Offset: 0x000C06FC
		private bool IsSelfieCapturePressed
		{
			get
			{
				return this.defaultCaptureKey != KeyCode.None && Input.GetKeyDown(this.selfieCaptureKey);
			}
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x000C231C File Offset: 0x000C071C
		public void Update()
		{
			bool isDefaultCapturePressed = this.IsDefaultCapturePressed;
			bool flag = false;
			if (!isDefaultCapturePressed)
			{
				flag = this.IsSelfieCapturePressed;
			}
			if (isDefaultCapturePressed)
			{
				Debug.LogWarning("Pressed default capture");
			}
			if (flag)
			{
				Debug.LogWarning("Pressed selfie capture");
			}
			if (this.CaptureIsLocked)
			{
				return;
			}
			if (this.initializeFailed || this.panoramaWidth < 4 || (this.captureStereoscopic && this.numCirclePoints < 8))
			{
				if (isDefaultCapturePressed)
				{
					if (this.panoramaWidth < 4)
					{
						this.UpdateStatus("Panorama Width must be at least 4. No panorama captured.", UnityEngine.Color.red);
						Debug.LogError("Panorama Width must be at least 4. No panorama captured.");
					}
					if (this.captureStereoscopic && this.numCirclePoints < 8)
					{
						this.UpdateStatus("Num Circle Points must be at least 8. No panorama captured.", UnityEngine.Color.red);
						Debug.LogError("Num Circle Points must be at least 8. No panorama captured.");
					}
					if (this.initializeFailed)
					{
						this.UpdateStatus("Initialization of Capture Panorama Control failed. Cannot capture content.", UnityEngine.Color.red);
						Debug.LogError("Initialization of Capture Panorama Control failed. Cannot capture content.");
					}
					this.PlaySound(this.failSound);
				}
				return;
			}
			if (this.IsConfigurationChanged(this.lastConfig))
			{
				this.Reinitialize();
			}
			if (this.capturingEveryFrame)
			{
				if (isDefaultCapturePressed || flag || (this.maxFramesToRecord > 0 && this.frameNumber >= this.maxFramesToRecord))
				{
					this.StopCaptureEveryFrame();
				}
				else
				{
					this.CaptureScreenshotSync(this.videoBaseName + "_" + this.frameNumber.ToString(new string('0', this.frameNumberDigits)));
					this.frameNumber++;
				}
			}
			else
			{
				if (isDefaultCapturePressed && !Eyeshot360cam.capturing)
				{
					if (this.captureEveryFrame)
					{
						this.StartCaptureEveryFrame();
					}
					else
					{
						this.UpdateStatus("Capturing...", UnityEngine.Color.cyan);
						string text = string.Format("{0}_{1:yyyy-MM-dd_HH-mm-ss-fff}", this.panoramaName, DateTime.Now);
						this.Log("Panorama capture key pressed, capturing " + text);
						this.CaptureScreenshotAsync(text, null);
					}
				}
				else if (isDefaultCapturePressed && Eyeshot360cam.capturing)
				{
					this.PlaySound(this.failSound);
				}
				if (flag && !Eyeshot360cam.capturing)
				{
					this.UpdateStatus("Capturing...", UnityEngine.Color.cyan);
					List<Transform> list = new List<Transform>();
					if (this.selfieCamera != null)
					{
						list.Add(this.selfieCamera);
					}
					foreach (Transform transform in this.additionalCameras)
					{
						if (transform != null)
						{
							list.Add(transform);
						}
					}
					if (list.Count == 0)
					{
						this.PlaySound(this.failSound);
						Debug.LogWarning("Selfie camera is NULL!!!");
					}
					else
					{
						string text2 = string.Format("{0}_{1:yyyy-MM-dd_HH-mm-ss-fff}", this.panoramaName, DateTime.Now);
						this.Log("Selfie panorama capture key pressed, capturing " + text2);
						this.CaptureScreenshotAsync(text2, list.ToArray());
					}
				}
				else if (flag && Eyeshot360cam.capturing)
				{
					this.PlaySound(this.failSound);
				}
			}
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x000C264D File Offset: 0x000C0A4D
		public void StartCaptureEveryFrame()
		{
			Time.captureFramerate = this.frameRate;
			this.videoBaseName = string.Format("{0}_{1:yyyy-MM-dd_HH-mm-ss-fff}", this.panoramaName, DateTime.Now);
			this.frameNumber = 0;
			this.capturingEveryFrame = true;
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x000C2688 File Offset: 0x000C0A88
		public void StopCaptureEveryFrame()
		{
			Time.captureFramerate = 0;
			this.capturingEveryFrame = false;
			this.lastTimeOfCapturingDone = new float?(Time.realtimeSinceStartup);
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x000C26A7 File Offset: 0x000C0AA7
		public void CaptureScreenshotSync(string filenameBase)
		{
			base.StartCoroutine(this.CaptureScreenshotSyncHelper(filenameBase));
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x000C26B8 File Offset: 0x000C0AB8
		public IEnumerator CaptureScreenshotSyncHelper(string filenameBase)
		{
			yield return new WaitForEndOfFrame();
			IEnumerator enumerator = this.CaptureScreenshotAsyncHelper(filenameBase, false, null);
			while (enumerator.MoveNext())
			{
			}
			yield break;
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x000C26DA File Offset: 0x000C0ADA
		public void CaptureScreenshotAsync(string filenameBase, Transform[] pivots = null)
		{
			base.StartCoroutine(this.CaptureScreenshotAsyncHelper(filenameBase, true, pivots));
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x000C26EC File Offset: 0x000C0AEC
		private void SetFadersEnabled(IEnumerable<ScreenFadeControl> fadeControls, bool value)
		{
			foreach (ScreenFadeControl screenFadeControl in fadeControls)
			{
				screenFadeControl.enabled = value;
			}
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x000C2740 File Offset: 0x000C0B40
		public IEnumerator FadeOut(IEnumerable<ScreenFadeControl> fadeControls)
		{
			this.Log("Doing fade out");
			float elapsedTime = 0f;
			UnityEngine.Color color = this.fadeColor;
			color.a = 0f;
			this.fadeMaterial.color = color;
			this.SetFadersEnabled(fadeControls, true);
			while (elapsedTime < this.fadeTime)
			{
				yield return new WaitForEndOfFrame();
				elapsedTime += Time.deltaTime;
				color.a = Mathf.Lerp(0f, this.fadeColor.a, Mathf.Clamp01(elapsedTime / this.fadeTime));
				this.fadeMaterial.color = color;
			}
			yield break;
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x000C2764 File Offset: 0x000C0B64
		public IEnumerator FadeIn(IEnumerable<ScreenFadeControl> fadeControls)
		{
			this.Log("Fading back in");
			float elapsedTime = 0f;
			UnityEngine.Color color2 = this.fadeColor;
			this.fadeMaterial.color = color2;
			UnityEngine.Color color = color2;
			while (elapsedTime < this.fadeTime)
			{
				yield return new WaitForEndOfFrame();
				elapsedTime += Time.deltaTime;
				color.a = Mathf.Lerp(this.fadeColor.a, 0f, Mathf.Clamp01(elapsedTime / this.fadeTime));
				this.fadeMaterial.color = color;
			}
			this.SetFadersEnabled(fadeControls, false);
			yield break;
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x000C2788 File Offset: 0x000C0B88
		public IEnumerator DoPanorama(Camera[] cameras, string filenameBase, bool async, Transform pivot = null)
		{
			ComputeBuffer convertPanoramaResultBuffer = null;
			ComputeBuffer forceWaitResultConvertPanoramaStereoBuffer = null;
			if (this.usingGpuTransform)
			{
				if (this.captureStereoscopic)
				{
					convertPanoramaResultBuffer = new ComputeBuffer(this.panoramaWidth * this.panoramaHeight * 2 + 1, 4);
					this.convertPanoramaStereoShader.SetBuffer(this.renderStereoIdx, "result", convertPanoramaResultBuffer);
					forceWaitResultConvertPanoramaStereoBuffer = new ComputeBuffer(1, 4);
					this.convertPanoramaStereoShader.SetBuffer(this.renderStereoIdx, "forceWaitResultBuffer", forceWaitResultConvertPanoramaStereoBuffer);
				}
				else
				{
					int num = (this.panoramaHeight + 8 - 1) / 8;
					convertPanoramaResultBuffer = new ComputeBuffer(this.panoramaWidth * num + 1, 4);
					foreach (int kernelIndex in this.convertPanoramaKernelIdxs)
					{
						this.convertPanoramaShader.SetBuffer(kernelIndex, "result", convertPanoramaResultBuffer);
					}
				}
			}
			int cameraPixelsBufferNumTextures = this.numCameras;
			this.overlapTextures = 0;
			int circlePointCircularBufferSize = 0;
			if (this.captureStereoscopic && this.usingGpuTransform)
			{
				this.overlapTextures = ((this.ssaaFactor != 1) ? 2 : 1);
				circlePointCircularBufferSize = 1 + this.overlapTextures;
				cameraPixelsBufferNumTextures = Math.Min(this.numCameras, 2 + 2 * circlePointCircularBufferSize);
			}
			ComputeBuffer cameraPixelsBuffer = new ComputeBuffer(cameraPixelsBufferNumTextures * this.cameraWidth * this.cameraHeight + 1, 4);
			this.textureToBufferShader.SetBuffer(this.textureToBufferIdx, "result", cameraPixelsBuffer);
			this.textureToBufferShader.SetInt("sentinelIdx", cameraPixelsBuffer.count - 1);
			if (this.usingGpuTransform && !this.captureStereoscopic)
			{
				this.convertPanoramaShader.SetInt("cameraPixelsSentinelIdx", cameraPixelsBuffer.count - 1);
				this.convertPanoramaShader.SetInt("sentinelIdx", convertPanoramaResultBuffer.count - 1);
				foreach (int kernelIndex2 in this.convertPanoramaKernelIdxs)
				{
					this.convertPanoramaShader.SetBuffer(kernelIndex2, "cameraPixels", cameraPixelsBuffer);
				}
			}
			if (this.usingGpuTransform && this.captureStereoscopic)
			{
				this.convertPanoramaStereoShader.SetInt("cameraPixelsSentinelIdx", cameraPixelsBuffer.count - 1);
				this.convertPanoramaStereoShader.SetBuffer(this.renderStereoIdx, "cameraPixels", cameraPixelsBuffer);
			}
			ComputeBuffer forceWaitResultTextureToBufferBuffer = new ComputeBuffer(1, 4);
			this.textureToBufferShader.SetBuffer(this.textureToBufferIdx, "forceWaitResultBuffer", forceWaitResultTextureToBufferBuffer);
			float startTime = Time.realtimeSinceStartup;
			Quaternion headOrientation = Quaternion.identity;
			if (VRSettings.enabled && VRSettings.loadedDeviceName != "None")
			{
				headOrientation = InputTracking.GetLocalRotation(VRNode.Head);
			}
			this.Log("Rendering camera views");
			foreach (Camera camera in cameras)
			{
				this.Log("Camera name: " + camera.gameObject.name);
			}
			Dictionary<Camera, List<ImageEffectCopyCamera.InstanceMethodPair>> methodMap = new Dictionary<Camera, List<ImageEffectCopyCamera.InstanceMethodPair>>();
			foreach (Camera camera2 in cameras)
			{
				methodMap[camera2] = ImageEffectCopyCamera.GenerateMethodList(camera2);
			}
			string suffix = "." + this.FormatToExtension(this.imageFormat);
			string imagePath = this.saveImagePath;
			if (string.IsNullOrEmpty(imagePath))
			{
				imagePath = Application.dataPath + "/..";
			}
			string filePath = imagePath + "/" + filenameBase + suffix;
			this.convertPanoramaStereoShader.SetInt("circlePointCircularBufferSize", circlePointCircularBufferSize);
			int ilimit = (!this.usingGpuTransform) ? this.numCameras : (this.numCameras + this.overlapTextures * 4);
			int leftRightPhaseEnd = (ilimit - 2) / 2 + 2;
			int nextCirclePointCircularBufferStart = 0;
			int nextCirclePointStart = 0;
			int writeIdx = 0;
			int circlePointsRendered = 0;
			int saveCubemapImageNum = 0;
			this.BeforeRenderPanorama();
			RenderTexture.active = null;
			for (int m = 0; m < ilimit; m++)
			{
				if (this.captureStereoscopic)
				{
					if (m < 2)
					{
						this.camGos[1].transform.localPosition = Vector3.zero;
						this.camGos[1].transform.localRotation = Quaternion.Euler((m != 0) ? -90f : 90f, 0f, 0f);
					}
					else
					{
						int num2;
						int num3;
						if (m < leftRightPhaseEnd)
						{
							num2 = m - 2;
							num3 = 0;
						}
						else
						{
							num2 = m - leftRightPhaseEnd;
							num3 = 2;
						}
						int num4 = num2 / 2 % this.numCirclePoints;
						int num5 = num2 % 2 + num3;
						float num6 = 360f * (float)num4 / (float)this.numCirclePoints;
						this.camGos[1].transform.localPosition = Quaternion.Euler(0f, num6, 0f) * Vector3.forward * this.circleRadius;
						if (num5 < 2)
						{
							this.camGos[1].transform.localRotation = Quaternion.Euler(0f, num6 + ((num5 != 0) ? this.hFovAdjustDegrees : (-this.hFovAdjustDegrees)), 0f);
						}
						else
						{
							this.camGos[1].transform.localRotation = Quaternion.Euler((num5 != 2) ? this.vFovAdjustDegrees : (-this.vFovAdjustDegrees), num6, 0f);
						}
						if (num5 == 1 || num5 == 3)
						{
							circlePointsRendered++;
						}
					}
				}
				else
				{
					switch (m)
					{
					case 0:
						this.camGos[1].transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
						break;
					case 1:
						this.camGos[1].transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
						break;
					case 2:
						this.camGos[1].transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
						break;
					case 3:
						this.camGos[1].transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
						break;
					case 4:
						this.camGos[1].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
						break;
					case 5:
						this.camGos[1].transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
						break;
					}
				}
				foreach (Camera camera3 in cameras)
				{
					this.camGos[2].transform.parent = null;
					this.renderCamera.CopyFrom(camera3);
					if (pivot != null)
					{
						this.renderCamera.transform.position = pivot.position;
					}
					this.camGos[0].transform.localPosition = this.renderCamera.transform.localPosition;
					Vector3 eulerAngles = this.renderCamera.transform.localRotation.eulerAngles;
					this.camGos[0].transform.localRotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
					this.camGos[2].transform.parent = this.camGos[1].transform;
					this.renderCamera.transform.localPosition = Vector3.zero;
					this.renderCamera.transform.localRotation = Quaternion.identity;
					if (methodMap.ContainsKey(camera3))
					{
						this.copyCameraScript.enabled = (methodMap[camera3].Count > 0);
						this.copyCameraScript.onRenderImageMethods = methodMap[camera3];
					}
					else
					{
						this.copyCameraScript.enabled = false;
					}
					this.renderCamera.fieldOfView = this.vFov;
					if (pivot == null)
					{
						this.camGos[0].transform.rotation = ((!this.useDefaultOrientation) ? this.camGos[0].transform.rotation : Quaternion.identity);
					}
					else
					{
						this.camGos[0].transform.rotation = pivot.rotation;
					}
					this.renderCamera.depthTextureMode = DepthTextureMode.None;
					this.renderCamera.targetTexture = this.cubemapRenderTexture;
					this.renderCamera.ResetAspect();
					this.renderCamera.fieldOfView = this.vFov;
					Vector3 position = camera3.transform.position;
					Quaternion rotation = camera3.transform.rotation;
					float fieldOfView = camera3.fieldOfView;
					RenderTexture targetTexture = camera3.targetTexture;
					camera3.transform.position = this.renderCamera.transform.position;
					camera3.transform.rotation = this.renderCamera.transform.rotation;
					camera3.fieldOfView = this.renderCamera.fieldOfView;
					this.renderCamera.Render();
					camera3.transform.position = position;
					camera3.transform.rotation = rotation;
					camera3.fieldOfView = fieldOfView;
					camera3.targetTexture = targetTexture;
				}
				RenderTexture.active = this.cubemapRenderTexture;
				this.forceWaitTexture.ReadPixels(new Rect((float)this.cameraWidth - 1f, (float)this.cameraHeight - 1f, 1f, 1f), 0, 0);
				int num7 = 1000000 + m;
				this.textureToBufferShader.SetInt("forceWaitValue", num7);
				this.textureToBufferShader.SetTexture(this.textureToBufferIdx, "source", this.cubemapRenderTexture);
				this.textureToBufferShader.SetInt("startIdx", writeIdx * this.cameraWidth * this.cameraHeight);
				this.textureToBufferShader.Dispatch(this.textureToBufferIdx, (this.cameraWidth + this.threadsX - 1) / this.threadsX, (this.cameraHeight + this.threadsY - 1) / this.threadsY, 1);
				uint[] array3 = new uint[1];
				forceWaitResultTextureToBufferBuffer.GetData(array3);
				if ((ulong)array3[0] != (ulong)((long)num7))
				{
					Debug.LogError(string.Concat(new object[]
					{
						"TextureToBufferShader: Unexpected forceWaitResult value ",
						array3[0],
						", should be ",
						num7
					}));
				}
				if (this.saveCubemap && (m < 2 || (m >= 2 && m < 2 + this.numCirclePoints * 2) || (m >= leftRightPhaseEnd && m < leftRightPhaseEnd + this.numCirclePoints * 2)))
				{
					cameraPixelsBuffer.GetData(this.cameraPixels);
					if (this.cameraPixels[cameraPixelsBuffer.count - 1] != 1419455993u)
					{
						Eyeshot360cam.ReportOutOfGraphicsMemory();
					}
					this.SaveCubemapImage(this.cameraPixels, filenameBase, suffix, imagePath, saveCubemapImageNum, writeIdx);
					saveCubemapImageNum++;
				}
				writeIdx++;
				if (writeIdx >= cameraPixelsBufferNumTextures)
				{
					writeIdx = 2;
				}
				if (this.captureStereoscopic && this.usingGpuTransform && (m - 2 + 1) % 2 == 0 && (circlePointsRendered - nextCirclePointStart >= circlePointCircularBufferSize || m + 1 == 2 + (ilimit - 2) / 2 || m + 1 == ilimit))
				{
					num7 = 2000000 + m;
					this.convertPanoramaStereoShader.SetInt("forceWaitValue", num7);
					this.convertPanoramaStereoShader.SetInt("leftRightPass", (m >= leftRightPhaseEnd) ? 0 : 1);
					this.convertPanoramaStereoShader.SetInt("circlePointStart", nextCirclePointStart);
					this.convertPanoramaStereoShader.SetInt("circlePointEnd", (cameraPixelsBufferNumTextures >= this.numCameras) ? (circlePointsRendered + 1) : circlePointsRendered);
					this.convertPanoramaStereoShader.SetInt("circlePointCircularBufferStart", nextCirclePointCircularBufferStart);
					this.convertPanoramaStereoShader.Dispatch(this.renderStereoIdx, (this.panoramaWidth + this.threadsX - 1) / this.threadsX, (this.panoramaHeight + this.threadsY - 1) / this.threadsY, 2);
					forceWaitResultConvertPanoramaStereoBuffer.GetData(array3);
					if ((ulong)array3[0] != (ulong)((long)num7))
					{
						Debug.LogError(string.Concat(new object[]
						{
							"ConvertPanoramaStereoShader: Unexpected forceWaitResult value ",
							array3[0],
							", should be ",
							num7
						}));
					}
					if (m + 1 == leftRightPhaseEnd)
					{
						nextCirclePointCircularBufferStart = (nextCirclePointCircularBufferStart + circlePointCircularBufferSize) % circlePointCircularBufferSize;
						nextCirclePointStart = 0;
						circlePointsRendered = 0;
					}
					else
					{
						nextCirclePointStart = circlePointsRendered - this.overlapTextures;
						nextCirclePointCircularBufferStart = (nextCirclePointCircularBufferStart + circlePointCircularBufferSize - this.overlapTextures) % circlePointCircularBufferSize;
					}
				}
				RenderTexture.active = null;
			}
			if (this.saveCubemap || !this.usingGpuTransform)
			{
				cameraPixelsBuffer.GetData(this.cameraPixels);
				if (this.cameraPixels[cameraPixelsBuffer.count - 1] != 1419455993u)
				{
					Eyeshot360cam.ReportOutOfGraphicsMemory();
				}
			}
			RenderTexture.active = null;
			if (this.saveCubemap && (!this.captureStereoscopic || !this.usingGpuTransform))
			{
				for (int num8 = 0; num8 < this.numCameras; num8++)
				{
					int bufferIdx = num8;
					this.SaveCubemapImage(this.cameraPixels, filenameBase, suffix, imagePath, num8, bufferIdx);
				}
			}
			bool producedImageSuccess = false;
			if (this.panoramaFormat.Equals(Eyeshot360cam.PanoramaFormat.LongLatUnwrap))
			{
				Bitmap bitmap = new Bitmap(this.panoramaWidth, this.panoramaHeight * ((!this.captureStereoscopic) ? 1 : 2), PixelFormat.Format32bppArgb);
				BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
				IntPtr ptr = bmpData.Scan0;
				byte[] pixelValues = new byte[Math.Abs(bmpData.Stride) * bitmap.Height];
				if (async)
				{
					yield return base.StartCoroutine(this.CubemapToEquirectangular(cameraPixelsBuffer, this.cameraPixels, convertPanoramaResultBuffer, this.cameraWidth, this.cameraHeight, pixelValues, bmpData.Stride, this.panoramaWidth, this.panoramaHeight, this.ssaaFactor, async));
				}
				else
				{
					IEnumerator enumerator = this.CubemapToEquirectangular(cameraPixelsBuffer, this.cameraPixels, convertPanoramaResultBuffer, this.cameraWidth, this.cameraHeight, pixelValues, bmpData.Stride, this.panoramaWidth, this.panoramaHeight, this.ssaaFactor, async);
					while (enumerator.MoveNext())
					{
					}
				}
				producedImageSuccess = (pixelValues[3] == byte.MaxValue);
				yield return null;
				Marshal.Copy(pixelValues, 0, ptr, pixelValues.Length);
				bitmap.UnlockBits(bmpData);
				yield return null;
				this.Log("Time to take panorama screenshot: " + (Time.realtimeSinceStartup - startTime) + " sec");
				if (producedImageSuccess)
				{
					Thread thread = new Thread(() =>
					{
						this.Log("Saving equirectangular image");
						bitmap.Save(filePath, this.FormatToDrawingFormat(this.imageFormat));
					});
					thread.Start();
					while (thread.ThreadState.Equals(ThreadState.Running))
					{
						if (async)
						{
							yield return null;
						}
						else
						{
							Thread.Sleep(0);
						}
					}
				}
				bitmap.Dispose();
			}
			else if (this.panoramaFormat.Equals(Eyeshot360cam.PanoramaFormat.CubeUnwrap))
			{
				this.SaveCubemapStripImage(this.cameraPixels, filenameBase, suffix, imagePath);
			}
			this.AfterRenderPanorama();
			foreach (ComputeBuffer computeBuffer in new ComputeBuffer[]
			{
				convertPanoramaResultBuffer,
				cameraPixelsBuffer,
				forceWaitResultConvertPanoramaStereoBuffer,
				forceWaitResultTextureToBufferBuffer
			})
			{
				if (computeBuffer != null)
				{
					computeBuffer.Release();
				}
			}
			convertPanoramaResultBuffer = (cameraPixelsBuffer = null);
			if (producedImageSuccess && this.uploadImages && !this.captureEveryFrame)
			{
				this.Log("Uploading image");
				this.imageFileBytes = File.ReadAllBytes(filePath);
				File.Delete(filePath);
				Eyeshot360cam.EEyeshotPanoramaFormat format = Eyeshot360cam.EEyeshotPanoramaFormat.CPF_Cube;
				if (this.panoramaFormat.Equals(Eyeshot360cam.PanoramaFormat.LongLatUnwrap))
				{
					format = ((!this.captureStereoscopic) ? Eyeshot360cam.EEyeshotPanoramaFormat.CPF_LongLat : Eyeshot360cam.EEyeshotPanoramaFormat.CPF_StereoLongLat);
				}
				else if (this.panoramaFormat.Equals(Eyeshot360cam.PanoramaFormat.CubeUnwrap))
				{
					format = ((!this.captureStereoscopic) ? Eyeshot360cam.EEyeshotPanoramaFormat.CPF_Cube : Eyeshot360cam.EEyeshotPanoramaFormat.CPF_StereoCube);
				}
				string mimeType = this.FormatMimeType(this.imageFormat);
				if (async)
				{
					yield return base.StartCoroutine(this.UploadImage(this.imageFileBytes, filenameBase + suffix, mimeType, format, async));
				}
				else
				{
					IEnumerator enumerator2 = this.UploadImage(this.imageFileBytes, filenameBase + suffix, mimeType, format, async);
					while (enumerator2.MoveNext())
					{
					}
				}
			}
			else if (!producedImageSuccess)
			{
				this.PlaySound(this.failSound);
			}
			else if (!this.captureEveryFrame)
			{
				this.PlaySound(this.doneSound);
			}
			yield break;
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x000C27C0 File Offset: 0x000C0BC0
		public IEnumerator DoDepthMap(Camera[] cameras, string filenameBase, bool async, Transform pivot = null)
		{
			this.renderDepth.depthLevel = this.depthLevel;
			this.renderDepth.enabled = true;
			ComputeBuffer convertPanoramaResultBuffer = null;
			ComputeBuffer forceWaitResultConvertPanoramaStereoBuffer = null;
			if (this.usingGpuTransform)
			{
				if (this.captureStereoscopic)
				{
					convertPanoramaResultBuffer = new ComputeBuffer(this.panoramaWidth * this.panoramaHeight * 2 + 1, 4);
					this.convertPanoramaStereoShader.SetBuffer(this.renderStereoIdx, "result", convertPanoramaResultBuffer);
					forceWaitResultConvertPanoramaStereoBuffer = new ComputeBuffer(1, 4);
					this.convertPanoramaStereoShader.SetBuffer(this.renderStereoIdx, "forceWaitResultBuffer", forceWaitResultConvertPanoramaStereoBuffer);
				}
				else
				{
					int num = (this.panoramaHeight + 8 - 1) / 8;
					convertPanoramaResultBuffer = new ComputeBuffer(this.panoramaWidth * num + 1, 4);
					foreach (int kernelIndex in this.convertPanoramaKernelIdxs)
					{
						this.convertPanoramaShader.SetBuffer(kernelIndex, "result", convertPanoramaResultBuffer);
					}
				}
			}
			int cameraPixelsBufferNumTextures = this.numCameras;
			this.overlapTextures = 0;
			int circlePointCircularBufferSize = 0;
			if (this.captureStereoscopic && this.usingGpuTransform)
			{
				this.overlapTextures = ((this.ssaaFactor != 1) ? 2 : 1);
				circlePointCircularBufferSize = 1 + this.overlapTextures;
				cameraPixelsBufferNumTextures = Math.Min(this.numCameras, 2 + 2 * circlePointCircularBufferSize);
			}
			ComputeBuffer cameraPixelsBuffer = new ComputeBuffer(cameraPixelsBufferNumTextures * this.cameraWidth * this.cameraHeight + 1, 4);
			this.textureToBufferShader.SetBuffer(this.textureToBufferIdx, "result", cameraPixelsBuffer);
			this.textureToBufferShader.SetInt("sentinelIdx", cameraPixelsBuffer.count - 1);
			if (this.usingGpuTransform && !this.captureStereoscopic)
			{
				this.convertPanoramaShader.SetInt("cameraPixelsSentinelIdx", cameraPixelsBuffer.count - 1);
				this.convertPanoramaShader.SetInt("sentinelIdx", convertPanoramaResultBuffer.count - 1);
				foreach (int kernelIndex2 in this.convertPanoramaKernelIdxs)
				{
					this.convertPanoramaShader.SetBuffer(kernelIndex2, "cameraPixels", cameraPixelsBuffer);
				}
			}
			if (this.usingGpuTransform && this.captureStereoscopic)
			{
				this.convertPanoramaStereoShader.SetInt("cameraPixelsSentinelIdx", cameraPixelsBuffer.count - 1);
				this.convertPanoramaStereoShader.SetBuffer(this.renderStereoIdx, "cameraPixels", cameraPixelsBuffer);
			}
			ComputeBuffer forceWaitResultTextureToBufferBuffer = new ComputeBuffer(1, 4);
			this.textureToBufferShader.SetBuffer(this.textureToBufferIdx, "forceWaitResultBuffer", forceWaitResultTextureToBufferBuffer);
			float startTime = Time.realtimeSinceStartup;
			Quaternion headOrientation = Quaternion.identity;
			if (VRSettings.enabled && VRSettings.loadedDeviceName != "None")
			{
				headOrientation = InputTracking.GetLocalRotation(VRNode.LeftEye);
			}
			this.Log("Rendering camera depth");
			foreach (Camera camera in cameras)
			{
				this.Log("Camera name: " + camera.gameObject.name);
			}
			string suffix = "." + this.FormatToExtension(this.imageFormat);
			string imagePath = this.saveImagePath;
			if (string.IsNullOrEmpty(imagePath))
			{
				imagePath = Application.dataPath + "/..";
			}
			string filePath = imagePath + "/depth_" + filenameBase + suffix;
			this.convertPanoramaStereoShader.SetInt("circlePointCircularBufferSize", circlePointCircularBufferSize);
			int ilimit = (!this.usingGpuTransform) ? this.numCameras : (this.numCameras + this.overlapTextures * 4);
			int leftRightPhaseEnd = (ilimit - 2) / 2 + 2;
			int nextCirclePointCircularBufferStart = 0;
			int nextCirclePointStart = 0;
			int writeIdx = 0;
			int circlePointsRendered = 0;
			int saveCubemapImageNum = 0;
			this.BeforeRenderDepthMap();
			RenderTexture.active = null;
			for (int l = 0; l < ilimit; l++)
			{
				if (this.captureStereoscopic)
				{
					if (l < 2)
					{
						this.camGos[1].transform.localPosition = Vector3.zero;
						this.camGos[1].transform.localRotation = Quaternion.Euler((l != 0) ? -90f : 90f, 0f, 0f);
					}
					else
					{
						int num2;
						int num3;
						if (l < leftRightPhaseEnd)
						{
							num2 = l - 2;
							num3 = 0;
						}
						else
						{
							num2 = l - leftRightPhaseEnd;
							num3 = 2;
						}
						int num4 = num2 / 2 % this.numCirclePoints;
						int num5 = num2 % 2 + num3;
						float num6 = 360f * (float)num4 / (float)this.numCirclePoints;
						this.camGos[1].transform.localPosition = Quaternion.Euler(0f, num6, 0f) * Vector3.forward * this.circleRadius;
						if (num5 < 2)
						{
							this.camGos[1].transform.localRotation = Quaternion.Euler(0f, num6 + ((num5 != 0) ? this.hFovAdjustDegrees : (-this.hFovAdjustDegrees)), 0f);
						}
						else
						{
							this.camGos[1].transform.localRotation = Quaternion.Euler((num5 != 2) ? this.vFovAdjustDegrees : (-this.vFovAdjustDegrees), num6, 0f);
						}
						if (num5 == 1 || num5 == 3)
						{
							circlePointsRendered++;
						}
					}
				}
				else
				{
					switch (l)
					{
					case 0:
						this.camGos[1].transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
						break;
					case 1:
						this.camGos[1].transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
						break;
					case 2:
						this.camGos[1].transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
						break;
					case 3:
						this.camGos[1].transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
						break;
					case 4:
						this.camGos[1].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
						break;
					case 5:
						this.camGos[1].transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
						break;
					}
				}
				foreach (Camera camera2 in cameras)
				{
					this.camGos[2].transform.parent = null;
					this.renderCamera.CopyFrom(camera2);
					if (pivot != null)
					{
						this.renderCamera.transform.position = pivot.position;
					}
					this.camGos[0].transform.localPosition = this.renderCamera.transform.localPosition;
					Vector3 eulerAngles = this.renderCamera.transform.localRotation.eulerAngles;
					this.camGos[0].transform.localRotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
					this.camGos[0].transform.localPosition = this.renderCamera.transform.localPosition;
					this.camGos[0].transform.localRotation = this.renderCamera.transform.localRotation;
					this.camGos[2].transform.parent = this.camGos[1].transform;
					this.renderCamera.transform.localPosition = Vector3.zero;
					this.renderCamera.transform.localRotation = Quaternion.identity;
					this.copyCameraScript.enabled = false;
					this.copyCameraScript.onRenderImageMethods = null;
					this.renderCamera.fieldOfView = this.vFov;
					if (pivot == null)
					{
						this.camGos[0].transform.rotation = ((!this.useDefaultOrientation) ? this.camGos[0].transform.rotation : Quaternion.identity);
					}
					else
					{
						this.camGos[0].transform.rotation = pivot.rotation;
					}
					this.renderCamera.farClipPlane = this.depthFar;
					this.renderCamera.depthTextureMode = DepthTextureMode.Depth;
					this.renderCamera.targetTexture = this.cubemapDepthRenderTexture;
					this.renderCamera.ResetAspect();
					this.renderCamera.fieldOfView = this.vFov;
					Vector3 position = camera2.transform.position;
					Quaternion rotation = camera2.transform.rotation;
					float fieldOfView = camera2.fieldOfView;
					RenderTexture targetTexture = camera2.targetTexture;
					camera2.transform.position = this.renderCamera.transform.position;
					camera2.transform.rotation = this.renderCamera.transform.rotation;
					camera2.fieldOfView = this.renderCamera.fieldOfView;
					this.renderCamera.Render();
					camera2.transform.position = position;
					camera2.transform.rotation = rotation;
					camera2.fieldOfView = fieldOfView;
					camera2.targetTexture = targetTexture;
				}
				RenderTexture.active = this.cubemapDepthRenderTexture;
				this.forceWaitTexture.ReadPixels(new Rect((float)this.cameraWidth - 1f, (float)this.cameraHeight - 1f, 1f, 1f), 0, 0);
				int num7 = 1000000 + l;
				this.textureToBufferShader.SetInt("forceWaitValue", num7);
				this.textureToBufferShader.SetTexture(this.textureToBufferIdx, "source", this.cubemapDepthRenderTexture);
				this.textureToBufferShader.SetInt("startIdx", writeIdx * this.cameraWidth * this.cameraHeight);
				this.textureToBufferShader.Dispatch(this.textureToBufferIdx, (this.cameraWidth + this.threadsX - 1) / this.threadsX, (this.cameraHeight + this.threadsY - 1) / this.threadsY, 1);
				uint[] array3 = new uint[1];
				forceWaitResultTextureToBufferBuffer.GetData(array3);
				if ((ulong)array3[0] != (ulong)((long)num7))
				{
					Debug.LogError(string.Concat(new object[]
					{
						"TextureToBufferShader: Unexpected forceWaitResult value ",
						array3[0],
						", should be ",
						num7
					}));
				}
				if (this.saveCubemap && (l < 2 || (l >= 2 && l < 2 + this.numCirclePoints * 2) || (l >= leftRightPhaseEnd && l < leftRightPhaseEnd + this.numCirclePoints * 2)))
				{
					cameraPixelsBuffer.GetData(this.cameraPixels);
					if (this.cameraPixels[cameraPixelsBuffer.count - 1] != 1419455993u)
					{
						Eyeshot360cam.ReportOutOfGraphicsMemory();
					}
					this.SaveCubemapImage(this.cameraPixels, "depth_" + filenameBase, suffix, imagePath, saveCubemapImageNum, writeIdx);
					saveCubemapImageNum++;
				}
				writeIdx++;
				if (writeIdx >= cameraPixelsBufferNumTextures)
				{
					writeIdx = 2;
				}
				if (this.captureStereoscopic && this.usingGpuTransform && (l - 2 + 1) % 2 == 0 && (circlePointsRendered - nextCirclePointStart >= circlePointCircularBufferSize || l + 1 == 2 + (ilimit - 2) / 2 || l + 1 == ilimit))
				{
					num7 = 2000000 + l;
					this.convertPanoramaStereoShader.SetInt("forceWaitValue", num7);
					this.convertPanoramaStereoShader.SetInt("leftRightPass", (l >= leftRightPhaseEnd) ? 0 : 1);
					this.convertPanoramaStereoShader.SetInt("circlePointStart", nextCirclePointStart);
					this.convertPanoramaStereoShader.SetInt("circlePointEnd", (cameraPixelsBufferNumTextures >= this.numCameras) ? (circlePointsRendered + 1) : circlePointsRendered);
					this.convertPanoramaStereoShader.SetInt("circlePointCircularBufferStart", nextCirclePointCircularBufferStart);
					this.convertPanoramaStereoShader.Dispatch(this.renderStereoIdx, (this.panoramaWidth + this.threadsX - 1) / this.threadsX, (this.panoramaHeight + this.threadsY - 1) / this.threadsY, 2);
					forceWaitResultConvertPanoramaStereoBuffer.GetData(array3);
					if ((ulong)array3[0] != (ulong)((long)num7))
					{
						Debug.LogError(string.Concat(new object[]
						{
							"ConvertPanoramaStereoShader: Unexpected forceWaitResult value ",
							array3[0],
							", should be ",
							num7
						}));
					}
					if (l + 1 == leftRightPhaseEnd)
					{
						nextCirclePointCircularBufferStart = (nextCirclePointCircularBufferStart + circlePointCircularBufferSize) % circlePointCircularBufferSize;
						nextCirclePointStart = 0;
						circlePointsRendered = 0;
					}
					else
					{
						nextCirclePointStart = circlePointsRendered - this.overlapTextures;
						nextCirclePointCircularBufferStart = (nextCirclePointCircularBufferStart + circlePointCircularBufferSize - this.overlapTextures) % circlePointCircularBufferSize;
					}
				}
				RenderTexture.active = null;
			}
			if (this.saveCubemap || !this.usingGpuTransform)
			{
				cameraPixelsBuffer.GetData(this.cameraPixels);
				if (this.cameraPixels[cameraPixelsBuffer.count - 1] != 1419455993u)
				{
					Eyeshot360cam.ReportOutOfGraphicsMemory();
				}
			}
			RenderTexture.active = null;
			if (this.saveCubemap && (!this.captureStereoscopic || !this.usingGpuTransform))
			{
				for (int n = 0; n < this.numCameras; n++)
				{
					int bufferIdx = n;
					this.SaveCubemapImage(this.cameraPixels, "depth_" + filenameBase, suffix, imagePath, n, bufferIdx);
				}
			}
			bool producedImageSuccess = false;
			if (this.panoramaFormat.Equals(Eyeshot360cam.PanoramaFormat.LongLatUnwrap))
			{
				Bitmap bitmap = new Bitmap(this.panoramaWidth, this.panoramaHeight * ((!this.captureStereoscopic) ? 1 : 2), PixelFormat.Format32bppArgb);
				BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
				IntPtr ptr = bmpData.Scan0;
				byte[] pixelValues = new byte[Math.Abs(bmpData.Stride) * bitmap.Height];
				if (async)
				{
					yield return base.StartCoroutine(this.CubemapToEquirectangular(cameraPixelsBuffer, this.cameraPixels, convertPanoramaResultBuffer, this.cameraWidth, this.cameraHeight, pixelValues, bmpData.Stride, this.panoramaWidth, this.panoramaHeight, this.ssaaFactor, async));
				}
				else
				{
					IEnumerator enumerator = this.CubemapToEquirectangular(cameraPixelsBuffer, this.cameraPixels, convertPanoramaResultBuffer, this.cameraWidth, this.cameraHeight, pixelValues, bmpData.Stride, this.panoramaWidth, this.panoramaHeight, this.ssaaFactor, async);
					while (enumerator.MoveNext())
					{
					}
				}
				producedImageSuccess = (pixelValues[3] == byte.MaxValue);
				yield return null;
				Marshal.Copy(pixelValues, 0, ptr, pixelValues.Length);
				bitmap.UnlockBits(bmpData);
				yield return null;
				this.Log("Time to take panorama screenshot: " + (Time.realtimeSinceStartup - startTime) + " sec");
				if (producedImageSuccess)
				{
					Thread thread = new Thread(() =>
					{
						this.Log("Saving equirectangular image");
						bitmap.Save(filePath, this.FormatToDrawingFormat(this.imageFormat));
					});
					thread.Start();
					while (thread.ThreadState == ThreadState.Running)
					{
						if (async)
						{
							yield return null;
						}
						else
						{
							Thread.Sleep(0);
						}
					}
				}
				bitmap.Dispose();
			}
			else if (this.panoramaFormat.Equals(Eyeshot360cam.PanoramaFormat.CubeUnwrap))
			{
				this.SaveCubemapStripImage(this.cameraPixels, "depth_" + filenameBase, suffix, imagePath);
			}
			this.AfterRenderDepthMap();
			this.renderDepth.enabled = false;
			foreach (ComputeBuffer computeBuffer in new ComputeBuffer[]
			{
				convertPanoramaResultBuffer,
				cameraPixelsBuffer,
				forceWaitResultConvertPanoramaStereoBuffer,
				forceWaitResultTextureToBufferBuffer
			})
			{
				if (computeBuffer != null)
				{
					computeBuffer.Release();
				}
			}
			convertPanoramaResultBuffer = (cameraPixelsBuffer = null);
			if (producedImageSuccess && this.uploadImages && !this.captureEveryFrame)
			{
				this.Log("Uploading depth image");
				this.imageFileBytes = File.ReadAllBytes(filePath);
				File.Delete(filePath);
				Eyeshot360cam.EEyeshotPanoramaFormat format = Eyeshot360cam.EEyeshotPanoramaFormat.CPF_Cube;
				if (this.panoramaFormat.Equals(Eyeshot360cam.PanoramaFormat.LongLatUnwrap))
				{
					format = ((!this.captureStereoscopic) ? Eyeshot360cam.EEyeshotPanoramaFormat.CPF_LongLat : Eyeshot360cam.EEyeshotPanoramaFormat.CPF_StereoLongLat);
				}
				else if (this.panoramaFormat.Equals(Eyeshot360cam.PanoramaFormat.CubeUnwrap))
				{
					format = ((!this.captureStereoscopic) ? Eyeshot360cam.EEyeshotPanoramaFormat.CPF_Cube : Eyeshot360cam.EEyeshotPanoramaFormat.CPF_StereoCube);
				}
				string mimeType = this.FormatMimeType(this.imageFormat);
				if (async)
				{
					yield return base.StartCoroutine(this.UploadDepthImage(this.imageFileBytes, "depth_" + filenameBase + suffix, mimeType, format, async));
				}
				else
				{
					IEnumerator enumerator2 = this.UploadDepthImage(this.imageFileBytes, "depth_" + filenameBase + suffix, mimeType, format, async);
					while (enumerator2.MoveNext())
					{
					}
				}
			}
			else if (!producedImageSuccess)
			{
				this.PlaySound(this.failSound);
			}
			else if (!this.captureEveryFrame)
			{
				this.PlaySound(this.doneSound);
			}
			yield break;
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x000C27F8 File Offset: 0x000C0BF8
		public IEnumerator CaptureScreenshotAsyncHelper(string filenameBase, bool async, Transform[] pivots = null)
		{
			if (async)
			{
				while (Eyeshot360cam.capturing)
				{
					yield return null;
				}
			}
			Eyeshot360cam.capturing = true;
			if (!this.OnCaptureStart())
			{
				this.PlaySound(this.failSound);
				this.UpdateStatus("Capture isn't avaliable", UnityEngine.Color.red);
				Eyeshot360cam.capturing = false;
				yield break;
			}
			Camera[] cameras = this.GetCaptureCameras();
			Array.Sort<Camera>(cameras, (Camera x, Camera y) => x.depth.CompareTo(y.depth));
			if (cameras.Length == 0)
			{
				this.UpdateStatus("No cameras found to capture", UnityEngine.Color.red);
				Debug.LogWarning("No cameras found to capture");
				this.PlaySound(this.failSound);
				Eyeshot360cam.capturing = false;
				yield break;
			}
			if (this.antiAliasing != Eyeshot360cam.AntiAliasing._1)
			{
				foreach (Camera camera in cameras)
				{
					if (camera.actualRenderingPath.Equals(RenderingPath.DeferredLighting) || camera.actualRenderingPath.Equals(RenderingPath.DeferredShading))
					{
						Debug.LogWarning("CapturePanorama: Setting Anti Aliasing=1 because at least one camera in deferred mode. Use SSAA setting or Antialiasing image effect if needed.");
						this.antiAliasing = Eyeshot360cam.AntiAliasing._1;
						this.Reinitialize();
						break;
					}
				}
			}
			this.Log("Starting panorama capture");
			if (!this.captureEveryFrame)
			{
				this.PlaySound(this.startSound);
			}
			List<ScreenFadeControl> fadeControls = new List<ScreenFadeControl>();
			if (this.fadeDuringCapture && async)
			{
				foreach (Camera camera2 in cameras)
				{
					ScreenFadeControl screenFadeControl = camera2.gameObject.AddComponent<ScreenFadeControl>();
					screenFadeControl.fadeMaterial = this.fadeMaterial;
					fadeControls.Add(screenFadeControl);
				}
				this.SetFadersEnabled(fadeControls, false);
				yield return base.StartCoroutine(this.FadeOut(fadeControls));
			}
			for (int i = 0; i < 2; i++)
			{
				yield return new WaitForEndOfFrame();
			}
			this.Log("Changing quality level");
			int saveQualityLevel = QualitySettings.GetQualityLevel();
			bool qualitySettingWasChanged = false;
			string[] qualitySettingNames = QualitySettings.names;
			if (this.qualitySetting != qualitySettingNames[saveQualityLevel])
			{
				for (int n = 0; n < qualitySettingNames.Length; n++)
				{
					if (qualitySettingNames[n].Equals(this.qualitySetting))
					{
						QualitySettings.SetQualityLevel(n, false);
						qualitySettingWasChanged = true;
						break;
					}
				}
				if (!this.qualitySetting.Equals(string.Empty) && !qualitySettingWasChanged)
				{
					Debug.LogError("Quality setting specified for CapturePanorama is invalid, ignoring.", this);
				}
			}
			if (pivots == null)
			{
				yield return this.DoPanorama(cameras, filenameBase, async, null);
				if (this.depthMap)
				{
					yield return this.DoDepthMap(cameras, filenameBase, async, null);
				}
			}
			else
			{
				for (int j = 0; j < pivots.Length; j++)
				{
					Transform pivot = pivots[j];
					if (pivot != null)
					{
						pivot.SendMessage("OnPrePanoCapture", SendMessageOptions.DontRequireReceiver);
						yield return this.DoPanorama(cameras, filenameBase + "_Camera_" + j, async, pivot);
						if (this.depthMap)
						{
							yield return this.DoDepthMap(cameras, filenameBase + "_Camera_" + j, async, pivot);
						}
						pivot.SendMessage("OnPostPanoCapture", SendMessageOptions.DontRequireReceiver);
					}
					else
					{
						Debug.LogWarning("Additional camera is NULL!");
					}
				}
			}
			this.Log("Resetting quality level");
			if (qualitySettingWasChanged)
			{
				QualitySettings.SetQualityLevel(saveQualityLevel, false);
			}
			for (int k = 0; k < 2; k++)
			{
				yield return new WaitForEndOfFrame();
			}
			if (this.fadeDuringCapture && async)
			{
				yield return base.StartCoroutine(this.FadeIn(fadeControls));
				foreach (ScreenFadeControl obj in fadeControls)
				{
					UnityEngine.Object.Destroy(obj);
				}
				fadeControls.Clear();
			}
			this.UpdateStatus("Capturing finished", UnityEngine.Color.green);
			yield return new WaitForSeconds(1f);
			this.UpdateStatus(string.Empty, UnityEngine.Color.white);
			this.lastTimeOfCapturingDone = new float?(Time.realtimeSinceStartup);
			Eyeshot360cam.capturing = false;
			yield break;
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x000C2828 File Offset: 0x000C0C28
		public virtual bool OnCaptureStart()
		{
			return true;
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x000C282C File Offset: 0x000C0C2C
		public virtual Camera[] GetCaptureCameras()
		{
			List<Camera> list = new List<Camera>();
			Camera[] allCameras = Camera.allCameras;
			foreach (Camera camera in allCameras)
			{
				if (camera.isActiveAndEnabled && !(camera.targetTexture != null))
				{
					if (camera.cullingMask != 0 && (camera.cullingMask & 1 << LayerMask.NameToLayer("UI")) == 0)
					{
						list.Add(camera);
					}
				}
			}
			if (list.Count == 0)
			{
				foreach (Camera camera2 in allCameras)
				{
					if (camera2.cullingMask == -1)
					{
						list.Add(camera2);
						break;
					}
				}
			}
			if (list.Count == 0)
			{
				foreach (Camera camera3 in allCameras)
				{
					if ((camera3.cullingMask & 1 << LayerMask.NameToLayer("UI")) != 0)
					{
						list.Add(camera3);
						break;
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x000C295C File Offset: 0x000C0D5C
		public virtual void BeforeRenderPanorama()
		{
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x000C295E File Offset: 0x000C0D5E
		public virtual void AfterRenderPanorama()
		{
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x000C2960 File Offset: 0x000C0D60
		public virtual void BeforeRenderDepthMap()
		{
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x000C2962 File Offset: 0x000C0D62
		public virtual void AfterRenderDepthMap()
		{
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x000C2964 File Offset: 0x000C0D64
		private static void ReportOutOfGraphicsMemory()
		{
			throw new OutOfMemoryException("Exhausted graphics memory while capturing panorama. Lower Panorama Width, increase Num Circle Points for stereoscopic images, disable Anti Aliasing, or disable Stereoscopic Capture.");
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x000C2970 File Offset: 0x000C0D70
		private void SaveCubemapImage(uint[] cameraPixels, string filenameBase, string suffix, string imagePath, int i, int bufferIdx)
		{
			Bitmap bitmap = new Bitmap(this.cameraWidth, this.cameraHeight, PixelFormat.Format32bppArgb);
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
			IntPtr scan = bitmapData.Scan0;
			byte[] array = new byte[Math.Abs(bitmapData.Stride) * bitmap.Height];
			int stride = bitmapData.Stride;
			int height = bitmapData.Height;
			int num = bufferIdx * this.cameraWidth * this.cameraHeight;
			for (int j = 0; j < this.cameraHeight; j++)
			{
				int num2 = stride * (height - 1 - j);
				for (int k = 0; k < this.cameraWidth; k++)
				{
					uint num3 = cameraPixels[num];
					array[num2] = (byte)(num3 & 255u);
					array[num2 + 1] = (byte)(num3 >> 8 & 255u);
					array[num2 + 2] = (byte)(num3 >> 16);
					array[num2 + 3] = byte.MaxValue;
					num2 += 4;
					num++;
				}
			}
			Marshal.Copy(array, 0, scan, array.Length);
			bitmap.UnlockBits(bitmapData);
			string text = string.Empty;
			if (this.captureStereoscopic)
			{
				text = i.ToString();
				this.Log("Saving lightfield camera image number " + text);
			}
			else
			{
				CubemapFace cubemapFace = (CubemapFace)i;
				text = cubemapFace.ToString();
				this.Log("Saving cubemap image " + text);
			}
			string filename = string.Concat(new string[]
			{
				imagePath,
				"/",
				filenameBase,
				"_",
				text,
				suffix
			});
			bitmap.Save(filename, this.FormatToDrawingFormat(this.imageFormat));
			bitmap.Dispose();
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x000C2B38 File Offset: 0x000C0F38
		private void SaveCubemapStripImage(uint[] cameraPixels, string filenameBase, string suffix, string imagePath)
		{
			Bitmap bitmap = new Bitmap(this.cameraWidth * 6, this.cameraHeight, PixelFormat.Format32bppArgb);
			Bitmap bitmap2 = new Bitmap(this.cameraWidth, this.cameraHeight, PixelFormat.Format32bppArgb);
			using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap))
			{
				for (int i = 0; i < 6; i++)
				{
					BitmapData bitmapData = bitmap2.LockBits(new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
					IntPtr scan = bitmapData.Scan0;
					byte[] array = new byte[Math.Abs(bitmapData.Stride) * bitmap.Height];
					int stride = bitmapData.Stride;
					int height = bitmapData.Height;
					int num = i * this.cameraWidth * this.cameraHeight;
					for (int j = 0; j < this.cameraHeight; j++)
					{
						int num2 = stride * (height - 1 - j);
						for (int k = 0; k < this.cameraWidth; k++)
						{
							uint num3 = cameraPixels[num];
							array[num2] = (byte)(num3 & 255u);
							array[num2 + 1] = (byte)(num3 >> 8 & 255u);
							array[num2 + 2] = (byte)(num3 >> 16);
							array[num2 + 3] = byte.MaxValue;
							num2 += 4;
							num++;
						}
					}
					Marshal.Copy(array, 0, scan, array.Length);
					bitmap2.UnlockBits(bitmapData);
					graphics.DrawImage(bitmap2, i * this.cameraWidth, 0);
				}
			}
			string filename = string.Concat(new string[]
			{
				imagePath,
				"/",
				filenameBase,
				"_CubeStrip",
				suffix
			});
			bitmap.Save(filename, this.FormatToDrawingFormat(this.imageFormat));
			bitmap.Dispose();
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x000C2D1C File Offset: 0x000C111C
		private Color32 GetCameraPixelBilinear(uint[] cameraPixels, int cameraNum, float u, float v)
		{
			u *= (float)this.cameraWidth;
			v *= (float)this.cameraHeight;
			int num = (int)Math.Floor((double)u);
			int num2 = Math.Min(this.cameraWidth - 1, num + 1);
			int num3 = (int)Math.Floor((double)v);
			int num4 = Math.Min(this.cameraHeight - 1, num3 + 1);
			float t = u - (float)num;
			float t2 = v - (float)num3;
			int num5 = cameraNum * this.cameraWidth * this.cameraHeight;
			int num6 = num5 + num3 * this.cameraWidth;
			int num7 = num5 + num4 * this.cameraWidth;
			uint num8 = cameraPixels[num6 + num];
			uint num9 = cameraPixels[num6 + num2];
			uint num10 = cameraPixels[num7 + num];
			uint num11 = cameraPixels[num7 + num2];
			float num12 = Mathf.Lerp(Mathf.Lerp(num8 >> 16, num10 >> 16, t2), Mathf.Lerp(num9 >> 16, num11 >> 16, t2), t);
			float num13 = Mathf.Lerp(Mathf.Lerp(num8 >> 8 & 255u, num10 >> 8 & 255u, t2), Mathf.Lerp(num9 >> 8 & 255u, num11 >> 8 & 255u, t2), t);
			float num14 = Mathf.Lerp(Mathf.Lerp(num8 & 255u, num10 & 255u, t2), Mathf.Lerp(num9 & 255u, num11 & 255u, t2), t);
			return new UnityEngine.Color(num12 / 255f, num13 / 255f, num14 / 255f, 1f);
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x000C2EB0 File Offset: 0x000C12B0
		private string SerailizeMetadata(Eyeshot360cam.Metadata mtd)
		{
			string format = "{{\"game\": \"{0}\",\"developerUrl\": \"{1}\",\"lookDirection\": {{\"x\": {2},\"y\": {3},\"z\": {4}}},\"eyeDistance\" : {5},\"roomTitle\": \"{6}\",\"gameDescription\": \"{7}\",\"tags\": [{8}],\"people\": [{9}],\"album\": \"{10}\",\"isPrivateAlbum\": {11},\"uploadSessionToAlbum\": {12}}}";
			object[] array = new object[13];
			array[0] = mtd.game;
			array[1] = mtd.developerUrl;
			array[2] = mtd.lookDirection.x;
			array[3] = mtd.lookDirection.y;
			array[4] = mtd.lookDirection.z;
			array[5] = mtd.eyeDistance;
			array[6] = mtd.roomTitle;
			array[7] = mtd.gameDescription;
			array[8] = string.Join(",", (from x in mtd.tags
			select "\"" + x + "\"").ToArray<string>());
			array[9] = string.Join(",", (from x in mtd.people
			select "\"" + x + "\"").ToArray<string>());
			array[10] = mtd.albumName;
			array[11] = mtd.isPrivateAlbum.ToString().ToLower();
			array[12] = mtd.uploadSessionToAlbum.ToString().ToLower();
			return string.Format(format, array);
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x000C2FFC File Offset: 0x000C13FC
		private IEnumerator UploadImage(byte[] imageFileBytes, string fileName, string mimeType, Eyeshot360cam.EEyeshotPanoramaFormat format, bool async)
		{
			this.UpdateStatus("Uploading capture...", UnityEngine.Color.cyan);
			float startTime = Time.realtimeSinceStartup;
			WWWForm form = new WWWForm();
			form.AddField("key", this.apiKey);
			form.AddField("userToken", this.userToken);
			form.AddField("action", "upload");
			form.AddField("format", this.ECapturePanoramaFormatToPostParameterValue[(int)format]);
			string metadataStr = this.SerailizeMetadata(this.metadata);
			Debug.Log(metadataStr);
			form.AddField("metadata", metadataStr);
			form.AddBinaryData("source", imageFileBytes, fileName, mimeType);
			WWW w = new WWW(this.apiUrl + "upload", form);
			yield return w;
			if (!string.IsNullOrEmpty(w.error))
			{
				this.UpdateStatus("Panorama upload failed: " + w.error, UnityEngine.Color.red);
				Debug.LogError("Panorama upload failed: " + w.error + " " + w.text, this);
				this.PlaySound(this.failSound);
			}
			else
			{
				this.Log("Time to upload panorama screenshot: " + (Time.realtimeSinceStartup - startTime) + " sec");
				Dictionary<string, object> root = Json.Decode(w.text) as Dictionary<string, object>;
				string imageId = (root["metadata"] as Dictionary<string, object>)["image_id"] as string;
				string shortUrl = ((root["image"] as Dictionary<string, object>)["url_viewer"] as string).Replace("\\/", "/");
				if (this.saveShortUrl)
				{
					string text = Directory.GetParent(Application.dataPath).FullName + "/uploadedURLs.txt";
					List<string> list = new List<string>();
					list.Add(shortUrl);
					if (File.Exists(text))
					{
						list.AddRange(File.ReadAllLines(text));
					}
					using (StreamWriter streamWriter = new StreamWriter(text))
					{
						foreach (char value in text)
						{
							streamWriter.WriteLine(value);
						}
					}
				}
				this.Log("Short Url: " + shortUrl);
				this.Log("Image Id: " + imageId);
				if (!this.captureEveryFrame)
				{
					this.PlaySound(this.doneSound);
				}
				this.UpdateStatus("Upload finished", UnityEngine.Color.green);
				yield return new WaitForSeconds(1f);
				this.UpdateStatus(string.Empty, UnityEngine.Color.white);
				if (Eyeshot360cam.OnShortURLGetted != null)
				{
					Eyeshot360cam.OnShortURLGetted(shortUrl);
				}
			}
			w.Dispose();
			yield break;
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x000C3034 File Offset: 0x000C1434
		private IEnumerator UploadDepthImage(byte[] imageFileBytes, string fileName, string mimeType, Eyeshot360cam.EEyeshotPanoramaFormat format, bool async)
		{
			this.UpdateStatus("Uploading depth map capture...", UnityEngine.Color.cyan);
			float startTime = Time.realtimeSinceStartup;
			WWWForm form = new WWWForm();
			form.AddField("key", this.apiKey);
			form.AddField("userToken", this.userToken);
			form.AddField("action", "upload");
			form.AddField("format", this.ECapturePanoramaFormatToPostParameterValue[(int)format]);
			form.AddField("metadata", this.SerailizeMetadata(this.metadata));
			form.AddBinaryData("source", imageFileBytes, fileName, mimeType);
			WWW w = new WWW(this.apiUrl + "upload", form);
			yield return w;
			if (!string.IsNullOrEmpty(w.error))
			{
				this.UpdateStatus("Panorama upload depth map failed: " + w.error, UnityEngine.Color.red);
				Debug.LogError("Panorama upload depth map failed: " + w.error, this);
				this.PlaySound(this.failSound);
			}
			else
			{
				this.Log("Time to upload panorama screenshot: " + (Time.realtimeSinceStartup - startTime) + " sec");
				if (!this.captureEveryFrame)
				{
					this.PlaySound(this.doneSound);
				}
				this.UpdateStatus("Upload depth map finished", UnityEngine.Color.green);
				yield return new WaitForSeconds(1f);
				this.UpdateStatus(string.Empty, UnityEngine.Color.white);
			}
			w.Dispose();
			yield break;
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x000C306C File Offset: 0x000C146C
		private IEnumerator CubemapToEquirectangular(ComputeBuffer cameraPixelsBuffer, uint[] cameraPixels, ComputeBuffer convertPanoramaResultBuffer, int cameraWidth, int cameraHeight, byte[] pixelValues, int stride, int panoramaWidth, int panoramaHeight, int ssaaFactor, bool async)
		{
			if (this.captureStereoscopic && this.usingGpuTransform)
			{
				convertPanoramaResultBuffer.GetData(this.resultPixels);
				if (this.resultPixels[convertPanoramaResultBuffer.count - 1] != 1419455993u)
				{
					Eyeshot360cam.ReportOutOfGraphicsMemory();
				}
				this.WriteOutputPixels(pixelValues, stride, panoramaWidth, panoramaHeight * 2, panoramaHeight * 2, 0);
			}
			else if (this.captureStereoscopic && !this.usingGpuTransform)
			{
				float startTime = Time.realtimeSinceStartup;
				float processingTimePerFrame = this.cpuMillisecondsPerFrame / 1000f;
				for (int y = 0; y < panoramaHeight; y++)
				{
					for (int x = 0; x < panoramaWidth; x++)
					{
						float xcoord = (float)x / (float)panoramaWidth;
						float ycoord = (float)y / (float)panoramaHeight;
						float latitude = (ycoord - 0.5f) * 3.14159274f;
						float sinLat = Mathf.Sin(latitude);
						float cosLat = Mathf.Cos(latitude);
						float longitude = (xcoord * 2f - 1f) * 3.14159274f;
						float sinLong = Mathf.Sin(longitude);
						float cosLong = Mathf.Cos(longitude);
						float latitudeNormalized = latitude / 1.57079637f;
						float ipdScale = this.IpdScaleFunction(latitudeNormalized);
						float scaledEyeRadius = ipdScale * this.interpupillaryDistance / 2f;
						float ipdScaleLerp = 1f - ipdScale * 5f;
						UnityEngine.Color colorCap = new UnityEngine.Color(0f, 0f, 0f, 0f);
						if (ipdScaleLerp > 0f)
						{
							Vector3 vector = new Vector3(cosLat * sinLong, sinLat, cosLat * cosLong);
							float num = 1f / vector.y;
							float u = vector.x * num;
							float v = vector.z * num;
							if (u * u <= 1f && v * v <= 1f)
							{
								int cameraNum;
								if (vector.y > 0f)
								{
									cameraNum = 0;
								}
								else
								{
									u = -u;
									cameraNum = 1;
								}
								u = (u + 1f) * 0.5f;
								v = (v + 1f) * 0.5f;
								colorCap = this.GetCameraPixelBilinear(cameraPixels, cameraNum, u, v);
							}
						}
						for (int i = 0; i < 2; i++)
						{
							Vector3 vector2 = new Vector3(sinLong, 0f, cosLong);
							float num2 = 1.57079637f - Mathf.Acos(scaledEyeRadius / this.circleRadius);
							if (i == 0)
							{
								num2 = -num2;
							}
							float num3 = longitude + num2;
							if (num3 < 0f)
							{
								num3 += 6.28318548f;
							}
							if (num3 >= 6.28318548f)
							{
								num3 -= 6.28318548f;
							}
							float num4 = num3 / 6.28318548f * (float)this.numCirclePoints;
							int num5 = (int)Mathf.Floor(num4) % this.numCirclePoints;
							UnityEngine.Color a = default(UnityEngine.Color);
							UnityEngine.Color b = default(UnityEngine.Color);
							for (int j = 0; j < 2; j++)
							{
								int num6 = (j != 0) ? ((num5 + 1) % this.numCirclePoints) : num5;
								float f = 6.28318548f * (float)num6 / (float)this.numCirclePoints;
								float num7 = Mathf.Sin(f);
								float num8 = Mathf.Cos(f);
								float num9 = Mathf.Sign(vector2.x * num8 - vector2.z * num7) * Mathf.Acos(vector2.z * num8 + vector2.x * num7);
								float num10 = Mathf.Cos(num9);
								float num11 = Mathf.Sin(num9);
								int cameraNum = 2 + num6 * 2 + ((num9 < 0f) ? 0 : 1);
								float num12 = (num9 < 0f) ? this.hFovAdjust : (-this.hFovAdjust);
								float f2 = num9 + num12;
								Vector3 vector3 = new Vector3(cosLat * Mathf.Sin(f2), sinLat, cosLat * Mathf.Cos(f2));
								float u = vector3.x / vector3.z / this.tanHalfHFov;
								float v = -vector3.y / vector3.z / this.tanHalfVFov;
								if (vector3.z <= 0f || u * u > 1f || v * v > 0.9f)
								{
									cameraNum = 2 + this.numCirclePoints * 2 + num6 * 2 + ((latitude < 0f) ? 0 : 1);
									float f3 = (latitude < 0f) ? (-this.vFovAdjust) : this.vFovAdjust;
									float num13 = Mathf.Cos(f3);
									float num14 = Mathf.Sin(f3);
									vector3 = new Vector3(cosLat * num11, num13 * sinLat - cosLat * num10 * num14, num14 * sinLat + cosLat * num10 * num13);
									u = vector3.x / vector3.z / this.tanHalfHFov;
									v = -vector3.y / vector3.z / this.tanHalfVFov;
								}
								u = (u + 1f) * 0.5f;
								v = (v + 1f) * 0.5f;
								if (j == 0)
								{
									a = this.GetCameraPixelBilinear(cameraPixels, cameraNum, u, v);
								}
								else
								{
									b = this.GetCameraPixelBilinear(cameraPixels, cameraNum, u, v);
								}
							}
							Color32 c = UnityEngine.Color.Lerp(a, b, num4 - Mathf.Floor(num4));
							if (colorCap.a > 0f && ipdScaleLerp > 0f)
							{
								c = UnityEngine.Color.Lerp(c, colorCap, ipdScaleLerp);
							}
							int num15 = stride * (y + panoramaHeight * i) + x * 4;
							pixelValues[num15] = c.b;
							pixelValues[num15 + 1] = c.g;
							pixelValues[num15 + 2] = c.r;
							pixelValues[num15 + 3] = byte.MaxValue;
						}
						if ((x & 255) == 0 && Time.realtimeSinceStartup - startTime > processingTimePerFrame)
						{
							yield return null;
							startTime = Time.realtimeSinceStartup;
						}
					}
				}
			}
			else if (!this.captureStereoscopic && this.usingGpuTransform)
			{
				int num16 = (panoramaHeight + 8 - 1) / 8;
				this.Log("Invoking GPU shader for equirectangular reprojection");
				int num17 = (int)Mathf.Floor((float)panoramaHeight * 0.25f);
				int num18 = (int)Mathf.Ceil((float)panoramaHeight * 0.75f);
				for (int k = 0; k < 8; k++)
				{
					int num19 = k * num16;
					int num20 = Math.Min(num19 + num16, panoramaHeight);
					this.convertPanoramaShader.SetInt("startY", k * num16);
					this.convertPanoramaShader.SetInt("sliceHeight", num20 - num19);
					if (num20 <= num17)
					{
						this.convertPanoramaShader.Dispatch(this.convertPanoramaYNegativeKernelIdx, (panoramaWidth + this.threadsX - 1) / this.threadsX, (num16 + this.threadsY - 1) / this.threadsY, 1);
					}
					else if (num19 >= num18)
					{
						this.convertPanoramaShader.Dispatch(this.convertPanoramaYPositiveKernelIdx, (panoramaWidth + this.threadsX - 1) / this.threadsX, (num16 + this.threadsY - 1) / this.threadsY, 1);
					}
					else
					{
						this.convertPanoramaShader.Dispatch(this.convertPanoramaKernelIdx, (panoramaWidth + this.threadsX - 1) / this.threadsX, (panoramaHeight + this.threadsY - 1) / this.threadsY, 1);
					}
					convertPanoramaResultBuffer.GetData(this.resultPixels);
					if (this.resultPixels[convertPanoramaResultBuffer.count - 1] != 1419455993u)
					{
						Eyeshot360cam.ReportOutOfGraphicsMemory();
					}
					this.WriteOutputPixels(pixelValues, stride, panoramaWidth, num16, panoramaHeight, num19);
				}
			}
			else if (async)
			{
				yield return base.StartCoroutine(this.CubemapToEquirectangularCpu(cameraPixels, cameraWidth, cameraHeight, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, async));
			}
			else
			{
				IEnumerator enumerator = this.CubemapToEquirectangularCpu(cameraPixels, cameraWidth, cameraHeight, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, async);
				while (enumerator.MoveNext())
				{
				}
			}
			yield break;
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x000C30D8 File Offset: 0x000C14D8
		private void WriteOutputPixels(byte[] pixelValues, int stride, int bitmapWidth, int inHeight, int outHeight, int yStart)
		{
			int num = 0;
			int num2 = yStart;
			while (num2 < yStart + inHeight && num2 < outHeight)
			{
				int num3 = stride * num2;
				for (int i = 0; i < bitmapWidth; i++)
				{
					uint num4 = this.resultPixels[num];
					pixelValues[num3] = (byte)(num4 >> 0 & 255u);
					pixelValues[num3 + 1] = (byte)(num4 >> 8 & 255u);
					pixelValues[num3 + 2] = (byte)(num4 >> 16 & 255u);
					pixelValues[num3 + 3] = byte.MaxValue;
					num3 += 4;
					num++;
				}
				num2++;
			}
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x000C3168 File Offset: 0x000C1568
		private IEnumerator CubemapToEquirectangularCpu(uint[] cameraPixels, int cameraWidth, int cameraHeight, byte[] pixelValues, int stride, int panoramaWidth, int panoramaHeight, int ssaaFactor, bool async)
		{
			this.Log("Converting to equirectangular");
			yield return null;
			float startTime = Time.realtimeSinceStartup;
			float processingTimePerFrame = this.cpuMillisecondsPerFrame / 1000f;
			float maxWidth = 1f - 1f / (float)cameraWidth;
			float maxHeight = 1f - 1f / (float)cameraHeight;
			int numPixelsAveraged = ssaaFactor * ssaaFactor;
			int endYPositive = (int)Mathf.Floor((float)panoramaHeight * 0.25f);
			int startYNegative = (int)Mathf.Ceil((float)panoramaHeight * 0.75f);
			int endTopMixedRegion = (int)Mathf.Ceil((float)panoramaHeight * 0.304086983f);
			int startBottomMixedRegion = (int)Mathf.Floor((float)panoramaHeight * 0.695913f);
			int startXNegative = (int)Mathf.Ceil((float)panoramaWidth * 1f / 8f);
			int endXNegative = (int)Mathf.Floor((float)panoramaWidth * 3f / 8f);
			int startZPositive = (int)Mathf.Ceil((float)panoramaWidth * 3f / 8f);
			int endZPositive = (int)Mathf.Floor((float)panoramaWidth * 5f / 8f);
			int startXPositive = (int)Mathf.Ceil((float)panoramaWidth * 5f / 8f);
			int endXPositive = (int)Mathf.Floor((float)panoramaWidth * 7f / 8f);
			int startZNegative = (int)Mathf.Ceil((float)panoramaWidth * 7f / 8f);
			int endZNegative = (int)Mathf.Floor((float)panoramaWidth * 1f / 8f);
			if (async)
			{
				yield return base.StartCoroutine(this.CubemapToEquirectangularCpuPositiveY(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, numPixelsAveraged, 0, 0, panoramaWidth, endYPositive));
				yield return base.StartCoroutine(this.CubemapToEquirectangularCpuNegativeY(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, numPixelsAveraged, 0, startYNegative, panoramaWidth, panoramaHeight));
				yield return base.StartCoroutine(this.CubemapToEquirectangularCpuPositiveX(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, numPixelsAveraged, startXPositive, endTopMixedRegion, endXPositive, startBottomMixedRegion));
				yield return base.StartCoroutine(this.CubemapToEquirectangularCpuNegativeX(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, numPixelsAveraged, startXNegative, endTopMixedRegion, endXNegative, startBottomMixedRegion));
				yield return base.StartCoroutine(this.CubemapToEquirectangularCpuPositiveZ(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, numPixelsAveraged, startZPositive, endTopMixedRegion, endZPositive, startBottomMixedRegion));
				yield return base.StartCoroutine(this.CubemapToEquirectangularCpuNegativeZ(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, numPixelsAveraged, startZNegative, endTopMixedRegion, panoramaWidth, startBottomMixedRegion));
				yield return base.StartCoroutine(this.CubemapToEquirectangularCpuNegativeZ(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, numPixelsAveraged, 0, endTopMixedRegion, endZNegative, startBottomMixedRegion));
				yield return base.StartCoroutine(this.CubemapToEquirectangularCpuGeneralCase(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, maxWidth, maxHeight, numPixelsAveraged, 0, endYPositive, panoramaWidth, endTopMixedRegion));
				yield return base.StartCoroutine(this.CubemapToEquirectangularCpuGeneralCase(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, maxWidth, maxHeight, numPixelsAveraged, 0, startBottomMixedRegion, panoramaWidth, startYNegative));
				if (endZNegative < startXNegative)
				{
					yield return base.StartCoroutine(this.CubemapToEquirectangularCpuGeneralCase(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, maxWidth, maxHeight, numPixelsAveraged, endZNegative, endTopMixedRegion, startXNegative, startBottomMixedRegion));
				}
				if (endXNegative < startZPositive)
				{
					yield return base.StartCoroutine(this.CubemapToEquirectangularCpuGeneralCase(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, maxWidth, maxHeight, numPixelsAveraged, endXNegative, endTopMixedRegion, startZPositive, startBottomMixedRegion));
				}
				if (endZPositive < startXPositive)
				{
					yield return base.StartCoroutine(this.CubemapToEquirectangularCpuGeneralCase(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, maxWidth, maxHeight, numPixelsAveraged, endZPositive, endTopMixedRegion, startXPositive, startBottomMixedRegion));
				}
				if (endXPositive < startZNegative)
				{
					yield return base.StartCoroutine(this.CubemapToEquirectangularCpuGeneralCase(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, maxWidth, maxHeight, numPixelsAveraged, endXPositive, endTopMixedRegion, startZNegative, startBottomMixedRegion));
				}
			}
			else
			{
				IEnumerator enumerator = this.CubemapToEquirectangularCpuPositiveY(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, numPixelsAveraged, 0, 0, panoramaWidth, endYPositive);
				while (enumerator.MoveNext())
				{
				}
				enumerator = this.CubemapToEquirectangularCpuNegativeY(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, numPixelsAveraged, 0, startYNegative, panoramaWidth, panoramaHeight);
				while (enumerator.MoveNext())
				{
				}
				enumerator = this.CubemapToEquirectangularCpuPositiveX(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, numPixelsAveraged, startXPositive, endTopMixedRegion, endXPositive, startBottomMixedRegion);
				while (enumerator.MoveNext())
				{
				}
				enumerator = this.CubemapToEquirectangularCpuNegativeX(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, numPixelsAveraged, startXNegative, endTopMixedRegion, endXNegative, startBottomMixedRegion);
				while (enumerator.MoveNext())
				{
				}
				enumerator = this.CubemapToEquirectangularCpuPositiveZ(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, numPixelsAveraged, startZPositive, endTopMixedRegion, endZPositive, startBottomMixedRegion);
				while (enumerator.MoveNext())
				{
				}
				enumerator = this.CubemapToEquirectangularCpuNegativeZ(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, numPixelsAveraged, startZNegative, endTopMixedRegion, panoramaWidth, startBottomMixedRegion);
				while (enumerator.MoveNext())
				{
				}
				enumerator = this.CubemapToEquirectangularCpuNegativeZ(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, numPixelsAveraged, 0, endTopMixedRegion, endZNegative, startBottomMixedRegion);
				while (enumerator.MoveNext())
				{
				}
				enumerator = this.CubemapToEquirectangularCpuGeneralCase(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, maxWidth, maxHeight, numPixelsAveraged, 0, endYPositive, panoramaWidth, endTopMixedRegion);
				while (enumerator.MoveNext())
				{
				}
				enumerator = this.CubemapToEquirectangularCpuGeneralCase(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, maxWidth, maxHeight, numPixelsAveraged, 0, startBottomMixedRegion, panoramaWidth, startYNegative);
				while (enumerator.MoveNext())
				{
				}
				if (endZNegative < startXNegative)
				{
					enumerator = this.CubemapToEquirectangularCpuGeneralCase(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, maxWidth, maxHeight, numPixelsAveraged, endZNegative, endTopMixedRegion, startXNegative, startBottomMixedRegion);
					while (enumerator.MoveNext())
					{
					}
				}
				if (endXNegative < startZPositive)
				{
					enumerator = this.CubemapToEquirectangularCpuGeneralCase(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, maxWidth, maxHeight, numPixelsAveraged, endXNegative, endTopMixedRegion, startZPositive, startBottomMixedRegion);
					while (enumerator.MoveNext())
					{
					}
				}
				if (endZPositive < startXPositive)
				{
					enumerator = this.CubemapToEquirectangularCpuGeneralCase(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, maxWidth, maxHeight, numPixelsAveraged, endZPositive, endTopMixedRegion, startXPositive, startBottomMixedRegion);
					while (enumerator.MoveNext())
					{
					}
				}
				if (endXPositive < startZNegative)
				{
					enumerator = this.CubemapToEquirectangularCpuGeneralCase(cameraPixels, pixelValues, stride, panoramaWidth, panoramaHeight, ssaaFactor, startTime, processingTimePerFrame, maxWidth, maxHeight, numPixelsAveraged, endXPositive, endTopMixedRegion, startZNegative, startBottomMixedRegion);
					while (enumerator.MoveNext())
					{
					}
				}
			}
			yield return null;
			yield break;
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x000C31C8 File Offset: 0x000C15C8
		private IEnumerator CubemapToEquirectangularCpuPositiveY(uint[] cameraPixels, byte[] pixelValues, int stride, int panoramaWidth, int panoramaHeight, int ssaaFactor, float startTime, float processingTimePerFrame, int numPixelsAveraged, int startX, int startY, int endX, int endY)
		{
			for (int y = startY; y < endY; y++)
			{
				for (int x = startX; x < endX; x++)
				{
					int rTotal = 0;
					int gTotal = 0;
					int bTotal = 0;
					int aTotal = 0;
					for (int i = y * ssaaFactor; i < (y + 1) * ssaaFactor; i++)
					{
						for (int j = x * ssaaFactor; j < (x + 1) * ssaaFactor; j++)
						{
							float num = (float)j / (float)(panoramaWidth * ssaaFactor);
							float num2 = (float)i / (float)(panoramaHeight * ssaaFactor);
							float f = (num2 - 0.5f) * 3.14159274f;
							float f2 = (num * 2f - 1f) * 3.14159274f;
							float num3 = Mathf.Cos(f);
							Vector3 vector = new Vector3(num3 * Mathf.Sin(f2), -Mathf.Sin(f), num3 * Mathf.Cos(f2));
							float num4 = 1f / vector.y;
							float num5 = vector.x * num4;
							float num6 = vector.z * num4;
							num5 = (num5 + 1f) / 2f;
							num6 = (num6 + 1f) / 2f;
							Color32 cameraPixelBilinear = this.GetCameraPixelBilinear(cameraPixels, 2, num5, num6);
							rTotal += (int)cameraPixelBilinear.r;
							gTotal += (int)cameraPixelBilinear.g;
							bTotal += (int)cameraPixelBilinear.b;
							aTotal += (int)cameraPixelBilinear.a;
						}
					}
					int baseIdx = stride * (panoramaHeight - 1 - y) + x * 4;
					pixelValues[baseIdx] = (byte)(bTotal / numPixelsAveraged);
					pixelValues[baseIdx + 1] = (byte)(gTotal / numPixelsAveraged);
					pixelValues[baseIdx + 2] = (byte)(rTotal / numPixelsAveraged);
					pixelValues[baseIdx + 3] = (byte)(aTotal / numPixelsAveraged);
					if ((x & 255) == 0 && Time.realtimeSinceStartup - startTime > processingTimePerFrame)
					{
						yield return null;
						startTime = Time.realtimeSinceStartup;
					}
				}
			}
			yield break;
		}

		// Token: 0x06002754 RID: 10068 RVA: 0x000C3248 File Offset: 0x000C1648
		private IEnumerator CubemapToEquirectangularCpuNegativeY(uint[] cameraPixels, byte[] pixelValues, int stride, int panoramaWidth, int panoramaHeight, int ssaaFactor, float startTime, float processingTimePerFrame, int numPixelsAveraged, int startX, int startY, int endX, int endY)
		{
			for (int y = startY; y < endY; y++)
			{
				for (int x = startX; x < endX; x++)
				{
					int rTotal = 0;
					int gTotal = 0;
					int bTotal = 0;
					int aTotal = 0;
					for (int i = y * ssaaFactor; i < (y + 1) * ssaaFactor; i++)
					{
						for (int j = x * ssaaFactor; j < (x + 1) * ssaaFactor; j++)
						{
							float num = (float)j / (float)(panoramaWidth * ssaaFactor);
							float num2 = (float)i / (float)(panoramaHeight * ssaaFactor);
							float f = (num2 - 0.5f) * 3.14159274f;
							float f2 = (num * 2f - 1f) * 3.14159274f;
							float num3 = Mathf.Cos(f);
							Vector3 vector = new Vector3(num3 * Mathf.Sin(f2), -Mathf.Sin(f), num3 * Mathf.Cos(f2));
							float num4 = 1f / vector.y;
							float num5 = vector.x * num4;
							float num6 = vector.z * num4;
							num5 = -num5;
							num5 = (num5 + 1f) / 2f;
							num6 = (num6 + 1f) / 2f;
							Color32 cameraPixelBilinear = this.GetCameraPixelBilinear(cameraPixels, 3, num5, num6);
							rTotal += (int)cameraPixelBilinear.r;
							gTotal += (int)cameraPixelBilinear.g;
							bTotal += (int)cameraPixelBilinear.b;
							aTotal += (int)cameraPixelBilinear.a;
						}
					}
					int baseIdx = stride * (panoramaHeight - 1 - y) + x * 4;
					pixelValues[baseIdx] = (byte)(bTotal / numPixelsAveraged);
					pixelValues[baseIdx + 1] = (byte)(gTotal / numPixelsAveraged);
					pixelValues[baseIdx + 2] = (byte)(rTotal / numPixelsAveraged);
					pixelValues[baseIdx + 3] = (byte)(aTotal / numPixelsAveraged);
					if ((x & 255) == 0 && Time.realtimeSinceStartup - startTime > processingTimePerFrame)
					{
						yield return null;
						startTime = Time.realtimeSinceStartup;
					}
				}
			}
			yield break;
		}

		// Token: 0x06002755 RID: 10069 RVA: 0x000C32C8 File Offset: 0x000C16C8
		private IEnumerator CubemapToEquirectangularCpuPositiveX(uint[] cameraPixels, byte[] pixelValues, int stride, int panoramaWidth, int panoramaHeight, int ssaaFactor, float startTime, float processingTimePerFrame, int numPixelsAveraged, int startX, int startY, int endX, int endY)
		{
			for (int y = startY; y < endY; y++)
			{
				for (int x = startX; x < endX; x++)
				{
					int rTotal = 0;
					int gTotal = 0;
					int bTotal = 0;
					int aTotal = 0;
					for (int i = y * ssaaFactor; i < (y + 1) * ssaaFactor; i++)
					{
						for (int j = x * ssaaFactor; j < (x + 1) * ssaaFactor; j++)
						{
							float num = (float)j / (float)(panoramaWidth * ssaaFactor);
							float num2 = (float)i / (float)(panoramaHeight * ssaaFactor);
							float f = (num2 - 0.5f) * 3.14159274f;
							float f2 = (num * 2f - 1f) * 3.14159274f;
							float num3 = Mathf.Cos(f);
							Vector3 vector = new Vector3(num3 * Mathf.Sin(f2), -Mathf.Sin(f), num3 * Mathf.Cos(f2));
							float num4 = 1f / vector.x;
							float num5 = -vector.z * num4;
							float num6 = vector.y * num4;
							num6 = -num6;
							num5 = (num5 + 1f) / 2f;
							num6 = (num6 + 1f) / 2f;
							Color32 cameraPixelBilinear = this.GetCameraPixelBilinear(cameraPixels, 0, num5, num6);
							rTotal += (int)cameraPixelBilinear.r;
							gTotal += (int)cameraPixelBilinear.g;
							bTotal += (int)cameraPixelBilinear.b;
							aTotal += (int)cameraPixelBilinear.a;
						}
					}
					int baseIdx = stride * (panoramaHeight - 1 - y) + x * 4;
					pixelValues[baseIdx] = (byte)(bTotal / numPixelsAveraged);
					pixelValues[baseIdx + 1] = (byte)(gTotal / numPixelsAveraged);
					pixelValues[baseIdx + 2] = (byte)(rTotal / numPixelsAveraged);
					pixelValues[baseIdx + 3] = (byte)(aTotal / numPixelsAveraged);
					if ((x & 255) == 0 && Time.realtimeSinceStartup - startTime > processingTimePerFrame)
					{
						yield return null;
						startTime = Time.realtimeSinceStartup;
					}
				}
			}
			yield break;
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x000C3348 File Offset: 0x000C1748
		private IEnumerator CubemapToEquirectangularCpuNegativeX(uint[] cameraPixels, byte[] pixelValues, int stride, int panoramaWidth, int panoramaHeight, int ssaaFactor, float startTime, float processingTimePerFrame, int numPixelsAveraged, int startX, int startY, int endX, int endY)
		{
			for (int y = startY; y < endY; y++)
			{
				for (int x = startX; x < endX; x++)
				{
					int rTotal = 0;
					int gTotal = 0;
					int bTotal = 0;
					int aTotal = 0;
					for (int i = y * ssaaFactor; i < (y + 1) * ssaaFactor; i++)
					{
						for (int j = x * ssaaFactor; j < (x + 1) * ssaaFactor; j++)
						{
							float num = (float)j / (float)(panoramaWidth * ssaaFactor);
							float num2 = (float)i / (float)(panoramaHeight * ssaaFactor);
							float f = (num2 - 0.5f) * 3.14159274f;
							float f2 = (num * 2f - 1f) * 3.14159274f;
							float num3 = Mathf.Cos(f);
							Vector3 vector = new Vector3(num3 * Mathf.Sin(f2), -Mathf.Sin(f), num3 * Mathf.Cos(f2));
							float num4 = 1f / vector.x;
							float num5 = -vector.z * num4;
							float num6 = vector.y * num4;
							num5 = (num5 + 1f) / 2f;
							num6 = (num6 + 1f) / 2f;
							Color32 cameraPixelBilinear = this.GetCameraPixelBilinear(cameraPixels, 1, num5, num6);
							rTotal += (int)cameraPixelBilinear.r;
							gTotal += (int)cameraPixelBilinear.g;
							bTotal += (int)cameraPixelBilinear.b;
							aTotal += (int)cameraPixelBilinear.a;
						}
					}
					int baseIdx = stride * (panoramaHeight - 1 - y) + x * 4;
					pixelValues[baseIdx] = (byte)(bTotal / numPixelsAveraged);
					pixelValues[baseIdx + 1] = (byte)(gTotal / numPixelsAveraged);
					pixelValues[baseIdx + 2] = (byte)(rTotal / numPixelsAveraged);
					pixelValues[baseIdx + 3] = (byte)(aTotal / numPixelsAveraged);
					if ((x & 255) == 0 && Time.realtimeSinceStartup - startTime > processingTimePerFrame)
					{
						yield return null;
						startTime = Time.realtimeSinceStartup;
					}
				}
			}
			yield break;
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x000C33C8 File Offset: 0x000C17C8
		private IEnumerator CubemapToEquirectangularCpuPositiveZ(uint[] cameraPixels, byte[] pixelValues, int stride, int panoramaWidth, int panoramaHeight, int ssaaFactor, float startTime, float processingTimePerFrame, int numPixelsAveraged, int startX, int startY, int endX, int endY)
		{
			for (int y = startY; y < endY; y++)
			{
				for (int x = startX; x < endX; x++)
				{
					int rTotal = 0;
					int gTotal = 0;
					int bTotal = 0;
					int aTotal = 0;
					for (int i = y * ssaaFactor; i < (y + 1) * ssaaFactor; i++)
					{
						for (int j = x * ssaaFactor; j < (x + 1) * ssaaFactor; j++)
						{
							float num = (float)j / (float)(panoramaWidth * ssaaFactor);
							float num2 = (float)i / (float)(panoramaHeight * ssaaFactor);
							float f = (num2 - 0.5f) * 3.14159274f;
							float f2 = (num * 2f - 1f) * 3.14159274f;
							float num3 = Mathf.Cos(f);
							Vector3 vector = new Vector3(num3 * Mathf.Sin(f2), -Mathf.Sin(f), num3 * Mathf.Cos(f2));
							float num4 = 1f / vector.z;
							float num5 = vector.x * num4;
							float num6 = vector.y * num4;
							num6 = -num6;
							num5 = (num5 + 1f) / 2f;
							num6 = (num6 + 1f) / 2f;
							Color32 cameraPixelBilinear = this.GetCameraPixelBilinear(cameraPixels, 4, num5, num6);
							rTotal += (int)cameraPixelBilinear.r;
							gTotal += (int)cameraPixelBilinear.g;
							bTotal += (int)cameraPixelBilinear.b;
							aTotal += (int)cameraPixelBilinear.a;
						}
					}
					int baseIdx = stride * (panoramaHeight - 1 - y) + x * 4;
					pixelValues[baseIdx] = (byte)(bTotal / numPixelsAveraged);
					pixelValues[baseIdx + 1] = (byte)(gTotal / numPixelsAveraged);
					pixelValues[baseIdx + 2] = (byte)(rTotal / numPixelsAveraged);
					pixelValues[baseIdx + 3] = (byte)(aTotal / numPixelsAveraged);
					if ((x & 255) == 0 && Time.realtimeSinceStartup - startTime > processingTimePerFrame)
					{
						yield return null;
						startTime = Time.realtimeSinceStartup;
					}
				}
			}
			yield break;
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x000C3448 File Offset: 0x000C1848
		private IEnumerator CubemapToEquirectangularCpuNegativeZ(uint[] cameraPixels, byte[] pixelValues, int stride, int panoramaWidth, int panoramaHeight, int ssaaFactor, float startTime, float processingTimePerFrame, int numPixelsAveraged, int startX, int startY, int endX, int endY)
		{
			for (int y = startY; y < endY; y++)
			{
				for (int x = startX; x < endX; x++)
				{
					int rTotal = 0;
					int gTotal = 0;
					int bTotal = 0;
					int aTotal = 0;
					for (int i = y * ssaaFactor; i < (y + 1) * ssaaFactor; i++)
					{
						for (int j = x * ssaaFactor; j < (x + 1) * ssaaFactor; j++)
						{
							float num = (float)j / (float)(panoramaWidth * ssaaFactor);
							float num2 = (float)i / (float)(panoramaHeight * ssaaFactor);
							float f = (num2 - 0.5f) * 3.14159274f;
							float f2 = (num * 2f - 1f) * 3.14159274f;
							float num3 = Mathf.Cos(f);
							Vector3 vector = new Vector3(num3 * Mathf.Sin(f2), -Mathf.Sin(f), num3 * Mathf.Cos(f2));
							float num4 = 1f / vector.z;
							float num5 = vector.x * num4;
							float num6 = vector.y * num4;
							num5 = (num5 + 1f) / 2f;
							num6 = (num6 + 1f) / 2f;
							Color32 cameraPixelBilinear = this.GetCameraPixelBilinear(cameraPixels, 5, num5, num6);
							rTotal += (int)cameraPixelBilinear.r;
							gTotal += (int)cameraPixelBilinear.g;
							bTotal += (int)cameraPixelBilinear.b;
							aTotal += (int)cameraPixelBilinear.a;
						}
					}
					int baseIdx = stride * (panoramaHeight - 1 - y) + x * 4;
					pixelValues[baseIdx] = (byte)(bTotal / numPixelsAveraged);
					pixelValues[baseIdx + 1] = (byte)(gTotal / numPixelsAveraged);
					pixelValues[baseIdx + 2] = (byte)(rTotal / numPixelsAveraged);
					pixelValues[baseIdx + 3] = (byte)(aTotal / numPixelsAveraged);
					if ((x & 255) == 0 && Time.realtimeSinceStartup - startTime > processingTimePerFrame)
					{
						yield return null;
						startTime = Time.realtimeSinceStartup;
					}
				}
			}
			yield break;
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x000C34C8 File Offset: 0x000C18C8
		private IEnumerator CubemapToEquirectangularCpuGeneralCase(uint[] cameraPixels, byte[] pixelValues, int stride, int panoramaWidth, int panoramaHeight, int ssaaFactor, float startTime, float processingTimePerFrame, float maxWidth, float maxHeight, int numPixelsAveraged, int startX, int startY, int endX, int endY)
		{
			for (int y = startY; y < endY; y++)
			{
				for (int x = startX; x < endX; x++)
				{
					int rTotal = 0;
					int gTotal = 0;
					int bTotal = 0;
					int aTotal = 0;
					for (int i = y * ssaaFactor; i < (y + 1) * ssaaFactor; i++)
					{
						for (int j = x * ssaaFactor; j < (x + 1) * ssaaFactor; j++)
						{
							float num = (float)j / (float)(panoramaWidth * ssaaFactor);
							float num2 = (float)i / (float)(panoramaHeight * ssaaFactor);
							float f = (num2 - 0.5f) * 3.14159274f;
							float f2 = (num * 2f - 1f) * 3.14159274f;
							float num3 = Mathf.Cos(f);
							Vector3 vector = new Vector3(num3 * Mathf.Sin(f2), -Mathf.Sin(f), num3 * Mathf.Cos(f2));
							float num4 = 1f / vector.y;
							float num5 = vector.x * num4;
							float num6 = vector.z * num4;
							CubemapFace cameraNum;
							if (vector.y > 0f)
							{
								cameraNum = CubemapFace.PositiveY;
							}
							else
							{
								cameraNum = CubemapFace.NegativeY;
								num5 = -num5;
							}
							if (Mathf.Abs(num5) > 1f || Mathf.Abs(num6) > 1f)
							{
								num4 = 1f / vector.x;
								num5 = -vector.z * num4;
								num6 = vector.y * num4;
								if (vector.x > 0f)
								{
									cameraNum = CubemapFace.PositiveX;
									num6 = -num6;
								}
								else
								{
									cameraNum = CubemapFace.NegativeX;
								}
							}
							if (Mathf.Abs(num5) > 1f || Mathf.Abs(num6) > 1f)
							{
								num4 = 1f / vector.z;
								num5 = vector.x * num4;
								num6 = vector.y * num4;
								if (vector.z > 0f)
								{
									cameraNum = CubemapFace.PositiveZ;
									num6 = -num6;
								}
								else
								{
									cameraNum = CubemapFace.NegativeZ;
								}
							}
							num5 = (num5 + 1f) / 2f;
							num6 = (num6 + 1f) / 2f;
							num5 = Mathf.Min(num5, maxWidth);
							num6 = Mathf.Min(num6, maxHeight);
							Color32 cameraPixelBilinear = this.GetCameraPixelBilinear(cameraPixels, (int)cameraNum, num5, num6);
							rTotal += (int)cameraPixelBilinear.r;
							gTotal += (int)cameraPixelBilinear.g;
							bTotal += (int)cameraPixelBilinear.b;
							aTotal += (int)cameraPixelBilinear.a;
						}
					}
					int baseIdx = stride * (panoramaHeight - 1 - y) + x * 4;
					pixelValues[baseIdx] = (byte)(bTotal / numPixelsAveraged);
					pixelValues[baseIdx + 1] = (byte)(gTotal / numPixelsAveraged);
					pixelValues[baseIdx + 2] = (byte)(rTotal / numPixelsAveraged);
					pixelValues[baseIdx + 3] = (byte)(aTotal / numPixelsAveraged);
					if ((x & 255) == 0 && Time.realtimeSinceStartup - startTime > processingTimePerFrame)
					{
						yield return null;
						startTime = Time.realtimeSinceStartup;
					}
				}
			}
			yield break;
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x000C3558 File Offset: 0x000C1958
		protected void PlaySound(AudioClip clip)
		{
			if (this.audioSource != null && clip != null)
			{
				this.audioSource.PlayOneShot(clip);
			}
		}

		// Token: 0x04001487 RID: 5255
		public const string VERSION = "0.1";

		// Token: 0x04001488 RID: 5256
		private const string PRE_CAPTURE_METHOD = "OnPrePanoCapture";

		// Token: 0x04001489 RID: 5257
		private const string POST_CAPTURE_METHOD = "OnPostPanoCapture";

		// Token: 0x0400148A RID: 5258
		public static Action<string> OnShortURLGetted;

		// Token: 0x0400148B RID: 5259
		public Text status;

		// Token: 0x0400148C RID: 5260
		public string panoramaName;

		// Token: 0x0400148D RID: 5261
		[HideInInspector]
		[NonSerialized]
		public string qualitySetting = "VRHigh";

		// Token: 0x0400148E RID: 5262
		public KeyCode defaultCaptureKey = KeyCode.P;

		// Token: 0x0400148F RID: 5263
		public KeyCode selfieCaptureKey = KeyCode.O;

		// Token: 0x04001490 RID: 5264
		public float lockTimeDelayAfterCaptureProcessingDone = 8f;

		// Token: 0x04001491 RID: 5265
		public Eyeshot360cam.ImageFormat imageFormat;

		// Token: 0x04001492 RID: 5266
		[HideInInspector]
		[NonSerialized]
		public Eyeshot360cam.PanoramaFormat panoramaFormat;

		// Token: 0x04001493 RID: 5267
		public bool captureStereoscopic = true;

		// Token: 0x04001494 RID: 5268
		public float interpupillaryDistance = 0.0635f;

		// Token: 0x04001495 RID: 5269
		[HideInInspector]
		[NonSerialized]
		public int numCirclePoints = 128;

		// Token: 0x04001496 RID: 5270
		public int panoramaWidth = 8192;

		// Token: 0x04001497 RID: 5271
		public Eyeshot360cam.AntiAliasing antiAliasing = Eyeshot360cam.AntiAliasing._8;

		// Token: 0x04001498 RID: 5272
		[HideInInspector]
		[NonSerialized]
		public int ssaaFactor = 1;

		// Token: 0x04001499 RID: 5273
		[HideInInspector]
		[NonSerialized]
		public bool depthMap;

		// Token: 0x0400149A RID: 5274
		[HideInInspector]
		[Range(0f, 3f)]
		[NonSerialized]
		public float depthLevel = 1f;

		// Token: 0x0400149B RID: 5275
		[HideInInspector]
		[NonSerialized]
		public float depthFar = 100f;

		// Token: 0x0400149C RID: 5276
		public bool useDefaultOrientation = true;

		// Token: 0x0400149D RID: 5277
		[HideInInspector]
		[NonSerialized]
		public bool useGPUTransform = true;

		// Token: 0x0400149E RID: 5278
		[HideInInspector]
		[NonSerialized]
		public float cpuMillisecondsPerFrame = 8.333333f;

		// Token: 0x0400149F RID: 5279
		[HideInInspector]
		[NonSerialized]
		public bool captureEveryFrame;

		// Token: 0x040014A0 RID: 5280
		[HideInInspector]
		[NonSerialized]
		public int frameRate = 30;

		// Token: 0x040014A1 RID: 5281
		[HideInInspector]
		[NonSerialized]
		public int maxFramesToRecord;

		// Token: 0x040014A2 RID: 5282
		[HideInInspector]
		[NonSerialized]
		public int frameNumberDigits = 6;

		// Token: 0x040014A3 RID: 5283
		public bool enableDebugging;

		// Token: 0x040014A4 RID: 5284
		public string apiUrl = "http://vrchive.com/gameapi/1/";

		// Token: 0x040014A5 RID: 5285
		[TextArea(3, 3)]
		public string apiKey = string.Empty;

		// Token: 0x040014A6 RID: 5286
		[TextArea(3, 3)]
		public string userToken = string.Empty;

		// Token: 0x040014A7 RID: 5287
		[HideInInspector]
		public string saveImagePath = string.Empty;

		// Token: 0x040014A8 RID: 5288
		[HideInInspector]
		[NonSerialized]
		public bool saveCubemap;

		// Token: 0x040014A9 RID: 5289
		[HideInInspector]
		[NonSerialized]
		public bool saveShortUrl;

		// Token: 0x040014AA RID: 5290
		public bool uploadImages = true;

		// Token: 0x040014AB RID: 5291
		public AudioClip startSound;

		// Token: 0x040014AC RID: 5292
		public AudioClip doneSound;

		// Token: 0x040014AD RID: 5293
		public AudioClip failSound;

		// Token: 0x040014AE RID: 5294
		public bool fadeDuringCapture = true;

		// Token: 0x040014AF RID: 5295
		public float fadeTime = 0.25f;

		// Token: 0x040014B0 RID: 5296
		public UnityEngine.Color fadeColor = new UnityEngine.Color(0f, 0f, 0f, 1f);

		// Token: 0x040014B1 RID: 5297
		public Material fadeMaterial;

		// Token: 0x040014B2 RID: 5298
		public ComputeShader convertPanoramaShader;

		// Token: 0x040014B3 RID: 5299
		public ComputeShader convertPanoramaStereoShader;

		// Token: 0x040014B4 RID: 5300
		public ComputeShader textureToBufferShader;

		// Token: 0x040014B5 RID: 5301
		[HideInInspector]
		public Eyeshot360cam.Metadata metadata = default(Eyeshot360cam.Metadata);

		// Token: 0x040014B6 RID: 5302
		public Transform selfieCamera;

		// Token: 0x040014B7 RID: 5303
		public Transform[] additionalCameras;

		// Token: 0x040014B8 RID: 5304
		private const int RESULT_BUFFER_SLICES = 8;

		// Token: 0x040014B9 RID: 5305
		private const int CAMERAS_PER_CIRCLE_POINT = 4;

		// Token: 0x040014BA RID: 5306
		private const uint BUFFER_SENTINEL_VALUE = 1419455993u;

		// Token: 0x040014BB RID: 5307
		private static Eyeshot360cam instance;

		// Token: 0x040014BC RID: 5308
		protected static bool capturing;

		// Token: 0x040014BD RID: 5309
		private GameObject[] camGos;

		// Token: 0x040014BE RID: 5310
		private Camera renderCamera;

		// Token: 0x040014BF RID: 5311
		private RenderDepth renderDepth;

		// Token: 0x040014C0 RID: 5312
		private ImageEffectCopyCamera copyCameraScript;

		// Token: 0x040014C1 RID: 5313
		private bool capturingEveryFrame;

		// Token: 0x040014C2 RID: 5314
		private bool usingGpuTransform;

		// Token: 0x040014C3 RID: 5315
		private CubemapFace[] faces;

		// Token: 0x040014C4 RID: 5316
		private int panoramaHeight;

		// Token: 0x040014C5 RID: 5317
		private int cameraHeight;

		// Token: 0x040014C6 RID: 5318
		private int cameraWidth;

		// Token: 0x040014C7 RID: 5319
		private RenderTexture cubemapRenderTexture;

		// Token: 0x040014C8 RID: 5320
		private RenderTexture cubemapDepthRenderTexture;

		// Token: 0x040014C9 RID: 5321
		private Texture2D forceWaitTexture;

		// Token: 0x040014CA RID: 5322
		private int convertPanoramaKernelIdx = -1;

		// Token: 0x040014CB RID: 5323
		private int convertPanoramaYPositiveKernelIdx = -1;

		// Token: 0x040014CC RID: 5324
		private int convertPanoramaYNegativeKernelIdx = -1;

		// Token: 0x040014CD RID: 5325
		private int textureToBufferIdx = -1;

		// Token: 0x040014CE RID: 5326
		private int renderStereoIdx = -1;

		// Token: 0x040014CF RID: 5327
		private int[] convertPanoramaKernelIdxs;

		// Token: 0x040014D0 RID: 5328
		private byte[] imageFileBytes;

		// Token: 0x040014D1 RID: 5329
		private string videoBaseName = string.Empty;

		// Token: 0x040014D2 RID: 5330
		private int frameNumber;

		// Token: 0x040014D3 RID: 5331
		private float hFov = -1f;

		// Token: 0x040014D4 RID: 5332
		private float vFov = -1f;

		// Token: 0x040014D5 RID: 5333
		private float hFovAdjustDegrees = -1f;

		// Token: 0x040014D6 RID: 5334
		private float vFovAdjustDegrees = -1f;

		// Token: 0x040014D7 RID: 5335
		private float circleRadius = -1f;

		// Token: 0x040014D8 RID: 5336
		private int threadsX = 32;

		// Token: 0x040014D9 RID: 5337
		private int threadsY = 32;

		// Token: 0x040014DA RID: 5338
		private int numCameras;

		// Token: 0x040014DB RID: 5339
		private uint[] cameraPixels;

		// Token: 0x040014DC RID: 5340
		private uint[] resultPixels;

		// Token: 0x040014DD RID: 5341
		private float tanHalfHFov;

		// Token: 0x040014DE RID: 5342
		private float tanHalfVFov;

		// Token: 0x040014DF RID: 5343
		private float hFovAdjust;

		// Token: 0x040014E0 RID: 5344
		private float vFovAdjust;

		// Token: 0x040014E1 RID: 5345
		private int overlapTextures;

		// Token: 0x040014E2 RID: 5346
		protected bool initializeFailed = true;

		// Token: 0x040014E3 RID: 5347
		private AudioSource audioSource;

		// Token: 0x040014E4 RID: 5348
		protected Eyeshot360cam.Configuration lastConfig;

		// Token: 0x040014E5 RID: 5349
		private float? startTimeOfAllTriggersPressed;

		// Token: 0x040014E6 RID: 5350
		private float? startTimeOfAllGripsPressed;

		// Token: 0x040014E7 RID: 5351
		private float? lastTimeOfCapturingDone;

		// Token: 0x040014E8 RID: 5352
		private string[] ECapturePanoramaFormatToPostParameterValue = new string[]
		{
			"longlat",
			"stereolonglat",
			"cube",
			"stereocube",
			"cube_front",
			"cube_back",
			"cube_right",
			"cube_left",
			"cube_top",
			"cube_bottom",
			"right_cube_front",
			"right_cube_back",
			"right_cube_right",
			"right_cube_left",
			"right_cube_top",
			"right_cube_bottom"
		};

		// Token: 0x02000464 RID: 1124
		[Serializable]
		public struct Metadata
		{
			// Token: 0x040014EB RID: 5355
			public string game;

			// Token: 0x040014EC RID: 5356
			public string username;

			// Token: 0x040014ED RID: 5357
			public string developerUrl;

			// Token: 0x040014EE RID: 5358
			public Vector3 lookDirection;

			// Token: 0x040014EF RID: 5359
			public float eyeDistance;

			// Token: 0x040014F0 RID: 5360
			public string roomTitle;

			// Token: 0x040014F1 RID: 5361
			public string gameDescription;

			// Token: 0x040014F2 RID: 5362
			public List<string> tags;

			// Token: 0x040014F3 RID: 5363
			public List<string> people;

			// Token: 0x040014F4 RID: 5364
			public string albumName;

			// Token: 0x040014F5 RID: 5365
			public bool uploadSessionToAlbum;

			// Token: 0x040014F6 RID: 5366
			public bool isPrivateAlbum;
		}

		// Token: 0x02000465 RID: 1125
		protected struct Configuration
		{
			// Token: 0x040014F7 RID: 5367
			public int panoramaWidth;

			// Token: 0x040014F8 RID: 5368
			public int numCirclePoints;

			// Token: 0x040014F9 RID: 5369
			public int ssaaFactor;

			// Token: 0x040014FA RID: 5370
			public float interpupillaryDistance;

			// Token: 0x040014FB RID: 5371
			public bool captureStereoscopic;

			// Token: 0x040014FC RID: 5372
			public bool saveCubemap;

			// Token: 0x040014FD RID: 5373
			public bool useGpuTransform;

			// Token: 0x040014FE RID: 5374
			public Eyeshot360cam.AntiAliasing antiAliasing;
		}

		// Token: 0x02000466 RID: 1126
		public enum PanoramaFormat
		{
			// Token: 0x04001500 RID: 5376
			LongLatUnwrap,
			// Token: 0x04001501 RID: 5377
			CubeUnwrap
		}

		// Token: 0x02000467 RID: 1127
		public enum ImageFormat
		{
			// Token: 0x04001503 RID: 5379
			PNG,
			// Token: 0x04001504 RID: 5380
			JPEG,
			// Token: 0x04001505 RID: 5381
			BMP
		}

		// Token: 0x02000468 RID: 1128
		public enum AntiAliasing
		{
			// Token: 0x04001507 RID: 5383
			_1 = 1,
			// Token: 0x04001508 RID: 5384
			_2,
			// Token: 0x04001509 RID: 5385
			_4 = 4,
			// Token: 0x0400150A RID: 5386
			_8 = 8
		}

		// Token: 0x02000469 RID: 1129
		private enum EEyeshotPanoramaFormat
		{
			// Token: 0x0400150C RID: 5388
			CPF_LongLat,
			// Token: 0x0400150D RID: 5389
			CPF_StereoLongLat,
			// Token: 0x0400150E RID: 5390
			CPF_Cube,
			// Token: 0x0400150F RID: 5391
			CPF_StereoCube,
			// Token: 0x04001510 RID: 5392
			CPF_CubeSideFront,
			// Token: 0x04001511 RID: 5393
			CPF_CubeSideBack,
			// Token: 0x04001512 RID: 5394
			CPF_CubeSideRight,
			// Token: 0x04001513 RID: 5395
			CPF_CubeSideLeft,
			// Token: 0x04001514 RID: 5396
			CPF_CubeSideTop,
			// Token: 0x04001515 RID: 5397
			CPF_CubeSideBottom,
			// Token: 0x04001516 RID: 5398
			CPF_RightCubeSideFront,
			// Token: 0x04001517 RID: 5399
			CPF_RightCubeSideBack,
			// Token: 0x04001518 RID: 5400
			CPF_RightCubeSideRight,
			// Token: 0x04001519 RID: 5401
			CPF_RightCubeSideLeft,
			// Token: 0x0400151A RID: 5402
			CPF_RightCubeSideTop,
			// Token: 0x0400151B RID: 5403
			CPF_RightCubeSideBottom,
			// Token: 0x0400151C RID: 5404
			LastItem
		}
	}
}
