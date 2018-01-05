using System;

// Token: 0x02000658 RID: 1624
[Serializable]
public class UISpriteData
{
	// Token: 0x17000897 RID: 2199
	// (get) Token: 0x060036CC RID: 14028 RVA: 0x00118136 File Offset: 0x00116536
	public bool hasBorder
	{
		get
		{
			return (this.borderLeft | this.borderRight | this.borderTop | this.borderBottom) != 0;
		}
	}

	// Token: 0x17000898 RID: 2200
	// (get) Token: 0x060036CD RID: 14029 RVA: 0x00118159 File Offset: 0x00116559
	public bool hasPadding
	{
		get
		{
			return (this.paddingLeft | this.paddingRight | this.paddingTop | this.paddingBottom) != 0;
		}
	}

	// Token: 0x060036CE RID: 14030 RVA: 0x0011817C File Offset: 0x0011657C
	public void SetRect(int x, int y, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}

	// Token: 0x060036CF RID: 14031 RVA: 0x0011819B File Offset: 0x0011659B
	public void SetPadding(int left, int bottom, int right, int top)
	{
		this.paddingLeft = left;
		this.paddingBottom = bottom;
		this.paddingRight = right;
		this.paddingTop = top;
	}

	// Token: 0x060036D0 RID: 14032 RVA: 0x001181BA File Offset: 0x001165BA
	public void SetBorder(int left, int bottom, int right, int top)
	{
		this.borderLeft = left;
		this.borderBottom = bottom;
		this.borderRight = right;
		this.borderTop = top;
	}

	// Token: 0x060036D1 RID: 14033 RVA: 0x001181DC File Offset: 0x001165DC
	public void CopyFrom(UISpriteData sd)
	{
		this.name = sd.name;
		this.x = sd.x;
		this.y = sd.y;
		this.width = sd.width;
		this.height = sd.height;
		this.borderLeft = sd.borderLeft;
		this.borderRight = sd.borderRight;
		this.borderTop = sd.borderTop;
		this.borderBottom = sd.borderBottom;
		this.paddingLeft = sd.paddingLeft;
		this.paddingRight = sd.paddingRight;
		this.paddingTop = sd.paddingTop;
		this.paddingBottom = sd.paddingBottom;
	}

	// Token: 0x060036D2 RID: 14034 RVA: 0x00118285 File Offset: 0x00116685
	public void CopyBorderFrom(UISpriteData sd)
	{
		this.borderLeft = sd.borderLeft;
		this.borderRight = sd.borderRight;
		this.borderTop = sd.borderTop;
		this.borderBottom = sd.borderBottom;
	}

	// Token: 0x04001FA5 RID: 8101
	public string name = "Sprite";

	// Token: 0x04001FA6 RID: 8102
	public int x;

	// Token: 0x04001FA7 RID: 8103
	public int y;

	// Token: 0x04001FA8 RID: 8104
	public int width;

	// Token: 0x04001FA9 RID: 8105
	public int height;

	// Token: 0x04001FAA RID: 8106
	public int borderLeft;

	// Token: 0x04001FAB RID: 8107
	public int borderRight;

	// Token: 0x04001FAC RID: 8108
	public int borderTop;

	// Token: 0x04001FAD RID: 8109
	public int borderBottom;

	// Token: 0x04001FAE RID: 8110
	public int paddingLeft;

	// Token: 0x04001FAF RID: 8111
	public int paddingRight;

	// Token: 0x04001FB0 RID: 8112
	public int paddingTop;

	// Token: 0x04001FB1 RID: 8113
	public int paddingBottom;
}
