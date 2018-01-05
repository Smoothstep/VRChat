using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200057C RID: 1404
[Serializable]
public class InvBaseItem
{
	// Token: 0x040019E4 RID: 6628
	public int id16;

	// Token: 0x040019E5 RID: 6629
	public string name;

	// Token: 0x040019E6 RID: 6630
	public string description;

	// Token: 0x040019E7 RID: 6631
	public InvBaseItem.Slot slot;

	// Token: 0x040019E8 RID: 6632
	public int minItemLevel = 1;

	// Token: 0x040019E9 RID: 6633
	public int maxItemLevel = 50;

	// Token: 0x040019EA RID: 6634
	public List<InvStat> stats = new List<InvStat>();

	// Token: 0x040019EB RID: 6635
	public GameObject attachment;

	// Token: 0x040019EC RID: 6636
	public Color color = Color.white;

	// Token: 0x040019ED RID: 6637
	public UIAtlas iconAtlas;

	// Token: 0x040019EE RID: 6638
	public string iconName = string.Empty;

	// Token: 0x0200057D RID: 1405
	public enum Slot
	{
		// Token: 0x040019F0 RID: 6640
		None,
		// Token: 0x040019F1 RID: 6641
		Weapon,
		// Token: 0x040019F2 RID: 6642
		Shield,
		// Token: 0x040019F3 RID: 6643
		Body,
		// Token: 0x040019F4 RID: 6644
		Shoulders,
		// Token: 0x040019F5 RID: 6645
		Bracers,
		// Token: 0x040019F6 RID: 6646
		Boots,
		// Token: 0x040019F7 RID: 6647
		Trinket,
		// Token: 0x040019F8 RID: 6648
		_LastDoNotUse
	}
}
