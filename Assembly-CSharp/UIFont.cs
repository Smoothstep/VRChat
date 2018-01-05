using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000641 RID: 1601
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Font")]
public class UIFont : UnityEngine.Component
{
	// Token: 0x17000820 RID: 2080
	// (get) Token: 0x0600356A RID: 13674 RVA: 0x0010F22E File Offset: 0x0010D62E
	// (set) Token: 0x0600356B RID: 13675 RVA: 0x0010F257 File Offset: 0x0010D657
	public BMFont bmFont
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mFont : this.mReplacement.bmFont;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.bmFont = value;
			}
			else
			{
				this.mFont = value;
			}
		}
	}

	// Token: 0x17000821 RID: 2081
	// (get) Token: 0x0600356C RID: 13676 RVA: 0x0010F282 File Offset: 0x0010D682
	// (set) Token: 0x0600356D RID: 13677 RVA: 0x0010F2C1 File Offset: 0x0010D6C1
	public int texWidth
	{
		get
		{
			return (!(this.mReplacement != null)) ? ((this.mFont == null) ? 1 : this.mFont.texWidth) : this.mReplacement.texWidth;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.texWidth = value;
			}
			else if (this.mFont != null)
			{
				this.mFont.texWidth = value;
			}
		}
	}

	// Token: 0x17000822 RID: 2082
	// (get) Token: 0x0600356E RID: 13678 RVA: 0x0010F2FC File Offset: 0x0010D6FC
	// (set) Token: 0x0600356F RID: 13679 RVA: 0x0010F33B File Offset: 0x0010D73B
	public int texHeight
	{
		get
		{
			return (!(this.mReplacement != null)) ? ((this.mFont == null) ? 1 : this.mFont.texHeight) : this.mReplacement.texHeight;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.texHeight = value;
			}
			else if (this.mFont != null)
			{
				this.mFont.texHeight = value;
			}
		}
	}

	// Token: 0x17000823 RID: 2083
	// (get) Token: 0x06003570 RID: 13680 RVA: 0x0010F378 File Offset: 0x0010D778
	public bool hasSymbols
	{
		get
		{
			return (!(this.mReplacement != null)) ? (this.mSymbols != null && this.mSymbols.Count != 0) : this.mReplacement.hasSymbols;
		}
	}

	// Token: 0x17000824 RID: 2084
	// (get) Token: 0x06003571 RID: 13681 RVA: 0x0010F3C5 File Offset: 0x0010D7C5
	public List<BMSymbol> symbols
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mSymbols : this.mReplacement.symbols;
		}
	}

	// Token: 0x17000825 RID: 2085
	// (get) Token: 0x06003572 RID: 13682 RVA: 0x0010F3EE File Offset: 0x0010D7EE
	// (set) Token: 0x06003573 RID: 13683 RVA: 0x0010F418 File Offset: 0x0010D818
	public UIAtlas atlas
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mAtlas : this.mReplacement.atlas;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.atlas = value;
			}
			else if (this.mAtlas != value)
			{
				if (value == null)
				{
					if (this.mAtlas != null)
					{
						this.mMat = this.mAtlas.spriteMaterial;
					}
					if (this.sprite != null)
					{
						this.mUVRect = this.uvRect;
					}
				}
				this.mPMA = -1;
				this.mAtlas = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000826 RID: 2086
	// (get) Token: 0x06003574 RID: 13684 RVA: 0x0010F4B4 File Offset: 0x0010D8B4
	// (set) Token: 0x06003575 RID: 13685 RVA: 0x0010F578 File Offset: 0x0010D978
	public Material material
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.material;
			}
			if (this.mAtlas != null)
			{
				return this.mAtlas.spriteMaterial;
			}
			if (this.mMat != null)
			{
				if (this.mDynamicFont != null && this.mMat != this.mDynamicFont.material)
				{
					this.mMat.mainTexture = this.mDynamicFont.material.mainTexture;
				}
				return this.mMat;
			}
			if (this.mDynamicFont != null)
			{
				return this.mDynamicFont.material;
			}
			return null;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.material = value;
			}
			else if (this.mMat != value)
			{
				this.mPMA = -1;
				this.mMat = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000827 RID: 2087
	// (get) Token: 0x06003576 RID: 13686 RVA: 0x0010F5CC File Offset: 0x0010D9CC
	[Obsolete("Use UIFont.premultipliedAlphaShader instead")]
	public bool premultipliedAlpha
	{
		get
		{
			return this.premultipliedAlphaShader;
		}
	}

	// Token: 0x17000828 RID: 2088
	// (get) Token: 0x06003577 RID: 13687 RVA: 0x0010F5D4 File Offset: 0x0010D9D4
	public bool premultipliedAlphaShader
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.premultipliedAlphaShader;
			}
			if (this.mAtlas != null)
			{
				return this.mAtlas.premultipliedAlpha;
			}
			if (this.mPMA == -1)
			{
				Material material = this.material;
				this.mPMA = ((!(material != null) || !(material.shader != null) || !material.shader.name.Contains("Premultiplied")) ? 0 : 1);
			}
			return this.mPMA == 1;
		}
	}

	// Token: 0x17000829 RID: 2089
	// (get) Token: 0x06003578 RID: 13688 RVA: 0x0010F67C File Offset: 0x0010DA7C
	public bool packedFontShader
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.packedFontShader;
			}
			if (this.mAtlas != null)
			{
				return false;
			}
			if (this.mPacked == -1)
			{
				Material material = this.material;
				this.mPacked = ((!(material != null) || !(material.shader != null) || !material.shader.name.Contains("Packed")) ? 0 : 1);
			}
			return this.mPacked == 1;
		}
	}

	// Token: 0x1700082A RID: 2090
	// (get) Token: 0x06003579 RID: 13689 RVA: 0x0010F71C File Offset: 0x0010DB1C
	public Texture2D texture
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.texture;
			}
			Material material = this.material;
			return (!(material != null)) ? null : (material.mainTexture as Texture2D);
		}
	}

	// Token: 0x1700082B RID: 2091
	// (get) Token: 0x0600357A RID: 13690 RVA: 0x0010F76C File Offset: 0x0010DB6C
	// (set) Token: 0x0600357B RID: 13691 RVA: 0x0010F7D8 File Offset: 0x0010DBD8
	public Rect uvRect
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.uvRect;
			}
			return (!(this.mAtlas != null) || this.sprite == null) ? new Rect(0f, 0f, 1f, 1f) : this.mUVRect;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.uvRect = value;
			}
			else if (this.sprite == null && this.mUVRect != value)
			{
				this.mUVRect = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700082C RID: 2092
	// (get) Token: 0x0600357C RID: 13692 RVA: 0x0010F830 File Offset: 0x0010DC30
	// (set) Token: 0x0600357D RID: 13693 RVA: 0x0010F860 File Offset: 0x0010DC60
	public string spriteName
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mFont.spriteName : this.mReplacement.spriteName;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteName = value;
			}
			else if (this.mFont.spriteName != value)
			{
				this.mFont.spriteName = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700082D RID: 2093
	// (get) Token: 0x0600357E RID: 13694 RVA: 0x0010F8B7 File Offset: 0x0010DCB7
	public bool isValid
	{
		get
		{
			return this.mDynamicFont != null || this.mFont.isValid;
		}
	}

	// Token: 0x1700082E RID: 2094
	// (get) Token: 0x0600357F RID: 13695 RVA: 0x0010F8D8 File Offset: 0x0010DCD8
	// (set) Token: 0x06003580 RID: 13696 RVA: 0x0010F8E0 File Offset: 0x0010DCE0
	[Obsolete("Use UIFont.defaultSize instead")]
	public int size
	{
		get
		{
			return this.defaultSize;
		}
		set
		{
			this.defaultSize = value;
		}
	}

	// Token: 0x1700082F RID: 2095
	// (get) Token: 0x06003581 RID: 13697 RVA: 0x0010F8EC File Offset: 0x0010DCEC
	// (set) Token: 0x06003582 RID: 13698 RVA: 0x0010F93E File Offset: 0x0010DD3E
	public int defaultSize
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.defaultSize;
			}
			if (this.isDynamic || this.mFont == null)
			{
				return this.mDynamicFontSize;
			}
			return this.mFont.charSize;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.defaultSize = value;
			}
			else
			{
				this.mDynamicFontSize = value;
			}
		}
	}

	// Token: 0x17000830 RID: 2096
	// (get) Token: 0x06003583 RID: 13699 RVA: 0x0010F96C File Offset: 0x0010DD6C
	public UISpriteData sprite
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.sprite;
			}
			if (this.mSprite == null && this.mAtlas != null && !string.IsNullOrEmpty(this.mFont.spriteName))
			{
				this.mSprite = this.mAtlas.GetSprite(this.mFont.spriteName);
				if (this.mSprite == null)
				{
					this.mSprite = this.mAtlas.GetSprite(base.name);
				}
				if (this.mSprite == null)
				{
					this.mFont.spriteName = null;
				}
				else
				{
					this.UpdateUVRect();
				}
				int i = 0;
				int count = this.mSymbols.Count;
				while (i < count)
				{
					this.symbols[i].MarkAsChanged();
					i++;
				}
			}
			return this.mSprite;
		}
	}

	// Token: 0x17000831 RID: 2097
	// (get) Token: 0x06003584 RID: 13700 RVA: 0x0010FA5C File Offset: 0x0010DE5C
	// (set) Token: 0x06003585 RID: 13701 RVA: 0x0010FA64 File Offset: 0x0010DE64
	public UIFont replacement
	{
		get
		{
			return this.mReplacement;
		}
		set
		{
			UIFont uifont = value;
			if (uifont == this)
			{
				uifont = null;
			}
			if (this.mReplacement != uifont)
			{
				if (uifont != null && uifont.replacement == this)
				{
					uifont.replacement = null;
				}
				if (this.mReplacement != null)
				{
					this.MarkAsChanged();
				}
				this.mReplacement = uifont;
				if (uifont != null)
				{
					this.mPMA = -1;
					this.mMat = null;
					this.mFont = null;
					this.mDynamicFont = null;
				}
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000832 RID: 2098
	// (get) Token: 0x06003586 RID: 13702 RVA: 0x0010FB02 File Offset: 0x0010DF02
	public bool isDynamic
	{
		get
		{
			return (!(this.mReplacement != null)) ? (this.mDynamicFont != null) : this.mReplacement.isDynamic;
		}
	}

	// Token: 0x17000833 RID: 2099
	// (get) Token: 0x06003587 RID: 13703 RVA: 0x0010FB31 File Offset: 0x0010DF31
	// (set) Token: 0x06003588 RID: 13704 RVA: 0x0010FB5C File Offset: 0x0010DF5C
	public Font dynamicFont
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mDynamicFont : this.mReplacement.dynamicFont;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.dynamicFont = value;
			}
			else if (this.mDynamicFont != value)
			{
				if (this.mDynamicFont != null)
				{
					this.material = null;
				}
				this.mDynamicFont = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000834 RID: 2100
	// (get) Token: 0x06003589 RID: 13705 RVA: 0x0010FBC1 File Offset: 0x0010DFC1
	// (set) Token: 0x0600358A RID: 13706 RVA: 0x0010FBEA File Offset: 0x0010DFEA
	public FontStyle dynamicFontStyle
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mDynamicFontStyle : this.mReplacement.dynamicFontStyle;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.dynamicFontStyle = value;
			}
			else if (this.mDynamicFontStyle != value)
			{
				this.mDynamicFontStyle = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x0600358B RID: 13707 RVA: 0x0010FC28 File Offset: 0x0010E028
	private void Trim()
	{
		Texture texture = this.mAtlas.texture;
		if (texture != null && this.mSprite != null)
		{
			Rect rect = NGUIMath.ConvertToPixels(this.mUVRect, this.texture.width, this.texture.height, true);
			Rect rect2 = new Rect((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
			int xMin = Mathf.RoundToInt(rect2.xMin - rect.xMin);
			int yMin = Mathf.RoundToInt(rect2.yMin - rect.yMin);
			int xMax = Mathf.RoundToInt(rect2.xMax - rect.xMin);
			int yMax = Mathf.RoundToInt(rect2.yMax - rect.yMin);
			this.mFont.Trim(xMin, yMin, xMax, yMax);
		}
	}

	// Token: 0x0600358C RID: 13708 RVA: 0x0010FD1C File Offset: 0x0010E11C
	private bool References(UIFont font)
	{
		return !(font == null) && (font == this || (this.mReplacement != null && this.mReplacement.References(font)));
	}

	// Token: 0x0600358D RID: 13709 RVA: 0x0010FD68 File Offset: 0x0010E168
	public static bool CheckIfRelated(UIFont a, UIFont b)
	{
		return !(a == null) && !(b == null) && ((a.isDynamic && b.isDynamic && a.dynamicFont.fontNames[0] == b.dynamicFont.fontNames[0]) || a == b || a.References(b) || b.References(a));
	}

	// Token: 0x17000835 RID: 2101
	// (get) Token: 0x0600358E RID: 13710 RVA: 0x0010FDED File Offset: 0x0010E1ED
	private Texture dynamicTexture
	{
		get
		{
			if (this.mReplacement)
			{
				return this.mReplacement.dynamicTexture;
			}
			if (this.isDynamic)
			{
				return this.mDynamicFont.material.mainTexture;
			}
			return null;
		}
	}

	// Token: 0x0600358F RID: 13711 RVA: 0x0010FE28 File Offset: 0x0010E228
	public void MarkAsChanged()
	{
		if (this.mReplacement != null)
		{
			this.mReplacement.MarkAsChanged();
		}
		this.mSprite = null;
		UILabel[] array = NGUITools.FindActive<UILabel>();
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			UILabel uilabel = array[i];
			if (uilabel.enabled && NGUITools.GetActive(uilabel.gameObject) && UIFont.CheckIfRelated(this, uilabel.bitmapFont))
			{
				UIFont bitmapFont = uilabel.bitmapFont;
				uilabel.bitmapFont = null;
				uilabel.bitmapFont = bitmapFont;
			}
			i++;
		}
		int j = 0;
		int count = this.symbols.Count;
		while (j < count)
		{
			this.symbols[j].MarkAsChanged();
			j++;
		}
	}

	// Token: 0x06003590 RID: 13712 RVA: 0x0010FEF4 File Offset: 0x0010E2F4
	public void UpdateUVRect()
	{
		if (this.mAtlas == null)
		{
			return;
		}
		Texture texture = this.mAtlas.texture;
		if (texture != null)
		{
			this.mUVRect = new Rect((float)(this.mSprite.x - this.mSprite.paddingLeft), (float)(this.mSprite.y - this.mSprite.paddingTop), (float)(this.mSprite.width + this.mSprite.paddingLeft + this.mSprite.paddingRight), (float)(this.mSprite.height + this.mSprite.paddingTop + this.mSprite.paddingBottom));
			this.mUVRect = NGUIMath.ConvertToTexCoords(this.mUVRect, texture.width, texture.height);
			if (this.mSprite.hasPadding)
			{
				this.Trim();
			}
		}
	}

	// Token: 0x06003591 RID: 13713 RVA: 0x0010FFE4 File Offset: 0x0010E3E4
	private BMSymbol GetSymbol(string sequence, bool createIfMissing)
	{
		int i = 0;
		int count = this.mSymbols.Count;
		while (i < count)
		{
			BMSymbol bmsymbol = this.mSymbols[i];
			if (bmsymbol.sequence == sequence)
			{
				return bmsymbol;
			}
			i++;
		}
		if (createIfMissing)
		{
			BMSymbol bmsymbol2 = new BMSymbol();
			bmsymbol2.sequence = sequence;
			this.mSymbols.Add(bmsymbol2);
			return bmsymbol2;
		}
		return null;
	}

	// Token: 0x06003592 RID: 13714 RVA: 0x00110054 File Offset: 0x0010E454
	public BMSymbol MatchSymbol(string text, int offset, int textLength)
	{
		int count = this.mSymbols.Count;
		if (count == 0)
		{
			return null;
		}
		textLength -= offset;
		for (int i = 0; i < count; i++)
		{
			BMSymbol bmsymbol = this.mSymbols[i];
			int length = bmsymbol.length;
			if (length != 0 && textLength >= length)
			{
				bool flag = true;
				for (int j = 0; j < length; j++)
				{
					if (text[offset + j] != bmsymbol.sequence[j])
					{
						flag = false;
						break;
					}
				}
				if (flag && bmsymbol.Validate(this.atlas))
				{
					return bmsymbol;
				}
			}
		}
		return null;
	}

	// Token: 0x06003593 RID: 13715 RVA: 0x0011010C File Offset: 0x0010E50C
	public void AddSymbol(string sequence, string spriteName)
	{
		BMSymbol symbol = this.GetSymbol(sequence, true);
		symbol.spriteName = spriteName;
		this.MarkAsChanged();
	}

	// Token: 0x06003594 RID: 13716 RVA: 0x00110130 File Offset: 0x0010E530
	public void RemoveSymbol(string sequence)
	{
		BMSymbol symbol = this.GetSymbol(sequence, false);
		if (symbol != null)
		{
			this.symbols.Remove(symbol);
		}
		this.MarkAsChanged();
	}

	// Token: 0x06003595 RID: 13717 RVA: 0x00110160 File Offset: 0x0010E560
	public void RenameSymbol(string before, string after)
	{
		BMSymbol symbol = this.GetSymbol(before, false);
		if (symbol != null)
		{
			symbol.sequence = after;
		}
		this.MarkAsChanged();
	}

	// Token: 0x06003596 RID: 13718 RVA: 0x0011018C File Offset: 0x0010E58C
	public bool UsesSprite(string s)
	{
		if (!string.IsNullOrEmpty(s))
		{
			if (s.Equals(this.spriteName))
			{
				return true;
			}
			int i = 0;
			int count = this.symbols.Count;
			while (i < count)
			{
				BMSymbol bmsymbol = this.symbols[i];
				if (s.Equals(bmsymbol.spriteName))
				{
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x04001ECC RID: 7884
	[HideInInspector]
	[SerializeField]
	private Material mMat;

	// Token: 0x04001ECD RID: 7885
	[HideInInspector]
	[SerializeField]
	private Rect mUVRect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x04001ECE RID: 7886
	[HideInInspector]
	[SerializeField]
	private BMFont mFont = new BMFont();

	// Token: 0x04001ECF RID: 7887
	[HideInInspector]
	[SerializeField]
	private UIAtlas mAtlas;

	// Token: 0x04001ED0 RID: 7888
	[HideInInspector]
	[SerializeField]
	private UIFont mReplacement;

	// Token: 0x04001ED1 RID: 7889
	[HideInInspector]
	[SerializeField]
	private List<BMSymbol> mSymbols = new List<BMSymbol>();

	// Token: 0x04001ED2 RID: 7890
	[HideInInspector]
	[SerializeField]
	private Font mDynamicFont;

	// Token: 0x04001ED3 RID: 7891
	[HideInInspector]
	[SerializeField]
	private int mDynamicFontSize = 16;

	// Token: 0x04001ED4 RID: 7892
	[HideInInspector]
	[SerializeField]
	private FontStyle mDynamicFontStyle;

	// Token: 0x04001ED5 RID: 7893
	[NonSerialized]
	private UISpriteData mSprite;

	// Token: 0x04001ED6 RID: 7894
	private int mPMA = -1;

	// Token: 0x04001ED7 RID: 7895
	private int mPacked = -1;
}
