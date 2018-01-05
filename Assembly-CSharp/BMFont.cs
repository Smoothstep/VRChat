using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005E3 RID: 1507
[Serializable]
public class BMFont
{
	// Token: 0x17000778 RID: 1912
	// (get) Token: 0x060031CD RID: 12749 RVA: 0x000F6FBD File Offset: 0x000F53BD
	public bool isValid
	{
		get
		{
			return this.mSaved.Count > 0;
		}
	}

	// Token: 0x17000779 RID: 1913
	// (get) Token: 0x060031CE RID: 12750 RVA: 0x000F6FCD File Offset: 0x000F53CD
	// (set) Token: 0x060031CF RID: 12751 RVA: 0x000F6FD5 File Offset: 0x000F53D5
	public int charSize
	{
		get
		{
			return this.mSize;
		}
		set
		{
			this.mSize = value;
		}
	}

	// Token: 0x1700077A RID: 1914
	// (get) Token: 0x060031D0 RID: 12752 RVA: 0x000F6FDE File Offset: 0x000F53DE
	// (set) Token: 0x060031D1 RID: 12753 RVA: 0x000F6FE6 File Offset: 0x000F53E6
	public int baseOffset
	{
		get
		{
			return this.mBase;
		}
		set
		{
			this.mBase = value;
		}
	}

	// Token: 0x1700077B RID: 1915
	// (get) Token: 0x060031D2 RID: 12754 RVA: 0x000F6FEF File Offset: 0x000F53EF
	// (set) Token: 0x060031D3 RID: 12755 RVA: 0x000F6FF7 File Offset: 0x000F53F7
	public int texWidth
	{
		get
		{
			return this.mWidth;
		}
		set
		{
			this.mWidth = value;
		}
	}

	// Token: 0x1700077C RID: 1916
	// (get) Token: 0x060031D4 RID: 12756 RVA: 0x000F7000 File Offset: 0x000F5400
	// (set) Token: 0x060031D5 RID: 12757 RVA: 0x000F7008 File Offset: 0x000F5408
	public int texHeight
	{
		get
		{
			return this.mHeight;
		}
		set
		{
			this.mHeight = value;
		}
	}

	// Token: 0x1700077D RID: 1917
	// (get) Token: 0x060031D6 RID: 12758 RVA: 0x000F7011 File Offset: 0x000F5411
	public int glyphCount
	{
		get
		{
			return (!this.isValid) ? 0 : this.mSaved.Count;
		}
	}

	// Token: 0x1700077E RID: 1918
	// (get) Token: 0x060031D7 RID: 12759 RVA: 0x000F702F File Offset: 0x000F542F
	// (set) Token: 0x060031D8 RID: 12760 RVA: 0x000F7037 File Offset: 0x000F5437
	public string spriteName
	{
		get
		{
			return this.mSpriteName;
		}
		set
		{
			this.mSpriteName = value;
		}
	}

	// Token: 0x1700077F RID: 1919
	// (get) Token: 0x060031D9 RID: 12761 RVA: 0x000F7040 File Offset: 0x000F5440
	public List<BMGlyph> glyphs
	{
		get
		{
			return this.mSaved;
		}
	}

	// Token: 0x060031DA RID: 12762 RVA: 0x000F7048 File Offset: 0x000F5448
	public BMGlyph GetGlyph(int index, bool createIfMissing)
	{
		BMGlyph bmglyph = null;
		if (this.mDict.Count == 0)
		{
			int i = 0;
			int count = this.mSaved.Count;
			while (i < count)
			{
				BMGlyph bmglyph2 = this.mSaved[i];
				this.mDict.Add(bmglyph2.index, bmglyph2);
				i++;
			}
		}
		if (!this.mDict.TryGetValue(index, out bmglyph) && createIfMissing)
		{
			bmglyph = new BMGlyph();
			bmglyph.index = index;
			this.mSaved.Add(bmglyph);
			this.mDict.Add(index, bmglyph);
		}
		return bmglyph;
	}

	// Token: 0x060031DB RID: 12763 RVA: 0x000F70E4 File Offset: 0x000F54E4
	public BMGlyph GetGlyph(int index)
	{
		return this.GetGlyph(index, false);
	}

	// Token: 0x060031DC RID: 12764 RVA: 0x000F70EE File Offset: 0x000F54EE
	public void Clear()
	{
		this.mDict.Clear();
		this.mSaved.Clear();
	}

	// Token: 0x060031DD RID: 12765 RVA: 0x000F7108 File Offset: 0x000F5508
	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		if (this.isValid)
		{
			int i = 0;
			int count = this.mSaved.Count;
			while (i < count)
			{
				BMGlyph bmglyph = this.mSaved[i];
				if (bmglyph != null)
				{
					bmglyph.Trim(xMin, yMin, xMax, yMax);
				}
				i++;
			}
		}
	}

	// Token: 0x04001C77 RID: 7287
	[HideInInspector]
	[SerializeField]
	private int mSize = 16;

	// Token: 0x04001C78 RID: 7288
	[HideInInspector]
	[SerializeField]
	private int mBase;

	// Token: 0x04001C79 RID: 7289
	[HideInInspector]
	[SerializeField]
	private int mWidth;

	// Token: 0x04001C7A RID: 7290
	[HideInInspector]
	[SerializeField]
	private int mHeight;

	// Token: 0x04001C7B RID: 7291
	[HideInInspector]
	[SerializeField]
	private string mSpriteName;

	// Token: 0x04001C7C RID: 7292
	[HideInInspector]
	[SerializeField]
	private List<BMGlyph> mSaved = new List<BMGlyph>();

	// Token: 0x04001C7D RID: 7293
	private Dictionary<int, BMGlyph> mDict = new Dictionary<int, BMGlyph>();
}
