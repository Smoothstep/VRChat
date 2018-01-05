using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200059B RID: 1435
[AddComponentMenu("NGUI/Interaction/Button")]
public class UIButton : UIButtonColor
{
	// Token: 0x17000745 RID: 1861
	// (get) Token: 0x06002FFF RID: 12287 RVA: 0x000EB884 File Offset: 0x000E9C84
	// (set) Token: 0x06003000 RID: 12288 RVA: 0x000EB8D8 File Offset: 0x000E9CD8
	public override bool isEnabled
	{
		get
		{
			if (!base.enabled)
			{
				return false;
			}
			Collider component = base.GetComponent<Collider>();
			if (component && component.enabled)
			{
				return true;
			}
			Collider2D component2 = base.GetComponent<Collider2D>();
			return component2 && component2.enabled;
		}
		set
		{
			if (this.isEnabled != value)
			{
				Collider component = base.GetComponent<Collider>();
				if (component != null)
				{
					component.enabled = value;
					this.SetState((!value) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, false);
				}
				else
				{
					Collider2D component2 = base.GetComponent<Collider2D>();
					if (component2 != null)
					{
						component2.enabled = value;
						this.SetState((!value) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, false);
					}
					else
					{
						base.enabled = value;
					}
				}
			}
		}
	}

