using System;
using UnityEngine;

// Token: 0x020005E5 RID: 1509
[Serializable]
public class BMSymbol
{
	// Token: 0x17000780 RID: 1920
	// (get) Token: 0x060031E3 RID: 12771 RVA: 0x000F731F File Offset: 0x000F571F
	public int length
	{
		get
		{
			if (this.mLength == 0)
			{
				this.mLength = this.sequence.Length;
			}
			return this.mLength;
		}
	}

	// Token: 0x17000781 RID: 1921
	// (get) Token: 0x060031E4 RID: 12772 RVA: 0x000F7343 File Offset: 0x000F5743
	public int offsetX
	{
		get
		{
			return this.mOffsetX;
		}
	}

	// Token: 0x17000782 RID: 1922
	// (get) Token: 0x060031E5 RID: 12773 RVA: 0x000F734B File Offset: 0x000F574B
	public int offsetY
	{
		get
		{
			return this.mOffsetY;
		}
	}

	// Token: 0x17000783 RID: 1923
	// (get) Token: 0x060031E6 RID: 12774 RVA: 0x000F7353 File Offset: 0x000F5753
	public int width
	{
		get
		{
			return this.mWidth;
		}
	}

	// Token: 0x17000784 RID: 1924
	// (get) Token: 0x060031E7 RID: 12775 RVA: 0x000F735B File Offset: 0x000F575B
	public int height
	{
		get
		{
			return this.mHeight;
		}
	}

	// Token: 0x17000785 RID: 1925
	// (get) Token: 0x060031E8 RID: 12776 RVA: 0x000F7363 File Offset: 0x000F5763
	public int advance
	{
		get
		{
			return this.mAdvance;
		}
	}

	// Token: 0x17000786 RID: 1926
	// (get) Token: 0x060031E9 RID: 12777 RVA: 0x000F736B File Offset: 0x000F576B
	public Rect uvRect
	{
		get
		{
			return this.mUV;
		}
	}

	// Token: 0x060031EA RID: 12778 RVA: 0x000F7373 File Offset: 0x000F5773
	public void MarkAsChanged()
	{
		this.mIsValid = false;
	}

	// Token: 0x060031EB RID: 12779 RVA: 0x000F737C File Offset: 0x000F577C
	public bool Validate(UIAtlas atlas)
	{
		if (atlas == null)
		{
			return false;
		}
		if (!this.mIsValid)
		{
			if (string.IsNullOrEmpty(this.spriteName))
			{
				return false;
			}
			this.mSprite = ((!(atlas != null)) ? null : atlas.GetSprite(this.spriteName));
			if (this.mSprite != null)
			{
				Texture texture = atlas.texture;
				if (texture == null)
				{
					this.mSprite = null;
				}
				else
				{
					this.mUV = new Rect((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
					this.mUV = NGUIMath.ConvertToTexCoords(this.mUV, texture.width, texture.height);
					this.mOffsetX = this.mSprite.paddingLeft;
					this.mOffsetY = this.mSprite.paddingTop;
					this.mWidth = this.mSprite.width;
					this.mHeight = this.mSprite.height;
					this.mAdvance = this.mSprite.width + (this.mSprite.paddingLeft + this.mSprite.paddingRight);
					this.mIsValid = true;
				}
			}
		}
		return this.mSprite != null;
	}

	// Token: 0x04001C88 RID: 7304
	public string sequence;

	// Token: 0x04001C89 RID: 7305
	public string spriteName;

	// Token: 0x04001C8A RID: 7306
	private UISpriteData mSprite;

	// Token: 0x04001C8B RID: 7307
	private bool mIsValid;

	// Token: 0x04001C8C RID: 7308
	private int mLength;

	// Token: 0x04001C8D RID: 7309
	private int mOffsetX;

	// Token: 0x04001C8E RID: 7310
	private int mOffsetY;

	// Token: 0x04001C8F RID: 7311
	private int mWidth;

	// Token: 0x04001C90 RID: 7312
	private int mHeight;

	// Token: 0x04001C91 RID: 7313
	private int mAdvance;

	// Token: 0x04001C92 RID: 7314
	private Rect mUV;
}
