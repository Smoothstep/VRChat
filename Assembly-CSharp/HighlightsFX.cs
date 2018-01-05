using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000AE0 RID: 2784
[RequireComponent(typeof(Camera))]
public class HighlightsFX : MonoBehaviour
{
	// Token: 0x17000C3F RID: 3135
	// (get) Token: 0x0600546E RID: 21614 RVA: 0x001D2923 File Offset: 0x001D0D23
	// (set) Token: 0x0600546F RID: 21615 RVA: 0x001D292A File Offset: 0x001D0D2A
	public static HighlightsFX Instance { get; private set; }

	// Token: 0x06005470 RID: 21616 RVA: 0x001D2932 File Offset: 0x001D0D32
	public static void EnableObjectHighlight(Renderer r, bool enable)
	{
		if (HighlightsFX.Instance != null)
		{
			HighlightsFX.Instance.EnableOutline(r, enable);
		}
	}

	// Token: 0x06005471 RID: 21617 RVA: 0x001D2950 File Offset: 0x001D0D50
	public void EnableOutline(Renderer renderer, bool enable)
	{
		if (renderer == null)
		{
			return;
		}
		if (enable)
		{
			this.objectsToRender.Add(renderer);
		}
		else
		{
			this.objectsToRender.Remove(renderer);
		}
	}

	// Token: 0x06005472 RID: 21618 RVA: 0x001D2984 File Offset: 0x001D0D84
	private void Awake()
	{
		if (HighlightsFX.Instance != null)
		{
			Debug.LogError("HighlightFX - more than one instance detected");
			return;
		}
		HighlightsFX.Instance = this;
		this.CreateBuffers();
		this.CreateMaterials();
		this.m_blur = base.gameObject.AddComponent<BlurOptimized>();
		this.m_blur.blurShader = this.BlurShader;
		this.m_blur.enabled = false;
		this.m_blur.blurSize = 2f;
		this.m_RTWidth = (int)((float)Screen.width / (float)this.m_resolution);
		this.m_RTHeight = (int)((float)Screen.height / (float)this.m_resolution);
	}

	// Token: 0x06005473 RID: 21619 RVA: 0x001D2A26 File Offset: 0x001D0E26
	private void OnDestroy()
	{
		HighlightsFX.Instance = null;
	}

	// Token: 0x06005474 RID: 21620 RVA: 0x001D2A2E File Offset: 0x001D0E2E
	private void CreateBuffers()
	{
		this.m_renderBuffer = new CommandBuffer();
	}

	// Token: 0x06005475 RID: 21621 RVA: 0x001D2A3B File Offset: 0x001D0E3B
	private void ClearCommandBuffers()
	{
		this.m_renderBuffer.Clear();
	}

	// Token: 0x06005476 RID: 21622 RVA: 0x001D2A48 File Offset: 0x001D0E48
	private void CreateMaterials()
	{
		this.m_highlightMaterial = new Material(this.HighlightShader);
	}

