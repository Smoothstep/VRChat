using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AE5 RID: 2789
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PostEffectsBase : MonoBehaviour
{
	// Token: 0x0600547D RID: 21629 RVA: 0x001D2080 File Offset: 0x001D0480
	protected Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
	{
		if (!s)
		{
			Debug.Log("Missing shader in " + this.ToString());
			base.enabled = false;
			return null;
		}
		if (s.isSupported && m2Create && m2Create.shader == s)
		{
			return m2Create;
		}
		if (!s.isSupported)
		{
			this.NotSupported();
			Debug.Log(string.Concat(new string[]
			{
				"The shader ",
				s.ToString(),
				" on effect ",
				this.ToString(),
				" is not supported on this platform!"
			}));
			return null;
		}
		m2Create = new Material(s);
		this.createdMaterials.Add(m2Create);
		m2Create.hideFlags = HideFlags.DontSave;
		return m2Create;
	}

	// Token: 0x0600547E RID: 21630 RVA: 0x001D214C File Offset: 0x001D054C
	protected Material CreateMaterial(Shader s, Material m2Create)
	{
		if (!s)
		{
			Debug.Log("Missing shader in " + this.ToString());
			return null;
		}
		if (m2Create && m2Create.shader == s && s.isSupported)
		{
			return m2Create;
		}
		if (!s.isSupported)
		{
			return null;
		}
		m2Create = new Material(s);
		this.createdMaterials.Add(m2Create);
		m2Create.hideFlags = HideFlags.DontSave;
		return m2Create;
	}

	// Token: 0x0600547F RID: 21631 RVA: 0x001D21CE File Offset: 0x001D05CE
	private void OnEnable()
	{
		this.isSupported = true;
	}

	// Token: 0x06005480 RID: 21632 RVA: 0x001D21D7 File Offset: 0x001D05D7
	private void OnDestroy()
	{
		this.RemoveCreatedMaterials();
	}

	// Token: 0x06005481 RID: 21633 RVA: 0x001D21E0 File Offset: 0x001D05E0
	private void RemoveCreatedMaterials()
	{
		while (this.createdMaterials.Count > 0)
		{
			Material obj = this.createdMaterials[0];
			this.createdMaterials.RemoveAt(0);
			UnityEngine.Object.Destroy(obj);
		}
	}

	// Token: 0x06005482 RID: 21634 RVA: 0x001D2222 File Offset: 0x001D0622
	protected bool CheckSupport()
	{
		return this.CheckSupport(false);
	}

	// Token: 0x06005483 RID: 21635 RVA: 0x001D222B File Offset: 0x001D062B
	public virtual bool CheckResources()
	{
		Debug.LogWarning("CheckResources () for " + this.ToString() + " should be overwritten.");
		return this.isSupported;
	}

	// Token: 0x06005484 RID: 21636 RVA: 0x001D224D File Offset: 0x001D064D
	protected void Start()
	{
		this.CheckResources();
	}

	// Token: 0x06005485 RID: 21637 RVA: 0x001D2258 File Offset: 0x001D0658
	protected bool CheckSupport(bool needDepth)
	{
		this.isSupported = true;
		this.supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
		this.supportDX11 = (SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders);
		if (!SystemInfo.supportsImageEffects)
		{
			this.NotSupported();
			return false;
		}
		if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
		{
			this.NotSupported();
			return false;
		}
		if (needDepth)
		{
			base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
		}
		return true;
	}

	// Token: 0x06005486 RID: 21638 RVA: 0x001D22D7 File Offset: 0x001D06D7
	protected bool CheckSupport(bool needDepth, bool needHdr)
	{
		if (!this.CheckSupport(needDepth))
		{
			return false;
		}
		if (needHdr && !this.supportHDRTextures)
		{
			this.NotSupported();
			return false;
		}
		return true;
	}

	// Token: 0x06005487 RID: 21639 RVA: 0x001D2301 File Offset: 0x001D0701
	public bool Dx11Support()
	{
		return this.supportDX11;
	}

	// Token: 0x06005488 RID: 21640 RVA: 0x001D2309 File Offset: 0x001D0709
	protected void ReportAutoDisable()
	{
		Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
	}

	// Token: 0x06005489 RID: 21641 RVA: 0x001D2328 File Offset: 0x001D0728
	private bool CheckShader(Shader s)
	{
		Debug.Log(string.Concat(new string[]
		{
			"The shader ",
			s.ToString(),
			" on effect ",
			this.ToString(),
			" is not part of the Unity 3.2+ effects suite anymore. For best performance and quality, please ensure you are using the latest Standard Assets Image Effects (Pro only) package."
		}));
		if (!s.isSupported)
		{
			this.NotSupported();
			return false;
		}
		return false;
	}

	// Token: 0x0600548A RID: 21642 RVA: 0x001D2383 File Offset: 0x001D0783
	protected void NotSupported()
	{
		base.enabled = false;
		this.isSupported = false;
	}

	// Token: 0x0600548B RID: 21643 RVA: 0x001D2394 File Offset: 0x001D0794
	protected void DrawBorder(RenderTexture dest, Material material)
	{
		RenderTexture.active = dest;
		bool flag = true;
		GL.PushMatrix();
		GL.LoadOrtho();
		for (int i = 0; i < material.passCount; i++)
		{
			material.SetPass(i);
			float y;
			float y2;
			if (flag)
			{
				y = 1f;
				y2 = 0f;
			}
			else
			{
				y = 0f;
				y2 = 1f;
			}
			float x = 0f;
			float x2 = 1f / ((float)dest.width * 1f);
			float y3 = 0f;
			float y4 = 1f;
			GL.Begin(7);
			GL.TexCoord2(0f, y);
			GL.Vertex3(x, y3, 0.1f);
			GL.TexCoord2(1f, y);
			GL.Vertex3(x2, y3, 0.1f);
			GL.TexCoord2(1f, y2);
			GL.Vertex3(x2, y4, 0.1f);
			GL.TexCoord2(0f, y2);
			GL.Vertex3(x, y4, 0.1f);
			x = 1f - 1f / ((float)dest.width * 1f);
			x2 = 1f;
			y3 = 0f;
			y4 = 1f;
			GL.TexCoord2(0f, y);
			GL.Vertex3(x, y3, 0.1f);
			GL.TexCoord2(1f, y);
			GL.Vertex3(x2, y3, 0.1f);
			GL.TexCoord2(1f, y2);
			GL.Vertex3(x2, y4, 0.1f);
			GL.TexCoord2(0f, y2);
			GL.Vertex3(x, y4, 0.1f);
			x = 0f;
			x2 = 1f;
			y3 = 0f;
			y4 = 1f / ((float)dest.height * 1f);
			GL.TexCoord2(0f, y);
			GL.Vertex3(x, y3, 0.1f);
			GL.TexCoord2(1f, y);
			GL.Vertex3(x2, y3, 0.1f);
			GL.TexCoord2(1f, y2);
			GL.Vertex3(x2, y4, 0.1f);
			GL.TexCoord2(0f, y2);
			GL.Vertex3(x, y4, 0.1f);
			x = 0f;
			x2 = 1f;
			y3 = 1f - 1f / ((float)dest.height * 1f);
			y4 = 1f;
			GL.TexCoord2(0f, y);
			GL.Vertex3(x, y3, 0.1f);
			GL.TexCoord2(1f, y);
			GL.Vertex3(x2, y3, 0.1f);
			GL.TexCoord2(1f, y2);
			GL.Vertex3(x2, y4, 0.1f);
			GL.TexCoord2(0f, y2);
			GL.Vertex3(x, y4, 0.1f);
			GL.End();
		}
		GL.PopMatrix();
	}

	// Token: 0x04003BA3 RID: 15267
	protected bool supportHDRTextures = true;

	// Token: 0x04003BA4 RID: 15268
	protected bool supportDX11;

	// Token: 0x04003BA5 RID: 15269
	protected bool isSupported = true;

	// Token: 0x04003BA6 RID: 15270
	private List<Material> createdMaterials = new List<Material>();
}
