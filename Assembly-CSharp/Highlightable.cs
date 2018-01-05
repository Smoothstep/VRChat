using System;
using UnityEngine;

// Token: 0x02000A97 RID: 2711
public class Highlightable : MonoBehaviour
{
	// Token: 0x17000C01 RID: 3073
	// (get) Token: 0x0600518D RID: 20877 RVA: 0x001BEDD2 File Offset: 0x001BD1D2
	public Material defaultMaterial
	{
		get
		{
			return this.mDefaultMaterial;
		}
	}

	// Token: 0x17000C02 RID: 3074
	// (get) Token: 0x0600518E RID: 20878 RVA: 0x001BEDDA File Offset: 0x001BD1DA
	public bool isHighlighted
	{
		get
		{
			return this.mIsHighlighted;
		}
	}

	// Token: 0x0600518F RID: 20879 RVA: 0x001BEDE2 File Offset: 0x001BD1E2
	private void Awake()
	{
		this.SetupRenderer();
	}

	// Token: 0x06005190 RID: 20880 RVA: 0x001BEDEC File Offset: 0x001BD1EC
	protected virtual void Start()
	{
		if (this.renderer == null)
		{
			this.SetupRenderer();
		}
		this.mDefaultMaterial = this.renderer.material;
		this.highlightMaterial = (Resources.Load("Avatar_Outline", typeof(Material)) as Material);
	}

	// Token: 0x06005191 RID: 20881 RVA: 0x001BEE40 File Offset: 0x001BD240
	public void SetupRenderer()
	{
		this.renderer = base.transform.GetComponent<Renderer>();
		if (this.renderer == null)
		{
			this.renderer = base.GetComponentInChildren<Renderer>();
		}
		if (this.renderer == null && base.transform.parent != null)
		{
			this.renderer = base.transform.parent.GetComponentInChildren<Renderer>();
		}
		if (this.renderer == null)
		{
			this.renderer = base.transform.root.GetComponentInChildren<Renderer>();
		}
	}

	// Token: 0x06005192 RID: 20882 RVA: 0x001BEEE0 File Offset: 0x001BD2E0
	public virtual void Highlight()
	{
		Texture mainTexture = this.renderer.material.mainTexture;
		this.renderer.material = this.highlightMaterial;
		this.renderer.material.SetTexture("_Diffuse", mainTexture);
		this.mIsHighlighted = true;
	}

	// Token: 0x06005193 RID: 20883 RVA: 0x001BEF2C File Offset: 0x001BD32C
	public virtual void UnHighlight()
	{
		Texture texture = this.renderer.material.GetTexture("_Diffuse");
		this.renderer.material = this.mDefaultMaterial;
		this.renderer.material.mainTexture = texture;
		this.mIsHighlighted = false;
	}

	// Token: 0x040039E3 RID: 14819
	private Material mDefaultMaterial;

	// Token: 0x040039E4 RID: 14820
	private Material highlightMaterial;

	// Token: 0x040039E5 RID: 14821
	public Renderer renderer;

	// Token: 0x040039E6 RID: 14822
	private bool mIsHighlighted;
}
