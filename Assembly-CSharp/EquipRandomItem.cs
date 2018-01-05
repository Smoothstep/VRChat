using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000575 RID: 1397
[AddComponentMenu("NGUI/Examples/Equip Random Item")]
public class EquipRandomItem : MonoBehaviour
{
	// Token: 0x06002F7C RID: 12156 RVA: 0x000E7C5C File Offset: 0x000E605C
	private void OnClick()
	{
		if (this.equipment == null)
		{
			return;
		}
		List<InvBaseItem> items = InvDatabase.list[0].items;
		if (items.Count == 0)
		{
			return;
		}
		int max = 12;
		int num = UnityEngine.Random.Range(0, items.Count);
		InvBaseItem invBaseItem = items[num];
		InvGameItem invGameItem = new InvGameItem(num, invBaseItem);
		invGameItem.quality = (InvGameItem.Quality)UnityEngine.Random.Range(0, max);
		invGameItem.itemLevel = NGUITools.RandomRange(invBaseItem.minItemLevel, invBaseItem.maxItemLevel);
		this.equipment.Equip(invGameItem);
	}

	// Token: 0x040019C5 RID: 6597
	public InvEquipment equipment;
}
