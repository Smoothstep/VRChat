using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UIWidgets
{
	// Token: 0x02000947 RID: 2375
	public class ListViewCustomHeight<TComponent, TItem> : ListViewCustom<TComponent, TItem> where TComponent : ListViewItem, IListViewItemHeight where TItem : IItemHeight
	{
		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x060047EC RID: 18412 RVA: 0x0017C6B4 File Offset: 0x0017AAB4
		protected TComponent DefaultItemCopy
		{
			get
			{
				if (this.defaultItemCopy == null)
				{
					this.defaultItemCopy = UnityEngine.Object.Instantiate<TComponent>(this.DefaultItem);
					Utilites.FixInstantiated(this.DefaultItem, this.defaultItemCopy);
					this.defaultItemCopy.transform.SetParent(this.DefaultItem.transform.parent);
					this.defaultItemCopy.gameObject.name = "copy";
					this.defaultItemCopy.gameObject.SetActive(false);
				}
				return this.defaultItemCopy;
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x060047ED RID: 18413 RVA: 0x0017C767 File Offset: 0x0017AB67
		protected RectTransform DefaultItemCopyRect
		{
			get
			{
				if (this.defaultItemCopyRect == null)
				{
					this.defaultItemCopyRect = this.defaultItemCopy.GetComponent<RectTransform>();
				}
				return this.defaultItemCopyRect;
			}
		}

		// Token: 0x060047EE RID: 18414 RVA: 0x0017C797 File Offset: 0x0017AB97
		protected override void Awake()
		{
			this.Start();
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x0017C79F File Offset: 0x0017AB9F
		protected override void CalculateMaxVisibleItems()
		{
			this.SetItemsHeight(this.customItems);
			base.CalculateMaxVisibleItems();
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x0017C7B4 File Offset: 0x0017ABB4
		protected override void ScrollTo(int index)
		{
			if (!base.CanOptimize())
			{
				return;
			}
			float scrollMargin = base.GetScrollMargin();
			float num = base.GetScrollMargin() + this.scrollHeight;
			float num2 = this.ItemStartAt(index);
			float num3 = this.ItemEndAt(index) + this.layout.GetMarginTop();
			if (num2 < scrollMargin)
			{
				float num4 = 1f - this.scrollRect.verticalScrollbar.size - num2 / base.FullHeight();
				float value = num4 / (1f - this.scrollRect.verticalScrollbar.size);
				this.scrollRect.verticalScrollbar.value = value;
			}
			else if (num3 > num)
			{
				float num5 = (base.FullHeight() - num3) / base.FullHeight();
				float value2 = num5 / (1f - this.scrollRect.verticalScrollbar.size);
				this.scrollRect.verticalScrollbar.value = value2;
			}
		}

		// Token: 0x060047F1 RID: 18417 RVA: 0x0017C8A0 File Offset: 0x0017ACA0
		protected override float CalculateBottomFillerHeight()
		{
			float num = this.customItems.GetRange(this.topHiddenItems + this.visibleItems, this.bottomHiddenItems).Sum((TItem x) => x.Height);
			return num + this.layout.Spacing.y * (float)(this.topHiddenItems - 1);
		}

		// Token: 0x060047F2 RID: 18418 RVA: 0x0017C90C File Offset: 0x0017AD0C
		protected override float CalculateTopFillerHeight()
		{
			float num = this.customItems.GetRange(0, this.topHiddenItems).Sum((TItem x) => x.Height);
			return num + this.layout.Spacing.y * (float)(this.topHiddenItems - 1);
		}

		// Token: 0x060047F3 RID: 18419 RVA: 0x0017C96C File Offset: 0x0017AD6C
		private float ItemStartAt(int index)
		{
			float num = this.customItems.GetRange(0, index).Sum((TItem x) => x.Height);
			return num + this.layout.Spacing.y * (float)index;
		}

		// Token: 0x060047F4 RID: 18420 RVA: 0x0017C9C0 File Offset: 0x0017ADC0
		private float ItemEndAt(int index)
		{
			float num = this.customItems.GetRange(0, index + 1).Sum((TItem x) => x.Height);
			return num + this.layout.Spacing.y * (float)index;
		}

		// Token: 0x060047F5 RID: 18421 RVA: 0x0017CA14 File Offset: 0x0017AE14
		public override int Add(TItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("Item is null.");
			}
			if (item.Height == 0f)
			{
				item.Height = this.GetItemHeight(item);
			}
			return base.Add(item);
		}

		// Token: 0x060047F6 RID: 18422 RVA: 0x0017CA69 File Offset: 0x0017AE69
		private void SetItemsHeight(List<TItem> items)
		{
			items.ForEach(delegate(TItem x)
			{
				if (x.Height == 0f)
				{
					x.Height = this.GetItemHeight(x);
				}
			});
		}

		// Token: 0x060047F7 RID: 18423 RVA: 0x0017CA7D File Offset: 0x0017AE7D
		protected override void UpdateItems(List<TItem> newItems)
		{
			this.SetItemsHeight(newItems);
			base.UpdateItems(newItems);
		}

		// Token: 0x060047F8 RID: 18424 RVA: 0x0017CA8D File Offset: 0x0017AE8D
		protected override void OnScroll(float value)
		{
			base.OnScroll(value);
		}

		// Token: 0x060047F9 RID: 18425 RVA: 0x0017CA98 File Offset: 0x0017AE98
		private int GetIndexByHeight(float height)
		{
			return this.customItems.TakeWhile(delegate(TItem item, int index)
			{
				height -= item.Height;
				if (index > 0)
				{
					height -= this.layout.Spacing.y;
				}
				return height >= 0f;
			}).Count<TItem>();
		}

		// Token: 0x060047FA RID: 18426 RVA: 0x0017CAD8 File Offset: 0x0017AED8
		protected override int GetLastVisibleIndex(bool strict = false)
		{
			int indexByHeight = this.GetIndexByHeight(base.GetScrollMargin() + this.scrollHeight);
			return (!strict) ? (indexByHeight + 2) : indexByHeight;
		}

		// Token: 0x060047FB RID: 18427 RVA: 0x0017CB08 File Offset: 0x0017AF08
		protected override int GetFirstVisibleIndex(bool strict = false)
		{
			int indexByHeight = this.GetIndexByHeight(base.GetScrollMargin());
			if (strict)
			{
				return indexByHeight;
			}
			return Math.Min(indexByHeight, Math.Max(0, this.customItems.Count - this.visibleItems));
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x0017CB48 File Offset: 0x0017AF48
		private float GetItemHeight(TItem item)
		{
			this.SetData(this.DefaultItemCopy, item);
			TComponent tcomponent = this.DefaultItemCopy;
			return tcomponent.Height;
		}

		// Token: 0x040030FB RID: 12539
		private TComponent defaultItemCopy;

		// Token: 0x040030FC RID: 12540
		private RectTransform defaultItemCopyRect;
	}
}
