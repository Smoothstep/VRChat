using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020007D7 RID: 2007
	public sealed class DitheringComponent : PostProcessingComponentRenderTexture<DitheringModel>
	{
		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06004067 RID: 16487 RVA: 0x00144CB2 File Offset: 0x001430B2
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x00144CD5 File Offset: 0x001430D5
		public override void OnDisable()
		{
			this.noiseTextures = null;
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x00144CE0 File Offset: 0x001430E0
		private void LoadNoiseTextures()
		{
			this.noiseTextures = new Texture2D[64];
			for (int i = 0; i < 64; i++)
			{
				this.noiseTextures[i] = Resources.Load<Texture2D>("Bluenoise64/LDR_LLL1_" + i);
			}
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x00144D2C File Offset: 0x0014312C
		public override void Prepare(Material uberMaterial)
		{
			if (++this.textureIndex >= 64)
			{
				this.textureIndex = 0;
			}
			float value = Random.value;
			float value2 = Random.value;
			if (this.noiseTextures == null)
			{
				this.LoadNoiseTextures();
			}
			Texture2D texture2D = this.noiseTextures[this.textureIndex];
			uberMaterial.EnableKeyword("DITHERING");
			uberMaterial.SetTexture(DitheringComponent.Uniforms._DitheringTex, texture2D);
			uberMaterial.SetVector(DitheringComponent.Uniforms._DitheringCoords, new Vector4((float)this.context.width / (float)texture2D.width, (float)this.context.height / (float)texture2D.height, value, value2));
		}

		// Token: 0x040028C7 RID: 10439
		private Texture2D[] noiseTextures;

		// Token: 0x040028C8 RID: 10440
		private int textureIndex;

		// Token: 0x040028C9 RID: 10441
		private const int k_TextureCount = 64;

		// Token: 0x020007D8 RID: 2008
		private static class Uniforms
		{
			// Token: 0x040028CA RID: 10442
			internal static readonly int _DitheringTex = Shader.PropertyToID("_DitheringTex");

			// Token: 0x040028CB RID: 10443
			internal static readonly int _DitheringCoords = Shader.PropertyToID("_DitheringCoords");
		}
	}
}
