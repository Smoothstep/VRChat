using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000580 RID: 1408
[Serializable]
public class InvGameItem
{
	// Token: 0x06002FAE RID: 12206 RVA: 0x000E8C1E File Offset: 0x000E701E
	public InvGameItem(int id)
	{
		this.mBaseItemID = id;
	}

	// Token: 0x06002FAF RID: 12207 RVA: 0x000E8C3B File Offset: 0x000E703B
	public InvGameItem(int id, InvBaseItem bi)
	{
		this.mBaseItemID = id;
		this.mBaseItem = bi;
	}

	// Token: 0x1700073F RID: 1855
	// (get) Token: 0x06002FB0 RID: 12208 RVA: 0x000E8C5F File Offset: 0x000E705F
	public int baseItemID
	{
		get
		{
			return this.mBaseItemID;
		}
	}

	// Token: 0x17000740 RID: 1856
	// (get) Token: 0x06002FB1 RID: 12209 RVA: 0x000E8C67 File Offset: 0x000E7067
	public InvBaseItem baseItem
	{
		get
		{
			if (this.mBaseItem == null)
			{
				this.mBaseItem = InvDatabase.FindByID(this.baseItemID);
			}
			return this.mBaseItem;
		}
	}

	// Token: 0x17000741 RID: 1857
	// (get) Token: 0x06002FB2 RID: 12210 RVA: 0x000E8C8B File Offset: 0x000E708B
	public string name
	{
		get
		{
			if (this.baseItem == null)
			{
				return null;
			}
			return this.quality.ToString() + " " + this.baseItem.name;
		}
	}

	// Token: 0x17000742 RID: 1858
	// (get) Token: 0x06002FB3 RID: 12211 RVA: 0x000E8CC0 File Offset: 0x000E70C0
	public float statMultiplier
	{
		get
		{
			float num = 0f;
			switch (this.quality)
			{
			case InvGameItem.Quality.Broken:
				num = 0f;
				break;
			case InvGameItem.Quality.Cursed:
				num = -1f;
				break;
			case InvGameItem.Quality.Damaged:
				num = 0.25f;
				break;
			case InvGameItem.Quality.Worn:
				num = 0.9f;
				break;
			case InvGameItem.Quality.Sturdy:
				num = 1f;
				break;
			case InvGameItem.Quality.Polished:
				num = 1.1f;
				break;
			case InvGameItem.Quality.Improved:
				num = 1.25f;
				break;
			case InvGameItem.Quality.Crafted:
				num = 1.5f;
				break;
			case InvGameItem.Quality.Superior:
				num = 1.75f;
				break;
			case InvGameItem.Quality.Enchanted:
				num = 2f;
				break;
			case InvGameItem.Quality.Epic:
				num = 2.5f;
				break;
			case InvGameItem.Quality.Legendary:
				num = 3f;
				break;
			}
			float num2 = (float)this.itemLevel / 50f;
			return num * Mathf.Lerp(num2, num2 * num2, 0.5f);
		}
	}

	// Token: 0x17000743 RID: 1859
	// (get) Token: 0x06002FB4 RID: 12212 RVA: 0x000E8DBC File Offset: 0x000E71BC
	public Color color
	{
		get
		{
			Color result = Color.white;
			switch (this.quality)
			{
			case InvGameItem.Quality.Broken:
				result = new Color(0.4f, 0.2f, 0.2f);
				break;
			case InvGameItem.Quality.Cursed:
				result = Color.red;
				break;
			case InvGameItem.Quality.Damaged:
				result = new Color(0.4f, 0.4f, 0.4f);
				break;
			case InvGameItem.Quality.Worn:
				result = new Color(0.7f, 0.7f, 0.7f);
				break;
			case InvGameItem.Quality.Sturdy:
				result = new Color(1f, 1f, 1f);
				break;
			case InvGameItem.Quality.Polished:
				result = NGUIMath.HexToColor(3774856959u);
				break;
			case InvGameItem.Quality.Improved:
				result = NGUIMath.HexToColor(2480359935u);
				break;
			case InvGameItem.Quality.Crafted:
				result = NGUIMath.HexToColor(1325334783u);
				break;
			case InvGameItem.Quality.Superior:
				result = NGUIMath.HexToColor(12255231u);
				break;
			case InvGameItem.Quality.Enchanted:
				result = NGUIMath.HexToColor(1937178111u);
				break;
			case InvGameItem.Quality.Epic:
				result = NGUIMath.HexToColor(2516647935u);
				break;
			case InvGameItem.Quality.Legendary:
				result = NGUIMath.HexToColor(4287627519u);
				break;
			}
			return result;
		}
	}

	// Token: 0x06002FB5 RID: 12213 RVA: 0x000E8EFC File Offset: 0x000E72FC
	public List<InvStat> CalculateStats()
	{
		List<InvStat> list = new List<InvStat>();
		if (this.baseItem != null)
		{
			float statMultiplier = this.statMultiplier;
			List<InvStat> stats = this.baseItem.stats;
			int i = 0;
			int count = stats.Count;
			while (i < count)
			{
				InvStat invStat = stats[i];
				int num = Mathf.RoundToInt(statMultiplier * (float)invStat.amount);
				if (num != 0)
				{
					bool flag = false;
					int j = 0;
					int count2 = list.Count;
					while (j < count2)
					{
						InvStat invStat2 = list[j];
						if (invStat2.id == invStat.id && invStat2.modifier == invStat.modifier)
						{
							invStat2.amount += num;
							flag = true;
							break;
						}
						j++;
					}
					if (!flag)
					{
						list.Add(new InvStat
						{
							id = invStat.id,
							amount = num,
							modifier = invStat.modifier
						});
					}
				}
				i++;
			}
			List<InvStat> list2 = list;
			if (InvGameItem.f__mg0 == null)
			{
				InvGameItem.f__mg0 = new Comparison<InvStat>(InvStat.CompareArmor);
			}
			list2.Sort(InvGameItem.f__mg0);
		}
		return list;
	}

	// Token: 0x04001A00 RID: 6656
	[SerializeField]
	private int mBaseItemID;

	// Token: 0x04001A01 RID: 6657
	public InvGameItem.Quality quality = InvGameItem.Quality.Sturdy;

	// Token: 0x04001A02 RID: 6658
	public int itemLevel = 1;

	// Token: 0x04001A03 RID: 6659
	private InvBaseItem mBaseItem;

	// Token: 0x04001A04 RID: 6660
	[CompilerGenerated]
	private static Comparison<InvStat> f__mg0;

	// Token: 0x02000581 RID: 1409
	public enum Quality
	{
		// Token: 0x04001A06 RID: 6662
		Broken,
		// Token: 0x04001A07 RID: 6663
		Cursed,
		// Token: 0x04001A08 RID: 6664
		Damaged,
		// Token: 0x04001A09 RID: 6665
		Worn,
		// Token: 0x04001A0A RID: 6666
		Sturdy,
		// Token: 0x04001A0B RID: 6667
		Polished,
		// Token: 0x04001A0C RID: 6668
		Improved,
		// Token: 0x04001A0D RID: 6669
		Crafted,
		// Token: 0x04001A0E RID: 6670
		Superior,
		// Token: 0x04001A0F RID: 6671
		Enchanted,
		// Token: 0x04001A10 RID: 6672
		Epic,
		// Token: 0x04001A11 RID: 6673
		Legendary,
		// Token: 0x04001A12 RID: 6674
		_LastDoNotUse
	}
}
