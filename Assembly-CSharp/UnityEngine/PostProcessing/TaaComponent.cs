using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020007EA RID: 2026
	public sealed class TaaComponent : PostProcessingComponentRenderTexture<AntialiasingModel>
	{
		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x060040A9 RID: 16553 RVA: 0x00147068 File Offset: 0x00145468
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.method == AntialiasingModel.Method.Taa && SystemInfo.supportsMotionVectors && SystemInfo.supportedRenderTargetCount >= 2 && !this.context.interrupted;
			}
		}

		// Token: 0x060040AA RID: 16554 RVA: 0x001470C4 File Offset: 0x001454C4
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x060040AB RID: 16555 RVA: 0x001470C7 File Offset: 0x001454C7
		// (set) Token: 0x060040AC RID: 16556 RVA: 0x001470CF File Offset: 0x001454CF
		public Vector2 jitterVector { get; private set; }

		// Token: 0x060040AD RID: 16557 RVA: 0x001470D8 File Offset: 0x001454D8
		public void ResetHistory()
		{
			this.m_ResetHistory = true;
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x001470E4 File Offset: 0x001454E4
		public void SetProjectionMatrix(Func<Vector2, Matrix4x4> jitteredFunc)
		{
			AntialiasingModel.TaaSettings taaSettings = base.model.settings.taaSettings;
			Vector2 vector = this.GenerateRandomOffset();
			vector *= taaSettings.jitterSpread;
			this.context.camera.nonJitteredProjectionMatrix = this.context.camera.projectionMatrix;
			if (jitteredFunc != null)
			{
				this.context.camera.projectionMatrix = jitteredFunc(vector);
			}
			else
			{
				this.context.camera.projectionMatrix = ((!this.context.camera.orthographic) ? this.GetPerspectiveProjectionMatrix(vector) : this.GetOrthographicProjectionMatrix(vector));
			}
			this.context.camera.useJitteredProjectionMatrixForTransparentRendering = false;
			vector.x /= (float)this.context.width;
			vector.y /= (float)this.context.height;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Temporal Anti-aliasing");
			material.SetVector(TaaComponent.Uniforms._Jitter, vector);
			this.jitterVector = vector;
		}

		// Token: 0x060040AF RID: 16559 RVA: 0x00147208 File Offset: 0x00145608
		public void Render(RenderTexture source, RenderTexture destination)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Temporal Anti-aliasing");
			material.shaderKeywords = null;
			AntialiasingModel.TaaSettings taaSettings = base.model.settings.taaSettings;
			if (this.m_ResetHistory || this.m_HistoryTexture == null || this.m_HistoryTexture.width != source.width || this.m_HistoryTexture.height != source.height)
			{
				if (this.m_HistoryTexture)
				{
					RenderTexture.ReleaseTemporary(this.m_HistoryTexture);
				}
				this.m_HistoryTexture = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
				this.m_HistoryTexture.name = "TAA History";
				Graphics.Blit(source, this.m_HistoryTexture, material, 2);
			}
			material.SetVector(TaaComponent.Uniforms._SharpenParameters, new Vector4(taaSettings.sharpen, 0f, 0f, 0f));
			material.SetVector(TaaComponent.Uniforms._FinalBlendParameters, new Vector4(taaSettings.stationaryBlending, taaSettings.motionBlending, 6000f, 0f));
			material.SetTexture(TaaComponent.Uniforms._MainTex, source);
			material.SetTexture(TaaComponent.Uniforms._HistoryTex, this.m_HistoryTexture);
			RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
			temporary.name = "TAA History";
			this.m_MRT[0] = destination.colorBuffer;
			this.m_MRT[1] = temporary.colorBuffer;
			Graphics.SetRenderTarget(this.m_MRT, source.depthBuffer);
			GraphicsUtils.Blit(material, (!this.context.camera.orthographic) ? 0 : 1);
			RenderTexture.ReleaseTemporary(this.m_HistoryTexture);
			this.m_HistoryTexture = temporary;
			this.m_ResetHistory = false;
		}

		// Token: 0x060040B0 RID: 16560 RVA: 0x001473F0 File Offset: 0x001457F0
		private float GetHaltonValue(int index, int radix)
		{
			float num = 0f;
			float num2 = 1f / (float)radix;
			while (index > 0)
			{
				num += (float)(index % radix) * num2;
				index /= radix;
				num2 /= (float)radix;
			}
			return num;
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x0014742C File Offset: 0x0014582C
		private Vector2 GenerateRandomOffset()
		{
			Vector2 result = new Vector2(this.GetHaltonValue(this.m_SampleIndex & 1023, 2), this.GetHaltonValue(this.m_SampleIndex & 1023, 3));
			if (++this.m_SampleIndex >= 8)
			{
				this.m_SampleIndex = 0;
			}
			return result;
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x00147488 File Offset: 0x00145888
		private Matrix4x4 GetPerspectiveProjectionMatrix(Vector2 offset)
		{
			float num = Mathf.Tan(0.008726646f * this.context.camera.fieldOfView);
			float num2 = num * this.context.camera.aspect;
			offset.x *= num2 / (0.5f * (float)this.context.width);
			offset.y *= num / (0.5f * (float)this.context.height);
			float num3 = (offset.x - num2) * this.context.camera.nearClipPlane;
			float num4 = (offset.x + num2) * this.context.camera.nearClipPlane;
			float num5 = (offset.y + num) * this.context.camera.nearClipPlane;
			float num6 = (offset.y - num) * this.context.camera.nearClipPlane;
			Matrix4x4 result = default(Matrix4x4);
			result[0, 0] = 2f * this.context.camera.nearClipPlane / (num4 - num3);
			result[0, 1] = 0f;
			result[0, 2] = (num4 + num3) / (num4 - num3);
			result[0, 3] = 0f;
			result[1, 0] = 0f;
			result[1, 1] = 2f * this.context.camera.nearClipPlane / (num5 - num6);
			result[1, 2] = (num5 + num6) / (num5 - num6);
			result[1, 3] = 0f;
			result[2, 0] = 0f;
			result[2, 1] = 0f;
			result[2, 2] = -(this.context.camera.farClipPlane + this.context.camera.nearClipPlane) / (this.context.camera.farClipPlane - this.context.camera.nearClipPlane);
			result[2, 3] = -(2f * this.context.camera.farClipPlane * this.context.camera.nearClipPlane) / (this.context.camera.farClipPlane - this.context.camera.nearClipPlane);
			result[3, 0] = 0f;
			result[3, 1] = 0f;
			result[3, 2] = -1f;
			result[3, 3] = 0f;
			return result;
		}

		// Token: 0x060040B3 RID: 16563 RVA: 0x00147718 File Offset: 0x00145B18
		private Matrix4x4 GetOrthographicProjectionMatrix(Vector2 offset)
		{
			float orthographicSize = this.context.camera.orthographicSize;
			float num = orthographicSize * this.context.camera.aspect;
			offset.x *= num / (0.5f * (float)this.context.width);
			offset.y *= orthographicSize / (0.5f * (float)this.context.height);
			float left = offset.x - num;
			float right = offset.x + num;
			float top = offset.y + orthographicSize;
			float bottom = offset.y - orthographicSize;
			return Matrix4x4.Ortho(left, right, bottom, top, this.context.camera.nearClipPlane, this.context.camera.farClipPlane);
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x001477E2 File Offset: 0x00145BE2
		public override void OnDisable()
		{
			if (this.m_HistoryTexture != null)
			{
				RenderTexture.ReleaseTemporary(this.m_HistoryTexture);
			}
			this.m_HistoryTexture = null;
			this.m_SampleIndex = 0;
			this.ResetHistory();
		}

		// Token: 0x0400294D RID: 10573
		private const string k_ShaderString = "Hidden/Post FX/Temporal Anti-aliasing";

		// Token: 0x0400294E RID: 10574
		private const int k_SampleCount = 8;

		// Token: 0x0400294F RID: 10575
		private readonly RenderBuffer[] m_MRT = new RenderBuffer[2];

		// Token: 0x04002950 RID: 10576
		private int m_SampleIndex;

		// Token: 0x04002951 RID: 10577
		private bool m_ResetHistory = true;

		// Token: 0x04002952 RID: 10578
		private RenderTexture m_HistoryTexture;

		// Token: 0x020007EB RID: 2027
		private static class Uniforms
		{
			// Token: 0x04002954 RID: 10580
			internal static int _Jitter = Shader.PropertyToID("_Jitter");

			// Token: 0x04002955 RID: 10581
			internal static int _SharpenParameters = Shader.PropertyToID("_SharpenParameters");

			// Token: 0x04002956 RID: 10582
			internal static int _FinalBlendParameters = Shader.PropertyToID("_FinalBlendParameters");

			// Token: 0x04002957 RID: 10583
			internal static int _HistoryTex = Shader.PropertyToID("_HistoryTex");

			// Token: 0x04002958 RID: 10584
			internal static int _MainTex = Shader.PropertyToID("_MainTex");
		}
	}
}