	// Token: 0x17000746 RID: 1862
	// (get) Token: 0x06003001 RID: 12289 RVA: 0x000EB95E File Offset: 0x000E9D5E
	// (set) Token: 0x06003002 RID: 12290 RVA: 0x000EB978 File Offset: 0x000E9D78
	public string normalSprite
	{
		get
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			return this.mNormalSprite;
		}
		set
		{
			if (this.mSprite != null && !string.IsNullOrEmpty(this.mNormalSprite) && this.mNormalSprite == this.mSprite.spriteName)
			{
				this.mNormalSprite = value;
				this.SetSprite(value);
				NGUITools.SetDirty(this.mSprite);
			}
			else
			{
				this.mNormalSprite = value;
				if (this.mState == UIButtonColor.State.Normal)
				{
					this.SetSprite(value);
				}
			}
		}
	}

	// Token: 0x17000747 RID: 1863
	// (get) Token: 0x06003003 RID: 12291 RVA: 0x000EB9F8 File Offset: 0x000E9DF8
	// (set) Token: 0x06003004 RID: 12292 RVA: 0x000EBA14 File Offset: 0x000E9E14
	public Sprite normalSprite2D
	{
		get
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			return this.mNormalSprite2D;
		}
		set
		{
			if (this.mSprite2D != null && this.mNormalSprite2D == this.mSprite2D.sprite2D)
			{
				this.mNormalSprite2D = value;
				this.SetSprite(value);
				NGUITools.SetDirty(this.mSprite);
			}
			else
			{
				this.mNormalSprite2D = value;
				if (this.mState == UIButtonColor.State.Normal)
				{
					this.SetSprite(value);
				}
			}
		}
	}

	// Token: 0x06003005 RID: 12293 RVA: 0x000EBA84 File Offset: 0x000E9E84
	protected override void OnInit()
	{
		base.OnInit();
		this.mSprite = (this.mWidget as UISprite);
		this.mSprite2D = (this.mWidget as UI2DSprite);
		if (this.mSprite != null)
		{
			this.mNormalSprite = this.mSprite.spriteName;
		}
		if (this.mSprite2D != null)
		{
			this.mNormalSprite2D = this.mSprite2D.sprite2D;
		}
	}

	// Token: 0x06003006 RID: 12294 RVA: 0x000EBB00 File Offset: 0x000E9F00
	protected override void OnEnable()
	{
		if (this.isEnabled)
		{
			if (this.mInitDone)
			{
				if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
				{
					this.OnHover(UICamera.selectedObject == base.gameObject);
				}
				else if (UICamera.currentScheme == UICamera.ControlScheme.Mouse)
				{
					this.OnHover(UICamera.hoveredObject == base.gameObject);
				}
				else
				{
					this.SetState(UIButtonColor.State.Normal, false);
				}
			}
		}
		else
		{
			this.SetState(UIButtonColor.State.Disabled, true);
		}
	}

	// Token: 0x06003007 RID: 12295 RVA: 0x000EBB83 File Offset: 0x000E9F83
	protected override void OnDragOver()
	{
		if (this.isEnabled && (this.dragHighlight || UICamera.currentTouch.pressed == base.gameObject))
		{
			base.OnDragOver();
		}
	}

	// Token: 0x06003008 RID: 12296 RVA: 0x000EBBBB File Offset: 0x000E9FBB
	protected override void OnDragOut()
	{
		if (this.isEnabled && (this.dragHighlight || UICamera.currentTouch.pressed == base.gameObject))
		{
			base.OnDragOut();
		}
	}

	// Token: 0x06003009 RID: 12297 RVA: 0x000EBBF3 File Offset: 0x000E9FF3
	protected virtual void OnClick()
	{
		if (UIButton.current == null && this.isEnabled)
		{
			UIButton.current = this;
			EventDelegate.Execute(this.onClick);
			UIButton.current = null;
		}
	}

	// Token: 0x0600300A RID: 12298 RVA: 0x000EBC28 File Offset: 0x000EA028
	public override void SetState(UIButtonColor.State state, bool immediate)
	{
		base.SetState(state, immediate);
		if (this.mSprite != null)
		{
			switch (state)
			{
			case UIButtonColor.State.Normal:
				this.SetSprite(this.mNormalSprite);
				break;
			case UIButtonColor.State.Hover:
				this.SetSprite((!string.IsNullOrEmpty(this.hoverSprite)) ? this.hoverSprite : this.mNormalSprite);
				break;
			case UIButtonColor.State.Pressed:
				this.SetSprite(this.pressedSprite);
				break;
			case UIButtonColor.State.Disabled:
				this.SetSprite(this.disabledSprite);
				break;
			}
		}
		else if (this.mSprite2D != null)
		{
			switch (state)
			{
			case UIButtonColor.State.Normal:
				this.SetSprite(this.mNormalSprite2D);
				break;
			case UIButtonColor.State.Hover:
				this.SetSprite((!(this.hoverSprite2D == null)) ? this.hoverSprite2D : this.mNormalSprite2D);
				break;
			case UIButtonColor.State.Pressed:
				this.SetSprite(this.pressedSprite2D);
				break;
			case UIButtonColor.State.Disabled:
				this.SetSprite(this.disabledSprite2D);
				break;
			}
		}
	}

	// Token: 0x0600300B RID: 12299 RVA: 0x000EBD5C File Offset: 0x000EA15C
	protected void SetSprite(string sp)
	{
		if (this.mSprite != null && !string.IsNullOrEmpty(sp) && this.mSprite.spriteName != sp)
		{
			this.mSprite.spriteName = sp;
			if (this.pixelSnap)
			{
				this.mSprite.MakePixelPerfect();
			}
		}
	}

	// Token: 0x0600300C RID: 12300 RVA: 0x000EBDC0 File Offset: 0x000EA1C0
	protected void SetSprite(Sprite sp)
	{
		if (sp != null && this.mSprite2D != null && this.mSprite2D.sprite2D != sp)
		{
			this.mSprite2D.sprite2D = sp;
			if (this.pixelSnap)
			{
				this.mSprite2D.MakePixelPerfect();
			}
		}
	}

	// Token: 0x04001A74 RID: 6772
	public static UIButton current;

	// Token: 0x04001A75 RID: 6773
	public bool dragHighlight;

	// Token: 0x04001A76 RID: 6774
	public string hoverSprite;

	// Token: 0x04001A77 RID: 6775
	public string pressedSprite;

	// Token: 0x04001A78 RID: 6776
	public string disabledSprite;

	// Token: 0x04001A79 RID: 6777
	public Sprite hoverSprite2D;

	// Token: 0x04001A7A RID: 6778
	public Sprite pressedSprite2D;

	// Token: 0x04001A7B RID: 6779
	public Sprite disabledSprite2D;

	// Token: 0x04001A7C RID: 6780
	public bool pixelSnap;

	// Token: 0x04001A7D RID: 6781
	public List<EventDelegate> onClick = new List<EventDelegate>();

	// Token: 0x04001A7E RID: 6782
	[NonSerialized]
	private UISprite mSprite;

	// Token: 0x04001A7F RID: 6783
	[NonSerialized]
	private UI2DSprite mSprite2D;

	// Token: 0x04001A80 RID: 6784
	[NonSerialized]
	private string mNormalSprite;

	// Token: 0x04001A81 RID: 6785
	[NonSerialized]
	private Sprite mNormalSprite2D;
}
