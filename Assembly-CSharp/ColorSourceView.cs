using System;
using UnityEngine;

// Token: 0x020004AB RID: 1195
public class ColorSourceView : MonoBehaviour
{
	// Token: 0x060029F7 RID: 10743 RVA: 0x000D5CBB File Offset: 0x000D40BB
	private void Start()
	{
		base.gameObject.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1f, 1f));
	}

	// Token: 0x060029F8 RID: 10744 RVA: 0x000D5CE8 File Offset: 0x000D40E8
	private void Update()
	{
		if (this.ColorSourceManager == null)
		{
			return;
		}
		this._ColorManager = this.ColorSourceManager.GetComponent<ColorSourceManager>();
		if (this._ColorManager == null)
		{
			return;
		}
		base.gameObject.GetComponent<Renderer>().material.mainTexture = this._ColorManager.GetColorTexture();
	}

	// Token: 0x040016DB RID: 5851
	public GameObject ColorSourceManager;

	// Token: 0x040016DC RID: 5852
	private ColorSourceManager _ColorManager;
}
