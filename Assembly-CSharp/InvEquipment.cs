using System;
using UnityEngine;

// Token: 0x0200057F RID: 1407
[AddComponentMenu("NGUI/Examples/Equipment")]
public class InvEquipment : MonoBehaviour
{
	// Token: 0x1700073E RID: 1854
	// (get) Token: 0x06002FA6 RID: 12198 RVA: 0x000E8986 File Offset: 0x000E6D86
	public InvGameItem[] equippedItems
	{
		get
		{
			return this.mItems;
		}
	}

	// Token: 0x06002FA7 RID: 12199 RVA: 0x000E8990 File Offset: 0x000E6D90
	public InvGameItem Replace(InvBaseItem.Slot slot, InvGameItem item)
	{
		InvBaseItem invBaseItem = (item == null) ? null : item.baseItem;
		if (slot == InvBaseItem.Slot.None)
		{
			if (item != null)
			{
				Debug.LogWarning("Can't equip \"" + item.name + "\" because it doesn't specify an item slot");
			}
			return item;
		}
		if (invBaseItem != null && invBaseItem.slot != slot)
		{
			return item;
		}
		if (this.mItems == null)
		{
			int num = 8;
			this.mItems = new InvGameItem[num];
		}
		InvGameItem result = this.mItems[slot - InvBaseItem.Slot.Weapon];
		this.mItems[slot - InvBaseItem.Slot.Weapon] = item;
		if (this.mAttachments == null)
		{
			this.mAttachments = base.GetComponentsInChildren<InvAttachmentPoint>();
		}
		int i = 0;
		int num2 = this.mAttachments.Length;
		while (i < num2)
		{
			InvAttachmentPoint invAttachmentPoint = this.mAttachments[i];
			if (invAttachmentPoint.slot == slot)
			{
				GameObject gameObject = invAttachmentPoint.Attach((invBaseItem == null) ? null : invBaseItem.attachment);
				if (invBaseItem != null && gameObject != null)
				{
					Renderer component = gameObject.GetComponent<Renderer>();
					if (component != null)
					{
						component.material.color = invBaseItem.color;
					}
				}
			}
			i++;
		}
		return result;
	}

	// Token: 0x06002FA8 RID: 12200 RVA: 0x000E8AC0 File Offset: 0x000E6EC0
	public InvGameItem Equip(InvGameItem item)
	{
		if (item != null)
		{
			InvBaseItem baseItem = item.baseItem;
			if (baseItem != null)
			{
				return this.Replace(baseItem.slot, item);
			}
			Debug.LogWarning("Can't resolve the item ID of " + item.baseItemID);
		}
		return item;
	}

	// Token: 0x06002FA9 RID: 12201 RVA: 0x000E8B0C File Offset: 0x000E6F0C
	public InvGameItem Unequip(InvGameItem item)
	{
		if (item != null)
		{
			InvBaseItem baseItem = item.baseItem;
			if (baseItem != null)
			{
				return this.Replace(baseItem.slot, null);
			}
		}
		return item;
	}

	// Token: 0x06002FAA RID: 12202 RVA: 0x000E8B3B File Offset: 0x000E6F3B
	public InvGameItem Unequip(InvBaseItem.Slot slot)
	{
		return this.Replace(slot, null);
	}

	// Token: 0x06002FAB RID: 12203 RVA: 0x000E8B48 File Offset: 0x000E6F48
	public bool HasEquipped(InvGameItem item)
	{
		if (this.mItems != null)
		{
			int i = 0;
			int num = this.mItems.Length;
			while (i < num)
			{
				if (this.mItems[i] == item)
				{
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x06002FAC RID: 12204 RVA: 0x000E8B8C File Offset: 0x000E6F8C
	public bool HasEquipped(InvBaseItem.Slot slot)
	{
		if (this.mItems != null)
		{
			int i = 0;
			int num = this.mItems.Length;
			while (i < num)
			{
				InvBaseItem baseItem = this.mItems[i].baseItem;
				if (baseItem != null && baseItem.slot == slot)
				{
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x06002FAD RID: 12205 RVA: 0x000E8BE4 File Offset: 0x000E6FE4
	public InvGameItem GetItem(InvBaseItem.Slot slot)
	{
		if (slot != InvBaseItem.Slot.None)
		{
			int num = slot - InvBaseItem.Slot.Weapon;
			if (this.mItems != null && num < this.mItems.Length)
			{
				return this.mItems[num];
			}
		}
		return null;
	}

	// Token: 0x040019FE RID: 6654
	private InvGameItem[] mItems;

	// Token: 0x040019FF RID: 6655
	private InvAttachmentPoint[] mAttachments;
}
