using System;
using UnityEngine;

// Token: 0x02000421 RID: 1057
[RequireComponent(typeof(Camera))]
[AddComponentMenu("")]
public class CC_Base : MonoBehaviour
{
	// Token: 0x170005E8 RID: 1512
	// (get) Token: 0x0600266F RID: 9839 RVA: 0x000BD5B8 File Offset: 0x000BB9B8
	protected Material material
	{
		get
		{
			if (this._material == null)
			{
				this._material = new Material(this.shader);
				this._material.hideFlags = HideFlags.HideAndDontSave;
			}
			return this._material;
		}
	}

	// Token: 0x06002670 RID: 9840 RVA: 0x000BD5EF File Offset: 0x000BB9EF
	public static bool IsLinear()
	{
		return QualitySettings.activeColorSpace == ColorSpace.Linear;
	}

	// Token: 0x06002671 RID: 9841 RVA: 0x000BD5F9 File Offset: 0x000BB9F9
	protected virtual void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
		if (!this.shader || !this.shader.isSupported)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06002672 RID: 9842 RVA: 0x000BD634 File Offset: 0x000BBA34
	protected virtual void OnDisable()
	{
		if (this._material)
		{
			UnityEngine.Object.DestroyImmediate(this._material);
		}
	}

	// Token: 0x0400134D RID: 4941
	public Shader shader;

	// Token: 0x0400134E RID: 4942
	protected Material _material;
}
