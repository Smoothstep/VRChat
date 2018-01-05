using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200062D RID: 1581
[AddComponentMenu("NGUI/UI/Atlas")]
public class UIAtlas : MonoBehaviour
{
	// Token: 0x1700080C RID: 2060
	// (get) Token: 0x060034F4 RID: 13556 RVA: 0x0010B749 File Offset: 0x00109B49
	// (set) Token: 0x060034F5 RID: 13557 RVA: 0x0010B774 File Offset: 0x00109B74
	public Material spriteMaterial
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.material : this.mReplacement.spriteMaterial;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteMaterial = value;
			}
			else if (this.material == null)
			{
				this.mPMA = 0;
				this.material = value;
			}
			else
			{
				this.MarkAsChanged();
				this.mPMA = -1;
				this.material = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700080D RID: 2061
	// (get) Token: 0x060034F6 RID: 13558 RVA: 0x0010B7E4 File Offset: 0x00109BE4
	public bool premultipliedAlpha
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.premultipliedAlpha;
			}
			if (this.mPMA == -1)
			{
				Material spriteMaterial = this.spriteMaterial;
				this.mPMA = ((!(spriteMaterial != null) || !(spriteMaterial.shader != null) || !spriteMaterial.shader.name.Contains("Premultiplied")) ? 0 : 1);
			}
			return this.mPMA == 1;
		}
	}

	// Token: 0x1700080E RID: 2062
	// (get) Token: 0x060034F7 RID: 13559 RVA: 0x0010B86E File Offset: 0x00109C6E
	// (set) Token: 0x060034F8 RID: 13560 RVA: 0x0010B8AA File Offset: 0x00109CAA
	public List<UISpriteData> spriteList
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.spriteList;
			}
			if (this.mSprites.Count == 0)
			{
				this.Upgrade();
			}
			return this.mSprites;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteList = value;
			}
			else
			{
				this.mSprites = value;
			}
		}
	}

	// Token: 0x1700080F RID: 2063
	// (get) Token: 0x060034F9 RID: 13561 RVA: 0x0010B8D8 File Offset: 0x00109CD8
	public Texture texture
	{
		get
		{
			return (!(this.mReplacement != null)) ? ((!(this.material != null)) ? null : this.material.mainTexture) : this.mReplacement.texture;
		}
	}

	// Token: 0x17000810 RID: 2064
	// (get) Token: 0x060034FA RID: 13562 RVA: 0x0010B928 File Offset: 0x00109D28
	// (set) Token: 0x060034FB RID: 13563 RVA: 0x0010B954 File Offset: 0x00109D54
	public float pixelSize
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mPixelSize : this.mReplacement.pixelSize;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.pixelSize = value;
			}
			else
			{
				float num = Mathf.Clamp(value, 0.25f, 4f);
				if (this.mPixelSize != num)
				{
					this.mPixelSize = num;
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x17000811 RID: 2065
	// (get) Token: 0x060034FC RID: 13564 RVA: 0x0010B9AD File Offset: 0x00109DAD
	// (set) Token: 0x060034FD RID: 13565 RVA: 0x0010B9B8 File Offset: 0x00109DB8
	public UIAtlas replacement
	{
		get
		{
			return this.mReplacement;
		}
		set
		{
			UIAtlas uiatlas = value;
			if (uiatlas == this)
			{
				uiatlas = null;
			}
			if (this.mReplacement != uiatlas)
			{
				if (uiatlas != null && uiatlas.replacement == this)
				{
					uiatlas.replacement = null;
				}
				if (this.mReplacement != null)
				{
					this.MarkAsChanged();
				}
				this.mReplacement = uiatlas;
				if (uiatlas != null)
				{
					this.material = null;
				}
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x060034FE RID: 13566 RVA: 0x0010BA44 File Offset: 0x00109E44
	public UISpriteData GetSprite(string name)
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetSprite(name);
		}
		if (!string.IsNullOrEmpty(name))
		{
			if (this.mSprites.Count == 0)
			{
				this.Upgrade();
			}
			if (this.mSprites.Count == 0)
			{
				return null;
			}
			if (this.mSpriteIndices.Count != this.mSprites.Count)
			{
				this.MarkSpriteListAsChanged();
			}
			int num;
			if (this.mSpriteIndices.TryGetValue(name, out num))
			{
				if (num > -1 && num < this.mSprites.Count)
				{
					return this.mSprites[num];
				}
				this.MarkSpriteListAsChanged();
				return (!this.mSpriteIndices.TryGetValue(name, out num)) ? null : this.mSprites[num];
			}
			else
			{
				int i = 0;
				int count = this.mSprites.Count;
				while (i < count)
				{
					UISpriteData uispriteData = this.mSprites[i];
					if (!string.IsNullOrEmpty(uispriteData.name) && name == uispriteData.name)
					{
						this.MarkSpriteListAsChanged();
						return uispriteData;
					}
					i++;
				}
			}
		}
		return null;
	}

	// Token: 0x060034FF RID: 13567 RVA: 0x0010BB80 File Offset: 0x00109F80
	public void MarkSpriteListAsChanged()
	{
		this.mSpriteIndices.Clear();
		int i = 0;
		int count = this.mSprites.Count;
		while (i < count)
		{
			this.mSpriteIndices[this.mSprites[i].name] = i;
			i++;
		}
	}

	// Token: 0x06003500 RID: 13568 RVA: 0x0010BBD3 File Offset: 0x00109FD3
	public void SortAlphabetically()
	{
		this.mSprites.Sort((UISpriteData s1, UISpriteData s2) => s1.name.CompareTo(s2.name));
	}

	// Token: 0x06003501 RID: 13569 RVA: 0x0010BC00 File Offset: 0x0010A000
	public BetterList<string> GetListOfSprites()
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetListOfSprites();
		}
		if (this.mSprites.Count == 0)
		{
			this.Upgrade();
		}
		BetterList<string> betterList = new BetterList<string>();
		int i = 0;
		int count = this.mSprites.Count;
		while (i < count)
		{
			UISpriteData uispriteData = this.mSprites[i];
			if (uispriteData != null && !string.IsNullOrEmpty(uispriteData.name))
			{
				betterList.Add(uispriteData.name);
			}
			i++;
		}
		return betterList;
	}

	// Token: 0x06003502 RID: 13570 RVA: 0x0010BC98 File Offset: 0x0010A098
	public BetterList<string> GetListOfSprites(string match)
	{
		if (this.mReplacement)
		{
			return this.mReplacement.GetListOfSprites(match);
		}
		if (string.IsNullOrEmpty(match))
		{
			return this.GetListOfSprites();
		}
		if (this.mSprites.Count == 0)
		{
			this.Upgrade();
		}
		BetterList<string> betterList = new BetterList<string>();
		int i = 0;
		int count = this.mSprites.Count;
		while (i < count)
		{
			UISpriteData uispriteData = this.mSprites[i];
			if (uispriteData != null && !string.IsNullOrEmpty(uispriteData.name) && string.Equals(match, uispriteData.name, StringComparison.OrdinalIgnoreCase))
			{
				betterList.Add(uispriteData.name);
				return betterList;
			}
			i++;
		}
		string[] array = match.Split(new char[]
		{
			' '
		}, StringSplitOptions.RemoveEmptyEntries);
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = array[j].ToLower();
		}
		int k = 0;
		int count2 = this.mSprites.Count;
		while (k < count2)
		{
			UISpriteData uispriteData2 = this.mSprites[k];
			if (uispriteData2 != null && !string.IsNullOrEmpty(uispriteData2.name))
			{
				string text = uispriteData2.name.ToLower();
				int num = 0;
				for (int l = 0; l < array.Length; l++)
				{
					if (text.Contains(array[l]))
					{
						num++;
					}
				}
				if (num == array.Length)
				{
					betterList.Add(uispriteData2.name);
				}
			}
			k++;
		}
		return betterList;
	}

	// Token: 0x06003503 RID: 13571 RVA: 0x0010BE34 File Offset: 0x0010A234
	private bool References(UIAtlas atlas)
	{
		return !(atlas == null) && (atlas == this || (this.mReplacement != null && this.mReplacement.References(atlas)));
	}

	// Token: 0x06003504 RID: 13572 RVA: 0x0010BE80 File Offset: 0x0010A280
	public static bool CheckIfRelated(UIAtlas a, UIAtlas b)
	{
		return !(a == null) && !(b == null) && (a == b || a.References(b) || b.References(a));
	}

	// Token: 0x06003505 RID: 13573 RVA: 0x0010BEC0 File Offset: 0x0010A2C0
	public void MarkAsChanged()
	{
		if (this.mReplacement != null)
		{
			this.mReplacement.MarkAsChanged();
		}
		UISprite[] array = NGUITools.FindActive<UISprite>();
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			UISprite uisprite = array[i];
			if (UIAtlas.CheckIfRelated(this, uisprite.atlas))
			{
				UIAtlas atlas = uisprite.atlas;
				uisprite.atlas = null;
				uisprite.atlas = atlas;
			}
			i++;
		}
		UIFont[] array2 = Resources.FindObjectsOfTypeAll(typeof(UIFont)) as UIFont[];
		int j = 0;
		int num2 = array2.Length;
		while (j < num2)
		{
			UIFont uifont = array2[j];
			if (UIAtlas.CheckIfRelated(this, uifont.atlas))
			{
				UIAtlas atlas2 = uifont.atlas;
				uifont.atlas = null;
				uifont.atlas = atlas2;
			}
			j++;
		}
		UILabel[] array3 = NGUITools.FindActive<UILabel>();
		int k = 0;
		int num3 = array3.Length;
		while (k < num3)
		{
			UILabel uilabel = array3[k];
			if (uilabel.bitmapFont != null && UIAtlas.CheckIfRelated(this, uilabel.bitmapFont.atlas))
			{
				UIFont bitmapFont = uilabel.bitmapFont;
				uilabel.bitmapFont = null;
				uilabel.bitmapFont = bitmapFont;
			}
			k++;
		}
	}

	// Token: 0x06003506 RID: 13574 RVA: 0x0010C008 File Offset: 0x0010A408
	private bool Upgrade()
	{
		if (this.mReplacement)
		{
			return this.mReplacement.Upgrade();
		}
		if (this.mSprites.Count == 0 && this.sprites.Count > 0 && this.material)
		{
			Texture mainTexture = this.material.mainTexture;
			int width = (!(mainTexture != null)) ? 512 : mainTexture.width;
			int height = (!(mainTexture != null)) ? 512 : mainTexture.height;
			for (int i = 0; i < this.sprites.Count; i++)
			{
				UIAtlas.Sprite sprite = this.sprites[i];
				Rect outer = sprite.outer;
				Rect inner = sprite.inner;
				if (this.mCoordinates == UIAtlas.Coordinates.TexCoords)
				{
					NGUIMath.ConvertToPixels(outer, width, height, true);
					NGUIMath.ConvertToPixels(inner, width, height, true);
				}
				UISpriteData uispriteData = new UISpriteData();
				uispriteData.name = sprite.name;
				uispriteData.x = Mathf.RoundToInt(outer.xMin);
				uispriteData.y = Mathf.RoundToInt(outer.yMin);
				uispriteData.width = Mathf.RoundToInt(outer.width);
				uispriteData.height = Mathf.RoundToInt(outer.height);
				uispriteData.paddingLeft = Mathf.RoundToInt(sprite.paddingLeft * outer.width);
				uispriteData.paddingRight = Mathf.RoundToInt(sprite.paddingRight * outer.width);
				uispriteData.paddingBottom = Mathf.RoundToInt(sprite.paddingBottom * outer.height);
				uispriteData.paddingTop = Mathf.RoundToInt(sprite.paddingTop * outer.height);
				uispriteData.borderLeft = Mathf.RoundToInt(inner.xMin - outer.xMin);
				uispriteData.borderRight = Mathf.RoundToInt(outer.xMax - inner.xMax);
				uispriteData.borderBottom = Mathf.RoundToInt(outer.yMax - inner.yMax);
				uispriteData.borderTop = Mathf.RoundToInt(inner.yMin - outer.yMin);
				this.mSprites.Add(uispriteData);
			}
			this.sprites.Clear();
			return true;
		}
		return false;
	}

	// Token: 0x04001E43 RID: 7747
	[HideInInspector]
	[SerializeField]
	private Material material;

	// Token: 0x04001E44 RID: 7748
	[HideInInspector]
	[SerializeField]
	private List<UISpriteData> mSprites = new List<UISpriteData>();

	// Token: 0x04001E45 RID: 7749
	[HideInInspector]
	[SerializeField]
	private float mPixelSize = 1f;

	// Token: 0x04001E46 RID: 7750
	[HideInInspector]
	[SerializeField]
	private UIAtlas mReplacement;

	// Token: 0x04001E47 RID: 7751
	[HideInInspector]
	[SerializeField]
	private UIAtlas.Coordinates mCoordinates;

	// Token: 0x04001E48 RID: 7752
	[HideInInspector]
	[SerializeField]
	private List<UIAtlas.Sprite> sprites = new List<UIAtlas.Sprite>();

	// Token: 0x04001E49 RID: 7753
	private int mPMA = -1;

	// Token: 0x04001E4A RID: 7754
	private Dictionary<string, int> mSpriteIndices = new Dictionary<string, int>();

	// Token: 0x0200062E RID: 1582
	[Serializable]
	private class Sprite
	{
		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06003509 RID: 13577 RVA: 0x0010C2CC File Offset: 0x0010A6CC
		public bool hasPadding
		{
			get
			{
				return this.paddingLeft != 0f || this.paddingRight != 0f || this.paddingTop != 0f || this.paddingBottom != 0f;
			}
		}

		// Token: 0x04001E4C RID: 7756
		public string name = "Unity Bug";

		// Token: 0x04001E4D RID: 7757
		public Rect outer = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04001E4E RID: 7758
		public Rect inner = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04001E4F RID: 7759
		public bool rotated;

		// Token: 0x04001E50 RID: 7760
		public float paddingLeft;

		// Token: 0x04001E51 RID: 7761
		public float paddingRight;

		// Token: 0x04001E52 RID: 7762
		public float paddingTop;

		// Token: 0x04001E53 RID: 7763
		public float paddingBottom;
	}

	// Token: 0x0200062F RID: 1583
	private enum Coordinates
	{
		// Token: 0x04001E55 RID: 7765
		Pixels,
		// Token: 0x04001E56 RID: 7766
		TexCoords
	}
}
