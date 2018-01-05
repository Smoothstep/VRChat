using System;
using UnityEngine;

// Token: 0x02000576 RID: 1398
[RequireComponent(typeof(UISprite))]
[AddComponentMenu("NGUI/Examples/UI Cursor")]
public class UICursor : MonoBehaviour
{
	// Token: 0x06002F7E RID: 12158 RVA: 0x000E7CF1 File Offset: 0x000E60F1
	private void Awake()
	{
		UICursor.instance = this;
	}

	// Token: 0x06002F7F RID: 12159 RVA: 0x000E7CF9 File Offset: 0x000E60F9
	private void OnDestroy()
	{
		UICursor.instance = null;
	}

	// Token: 0x06002F80 RID: 12160 RVA: 0x000E7D04 File Offset: 0x000E6104
	private void Start()
	{
		this.mTrans = base.transform;
		this.mSprite = base.GetComponentInChildren<UISprite>();
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		if (this.mSprite != null)
		{
			this.mAtlas = this.mSprite.atlas;
			this.mSpriteName = this.mSprite.spriteName;
			if (this.mSprite.depth < 100)
			{
				this.mSprite.depth = 100;
			}
		}
	}

	// Token: 0x06002F81 RID: 12161 RVA: 0x000E7DA4 File Offset: 0x000E61A4
	private void Update()
	{
		Vector3 mousePosition = Input.mousePosition;
		if (this.uiCamera != null)
		{
			mousePosition.x = Mathf.Clamp01(mousePosition.x / (float)Screen.width);
			mousePosition.y = Mathf.Clamp01(mousePosition.y / (float)Screen.height);
			this.mTrans.position = this.uiCamera.ViewportToWorldPoint(mousePosition);
			if (this.uiCamera.orthographic)
			{
				Vector3 localPosition = this.mTrans.localPosition;
				localPosition.x = Mathf.Round(localPosition.x);
				localPosition.y = Mathf.Round(localPosition.y);
				this.mTrans.localPosition = localPosition;
			}
		}
		else
		{
			mousePosition.x -= (float)Screen.width * 0.5f;
			mousePosition.y -= (float)Screen.height * 0.5f;
			mousePosition.x = Mathf.Round(mousePosition.x);
			mousePosition.y = Mathf.Round(mousePosition.y);
			this.mTrans.localPosition = mousePosition;
		}
	}

	// Token: 0x06002F82 RID: 12162 RVA: 0x000E7ECC File Offset: 0x000E62CC
	public static void Clear()
	{
		if (UICursor.instance != null && UICursor.instance.mSprite != null)
		{
			UICursor.Set(UICursor.instance.mAtlas, UICursor.instance.mSpriteName);
		}
	}

	// Token: 0x06002F83 RID: 12163 RVA: 0x000E7F0C File Offset: 0x000E630C
	public static void Set(UIAtlas atlas, string sprite)
	{
		if (UICursor.instance != null && UICursor.instance.mSprite)
		{
			UICursor.instance.mSprite.atlas = atlas;
			UICursor.instance.mSprite.spriteName = sprite;
			UICursor.instance.mSprite.MakePixelPerfect();
			UICursor.instance.Update();
		}
	}

	// Token: 0x040019C6 RID: 6598
	public static UICursor instance;

	// Token: 0x040019C7 RID: 6599
	public Camera uiCamera;

	// Token: 0x040019C8 RID: 6600
	private Transform mTrans;

	// Token: 0x040019C9 RID: 6601
	private UISprite mSprite;

	// Token: 0x040019CA RID: 6602
	private UIAtlas mAtlas;

	// Token: 0x040019CB RID: 6603
	private string mSpriteName;
}