	// Token: 0x06005477 RID: 21623 RVA: 0x001D2A5C File Offset: 0x001D0E5C
	private void SetOccluderObjects()
	{
		if (string.IsNullOrEmpty(this.m_occludersTag))
		{
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag(this.m_occludersTag);
		List<Renderer> list = new List<Renderer>();
		foreach (GameObject gameObject in array)
		{
			Renderer component = gameObject.GetComponent<Renderer>();
			if (component != null)
			{
				list.Add(component);
			}
		}
		this.m_occluders = list.ToArray();
	}

	// Token: 0x06005478 RID: 21624 RVA: 0x001D2AD4 File Offset: 0x001D0ED4
	private void RenderHighlights(RenderTexture rt)
	{
		RenderTargetIdentifier renderTarget = new RenderTargetIdentifier(rt);
		this.m_renderBuffer.SetRenderTarget(renderTarget);
		this.objectsToRender.RemoveWhere((Renderer o) => o == null);
		foreach (Renderer renderer in this.objectsToRender)
		{
			if (!(renderer == null))
			{
				int num = 1;
				MeshFilter component = renderer.GetComponent<MeshFilter>();
				if (component != null && component.mesh != null)
				{
					num = component.mesh.subMeshCount;
				}
				for (int i = 0; i < num; i++)
				{
					this.m_renderBuffer.DrawRenderer(renderer, this.m_highlightMaterial, i, (int)this.m_sortingType);
				}
			}
		}
		RenderTexture.active = rt;
		Graphics.ExecuteCommandBuffer(this.m_renderBuffer);
		RenderTexture.active = null;
	}

	// Token: 0x06005479 RID: 21625 RVA: 0x001D2BF4 File Offset: 0x001D0FF4
	private void RenderOccluders(RenderTexture rt)
	{
		if (this.m_occluders == null)
		{
			return;
		}
		RenderTargetIdentifier renderTarget = new RenderTargetIdentifier(rt);
		this.m_renderBuffer.SetRenderTarget(renderTarget);
		this.m_renderBuffer.Clear();
		foreach (Renderer renderer in this.m_occluders)
		{
			int num = 1;
			MeshFilter component = renderer.GetComponent<MeshFilter>();
			if (component != null && component.mesh != null)
			{
				num = component.mesh.subMeshCount;
			}
			for (int j = 0; j < num; j++)
			{
				this.m_renderBuffer.DrawRenderer(renderer, this.m_highlightMaterial, j, (int)this.m_sortingType);
			}
		}
		RenderTexture.active = rt;
		Graphics.ExecuteCommandBuffer(this.m_renderBuffer);
		RenderTexture.active = null;
	}

	// Token: 0x0600547A RID: 21626 RVA: 0x001D2CCC File Offset: 0x001D10CC
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		RenderTexture renderTexture = RenderTexture.active = RenderTexture.GetTemporary(this.m_RTWidth, this.m_RTHeight, 0, RenderTextureFormat.R8);
		GL.Clear(true, true, Color.clear);
		RenderTexture.active = null;
		this.ClearCommandBuffers();
		this.RenderHighlights(renderTexture);
		RenderTexture temporary = RenderTexture.GetTemporary(this.m_RTWidth, this.m_RTHeight, 0, RenderTextureFormat.R8);
		this.m_blur.OnRenderImage(renderTexture, temporary);
		this.RenderOccluders(renderTexture);
		if (this.m_fillType == HighlightsFX.FillType.Outline)
		{
			RenderTexture temporary2 = RenderTexture.GetTemporary(this.m_RTWidth, this.m_RTHeight, 0, RenderTextureFormat.R8);
			this.m_highlightMaterial.SetTexture("_OccludeMap", renderTexture);
			Graphics.Blit(temporary, temporary2, this.m_highlightMaterial, 2);
			this.m_highlightMaterial.SetTexture("_OccludeMap", temporary2);
			RenderTexture.ReleaseTemporary(temporary2);
		}
		else
		{
			this.m_highlightMaterial.SetTexture("_OccludeMap", temporary);
		}
		this.m_highlightMaterial.SetColor("_Color", this.m_highlightColor);
		Graphics.Blit(source, destination, this.m_highlightMaterial, (int)this.m_selectionType);
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(renderTexture);
	}

	// Token: 0x04003B86 RID: 15238
	private HashSet<Renderer> objectsToRender = new HashSet<Renderer>();

	// Token: 0x04003B87 RID: 15239
	public HighlightsFX.HighlightType m_selectionType;

	// Token: 0x04003B88 RID: 15240
	public HighlightsFX.SortingType m_sortingType = HighlightsFX.SortingType.DepthFilter;

	// Token: 0x04003B89 RID: 15241
	public HighlightsFX.FillType m_fillType = HighlightsFX.FillType.Outline;

	// Token: 0x04003B8A RID: 15242
	public HighlightsFX.RTResolution m_resolution = HighlightsFX.RTResolution.Full;

	// Token: 0x04003B8B RID: 15243
	public string m_occludersTag = "Occluder";

	// Token: 0x04003B8C RID: 15244
	public Color m_highlightColor = new Color(1f, 0f, 0f, 0.65f);

	// Token: 0x04003B8D RID: 15245
	public Shader HighlightShader;

	// Token: 0x04003B8E RID: 15246
	public Shader BlurShader;

	// Token: 0x04003B8F RID: 15247
	private BlurOptimized m_blur;

	// Token: 0x04003B90 RID: 15248
	private Renderer[] m_occluders;

	// Token: 0x04003B91 RID: 15249
	private Material m_highlightMaterial;

	// Token: 0x04003B92 RID: 15250
	private CommandBuffer m_renderBuffer;

	// Token: 0x04003B93 RID: 15251
	private int m_RTWidth = 512;

	// Token: 0x04003B94 RID: 15252
	private int m_RTHeight = 512;

	// Token: 0x02000AE1 RID: 2785
	public enum HighlightType
	{
		// Token: 0x04003B97 RID: 15255
		Glow,
		// Token: 0x04003B98 RID: 15256
		Solid
	}

	// Token: 0x02000AE2 RID: 2786
	public enum SortingType
	{
		// Token: 0x04003B9A RID: 15258
		Overlay = 3,
		// Token: 0x04003B9B RID: 15259
		DepthFilter
	}

	// Token: 0x02000AE3 RID: 2787
	public enum FillType
	{
		// Token: 0x04003B9D RID: 15261
		Fill,
		// Token: 0x04003B9E RID: 15262
		Outline
	}

	// Token: 0x02000AE4 RID: 2788
	public enum RTResolution
	{
		// Token: 0x04003BA0 RID: 15264
		Quarter = 4,
		// Token: 0x04003BA1 RID: 15265
		Half = 2,
		// Token: 0x04003BA2 RID: 15266
		Full = 1
	}
}
