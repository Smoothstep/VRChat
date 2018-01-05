using System;
using UnityEngine;

// Token: 0x020005B8 RID: 1464
[AddComponentMenu("NGUI/UI/Image Button")]
public class UIImageButton : MonoBehaviour
{
	// Token: 0x1700074F RID: 1871
	// (get) Token: 0x060030BC RID: 12476 RVA: 0x000EF168 File Offset: 0x000ED568
	// (set) Token: 0x060030BD RID: 12477 RVA: 0x000EF190 File Offset: 0x000ED590
	public bool isEnabled
	{
		get
		{
			Collider component = base.GetComponent<Collider>();
			return component && component.enabled;
		}
		set
		{
			Collider component = base.GetComponent<Collider>();
			if (!component)
			{
				return;
			}
			if (component.enabled != value)
			{
				component.enabled = value;
				this.UpdateImage();
			}
		}
	}

	// Token: 0x060030BE RID: 12478 RVA: 0x000EF1C9 File Offset: 0x000ED5C9
	private void OnEnable()
	{
		if (this.target == null)
		{
			this.target = base.GetComponentInChildren<UISprite>();
		}
		this.UpdateImage();
	}

	// Token: 0x060030BF RID: 12479 RVA: 0x000EF1F0 File Offset: 0x000ED5F0
	private void OnValidate()
	{
		if (this.target != null)
		{
			if (string.IsNullOrEmpty(this.normalSprite))
			{
				this.normalSprite = this.target.spriteName;
			}
			if (string.IsNullOrEmpty(this.hoverSprite))
			{
				this.hoverSprite = this.target.spriteName;
			}
			if (string.IsNullOrEmpty(this.pressedSprite))
			{
				this.pressedSprite = this.target.spriteName;
			}
			if (string.IsNullOrEmpty(this.disabledSprite))
			{
				this.disabledSprite = this.target.spriteName;
			}
		}
	}

	// Token: 0x060030C0 RID: 12480 RVA: 0x000EF294 File Offset: 0x000ED694
	private void UpdateImage()
	{
		if (this.target != null)
		{
			if (this.isEnabled)
			{
				this.SetSprite((!UICamera.IsHighlighted(base.gameObject)) ? this.normalSprite : this.hoverSprite);
			}
			else
			{
				this.SetSprite(this.disabledSprite);
			}
		}
	}

	// Token: 0x060030C1 RID: 12481 RVA: 0x000EF2F5 File Offset: 0x000ED6F5
	private void OnHover(bool isOver)
	{
		if (this.isEnabled && this.target != null)
		{
			this.SetSprite((!isOver) ? this.normalSprite : this.hoverSprite);
		}
	}

	// Token: 0x060030C2 RID: 12482 RVA: 0x000EF330 File Offset: 0x000ED730
	private void OnPress(bool pressed)
	{
		if (pressed)
		{
			this.SetSprite(this.pressedSprite);
		}
		else
		{
			this.UpdateImage();
		}
	}

	// Token: 0x060030C3 RID: 12483 RVA: 0x000EF350 File Offset: 0x000ED750
	private void SetSprite(string sprite)
	{
		if (this.target.atlas == null || this.target.atlas.GetSprite(sprite) == null)
		{
			return;
		}
		this.target.spriteName = sprite;
		if (this.pixelSnap)
		{
			this.target.MakePixelPerfect();
		}
	}

	// Token: 0x04001B3E RID: 6974
	public UISprite target;

	// Token: 0x04001B3F RID: 6975
	public string normalSprite;

	// Token: 0x04001B40 RID: 6976
	public string hoverSprite;

	// Token: 0x04001B41 RID: 6977
	public string pressedSprite;

	// Token: 0x04001B42 RID: 6978
	public string disabledSprite;

	// Token: 0x04001B43 RID: 6979
	public bool pixelSnap = true;
}
