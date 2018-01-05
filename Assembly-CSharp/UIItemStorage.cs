using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000579 RID: 1401
[AddComponentMenu("NGUI/Examples/UI Item Storage")]
public class UIItemStorage : MonoBehaviour
{
	// Token: 0x1700073B RID: 1851
	// (get) Token: 0x06002F91 RID: 12177 RVA: 0x000E8454 File Offset: 0x000E6854
	public List<InvGameItem> items
	{
		get
		{
			while (this.mItems.Count < this.maxItemCount)
			{
				this.mItems.Add(null);
			}
			return this.mItems;
		}
	}

	// Token: 0x06002F92 RID: 12178 RVA: 0x000E8483 File Offset: 0x000E6883
	public InvGameItem GetItem(int slot)
	{
		return (slot >= this.items.Count) ? null : this.mItems[slot];
	}

	// Token: 0x06002F93 RID: 12179 RVA: 0x000E84A8 File Offset: 0x000E68A8
	public InvGameItem Replace(int slot, InvGameItem item)
	{
		if (slot < this.maxItemCount)
		{
			InvGameItem result = this.items[slot];
			this.mItems[slot] = item;
			return result;
		}
		return item;
	}

	// Token: 0x06002F94 RID: 12180 RVA: 0x000E84E0 File Offset: 0x000E68E0
	private void Start()
	{
		if (this.template != null)
		{
			int num = 0;
			Bounds bounds = default(Bounds);
			for (int i = 0; i < this.maxRows; i++)
			{
				for (int j = 0; j < this.maxColumns; j++)
				{
					GameObject gameObject = NGUITools.AddChild(base.gameObject, this.template);
					Transform transform = gameObject.transform;
					transform.localPosition = new Vector3((float)this.padding + ((float)j + 0.5f) * (float)this.spacing, (float)(-(float)this.padding) - ((float)i + 0.5f) * (float)this.spacing, 0f);
					UIStorageSlot component = gameObject.GetComponent<UIStorageSlot>();
					if (component != null)
					{
						component.storage = this;
						component.slot = num;
					}
					bounds.Encapsulate(new Vector3((float)this.padding * 2f + (float)((j + 1) * this.spacing), (float)(-(float)this.padding) * 2f - (float)((i + 1) * this.spacing), 0f));
					if (++num >= this.maxItemCount)
					{
						if (this.background != null)
						{
							this.background.transform.localScale = bounds.size;
						}
						return;
					}
				}
			}
			if (this.background != null)
			{
				this.background.transform.localScale = bounds.size;
			}
		}
	}

	// Token: 0x040019D7 RID: 6615
	public int maxItemCount = 8;

	// Token: 0x040019D8 RID: 6616
	public int maxRows = 4;

	// Token: 0x040019D9 RID: 6617
	public int maxColumns = 4;

	// Token: 0x040019DA RID: 6618
	public GameObject template;

	// Token: 0x040019DB RID: 6619
	public UIWidget background;

	// Token: 0x040019DC RID: 6620
	public int spacing = 128;

	// Token: 0x040019DD RID: 6621
	public int padding = 10;

	// Token: 0x040019DE RID: 6622
	private List<InvGameItem> mItems = new List<InvGameItem>();
}
