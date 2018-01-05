using System;
using UnityEngine;

// Token: 0x02000656 RID: 1622
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Sprite")]
public class UISprite : UIBasicSprite
{
	// Token: 0x17000887 RID: 2183
	// (get) Token: 0x060036A8 RID: 13992 RVA: 0x0011745B File Offset: 0x0011585B
	public override Material material
	{
		get
		{
			return (!(this.mAtlas != null)) ? null : this.mAtlas.spriteMaterial;
		}
	}

	// Token: 0x17000888 RID: 2184
	// (get) Token: 0x060036A9 RID: 13993 RVA: 0x0011747F File Offset: 0x0011587F
	// (set) Token: 0x060036AA RID: 13994 RVA: 0x00117488 File Offset: 0x00115888
	public UIAtlas atlas
	{
		get
		{
			return this.mAtlas;
		}
		set
		{
			if (this.mAtlas != value)
			{
				base.RemoveFromPanel();
				this.mAtlas = value;
				this.mSpriteSet = false;
				this.mSprite = null;
				if (string.IsNullOrEmpty(this.mSpriteName) && this.mAtlas != null && this.mAtlas.spriteList.Count > 0)
				{
					this.SetAtlasSprite(this.mAtlas.spriteList[0]);
					this.mSpriteName = this.mSprite.name;
				}
				if (!string.IsNullOrEmpty(this.mSpriteName))
				{
					string spriteName = this.mSpriteName;
					this.mSpriteName = string.Empty;
					this.spriteName = spriteName;
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x17000889 RID: 2185
	// (get) Token: 0x060036AB RID: 13995 RVA: 0x0011754F File Offset: 0x0011594F
	// (set) Token: 0x060036AC RID: 13996 RVA: 0x00117558 File Offset: 0x00115958
	public string spriteName
	{
		get
		{
			return this.mSpriteName;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				if (string.IsNullOrEmpty(this.mSpriteName))
				{
					return;
				}
				this.mSpriteName = string.Empty;
				this.mSprite = null;
				this.mChanged = true;
				this.mSpriteSet = false;
			}
			else if (this.mSpriteName != value)
			{
				this.mSpriteName = value;
				this.mSprite = null;
				this.mChanged = true;
				this.mSpriteSet = false;
			}
		}
	}

	// Token: 0x1700088A RID: 2186
	// (get) Token: 0x060036AD RID: 13997 RVA: 0x001175D3 File Offset: 0x001159D3
	public bool isValid
	{
		get
		{
			return this.GetAtlasSprite() != null;
		}
	}

	// Token: 0x1700088B RID: 2187
	// (get) Token: 0x060036AE RID: 13998 RVA: 0x001175E1 File Offset: 0x001159E1
	// (set) Token: 0x060036AF RID: 13999 RVA: 0x001175EF File Offset: 0x001159EF
	[Obsolete("Use 'centerType' instead")]
	public bool fillCenter
	{
		get
		{
			return this.centerType != UIBasicSprite.AdvancedType.Invisible;
		}
		set
		{
			if (value != (this.centerType != UIBasicSprite.AdvancedType.Invisible))
			{
				this.centerType = ((!value) ? UIBasicSprite.AdvancedType.Invisible : UIBasicSprite.AdvancedType.Sliced);
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700088C RID: 2188
	// (get) Token: 0x060036B0 RID: 14000 RVA: 0x0011761C File Offset: 0x00115A1C
	public override Vector4 border
	{
		get
		{
			UISpriteData atlasSprite = this.GetAtlasSprite();
			if (atlasSprite == null)
			{
				return base.border;
			}
			return new Vector4((float)atlasSprite.borderLeft, (float)atlasSprite.borderBottom, (float)atlasSprite.borderRight, (float)atlasSprite.borderTop);
		}
	}

	// Token: 0x1700088D RID: 2189
	// (get) Token: 0x060036B1 RID: 14001 RVA: 0x0011765E File Offset: 0x00115A5E
	public override float pixelSize
	{
		get
		{
			return (!(this.mAtlas != null)) ? 1f : this.mAtlas.pixelSize;
		}
	}

	// Token: 0x1700088E RID: 2190
	// (get) Token: 0x060036B2 RID: 14002 RVA: 0x00117688 File Offset: 0x00115A88
	public override int minWidth
	{
		get
		{
			if (this.type == UIBasicSprite.Type.Sliced || this.type == UIBasicSprite.Type.Advanced)
			{
				Vector4 vector = this.border * this.pixelSize;
				int num = Mathf.RoundToInt(vector.x + vector.z);
				UISpriteData atlasSprite = this.GetAtlasSprite();
				if (atlasSprite != null)
				{
					num += atlasSprite.paddingLeft + atlasSprite.paddingRight;
				}
				return Mathf.Max(base.minWidth, ((num & 1) != 1) ? num : (num + 1));
			}
			return base.minWidth;
		}
	}

	// Token: 0x1700088F RID: 2191
	// (get) Token: 0x060036B3 RID: 14003 RVA: 0x00117718 File Offset: 0x00115B18
	public override int minHeight
	{
		get
		{
			if (this.type == UIBasicSprite.Type.Sliced || this.type == UIBasicSprite.Type.Advanced)
			{
				Vector4 vector = this.border * this.pixelSize;
				int num = Mathf.RoundToInt(vector.y + vector.w);
				UISpriteData atlasSprite = this.GetAtlasSprite();
				if (atlasSprite != null)
				{
					num += atlasSprite.paddingTop + atlasSprite.paddingBottom;
				}
				return Mathf.Max(base.minHeight, ((num & 1) != 1) ? num : (num + 1));
			}
			return base.minHeight;
		}
	}

	// Token: 0x17000890 RID: 2192
	// (get) Token: 0x060036B4 RID: 14004 RVA: 0x001177A8 File Offset: 0x00115BA8
	public override Vector4 drawingDimensions
	{
		get
		{
			Vector2 pivotOffset = base.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			if (this.GetAtlasSprite() != null && this.mType != UIBasicSprite.Type.Tiled)
			{
				int paddingLeft = this.mSprite.paddingLeft;
				int paddingBottom = this.mSprite.paddingBottom;
				int num5 = this.mSprite.paddingRight;
				int num6 = this.mSprite.paddingTop;
				int num7 = this.mSprite.width + paddingLeft + num5;
				int num8 = this.mSprite.height + paddingBottom + num6;
				float num9 = 1f;
				float num10 = 1f;
				if (num7 > 0 && num8 > 0 && (this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled))
				{
					if ((num7 & 1) != 0)
					{
						num5++;
					}
					if ((num8 & 1) != 0)
					{
						num6++;
					}
					num9 = 1f / (float)num7 * (float)this.mWidth;
					num10 = 1f / (float)num8 * (float)this.mHeight;
				}
				if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
				{
					num += (float)num5 * num9;
					num3 -= (float)paddingLeft * num9;
				}
				else
				{
					num += (float)paddingLeft * num9;
					num3 -= (float)num5 * num9;
				}
				if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
				{
					num2 += (float)num6 * num10;
					num4 -= (float)paddingBottom * num10;
				}
				else
				{
					num2 += (float)paddingBottom * num10;
					num4 -= (float)num6 * num10;
				}
			}
			Vector4 vector = (!(this.mAtlas != null)) ? Vector4.zero : (this.border * this.pixelSize);
			float num11 = vector.x + vector.z;
			float num12 = vector.y + vector.w;
			float x = Mathf.Lerp(num, num3 - num11, this.mDrawRegion.x);
			float y = Mathf.Lerp(num2, num4 - num12, this.mDrawRegion.y);
			float z = Mathf.Lerp(num + num11, num3, this.mDrawRegion.z);
			float w = Mathf.Lerp(num2 + num12, num4, this.mDrawRegion.w);
			return new Vector4(x, y, z, w);
		}
	}

	// Token: 0x17000891 RID: 2193
	// (get) Token: 0x060036B5 RID: 14005 RVA: 0x00117A1E File Offset: 0x00115E1E
	public override bool premultipliedAlpha
	{
		get
		{
			return this.mAtlas != null && this.mAtlas.premultipliedAlpha;
		}
	}

	// Token: 0x060036B6 RID: 14006 RVA: 0x00117A40 File Offset: 0x00115E40
	public UISpriteData GetAtlasSprite()
	{
		if (!this.mSpriteSet)
		{
			this.mSprite = null;
		}
		if (this.mSprite == null && this.mAtlas != null)
		{
			if (!string.IsNullOrEmpty(this.mSpriteName))
			{
				UISpriteData sprite = this.mAtlas.GetSprite(this.mSpriteName);
				if (sprite == null)
				{
					return null;
				}
				this.SetAtlasSprite(sprite);
			}
			if (this.mSprite == null && this.mAtlas.spriteList.Count > 0)
			{
				UISpriteData uispriteData = this.mAtlas.spriteList[0];
				if (uispriteData == null)
				{
					return null;
				}
				this.SetAtlasSprite(uispriteData);
				if (this.mSprite == null)
				{
					Debug.LogError(this.mAtlas.name + " seems to have a null sprite!");
					return null;
				}
				this.mSpriteName = this.mSprite.name;
			}
		}
		return this.mSprite;
	}

	// Token: 0x060036B7 RID: 14007 RVA: 0x00117B2C File Offset: 0x00115F2C
	protected void SetAtlasSprite(UISpriteData sp)
	{
		this.mChanged = true;
		this.mSpriteSet = true;
		if (sp != null)
		{
			this.mSprite = sp;
			this.mSpriteName = this.mSprite.name;
		}
		else
		{
			this.mSpriteName = ((this.mSprite == null) ? string.Empty : this.mSprite.name);
			this.mSprite = sp;
		}
	}

	// Token: 0x060036B8 RID: 14008 RVA: 0x00117B98 File Offset: 0x00115F98
	public override void MakePixelPerfect()
	{
		if (!this.isValid)
		{
			return;
		}
		base.MakePixelPerfect();
		if (this.mType == UIBasicSprite.Type.Tiled)
		{
			return;
		}
		UISpriteData atlasSprite = this.GetAtlasSprite();
		if (atlasSprite == null)
		{
			return;
		}
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		if ((this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled || !atlasSprite.hasBorder) && mainTexture != null)
		{
			int num = Mathf.RoundToInt(this.pixelSize * (float)(atlasSprite.width + atlasSprite.paddingLeft + atlasSprite.paddingRight));
			int num2 = Mathf.RoundToInt(this.pixelSize * (float)(atlasSprite.height + atlasSprite.paddingTop + atlasSprite.paddingBottom));
			if ((num & 1) == 1)
			{
				num++;
			}
			if ((num2 & 1) == 1)
			{
				num2++;
			}
			base.width = num;
			base.height = num2;
		}
	}

	// Token: 0x060036B9 RID: 14009 RVA: 0x00117C80 File Offset: 0x00116080
	protected override void OnInit()
	{
		if (!this.mFillCenter)
		{
			this.mFillCenter = true;
			this.centerType = UIBasicSprite.AdvancedType.Invisible;
		}
		base.OnInit();
	}

	// Token: 0x060036BA RID: 14010 RVA: 0x00117CA1 File Offset: 0x001160A1
	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.mChanged || !this.mSpriteSet)
		{
			this.mSpriteSet = true;
			this.mSprite = null;
			this.mChanged = true;
		}
	}

	// Token: 0x060036BB RID: 14011 RVA: 0x00117CD4 File Offset: 0x001160D4
	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		if (this.mSprite == null)
		{
			this.mSprite = this.atlas.GetSprite(this.spriteName);
		}
		if (this.mSprite == null)
		{
			return;
		}
		Rect rect = new Rect((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
		Rect rect2 = new Rect((float)(this.mSprite.x + this.mSprite.borderLeft), (float)(this.mSprite.y + this.mSprite.borderTop), (float)(this.mSprite.width - this.mSprite.borderLeft - this.mSprite.borderRight), (float)(this.mSprite.height - this.mSprite.borderBottom - this.mSprite.borderTop));
		rect = NGUIMath.ConvertToTexCoords(rect, mainTexture.width, mainTexture.height);
		rect2 = NGUIMath.ConvertToTexCoords(rect2, mainTexture.width, mainTexture.height);
		int size = verts.size;
		base.Fill(verts, uvs, cols, rect, rect2);
		if (this.onPostFill != null)
		{
			this.onPostFill(this, size, verts, uvs, cols);
		}
	}

	// Token: 0x04001F97 RID: 8087
	[HideInInspector]
	[SerializeField]
	private UIAtlas mAtlas;

	// Token: 0x04001F98 RID: 8088
	[HideInInspector]
	[SerializeField]
	private string mSpriteName;

	// Token: 0x04001F99 RID: 8089
	[HideInInspector]
	[SerializeField]
	private bool mFillCenter = true;

	// Token: 0x04001F9A RID: 8090
	[NonSerialized]
	protected UISpriteData mSprite;

	// Token: 0x04001F9B RID: 8091
	[NonSerialized]
	private bool mSpriteSet;
}
