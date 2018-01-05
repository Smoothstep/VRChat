using System;
using System.Collections.Generic;

// Token: 0x020005E4 RID: 1508
[Serializable]
public class BMGlyph
{
	// Token: 0x060031DF RID: 12767 RVA: 0x000F7164 File Offset: 0x000F5564
	public int GetKerning(int previousChar)
	{
		if (this.kerning != null && previousChar != 0)
		{
			int i = 0;
			int count = this.kerning.Count;
			while (i < count)
			{
				if (this.kerning[i] == previousChar)
				{
					return this.kerning[i + 1];
				}
				i += 2;
			}
		}
		return 0;
	}

	// Token: 0x060031E0 RID: 12768 RVA: 0x000F71C4 File Offset: 0x000F55C4
	public void SetKerning(int previousChar, int amount)
	{
		if (this.kerning == null)
		{
			this.kerning = new List<int>();
		}
		for (int i = 0; i < this.kerning.Count; i += 2)
		{
			if (this.kerning[i] == previousChar)
			{
				this.kerning[i + 1] = amount;
				return;
			}
		}
		this.kerning.Add(previousChar);
		this.kerning.Add(amount);
	}

	// Token: 0x060031E1 RID: 12769 RVA: 0x000F7240 File Offset: 0x000F5640
	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		int num = this.x + this.width;
		int num2 = this.y + this.height;
		if (this.x < xMin)
		{
			int num3 = xMin - this.x;
			this.x += num3;
			this.width -= num3;
			this.offsetX += num3;
		}
		if (this.y < yMin)
		{
			int num4 = yMin - this.y;
			this.y += num4;
			this.height -= num4;
			this.offsetY += num4;
		}
		if (num > xMax)
		{
			this.width -= num - xMax;
		}
		if (num2 > yMax)
		{
			this.height -= num2 - yMax;
		}
	}

	// Token: 0x04001C7E RID: 7294
	public int index;

	// Token: 0x04001C7F RID: 7295
	public int x;

	// Token: 0x04001C80 RID: 7296
	public int y;

	// Token: 0x04001C81 RID: 7297
	public int width;

	// Token: 0x04001C82 RID: 7298
	public int height;

	// Token: 0x04001C83 RID: 7299
	public int offsetX;

	// Token: 0x04001C84 RID: 7300
	public int offsetY;

	// Token: 0x04001C85 RID: 7301
	public int advance;

	// Token: 0x04001C86 RID: 7302
	public int channel;

	// Token: 0x04001C87 RID: 7303
	public List<int> kerning;
}
