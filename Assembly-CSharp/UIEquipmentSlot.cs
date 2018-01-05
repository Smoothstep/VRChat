using System;
using UnityEngine;

// Token: 0x02000577 RID: 1399
[AddComponentMenu("NGUI/Examples/UI Equipment Slot")]
public class UIEquipmentSlot : UIItemSlot
{
	// Token: 0x17000739 RID: 1849
	// (get) Token: 0x06002F85 RID: 12165 RVA: 0x000E83C4 File Offset: 0x000E67C4
	protected override InvGameItem observedItem
	{
		get
		{
			return (!(this.equipment != null)) ? null : this.equipment.GetItem(this.slot);
		}
	}

	// Token: 0x06002F86 RID: 12166 RVA: 0x000E83EE File Offset: 0x000E67EE
	protected override InvGameItem Replace(InvGameItem item)
	{
		return (!(this.equipment != null)) ? item : this.equipment.Replace(this.slot, item);
	}

	// Token: 0x040019CC RID: 6604
	public InvEquipment equipment;

	// Token: 0x040019CD RID: 6605
	public InvBaseItem.Slot slot;
}
