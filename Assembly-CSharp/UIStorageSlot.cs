using System;
using UnityEngine;

// Token: 0x0200057A RID: 1402
[AddComponentMenu("NGUI/Examples/UI Storage Slot")]
public class UIStorageSlot : UIItemSlot
{
	// Token: 0x1700073C RID: 1852
	// (get) Token: 0x06002F96 RID: 12182 RVA: 0x000E8667 File Offset: 0x000E6A67
	protected override InvGameItem observedItem
	{
		get
		{
			return (!(this.storage != null)) ? null : this.storage.GetItem(this.slot);
		}
	}

	// Token: 0x06002F97 RID: 12183 RVA: 0x000E8691 File Offset: 0x000E6A91
	protected override InvGameItem Replace(InvGameItem item)
	{
		return (!(this.storage != null)) ? item : this.storage.Replace(this.slot, item);
	}

	// Token: 0x040019DF RID: 6623
	public UIItemStorage storage;

	// Token: 0x040019E0 RID: 6624
	public int slot;
}
