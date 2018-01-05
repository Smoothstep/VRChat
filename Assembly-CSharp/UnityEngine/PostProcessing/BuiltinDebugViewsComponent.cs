using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020007CD RID: 1997
	public sealed class BuiltinDebugViewsComponent : PostProcessingComponentCommandBuffer<BuiltinDebugViewsModel>
	{
		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x0600402D RID: 16429 RVA: 0x00142E9F File Offset: 0x0014129F
		public override bool active
		{
			get
			{
				return base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Depth) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Normals) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.MotionVectors);
			}
		}

		// Token: 0x0600402E RID: 16430 RVA: 0x00142ED4 File Offset: 0x001412D4
		public override DepthTextureMode GetCameraFlags()
		{
			BuiltinDebugViewsModel.Mode mode = base.model.settings.mode;
			DepthTextureMode depthTextureMode = DepthTextureMode.None;
			if (mode != BuiltinDebugViewsModel.Mode.Normals)
			{
				if (mode != BuiltinDebugViewsModel.Mode.MotionVectors)
				{
					if (mode == BuiltinDebugViewsModel.Mode.Depth)
					{
						depthTextureMode |= DepthTextureMode.Depth;
					}
				}
				else
				{
					depthTextureMode |= (DepthTextureMode.Depth | DepthTextureMode.MotionVectors);
				}
			}
			else
			{
				depthTextureMode |= DepthTextureMode.DepthNormals;
			}
			return depthTextureMode;
		}

		// Token: 0x0600402F RID: 16431 RVA: 0x00142F30 File Offset: 0x00141330
		public override CameraEvent GetCameraEvent()
		{
			return (base.model.settings.mode != BuiltinDebugViewsModel.Mode.MotionVectors) ? CameraEvent.BeforeImageEffectsOpaque : CameraEvent.BeforeImageEffects;
		}

		// Token: 0x06004030 RID: 16432 RVA: 0x00142F5F File Offset: 0x0014135F
		public override string GetName()
		{
			return "Builtin Debug Views";
		}

		// Token: 0x06004031 RID: 16433 RVA: 0x00142F68 File Offset: 0x00141368
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			BuiltinDebugViewsModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			material.shaderKeywords = null;
			if (this.context.isGBufferAvailable)
			{
				material.EnableKeyword("SOURCE_GBUFFER");
			}
			BuiltinDebugViewsModel.Mode mode = settings.mode;
			if (mode != BuiltinDebugViewsModel.Mode.Depth)
			{
				if (mode != BuiltinDebugViewsModel.Mode.Normals)
				{
					if (mode == BuiltinDebugViewsModel.Mode.MotionVectors)
					{
						this.MotionVectorsPass(cb);
					}
				}
				else
				{
					this.DepthNormalsPass(cb);
				}
			}
			else
			{
				this.DepthPass(cb);
			}
			this.context.Interrupt();
		}

		// Token: 0x06004032 RID: 16434 RVA: 0x0014300C File Offset: 0x0014140C
		private void DepthPass(CommandBuffer cb)
		{
			Material mat = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.DepthSettings depth = base.model.settings.depth;
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._DepthScale, 1f / depth.scale);
			cb.Blit(null, BuiltinRenderTextureType.CameraTarget, mat, 0);
		}

		// Token: 0x06004033 RID: 16435 RVA: 0x0014306C File Offset: 0x0014146C
		private void DepthNormalsPass(CommandBuffer cb)
		{
			Material mat = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			cb.Blit(null, BuiltinRenderTextureType.CameraTarget, mat, 1);
		}

		// Token: 0x06004034 RID: 16436 RVA: 0x001430A0 File Offset: 0x001414A0
		private void MotionVectorsPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.MotionVectorsSettings motionVectors = base.model.settings.motionVectors;
			int nameID = BuiltinDebugViewsComponent.Uniforms._TempRT;
			cb.GetTemporaryRT(nameID, this.context.width, this.context.height, 0, FilterMode.Bilinear);
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.sourceOpacity);
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, BuiltinRenderTextureType.CameraTarget);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, nameID, material, 2);
			if (motionVectors.motionImageOpacity > 0f && motionVectors.motionImageAmplitude > 0f)
			{
				int tempRT = BuiltinDebugViewsComponent.Uniforms._TempRT2;
				cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, FilterMode.Bilinear);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionImageOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionImageAmplitude);
				cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, nameID);
				cb.Blit(nameID, tempRT, material, 3);
				cb.ReleaseTemporaryRT(nameID);
				nameID = tempRT;
			}
			if (motionVectors.motionVectorsOpacity > 0f && motionVectors.motionVectorsAmplitude > 0f)
			{
				this.PrepareArrows();
				float num = 1f / (float)motionVectors.motionVectorsResolution;
				float x = num * (float)this.context.height / (float)this.context.width;
				cb.SetGlobalVector(BuiltinDebugViewsComponent.Uniforms._Scale, new Vector2(x, num));
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionVectorsOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionVectorsAmplitude);
				cb.DrawMesh(this.m_Arrows.mesh, Matrix4x4.identity, material, 0, 4);
			}
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, nameID);
			cb.Blit(nameID, BuiltinRenderTextureType.CameraTarget);
			cb.ReleaseTemporaryRT(nameID);
		}

		// Token: 0x06004035 RID: 16437 RVA: 0x001432A8 File Offset: 0x001416A8
		private void PrepareArrows()
		{
			int motionVectorsResolution = base.model.settings.motionVectors.motionVectorsResolution;
			int num = motionVectorsResolution * Screen.width / Screen.height;
			if (this.m_Arrows == null)
			{
				this.m_Arrows = new BuiltinDebugViewsComponent.ArrowArray();
			}
			if (this.m_Arrows.columnCount != num || this.m_Arrows.rowCount != motionVectorsResolution)
			{
				this.m_Arrows.Release();
				this.m_Arrows.BuildMesh(num, motionVectorsResolution);
			}
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x0014332C File Offset: 0x0014172C
		public override void OnDisable()
		{
			if (this.m_Arrows != null)
			{
				this.m_Arrows.Release();
			}
			this.m_Arrows = null;
		}

		// Token: 0x0400288B RID: 10379
		private const string k_ShaderString = "Hidden/Post FX/Builtin Debug Views";

		// Token: 0x0400288C RID: 10380
		private BuiltinDebugViewsComponent.ArrowArray m_Arrows;

		// Token: 0x020007CE RID: 1998
		private static class Uniforms
		{
			// Token: 0x0400288D RID: 10381
			internal static readonly int _DepthScale = Shader.PropertyToID("_DepthScale");

			// Token: 0x0400288E RID: 10382
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");

			// Token: 0x0400288F RID: 10383
			internal static readonly int _Opacity = Shader.PropertyToID("_Opacity");

			// Token: 0x04002890 RID: 10384
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x04002891 RID: 10385
			internal static readonly int _TempRT2 = Shader.PropertyToID("_TempRT2");

			// Token: 0x04002892 RID: 10386
			internal static readonly int _Amplitude = Shader.PropertyToID("_Amplitude");

			// Token: 0x04002893 RID: 10387
			internal static readonly int _Scale = Shader.PropertyToID("_Scale");
		}

		// Token: 0x020007CF RID: 1999
		private enum Pass
		{
			// Token: 0x04002895 RID: 10389
			Depth,
			// Token: 0x04002896 RID: 10390
			Normals,
			// Token: 0x04002897 RID: 10391
			MovecOpacity,
			// Token: 0x04002898 RID: 10392
			MovecImaging,
			// Token: 0x04002899 RID: 10393
			MovecArrows
		}

		// Token: 0x020007D0 RID: 2000
		private class ArrowArray
		{
			// Token: 0x17000A2B RID: 2603
			// (get) Token: 0x06004039 RID: 16441 RVA: 0x001433CA File Offset: 0x001417CA
			// (set) Token: 0x0600403A RID: 16442 RVA: 0x001433D2 File Offset: 0x001417D2
			public Mesh mesh { get; private set; }

			// Token: 0x17000A2C RID: 2604
			// (get) Token: 0x0600403B RID: 16443 RVA: 0x001433DB File Offset: 0x001417DB
			// (set) Token: 0x0600403C RID: 16444 RVA: 0x001433E3 File Offset: 0x001417E3
			public int columnCount { get; private set; }

			// Token: 0x17000A2D RID: 2605
			// (get) Token: 0x0600403D RID: 16445 RVA: 0x001433EC File Offset: 0x001417EC
			// (set) Token: 0x0600403E RID: 16446 RVA: 0x001433F4 File Offset: 0x001417F4
			public int rowCount { get; private set; }

			// Token: 0x0600403F RID: 16447 RVA: 0x00143400 File Offset: 0x00141800
			public void BuildMesh(int columns, int rows)
			{
				Vector3[] array = new Vector3[]
				{
					new Vector3(0f, 0f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(-1f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(1f, 1f, 0f)
				};
				int num = 6 * columns * rows;
				List<Vector3> list = new List<Vector3>(num);
				List<Vector2> list2 = new List<Vector2>(num);
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < columns; j++)
					{
						Vector2 item = new Vector2((0.5f + (float)j) / (float)columns, (0.5f + (float)i) / (float)rows);
						for (int k = 0; k < 6; k++)
						{
							list.Add(array[k]);
							list2.Add(item);
						}
					}
				}
				int[] array2 = new int[num];
				for (int l = 0; l < num; l++)
				{
					array2[l] = l;
				}
				this.mesh = new Mesh
				{
					hideFlags = HideFlags.DontSave
				};
				this.mesh.SetVertices(list);
				this.mesh.SetUVs(0, list2);
				this.mesh.SetIndices(array2, MeshTopology.Lines, 0);
				this.mesh.UploadMeshData(true);
				this.columnCount = columns;
				this.rowCount = rows;
			}

			// Token: 0x06004040 RID: 16448 RVA: 0x001435E3 File Offset: 0x001419E3
			public void Release()
			{
				GraphicsUtils.Destroy(this.mesh);
				this.mesh = null;
			}
		}
	}
}
