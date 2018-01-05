using System;
using UnityEngine;

namespace VRC.Core
{
	// Token: 0x02000A54 RID: 2644
	public static class StandardShaderHack
	{
		// Token: 0x0600500F RID: 20495 RVA: 0x001B5D18 File Offset: 0x001B4118
		private static StandardShaderHack.WorkflowMode DetermineWorkflow(Material mat)
		{
			if (mat.HasProperty("_SpecGlossMap") && mat.HasProperty("_SpecColor"))
			{
				return StandardShaderHack.WorkflowMode.Specular;
			}
			if (mat.HasProperty("_MetallicGlossMap") && mat.HasProperty("_Metallic"))
			{
				return StandardShaderHack.WorkflowMode.Metallic;
			}
			return StandardShaderHack.WorkflowMode.Dielectric;
		}

		// Token: 0x06005010 RID: 20496 RVA: 0x001B5D6C File Offset: 0x001B416C
		private static void SetupMaterialWithBlendMode(Material material, StandardShaderHack.BlendMode blendMode)
		{
			switch (blendMode)
			{
			case StandardShaderHack.BlendMode.Opaque:
				material.SetOverrideTag("RenderType", string.Empty);
				material.SetInt("_SrcBlend", 1);
				material.SetInt("_DstBlend", 0);
				material.SetInt("_ZWrite", 1);
				material.DisableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = -1;
				break;
			case StandardShaderHack.BlendMode.Cutout:
				material.SetOverrideTag("RenderType", "TransparentCutout");
				material.SetInt("_SrcBlend", 1);
				material.SetInt("_DstBlend", 0);
				material.SetInt("_ZWrite", 1);
				material.EnableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = 2450;
				break;
			case StandardShaderHack.BlendMode.Fade:
				material.SetOverrideTag("RenderType", "Transparent");
				material.SetInt("_SrcBlend", 5);
				material.SetInt("_DstBlend", 10);
				material.SetInt("_ZWrite", 0);
				material.DisableKeyword("_ALPHATEST_ON");
				material.EnableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = 3000;
				break;
			case StandardShaderHack.BlendMode.Transparent:
				material.SetOverrideTag("RenderType", "Transparent");
				material.SetInt("_SrcBlend", 1);
				material.SetInt("_DstBlend", 10);
				material.SetInt("_ZWrite", 0);
				material.DisableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = 3000;
				break;
			}
		}

		// Token: 0x06005011 RID: 20497 RVA: 0x001B5F28 File Offset: 0x001B4328
		private static StandardShaderHack.SmoothnessMapChannel GetSmoothnessMapChannel(Material material)
		{
			int num = (int)material.GetFloat("_SmoothnessTextureChannel");
			if (num == 1)
			{
				return StandardShaderHack.SmoothnessMapChannel.AlbedoAlpha;
			}
			return StandardShaderHack.SmoothnessMapChannel.SpecularMetallicAlpha;
		}

		// Token: 0x06005012 RID: 20498 RVA: 0x001B5F4C File Offset: 0x001B434C
		private static void SetMaterialKeywords(Material material, StandardShaderHack.WorkflowMode workflowMode)
		{
			Texture texture = null;
			if (material.HasProperty("_BumpMap"))
			{
				texture = material.GetTexture("_BumpMap");
			}
			else if (material.HasProperty("_DetailNormalMap"))
			{
				texture = material.GetTexture("_DetailNormalMap");
			}
			if (texture != null)
			{
				StandardShaderHack.SetKeyword(material, "_NORMALMAP", texture);
			}
			if (workflowMode == StandardShaderHack.WorkflowMode.Specular)
			{
				StandardShaderHack.SetKeyword(material, "_SPECGLOSSMAP", material.GetTexture("_SpecGlossMap"));
			}
			else if (workflowMode == StandardShaderHack.WorkflowMode.Metallic)
			{
				StandardShaderHack.SetKeyword(material, "_METALLICGLOSSMAP", material.GetTexture("_MetallicGlossMap"));
			}
			if (material.HasProperty("_ParallaxMap"))
			{
				StandardShaderHack.SetKeyword(material, "_PARALLAXMAP", material.GetTexture("_ParallaxMap"));
			}
			texture = null;
			if (material.HasProperty("_DetailAlbedoMap"))
			{
				texture = material.GetTexture("_DetailAlbedoMap");
			}
			else if (material.HasProperty("_DetailNormalMap"))
			{
				texture = material.GetTexture("_DetailNormalMap");
			}
			if (texture != null)
			{
				StandardShaderHack.SetKeyword(material, "_DETAIL_MULX2", texture);
			}
			bool state = (material.globalIlluminationFlags & MaterialGlobalIlluminationFlags.EmissiveIsBlack) == MaterialGlobalIlluminationFlags.None;
			StandardShaderHack.SetKeyword(material, "_EMISSION", state);
			if (material.HasProperty("_SmoothnessTextureChannel"))
			{
				StandardShaderHack.SetKeyword(material, "_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A", StandardShaderHack.GetSmoothnessMapChannel(material) == StandardShaderHack.SmoothnessMapChannel.AlbedoAlpha);
			}
		}

		// Token: 0x06005013 RID: 20499 RVA: 0x001B60C0 File Offset: 0x001B44C0
		public static void MaterialChanged(Material material)
		{
			if (material.HasProperty("_Mode"))
			{
				StandardShaderHack.SetupMaterialWithBlendMode(material, (StandardShaderHack.BlendMode)material.GetInt("_Mode"));
			}
			StandardShaderHack.SetMaterialKeywords(material, StandardShaderHack.DetermineWorkflow(material));
		}

		// Token: 0x06005014 RID: 20500 RVA: 0x001B60EF File Offset: 0x001B44EF
		private static void SetKeyword(Material m, string keyword, bool state)
		{
			if (state)
			{
				m.EnableKeyword(keyword);
			}
			else
			{
				m.DisableKeyword(keyword);
			}
		}

		// Token: 0x02000A55 RID: 2645
		public enum WorkflowMode
		{
			// Token: 0x040038E7 RID: 14567
			Specular,
			// Token: 0x040038E8 RID: 14568
			Metallic,
			// Token: 0x040038E9 RID: 14569
			Dielectric
		}

		// Token: 0x02000A56 RID: 2646
		public enum BlendMode
		{
			// Token: 0x040038EB RID: 14571
			Opaque,
			// Token: 0x040038EC RID: 14572
			Cutout,
			// Token: 0x040038ED RID: 14573
			Fade,
			// Token: 0x040038EE RID: 14574
			Transparent
		}

		// Token: 0x02000A57 RID: 2647
		public enum SmoothnessMapChannel
		{
			// Token: 0x040038F0 RID: 14576
			SpecularMetallicAlpha,
			// Token: 0x040038F1 RID: 14577
			AlbedoAlpha
		}
	}
}
