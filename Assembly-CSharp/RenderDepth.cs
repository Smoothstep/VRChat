using System;
using UnityEngine;

// Token: 0x02000471 RID: 1137
public class RenderDepth : MonoBehaviour
{
	// Token: 0x170005F5 RID: 1525
	// (get) Token: 0x06002772 RID: 10098 RVA: 0x000CC3B0 File Offset: 0x000CA7B0
	private Shader DepthShader
	{
		get
		{
			Shader result;
			if ((result = this.shader) == null)
			{
				result = (this.shader = Shader.Find("Render Depth"));
			}
			return result;
		}
	}

	// Token: 0x170005F6 RID: 1526
	// (get) Token: 0x06002773 RID: 10099 RVA: 0x000CC3DD File Offset: 0x000CA7DD
	private Material DepthMaterial
	{
		get
		{
			if (this.material == null)
			{
				this.material = new Material(this.DepthShader);
				this.material.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.material;
		}
	}

	// Token: 0x06002774 RID: 10100 RVA: 0x000CC414 File Offset: 0x000CA814
	private void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			MonoBehaviour.print("System doesn't support image effects");
			base.enabled = false;
			return;
		}
		if (this.DepthShader == null || !this.DepthShader.isSupported)
		{
			base.enabled = false;
			MonoBehaviour.print("Shader " + this.DepthShader.name + " is not supported");
			return;
		}
	}

	// Token: 0x06002775 RID: 10101 RVA: 0x000CC485 File Offset: 0x000CA885
	private void OnDisable()
	{
		if (this.material != null)
		{
			UnityEngine.Object.DestroyImmediate(this.material);
		}
	}

	// Token: 0x06002776 RID: 10102 RVA: 0x000CC4A4 File Offset: 0x000CA8A4
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if (this.shader != null)
		{
			this.DepthMaterial.SetFloat("_DepthLevel", this.depthLevel);
			Graphics.Blit(src, dest, this.DepthMaterial);
		}
		else
		{
			Graphics.Blit(src, dest);
		}
	}

	// Token: 0x0400155C RID: 5468
	[Range(0f, 3f)]
	public float depthLevel = 0.5f;

	// Token: 0x0400155D RID: 5469
	private Shader shader;

	// Token: 0x0400155E RID: 5470
	private Material material;
}
